using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ns11;
using ns9;
using UnityEngine;

public class FlightDriver : MonoBehaviour
{
	public enum StartupBehaviours
	{
		NEW_FROM_FILE,
		RESUME_SAVED_FILE,
		RESUME_SAVED_CACHE,
		NEW_FROM_CRAFT_NODE
	}

	public enum TimeInMode
	{
		Flight,
		Map,
		External,
		const_3,
		Internal,
		const_5,
		EVAConstruction
	}

	public bool canRun = true;

	public StartupBehaviours inEditorStartUpMode;

	public int targetVessel;

	public static string newShipToLoadPath = "";

	public static StartupBehaviours StartupBehaviour = StartupBehaviours.NEW_FROM_FILE;

	public static bool resumeCacheUsed;

	public static string newShipFlagURL = "";

	public static VesselCrewManifest newShipManifest;

	public static string StateFileToLoad = "persistent";

	public static Game FlightStateCache;

	public static int FocusVesselAfterLoad;

	public static string LaunchSiteName = "LaunchPad";

	public static bool flightStarted;

	public bool bypassPersistence;

	public bool DEBUG_PauseAfterStart;

	public static bool CanRevertToPostInit;

	public static bool CanRevertToPrelaunch;

	public static GameBackup PostInitState;

	public static GameBackup PreLaunchState;

	public static FlightDriver fetch;

	public bool bypassLoadingEnforce;

	public int framesBeforeInitialSave = 15;

	public int framesAtStartup;

	public ShipConstruct newVessel;

	public string uiSkinName = "KSP window 1";

	public UISkinDef uiskin;

	public static bool pause;

	public double startTime;

	public double modeStartTime;

	public Dictionary<string, double> timeInMode;

	public TimeInMode currentMode;

	public static bool BypassPersistence => fetch.bypassPersistence;

	public static bool CanRevert
	{
		get
		{
			if (!CanRevertToPrelaunch)
			{
				return CanRevertToPostInit;
			}
			return true;
		}
	}

	public static bool Pause => pause;

	public void Awake()
	{
		fetch = this;
		flightStarted = false;
		framesAtStartup = Time.frameCount;
		if (!bypassLoadingEnforce && PartLoader.LoadedPartsList == null)
		{
			Debug.LogError("[Flight Driver]: Game does not seem to be loaded.");
			canRun = false;
		}
	}

