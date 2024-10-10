using System;
using UnityEngine;

public class Trajectory
{
	public Vector3d[] tPoints;

	public double[] Vs;

	public Vector3d periapsis;

	public Vector3d apoapsis;

	public Vector3d patchStartPoint;

	public Vector3d patchEndPoint;

	public Vector3d refBodyPos;

	public double[] tTimes;

	public CelestialBody referenceBody;

	public Orbit patch;

	public Vector3d[] TPoints
	{
		get
		{
			return tPoints;
		}
		set
		{
			tPoints = value;
		}
	}

	public void UpdateFromOrbit(Orbit orbit, int sampleCount)
	{
		if (tPoints == null || tPoints.Length != sampleCount)
		{
			tPoints = new Vector3d[sampleCount];
		}
		if (Vs == null || Vs.Length != sampleCount)
		{
			Vs = new double[sampleCount];
		}
		if (tTimes == null || tTimes.Length != sampleCount)
		{
			tTimes = new double[sampleCount];
		}
		patch = orbit;
		if (orbit.eccentricity < 1.0)
		{
			if (orbit.patchEndTransition != Orbit.PatchTransitionType.FINAL && !orbit.debug_returnFullEllipseTrajectory)
			{
				orbit.fromE = orbit.EccentricAnomalyAtUT(orbit.StartUT);
				orbit.toE = orbit.EccentricAnomalyAtUT(orbit.EndUT);
				orbit.fromV = orbit.GetTrueAnomaly(orbit.fromE);
				orbit.toV = orbit.GetTrueAnomaly(orbit.toE);
				if (orbit.fromV > orbit.toV)
				{
					orbit.fromE = 0.0 - (Math.PI * 2.0 - orbit.fromE);
					orbit.fromV = 0.0 - (Math.PI * 2.0 - orbit.fromV);
				}
				orbit.sampleInterval = (orbit.toE - orbit.fromE) / (double)(sampleCount - 5);
				orbit.fromE -= orbit.sampleInterval * 2.0;
				for (int i = 0; i < sampleCount; i++)
				{
					orbit.double_1 = orbit.fromE + orbit.sampleInterval * (double)i;
					orbit.double_2 = orbit.GetTrueAnomaly(orbit.double_1);
					Vs[i] = orbit.double_2;
					tTimes[i] = orbit.StartUT + orbit.GetDTforTrueAnomaly(orbit.double_2, double.MaxValue);
					if (orbit.fromE < 0.0 && orbit.double_1 > 0.0 && !double.IsInfinity(orbit.period))
					{
						tTimes[i] += orbit.period;
					}
					tPoints[i] = orbit.getRelativePositionFromEccAnomaly(orbit.double_1).xzy;
				}
			}
			else
			{
				orbit.sampleInterval = Math.PI * 2.0 / (double)sampleCount;
				for (int j = 0; j < sampleCount; j++)
				{
					orbit.double_1 = (double)j * orbit.sampleInterval;
					orbit.double_2 = orbit.GetTrueAnomaly(orbit.double_1);
					Vs[j] = orbit.double_2;
					tTimes[j] = orbit.StartUT + orbit.GetDTforTrueAnomaly(orbit.double_2, double.MaxValue);
					tPoints[j] = orbit.getRelativePositionFromEccAnomaly(orbit.double_1).xzy;
				}
			}
		}
		else
		{
			orbit.fromE = orbit.EccentricAnomalyAtUT(orbit.StartUT);
			orbit.toE = orbit.EccentricAnomalyAtUT(orbit.EndUT);
			orbit.fromV = orbit.GetTrueAnomaly(orbit.fromE);
			orbit.toV = orbit.GetTrueAnomaly(orbit.toE);
			if (double.IsInfinity(orbit.fromE))
			{
				if (orbit.fromE > 0.0)
				{
					orbit.fromV *= 0.95;
				}
				else
				{
					orbit.fromV = 0.95 * (orbit.fromV - Math.PI * 2.0);
				}
				orbit.fromE = orbit.GetEccentricAnomaly(orbit.fromV);
			}
			if (double.IsInfinity(orbit.toE))
			{
				if (orbit.toE > 0.0)
				{
					orbit.toV *= 0.95;
				}
				else
				{
					orbit.toV = 0.95 * (orbit.toV - Math.PI * 2.0);
				}
				orbit.toE = orbit.GetEccentricAnomaly(orbit.toV);
			}
			orbit.sampleInterval = (orbit.toE - orbit.fromE) / (double)(sampleCount - 1);
			for (int k = 0; k < sampleCount; k++)
			{
				orbit.double_1 = orbit.fromE + orbit.sampleInterval * (double)k;
				orbit.double_2 = orbit.GetTrueAnomaly(orbit.double_1);
				tTimes[k] = orbit.StartUT + orbit.GetDTforTrueAnomaly(orbit.double_2, double.MaxValue);
				if (orbit.fromE < 0.0 && orbit.double_1 > 0.0 && !double.IsInfinity(orbit.period))
				{
					tTimes[k] += orbit.period;
				}
				tPoints[k] = orbit.getRelativePositionFromEccAnomaly(orbit.double_1).xzy;
			}
		}
		if (orbit.eccentricity < 1.0)
		{
			periapsis = orbit.getRelativePositionAtT(0.0).xzy;
			apoapsis = orbit.getRelativePositionAtT(orbit.period * 0.5).xzy;
		}
		else
		{
			periapsis = orbit.GetEccVector().xzy.normalized * ((0.0 - orbit.semiMajorAxis) * (orbit.eccentricity - 1.0));
			apoapsis = Vector3d.zero;
		}
		patchStartPoint = orbit.getRelativePositionAtUT(orbit.StartUT).xzy;
		patchEndPoint = orbit.getRelativePositionAtUT(orbit.EndUT).xzy;
		refBodyPos = Vector3d.zero;
		referenceBody = orbit.referenceBody;
	}

