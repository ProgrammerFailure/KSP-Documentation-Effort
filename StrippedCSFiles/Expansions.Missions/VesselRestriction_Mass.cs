using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using KSP.UI.Screens;

namespace Expansions.Missions;

public class VesselRestriction_Mass : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100168")]
	public float targetMass;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Mass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction_Mass(MENode node)
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
	public float GetCurrentMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStateMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_ShipModified(ShipConstruct ship)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Callback_CrewModified(VesselCrewManifest vcm)
	{
		throw null;
	}
}
