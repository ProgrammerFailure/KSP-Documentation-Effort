using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class SurfaceVolume : IConfigNode
{
	public enum VolumeShape
	{
		[Description("#autoLOC_8100034")]
		Cone,
		[Description("#autoLOC_8100031")]
		Sphere
	}

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 90f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -90f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 2, guiName = "#autoLOC_8200027")]
	public double latitude;

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8200025", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = -180f, resetValue = "0.0", displayFormat = "F3", group = "Translation", order = 1, guiName = "#autoLOC_8200026")]
	public double longitude;

	public double altitude;

	[MEGUI_Dropdown(group = "Shape", addDefaultOption = false, order = 3, groupDisplayName = "#autoLOC_8200033", guiName = "#autoLOC_8200034")]
	public VolumeShape shape;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 10f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 4, guiName = "#autoLOC_8200035")]
	public float radius;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 5, guiName = "#autoLOC_8200038")]
	public float heightSphere;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 6, guiName = "#autoLOC_8200036")]
	public float heightConeMin;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 600000f, clampTextInput = true, roundToPlaces = 6, displayUnits = "#autoLOC_289929", minValue = 0f, resetValue = "0.0", displayFormat = "F3", group = "Shape", order = 7, guiName = "#autoLOC_8200037")]
	public float heightConeMax;

	public bool showNodeLabel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceVolume(CelestialBody targetBody, double longitude, double latitude, float radius, VolumeShape shape, float heightSphere, float heightConeMin, float heightConeMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetBodyToDefaultValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AjustToBody(float radius, float heightSphere, float heigthConeMin, float heightConeMax, CelestialBody newBody, CelestialBody pastBody)
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
	public string GetNodeBodyParameterString()
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
	public bool IsInsideVolume(float targetLatitude, float targetLongitude, float targetAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		throw null;
	}
}
