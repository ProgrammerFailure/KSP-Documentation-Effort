using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestResourcesRecovery : TestVessel, IScoreableObjective
{
	[MEGUI_Dropdown(order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName;

	[MEGUI_InputField(order = 30, resetValue = "1000", guiName = "#autoLOC_8000170", Tooltip = "#autoLOC_8000171")]
	public double resourcesToRecover;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	private static double epsilon;

	private bool eventFound;

	private double resourcesRecovered;

	private double maxResourceAmount;

	private string operatorString;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestResourcesRecovery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TestResourcesRecovery()
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
	public void Recovered(ProtoVessel pv, MissionRecoveryDialog mrd, float x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CompareResource(double resourcesRecovered, double resourcesToRecover, TestComparisonOperator comparisonOperator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetResourceNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ParameterSetupComplete()
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
