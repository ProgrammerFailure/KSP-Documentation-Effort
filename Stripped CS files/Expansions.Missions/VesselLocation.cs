using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

[Serializable]
public class VesselLocation : IConfigNode
{
	[MEGUI_Dropdown(addDefaultOption = false, canBePinned = true, guiName = "#autoLOC_8100078")]
	public MissionSituation.VesselStartSituations situation = MissionSituation.VesselStartSituations.PRELAUNCH;

	[MEGUI_Dropdown(addDefaultOption = false, canBePinned = true, guiName = "#autoLOC_8100067", Tooltip = "#autoLOC_8100080")]
	public EditorFacility facility = EditorFacility.const_1;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8100081", Tooltip = "#autoLOC_8100082")]
	public bool brakesOn;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SelectedlaunchSite_SetDropDownValues", canBePinned = true, guiName = "#autoLOC_8000067")]
	public string launchSite = "";

	[MEGUI_CelestialBody_Orbit(gapDisplay = true, guiName = "#autoLOC_8100084")]
	public MissionOrbit orbitSnapShot;

	[MEGUI_VesselGroundLocation(DisplayVesselGizmoOptions = true, AllowWaterSurfacePlacement = true, gapDisplay = true, guiName = "#autoLOC_8100085")]
	public VesselGroundLocation vesselGroundLocation;

	public VesselLocation()
	{
		launchSite = "LaunchPad";
		brakesOn = true;
	}

	public VesselLocation(MENode node)
		: this()
	{
		orbitSnapShot = createZeroOrbit();
		vesselGroundLocation = new VesselGroundLocation(node, VesselGroundLocation.GizmoIcon.Rocket);
		launchSite = "LaunchPad";
		brakesOn = true;
		vesselGroundLocation.splashed = true;
	}

	public MissionOrbit createZeroOrbit()
	{
		return new MissionOrbit(FlightGlobals.GetHomeBody());
	}

	public List<MEGUIDropDownItem> SelectedlaunchSite_SetDropDownValues()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		PSystemSetup.SpaceCenterFacility[] spaceCenterFacilities = PSystemSetup.Instance.SpaceCenterFacilities;
		foreach (PSystemSetup.SpaceCenterFacility spaceCenterFacility in spaceCenterFacilities)
		{
			if (spaceCenterFacility.editorFacility != 0)
			{
				string miniBiomedisplayNameByUnityTag = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(spaceCenterFacility.name, formatted: true);
				list.Add(new MEGUIDropDownItem(spaceCenterFacility.name, spaceCenterFacility.name, string.IsNullOrEmpty(miniBiomedisplayNameByUnityTag) ? spaceCenterFacility.facilityName : miniBiomedisplayNameByUnityTag));
			}
		}
		List<LaunchSite> launchSites = PSystemSetup.Instance.LaunchSites;
		for (int j = 0; j < launchSites.Count; j++)
		{
			LaunchSite launchSite = launchSites[j];
			string launchSiteName = launchSite.launchSiteName;
			list.Add(new MEGUIDropDownItem(launchSite.name, launchSite.name, string.IsNullOrEmpty(launchSiteName) ? launchSite.launchSiteName : launchSiteName));
		}
		return list;
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "brakesOn":
				brakesOn = bool.Parse(value.value);
				break;
			case "facility":
				node.TryGetEnum("facility", ref facility, EditorFacility.None);
				break;
			case "launchSite":
				node.TryGetValue("launchSite", ref launchSite);
				break;
			case "sit":
				node.TryGetEnum("sit", ref situation, MissionSituation.VesselStartSituations.PRELAUNCH);
				break;
			}
		}
		orbitSnapShot = createZeroOrbit();
		vesselGroundLocation = new VesselGroundLocation();
		vesselGroundLocation.targetBody = FlightGlobals.GetHomeBody();
		for (int j = 0; j < node.nodes.Count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			string name = configNode.name;
			if (!(name == "ORBIT"))
			{
				if (name == "GROUNDPOINT")
				{
					vesselGroundLocation.Load(configNode);
				}
			}
			else
			{
				orbitSnapShot = new MissionOrbit(configNode);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("sit", situation);
		node.AddValue("launchSite", launchSite);
		node.AddValue("facility", facility);
		node.AddValue("brakesOn", brakesOn);
		if (vesselGroundLocation != null)
		{
			vesselGroundLocation.Save(node.AddNode("GROUNDPOINT"));
		}
		if (orbitSnapShot == null)
		{
			orbitSnapShot = createZeroOrbit();
		}
		orbitSnapShot.Save(node.AddNode("ORBIT"));
	}
}