	public Trajectory ReframeToLocal()
	{
		Trajectory trajectory = new Trajectory();
		trajectory.patch = patch;
		trajectory.referenceBody = referenceBody;
		GetPointsLocal(trajectory.tPoints);
		trajectory.periapsis = ConvertPointToLocal(periapsis);
		trajectory.apoapsis = ConvertPointToLocal(apoapsis);
		trajectory.patchStartPoint = ConvertPointToLocal(patchStartPoint);
		trajectory.patchEndPoint = ConvertPointToLocal(patchEndPoint);
		trajectory.refBodyPos = ConvertPointToLocal(refBodyPos);
		return trajectory;
	}

	public Trajectory ReframeToRelative(CelestialBody relativeTo)
	{
		Trajectory trajectory = new Trajectory();
		trajectory.patch = patch;
		trajectory.referenceBody = referenceBody;
		GetPointsRelative(trajectory.tPoints, relativeTo);
		trajectory.periapsis = ConvertPointToRelative(periapsis, patch.StartUT + patch.timeToPe, relativeTo);
		trajectory.apoapsis = ConvertPointToRelative(apoapsis, patch.StartUT + patch.timeToAp, relativeTo);
		trajectory.patchStartPoint = ConvertPointToRelative(patchStartPoint, patch.StartUT, relativeTo);
		trajectory.patchEndPoint = ConvertPointToRelative(patchEndPoint, patch.EndUT, relativeTo);
		trajectory.refBodyPos = ConvertPointToRelative(refBodyPos, patch.StartUT + patch.timeToPe, relativeTo);
		return trajectory;
	}

