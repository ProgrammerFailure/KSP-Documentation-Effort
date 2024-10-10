using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestMETTime : TestVessel, IScoreableObjective
{
	[MEGUI_Time(resetValue = "300", guiName = "#autoLOC_8000188", Tooltip = "#autoLOC_8000189")]
	public double time = 300.0;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8004046");
		time = 300.0;
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		if (vessel != null)
		{
			return comparisonOperator switch
			{
				TestComparisonLessGreaterOnly.GreaterThan => vessel.missionTime > time, 
				TestComparisonLessGreaterOnly.LessThan => vessel.missionTime < time, 
				_ => false, 
			};
		}
		return false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "time")
		{
			return ((comparisonOperator == TestComparisonLessGreaterOnly.LessThan) ? "< " : "> ") + KSPUtil.PrintDateDeltaCompact(time, includeTime: true, includeSeconds: true);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004036");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("time", time);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("time", ref time);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterOnly.LessThan);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
