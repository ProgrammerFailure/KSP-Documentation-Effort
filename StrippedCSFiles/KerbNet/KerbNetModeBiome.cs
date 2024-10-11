using System.Runtime.CompilerServices;
using UnityEngine;

namespace KerbNet;

internal class KerbNetModeBiome : KerbNetMode
{
	private CelestialBody bodyCache;

	private OrbitDriver driverCache;

	private bool sunCache;

	private CBAttributeMapSO biomeCache;

	private static string cacheAutoLOC_438890;

	private static string cacheAutoLOC_258912;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbNetModeBiome()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPrecache(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetCoordinateColor(Vessel vessel, double currentLatitude, double currentLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
