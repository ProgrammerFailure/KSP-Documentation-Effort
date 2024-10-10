using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateLaunchSite : ActionModule
{
	[MEGUI_LaunchSiteSituation(guiName = "#autoLOC_8000067")]
	public LaunchSiteSituation launchSiteSituation;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000066");
	}

	public override void Initialize(MENode node)
	{
		base.Initialize(node);
		launchSiteSituation = new LaunchSiteSituation(node);
	}

	public override IEnumerator Fire()
	{
		if (!node.IsDockedToStartNode)
		{
			Debug.LogWarning("[MissionSystem]: Cannot spawn LaunchSites if they are not docked to the start node.");
			yield return null;
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "launchSiteSituation")
		{
			string empty = string.Empty;
			if (launchSiteSituation == null)
			{
				return empty + Localizer.Format("autoLOC_8006028");
			}
			empty += Localizer.Format("#autoLOC_8000283", launchSiteSituation.launchSiteName);
			return empty + launchSiteSituation.launchSiteGroundLocation.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (!node.IsDockedToStartNode)
		{
			validator.AddNodeFail(node, Localizer.Format("#autoLOC_8006041"));
		}
		if (launchSiteSituation.launchSiteName == "Unnamed LaunchSite")
		{
			validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8006042"));
		}
		List<MENode>.Enumerator listEnumerator = node.mission.nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			if (listEnumerator.Current.IsDockedToStartNode && listEnumerator.Current.IsLaunchPadNode)
			{
				ActionCreateLaunchSite actionCreateLaunchSite = listEnumerator.Current.actionModules[0] as ActionCreateLaunchSite;
				if (actionCreateLaunchSite != null && actionCreateLaunchSite.launchSiteSituation.launchSiteName == launchSiteSituation.launchSiteName && listEnumerator.Current != node)
				{
					validator.AddNodeFail(node, Localizer.Format("#autoLOC_8006043"));
				}
			}
		}
		if (launchSiteSituation.launchSiteGroundLocation.targetBody == FlightGlobals.GetHomeBody() && launchSiteSituation.launchSiteGroundLocation.latitude == 0.0 && launchSiteSituation.launchSiteGroundLocation.longitude == 0.0)
		{
			validator.AddNodeFail(node, Localizer.Format("#autoLOC_8006044"));
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004009");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		launchSiteSituation.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		ConfigNode configNode = node.GetNode("LAUNCHSITESITUATION");
		if (configNode != null)
		{
			launchSiteSituation.Load(configNode);
		}
	}
}
