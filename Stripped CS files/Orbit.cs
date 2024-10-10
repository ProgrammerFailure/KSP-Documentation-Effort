using System;
using System.Collections.Generic;
using System.Threading;
using Smooth.Pools;
using UnityEngine;

[Serializable]
public class Orbit
{
	public enum ObjectType
	{
		VESSEL,
		SPACE_DEBRIS,
		CELESTIAL_BODIES,
		UNKNOWN_MISC,
		KERBAL
	}

	public delegate int FindClosestPointsDelegate(Orbit p, Orbit s, ref double double_0, ref double double_1, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount);

	public struct State
	{
		public Vector3d pos;

		public Vector3d vel;

		public Vector3d acc;

		public Vector3d jrk;

		public double w;

		public void DumpData(string name, double double_0)
		{
		}
	}

	public struct CASolutionState
	{
		public Orbit p;

		public Orbit s;

		public State pstate;

		public State sstate;

		public State rstate;

		public double rdv;

		public double drdv;

		public double ddrdv;

		public double MaxDT;

		public double maxdt;

		public bool targetAhead => Vector3d.Dot(rstate.pos, pstate.vel) < 0.0;

		public CASolutionState(Orbit p, Orbit s, double MaxDT)
		{
			this.p = p;
			this.s = s;
			this.MaxDT = MaxDT;
			pstate = default(State);
			sstate = default(State);
			rstate = default(State);
			double num = 0.0;
			maxdt = 0.0;
			double num2 = num;
			num = 0.0;
			ddrdv = num2;
			double num3 = num;
			num = 0.0;
			drdv = num3;
			rdv = num;
		}

		public void Update(double t, ref int iter, bool dump = false)
		{
			iter++;
			rdv = RelativeStateAtUT(p, s, t, out pstate, out sstate, out rstate, dump);
			drdv = Vector3d.Dot(rstate.vel, rstate.vel) + Vector3d.Dot(rstate.pos, rstate.acc);
			ddrdv = 3.0 * Vector3d.Dot(rstate.vel, rstate.acc) + Vector3d.Dot(rstate.pos, rstate.jrk);
			maxdt = 2.0 * rstate.pos.magnitude / rstate.vel.magnitude;
			maxdt = Math.Min(maxdt, MaxDT / 2.0);
		}

		public double Halley_dt()
		{
			return -2.0 * rdv * drdv / (2.0 * drdv * drdv - rdv * ddrdv);
		}

		public double Clamp_dt(double dt)
		{
			return Math.Min(maxdt, Math.Max(0.0 - maxdt, dt));
		}
	}

