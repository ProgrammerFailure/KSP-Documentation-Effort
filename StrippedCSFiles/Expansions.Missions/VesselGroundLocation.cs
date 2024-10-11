using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class VesselGroundLocation : IConfigNode
{
	public enum GizmoIcon
	{
		[Description("#autoLOC_8007216")]
		Rocket,
		[Description("#autoLOC_8007217")]
		AirCraft,
		[Description("#autoLOC_8007218")]
		Asteroid,
		[Description("#autoLOC_8007219")]
		Kerbal,
		[Description("#autoLOC_8000067")]
		LaunchSite,
		[Description("#autoLOC_8007221")]
		Flag,
		[Description("#autoLOC_8007221")]
		Comet
	}

	public delegate void OnGizmoIconChangeDelegate(GizmoIcon gizmoIcon);

	public OnGizmoIconChangeDelegate OnGizmoIconChange;

	[MEGUI_Dropdown(onValueChange = "OnVesselIconChange", addDefaultOption = false, order = 1, SetDropDownItems = "SetAvailableGizmoIcons", canBePinned = true, guiName = "#autoLOC_8007215")]
	public GizmoIcon gizmoIcon;

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody targetBody;

	[MEGUI_Checkbox(order = 2, canBePinned = true, guiName = "#autoLOC_8002007", Tooltip = "#autoLOC_8006110")]
	public bool splashed;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F5", group = "Translation", order = 4, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F5", group = "Translation", order = 3, guiName = "#autoLOC_8200026")]
	public double longitude;

	[MEGUI_InputField(group = "Translation", ContentType = MEGUI_Control.InputContentType.Alphanumeric, order = 5, guiName = "#autoLOC_8200028")]
	public double altitude;

	[MEGUI_Quaternion(group = "Angles", order = 6, groupDisplayName = "#autoLOC_8200029", guiName = "#autoLOC_8200029")]
	public MissionQuaternion rotation;

	private double startLatMin;

	private double startLatMax;

	private double startLonMin;

	private double startLonMax;

	public GizmoIcon GAPGizmoIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselGroundLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselGroundLocation(MENode node, GizmoIcon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Waypoint GetWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetWorldPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetExtraText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselIconChange(GizmoIcon newIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetAvailableGizmoIcons()
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
