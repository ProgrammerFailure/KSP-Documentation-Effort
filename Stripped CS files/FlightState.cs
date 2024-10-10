using System;
using System.Collections.Generic;
using CommNet;
using FinePrint.Utilities;
using UnityEngine;

public class FlightState
{
	public const int lastCompatibleMajor = 0;

	public const int lastCompatibleMinor = 18;

	public const int lastCompatibleRev = 0;

	public bool compatible;

	public List<ProtoVessel> protoVessels;

	public Dictionary<string, KSPParseable> sceneStateValues;

	public int file_version_major;

	public int file_version_minor;

	public int file_version_revision;

	public double universalTime;

	public int activeVesselIdx;

	public int mapViewFilterState;

	public FlightState()
	{
		protoVessels = new List<ProtoVessel>();
		sceneStateValues = new Dictionary<string, KSPParseable>();
		file_version_major = Versioning.version_major;
		file_version_minor = Versioning.version_minor;
		file_version_revision = Versioning.Revision;
		compatible = true;
		universalTime = Planetarium.GetUniversalTime();
		mapViewFilterState = MapViewFiltering.GetFilterState();
		List<Vessel> list = new List<Vessel>();
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if (vessel == null)
			{
				continue;
			}
			if (vessel.state == Vessel.State.DEAD)
			{
				Debug.LogWarning("[FlightPersistence]: Vessel " + vessel.GetDisplayName() + " not saved because it was dead.", vessel.gameObject);
				continue;
			}
			if (GameSettings.DECLUTTER_KSC && !vessel.isCommandable && !vessel.isPersistent && vessel.situation == Vessel.Situations.LANDED)
			{
				if (vessel.LandedInKSC)
				{
					vessel.SetAutoClean("it was debris cluttering up KSC");
				}
				if (vessel.LandedInStockLaunchSite)
				{
					vessel.SetAutoClean("it was debris cluttering up a LaunchSite");
				}
			}
			list.Add(vessel);
		}
		if (GameSettings.MAX_VESSELS_BUDGET != -1)
		{
			int count2 = list.Count;
			while (count2-- > 0 && list.Count > GameSettings.MAX_VESSELS_BUDGET)
			{
				if (!list[count2].isPersistent && !list[count2].isCommandable)
				{
					Debug.LogWarning("[Flight Persistence]: Too many vessels in scene - skipping save for " + list[count2].GetDisplayName());
					list.RemoveAt(count2);
				}
			}
		}
		int count3 = list.Count;
		while (count3-- > 0)
		{
			protoVessels.Add(list[count3].BackupVessel());
		}
		activeVesselIdx = (FlightGlobals.ready ? protoVessels.IndexOf(FlightGlobals.ActiveVessel.protoVessel) : 0);
		Debug.Log("Flight State Captured");
	}

	public FlightState(ConfigNode rootNode, Game game)
	{
		protoVessels = new List<ProtoVessel>();
		sceneStateValues = new Dictionary<string, KSPParseable>();
		CompatibilityUtilities.CleanUpUnsanitaryEVAKerbals(rootNode);
		int num = 0;
		while (true)
		{
			if (num < rootNode.values.Count)
			{
				ConfigNode.Value value = rootNode.values[num];
				switch (value.name)
				{
				case "commNetUIModeFlight":
					CommNetUI.ModeFlightMap = (CommNetUI.DisplayMode)Enum.Parse(typeof(CommNetUI.DisplayMode), value.value);
					break;
				case "commNetUIModeTracking":
					CommNetUI.ModeTrackingStation = (CommNetUI.DisplayMode)Enum.Parse(typeof(CommNetUI.DisplayMode), value.value);
					break;
				case "mapViewFiltering":
					mapViewFilterState = int.Parse(value.value);
					break;
				case "activeVessel":
					activeVesselIdx = int.Parse(value.value);
					break;
				case "UT":
					universalTime = double.Parse(value.value);
					break;
				case "version":
				{
					string[] array = value.value.Split('.');
					file_version_major = int.Parse(array[0]);
					file_version_minor = int.Parse(array[1]);
					file_version_revision = int.Parse(array[2]);
					VersionCompareResult versionCompareResult = KSPUtil.CheckVersion(value.value, 0, 18, 0);
					compatible = versionCompareResult == VersionCompareResult.COMPATIBLE;
					if (!compatible)
					{
						return;
					}
					break;
				}
				}
				num++;
				continue;
			}
			for (int i = 0; i < rootNode.nodes.Count; i++)
			{
				ConfigNode configNode = rootNode.nodes[i];
				string name = configNode.name;
				if (name == "VESSEL")
				{
					protoVessels.Add(new ProtoVessel(configNode, game));
				}
			}
			break;
		}
	}

	public void Save(ConfigNode rootNode)
	{
		rootNode.AddValue("version", file_version_major + "." + file_version_minor + "." + file_version_revision);
		rootNode.AddValue("UT", universalTime);
		rootNode.AddValue("activeVessel", activeVesselIdx);
		rootNode.AddValue("mapViewFiltering", mapViewFilterState);
		rootNode.AddValue("commNetUIModeTracking", CommNetUI.ModeTrackingStation);
		rootNode.AddValue("commNetUIModeFlight", CommNetUI.ModeFlightMap);
		for (int i = 0; i < protoVessels.Count; i++)
		{
			protoVessels[i].Save(rootNode.AddNode("VESSEL"));
		}
	}

	public void Load()
	{
		if (compatible)
		{
			for (int i = 0; i < protoVessels.Count; i++)
			{
				protoVessels[i].Load(this);
			}
			Planetarium.SetUniversalTime(universalTime);
			MapViewFiltering.LoadFilterState(mapViewFilterState);
		}
	}

	public bool ContainsFlightID(uint id)
	{
		for (int i = 0; i < protoVessels.Count; i++)
		{
			ProtoVessel protoVessel = protoVessels[i];
			for (int j = 0; j < protoVessel.protoPartSnapshots.Count; j++)
			{
				if (protoVessel.protoPartSnapshots[j].flightID == id)
				{
					return true;
				}
			}
		}
		return false;
	}
}
