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
public class TestVesselStage : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8000125", Tooltip = "#autoLOC_8000126")]
	public int stage;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator = TestComparisonOperator.GreaterThan;

	public string operatorString;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000124");
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			int currentStage = vessel.currentStage;
			return comparisonOperator switch
			{
				TestComparisonOperator.LessThan => currentStage < stage, 
				TestComparisonOperator.LessThanorEqual => currentStage <= stage, 
				TestComparisonOperator.Equal => currentStage == stage, 
				TestComparisonOperator.GreaterThanorEqual => currentStage >= stage, 
				TestComparisonOperator.GreaterThan => currentStage > stage, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004043");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "stage")
		{
			operatorString = "";
			switch (comparisonOperator)
			{
			case TestComparisonOperator.LessThan:
				operatorString = "<";
				break;
			case TestComparisonOperator.LessThanorEqual:
				operatorString = "<=";
				break;
			case TestComparisonOperator.Equal:
				operatorString = "=";
				break;
			case TestComparisonOperator.GreaterThanorEqual:
				operatorString = ">=";
				break;
			case TestComparisonOperator.GreaterThan:
				operatorString = ">";
				break;
			}
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, stage.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("stage", stage);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("stage", ref stage);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.GreaterThan);
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
			Debug.LogErrorFormat("[TestVesselStage] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
