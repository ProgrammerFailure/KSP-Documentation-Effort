using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

[Serializable]
public class VesselLocation : IConfigNode
{
	[MEGUI_Dropdown(addDefaultOption = false, canBePinned = true, guiName = "#autoLOC_8100078")]
	public MissionSituation.VesselStartSituations situation;

	[MEGUI_Dropdown(addDefaultOption = false, canBePinned = true, guiName = "#autoLOC_8100067", Tooltip = "#autoLOC_8100080")]
	public EditorFacility facility;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8100081", Tooltip = "#autoLOC_8100082")]
	public bool brakesOn;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SelectedlaunchSite_SetDropDownValues", canBePinned = true, guiName = "#autoLOC_8000067")]
	public string launchSite;

	[MEGUI_CelestialBody_Orbit(gapDisplay = true, guiName = "#autoLOC_8100084")]
	public MissionOrbit orbitSnapShot;

	[MEGUI_VesselGroundLocation(DisplayVesselGizmoOptions = true, AllowWaterSurfacePlacement = true, gapDisplay = true, guiName = "#autoLOC_8100085")]
	public VesselGroundLocation vesselGroundLocation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLocation(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MissionOrbit createZeroOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SelectedlaunchSite_SetDropDownValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
