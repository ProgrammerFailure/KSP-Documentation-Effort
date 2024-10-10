using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PatchedConics
{
	public class SolverParameters
	{
		public int maxGeometrySolverIterations = 25;

		public int maxTimeSolverIterations = 50;

		public int GeoSolverIterations;

		public int TimeSolverIterations1;

		public int TimeSolverIterations2;

		public bool FollowManeuvers;

		public bool debug_disableEscapeCheck;

		public double outerReaches = 1E+20;

		public double epsilon = 0.0001;
	}

	public delegate double GetClosestApproachDelegate(Orbit p, Orbit s, double startEpoch, double dT, SolverParameters pars);

	public delegate bool EncountersBodyDelegate(Orbit p, Orbit s, Orbit nextPatch, OrbitDriver sec, double startEpoch, SolverParameters pars);

	public delegate bool CheckEncounterDelegate(Orbit p, Orbit nextPatch, double startEpoch, OrbitDriver sec, CelestialBody targetBody, SolverParameters pars, bool logErrors = true);

	public delegate bool CalculatePatchDelegate(Orbit p, Orbit nextPatch, double startEpoch, SolverParameters pars, CelestialBody targetBody);

	public struct PatchCastHit : IScreenCaster
	{
		public Vector3 orbitOrigin;

		public Vector3 hitPoint;

		public Vector3 orbitPoint;

		public Vector3 orbitScreenPoint;

		public double mouseTA;

		public double radiusAtTA;

		public double UTatTA;

		public PatchRendering pr;

		public Vector3 GetUpdatedOrbitPoint()
		{
			return pr.GetScaledSpacePointFromTA(mouseTA, UTatTA);
		}

		public Vector3 GetScreenSpacePoint()
		{
			return PlanetariumCamera.Camera.WorldToScreenPoint(GetUpdatedOrbitPoint());
		}

		public Vector3 GetUpdatedOrigin()
		{
			if (pr.relativityMode == PatchRendering.RelativityMode.RELATIVE)
			{
				return ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToRelative(Vector3d.zero, UTatTA, pr.relativeTo));
			}
			if (pr.relativityMode == PatchRendering.RelativityMode.DYNAMIC)
			{
				return ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLerped(Vector3d.zero, UTatTA, pr.patch.referenceBody, pr.relativeTo, pr.patch.timeToPe, pr.patch.EndUT, pr.dynamicLinearity));
			}
			return pr.cb;
		}
	}

	public delegate bool ScreenCastDelegate(Vector3 screenPos, List<PatchRendering> patchRenders, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, double maxUT = -1.0, bool clampToPatches = false);

	public delegate bool ScreenCastWorkerDelegate(Vector3 screenPos, PatchRendering pr, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, bool clampToPatch = false);

	public static GetClosestApproachDelegate GetClosestApproach = _GetClosestApproach;

	public static EncountersBodyDelegate EncountersBody = _EncountersBody;

	public static CheckEncounterDelegate CheckEncounter = _CheckEncounter;

	public static CalculatePatchDelegate CalculatePatch = _CalculatePatch;

	public static ScreenCastDelegate ScreenCast = _ScreenCast;

	public static ScreenCastWorkerDelegate ScreenCastWorker = _ScreenCastWorker;

	public static double _GetClosestApproach(Orbit p, Orbit s, double startEpoch, double dT, SolverParameters pars)
	{
		double num = Orbit.SynodicPeriod(p, s);
		if (double.IsNaN(num))
		{
			double tA = ((!double.IsInfinity(p.referenceBody.sphereOfInfluence)) ? p.TrueAnomalyAtRadius(p.referenceBody.sphereOfInfluence) : ((!(s.eccentricity < 1.0)) ? p.TrueAnomalyAtRadius(pars.outerReaches) : p.TrueAnomalyAtRadius(s.semiMajorAxis * 3.0)));
			num = 2.0 * p.GetUTforTrueAnomaly(tA, 0.0);
		}
		double num2;
		double maxUT;
		if (num > 0.0)
		{
			num2 = Math.Max(p.UTappr - num / 2.0, startEpoch);
			maxUT = num2 + num;
		}
		else
		{
			if (p.timeToPe < 0.0)
			{
				num2 = startEpoch;
				maxUT = num2 - num;
			}
			num2 = Math.Max(p.UTappr + num, startEpoch);
			maxUT = num2 - num * 2.0;
		}
		dT = Math.Abs(num);
		return Orbit.SolveClosestApproach(p, s, ref p.UTappr, dT, 0.0, num2, maxUT, pars.epsilon, pars.maxTimeSolverIterations, ref pars.TimeSolverIterations1);
	}

	public static bool _EncountersBody(Orbit p, Orbit s, Orbit nextPatch, OrbitDriver sec, double startEpoch, SolverParameters pars)
	{
		if (p.ClAppr < sec.celestialBody.sphereOfInfluence && p.ClAppr != -1.0)
		{
			p.UTsoi = p.UTappr;
			Orbit.SolveSOI(p, s, ref p.UTsoi, (p.UTappr - startEpoch) * 0.5, sec.celestialBody.sphereOfInfluence, startEpoch, p.UTappr, pars.epsilon, pars.maxTimeSolverIterations, ref pars.TimeSolverIterations1);
			p.GetOrbitalStateVectorsAtUT(p.UTsoi, out var pos, out var vel);
			s.GetOrbitalStateVectorsAtUT(p.UTsoi, out var pos2, out var vel2);
			Vector3d lhs = pos - pos2;
			Vector3d rhs = vel - vel2;
			if (Vector3d.Dot(lhs, rhs) >= 0.0)
			{
				return false;
			}
			_ = p.referenceBody.position;
			nextPatch.UpdateFromOrbitAtUT(p, p.UTsoi, sec.celestialBody);
			p.StartUT = startEpoch;
			p.EndUT = p.UTsoi;
			p.patchEndTransition = Orbit.PatchTransitionType.ENCOUNTER;
			return true;
		}
		return false;
	}

	public static bool _CheckEncounter(Orbit p, Orbit nextPatch, double startEpoch, OrbitDriver sec, CelestialBody targetBody, SolverParameters pars, bool logErrors = true)
	{
		try
		{
			Orbit orbit = sec.orbit;
			double num = 1.1;
			if ((!GameSettings.ALWAYS_SHOW_TARGET_APPROACH_MARKERS || !(sec.celestialBody == targetBody)) && !Orbit.PeApIntersects(p, orbit, sec.celestialBody.sphereOfInfluence * num))
			{
				return false;
			}
			if (p.closestEncounterLevel < Orbit.EncounterSolutionLevel.ORBIT_INTERSECT)
			{
				p.closestEncounterLevel = Orbit.EncounterSolutionLevel.ORBIT_INTERSECT;
				p.closestEncounterBody = sec.celestialBody;
			}
			double double_ = p.ClEctr1;
			double double_2 = p.ClEctr2;
			double FFp = p.FEVp;
			double FFs = p.FEVs;
			double SFp = p.SEVp;
			double SFs = p.SEVs;
			int num2 = Orbit.FindClosestPoints(p, orbit, ref double_, ref double_2, ref FFp, ref FFs, ref SFp, ref SFs, 0.0001, pars.maxGeometrySolverIterations, ref pars.GeoSolverIterations);
			if (num2 < 1)
			{
				if (logErrors && !Thread.CurrentThread.IsBackground)
				{
					Debug.Log("CheckEncounter: failed to find any intercepts at all");
				}
				return false;
			}
			double a = p.GetDTforTrueAnomaly(FFp, 0.0);
			double b = p.GetDTforTrueAnomaly(SFp, 0.0);
			double a2 = a + startEpoch;
			double b2 = b + startEpoch;
			if (double.IsInfinity(a2) && double.IsInfinity(b2))
			{
				if (logErrors && !Thread.CurrentThread.IsBackground)
				{
					Debug.Log("CheckEncounter: both intercept UTs are infinite");
				}
				return false;
			}
			if ((!(a2 < p.StartUT) && !(a2 > p.EndUT)) || (!(b2 < p.StartUT) && b2 <= p.EndUT))
			{
				if (b2 < a2 || a2 < p.StartUT || a2 > p.EndUT)
				{
					UtilMath.SwapValues(ref FFp, ref SFp);
					UtilMath.SwapValues(ref FFs, ref SFs);
					UtilMath.SwapValues(ref double_, ref double_2);
					UtilMath.SwapValues(ref a, ref b);
					UtilMath.SwapValues(ref a2, ref b2);
				}
				if (b2 < p.StartUT || b2 > p.EndUT || double.IsInfinity(b2))
				{
					num2 = 1;
				}
				p.numClosePoints = num2;
				p.FEVp = FFp;
				p.FEVs = FFs;
				p.SEVp = SFp;
				p.SEVs = SFs;
				p.ClEctr1 = double_;
				p.ClEctr2 = double_2;
				p.UTappr = a2;
				if (Math.Min(p.ClEctr1, p.ClEctr2) > sec.celestialBody.sphereOfInfluence)
				{
					if ((GameSettings.ALWAYS_SHOW_TARGET_APPROACH_MARKERS || Thread.CurrentThread.IsBackground) && sec.celestialBody == targetBody)
					{
						p.ClAppr = GetClosestApproach(p, orbit, startEpoch, p.nearestTT * 0.5, pars);
						p.closestTgtApprUT = p.UTappr;
					}
					return false;
				}
				if (p.closestEncounterLevel < Orbit.EncounterSolutionLevel.SOI_INTERSECT_1)
				{
					p.closestEncounterLevel = Orbit.EncounterSolutionLevel.SOI_INTERSECT_1;
					p.closestEncounterBody = sec.celestialBody;
				}
				p.timeToTransition1 = a;
				p.secondaryPosAtTransition1 = orbit.getPositionAtUT(a2);
				p.timeToTransition2 = b;
				p.secondaryPosAtTransition2 = orbit.getPositionAtUT(b2);
				p.nearestTT = p.timeToTransition1;
				p.nextTT = p.timeToTransition2;
				if (double.IsNaN(p.nearestTT) && !Thread.CurrentThread.IsBackground && logErrors)
				{
					Debug.Log("nearestTT is NaN! t1: " + p.timeToTransition1 + ", t2: " + p.timeToTransition2 + ", FEVp: " + p.FEVp + ", SEVp: " + p.SEVp);
				}
				p.ClAppr = GetClosestApproach(p, orbit, startEpoch, p.nearestTT * 0.5, pars);
				if (EncountersBody(p, orbit, nextPatch, sec, startEpoch, pars))
				{
					return true;
				}
				if (sec.celestialBody == targetBody)
				{
					p.closestTgtApprUT = p.UTappr;
				}
				return false;
			}
			return false;
		}
		catch (Exception value)
		{
			if (!Thread.CurrentThread.IsBackground)
			{
				Console.WriteLine(value);
			}
			return false;
		}
	}

	public static bool _CalculatePatch(Orbit p, Orbit nextPatch, double startEpoch, SolverParameters pars, CelestialBody targetBody)
	{
		try
		{
			p.activePatch = true;
			p.nextPatch = nextPatch;
			p.patchEndTransition = Orbit.PatchTransitionType.FINAL;
			p.closestEncounterLevel = Orbit.EncounterSolutionLevel.NONE;
			p.numClosePoints = 0;
			int count = Planetarium.Orbits.Count;
			for (int i = 0; i < count; i++)
			{
				OrbitDriver orbitDriver = Planetarium.Orbits[i];
				if (orbitDriver.orbit != p && (bool)orbitDriver.celestialBody)
				{
					if (targetBody == null || orbitDriver.celestialBody == targetBody)
					{
						p.closestTgtApprUT = 0.0;
					}
					if (p.referenceBody != null && orbitDriver.referenceBody == p.referenceBody)
					{
						CheckEncounter(p, nextPatch, startEpoch, orbitDriver, targetBody, pars);
					}
				}
			}
			if (p.patchEndTransition == Orbit.PatchTransitionType.FINAL && !pars.debug_disableEscapeCheck)
			{
				if (!(p.ApR > p.referenceBody.sphereOfInfluence) && p.eccentricity < 1.0)
				{
					p.UTsoi = -1.0;
					p.StartUT = startEpoch;
					p.EndUT = startEpoch + p.period;
					p.patchEndTransition = Orbit.PatchTransitionType.FINAL;
				}
				else if (double.IsInfinity(p.referenceBody.sphereOfInfluence))
				{
					p.FEVp = Math.Acos(0.0 - 1.0 / p.eccentricity);
					p.SEVp = 0.0 - p.FEVp;
					p.StartUT = startEpoch;
					p.EndUT = double.PositiveInfinity;
					p.UTsoi = double.PositiveInfinity;
					p.patchEndTransition = Orbit.PatchTransitionType.FINAL;
				}
				else
				{
					p.FEVp = p.TrueAnomalyAtRadius(p.referenceBody.sphereOfInfluence);
					p.SEVp = 0.0 - p.FEVp;
					p.timeToTransition1 = p.GetDTforTrueAnomaly(p.FEVp, 0.0);
					p.timeToTransition2 = p.GetDTforTrueAnomaly(p.SEVp, 0.0);
					p.UTsoi = startEpoch + p.timeToTransition1;
					nextPatch.UpdateFromOrbitAtUT(p, p.UTsoi, p.referenceBody.referenceBody);
					p.StartUT = startEpoch;
					p.EndUT = p.UTsoi;
					p.patchEndTransition = Orbit.PatchTransitionType.ESCAPE;
				}
			}
			nextPatch.StartUT = p.EndUT;
			nextPatch.EndUT = ((nextPatch.eccentricity < 1.0) ? (nextPatch.StartUT + nextPatch.period) : nextPatch.period);
			nextPatch.patchStartTransition = p.patchEndTransition;
			nextPatch.previousPatch = p;
			return p.patchEndTransition != Orbit.PatchTransitionType.FINAL;
		}
		catch (Exception value)
		{
			if (!Thread.CurrentThread.IsBackground)
			{
				Console.WriteLine(value);
			}
			return false;
		}
	}

	public static bool _ScreenCast(Vector3 screenPos, List<PatchRendering> patchRenders, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, double maxUT = -1.0, bool clampToPatches = false)
	{
		PatchCastHit hitInfo2 = default(PatchCastHit);
		hitInfo = default(PatchCastHit);
		bool result = false;
		int count = patchRenders.Count;
		for (int i = 0; i < count; i++)
		{
			PatchRendering patchRendering = patchRenders[i];
			if (patchRendering.enabled && patchRendering.patch.activePatch && (maxUT == -1.0 || !(patchRendering.patch.StartUT >= maxUT)) && ScreenCastWorker(screenPos, patchRendering, out hitInfo2, orbitPixelWidth, clampToPatches))
			{
				hitInfo = hitInfo2;
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool _ScreenCastWorker(Vector3 screenPos, PatchRendering pr, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, bool clampToPatch = false)
	{
		hitInfo = default(PatchCastHit);
		hitInfo.pr = pr;
		if (pr.relativityMode == PatchRendering.RelativityMode.RELATIVE)
		{
			hitInfo.UTatTA = pr.patch.StartUT;
			hitInfo.mouseTA = SolveRelativeTA_BSP(pr, screenPos, ref hitInfo.UTatTA, (pr.patch.EndUT - pr.patch.StartUT) * 0.5, pr.patch.StartUT, pr.patch.EndUT, 0.001);
			hitInfo.orbitOrigin = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToRelative(Vector3d.zero, hitInfo.UTatTA, pr.relativeTo));
			hitInfo.hitPoint = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToRelative(pr.patch.getRelativePositionAtUT(hitInfo.UTatTA).xzy, hitInfo.UTatTA, pr.relativeTo)) - hitInfo.orbitOrigin;
		}
		else if (pr.relativityMode == PatchRendering.RelativityMode.DYNAMIC)
		{
			hitInfo.UTatTA = pr.patch.StartUT;
			hitInfo.mouseTA = SolveDynamicTA_BSP(pr, screenPos, ref hitInfo.UTatTA, (pr.patch.EndUT - pr.patch.StartUT) * 0.5, pr.patch.StartUT, pr.patch.EndUT, 0.001);
			hitInfo.orbitOrigin = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLerped(Vector3d.zero, hitInfo.UTatTA, pr.patch.referenceBody, pr.relativeTo, pr.patch.timeToPe, pr.patch.EndUT, pr.dynamicLinearity));
			hitInfo.hitPoint = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLerped(pr.patch.getRelativePositionAtUT(hitInfo.UTatTA).xzy, hitInfo.UTatTA, pr.patch.referenceBody, pr.relativeTo, pr.patch.timeToPe, pr.patch.EndUT, pr.dynamicLinearity)) - hitInfo.orbitOrigin;
		}
		else if (pr.patch.eccentricity >= 1.0)
		{
			hitInfo.UTatTA = pr.patch.StartUT;
			hitInfo.mouseTA = pr.patch.fromV;
			double num = Math.Acos(-1.0 / pr.patch.eccentricity);
			hitInfo.mouseTA = SolveLocalTA_BSP(pr, screenPos, ref hitInfo.UTatTA, hitInfo.mouseTA, (num - pr.patch.fromV) * 0.5, pr.patch.fromV, num, 1E-08);
			hitInfo.orbitOrigin = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLocal(Vector3d.zero));
			hitInfo.hitPoint = ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLocal(pr.patch.getRelativePositionAtUT(hitInfo.UTatTA).xzy)) - hitInfo.orbitOrigin;
		}
		else
		{
			Vector3d orbitNormal = pr.patch.GetOrbitNormal();
			hitInfo.orbitOrigin = pr.cb;
			Plane plane = new Plane(orbitNormal.xzy.normalized, hitInfo.orbitOrigin);
			Debug.DrawRay(hitInfo.orbitOrigin, plane.normal * 1000f, Color.cyan);
			Ray ray = PlanetariumCamera.Camera.ScreenPointToRay(screenPos);
			plane.Raycast(ray, out var enter);
			hitInfo.hitPoint = ray.origin + ray.direction * enter - hitInfo.orbitOrigin;
			Vector3 vector = QuaternionD.Inverse(QuaternionD.LookRotation(-(pr.pe - pr.cb).normalized, orbitNormal.xzy)) * hitInfo.hitPoint;
			hitInfo.mouseTA = Mathf.Atan2(vector.x, 0f - vector.z);
			hitInfo.UTatTA = pr.patch.StartUT + pr.patch.GetDTforTrueAnomaly(hitInfo.mouseTA, 0.0);
		}
		if (clampToPatch && !TAIsWithinPatchBounds(hitInfo.mouseTA, pr.patch))
		{
			Debug.DrawRay(hitInfo.orbitOrigin, hitInfo.hitPoint, Color.red);
			return false;
		}
		hitInfo.radiusAtTA = pr.patch.RadiusAtTrueAnomaly(hitInfo.mouseTA) * (double)ScaledSpace.InverseScaleFactor;
		hitInfo.orbitPoint = hitInfo.hitPoint.normalized * (float)hitInfo.radiusAtTA + hitInfo.orbitOrigin;
		hitInfo.orbitScreenPoint = PlanetariumCamera.Camera.WorldToScreenPoint(hitInfo.orbitPoint);
		hitInfo.orbitScreenPoint = new Vector3(hitInfo.orbitScreenPoint.x, hitInfo.orbitScreenPoint.y, 0f);
		if (Vector3.Distance(hitInfo.orbitScreenPoint, Input.mousePosition) < orbitPixelWidth)
		{
			Debug.DrawLine(hitInfo.orbitOrigin, hitInfo.orbitPoint, Color.green);
			return true;
		}
		Debug.DrawLine(hitInfo.orbitOrigin, hitInfo.orbitPoint, Color.yellow);
		return false;
	}

	public static double SolveRelativeTA_BSP(PatchRendering pr, Vector3 screenPos, ref double double_0, double dT, double MinUT, double MaxUT, double epsilon)
	{
		Vector3 v;
		UtilMath.BSPSolver(ref double_0, dT, delegate(double ut)
		{
			v = PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToRelative(pr.patch.getRelativePositionAtUT(ut).xzy, ut, pr.relativeTo)));
			v = new Vector3(v.x, v.y, 0f);
			return (v - screenPos).sqrMagnitude;
		}, MinUT, MaxUT, epsilon, 50);
		return pr.patch.TrueAnomalyAtUT(double_0);
	}

	public static double SolveDynamicTA_BSP(PatchRendering pr, Vector3 screenPos, ref double double_0, double dT, double MinUT, double MaxUT, double epsilon)
	{
		Vector3 v;
		UtilMath.BSPSolver(ref double_0, dT, delegate(double ut)
		{
			v = PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(pr.trajectory.ConvertPointToLerped(pr.patch.getRelativePositionAtUT(ut).xzy, ut, pr.patch.referenceBody, pr.relativeTo, pr.patch.timeToPe, pr.patch.EndUT, pr.dynamicLinearity)));
			v = new Vector3(v.x, v.y, 0f);
			return (v - screenPos).sqrMagnitude;
		}, MinUT, MaxUT, epsilon, 50);
		return pr.patch.TrueAnomalyAtUT(double_0);
	}

	public static double SolveLocalTA_BSP(PatchRendering pr, Vector3 screenPos, ref double double_0, double double_1, double dTA, double MinTA, double MaxTA, double epsilon)
	{
		Vector3 v;
		UtilMath.BSPSolver(ref double_1, dTA, delegate(double ta)
		{
			v = PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(pr.patch.referenceBody.position + pr.patch.getRelativePositionFromTrueAnomaly(ta).xzy));
			v = new Vector3(v.x, v.y, 0f);
			return (v - screenPos).sqrMagnitude;
		}, MinTA, MaxTA, epsilon, 50);
		double_0 = pr.patch.GetUTforTrueAnomaly(double_1, 0.0);
		return double_1;
	}

	public static double AngleWrap(double a)
	{
		double num = Math.Floor(a / (Math.PI * 2.0) + 0.5);
		return a - num * 2.0 * Math.PI;
	}

	public static bool TAIsWithinPatchBounds(double tA, Orbit patch)
	{
		if ((patch.eccentricity > 1.0 || patch.patchEndTransition != Orbit.PatchTransitionType.FINAL) && patch.fromV != 0.0 && patch.toV != 0.0)
		{
			double num = AngleWrap(tA);
			double num2 = AngleWrap(patch.fromV);
			double num3 = AngleWrap(patch.toV);
			if (num2 < num3)
			{
				if (num < num2 || num > num3)
				{
					return false;
				}
			}
			else if (num < num2 && num > num3)
			{
				return false;
			}
		}
		return true;
	}
}
