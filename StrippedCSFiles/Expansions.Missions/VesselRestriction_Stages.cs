using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class VesselRestriction_Stages : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, maxValue = 20f, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100203")]
	public int targetStages;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Stages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Stages(MENode node)
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
	private int GetCurrentStageCount()
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
	private void Callback_AddStage(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_ModifyStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_RemoveStage(int index)
	{
		throw null;
	}
}
