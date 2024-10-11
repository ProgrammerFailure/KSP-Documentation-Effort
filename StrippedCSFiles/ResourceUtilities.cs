using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ResourceUtilities
{
	public const double FLOAT_TOLERANCE = 1E-09;

	public const double SECONDS_PER_TICK = 0.02;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CBAttributeMapSO.MapAttribute GetBiome(double lat, double lon, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T LoadNodeProperties<T>(ConfigNode node) where T : new()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ResourceData> ImportConfigNodeList(ConfigNode[] nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<DepletionData> ImportDepletionNodeList(ConfigNode[] nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<BiomeLockData> ImportBiomeLockNodeList(ConfigNode[] nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<PlanetScanData> ImportPlanetScanNodeList(ConfigNode[] nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetValue(ConfigNode node, string name, double curVal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetMaxDeltaTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetAltitude(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T DeepClone<T>(T obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Deg2Rad(double degrees)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Rad2Lat(double radians)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Rad2Lon(double radians)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color HSL2RGB(double h, double sl, double l, float alpha)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static double clampLat(double lat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static double clampLon(double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static float GetDifficultyLevel()
	{
		throw null;
	}
}
