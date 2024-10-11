using System;
using System.Runtime.CompilerServices;

namespace SentinelMission;

public static class SentinelUtilities
{
	private static string _SentinelPartName;

	private static string _SentinelPartDisplayName;

	private static string _SentinelModuleName;

	private static string _SentinelModuleDisplayName;

	private static double _SentinelViewAngle;

	private static float _SpawnChance;

	private static int _WeightedStability;

	public static double MinAsteroidEccentricity;

	public static double MaxAsteroidEccentricity;

	public static double MinAsteroidInclination;

	public static double MaxAsteroidInclination;

	public static int perSentinelObjectLimit;

	private static string sentinelPartTitle;

	public static string SentinelPartName
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

	public static string SentinelPartDisplayName
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

	public static string SentinelModuleName
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

	public static string SentinelModuleDisplayName
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

	public static double SentinelViewAngle
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

	public static float SpawnChance
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

	public static int WeightedStability
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

	public static string SentinelPartTitle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SentinelUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SentinelCanScan(Vessel v, CelestialBody innerBody = null, CelestialBody outerBody = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindInnerAndOuterBodies(double SMA, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindInnerAndOuterBodies(Orbit o, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindInnerAndOuterBodies(Vessel v, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AdjustedSentinelViewAngle(Orbit innerOrbit, Orbit outerOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetEscapeVelocity(CelestialBody body, double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetMinimumOrbitalSpeed(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetProgradeBurnAllowance(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetRetrogradeBurnAllowance(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UntrackedObjectClass GetVesselClass(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SentinelScanType RandomScanType(Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UntrackedObjectClass WeightedAsteroidClass(Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WeightedRandom(Random generator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WeightedRandom(Random generator, double min, double max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WeightedRandom(Random generator, double max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double RandomRange(Random generator = null, double min = double.MinValue, double max = double.MaxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float CalculateReadDuration(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsOnSolarOrbit(CelestialBody body)
	{
		throw null;
	}
}
