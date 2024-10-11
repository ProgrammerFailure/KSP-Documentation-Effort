using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

public class TestVesselActiveCount : TestModule
{
	public enum VesselType
	{
		[Description("#autoLOC_900712")]
		All,
		[Description("#autoLOC_8001049")]
		PlayerControlled,
		[Description("#autoLOC_308682")]
		Asteroids,
		[Description("#autoLOC_6006050")]
		Comets,
		[Description("#autoLOC_901070")]
		Unowned
	}

	[MEGUI_Dropdown(canBePinned = false, resetValue = "All", guiName = "#autoLOC_8001042", Tooltip = "#autoLOC_8001043")]
	public VesselType vesselType;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, resetValue = "0", guiName = "#autoLOC_8001044", Tooltip = "#autoLOC_8001045")]
	public int vesselCount;

	protected int countCompare;

	private string operatorString;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselActiveCount()
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
	private void OnAddVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVesselCount(Vessel v, int value)
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
