using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using ns16;
using ns7;
using ns9;
using SaveUpgradePipeline;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public class PlayerProfileInfo
	{
		public int vesselCount;

		public int int_0 = -1;
	}

	public TextProButton3D startBtn;

	public TextProButton3D commBtn;

	public TextProButton3D spaceportBtn;

	public TextProButton3D quitBtn;

	public TextProButton3D settingBtn;

	public TextProButton3D creditsBtn;

	public TextProButton3D newGameBtn;

	public TextProButton3D continueBtn;

	public TextProButton3D trainingBtn;

	public TextProButton3D scenariosBtn;

	public TextProButton3D playESAMissionsBtn;

	public TextProButton3D playMissionsBtn;

	public TextProButton3D missionBuilderBtn;

	public TextProButton3D buyMakingHistoryBtn;

	public TextProButton3D buySerenityBtn;

	public TextProButton3D merchandiseBtn;

	public TextProButton3D lastButtonSelected;

	[SerializeField]
	public MakingHistoryAboutDialog mhAboutDialogPrefab;

	[SerializeField]
	public SerenityAboutDialog serenityAboutDialogPrefab;

	[SerializeField]
	public TMP_Text playMissionsNewBadge;

	[SerializeField]
	public TMP_Text playMissionsNewESABadge;

	[SerializeField]
	public TMP_Text missionBuilderNewBadge;

	public TextProButton3D backBtn;

	public UISkinDefSO guiSkinDef;

	public Button unitTestsBtn;

	[SerializeField]
	public PrivacyDialog phDialogPrefab;

	[SerializeField]
	public WhatsNewDialog wnDialogPrefab;

	public Texture careerIcon;

	public Texture scienceSandboxIcon;

	public Texture sandboxIcon;

	public Texture scenarioIcon;

	public Texture tutorialIcon;

	public MainMenuEnvLogic envLogic;

	public string KSPsiteURL;

	public string SpaceportURL;

	public string DefaultFlagURL;

	public string makingHistoryExpansionURL;

	public string serenityExpansionURL;

	public string merchandiseURL;

	public bool hasUsedArrowsOnce;

	[SerializeField]
	public List<TextProButton3D> stageOneBtn = new List<TextProButton3D>();

	[SerializeField]
	public List<TextProButton3D> stageTwoBtn = new List<TextProButton3D>();

	public Rect menuWindowRect;

	public string menuWindowTitle = "";

	public static bool mhAboutDialogDisplayed = false;

	public static bool serenityAboutDialogDisplayed = false;

	public static bool expansionLoadErrorDisplayed = false;

	public static bool mainMenuVisited = false;

	[SerializeField]
	public List<string> invalidNames;

	public static string pName = null;

	public static string fURL = null;

	public static Game.Modes newGameMode = Game.Modes.CAREER;

	public static GameParameters newGameParameters;

	public FlagBrowserGUIButton flagBrowserGUIButton;

	public PopupDialog createGameDialog;

	public static bool MainMenuVisited => mainMenuVisited;

	public static string flagURL
	{
		get
		{
			return fURL;
		}
		set
		{
			fURL = value;
		}
	}

	public void Awake()
	{
		if (!TestManager.HaveUnitTests)
		{
			unitTestsBtn.gameObject.SetActive(value: false);
		}
		GameEvents.onInputLocksModified.Add(OnInputLocksModified);
		GameEvents.onMenuNavGetInput.Add(MenuNavHandler);
	}

	public void OnDestroy()
	{
		GameEvents.onInputLocksModified.Remove(OnInputLocksModified);
		InputLockManager.RemoveControlLock("applicationFocus");
		GameEvents.onMenuNavGetInput.Remove(MenuNavHandler);
	}

	public void Start()
	{
		HighLogic.CurrentGame = null;
		FlightDriver.FlightStateCache = null;
		FlightGlobals.ClearpersistentIdDictionaries();
		if (string.IsNullOrEmpty(pName))
		{
			pName = Localizer.Format("#autoLOC_7001234");
			flagURL = DefaultFlagURL;
			newGameMode = Game.Modes.CAREER;
			newGameParameters = GameParameters.GetDefaultParameters(newGameMode, GameParameters.Preset.Normal);
		}
		startBtn.onTap = startGame;
		quitBtn.onTap = quit;
		commBtn.onTap = goToKSPSite;
		spaceportBtn.onTap = goToSpacePort;
		settingBtn.onTap = settingsKSP;
		creditsBtn.onTap = credits;
		unitTestsBtn.onClick.AddListener(runUnitTests);
		newGameBtn.onTap = NewGame;
		continueBtn.onTap = LoadGame;
		trainingBtn.onTap = StartTraining;
		scenariosBtn.onTap = StartScenario;
		playESAMissionsBtn.onTap = PlayBaseMissions;
		merchandiseBtn.onTap = GoToMerchandisingSite;
		playMissionsBtn.onTap = PlayMissions;
		missionBuilderBtn.onTap = MissionEditor;
		buyMakingHistoryBtn.onTap = BuyExpansionPack;
		buySerenityBtn.onTap = BuyExpansionPack2;
		playESAMissionsBtn.gameObject.SetActive(!ExpansionsLoader.IsExpansionInstalled("MakingHistory"));
		playMissionsBtn.gameObject.SetActive(ExpansionsLoader.IsExpansionInstalled("MakingHistory"));
		missionBuilderBtn.gameObject.SetActive(ExpansionsLoader.IsExpansionInstalled("MakingHistory"));
		buyMakingHistoryBtn.gameObject.SetActive(!ExpansionsLoader.IsExpansionInstalled("MakingHistory"));
		buySerenityBtn.gameObject.SetActive(!ExpansionsLoader.IsExpansionInstalled("Serenity"));
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			RectTransform rectTransform = (RectTransform)playMissionsBtn.gameObject.transform;
			rectTransform.sizeDelta = new Vector2(playMissionsBtn.Text.GetPreferredValues().x, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = (RectTransform)missionBuilderBtn.gameObject.transform;
			rectTransform2.sizeDelta = new Vector2(missionBuilderBtn.Text.GetPreferredValues().x, rectTransform2.sizeDelta.y);
			playMissionsNewBadge.gameObject.SetActive(!GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED);
			if (playMissionsNewBadge.gameObject.activeSelf)
			{
				playMissionsNewESABadge.gameObject.SetActive(value: false);
			}
			else
			{
				playMissionsNewESABadge.gameObject.SetActive(Versioning.version_major == 1 && Versioning.version_minor == 9);
			}
			missionBuilderNewBadge.gameObject.SetActive(!GameSettings.TUTORIALS_MISSION_BUILDER_ENTERED);
		}
		else
		{
			bool flag = false;
			if (ExpansionsLoader.Instance != null && ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad().Count > 0)
			{
				List<string> expansionsThatFailedToLoad = ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad();
				int num = 0;
				while (!flag && num < expansionsThatFailedToLoad.Count)
				{
					flag = expansionsThatFailedToLoad[num].StartsWith("Making History");
					num++;
				}
			}
			if (!GameSettings.SHOW_ANALYTICS_DIALOG && !GameSettings.SHOW_WHATSNEW_DIALOG && GameSettings.MISSION_SHOW_EXPANSION_INFO && !flag && !mhAboutDialogDisplayed)
			{
				mhAboutDialogPrefab.Create(makingHistoryExpansionURL);
				mhAboutDialogDisplayed = true;
			}
			playMissionsNewBadge.gameObject.SetActive(value: false);
			missionBuilderNewBadge.gameObject.SetActive(value: false);
		}
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			bool flag2 = false;
			if (ExpansionsLoader.Instance != null && ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad().Count > 0)
			{
				List<string> expansionsThatFailedToLoad2 = ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad();
				int num2 = 0;
				while (!flag2 && num2 < expansionsThatFailedToLoad2.Count)
				{
					flag2 = expansionsThatFailedToLoad2[num2].StartsWith("Serenity");
					num2++;
				}
			}
			if (!GameSettings.SHOW_ANALYTICS_DIALOG && !GameSettings.SHOW_WHATSNEW_DIALOG && GameSettings.SERENITY_SHOW_EXPANSION_INFO && !flag2 && !serenityAboutDialogDisplayed)
			{
				serenityAboutDialogPrefab.Create(serenityExpansionURL);
				serenityAboutDialogDisplayed = true;
			}
		}
		backBtn.onTap = cancelStartGame;
		Application.targetFrameRate = GameSettings.FRAMERATE_LIMIT;
		if (InputLockManager.IsLocked(ControlTypes.MAIN_MENU))
		{
			lockEverything();
		}
		Time.timeScale = 1f;
		if (!expansionLoadErrorDisplayed && ExpansionsLoader.Instance != null && ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad().Count > 0)
		{
			expansionLoadErrorDisplayed = true;
			List<string> expansionsThatFailedToLoad3 = ExpansionsLoader.Instance.GetExpansionsThatFailedToLoad();
			string text = "";
			for (int i = 0; i < expansionsThatFailedToLoad3.Count; i++)
			{
				text = text + "\n" + expansionsThatFailedToLoad3[i];
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExpansionLoadFailed", Localizer.Format("#autoLOC_8004229", text), Localizer.Format("#autoLOC_8004230"), null, new DialogGUIButton(Localizer.Format("#autoLOC_190905"), null, dismissOnSelect: true)), persistAcrossScenes: false, null);
		}
		if (!mainMenuVisited)
		{
			if (GameSettings.SHOW_ANALYTICS_DIALOG_SettingExists && GameSettings.SHOW_ANALYTICS_DIALOG)
			{
				phDialogPrefab.Create();
			}
			else if (!GameSettings.SHOW_ANALYTICS_DIALOG_SettingExists && AnalyticsUtil.optOutValueAtLaunch < 0)
			{
				phDialogPrefab.Create();
			}
			if (!GameSettings.SHOW_ANALYTICS_DIALOG && GameSettings.SHOW_WHATSNEW_DIALOG_SettingExists && GameSettings.SHOW_WHATSNEW_DIALOG)
			{
				wnDialogPrefab.Create();
			}
			else if (!GameSettings.SHOW_WHATSNEW_DIALOG_SettingExists)
			{
				wnDialogPrefab.Create();
			}
			else if (!GameSettings.SHOW_WHATSNEW_DIALOG && GameSettings.SHOW_WHATSNEW_DIALOG_VersionsShown.Split(',').IndexOf(VersioningBase.GetVersionString()) < 0)
			{
				wnDialogPrefab.Create();
			}
			if (GameSettings.SHOW_ANALYTICS_DIALOG)
			{
				GameSettings.SHOW_ANALYTICS_DIALOG = false;
				GameSettings.SaveGameSettingsOnly();
			}
			KSPUpgradePipeline.Init();
		}
		mainMenuVisited = true;
	}

	public void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			InputLockManager.SetControlLock("applicationFocus");
		}
		else
		{
			InputLockManager.RemoveControlLock("applicationFocus");
		}
	}

	public void runUnitTests()
	{
		UnitTestingWindow.OpenWindow();
	}

	public void startGame()
	{
		if ((bool)envLogic)
		{
			envLogic.GoToStage(1);
		}
		ResetNavigationOnStageChange();
	}

	public void NewGame()
	{
		menuWindowRect = new Rect(0.5f, 0.5f, 500f, 150f);
		menuWindowTitle = Localizer.Format("#autoLOC_190662");
		flagBrowserGUIButton = null;
		if (GameDatabase.Instance != null)
		{
			Texture2D texture = GameDatabase.Instance.GetTexture(flagURL, asNormalMap: false);
			flagBrowserGUIButton = new FlagBrowserGUIButton(texture, OnOpenFlagBrowser, OnFlagSelect, OnFlagCancel);
			flagBrowserGUIButton.flagURL = flagURL;
		}
		else
		{
			UnityEngine.Debug.LogError("[MainMenu Error]: Flag Browser button is set, but GameDatabase has no instance!");
		}
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "newGameDialog");
		createGameDialog = CreateNewGameDialog();
		MenuNavigation.SpawnMenuNavigation(createGameDialog.gameObject, Navigation.Mode.Automatic, hasText: true, limitCheck: true);
		createGameDialog.OnDismiss = CancelNewGame;
	}

	public PopupDialog CreateNewGameDialog()
	{
		PopupDialog popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CreateNewGame", "", menuWindowTitle, guiSkinDef.SkinDef, menuWindowRect, new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUIVerticalLayout(new DialogGUIHorizontalLayout(new DialogGUILabel(Localizer.Format("#autoLOC_190689"), expandW: true), new DialogGUIFlexibleSpace(), new DialogGUITextInput(pName, Localizer.Format("#autoLOC_455855"), multiline: false, 32, delegate(string s)
		{
			pName = s;
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				ConfirmNewGame();
			}
			return pName;
		}, 200f, 30f)), new DialogGUIHorizontalLayout(new DialogGUILabel(Localizer.Format("#autoLOC_190702"), expandW: true), new DialogGUIFlexibleSpace(), new DialogGUIVerticalLayout(false, true, 0f, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIToggleGroup(new DialogGUIToggle(newGameMode == Game.Modes.SANDBOX, Localizer.Format("#autoLOC_190706"), delegate(bool b)
		{
			if (b && newGameMode != 0)
			{
				newGameMode = Game.Modes.SANDBOX;
				newGameParameters = UpdatedGameParameters(newGameParameters);
			}
		}, 200f, 30f), new DialogGUIToggle(newGameMode == Game.Modes.SCIENCE_SANDBOX, Localizer.Format("#autoLOC_190714"), delegate(bool b)
		{
			if (b && newGameMode != Game.Modes.SCIENCE_SANDBOX)
			{
				newGameMode = Game.Modes.SCIENCE_SANDBOX;
				newGameParameters = UpdatedGameParameters(newGameParameters);
			}
		}, 200f, 30f), new DialogGUIToggle(newGameMode == Game.Modes.CAREER, Localizer.Format("#autoLOC_190722"), delegate(bool b)
		{
			if (b && newGameMode != Game.Modes.CAREER)
			{
				newGameMode = Game.Modes.CAREER;
				newGameParameters = UpdatedGameParameters(newGameParameters);
			}
		}, 200f, 30f))))), new DialogGUIVerticalLayout(0f, -1f, 8f, new RectOffset(4, 0, 8, 0), TextAnchor.UpperLeft, new DialogGUILabel(Localizer.Format("#autoLOC_190734")), flagBrowserGUIButton)), new DialogGUIBox("", -1f, 100f, () => newGameMode == Game.Modes.SANDBOX, new DialogGUIHorizontalLayout(false, false, 2f, new RectOffset(8, 8, 8, 8), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(96f, 96f), Vector2.zero, Color.white, sandboxIcon), new DialogGUILabel(Localizer.Format("#autoLOC_190742") + "\n\n" + Localizer.Format("#autoLOC_190743"), guiSkinDef.SkinDef.customStyles[6], expandW: true, expandH: true))), new DialogGUIBox("", -1f, 100f, () => newGameMode == Game.Modes.SCIENCE_SANDBOX, new DialogGUIHorizontalLayout(false, false, 2f, new RectOffset(8, 8, 8, 8), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(96f, 96f), Vector2.zero, Color.white, scienceSandboxIcon), new DialogGUILabel(Localizer.Format("#autoLOC_190750") + "\n\n" + Localizer.Format("#autoLOC_190751"), guiSkinDef.SkinDef.customStyles[6], expandW: true, expandH: true))), new DialogGUIBox("", -1f, 100f, () => newGameMode == Game.Modes.CAREER, new DialogGUIHorizontalLayout(false, false, 2f, new RectOffset(8, 8, 8, 8), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(96f, 96f), Vector2.zero, Color.white, careerIcon), new DialogGUILabel(Localizer.Format("#autoLOC_190758") + "\n\n" + Localizer.Format("#autoLOC_190759"), guiSkinDef.SkinDef.customStyles[7], expandW: true, expandH: true))), new DialogGUIHorizontalLayout(false, false, 8f, new RectOffset(0, 0, 0, 0), TextAnchor.MiddleLeft, new DialogGUIButton(() => Localizer.Format("#autoLOC_190762", GameParameters.GetPresetColorHex(newGameParameters.preset), newGameParameters.preset.displayDescription()), delegate
		{
			OpenDifficultyOptions();
		}, 180f, -1f, false), new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_190768"), delegate
		{
			CancelNewGame();
		}, 80f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_190772"), delegate
		{
			ConfirmNewGame();
		}, 80f, -1f, false))), persistAcrossScenes: false, guiSkinDef.SkinDef);
		popupDialog.OnDismiss = CancelNewGame;
		return popupDialog;
	}

	public GameParameters UpdatedGameParameters(GameParameters pars)
	{
		if (pars.preset != GameParameters.Preset.Custom)
		{
			return GameParameters.GetDefaultParameters(newGameMode, pars.preset);
		}
		return GameParameters.GetDefaultParameters(newGameMode, GameParameters.Preset.Normal);
	}

	public void ConfirmNewGame()
	{
		pName = KSPUtil.SanitizeString(pName, '_', replaceEmpty: true);
		string item = pName.ToLowerInvariant();
		bool flag = invalidNames.Contains(item);
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + pName) && !Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + Localizer.Format(pName)))
		{
			if (flag)
			{
				PopupDialog popupDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("UnavailableName", Localizer.Format("#autoLOC_190801"), Localizer.Format("#autoLOC_190802"), null, new DialogGUIButton(Localizer.Format("#autoLOC_190768"), CancelOverwriteNewGame, dismissOnSelect: true)), persistAcrossScenes: false, null);
				popupDialog.OnDismiss = CancelOverwriteNewGame;
				MenuNavigation.SpawnMenuNavigation(popupDialog.gameObject, Navigation.Mode.Automatic, limitCheck: true);
			}
			else
			{
				createGameDialog.Dismiss();
				CreateAndStartGame(pName, newGameMode);
				InputLockManager.RemoveControlLock("newGameDialog");
			}
			return;
		}
		createGameDialog.gameObject.SetActive(value: false);
		if (flag)
		{
			PopupDialog popupDialog2 = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("UnavailableName", Localizer.Format("#autoLOC_190801"), Localizer.Format("#autoLOC_190802"), null, new DialogGUIButton(Localizer.Format("#autoLOC_190768"), CancelOverwriteNewGame, dismissOnSelect: true)), persistAcrossScenes: false, null);
			popupDialog2.OnDismiss = CancelOverwriteNewGame;
			MenuNavigation.SpawnMenuNavigation(popupDialog2.gameObject, Navigation.Mode.Vertical, limitCheck: true);
		}
		else
		{
			PopupDialog popupDialog3 = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ConfirmOverwrite", Localizer.Format("#autoLOC_190809"), Localizer.Format("#autoLOC_190810"), null, new DialogGUIButton(Localizer.Format("#autoLOC_190811"), OverWriteNewGame), new DialogGUIButton(Localizer.Format("#autoLOC_190812"), CancelOverwriteNewGame, dismissOnSelect: true)), persistAcrossScenes: false, null);
			popupDialog3.OnDismiss = CancelOverwriteNewGame;
			MenuNavigation.SpawnMenuNavigation(popupDialog3.gameObject, Navigation.Mode.Vertical, limitCheck: true);
		}
	}

	public void CreateAndStartGame(string name, Game.Modes mode)
	{
		CreateAndStartGame(name, mode, GameScenes.SPACECENTER, EditorFacility.None);
	}

	public void CreateAndStartGame(string name, Game.Modes mode, GameScenes startScene, EditorFacility editorFacility)
	{
		HighLogic.CurrentGame = GamePersistence.CreateNewGame(name, mode, newGameParameters, flagURL, startScene, editorFacility);
		AnalyticsUtil.LogSaveGameCreated(HighLogic.CurrentGame);
		GameEvents.onGameNewStart.Fire();
		HighLogic.CurrentGame.Start();
	}

	public void OverWriteNewGame()
	{
		try
		{
			DeleteDirectory(KSPUtil.ApplicationRootPath + "saves/" + pName);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogException(ex);
			string windowTitle = Localizer.Format("#autoLOC_190901", pName);
			string msg = Localizer.Format("#autoLOC_190902", pName, ex.Message);
			PopupDialog.SpawnPopupDialog(new MultiOptionDialog("OverWriteError", msg, windowTitle, UISkinManager.GetSkin("KSP window 7"), 450f, new DialogGUIButton(Localizer.Format("#autoLOC_190905"), delegate
			{
				CancelOverwriteNewGame();
			}, dismissOnSelect: true)), persistAcrossScenes: false, HighLogic.UISkin);
			return;
		}
		ConfirmNewGame();
	}

	public void DeleteDirectory(string targetDir)
	{
		if (Directory.Exists(targetDir))
		{
			File.SetAttributes(targetDir, FileAttributes.Normal);
			string[] files = Directory.GetFiles(targetDir);
			string[] directories = Directory.GetDirectories(targetDir);
			int num = files.Length;
			while (num-- > 0)
			{
				string path = files[num];
				File.SetAttributes(path, FileAttributes.Normal);
				File.Delete(path);
			}
			int num2 = directories.Length;
			while (num2-- > 0)
			{
				string targetDir2 = directories[num2];
				DeleteDirectory(targetDir2);
			}
			Directory.Delete(targetDir, recursive: false);
		}
	}

	public void CancelOverwriteNewGame()
	{
		if (createGameDialog != null)
		{
			createGameDialog.gameObject.SetActive(value: true);
		}
	}

	public void CancelNewGame()
	{
		InputLockManager.RemoveControlLock("newGameDialog");
	}

	public void OnOpenFlagBrowser()
	{
		createGameDialog.gameObject.SetActive(value: false);
	}

	public void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		flagURL = selected.textureInfo.name;
		createGameDialog.gameObject.SetActive(value: true);
	}

	public void OnFlagCancel()
	{
		createGameDialog.gameObject.SetActive(value: true);
	}

	public void OpenDifficultyOptions()
	{
		DifficultyOptionsMenu.Create(newGameMode, newGameParameters, newGame: true, OnDifficultyOptionsDismiss);
		createGameDialog.gameObject.SetActive(value: false);
	}

	public void OnDifficultyOptionsDismiss(GameParameters pars, bool changed)
	{
		newGameParameters = pars;
		createGameDialog.gameObject.SetActive(value: true);
	}

	public void LoadGame()
	{
		LoadGameDialog.Create(OnLoadDialogFinished, "saves", persistent: true, guiSkinDef.SkinDef);
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "loadGameDialog");
	}

	public void OnLoadDialogFinished(string save)
	{
		InputLockManager.RemoveControlLock("loadGameDialog");
		if (string.IsNullOrEmpty(save))
		{
			return;
		}
		ConfigNode configNode = GamePersistence.LoadSFSFile("persistent", save);
		if (configNode == null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_485739", save), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + save + "/Ships");
		Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + save + "/Ships/VAB");
		Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + save + "/Ships/SPH");
		KSPUpgradePipeline.Process(configNode, save, LoadContext.flag_1, delegate(ConfigNode n)
		{
			OnLoadDialogPipelineFinished(n, save);
		}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
		{
			OnLoadDialogPipelineFail(opt, n, save);
		});
	}

	public void OnLoadDialogPipelineFail(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode node, string save)
	{
		switch (opt)
		{
		case KSPUpgradePipeline.UpgradeFailOption.LoadAnyway:
			OnLoadDialogPipelineFinished(node, save);
			break;
		case KSPUpgradePipeline.UpgradeFailOption.Cancel:
			LoadGame();
			break;
		}
	}

	public void OnLoadDialogPipelineFinished(ConfigNode node, string save)
	{
		HighLogic.CurrentGame = GamePersistence.LoadGameCfg(node, save, nullIfIncompatible: true, suppressIncompatibleMessage: false);
		if (HighLogic.CurrentGame != null)
		{
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", save, SaveMode.OVERWRITE);
			GameEvents.onGameStatePostLoad.Fire(node);
			HighLogic.SaveFolder = save;
			AnalyticsUtil.LogSaveGameResumed(HighLogic.CurrentGame);
			HighLogic.CurrentGame.Start();
		}
	}

	public void StartTraining()
	{
		ScenarioLoadDialog.Create(OnScenarioLoadDismiss, ScenarioLoadDialog.ScenarioSet.Training, guiSkinDef.SkinDef);
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "loadScenarioDialog");
	}

	public void StartScenario()
	{
		ScenarioLoadDialog.Create(OnScenarioLoadDismiss, ScenarioLoadDialog.ScenarioSet.Scenarios, guiSkinDef.SkinDef);
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "loadScenarioDialog");
	}

	public void OnScenarioLoadDismiss(ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		InputLockManager.RemoveControlLock("loadScenarioDialog");
		if (save != null)
		{
			KSPUpgradePipeline.Process(ConfigNode.Load(save.LoadPath()), save.name, LoadContext.flag_1, delegate(ConfigNode n)
			{
				OnScenarioLoadPipelineFinished(n, save);
			}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
			{
				OnScenarioLoadPipelineFailed(opt, n, save);
			});
		}
	}

	public void OnScenarioLoadPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode node, ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		switch (opt)
		{
		case KSPUpgradePipeline.UpgradeFailOption.LoadAnyway:
			OnScenarioLoadPipelineFinished(node, save);
			break;
		case KSPUpgradePipeline.UpgradeFailOption.Cancel:
			StartScenario();
			break;
		}
	}

	public void OnScenarioLoadPipelineFinished(ConfigNode node, ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		HighLogic.CurrentGame = GamePersistence.LoadGameCfg(node, save.name, nullIfIncompatible: true, suppressIncompatibleMessage: false);
		if (HighLogic.CurrentGame == null)
		{
			return;
		}
		HighLogic.SaveFolder = save.savePath;
		if (!Directory.Exists(save.resumeFolderFullPath))
		{
			Directory.CreateDirectory(save.resumeFolderFullPath);
			Directory.CreateDirectory(save.resumeFolderFullPath + "/Ships");
			Directory.CreateDirectory(save.resumeFolderFullPath + "/Ships/VAB");
			Directory.CreateDirectory(save.resumeFolderFullPath + "/Ships/SPH");
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			if (node != null)
			{
				GameEvents.onGameStatePostLoad.Fire(node);
			}
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", save.savePath, SaveMode.OVERWRITE);
		}
		HighLogic.CurrentGame.Start();
	}

	public void PlayBaseMissions()
	{
		HighLogic.CurrentGame = null;
		try
		{
			MissionPlayDialog.Create(OnMissionLoadDismiss);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("[MainMenu]: Unable to open the PlayMissions Dialog");
			UnityEngine.Debug.LogException(exception);
		}
	}

	public void PlayMissions()
	{
		HighLogic.CurrentGame = null;
		try
		{
			MissionPlayDialog.Create(OnMissionLoadDismiss);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("[MainMenu]: Unable to open the PlayMissions Dialog");
			UnityEngine.Debug.LogException(exception);
		}
	}

	public void OnMissionLoadDismiss()
	{
		playMissionsNewBadge.gameObject.SetActive(!GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED);
		if (playMissionsNewBadge.gameObject.activeSelf)
		{
			playMissionsNewESABadge.gameObject.SetActive(value: false);
		}
		else
		{
			playMissionsNewESABadge.gameObject.SetActive(Versioning.version_major == 1 && Versioning.version_minor == 9);
		}
		missionBuilderNewBadge.gameObject.SetActive(!GameSettings.TUTORIALS_MISSION_BUILDER_ENTERED);
	}

	public void MissionEditor()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			MissionsUtils.OpenMissionBuilder();
		}
		else
		{
			UnityEngine.Debug.Log("MakingHistory Expansion is not installed");
		}
	}

	public void BuyExpansionPack()
	{
		Process.Start(makingHistoryExpansionURL);
	}

	public void BuyExpansionPack2()
	{
		Process.Start(serenityExpansionURL);
	}

	public void cancelStartGame()
	{
		if ((bool)envLogic)
		{
			envLogic.GoToStage(0);
		}
		ResetNavigationOnStageChange();
	}

	public void goToKSPSite()
	{
		Process.Start(KSPsiteURL);
	}

	public void goToSpacePort()
	{
		Process.Start(SpaceportURL);
	}

	public void GoToMerchandisingSite()
	{
		Process.Start(merchandiseURL);
	}

	public void quit()
	{
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "confirmQuitDialog");
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("QuitConfirmation", "", Localizer.Format("#autoLOC_191152"), guiSkinDef.SkinDef, new DialogGUIButton(Localizer.Format("#autoLOC_191154"), delegate
		{
			InputLockManager.RemoveControlLock("confirmQuitDialog");
		}), new DialogGUIButton(Localizer.Format("#autoLOC_191158"), delegate
		{
			Application.Quit();
		})), persistAcrossScenes: false, null).OnDismiss = ResetInputLockOnQuit;
	}

	public void ResetInputLockOnQuit()
	{
		InputLockManager.RemoveControlLock("confirmQuitDialog");
	}

	public void settingsKSP()
	{
		HighLogic.LoadScene(GameScenes.SETTINGS);
	}

	public void credits()
	{
		HighLogic.LoadScene(GameScenes.CREDITS);
	}

	public void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ctrls)
	{
		if (InputLockManager.IsLocking(ControlTypes.MAIN_MENU, ctrls))
		{
			lockEverything();
		}
		if (InputLockManager.IsUnlocking(ControlTypes.MAIN_MENU, ctrls))
		{
			unlockEverything();
		}
	}

	public void lockEverything()
	{
		startBtn.Lock();
		quitBtn.Lock();
		commBtn.Lock();
		spaceportBtn.Lock();
		settingBtn.Lock();
		creditsBtn.Lock();
		merchandiseBtn.Lock();
		unitTestsBtn.interactable = false;
		newGameBtn.Lock();
		continueBtn.Lock();
		trainingBtn.Lock();
		scenariosBtn.Lock();
		playESAMissionsBtn.Lock();
		playMissionsBtn.Lock();
		missionBuilderBtn.Lock();
		backBtn.Lock();
		buyMakingHistoryBtn.Lock();
		buySerenityBtn.Lock();
	}

	public void unlockEverything()
	{
		startBtn.Unlock();
		quitBtn.Unlock();
		commBtn.Unlock();
		spaceportBtn.Unlock();
		settingBtn.Unlock();
		creditsBtn.Unlock();
		merchandiseBtn.Unlock();
		unitTestsBtn.interactable = true;
		newGameBtn.Unlock();
		continueBtn.Unlock();
		trainingBtn.Unlock();
		scenariosBtn.Unlock();
		playESAMissionsBtn.Unlock();
		playMissionsBtn.Unlock();
		missionBuilderBtn.Unlock();
		backBtn.Unlock();
		buyMakingHistoryBtn.Unlock();
		buySerenityBtn.Unlock();
	}

	public void HighlightFirstItem()
	{
		for (int i = 0; i < stageOneBtn.Count; i++)
		{
			stageOneBtn[i].UnHighlight();
		}
		for (int j = 0; j < stageTwoBtn.Count; j++)
		{
			stageTwoBtn[j].UnHighlight();
		}
		switch (envLogic.currentStage)
		{
		case 1:
			stageTwoBtn[0].Highlight();
			break;
		case 0:
			stageOneBtn[0].Highlight();
			break;
		}
	}

	public void ResetNavigationOnStageChange()
	{
		lastButtonSelected = null;
		hasUsedArrowsOnce = false;
		for (int i = 0; i < stageOneBtn.Count; i++)
		{
			stageOneBtn[i].UnHighlight();
		}
		for (int j = 0; j < stageTwoBtn.Count; j++)
		{
			stageTwoBtn[j].UnHighlight();
		}
	}

	public void MenuNavHandler(MenuNavInput input)
	{
		if (!hasUsedArrowsOnce)
		{
			hasUsedArrowsOnce = true;
			HighlightFirstItem();
			return;
		}
		switch (input)
		{
		case MenuNavInput.Up:
			HighlightNextItem(-1);
			break;
		case MenuNavInput.Down:
			HighlightNextItem(1);
			break;
		case MenuNavInput.Accept:
			SelectHighlithedItem();
			break;
		case MenuNavInput.Left:
		case MenuNavInput.Right:
			break;
		}
	}

	public void HighlightNextItem(int dir)
	{
		int num = 0;
		switch (envLogic.currentStage)
		{
		case 1:
		{
			for (int j = 0; j < stageTwoBtn.Count; j++)
			{
				if (stageTwoBtn[j].isBeingHovered)
				{
					num = j;
					break;
				}
			}
			stageTwoBtn[num].UnHighlight();
			num += dir;
			if (num < 0)
			{
				num = 0;
			}
			else if (num > stageTwoBtn.Count - 1)
			{
				num = stageTwoBtn.Count - 1;
			}
			stageTwoBtn[num].Highlight();
			lastButtonSelected = stageTwoBtn[num];
			break;
		}
		case 0:
		{
			for (int i = 0; i < stageOneBtn.Count; i++)
			{
				if (stageOneBtn[i].isBeingHovered)
				{
					num = i;
					break;
				}
			}
			stageOneBtn[num].UnHighlight();
			num += dir;
			if (num < 0)
			{
				num = 0;
			}
			else if (num > stageOneBtn.Count - 1)
			{
				num = stageOneBtn.Count - 1;
			}
			stageOneBtn[num].Highlight();
			lastButtonSelected = stageOneBtn[num];
			break;
		}
		}
	}

	public void SelectHighlithedItem()
	{
		switch (envLogic.currentStage)
		{
		case 1:
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < stageTwoBtn.Count)
				{
					if (stageTwoBtn[num2].isBeingHovered && !stageTwoBtn[num2].IsButtonLocked())
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			stageTwoBtn[num2].onTap();
			stageTwoBtn[num2].KeyStrokeLock();
			break;
		}
		case 0:
		{
			int num = 0;
			while (true)
			{
				if (num < stageOneBtn.Count)
				{
					if (stageOneBtn[num].isBeingHovered && !stageOneBtn[num].IsButtonLocked())
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			stageOneBtn[num].onTap();
			stageOneBtn[num].KeyStrokeLock();
			break;
		}
		}
	}

	public void MouseIsHovering(bool b)
	{
		bool flag = (bool)lastButtonSelected && lastButtonSelected.isActiveAndEnabled;
		if (b)
		{
			if (flag)
			{
				lastButtonSelected.UnHighlight();
			}
		}
		else if (flag)
		{
			lastButtonSelected.Highlight();
		}
	}
}
