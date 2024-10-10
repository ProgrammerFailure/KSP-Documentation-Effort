using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class TransferMath
{
	public delegate double BisectFunc(double x);

	public static double safetyEnvelope = 1.025;

	public static readonly PatchedConics.SolverParameters solverParameters = new PatchedConics.SolverParameters();

	public static double epsilon = 0.0001;

	public static double AlignmentTime(TransferDataSimple transferDataSimple, Orbit sourceOrbit, Orbit targetOrbit, double startUT, double offsetDegrees = 0.0)
	{
		try
		{
			if (sourceOrbit != null && targetOrbit != null)
			{
				bool num = SameSOITransfer(transferDataSimple);
				bool flag = sourceOrbit.GetRelativeInclination(targetOrbit) > 90.0;
				double num2 = 0.0;
				double num3 = 0.0;
				if (num)
				{
					transferDataSimple.phaseAngleTarget = UtilMath.ClampRadians(Math.PI * (1.0 - Math.Pow((sourceOrbit.GetRadiusAtUT(startUT) + targetOrbit.semiMajorAxis) / (2.0 * targetOrbit.semiMajorAxis), 1.5)));
					sourceOrbit.referenceBody.GetLatLonAltOrbital(sourceOrbit.getPositionAtUT(startUT), out var _, out var lon, out var _);
					transferDataSimple.phaseAngleCurrent = UtilMath.ClampRadians(targetOrbit.PhaseAngle(startUT) - sourceOrbit.PhaseAngle(startUT, lon));
					num2 = transferDataSimple.phaseAngleCurrent - (transferDataSimple.phaseAngleTarget + offsetDegrees * (Math.PI / 180.0));
					num3 = UtilMath.TwoPI / targetOrbit.period - ((sourceOrbit.period == 0.0 || !sourceOrbit.referenceBody.rotates) ? 0.0 : (UtilMath.TwoPI / sourceOrbit.period));
					if (num2 > 0.0 && num3 > 0.0)
					{
						num2 -= UtilMath.TwoPI;
					}
					if (num2 < 0.0)
					{
						num2 += UtilMath.TwoPI;
					}
				}
				else if (!flag)
				{
					transferDataSimple.phaseAngleTarget = UtilMath.ClampRadians(Math.PI * (1.0 - Math.Pow((sourceOrbit.semiMajorAxis + targetOrbit.semiMajorAxis) / (2.0 * targetOrbit.semiMajorAxis), 1.5)));
					transferDataSimple.phaseAngleCurrent = UtilMath.ClampRadians(targetOrbit.PhaseAngle(startUT) - sourceOrbit.PhaseAngle(startUT));
					num2 = transferDataSimple.phaseAngleCurrent - (transferDataSimple.phaseAngleTarget + offsetDegrees * (Math.PI / 180.0));
					num3 = UtilMath.TwoPI / targetOrbit.period - ((sourceOrbit.period == 0.0 || !sourceOrbit.referenceBody.rotates) ? 0.0 : (UtilMath.TwoPI / sourceOrbit.period));
					if (num2 > 0.0 && num3 > 0.0)
					{
						num2 -= UtilMath.TwoPI;
					}
					if (num2 < 0.0)
					{
						num2 += UtilMath.TwoPI;
					}
				}
				else
				{
					transferDataSimple.phaseAngleTarget = UtilMath.TwoPI - UtilMath.ClampRadians(Math.PI * (1.0 - Math.Pow((sourceOrbit.semiMajorAxis + targetOrbit.semiMajorAxis) / (2.0 * targetOrbit.semiMajorAxis), 1.5)));
					transferDataSimple.phaseAngleCurrent = UtilMath.TwoPI - UtilMath.ClampRadians(targetOrbit.PhaseAngle(startUT) - sourceOrbit.PhaseAngle(startUT));
					num2 = transferDataSimple.phaseAngleCurrent - (transferDataSimple.phaseAngleTarget + offsetDegrees * (Math.PI / 180.0));
					num3 = UtilMath.TwoPI / targetOrbit.period + UtilMath.TwoPI / sourceOrbit.period;
				}
				return Math.Abs(num2 / num3);
			}
			return -1.0;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return -1.0;
		}
	}

	public static double TransferDV(TransferDataSimple transferDataSimple, double startUT)
	{
		try
		{
			if (transferDataSimple != null && !(transferDataSimple.vessel == null) && transferDataSimple.startingOrbit != null && !(transferDataSimple.TargetBody == null) && !(transferDataSimple.SourceBody == null))
			{
				Orbit startingOrbit = transferDataSimple.startingOrbit;
				Orbit orbit = transferDataSimple.TargetBody.GetOrbit();
				if (!SolveLambert(transferDataSimple, startUT, startingOrbit, orbit))
				{
					return -1.0;
				}
				transferDataSimple.circularizedV = 0.0;
				return transferDataSimple.transferdV.magnitude + transferDataSimple.circularizedV;
			}
			return -1.0;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return -1.0;
		}
	}

	public static bool SolveLambert(TransferDataSimple transferDataSimple, double startUT, Orbit startOrbit, Orbit targetOrbit)
	{
		try
		{
			transferDataSimple.startBurnTime = startUT;
			transferDataSimple.arrivalTime = startUT + transferDataSimple.transferTime;
			double tA = startOrbit.referenceBody.orbit.TrueAnomalyAtUT(startUT);
			Vector3d pos = Vector3d.zero;
			Vector3d vel = Vector3d.zero;
			Vector3d pos2 = Vector3d.zero;
			Vector3d vel2 = Vector3d.zero;
			Vector3d vi = vel;
			Vector3d vf = vel2;
			transferDataSimple.altitudeAtTransferTime = startOrbit.getRelativePositionAtUT(startUT).magnitude - startOrbit.referenceBody.Radius;
			startOrbit.referenceBody.orbit.GetOrbitalStateVectorsAtTrueAnomaly(tA, startUT, worldToLocal: false, out pos, out vel);
			tA = targetOrbit.TrueAnomalyAtUT(transferDataSimple.arrivalTime);
			targetOrbit.GetOrbitalStateVectorsAtTrueAnomaly(tA, transferDataSimple.arrivalTime, worldToLocal: false, out pos2, out vel2);
			double gravParameter = targetOrbit.referenceBody.gravParameter;
			double transferTime = transferDataSimple.transferTime;
			SuperiorLambert(gravParameter, pos, vel, pos2, vel2, transferTime, out vi, out vf);
			transferDataSimple.ejectionVelocity = vi - vel;
			transferDataSimple.captureVelocity = vel2 - vf;
			transferDataSimple.sourceBodyVelocity = vel;
			double magnitude = transferDataSimple.ejectionVelocity.magnitude;
			gravParameter = startOrbit.referenceBody.gravParameter;
			double magnitude2 = transferDataSimple.startingOrbit.getRelativePositionAtUT(startUT).magnitude;
			double sphereOfInfluence = startOrbit.referenceBody.sphereOfInfluence;
			double num = Math.Sqrt(gravParameter / magnitude2);
			double num2 = Math.Sqrt(magnitude * magnitude + 2.0 * num * num - 2.0 * gravParameter / sphereOfInfluence);
			transferDataSimple.vesselOriginOrbitalSpeed = num;
			double num3 = magnitude2 * num2 * num2 / gravParameter - 1.0;
			double num4 = magnitude2 * (1.0 + num3) / (1.0 - num3);
			if (num4 > 0.0 && num4 <= sphereOfInfluence)
			{
				return false;
			}
			if (!transferDataSimple.ejectionVelocity.z.Equals(0.0))
			{
				double num5 = transferDataSimple.ejectionVelocity.z / magnitude;
				transferDataSimple.ejectionInclination = Math.Asin(num5);
				magnitude = Math.Sqrt(num * num + num2 * num2 - 2.0 * num * num2 * Math.Sqrt(1.0 - num5 * num5));
			}
			else
			{
				transferDataSimple.ejectionInclination = 0.0;
				magnitude = num2 - num;
			}
			transferDataSimple.transferdV = transferDataSimple.ejectionVelocity.normalized * magnitude;
			return true;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return false;
		}
	}

	public static double FinalizeLambert(TransferDataSimple transferDataSimple, double startUT, Orbit startOrbit, Orbit targetOrbit)
	{
		try
		{
			if (!SolveLambert(transferDataSimple, startUT, startOrbit, targetOrbit))
			{
				transferDataSimple.calculationPercentage = 0;
				transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
				return -1.0;
			}
			CalcEjectionValues(transferDataSimple, out var ejectionDVNormal, out var ejectionDVPrograde, out var _, out var ejectionAngle, out var ejectionAngleisRetrograde);
			transferDataSimple.ejectAngle = ejectionAngle;
			Vector3d pos = Vector3d.zero;
			Vector3d vel = Vector3d.zero;
			startOrbit.GetOrbitalStateVectorsAtUT(startUT, out pos, out vel);
			transferDataSimple.ejectAngleRetrograde = ejectionAngleisRetrograde;
			transferDataSimple.transferdV = new Vector3d(0.0, ejectionDVNormal, ejectionDVPrograde);
			return transferDataSimple.transferdV.magnitude + transferDataSimple.correctiondV.magnitude;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return -1.0;
		}
	}

	public static bool CalculateCorrection(TransferDataSimple transferDataSimple, double startUT, SafeAbortBackgroundWorker bw)
	{
		try
		{
			if (transferDataSimple.SourceBody.orbit != null && !transferDataSimple.SourceBody.isStar)
			{
				NoBurnRequired(transferDataSimple);
				if (transferDataSimple.vessel != null && Orbit.RelativeInclination((transferDataSimple.SourceBody.orbit != null) ? transferDataSimple.SourceBody.orbit : transferDataSimple.startingOrbit, transferDataSimple.TargetBody.orbit) > 1.0)
				{
					double burnUT;
					double transferTime;
					Vector3d rhs = DVTimeInterCBXfer(transferDataSimple.startingOrbit, startUT, transferDataSimple.TargetBody.orbit, syncPhaseAngle: true, out burnUT, out transferTime);
					Orbit orbit = ((transferDataSimple.currentPositionNode == null) ? transferDataSimple.vessel.patchedConicSolver.FindPatchContainingUT(burnUT) : transferDataSimple.currentPositionNode.nextPatch);
					transferDataSimple.transferdV = new Vector3d(Vector3d.Dot(orbit.Radial(burnUT), rhs), Vector3d.Dot(-orbit.Normal(burnUT), rhs), Vector3d.Dot(orbit.Prograde(burnUT), rhs));
					transferDataSimple.startBurnTime = burnUT;
					double closestApproach = double.MaxValue;
					bool flag = InterceptBody(bw, orbit, transferDataSimple.TargetBody, transferDataSimple.transferdV, burnUT, 10, out var intercept, out closestApproach);
					if (bw == null || bw.CancellationPending)
					{
						return false;
					}
					transferDataSimple.arrivalTime = Orbit.NextCloseApproachTime(intercept, transferDataSimple.TargetBody.orbit, burnUT);
					transferDataSimple.transferTime = transferDataSimple.arrivalTime - burnUT;
					if (flag)
					{
						return true;
					}
					bool sameSOIXfer = SameSOITransfer(transferDataSimple);
					flag = FindIntercept(transferDataSimple, orbit, transferDataSimple.TargetBody, transferDataSimple.transferdV, transferDataSimple.startBurnTime, sameSOIXfer, closestApproach, bw, out var adjustedDV);
					if (bw == null || bw.CancellationPending)
					{
						return false;
					}
					transferDataSimple.transferdV = adjustedDV;
					InterceptBody(bw, orbit, transferDataSimple.TargetBody.referenceBody, adjustedDV, transferDataSimple.startBurnTime, 10, out intercept, out closestApproach);
					if (bw == null || bw.CancellationPending)
					{
						return false;
					}
					if (flag)
					{
						return true;
					}
					if (intercept != null && intercept.referenceBody == transferDataSimple.TargetBody.referenceBody)
					{
						transferDataSimple.correctionBurnRequired = true;
						double num = TimeToANDN(intercept, transferDataSimple.TargetBody.orbit, transferDataSimple.startBurnTime, ascending: false, closest: true, out transferDataSimple.otherNodeTime);
						if (double.IsNaN(num) || num < transferDataSimple.startBurnTime + transferDataSimple.transferTime * 30.0 / 100.0 || num > transferDataSimple.startBurnTime + transferDataSimple.transferTime * 80.0 / 100.0)
						{
							num = transferDataSimple.startBurnTime + transferDataSimple.transferTime / 2.0;
						}
						transferDataSimple.correctionBurnTime = num;
						transferDataSimple.correctiondV = CalculateCorrectionBurn(transferDataSimple, intercept, transferDataSimple.TargetBody.orbit, num);
						if (bw == null || bw.CancellationPending)
						{
							return false;
						}
						if (double.IsNaN(transferDataSimple.correctiondV.x) || double.IsNaN(transferDataSimple.correctiondV.y) || double.IsNaN(transferDataSimple.correctiondV.z))
						{
							NoBurnRequired(transferDataSimple);
							return false;
						}
						transferDataSimple.correctiondV = new Vector3d(Vector3d.Dot(orbit.Radial(num), transferDataSimple.correctiondV), Vector3d.Dot(-orbit.Normal(num), transferDataSimple.correctiondV), Vector3d.Dot(orbit.Prograde(num), transferDataSimple.correctiondV));
					}
					else
					{
						NoBurnRequired(transferDataSimple);
					}
				}
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return false;
		}
	}

	public static void NoBurnRequired(TransferDataSimple transferDataSimple)
	{
		transferDataSimple.correctionBurnRequired = false;
		transferDataSimple.correctionBurnTime = 0.0;
		transferDataSimple.correctiondV = Vector3d.zero;
	}

	public static Vector3d CalculateCorrectionBurn(TransferDataSimple transferDataSimple, Orbit currentOrbit, Orbit target, double burnUT)
	{
		Vector3d secondDV;
		return DeltaVToInterceptAtTime(currentOrbit, burnUT, target, transferDataSimple.arrivalTime - burnUT, out secondDV);
	}

	public static Vector3d DVTimeInterCBXfer(Orbit o, double double_0, Orbit target, bool syncPhaseAngle, out double burnUT, out double transferTime)
	{
		double burnUT2;
		Vector3d fromThat = DVTimeHohmann((!(o.referenceBody != null) || o.referenceBody.orbit == null) ? o : o.referenceBody.orbit, target, double_0, syncPhaseAngle, out burnUT2);
		Vector3d normalized = Vector3d.Exclude(-o.GetOrbitNormal().xzy.normalized, fromThat).normalized;
		double num = 0.5 * fromThat.sqrMagnitude - o.referenceBody.gravParameter / o.referenceBody.sphereOfInfluence;
		double semiMajorAxis = o.semiMajorAxis;
		double num2 = num + o.referenceBody.gravParameter / semiMajorAxis;
		double num3 = Math.Sqrt(2.0 * num2);
		Vector3d pos = o.referenceBody.position + semiMajorAxis * o.referenceBody.transformUp;
		Vector3d vector3d = num3 * o.referenceBody.transformRight;
		Orbit orbit = Orbit.OrbitFromStateVectors(pos, vector3d, o.referenceBody, 0.0);
		transferTime = orbit.GetNextTimeOfRadius(0.0, o.referenceBody.sphereOfInfluence);
		Vector3d xzy = orbit.getOrbitalVelocityAtUT(transferTime).xzy;
		double num4 = Math.Abs(Vector3d.Angle(vector3d, xzy));
		Vector3d vec = Quaternion.AngleAxis(0f - (float)(90.0 + num4), -o.GetOrbitNormal().xzy.normalized) * normalized;
		double tA = o.TrueAnomalyFromVector(vec);
		burnUT = o.TimeOfTrueAnomaly(tA, burnUT2 - o.period);
		if (burnUT2 - burnUT > o.period / 2.0 || burnUT < double_0)
		{
			burnUT += o.period;
		}
		Vector3d vector3d2 = Quaternion.AngleAxis(0f - (float)num4, -o.GetOrbitNormal().xzy.normalized) * normalized;
		Vector3d vector3d3 = num3 * vector3d2;
		Vector3d xzy2 = o.getOrbitalVelocityAtUT(burnUT).xzy;
		return vector3d3 - xzy2;
	}

	public static Vector3d DeltaVToInterceptAtTime(Orbit o, double initialUT, Orbit target, double double_0, out Vector3d secondDV)
	{
		double tA = o.TrueAnomalyAtUT(initialUT);
		o.GetOrbitalStateVectorsAtTrueAnomaly(tA, initialUT, worldToLocal: false, out var pos, out var vel);
		tA = target.TrueAnomalyAtUT(initialUT + double_0);
		target.GetOrbitalStateVectorsAtTrueAnomaly(tA, initialUT + double_0, worldToLocal: false, out var pos2, out var vel2);
		LambertSolver(target.referenceBody.gravParameter, pos, vel, pos2, vel2, double_0, 0, out var vi, out var vf);
		secondDV = vel2 - vf;
		return vi - vel;
	}

	public static void CalcEjectionValues(TransferDataSimple transferDataSimple, out double ejectionDVNormal, out double ejectionDVPrograde, out double ejectionHeading, out double ejectionAngle, out bool ejectionAngleisRetrograde)
	{
		try
		{
			double gravParameter = transferDataSimple.SourceBody.gravParameter;
			double sphereOfInfluence = transferDataSimple.SourceBody.sphereOfInfluence;
			double magnitude = transferDataSimple.ejectionVelocity.magnitude;
			double num = Math.Sqrt(magnitude * magnitude + 2.0 * transferDataSimple.vesselOriginOrbitalSpeed * transferDataSimple.vesselOriginOrbitalSpeed - 2.0 * gravParameter / sphereOfInfluence);
			ejectionDVNormal = num * Math.Sin(transferDataSimple.ejectionInclination);
			ejectionDVPrograde = num * Math.Cos(transferDataSimple.ejectionInclination) - transferDataSimple.vesselOriginOrbitalSpeed;
			ejectionHeading = Math.Atan2(ejectionDVPrograde, ejectionDVNormal);
			double num2 = gravParameter / (transferDataSimple.vesselOriginOrbitalSpeed * transferDataSimple.vesselOriginOrbitalSpeed);
			double num3 = num2 * num * num / gravParameter - 1.0;
			double num4 = Math.Acos((num2 / (1.0 - num3) * (1.0 - num3 * num3) - sphereOfInfluence) / (num3 * sphereOfInfluence));
			num4 += Math.Asin(num * num2 / (magnitude * sphereOfInfluence));
			ejectionAngle = EjectionAngleCalc(transferDataSimple.ejectionVelocity, num4, transferDataSimple.sourceBodyVelocity.normalized);
			if (transferDataSimple.TargetBody.orbit.semiMajorAxis < transferDataSimple.SourceBody.orbit.semiMajorAxis)
			{
				ejectionAngleisRetrograde = true;
				ejectionAngle -= Math.PI;
				if (ejectionAngle < 0.0)
				{
					ejectionAngle += Math.PI * 2.0;
				}
			}
			else
			{
				ejectionAngleisRetrograde = false;
			}
		}
		catch (Exception)
		{
			double num5 = 0.0;
			ejectionAngle = 0.0;
			double num6 = num5;
			num5 = 0.0;
			ejectionHeading = num6;
			double num7 = num5;
			num5 = 0.0;
			ejectionDVPrograde = num7;
			ejectionDVNormal = num5;
			ejectionAngleisRetrograde = false;
		}
	}

	public static bool SameSOITransfer(TransferDataSimple transferDataSimple)
	{
		if (!transferDataSimple.SourceBody.HasChild(transferDataSimple.TargetBody) && !transferDataSimple.SourceBody.HasParent(transferDataSimple.TargetBody) && !transferDataSimple.SourceBody.isStar && !transferDataSimple.TargetBody.isStar)
		{
			if (transferDataSimple.SourceBody.referenceBody != null && transferDataSimple.TargetBody.referenceBody != null)
			{
				return transferDataSimple.SourceBody.referenceBody.name == transferDataSimple.TargetBody.name;
			}
			return false;
		}
		return true;
	}

	public static double CalcSameSOITransferDV(TransferDataSimple transferDataSimple, Orbit startOrbit)
	{
		return CalcSameSOITransferDV(transferDataSimple, startOrbit, -1.0);
	}

	public static double CalcSameSOITransferDV(TransferDataSimple transferDataSimple, Orbit startOrbit, double overrideTime)
	{
		try
		{
			bool findBestUT = true;
			if (Orbit.RelativeInclination(startOrbit, transferDataSimple.TargetBody.orbit) > 1.0)
			{
				findBestUT = false;
				bool flag = startOrbit.GetRelativeInclination(transferDataSimple.TargetBody.orbit) > 90.0;
				if (overrideTime > -1.0)
				{
					transferDataSimple.startBurnTime = overrideTime;
				}
				else
				{
					double startBurnTime = TimeToANDN(startOrbit, transferDataSimple.TargetBody.orbit, transferDataSimple.startBurnTime, ascending: false, !flag, out transferDataSimple.otherNodeTime);
					transferDataSimple.startBurnTime = startBurnTime;
				}
			}
			Vector3d zero = Vector3d.zero;
			transferDataSimple.transferPe = SafeOrbitRadius(transferDataSimple.TargetBody) * 2.0 - transferDataSimple.TargetBody.Radius;
			if (transferDataSimple.SourceBody.HasParent(transferDataSimple.TargetBody))
			{
				Orbit destOrb = new Orbit(0.0, 0.0, transferDataSimple.transferPe + transferDataSimple.TargetBody.Radius, 1.0, 1.0, 1.0, 1.0, transferDataSimple.TargetBody);
				transferDataSimple.transferTime = CalcTransferTime(startOrbit.referenceBody.orbit, destOrb, transferDataSimple.startBurnTime);
				transferDataSimple.arrivalTime = transferDataSimple.startBurnTime + transferDataSimple.transferTime;
				zero = CalcdVReturn(transferDataSimple, startOrbit, transferDataSimple.startBurnTime, transferDataSimple.transferPe);
			}
			else
			{
				transferDataSimple.transferTime = CalcTransferTime(startOrbit, transferDataSimple.TargetBody.orbit, transferDataSimple.startBurnTime);
				transferDataSimple.arrivalTime = transferDataSimple.startBurnTime + transferDataSimple.transferTime;
				zero = CalcdV(transferDataSimple, startOrbit, findBestUT, transferDataSimple.startBurnTime, transferDataSimple.transferPe);
			}
			Vector3d zero2 = Vector3d.zero;
			if (transferDataSimple.startingOrbit.referenceBody.isStar)
			{
				double tA = transferDataSimple.startingOrbit.TrueAnomalyAtUT(transferDataSimple.startBurnTime);
				zero2 = transferDataSimple.startingOrbit.getOrbitalVelocityAtTrueAnomaly(tA, worldToLocal: false).xzy;
				tA = transferDataSimple.TargetBody.orbit.TrueAnomalyAtUT(transferDataSimple.arrivalTime);
				transferDataSimple.captureVelocity = transferDataSimple.TargetBody.orbit.getOrbitalVelocityAtTrueAnomaly(tA, worldToLocal: false).xzy;
			}
			else
			{
				zero2 = startOrbit.getOrbitalVelocityAtUT(transferDataSimple.startBurnTime).xzy;
				transferDataSimple.captureVelocity = transferDataSimple.TargetBody.orbit.getOrbitalVelocityAtUT(transferDataSimple.arrivalTime).xzy;
			}
			transferDataSimple.ejectionVelocity = zero2 + new Vector3d(0.0, 0.0, zero.z);
			transferDataSimple.altitudeAtTransferTime = startOrbit.getRelativePositionAtUT(transferDataSimple.startBurnTime).magnitude - startOrbit.referenceBody.Radius;
			transferDataSimple.circularizedV = 0.0;
			transferDataSimple.transferdV = zero;
			return zero.magnitude + transferDataSimple.circularizedV;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return 0.0;
		}
	}

	public static bool FindIntercept(TransferDataSimple transferDataSimple, Orbit startOrbit, CelestialBody targetBody, Vector3d dV, double double_0, bool sameSOIXfer, double initialClosestApproach, SafeAbortBackgroundWorker bw, out Vector3d adjustedDV)
	{
		try
		{
			adjustedDV = dV;
			double num = dV.z - (double)(sameSOIXfer ? 50 : 200);
			double num2 = dV.z + (double)(sameSOIXfer ? 50 : 200);
			string userState = "[TransferSimple]: startdV:" + num + " enddV:" + num2 + " initialClosestApproach " + initialClosestApproach;
			bw.ReportProgress(-1, userState);
			double closestApproach = initialClosestApproach;
			Vector3d newDV = Vector3d.zero;
			if (SearchDVRange(num, num2, 1.0, dV, startOrbit, targetBody, double_0, ref closestApproach, out adjustedDV, out newDV, bw))
			{
				return true;
			}
			if (!closestApproach.Equals(initialClosestApproach))
			{
				num = newDV.z - 5.0;
				num2 = newDV.z + 5.0;
				return SearchDVRange(num, num2, 0.1, newDV, startOrbit, targetBody, double_0, ref closestApproach, out adjustedDV, out newDV, bw);
			}
			return false;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			adjustedDV = dV;
			return false;
		}
	}

	public static bool SearchDVRange(double startDv, double endDv, double step, Vector3d dV, Orbit startOrbit, CelestialBody targetBody, double double_0, ref double closestApproach, out Vector3d adjustedDV, out Vector3d newDV, SafeAbortBackgroundWorker bw)
	{
		try
		{
			if (startDv > endDv)
			{
				double num = endDv;
				endDv = startDv;
				startDv = num;
			}
			adjustedDV = dV;
			newDV = dV;
			int num2 = (int)Math.Abs((Math.Abs(endDv) - Math.Abs(startDv)) / step) - 1;
			int num3 = 0;
			for (double num4 = startDv; num4 < endDv; num4 += step)
			{
				if (num3 >= num2)
				{
					break;
				}
				Vector3d vector3d = new Vector3d(dV.x, dV.y, num4);
				if (InterceptBody(bw, startOrbit, targetBody, vector3d, double_0, 3, out var _, out var closestApproach2))
				{
					if (bw == null || bw.CancellationPending)
					{
						return false;
					}
					if (closestApproach2 > 0.0 && !(closestApproach2 >= targetBody.sphereOfInfluence))
					{
						adjustedDV = vector3d;
						return true;
					}
				}
				if (closestApproach2 > 0.0 && closestApproach2 < double.MaxValue && closestApproach2 < closestApproach)
				{
					closestApproach = closestApproach2;
					newDV = vector3d;
				}
				if (bw != null && !bw.CancellationPending)
				{
					num3++;
					continue;
				}
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			adjustedDV = dV;
			newDV = dV;
			return false;
		}
	}

	public static Vector3d CalcdV(TransferDataSimple transferDataSimple, Orbit startObit, bool findBestUT, double startPeriapsis, double newApoapsis)
	{
		try
		{
			return DVTimeHohmann(startObit, transferDataSimple.TargetBody.orbit, transferDataSimple.startBurnTime, findBestUT, out transferDataSimple.startBurnTime);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return Vector3d.zero;
		}
	}

	public static Vector3d CalcdVReturn(TransferDataSimple transferDataSimple, Orbit startOrbit, double burnTime, double newPeriapsis)
	{
		try
		{
			Orbit orbit = startOrbit.referenceBody.orbit;
			Orbit orbit2 = new Orbit(orbit.inclination, orbit.eccentricity, newPeriapsis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, transferDataSimple.TargetBody);
			double num = burnTime;
			Vector3d vector3d = ((!(orbit2.semiMajorAxis < orbit.semiMajorAxis)) ? DVToChangeApoapsis(orbit, num, orbit2.semiMajorAxis) : DVToChangePeriapsis(orbit, num, orbit2.semiMajorAxis));
			Vector3d fromThat = vector3d;
			Vector3d normalized = Vector3d.Exclude(-startOrbit.GetOrbitNormal().xzy.normalized, fromThat).normalized;
			double num2 = 0.5 * fromThat.sqrMagnitude - startOrbit.referenceBody.gravParameter / startOrbit.referenceBody.sphereOfInfluence;
			double semiMajorAxis = startOrbit.semiMajorAxis;
			double num3 = num2 + startOrbit.referenceBody.gravParameter / semiMajorAxis;
			double num4 = Math.Sqrt(2.0 * num3);
			Vector3d vector3d2 = num4 * startOrbit.referenceBody.transformRight;
			Orbit orbit3 = Orbit.OrbitFromStateVectors(startOrbit.referenceBody.position + semiMajorAxis * startOrbit.referenceBody.transformUp, vector3d2, startOrbit.referenceBody, 0.0);
			double nextTimeOfRadius = orbit3.GetNextTimeOfRadius(0.0, startOrbit.referenceBody.sphereOfInfluence);
			Vector3d xzy = orbit3.getOrbitalVelocityAtUT(nextTimeOfRadius).xzy;
			double num5 = Math.Abs(Vector3d.Angle(vector3d2, xzy));
			Vector3d vec = Quaternion.AngleAxis(0f - (float)(90.0 + num5), -startOrbit.GetOrbitNormal().xzy.normalized) * normalized;
			double tA = startOrbit.TrueAnomalyFromVector(vec);
			burnTime = startOrbit.TimeOfTrueAnomaly(tA, num - startOrbit.period);
			if (num - burnTime > startOrbit.period / 2.0 || burnTime < num)
			{
				burnTime += startOrbit.period;
			}
			Vector3d vector3d3 = Quaternion.AngleAxis(0f - (float)num5, -startOrbit.GetOrbitNormal().xzy.normalized) * normalized;
			Vector3d vector3d4 = num4 * vector3d3;
			Vector3d xzy2 = startOrbit.getOrbitalVelocityAtUT(burnTime).xzy;
			transferDataSimple.startBurnTime = burnTime;
			return vector3d4 - xzy2;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return Vector3d.zero;
		}
	}

	public static void CalculateStartEndXferTimes(CelestialBody sourceBody, CelestialBody targetBody, double offsetDegrees, out double startTime, out double endTime)
	{
		double num = 0.0;
		endTime = 0.0;
		startTime = num;
		double universalTime = Planetarium.GetUniversalTime();
		CalculateStartEndXferTimes(sourceBody, targetBody, universalTime, offsetDegrees, out startTime, out endTime);
	}

	public static void CalculateStartEndXferTimes(CelestialBody sourceBody, CelestialBody targetBody, double afterUT, double offsetDegrees, out double startTime, out double endTime)
	{
		double num = 0.0;
		endTime = 0.0;
		startTime = num;
		TransferDataSimple transferDataSimple = new TransferDataSimple(null, null);
		transferDataSimple.SourceBody = sourceBody;
		transferDataSimple.TargetBody = targetBody;
		startTime = AlignmentTime(transferDataSimple, sourceBody.orbit, targetBody.orbit, afterUT, offsetDegrees);
		endTime = AlignmentTime(transferDataSimple, sourceBody.orbit, targetBody.orbit, afterUT, 0.0 - offsetDegrees);
	}

	public static void CalculateStartEndXferTimes(TransferDataSimple transferDataSimple, CelestialBody sourceBody, CelestialBody targetBody, double offsetDegrees, out string startTime, out string endTime)
	{
		startTime = (endTime = "xxd:xxh");
		double universalTime = Planetarium.GetUniversalTime();
		double time = AlignmentTime(transferDataSimple, sourceBody.orbit, targetBody.orbit, universalTime, offsetDegrees);
		double time2 = AlignmentTime(transferDataSimple, sourceBody.orbit, targetBody.orbit, universalTime, 0.0 - offsetDegrees);
		startTime = KSPUtil.PrintDateDeltaCompact(time, includeTime: true, includeSeconds: false, 2);
		endTime = KSPUtil.PrintDateDeltaCompact(time2, includeTime: true, includeSeconds: false, 2);
	}

	public static double CalcCircularizeDV(TransferDataSimple transferDataSimple, double newApoapsis)
	{
		try
		{
			if (newApoapsis > 0.0)
			{
				double magnitude = transferDataSimple.captureVelocity.magnitude;
				double num = Math.Sqrt(transferDataSimple.TargetBody.gravParameter / newApoapsis);
				magnitude = Math.Sqrt(magnitude * magnitude + 2.0 * num * num - 2.0 * transferDataSimple.TargetBody.gravParameter / transferDataSimple.TargetBody.sphereOfInfluence) - num;
				magnitude = (transferDataSimple.ejectionVelocity.normalized * magnitude).magnitude;
				if (transferDataSimple.startingOrbit.referenceBody.isStar || transferDataSimple.SourceBody.HasParent(transferDataSimple.TargetBody))
				{
					magnitude = transferDataSimple.captureVelocity.magnitude - magnitude;
				}
				return magnitude;
			}
			return 0.0;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return 0.0;
		}
	}

	public static double EjectionAngleCalc(Vector3d vsoi, double theta, Vector3d prograde)
	{
		Vector3d normalized = vsoi.normalized;
		double x = normalized.x;
		double y = normalized.y;
		double z = normalized.z;
		double num = Math.Cos(theta);
		double num2 = (0.0 - x) / y;
		double num3 = 1.0 + num2 * num2;
		double num4 = 2.0 * num2 * num / y;
		double num5 = num * num / (y * y) - 1.0;
		double num6 = ((!(num4 < 0.0)) ? (-0.5 * (num4 + Math.Sqrt(num4 * num4 - 4.0 * num3 * num5))) : (-0.5 * (num4 - Math.Sqrt(num4 * num4 - 4.0 * num3 * num5))));
		double num7 = num6 / num3;
		double y2 = num2 * num7 + num / y;
		try
		{
			if (Math.Sign(Vector3d.Cross(new Vector3d(num7, y2, 0.0), new Vector3d(x, y, z))[2]) != Math.Sign(Math.PI - theta))
			{
				num7 = num5 / num6;
				y2 = num2 * num7 + num / y;
			}
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return 0.0;
		}
		prograde = new Vector3d(prograde.x, prograde.y, 0.0);
		if (Vector3d.Cross(new Vector3d(num7, y2, 0.0), prograde).z < 0.0)
		{
			return Math.PI * 2.0 - Math.Acos(Vector3d.Dot(new Vector3d(num7, y2, 0.0), prograde));
		}
		return Math.Acos(Vector3d.Dot(new Vector3d(num7, y2, 0.0), prograde));
	}

	public static double SafeOrbitRadius(CelestialBody body)
	{
		return body.minOrbitalDistance * safetyEnvelope;
	}

	public static bool InterceptBody(SafeAbortBackgroundWorker bw, Orbit startOrbit, CelestialBody target, Vector3d dV, double double_0, int maxIter, out Orbit intercept, out double closestApproach)
	{
		closestApproach = double.MaxValue;
		try
		{
			maxIter = Math.Max(maxIter, 5);
			solverParameters.maxTimeSolverIterations = 10;
			Orbit orbit = new Orbit();
			startOrbit.GetOrbitalStateVectorsAtUT(double_0, out var state);
			Vector3d xzy = state.pos.xzy;
			Vector3d xzy2 = state.vel.xzy;
			Vector3d vector3d = (QuaternionD)Quaternion.LookRotation(xzy2, Vector3d.Cross(-xzy, xzy2)) * dV;
			orbit.previousPatch = startOrbit;
			orbit.UpdateFromFixedVectors(xzy.xzy, xzy2.xzy + vector3d.xzy, startOrbit.referenceBody, double_0);
			orbit.patchStartTransition = Orbit.PatchTransitionType.MANEUVER;
			orbit.StartUT = double_0;
			orbit.EndUT = ((orbit.eccentricity < 1.0) ? (double_0 + orbit.period) : orbit.period);
			int num = 0;
			Orbit orbit2 = new Orbit();
			PatchedConics.CalculatePatch(orbit, orbit2, orbit.epoch, solverParameters, target);
			if ((orbit.referenceBody == target || orbit.referenceBody == target.referenceBody) && (orbit.closestTgtApprUT > 0.0 || orbit.patchEndTransition == Orbit.PatchTransitionType.ENCOUNTER) && orbit.ClAppr > 0.0)
			{
				closestApproach = Math.Min(closestApproach, orbit.ClAppr);
			}
			while (orbit.referenceBody != target && num < maxIter)
			{
				if (bw != null && !bw.CancellationPending)
				{
					if (orbit2.referenceBody == null)
					{
						orbit2.referenceBody = orbit.referenceBody;
					}
					orbit = orbit2;
					orbit2 = new Orbit();
					orbit2.previousPatch = orbit;
					PatchedConics.CalculatePatch(orbit, orbit2, orbit.epoch, solverParameters, target);
					orbit2.previousPatch = orbit;
					if ((orbit.referenceBody == target || orbit.referenceBody == target.referenceBody) && (orbit.closestTgtApprUT > 0.0 || orbit.patchEndTransition == Orbit.PatchTransitionType.ENCOUNTER) && orbit.ClAppr > 0.0)
					{
						closestApproach = Math.Min(closestApproach, orbit.ClAppr);
					}
					num++;
					continue;
				}
				intercept = null;
				return false;
			}
			intercept = orbit;
			return intercept.referenceBody == target;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			intercept = null;
			return false;
		}
	}

	public static double TimeToANDN(Orbit currentOrbit, Orbit targetOrbit, double startTime, bool ascending, bool closest, out double otherNode)
	{
		try
		{
			otherNode = 0.0;
			double num = 0.0;
			if (ascending || closest)
			{
				num = currentOrbit.TimeOfTrueAnomaly(Orbit.AscendingNodeTrueAnomaly(currentOrbit, targetOrbit), startTime);
				if (ascending)
				{
					return num;
				}
			}
			double num2 = currentOrbit.TimeOfTrueAnomaly(Orbit.DescendingNodeTrueAnomaly(currentOrbit, targetOrbit), startTime);
			if (closest && num < num2)
			{
				otherNode = num2;
				return num;
			}
			otherNode = num;
			return num2;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			otherNode = 0.0;
			return 0.0;
		}
	}

	public static double Bisect(BisectFunc f, double minimum, double maximum, double acceptableRange = 0.01, int maxIter = 15)
	{
		int num = 1;
		double num2 = f(minimum);
		double num3 = f(maximum);
		double num4 = (minimum + maximum) / 2.0;
		if (Math.Sign(num2) == Math.Sign(num3))
		{
			return double.NaN;
		}
		while (Math.Abs(maximum - minimum) > epsilon)
		{
			num4 = (minimum + maximum) / 2.0;
			double num5 = f(num4);
			num++;
			if (Math.Sign(num5) == Math.Sign(num2))
			{
				minimum = num4;
				num2 = num5;
			}
			else
			{
				maximum = num4;
				num3 = num5;
			}
			if (num > maxIter)
			{
				break;
			}
		}
		if (Math.Abs(num3 - num2) < acceptableRange)
		{
			return num4;
		}
		return double.NaN;
	}

	public static double Root(BisectFunc f, double a, double b, double tol, int maxIterations = 50, int sign = 0)
	{
		double num = f(a);
		double num2 = f(b);
		if (num * num2 >= 0.0)
		{
			if (Thread.CurrentThread.IsBackground)
			{
				if ((bool)ManeuverTool.Instance)
				{
					ManeuverTool.Instance.InvokeAsync(delegate
					{
						Debug.Log("[TransferMath]:  Root method values cannot bracket the root.");
					});
				}
			}
			else
			{
				Debug.Log("[TransferMath]: Root method values cannot bracket the root.");
			}
			return double.NaN;
		}
		if (Math.Abs(num) < Math.Abs(num2))
		{
			double num3 = a;
			a = b;
			b = num3;
			double num4 = num;
			num = num2;
			num2 = num4;
		}
		double num5 = double.MaxValue;
		double num6 = a;
		double num7 = num;
		bool flag = true;
		int num8 = 0;
		double num9 = 0.0;
		double num10 = 0.0;
		while ((num2 != 0.0 && Math.Abs(a - b) > tol) || (sign != 0 && Math.Sign(num2) != sign))
		{
			num9 = ((num == num7 || num2 == num7) ? (b - num2 * (b - a) / (num2 - num)) : (a * num2 * num7 / (num - num2) / (num - num7) + b * num * num7 / (num2 - num) / (num2 - num7) + num6 * num * num2 / (num7 - num) / (num7 - num2)));
			double num11 = (3.0 * a + b) / 4.0;
			if (((num9 > num11 && num9 < b) || (num9 < num11 && num9 > b)) && (!flag || !(Math.Abs(num9 - b) >= Math.Abs(b - num6) / 2.0)) && (flag || Math.Abs(num9 - b) < Math.Abs(num6 - num5) / 2.0))
			{
				if ((flag && Math.Abs(b - num6) < tol) || (!flag && Math.Abs(num6 - num5) < tol))
				{
					num9 = (a + b) / 2.0;
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				num9 = (a + b) / 2.0;
				flag = true;
			}
			num10 = f(num9);
			num5 = num6;
			num6 = b;
			num7 = num2;
			if (num * num10 < 0.0)
			{
				b = num9;
				num2 = num10;
			}
			else
			{
				a = num9;
				num = num10;
			}
			if (Math.Abs(num) < Math.Abs(num2))
			{
				double num12 = a;
				a = b;
				b = num12;
				double num13 = num;
				num = num2;
				num2 = num13;
			}
			num8++;
		}
		return b;
	}

	public static double CalcTransferTime(Orbit origOrb, Orbit destOrb, double transTime, double arrivalPhaseAngle = double.NegativeInfinity)
	{
		return 0.5 * OrbitalPeriod(destOrb.referenceBody, origOrb.GetRadiusAtUT(transTime), destOrb.GetRadiusAtPhaseAngle((!double.IsNegativeInfinity(arrivalPhaseAngle)) ? arrivalPhaseAngle : (origOrb.PhaseAngle(transTime) + Math.PI)));
	}

	public static double HohmannTimeOfFlight(Orbit origin, Orbit destination)
	{
		double num = (origin.semiMajorAxis + destination.semiMajorAxis) * 0.5;
		double gravParameter = origin.referenceBody.gravParameter;
		return Math.PI * Math.Sqrt(num * num * num / gravParameter);
	}

	public static double OrbitalPeriod(CelestialBody body, double apoapsis, double periapsis)
	{
		if (body == null)
		{
			return -1.0;
		}
		double x = (apoapsis + periapsis) / 2.0;
		return Math.Sqrt(4.0 * Math.Pow(Math.PI, 2.0) * Math.Pow(x, 3.0) / body.gravParameter);
	}

	public static Vector3d DVTimeHohmann(Orbit o, Orbit target, double double_0, bool findBestUT, out double burnUT)
	{
		try
		{
			double num = Orbit.SynodicPeriod(o, target);
			double aPPhaseAng = 0.0;
			Vector3d result = DVApPhaseHohmann(o, target, double_0, out aPPhaseAng);
			if (!findBestUT)
			{
				burnUT = double_0;
				return result;
			}
			double num2 = double_0;
			double num3 = double_0 + 1.5 * num;
			int num4 = 30;
			double num5 = (num3 - num2) / 30.0;
			int num6 = 1;
			while (true)
			{
				if (num6 <= num4)
				{
					double num7 = num2 + num5 * (double)num6;
					double aPPhaseAng2 = 0.0;
					DVApPhaseHohmann(o, target, num7, out aPPhaseAng2);
					if (!(Math.Abs(aPPhaseAng2) < 90.0) || Math.Sign(aPPhaseAng) == Math.Sign(aPPhaseAng2))
					{
						if (num6 == 1 && Math.Abs(aPPhaseAng) < 0.5 && Math.Sign(aPPhaseAng) == Math.Sign(aPPhaseAng2))
						{
							break;
						}
						aPPhaseAng = aPPhaseAng2;
						num6++;
						continue;
					}
					num2 = num7 - num5;
					num3 = num7;
				}
				burnUT = 0.0;
				BisectFunc f = delegate(double testTime)
				{
					DVApPhaseHohmann(o, target, testTime, out var aPPhaseAng4);
					return aPPhaseAng4;
				};
				try
				{
					burnUT = Root(f, num2, num3, 1E-08);
				}
				catch (Exception)
				{
					burnUT = double_0;
				}
				if (double.IsNaN(burnUT))
				{
					burnUT = double_0;
				}
				double aPPhaseAng3;
				return DVApPhaseHohmann(o, target, burnUT, out aPPhaseAng3);
			}
			burnUT = double_0;
			return result;
		}
		catch (Exception ex2)
		{
			Exception ex3 = ex2;
			Exception e = ex3;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			burnUT = double_0;
			return Vector3d.zero;
		}
	}

	public static Vector3d DVApPhaseHohmann(Orbit o, Orbit target, double double_0, out double aPPhaseAng)
	{
		try
		{
			Vector3d vec = -o.getRelativePositionAtUT(double_0).xzy;
			double num = target.RadiusAtTrueAnomaly(target.TrueAnomalyFromVector(vec));
			Vector3d vector3d;
			if (num > o.ApR)
			{
				vector3d = DVToChangeApoapsis(o, double_0, num);
				if (double.IsNaN(vector3d.x))
				{
					vector3d = Vector3d.zero;
				}
				Orbit orbit = TransferOrbit(o, double_0, vector3d);
				double nextApoapsisTime = orbit.GetNextApoapsisTime(double_0);
				Vector3d vector3d2 = Quaternion.AngleAxis(0f - (float)orbit.double_0, Planetarium.up) * Planetarium.right;
				Vector3d vector3d3 = Quaternion.AngleAxis((float)orbit.argumentOfPeriapsis, -orbit.GetOrbitNormal().xzy.normalized) * vector3d2;
				Vector3d vec2 = (0.0 - orbit.ApR) * vector3d3;
				double tA = target.TrueAnomalyFromVector(vec2);
				double num2 = 360.0 * (target.TimeOfTrueAnomaly(tA, double_0) - nextApoapsisTime) / target.period;
				aPPhaseAng = num2;
			}
			else
			{
				vector3d = DVToChangePeriapsis(o, double_0, num);
				if (double.IsNaN(vector3d.x))
				{
					vector3d = Vector3d.zero;
				}
				Orbit orbit2 = TransferOrbit(o, double_0, vector3d);
				double nextPeriapsisTime = orbit2.GetNextPeriapsisTime(double_0);
				Vector3d vector3d4 = Quaternion.AngleAxis(0f - (float)orbit2.double_0, Planetarium.up) * Planetarium.right;
				Vector3d vector3d5 = Quaternion.AngleAxis((float)orbit2.argumentOfPeriapsis, -orbit2.GetOrbitNormal().xzy.normalized) * vector3d4;
				Vector3d vec3 = orbit2.PeR * vector3d5;
				double tA2 = target.TrueAnomalyFromVector(vec3);
				double num3 = 360.0 * (target.TimeOfTrueAnomaly(tA2, double_0) - nextPeriapsisTime) / target.period;
				aPPhaseAng = num3;
			}
			if (double.IsNaN(aPPhaseAng))
			{
				aPPhaseAng = 0.0;
			}
			aPPhaseAng = UtilMath.ClampDegrees180(aPPhaseAng);
			return vector3d;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			aPPhaseAng = 0.0;
			return Vector3d.zero;
		}
	}

	public static bool ChangingPeriapsis(Orbit start, Orbit target, double double_0)
	{
		Vector3d vec = -start.getRelativePositionAtUT(double_0).xzy;
		if (target.RadiusAtTrueAnomaly(target.TrueAnomalyFromVector(vec)) <= start.ApR)
		{
			return true;
		}
		return false;
	}

	public static Vector3d DVToChangePeriapsis(Orbit o, double double_0, double newPe)
	{
		try
		{
			double magnitude = o.getRelativePositionAtUT(double_0).xzy.magnitude;
			newPe = UtilMath.Clamp(newPe, 1.0, magnitude - 1.0);
			bool flag = newPe > o.PeR;
			Vector3d burnDir = (flag ? 1 : (-1)) * o.Horizontal(double_0);
			double a = 0.0;
			double num;
			if (flag)
			{
				num = 0.25;
				while (TransferOrbit(o, double_0, num * burnDir).PeR < newPe)
				{
					a = num;
					num *= 2.0;
					if (num > 100000.0)
					{
						break;
					}
				}
			}
			else
			{
				num = Math.Abs(Vector3d.Dot(o.getOrbitalVelocityAtUT(double_0).xzy, burnDir));
			}
			BisectFunc f = (double testDeltaV) => TransferOrbit(o, double_0, testDeltaV * burnDir).PeR - newPe;
			double num2 = 0.0;
			try
			{
				num2 = Root(f, a, num, 1E-08);
			}
			catch (Exception)
			{
				num2 = 0.0;
			}
			if (double.IsNaN(num2))
			{
				num2 = 0.0;
			}
			return num2 * burnDir;
		}
		catch (Exception ex2)
		{
			Exception ex3 = ex2;
			Exception e = ex3;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return Vector3d.zero;
		}
	}

	public static Vector3d DVToChangeApoapsis(Orbit o, double double_0, double newAp)
	{
		try
		{
			double magnitude = o.getRelativePositionAtUT(double_0).xzy.magnitude;
			if (newAp > 0.0)
			{
				newAp = Math.Max(newAp, magnitude + 1.0);
			}
			bool flag = false;
			flag = (o.ApR > 0.0 && newAp < 0.0) || ((!(o.ApR < 0.0) || !(newAp > 0.0)) && newAp > o.ApR);
			Vector3d burnDir = (flag ? 1 : (-1)) * o.Prograde(double_0);
			double a = 0.0;
			double b = (flag ? 10000.0 : o.getOrbitalVelocityAtUT(double_0).xzy.magnitude);
			BisectFunc f = (double testDeltaV) => 1.0 / TransferOrbit(o, double_0, testDeltaV * burnDir).ApR - 1.0 / newAp;
			double num = 0.0;
			try
			{
				num = Root(f, a, b, 1E-08);
			}
			catch (Exception)
			{
				num = 0.0;
			}
			if (double.IsNaN(num))
			{
				num = 0.0;
			}
			return num * burnDir;
		}
		catch (Exception ex2)
		{
			Exception ex3 = ex2;
			Exception e = ex3;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return Vector3d.zero;
		}
	}

	public static Orbit TransferOrbit(Orbit o, double double_0, Vector3d dV)
	{
		return Orbit.OrbitFromStateVectors(o.referenceBody.position + o.getRelativePositionAtUT(double_0).xzy, o.getOrbitalVelocityAtUT(double_0).xzy + dV, o.referenceBody, double_0);
	}

	public static void LambertSolver(double mu, Vector3d r1, Vector3d v1, Vector3d r2, Vector3d v2, double tof, int nrev, out Vector3d vi, out Vector3d vf)
	{
		double magnitude = r1.magnitude;
		double magnitude2 = r2.magnitude;
		Vector3d lhs = Vector3d.Cross(r1, v1);
		lhs /= lhs.magnitude;
		Vector3d vector3d = r1 / magnitude;
		Vector3d vector3d2 = r2 / magnitude2;
		Vector3d vector3d3 = Vector3d.Cross(vector3d, vector3d2);
		vector3d3 /= vector3d3.magnitude;
		double num = Vector3d.Dot(vector3d, vector3d2);
		if (num > 1.0)
		{
			num = 1.0;
		}
		if (num < -1.0)
		{
			num = -1.0;
		}
		num = Math.Acos(num);
		double num2 = Vector3d.Dot(lhs, vector3d3);
		if (num2 > 1.0)
		{
			num2 = 1.0;
		}
		if (num2 < -1.0)
		{
			num2 = -1.0;
		}
		num2 = Math.Acos(num2);
		if (num2 > Math.PI / 2.0 && tof > 0.0)
		{
			num = Math.PI * 2.0 - num;
			vector3d3 = -vector3d3;
		}
		if (num2 < Math.PI / 2.0 && tof < 0.0)
		{
			num = Math.PI * 2.0 - num;
			vector3d3 = -vector3d3;
		}
		Vector3d lhs2 = vector3d3;
		Vector3d vector3d4 = Vector3d.Cross(vector3d3, vector3d);
		vector3d4 /= vector3d4.magnitude;
		Vector3d vector3d5 = Vector3d.Cross(lhs2, vector3d2);
		vector3d5 /= vector3d5.magnitude;
		num += Math.PI * 2.0 * (double)Math.Abs(nrev);
		double VR = 0.0;
		double VT = 0.0;
		double VR2 = 0.0;
		double VT2 = 0.0;
		double VR3 = 0.0;
		double VT3 = 0.0;
		double VR4 = 0.0;
		double VT4 = 0.0;
		int int_ = 0;
		LambertRHG(mu, magnitude, magnitude2, num, tof, out int_, out VR, out VT, out VR2, out VT2, out VR3, out VT3, out VR4, out VT4);
		if (nrev > 0)
		{
			switch (int_)
			{
			case -1:
				vi = Vector3d.zero;
				vf = Vector3d.zero;
				break;
			case 0:
				vi = Vector3d.zero;
				vf = Vector3d.zero;
				break;
			}
		}
		if (nrev > 0 && int_ > 1)
		{
			vi = VR3 * vector3d + VR3 * vector3d4;
			vf = VR4 * vector3d2 + VT4 * vector3d5;
		}
		else
		{
			vi = VR * vector3d + VT * vector3d4;
			vf = VR2 * vector3d2 + VT2 * vector3d5;
		}
	}

	public static void LambertRHG(double double_0, double R1, double R2, double double_1, double TDELT, out int int_0, out double VR11, out double VT11, out double VR12, out double VT12, out double VR21, out double VT21, out double VR22, out double VT22)
	{
		double num = 0.0;
		VT22 = 0.0;
		double num2 = num;
		num = 0.0;
		VT21 = num2;
		double num3 = num;
		num = 0.0;
		VT12 = num3;
		double num4 = num;
		num = 0.0;
		VT11 = num4;
		double num5 = num;
		num = 0.0;
		VR22 = num5;
		double num6 = num;
		num = 0.0;
		VR21 = num6;
		double num7 = num;
		num = 0.0;
		VR12 = num7;
		VR11 = num;
		int_0 = 0;
		double num8 = Math.PI * 2.0;
		int IGTL = 0;
		double num9 = double_1;
		int num10 = 0;
		while (num9 > num8)
		{
			num9 = (num9 = num8);
			num10++;
		}
		num9 /= 2.0;
		double num11 = R1 - R2;
		double num12 = R1 * R2;
		double num13 = 4.0 * num12 * Math.Pow(Math.Sin(num9), 2.0);
		double num14 = Math.Pow(num11, 2.0) + num13;
		double num15 = Math.Sqrt(num14);
		double num16 = (R1 + R2 + num15) / 2.0;
		double num17 = Math.Sqrt(double_0 * num16 / 2.0);
		double qSQFM = num15 / num16;
		double double_2 = Math.Sqrt(num12) * Math.Cos(num9) / num16;
		double num18 = 0.0;
		double d = 1.0;
		if (num15 != 0.0)
		{
			num18 = num11 / num15;
			d = num13 / num14;
		}
		double double_3 = 4.0 * num17 * TDELT / Math.Pow(num16, 2.0);
		double num19 = 0.0;
		double double_4 = 0.0;
		double double_5 = 0.0;
		double double_6 = 0.0;
		double double_7 = 0.0;
		double D2T = 0.0;
		double D3T = 0.0;
		LamGXL(num10, double_2, qSQFM, double_3, ref int_0, ref double_4, ref double_5, ref IGTL);
		for (int i = 1; i <= int_0; i++)
		{
			num19 = ((i != 1) ? double_5 : double_4);
			LamGTL(num10, double_2, qSQFM, num19, -1.0, out double_6, out double_7, out D2T, out D3T, ref IGTL);
			double num20 = num17 * D3T * Math.Sqrt(d);
			double num21 = num17 * (double_7 - D2T * num18) / R1;
			double num22 = num20 / R1;
			double num23 = (0.0 - num17) * (double_7 + D2T * num18) / R2;
			num20 /= R2;
			if (i == 1)
			{
				VR11 = num21;
				VT11 = num22;
				VR12 = num23;
				VT12 = num20;
			}
			else
			{
				VR21 = num21;
				VT21 = num22;
				VR22 = num23;
				VT22 = num20;
			}
		}
	}

	public static void LamGXL(double double_0, double double_1, double QSQFM1, double double_2, ref int int_0, ref double double_3, ref double double_4, ref int IGTL)
	{
		double num = 1.7;
		double num2 = 0.5;
		double num3 = 0.03;
		double num4 = 0.15;
		double num5 = 1.0;
		double num6 = 0.24;
		double num7 = 3E-07;
		double double_5 = 0.0;
		double double_6 = 0.0;
		double D2T = 0.0;
		double D3T = 0.0;
		double num8 = 0.0;
		double num9 = Math.Atan2(QSQFM1, 2.0 * double_1) / Math.PI;
		if (double_0 == 0.0)
		{
			int_0 = 1;
			LamGTL(double_0, double_1, QSQFM1, 0.0, 0.0, out double_5, out double_6, out D2T, out D3T, ref IGTL);
			double num10 = double_2 - double_5;
			if (num10 <= 0.0)
			{
				double_3 = double_5 * num10 / (-4.0 * double_2);
				return;
			}
			double_3 = (0.0 - num10) / (num10 + 4.0);
			num8 = double_3 + num * Math.Sqrt(2.0 * (1.0 - num9));
			if (num8 < 0.0)
			{
				double_3 -= Math.Sqrt(D8RT(0.0 - num8)) * (double_3 + Math.Sqrt(num10 / (num10 + 1.5 * double_5)));
			}
			num8 = 4.0 / (4.0 + num10);
			double_3 *= 1.0 + double_3 * (num2 * num8 - num3 * double_3 * Math.Sqrt(num8));
			return;
		}
		double num11 = 1.0 / (1.5 * (double_0 + 0.5) * Math.PI);
		if (num9 < 0.5)
		{
			num11 = D8RT(2.0 * num9) * num11;
		}
		if (num9 > 0.5)
		{
			num11 = (2.0 - D8RT(2.0 - 2.0 * num9)) * num11;
		}
		double double_7 = 0.0;
		double num12 = 0.0;
		bool flag = false;
		for (int i = 1; i <= 12; i++)
		{
			LamGTL(double_0, double_1, QSQFM1, num11, 3.0, out double_7, out double_6, out D2T, out D3T, ref IGTL);
			if (D2T != 0.0)
			{
				double num13 = num11;
				num11 -= double_6 * D2T / (D2T * D2T - double_6 * D3T / 2.0);
				if (!(Math.Abs(num13 / num11 - 1.0) > num7))
				{
					flag = true;
					break;
				}
				continue;
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			int_0 = -1;
			return;
		}
		double num14 = double_2 - double_7;
		bool flag2 = true;
		if (num14 < 0.0)
		{
			int_0 = 0;
			return;
		}
		if (num14 == 0.0)
		{
			double_3 = num11;
			int_0 = 1;
			return;
		}
		int_0 = 3;
		if (D2T == 0.0)
		{
			D2T = 6.0 * double_0 * Math.PI;
		}
		double_3 = Math.Sqrt(num14 / (D2T / 2.0 + num14 / Math.Pow(1.0 - num11, 2.0)));
		num8 = num11 + double_3;
		num8 = num8 * 4.0 / (4.0 + num14) + Math.Pow(1.0 - num8, 2.0);
		double_3 = double_3 * (1.0 - (1.0 + double_0 + num5 * (num9 - 0.5)) / (1.0 + num4 * double_0) * double_3 * (num2 * num8 + num3 * double_3 * Math.Sqrt(num8))) + num11;
		num12 = D2T / 2.0;
		if (double_3 >= 1.0)
		{
			int_0 = 1;
			flag2 = false;
		}
		double double_8 = 0.0;
		while (true)
		{
			if (flag2)
			{
				for (int j = 0; j < 3; j++)
				{
					LamGTL(double_0, double_1, QSQFM1, double_3, 2.0, out double_8, out double_6, out D2T, out D3T, ref IGTL);
					double_8 = double_2 - double_8;
					if (double_6 != 0.0)
					{
						double_3 += double_8 * double_6 / (double_6 * double_6 + double_8 * D2T / 2.0);
					}
				}
				if (int_0 != 3)
				{
					break;
				}
				int_0 = 2;
				double_4 = double_3;
			}
			flag2 = true;
			LamGTL(double_0, double_1, QSQFM1, 0.0, 0.0, out double_5, out double_6, out D2T, out D3T, ref IGTL);
			double num15 = double_5 - double_7;
			double num16 = double_2 - double_5;
			if (num16 <= 0.0)
			{
				double_3 = num11 - Math.Sqrt(num14 / (num12 - num14 * (num12 / num15 - 1.0 / Math.Pow(num11, 2.0))));
				continue;
			}
			double_3 = (0.0 - num16) / (num16 + 4.0);
			num8 = double_3 + num * Math.Sqrt(2.0 * (1.0 - num9));
			if (num8 < 0.0)
			{
				double_3 -= Math.Sqrt(D8RT(0.0 - num8)) * (double_3 + Math.Sqrt(num16 / (num16 + 1.5 * double_5)));
			}
			num8 = 4.0 / (4.0 + num16);
			double_3 *= 1.0 + (1.0 + double_0 + num6 * (num9 - 0.5)) / (1.0 + num4 * double_0) * double_3 * (num2 * num8 - num3 * double_3 * Math.Sqrt(num8));
			if (double_3 <= -1.0)
			{
				int_0--;
				if (int_0 == 1)
				{
					double_3 = double_4;
				}
			}
		}
	}

	public static double D8RT(double x)
	{
		return Math.Sqrt(Math.Sqrt(Math.Sqrt(x)));
	}

	public static void LamGTL(double double_0, double double_1, double QSQFM1, double double_2, double double_3, out double double_4, out double double_5, out double D2T, out double D3T, ref int IGTL)
	{
		double num = 0.4;
		double_4 = 0.0;
		double_5 = 0.0;
		D2T = 0.0;
		D3T = 0.0;
		IGTL++;
		bool flag = double_3 == -1.0;
		bool flag2 = double_3 >= 1.0;
		bool flag3 = double_3 >= 2.0;
		bool flag4 = double_3 == 3.0;
		double num2 = double_1 * double_1;
		double num3 = double_2 * double_2;
		double num4 = (1.0 - double_2) * (1.0 + double_2);
		if (!flag)
		{
			double_5 = 0.0;
			D2T = 0.0;
			D3T = 0.0;
		}
		if (!flag && !(double_0 > 0.0) && !(double_2 < 0.0) && Math.Abs(num4) <= num)
		{
			double num5 = 1.0;
			double num6 = 0.0;
			double num7 = 0.0;
			double num8 = 0.0;
			if (flag2)
			{
				num6 = 1.0;
			}
			if (flag3)
			{
				num7 = 1.0;
			}
			if (flag4)
			{
				num8 = 1.0;
			}
			double num9 = 4.0;
			double num10 = double_1 * QSQFM1;
			int num11 = 0;
			double num12 = 0.0;
			if (double_1 < 0.5)
			{
				num12 = 1.0 - double_1 * num2;
			}
			if (double_1 >= 0.5)
			{
				num12 = (1.0 / (1.0 + double_1) + double_1) * QSQFM1;
			}
			double num13 = num9 / 3.0;
			double_4 = num13 * num12;
			double num14 = double_4;
			do
			{
				num11++;
				double num15 = num11;
				num5 *= num4;
				if (flag2 && num11 > 1)
				{
					num6 *= num4;
				}
				if (flag3 && num11 > 2)
				{
					num7 *= num4;
				}
				if (flag4 && num11 > 3)
				{
					num8 *= num4;
				}
				num9 = num9 * (num15 - 0.5) / num15;
				num10 *= num2;
				num12 += num10;
				num14 = double_4;
				double num16 = num9 / (2.0 * num15 + 3.0);
				double num17 = num16 * num12;
				double_4 -= num5 * ((1.5 * num15 + 0.25) * num17 / (num15 * num15 - 0.25) - num13 * num10);
				num13 = num16;
				num17 *= num15;
				if (flag2)
				{
					double_5 += num17 * num6;
				}
				if (flag3)
				{
					D2T += num17 * num7 * (num15 - 1.0);
				}
				if (flag4)
				{
					D3T += num17 * num8 * (num15 - 1.0) * (num15 - 2.0);
				}
			}
			while ((double)num11 < double_3 || double_4 != num14);
			if (flag4)
			{
				D3T = 8.0 * double_2 * (1.5 * D2T - num3 * D3T);
			}
			if (flag3)
			{
				D2T = 2.0 * (2.0 * num3 * D2T - double_5);
			}
			if (flag2)
			{
				double_5 = -2.0 * double_2 * double_5;
			}
			double_4 /= num3;
			return;
		}
		double num18 = Math.Sqrt(Math.Abs(num4));
		double num19 = Math.Sqrt(QSQFM1 + num2 * num3);
		double num20 = double_1 * double_2;
		double num21 = 0.0;
		double num22 = 0.0;
		double num23 = 0.0;
		double num24 = 0.0;
		double num25 = 0.0;
		double num26 = 0.0;
		if (num20 <= 0.0)
		{
			num21 = num19 - num20;
			num22 = double_1 * num19 - double_2;
		}
		if (num20 < 0.0 && flag)
		{
			num23 = QSQFM1 / num21;
			num24 = QSQFM1 * (num2 * num4 - num3) / num22;
		}
		if ((num20 == 0.0 && flag) || num20 > 0.0)
		{
			num23 = num19 + num20;
			num24 = double_1 * num19 + double_2;
		}
		if (num20 > 0.0)
		{
			num21 = QSQFM1 / num23;
			num22 = QSQFM1 * (num2 * num4 - num3) / num24;
		}
		if (!flag)
		{
			num25 = ((!(num20 * num4 >= 0.0)) ? ((num3 - num2 * num4) / (double_2 * num19 - double_1 * num4)) : (double_2 * num19 + double_1 * num4));
			num26 = num21 * num18;
			if (double_2 <= 1.0)
			{
				double_4 = double_0 * Math.PI + Math.Atan2(num26, num25);
			}
			else if (num26 > num)
			{
				double_4 = Math.Log(num26 + num25);
			}
			else
			{
				double num27 = num26 / (num25 + 1.0);
				double num28 = 2.0 * num27;
				double num29 = num27 * num27;
				double_4 = num28;
				double num30 = 1.0;
				double num31 = double_4;
				do
				{
					num30 += 2.0;
					num28 *= num29;
					num31 = double_4;
					double_4 += num28 / num30;
				}
				while (double_4 != num31);
			}
		}
		double_4 = 2.0 * (double_4 / num18 + num22) / num4;
		if (flag2 && num19 != 0.0)
		{
			double num32 = double_1 / num19;
			double num33 = num32 * num32;
			num32 *= num33;
			double_5 = (3.0 * double_2 * double_4 - 4.0 * (num21 + num20 * QSQFM1) / num19) / num4;
			if (flag3)
			{
				D2T = (3.0 * double_4 + 5.0 * double_2 * double_5 + 4.0 * num32 * QSQFM1) / num4;
			}
			if (flag4)
			{
				D3T = (8.0 * double_5 + 7.0 * double_2 * D2T - 12.0 * num32 * num33 * double_2 * QSQFM1) / num4;
			}
		}
		else
		{
			double_5 = num22;
			D2T = num24;
			D3T = num23;
		}
	}

	public static void SuperiorLambert(double mu, Vector3d r1, Vector3d v1, Vector3d r2, Vector3d v2, double tof, out Vector3d vi, out Vector3d vf)
	{
		try
		{
			double magnitude = r1.magnitude;
			double magnitude2 = r2.magnitude;
			double num = Math.Abs((r2 - r1).magnitude);
			double num2 = magnitude + magnitude2 + num;
			double num3 = magnitude + magnitude2 - num;
			double angleParameter = Math.Sqrt(num3 / num2);
			if (Vector3d.Cross(r1, r2).z < 0.0)
			{
				angleParameter = 0.0 - angleParameter;
			}
			double nTime = 4.0 * tof * Math.Sqrt(mu / (num2 * num2 * num2));
			double num4 = 2.0 / 3.0 * (1.0 - angleParameter * angleParameter * angleParameter);
			Func<double, double> fy = (double xd) => (angleParameter < 0.0) ? (0.0 - Math.Sqrt(1.0 - angleParameter * angleParameter * (1.0 - xd * xd))) : Math.Sqrt(1.0 - angleParameter * angleParameter * (1.0 - xd * xd));
			BisectFunc bisectFunc = delegate(double xx)
			{
				double num14 = fy(xx);
				double num15 = Math.Sqrt(xx * xx - 1.0);
				double num16 = Math.Sqrt(num14 * num14 - 1.0);
				return (Math.PI / 2.0 - Math.Atan(num14 / num16) - (Math.PI / 2.0 - Math.Atan(xx / num15)) + xx * num15 - num14 * num16) / (num15 * num15 * num15) - nTime;
			};
			BisectFunc f = delegate(double xx)
			{
				double num11 = fy(xx);
				double num12 = Math.Sqrt(1.0 - xx * xx);
				double num13 = Math.Sqrt(1.0 - num11 * num11);
				return (Math.PI / 2.0 - Math.Atan(xx / num12) - Math.Atan(num13 / num11) - xx * num12 + num11 * num13) / (num12 * num12 * num12) - nTime;
			};
			double num5;
			double num6;
			if (Math.Abs(nTime - num4) < 1E-06)
			{
				num5 = 1.0;
				num6 = ((!(angleParameter < 0.0)) ? 1 : (-1));
			}
			else if (nTime < num4)
			{
				double a = 1.0;
				double num7 = 2.0;
				while (bisectFunc(num7) > 0.0)
				{
					a = num7;
					num7 *= 2.0;
				}
				num5 = Root(bisectFunc, a, num7, 1E-06);
				num6 = fy(num5);
			}
			else
			{
				double num8 = Math.Acos(angleParameter) + angleParameter * Math.Sqrt(1.0 - angleParameter * angleParameter);
				if (Math.Abs(nTime - num8) < 1E-06)
				{
					num5 = 0.0;
					num6 = fy(num5);
				}
				else
				{
					double a2 = 0.0;
					double b = 1.0;
					if (nTime > num8)
					{
						a2 = -1.0;
						b = 0.0;
					}
					num5 = Root(f, a2, b, 1E-06);
					num6 = fy(num5);
				}
			}
			double num9 = Math.Sqrt(mu) * (num6 * (1.0 / Math.Sqrt(num3)) + num5 * (1.0 / Math.Sqrt(num2)));
			double num10 = Math.Sqrt(mu) * (num6 * (1.0 / Math.Sqrt(num3)) - num5 * (1.0 / Math.Sqrt(num2)));
			Vector3d vector3d = (r2 - r1) * (num9 / num);
			vf = vector3d - r2 * (num10 / magnitude2);
			vi = vector3d + r1 * (num10 / magnitude);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			vi = Vector3d.zero;
			vf = Vector3d.zero;
		}
	}
}
