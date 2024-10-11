using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions;

public class ParamChoices_CelestialBodySurface : IConfigNode
{
	public enum Choices
	{
		[Description("#autoLOC_8000272")]
		anyData,
		[Description("#autoLOC_8000263")]
		bodyData,
		[Description("#autoLOC_8000136")]
		biomeData,
		[Description("#autoLOC_8000264")]
		areaData,
		[Description("#autoLOC_8000067")]
		launchSiteName
	}

	[MEGUI_ParameterSwitchCompound_KeyField(gapDisplay = false, guiName = "#autoLOC_8000265", Tooltip = "#autoLOC_8000266")]
	public Choices locationChoice;

	[MEGUI_CelestialBody(gapDisplay = true, guiName = "#autoLOC_8000263")]
	public MissionCelestialBody bodyData;

	[MEGUI_CelestialBody_Biomes(gapDisplay = true, guiName = "#autoLOC_8000136")]
	public MissionBiome biomeData;

	[MEGUI_SurfaceArea(gapDisplay = true, guiName = "#autoLOC_8000264")]
	public SurfaceArea areaData;

	[MEGUI_LaunchSiteSelect(canBePinned = false, onControlCreated = "OnDistanceToLaunchSiteControlCreated", resetValue = "LaunchPad", guiName = "#autoLOC_8000067")]
	public string launchSiteName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ParamChoices_CelestialBodySurface()
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
	public string NodeBodyParameterString()
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
