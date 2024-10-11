using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestMissionScore : TestModule, IScoreableObjective
{
	[MEGUI_InputField(canBePinned = false, resetValue = "0", guiName = "#autoLOC_8000050", Tooltip = "#autoLOC_8000051")]
	public float score;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	private static double epsilon;

	private string bodyScore;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestMissionScore()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TestMissionScore()
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
