using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using ModuleWheels;
using ns11;
using ns9;
using UnityEngine;
using Upgradeables;

namespace Expansions.Missions.Runtime;

[KSPScenario(ScenarioCreationOptions.AddToAllMissionGames, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.MAINMENU,
	GameScenes.MISSIONBUILDER
})]
public class MissionSystem : ScenarioModule
{
	public static List<Mission> missions;

	public static MissionScoreInfo missionScoreInfo;

	[NonSerialized]
	public static Awards awardDefinitions;

	public static MissionFileInfo missionToEdit;

	public static GameObject missionsGameObject;

	public MissionCameraModeOptions _cameraLockMode;

	public MissionCameraLockOptions _cameraLockOptions = MissionCameraLockOptions.Unlock;

	public static bool _setupFlightState;

	public Vessel returnVessel;

	public string errorString = "";

	public ConfigNode vesselConfigNode;

	public static Mission newMission;

	public static MissionSystem Instance { get; set; }

	public static bool IsActive { get; set; }

	public static bool IsTestMode { get; set; }

	public static bool HasMissions
	{
		get
		{
			if (missions != null)
			{
				return missions.Count > 0;
			}
			return false;
		}
	}

	public static GameObject MissionsGameObject
	{
		get
		{
			if (missionsGameObject == null)
			{
				missionsGameObject = new GameObject("MissionsGameObject");
				UnityEngine.Object.DontDestroyOnLoad(missionsGameObject);
			}
			return missionsGameObject;
		}
	}

	public MissionCameraModeOptions CameraLockMode => _cameraLockMode;

	public MissionCameraLockOptions CameraLockOptions => _cameraLockOptions;

