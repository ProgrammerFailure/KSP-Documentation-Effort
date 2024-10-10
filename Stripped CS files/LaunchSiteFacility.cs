using System;
using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions;
using ns11;
using ns9;
using PreFlightTests;
using TMPro;
using UnityEngine;

public class LaunchSiteFacility : SpaceCenterBuilding
{
	public string launchSiteName = "LaunchPad";

	public string SCFacilityName = "VAB";

	public string craftSubfolder = "VAB";

	public EditorFacility facilityType = EditorFacility.const_1;

	public string vehicleName = "Rocket";

	public string constructionFacilityName = "Vehicle Assembly Building";

	public GUISkin shipBrowserSkin;

	public Texture2D shipFileImage;

	public LaunchSiteClear launchSiteClearTest;

	public bool awaitingLaunchClear;

	public VesselCrewManifest crewManifest;

	public string fileName;

	public string flagURL;

	public List<MissionRecoveryDialog> missionDialogs;

	public string path;

	public string flag;

	public VesselCrewManifest manifest;

	public override void OnStart()
	{
		missionDialogs = new List<MissionRecoveryDialog>();
		constructionFacilityName = Localizer.Format(constructionFacilityName);
		GameEvents.onGUILaunchScreenDespawn.Add(onGUILaunchScreenDespawn);
	}

