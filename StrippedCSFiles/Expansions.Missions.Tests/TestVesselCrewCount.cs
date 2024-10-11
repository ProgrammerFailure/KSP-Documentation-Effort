using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselCrewCount : TestVessel, IScoreableObjective
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 10, resetValue = "0", guiName = "#autoLOC_8000107", Tooltip = "#autoLOC_8000108")]
	public int crewAmount;

	[MEGUI_Dropdown(order = 20, canBePinned = false, resetValue = "LessThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	[MEGUI_Dropdown(order = 30, SetDropDownItems = "SetKerbalRoles", resetValue = "", guiName = "#autoLOC_8000111", Tooltip = "#autoLOC_8000112")]
	public string traitName;

	private int currentCrewCounter;

	private List<ProtoCrewMember> pcmList;

	private string operatorString;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselCrewCount()
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
	public List<MEGUIDropDownItem> SetKerbalRoles()
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
