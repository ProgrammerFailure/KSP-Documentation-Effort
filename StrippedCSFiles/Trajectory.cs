using System.Runtime.CompilerServices;
using UnityEngine;

public class Trajectory
{
	private Vector3d[] tPoints;

	public double[] Vs;

	private Vector3d periapsis;

	private Vector3d apoapsis;

	private Vector3d patchStartPoint;

	private Vector3d patchEndPoint;

	private Vector3d refBodyPos;

	private double[] tTimes;

	public CelestialBody referenceBody;

	public Orbit patch;

	public Vector3d[] TPoints
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Trajectory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromOrbit(Orbit orbit, int sampleCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Trajectory ReframeToLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Trajectory ReframeToRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Trajectory ReframeToLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Trajectory ReframeToLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d ConvertPointToLocal(Vector3d point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d ConvertPointToRelative(Vector3d point, double time, CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d ConvertPointToLocalAtUT(Vector3d point, double atUT, CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d ConvertPointToLerped(Vector3d point, double time, CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPointsLocal(Vector3d[] rPoints)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPointsRelative(Vector3d[] rPoints, CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPointsLocalAtUT(Vector3d[] rPoints, CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPointsLerped(Vector3d[] rPoints, CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPeriapsisLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPeriapsisRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPeriapsisLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPeriapsisLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetApoapsisLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetApoapsisRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetApoapsisLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetApoapsisLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchStartLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchStartRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchStartLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchStartLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchEndLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchEndRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchEndLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPatchEndLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRefBodyPosLocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRefBodyPosRelative(CelestialBody relativeTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRefBodyPosLocalAtUT(CelestialBody relativeTo, double atUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRefBodyPosLerped(CelestialBody relativeFrom, CelestialBody relativeTo, double minUT, double escapeUT, double linearity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d[] GetPoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double[] GetTimes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetColors(Color baseColor, Color[] colors)
	{
		throw null;
	}
}
