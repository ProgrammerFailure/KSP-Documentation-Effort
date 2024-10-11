using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint;

public class WaypointManager : MonoBehaviour
{
	private readonly List<Waypoint> waypoints;

	private static Material waypointMaterial;

	public List<Waypoint> Waypoints
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Material WaypointMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WaypointManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static WaypointManager Instance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddWaypoint(Waypoint waypoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveWaypoint(Waypoint waypoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateWaypointNodes(bool mapOpen, bool clicked, CelestialBody focusBody, Vector3d cameraPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateContractWaypoints(bool mapOpen, CelestialBody focus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Waypoint FindWaypoint(Guid id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Distance(double latitude1, double longitude1, double altitude1, double latitude2, double longitude2, double altitude2, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float DistanceToVessel(Waypoint wp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float LateralDistanceToVessel(Waypoint wp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float LateralDistanceToVessel(Waypoint wp, Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Waypoint> WaypointsNearVessel(float distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ChooseRandomPosition(out double latitude, out double longitude, string celestialName, bool waterAllowed = true, bool equatorial = false, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ChooseRandomPositionNear(out double latitude, out double longitude, double centerLatitude, double centerLongitude, string celestialName, double searchRadius, bool waterAllowed = true, System.Random generator = null)
	{
		throw null;
	}
}
