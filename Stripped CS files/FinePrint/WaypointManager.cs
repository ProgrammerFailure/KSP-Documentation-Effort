using System;
using System.Collections.Generic;
using Contracts;
using FinePrint.Utilities;
using UnityEngine;

namespace FinePrint;

public class WaypointManager : MonoBehaviour
{
	public readonly List<Waypoint> waypoints;

	public static Material waypointMaterial;

	public List<Waypoint> Waypoints => waypoints;

	public static Material WaypointMaterial
	{
		get
		{
			if (waypointMaterial != null)
			{
				return waypointMaterial;
			}
			waypointMaterial = new Material(Shader.Find("Sprites/Default") ?? Shader.Find("Diffuse"));
			return waypointMaterial;
		}
	}

	public WaypointManager()
	{
		waypoints = new List<Waypoint>();
	}

	public static WaypointManager Instance()
	{
		if (!(MapView.MapCamera == null) && !(MapView.MapCamera.gameObject == null))
		{
			return MapView.MapCamera.gameObject.GetComponent<WaypointManager>();
		}
		return null;
	}

	public static void AddWaypoint(Waypoint waypoint)
	{
		if (waypoint == null)
		{
			return;
		}
		WaypointManager waypointManager = Instance();
		if (waypointManager == null)
		{
			waypointManager = MapView.MapCamera.gameObject.AddComponent<WaypointManager>();
			if (waypointManager == null)
			{
				return;
			}
		}
		waypointManager.enabled = true;
		waypointManager.waypoints.Add(waypoint);
		if (waypoint.node == null)
		{
			waypoint.SetupMapNode();
		}
	}

	public static void RemoveWaypoint(Waypoint waypoint)
	{
		if (waypoint == null)
		{
			return;
		}
		WaypointManager waypointManager = Instance();
		if (!(waypointManager == null))
		{
			waypoint.CleanupMapNode();
			waypointManager.waypoints.Remove(waypoint);
			if (waypointManager.waypoints.Count <= 0)
			{
				waypointManager.enabled = false;
			}
		}
	}

	public void OnDestroy()
	{
		int count = waypoints.Count;
		while (count-- > 0)
		{
			RemoveWaypoint(waypoints[count]);
		}
	}

	public void LateUpdate()
	{
		bool mapOpen = (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedScene == GameScenes.TRACKSTATION) && MapView.MapIsEnabled;
		bool clicked = Mouse.Left.GetButtonUp() || (Mouse.Right.GetButtonUp() && !Mouse.Right.WasDragging());
		CelestialBody celestialBody = CelestialUtilities.MapFocusBody(Planetarium.fetch.Sun);
		Vector3d cameraPosition = ((PlanetariumCamera.Camera != null) ? ScaledSpace.ScaledToLocalSpace(PlanetariumCamera.Camera.transform.position) : Vector3d.zero);
		UpdateWaypointNodes(mapOpen, clicked, celestialBody, cameraPosition);
		UpdateContractWaypoints(mapOpen, celestialBody);
	}

	public void UpdateWaypointNodes(bool mapOpen, bool clicked, CelestialBody focusBody, Vector3d cameraPosition)
	{
		if (waypoints == null)
		{
			return;
		}
		int count = waypoints.Count;
		while (count-- > 0)
		{
			if (waypoints[count] != null)
			{
				waypoints[count].UpdateWaypoint(mapOpen, clicked, focusBody, cameraPosition);
			}
		}
	}

