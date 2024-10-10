using System;
using System.IO;
using ns2;
using ns9;
using SaveUpgradePipeline;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSaveLoad : MonoBehaviour
{
	public delegate void FinishedSaveLoadDialogCallback(string saveName);

	public class loadDialogParameters
	{
		public bool pauseGame;

		public FinishedSaveLoadDialogCallback onLoadCloseReturn;

		public loadDialogParameters(bool pauseGame, FinishedSaveLoadDialogCallback onLoadCloseReturn)
		{
			this.pauseGame = pauseGame;
			this.onLoadCloseReturn = onLoadCloseReturn;
		}
	}

	public enum FilenameCheckResults
	{
		Empty,
		AlreadyExists,
		InvalidCharacters,
		Good
	}

	public static QuickSaveLoad fetch;

	public bool AutoSaveOnQuickSave;

	public bool holdToLoad;

	public float holdTime = 1f;

	public float TatHold;

	public UISkinDef saveBrowserSkin;

	public ScreenMessage screenMessage;

	public DialogUISelectFile loadDialog;

	public PopupDialog saveDialog;

	public PopupDialog confirmDialog;

	public FinishedSaveLoadDialogCallback onSaveAsCloseReturn;

	public bool pauseGame;

	public MenuNavigation menuNav;

	public TMP_InputField inputField;

	public bool blockedSubmitOnStart;

	public string quicksaveFilename;

	public LoadGameDialog loadBrowser;

	public loadDialogParameters loadDialogParams;

	public static string cacheAutoLOC_174575;

	public static string cacheAutoLOC_174610;

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		fetch = this;
		screenMessage = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void start()
	{
		saveBrowserSkin = UISkinManager.GetSkin("MiniSettingsSkin");
		blockedSubmitOnStart = false;
	}

	public void Update()
	{
		if (UIMasterController.Instance.CameraMode)
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (saveDialog != null)
			{
				OnSaveAsClose("");
				if (confirmDialog != null)
				{
					confirmDialog.Dismiss();
				}
			}
			else if (loadDialog != null)
			{
				OnLoadClose("");
				loadDialog.Destroy();
			}
		}
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && saveDialog != null)
		{
			if (blockedSubmitOnStart)
			{
				blockedSubmitOnStart = false;
			}
			else if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
			{
				ConfirmDialog(pauseGame, onSaveAsCloseReturn);
			}
		}
		if (InputLockManager.IsUnlocked(ControlTypes.QUICKSAVE) && GameSettings.QUICKSAVE.GetKeyDown())
		{
			quickSave(GameSettings.MODIFIER_KEY.GetKey());
		}
		if (InputLockManager.IsUnlocked(ControlTypes.QUICKLOAD) && GameSettings.QUICKLOAD.GetKeyDown())
		{
			if (!HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad)
			{
				screenMessage.message = cacheAutoLOC_174575;
				screenMessage.duration = 2f;
				ScreenMessages.PostScreenMessage(screenMessage);
				return;
			}
			if (holdToLoad && !FlightDriver.Pause)
			{
				if (!GameSettings.MODIFIER_KEY.GetKey())
				{
					screenMessage.message = Localizer.Format("#autoLOC_174589", GameSettings.QUICKLOAD.name);
					screenMessage.duration = holdTime;
					ScreenMessages.PostScreenMessage(screenMessage);
				}
				if (!IsInvoking("onHoldComplete"))
				{
					Invoke("onHoldComplete", holdTime);
				}
			}
			else
			{
				quickLoad("quicksave", HighLogic.SaveFolder);
			}
		}
		if (GameSettings.QUICKLOAD.GetKeyUp() && IsInvoking("onHoldComplete"))
		{
			CancelInvoke("onHoldComplete");
		}
		if (InputLockManager.IsUnlocked(ControlTypes.QUICKLOAD) && GameSettings.MODIFIER_KEY.GetKey() && GameSettings.QUICKLOAD.GetKeyDown())
		{
			if (!HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad)
			{
				screenMessage.message = cacheAutoLOC_174610;
				screenMessage.duration = 2f;
				ScreenMessages.PostScreenMessage(screenMessage);
			}
			else
			{
				SpawnQuickLoadDialog();
			}
		}
	}

	public void onHoldComplete()
	{
		quickLoad("quicksave", HighLogic.SaveFolder);
	}

	public static void QuickSave()
	{
		fetch.quickSave(saveAs: false);
	}

	public void quickSave(bool saveAs)
	{
		if (!HighLogic.CurrentGame.Parameters.Flight.CanQuickSave)
		{
			screenMessage.message = Localizer.Format("#autoLOC_174631");
			screenMessage.duration = 2f;
			ScreenMessages.PostScreenMessage(screenMessage);
		}
		else if (FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
		{
			if (QuickSaveClearToSave())
			{
				if (saveAs)
				{
					SpawnSaveAsDialog();
				}
				else
				{
					doSave("quicksave");
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(FlightGlobals.GetNotClearToSaveStatusReason(FlightGlobals.ClearToSave(), Localizer.Format("#autoLOC_174654")), 2f);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_174659"), 2f);
		}
	}

	public static bool QuickSaveClearToSave()
	{
		ClearToSaveStatus clearToSaveStatus = FlightGlobals.ClearToSave(logMsg: false);
		if (clearToSaveStatus != 0 && !GameSettings.CAN_ALWAYS_QUICKSAVE && (clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_ON_A_LADDER || clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE || clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH))
		{
			return false;
		}
		return true;
	}

	public void doSave(string filename, string screenMsg = "#autoLOC_174669")
	{
		GameEvents.onGameAboutToQuicksave.Fire();
		Game game = HighLogic.CurrentGame.Updated();
		game.startScene = GameScenes.FLIGHT;
		GamePersistence.SaveGame(game, filename, HighLogic.SaveFolder, SaveMode.BACKUP);
		screenMessage.message = Localizer.Format(screenMsg);
		screenMessage.duration = 2f;
		ScreenMessages.PostScreenMessage(screenMessage);
		game.startScene = GameScenes.SPACECENTER;
		if (FlightGlobals.ClearToSave() == ClearToSaveStatus.CLEAR && AutoSaveOnQuickSave)
		{
			GamePersistence.SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		}
	}

	public void quickLoad(string filename, string folder)
	{
		if (ROCManager.Instance != null)
		{
			ROCManager.Instance.ClearROCsCache();
		}
		ConfigNode configNode = GamePersistence.LoadSFSFile(filename, folder);
		if (configNode == null)
		{
			screenMessage.message = Localizer.Format("#autoLOC_174686", "#autoLOC_6002266");
			screenMessage.duration = 2f;
			ScreenMessages.PostScreenMessage(screenMessage);
		}
		else if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && configNode.HasNode("GAME") && configNode.GetNode("GAME").HasNode("MISSIONTOSTART"))
		{
			ConfigNode configNode2 = configNode;
			HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode2, filename, nullIfIncompatible: true, suppressIncompatibleMessage: false);
			HighLogic.SaveFolder = folder;
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			if (configNode2 != null)
			{
				GameEvents.onGameStatePostLoad.Fire(configNode2);
			}
			HighLogic.CurrentGame.missionToStart.InitMission();
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", folder, SaveMode.OVERWRITE);
			HighLogic.CurrentGame.Start();
		}
		else
		{
			Version originalVersion = NodeUtil.GetCfgVersion(configNode, LoadContext.flag_1);
			KSPUpgradePipeline.Process(configNode, folder, LoadContext.flag_1, delegate(ConfigNode n)
			{
				onQuickloadPipelineFinished(n, folder, originalVersion);
			}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
			{
				onQuickloadPipelineFailed(opt, n, folder, originalVersion);
			});
		}
	}

	public void onQuickloadPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, string folder, Version originalVersion)
	{
		switch (opt)
		{
		case KSPUpgradePipeline.UpgradeFailOption.LoadAnyway:
			onQuickloadPipelineFinished(n, folder, originalVersion);
			break;
		case KSPUpgradePipeline.UpgradeFailOption.Cancel:
			FlightDriver.SetPause(pauseState: false);
			break;
		}
	}

	public void onQuickloadPipelineFinished(ConfigNode node, string saveName, Version originalVersion)
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
		else
		{
			FlightDriver.SetPause(pauseState: false);
		}
	}

	public void SpawnSaveAsDialog()
	{
		SpawnSaveAsDialog(pauseGame: true, null);
	}

	public void SpawnSaveAsDialog(bool pauseGame, FinishedSaveLoadDialogCallback onSaveAsCloseReturn)
	{
		blockedSubmitOnStart = Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter);
		this.pauseGame = pauseGame;
		this.onSaveAsCloseReturn = onSaveAsCloseReturn;
		if (pauseGame)
		{
			FlightDriver.SetPause(pauseState: true);
		}
		InputLockManager.SetControlLock("SaveAsDialog");
		quicksaveFilename = Localizer.Format("#autoLOC_6002266");
		int num = 0;
		while (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/" + quicksaveFilename + ".sfs"))
		{
			quicksaveFilename = Localizer.Format("#autoLOC_6002266") + " #" + ++num;
		}
		saveDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Quicksave", Localizer.Format("#autoLOC_417232"), Localizer.Format("#autoLOC_174772"), saveBrowserSkin, new DialogGUITextInput(quicksaveFilename, multiline: false, 64, delegate(string n)
		{
			quicksaveFilename = EscapeString(n);
			return quicksaveFilename;
		}, 28f), new DialogGUIButton(Localizer.Format("#autoLOC_174778"), delegate
		{
			ConfirmDialog(pauseGame, onSaveAsCloseReturn);
		}, dismissOnSelect: false), new DialogGUIButton(Localizer.Format("#autoLOC_174783"), delegate
		{
			OnSaveAsClose("", pauseGame, onSaveAsCloseReturn);
		}, dismissOnSelect: true)), persistAcrossScenes: false, null);
		saveDialog.OnDismiss = delegate
		{
			OnSaveAsClose("", pauseGame, onSaveAsCloseReturn);
		};
		menuNav = MenuNavigation.SpawnMenuNavigation(saveDialog.gameObject, Navigation.Mode.Automatic);
		if (menuNav != null)
		{
			menuNav.SetItemAsFirstSelected(saveDialog.gameObject);
		}
	}

	public void ConfirmDialog(bool pauseGame = true, FinishedSaveLoadDialogCallback onSaveAsCloseReturn = null)
	{
		if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/" + quicksaveFilename + ".sfs"))
		{
			saveDialog.gameObject.SetActive(value: false);
			confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("OverwriteSave", Localizer.Format("#autoLOC_417256", quicksaveFilename), Localizer.Format("#autoLOC_465206"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUIButton(Localizer.Format("#autoLOC_174798"), delegate
			{
				OnSaveAsClose(quicksaveFilename, pauseGame, onSaveAsCloseReturn);
				confirmDialog.Dismiss();
			}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_174804"), delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			}, dismissOnSelect: true)), persistAcrossScenes: false, null);
			confirmDialog.OnDismiss = delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			};
		}
		else if (CheckFilename(quicksaveFilename) != FilenameCheckResults.Good)
		{
			confirmDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("InvalidSaveName", Localizer.Format("#autoLOC_417273"), Localizer.Format("#autoLOC_236416"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUIButton(Localizer.Format("#autoLOC_174814"), delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			}, dismissOnSelect: true)), persistAcrossScenes: false, null);
			confirmDialog.OnDismiss = delegate
			{
				confirmDialog.Dismiss();
				saveDialog.gameObject.SetActive(value: true);
			};
		}
		else
		{
			OnSaveAsClose(quicksaveFilename, pauseGame, onSaveAsCloseReturn);
		}
	}

	public void OnSaveAsClose(string saveName, bool pauseGame = true, FinishedSaveLoadDialogCallback onSaveAsCloseReturn = null)
	{
		saveDialog.Dismiss();
		InputLockManager.RemoveControlLock("SaveAsDialog");
		if (!string.IsNullOrEmpty(saveName) && CheckFilename(saveName) == FilenameCheckResults.Good)
		{
			doSave(quicksaveFilename);
		}
		if (pauseGame)
		{
			FlightDriver.SetPause(pauseState: false);
		}
		onSaveAsCloseReturn?.Invoke(saveName);
		blockedSubmitOnStart = false;
	}

	public void SpawnQuickLoadDialog()
	{
		SpawnQuickLoadDialog(pauseGame: true, null);
	}

	public void SpawnQuickLoadDialog(bool pauseGame, FinishedSaveLoadDialogCallback onLoadCloseReturn)
	{
		loadDialogParams = new loadDialogParameters(pauseGame, onLoadCloseReturn);
		if (pauseGame)
		{
			FlightDriver.SetPause(pauseState: true);
		}
		InputLockManager.SetControlLock("LoadFileDialog");
		if (loadBrowser == null)
		{
			loadBrowser = LoadGameDialog.Create(OnLoadClose, HighLogic.SaveFolder, persistent: false, UISkinManager.GetSkin("MainMenuSkin"));
		}
	}

	public void OnLoadClose(string saveName)
	{
		loadBrowser = null;
		InputLockManager.RemoveControlLock("LoadFileDialog");
		if (!string.IsNullOrEmpty(saveName))
		{
			quickLoad(Path.GetFileNameWithoutExtension(saveName), HighLogic.SaveFolder);
		}
		if (loadDialogParams.pauseGame)
		{
			FlightDriver.SetPause(pauseState: false);
		}
		if (loadDialogParams.onLoadCloseReturn != null)
		{
			loadDialogParams.onLoadCloseReturn(saveName);
		}
		loadDialogParams = null;
	}

	public string EscapeString(string str)
	{
		str = str.Replace('\r', ' ');
		str = str.Replace('\n', ' ');
		str = str.Replace('"', '\'');
		str = KSPUtil.SanitizeFilename(str);
		return str;
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

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_174575 = Localizer.Format("#autoLOC_174575");
		cacheAutoLOC_174610 = Localizer.Format("#autoLOC_174610");
	}
}