	public override void OnAwake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		IsActive = false;
		awardDefinitions = UnityEngine.Object.FindObjectOfType<Awards>();
		if (awardDefinitions == null)
		{
			GameObject gameObject = MissionsUtils.MEPrefab("Prefabs/MEAwards.prefab");
			if (gameObject != null)
			{
				awardDefinitions = UnityEngine.Object.Instantiate(gameObject).GetComponent<Awards>();
			}
		}
		missions = new List<Mission>();
		missionScoreInfo = MissionScoreInfo.LoadScores();
		GameEvents.Mission.onStarted.Add(onMissionStarted);
		GameEvents.Mission.onFinished.Add(onMissionEnded);
		GameEvents.Mission.onActiveNodeChanged.Add(onMissionNodeChanged);
		GameEvents.onVesselLoaded.Add(onVesselLoaded);
		GameEvents.onGameSceneLoadRequested.Add(onGameSceneLoadRequested);
		RemoveMissionObjects();
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.missionToStart != null)
		{
			if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", HighLogic.CurrentGame.missionToStart.HashBytes, HighLogic.CurrentGame.missionToStart.Signature))
			{
				Debug.LogError("[MissionSystem]: Unable to add mission to missions list: " + HighLogic.CurrentGame.missionToStart.title + " Making History DLC is not installed, or mission is not an allowed stock mission.");
				UnityEngine.Object.Destroy(Instance);
				HighLogic.CurrentGame.missionToStart = null;
				return;
			}
			HighLogic.CurrentGame.missionToStart.InitMission();
			missions.Add(HighLogic.CurrentGame.missionToStart);
			HighLogic.CurrentGame.missionToStart = null;
		}
		if (HighLogic.LoadedSceneIsEditor && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER && missionToEdit != null)
		{
			AddMission(missionToEdit);
			missionToEdit = null;
		}
	}

	public void Update()
	{
		if (IsActive && Instance != null && HighLogic.LoadedSceneHasPlanetarium)
		{
			UpdateMissions();
		}
	}

	public void OnDestroy()
	{
		if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			for (int i = 0; i < missions.Count; i++)
			{
				missions[i].situation.DestroySituation();
			}
		}
		RemoveMissionObjects(removeAll: true);
		GameEvents.Mission.onStarted.Remove(onMissionStarted);
		GameEvents.Mission.onFinished.Remove(onMissionEnded);
		GameEvents.Mission.onActiveNodeChanged.Remove(onMissionNodeChanged);
		GameEvents.onVesselLoaded.Remove(onVesselLoaded);
		GameEvents.onGameSceneLoadRequested.Remove(onGameSceneLoadRequested);
	}

	public bool AddMission(MissionFileInfo missionInfo)
	{
		try
		{
			Mission mission = LoadMission(missionInfo, loadSavedMission: false, initMission: true);
			if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", mission.HashBytes, mission.Signature))
			{
				Debug.LogError("[MissionSystem]: Unable to add mission to misions list: " + missionInfo.folderName + " Making History DLC is not installed, or mission is not an allowed stock mission.");
				return false;
			}
			missions.Add(mission);
			return true;
		}
		catch (Exception)
		{
			Debug.LogError("[MissionSystem]: Unable to add mission to misions list: " + missionInfo.folderName);
			return false;
		}
	}

	public static Mission LoadMission(MissionFileInfo missionInfo, bool loadSavedMission = false, bool initMission = false, bool simple = false)
	{
		RemoveMissonObject(missionInfo);
		Mission component = Mission.Spawn(missionInfo).GetComponent<Mission>();
		IsActive = false;
		component.isSimple = simple;
		string text = (loadSavedMission ? missionInfo.SavePath : missionInfo.FilePath);
		if (File.Exists(text))
		{
			try
			{
				ConfigNode configNode = ConfigNode.Load(text, bypassLocalization: true);
				if (configNode != null)
				{
					if (loadSavedMission)
					{
						configNode = extractmissionNode(configNode);
						if (configNode == null)
						{
							Debug.Log("[MissionSystem]: Unable to load mission: " + text + " Mission Node not found in file.");
							return null;
						}
					}
					component.Load(configNode, simple);
					if (initMission)
					{
						component.InitMission();
					}
					return component;
				}
				Debug.Log("[MissionSystem]: <b><color=orange>Mission Name:</color></b> " + component.title);
				Debug.Log("[MissionSystem]: <b><color=orange>Mission path:</color></b> " + text);
				Debug.Log("[MissionSystem]: <b><color=orange>Node Count:</color></b> " + component.nodes.Count);
			}
			catch (Exception ex)
			{
				Debug.LogError("[MissionSystem]: Unable to load mission: " + text + "\r\n" + ex.Message);
			}
		}
		else if (missionInfo.missionType != MissionTypes.Steam)
		{
			Debug.Log("[MissionSystem]: <b><color=orange>Mission file not found:</color></b> " + text);
		}
		return null;
	}

	public static ConfigNode extractmissionNode(ConfigNode node)
	{
		ConfigNode configNode = null;
		if (node.HasNode("GAME"))
		{
			ConfigNode node2 = node.GetNode("GAME");
			ConfigNode[] nodes = node2.GetNodes("SCENARIO");
			for (int i = 0; i < nodes.Length; i++)
			{
				string value = "";
				nodes[i].TryGetValue("name", ref value);
				if (value == "MissionSystem" && nodes[i].HasNode("MISSIONS"))
				{
					ConfigNode node3 = nodes[i].GetNode("MISSIONS");
					if (node3.HasNode("MISSION"))
					{
						configNode = node3.GetNode("MISSION");
						break;
					}
				}
			}
			if (configNode == null && node2.HasNode("MISSIONTOSTART"))
			{
				configNode = node2.GetNode("MISSIONTOSTART");
			}
		}
		return configNode;
	}

	public static MissionPlayDialog.MissionProfileInfo LoadMetaMission(MissionFileInfo missionInfo)
	{
		MissionPlayDialog.MissionProfileInfo missionProfileInfo = new MissionPlayDialog.MissionProfileInfo();
		bool flag = false;
		string text = "persistent";
		string folderPath = missionInfo.FolderPath;
		long num = -1L;
		try
		{
			num = MissionPlayDialog.MissionProfileInfo.GetLastWriteTime(text, folderPath);
		}
		catch (Exception ex)
		{
			Debug.LogFormat("[MissionSystem]: Unable to get last write time for SFS file-{0}-{1}\n{2}", folderPath, text, ex.Message);
		}
		if (num != -1L)
		{
			try
			{
				MissionPlayDialog.MissionProfileInfo missionProfileInfo2 = new MissionPlayDialog.MissionProfileInfo();
				missionProfileInfo2.LoadFromMetaFile(text, folderPath);
				if (missionProfileInfo2.lastWriteTime == num)
				{
					missionProfileInfo = missionProfileInfo2;
					flag = true;
				}
			}
			catch (Exception ex2)
			{
				Debug.LogWarningFormat("[MissionSystem]: Errored when loading .loadmeta file, will load full save-{0}-{1}\n{2}", folderPath, text, ex2.Message);
			}
		}
		if (!flag)
		{
			Mission simpleMission = missionInfo.SimpleMission;
			if (simpleMission != null)
			{
				try
				{
					missionProfileInfo = missionProfileInfo.LoadDetailsFromMission(simpleMission);
					missionProfileInfo.lastWriteTime = num;
					missionProfileInfo.SaveToMetaFile(text, folderPath);
				}
				catch (Exception ex3)
				{
					Debug.LogFormat("[MissionSystem]: Failed to save .loadmeta data for save-{0}-{1}\n{2}", folderPath, text, ex3.Message);
				}
			}
		}
		return missionProfileInfo;
	}

	public IEnumerator SetupMissionGame(MissionFileInfo missionInfo, bool playMission, bool testMode, Callback onSuccess = null, Callback onFail = null, string overrideSaveFile = null, bool trackingStation = false)
	{
		bool planetariumTimePaused = false;
		if (missionInfo == null)
		{
			Debug.LogError("[MissionSystem]: missionInfo is null. Cannot build mission.");
			onFail?.Invoke();
			yield break;
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = Localizer.Format("#autoLOC_8002118");
			LoadingBufferMask.Instance.Show();
		}
		IsTestMode = testMode;
		if (playMission)
		{
			if (HighLogic.LoadedSceneIsMissionBuilder)
			{
				if (MissionEditorLogic.Instance.actionPane.CurrentGapDisplay != null)
				{
					MissionEditorLogic.Instance.actionPane.CurrentGapDisplay.Clean();
				}
				MissionEditorLogic.Instance.actionPane.Clean();
				ScaledSpace.ToggleAll(toggleValue: true);
				ScaledCamera.Instance.ResetTarget();
			}
			if (HighLogic.LoadedScene != GameScenes.MAINMENU)
			{
				PSystemSetup.Instance.OnSceneChange(GameScenes.MAINMENU);
				PSystemSetup.Instance.OnLevelLoaded(GameScenes.MAINMENU);
			}
			yield return null;
			SetupCraftSaveFiles(missionInfo);
		}
		newMission = LoadMission(missionInfo, loadSavedMission: false, initMission: true);
		yield return null;
		if ((bool)Planetarium.fetch)
		{
			Planetarium.SetUniversalTime(newMission.situation.startUT);
			planetariumTimePaused = Planetarium.fetch.pause;
			Planetarium.fetch.pause = true;
			Planetarium.fetch.UpdateCBs();
			yield return null;
		}
		yield return StartCoroutine(newMission.GenerateMissionLaunchSites(null));
		yield return null;
		if (playMission && !trackingStation && newMission.BlockPlayMission())
		{
			if ((bool)LoadingBufferMask.Instance)
			{
				LoadingBufferMask.Instance.Hide();
				LoadingBufferMask.Instance.textInfo.text = "";
			}
			onFail?.Invoke();
			if ((bool)Planetarium.fetch)
			{
				Planetarium.fetch.pause = planetariumTimePaused;
			}
			yield break;
		}
		if (playMission && missionInfo.HasSave)
		{
			string text = (string.IsNullOrEmpty(overrideSaveFile) ? "persistent" : overrideSaveFile);
			string saveFolder = MissionsUtils.SavesPath + missionInfo.folderName;
			ConfigNode configNode = GamePersistence.LoadSFSFile(text, saveFolder);
			HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode, text, nullIfIncompatible: true, suppressIncompatibleMessage: false);
			HighLogic.SaveFolder = saveFolder;
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			if (configNode != null)
			{
				GameEvents.onGameStatePostLoad.Fire(configNode);
			}
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", saveFolder, SaveMode.OVERWRITE);
			yield return null;
		}
		else
		{
			if (newMission == null || newMission.situation == null)
			{
				Debug.LogError("[MissionSystem]: Unable to start mission " + missionInfo.folderName + " as mission file would not load");
				if (playMission)
				{
					PSystemSetup.Instance.OnSceneChange(HighLogic.LoadedScene);
					PSystemSetup.Instance.OnLevelLoaded(HighLogic.LoadedScene);
				}
				if ((bool)LoadingBufferMask.Instance)
				{
					LoadingBufferMask.Instance.Hide();
					LoadingBufferMask.Instance.textInfo.text = "";
				}
				onFail?.Invoke();
				if ((bool)Planetarium.fetch)
				{
					Planetarium.fetch.pause = planetariumTimePaused;
				}
				yield break;
			}
			GenerateBriefingDialogNode(newMission);
			yield return null;
			MissionSituation newSituation = newMission.situation;
			HighLogic.CurrentGame = GamePersistence.CreateNewGame(newMission.PersistentSaveName, playMission ? Game.Modes.MISSION : Game.Modes.MISSION_BUILDER, newSituation.gameParameters, "", newSituation.startScene, newSituation.startFacility);
			yield return null;
			HighLogic.CurrentGame.flagURL = newMission.flagURL;
			newSituation.FillVesselSituationList(newMission.startNode, replaceIfFound: false, trackingStation);
			yield return null;
			SetupMissionParameters(newMission);
			yield return null;
			if (newSituation.crewRoster != null)
			{
				newSituation.crewRoster.UpdateExperience();
				HighLogic.CurrentGame.CrewRoster = newSituation.crewRoster;
				ValidateCrewAssignments(newMission);
			}
			else
			{
				HighLogic.CurrentGame.CrewRoster = KerbalRoster.GenerateInitialCrewRoster(playMission ? Game.Modes.MISSION : Game.Modes.MISSION_BUILDER);
			}
			yield return null;
			if (playMission && newMission.situation.VesselsArePending && !trackingStation)
			{
				newSituation.startType = ((newMission.situation.CurrentVesselToBuild.location.facility == EditorFacility.const_2) ? MissionSituation.StartTypeEnum.const_1 : MissionSituation.StartTypeEnum.const_0);
				HighLogic.CurrentGame.editorFacility = ((newMission.situation.CurrentVesselToBuild.location.facility != EditorFacility.const_2) ? EditorFacility.const_1 : EditorFacility.const_2);
				HighLogic.CurrentGame.Parameters.Editor.startUpMode = 0;
				HighLogic.CurrentGame.missionToStart = newMission;
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.EDITOR);
				File.Copy(missionInfo.SaveFolderPath + "persistent.sfs", missionInfo.SaveFolderPath + "reset.missionsfs", overwrite: true);
				if ((bool)LoadingBufferMask.Instance)
				{
					LoadingBufferMask.Instance.Hide();
					LoadingBufferMask.Instance.textInfo.text = "";
				}
				onSuccess?.Invoke();
				if ((bool)Planetarium.fetch)
				{
					Planetarium.fetch.pause = planetariumTimePaused;
				}
				yield break;
			}
			yield return null;
			if (playMission && (newSituation.startType == MissionSituation.StartTypeEnum.VesselList || newSituation.startType == MissionSituation.StartTypeEnum.SpaceCenter || trackingStation))
			{
				string errorString2 = "";
				_setupFlightState = false;
				if ((bool)LoadingBufferMask.Instance)
				{
					LoadingBufferMask.Instance.textInfo.text = Localizer.Format("#autoLOC_8002116");
				}
				yield return StartCoroutine(SetupFlight(newMission));
				yield return null;
				if (!_setupFlightState)
				{
					if (playMission)
					{
						PSystemSetup.Instance.OnSceneChange(HighLogic.LoadedScene);
						PSystemSetup.Instance.OnLevelLoaded(HighLogic.LoadedScene);
					}
					newMission.MissionCriticalError(errorString2);
					if ((bool)LoadingBufferMask.Instance)
					{
						LoadingBufferMask.Instance.Hide();
						LoadingBufferMask.Instance.textInfo.text = "";
					}
					onFail?.Invoke();
					if ((bool)Planetarium.fetch)
					{
						Planetarium.fetch.pause = planetariumTimePaused;
					}
					yield break;
				}
				if (!trackingStation)
				{
					newMission.StartMission();
					yield return null;
				}
			}
			yield return null;
			HighLogic.CurrentGame.missionToStart = newMission;
			if (playMission)
			{
				string errorString2 = GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, newSituation.startScene);
				yield return null;
				if (!string.IsNullOrEmpty(errorString2))
				{
					File.Copy(missionInfo.SaveFolderPath + "persistent.sfs", missionInfo.SaveFolderPath + "reset.missionsfs", overwrite: true);
				}
				if ((HighLogic.LoadedSceneIsMissionBuilder || HighLogic.LoadedScene == GameScenes.MAINMENU) && !string.IsNullOrEmpty(errorString2) && HighLogic.CurrentGame.flightState.protoVessels.Count > 0)
				{
					VesselType vesselType = HighLogic.CurrentGame.flightState.protoVessels[HighLogic.CurrentGame.flightState.activeVesselIdx].vesselType;
					if (HighLogic.CurrentGame.flightState.activeVesselIdx < HighLogic.CurrentGame.flightState.protoVessels.Count && vesselType > VesselType.Unknown && vesselType != VesselType.Flag && vesselType != VesselType.DeployedScienceController && vesselType != VesselType.DeployedSciencePart)
					{
						if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs"))
						{
							File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs");
						}
						yield return null;
						File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs", overwrite: true);
						yield return null;
						if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/startcreatevesselspawn.missionsfs"))
						{
							File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/startcreatevesselspawn.missionsfs");
						}
						yield return null;
						File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/startcreatevesselspawn.missionsfs", overwrite: true);
						yield return null;
					}
				}
			}
		}
		if (playMission && !testMode)
		{
			if (missionInfo.HasSave)
			{
				AnalyticsUtil.LogSaveGameResumed(HighLogic.CurrentGame);
			}
			else
			{
				AnalyticsUtil.LogSaveGameCreated(HighLogic.CurrentGame);
			}
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Hide();
			LoadingBufferMask.Instance.textInfo.text = "";
		}
		onSuccess?.Invoke();
	}

	public CelestialBody GetFlightStateActiveBody(Mission newMission)
	{
		VesselSituation vesselSituationByVesselID = newMission.GetVesselSituationByVesselID(newMission.situation.startVesselID);
		if (vesselSituationByVesselID != null && vesselSituationByVesselID.location != null)
		{
			switch (vesselSituationByVesselID.location.situation)
			{
			case MissionSituation.VesselStartSituations.ORBITING:
				return vesselSituationByVesselID.location.orbitSnapShot.Body;
			case MissionSituation.VesselStartSituations.PRELAUNCH:
			{
				CelestialBody launchSiteBody = PSystemSetup.Instance.GetLaunchSiteBody(vesselSituationByVesselID.location.launchSite);
				if (!(launchSiteBody != null))
				{
					return FlightGlobals.GetHomeBody();
				}
				return launchSiteBody;
			}
			case MissionSituation.VesselStartSituations.LANDED:
				return vesselSituationByVesselID.location.vesselGroundLocation.targetBody;
			}
		}
		return FlightGlobals.GetHomeBody();
	}

	public void SetupCraftSaveFiles(MissionFileInfo missionInfo)
	{
		string path = missionInfo.ShipFolderPath + "VAB/";
		string text = missionInfo.SaveFolderPath + "Ships/VAB/";
		if (Directory.Exists(path))
		{
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string[] files = Directory.GetFiles(path, "*.craft");
			int i = 0;
			for (int num = files.Length; i < num; i++)
			{
				string fileName = Path.GetFileName(files[i]);
				File.Copy(files[i], text + fileName, overwrite: true);
			}
		}
		path = missionInfo.ShipFolderPath + "SPH/";
		text = missionInfo.SaveFolderPath + "Ships/SPH/";
		if (Directory.Exists(path))
		{
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string[] files2 = Directory.GetFiles(path, "*.craft");
			int j = 0;
			for (int num2 = files2.Length; j < num2; j++)
			{
				File.Copy(files2[j], text + Path.GetFileName(files2[j]), overwrite: true);
			}
		}
		string bannerPath = missionInfo.BannerPath;
		string text2 = missionInfo.SaveFolderPath + "Banners/";
		if (!Directory.Exists(bannerPath))
		{
			return;
		}
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		MEBannerType[] array = (MEBannerType[])Enum.GetValues(typeof(MEBannerType));
		for (int k = 0; k < array.Length; k++)
		{
			string text3 = text2 + "/" + array[k].ToString() + "/";
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			MEBannerEntry banner = missionInfo.SimpleMission.GetBanner(array[k]);
			if (File.Exists(banner.FullPath))
			{
				File.Copy(banner.FullPath, text3 + Path.GetFileName(banner.fileName), overwrite: true);
			}
		}
	}

	public void GenerateBriefingDialogNode(Mission newMission)
	{
		if (newMission.situation.showBriefing)
		{
			MENode mENode = MENode.Spawn(newMission);
			mENode.AddAction("ActionDialogMessage", null);
			ActionDialogMessage actionDialogMessage = mENode.actionModules[0] as ActionDialogMessage;
			if (actionDialogMessage != null)
			{
				mENode.Title = "MissionBriefingMessage";
				newMission.startNode.dockedNodes.Add(mENode);
				mENode.dockParentNode = newMission.startNode;
				actionDialogMessage.messageHeading = Localizer.Format("#autoLOC_8002028");
				actionDialogMessage.message = newMission.briefing;
				actionDialogMessage.missionInstructor = newMission.situation.missionInstructor;
				actionDialogMessage.isBriefingMessage = true;
				actionDialogMessage.autoGrowDialogHeight = true;
				newMission.nodes.Add(mENode.id, mENode);
				newMission.briefingNodeActive = true;
			}
			else
			{
				mENode.gameObject.DestroyGameObject();
			}
		}
	}

	public void CheckFireBriefingDialog(Mission mission, bool chckMissionStarted)
	{
		if (!mission.situation.showBriefing || ((!chckMissionStarted || mission.isStarted) && chckMissionStarted) || mission.startNode.dockedNodes.Count <= 0)
		{
			return;
		}
		List<ActionDialogMessage> allActionModules = mission.GetAllActionModules<ActionDialogMessage>();
		int num = 0;
		while (true)
		{
			if (num < allActionModules.Count)
			{
				if (allActionModules[num].node.IsDockedToStartNode && allActionModules[num].node.Title == "MissionBriefingMessage")
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Instance.StartCoroutine(allActionModules[num].Fire());
	}

	public void SetupMissionParameters(Mission mission)
	{
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToAstronautC = HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().facilityOpenAC;
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInSPH = HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().facilityOpenEditor;
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB = HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().facilityOpenEditor;
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToRnD = false;
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToAdmin = false;
		HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToMissionControl = false;
		HighLogic.CurrentGame.Parameters.Difficulty.AutoHireCrews = mission.situation.autoGenerateCrew;
	}

	public IEnumerator SetupFlight(Mission mission, bool trackingStation = false, Callback<Mission> onCompleted = null, Callback<Mission, string> onFailed = null)
	{
		string errorString = "";
		MissionSituation situation = mission.situation;
		ConfigNode flightstateNode = new ConfigNode("FLIGHTSTATE");
		flightstateNode.AddValue("version", Versioning.version_major + "." + Versioning.version_minor + "." + Versioning.Revision);
		flightstateNode.AddValue("UT", situation.startUT);
		if (HighLogic.LoadedScene != GameScenes.MAINMENU)
		{
			PSystemSetup.Instance.OnSceneChange(GameScenes.MAINMENU);
			PSystemSetup.Instance.OnLevelLoaded(GameScenes.MAINMENU);
		}
		yield return null;
		if ((bool)Planetarium.fetch)
		{
			Planetarium.SetUniversalTime(situation.startUT);
			Planetarium.fetch.UpdateCBs();
		}
		yield return null;
		List<ConfigNode> vesselsNodes = new List<ConfigNode>();
		int startVslIndx = 0;
		int totalVslIndex = 0;
		bool failedtobuildVessel = false;
		for (int vsI2 = 0; vsI2 < situation.VesselSituationList.Count; vsI2++)
		{
			VesselSituation vesselSituation = situation.VesselSituationList.KeyAt(vsI2);
			if ((vesselSituation.readyToLaunch && !vesselSituation.launched) || trackingStation)
			{
				yield return StartCoroutine(ConstructShip(situation.VesselSituationList.KeyAt(vsI2), mission, instantiateVessel: false));
				yield return null;
				if (vesselConfigNode != null)
				{
					vesselsNodes.Add(vesselConfigNode);
					if (situation.VesselSituationList.KeyAt(vsI2).persistentId == situation.startVesselID)
					{
						startVslIndx = totalVslIndex;
					}
					totalVslIndex++;
				}
				else
				{
					failedtobuildVessel = true;
				}
			}
			yield return null;
		}
		if (failedtobuildVessel && vesselsNodes.Count == 0)
		{
			Debug.LogWarning("[MissionSystem]: Setup FlightMode failed. Cannot start mission.");
			onFailed?.Invoke(mission, errorString);
			_setupFlightState = false;
			yield break;
		}
		for (int vsI2 = 0; vsI2 < situation.StartingActions.Count; vsI2++)
		{
			if (situation.StartingActions[vsI2].GetType() == typeof(ActionCreateAsteroid))
			{
				ActionCreateAsteroid asteroidAction = situation.StartingActions[vsI2] as ActionCreateAsteroid;
				if (asteroidAction.location.locationChoice == ParamChoices_VesselSimpleLocation.Choices.orbit)
				{
					asteroidAction.location.orbit.Body.inverseRotation = false;
					asteroidAction.location.orbit.Body.CBUpdate();
					yield return null;
				}
				ConfigNode protoVesselNode = asteroidAction.GetProtoVesselNode();
				vesselsNodes.Add(protoVesselNode);
				if (asteroidAction.PersistentID == situation.startVesselID)
				{
					startVslIndx = totalVslIndex;
				}
				totalVslIndex++;
			}
			else if (situation.StartingActions[vsI2].GetType() == typeof(ActionCreateComet))
			{
				ActionCreateComet cometAction = situation.StartingActions[vsI2] as ActionCreateComet;
				if (cometAction.location.locationChoice == ParamChoices_VesselSimpleLocation.Choices.orbit)
				{
					cometAction.location.orbit.Body.inverseRotation = false;
					cometAction.location.orbit.Body.CBUpdate();
					yield return null;
				}
				ConfigNode protoVesselNode2 = cometAction.GetProtoVesselNode();
				vesselsNodes.Add(protoVesselNode2);
				if (cometAction.PersistentID == situation.startVesselID)
				{
					startVslIndx = totalVslIndex;
				}
				totalVslIndex++;
			}
			else if (situation.StartingActions[vsI2].GetType() == typeof(ActionCreateKerbal))
			{
				ActionCreateKerbal kerbalAction = situation.StartingActions[vsI2] as ActionCreateKerbal;
				if (kerbalAction.location.locationChoice == ParamChoices_VesselSimpleLocation.Choices.orbit)
				{
					kerbalAction.location.orbit.Body.inverseRotation = false;
					kerbalAction.location.orbit.Body.CBUpdate();
					yield return null;
				}
				ConfigNode item = kerbalAction.SpawnKerbal(addtoCurrentGame: false);
				vesselsNodes.Add(item);
				if (kerbalAction.persistentId == situation.startVesselID)
				{
					startVslIndx = totalVslIndex;
				}
				totalVslIndex++;
			}
			else if (situation.StartingActions[vsI2].GetType() == typeof(ActionCreateFlag))
			{
				ConfigNode protoVesselNode3 = (situation.StartingActions[vsI2] as ActionCreateFlag).GetProtoVesselNode();
				vesselsNodes.Add(protoVesselNode3);
				totalVslIndex++;
			}
			yield return null;
		}
		situation.RemoveLaunchedVessels();
		yield return null;
		situation.StartingActions.Clear();
		flightstateNode.AddValue("activeVessel", startVslIndx);
		flightstateNode.AddValue("mapViewFiltering", -1026);
		flightstateNode.AddValue("commNetUIModeTracking", "Network");
		for (int i = 0; i < vesselsNodes.Count; i++)
		{
			flightstateNode.AddNode("VESSEL", vesselsNodes[i]);
		}
		HighLogic.CurrentGame.flightState = new FlightState(flightstateNode, HighLogic.CurrentGame);
		onCompleted?.Invoke(mission);
		_setupFlightState = true;
		yield return null;
	}

	public IEnumerator ConstructShip(VesselSituation vesselSituation, Mission newMission, bool instantiateVessel)
	{
		Debug.LogFormat("[MissionSystem]: Constructing Ship ({0})", Localizer.Format(vesselSituation.vesselName));
		returnVessel = null;
		vesselConfigNode = null;
		newMission.situation.VesselSituationLaunched(vesselSituation);
		ConfigNode node = new ConfigNode();
		ShipConstruct shipOut = new ShipConstruct();
		bool flag = false;
		errorString = "";
		bool resetCBRequired = false;
		bool originalCBInverseRotation = false;
		bool flag2 = false;
		Vessel vessel = null;
		bool flag3 = false;
		Vector3d vector3d = Vector3d.zero;
		double alt = 0.0;
		float num = 0f;
		float num2 = 0f;
		MissionCraft missionCraft = null;
		ConfigNode configNode = new ConfigNode();
		for (int i = 0; i < newMission.craftFileList.Count; i++)
		{
			if (!string.Equals(newMission.craftFileList.At(i).craftFile, vesselSituation.craftFile, StringComparison.InvariantCultureIgnoreCase))
			{
				continue;
			}
			missionCraft = newMission.craftFileList.At(i);
			configNode = newMission.craftFileList.At(i).CraftNode;
			if (!(newMission.craftFileList.At(i).facility == vesselSituation.location.facility.ToString()))
			{
				continue;
			}
			if (shipOut.LoadShip(configNode, vesselSituation.persistentId, returnErrors: true, out errorString))
			{
				flag = true;
				if (shipOut.steamPublishedFileId == 0L)
				{
					shipOut.steamPublishedFileId = KSPSteamUtils.GetSteamIDFromSteamFolder(newMission.craftFileList.At(i).craftFolder);
				}
			}
			break;
		}
		if (!flag)
		{
			if (configNode != null && configNode.nodes.Count > 0 && shipOut.LoadShip(configNode, vesselSituation.persistentId, returnErrors: true, out errorString))
			{
				flag = true;
				if (shipOut.steamPublishedFileId == 0L && missionCraft != null && missionCraft.craftFile == vesselSituation.craftFile)
				{
					shipOut.steamPublishedFileId = KSPSteamUtils.GetSteamIDFromSteamFolder(missionCraft.craftFolder);
				}
			}
			if (!flag)
			{
				Debug.LogError("[MissionsExpansion] Failed to load mission vessel: " + vesselSituation.vesselName);
				Debug.LogError(errorString);
				newMission.situation.VesselSituationLaunched(vesselSituation);
				yield break;
			}
		}
		if (!vesselSituation.playerCreated)
		{
			shipOut.shipName = Localizer.Format(vesselSituation.vesselName);
			if (vesselSituation.location.situation == MissionSituation.VesselStartSituations.ORBITING && !EditorLogic.MissionCheckLaunchClamps(shipOut, shipOut.parts[0].localRoot, vesselSituation, out shipOut))
			{
				Debug.LogError("[MissionsExpansion] Failed to build mission vessel: " + vesselSituation.vesselName + " as it contains launch clamps and has a starting situation of Orbiting.");
				yield break;
			}
		}
		VesselCrewManifest vesselCrewManifest = VesselCrewManifest.FromConfigNode(configNode);
		vesselCrewManifest.AddCrewMembers(ref vesselSituation.vesselCrew, HighLogic.CurrentGame.CrewRoster);
		if (vesselSituation.autoGenerateCrew)
		{
			bool autoHireCrews = HighLogic.CurrentGame.Parameters.Difficulty.AutoHireCrews;
			HighLogic.CurrentGame.Parameters.Difficulty.AutoHireCrews = vesselSituation.autoGenerateCrew;
			vesselCrewManifest = HighLogic.CurrentGame.CrewRoster.DefaultCrewForVessel(configNode, vesselCrewManifest, autohire: true, usePreviousVCMToFill: true);
			HighLogic.CurrentGame.Parameters.Difficulty.AutoHireCrews = autoHireCrews;
		}
		vesselSituation.SetVesselCrew(HighLogic.CurrentGame.CrewRoster, vesselCrewManifest);
		ShipConstruction.ShipManifest = vesselCrewManifest;
		string text = "";
		string displaylandedAt = "";
		bool flag4;
		bool flag5 = (flag4 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.LANDED) && vesselSituation.location.vesselGroundLocation.splashed;
		bool flag6 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH;
		bool flag7 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.ORBITING;
		if (flag7)
		{
			setBodyRotation(vesselSituation.location.orbitSnapShot.Body, out resetCBRequired, out originalCBInverseRotation);
		}
		Orbit orbit = vesselSituation.location.orbitSnapShot.RelativeOrbit(newMission);
		if (flag4 || flag6)
		{
			if (flag6)
			{
				CelestialBody launchSiteBody = PSystemSetup.Instance.GetLaunchSiteBody(vesselSituation.location.launchSite);
				if (launchSiteBody != null)
				{
					vesselSituation.location.vesselGroundLocation.targetBody = launchSiteBody;
				}
			}
			orbit.referenceBody = vesselSituation.location.vesselGroundLocation.targetBody;
		}
		if (flag4)
		{
			Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
			Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
			Vector3d surfaceNVector = LatLon.GetSurfaceNVector(cf, vesselSituation.location.vesselGroundLocation.latitude, vesselSituation.location.vesselGroundLocation.longitude);
			GameObject gameObject = new GameObject("TempObject " + vesselSituation.vesselName);
			gameObject.transform.SetParent(vesselSituation.location.vesselGroundLocation.targetBody.transform);
			Vector3d vector3d2 = surfaceNVector * vesselSituation.location.vesselGroundLocation.targetBody.pqsController.radius;
			gameObject.transform.localPosition = vector3d2;
			alt = vesselSituation.location.vesselGroundLocation.targetBody.pqsController.GetSurfaceHeight(surfaceNVector, overrideQuadBuildCheck: true) - vesselSituation.location.vesselGroundLocation.targetBody.Radius;
			if (flag5)
			{
				if (alt < 0.0)
				{
					alt = 0.0;
					if (!vesselSituation.playerCreated)
					{
						shipOut.shipName = Localizer.Format(vesselSituation.vesselName);
						EditorDriver.editorFacility = vesselSituation.location.facility;
						if (!EditorLogic.MissionCheckLaunchClamps(shipOut, shipOut.parts[0].localRoot, vesselSituation, out shipOut, overrideSituationCheck: true))
						{
							Debug.LogError("[MissionsExpansion] Failed to build mission vessel: " + vesselSituation.vesselName + " as it contains launch clamps and has a starting situation of Orbiting.");
							yield break;
						}
					}
				}
				else
				{
					flag5 = false;
				}
			}
			CelestialBody targetBody = vesselSituation.location.vesselGroundLocation.targetBody;
			num = (float)vesselSituation.location.vesselGroundLocation.latitude;
			num2 = (float)vesselSituation.location.vesselGroundLocation.longitude;
			Vector3d worldSurfacePosition = vesselSituation.location.vesselGroundLocation.targetBody.GetWorldSurfacePosition(num, num2, alt);
			Vector3d relSurfacePosition = targetBody.GetRelSurfacePosition(num, (double)num2 + targetBody.directRotAngle, alt);
			Quaternion quaternion = vesselSituation.location.vesselGroundLocation.rotation.quaternion;
			Quaternion quaternion2 = ((configNode.GetValue("type") == "VAB") ? Quaternion.Euler(-90f, -90f, -90f) : Quaternion.Euler(0f, -90f, -90f));
			QuaternionD quaternionD = Quaternion.LookRotation(relSurfacePosition) * quaternion * quaternion2;
			putShiptoGround(shipOut, vesselSituation, null, worldSurfacePosition, quaternionD);
			double num3 = Vector3d.Distance(worldSurfacePosition, Vector3d.zero);
			if (num3 > 10000.0)
			{
				flag3 = true;
				vector3d = worldSurfacePosition;
				Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} Setting pos to {2}", shipOut.shipName, num3.ToString("N2"), vector3d);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		if (flag6)
		{
			text = vesselSituation.location.launchSite;
			displaylandedAt = PSystemSetup.Instance.GetLaunchSiteDisplayName(text);
			PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(vesselSituation.location.launchSite);
			if (spaceCenterFacility != null)
			{
				PQSCity componentInParent = spaceCenterFacility.facilityTransform.GetComponentInParent<PQSCity>();
				componentInParent.Orientate();
				UpgradeableFacility component = spaceCenterFacility.facilityTransform.GetComponent<UpgradeableFacility>();
				component.SetupLevels();
				component.SetLevel(MissionsUtils.GetFacilityLimit(vesselSituation.location.launchSite, component.MaxLevel + 1) - 1);
				spaceCenterFacility.SetSpawnPointsLatLonAlt();
				PSystemSetup.SpaceCenterFacility.SpawnPoint spawnPoint = spaceCenterFacility.GetSpawnPoint(vesselSituation.location.launchSite);
				if (spawnPoint == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find Facility spawn point for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				Transform spawnPointTransform = spawnPoint.GetSpawnPointTransform();
				if (spawnPointTransform != null)
				{
					putShiptoGround(shipOut, vesselSituation, spawnPointTransform, Vector3.zero, Quaternion.identity);
					float num4 = Vector3.Distance(spawnPointTransform.position, Vector3.zero);
					if (num4 > 10000f)
					{
						flag3 = true;
						vector3d = componentInParent.sphere.PrecisePosition + componentInParent.PlanetRelativePosition + spawnPointTransform.localPosition;
						Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} KSC spawnPosition {2} Setting pos to {3}", shipOut.shipName, num4.ToString("N2"), spawnPointTransform.position, vector3d);
					}
					double lat = 0.0;
					double lon = 0.0;
					if (spawnPoint.latlonaltSet)
					{
						num = (float)spawnPoint.latitude;
						num2 = (float)spawnPoint.longitude;
						alt = (float)spawnPoint.altitude;
						Debug.LogFormat("[MissionSystem]: Vessel {0} Set Lat:{1}, Lon:{2}, Alt:{3} to Space Center Facility Spawn Point.", shipOut.shipName, num, num2, alt);
					}
					else
					{
						componentInParent.celestialBody.GetLatLonAlt(vector3d, out lat, out lon, out alt);
						Debug.LogFormat("[MissionSystem]: Vessel {0} Set Lat:{1}, Lon:{2}, Alt:{3} to Space Center Facility Point.", shipOut.shipName, num, num2, alt);
					}
				}
			}
			else
			{
				LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(vesselSituation.location.launchSite);
				if (launchSite == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find LaunchSite for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				if (launchSite.pqsCity != null)
				{
					launchSite.pqsCity.Orientate();
				}
				else
				{
					if (!(launchSite.pqsCity2 != null))
					{
						Debug.LogError("[MissionSystem]: Unable to find PQSCity for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
						yield break;
					}
					launchSite.pqsCity2.Orientate();
				}
				LaunchSite.SpawnPoint spawnPoint2 = launchSite.GetSpawnPoint(vesselSituation.location.launchSite);
				if (spawnPoint2 == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find Facility spawn point for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				Transform spawnPointTransform2 = spawnPoint2.GetSpawnPointTransform();
				if (spawnPointTransform2 != null)
				{
					putShiptoGround(shipOut, vesselSituation, spawnPointTransform2, Vector3.zero, Quaternion.identity);
					float num5 = Vector3.Distance(spawnPointTransform2.position, Vector3.zero);
					if (num5 > 10000f)
					{
						flag3 = true;
						if (launchSite.isPQSCity)
						{
							vector3d = launchSite.pqsCity.sphere.PrecisePosition + launchSite.pqsCity.PlanetRelativePosition + spawnPointTransform2.localPosition;
							Debug.LogFormat("[MissionSystem]: VesselPosToSet: Sphere Pos {0} City Pos {1} spawnpoint LocalPos {2}", launchSite.pqsCity.sphere.PrecisePosition, launchSite.pqsCity.PlanetRelativePosition, spawnPointTransform2.localPosition);
						}
						else if (launchSite.isPQSCity2)
						{
							vector3d = launchSite.pqsCity2.sphere.PrecisePosition + launchSite.pqsCity2.PlanetRelativePosition + spawnPointTransform2.localPosition;
							Debug.LogFormat("[MissionSystem]: VesselPosToSet: Sphere Pos {0} City Pos {1} spawnpoint LocalPos {2}", launchSite.pqsCity2.sphere.PrecisePosition, launchSite.pqsCity2.PlanetRelativePosition, spawnPointTransform2.localPosition);
						}
						Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} spawnPosition {2} Setting pos to {3}", shipOut.shipName, num5.ToString("N2"), spawnPointTransform2.position, vector3d);
					}
					if (launchSite.isPQSCity)
					{
						if (spawnPoint2.latlonaltSet)
						{
							num = (float)spawnPoint2.latitude;
							num2 = (float)spawnPoint2.longitude;
							if (flag3)
							{
								alt = (float)spawnPoint2.altitude;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to SpawnPoint coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
						else
						{
							num = (float)launchSite.pqsCity.lat;
							num2 = (float)launchSite.pqsCity.lon;
							if (flag3)
							{
								alt = (float)launchSite.pqsCity.alt;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to PQSCity coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
					}
					if (launchSite.isPQSCity2)
					{
						if (spawnPoint2.latlonaltSet)
						{
							num = (float)spawnPoint2.latitude;
							num2 = (float)spawnPoint2.longitude;
							if (flag3)
							{
								alt = (float)spawnPoint2.altitude;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to SpawnPoint coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
						else
						{
							num = (float)launchSite.pqsCity2.lat;
							num2 = (float)launchSite.pqsCity2.lon;
							if (flag3)
							{
								alt = (float)launchSite.pqsCity2.alt;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to PQSCity2 coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
					}
				}
			}
		}
		Vessel vessel2;
		try
		{
			vessel2 = ShipConstruction.AssembleForLaunch(shipOut, text, displaylandedAt, (shipOut.missionFlag != string.Empty) ? shipOut.missionFlag : newMission.flagURL, HighLogic.CurrentGame, vesselCrewManifest, fromShipAssembly: true, setActiveVessel: false, flag4 || flag6, !instantiateVessel, orbit, flag7, flag5);
			vessel2.gameObject.SetActive(value: false);
			ToggleVesselComponents(vessel2, toggleValue: false);
		}
		catch (Exception)
		{
			Debug.LogError("[MissionSystem]: Unable to Assemble Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
			yield break;
		}
		if (flag5)
		{
			vessel2.protoVessel.altitude = alt;
			vessel2.protoVessel.situation = Vessel.Situations.SPLASHED;
			vessel2.protoVessel.skipGroundPositioning = true;
			vessel2.protoVessel.vesselSpawning = false;
		}
		else
		{
			vessel2.protoVessel.situation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), vesselSituation.location.situation.ToString());
			vessel2.protoVessel.skipGroundPositioning = vessel2.skipGroundPositioning;
			vessel2.protoVessel.vesselSpawning = vessel2.vesselSpawning;
		}
		if (flag4 || flag6)
		{
			if (flag5)
			{
				vessel2.protoVessel.landed = false;
				vessel2.protoVessel.splashed = true;
			}
			else
			{
				vessel2.protoVessel.landed = true;
				vessel2.protoVessel.landedAt = vessel2.landedAt;
				vessel2.protoVessel.displaylandedAt = vessel2.displaylandedAt;
				vessel2.protoVessel.PQSminLevel = 0;
				vessel2.PQSminLevel = 0;
				vessel2.protoVessel.PQSmaxLevel = 0;
				vessel2.PQSmaxLevel = 0;
			}
			vessel2.latitude = (vessel2.protoVessel.latitude = num);
			vessel2.longitude = (vessel2.protoVessel.longitude = num2);
			if (flag3)
			{
				vessel2.altitude = (vessel2.protoVessel.altitude = alt);
			}
		}
		if (flag6)
		{
			vessel2.launchedFrom = text;
			vessel2.protoVessel.launchedFrom = text;
		}
		List<ModuleWheelBrakes> list = vessel2.FindPartModulesImplementing<ModuleWheelBrakes>();
		if (list.Count > 0)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].brakeInput = (vesselSituation.location.brakesOn ? 1f : 0f);
			}
			if (vesselSituation.location.brakesOn)
			{
				vessel2.ActionGroups.SetGroup(KSPActionGroup.Brakes, active: true);
				int groupIndex = BaseAction.GetGroupIndex(KSPActionGroup.Brakes);
				vessel2.ActionGroups.cooldownTimes[groupIndex] = 0.0;
				vessel2.protoVessel.actionGroups = new ConfigNode();
				vessel2.ActionGroups.Save(vessel2.protoVessel.actionGroups);
			}
		}
		vessel2.protoVessel.ResetProtoPartSnapShots();
		int num6 = 0;
		for (int k = 0; k < vessel2.protoVessel.protoPartSnapshots.Count; k++)
		{
			ProtoPartSnapshot protoPartSnapshot = vessel2.protoVessel.protoPartSnapshots[k];
			protoPartSnapshot.flagURL = ((shipOut.missionFlag != string.Empty) ? shipOut.missionFlag : newMission.flagURL);
			if (protoPartSnapshot.inverseStageIndex > num6)
			{
				num6 = protoPartSnapshot.inverseStageIndex;
			}
		}
		vessel2.protoVessel.stage = num6 + 1;
		vessel2.protoVessel.refTransform = vessel2.referenceTransformId;
		if (flag7 && !instantiateVessel)
		{
			setVesselOrbitPosition(vessel2);
		}
		vessel2.protoVessel.Save(node);
		FlightGlobals.RemoveVessel(vessel2);
		if (!instantiateVessel || HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			UnityEngine.Object.DestroyImmediate(vessel2.gameObject);
		}
		if (flag2)
		{
			Vector3d offset = ((!(vessel != null)) ? FloatingOrigin.ReverseOffset : vessel.CoMD);
			FloatingOrigin.SetOffset(offset);
		}
		if (resetCBRequired)
		{
			if (flag7 && !HighLogic.LoadedSceneIsFlight)
			{
				vesselSituation.location.orbitSnapShot.Body.inverseRotation = originalCBInverseRotation;
				vesselSituation.location.orbitSnapShot.Body.CBUpdate();
			}
			else if (!flag7)
			{
				vesselSituation.location.vesselGroundLocation.targetBody.inverseRotation = originalCBInverseRotation;
				vesselSituation.location.vesselGroundLocation.targetBody.CBUpdate();
			}
		}
		if (instantiateVessel)
		{
			vessel2.protoVessel.Load(HighLogic.CurrentGame.flightState, vessel2);
			if (flag7 && HighLogic.LoadedSceneIsFlight)
			{
				Instance.setVesselOrbit(vessel2);
			}
			returnVessel = vessel2.protoVessel.vesselRef;
			if (HighLogic.LoadedScene != GameScenes.FLIGHT)
			{
				HighLogic.CurrentGame.flightState.protoVessels.Add(vessel2.protoVessel);
				if (vesselSituation.focusonSpawn)
				{
					HighLogic.CurrentGame.flightState.activeVesselIdx = HighLogic.CurrentGame.flightState.protoVessels.IndexOf(vessel2.protoVessel);
				}
				GameEvents.onNewVesselCreated.Fire(vessel2);
			}
			if (HighLogic.LoadedSceneIsFlight && !vesselSituation.focusonSpawn)
			{
				ToggleVesselComponents(vessel2, toggleValue: false);
				vessel2.GoOnRails();
				if (flag3)
				{
					vessel2.SetPosition(vector3d);
					vessel2.CoMD = vector3d;
					vessel2.CoM = vector3d;
					vessel2.protoVessel.CoM = vessel2.CoM;
				}
			}
			if (vessel2 != null && vessel2.gameObject != null)
			{
				vessel2.gameObject.SetActive(value: true);
				StartCoroutine(ReactivateComponents(vessel2));
			}
		}
		vesselConfigNode = node;
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			StartCoroutine(ResetKSCMarkers());
		}
	}

	public IEnumerator ReactivateComponents(Vessel vessel)
	{
		yield return new WaitForEndOfFrame();
		if (vessel != null && vessel.parts != null && vessel.parts.Count > 0)
		{
			ToggleVesselComponents(vessel, toggleValue: true);
		}
	}

	public static IEnumerator ResetKSCMarkers()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (KSCVesselMarkers.fetch != null)
		{
			KSCVesselMarkers.fetch.RefreshMarkers();
		}
	}

	public static void ToggleVesselComponents(Vessel vessel, bool toggleValue)
	{
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			Renderer[] componentsInChildren = vessel.parts[i].GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].enabled = toggleValue;
			}
			SkinnedMeshRenderer[] componentsInChildren2 = vessel.parts[i].GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].enabled = toggleValue;
			}
			if (vessel.parts[i].GetComponent<ModuleProceduralFairing>() != null)
			{
				vessel.parts[i].enabled = toggleValue;
			}
			Collider[] componentsInChildren3 = vessel.parts[i].GetComponentsInChildren<Collider>();
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				componentsInChildren3[l].enabled = toggleValue;
			}
		}
	}

	public void setVesselOrbit(Vessel vessel)
	{
		FlightGlobals.overrideOrbit = true;
		Invoke("disableOverride", 2f);
		setVesselOrbitPosition(vessel);
		vessel.ignoreCollisionsFrames = 60;
		CollisionEnhancer.bypass = true;
		vessel.protoVessel = new ProtoVessel(vessel, preCreate: true);
	}

	public static void setVesselOrbitPosition(Vessel vessel)
	{
		vessel.mainBody.CBUpdate();
		double sma = Math.Min(vessel.mainBody.sphereOfInfluence * 0.99, vessel.orbit.semiMajorAxis);
		Vector3d positionAtUT = vessel.orbit.getPositionAtUT(Planetarium.GetUniversalTime());
		vessel.orbit.referenceBody.GetLatLonAlt(positionAtUT, out var lat, out var lon, out var alt);
		vessel.latitude = (vessel.protoVessel.latitude = lat);
		vessel.longitude = (vessel.protoVessel.longitude = lon);
		vessel.altitude = (vessel.protoVessel.altitude = alt);
		vessel.skipGroundPositioning = true;
		vessel.orbit.SetOrbit(vessel.orbit.inclination, vessel.orbit.eccentricity, sma, vessel.orbit.double_0, vessel.orbit.argumentOfPeriapsis, vessel.orbit.meanAnomalyAtEpoch, Planetarium.GetUniversalTime(), vessel.mainBody);
		if (vessel.orbitDriver != null)
		{
			vessel.orbitDriver.updateFromParameters();
			vessel.orbitDriver.SetOrbitMode(OrbitDriver.UpdateMode.UPDATE);
		}
		vessel.transform.position = vessel.orbit.referenceBody.GetWorldSurfacePosition(lat, lon, alt);
		vessel.SetPosition(positionAtUT, usePristineCoords: false);
		vessel.transform.position = (vessel.CoM = (vessel.CoMD = positionAtUT));
	}

	public static void setBodyRotation(CelestialBody body, out bool resetCBRequired, out bool originalCBInverseRotation)
	{
		resetCBRequired = false;
		originalCBInverseRotation = body.inverseRotation;
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER && body.inverseRotation)
		{
			resetCBRequired = true;
			originalCBInverseRotation = body.inverseRotation;
			body.inverseRotation = false;
			body.CBUpdate();
		}
	}

	public void disableOverride()
	{
		CollisionEnhancer.bypass = false;
		FlightGlobals.overrideOrbit = false;
	}

	public static void putShiptoGround(ShipConstruct ship, VesselSituation vesselSituation, Transform spawnPointTransform, Vector3 spawnPointPos, Quaternion spawnPointRot)
	{
		GameObject gameObject = new GameObject("SpawnPoint" + ship.shipName);
		if (spawnPointTransform == null)
		{
			gameObject.transform.position = spawnPointPos;
			gameObject.transform.rotation = spawnPointRot;
			Vector3 vector = FlightGlobals.getUpAxis(vesselSituation.location.vesselGroundLocation.targetBody, gameObject.transform.position);
			Vector3 origin = gameObject.transform.position + vector * 1500f;
			Vector3 position = gameObject.transform.position;
			if (Physics.Raycast(origin, -vector, out var hitInfo, 5000f, 32768))
			{
				float num = 1500f - hitInfo.distance;
				gameObject.transform.position += vector * num;
				Debug.Log($"[MissionSystem] Moving Vessel Landed spawnpoint {num}.");
			}
			else
			{
				gameObject.transform.position = position;
			}
		}
		else
		{
			gameObject.transform.position = spawnPointTransform.position;
			gameObject.transform.rotation = spawnPointTransform.rotation;
		}
		ShipConstruction.PutShipToGround(ship, gameObject.transform);
		gameObject.DestroyGameObject();
	}

	public void ValidateCrewAssignments(Mission mission)
	{
		List<VesselSituation> allVesselSituations = mission.GetAllVesselSituations();
		int i = 0;
		for (int count = allVesselSituations.Count; i < count; i++)
		{
			MissionCraft craftBySituationsVesselID = mission.GetCraftBySituationsVesselID(allVesselSituations[i].persistentId);
			if (craftBySituationsVesselID != null)
			{
				VesselCrewManifest.FromConfigNode(craftBySituationsVesselID.CraftNode).AddCrewMembers(ref allVesselSituations[i].vesselCrew, mission.situation.crewRoster);
			}
			else
			{
				for (int j = 0; j < allVesselSituations[i].vesselCrew.Count; j++)
				{
					for (int k = 0; k < allVesselSituations[i].vesselCrew[j].crewNames.Count; k++)
					{
						ProtoCrewMember protoCrewMember = mission.situation.crewRoster[allVesselSituations[i].vesselCrew[j].crewNames[k]];
						if (protoCrewMember != null)
						{
							protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
						}
					}
				}
				allVesselSituations[i].vesselCrew = new List<Crew>();
			}
			for (int l = 0; l < allVesselSituations[i].vesselCrew.Count; l++)
			{
				for (int m = 0; m < allVesselSituations[i].vesselCrew[l].crewNames.Count; m++)
				{
					ProtoCrewMember protoCrewMember2 = mission.situation.crewRoster[allVesselSituations[i].vesselCrew[l].crewNames[m]];
					if (protoCrewMember2 != null)
					{
						protoCrewMember2.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
					}
				}
			}
		}
	}

	public void UpdateMissions()
	{
		for (int i = 0; i < missions.Count; i++)
		{
			Mission mission = missions[i];
			if (mission != null && !mission.isEnded)
			{
				mission.UpdateMission();
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		StartCoroutine(OnLoadRoutine(node));
	}

	public IEnumerator OnLoadRoutine(ConfigNode node)
	{
		yield return null;
		node.TryGetEnum("cameraLockOptions", ref _cameraLockOptions, MissionCameraLockOptions.NoChange);
		node.TryGetEnum("cameraLockMode", ref _cameraLockMode, MissionCameraModeOptions.NoChange);
		if (missions == null)
		{
			missions = new List<Mission>();
		}
		if (missionScoreInfo == null)
		{
			missionScoreInfo = MissionScoreInfo.LoadScores();
		}
		if (node.HasNode("MISSIONS"))
		{
			ConfigNode[] nodes = node.GetNode("MISSIONS").GetNodes("MISSION");
			foreach (ConfigNode configNode in nodes)
			{
				bool flag = false;
				if (configNode.HasValue("id"))
				{
					Guid guid = new Guid(configNode.GetValue("id"));
					for (int j = 0; j < missions.Count; j++)
					{
						if (missions[j].id == guid)
						{
							Debug.LogError(string.Concat("[MissionSystem]: Attempting to load duplicate mission with id: (", guid, ") Skipping.."));
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Mission mission = Mission.Spawn();
					mission.Load(configNode);
					if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", mission.HashBytes, mission.Signature))
					{
						Debug.LogError("[MissionSystem]: Unable to add mission to misions list: " + mission.title + " Making History DLC is not installed, or mission is not an allowed stock mission.");
						continue;
					}
					mission.InitMission();
					missions.Add(mission);
				}
			}
		}
		for (int k = 0; k < missions.Count; k++)
		{
			missions[k].ResumeMission();
		}
		IsActive = true;
		if (CameraLockMode == MissionCameraModeOptions.const_2)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003106"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		GameEvents.Mission.onMissionsLoaded.Fire();
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode("MISSIONS");
		for (int i = 0; i < missions.Count; i++)
		{
			ConfigNode node2 = new ConfigNode("MISSION");
			missions[i].Save(node2);
			configNode.AddNode(node2);
		}
		node.AddValue("cameraLockOptions", _cameraLockOptions);
		node.AddValue("cameraLockMode", _cameraLockMode);
	}

	public static void CreateCheckpoint(MENode node, bool addUTStamp = false)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < node.testGroups.Count)
			{
				for (int i = 0; i < node.testGroups[num].testModules.Count; i++)
				{
					if (!node.testGroups[num].testModules[i].ShouldCreateCheckpoint())
					{
						return;
					}
				}
				num++;
				continue;
			}
			ClearToSaveStatus clearToSaveStatus = ((FlightGlobals.ActiveVessel == null) ? ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE : FlightGlobals.ClearToSave());
			if (clearToSaveStatus != 0 && !GameSettings.CAN_ALWAYS_QUICKSAVE && (clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_ON_A_LADDER || clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE || node.isStartNode))
			{
				ScreenMessages.PostScreenMessage(FlightGlobals.GetNotClearToSaveStatusReason(clearToSaveStatus, "Checkpoint"), 2f);
				break;
			}
			string format = (addUTStamp ? "checkpoint_{0}_{1:F2}" : "checkpoint_{0}");
			Game game = HighLogic.CurrentGame.Updated(GameScenes.FLIGHT);
			game.startScene = GameScenes.FLIGHT;
			GamePersistence.SaveGame(game, string.Format(format, node.id, node.activatedUT), HighLogic.SaveFolder, SaveMode.BACKUP);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005069"), 2f, ScreenMessageStyle.UPPER_CENTER);
			if (clearToSaveStatus == ClearToSaveStatus.CLEAR)
			{
				GamePersistence.SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			}
			break;
		}
	}

	public void onGameSceneLoadRequested(GameScenes scene)
	{
		IsActive = false;
	}

	public void onMissionStarted(Mission mission)
	{
		Debug.LogFormat("[MissionSystem]: Mission Started - {0}", base.name);
	}

	public void onMissionEnded(Mission mission)
	{
		DisplayMissionEndMessage(mission);
		if (!IsTestMode)
		{
			missionScoreInfo.AddScore(mission);
			missionScoreInfo.SaveScores();
		}
	}

	public void onMissionNodeChanged(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		if (GameSettings.MISSION_LOG_NODE_ACTIVATIONS)
		{
			Debug.Log("[MissionSystem]: Node Changed in Mission: " + Localizer.Format(mission.title));
			Debug.Log("[MissionSystem]: fromNode: " + Localizer.Format(nodeChanges.from.Title));
			Debug.Log("[MissionSystem]: toNode: " + Localizer.Format(nodeChanges.to.Title));
		}
		if (IsTestMode && GameSettings.MISSION_TEST_AUTOMATIC_CHECKPOINTS)
		{
			CreateCheckpoint(nodeChanges.to);
		}
	}

	public void onVesselLoaded(Vessel v)
	{
		if (!v.isEVA || v.DiscoveryInfo.Level == DiscoveryLevels.Owned)
		{
			return;
		}
		if (v.loaded)
		{
			if (v.DiscoveryInfo.Level != DiscoveryLevels.Owned)
			{
				v.DiscoveryInfo.SetLevel(DiscoveryLevels.Owned);
			}
			return;
		}
		ProtoVessel protoVessel = v.protoVessel;
		DiscoveryLevels discoveryLevels = DiscoveryLevels.None;
		if (protoVessel.discoveryInfo.HasValue("state"))
		{
			discoveryLevels = (DiscoveryLevels)int.Parse(protoVessel.discoveryInfo.GetValue("state"));
		}
		if (discoveryLevels != DiscoveryLevels.Owned)
		{
			protoVessel.discoveryInfo.SetValue("state", (-1).ToString(CultureInfo.InvariantCulture), createIfNotFound: true);
		}
	}

	public static void DisplayMissionEndMessage()
	{
		int count = missions.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (missions[num].isEnded)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		DisplayMissionEndMessage(missions[num]);
	}

	public static void DisplayMissionEndMessage(Mission finishedMission)
	{
		Debug.LogFormat("[MissionSystem]: Mission Finished - {0}", finishedMission.name);
		MissionEndDialog.Display(finishedMission);
	}

	public static bool HasFinishedMission()
	{
		bool result = false;
		int count = missions.Count;
		for (int i = 0; i < count; i++)
		{
			if (missions[i].isEnded)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static void RemoveMissionObjects(bool removeAll = false)
	{
		if (missions == null)
		{
			missions = new List<Mission>();
		}
		for (int num = MissionsGameObject.transform.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = MissionsGameObject.transform.GetChild(num).gameObject;
			Mission component = gameObject.GetComponent<Mission>();
			if (((component != null && MissionEditorLogic.Instance == null) || (MissionEditorLogic.Instance != null && component != MissionEditorLogic.Instance.EditorMission)) && (!missions.Contains(component) || removeAll || component.isSimple) && (component.isSimple || HighLogic.CurrentGame == null || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.missionToStart == null) || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.missionToStart != null && HighLogic.CurrentGame.missionToStart != component)))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	public static void RemoveMissonObject(MissionFileInfo fileInfo)
	{
		for (int num = MissionsGameObject.transform.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = MissionsGameObject.transform.GetChild(num).gameObject;
			Mission component = gameObject.GetComponent<Mission>();
			if (component == null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			else if (component.MissionInfo == fileInfo && (HighLogic.CurrentGame == null || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.missionToStart == null) || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.missionToStart != null && HighLogic.CurrentGame.missionToStart != component)))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	public static void SetLockedCamera(Mission mission, MissionCameraModeOptions newCameraMode, MissionCameraLockOptions newCameraLock)
	{
		if (!(Instance == null))
		{
			Instance.UpdateLockedCameraSettings(mission, newCameraMode, newCameraLock);
		}
	}

	public void UpdateLockedCameraSettings(Mission mission, MissionCameraModeOptions newCameraMode, MissionCameraLockOptions newCameraLock)
	{
		for (int i = 0; i < missions.Count; i++)
		{
			if (mission != null && missions[i].id != mission.id)
			{
				if (newCameraLock != 0)
				{
					mission.cameraLockOptions = MissionCameraLockOptions.NoChange;
				}
				if (newCameraMode != 0)
				{
					mission.cameraLockMode = MissionCameraModeOptions.NoChange;
				}
			}
		}
		if (newCameraLock != 0)
		{
			_cameraLockOptions = newCameraLock;
		}
		if (newCameraMode != 0)
		{
			_cameraLockMode = newCameraMode;
		}
		else
		{
			if ((_cameraLockOptions != MissionCameraLockOptions.LockAllowMap && _cameraLockOptions != MissionCameraLockOptions.LockDisableMap) || !(CameraManager.Instance != null))
			{
				return;
			}
			if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Flight)
			{
				_cameraLockMode = MissionCameraModeOptions.Flight;
				if (mission != null)
				{
					mission.cameraLockMode = MissionCameraModeOptions.Flight;
				}
			}
			else if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3 || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
			{
				_cameraLockMode = MissionCameraModeOptions.const_2;
				if (mission != null)
				{
					mission.cameraLockMode = MissionCameraModeOptions.Flight;
				}
			}
		}
	}

	public static bool AllowCameraSwitch(CameraManager.CameraMode newCameraMode)
	{
		if (!(Instance == null) && ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			switch (newCameraMode)
			{
			case CameraManager.CameraMode.Flight:
				if (Instance.CameraLockOptions != 0 && Instance.CameraLockOptions != MissionCameraLockOptions.Unlock)
				{
					return Instance.CameraLockMode == MissionCameraModeOptions.Flight;
				}
				return true;
			case CameraManager.CameraMode.Map:
				return Instance.CameraLockOptions != MissionCameraLockOptions.LockDisableMap;
			default:
				return false;
			case CameraManager.CameraMode.const_3:
			case CameraManager.CameraMode.Internal:
				if (Instance.CameraLockOptions != 0 && Instance.CameraLockOptions != MissionCameraLockOptions.Unlock)
				{
					return Instance.CameraLockMode == MissionCameraModeOptions.const_2;
				}
				return true;
			}
		}
		return true;
	}

	public static void EnforceCameraModeAndLocks()
	{
		if (Instance == null || !ExpansionsLoader.IsExpansionInstalled("MakingHistory") || CameraManager.Instance == null || (Instance.CameraLockOptions == MissionCameraLockOptions.LockAllowMap && CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Map))
		{
			return;
		}
		switch (Instance.CameraLockMode)
		{
		case MissionCameraModeOptions.Flight:
			if (CameraManager.Instance.currentCameraMode != 0)
			{
				CameraManager.Instance.SetCameraMode(CameraManager.CameraMode.Flight);
			}
			else if (Instance.CameraLockOptions == MissionCameraLockOptions.Unlock)
			{
				ClearCameraModeAndLocks();
			}
			break;
		case MissionCameraModeOptions.const_2:
			if (CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.const_3)
			{
				CameraManager.Instance.SetCameraMode(CameraManager.CameraMode.const_3);
			}
			else if (Instance.CameraLockOptions == MissionCameraLockOptions.Unlock)
			{
				ClearCameraModeAndLocks();
			}
			break;
		case MissionCameraModeOptions.NoChange:
			break;
		}
	}

	public static void ClearCameraModeAndLocks()
	{
		Instance._cameraLockMode = MissionCameraModeOptions.NoChange;
		Instance._cameraLockOptions = MissionCameraLockOptions.NoChange;
		Instance.UpdateLockedCameraSettings(null, MissionCameraModeOptions.NoChange, MissionCameraLockOptions.NoChange);
	}
}