	public delegate double SolveClosestApproachDelegate(Orbit p, Orbit s, ref double double_0, double dT, double threshold, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public delegate bool SolveSOI_Delegate(Orbit p, Orbit s, ref double double_0, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public delegate bool SolveSOI_BSPDelegate(Orbit p, Orbit s, ref double double_0, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public enum EncounterSolutionLevel
	{
		NONE,
		ESCAPE,
		ORBIT_INTERSECT,
		SOI_INTERSECT_2,
		SOI_INTERSECT_1
	}

	public enum PatchTransitionType
	{
		INITIAL,
		FINAL,
		ENCOUNTER,
		ESCAPE,
		MANEUVER,
		IMPACT
	}

	public CelestialBody referenceBody;

	public double inclination;

	public double eccentricity;

	public double semiMajorAxis;

	public double double_0;

	public double argumentOfPeriapsis;

	public double epoch;

	public const double Rad2Deg = 180.0 / Math.PI;

	public const double Deg2Rad = Math.PI / 180.0;

	public Planetarium.CelestialFrame OrbitFrame;

	public Vector3d pos;

	public Vector3d vel;

	public double orbitalEnergy;

	public double meanAnomaly;

	public double trueAnomaly;

	public double eccentricAnomaly;

	public double radius;

	public double altitude;

	public double orbitalSpeed;

	public double orbitPercent;

	public double ObT;

	public double ObTAtEpoch;

	public double timeToPe;

	public double timeToAp;

	[Obsolete("Use VesselType or CelestialBodyType instead")]
	public ObjectType objectType = ObjectType.UNKNOWN_MISC;

	public Vector3d h;

	public Vector3d eccVec;

	public Vector3d an;

	public double meanAnomalyAtEpoch;

	public double meanMotion;

	public double period;

	public Vector3 OrbitFrameX;

	public Vector3 OrbitFrameY;

	public Vector3 OrbitFrameZ;

	public Vector3 debugPos;

	public Vector3 debugVel;

	public Vector3 debugH;

	public Vector3 debugAN;

	public Vector3 debugEccVec;

	public double mag;

	public double drawResolution = 15.0;

	public static FindClosestPointsDelegate FindClosestPoints = _FindClosestPoints;

	public static SolveClosestApproachDelegate SolveClosestApproach = _SolveClosestApproach;

	public static SolveSOI_Delegate SolveSOI = _SolveSOI;

	public static SolveSOI_BSPDelegate SolveSOI_BSP = _SolveSOI_BSP;

	public int numClosePoints;

	public double FEVp;

	public double FEVs;

	public double SEVp;

	public double SEVs;

	public double UTappr;

	public double UTsoi;

	public double ClAppr;

	public double CrAppr;

	public double ClEctr1;

	public double ClEctr2;

	public double timeToTransition1;

	public double timeToTransition2;

	public double nearestTT;

	public double nextTT;

	public Vector3d secondaryPosAtTransition1;

	public Vector3d secondaryPosAtTransition2;

	public double closestTgtApprUT;

	public double StartUT;

	public double EndUT;

	public bool activePatch;

	public Orbit closestEncounterPatch;

	public CelestialBody closestEncounterBody;

	public EncounterSolutionLevel closestEncounterLevel;

	public PatchTransitionType patchStartTransition;

	public PatchTransitionType patchEndTransition;

	public Orbit nextPatch;

	public Orbit previousPatch;

	public double fromE;

	public double toE;

	public double sampleInterval;

	public double double_1;

	public double double_2;

	public double fromV;

	public double toV;

	public bool debug_returnFullEllipseTrajectory;

	public double semiMinorAxis
	{
		get
		{
			if (!(eccentricity < 1.0))
			{
				return semiMajorAxis * Math.Sqrt(eccentricity * eccentricity - 1.0);
			}
			return semiMajorAxis * Math.Sqrt(1.0 - eccentricity * eccentricity);
		}
	}

	public double semiLatusRectum => h.sqrMagnitude / referenceBody.gravParameter;

	public double PeR => (1.0 - eccentricity) * semiMajorAxis;

	public double ApR => (1.0 + eccentricity) * semiMajorAxis;

	public double PeA => PeR - referenceBody.Radius;

	public double ApA => ApR - referenceBody.Radius;

	public Orbit()
	{
	}

	public Orbit(double inc, double e, double sma, double lan, double argPe, double mEp, double t, CelestialBody body)
	{
		SetOrbit(inc, e, sma, lan, argPe, mEp, t, body);
	}

	public Orbit(Orbit orbit)
	{
		inclination = orbit.inclination;
		eccentricity = orbit.eccentricity;
		semiMajorAxis = orbit.semiMajorAxis;
		double_0 = orbit.double_0;
		argumentOfPeriapsis = orbit.argumentOfPeriapsis;
		meanAnomalyAtEpoch = orbit.meanAnomalyAtEpoch;
		epoch = orbit.epoch;
		referenceBody = orbit.referenceBody;
		Init();
	}

	public double GetMeanMotion(double sma)
	{
		double num = Math.Abs(sma);
		return Math.Sqrt(referenceBody.gravParameter / (num * num * num));
	}

	public void SetOrbit(double inc, double e, double sma, double lan, double argPe, double mEp, double t, CelestialBody body)
	{
		inclination = inc;
		eccentricity = e;
		semiMajorAxis = sma;
		double_0 = lan;
		argumentOfPeriapsis = argPe;
		meanAnomalyAtEpoch = mEp;
		epoch = t;
		referenceBody = body;
		Init();
	}

	public void Init()
	{
		Planetarium.CelestialFrame.OrbitalFrame(double_0, inclination, argumentOfPeriapsis, ref OrbitFrame);
		an = Vector3d.Cross(Vector3d.forward, OrbitFrame.vector3d_2);
		if (an.sqrMagnitude == 0.0)
		{
			an = Vector3d.right;
		}
		eccVec = Planetarium.Zup.WorldToLocal(OrbitFrame.vector3d_0 * eccentricity);
		double d = referenceBody.gravParameter * semiMajorAxis * (1.0 - eccentricity * eccentricity);
		h = Planetarium.Zup.WorldToLocal(OrbitFrame.vector3d_2 * Math.Sqrt(d));
		meanMotion = GetMeanMotion(semiMajorAxis);
		meanAnomaly = meanAnomalyAtEpoch;
		ObT = meanAnomaly / meanMotion;
		ObTAtEpoch = ObT;
		if (eccentricity < 1.0)
		{
			period = Math.PI * 2.0 / meanMotion;
			orbitPercent = meanAnomaly / (Math.PI * 2.0);
		}
		else
		{
			period = double.PositiveInfinity;
			orbitPercent = 0.0;
		}
	}

	public static double SafeAcos(double c)
	{
		if (c > 1.0)
		{
			c = 1.0;
		}
		else if (c < -1.0)
		{
			c = -1.0;
		}
		return Math.Acos(c);
	}

	public void UpdateFromOrbitAtUT(Orbit orbit, double double_3, CelestialBody toBody)
	{
		State state;
		State state2;
		if (orbit.referenceBody.HasChild(toBody))
		{
			if (!double.IsNaN(orbit.GetOrbitalStateVectorsAtUT(double_3, out state)) && !double.IsNaN(toBody.orbit.GetOrbitalStateVectorsAtUT(double_3, out state2)))
			{
				pos = state.pos - state2.pos;
				vel = state.vel - state2.vel;
				UpdateFromFixedVectors(pos, vel, toBody, double_3);
			}
		}
		else if (toBody.HasChild(orbit.referenceBody))
		{
			if (!double.IsNaN(orbit.GetOrbitalStateVectorsAtUT(double_3, out state)) && !double.IsNaN(orbit.referenceBody.orbit.GetOrbitalStateVectorsAtUT(double_3, out state2)))
			{
				pos = state.pos + state2.pos;
				vel = state.vel + state2.vel;
				UpdateFromFixedVectors(pos, vel, toBody, double_3);
			}
		}
		else
		{
			pos = (orbit.getTruePositionAtUT(double_3) - toBody.getTruePositionAtUT(double_3)).xzy;
			vel = orbit.getOrbitalVelocityAtUT(double_3) + orbit.referenceBody.GetFrameVelAtUT(double_3) - toBody.GetFrameVelAtUT(double_3);
			UpdateFromStateVectors(pos, vel, toBody, double_3);
		}
	}

	public void UpdateFromStateVectors(Vector3d pos, Vector3d vel, CelestialBody refBody, double double_3)
	{
		pos = Planetarium.Zup.LocalToWorld(pos);
		vel = Planetarium.Zup.LocalToWorld(vel);
		UpdateFromFixedVectors(pos, vel, refBody, double_3);
		if (double.IsNaN(argumentOfPeriapsis))
		{
			Vector3d lhs = Quaternion.AngleAxis(0f - (float)double_0, Planetarium.up) * Planetarium.right;
			Vector3d xzy = eccVec.xzy;
			double num = Vector3d.Dot(lhs, xzy) / (lhs.magnitude * xzy.magnitude);
			if (num > 1.0)
			{
				argumentOfPeriapsis = 0.0;
			}
			else if (num < -1.0)
			{
				argumentOfPeriapsis = 180.0;
			}
			else
			{
				argumentOfPeriapsis = Math.Acos(num);
			}
		}
	}

	public void UpdateFromFixedVectors(Vector3d pos, Vector3d vel, CelestialBody refBody, double double_3)
	{
		referenceBody = refBody;
		h = Vector3d.Cross(pos, vel);
		if (h.sqrMagnitude.Equals(0.0))
		{
			inclination = Math.Acos(pos.z / pos.magnitude) * (180.0 / Math.PI);
			an = Vector3d.Cross(pos, Vector3d.forward);
		}
		else
		{
			an = Vector3d.Cross(Vector3d.forward, h);
			OrbitFrame.vector3d_2 = h / h.magnitude;
			inclination = UtilMath.AngleBetween(OrbitFrame.vector3d_2, Vector3d.forward) * (180.0 / Math.PI);
		}
		if (an.sqrMagnitude.Equals(0.0))
		{
			an = Vector3d.right;
		}
		double_0 = Math.Atan2(an.y, an.x) * (180.0 / Math.PI);
		double_0 = (double_0 + 360.0) % 360.0;
		eccVec = (Vector3d.Dot(vel, vel) / refBody.gravParameter - 1.0 / pos.magnitude) * pos - Vector3d.Dot(pos, vel) * vel / refBody.gravParameter;
		eccentricity = eccVec.magnitude;
		orbitalEnergy = vel.sqrMagnitude / 2.0 - refBody.gravParameter / pos.magnitude;
		semiMajorAxis = ((eccentricity < 1.0) ? ((0.0 - refBody.gravParameter) / (2.0 * orbitalEnergy)) : ((0.0 - semiLatusRectum) / (eccVec.sqrMagnitude - 1.0)));
		if (eccentricity.Equals(0.0))
		{
			OrbitFrame.vector3d_0 = an.normalized;
			argumentOfPeriapsis = 0.0;
		}
		else
		{
			OrbitFrame.vector3d_0 = eccVec.normalized;
			argumentOfPeriapsis = UtilMath.AngleBetween(an, OrbitFrame.vector3d_0);
			if (Vector3d.Dot(Vector3d.Cross(an, OrbitFrame.vector3d_0), h) < 0.0)
			{
				argumentOfPeriapsis = Math.PI * 2.0 - argumentOfPeriapsis;
			}
		}
		if (h.sqrMagnitude.Equals(0.0))
		{
			OrbitFrame.vector3d_1 = an.normalized;
			OrbitFrame.vector3d_2 = Vector3d.Cross(OrbitFrame.vector3d_0, OrbitFrame.vector3d_1);
		}
		else
		{
			OrbitFrame.vector3d_1 = Vector3d.Cross(OrbitFrame.vector3d_2, OrbitFrame.vector3d_0);
		}
		argumentOfPeriapsis *= 180.0 / Math.PI;
		meanMotion = GetMeanMotion(semiMajorAxis);
		double x = Vector3d.Dot(pos, OrbitFrame.vector3d_0);
		double y = Vector3d.Dot(pos, OrbitFrame.vector3d_1);
		trueAnomaly = Math.Atan2(y, x);
		eccentricAnomaly = GetEccentricAnomaly(trueAnomaly);
		meanAnomaly = GetMeanAnomaly(eccentricAnomaly);
		meanAnomalyAtEpoch = meanAnomaly;
		ObT = meanAnomaly / meanMotion;
		ObTAtEpoch = ObT;
		if (eccentricity < 1.0)
		{
			period = Math.PI * 2.0 / meanMotion;
			orbitPercent = meanAnomaly / (Math.PI * 2.0);
			orbitPercent = (orbitPercent + 1.0) % 1.0;
			timeToPe = (period - ObT) % period;
			timeToAp = timeToPe - period / 2.0;
			if (timeToAp < 0.0)
			{
				timeToAp += period;
			}
		}
		else
		{
			period = double.PositiveInfinity;
			orbitPercent = 0.0;
			timeToPe = 0.0 - ObT;
			timeToAp = double.PositiveInfinity;
		}
		radius = pos.magnitude;
		altitude = radius - refBody.Radius;
		epoch = double_3;
		this.pos = Planetarium.Zup.WorldToLocal(pos);
		this.vel = Planetarium.Zup.WorldToLocal(vel);
		h = Planetarium.Zup.WorldToLocal(h);
		debugPos = this.pos;
		debugVel = this.vel;
		debugH = h;
		debugAN = an;
		debugEccVec = eccVec;
		OrbitFrameX = OrbitFrame.vector3d_0;
		OrbitFrameY = OrbitFrame.vector3d_1;
		OrbitFrameZ = OrbitFrame.vector3d_2;
	}

	public void UpdateFromUT(double double_3)
	{
		ObT = getObtAtUT(double_3);
		meanAnomaly = ObT * meanMotion;
		eccentricAnomaly = solveEccentricAnomaly(meanAnomaly, eccentricity);
		trueAnomaly = GetTrueAnomaly(eccentricAnomaly);
		radius = GetOrbitalStateVectorsAtTrueAnomaly(trueAnomaly, double_3, out pos, out vel);
		orbitalSpeed = vel.magnitude;
		orbitalEnergy = orbitalSpeed * orbitalSpeed / 2.0 - referenceBody.gravParameter / radius;
		altitude = radius - referenceBody.Radius;
		if (eccentricity < 1.0)
		{
			orbitPercent = meanAnomaly / (Math.PI * 2.0);
			orbitPercent = (orbitPercent + 1.0) % 1.0;
			timeToPe = (period - ObT) % period;
			timeToAp = timeToPe - period / 2.0;
			if (timeToAp < 0.0)
			{
				timeToAp += period;
			}
		}
		else
		{
			orbitPercent = 0.0;
			timeToPe = 0.0 - ObT;
			timeToAp = double.PositiveInfinity;
		}
		debugPos = pos;
		debugVel = vel;
		debugH = h;
		debugAN = an;
		debugEccVec = eccVec;
	}

	public double GetDTforTrueAnomalyAtUT(double tA, double double_3)
	{
		double obtAtTrueAnomaly = getObtAtTrueAnomaly(tA);
		double obtAtUT = getObtAtUT(double_3);
		if (double.IsInfinity(obtAtTrueAnomaly) && double.IsInfinity(obtAtUT))
		{
			return double.NaN;
		}
		if (double.IsInfinity(obtAtTrueAnomaly))
		{
			return obtAtTrueAnomaly;
		}
		if (double.IsInfinity(obtAtUT))
		{
			return 0.0 - obtAtUT;
		}
		double num = obtAtTrueAnomaly - obtAtUT;
		if (eccentricity < 1.0)
		{
			num -= period * Math.Floor(num / period + 0.5);
		}
		return num;
	}

	public double GetDTforTrueAnomaly(double tA, double wrapAfterSeconds)
	{
		double num = GetEccentricAnomaly(tA);
		double num2 = GetMeanAnomaly(num);
		double num3 = num2 / meanMotion;
		double num4;
		if (eccentricity < 1.0)
		{
			num4 = num3 - ObT;
			if (num4 < 0.0 - Math.Abs(wrapAfterSeconds))
			{
				num4 += period;
			}
		}
		else
		{
			num4 = num3 - ObT;
		}
		if (double.IsNaN(num4) && !Thread.CurrentThread.IsBackground)
		{
			Debug.Log("dT is NaN! tA: " + tA + ", E: " + num + ", M: " + num2 + ", T: " + num3 + "\n" + Environment.StackTrace);
		}
		return num4;
	}

	public double GetUTforTrueAnomaly(double tA, double wrapAfterSeconds)
	{
		return epoch + (ObT - ObTAtEpoch) + GetDTforTrueAnomaly(tA, wrapAfterSeconds);
	}

	public Vector3d getPositionAtUT(double double_3)
	{
		return getPositionAtT(getObtAtUT(double_3));
	}

	public Vector3d getTruePositionAtUT(double double_3)
	{
		return getRelativePositionAtUT(double_3).xzy + referenceBody.getTruePositionAtUT(double_3);
	}

	public Vector3d getRelativePositionAtUT(double double_3)
	{
		return getRelativePositionAtT(getObtAtUT(double_3));
	}

	public Vector3d getOrbitalVelocityAtUT(double double_3)
	{
		return getOrbitalVelocityAtObT(getObtAtUT(double_3));
	}

	public double getObtAtUT(double double_3)
	{
		double num;
		if (eccentricity < 1.0)
		{
			if (double.IsInfinity(double_3) && !Thread.CurrentThread.IsBackground)
			{
				Debug.Log("getObtAtUT infinite UT on elliptical orbit UT: " + double_3 + ", returning NaN\n" + Environment.StackTrace);
				return double.NaN;
			}
			num = (double_3 - epoch + ObTAtEpoch) % period;
			if (num > period / 2.0)
			{
				num -= period;
			}
		}
		else
		{
			if (double.IsInfinity(double_3))
			{
				return double_3;
			}
			num = ObTAtEpoch + (double_3 - epoch);
		}
		return num;
	}

	public double getObTAtMeanAnomaly(double double_3)
	{
		return double_3 / meanMotion;
	}

	public double getObtAtTrueAnomaly(double tA)
	{
		double double_ = GetEccentricAnomaly(tA);
		return GetMeanAnomaly(double_) / meanMotion;
	}

	public Vector3d GetOrbitNormal()
	{
		return Planetarium.Zup.WorldToLocal(OrbitFrame.vector3d_2);
	}

	public Vector3d GetEccVector()
	{
		return Planetarium.Zup.WorldToLocal(OrbitFrame.vector3d_0);
	}

	public Vector3d GetANVector()
	{
		return Planetarium.Zup.WorldToLocal(an);
	}

	public Vector3d GetVel()
	{
		Vector3d vector3d = GetFrameVel() - (FlightGlobals.ActiveVessel ? FlightGlobals.ActiveVessel.orbitDriver.referenceBody.GetFrameVel() : Vector3d.zero);
		return new Vector3d(vector3d.x, vector3d.z, vector3d.y);
	}

	public Vector3d GetRelativeVel()
	{
		return vel.xzy;
	}

	public Vector3d GetRotFrameVel(CelestialBody refBody)
	{
		if (refBody.rotates && refBody.inverseRotation)
		{
			return Vector3d.Cross(refBody.zUpAngularVelocity, -pos);
		}
		return Vector3d.zero;
	}

	public Vector3d GetRotFrameVelAtPos(CelestialBody refBody, Vector3d refPos)
	{
		if (refBody.rotates && refBody.inverseRotation)
		{
			return Vector3d.Cross(refBody.zUpAngularVelocity, -refPos);
		}
		return Vector3d.zero;
	}

	public Vector3d GetFrameVel()
	{
		return vel + referenceBody.GetFrameVel();
	}

	public Vector3d GetFrameVelAtUT(double double_3)
	{
		return getOrbitalVelocityAtUT(double_3) + referenceBody.GetFrameVelAtUT(double_3);
	}

	public Vector3d GetWorldSpaceVel()
	{
		return GetVel() - (referenceBody.inverseRotation ? referenceBody.getRFrmVel(pos + referenceBody.position) : Vector3d.zero);
	}

	public double GetTimeToPeriapsis()
	{
		if (eccentricity < 1.0 && patchEndTransition != PatchTransitionType.FINAL && StartUT + timeToPe > EndUT)
		{
			return timeToPe - period;
		}
		return timeToPe;
	}

	public double GetRadiusAtUT(double time)
	{
		GetOrbitalStateVectorsAtUT(time, out var vector3d, out var _);
		return vector3d.magnitude;
	}

	public double GetRadiusAtPhaseAngle(double phaseAngle)
	{
		double num = ((inclination < 90.0) ? 1 : (-1));
		return RadiusAtTrueAnomaly(phaseAngle - 0.01745329238474369 * (double_0 + argumentOfPeriapsis * num));
	}

	public double PhaseAngle(double time, double longitude)
	{
		if (!referenceBody.rotates && referenceBody.rotationPeriod != 0.0)
		{
			return 0.01745329238474369 * (referenceBody.initialRotation + longitude) + Math.PI * 2.0 / referenceBody.rotationPeriod * time;
		}
		return 0.01745329238474369 * (referenceBody.initialRotation + longitude);
	}

	public double PhaseAngle(double time)
	{
		double num = ((inclination < 90.0) ? 1 : (-1));
		double num2;
		for (num2 = 0.01745329238474369 * (double_0 + argumentOfPeriapsis * num) + TrueAnomalyAtUT(time) * num; num2 > Math.PI * 2.0; num2 -= Math.PI * 2.0)
		{
		}
		for (; num2 < 0.0; num2 += Math.PI * 2.0)
		{
		}
		return num2;
	}

	public double GetRelativeInclination(Orbit otherOrbit)
	{
		return Vector3d.Angle(GetOrbitNormal(), otherOrbit.GetOrbitNormal());
	}

	public double TrueAnomalyFromVector(Vector3d vec)
	{
		Vector3d vector3d = Vector3d.Exclude(-GetOrbitNormal().xzy.normalized, vec);
		Vector3d vector3d2 = Quaternion.AngleAxis(0f - (float)double_0, Planetarium.up) * Planetarium.right;
		Vector3d vector3d3 = Quaternion.AngleAxis((float)argumentOfPeriapsis, -GetOrbitNormal().xzy.normalized) * vector3d2;
		vector3d3 = PeR * vector3d3;
		double num = Vector3d.Angle(vector3d3, vector3d);
		if (Math.Abs(Vector3d.Angle(vector3d, Vector3d.Cross(-GetOrbitNormal().xzy.normalized, vector3d3))) < 90.0)
		{
			return num * (Math.PI / 180.0);
		}
		return (360.0 - num) * (Math.PI / 180.0);
	}

	public double GetEccentricAnomaly(double tA)
	{
		double num = Math.Cos(tA / 2.0);
		double num2 = Math.Sin(tA / 2.0);
		if (eccentricity < 1.0)
		{
			return 2.0 * Math.Atan2(Math.Sqrt(1.0 - eccentricity) * num2, Math.Sqrt(1.0 + eccentricity) * num);
		}
		double num3 = Math.Sqrt((eccentricity - 1.0) / (eccentricity + 1.0)) * num2 / num;
		if (num3 >= 1.0)
		{
			return double.PositiveInfinity;
		}
		if (num3 <= -1.0)
		{
			return double.NegativeInfinity;
		}
		return Math.Log((1.0 + num3) / (1.0 - num3));
	}

	public double GetMeanAnomaly(double double_3)
	{
		if (eccentricity < 1.0)
		{
			return UtilMath.ClampRadiansTwoPI(double_3 - eccentricity * Math.Sin(double_3));
		}
		if (double.IsInfinity(double_3))
		{
			return double_3;
		}
		return eccentricity * Math.Sinh(double_3) - double_3;
	}

	public double RadiusAtTrueAnomaly(double tA)
	{
		return semiLatusRectum * (1.0 / (1.0 + eccentricity * Math.Cos(tA)));
	}

	public double TrueAnomalyAtRadiusSimple(double double_3)
	{
		return Math.Acos(semiLatusRectum / (double_3 * eccentricity) - 1.0 / eccentricity);
	}

	public double TrueAnomalyAtRadius(double double_3)
	{
		return trueAnomalyAtRadiusExtreme(double_3);
	}

	public double trueAnomalyAtRadiusExtreme(double double_3)
	{
		double num = Vector3d.Cross(getRelativePositionFromEccAnomaly(eccentricAnomaly), getOrbitalVelocityAtObT(ObT)).sqrMagnitude / referenceBody.gravParameter;
		double_3 = ((!(eccentricity < 1.0)) ? Math.Max(PeR, double_3) : Math.Min(Math.Max(PeR, double_3), ApR));
		return Math.Acos(num / (eccentricity * double_3) - 1.0 / eccentricity);
	}

	public double TrueAnomalyAtUT(double double_3)
	{
		return TrueAnomalyAtT(getObtAtUT(double_3));
	}

	public double TrueAnomalyAtT(double double_3)
	{
		double double_4 = double_3 * meanMotion;
		double num = solveEccentricAnomaly(double_4, eccentricity);
		if (double.IsNaN(num))
		{
			return double.NaN;
		}
		return GetTrueAnomaly(num);
	}

	public double EccentricAnomalyAtUT(double double_3)
	{
		return EccentricAnomalyAtObT(getObtAtUT(double_3));
	}

	public double EccentricAnomalyAtObT(double double_3)
	{
		double double_4 = double_3 * meanMotion;
		return solveEccentricAnomaly(double_4, eccentricity);
	}

	public double solveEccentricAnomaly(double double_3, double ecc, double maxError = 1E-07, int maxIterations = 8)
	{
		if (!(eccentricity < 1.0))
		{
			return solveEccentricAnomalyHyp(double_3, eccentricity, maxError);
		}
		if (!(eccentricity < 0.8))
		{
			return solveEccentricAnomalyExtremeEcc(double_3, eccentricity, maxIterations);
		}
		return solveEccentricAnomalyStd(double_3, eccentricity, maxError);
	}

	public double solveEccentricAnomalyStd(double double_3, double ecc, double maxError = 1E-07)
	{
		double num = 1.0;
		double num2 = double_3 + ecc * Math.Sin(double_3) + 0.5 * ecc * ecc * Math.Sin(2.0 * double_3);
		while (Math.Abs(num) > maxError)
		{
			double num3 = num2 - ecc * Math.Sin(num2);
			num = (double_3 - num3) / (1.0 - ecc * Math.Cos(num2));
			num2 += num;
		}
		return num2;
	}

	public double solveEccentricAnomalyExtremeEcc(double double_3, double ecc, int iterations = 8)
	{
		try
		{
			double num = double_3 + 0.85 * eccentricity * (double)Math.Sign(Math.Sin(double_3));
			for (int i = 0; i < iterations; i++)
			{
				double num2 = ecc * Math.Sin(num);
				double num3 = ecc * Math.Cos(num);
				double num4 = num - num2 - double_3;
				double num5 = 1.0 - num3;
				double num6 = num2;
				num += -5.0 * num4 / (num5 + (double)Math.Sign(num5) * Math.Sqrt(Math.Abs(16.0 * num5 * num5 - 20.0 * num4 * num6)));
			}
			return num;
		}
		catch (Exception value)
		{
			if (!Thread.CurrentThread.IsBackground)
			{
				Console.WriteLine(value);
			}
			return double.NaN;
		}
	}

	public double solveEccentricAnomalyHyp(double double_3, double ecc, double maxError = 1E-07)
	{
		if (double.IsInfinity(double_3))
		{
			return double_3;
		}
		double num = 1.0;
		double num2 = 2.0 * double_3 / ecc;
		double num3 = Math.Log(Math.Sqrt(num2 * num2 + 1.0) + num2);
		while (Math.Abs(num) > maxError)
		{
			num = (eccentricity * Math.Sinh(num3) - num3 - double_3) / (eccentricity * Math.Cosh(num3) - 1.0);
			num3 -= num;
		}
		return num3;
	}

	public double GetTrueAnomaly(double double_3)
	{
		if (eccentricity < 1.0)
		{
			double num = Math.Cos(double_3 / 2.0);
			double num2 = Math.Sin(double_3 / 2.0);
			return 2.0 * Math.Atan2(Math.Sqrt(1.0 + eccentricity) * num2, Math.Sqrt(1.0 - eccentricity) * num);
		}
		if (double.IsPositiveInfinity(double_3))
		{
			return Math.Acos(-1.0 / eccentricity);
		}
		if (double.IsNegativeInfinity(double_3))
		{
			return 0.0 - Math.Acos(-1.0 / eccentricity);
		}
		double num3 = Math.Sinh(double_3 / 2.0);
		double num4 = Math.Cosh(double_3 / 2.0);
		return 2.0 * Math.Atan2(Math.Sqrt(eccentricity + 1.0) * num3, Math.Sqrt(eccentricity - 1.0) * num4);
	}

	public double GetTrueAnomalyOfZupVector(Vector3d vector)
	{
		Vector3d lhs = Planetarium.Zup.LocalToWorld(vector);
		return Math.Atan2(x: Vector3d.Dot(lhs, OrbitFrame.vector3d_0), y: Vector3d.Dot(lhs, OrbitFrame.vector3d_1));
	}

	public Vector3d getPositionAtT(double double_3)
	{
		return referenceBody.position + getRelativePositionAtT(double_3).xzy;
	}

	public Vector3d getRelativePositionAtT(double double_3)
	{
		double double_4 = double_3 * meanMotion;
		double double_5 = solveEccentricAnomaly(double_4, eccentricity);
		double tA = GetTrueAnomaly(double_5);
		return getRelativePositionFromTrueAnomaly(tA);
	}

	public Vector3d getPositionFromMeanAnomaly(double double_3)
	{
		return referenceBody.position + getRelativePositionFromMeanAnomaly(double_3).xzy;
	}

	public Vector3d getRelativePositionFromMeanAnomaly(double double_3)
	{
		double double_4 = solveEccentricAnomaly(double_3, eccentricity, 1E-05);
		return getRelativePositionFromEccAnomaly(double_4);
	}

	public double TimeOfTrueAnomaly(double tA, double double_3)
	{
		return getUTAtMeanAnomaly(GetMeanAnomaly(GetEccentricAnomaly(tA)), double_3);
	}

	public double getUTAtMeanAnomaly(double double_3, double double_4)
	{
		double meanAnomalyAtUT = getMeanAnomalyAtUT(double_4);
		double num = double_3 - meanAnomalyAtUT;
		if (eccentricity < 1.0)
		{
			num = UtilMath.ClampRadiansTwoPI(num);
		}
		return double_4 + num / GetMeanMotion(semiMajorAxis);
	}

	public double getMeanAnomalyAtUT(double double_3)
	{
		double num = meanAnomalyAtEpoch + GetMeanMotion(semiMajorAxis) * (double_3 - epoch);
		if (eccentricity < 1.0)
		{
			num = UtilMath.ClampRadiansTwoPI(num);
		}
		return num;
	}

	public Vector3d getPositionFromEccAnomaly(double double_3)
	{
		return referenceBody.position + getRelativePositionFromEccAnomaly(double_3).xzy;
	}

	public Vector3d getRelativePositionFromEccAnomaly(double double_3)
	{
		double num;
		double num2;
		if (eccentricity < 1.0)
		{
			num = semiMajorAxis * (Math.Cos(double_3) - eccentricity);
			num2 = semiMajorAxis * Math.Sqrt(1.0 - eccentricity * eccentricity) * Math.Sin(double_3);
		}
		else if (eccentricity > 1.0)
		{
			num = (0.0 - semiMajorAxis) * (eccentricity - Math.Cosh(double_3));
			num2 = (0.0 - semiMajorAxis) * Math.Sqrt(eccentricity * eccentricity - 1.0) * Math.Sinh(double_3);
		}
		else
		{
			num = 0.0;
			num2 = 0.0;
		}
		Vector3d r = OrbitFrame.vector3d_0 * num + OrbitFrame.vector3d_1 * num2;
		return Planetarium.Zup.WorldToLocal(r);
	}

	public Vector3d getPositionFromEccAnomalyWithSemiMinorAxis(double double_3, double semiMinorAxis)
	{
		return referenceBody.position + getRelativePositionFromEccAnomalyWithSemiMinorAxis(double_3, semiMinorAxis).xzy;
	}

	public Vector3d getRelativePositionFromEccAnomalyWithSemiMinorAxis(double double_3, double semiMinorAxis)
	{
		double num;
		double num2;
		if (eccentricity < 1.0)
		{
			num = semiMajorAxis * (Math.Cos(double_3) - eccentricity);
			num2 = semiMinorAxis * Math.Sin(double_3);
		}
		else if (eccentricity > 1.0)
		{
			num = (0.0 - semiMajorAxis) * (eccentricity - Math.Cosh(double_3));
			num2 = (0.0 - semiMinorAxis) * Math.Sinh(double_3);
		}
		else
		{
			num = 0.0;
			num2 = 0.0;
		}
		Vector3d r = OrbitFrame.vector3d_0 * num + OrbitFrame.vector3d_1 * num2;
		return Planetarium.Zup.WorldToLocal(r);
	}

	public Vector3d getPositionFromTrueAnomaly(double tA)
	{
		return referenceBody.position + getRelativePositionFromTrueAnomaly(tA).xzy;
	}

	public Vector3d getRelativePositionFromTrueAnomaly(double tA)
	{
		return getPositionFromTrueAnomaly(tA, worldToLocal: true);
	}

	public Vector3d getPositionFromTrueAnomaly(double tA, bool worldToLocal)
	{
		double num = Math.Cos(tA);
		double num2 = Math.Sin(tA);
		Vector3d vector3d = semiLatusRectum / (1.0 + eccentricity * num) * (OrbitFrame.vector3d_0 * num + OrbitFrame.vector3d_1 * num2);
		if (!worldToLocal)
		{
			return vector3d;
		}
		return Planetarium.Zup.WorldToLocal(vector3d);
	}

	public double getOrbitalSpeedAt(double time)
	{
		return getOrbitalSpeedAtDistance(getRelativePositionAtT(time).magnitude);
	}

	public double getOrbitalSpeedAtRelativePos(Vector3d relPos)
	{
		return getOrbitalSpeedAtDistance(relPos.magnitude);
	}

	public double getOrbitalSpeedAtPos(Vector3d pos)
	{
		return getOrbitalSpeedAtDistance((referenceBody.position - pos).magnitude);
	}

	public double getOrbitalSpeedAtDistance(double d)
	{
		return Math.Sqrt(referenceBody.gravParameter * (2.0 / d - 1.0 / semiMajorAxis));
	}

	public Vector3d getOrbitalVelocityAtObT(double ObT)
	{
		return getOrbitalVelocityAtTrueAnomaly(TrueAnomalyAtT(ObT));
	}

	public Vector3d getOrbitalVelocityAtTrueAnomaly(double tA)
	{
		return getOrbitalVelocityAtTrueAnomaly(tA, worldToLocal: true);
	}

	public Vector3d getOrbitalVelocityAtTrueAnomaly(double tA, bool worldToLocal)
	{
		double num = Math.Cos(tA);
		double num2 = Math.Sin(tA);
		double num3 = Math.Sqrt(referenceBody.gravParameter / (semiMajorAxis * (1.0 - eccentricity * eccentricity)));
		double num4 = (0.0 - num2) * num3;
		double num5 = (num + eccentricity) * num3;
		Vector3d vector3d = OrbitFrame.vector3d_0 * num4 + OrbitFrame.vector3d_1 * num5;
		if (!worldToLocal)
		{
			return vector3d;
		}
		return Planetarium.Zup.WorldToLocal(vector3d);
	}

	public double GetOrbitalStateVectorsAtUT(double double_3, out Vector3d pos, out Vector3d vel)
	{
		return GetOrbitalStateVectorsAtObT(getObtAtUT(double_3), double_3, out pos, out vel);
	}

	public double GetOrbitalStateVectorsAtObT(double ObT, double double_3, out Vector3d pos, out Vector3d vel)
	{
		return GetOrbitalStateVectorsAtTrueAnomaly(TrueAnomalyAtT(ObT), double_3, out pos, out vel);
	}

	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, double double_3, out Vector3d pos, out Vector3d vel)
	{
		return GetOrbitalStateVectorsAtTrueAnomaly(tA, double_3, worldToLocal: true, out pos, out vel);
	}

	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, double double_3, bool worldToLocal, out Vector3d pos, out Vector3d vel)
	{
		double num = Math.Cos(tA);
		double num2 = Math.Sin(tA);
		double num3 = semiMajorAxis * (1.0 - eccentricity * eccentricity);
		double num4 = Math.Sqrt(referenceBody.gravParameter / num3);
		double num5 = (0.0 - num2) * num4;
		double num6 = (num + eccentricity) * num4;
		double num7 = num3 / (1.0 + eccentricity * num);
		double num8 = num * num7;
		double num9 = num2 * num7;
		if (worldToLocal)
		{
			Planetarium.CelestialFrame tempZup = default(Planetarium.CelestialFrame);
			Planetarium.ZupAtT(double_3, referenceBody, ref tempZup);
			pos = tempZup.WorldToLocal(OrbitFrame.vector3d_0 * num8 + OrbitFrame.vector3d_1 * num9);
			vel = tempZup.WorldToLocal(OrbitFrame.vector3d_0 * num5 + OrbitFrame.vector3d_1 * num6);
		}
		else
		{
			pos = OrbitFrame.vector3d_0 * num8 + OrbitFrame.vector3d_1 * num9;
			vel = OrbitFrame.vector3d_0 * num5 + OrbitFrame.vector3d_1 * num6;
		}
		return num7;
	}

	public double GetOrbitalStateVectorsAtUT(double double_3, out State state)
	{
		return GetOrbitalStateVectorsAtObT(getObtAtUT(double_3), out state);
	}

	public double GetOrbitalStateVectorsAtObT(double ObT, out State state)
	{
		double num = TrueAnomalyAtT(ObT);
		if (double.IsNaN(num))
		{
			state = default(State);
			return double.NaN;
		}
		return GetOrbitalStateVectorsAtTrueAnomaly(num, out state);
	}

	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, out State state)
	{
		double gravParameter = referenceBody.gravParameter;
		double num = semiMajorAxis * (1.0 - eccentricity * eccentricity);
		double num2 = eccentricity;
		double num3 = Math.Cos(tA);
		double num4 = Math.Sin(tA);
		double num5 = 1.0 + num2 * num3;
		double num6 = num / num5;
		double num7 = num3 * num6;
		double num8 = num4 * num6;
		double num9 = Math.Sqrt(gravParameter / num);
		double num10 = (0.0 - num4) * num9;
		double num11 = (num3 + num2) * num9;
		double num12 = (0.0 - gravParameter) / (num6 * num6);
		double num13 = num3 * num12;
		double num14 = num4 * num12;
		double num15 = gravParameter * num9 / (num6 * num6 * num6);
		double num16 = num15 * ((3.0 * num5 - 2.0) * num4);
		double num17 = num15 * (2.0 * num2 - (3.0 * num5 - 2.0) * num3);
		state.pos = OrbitFrame.vector3d_0 * num7 + OrbitFrame.vector3d_1 * num8;
		state.vel = OrbitFrame.vector3d_0 * num10 + OrbitFrame.vector3d_1 * num11;
		state.acc = OrbitFrame.vector3d_0 * num13 + OrbitFrame.vector3d_1 * num14;
		state.jrk = OrbitFrame.vector3d_0 * num16 + OrbitFrame.vector3d_1 * num17;
		state.w = (0.0 - num12) / num9;
		return num6;
	}

