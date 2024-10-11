using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public static class OrbitUtilities
{
	private class OrbitGenerationInfo
	{
		public CelestialBody body;

		public OrbitType type;

		public double altitudeFactor;

		public double inclinationFactor;

		public double eccentricityOverride;

		public Orbit orbit;

		public KSPRandom generator;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public OrbitGenerationInfo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public OrbitGenerationInfo(int seed, CelestialBody body, OrbitType type, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public OrbitGenerationInfo(int seed, ref Orbit orbit, OrbitType type, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d PositionOfApoapsis(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d PositionOfPeriapsis(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AngleOfAscendingNode(Orbit currentOrbit, Orbit targetOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AngleOfDescendingNode(Orbit currentOrbit, Orbit targetOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetRelativeInclination(Orbit a, Orbit b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static OrbitType IdentifyOrbit(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit GenerateOrbit(int seed, CelestialBody targetBody, OrbitType orbitType, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ValidateOrbit(int seed, ref Orbit orbit, OrbitType orbitType, double altitudeDifficulty, double inclinationDifficulty, string source = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateApsides(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateInclination(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateLongitudeOfAscendingNode(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateArgumentOfPeriapsis(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateEpoch(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateMeanAnomalyAtEpoch(ref OrbitGenerationInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateFinalization(ref OrbitGenerationInfo info)
	{
		throw null;
	}
}
