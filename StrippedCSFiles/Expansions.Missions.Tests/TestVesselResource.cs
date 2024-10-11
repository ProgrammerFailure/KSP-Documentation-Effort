using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestVesselResource : TestVessel, IScoreableObjective
{
	[MEGUI_Dropdown(order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 30, resetValue = "0", guiName = "#autoLOC_8000088", Tooltip = "#autoLOC_8000089")]
	public double resourceAmount;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	private static double epsilon;

	private double currentResourceAmount;

	private double maxResourceAmount;

	private string resourceDisplay;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TestVesselResource()
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
	private List<MEGUIDropDownItem> SetResourceNames()
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
