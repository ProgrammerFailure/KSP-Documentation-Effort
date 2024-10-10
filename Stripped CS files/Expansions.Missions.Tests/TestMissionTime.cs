using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestMissionTime : TestModule, IScoreableObjective
{
	[MEGUI_Time(resetValue = "300", guiName = "#autoLOC_8000081", Tooltip = "#autoLOC_8000082")]
	public double time = 300.0;

	public double missionTime;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator = TestComparisonLessGreaterOnly.GreaterThan;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000080");
		time = 300.0;
	}

	public override bool Test()
	{
		missionTime = base.node.mission.startedUT + time;
		return comparisonOperator switch
		{
			TestComparisonLessGreaterOnly.GreaterThan => Planetarium.GetUniversalTime() > missionTime, 
			TestComparisonLessGreaterOnly.LessThan => Planetarium.GetUniversalTime() < missionTime, 
			_ => false, 
		};
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
		return Localizer.Format("#autoLOC_8004037");
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