	public Trajectory ReframeToLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		Trajectory trajectory = new Trajectory();
		trajectory.patch = patch;
		trajectory.referenceBody = referenceBody;
		GetPointsLocalAtUT(trajectory.tPoints, relativeTo, atUT);
		trajectory.periapsis = ConvertPointToLocalAtUT(periapsis, atUT, relativeTo);
		trajectory.apoapsis = ConvertPointToLocalAtUT(apoapsis, atUT, relativeTo);
		trajectory.patchStartPoint = ConvertPointToLocalAtUT(patchStartPoint, atUT, relativeTo);
		trajectory.patchEndPoint = ConvertPointToLocalAtUT(patchEndPoint, atUT, relativeTo);
		trajectory.refBodyPos = ConvertPointToLocalAtUT(refBodyPos, atUT, relativeTo);
		return trajectory;
	}

	public Trajectory ReframeToLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		Trajectory trajectory = new Trajectory();
		trajectory.patch = patch;
		trajectory.referenceBody = referenceBody;
		GetPointsLerped(trajectory.tPoints, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		trajectory.periapsis = ConvertPointToLerped(periapsis, patch.StartUT + patch.timeToPe, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		trajectory.apoapsis = ConvertPointToLerped(apoapsis, patch.StartUT + patch.timeToAp, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		trajectory.patchStartPoint = ConvertPointToLerped(patchStartPoint, patch.StartUT, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		trajectory.patchEndPoint = ConvertPointToLerped(patchEndPoint, patch.EndUT, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		trajectory.refBodyPos = ConvertPointToLerped(refBodyPos, patch.StartUT + patch.timeToPe, relativeFrom, relativeTo, minUT, escapeUT, linearity);
		return trajectory;
	}

	public Vector3d ConvertPointToLocal(Vector3d point)
	{
		return point + referenceBody.position;
	}

	public Vector3d ConvertPointToRelative(Vector3d point, double time, CelestialBody relativeTo)
	{
		return point + referenceBody.getTruePositionAtUT(time) - relativeTo.getTruePositionAtUT(time) + relativeTo.position;
	}

	public Vector3d ConvertPointToLocalAtUT(Vector3d point, double atUT, CelestialBody relativeTo)
	{
		return point + referenceBody.getTruePositionAtUT(atUT) - relativeTo.getTruePositionAtUT(atUT) + relativeTo.position;
	}

	public Vector3d ConvertPointToLerped(Vector3d point, double time, CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		Vector3d from = point + relativeFrom.position;
		Vector3d to = point + relativeFrom.getTruePositionAtUT(time) - relativeTo.getTruePositionAtUT(time) + relativeTo.position;
		return Vector3d.Lerp(from, to, Math.Pow(UtilMath.InverseLerp(Math.Max(minUT, Planetarium.GetUniversalTime()), escapeUT, time), linearity));
	}

	public void GetPointsLocal(Vector3d[] rPoints)
	{
		if (referenceBody != null)
		{
			int i = 0;
			for (int num = tPoints.Length; i < num; i++)
			{
				rPoints[i] = ConvertPointToLocal(tPoints[i]);
			}
		}
	}

	public void GetPointsRelative(Vector3d[] rPoints, CelestialBody relativeTo)
	{
		if (referenceBody != null)
		{
			int i = 0;
			for (int num = tPoints.Length; i < num; i++)
			{
				rPoints[i] = ConvertPointToRelative(tPoints[i], tTimes[i], relativeTo);
			}
		}
	}

	public void GetPointsLocalAtUT(Vector3d[] rPoints, CelestialBody relativeTo, double atUT)
	{
		if (referenceBody != null)
		{
			int i = 0;
			for (int num = tPoints.Length; i < num; i++)
			{
				rPoints[i] = ConvertPointToLocalAtUT(tPoints[i], atUT, relativeTo);
			}
		}
	}

	public void GetPointsLerped(Vector3d[] rPoints, CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		if (referenceBody != null)
		{
			int i = 0;
			for (int num = tPoints.Length; i < num; i++)
			{
				rPoints[i] = ConvertPointToLerped(tPoints[i], tTimes[i], relativeFrom, relativeTo, minUT, escapeUT, linearity);
			}
		}
	}

	public Vector3d GetPeriapsisLocal()
	{
		return ConvertPointToLocal(periapsis);
	}

	public Vector3d GetPeriapsisRelative(CelestialBody relativeTo)
	{
		if (patch.eccentricity < 1.0 && patch.patchEndTransition != Orbit.PatchTransitionType.FINAL && patch.StartUT + patch.timeToPe > patch.EndUT)
		{
			patch.timeToPe -= patch.period;
		}
		return ConvertPointToRelative(periapsis, patch.StartUT + patch.timeToPe, relativeTo);
	}

	public Vector3d GetPeriapsisLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		return ConvertPointToLocalAtUT(periapsis, atUT, relativeTo);
	}

	public Vector3d GetPeriapsisLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		if (patch.eccentricity < 1.0 && patch.patchEndTransition != Orbit.PatchTransitionType.FINAL && patch.StartUT + patch.timeToPe > patch.EndUT)
		{
			patch.timeToPe -= patch.period;
		}
		return ConvertPointToLerped(periapsis, patch.StartUT + patch.timeToPe, relativeFrom, relativeTo, minUT, escapeUT, linearity);
	}

	public Vector3d GetApoapsisLocal()
	{
		if (patch.eccentricity >= 1.0)
		{
			return Vector3d.zero;
		}
		return ConvertPointToLocal(apoapsis);
	}

	public Vector3d GetApoapsisRelative(CelestialBody relativeTo)
	{
		if (patch.eccentricity >= 1.0)
		{
			return Vector3d.zero;
		}
		return ConvertPointToRelative(apoapsis, patch.StartUT + patch.timeToAp, relativeTo);
	}

	public Vector3d GetApoapsisLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		if (patch.eccentricity >= 1.0)
		{
			return Vector3d.zero;
		}
		return ConvertPointToLocalAtUT(apoapsis, atUT, relativeTo);
	}

	public Vector3d GetApoapsisLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		if (patch.eccentricity >= 1.0)
		{
			return Vector3d.zero;
		}
		return ConvertPointToLerped(apoapsis, patch.StartUT + patch.timeToAp, relativeFrom, relativeTo, minUT, escapeUT, linearity);
	}

	public Vector3d GetPatchStartLocal()
	{
		return ConvertPointToLocal(patchStartPoint);
	}

	public Vector3d GetPatchStartRelative(CelestialBody relativeTo)
	{
		return ConvertPointToRelative(patchStartPoint, patch.StartUT, relativeTo);
	}

	public Vector3d GetPatchStartLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		return ConvertPointToLocalAtUT(patchStartPoint, atUT, relativeTo);
	}

	public Vector3d GetPatchStartLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		return ConvertPointToLerped(patchStartPoint, patch.StartUT, relativeFrom, relativeTo, minUT, escapeUT, linearity);
	}

	public Vector3d GetPatchEndLocal()
	{
		return ConvertPointToLocal(patchEndPoint);
	}

	public Vector3d GetPatchEndRelative(CelestialBody relativeTo)
	{
		return ConvertPointToRelative(patchEndPoint, patch.EndUT, relativeTo);
	}

	public Vector3d GetPatchEndLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		return ConvertPointToLocalAtUT(patchEndPoint, atUT, relativeTo);
	}

	public Vector3d GetPatchEndLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		return ConvertPointToLerped(patchEndPoint, patch.EndUT, relativeFrom, relativeTo, minUT, escapeUT, linearity);
	}

	public Vector3d GetRefBodyPosLocal()
	{
		return ConvertPointToLocal(refBodyPos);
	}

	public Vector3d GetRefBodyPosRelative(CelestialBody relativeTo)
	{
		if (patch.eccentricity < 1.0 && patch.patchEndTransition != Orbit.PatchTransitionType.FINAL && patch.StartUT + patch.timeToPe > patch.EndUT)
		{
			patch.timeToPe -= patch.period;
		}
		return ConvertPointToRelative(refBodyPos, patch.StartUT + patch.timeToPe, relativeTo);
	}

	public Vector3d GetRefBodyPosLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		return ConvertPointToLocalAtUT(refBodyPos, atUT, relativeTo);
	}

	public Vector3d GetRefBodyPosLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		if (patch.eccentricity < 1.0 && patch.patchEndTransition != Orbit.PatchTransitionType.FINAL && patch.StartUT + patch.timeToPe > patch.EndUT)
		{
			patch.timeToPe -= patch.period;
		}
		return ConvertPointToLerped(refBodyPos, patch.StartUT + patch.timeToPe, relativeFrom, relativeTo, minUT, escapeUT, linearity);
	}

	public Vector3d[] GetPoints()
	{
		return tPoints;
	}

	public double[] GetTimes()
	{
		return tTimes;
	}

	public void GetColors(Color baseColor, Color[] colors)
	{
		int num = tPoints.Length;
		while (num-- > 0)
		{
			if (patch.eccentricity < 1.0 && patch.patchEndTransition != Orbit.PatchTransitionType.FINAL && (num < 2 || num >= tPoints.Length - 2))
			{
				colors[num] = Color.clear;
			}
			else
			{
				colors[num] = baseColor;
			}
		}
	}
}
