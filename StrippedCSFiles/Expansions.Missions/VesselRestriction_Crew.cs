using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions;

public class VesselRestriction_Crew : VesselRestriction
{
	public enum CrewRestrictionType
	{
		[Description("#autoLOC_8005005")]
		TotalCrew,
		[Description("#autoLOC_8005008")]
		Engineers,
		[Description("#autoLOC_8005007")]
		Scientists,
		[Description("#autoLOC_8005006")]
		Pilots
	}

	[MEGUI_Dropdown(canBePinned = false, resetValue = "TotalCrew", canBeReset = true, guiName = "#autoLOC_8100140")]
	protected CrewRestrictionType crewType;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	protected TestComparisonLessGreaterEqual comparisonOperator;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, maxValue = 100f, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100142")]
	protected int targetCrew;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Crew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Crew(MENode node)
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
	private int GetCrewCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetCrewTraitCount(string kerbalTrait)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStateMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_CrewChange(VesselCrewManifest newCrew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}
}
