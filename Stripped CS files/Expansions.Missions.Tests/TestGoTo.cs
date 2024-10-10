using System;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestGoTo : TestVessel, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_SurfaceArea(order = 10, gapDisplay = true, guiName = "#autoLOC_8000264", Tooltip = "#autoLOC_8000147")]
	public SurfaceArea areaData;

	[MEGUI_NumberRange(onControlCreated = "OnSurfaceVelocityControlCreated", maxValue = 10f, roundToPlaces = 1, displayUnits = "#autoLOC_180095", minValue = 0f, resetValue = "0.1", displayFormat = "0.0", order = 30, guiName = "#autoLOC_8003020")]
	public double surfaceVelocity = 0.10000000149011612;

	[MEGUI_Checkbox(onValueChange = "OnIgnoreSurfaceVelocity", order = 20, canBePinned = false, guiName = "#autoLOC_8003050", Tooltip = "#autoLOC_8003051")]
	public bool ignoreSurfaceVelocity;

	public MEGUIParameterNumberRange surfaceVelocityRange;

	public bool speedSuccess;

	public bool passed;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000145");
		areaData = new SurfaceArea(FlightGlobals.GetHomeBody(), 0.0, 0.0, 150000f);
		passed = false;
		useActiveVessel = true;
	}

	public bool HasNodeWaypoint()
	{
		return areaData.HasWaypoint();
	}

	public Waypoint GetNodeWaypoint()
	{
		Waypoint obj = areaData.GetWaypoint();
		obj.name = title;
		return obj;
	}

	public override bool Test()
	{
		base.Test();
		speedSuccess = ignoreSurfaceVelocity || (vessel != null && vessel.srfSpeed <= surfaceVelocity + 1.401298464324817E-45);
		if (!speedSuccess)
		{
			return false;
		}
		if (vessel != null && (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.SPLASHED))
		{
			if (areaData.IsPointInCircle((float)vessel.latitude, (float)vessel.longitude))
			{
				passed = true;
			}
			return passed;
		}
		return false;
	}

	public void OnSurfaceVelocityControlCreated(MEGUIParameterNumberRange parameter)
	{
		surfaceVelocityRange = parameter;
		Invoke("OnSurfaceVelocitySetState", 0.1f);
	}

	public void OnSurfaceVelocitySetState()
	{
		surfaceVelocityRange.gameObject.SetActive(!ignoreSurfaceVelocity);
	}

	public void OnIgnoreSurfaceVelocity(bool value)
	{
		surfaceVelocityRange.gameObject.SetActive(!value);
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.FieldType == typeof(SurfaceArea))
		{
			return areaData.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public bool HasNodeLabel()
	{
		return areaData.HasNodeLabel();
	}

	public bool HasWorldPosition()
	{
		return areaData.HasWorldPosition();
	}

	public Vector3 GetWorldPosition()
	{
		return areaData.GetWorldPosition();
	}

	public string GetExtraText()
	{
		return areaData.GetExtraText();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004035");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		areaData.Save(node);
		node.AddValue("surfaceVelocity", surfaceVelocity);
		node.AddValue("ignoreSurfaceVelocity", ignoreSurfaceVelocity);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		areaData.Load(node);
		node.TryGetValue("surfaceVelocity", ref surfaceVelocity);
		node.TryGetValue("ignoreSurfaceVelocity", ref ignoreSurfaceVelocity);
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
			Debug.LogErrorFormat("[TestGoTo] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		if (scoreModule == typeof(ScoreModule_Accuracy))
		{
			return areaData.PointInCircleAccuracy(vessel.latitude, vessel.longitude);
		}
		return null;
	}
}
