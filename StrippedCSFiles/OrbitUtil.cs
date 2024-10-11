using System.Runtime.CompilerServices;

public class OrbitUtil
{
	private static CelestialBody[] p1Hierarchy;

	private static int p1i;

	private static CelestialBody[] p2Hierarchy;

	private static int p2i;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OrbitUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CurrentPhaseAngle(Orbit origin, Orbit destination)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CurrentEjectionAngle(Orbit origin, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CurrentEjectionAngle(Orbit origin, double UT, Vector3d prograde)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IntersectInformation CalculateIntersections(Orbit referenceOrbit, Orbit targetOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetTransferTime(Orbit o1, Orbit o2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetTransferPhaseAngle(Orbit o1, Orbit o2, double Th)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody FindPlanet(CelestialBody src)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody FindCommonAncestor(Orbit o, CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GetBodyHierarchy(CelestialBody cb, CelestialBody[] hierarchy, ref int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetSmaDistance(Orbit o1, Orbit o2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetSmaDistanceToSun(Orbit obt, double d = 0.0)
	{
		throw null;
	}
}
