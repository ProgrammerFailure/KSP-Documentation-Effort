using System.Runtime.CompilerServices;
using UnityEngine;

public static class LatLon
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetRelSurfaceNVector(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetSurfaceNVector(Planetarium.CelestialFrame BodyFrame, double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, double Radius, double lat, double lon, double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, double Radius, double lat, double lon, double alt, out Vector3d normal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetWorldSurfacePosition(Planetarium.CelestialFrame BodyFrame, Vector3 bodyPosition, double radius, double lat, double lon, double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetLatitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetLongitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2d GetLatitudeAndLongitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetImpactLatitudeAndLongitude(double Radius, Vector3d position, Vector3d velocity, out double latitude, out double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetAltitude(Vector3d bodyPosition, Vector3d worldPos, double Radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GetLatLongAlt(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, double radius, Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		throw null;
	}
}
