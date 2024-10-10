using System.ComponentModel;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class ParamChoices_CelestialBodySurface : IConfigNode
{
	public enum Choices
	{
		[Description("#autoLOC_8000272")]
		anyData,
		[Description("#autoLOC_8000263")]
		bodyData,
		[Description("#autoLOC_8000136")]
		biomeData,
		[Description("#autoLOC_8000264")]
		areaData,
		[Description("#autoLOC_8000067")]
		launchSiteName
	}

	[MEGUI_ParameterSwitchCompound_KeyField(gapDisplay = false, guiName = "#autoLOC_8000265", Tooltip = "#autoLOC_8000266")]
	public Choices locationChoice = Choices.bodyData;

	[MEGUI_CelestialBody(gapDisplay = true, guiName = "#autoLOC_8000263")]
	public MissionCelestialBody bodyData;

	[MEGUI_CelestialBody_Biomes(gapDisplay = true, guiName = "#autoLOC_8000136")]
	public MissionBiome biomeData;

	[MEGUI_SurfaceArea(gapDisplay = true, guiName = "#autoLOC_8000264")]
	public SurfaceArea areaData;

	[MEGUI_LaunchSiteSelect(canBePinned = false, onControlCreated = "OnDistanceToLaunchSiteControlCreated", resetValue = "LaunchPad", guiName = "#autoLOC_8000067")]
	public string launchSiteName = "LaunchPad";

	public bool HasWaypoint()
	{
		Choices choices = locationChoice;
		if (choices == Choices.areaData)
		{
			return areaData.HasWaypoint();
		}
		return false;
	}

	public Waypoint GetWaypoint()
	{
		Choices choices = locationChoice;
		if (choices == Choices.areaData)
		{
			return areaData.GetWaypoint();
		}
		return null;
	}

	public bool HasNodeLabel()
	{
		return HasWorldPosition();
	}

	public bool HasWorldPosition()
	{
		Choices choices = locationChoice;
		if (choices == Choices.areaData)
		{
			return areaData.HasNodeLabel();
		}
		return false;
	}

	public Vector3 GetWorldPosition()
	{
		Choices choices = locationChoice;
		if (choices == Choices.areaData)
		{
			return areaData.GetWorldPosition();
		}
		return default(Vector3);
	}

	public string GetExtraText()
	{
		Choices choices = locationChoice;
		if (choices == Choices.areaData)
		{
			return areaData.GetExtraText();
		}
		return "";
	}

	public string NodeBodyParameterString()
	{
		return locationChoice switch
		{
			Choices.bodyData => bodyData.GetNodeBodyParameterString(), 
			Choices.biomeData => biomeData.GetNodeBodyParameterString(), 
			Choices.areaData => areaData.GetNodeBodyParameterString(), 
			Choices.launchSiteName => Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000067"), PSystemSetup.Instance.GetLaunchSiteDisplayName(launchSiteName)), 
			_ => Localizer.Format("#autoLOC_8000272"), 
		};
	}

	public void Load(ConfigNode node)
	{
		node.TryGetEnum("locationChoice", ref locationChoice, Choices.bodyData);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("BODYDATA", ref node2))
		{
			bodyData.Load(node2);
		}
		if (node.TryGetNode("BIOMEDATA", ref node2))
		{
			biomeData.Load(node2);
		}
		if (node.TryGetNode("GROUNDAREA", ref node2))
		{
			areaData.Load(node2);
		}
		node.TryGetValue("LAUNCHSITEDATA", ref launchSiteName);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("locationChoice", locationChoice);
		if (bodyData != null)
		{
			bodyData.Save(node.AddNode("BODYDATA"));
		}
		if (biomeData != null)
		{
			biomeData.Save(node.AddNode("BIOMEDATA"));
		}
		if (areaData != null)
		{
			areaData.Save(node.AddNode("GROUNDAREA"));
		}
		node.AddValue("LAUNCHSITEDATA", launchSiteName);
	}
}
