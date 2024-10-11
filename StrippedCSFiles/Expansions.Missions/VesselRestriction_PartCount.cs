using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions;

public class VesselRestriction_PartCount : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100176")]
	public int targetParts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_PartCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_PartCount(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SuscribeToEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ClearEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetCurrentParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStateMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_PlacePart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}
}