	public void onGUILaunchScreenDespawn()
	{
		InputLockManager.RemoveControlLock("launchSiteFacility");
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("launchSiteFacility");
		GameEvents.onGUILaunchScreenDespawn.Remove(onGUILaunchScreenDespawn);
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onMissionDialogUp);
		GameEvents.onGUIRecoveryDialogDespawn.Remove(onMissionDialogDismiss);
	}

	public override bool IsOpen()
	{
		return HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE;
	}

	public override void OnClicked()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			return;
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION && HighLogic.CurrentGame.Parameters.SpaceCenter.CanLaunchAtPad)
		{
			showShipSelection();
		}
		else if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			if (!HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().facilityOpenEditor)
			{
				showFacilityLocked();
				return;
			}
			if (HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsGeneral>().preventVesselRecovery)
			{
				string launchSiteDisplayName = launchSiteName;
				if (!(launchSiteName == "LaunchPad") && !(launchSiteName == "Runway"))
				{
					launchSiteDisplayName = PSystemSetup.Instance.GetLaunchSiteDisplayName(launchSiteName);
				}
				else
				{
					switch (facilityType)
					{
					case EditorFacility.const_2:
						launchSiteDisplayName = ScenarioUpgradeableFacilities.GetFacilityName(SpaceCenterFacility.Runway);
						break;
					case EditorFacility.const_1:
						launchSiteDisplayName = ScenarioUpgradeableFacilities.GetFacilityName(SpaceCenterFacility.LaunchPad);
						break;
					}
				}
				launchSiteClearTest = new LaunchSiteClear(launchSiteName, launchSiteDisplayName);
				if (!launchSiteClearTest.Test())
				{
					DialogGUIButton.TextLabelOptions textLabelOptions = new DialogGUIButton.TextLabelOptions
					{
						enableWordWrapping = true,
						OverflowMode = TextOverflowModes.Ellipsis
					};
					string optionText = Localizer.Format("#autoLOC_350811", launchSiteClearTest.GetObstructingVesselName(), Localizer.Format(launchSiteDisplayName));
					string msg = launchSiteClearTest.GetWarningDescription() + "\n" + Localizer.Format("#autoLOC_8002108");
					PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LaunchSiteWarning", msg, launchSiteClearTest.GetWarningTitle(), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(optionText, ResumeFlightOnSite, 288f, 32f, true)
					{
						textLabelOptions = textLabelOptions
					}, new DialogGUIButton(Localizer.Format("#autoLOC_360725"), Cancel, 288f, 32f, true)), persistAcrossScenes: false, null).OnDismiss = Cancel;
					InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "launchSiteFacility");
					return;
				}
				showShipSelection();
			}
			showShipSelection();
		}
		else
		{
			showFacilityLocked();
		}
	}

	public void showFacilityLocked()
	{
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "launchSiteFacility");
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format(buildingInfoName), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), RemoveInputlock)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = RemoveInputlock;
	}

	public void RemoveInputlock()
	{
		InputLockManager.RemoveControlLock("launchSiteFacility");
	}

	public void ResumeFlightOnSite()
	{
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onMissionDialogUp);
		MonoBehaviour.print("Resuming Flight on " + launchSiteName);
		Cancel();
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		FlightDriver.StartAndFocusVessel("persistent", launchSiteClearTest.GetObstructingVesselIndex());
	}

	public void ClearSite()
	{
		GameEvents.onGUIRecoveryDialogSpawn.Add(onMissionDialogUp);
		foreach (ProtoVessel obstructingVessel in launchSiteClearTest.GetObstructingVessels())
		{
			ShipConstruction.RecoverVesselFromFlight(obstructingVessel, HighLogic.CurrentGame.flightState);
		}
	}

	public void onMissionDialogUp(MissionRecoveryDialog dialog)
	{
		if (missionDialogs.Count == 0)
		{
			GameEvents.onGUIRecoveryDialogDespawn.Add(onMissionDialogDismiss);
		}
		missionDialogs.Add(dialog);
	}

	public void onMissionDialogDismiss(MissionRecoveryDialog dialog)
	{
		missionDialogs.Remove(dialog);
		if (missionDialogs.Count == 0)
		{
			GameEvents.onGUIRecoveryDialogDespawn.Remove(onMissionDialogDismiss);
			if (awaitingLaunchClear)
			{
				launchChecks();
			}
			else
			{
				showShipSelection();
			}
		}
	}

	public void Cancel()
	{
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onMissionDialogUp);
		GameEvents.onGUILaunchScreenDespawn.Fire();
		InputLockManager.RemoveControlLock("launchSiteFacility");
	}

	public void showShipSelection()
	{
		EditorDriver.editorFacility = facilityType;
		EditorDriver.setupValidLaunchSites();
		EditorFacility editorFacility = facilityType;
		if ((uint)editorFacility > 1u && editorFacility == EditorFacility.const_2)
		{
			EditorDriver.setLaunchSite(HighLogic.CurrentGame.defaultSPHLaunchSite);
		}
		else
		{
			EditorDriver.setLaunchSite(HighLogic.CurrentGame.defaultVABLaunchSite);
		}
		launchSiteName = EditorDriver.SelectedLaunchSiteName;
		List<FileInfo> list = new List<FileInfo>();
		if (HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels)
		{
			try
			{
				string text = KSPUtil.ApplicationRootPath + "Ships/" + craftSubfolder;
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				FileInfo[] files = new DirectoryInfo(text).GetFiles("*.craft");
				int num = files.Length;
				while (num-- > 0)
				{
					list.Add(files[num]);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[LaunchSiteFacility]: Failed to load stock craft files:\n" + ex);
			}
			if (ExpansionsLoader.IsAnyExpansionInstalled())
			{
				List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
				string text2 = "";
				for (int i = 0; i < installedExpansions.Count; i++)
				{
					try
					{
						text2 = KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[i].FolderName + "/Ships/" + craftSubfolder;
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						FileInfo[] files2 = new DirectoryInfo(text2).GetFiles("*.craft");
						int num2 = files2.Length;
						while (num2-- > 0)
						{
							list.Add(files2[num2]);
						}
					}
					catch (Exception ex2)
					{
						Debug.LogWarning("[LaunchSiteFacility]: Failed to load stock expansion craft files:\n" + ex2);
					}
				}
			}
		}
		try
		{
			string text3 = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + craftSubfolder;
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			FileInfo[] files3 = new DirectoryInfo(text3).GetFiles("*.craft");
			int num3 = files3.Length;
			while (num3-- > 0)
			{
				list.Add(files3[num3]);
			}
		}
		catch (Exception ex3)
		{
			Debug.LogWarning("[LaunchSiteFacility]: Failed to load player craft files:\n" + ex3);
		}
		if (list.Count > 0)
		{
			GameEvents.onGUILaunchScreenSpawn.Fire(new GameEvents.VesselSpawnInfo(craftSubfolder, HighLogic.SaveFolder, shipSelected, this));
		}
		else
		{
			NothingToLaunch();
		}
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "launchSiteFacility");
	}

	public void NothingToLaunch()
	{
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("NoVesselForLaunch", Localizer.Format("#autoLOC_350955", constructionFacilityName), Localizer.Format("#autoLOC_350956"), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(Localizer.Format("#autoLOC_190768"), Cancel)), persistAcrossScenes: false, null).OnDismiss = Cancel;
	}

	public void shipSelected(string path, string flag, VesselCrewManifest manifest)
	{
		InputLockManager.RemoveControlLock("launchSiteFacility");
		if (path == null)
		{
			return;
		}
		this.path = path;
		this.flag = flag;
		this.manifest = manifest;
		string launchSiteDisplayName = launchSiteName;
		if (!(launchSiteName == "LaunchPad") && !(launchSiteName == "Runway"))
		{
			launchSiteDisplayName = PSystemSetup.Instance.GetLaunchSiteDisplayName(launchSiteName);
		}
		else
		{
			switch (facilityType)
			{
			case EditorFacility.const_2:
				launchSiteDisplayName = ScenarioUpgradeableFacilities.GetFacilityName(SpaceCenterFacility.Runway);
				break;
			case EditorFacility.const_1:
				launchSiteDisplayName = ScenarioUpgradeableFacilities.GetFacilityName(SpaceCenterFacility.LaunchPad);
				break;
			}
		}
		launchSiteClearTest = new LaunchSiteClear(launchSiteName, launchSiteDisplayName);
		if (launchSiteClearTest.Test())
		{
			awaitingLaunchClear = true;
			launchChecks();
			return;
		}
		DialogGUIButton.TextLabelOptions textLabelOptions = new DialogGUIButton.TextLabelOptions
		{
			enableWordWrapping = true,
			OverflowMode = TextOverflowModes.Ellipsis
		};
		string optionText = Localizer.Format("#autoLOC_350811", launchSiteClearTest.GetObstructingVesselName(), Localizer.Format(launchSiteDisplayName));
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LaunchSiteWarning", launchSiteClearTest.GetWarningDescription(), launchSiteClearTest.GetWarningTitle(), UISkinManager.GetSkin("KSP window 7"), new DialogGUIButton(optionText, ResumeFlightOnSite, 288f, 32f, true)
		{
			textLabelOptions = textLabelOptions
		}, new DialogGUIButton(launchSiteClearTest.GetProceedOption(), delegate
		{
			ClearSite();
			awaitingLaunchClear = true;
			if (missionDialogs.Count == 0)
			{
				launchChecks();
			}
		}, 288f, 32f, true)
		{
			textLabelOptions = textLabelOptions
		}, new DialogGUIButton(Localizer.Format("#autoLOC_360725"), Cancel, 288f, 32f, true)), persistAcrossScenes: false, null).OnDismiss = Cancel;
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "launchSiteFacility");
	}

	public void launchChecks()
	{
		if (missionDialogs.Count > 0 || !awaitingLaunchClear)
		{
			return;
		}
		GameEvents.onGUIRecoveryDialogSpawn.Remove(onMissionDialogUp);
		GameEvents.onGUIRecoveryDialogDespawn.Remove(onMissionDialogDismiss);
		awaitingLaunchClear = false;
		fileName = path;
		flagURL = flag;
		crewManifest = manifest;
		ShipTemplate shipTemplate = new ShipTemplate();
		shipTemplate.LoadShip(ConfigNode.Load(path));
		PreFlightCheck preFlightCheck = new PreFlightCheck(launchVessel, ReturnToDialog);
		switch (facilityType)
		{
		case EditorFacility.const_2:
			preFlightCheck.AddTest(new CraftWithinPartCountLimit(shipTemplate, SpaceCenterFacility.SpaceplaneHangar, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.SpaceplaneHangar), isVAB: false)));
			if (launchSiteName == "Runway")
			{
				preFlightCheck.AddTest(new CraftWithinSizeLimits(shipTemplate, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
				preFlightCheck.AddTest(new CraftWithinMassLimits(shipTemplate, SpaceCenterFacility.Runway, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway), isPad: false)));
			}
			break;
		case EditorFacility.const_1:
			preFlightCheck.AddTest(new CraftWithinPartCountLimit(shipTemplate, SpaceCenterFacility.VehicleAssemblyBuilding, GameVariables.Instance.GetPartCountLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.VehicleAssemblyBuilding), isVAB: true)));
			if (launchSiteName == "LaunchPad")
			{
				preFlightCheck.AddTest(new CraftWithinSizeLimits(shipTemplate, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftSizeLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
				preFlightCheck.AddTest(new CraftWithinMassLimits(shipTemplate, SpaceCenterFacility.LaunchPad, GameVariables.Instance.GetCraftMassLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad), isPad: true)));
			}
			break;
		}
		preFlightCheck.AddTest(new ExperimentalPartsAvailable(manifest));
		preFlightCheck.AddTest(new CanAffordLaunchTest(shipTemplate, Funding.Instance));
		if (PSystemSetup.Instance.GetSpaceCenterFacility(launchSiteName) != null)
		{
			preFlightCheck.AddTest(new FacilityOperational(launchSiteName, launchSiteName));
		}
		preFlightCheck.AddTest(new FacilityOperational(SCFacilityName, constructionFacilityName));
		preFlightCheck.AddTest(new NoControlSources(manifest));
		preFlightCheck.AddTest(new WrongVesselTypeForLaunchSite(facilityType, fileName, vehicleName, launchSiteName));
		if (!preFlightCheck.RunTests())
		{
			InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "launchSiteFacility");
		}
	}

	public void ReturnToDialog()
	{
		Cancel();
		showShipSelection();
	}

	public void launchVessel()
	{
		InputLockManager.RemoveControlLock("launchSiteFacility");
		FlightDriver.StartWithNewLaunch(fileName, flagURL, launchSiteName, crewManifest);
	}
}
