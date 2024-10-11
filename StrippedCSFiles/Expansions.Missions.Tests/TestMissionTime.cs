using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestMissionTime : TestModule, IScoreableObjective
{
	[MEGUI_Time(resetValue = "300", guiName = "#autoLOC_8000081", Tooltip = "#autoLOC_8000082")]
	public double time;

	private double missionTime;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestMissionTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
