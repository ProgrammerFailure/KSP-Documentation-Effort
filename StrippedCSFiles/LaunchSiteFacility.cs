using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using PreFlightTests;
using UnityEngine;

public class LaunchSiteFacility : SpaceCenterBuilding
{
	public string launchSiteName;

	public string SCFacilityName;

	public string craftSubfolder;

	public EditorFacility facilityType;

	public string vehicleName;

	public string constructionFacilityName;

	public GUISkin shipBrowserSkin;

	public Texture2D shipFileImage;

	private LaunchSiteClear launchSiteClearTest;

	private bool awaitingLaunchClear;

	private VesselCrewManifest crewManifest;

	private string fileName;

	private string flagURL;

	private List<MissionRecoveryDialog> missionDialogs;

	private string path;

	private string flag;

	private VesselCrewManifest manifest;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchSiteFacility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGUILaunchScreenDespawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void showFacilityLocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveInputlock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResumeFlightOnSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionDialogUp(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionDialogDismiss(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Cancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void showShipSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NothingToLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void shipSelected(string path, string flag, VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void launchChecks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReturnToDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void launchVessel()
	{
		throw null;
	}
}