	public void UpdateContractWaypoints(bool mapOpen, CelestialBody focus)
	{
		if (!mapOpen || HighLogic.CurrentGame.Mode != Game.Modes.CAREER || ContractSystem.Instance == null)
		{
			return;
		}
		int count = ContractSystem.Instance.Contracts.Count;
		while (count-- > 0)
		{
			if (!(ContractSystem.Instance.Contracts[count] is IUpdateWaypoints))
			{
				continue;
			}
			IEnumerator<ContractParameter> enumerator = ContractSystem.Instance.Contracts[count].AllParameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is WaypointParameter waypointParameter)
				{
					waypointParameter.UpdateWaypoints(focus != null && focus == waypointParameter.TargetBody);
				}
			}
		}
	}

	public static Waypoint FindWaypoint(Guid id)
	{
		WaypointManager waypointManager = Instance();
		if (waypointManager == null)
		{
			return null;
		}
		int count = waypointManager.waypoints.Count;
		Waypoint waypoint;
		do
		{
			if (count-- > 0)
			{
				waypoint = waypointManager.waypoints[count];
				continue;
			}
			return null;
		}
		while (!(waypoint.navigationId == id));
		return waypoint;
	}

	public float Distance(double latitude1, double longitude1, double altitude1, double latitude2, double longitude2, double altitude2, CelestialBody body)
	{
		Vector3d worldSurfacePosition = body.GetWorldSurfacePosition(latitude1, longitude1, altitude1);
		Vector3d worldSurfacePosition2 = body.GetWorldSurfacePosition(latitude2, longitude2, altitude2);
		return (float)Vector3d.Distance(worldSurfacePosition, worldSurfacePosition2);
	}

	public float DistanceToVessel(Waypoint wp)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return float.PositiveInfinity;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel.mainBody.GetName() == wp.celestialName))
		{
			return float.PositiveInfinity;
		}
		return Distance(activeVessel.latitude, activeVessel.longitude, activeVessel.altitude, wp.latitude, wp.longitude, wp.altitude, activeVessel.mainBody);
	}

	public float LateralDistanceToVessel(Waypoint wp)
	{
		return LateralDistanceToVessel(wp, null);
	}

	public float LateralDistanceToVessel(Waypoint wp, Vessel v)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return float.PositiveInfinity;
		}
		if (v == null)
		{
			v = FlightGlobals.ActiveVessel;
		}
		if (!(v.mainBody.GetName() == wp.celestialName))
		{
			return float.PositiveInfinity;
		}
		return Distance(v.latitude, v.longitude, v.altitude, wp.latitude, wp.longitude, v.altitude, v.mainBody);
	}

	public List<Waypoint> WaypointsNearVessel(float distance)
	{
		List<Waypoint> list = new List<Waypoint>();
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return list;
		}
		int count = waypoints.Count;
		while (count-- > 0)
		{
			if (DistanceToVessel(waypoints[count]) < distance)
			{
				list.Add(waypoints[count]);
			}
		}
		return list;
	}

	public static void ChooseRandomPosition(out double latitude, out double longitude, string celestialName, bool waterAllowed = true, bool equatorial = false, System.Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		latitude = 0.0;
		longitude = 0.0;
		CelestialBody celestialBody = null;
		int count = FlightGlobals.Bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody2 = FlightGlobals.Bodies[count];
			if (celestialBody2.GetName() == celestialName)
			{
				celestialBody = celestialBody2;
			}
		}
		if (celestialBody == null)
		{
			return;
		}
		if (!celestialBody.hasSolidSurface)
		{
			waterAllowed = true;
		}
		if (celestialBody.ocean && !waterAllowed)
		{
			int num = 10000;
			while (num >= 0)
			{
				if (!equatorial)
				{
					double num2 = kSPRandom?.NextDouble() ?? generator.NextDouble();
					num2 = 1.0 - num2 * 2.0;
					latitude = Math.Asin(num2) * 57.295780181884766;
				}
				else
				{
					latitude = 0.0;
				}
				longitude = kSPRandom?.NextDouble() ?? (generator.NextDouble() * 360.0 - 180.0);
				if (!(CelestialUtilities.TerrainAltitude(celestialBody, latitude, longitude, underwater: true) > 0.0))
				{
					num--;
					continue;
				}
				break;
			}
		}
		else
		{
			if (!equatorial)
			{
				double num3 = kSPRandom?.NextDouble() ?? generator.NextDouble();
				num3 = 1.0 - num3 * 2.0;
				latitude = Math.Asin(num3) * 57.295780181884766;
			}
			else
			{
				latitude = 0.0;
			}
			longitude = kSPRandom?.NextDouble() ?? (generator.NextDouble() * 360.0 - 180.0);
		}
	}

	public static void ChooseRandomPositionNear(out double latitude, out double longitude, double centerLatitude, double centerLongitude, string celestialName, double searchRadius, bool waterAllowed = true, System.Random generator = null)
	{
		latitude = 0.0;
		longitude = 0.0;
		CelestialBody celestialBody = null;
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		int count = FlightGlobals.Bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody2 = FlightGlobals.Bodies[count];
			if (celestialBody2.GetName() == celestialName)
			{
				celestialBody = celestialBody2;
			}
		}
		if (celestialBody == null)
		{
			return;
		}
		if (!celestialBody.hasSolidSurface)
		{
			waterAllowed = true;
		}
		if (celestialBody.ocean && !waterAllowed)
		{
			int num = 10000;
			while (num >= 0)
			{
				double num2 = celestialBody.Radius * 2.0 * 3.1415927410125732 / 360.0;
				double num3 = centerLongitude - searchRadius / (double)Mathf.Abs(Mathf.Cos((float)Math.PI / 180f * (float)centerLatitude) * (float)num2);
				double num4 = centerLongitude + searchRadius / (double)Mathf.Abs(Mathf.Cos((float)Math.PI / 180f * (float)centerLatitude) * (float)num2);
				double num5 = centerLatitude - searchRadius / num2;
				double num6 = centerLatitude + searchRadius / num2;
				latitude = num5 + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (num6 - num5);
				longitude = num3 + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (num4 - num3);
				if (!(CelestialUtilities.TerrainAltitude(celestialBody, latitude, longitude, underwater: true) > 0.0))
				{
					searchRadius *= 1.05;
					num--;
					continue;
				}
				break;
			}
		}
		else
		{
			double num7 = celestialBody.Radius * 2.0 * 3.1415927410125732 / 360.0;
			double num8 = centerLongitude - searchRadius / (double)Mathf.Abs(Mathf.Cos((float)Math.PI / 180f * (float)centerLatitude) * (float)num7);
			double num9 = centerLongitude + searchRadius / (double)Mathf.Abs(Mathf.Cos((float)Math.PI / 180f * (float)centerLatitude) * (float)num7);
			double num10 = centerLatitude - searchRadius / num7;
			double num11 = centerLatitude + searchRadius / num7;
			latitude = num10 + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (num11 - num10);
			longitude = num8 + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (num9 - num8);
		}
	}
}