	public double GetOrbitalCurvatureAtTrueAnomaly(double ta)
	{
		double num = Math.Cos(ta);
		double num2 = eccentricity;
		double num4;
		if (num2 != 1.0)
		{
			double num3 = 1.0 + num2 * num;
			num4 = num3 * num3 / (1.0 + 2.0 * num2 * num + num2 * num2);
		}
		else
		{
			num4 = (1.0 + num) / 2.0;
		}
		return num4 * Math.Sqrt(num4) / semiLatusRectum;
	}

	public Vector3d Prograde(double double_3)
	{
		return getOrbitalVelocityAtUT(double_3).xzy.normalized;
	}

	public Vector3d Radial(double double_3)
	{
		return Vector3d.Exclude(Prograde(double_3), Up(double_3)).normalized;
	}

	public Vector3d Normal(double double_3)
	{
		return -GetOrbitNormal().xzy.normalized;
	}

	public Vector3d Up(double double_3)
	{
		return getRelativePositionAtUT(double_3).xzy.normalized;
	}

	public Vector3d Horizontal(double double_3)
	{
		return Vector3d.Exclude(Up(double_3), Prograde(double_3)).normalized;
	}

	public double GetNextPeriapsisTime(double double_3)
	{
		if (eccentricity < 1.0)
		{
			return TimeOfTrueAnomaly(0.0, double_3);
		}
		return double_3 - getMeanAnomalyAtUT(double_3) / GetMeanMotion(semiMajorAxis);
	}

