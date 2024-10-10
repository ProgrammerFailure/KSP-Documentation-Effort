using System;
using System.Collections.Generic;
using FinePrint;
using FinePrint.Utilities;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.MISSIONBUILDER
})]
public class ScenarioCustomWaypoints : ScenarioModule
{
	public List<Waypoint> waypoints;

	public static ScenarioCustomWaypoints Instance { get; set; }

	public override void OnAwake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		waypoints = new List<Waypoint>();
	}

	public void OnDestroy()
	{
		if (waypoints != null)
		{
			for (int num = waypoints.Count - 1; num >= 0; num--)
			{
				WaypointManager.RemoveWaypoint(waypoints[num]);
			}
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public static void AddWaypoint(Waypoint waypoint)
	{
		AddWaypoint(waypoint, isMission: false);
	}

	public static void AddWaypoint(Waypoint waypoint, bool isMission)
	{
		if (!(Instance == null))
		{
			ProcessWaypoint(waypoint);
			if (isMission)
			{
				waypoint.isMission = true;
				waypoint.isCustom = false;
			}
			Instance.waypoints.Add(waypoint);
			WaypointManager.AddWaypoint(waypoint);
		}
	}

	public static void RemoveWaypoint(Waypoint waypoint)
	{
		if (!(Instance == null))
		{
			WaypointManager.RemoveWaypoint(waypoint);
			Instance.waypoints.Remove(waypoint);
		}
	}

	public static void ProcessWaypoint(Waypoint waypoint)
	{
		waypoint.seed = waypoint.name.GetHashCode_Net35();
		waypoint.id = "custom";
		waypoint.isCustom = true;
		waypoint.isNavigatable = true;
		waypoint.isOnSurface = true;
		waypoint.landLocked = false;
		waypoint.index = 0;
		waypoint.SetFadeRange();
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		ConfigNode[] nodes = node.GetNodes("WAYPOINT");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ConfigNode configNode = nodes[i];
			Waypoint waypoint = new Waypoint();
			SystemUtilities.LoadNode(configNode, "ScenarioCustomWaypoints", "name", ref waypoint.name, "Custom Waypoint");
			SystemUtilities.LoadNode(configNode, "ScenarioCustomWaypoints", "celestialName", ref waypoint.celestialName, FlightGlobals.GetHomeBodyName());
			SystemUtilities.LoadNode(configNode, "ScenarioCustomWaypoints", "latitude", ref waypoint.latitude, 0.0);
			SystemUtilities.LoadNode(configNode, "ScenarioCustomWaypoints", "longitude", ref waypoint.longitude, 0.0);
			SystemUtilities.LoadNode(configNode, "ScenarioCustomWaypoints", "navigationId", ref waypoint.navigationId, Guid.NewGuid());
			if (waypoint.navigationId == Guid.Empty)
			{
				Debug.LogWarningFormat("Stored navigationId was empty for {0} - Generating new id", waypoint.name);
				waypoint.navigationId = Guid.NewGuid();
			}
			ProcessWaypoint(waypoint);
			GameEvents.onCustomWaypointLoad.Fire(new GameEvents.FromToAction<Waypoint, ConfigNode>(waypoint, configNode));
			waypoints.Add(waypoint);
			WaypointManager.AddWaypoint(waypoint);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		int i = 0;
		for (int count = waypoints.Count; i < count; i++)
		{
			Waypoint waypoint = waypoints[i];
			if (!waypoint.isMission)
			{
				ConfigNode configNode = new ConfigNode("WAYPOINT");
				node.AddNode(configNode);
				configNode.AddValue("name", waypoint.name);
				configNode.AddValue("celestialName", waypoint.celestialName);
				configNode.AddValue("latitude", waypoint.latitude);
				configNode.AddValue("longitude", waypoint.longitude);
				configNode.AddValue("navigationId", waypoint.navigationId);
				GameEvents.onCustomWaypointSave.Fire(new GameEvents.FromToAction<Waypoint, ConfigNode>(waypoint, configNode));
			}
		}
	}
}
