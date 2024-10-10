using System;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestFlyThrough : TestVessel, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_SurfaceVolume(order = 10, gapDisplay = true, guiName = "#autoLOC_8000146", Tooltip = "#autoLOC_8000147")]
	public SurfaceVolume volumeData;

	[MEGUI_Checkbox(onValueChange = "OnShowLabelChanged", order = 20, guiName = "#autoLOC_8003073", Tooltip = "#autoLOC_8003074")]
	public bool showNodeLabel;

	[MEGUI_Checkbox(order = 21, guiName = "#autoLOC_8003394", Tooltip = "#autoLOC_8003395")]
	public bool showMapNode = true;

	public bool passed;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000133");
		volumeData = new SurfaceVolume();
		passed = false;
		useActiveVessel = true;
	}

	public void OnShowLabelChanged(bool value)
	{
		volumeData.showNodeLabel = showNodeLabel;
	}

	public bool HasNodeLabel()
	{
		return volumeData.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return volumeData.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return volumeData.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return volumeData.GetExtraText();
	}

	public bool HasNodeWaypoint()
	{
		return volumeData.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		if (showMapNode)
		{
			Waypoint obj = volumeData.GetWaypoint();
			obj.name = title;
			return obj;
		}
		return null;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			if (volumeData.IsInsideVolume((float)vessel.latitude, (float)vessel.longitude, (float)vessel.altitude))
			{
				passed = true;
			}
			return passed;
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.FieldType == typeof(SurfaceVolume))
		{
			return volumeData.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004034");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		volumeData.Save(node);
		node.AddValue("showNodeLabel", showNodeLabel);
		node.AddValue("showMapNode", showMapNode);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		volumeData.Load(node);
		node.TryGetValue("showNodeLabel", ref showNodeLabel);
		node.TryGetValue("showMapNode", ref showMapNode);
		volumeData.showNodeLabel = showNodeLabel;
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID == 0)
			{
				return FlightGlobals.ActiveVessel;
			}
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				return FlightGlobals.PersistentVesselIds[num];
			}
			Debug.LogErrorFormat("[TestFlyThrough] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
