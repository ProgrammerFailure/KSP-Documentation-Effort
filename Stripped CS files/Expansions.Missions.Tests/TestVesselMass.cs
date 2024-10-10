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
public class TestVesselMass : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "25", guiName = "#autoLOC_8000114", Tooltip = "#autoLOC_8000115")]
	public double mass = 25.0;

	public double massCompare;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000113");
		mass = 25.0;
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			massCompare = vessel.GetTotalMass();
			return comparisonOperator switch
			{
				TestComparisonLessGreaterOnly.GreaterThan => massCompare > mass, 
				TestComparisonLessGreaterOnly.LessThan => massCompare < mass, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "mass")
		{
			return Localizer.Format("#autoLOC_8100175", (comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "<" : ">", mass.ToString("0.00"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004042");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("mass", mass);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("mass", ref mass);
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
			Debug.LogErrorFormat("[TestVesselMass] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		return null;
	}
}
