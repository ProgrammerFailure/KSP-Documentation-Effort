using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint.Utilities;

public static class CelestialUtilities
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody RandomBody(List<CelestialBody> bodies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody HighestBody(List<CelestialBody> bodies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody LowestBody(List<CelestialBody> bodies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetNeighbors(CelestialBody body, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> ChildrenOf(CelestialBody parentBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddChildren(CelestialBody parent, List<CelestialBody> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetHighestPeak(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetNextTimeWarp(CelestialBody body, double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetMinimumOrbitalDistance(CelestialBody body, float margin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetAltitudeForDensity(CelestialBody body, double density)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vessel.Situations ApplicableSituation(int seed, CelestialBody body, bool splashAllowed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsGasGiant(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsFlyablePlanet(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float PlanetScienceRanking(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 LLAtoECEF(double lat, double lon, double alt, double radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SynchronousSMA(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CanBodyBeSynchronous(CelestialBody body, double eccentricity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double KolniyaSMA(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CanBodyBeKolniya(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CanBodyBeTundra(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double TerrainAltitude(CelestialBody body, double latitude, double longitude, bool underwater = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody MapFocusBody(CelestialBody fallback = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetSolarExtents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GreatCircleDistance(CelestialBody body, double latitude1, double longitude1, double latitude2, double longitude2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GreatCircleDistance(CelestialBody body, Vector3d position1, Vector3d position2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody GetHostPlanet(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetPlanetarySystem(CelestialBody body)
	{
		throw null;
	}
}
