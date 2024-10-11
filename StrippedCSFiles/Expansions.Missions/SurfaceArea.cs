using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class SurfaceArea : IConfigNode
{
	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 2, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 1, guiName = "#autoLOC_8200026")]
	public double longitude;

	public double altitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200040", canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "m", minValue = 100f, resetValue = "0.0", displayFormat = "F3", group = "Area", order = 3, guiName = "#autoLOC_8200035")]
	public float radius;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceArea(CelestialBody targetBody, double longitude, double latitude, float radius)
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
	public bool HasWorldPosition()
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
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsPointInCircle(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double PointInCircleAccuracy(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		throw null;
	}
}
