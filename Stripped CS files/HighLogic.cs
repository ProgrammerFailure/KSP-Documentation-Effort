using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using Expansions.Missions.Runtime;
using FinePrint.Utilities;
using ns16;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighLogic : MonoBehaviour
{
	public bool showConsole = true;

	public bool showConsoleOnError;

	public string skinName = "KSP window 7";

	public GUISkin skin;

	public bool joystickUsed;

	public string[] joystickNames;

	public bool joySticksPresent;

	public static double gameRunTimeAdditive;

	[SerializeField]
	public float gameRunTimeFrameLimit = 10f;

	public UISkinDefSO uiSkinDefAsset;

	public UISkinDef uiskin;

	public static HighLogic fetch;

	public string GameSaveFolder = "default";

	public Game currentGame;

	public SceneTransitionMatrix sceneBufferTransitionMatrix;

	public static double TimeSceneLoaded;

	public static bool FastEditorLoading;

	public static bool LoadedSceneIsEditor;

	public static bool LoadedSceneIsFlight;

	public static bool LoadedSceneHasPlanetarium;

	public static bool LoadedSceneIsGame;

	public static bool LoadedSceneIsMissionBuilder;

	public static GameScenes LoadedScene;

	public static GUISkin Skin => fetch.skin;

	public static UISkinDef UISkin
	{
		get
		{
			if (fetch.uiskin == null)
			{
				fetch.uiskin = fetch.uiSkinDefAsset.SkinDef;
			}
			return fetch.uiskin;
		}
	}

	public static string SaveFolder
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.GameSaveFolder;
		}
		set
		{
			if ((bool)fetch)
			{
				fetch.GameSaveFolder = value;
			}
		}
	}

	public static Game CurrentGame
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.currentGame;
		}
		set
		{
			if ((bool)fetch)
			{
				fetch.currentGame = value;
			}
		}
	}

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		fetch = this;
		gameRunTimeAdditive = 0.0;
		if (!GameSettings.Ready)
		{
			base.gameObject.AddComponent<GameSettings>();
		}
		Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
		SceneManager.sceneLoaded += OnSceneLoaded;
		AnalyticsUtil.Initialize();
	}

	public void Start()
	{
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		if (!(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
		{
			SpaceNavigator.SetTranslationSensitivity(100f);
			SpaceNavigator.SetRotationSensitivity(100f);
		}
		StringUtilities.LoadSiteGenerationInfo();
		joystickNames = Input.GetJoystickNames();
		if (joystickNames == null || joystickNames.Length == 0)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < joystickNames.Length)
			{
				if (joystickNames[num] != "")
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		joySticksPresent = true;
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Update()
	{
		if (joySticksPresent && !joystickUsed && (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.JoystickButton3) || Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.JoystickButton5) || Input.GetKey(KeyCode.JoystickButton6) || Input.GetKey(KeyCode.JoystickButton7) || Input.GetKey(KeyCode.JoystickButton8) || Input.GetKey(KeyCode.JoystickButton9) || Input.GetKey(KeyCode.JoystickButton10) || Input.GetKey(KeyCode.JoystickButton11)))
		{
			for (int i = 1; i <= joystickNames.Length; i++)
			{
				for (int j = 0; j <= 11; j++)
				{
					if (Input.GetKey("joystick " + i + " button " + j))
					{
						Debug.Log("[HighLogic]: " + joystickNames[i - 1] + " pressed a button.");
						joystickUsed = true;
						AnalyticsUtil.LogJoystickUsage(joystickNames[i - 1]);
					}
				}
			}
		}
		if (Time.unscaledDeltaTime < gameRunTimeFrameLimit)
		{
			gameRunTimeAdditive += Time.unscaledDeltaTime;
		}
	}

	[ContextMenu("Debug Current Game")]
	public void printCurrentGame()
	{
		Debug.Log(string.Concat("Current Game: ", currentGame, ", ", currentGame.Title));
	}

	public void OnApplicationFocus(bool focus)
	{
		GameEvents.OnAppFocus.Fire(focus);
		if (!focus)
		{
			GameEvents.onTooltipDestroyRequested.Fire();
		}
	}

	public static void LoadScene(GameScenes scene)
	{
		if (LoadedSceneIsGame)
		{
			if (scene == GameScenes.MAINMENU && PSystemSetup.Instance != null)
			{
				PSystemSetup.Instance.RemoveNonStockLaunchSites();
			}
			if (scene == GameScenes.MAINMENU)
			{
				MissionSystem.RemoveMissionObjects(removeAll: true);
				if (CurrentGame != null && CurrentGame.missionToStart != null)
				{
					MissionSystem.RemoveMissonObject(CurrentGame.missionToStart.MissionInfo);
				}
			}
		}
		bool transitionValue = fetch.sceneBufferTransitionMatrix.GetTransitionValue(LoadedScene, scene);
		SetLoadSceneEventsAndFlags(scene, transitionValue);
		if (transitionValue)
		{
			fetch.StartCoroutine(bufferedLoad((int)scene, loadAsync: true));
		}
		else
		{
			SceneManager.LoadScene((int)scene);
		}
	}

	public static void LoadSceneFromBundle(GameScenes scene, string sceneName)
	{
		SetLoadSceneEventsAndFlags(scene, useAsyncBufferedLoad: true);
		if (LoadedSceneIsGame && scene == GameScenes.MAINMENU && PSystemSetup.Instance != null)
		{
			PSystemSetup.Instance.RemoveNonStockLaunchSites();
		}
		if (scene == GameScenes.MAINMENU)
		{
			MissionSystem.RemoveMissionObjects(removeAll: true);
			if (CurrentGame != null && CurrentGame.missionToStart != null)
			{
				MissionSystem.RemoveMissonObject(CurrentGame.missionToStart.MissionInfo);
			}
		}
		SceneManager.LoadSceneAsync(sceneName);
	}

	public static void SetLoadSceneEventsAndFlags(GameScenes scene, bool useAsyncBufferedLoad)
	{
		GameEvents.onGameSceneLoadRequested.Fire(scene);
		GameEvents.onGameSceneSwitchRequested.Fire(new GameEvents.FromToAction<GameScenes, GameScenes>(LoadedScene, scene));
		Debug.Log(string.Concat("[HighLogic]: =========================== Scene Change : From ", LoadedScene, " to ", scene, useAsyncBufferedLoad ? " (Async) " : " ", "====================="));
		setLevelFlags(scene);
	}

	public static IEnumerator bufferedLoad(int sceneToBeLoaded, bool loadAsync)
	{
		Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.High;
		if (loadAsync)
		{
			SceneManager.LoadScene("loadingBuffer", LoadSceneMode.Single);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			AsyncOperation asyncLoadNext = SceneManager.LoadSceneAsync(sceneToBeLoaded, LoadSceneMode.Single);
			asyncLoadNext.allowSceneActivation = false;
			while (!asyncLoadNext.isDone)
			{
				if (asyncLoadNext.progress >= 0.9f)
				{
					yield return new WaitForEndOfFrame();
					asyncLoadNext.allowSceneActivation = true;
				}
				else
				{
					yield return null;
				}
			}
		}
		else
		{
			SceneManager.LoadScene("loadingBuffer", LoadSceneMode.Single);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			SceneManager.LoadScene(sceneToBeLoaded, LoadSceneMode.Single);
			yield return new WaitForSeconds(0.1f);
		}
		Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.Normal;
	}

	public static void setLevelFlags(GameScenes scene)
	{
		LoadedSceneIsEditor = scene == GameScenes.EDITOR;
		LoadedSceneHasPlanetarium = scene == GameScenes.FLIGHT || scene == GameScenes.TRACKSTATION || scene == GameScenes.SPACECENTER;
		LoadedSceneIsFlight = scene == GameScenes.FLIGHT;
		LoadedSceneIsGame = scene == GameScenes.SPACECENTER || scene == GameScenes.EDITOR || scene == GameScenes.FLIGHT || scene == GameScenes.TRACKSTATION;
		LoadedSceneIsMissionBuilder = scene == GameScenes.MISSIONBUILDER;
		LoadedScene = scene;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode != LoadSceneMode.Additive)
		{
			OnLevelLoaded(GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
		}
	}

	public void OnLevelLoaded(GameScenes level)
	{
		if (level != GameScenes.LOADINGBUFFER)
		{
			StartCoroutine(FireLoadedEvent(level));
			StartCoroutine(FireLoadedEventGUIReady(level));
		}
	}

	public IEnumerator FireLoadedEvent(GameScenes scene)
	{
		if (!(Planetarium.fetch != null) && CurrentGame == null)
		{
			TimeSceneLoaded = Time.time;
		}
		else
		{
			TimeSceneLoaded = Planetarium.GetUniversalTime();
		}
		yield return null;
		GameEvents.onLevelWasLoaded.Fire(scene);
	}

	public IEnumerator FireLoadedEventGUIReady(GameScenes scene)
	{
		yield return null;
		GameEvents.onLevelWasLoadedGUIReady.Fire(scene);
	}

	public static GameScenes GetLoadedGameSceneFromBuildIndex(int loadedSceneBuildIndex)
	{
		if (loadedSceneBuildIndex == -1)
		{
			return LoadedScene;
		}
		return (GameScenes)loadedSceneBuildIndex;
	}
}
