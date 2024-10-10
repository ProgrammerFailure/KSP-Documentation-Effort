using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using ns11;
using ns2;
using ns25;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public bool display;

	public string guiSkinName = "MainMenuSkin";

	public string guiMiniSkinName = "MainMenuSkin";

	public UISkinDef guiSkin;

	public Color backgroundColor;

	public static PauseMenu fetch;

	public MiniSettings miniSettings;

	public MiniKeyBindings miniKeyBindings;

	public DialogGUIHorizontalLayout dialogObj;

	public Rect dialogRect;

	public static ClearToSaveStatus canSaveAndExit;

	public PopupDialog confirmOptionDialog;

	public GameScenes sceneToLeaveTo;

	public PopupDialog dialog;

	public bool loadBrowserShown;

	public bool showExitToMainConfirmation;

	public PopupDialog dialogExitToMainConfirmation;

	public bool exitToMainConfirmationShown;

	public static bool isOpen => fetch.display;

	public static bool exists => fetch != null;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void Start()
	{
		dialogRect = new Rect(0.5f, 0.5f, 350f, 130f);
		guiSkin = UISkinManager.GetSkin(guiSkinName);
		showExitToMainConfirmation = GameSettings.SHOW_EXIT_TO_MENU_CONFIRMATION;
	}

	public void OnDestroy()
	{
		fetch.display = false;
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public static void Display()
	{
		FlightDriver.SetPause(pauseState: true);
		fetch.display = true;
		canSaveAndExit = FlightGlobals.ClearToSave();
		if (fetch.dialog != null)
		{
			fetch.dialog.gameObject.SetActive(value: true);
			return;
		}
		fetch.dialog = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("GamePaused", string.Empty, Localizer.Format("#autoLOC_360479"), fetch.guiSkin, fetch.dialogRect, new DialogGUIVerticalLayout(sw: true, sh: false, 2f, new RectOffset(), TextAnchor.UpperLeft, fetch.draw())), persistAcrossScenes: false, fetch.guiSkin);
		if (fetch.dialogObj.uiItem != null)
		{
			fetch.BuildButtonList(fetch.dialogObj.uiItem);
		}
	}

	public static void Close()
	{
		fetch.display = false;
		if (fetch.confirmOptionDialog != null)
		{
			fetch.confirmOptionDialog.Dismiss();
		}
		if (fetch.miniSettings != null)
		{
			fetch.miniSettings.Dismiss();
		}
		if (fetch.miniKeyBindings != null)
		{
			fetch.miniKeyBindings.Dismiss();
		}
		if (fetch.dialog != null)
		{
			fetch.dialog.Dismiss();
			fetch.dialog = null;
		}
		FlightDriver.SetPause(pauseState: false);
	}

	public void Update()
	{
		if (!GameSettings.PAUSE.GetKeyUp() || UIMasterController.Instance.CameraMode || FlightGlobals.PartPlacementMode)
		{
			return;
		}
		if (!FlightDriver.Pause)
		{
			if (InputLockManager.IsUnlocked(ControlTypes.PAUSE))
			{
				if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.HasFinishedMission())
				{
					MissionSystem.DisplayMissionEndMessage();
				}
				else if (ActionGroupsFlightController.Instance != null && !ActionGroupsFlightController.Instance.IsOpen && !ActionGroupsPanel.actionGroupPanelOpen)
				{
					Display();
				}
			}
		}
		else if (display)
		{
			if (exitToMainConfirmationShown)
			{
				CancelExitToMainMenuConfirmation();
				dialogExitToMainConfirmation.Dismiss();
			}
			else if (miniSettings == null && miniKeyBindings == null && confirmOptionDialog == null && !loadBrowserShown)
			{
				Close();
			}
		}
	}

	public DialogGUIBase[] draw()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.missions != null && MissionSystem.missions.Count > 0)
		{
			list.Add(new DialogGUIBox("<size=12><i>" + Localizer.Format(MissionSystem.missions[0].title) + " ( " + Localizer.Format("#autoLOC_8005013").ToUpper() + " )</i></size>", -1f, 24f, null));
		}
		else if (HighLogic.CurrentGame.Title.LastIndexOf("(") >= 0)
		{
			list.Add(new DialogGUIBox("<size=12><i>" + Localizer.Format(HighLogic.CurrentGame.Title.Substring(0, HighLogic.CurrentGame.Title.LastIndexOf("(") - 1)) + " " + HighLogic.CurrentGame.Title.Substring(HighLogic.CurrentGame.Title.LastIndexOf("(")) + "</i></size>", -1f, 24f, null));
		}
		else
		{
			list.Add(new DialogGUIBox("<size=12><i>" + Localizer.Format(HighLogic.CurrentGame.Title) + "</i></size>", -1f, 24f, null));
		}
		list.Add(new DialogGUISpace(4f));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout();
		dialogGUIVerticalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360539"), delegate
		{
			Close();
		}, 167f, 32f, true));
		if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor || HighLogic.CurrentGame.Parameters.Flight.CanRestart || (MissionModeValid() && MissionSystem.IsTestMode))
		{
			dialogGUIVerticalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360545"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				if (MissionModeValid())
				{
					confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RevertingFlight", null, Localizer.Format("#autoLOC_360548"), HighLogic.UISkin, 350f, drawRevertOptions()), persistAcrossScenes: false, HighLogic.UISkin);
					confirmOptionDialog.OnDismiss = delegate
					{
						dialog.gameObject.SetActive(value: true);
					};
				}
				else
				{
					confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RevertingFlight", null, Localizer.Format("#autoLOC_360548"), HighLogic.UISkin, 450f, drawRevertOptions()), persistAcrossScenes: false, HighLogic.UISkin);
					confirmOptionDialog.OnDismiss = delegate
					{
						dialog.gameObject.SetActive(value: true);
					};
				}
				MenuNavigation.SpawnMenuNavigation(confirmOptionDialog.gameObject, Navigation.Mode.Vertical, limitCheck: true);
			}, delegate
			{
				if (MissionModeValid())
				{
					string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs";
					string path2 = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs";
					if (!File.Exists(path))
					{
						return File.Exists(path2);
					}
					return true;
				}
				return FlightDriver.CanRevert;
			}, 167f, 32f, dismissOnSelect: false));
		}
		if (MissionModeValid() && MissionSystem.missions != null && MissionSystem.missions.Count > 0)
		{
			dialogGUIVerticalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_8006013"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RestartMission", null, Localizer.Format("#autoLOC_8006013"), HighLogic.UISkin, 350f, drawMissionRestartOptions(dialog)), persistAcrossScenes: false, HighLogic.UISkin);
				confirmOptionDialog.OnDismiss = delegate
				{
					dialog.gameObject.SetActive(value: true);
				};
			}, () => File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/reset.missionsfs"), 167f, 32f, dismissOnSelect: false));
		}
		if (HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad)
		{
			dialogGUIVerticalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360553"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				loadBrowserShown = true;
				QuickSaveLoad.fetch.SpawnQuickLoadDialog(pauseGame: false, delegate(string saveName)
				{
					dialog.gameObject.SetActive(value: true);
					loadBrowserShown = false;
					if (!string.IsNullOrEmpty(saveName))
					{
						Close();
					}
				});
			}, 167f, 32f, false));
		}
		if (HighLogic.CurrentGame.Parameters.Flight.CanQuickSave && FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
		{
			DialogGUIButton child = new DialogGUIButton(QuickSaveLoad.QuickSaveClearToSave() ? Localizer.Format("#autoLOC_360569") : Localizer.Format("#autoLOC_8004249"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				QuickSaveLoad.fetch.SpawnSaveAsDialog(pauseGame: false, delegate(string saveName)
				{
					dialog.gameObject.SetActive(value: true);
					if (!string.IsNullOrEmpty(saveName))
					{
						Close();
					}
				});
			}, QuickSaveLoad.QuickSaveClearToSave, 167f, 32f, dismissOnSelect: false);
			dialogGUIVerticalLayout.AddChild(child);
		}
		DialogGUIVerticalLayout dialogGUIVerticalLayout2 = new DialogGUIVerticalLayout();
		if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton((canSaveAndExit == ClearToSaveStatus.CLEAR) ? Localizer.Format("#autoLOC_360586") : ("<color=orange>" + Localizer.Format("#autoLOC_360586") + "</color>"), delegate
			{
				if (canSaveAndExit == ClearToSaveStatus.CLEAR)
				{
					GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
					saveAndExit(GameScenes.SPACECENTER, HighLogic.CurrentGame.Updated());
				}
				else
				{
					dialog.gameObject.SetActive(value: false);
					sceneToLeaveTo = GameScenes.SPACECENTER;
					confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveToSpaceCenter", null, Localizer.Format("#autoLOC_360593"), HighLogic.UISkin, 450f, drawExitWithoutSaveOptions()), persistAcrossScenes: false, HighLogic.UISkin);
					confirmOptionDialog.OnDismiss = delegate
					{
						dialog.gameObject.SetActive(value: true);
					};
				}
			}, 167f, 32f, false));
		}
		if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation && HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInTrackingStation)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton((canSaveAndExit == ClearToSaveStatus.CLEAR) ? Localizer.Format("#autoLOC_360600") : ("<color=orange>" + Localizer.Format("#autoLOC_360600") + "</color>"), delegate
			{
				if (canSaveAndExit == ClearToSaveStatus.CLEAR)
				{
					GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
					saveAndExit(GameScenes.TRACKSTATION, HighLogic.CurrentGame.Updated());
				}
				else
				{
					dialog.gameObject.SetActive(value: false);
					sceneToLeaveTo = GameScenes.TRACKSTATION;
					confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveToTrackingStation", null, Localizer.Format("#autoLOC_360607"), HighLogic.UISkin, 450f, drawExitWithoutSaveOptions()), persistAcrossScenes: false, HighLogic.UISkin);
					confirmOptionDialog.OnDismiss = delegate
					{
						dialog.gameObject.SetActive(value: true);
					};
				}
			}, 167f, 32f, false));
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360614"), delegate
			{
				miniKeyBindings = MiniKeyBindings.Create(miniSettingsFinished);
				dialog.gameObject.SetActive(value: false);
			}, 167f, 32f, false));
		}
		DialogGUISpace dialogGUISpace = new DialogGUISpace(0f);
		dialogGUIVerticalLayout2.AddChild(dialogGUISpace);
		dialogGUIVerticalLayout2.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360624"), delegate
		{
			miniSettings = MiniSettings.Create(miniSettingsFinished);
			dialog.gameObject.SetActive(value: false);
		}, 167f, 32f, false));
		if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToMainMenu && HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_360632"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				sceneToLeaveTo = GameScenes.MAINMENU;
				InputLockManager.ClearControlLocks();
				confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveScenario", null, Localizer.Format("#autoLOC_7000034"), HighLogic.UISkin, 250f, drawExitScenarioOptions()), persistAcrossScenes: false, HighLogic.UISkin);
				confirmOptionDialog.OnDismiss = delegate
				{
					dialog.gameObject.SetActive(value: true);
				};
			}, 167f, 32f, false));
		}
		if (MissionModeValid() && MissionSystem.IsTestMode)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton("#autoLOC_8003053", delegate
			{
				dialog.gameObject.SetActive(value: false);
				sceneToLeaveTo = GameScenes.MISSIONBUILDER;
				InputLockManager.ClearControlLocks();
				confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveTestMission", null, Localizer.Format("#autoLOC_8003053"), HighLogic.UISkin, 350f, drawExitTestMissionOptions(dialog)), persistAcrossScenes: false, HighLogic.UISkin);
				confirmOptionDialog.OnDismiss = delegate
				{
					dialog.gameObject.SetActive(value: true);
				};
			}, 167f, 32f, false));
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUIButton((canSaveAndExit == ClearToSaveStatus.CLEAR) ? Localizer.Format("#autoLOC_360644") : ("<color=orange>" + Localizer.Format("#autoLOC_360644") + "</color>"), delegate
			{
				if (canSaveAndExit == ClearToSaveStatus.CLEAR)
				{
					if (showExitToMainConfirmation)
					{
						ShowExitToMainConfirmation();
						dialog.gameObject.SetActive(value: false);
					}
					else
					{
						GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
						saveAndExit(GameScenes.MAINMENU, HighLogic.CurrentGame.Updated());
					}
				}
				else
				{
					dialog.gameObject.SetActive(value: false);
					sceneToLeaveTo = GameScenes.MAINMENU;
					confirmOptionDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveFlight", null, Localizer.Format("#autoLOC_7000033"), HighLogic.UISkin, 450f, drawExitWithoutSaveOptions()), persistAcrossScenes: false, HighLogic.UISkin);
					confirmOptionDialog.OnDismiss = delegate
					{
						dialog.gameObject.SetActive(value: true);
					};
				}
			}, 167f, 32f, false));
		}
		if (dialogGUIVerticalLayout.children.Count > dialogGUIVerticalLayout2.children.Count - 1)
		{
			dialogGUISpace.space = (dialogGUIVerticalLayout.children.Count - (dialogGUIVerticalLayout2.children.Count - 1)) * 36 - 4;
		}
		else
		{
			dialogGUIVerticalLayout2.children.Remove(dialogGUISpace);
		}
		list.Add(dialogObj = new DialogGUIHorizontalLayout(dialogGUIVerticalLayout, dialogGUIVerticalLayout2));
		list.Add(new DialogGUISpace(4f));
		UIStyle uIStyle = new UIStyle(HighLogic.UISkin.label);
		uIStyle.alignment = TextAnchor.UpperRight;
		DialogGUIButton dialogGUIButton = new DialogGUIButton(Localizer.Format("<color=#d0d0d0><i><<1>></i></color>", "#autoLOC_901103", Versioning.GetVersionStringFull()), delegate
		{
			DebugScreenSpawner.ShowTab("VersionInfo");
		}, 300f, 20f, dismissOnSelect: false, uIStyle);
		dialogGUIButton.textLabelOptions = new DialogGUIButton.TextLabelOptions
		{
			enableWordWrapping = false,
			resizeBestFit = false
		};
		dialogGUIButton.ClearButtonImage();
		list.Add(new DialogGUIHorizontalLayout(true, false, new DialogGUIFlexibleSpace(), dialogGUIButton));
		return list.ToArray();
	}

	public static DialogGUIBase[] drawMissionRestartOptions(PopupDialog dialog)
	{
		DialogGUIButton dialogGUIButton = null;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_8002062"));
		list.Add(item);
		string saveName = "persistent";
		string savePath = HighLogic.SaveFolder;
		string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/reset.missionsfs";
		if (File.Exists(path))
		{
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_417274"), delegate
			{
				if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs"))
				{
					File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs");
				}
				if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs"))
				{
					File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs");
				}
				if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/startcreatevesselspawn.missionsfs"))
				{
					File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/startcreatevesselspawn.missionsfs", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs", overwrite: true);
				}
				ConfigNode configNode = ConfigNode.Load(path);
				HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode, saveName, nullIfIncompatible: true, suppressIncompatibleMessage: false);
				HighLogic.SaveFolder = savePath;
				GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
				if (configNode != null)
				{
					GameEvents.onGameStatePostLoad.Fire(configNode);
				}
				HighLogic.CurrentGame.missionToStart.InitMission();
				GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", savePath, SaveMode.OVERWRITE);
				HighLogic.CurrentGame.Start();
				dialog.Dismiss();
			}, dismissOnSelect: true);
			list.Add(dialogGUIButton);
		}
		dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			dialog.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(dialogGUIButton);
		return list.ToArray();
	}

	public DialogGUIBase[] drawRevertOptions()
	{
		DialogGUIButton dialogGUIButton = null;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360683"));
		list.Add(item);
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			drawMissionsRevertOptions(dialog, list);
		}
		else
		{
			drawStockRevertOptions(dialog, list);
		}
		dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			dialog.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(dialogGUIButton);
		return list.ToArray();
	}

	public static void drawStockRevertOptions(PopupDialog dialog, List<DialogGUIBase> options)
	{
		if (FlightDriver.CanRevertToPostInit && HighLogic.CurrentGame.Parameters.Flight.CanRestart)
		{
			DialogGUIButton item = new DialogGUIButton(Localizer.Format("#autoLOC_360687", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PostInitState.UniversalTime, 3, explicitPositive: false)), delegate
			{
				dialog.Dismiss();
				FlightDriver.RevertToLaunch();
			}, dismissOnSelect: true);
			options.Add(item);
		}
		if (!HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor || !FlightDriver.CanRevertToPrelaunch || ShipConstruction.ShipConfig == null)
		{
			return;
		}
		switch (ShipConstruction.ShipType)
		{
		case EditorFacility.const_2:
		{
			DialogGUIButton item3 = new DialogGUIButton(Localizer.Format("#autoLOC_360710", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PreLaunchState.UniversalTime, 3, explicitPositive: false)), delegate
			{
				dialog.Dismiss();
				FlightDriver.RevertToPrelaunch(EditorFacility.const_2);
			}, dismissOnSelect: true);
			options.Add(item3);
			break;
		}
		case EditorFacility.const_1:
		{
			DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_360700", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PreLaunchState.UniversalTime, 3, explicitPositive: false)), delegate
			{
				dialog.Dismiss();
				FlightDriver.RevertToPrelaunch(EditorFacility.const_1);
			}, dismissOnSelect: true);
			options.Add(item2);
			break;
		}
		}
	}

	public static void drawMissionsRevertOptions(PopupDialog dialog, List<DialogGUIBase> options)
	{
		string saveName = "persistent";
		string savePath = HighLogic.SaveFolder;
		string pathspawn = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs";
		if (File.Exists(pathspawn))
		{
			DialogGUIButton item = new DialogGUIButton(Localizer.Format("#autoLOC_8002058"), delegate
			{
				ConfigNode configNode2 = ConfigNode.Load(pathspawn);
				HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode2, saveName, nullIfIncompatible: true, suppressIncompatibleMessage: false);
				HighLogic.SaveFolder = savePath;
				GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
				if (configNode2 != null)
				{
					GameEvents.onGameStatePostLoad.Fire(configNode2);
				}
				GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", savePath, SaveMode.OVERWRITE);
				HighLogic.CurrentGame.Start();
				if (dialog != null)
				{
					dialog.Dismiss();
				}
			}, dismissOnSelect: true);
			options.Add(item);
		}
		string pathcreate = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs";
		if (!File.Exists(pathcreate))
		{
			return;
		}
		DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_8002059"), delegate
		{
			ConfigNode configNode = ConfigNode.Load(pathcreate);
			HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode, saveName, nullIfIncompatible: true, suppressIncompatibleMessage: false);
			HighLogic.SaveFolder = savePath;
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			if (configNode != null)
			{
				GameEvents.onGameStatePostLoad.Fire(configNode);
			}
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", savePath, SaveMode.OVERWRITE);
			HighLogic.CurrentGame.Start();
			if (dialog != null)
			{
				dialog.Dismiss();
			}
		}, dismissOnSelect: true);
		options.Add(item2);
	}

	public DialogGUIBase[] drawExitWithoutSaveOptions()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		switch (canSaveAndExit)
		{
		case ClearToSaveStatus.NOT_IN_ATMOSPHERE:
		{
			DialogGUILabel item6 = new DialogGUILabel(Localizer.Format("#autoLOC_360742"));
			list.Add(item6);
			break;
		}
		case ClearToSaveStatus.NOT_UNDER_ACCELERATION:
		{
			DialogGUILabel item5 = new DialogGUILabel(Localizer.Format("#autoLOC_360747"));
			list.Add(item5);
			break;
		}
		case ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE:
		{
			DialogGUILabel item4 = new DialogGUILabel(Localizer.Format("#autoLOC_360757"));
			list.Add(item4);
			break;
		}
		case ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH:
		{
			DialogGUILabel item3 = new DialogGUILabel(Localizer.Format("#autoLOC_360752"));
			list.Add(item3);
			break;
		}
		case ClearToSaveStatus.NOT_WHILE_ON_A_LADDER:
		{
			DialogGUILabel item2 = new DialogGUILabel(Localizer.Format("#autoLOC_360762"));
			list.Add(item2);
			break;
		}
		case ClearToSaveStatus.NOT_WHILE_THROTTLED_UP:
		{
			DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360767"));
			list.Add(item);
			break;
		}
		}
		if (HighLogic.CurrentGame.Parameters.Flight.CanRestart)
		{
			DialogGUILabel item7 = new DialogGUILabel(Localizer.Format("#autoLOC_360774", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - HighLogic.CurrentGame.UniversalTime, 3, explicitPositive: false)));
			list.Add(item7);
			DialogGUIButton item8 = new DialogGUIButton(Localizer.Format("#autoLOC_360779"), delegate
			{
				dialog.Dismiss();
				GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
				if (sceneToLeaveTo == GameScenes.MAINMENU)
				{
					AnalyticsUtil.LogSaveGameClosed(HighLogic.CurrentGame);
				}
				HighLogic.LoadScene(sceneToLeaveTo);
			}, dismissOnSelect: true);
			list.Add(item8);
		}
		else
		{
			DialogGUILabel item9 = new DialogGUILabel(Localizer.Format("#autoLOC_360788"));
			list.Add(item9);
			DialogGUIButton item10 = new DialogGUIButton(Localizer.Format("#autoLOC_360791"), delegate
			{
				GameEvents.onSceneConfirmExit.Fire(GameScenes.FLIGHT);
				saveAndExit(sceneToLeaveTo, HighLogic.CurrentGame.Updated());
			}, dismissOnSelect: true);
			list.Add(item10);
		}
		DialogGUIButton item11 = new DialogGUIButton(Localizer.Format("#autoLOC_360821"), delegate
		{
			dialog.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(item11);
		return list.ToArray();
	}

	public DialogGUIBase[] drawExitScenarioOptions()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360812"));
		list.Add(item);
		DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_360815"), delegate
		{
			GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
			saveAndExit(sceneToLeaveTo, HighLogic.CurrentGame.Updated());
		}, dismissOnSelect: true);
		list.Add(item2);
		item2 = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			dialog.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(item2);
		return list.ToArray();
	}

	public static DialogGUIBase[] drawExitTestMissionOptions(PopupDialog dialogMenu)
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_8003054"));
		list.Add(item);
		DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_8005061"), delegate
		{
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
			{
				dialogMenu.Dismiss();
				FlightGlobals.ClearpersistentIdDictionaries();
				GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
				GamePersistence.SaveGame(HighLogic.CurrentGame.Updated(), "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)
				{
					FlightGlobals.fetch.activeVessel = null;
				}
				MissionEditorLogic.StartUpMissionEditor(MissionSystem.missions[0].MissionInfo.FilePath);
			}
		}, dismissOnSelect: true);
		list.Add(item2);
		item2 = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			dialogMenu.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(item2);
		return list.ToArray();
	}

	public void ShowExitToMainConfirmation()
	{
		exitToMainConfirmationShown = true;
		dialogExitToMainConfirmation = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExitToMainMenu", Localizer.Format("#autoLOC_360836"), Localizer.Format("#autoLOC_360837"), null, new DialogGUISpace(5f), new DialogGUIToggle(() => !showExitToMainConfirmation, Localizer.Format("#autoLOC_360842"), delegate
		{
			showExitToMainConfirmation = !showExitToMainConfirmation;
		}, 120f), new DialogGUISpace(5f), new DialogGUIHorizontalLayout(new DialogGUIButton(Localizer.Format("#autoLOC_360725"), CancelExitToMainMenuConfirmation, 144f, 32f, true), new DialogGUIButton(Localizer.Format("#autoLOC_360837"), SubmitExitToMainMenuConfirmation, 144f, 32f, true))), persistAcrossScenes: false, guiSkin);
		dialogExitToMainConfirmation.OnDismiss = CancelExitToMainMenuConfirmation;
		MenuNavigation.SpawnMenuNavigation(dialogExitToMainConfirmation.gameObject, Navigation.Mode.Automatic, hasText: true, limitCheck: true);
	}

	public void SubmitExitToMainMenuConfirmation()
	{
		if (GameSettings.SHOW_EXIT_TO_MENU_CONFIRMATION != showExitToMainConfirmation)
		{
			GameSettings.SHOW_EXIT_TO_MENU_CONFIRMATION = showExitToMainConfirmation;
			GameSettings.SaveSettings();
		}
		exitToMainConfirmationShown = false;
		GameEvents.onSceneConfirmExit.Fire(HighLogic.CurrentGame.startScene);
		saveAndExit(GameScenes.MAINMENU, HighLogic.CurrentGame.Updated());
	}

	public void CancelExitToMainMenuConfirmation()
	{
		dialog.gameObject.SetActive(value: true);
		showExitToMainConfirmation = GameSettings.SHOW_EXIT_TO_MENU_CONFIRMATION;
		exitToMainConfirmationShown = false;
	}

	public void saveAndExit(GameScenes sceneToLoad, Game stateToSave)
	{
		dialog.Dismiss();
		FlightGlobals.ClearpersistentIdDictionaries();
		GamePersistence.SaveGame(stateToSave, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		if (sceneToLoad == GameScenes.MAINMENU)
		{
			AnalyticsUtil.LogSaveGameClosed(HighLogic.CurrentGame);
		}
		HighLogic.LoadScene(sceneToLoad);
	}

	public void miniSettingsFinished()
	{
		miniSettings = null;
		miniKeyBindings = null;
		dialog.gameObject.SetActive(value: true);
	}

	public bool MissionModeValid()
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			return false;
		}
		byte[] hashBytes = null;
		string signature = null;
		if (MissionSystem.Instance != null && MissionSystem.missions.Count > 0 && MissionSystem.missions[0] != null)
		{
			hashBytes = MissionSystem.missions[0].HashBytes;
			signature = MissionSystem.missions[0].Signature;
		}
		return ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature);
	}

	public void BuildButtonList(GameObject obj)
	{
		MenuNavigation.SpawnMenuNavigation(obj, Navigation.Mode.Automatic, limitCheck: true);
	}
}
