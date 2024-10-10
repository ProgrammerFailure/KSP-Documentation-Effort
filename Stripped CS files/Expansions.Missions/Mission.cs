using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using Expansions.Missions.Runtime;
using Expansions.Missions.Tests;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Mission : MonoBehaviour, IConfigNode
{
	public string expansionVersion = "";

	public static string lastCompatibleVersion = "1.0.0";

	[SerializeField]
	public string _title = "";

	[SerializeField]
	public string activeNodeName;

	public string briefing = "";

	public string author = "";

	public string modsBriefing = "";

	public string packName;

	public int order;

	public bool hardIcon;

	public MissionDifficulty difficulty = MissionDifficulty.Intermediate;

	[SerializeField]
	public bool isBriefingSet;

	[SerializeField]
	public int seed;

	public string flagURL = "";

	public MEBannerEntry bannerMenu;

	public MEBannerEntry bannerSuccess;

	public MEBannerEntry bannerFail;

	public bool isScoreEnabled = true;

	public float maxScore;

	public DictionaryValueList<string, MissionCraft> craftFileList;

	[SerializeField]
	public MissionFileInfo _missionInfo;

	public MissionSituation situation;

	public MissionFlow flow;

	public DictionaryValueList<Guid, MENode> nodes;

	public List<string> tags;

	public MissionScore globalScore;

	public MissionAwards awards;

	public Guid loadActiveNodeID;

	public double startedUT;

	public float currentScore;

	public string exportName = "";

	public string missionNameAtLastExport = "";

	public ulong steamPublishedFileId;

	public bool briefingNodeActive;

	[SerializeField]
	public List<MENode> orphanNodes;

	public List<MENode> nextObjectivesNodes;

	public List<MENode> inactiveEventNodes;

	public bool switchActiveVessel;

	public Vessel vesselToSwitchActive;

	public bool saveRevertOnSwitchActiveVessel;

	public MissionCameraModeOptions cameraLockMode = MissionCameraModeOptions.Flight;

	public MissionCameraLockOptions cameraLockOptions = MissionCameraLockOptions.Unlock;

	public Guid historyId;

	public List<MissionMappedVessel> mappedVessels;

	public List<MissionMappedVessel> mappedVesselKeys;

	public byte[] hashBytes;

	public string signature;

	public bool isSimple;

	public bool registeredGameEvents;

	public List<uint> missingVessels;

	public List<string> blockedMessages;

	public Callback<Mission> generateLaunchSitesCallback;

	public List<MissionValidationTestResult> validationResults;

	public bool hasBeenValidated;

	public Guid id { get; set; }

	public string idName { get; set; }

	public string title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			base.gameObject.name = GetGameObjectName(this);
		}
	}

	public string ActiveNodeName => activeNodeName;

	public int Seed => seed;

	public string PersistentSaveName
	{
		get
		{
			if (MissionLoadedFromSFS)
			{
				return HighLogic.SaveFolder.Replace(MissionsUtils.SavesPath, "");
			}
			return MissionInfo.folderName;
		}
	}

	public bool MissionLoadedFromSFS
	{
		get
		{
			if (!HighLogic.LoadedSceneIsMissionBuilder && HighLogic.CurrentGame != null)
			{
				if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
				{
					if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
					{
						return HighLogic.LoadedScene == GameScenes.TRACKSTATION;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public string ShipsPath
	{
		get
		{
			if (MissionLoadedFromSFS)
			{
				return MissionsUtils.SavesRootPath + PersistentSaveName + "/Ships/";
			}
			if (MissionInfo == null)
			{
				return string.Empty;
			}
			return MissionInfo.ShipFolderPath;
		}
	}

	public string BannersPath
	{
		get
		{
			if (MissionLoadedFromSFS)
			{
				return MissionsUtils.SavesRootPath + PersistentSaveName + "/Banners/";
			}
			if (MissionInfo == null)
			{
				return string.Empty;
			}
			return MissionInfo.BannerPath;
		}
	}

	public MissionFileInfo MissionInfo
	{
		get
		{
			return _missionInfo;
		}
		set
		{
			_missionInfo = value;
		}
	}

	public MENode activeNode { get; set; }

	public MENode startNode { get; set; }

	public bool isInitialized { get; set; }

	public bool isStarted { get; set; }

	public bool isEnded { get; set; }

	public bool isSuccesful { get; set; }

	public List<MENode> InactiveEventNodes => inactiveEventNodes;

	public bool IsTutorialMission => packName == "squad_MakingHistory_Tutorial";

	public byte[] HashBytes => hashBytes;

	public string Signature => signature;

	public List<MissionValidationTestResult> ValidationResults => validationResults;

	public bool HasBeenValidated => hasBeenValidated;

	public void Awake()
	{
		InitSeed();
		id = Guid.NewGuid();
		situation = new MissionSituation(this);
		nodes = new DictionaryValueList<Guid, MENode>();
		tags = new List<string>();
		craftFileList = new DictionaryValueList<string, MissionCraft>();
		mappedVessels = new List<MissionMappedVessel>();
		orphanNodes = new List<MENode>();
		flow = new MissionFlow(this);
		currentScore = 0f;
		isSuccesful = false;
		isEnded = false;
		isStarted = false;
		isInitialized = false;
		hasBeenValidated = false;
		globalScore = new MissionScore(null);
		awards = new MissionAwards(null);
		flagURL = "Squad/Flags/default";
		bannerMenu = new MEBannerEntry(MEBannerType.Menu);
		bannerSuccess = new MEBannerEntry(MEBannerType.Success);
		bannerFail = new MEBannerEntry(MEBannerType.Fail);
		missingVessels = new List<uint>();
		order = int.MaxValue;
		GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			RegisterGameEvents();
		}
	}

	public void OnDestroy()
	{
		if (bannerMenu != null)
		{
			bannerMenu.DestroyTexture();
		}
		if (bannerSuccess != null)
		{
			bannerSuccess.DestroyTexture();
		}
		if (bannerFail != null)
		{
			bannerFail.DestroyTexture();
		}
		ClearCraftFiles();
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.SPACECENTER))
		{
			awards.StopTracking();
		}
		GameEvents.onGameSceneSwitchRequested.Remove(OnGameSceneSwitchRequested);
		GameEvents.onEditorStarted.Remove(OnEditorStarted);
		GameEvents.onPartVesselNamingChanged.Remove(OnPartVesselNamingChanged);
		GameEvents.onVesselPersistentIdChanged.Remove(OnVesselPersistentIdChanged);
		GameEvents.onPartPersistentIdChanged.Remove(OnPartPersistentIdChanged);
		GameEvents.onPartDeCoupleNewVesselComplete.Remove(OnPartDecoupleNewVesselComplete);
		GameEvents.onVesselRename.Remove(OnVesselRename);
	}

	public static Mission Spawn()
	{
		GameObject obj = new GameObject("Mission");
		obj.transform.SetParent(MissionSystem.MissionsGameObject.transform);
		Mission mission = obj.gameObject.AddComponent<Mission>();
		mission.isBriefingSet = false;
		return mission;
	}

	public static Mission Spawn(MissionFileInfo missionInfo)
	{
		Mission mission = Spawn();
		mission.MissionInfo = missionInfo;
		mission.gameObject.name = GetGameObjectName(missionInfo);
		return mission;
	}

	public static Mission SpawnAndLoad(MissionFileInfo missionInfo, ConfigNode missionNode)
	{
		Mission mission = Spawn();
		mission._missionInfo = missionInfo;
		mission.gameObject.name = GetGameObjectName(missionInfo);
		mission.Load(missionNode, simple: false);
		mission.InitMission();
		MEFlowParser.ParseMission(mission);
		return mission;
	}

	public void InitSeed()
	{
		seed = (int)(KSPUtil.SystemDateTime.DateTimeNow().Ticks & 0x7FFFFFFFL);
	}

	public static string GetGameObjectName(MissionFileInfo missionInfo)
	{
		string text = "";
		text = (string.IsNullOrEmpty(missionInfo.idName) ? Localizer.Format(missionInfo.title) : missionInfo.idName);
		return "Mission (" + text + ")";
	}

	public static string GetGameObjectName(Mission mission)
	{
		string text = "";
		text = (string.IsNullOrEmpty(mission.idName) ? Localizer.Format(mission.title) : mission.idName);
		return "Mission (" + text + ")";
	}

	public bool CheckHash()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			return true;
		}
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			return true;
		}
		Debug.LogError("[Mission]: Mission Save failed Validation Check.");
		return false;
	}

	public void RegenerateMissionID()
	{
		id = Guid.NewGuid();
		packName = "";
	}

	public void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> scenes)
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			nodes.ValuesList[i].ClearTestGroups();
		}
	}

	public void RegisterGameEvents()
	{
		GameEvents.onPartVesselNamingChanged.Add(OnPartVesselNamingChanged);
		GameEvents.onVesselPersistentIdChanged.Add(OnVesselPersistentIdChanged);
		GameEvents.onPartPersistentIdChanged.Add(OnPartPersistentIdChanged);
		GameEvents.onPartDeCoupleNewVesselComplete.Add(OnPartDecoupleNewVesselComplete);
		GameEvents.onVesselDocking.Add(OnVesselDocking);
		GameEvents.onVesselsUndocking.Add(OnVesselsUndocking);
		GameEvents.onVesselRename.Add(OnVesselRename);
		registeredGameEvents = true;
	}

	public void InitMission()
	{
		orphanNodes.Clear();
		SetStartNode();
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current = listEnumerator.Current;
				current.fromNodes.Clear();
				if (current.fromNodeIDs.Count > 0)
				{
					for (int i = 0; i < current.fromNodeIDs.Count; i++)
					{
						if (nodes.ContainsKey(current.fromNodeIDs[i]))
						{
							current.fromNodes.Add(nodes[current.fromNodeIDs[i]]);
						}
					}
				}
				else if (current.IsOrphanNode)
				{
					orphanNodes.Add(current);
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
		listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current2 = listEnumerator.Current;
				if (current2.toNodeIDs.Count <= 0)
				{
					continue;
				}
				for (int j = 0; j < current2.toNodeIDs.Count; j++)
				{
					if (nodes.ContainsKey(current2.toNodeIDs[j]) && nodes[current2.toNodeIDs[j]].fromNodes.Contains(current2))
					{
						current2.toNodes.AddUnique(nodes[current2.toNodeIDs[j]]);
					}
				}
				current2.toNodeIDs.Clear();
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
		listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current3 = listEnumerator.Current;
				for (int k = 0; k < current3.fromNodes.Count; k++)
				{
					current3.fromNodes[k].toNodes.AddUnique(current3);
				}
				if (!current3.isEndNode)
				{
					current3.toNodes.AddRange(orphanNodes);
					if (current3.IsOrphanNode)
					{
						current3.toNodes.Remove(current3);
					}
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
		listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current4 = listEnumerator.Current;
				if (current4.toNodes.Contains(current4))
				{
					current4.toNodes.Remove(current4);
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
		if (!isInitialized)
		{
			isInitialized = true;
			situation.InitSituation();
		}
		DictionaryValueList<VesselSituation, Guid> dictionaryValueList = new DictionaryValueList<VesselSituation, Guid>();
		for (int l = 0; l < situation.VesselSituationList.Count; l++)
		{
			if (!string.IsNullOrEmpty(situation.VesselSituationList.KeyAt(l).craftFile) && GetMissionCraftByFileName(situation.VesselSituationList.KeyAt(l).craftFile) == null)
			{
				dictionaryValueList.Add(situation.VesselSituationList.KeyAt(l), situation.VesselSituationList.At(l));
			}
		}
		for (int m = 0; m < dictionaryValueList.Count; m++)
		{
			Debug.LogWarning("[Mission]: Vessel " + dictionaryValueList.KeyAt(m).vesselName + " removed from mission as filename:" + dictionaryValueList.KeyAt(m).craftFile + " not found.");
			situation.VesselSituationList.Remove(dictionaryValueList.KeyAt(m));
		}
		if (dictionaryValueList.Count > 0)
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8006009", 10f);
		}
		if (HighLogic.LoadedScene != GameScenes.EDITOR)
		{
			GameEvents.onEditorStarted.Add(OnEditorStarted);
		}
		else
		{
			OnEditorStarted();
		}
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.SPACECENTER))
		{
			awards.StartTracking();
		}
	}

	public void SetStartNode()
	{
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current = listEnumerator.Current;
				if (current.isStartNode)
				{
					startNode = current;
					break;
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
	}

	public void SetActiveNode()
	{
		if (nodes.Contains(loadActiveNodeID))
		{
			activeNode = nodes[loadActiveNodeID];
		}
		else
		{
			activeNode = startNode;
		}
		activeNodeName = activeNode.Title;
		loadActiveNodeID = Guid.Empty;
	}

	public void SetDockedNodes()
	{
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current = listEnumerator.Current;
				if (nodes.ContainsKey(current.dockParentNodeID))
				{
					current.dockParentNode = nodes[current.dockParentNodeID];
				}
				if (current.dockedNodesIDsOnLoad.Count <= 0)
				{
					continue;
				}
				for (int i = 0; i < current.dockedNodesIDsOnLoad.Count; i++)
				{
					if (nodes.ContainsKey(current.dockedNodesIDsOnLoad[i]))
					{
						current.dockedNodes.AddUnique(nodes[current.dockedNodesIDsOnLoad[i]]);
						continue;
					}
					Debug.LogFormat("[Mission]: ({0}) SetDockedNodes found invalid docked Node reference from {1} contains reference to {2}, ignoring.", title, string.Concat(current.Title, "(", current.id, ")"), current.dockedNodesIDsOnLoad[i]);
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
	}

	public bool BlockPlayMission(bool showDialog = false)
	{
		if (blockedMessages == null)
		{
			blockedMessages = new List<string>();
		}
		else
		{
			blockedMessages.Clear();
		}
		bool flag = false;
		List<VesselSituation> allVesselSituations = GetAllVesselSituations();
		for (int i = 0; i < allVesselSituations.Count; i++)
		{
			if (!allVesselSituations[i].playerCreated && string.IsNullOrEmpty(allVesselSituations[i].craftFile))
			{
				string text = Localizer.Format("#autoLOC_8003065");
				if (allVesselSituations[i].node != null)
				{
					text += Localizer.Format("#autoLOC_8003066", allVesselSituations[i].node.Title);
				}
				blockedMessages.Add(text);
				flag = true;
			}
		}
		string text2 = Localizer.Format("#autoLOC_8003067");
		for (int j = 0; j < blockedMessages.Count; j++)
		{
			text2 = text2 + blockedMessages[j] + "\n";
		}
		text2 += Localizer.Format("#autoLOC_8003068");
		if (flag)
		{
			Debug.LogError("[Mission] Unable to be started due to blocking errors - " + text2.Replace("\n", "-").Replace("\t", " "));
			if (showDialog)
			{
				MissionEditorLogic.Instance.unableToStartMission = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("MissionStartBlocked", text2, Localizer.Format("#autoLOC_8003069"), null, 350f, new DialogGUIButton("#autoLOC_417274", null, dismissOnSelect: true)), persistAcrossScenes: false, null);
			}
		}
		return flag;
	}

	public void MissionCriticalError(string errorString = "")
	{
		FlightDriver.SetPause(pauseState: true);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("MissionStartBlocked", string.IsNullOrEmpty(errorString) ? Localizer.Format("#autoLOC_8003069") : errorString, Localizer.Format("#autoLOC_8003069"), null, 350f, new DialogGUIButton("#autoLOC_417274", ContinueCriticalError, dismissOnSelect: true)), persistAcrossScenes: false, null).OnDismiss = ContinueCriticalError;
	}

	public void ContinueCriticalError()
	{
		FlightGlobals.ClearAllVessels();
		if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)
		{
			FlightGlobals.fetch.activeVessel = null;
		}
		HighLogic.CurrentGame = null;
		FlightDriver.SetPause(pauseState: false);
		InputLockManager.ClearControlLocks();
		if (MissionSystem.IsTestMode)
		{
			MissionEditorLogic.StartUpMissionEditor((MissionInfo != null) ? MissionInfo.FilePath : "");
			return;
		}
		if (MissionInfo != null && Directory.Exists(MissionInfo.SaveFolderPath))
		{
			FileInfo[] files = new DirectoryInfo(MissionInfo.SaveFolderPath).GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
		}
		if (HighLogic.LoadedScene != GameScenes.MAINMENU)
		{
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
	}

	public IEnumerator GenerateMissionLaunchSites(Callback<Mission> onComplete, bool createPQSObject = true)
	{
		generateLaunchSitesCallback = onComplete;
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = Localizer.Format("#autoLOC_8002119");
		}
		List<string> missionLaunchSites = new List<string>();
		nodes.GetListEnumerator();
		List<MENode>.Enumerator nodeList = nodes.GetListEnumerator();
		try
		{
			while (nodeList.MoveNext())
			{
				MENode current = nodeList.Current;
				if (!current.IsLaunchPadNode || !current.IsDockedToStartNode)
				{
					continue;
				}
				ActionCreateLaunchSite actionLaunchSite = current.actionModules[0] as ActionCreateLaunchSite;
				if (!(actionLaunchSite != null))
				{
					continue;
				}
				missionLaunchSites.Add(actionLaunchSite.launchSiteSituation.LaunchSiteObjectName);
				LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(actionLaunchSite.launchSiteSituation.LaunchSiteObjectName);
				if (launchSite != null && launchSite.IsSetup)
				{
					continue;
				}
				if (launchSite != null)
				{
					PSystemSetup.Instance.RemoveLaunchSite(actionLaunchSite.launchSiteSituation.LaunchSiteObjectName);
				}
				GClass4 sphere = null;
				Transform sphereTarget = null;
				GameObject tempObject = new GameObject("TempLocation_" + actionLaunchSite.launchSiteSituation.LaunchSiteObjectName);
				if (createPQSObject)
				{
					for (int i = 0; i < PSystemSetup.Instance.pqsArray.Length; i++)
					{
						if (PSystemSetup.Instance.pqsArray[i].gameObject.name == actionLaunchSite.launchSiteSituation.launchSiteGroundLocation.targetBody.bodyName)
						{
							sphere = PSystemSetup.Instance.pqsArray[i];
							sphereTarget = sphere.target;
							sphere.SetTarget(null);
							FloatingOrigin.SetOffset(sphere.PrecisePosition);
							Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
							Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
							Vector3d vector3d = LatLon.GetSurfaceNVector(cf, actionLaunchSite.launchSiteSituation.launchSiteGroundLocation.latitude, actionLaunchSite.launchSiteSituation.launchSiteGroundLocation.longitude) * (sphere.radius + actionLaunchSite.launchSiteSituation.launchSiteGroundLocation.altitude);
							tempObject.transform.SetParent(sphere.transform);
							tempObject.transform.localPosition = vector3d;
							FloatingOrigin.SetOffset(tempObject.transform.position);
							yield return new WaitForFixedUpdate();
							break;
						}
					}
				}
				yield return StartCoroutine(actionLaunchSite.launchSiteSituation.AddLaunchSite(createPQSObject));
				if (createPQSObject)
				{
					sphere.SetTarget(sphereTarget);
				}
				UnityEngine.Object.Destroy(tempObject);
			}
		}
		finally
		{
			nodeList.Dispose();
		}
		LaunchSite[] nonStockLaunchSites = PSystemSetup.Instance.NonStockLaunchSites;
		for (int num = nonStockLaunchSites.Length - 1; num >= 0; num--)
		{
			if (!missionLaunchSites.Contains(nonStockLaunchSites[num].name))
			{
				PSystemSetup.Instance.RemoveLaunchSite(nonStockLaunchSites[num].name);
			}
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = "";
		}
		if (createPQSObject)
		{
			PSystemSetup.Instance.OnSceneChange(GameScenes.MAINMENU);
			PSystemSetup.Instance.OnLevelLoaded(GameScenes.MAINMENU);
		}
		yield return null;
		if (generateLaunchSitesCallback != null)
		{
			generateLaunchSitesCallback(this);
		}
	}

	public void StartMission()
	{
		if (CheckHash())
		{
			for (int i = 0; i < orphanNodes.Count; i++)
			{
				orphanNodes[i].InitializeTestGroups();
			}
			if (activeNode == null)
			{
				InitMission();
			}
			activeNode.ActivateNode(Loading: true);
			if (!isStarted && HighLogic.CurrentGame != null)
			{
				isStarted = true;
				startedUT = HighLogic.CurrentGame.UniversalTime;
				startNode.activatedUT = startedUT;
				AnalyticsUtil.LogMissionStart(this);
				GameEvents.Mission.onStarted.Fire(this);
			}
		}
	}

	public void ResumeMission()
	{
		if (!CheckHash() || !isStarted || isEnded || !isInitialized)
		{
			return;
		}
		for (int i = 0; i < orphanNodes.Count; i++)
		{
			orphanNodes[i].InitializeTestGroups();
		}
		if (briefingNodeActive)
		{
			MissionSystem.Instance.CheckFireBriefingDialog(this, chckMissionStarted: false);
		}
		activeNode.ActivateNode(Loading: true);
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.Mode != Game.Modes.MISSION || !HighLogic.LoadedSceneHasPlanetarium)
		{
			return;
		}
		for (int j = 0; j < nodes.ValuesList.Count; j++)
		{
			for (int k = 0; k < nodes.ValuesList[j].actionModules.Count; k++)
			{
				ActionModule actionModule = nodes.ValuesList[j].actionModules[k] as ActionModule;
				if (actionModule != null && actionModule.isRunning && actionModule.restartOnSceneLoad)
				{
					MissionSystem.Instance.StartCoroutine(actionModule.Fire());
				}
			}
		}
	}

	public void SetStartNode(MENode newStart)
	{
		startNode = newStart;
		newStart.isStartNode = true;
	}

	public void UpdateMission()
	{
		if (!registeredGameEvents)
		{
			RegisterGameEvents();
		}
		if (activeNode == null || !isStarted || !MissionSystem.IsActive || activeNode.isPaused || briefingNodeActive)
		{
			return;
		}
		if (activeNode.isEndNode)
		{
			DoEndMissionActions();
			return;
		}
		if (switchActiveVessel)
		{
			switchActiveVessel = false;
			if (HighLogic.LoadedSceneIsFlight)
			{
				FlightGlobals.SetActiveVessel(vesselToSwitchActive);
			}
			return;
		}
		if (saveRevertOnSwitchActiveVessel)
		{
			bool flag = false;
			if (briefingNodeActive)
			{
				flag = true;
				briefingNodeActive = false;
			}
			saveRevertOnSwitchActiveVessel = false;
			string value = GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, HighLogic.LoadedScene);
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs"))
			{
				File.Delete(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs");
			}
			if (!string.IsNullOrEmpty(value))
			{
				File.Copy(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs", KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/lastcreatevesselspawn.missionsfs", overwrite: true);
			}
			else
			{
				Debug.Log("[Mission]: Unable to save revert to vessel spawn for mission as autosave is not available.");
			}
			if (flag)
			{
				briefingNodeActive = true;
			}
		}
		if (PendingVesselLaunch(activeNode))
		{
			return;
		}
		for (int i = 0; i < activeNode.toNodes.Count; i++)
		{
			MENode mENode = activeNode.toNodes[i];
			if (!mENode.RunTests())
			{
				continue;
			}
			SwitchActiveNodes(mENode);
			i = activeNode.toNodes.Count;
			for (int j = 0; j < activeNode.dockedNodes.Count; j++)
			{
				MENode mENode2 = activeNode.dockedNodes[j];
				if (mENode2.HasTestModules)
				{
					if (mENode2.RunTests())
					{
						SwitchActiveNodes(mENode2);
						i = activeNode.toNodes.Count;
					}
				}
				else
				{
					mENode2.ActivateNode();
				}
			}
			if (mENode.isEndNode && !mENode.isPaused)
			{
				DoEndMissionActions();
			}
		}
	}

	public void DoEndMissionActions()
	{
		isSuccesful = activeNode.missionEndOptions == MissionEndOptions.Success;
		if (isScoreEnabled)
		{
			globalScore.AwardScore(this);
		}
		if (isSuccesful)
		{
			if (isScoreEnabled)
			{
				awards.EvaluateAwards(this);
			}
			AnalyticsUtil.LogMissionCompleted(this);
			GameEvents.Mission.onCompleted.Fire(this);
		}
		else
		{
			AnalyticsUtil.LogMissionFailed(this);
			GameEvents.Mission.onFailed.Fire(this);
		}
		GameEvents.Mission.onFinished.Fire(this);
		isEnded = true;
		GameEvents.onEditorStarted.Remove(OnEditorStarted);
	}

	public void SwitchActiveNodes(MENode newNode)
	{
		MENode mENode = activeNode;
		GameEvents.Mission.onActiveNodeChanging.Fire(this, new GameEvents.FromToAction<MENode, MENode>(mENode, newNode));
		mENode.DeactivateNode();
		activeNode = newNode.ActivateNode();
		GameEvents.Mission.onActiveNodeChanged.Fire(this, new GameEvents.FromToAction<MENode, MENode>(mENode, newNode));
	}

	public void SetNextObjectives(MENode node)
	{
		if (nextObjectivesNodes == null)
		{
			nextObjectivesNodes = new List<MENode>();
		}
		else
		{
			nextObjectivesNodes.Clear();
		}
		if (!flow.NodePaths.ContainsKey(node))
		{
			return;
		}
		MENodePathInfo mENodePathInfo = flow.NodePaths[node];
		for (int i = 0; i < mENodePathInfo.paths.Count; i++)
		{
			MEPath mEPath = mENodePathInfo.paths[i];
			for (int j = 0; j < mEPath.Nodes.Count && (!mEPath.Nodes[j].isEvent || mEPath.Nodes[j].HasBeenActivated); j++)
			{
				if (mEPath.Nodes[j].isObjective)
				{
					nextObjectivesNodes.AddUnique(mEPath.Nodes[j]);
					break;
				}
			}
		}
	}

	public bool IsNextObjective(MENode node)
	{
		if (nextObjectivesNodes != null && nextObjectivesNodes.Contains(node))
		{
			return true;
		}
		return false;
	}

	public void RemoveInactiveEvent(MENode node)
	{
		if (inactiveEventNodes != null && inactiveEventNodes.Contains(node))
		{
			inactiveEventNodes.Remove(node);
		}
	}

	public void UpdateOrphanNodeState(MENode node, bool makeOrphan)
	{
		if (makeOrphan && node.IsOrphanNode)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes.ValuesList[i] != node)
				{
					nodes.ValuesList[i].toNodes.AddUnique(node);
				}
			}
			orphanNodes.AddUnique(node);
			return;
		}
		for (int j = 0; j < nodes.Count; j++)
		{
			if (!node.fromNodes.Contains(nodes.ValuesList[j]))
			{
				nodes.ValuesList[j].toNodes.Remove(node);
			}
		}
		orphanNodes.Remove(node);
	}

	public string PrintObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode)
	{
		return PrintObjectives(onlyPrintActivatedNodes, startWithActiveNode, onlyPrintScore: false, onlyAwardedScores: false);
	}

	public string PrintScoreObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode, bool onlyAwardedScores)
	{
		return PrintObjectives(onlyPrintActivatedNodes, startWithActiveNode, onlyPrintScore: true, onlyAwardedScores);
	}

	public string PrintObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode, bool onlyPrintScore, bool onlyAwardedScores)
	{
		string objectiveString = "";
		List<MENode> visitedNodesList = new List<MENode>();
		if (startWithActiveNode)
		{
			visitedNodesList.Add(activeNode);
			for (int i = 0; i < activeNode.toNodes.Count; i++)
			{
				if (onlyPrintScore)
				{
					RecursivelyCreateObjectiveScoreString(activeNode.toNodes[i], ref objectiveString, onlyPrintActivatedNodes, onlyAwardedScores, ref visitedNodesList, rejectOrphanNodes: true);
				}
				else
				{
					RecursivelyCreateObjectiveString(activeNode.toNodes[i], ref objectiveString, "", onlyPrintActivatedNodes, ref visitedNodesList, rejectOrphanNodes: true);
				}
			}
		}
		else if (onlyPrintScore)
		{
			RecursivelyCreateObjectiveScoreString(startNode, ref objectiveString, onlyPrintActivatedNodes, onlyAwardedScores, ref visitedNodesList, rejectOrphanNodes: true);
		}
		else
		{
			RecursivelyCreateObjectiveString(startNode, ref objectiveString, "", onlyPrintActivatedNodes, ref visitedNodesList, rejectOrphanNodes: true);
		}
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			MENode current = listEnumerator.Current;
			if (current.fromNodeIDs.Count == 0 && !current.isStartNode)
			{
				if (onlyPrintScore)
				{
					RecursivelyCreateObjectiveScoreString(current, ref objectiveString, onlyPrintActivatedNodes, onlyAwardedScores, ref visitedNodesList, rejectOrphanNodes: false);
				}
				else
				{
					RecursivelyCreateObjectiveString(current, ref objectiveString, "", onlyPrintActivatedNodes, ref visitedNodesList, rejectOrphanNodes: false);
				}
			}
		}
		if (objectiveString == "")
		{
			objectiveString = "#autoLOC_8100310";
		}
		return objectiveString;
	}

	public void RecursivelyCreateObjectiveString(MENode currentNode, ref string objectiveString, string indent, bool onlyPrintActivatedNodes, ref List<MENode> visitedNodesList, bool rejectOrphanNodes)
	{
		if (currentNode == null || (onlyPrintActivatedNodes && !currentNode.isStartNode && !currentNode.HasBeenActivated) || (rejectOrphanNodes && currentNode.IsOrphanNode))
		{
			return;
		}
		if (currentNode.isObjective)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			stringBuilder.Append(indent).Append("<color=#EDFEAAFF>• ").Append(Localizer.Format(currentNode.ObjectiveString))
				.Append("</color> ");
			if (currentNode.HasBeenActivated)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_8001017", KSPUtil.PrintTimeCompact(currentNode.activatedUT, explicitPositive: false)));
			}
			stringBuilder.Append("\n");
			objectiveString += stringBuilder.ToStringAndRelease();
			indent += "\t";
		}
		visitedNodesList.Add(currentNode);
		for (int i = 0; i < currentNode.toNodes.Count; i++)
		{
			if (!visitedNodesList.Contains(currentNode.toNodes[i]))
			{
				RecursivelyCreateObjectiveString(currentNode.toNodes[i], ref objectiveString, indent, onlyPrintActivatedNodes, ref visitedNodesList, rejectOrphanNodes: true);
			}
		}
	}

	public void RecursivelyCreateObjectiveScoreString(MENode currentNode, ref string objectiveString, bool onlyPrintActivatedNodes, bool onlyAwardedScores, ref List<MENode> visitedNodesList, bool rejectOrphanNodes)
	{
		if (currentNode == null || (onlyPrintActivatedNodes && !currentNode.isStartNode && !currentNode.HasBeenActivated) || (rejectOrphanNodes && currentNode.IsOrphanNode))
		{
			return;
		}
		bool flag = false;
		if (currentNode.isObjective)
		{
			foreach (MENode dockedNode in currentNode.dockedNodes)
			{
				if (dockedNode.toNodes.Count < 1 || dockedNode.toNodes[0].actionModules.Count <= 0 || (!(dockedNode.toNodes[0].actionModules[0].GetType() == typeof(ActionMissionScore)) && !dockedNode.toNodes[0].actionModules[0].GetType().IsSubclassOf(typeof(ActionMissionScore))))
				{
					continue;
				}
				StringBuilder stringBuilder = StringBuilderCache.Acquire();
				if (!flag)
				{
					stringBuilder.Append("<color=#EDFEAAFF>• ").Append(Localizer.Format(currentNode.ObjectiveString)).Append("</color> ");
					if (currentNode.HasBeenActivated)
					{
						stringBuilder.Append(Localizer.Format("#autoLOC_8001017", KSPUtil.PrintTimeCompact(currentNode.activatedUT, explicitPositive: false)));
					}
				}
				if (onlyAwardedScores && dockedNode.HasBeenActivated)
				{
					stringBuilder.Append("\n" + (dockedNode.toNodes[0].actionModules[0] as ActionMissionScore).GetAwardedScoreDescription());
				}
				else if (!onlyAwardedScores)
				{
					stringBuilder.Append((dockedNode.toNodes[0].actionModules[0] as ActionMissionScore).GetScoreDescription());
				}
				visitedNodesList.Add(dockedNode.toNodes[0]);
				objectiveString += stringBuilder.ToStringAndRelease();
				flag = true;
			}
			if (flag)
			{
				objectiveString += "\n\n";
			}
			if (!flag && onlyAwardedScores && (currentNode.actionModules.Count <= 0 || (!(currentNode.actionModules[0].GetType() == typeof(ActionMissionScore)) && !currentNode.actionModules[0].GetType().IsSubclassOf(typeof(ActionMissionScore)))))
			{
				StringBuilder stringBuilder2 = StringBuilderCache.Acquire();
				stringBuilder2.Append("<color=#EDFEAAFF>• ").Append(Localizer.Format(currentNode.ObjectiveString)).Append("</color> ");
				if (currentNode.HasBeenActivated)
				{
					stringBuilder2.Append(Localizer.Format("#autoLOC_8001017", KSPUtil.PrintTimeCompact(currentNode.activatedUT, explicitPositive: false)));
				}
				stringBuilder2.Append("\n\n");
				objectiveString += stringBuilder2.ToStringAndRelease();
			}
		}
		if (!flag && currentNode.dockParentNode == null && currentNode.actionModules.Count > 0 && (currentNode.actionModules[0].GetType() == typeof(ActionMissionScore) || currentNode.actionModules[0].GetType().IsSubclassOf(typeof(ActionMissionScore))))
		{
			StringBuilder stringBuilder3 = StringBuilderCache.Acquire();
			if (onlyAwardedScores && currentNode.HasBeenActivated)
			{
				stringBuilder3.Append("<color=#EDFEAAFF>• ").Append(Localizer.Format(currentNode.ObjectiveString)).Append("</color>");
				stringBuilder3.Append("\n" + (currentNode.actionModules[0] as ActionMissionScore).GetAwardedScoreDescription());
				stringBuilder3.Append("\n\n");
			}
			else if (!onlyAwardedScores)
			{
				stringBuilder3.Append("<color=#EDFEAAFF>• ").Append(Localizer.Format(currentNode.ObjectiveString)).Append("</color>");
				stringBuilder3.Append((currentNode.actionModules[0] as ActionMissionScore).GetScoreDescription());
				stringBuilder3.Append("\n\n");
			}
			objectiveString += stringBuilder3.ToStringAndRelease();
		}
		visitedNodesList.Add(currentNode);
		for (int i = 0; i < currentNode.toNodes.Count; i++)
		{
			if (!visitedNodesList.Contains(currentNode.toNodes[i]))
			{
				RecursivelyCreateObjectiveScoreString(currentNode.toNodes[i], ref objectiveString, onlyPrintActivatedNodes, onlyAwardedScores, ref visitedNodesList, rejectOrphanNodes: true);
			}
		}
	}

	public void OnEditorStarted()
	{
		if ((bool)ResearchAndDevelopment.Instance && HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			ResearchAndDevelopment.Instance.SetScience(1337f, TransactionReasons.Cheating);
			ResearchAndDevelopment.Instance.CheatTechnology();
		}
	}

	public MissionFileInfo UpdateMissionFileInfo(string MissionFilePath)
	{
		MissionInfo = MissionFileInfo.CreateFromPath(MissionFilePath);
		return MissionInfo;
	}

	public void OnPartVesselNamingChanged(Part part)
	{
		if (part == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].partPersistentId == part.persistentId)
			{
				mappedVessels[i].partVesselName = part.vesselNaming.vesselName;
				if (part.vessel != null)
				{
					mappedVessels[i].currentVesselPersistentId = part.vessel.persistentId;
				}
				flag = true;
			}
		}
		if ((bool)MissionsApp.Instance && flag)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> hostedFromTo)
	{
		if (hostedFromTo.host == null)
		{
			return;
		}
		bool flag = false;
		List<VesselSituation> allVesselSituations = GetAllVesselSituations();
		for (int i = 0; i < allVesselSituations.Count; i++)
		{
			if (allVesselSituations[i].persistentId == hostedFromTo.host.persistentId)
			{
				allVesselSituations[i].vesselName = hostedFromTo.to;
				flag = true;
			}
		}
		for (int j = 0; j < mappedVessels.Count; j++)
		{
			if (mappedVessels[j].currentVesselPersistentId == hostedFromTo.host.persistentId)
			{
				mappedVessels[j].situationVesselName = hostedFromTo.to;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnPartDecoupleNewVesselComplete(Vessel oldVessel, Vessel newVessel)
	{
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].currentVesselPersistentId == oldVessel.persistentId && (bool)newVessel.PartsContain(mappedVessels[i].partPersistentId))
			{
				mappedVessels[i].currentVesselPersistentId = newVessel.persistentId;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].currentVesselPersistentId == oldId)
			{
				mappedVessels[i].currentVesselPersistentId = newId;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].currentVesselPersistentId == vesselID && mappedVessels[i].partPersistentId == oldId)
			{
				mappedVessels[i].partPersistentId = newId;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnVesselDocking(uint oldId, uint newId)
	{
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].currentVesselPersistentId == oldId)
			{
				mappedVessels[i].currentVesselPersistentId = newId;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		bool flag = false;
		for (int i = 0; i < mappedVessels.Count; i++)
		{
			if (mappedVessels[i].currentVesselPersistentId == oldVessel.persistentId && (bool)newVessel.PartsContain(mappedVessels[i].partPersistentId))
			{
				mappedVessels[i].currentVesselPersistentId = newVessel.persistentId;
				flag = true;
			}
		}
		if (flag && (bool)MissionsApp.Instance)
		{
			MissionsApp.Instance.RefreshMissionRequested(this, changePositions: false);
		}
	}

	public void RebuildCraftFileList()
	{
		ClearCraftFiles();
		if (mappedVessels == null)
		{
			mappedVessels = new List<MissionMappedVessel>();
		}
		mappedVesselKeys = new List<MissionMappedVessel>();
		ProcessCraftFileFolder(ShipsPath + "VAB/");
		ProcessCraftFileFolder(ShipsPath + "SPH/");
		ClearMappedVessels();
	}

	public void ClearCraftFiles()
	{
		if (craftFileList != null && craftFileList.Count > 0)
		{
			for (int i = 0; i < craftFileList.Count; i++)
			{
				craftFileList.At(i).Clear();
			}
			craftFileList.Clear();
		}
	}

	public bool UpdateFromMappedVesselIDs(uint vesselId, ref string vesselName, ref VesselSituation vesselSituation)
	{
		int num = mappedVessels.IndexMappedVesselId(vesselId);
		if (num != -1)
		{
			vesselName = mappedVessels[num].partVesselName;
			vesselSituation = GetVesselSituationByVesselID(mappedVessels[num].currentVesselPersistentId);
			return true;
		}
		return false;
	}

	public void ClearMappedVessels()
	{
		int count = mappedVessels.Count;
		while (count-- > 0)
		{
			if (!mappedVesselKeys.Exists(mappedVessels[count].currentVesselPersistentId, mappedVessels[count].partPersistentId, mappedVessels[count].partVesselName, mappedVessels[count].craftFileName))
			{
				mappedVessels.RemoveAt(count);
			}
		}
	}

	public void ProcessCraftFileFolder(string saveShipFolderPath)
	{
		if (Directory.Exists(saveShipFolderPath))
		{
			string[] files = Directory.GetFiles(saveShipFolderPath, "*.craft");
			for (int i = 0; i < files.Length; i++)
			{
				string fileName = Path.GetFileName(files[i]);
				MissionCraft craftFile = new MissionCraft(saveShipFolderPath, fileName);
				ProcessCraftRenames(fileName, craftFile);
			}
		}
	}

	public void ProcessCraftRenames(string craftFileName, MissionCraft craftFile)
	{
		if (craftFile == null)
		{
			return;
		}
		ConfigNode craftNode = craftFile.CraftNode;
		GetAllVesselSituations();
		uint value = 0u;
		VesselSituation missionSituationByCraftFileName = GetMissionSituationByCraftFileName(craftFile.craftFile);
		if (missionSituationByCraftFileName != null)
		{
			value = missionSituationByCraftFileName.persistentId;
		}
		else
		{
			craftFile.CraftNode.TryGetValue("persistentId", ref value);
		}
		if (craftNode == null || value == 0)
		{
			return;
		}
		string value2 = "";
		if (!craftNode.TryGetValue("ship", ref value2))
		{
			return;
		}
		ConfigNode[] array = craftNode.GetNodes("PART");
		for (int i = 0; i < array.Length; i++)
		{
			uint num = 0u;
			string value3 = "";
			num = craftFile.GetPartPersistentId(array[i]);
			ConfigNode node = new ConfigNode();
			if (array[i].TryGetNode("VESSELNAMING", ref node))
			{
				node.TryGetValue("name", ref value3);
				if (!string.IsNullOrEmpty(value3))
				{
					MissionMappedVessel missionMappedVessel = mappedVessels.Find(value, num, value3, craftFile.craftFile);
					if (missionMappedVessel == null)
					{
						missionMappedVessel = AddMappedPartVesselName(num, value, value3, craftFile, mappedItem: true, missionSituationByCraftFileName);
					}
					if (!mappedVesselKeys.Exists(value, num, value3, craftFile.craftFile))
					{
						mappedVesselKeys.AddUnique(missionMappedVessel);
					}
					if (i == 0)
					{
						AddMappedCraft(craftFileName, craftFile);
					}
				}
			}
			else if (i == 0)
			{
				AddMappedPartVesselName(num, value, craftFileName, craftFile, mappedItem: false, missionSituationByCraftFileName);
			}
		}
	}

	public MissionMappedVessel AddMappedPartVesselName(uint partId, uint vslPersistentId, string craftFileName, MissionCraft craftFile, bool mappedItem, VesselSituation vesselSituation)
	{
		string text = (mappedItem ? craftFileName : craftFile.VesselName);
		MissionCraft missionCraft = new MissionCraft(craftFile.craftFolder, craftFile.craftFile);
		missionCraft.mappedVesselName = text;
		missionCraft.mappedItem = mappedItem;
		if (!mappedItem && craftFileList.ContainsKey(craftFileName))
		{
			craftFileList.Remove(craftFileName);
		}
		MissionMappedVessel missionMappedVessel = new MissionMappedVessel(partId, vesselSituation?.persistentId ?? vslPersistentId, vslPersistentId, text, craftFile.craftFile, (vesselSituation != null) ? Localizer.Format(vesselSituation.vesselName) : "");
		mappedVesselKeys.AddUnique(missionMappedVessel);
		if (!mappedItem)
		{
			craftFileList.Add(craftFileName, missionCraft);
		}
		if (!mappedVessels.Exists(vslPersistentId, partId, text, craftFile.craftFile))
		{
			mappedVessels.Add(missionMappedVessel);
		}
		return missionMappedVessel;
	}

	public void AddMappedCraft(string craftFileName, MissionCraft craftFile)
	{
		string vesselName = craftFile.VesselName;
		MissionCraft missionCraft = new MissionCraft(craftFile.craftFolder, craftFile.craftFile);
		missionCraft.mappedVesselName = vesselName;
		missionCraft.mappedItem = false;
		if (craftFileList.ContainsKey(craftFileName))
		{
			craftFileList.Remove(craftFileName);
		}
		craftFileList.Add(craftFileName, missionCraft);
	}

	public MissionCraft GetCraftBySituationsVesselID(uint persistentId)
	{
		persistentId = mappedVessels.ConvertMappedId(persistentId);
		DictionaryValueList<VesselSituation, Guid> allVesselSituationsGuid = GetAllVesselSituationsGuid();
		int num = 0;
		while (true)
		{
			if (num < allVesselSituationsGuid.Count)
			{
				if (allVesselSituationsGuid.KeyAt(num).persistentId == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		if (craftFileList.ContainsKey(allVesselSituationsGuid.KeyAt(num).craftFile))
		{
			return craftFileList[allVesselSituationsGuid.KeyAt(num).craftFile];
		}
		return null;
	}

	public MissionCraft GetMissionCraftByFileName(string craftFileName)
	{
		MissionCraft val = null;
		craftFileList.TryGetValue(craftFileName, out val);
		return val;
	}

	public VesselSituation GetMissionSituationByCraftFileName(string craftFileName)
	{
		List<VesselSituation> allVesselSituations = GetAllVesselSituations();
		int num = 0;
		while (true)
		{
			if (num < allVesselSituations.Count)
			{
				if (string.Equals(allVesselSituations[num].craftFile, craftFileName))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return allVesselSituations[num];
	}

	public MissionCraft GetMissionCraftByName(string vesselName)
	{
		int num = 0;
		while (true)
		{
			if (num < craftFileList.Count)
			{
				if (craftFileList.At(num).VesselName == vesselName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return craftFileList.At(num);
	}

	public int GetSituationsIndexByVessel(uint persistentId)
	{
		persistentId = mappedVessels.ConvertMappedId(persistentId);
		List<VesselSituation> allVesselSituations = GetAllVesselSituations();
		int num = 0;
		while (true)
		{
			if (num < allVesselSituations.Count)
			{
				if (allVesselSituations[num].persistentId == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public int GetSituationsIndexByPart(uint persistentId)
	{
		List<VesselSituation> allVesselSituations = GetAllVesselSituations();
		for (int i = 0; i < allVesselSituations.Count; i++)
		{
			MissionCraft missionCraftByFileName = GetMissionCraftByFileName(allVesselSituations[i].craftFile);
			if (missionCraftByFileName == null)
			{
				continue;
			}
			ConfigNode[] array = missionCraftByFileName.CraftNode.GetNodes("PART");
			int j = 0;
			for (int num = array.Length; j < num; j++)
			{
				if (missionCraftByFileName.GetPartPersistentId(array[j]) == persistentId)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public int GetCraftFileIndexByPart(uint persistentId)
	{
		int i = 0;
		for (int count = craftFileList.Count; i < count; i++)
		{
			if (craftFileList.At(i).CraftNode == null)
			{
				continue;
			}
			ConfigNode[] array = craftFileList.At(i).CraftNode.GetNodes("PART");
			int j = 0;
			for (int num = array.Length; j < num; j++)
			{
				if (craftFileList.At(i).GetPartPersistentId(array[j]) == persistentId)
				{
					return i;
				}
			}
		}
		return 0;
	}

	public void SwitchActiveVessel(Vessel vessel, bool saveRevertOnSwitch = false, bool switchImmediately = false)
	{
		if (HighLogic.LoadedSceneIsFlight && vessel != null)
		{
			if (switchImmediately)
			{
				FlightGlobals.ForceSetActiveVessel(MissionSystem.Instance.returnVessel);
				saveRevertOnSwitchActiveVessel = saveRevertOnSwitch;
			}
			else
			{
				vesselToSwitchActive = vessel;
				switchActiveVessel = true;
				saveRevertOnSwitchActiveVessel = saveRevertOnSwitch;
			}
		}
	}

	public void SetLockedCamera(MissionCameraModeOptions newCameraMode, MissionCameraLockOptions newCameraLock)
	{
		cameraLockMode = newCameraMode;
		cameraLockOptions = newCameraLock;
		MissionSystem.SetLockedCamera(this, cameraLockMode, cameraLockOptions);
	}

	public bool Export(string exportFileName, bool overwrite = false)
	{
		if (MissionInfo == null)
		{
			Debug.LogError("Unable to Export Mission (" + Localizer.Format(title) + "). It hasn't been saved yet!");
			return false;
		}
		if (!Directory.Exists(MissionsUtils.MissionExportsPath))
		{
			Directory.CreateDirectory(MissionsUtils.MissionExportsPath);
		}
		KSPCompression.CompressDirectory(MissionInfo.FolderPath, MissionsUtils.MissionExportsPath + exportFileName, includeTopLevelFolder: true, overwrite);
		missionNameAtLastExport = base.name;
		return true;
	}

	public void Load(ConfigNode node)
	{
		Load(node, simple: false);
	}

	public void Load(ConfigNode node, bool simple = false)
	{
		if (node == null)
		{
			return;
		}
		hasBeenValidated = false;
		ConfigNode configNode = new ConfigNode();
		if (!(node.name == "MISSION") && !(node.name == "MISSIONTOSTART"))
		{
			if (node.HasNode("MISSION"))
			{
				configNode = node.GetNode("MISSION");
			}
		}
		else
		{
			configNode = node;
		}
		if (configNode.HasValue("id"))
		{
			id = new Guid(configNode.GetValue("id"));
		}
		if (configNode.HasValue("idName"))
		{
			idName = configNode.GetValue("idName");
		}
		configNode.TryGetValue("expansionVersion", ref expansionVersion);
		string value = "";
		configNode.TryGetValue("title", ref value);
		title = value;
		configNode.TryGetValue("briefing", ref briefing);
		configNode.TryGetValue("author", ref author);
		configNode.TryGetValue("modsBriefing", ref modsBriefing);
		configNode.TryGetValue("isBriefingSet", ref isBriefingSet);
		configNode.TryGetValue("packName", ref packName);
		configNode.TryGetValue("order", ref order);
		configNode.TryGetValue("hardIcon", ref hardIcon);
		configNode.TryGetEnum("difficulty", ref difficulty, hardIcon ? MissionDifficulty.Advanced : MissionDifficulty.Beginner);
		if (!configNode.TryGetValue("seed", ref seed))
		{
			InitSeed();
		}
		configNode.TryGetValue("historyId", ref historyId);
		configNode.TryGetValue("steamPublishedFileId", ref steamPublishedFileId);
		configNode.TryGetValue("flag", ref flagURL);
		string value2 = string.Empty;
		configNode.TryGetValue("bannerMenu", ref value2);
		bannerMenu.LoadFromMissionFolder(value2, MEBannerType.Menu, this);
		string value3 = string.Empty;
		configNode.TryGetValue("bannerSuccess", ref value3);
		bannerSuccess.LoadFromMissionFolder(value3, MEBannerType.Success, this);
		string value4 = string.Empty;
		configNode.TryGetValue("bannerFail", ref value4);
		configNode.TryGetValue("isScoreEnabled", ref isScoreEnabled);
		configNode.TryGetValue("maxScore", ref maxScore);
		bannerFail.LoadFromMissionFolder(value4, MEBannerType.Fail, this);
		configNode.TryGetValue("activeNodeID", ref loadActiveNodeID);
		configNode.TryGetValue("currentScore", ref currentScore);
		configNode.TryGetValue("exportName", ref exportName);
		configNode.TryGetValue("missionNameAtLastExport", ref missionNameAtLastExport);
		if (configNode.HasValue("isStarted"))
		{
			isStarted = Convert.ToBoolean(configNode.GetValue("isStarted"));
		}
		if (configNode.HasValue("isEnded"))
		{
			isEnded = Convert.ToBoolean(configNode.GetValue("isEnded"));
		}
		if (configNode.HasValue("isSuccesful"))
		{
			isSuccesful = Convert.ToBoolean(configNode.GetValue("isSuccesful"));
		}
		configNode.TryGetEnum("cameraLockMode", ref cameraLockMode, MissionCameraModeOptions.NoChange);
		configNode.TryGetEnum("cameraLockOptions", ref cameraLockOptions, MissionCameraLockOptions.NoChange);
		configNode.TryGetValue("saveRevertOnSwitchActiveVessel", ref saveRevertOnSwitchActiveVessel);
		ConfigNode node2 = new ConfigNode();
		if (configNode.TryGetNode("TAGS", ref node2))
		{
			ConfigNode[] array = node2.GetNodes("TAG");
			for (int i = 0; i < array.Length; i++)
			{
				string value5 = "";
				array[i].TryGetValue("name", ref value5);
				tags.Add(value5);
			}
		}
		configNode.TryGetValue("briefingNodeActive", ref briefingNodeActive);
		globalScore.Load(configNode);
		awards.Load(configNode);
		if (configNode.HasNode("MAPPEDVESSELS"))
		{
			mappedVessels = MissionMappedVessel.Load(configNode.GetNode("MAPPEDVESSELS"));
			if (mappedVesselKeys == null)
			{
				mappedVesselKeys = new List<MissionMappedVessel>();
			}
			for (int j = 0; j < mappedVessels.Count; j++)
			{
				MissionMappedVessel missionMappedVessel = new MissionMappedVessel(mappedVessels[j].partPersistentId, mappedVessels[j].originalVesselPersistentId, mappedVessels[j].currentVesselPersistentId, mappedVessels[j].partVesselName, mappedVessels[j].craftFileName, mappedVessels[j].situationVesselName);
				missionMappedVessel.mappedVesselPersistentId = mappedVessels[j].mappedVesselPersistentId;
				mappedVesselKeys.AddUnique(missionMappedVessel);
			}
		}
		if (configNode.HasNode("NODES"))
		{
			ConfigNode node3 = new ConfigNode();
			if (configNode.TryGetNode("SITUATION", ref node3))
			{
				situation.Load(node3);
				if (expansionVersion == "" && node3.HasValue("version"))
				{
					expansionVersion = "1.2.0";
				}
			}
			ConfigNode node4 = new ConfigNode();
			if (configNode.TryGetNode("NODES", ref node4))
			{
				inactiveEventNodes = new List<MENode>();
				ConfigNode[] array2 = node4.GetNodes("NODE");
				foreach (ConfigNode node5 in array2)
				{
					MENode mENode = MENode.Spawn(this);
					mENode.Load(node5);
					if (mENode.isEvent && !mENode.HasBeenActivated)
					{
						inactiveEventNodes.AddUnique(mENode);
					}
					nodes.Add(mENode.id, mENode);
				}
			}
			if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
			{
				ConfigNode[] array2 = situation.vesselsToBuildNode.GetNodes("VESSELTOBUILD");
				foreach (ConfigNode obj in array2)
				{
					Guid value6 = Guid.Empty;
					obj.TryGetValue("NodeGuid", ref value6);
					if (!(value6 != Guid.Empty))
					{
						continue;
					}
					MENode nodeById = GetNodeById(value6);
					if (nodeById != null)
					{
						ActionCreateVessel actionCreateVessel = GetActionCreateVessel(nodeById);
						if (actionCreateVessel != null)
						{
							situation.vesselSituationList.Add(actionCreateVessel.vesselSituation, value6);
						}
					}
				}
				array2 = situation.startingActionsNode.GetNodes("STARTINGACTION");
				foreach (ConfigNode obj2 in array2)
				{
					Guid value7 = Guid.Empty;
					obj2.TryGetValue("NodeGuid", ref value7);
					if (!(value7 != Guid.Empty))
					{
						continue;
					}
					MENode nodeById2 = GetNodeById(value7);
					if (!(nodeById2 != null))
					{
						continue;
					}
					ActionCreateKerbal actionCreateKerbal = GetActionCreateKerbal(nodeById2);
					if (actionCreateKerbal != null)
					{
						situation.startingActions.Add(actionCreateKerbal);
						continue;
					}
					ActionCreateAsteroid actionCreateAsteroid = GetActionCreateAsteroid(nodeById2);
					if (actionCreateAsteroid != null)
					{
						situation.startingActions.Add(actionCreateAsteroid);
						continue;
					}
					ActionCreateComet actionCreateComet = GetActionCreateComet(nodeById2);
					if (actionCreateComet != null)
					{
						situation.startingActions.Add(actionCreateComet);
						continue;
					}
					ActionCreateFlag actionCreateFlag = GetActionCreateFlag(nodeById2);
					if (actionCreateFlag != null)
					{
						situation.startingActions.Add(actionCreateFlag);
					}
				}
			}
			if (!simple)
			{
				RebuildCraftFileList();
				if (KSPUtil.CheckVersion(expansionVersion, 1, 10, 0) == VersionCompareResult.INCOMPATIBLE_TOO_EARLY)
				{
					ProcessMappedVesselNodeMap();
				}
			}
			SetStartNode();
			SetActiveNode();
			SetDockedNodes();
			if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionInfo == null)
			{
				string text = "";
				if (MissionSystem.IsTestMode && !string.IsNullOrEmpty(MissionEditorLogic.MissionToTest))
				{
					text = MissionEditorLogic.MissionToTest;
				}
				else
				{
					string text2 = HighLogic.SaveFolder.Substring(MissionsUtils.SavesPath.Length);
					text = ((packName == null || !packName.StartsWith("squad_")) ? MissionsUtils.UsersMissionsPath : MissionsUtils.StockMissionsPath) + text2 + "/persistent.mission";
					if (!File.Exists(text))
					{
						text = ((packName == null || !packName.StartsWith("squad_")) ? MissionsUtils.StockMissionsPath : MissionsUtils.UsersMissionsPath) + text2 + "/persistent.mission";
					}
					if (!File.Exists(text) && packName != null && packName.StartsWith("squad_"))
					{
						text = MissionsUtils.BaseMissionsPath + text2 + "/persistent.mission";
					}
					if (!File.Exists(text))
					{
						text = SteamManager.KSPSteamWorkshopFolder + text2 + "/persistent.mission";
					}
				}
				if (File.Exists(text))
				{
					UpdateMissionFileInfo(text);
				}
				else
				{
					Debug.LogError("Failed to load mission. Unable to find Mission folder");
				}
			}
			if (configNode.HasNode("FLOW"))
			{
				try
				{
					flow = new MissionFlow(this);
					flow.Load(configNode.GetNode("FLOW"));
					SetNextObjectives(activeNode);
				}
				catch (Exception)
				{
					Debug.LogWarning("Unable to load mission flow from ConfigNode");
				}
			}
		}
		else
		{
			Debug.LogError("Unable to load mission from ConfigNode");
		}
		hashBytes = ExpansionsLoader.BuildMissionFileHash(configNode);
		if (MissionInfo != null)
		{
			if (MissionInfo.missionType == MissionTypes.Base && File.Exists(MissionInfo.FolderPath + "signature"))
			{
				signature = File.ReadAllText(MissionInfo.FolderPath + "signature");
			}
		}
		else if (configNode.HasValue("signature"))
		{
			signature = configNode.GetValue("signature");
		}
		if (!simple && !ExpansionsLoader.IsExpansionInstalled("MakingHistory") && !CheckHash())
		{
			Debug.LogError("[Mission]: Mission Save failed Validation Check.");
		}
	}

	public void ProcessMappedVesselNodeMap(uint mappedPersistentId = 0u, uint newPersistentId = 0u)
	{
		bool flag = newPersistentId != 0 && mappedPersistentId != 0;
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<TestGroup> testGroups = listEnumerator.Current.testGroups;
			if (testGroups != null)
			{
				for (int i = 0; i < testGroups.Count; i++)
				{
					List<ITestModule> testModules = listEnumerator.Current.testGroups[i].testModules;
					if (testModules == null)
					{
						continue;
					}
					for (int j = 0; j < testModules.Count; j++)
					{
						TestVessel testVessel = testModules[j] as TestVessel;
						if (testVessel != null && testVessel.vesselID != 0)
						{
							if (flag)
							{
								if (testVessel.vesselID == mappedPersistentId)
								{
									testVessel.vesselID = newPersistentId;
								}
							}
							else
							{
								testVessel.vesselID = UpgradeMappedVesselId(testVessel.vesselID, listEnumerator.Current);
							}
						}
						TestDistance testDistance = testModules[j] as TestDistance;
						if (testDistance != null)
						{
							if (testDistance.distanceToVesselID != 0 && testDistance.distanceToTarget == TestDistance.DistanceToChoices.Vessel)
							{
								if (flag)
								{
									if (testDistance.distanceToVesselID == mappedPersistentId)
									{
										testDistance.distanceToVesselID = newPersistentId;
									}
								}
								else
								{
									testDistance.distanceToVesselID = UpgradeMappedVesselId(testDistance.distanceToVesselID, listEnumerator.Current);
								}
							}
							if (testDistance.distanceFromVesselID != 0 && testDistance.distanceFromTarget == TestDistance.DistanceFromChoices.Vessel)
							{
								if (flag)
								{
									if (testDistance.distanceFromVesselID == mappedPersistentId)
									{
										testDistance.distanceFromVesselID = newPersistentId;
									}
								}
								else
								{
									testDistance.distanceFromVesselID = UpgradeMappedVesselId(testDistance.distanceFromVesselID, listEnumerator.Current);
								}
							}
						}
						TestPartDocking testPartDocking = testModules[j] as TestPartDocking;
						if (testPartDocking != null)
						{
							if (testPartDocking.partOnevesselPartIDs.VesselID != 0)
							{
								if (flag)
								{
									if (testPartDocking.partOnevesselPartIDs.VesselID == mappedPersistentId)
									{
										testPartDocking.partOnevesselPartIDs.VesselID = newPersistentId;
									}
								}
								else
								{
									testPartDocking.partOnevesselPartIDs.VesselID = UpgradeMappedVesselId(testPartDocking.partOnevesselPartIDs.VesselID, listEnumerator.Current);
								}
							}
							if (testPartDocking.partTwovesselPartIDs.VesselID != 0)
							{
								if (flag)
								{
									if (testPartDocking.partTwovesselPartIDs.VesselID == mappedPersistentId)
									{
										testPartDocking.partTwovesselPartIDs.VesselID = newPersistentId;
									}
								}
								else
								{
									testPartDocking.partTwovesselPartIDs.VesselID = UpgradeMappedVesselId(testPartDocking.partTwovesselPartIDs.VesselID, listEnumerator.Current);
								}
							}
						}
						TestCrewAssignment testCrewAssignment = testModules[j] as TestCrewAssignment;
						if (!(testCrewAssignment != null) || testCrewAssignment.vesselPartIDs.VesselID == 0)
						{
							continue;
						}
						if (flag)
						{
							if (testCrewAssignment.vesselPartIDs.VesselID == mappedPersistentId)
							{
								testCrewAssignment.vesselPartIDs.VesselID = newPersistentId;
							}
						}
						else
						{
							testCrewAssignment.vesselPartIDs.VesselID = UpgradeMappedVesselId(testCrewAssignment.vesselPartIDs.VesselID, listEnumerator.Current);
						}
					}
				}
			}
			List<IActionModule> actionModules = listEnumerator.Current.actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int k = 0; k < actionModules.Count; k++)
			{
				ActionPartExplode actionPartExplode = actionModules[k] as ActionPartExplode;
				if (actionPartExplode != null && actionPartExplode.vesselPartIDs.VesselID != 0)
				{
					if (flag)
					{
						if (actionPartExplode.vesselPartIDs.VesselID == mappedPersistentId)
						{
							actionPartExplode.vesselPartIDs.VesselID = newPersistentId;
						}
					}
					else
					{
						actionPartExplode.vesselPartIDs.VesselID = UpgradeMappedVesselId(actionPartExplode.vesselPartIDs.VesselID, listEnumerator.Current);
					}
				}
				ActionPartFailure actionPartFailure = actionModules[k] as ActionPartFailure;
				if (actionPartFailure != null && actionPartFailure.vesselPartIDs.VesselID != 0)
				{
					if (flag)
					{
						if (actionPartFailure.vesselPartIDs.VesselID == mappedPersistentId)
						{
							actionPartFailure.vesselPartIDs.VesselID = newPersistentId;
						}
					}
					else
					{
						actionPartFailure.vesselPartIDs.VesselID = UpgradeMappedVesselId(actionPartFailure.vesselPartIDs.VesselID, listEnumerator.Current);
					}
				}
				ActionPartRepair actionPartRepair = actionModules[k] as ActionPartRepair;
				if (actionPartRepair != null && actionPartRepair.vesselPartIDs.VesselID != 0)
				{
					if (flag)
					{
						if (actionPartRepair.vesselPartIDs.VesselID == mappedPersistentId)
						{
							actionPartRepair.vesselPartIDs.VesselID = newPersistentId;
						}
					}
					else
					{
						actionPartRepair.vesselPartIDs.VesselID = UpgradeMappedVesselId(actionPartRepair.vesselPartIDs.VesselID, listEnumerator.Current);
					}
				}
				ActionPartResourceDrain actionPartResourceDrain = actionModules[k] as ActionPartResourceDrain;
				if (!(actionPartResourceDrain != null) || actionPartResourceDrain.vesselPartIDs.VesselID == 0)
				{
					continue;
				}
				if (flag)
				{
					if (actionPartResourceDrain.vesselPartIDs.VesselID == mappedPersistentId)
					{
						actionPartResourceDrain.vesselPartIDs.VesselID = newPersistentId;
					}
				}
				else
				{
					actionPartResourceDrain.vesselPartIDs.VesselID = UpgradeMappedVesselId(actionPartResourceDrain.vesselPartIDs.VesselID, listEnumerator.Current);
				}
			}
		}
		listEnumerator.Dispose();
	}

	public void RemapVesselId(uint currentVslId)
	{
		int num = mappedVessels.IndexCurrentVesselId(currentVslId);
		if (num != -1)
		{
			ProcessMappedVesselNodeMap(mappedVessels[num].mappedVesselPersistentId, mappedVessels[num].currentVesselPersistentId);
			mappedVessels.RemoveAt(num);
		}
	}

	public uint UpgradeMappedVesselId(uint currentVslId, MENode currentNode)
	{
		int num = mappedVessels.IndexCurrentVesselId(currentVslId);
		if (num != -1)
		{
			Debug.LogFormat("[Mission]: Upgrade - Vessel Id changed on Node {0} {1} from {2} to {3}", currentNode.id, Localizer.Format(currentNode.Title), currentVslId, mappedVessels[num].mappedVesselPersistentId);
			return mappedVessels[num].mappedVesselPersistentId;
		}
		return currentVslId;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", id.ToString());
		if (!string.IsNullOrEmpty(idName))
		{
			node.AddValue("idName", idName);
		}
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			node.AddValue("expansionVersion", ExpansionsLoader.GetExpansionVersion("MakingHistory"));
		}
		else
		{
			node.AddValue("expansionVersion", expansionVersion);
		}
		node.AddValue("title", title);
		string text = briefing.Replace("\n", "\\n");
		text = text.Replace("\t", "\\t");
		node.AddValue("briefing", text);
		node.AddValue("author", author);
		string text2 = modsBriefing.Replace("\n", "\\n");
		text2 = text2.Replace("\t", "\\t");
		node.AddValue("modsBriefing", text2);
		if (!string.IsNullOrEmpty(packName))
		{
			node.AddValue("packName", packName);
		}
		if (order < int.MaxValue)
		{
			node.AddValue("order", order);
		}
		node.AddValue("hardIcon", hardIcon);
		node.AddValue("difficulty", difficulty);
		node.AddValue("isBriefingSet", isBriefingSet);
		node.AddValue("seed", seed);
		node.AddValue("flag", flagURL);
		node.AddValue("bannerMenu", bannerMenu.fileName);
		node.AddValue("bannerSuccess", bannerSuccess.fileName);
		node.AddValue("bannerFail", bannerFail.fileName);
		node.AddValue("isScoreEnabled", isScoreEnabled);
		node.AddValue("maxScore", maxScore);
		node.AddValue("steamPublishedFileId", steamPublishedFileId);
		node.AddValue("historyId", historyId);
		if (activeNode != null)
		{
			node.AddValue("activeNodeID", activeNode.id);
		}
		node.AddValue("currentScore", currentScore);
		node.AddValue("exportName", exportName);
		node.AddValue("missionNameAtLastExport", missionNameAtLastExport);
		node.AddValue("isStarted", isStarted);
		node.AddValue("isEnded", isEnded);
		node.AddValue("isSuccesful", isSuccesful);
		node.AddValue("briefingNodeActive", briefingNodeActive);
		node.AddValue("cameraLockMode", cameraLockMode);
		node.AddValue("cameraLockOptions", cameraLockOptions);
		if (saveRevertOnSwitchActiveVessel)
		{
			node.AddValue("saveRevertOnSwitchActiveVessel", saveRevertOnSwitchActiveVessel);
		}
		if (signature != null)
		{
			node.AddValue("signature", signature);
		}
		ConfigNode node2 = node.AddNode("SITUATION");
		situation.Save(node2);
		globalScore.Save(node);
		awards.Save(node);
		MissionMappedVessel.Save(node, mappedVessels);
		ConfigNode configNode = node.AddNode("NODES");
		foreach (MENode value in nodes.Values)
		{
			ConfigNode node3 = configNode.AddNode("NODE");
			value.Save(node3);
		}
		ConfigNode configNode2 = node.AddNode("TAGS");
		for (int i = 0; i < tags.Count; i++)
		{
			configNode2.AddNode("TAG").AddValue("name", tags[i]);
		}
		ConfigNode node4 = node.AddNode("FLOW");
		flow.Save(node4);
	}

	public uint CurrentVesselID(MENode node, uint vesselID)
	{
		uint num = mappedVessels.ConvertMappedId(vesselID);
		if (!FlightGlobals.PersistentVesselIds.ContainsKey(num))
		{
			int num2 = node.mission.mappedVessels.IndexMappedVesselId(vesselID);
			if (num2 != -1)
			{
				uint partPersistentId = mappedVessels[num2].partPersistentId;
				if (FlightGlobals.PersistentLoadedPartIds.ContainsKey(partPersistentId))
				{
					if (FlightGlobals.PersistentLoadedPartIds[partPersistentId].vessel != null)
					{
						num = FlightGlobals.PersistentLoadedPartIds[partPersistentId].vessel.persistentId;
					}
					else if (!missingVessels.Contains(num))
					{
						missingVessels.Add(num);
						Debug.LogErrorFormat("[TestVessel] Unable to find VesselID ({0}) from Node ({1}) in FlightGlobals.", num, (node != null) ? node.Title : "Unknown");
					}
				}
				else if (FlightGlobals.PersistentUnloadedPartIds.ContainsKey(partPersistentId))
				{
					if (FlightGlobals.PersistentUnloadedPartIds[partPersistentId].pVesselRef != null)
					{
						num = FlightGlobals.PersistentUnloadedPartIds[partPersistentId].pVesselRef.persistentId;
					}
					else if (!missingVessels.Contains(num))
					{
						missingVessels.Add(num);
						Debug.LogErrorFormat("[TestVessel] Unable to find VesselID ({0}) from Node ({1}) in FlightGlobals.", num, (node != null) ? node.Title : "Unknown");
					}
				}
			}
		}
		return num;
	}

	public MENode GetNodeById(Guid guid)
	{
		int num = 0;
		while (true)
		{
			if (num < nodes.Count)
			{
				if (nodes.KeyAt(num) == guid)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return nodes.At(num);
	}

	public Guid GetNodeGuidByVesselID(uint persistentId)
	{
		return GetNodeGuidByVesselID(persistentId, processMappedVessels: false);
	}

	public Guid GetNodeGuidByVesselID(uint persistentId, bool processMappedVessels)
	{
		if (processMappedVessels)
		{
			persistentId = mappedVessels.ConvertMappedId(persistentId);
		}
		DictionaryValueList<VesselSituation, Guid> allVesselSituationsGuid = GetAllVesselSituationsGuid();
		int num = 0;
		while (true)
		{
			if (num < allVesselSituationsGuid.Count)
			{
				if (allVesselSituationsGuid.KeyAt(num).persistentId == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return Guid.Empty;
		}
		return allVesselSituationsGuid.At(num);
	}

	public bool PendingVesselLaunch(MENode node)
	{
		return node.HasPendingVesselLaunch;
	}

	public VesselSituation GetVesselSituationByVesselID(uint persistentId)
	{
		return GetVesselSituationByVesselID(persistentId, processMappedVessels: false);
	}

	public VesselSituation GetVesselSituationByVesselID(uint persistentId, bool processMappedVessels)
	{
		Guid nodeGuidByVesselID = GetNodeGuidByVesselID(persistentId, processMappedVessels);
		if (nodeGuidByVesselID != Guid.Empty)
		{
			MENode nodeById = GetNodeById(nodeGuidByVesselID);
			if (nodeById != null)
			{
				ActionCreateVessel actionCreateVessel = GetActionCreateVessel(nodeById);
				if (actionCreateVessel != null)
				{
					return actionCreateVessel.vesselSituation;
				}
			}
		}
		return null;
	}

	public Asteroid GetAsteroidByPersistentID(uint persistentId)
	{
		List<ActionCreateAsteroid> allActionModules = GetAllActionModules<ActionCreateAsteroid>();
		int num = 0;
		while (true)
		{
			if (num < allActionModules.Count)
			{
				if (allActionModules[num].asteroid.persistentId == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return allActionModules[num].asteroid;
	}

	public Comet GetCometByPersistentID(uint PersistentId)
	{
		List<ActionCreateComet> allActionModules = GetAllActionModules<ActionCreateComet>();
		int num = 0;
		while (true)
		{
			if (num < allActionModules.Count)
			{
				if (allActionModules[num].comet != null && allActionModules[num].comet.persistentId == PersistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return allActionModules[num].comet;
	}

	public ActionCreateFlag GetActionCreateFlagByPersistentID(uint persistentId)
	{
		List<ActionCreateFlag> allActionModules = GetAllActionModules<ActionCreateFlag>();
		int num = 0;
		while (true)
		{
			if (num < allActionModules.Count)
			{
				if (allActionModules[num].persistentID == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return allActionModules[num];
	}

	public ActionCreateVessel GetActionCreateVessel(MENode node)
	{
		List<IActionModule> actionModules = node.actionModules;
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is ActionCreateVessel)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return actionModules[num] as ActionCreateVessel;
	}

	public ActionCreateKerbal GetActionCreateKerbal(MENode node)
	{
		List<IActionModule> actionModules = node.actionModules;
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is ActionCreateKerbal)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return actionModules[num] as ActionCreateKerbal;
	}

	public ActionCreateAsteroid GetActionCreateAsteroid(MENode node)
	{
		List<IActionModule> actionModules = node.actionModules;
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is ActionCreateAsteroid)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return actionModules[num] as ActionCreateAsteroid;
	}

	public ActionCreateComet GetActionCreateComet(MENode node)
	{
		List<IActionModule> actionModules = node.actionModules;
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is ActionCreateComet)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return actionModules[num] as ActionCreateComet;
	}

	public ActionCreateFlag GetActionCreateFlag(MENode node)
	{
		List<IActionModule> actionModules = node.actionModules;
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is ActionCreateFlag)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return actionModules[num] as ActionCreateFlag;
	}

	public List<VesselSituation> GetAllVesselSituations()
	{
		List<VesselSituation> list = new List<VesselSituation>();
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<IActionModule> actionModules = listEnumerator.Current.actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int i = 0; i < actionModules.Count; i++)
			{
				if (actionModules[i].GetType() == typeof(ActionCreateVessel))
				{
					ActionCreateVessel actionCreateVessel = actionModules[i] as ActionCreateVessel;
					if (actionCreateVessel != null)
					{
						list.Add(actionCreateVessel.vesselSituation);
					}
				}
			}
		}
		return list;
	}

	public DictionaryValueList<VesselSituation, Guid> GetAllVesselSituationsGuid()
	{
		DictionaryValueList<VesselSituation, Guid> dictionaryValueList = new DictionaryValueList<VesselSituation, Guid>();
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<IActionModule> actionModules = listEnumerator.Current.actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int i = 0; i < actionModules.Count; i++)
			{
				if (actionModules[i].GetType() == typeof(ActionCreateVessel))
				{
					ActionCreateVessel actionCreateVessel = actionModules[i] as ActionCreateVessel;
					if (actionCreateVessel != null)
					{
						dictionaryValueList.Add(actionCreateVessel.vesselSituation, listEnumerator.Current.id);
					}
				}
			}
		}
		return dictionaryValueList;
	}

	public List<T> GetAllActionModules<T>() where T : ActionModule
	{
		List<T> list = new List<T>();
		Type typeFromHandle = typeof(T);
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<IActionModule> actionModules = listEnumerator.Current.actionModules;
			if (actionModules == null)
			{
				continue;
			}
			for (int i = 0; i < actionModules.Count; i++)
			{
				if (actionModules[i].GetType() == typeFromHandle)
				{
					T val = actionModules[i] as T;
					if (val != null)
					{
						list.Add(val);
					}
				}
			}
		}
		return list;
	}

	public List<T> GetAllTestModules<T>() where T : TestModule
	{
		List<T> list = new List<T>();
		Type typeFromHandle = typeof(T);
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (listEnumerator.MoveNext())
		{
			List<TestGroup> testGroups = listEnumerator.Current.testGroups;
			if (testGroups == null)
			{
				continue;
			}
			for (int i = 0; i < testGroups.Count; i++)
			{
				List<ITestModule> testModules = listEnumerator.Current.testGroups[i].testModules;
				if (testModules == null)
				{
					continue;
				}
				for (int j = 0; j < testModules.Count; j++)
				{
					if (testModules[j].GetType() == typeFromHandle)
					{
						T val = testModules[j] as T;
						if (val != null)
						{
							list.Add(val);
						}
					}
				}
			}
		}
		return list;
	}

	public bool MissionHasLaunchSite(string name)
	{
		List<MENode>.Enumerator listEnumerator = nodes.GetListEnumerator();
		while (true)
		{
			if (listEnumerator.MoveNext())
			{
				if (listEnumerator.Current.IsDockedToStartNode && listEnumerator.Current.IsLaunchPadNode)
				{
					ActionCreateLaunchSite actionCreateLaunchSite = listEnumerator.Current.actionModules[0] as ActionCreateLaunchSite;
					if (actionCreateLaunchSite != null && actionCreateLaunchSite.launchSiteSituation.launchSiteName == name)
					{
						break;
					}
				}
				continue;
			}
			return false;
		}
		return true;
	}

	public MEBannerEntry GetBanner(MEBannerType bannerType)
	{
		MEBannerEntry result = null;
		switch (bannerType)
		{
		case MEBannerType.Menu:
			result = bannerMenu;
			break;
		case MEBannerType.Success:
			result = bannerSuccess;
			break;
		case MEBannerType.Fail:
			result = bannerFail;
			break;
		}
		return result;
	}

	public void SetBanner(MEBannerEntry newBanner, MEBannerType bannerType)
	{
		if (newBanner != null)
		{
			switch (bannerType)
			{
			case MEBannerType.Menu:
				bannerMenu = newBanner;
				break;
			case MEBannerType.Success:
				bannerSuccess = newBanner;
				break;
			case MEBannerType.Fail:
				bannerFail = newBanner;
				break;
			}
		}
	}
}