	public void Start()
	{
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Show();
		}
		uiskin = UISkinManager.GetSkin("uiSkinName");
		flightStarted = false;
		if (!canRun)
		{
			return;
		}
		MonoBehaviour.print("------------------- initializing flight mode... ------------------");
		InputLockManager.RemoveControlLock("flightDriver_ApplicationFocus");
		pause = false;
		Time.timeScale = 1f;
		if (StartupBehaviour != StartupBehaviours.RESUME_SAVED_CACHE && !bypassPersistence)
		{
			FlightStateCache = GamePersistence.LoadGame(StateFileToLoad, HighLogic.SaveFolder, nullIfIncompatible: true, suppressIncompatibleMessage: true);
			if (FlightStateCache == null)
			{
				bypassPersistence = true;
				if (StartupBehaviour == StartupBehaviours.RESUME_SAVED_FILE)
				{
					StartupBehaviour = StartupBehaviours.NEW_FROM_FILE;
				}
			}
			else if (!FlightStateCache.compatible)
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Load Failed", Localizer.Format("#autoLOC_6002422"), Localizer.Format("#autoLOC_6002423", StateFileToLoad), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, uiskin);
				HighLogic.LoadScene(GameScenes.SPACECENTER);
				return;
			}
		}
		if (FlightStateCache.flightState == null)
		{
			FlightStateCache.flightState = new FlightState();
		}
		if (!bypassPersistence)
		{
			if (StartupBehaviour == StartupBehaviours.NEW_FROM_FILE)
			{
				PreLaunchState = new GameBackup(FlightStateCache);
				List<ProtoVessel> list = ShipConstruction.FindVesselsLandedAt(FlightStateCache.flightState, LaunchSiteName);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					ShipConstruction.RecoverVesselFromFlight(list[i], FlightStateCache.flightState);
				}
			}
			if ((bool)Planetarium.fetch)
			{
				Planetarium.SetUniversalTime(FlightStateCache.UniversalTime);
				Planetarium.fetch.UpdateCBs();
			}
			FlightStateCache.Load();
		}
		switch (StartupBehaviour)
		{
		case StartupBehaviours.NEW_FROM_FILE:
			if (!File.Exists(newShipToLoadPath))
			{
				break;
			}
			MonoBehaviour.print("Loading ship from file: " + newShipToLoadPath);
			setStartupNewVessel();
			goto IL_0434;
		case StartupBehaviours.RESUME_SAVED_FILE:
		case StartupBehaviours.RESUME_SAVED_CACHE:
		{
			FocusVesselAfterLoad = Mathf.Clamp(FocusVesselAfterLoad, 0, FlightGlobals.Vessels.Count - 1);
			if (FlightGlobals.Vessels.Count == 0)
			{
				Debug.LogWarning("Flight State contains no vessels. Returning to KSC");
				HighLogic.LoadScene(GameScenes.SPACECENTER);
				return;
			}
			Debug.Log("Target vessel index: " + FocusVesselAfterLoad + "  vessel count: " + FlightGlobals.Vessels.Count);
			Vessel vessel = FlightGlobals.Vessels[FocusVesselAfterLoad];
			CanRevertToPostInit = vessel.situation == Vessel.Situations.PRELAUNCH;
			if (CanRevertToPostInit && PostInitState != null && FlightStateCache.flightState.protoVessels[FocusVesselAfterLoad].vesselID == PostInitState.ActiveVesselID && ShipConstruction.ShipConfig != null && PreLaunchState != null)
			{
				CanRevertToPrelaunch = true;
			}
			else
			{
				CanRevertToPrelaunch = false;
				if (ShipConstruction.ShipConfig != null)
				{
					ShipConstruction.ShipConfig = null;
				}
			}
			for (int j = 0; j < FlightGlobals.Vessels.Count; j++)
			{
				Vessel vessel2 = FlightGlobals.Vessels[j];
				if (vessel2.LandedOrSplashed)
				{
					vessel2.SetPosition(vessel2.mainBody.GetWorldSurfacePosition(vessel2.latitude, vessel2.longitude, vessel2.altitude));
					continue;
				}
				vessel2.orbitDriver.orbit.Init();
				vessel2.orbitDriver.updateFromParameters(setPosition: true);
			}
			FloatingOrigin.SetOffset(vessel.transform.position);
			FlightGlobals.SetActiveVessel(vessel);
			if (vessel.mainBody.pqsController != null)
			{
				FlightCamera.fetch.transform.position = vessel.transform.position;
				vessel.mainBody.pqsController.SetTarget(vessel.transform);
				PSystemSetup.Instance.SetPQSActive(vessel.mainBody.pqsController);
			}
			else
			{
				PSystemSetup.Instance.SetPQSActive();
			}
			if (StartupBehaviour == StartupBehaviours.RESUME_SAVED_CACHE)
			{
				resumeCacheUsed = true;
			}
			goto IL_0434;
		}
		case StartupBehaviours.NEW_FROM_CRAFT_NODE:
			{
				if (ShipConstruction.ShipConfig == null)
				{
					break;
				}
				MonoBehaviour.print("Loading ship from config");
				setStartupNewVessel();
				goto IL_0434;
			}
			IL_0434:
			MonoBehaviour.print("all systems started");
			flightStarted = true;
			if (DEBUG_PauseAfterStart)
			{
				Debug.Break();
			}
			StartCoroutine(PostInit());
			if (FlightGlobals.ActiveVessel != null && !FlightGlobals.ActiveVessel.Landed && (bool)LoadingBufferMask.Instance)
			{
				LoadingBufferMask.Instance.Hide();
			}
			InitAnalytics();
			return;
		}
		MonoBehaviour.print("No ship in ShipAssembly or no such filename exists! cannot start flight mode!");
		MonoBehaviour.print("try building a ship first, that could help somewhat");
		canRun = false;
		HighLogic.LoadScene(GameScenes.SPACECENTER);
	}

	public void setStartupNewVessel()
	{
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Show();
		}
		PSystemSetup.SpaceCenterFacility spaceCenterFacility = null;
		LaunchSite launchSite = null;
		LaunchSite.SpawnPoint spawnPoint = null;
		PSystemSetup.SpaceCenterFacility.SpawnPoint spawnPoint2 = null;
		Transform transform = null;
		string displaylandedAt = "";
		newVessel = ShipConstruction.LoadShip(newShipToLoadPath);
		if (newVessel == null)
		{
			HighLogic.LoadScene(GameScenes.SPACECENTER);
			return;
		}
		ShipConstruction.ShipManifest = newShipManifest;
		try
		{
			spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(LaunchSiteName);
			if (spaceCenterFacility != null)
			{
				spawnPoint2 = spaceCenterFacility.GetSpawnPoint(LaunchSiteName);
				if (spawnPoint2 != null)
				{
					FloatingOrigin.SetOffset(spawnPoint2.GetSpawnPointTransform().position);
					displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(spawnPoint2.GetSpawnPointTransform().gameObject.tag, formatted: false);
					spaceCenterFacility.facilityPQS.SetTarget(spawnPoint2.GetSpawnPointTransform());
					PSystemSetup.Instance.SetPQSActive(spaceCenterFacility.facilityPQS);
					FlightCamera.fetch.transform.position = spawnPoint2.GetSpawnPointTransform().position;
					newVessel.shipFacility = spaceCenterFacility.GetFacility().GetEditorFacility().ToEditor();
					ShipConstruction.CreateBackup(newVessel);
					transform = spawnPoint2.GetSpawnPointTransform();
					displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(spawnPoint2.GetSpawnPointTransform().gameObject.tag, formatted: false);
				}
			}
			else
			{
				launchSite = PSystemSetup.Instance.GetLaunchSite(LaunchSiteName);
				if (launchSite != null)
				{
					spawnPoint = launchSite.GetSpawnPoint(LaunchSiteName);
					if (spawnPoint != null)
					{
						if (launchSite.isPQSCity)
						{
							launchSite.pqsCity.celestialBody.CBUpdate();
							FloatingOrigin.SetOffset(spawnPoint.GetSpawnPointTransform().position);
							launchSite.launchsitePQS.SetTarget(spawnPoint.GetSpawnPointTransform());
							PSystemSetup.Instance.SetPQSActive(launchSite.launchsitePQS);
							launchSite.pqsCity.Orientate();
						}
						if (launchSite.isPQSCity2)
						{
							launchSite.pqsCity2.celestialBody.CBUpdate();
							FloatingOrigin.SetOffset(spawnPoint.GetSpawnPointTransform().position);
							launchSite.launchsitePQS.SetTarget(spawnPoint.GetSpawnPointTransform());
							PSystemSetup.Instance.SetPQSActive(launchSite.launchsitePQS);
							if (!launchSite.pqsCity2.PositioningCompleted)
							{
								if (launchSite.positionMobileLaunchPad != null)
								{
									launchSite.positionMobileLaunchPad.OnScenerySettingChanged();
								}
								launchSite.pqsCity2.Reset();
								launchSite.pqsCity2.Orientate();
							}
						}
						displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(spawnPoint.GetSpawnPointTransform().gameObject.tag, formatted: false);
						FlightCamera.fetch.transform.position = spawnPoint.GetSpawnPointTransform().position;
						newVessel.shipFacility = launchSite.editorFacility;
						ShipConstruction.CreateBackup(newVessel);
						transform = spawnPoint.GetSpawnPointTransform();
						displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(spawnPoint.GetSpawnPointTransform().gameObject.tag, formatted: false);
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		if (transform == null)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LaunchFailed", "Launch of Vessel " + newVessel.shipName + " failed.", "Launch Failed", HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), delegate
			{
			})), persistAcrossScenes: false, HighLogic.UISkin);
			HighLogic.LoadScene(GameScenes.SPACECENTER);
			return;
		}
		if (FlightGlobals.ready)
		{
			FloatingOrigin.SetOffset(transform.position);
		}
		ShipConstruction.PutShipToGround(newVessel, transform);
		ShipConstruction.AssembleForLaunch(newVessel, LaunchSiteName, displaylandedAt, newShipFlagURL, FlightStateCache, newShipManifest);
		PSystemSetup.Instance.SetPQSActive();
		StageManager.BeginFlight();
		CanRevertToPostInit = true;
		CanRevertToPrelaunch = true;
	}

	public IEnumerator PostInit()
	{
		while (Time.frameCount - framesAtStartup < framesBeforeInitialSave)
		{
			yield return null;
		}
		if (StartupBehaviour == StartupBehaviours.NEW_FROM_FILE)
		{
			GameEvents.OnVesselRollout.Fire(newVessel);
		}
		PostInitState = new GameBackup(HighLogic.CurrentGame.Updated());
		if (FlightGlobals.ClearToSave() == ClearToSaveStatus.CLEAR)
		{
			GamePersistence.SaveGame(PostInitState, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		}
		GameEvents.onFlightReady.Fire();
	}

	[ContextMenu("Focus Target Vessel")]
	public void FocusTargetVessel()
	{
		if (FlightGlobals.SetActiveVessel(FlightGlobals.Vessels[targetVessel]))
		{
			FlightInputHandler.SetNeutralControls();
		}
	}

	public void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			InputLockManager.SetControlLock("flightDriver_ApplicationFocus");
		}
		else
		{
			InputLockManager.RemoveControlLock("flightDriver_ApplicationFocus");
		}
	}

	public static void SetPause(bool pauseState, bool postScreenMessage = true)
	{
		GameEvents.onShowUI.Fire();
		if (pauseState)
		{
			pause = true;
			MonoBehaviour.print("Game Paused!");
			GameEvents.onGamePause.Fire();
			Time.timeScale = 0f;
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.PAUSE), "gamePause");
			if ((bool)LoadingBufferMask.Instance)
			{
				LoadingBufferMask.Instance.Hide();
			}
		}
		else
		{
			pause = false;
			MonoBehaviour.print("Game Unpaused!");
			GameEvents.onGameUnpause.Fire();
			TimeWarp.SetRate(TimeWarp.CurrentRateIndex, instant: true, postScreenMessage);
			InputLockManager.RemoveControlLock("gamePause");
		}
	}

	public void OnDestroy()
	{
		if (pause)
		{
			InputLockManager.RemoveControlLock("gamePause");
		}
		InputLockManager.RemoveControlLock("flightDriver_ApplicationFocus");
		Time.timeScale = 1f;
		pause = false;
		if (StartupBehaviour == StartupBehaviours.RESUME_SAVED_CACHE && resumeCacheUsed)
		{
			FlightStateCache = null;
			resumeCacheUsed = false;
		}
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
		FinalizeAnalytics();
	}

	public static void StartWithNewLaunch(string fullFilePath, string missionFlagURL, string launchSiteName, VesselCrewManifest manifest)
	{
		Debug.Log("Launching vessel from " + launchSiteName + ". Craft file: " + fullFilePath);
		FlightGlobals.ClearpersistentIdDictionaries();
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		StartupBehaviour = StartupBehaviours.NEW_FROM_FILE;
		newShipToLoadPath = fullFilePath;
		StateFileToLoad = "persistent";
		newShipFlagURL = missionFlagURL;
		newShipManifest = manifest;
		LaunchSiteName = launchSiteName;
		HighLogic.LoadScene(GameScenes.FLIGHT);
	}

	public static void RevertToLaunch()
	{
		FlightGlobals.ClearpersistentIdDictionaries();
		StartupBehaviour = StartupBehaviours.RESUME_SAVED_CACHE;
		GameParameters parameters = FlightStateCache.Parameters;
		FlightStateCache = new Game(PostInitState.Config);
		FlightStateCache.Parameters = parameters;
		resumeCacheUsed = false;
		FocusVesselAfterLoad = PostInitState.ActiveVessel;
		GameEvents.onGameStatePostLoad.Fire(FlightStateCache.config);
		GameEvents.OnRevertToLaunchFlightState.Fire(HighLogic.CurrentGame.flightState);
		HighLogic.LoadScene(GameScenes.FLIGHT);
	}

	public static void RevertToPrelaunch(EditorFacility facility)
	{
		Debug.Log("[FlightDriver]: Flight State Reverted to Prelaunch.");
		FlightGlobals.ClearpersistentIdDictionaries();
		EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
		GamePersistence.SaveGame(PreLaunchState, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		GameEvents.onGameStatePostLoad.Fire(FlightStateCache.config);
		GameEvents.OnRevertToPrelaunchFlightState.Fire(HighLogic.CurrentGame.flightState);
		EditorDriver.StartEditor(facility);
	}

	public static void ReturnToEditor(EditorFacility facility)
	{
		Debug.Log("[FlightDriver]: Returning to Editor.");
		FlightGlobals.ClearpersistentIdDictionaries();
		EditorDriver.StartupBehaviour = EditorDriver.StartupBehaviours.LOAD_FROM_CACHE;
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.EDITOR);
		EditorDriver.StartEditor(facility);
	}

	public static void StartAndFocusVessel(string stateFileToLoad, int vesselToFocusIdx)
	{
		StartupBehaviour = StartupBehaviours.RESUME_SAVED_FILE;
		StateFileToLoad = stateFileToLoad;
		FocusVesselAfterLoad = vesselToFocusIdx;
		HighLogic.LoadScene(GameScenes.FLIGHT);
	}

	public static void StartAndFocusVessel(Game stateToLoad, int vesselToFocusIdx)
	{
		FlightStateCache = stateToLoad;
		resumeCacheUsed = false;
		FocusVesselAfterLoad = vesselToFocusIdx;
		StartupBehaviour = StartupBehaviours.RESUME_SAVED_CACHE;
		HighLogic.LoadScene(GameScenes.FLIGHT);
	}

	public static Game RemoveSavedVessel(Game original, int vesselIndex)
	{
		if (original.flightState.protoVessels[vesselIndex] == null)
		{
			throw new ArgumentException("Flight Driver Error: Cannot Generate Prelaunch State from given FlightState, as it does not contain a vessel at index " + vesselIndex);
		}
		List<ProtoCrewMember> vesselCrew = original.flightState.protoVessels[vesselIndex].GetVesselCrew();
		int i = 0;
		for (int count = vesselCrew.Count; i < count; i++)
		{
			vesselCrew[i].rosterStatus = ProtoCrewMember.RosterStatus.Available;
		}
		Game cloneOf = Game.GetCloneOf(original);
		cloneOf.flightState.protoVessels.RemoveAt(vesselIndex);
		return cloneOf;
	}

	public void InitAnalytics()
	{
		timeInMode = new Dictionary<string, double>();
		startTime = HighLogic.gameRunTimeAdditive;
		modeStartTime = HighLogic.gameRunTimeAdditive;
		currentMode = TimeInMode.Flight;
		GameEvents.OnCameraChange.Add(OnCameraChange);
		GameEvents.OnEVAConstructionMode.Add(OnConstructionModeChange);
		GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
		GameEvents.onCrewOnEva.Add(OnCrewOnEva);
	}

	public void FinalizeAnalytics()
	{
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		GameEvents.OnEVAConstructionMode.Remove(OnConstructionModeChange);
		GameEvents.onCrewBoardVessel.Remove(OnCrewBoardVessel);
		GameEvents.onCrewOnEva.Remove(OnCrewOnEva);
		string key = currentMode.ToString();
		if (!timeInMode.ContainsKey(key))
		{
			timeInMode.Add(key, 0.0);
		}
		timeInMode[key] += HighLogic.gameRunTimeAdditive - modeStartTime;
		AnalyticsUtil.LogExitFlight(HighLogic.gameRunTimeAdditive - startTime, timeInMode);
	}

	public void OnModeChange(TimeInMode mode)
	{
		string key = currentMode.ToString();
		if (!timeInMode.ContainsKey(key))
		{
			timeInMode.Add(key, 0.0);
		}
		timeInMode[key] += HighLogic.gameRunTimeAdditive - modeStartTime;
		modeStartTime = HighLogic.gameRunTimeAdditive;
		currentMode = mode;
	}

	public void OnCameraChange(CameraManager.CameraMode mode)
	{
		OnModeChange(mode switch
		{
			CameraManager.CameraMode.Map => TimeInMode.Map, 
			CameraManager.CameraMode.External => TimeInMode.External, 
			CameraManager.CameraMode.const_3 => TimeInMode.const_3, 
			CameraManager.CameraMode.Internal => TimeInMode.Internal, 
			_ => TimeInMode.Flight, 
		});
	}

	public void OnConstructionModeChange(bool open)
	{
		if (!open && CameraManager.Instance != null)
		{
			OnCameraChange(CameraManager.Instance.currentCameraMode);
		}
		else if (!(EVAConstructionModeController.Instance == null) && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction)
		{
			OnModeChange(TimeInMode.EVAConstruction);
		}
	}

	public void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		if (CameraManager.Instance != null)
		{
			OnCameraChange(CameraManager.Instance.currentCameraMode);
		}
	}

	public void OnCrewOnEva(GameEvents.FromToAction<Part, Part> fv)
	{
		OnModeChange(TimeInMode.const_5);
	}
}