	public double GetNextApoapsisTime(double double_3)
	{
		if (eccentricity < 1.0)
		{
			return TimeOfTrueAnomaly(Math.PI, double_3);
		}
		return double_3;
	}

	public double GetNextTimeOfRadius(double double_3, double radius)
	{
		if (!(radius < PeR) && (!(eccentricity < 1.0) || radius <= ApR))
		{
			double num = TrueAnomalyAtRadius(radius);
			double tA = UtilMath.TwoPI - num;
			double num2 = TimeOfTrueAnomaly(num, double_3);
			double num3 = TimeOfTrueAnomaly(tA, double_3);
			if (num3 < num2 && num3 > double_3)
			{
				return num3;
			}
			return num2;
		}
		return double_3;
	}

	public void DrawOrbit()
	{
		DrawOrbit(Color.black);
	}

	public void DrawOrbit(Color color)
	{
		if (eccentricity < 1.0)
		{
			for (double num = 0.0; num < Math.PI * 2.0; num += drawResolution * (Math.PI / 180.0))
			{
				Vector3 vector = getPositionFromTrueAnomaly(num % (Math.PI * 2.0));
				Vector3 vector2 = getPositionFromTrueAnomaly((num + drawResolution * (Math.PI / 180.0)) % (Math.PI * 2.0));
				if (color == Color.black)
				{
					Debug.DrawLine(ScaledSpace.LocalToScaledSpace(vector), ScaledSpace.LocalToScaledSpace(vector2), Color.Lerp(Color.yellow, Color.green, Mathf.InverseLerp((float)getOrbitalSpeedAtDistance(PeR), (float)getOrbitalSpeedAtDistance(ApR), (float)getOrbitalSpeedAtPos(vector))));
				}
				else
				{
					Debug.DrawLine(ScaledSpace.LocalToScaledSpace(vector), ScaledSpace.LocalToScaledSpace(vector2), color);
				}
			}
		}
		else
		{
			for (double num2 = 0.0 - Math.Acos(0.0 - 1.0 / eccentricity) + drawResolution * (Math.PI / 180.0); num2 < Math.Acos(0.0 - 1.0 / eccentricity) - drawResolution * (Math.PI / 180.0); num2 += drawResolution * (Math.PI / 180.0))
			{
				if (color == Color.black)
				{
					Debug.DrawLine(ScaledSpace.LocalToScaledSpace(getPositionFromTrueAnomaly(num2)), ScaledSpace.LocalToScaledSpace(getPositionFromTrueAnomaly(Math.Min(Math.Acos(0.0 - 1.0 / eccentricity), num2 + drawResolution * (Math.PI / 180.0)))), Color.green);
				}
				else
				{
					Debug.DrawLine(ScaledSpace.LocalToScaledSpace(getPositionFromTrueAnomaly(num2)), ScaledSpace.LocalToScaledSpace(getPositionFromTrueAnomaly(Math.Min(Math.Acos(0.0 - 1.0 / eccentricity), num2 + drawResolution * (Math.PI / 180.0)))), color);
				}
			}
		}
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(getPositionAtT(ObT)), ScaledSpace.LocalToScaledSpace(referenceBody.position), Color.green);
		Debug.DrawRay(ScaledSpace.LocalToScaledSpace(getPositionAtT(ObT)), new Vector3d(vel.x, vel.z, vel.y) * 0.009999999776482582, Color.white);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(referenceBody.position), ScaledSpace.LocalToScaledSpace(referenceBody.position + (Vector3)(an.xzy * radius)), Color.cyan);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(referenceBody.position), ScaledSpace.LocalToScaledSpace(getPositionAtT(0.0)), Color.magenta);
		Debug.DrawRay(ScaledSpace.LocalToScaledSpace(referenceBody.position), ScaledSpace.LocalToScaledSpace(h.xzy), Color.blue);
	}

	public static Orbit OrbitFromStateVectors(Vector3d pos, Vector3d vel, CelestialBody body, double double_3)
	{
		Orbit orbit = new Orbit();
		orbit.UpdateFromStateVectors((pos - body.position).xzy, vel.xzy, body, double_3);
		return orbit;
	}

	public static bool PeApIntersects(Orbit primary, Orbit secondary, double threshold)
	{
		if (primary.eccentricity >= 1.0)
		{
			return primary.PeR < secondary.ApR + threshold;
		}
		if (secondary.eccentricity >= 1.0)
		{
			return secondary.PeR < primary.ApR + threshold;
		}
		return Math.Max(primary.PeR, secondary.PeR) - Math.Min(primary.ApR, secondary.ApR) <= threshold;
	}

	public static int _FindClosestPoints(Orbit p, Orbit s, ref double double_3, ref double double_4, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount)
	{
		if (GameSettings.LEGACY_ORBIT_TARGETING)
		{
			FindClosestPoints_old(p, s, ref double_3, ref double_4, ref FFp, ref FFs, ref SFp, ref SFs, epsilon, maxIterations, ref iterationCount);
			return 2;
		}
		Targeting.Conic conic = Targeting.Conic.Borrow(p);
		Targeting.Conic conic2 = Targeting.Conic.Borrow(s);
		List<Targeting.Sample> list = Targeting.Intercepts(conic, conic2, 24);
		conic.Release();
		conic2.Release();
		int num = 0;
		int index = -1;
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			if (!(Vector3d.Dot(list[num2].tgt.p, list[num2].src.p) <= 0.0) && list[num2].minimum)
			{
				if (num != 0)
				{
					num++;
					process_intercept(list[num2], out SFp, out SFs, out double_4);
					break;
				}
				num++;
				process_intercept(list[num2], out FFp, out FFs, out double_3);
				index = num2;
			}
		}
		if (num == 1)
		{
			process_intercept(list[index], out SFp, out SFs, out double_4);
		}
		Targeting.Sample.ReleaseAllBorrowed();
		ListPool<Targeting.Sample>.Instance.Release(list);
		return num;
	}

	public static void process_intercept(Targeting.Sample cept, out double Vp, out double Vs, out double double_3)
	{
		Vp = cept.v;
		Vs = cept.orbit2.TrueAnomaly(cept.tgt.p);
		double_3 = cept.info.d.magnitude;
	}

	public static void FindClosestPoints_old(Orbit p, Orbit s, ref double double_3, ref double double_4, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount)
	{
		double num = p.inclination * (Math.PI / 180.0);
		double num2 = s.inclination * (Math.PI / 180.0);
		double num3 = num - num2;
		Vector3d vector3d = Vector3d.Cross(s.h, p.h);
		Debug.DrawRay(ScaledSpace.LocalToScaledSpace(p.referenceBody.position), vector3d.xzy * 1000.0, Color.white);
		double x = 1.0 / Math.Sin(num3) * (Math.Sin(num) * Math.Cos(num2) - Math.Sin(num2) * Math.Cos(num) * Math.Cos(p.double_0 * (Math.PI / 180.0) - s.double_0 * (Math.PI / 180.0)));
		double num4 = Math.Atan2(1.0 / Math.Sin(num3) * (Math.Sin(num2) * Math.Sin(p.double_0 * (Math.PI / 180.0) - s.double_0 * (Math.PI / 180.0))), x);
		double x2 = 1.0 / Math.Sin(num3) * (Math.Sin(num) * Math.Cos(num2) * Math.Cos(p.double_0 * (Math.PI / 180.0) - s.double_0 * (Math.PI / 180.0)) - Math.Sin(num2) * Math.Cos(num));
		double num5 = Math.Atan2(1.0 / Math.Sin(num3) * (Math.Sin(num) * Math.Sin(p.double_0 * (Math.PI / 180.0) - s.double_0 * (Math.PI / 180.0))), x2);
		FFp = num4 - p.argumentOfPeriapsis * (Math.PI / 180.0);
		FFs = num5 - s.argumentOfPeriapsis * (Math.PI / 180.0);
		if (p.eccentricity == 0.0 && s.eccentricity == 0.0)
		{
			double_3 = Vector3d.Distance(p.getPositionFromTrueAnomaly(FFp), s.getPositionFromTrueAnomaly(FFs));
			double_4 = double_3;
		}
		double_3 = SolveClosestBSP(ref FFp, ref FFs, num3, Math.PI, p, s, 0.0001, maxIterations, ref iterationCount);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(p.referenceBody.position), ScaledSpace.LocalToScaledSpace(p.getPositionFromTrueAnomaly(FFp)), Color.green);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(s.referenceBody.position), ScaledSpace.LocalToScaledSpace(s.getPositionFromTrueAnomaly(FFs)), Color.grey);
		SFp = FFp + Math.PI;
		SFs = FFs + Math.PI;
		double_4 = SolveClosestBSP(ref SFp, ref SFs, num3, Math.PI / 2.0, p, s, 0.0001, maxIterations, ref iterationCount);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(p.referenceBody.position), ScaledSpace.LocalToScaledSpace(p.getPositionFromTrueAnomaly(SFp)), Color.cyan);
		Debug.DrawLine(ScaledSpace.LocalToScaledSpace(s.referenceBody.position), ScaledSpace.LocalToScaledSpace(s.getPositionFromTrueAnomaly(SFs)), Color.magenta);
		double_3 = Math.Sqrt(double_3);
		double_4 = Math.Sqrt(double_4);
	}

	public static double SolveClosestBSP(ref double Fp, ref double Fs, double Ir, double dF, Orbit p, Orbit s, double epsilon, int maxIterations, ref int iterationCount)
	{
		double num = double.MaxValue;
		double num2 = double.MaxValue;
		double num3 = 0.0;
		double num4 = dF;
		double num5 = dF;
		if (Math.Abs(Ir) % Math.PI * 2.0 > Math.PI / 2.0)
		{
			num5 *= -1.0;
		}
		iterationCount = 0;
		num3 = (p.getRelativePositionFromTrueAnomaly(Fp) - s.getRelativePositionFromTrueAnomaly(Fs)).sqrMagnitude;
		while (!(num4 <= 0.0001) && iterationCount < maxIterations)
		{
			num = (p.getRelativePositionFromTrueAnomaly(Fp + num4) - s.getRelativePositionFromTrueAnomaly(Fs + num5)).sqrMagnitude;
			num2 = (p.getRelativePositionFromTrueAnomaly(Fp - num4) - s.getRelativePositionFromTrueAnomaly(Fs - num5)).sqrMagnitude;
			num3 = Math.Min(num3, Math.Min(num, num2));
			if (num3 == num)
			{
				Fp += num4;
				Fs += num5;
			}
			else if (num3 == num2)
			{
				Fp -= num4;
				Fs -= num5;
			}
			num4 *= 0.5;
			num5 *= 0.5;
			iterationCount++;
		}
		return num3;
	}

	public static double RelativeStateAtUT(Orbit p, Orbit s, double double_3, out State pstate, out State sstate, out State rstate, bool dump = false)
	{
		p.GetOrbitalStateVectorsAtUT(double_3, out pstate);
		s.GetOrbitalStateVectorsAtUT(double_3, out sstate);
		rstate.pos = pstate.pos - sstate.pos;
		rstate.vel = pstate.vel - sstate.vel;
		rstate.acc = pstate.acc - sstate.acc;
		rstate.jrk = pstate.jrk - sstate.jrk;
		rstate.w = pstate.w - sstate.w;
		if (dump && !Thread.CurrentThread.IsBackground)
		{
			pstate.DumpData("Ship", double_3);
			sstate.DumpData("Mun", double_3);
			rstate.DumpData("rel", double_3);
		}
		return Vector3d.Dot(rstate.vel, rstate.pos);
	}

	public static double SynodicPeriod(Orbit o1, Orbit o2)
	{
		if (o1.eccentricity < 1.0 && o2.eccentricity < 1.0)
		{
			return o2.period * o1.period / (o2.period - o1.period);
		}
		if (o1.eccentricity < 1.0)
		{
			return o1.period;
		}
		if (o2.eccentricity < 1.0)
		{
			return 0.0 - o2.period;
		}
		return double.NaN;
	}

	public static double _SolveClosestApproach(Orbit p, Orbit s, ref double double_3, double dT, double threshold, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		if (double_3 < MinUT)
		{
			return -1.0;
		}
		if (double_3 > MaxUT)
		{
			return -1.0;
		}
		CASolutionState cASolutionState = new CASolutionState(p, s, dT);
		iterationCount = 0;
		if (p.numClosePoints > 1)
		{
			double num = double_3 + p.GetDTforTrueAnomalyAtUT(p.FEVp, double_3);
			double num2 = double_3 + p.GetDTforTrueAnomalyAtUT(p.SEVp, double_3);
			cASolutionState.Update(num, ref iterationCount, dump: true);
			double magnitude = cASolutionState.rstate.pos.magnitude;
			double rdv = cASolutionState.rdv;
			double num3 = num2;
			cASolutionState.Update(num3, ref iterationCount);
			double magnitude2 = cASolutionState.rstate.pos.magnitude;
			double rdv2 = cASolutionState.rdv;
			if (rdv < 0.0 && rdv2 > 0.0)
			{
				double num4 = (0.0 - rdv) / (rdv2 - rdv);
				dT = num4 * (num3 - double_3);
				double_3 += dT;
				if (num4 < 0.5)
				{
					if (cASolutionState.rdv > rdv2 / 2.0)
					{
						double_3 = (num + double_3) / 2.0;
					}
				}
				else if (cASolutionState.rdv < rdv / 2.0)
				{
					double_3 = (num2 + double_3) / 2.0;
				}
			}
			else if (magnitude2 < magnitude)
			{
				double_3 = num3;
			}
		}
		cASolutionState.Update(double_3, ref iterationCount);
		if (cASolutionState.targetAhead && cASolutionState.rdv > 0.0 && cASolutionState.drdv > 0.0)
		{
			dT = (0.0 - cASolutionState.rdv) / cASolutionState.drdv;
			dT = Math.Max(dT, 0.0 - cASolutionState.maxdt);
			double_3 += dT;
			cASolutionState.Update(double_3, ref iterationCount);
		}
		while (!(cASolutionState.drdv > 0.0) && iterationCount < maxIterations)
		{
			double rdv3 = cASolutionState.rdv;
			double drdv = cASolutionState.drdv;
			double ddrdv = cASolutionState.ddrdv;
			double num5 = drdv * drdv - 2.0 * ddrdv * rdv3;
			if (num5 >= 0.0)
			{
				if (cASolutionState.rdv > 0.0)
				{
					dT = (drdv + Math.Sqrt(num5)) / ddrdv;
					dT = cASolutionState.Clamp_dt(dT);
				}
				else
				{
					dT = (drdv + Math.Sqrt(num5)) / ddrdv;
					dT = cASolutionState.Clamp_dt(dT);
				}
			}
			else
			{
				dT = ((!(cASolutionState.rdv > 0.0)) ? cASolutionState.maxdt : (0.0 - cASolutionState.maxdt));
			}
			double_3 += dT;
			cASolutionState.Update(double_3, ref iterationCount);
		}
		if (iterationCount >= maxIterations)
		{
			if (GameSettings.VERBOSE_DEBUG_LOG && !Thread.CurrentThread.IsBackground)
			{
				Debug.LogFormat("[Orbit] SolveClosestApproach: presolve took too many iterations, bailing UT:{0} MinUT:{1} MaxUT:{2}", double_3, MinUT, MaxUT);
			}
			return cASolutionState.rstate.pos.magnitude;
		}
		dT = cASolutionState.Halley_dt();
		if (double_3 + dT < MinUT)
		{
			if (!(MaxUT - MinUT >= cASolutionState.MaxDT) || (!(p.eccentricity < 1.0) && !(s.eccentricity < 1.0)))
			{
				double_3 = MinUT;
				cASolutionState.Update(double_3, ref iterationCount, dump: true);
				return cASolutionState.rstate.pos.magnitude;
			}
			dT += cASolutionState.MaxDT;
		}
		else if (double_3 + dT > MaxUT)
		{
			double_3 = MaxUT;
			cASolutionState.Update(double_3, ref iterationCount, dump: true);
			return cASolutionState.rstate.pos.magnitude;
		}
		while (iterationCount < maxIterations && !(Math.Abs(dT) <= epsilon))
		{
			double_3 += dT;
			double_3 = Math.Min(MaxUT, Math.Max(MinUT, double_3));
			cASolutionState.Update(double_3, ref iterationCount, dump: true);
			dT = cASolutionState.Halley_dt();
		}
		if (iterationCount >= maxIterations && GameSettings.VERBOSE_DEBUG_LOG && !Thread.CurrentThread.IsBackground)
		{
			Debug.Log("[Orbit] SolveClosestApproach: solve took too many iterations, result incorrect");
		}
		return cASolutionState.rstate.pos.magnitude;
	}

	public static bool _SolveSOI(Orbit p, Orbit s, ref double double_3, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		double num = RelativeStateAtUT(p, s, MaxUT, out var pstate, out var sstate, out var rstate);
		double magnitude = rstate.pos.magnitude;
		if (magnitude > Rsoi)
		{
			if (!Thread.CurrentThread.IsBackground)
			{
				Debug.Log("[Orbit] _SolveSOI: MaxUT puts p outside of s's SoI, punting to old solver");
			}
			return SolveSOI_BSP(p, s, ref double_3, dT, Rsoi, MinUT, MaxUT, epsilon, maxIterations, ref iterationCount);
		}
		iterationCount = 1;
		if (magnitude == Rsoi)
		{
			double_3 = MaxUT;
			return true;
		}
		UtilMath.SphereIntersection(Rsoi, rstate.pos, rstate.vel, out dT, later: false);
		double_3 = Math.Max(MinUT, MaxUT + dT);
		do
		{
			num = RelativeStateAtUT(p, s, double_3, out pstate, out sstate, out rstate);
			magnitude = rstate.pos.sqrMagnitude;
			dT = (Math.Sqrt(magnitude) * Rsoi - magnitude) / num;
			double_3 += dT;
			double_3 = Math.Min(MaxUT, Math.Max(MinUT, double_3));
		}
		while (iterationCount++ < maxIterations && !(Math.Abs(dT) <= epsilon));
		return iterationCount < maxIterations;
	}

	public static bool _SolveSOI_BSP(Orbit p, Orbit s, ref double double_3, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		if (double_3 < MinUT)
		{
			return false;
		}
		if (double_3 > MaxUT)
		{
			return false;
		}
		iterationCount = 0;
		bool result = false;
		double num = Rsoi * Rsoi;
		double num2 = Math.Abs((p.getPositionAtUT(double_3) - s.getPositionAtUT(double_3)).sqrMagnitude - num);
		while (!(dT <= epsilon) && iterationCount < maxIterations)
		{
			double num3 = (p.getPositionAtUT(double_3 + dT) - s.getPositionAtUT(double_3 + dT)).sqrMagnitude - num;
			double num4 = (p.getPositionAtUT(double_3 - dT) - s.getPositionAtUT(double_3 - dT)).sqrMagnitude - num;
			if (double_3 - dT < MinUT)
			{
				num4 = double.MaxValue;
			}
			if (double_3 + dT > MaxUT)
			{
				num3 = double.MaxValue;
			}
			if (num2 < 0.0 || num3 < 0.0 || num4 < 0.0)
			{
				result = true;
			}
			num3 = Math.Abs(num3);
			num4 = Math.Abs(num4);
			num2 = Math.Min(num2, Math.Min(num3, num4));
			if (num2 == num4)
			{
				double_3 -= dT;
			}
			else if (num2 == num3)
			{
				double_3 += dT;
			}
			dT /= 2.0;
			iterationCount++;
			if (!Thread.CurrentThread.IsBackground)
			{
				Debug.DrawLine(ScaledSpace.LocalToScaledSpace(p.referenceBody.position), ScaledSpace.LocalToScaledSpace(p.getPositionAtUT(double_3)), XKCDColors.LightMagenta * 0.5f);
			}
		}
		return result;
	}

	public static double NextCloseApproachTime(Orbit p, Orbit s, double double_3)
	{
		double num = double_3;
		double num2 = double.MaxValue;
		double num3 = double_3;
		double num4 = p.period;
		if (p.eccentricity > 1.0)
		{
			num4 = 100.0 / p.GetMeanMotion(p.semiMajorAxis);
		}
		double num5 = double_3 + num4;
		for (int i = 0; i < 8; i++)
		{
			double num6 = (num5 - num3) / 20.0;
			for (int j = 0; j < 20; j++)
			{
				double num7 = num3 + (double)j * num6;
				double num8 = Separation(p, s, num7);
				if (num8 < num2)
				{
					num2 = num8;
					num = num7;
				}
			}
			num3 = UtilMath.Clamp(num - num6, double_3, double_3 + num4);
			num5 = UtilMath.Clamp(num + num6, double_3, double_3 + num4);
		}
		return num;
	}

	public static double Separation(Orbit p, Orbit s, double double_3)
	{
		return (p.referenceBody.position + p.getRelativePositionAtUT(double_3).xzy - (s.referenceBody.position + s.getRelativePositionAtUT(double_3).xzy)).magnitude;
	}

	public static double AscendingNodeTrueAnomaly(Orbit p, Orbit s)
	{
		Vector3d orbitNormal = p.GetOrbitNormal();
		Vector3d normalized = Vector3d.Cross(s.GetOrbitNormal(), orbitNormal).normalized;
		return p.GetTrueAnomalyOfZupVector(normalized);
	}

	public static double DescendingNodeTrueAnomaly(Orbit p, Orbit s)
	{
		return (AscendingNodeTrueAnomaly(p, s) + Math.PI) % UtilMath.TwoPI;
	}

	public static double RelativeInclination(Orbit p, Orbit s)
	{
		return Math.Abs(Vector3d.Angle(-p.GetOrbitNormal().xzy.normalized, -s.GetOrbitNormal().xzy.normalized));
	}

	public static Orbit CreateRandomOrbitAround(CelestialBody body)
	{
		Orbit orbit = new Orbit();
		orbit.referenceBody = body;
		orbit.eccentricity = UnityEngine.Random.Range(0.0001f, 0.01f);
		orbit.semiMajorAxis = UnityEngine.Random.Range((float)body.atmosphereDepth, (float)body.sphereOfInfluence);
		orbit.inclination = UnityEngine.Random.Range(-0.001f, 0.001f);
		orbit.double_0 = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.argumentOfPeriapsis = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.meanAnomalyAtEpoch = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.epoch = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.Init();
		return orbit;
	}

	public static Orbit CreateRandomOrbitAround(CelestialBody body, double minAltitude, double maxAltitude)
	{
		Orbit orbit = new Orbit();
		orbit.referenceBody = body;
		orbit.eccentricity = UnityEngine.Random.Range(0.0001f, 0.01f);
		orbit.semiMajorAxis = UnityEngine.Random.Range((float)minAltitude, (float)maxAltitude);
		orbit.inclination = UnityEngine.Random.Range(-0.001f, 0.001f);
		orbit.double_0 = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.argumentOfPeriapsis = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.meanAnomalyAtEpoch = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.epoch = UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.Init();
		return orbit;
	}

	public static Orbit CreateRandomOrbitNearby(Orbit baseOrbit)
	{
		Orbit orbit = new Orbit();
		orbit.eccentricity = baseOrbit.eccentricity + (double)UnityEngine.Random.Range(0.0001f, 0.01f);
		orbit.semiMajorAxis = baseOrbit.semiMajorAxis * (double)UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.inclination = baseOrbit.inclination + (double)UnityEngine.Random.Range(-0.001f, 0.001f);
		orbit.double_0 = baseOrbit.double_0 * (double)UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.argumentOfPeriapsis = baseOrbit.argumentOfPeriapsis * (double)UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.meanAnomalyAtEpoch = baseOrbit.meanAnomalyAtEpoch * (double)UnityEngine.Random.Range(0.999f, 1.001f);
		orbit.epoch = baseOrbit.epoch;
		orbit.referenceBody = baseOrbit.referenceBody;
		orbit.Init();
		return orbit;
	}

	public static Orbit CreateRandomOrbitFlyBy(CelestialBody tgtBody, double daysToClosestApproach)
	{
		double periapsis = Math.Max(tgtBody.Radius * 3.0, tgtBody.sphereOfInfluence * (double)UnityEngine.Random.Range(0f, 1.1f));
		double deltaVatPeriapsis = UnityEngine.Random.Range(100f, 500f);
		return CreateRandomOrbitFlyBy(tgtBody.orbit, daysToClosestApproach * 24.0 * 60.0 * 60.0, periapsis, deltaVatPeriapsis);
	}

	public static Orbit CreateRandomOrbitFlyBy(Orbit targetOrbit, double timeToPeriapsis, double periapsis, double deltaVatPeriapsis)
	{
		double universalTime = Planetarium.GetUniversalTime();
		Vector3d relativePositionAtUT = targetOrbit.getRelativePositionAtUT(universalTime + timeToPeriapsis);
		Vector3d orbitalVelocityAtUT = targetOrbit.getOrbitalVelocityAtUT(universalTime + timeToPeriapsis);
		Orbit orbit = new Orbit();
		Vector3d vector3d = relativePositionAtUT + (Vector3d)UnityEngine.Random.onUnitSphere * periapsis;
		Vector3d vector3d2 = orbitalVelocityAtUT + (orbitalVelocityAtUT.normalized + (Vector3d)UnityEngine.Random.onUnitSphere) * deltaVatPeriapsis;
		orbit.UpdateFromStateVectors(vector3d, vector3d2, targetOrbit.referenceBody, universalTime + timeToPeriapsis);
		return orbit;
	}

	public static Vector3d Swizzle(Vector3d vec)
	{
		double y = vec.y;
		vec.y = vec.z;
		vec.z = y;
		return vec;
	}
}
