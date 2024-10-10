using System;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselAltitude : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8000093", Tooltip = "#autoLOC_8000094")]
	public double altitude;

	public double altitudeCompare;

	[MEGUI_Checkbox(resetValue = "false", guiName = "#autoLOC_8000095", Tooltip = "#autoLOC_8000096")]
	public bool useRadarAltimiter;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000092");
		altitude = 0.0;
		useRadarAltimiter = false;
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			if (useRadarAltimiter)
			{
				altitudeCompare = vessel.radarAltitude;
			}
			else
			{
				altitudeCompare = vessel.altitude;
			}
			return comparisonOperator switch
			{
				TestComparisonLessGreaterOnly.GreaterThan => altitudeCompare > altitude, 
				TestComparisonLessGreaterOnly.LessThan => altitudeCompare < altitude, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "altitude")
		{
			return Localizer.Format("#autoLOC_8100035", (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", altitude.ToString());
		}
		if (field.name == "useRadarAltimiter")
		{
			if (!useRadarAltimiter)
			{
				return Localizer.Format("#autoLOC_8100167");
			}
			return Localizer.Format("#autoLOC_8100036");
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004040");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("altitude", altitude);
		node.AddValue("useRadarAltimiter", useRadarAltimiter);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("altitude", ref altitude);
		node.TryGetValue("useRadarAltimiter", ref useRadarAltimiter);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.GreaterThan);
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
			Debug.LogErrorFormat("[TestVesselAltitude] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
