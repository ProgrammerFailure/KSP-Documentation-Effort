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
public class TestVesselPartCount : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, resetValue = "1", guiName = "#autoLOC_8100177", Tooltip = "#autoLOC_8000307")]
	public int parts = 25;

	public int partsCompare;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator = TestComparisonOperator.Equal;

	public string operatorString;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8100293");
		parts = 1;
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			if (vessel.loaded)
			{
				partsCompare = vessel.Parts.Count;
			}
			else
			{
				partsCompare = vessel.protoVessel.protoPartSnapshots.Count;
			}
			return comparisonOperator switch
			{
				TestComparisonOperator.LessThan => partsCompare < parts, 
				TestComparisonOperator.LessThanorEqual => partsCompare <= parts, 
				TestComparisonOperator.Equal => partsCompare == parts, 
				TestComparisonOperator.GreaterThanorEqual => partsCompare >= parts, 
				TestComparisonOperator.GreaterThan => partsCompare > parts, 
				_ => false, 
			};
		}
		CheckForUnloadedVessel();
		return false;
	}

	public void CheckForUnloadedVessel()
	{
		if (FlightGlobals.VesselsUnloaded.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			if (FlightGlobals.VesselsUnloaded[i].persistentId == vesselID)
			{
				vessel = FlightGlobals.VesselsUnloaded[i];
			}
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "parts")
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
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, parts.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8000305");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("numberOfParts", parts);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("numberOfParts", ref parts);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.Equal);
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
			Debug.LogErrorFormat("[TestVesselPartCount] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
