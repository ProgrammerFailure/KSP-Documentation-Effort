using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestFundsRecovery : TestModule, IScoreableObjective
{
	[MEGUI_InputField(canBePinned = false, resetValue = "1000", guiName = "#autoLOC_8000141", Tooltip = "#autoLOC_8000142")]
	public double fundsToRecover;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	private static double epsilon;

	private bool eventFound;

	private double fundsCollected;

	private string operatorString;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestFundsRecovery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TestFundsRecovery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Cleared()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Recovered(ProtoVessel v, MissionRecoveryDialog mrd, float x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
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
