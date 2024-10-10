using System;
using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions.Runtime;
using ns25;
using ns9;
using SaveUpgradePipeline;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KSCPauseMenu : MonoBehaviour
{
	public enum FilenameCheckResults
	{
		Empty,
		AlreadyExists,
		InvalidCharacters,
		Good
	}

	public UISkinDef windowSkin;

	public Rect windowRect;

	public Callback onDismiss;

	public PopupDialog saveDialog;

	public PopupDialog confirmDialog;

	public LoadGameDialog saveBrowser;

	public MiniSettings miniSettings;

	public PopupDialog dialog;

	public DialogGUIButton button;

	public GameObject dialogObj;

	public TMP_InputField saveDialogInputField;

	public MenuNavigation menuNav;

	public string quicksaveFilename;

	public static KSCPauseMenu Instance { get; set; }

	public static KSCPauseMenu Create(Callback onDismissMenu)
	{
		KSCPauseMenu kSCPauseMenu2 = (Instance = new GameObject("KSC Pause Menu Handler").AddComponent<KSCPauseMenu>());
		kSCPauseMenu2.windowSkin = UISkinManager.GetSkin("MainMenuSkin");
		kSCPauseMenu2.windowRect = new Rect(0.5f, 0.5f, 250f, 200f);
		kSCPauseMenu2.onDismiss = onDismissMenu;
		kSCPauseMenu2.dialog = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("GamePaused", "", Localizer.Format("#autoLOC_360479"), kSCPauseMenu2.windowSkin, kSCPauseMenu2.windowRect, new DialogGUIVerticalLayout(sw: true, sh: false, 2f, new RectOffset(), TextAnchor.UpperLeft, kSCPauseMenu2.draw())), persistAcrossScenes: false, kSCPauseMenu2.windowSkin);
		Instance.dialogObj = kSCPauseMenu2.dialog.gameObject;
		return kSCPauseMenu2;
	}

	public void Awake()
	{
		Instance = this;
		FlightDriver.SetPause(pauseState: true);
		InputLockManager.SetControlLock("kscPauseMenu");
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		InputLockManager.RemoveControlLock("kscPauseMenu");
		FlightDriver.SetPause(pauseState: false);
	}

	public void Update()
	{
		if (miniSettings == null && saveDialog == null && saveBrowser == null && Input.GetKeyUp(KeyCode.Escape))
		{
			Dismiss();
		}
		else
		{
			if (!(saveDialog != null))
			{
				return;
			}
			if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && saveDialogInputField != null && EventSystem.current.currentSelectedGameObject == saveDialogInputField.gameObject)
			{
				ConfirmDialog();
			}
			else if (Input.GetKeyUp(KeyCode.Escape))
			{
				if (confirmDialog != null)
				{
					confirmDialog.Dismiss();
				}
				onSaveDialogDismiss(dismissAll: false);
			}
		}
	}

	public DialogGUIBase[] draw()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
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
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_417133"), delegate
		{
			Dismiss();
		}, dismissOnSelect: true));
		list.Add(new DialogGUISpace(5f));
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_417139"), delegate
		{
			dialog.gameObject.SetActive(value: false);
			SaveGame();
		}, dismissOnSelect: false));
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_417146"), delegate
		{
			dialog.gameObject.SetActive(value: false);
			LoadGame();
		}, dismissOnSelect: false));
		if (MissionModeValid())
		{
			list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_8006013"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RestartMission", null, Localizer.Format("#autoLOC_8006013"), HighLogic.UISkin, 350f, PauseMenu.drawMissionRestartOptions(dialog)), persistAcrossScenes: false, HighLogic.UISkin);
			}, () => File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/reset.missionsfs"), 167f, 32f, dismissOnSelect: false));
		}
		if (MissionModeValid() && HighLogic.CurrentGame.Parameters.Flight.CanRestart)
		{
			list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_8002060"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RevertingMission", null, Localizer.Format("#autoLOC_8002061"), HighLogic.UISkin, 350f, drawMissionRevertOptions()), persistAcrossScenes: false, HighLogic.UISkin);
			}, delegate
			{
				string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs";
				string path2 = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesseleditor.missionsfs";
				return File.Exists(path) || File.Exists(path2);
			}, 167f, 32f, dismissOnSelect: false));
		}
		list.Add(new DialogGUISpace(5f));
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_417154"), delegate
		{
			dialog.gameObject.SetActive(value: false);
			miniSettings = MiniSettings.Create(onMiniSettingsFinished);
		}, dismissOnSelect: false));
		list.Add(new DialogGUISpace(5f));
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.IsTestMode)
		{
			list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_8003053"), delegate
			{
				dialog.gameObject.SetActive(value: false);
				InputLockManager.ClearControlLocks();
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveTestMission", null, Localizer.Format("#autoLOC_8003053"), HighLogic.UISkin, 350f, PauseMenu.drawExitTestMissionOptions(dialog)), persistAcrossScenes: false, HighLogic.UISkin);
			}, 167f, 32f, false));
		}
		list.Add(new DialogGUISpace(5f));
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_417162"), delegate
		{
			if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanLeaveToMainMenu)
			{
				Dismiss();
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				AnalyticsUtil.LogSaveGameClosed(HighLogic.CurrentGame);
				HighLogic.LoadScene(GameScenes.MAINMENU);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_417172"), 4f, ScreenMessageStyle.UPPER_CENTER);
			}
		}, dismissOnSelect: true));
		list.Add(new DialogGUISpace(4f));
		UIStyle uIStyle = new UIStyle(HighLogic.UISkin.label);
		uIStyle.alignment = TextAnchor.UpperRight;
		DialogGUIButton dialogGUIButton = new DialogGUIButton(Localizer.Format("<color=#d0d0d0><i><<1>></i></color>", "#autoLOC_901103", Versioning.GetVersionStringFull()), delegate
		{
			DebugScreenSpawner.ShowTab("VersionInfo");
		}, 238f, 20f, dismissOnSelect: false, uIStyle);
		dialogGUIButton.textLabelOptions = new DialogGUIButton.TextLabelOptions
		{
			enableWordWrapping = false,
			resizeBestFit = false
		};
		dialogGUIButton.ClearButtonImage();
		list.Add(new DialogGUIHorizontalLayout(true, false, new DialogGUIFlexibleSpace(), dialogGUIButton));
		return list.ToArray();
	}

	public DialogGUIBase[] drawMissionRevertOptions()
	{
		DialogGUIButton dialogGUIButton = null;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360683"));
		list.Add(item);
		PauseMenu.drawMissionsRevertOptions(dialog, list);
		dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			dialog.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(dialogGUIButton);
		return list.ToArray();
	}

	public void onMiniSettingsFinished()
	{
		miniSettings = null;
		dialog.gameObject.SetActive(value: true);
	}

	public void Dismiss()
	{
		onDismiss();
		if (dialog != null)
		{
			dialog.Dismiss();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void InitiateSave()
	{
		if (saveBrowser != null)
		{
			onLoadDialogDismiss(null);
		}
		dialog.gameObject.SetActive(value: false);
		SaveGame();
	}

	public void SaveGame()
	{
		if (!HighLogic.CurrentGame.Parameters.Flight.CanQuickSave)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_417216"), 4f, ScreenMessageStyle.UPPER_CENTER);
			Dismiss();
		}
		else
		{
			spawnSaveDialog();
		}
	}

	public void spawnSaveDialog()
	{
		quicksaveFilename = Localizer.Format("#autoLOC_6002266");
		int num = 0;
		while (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/" + quicksaveFilename + ".sfs"))
		{
			quicksaveFilename = Localizer.Format("#autoLOC_6002266") + " #" + ++num;
		}
		saveDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("QuickSave", Localizer.Format("#autoLOC_417232"), Localizer.Format("#autoLOC_174772"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUITextInput(quicksaveFilename, multiline: false, 64, delegate(string n)
		{
			quicksaveFilename = n;
			return quicksaveFilename;
		}, 28f), new DialogGUIButton(Localizer.Format("#autoLOC_417238"), delegate
		{
			ConfirmDialog();
		}, dismissOnSelect: false), new DialogGUIButton(Localizer.Format("#autoLOC_417243"), delegate
		{
			onSaveDialogDismiss(dismissAll: false);
		}, dismissOnSelect: true)), persistAcrossScenes: false, null);
		saveDialog.onDestroy.AddListener(OnSaveDialogDestroy);
		menuNav = MenuNavigation.SpawnMenuNavigation(saveDialog.gameObject, Navigation.Mode.Automatic);
		if (menuNav != null)
		{
			menuNav.SetItemAsFirstSelected(saveDialog.gameObject);
			saveDialogInputField = saveDialog.gameObject.GetComponentInChildren<TMP_InputField>();
		}
	}

	public void ConfirmDialog()
	{
		if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/" + quicksaveFilename + ".sfs"))
		{
			saveDialog.gameObject.SetActive(value: false);
			confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SavegameConfirmation", Localizer.Format("#autoLOC_417256", quicksaveFilename), Localizer.Format("#autoLOC_465206"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUIButton(Localizer.Format("#autoLOC_417257"), delegate
			{
				doSave(quicksaveFilename);
				confirmDialog.Dismiss();
				onSaveDialogDismiss(dismissAll: true);
			}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_417264"), delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			}, dismissOnSelect: true)), persistAcrossScenes: false, null);
		}
		else if (CheckFilename(quicksaveFilename) != FilenameCheckResults.Good)
		{
			confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SavegameInvalidName", Localizer.Format("#autoLOC_417273"), Localizer.Format("#autoLOC_236416"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			}, dismissOnSelect: true)), persistAcrossScenes: false, null);
		}
		else
		{
			doSave(quicksaveFilename);
			onSaveDialogDismiss(dismissAll: true);
		}
	}

	public void OnSaveDialogDestroy()
	{
	}

	public void doSave(string filename)
	{
		Game game = HighLogic.CurrentGame.Updated();
		game.startScene = GameScenes.SPACECENTER;
		GamePersistence.SaveGame(game, filename, HighLogic.SaveFolder, SaveMode.OVERWRITE);
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_417323"), 2f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void onSaveDialogDismiss(bool dismissAll)
	{
		saveDialog.Dismiss();
		if (dismissAll)
		{
			Dismiss();
		}
		else
		{
			dialog.gameObject.SetActive(value: true);
		}
	}

	public FilenameCheckResults CheckFilename(string filename)
	{
		if (string.IsNullOrEmpty(filename))
		{
			return FilenameCheckResults.Empty;
		}
		if (KSPUtil.SanitizeFilename(filename) != filename)
		{
			return FilenameCheckResults.InvalidCharacters;
		}
		return FilenameCheckResults.Good;
	}

	public void InitiateLoad()
	{
		if (saveDialog != null)
		{
			onSaveDialogDismiss(dismissAll: false);
		}
		dialog.gameObject.SetActive(value: false);
		LoadGame();
	}

	public void LoadGame()
	{
		if (!HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_417375"), 4f, ScreenMessageStyle.UPPER_CENTER);
			Dismiss();
		}
		else if (saveBrowser == null)
		{
			saveBrowser = LoadGameDialog.Create(onLoadDialogDismiss, HighLogic.SaveFolder, persistent: false, UISkinManager.GetSkin("MainMenuSkin"));
		}
	}

	public void onLoadDialogDismiss(string path)
	{
		saveBrowser = null;
		if (!string.IsNullOrEmpty(path))
		{
			Dismiss();
			quickLoad(Path.GetFileNameWithoutExtension(path), HighLogic.SaveFolder);
		}
		else
		{
			dialog.gameObject.SetActive(value: true);
		}
	}

	public void quickLoad(string filename, string folder)
	{
		ConfigNode configNode = GamePersistence.LoadSFSFile(filename, folder);
		if (configNode == null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_485770", filename), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		Version originalVersion = NodeUtil.GetCfgVersion(configNode, LoadContext.flag_1);
		KSPUpgradePipeline.Process(configNode, folder, LoadContext.flag_1, delegate(ConfigNode n)
		{
			onPipelineFinished(n, folder, originalVersion);
		}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
		{
			onPipelineFailed(opt, n, folder, originalVersion);
		});
	}

	public void onPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, string folder, Version originalVersion)
	{
		if (opt != 0 && opt == KSPUpgradePipeline.UpgradeFailOption.LoadAnyway)
		{
			onPipelineFinished(n, folder, originalVersion);
		}
	}

	public void onPipelineFinished(ConfigNode node, string saveName, Version originalVersion)
	{
		Game game = GamePersistence.LoadGameCfg(node, saveName, nullIfIncompatible: true, suppressIncompatibleMessage: false);
		if (game != null && game.flightState != null && game.compatible)
		{
			GamePersistence.UpdateScenarioModules(game);
			if (node != null)
			{
				GameEvents.onGameStatePostLoad.Fire(node);
			}
			if (game.startScene != GameScenes.FLIGHT && !(originalVersion < new Version(0, 24, 0)))
			{
				GamePersistence.SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				HighLogic.LoadScene(GameScenes.SPACECENTER);
			}
			else
			{
				FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
			}
		}
	}

	public void BuildButtonList(GameObject obj)
	{
		MenuNavigation.SpawnMenuNavigation(obj, Navigation.Mode.Vertical, limitCheck: true);
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
}
