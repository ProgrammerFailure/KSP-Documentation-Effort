using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

public class TestAccuracy : TestModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.Percentage, resetValue = "0", guiName = "#autoLOC_8001010", Tooltip = "#autoLOC_8001011")]
	public double accuracy;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator;

	protected IScoreableObjective scoreableObjective;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestAccuracy()
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
}
