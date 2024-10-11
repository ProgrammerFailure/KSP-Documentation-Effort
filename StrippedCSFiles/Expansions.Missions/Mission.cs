using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Mission : MonoBehaviour, IConfigNode
{
	[CompilerGenerated]
	private sealed class _003CGenerateMissionLaunchSites_003Ed__129 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Mission _003C_003E4__this;

		public Callback<Mission> onComplete;

		public bool createPQSObject;

		private List<string> _003CmissionLaunchSites_003E5__2;

		private List<MENode>.Enumerator _003CnodeList_003E5__3;

		private ActionCreateLaunchSite _003CactionLaunchSite_003E5__4;

		private PQS _003Csphere_003E5__5;

		private Transform _003CsphereTarget_003E5__6;

		private GameObject _003CtempObject_003E5__7;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CGenerateMissionLaunchSites_003Ed__129(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public string expansionVersion;

	public static string lastCompatibleVersion;

	[SerializeField]
	private string _title;

	[SerializeField]
	private string activeNodeName;

	public string briefing;

	public string author;

	public string modsBriefing;

	public string packName;

	public int order;

	public bool hardIcon;

	public MissionDifficulty difficulty;

	[SerializeField]
	internal bool isBriefingSet;

	[SerializeField]
	private int seed;

	public string flagURL;

	private MEBannerEntry bannerMenu;

	private MEBannerEntry bannerSuccess;

	private MEBannerEntry bannerFail;

	public bool isScoreEnabled;

	public float maxScore;

	public DictionaryValueList<string, MissionCraft> craftFileList;

	[SerializeField]
	private MissionFileInfo _missionInfo;

	public MissionSituation situation;

	public MissionFlow flow;

	public DictionaryValueList<Guid, MENode> nodes;

	public List<string> tags;

	public MissionScore globalScore;

	public MissionAwards awards;

	private Guid loadActiveNodeID;

	public double startedUT;

	public float currentScore;

	public string exportName;

	internal string missionNameAtLastExport;

	public ulong steamPublishedFileId;

	public bool briefingNodeActive;

	[SerializeField]
	private List<MENode> orphanNodes;

	private List<MENode> nextObjectivesNodes;

	private List<MENode> inactiveEventNodes;

	private bool switchActiveVessel;

	private Vessel vesselToSwitchActive;

	internal bool saveRevertOnSwitchActiveVessel;

	public MissionCameraModeOptions cameraLockMode;

	public MissionCameraLockOptions cameraLockOptions;

	public Guid historyId;

	public List<MissionMappedVessel> mappedVessels;

	private List<MissionMappedVessel> mappedVesselKeys;

	private byte[] hashBytes;

	private string signature;

	internal bool isSimple;

	private bool registeredGameEvents;

	private List<uint> missingVessels;

	private List<string> blockedMessages;

	private Callback<Mission> generateLaunchSitesCallback;

	internal List<MissionValidationTestResult> validationResults;

	internal bool hasBeenValidated;

	public Guid id
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	internal string idName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public string title
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string ActiveNodeName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal int Seed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string PersistentSaveName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool MissionLoadedFromSFS
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ShipsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string BannersPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MissionFileInfo MissionInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	public MENode activeNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public MENode startNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool isInitialized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool isStarted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool isEnded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool isSuccesful
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public List<MENode> InactiveEventNodes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsTutorialMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal byte[] HashBytes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal string Signature
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<MissionValidationTestResult> ValidationResults
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasBeenValidated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Mission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Mission Spawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Mission Spawn(MissionFileInfo missionInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Mission SpawnAndLoad(MissionFileInfo missionInfo, ConfigNode missionNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GetGameObjectName(MissionFileInfo missionInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GetGameObjectName(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckHash()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RegenerateMissionID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> scenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RegisterGameEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetStartNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetActiveNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDockedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool BlockPlayMission(bool showDialog = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void MissionCriticalError(string errorString = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContinueCriticalError()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CGenerateMissionLaunchSites_003Ed__129))]
	internal IEnumerator GenerateMissionLaunchSites(Callback<Mission> onComplete, bool createPQSObject = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResumeMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStartNode(MENode newStart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoEndMissionActions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SwitchActiveNodes(MENode newNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetNextObjectives(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsNextObjective(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemoveInactiveEvent(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateOrphanNodeState(MENode node, bool makeOrphan)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintScoreObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode, bool onlyAwardedScores)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string PrintObjectives(bool onlyPrintActivatedNodes, bool startWithActiveNode, bool onlyPrintScore, bool onlyAwardedScores)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecursivelyCreateObjectiveString(MENode currentNode, ref string objectiveString, string indent, bool onlyPrintActivatedNodes, ref List<MENode> visitedNodesList, bool rejectOrphanNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecursivelyCreateObjectiveScoreString(MENode currentNode, ref string objectiveString, bool onlyPrintActivatedNodes, bool onlyAwardedScores, ref List<MENode> visitedNodesList, bool rejectOrphanNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal MissionFileInfo UpdateMissionFileInfo(string MissionFilePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartVesselNamingChanged(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> hostedFromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDecoupleNewVesselComplete(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDocking(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RebuildCraftFileList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearCraftFiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool UpdateFromMappedVesselIDs(uint vesselId, ref string vesselName, ref VesselSituation vesselSituation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearMappedVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessCraftFileFolder(string saveShipFolderPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessCraftRenames(string craftFileName, MissionCraft craftFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MissionMappedVessel AddMappedPartVesselName(uint partId, uint vslPersistentId, string craftFileName, MissionCraft craftFile, bool mappedItem, VesselSituation vesselSituation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddMappedCraft(string craftFileName, MissionCraft craftFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionCraft GetCraftBySituationsVesselID(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionCraft GetMissionCraftByFileName(string craftFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation GetMissionSituationByCraftFileName(string craftFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionCraft GetMissionCraftByName(string vesselName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetSituationsIndexByVessel(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetSituationsIndexByPart(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCraftFileIndexByPart(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SwitchActiveVessel(Vessel vessel, bool saveRevertOnSwitch = false, bool switchImmediately = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLockedCamera(MissionCameraModeOptions newCameraMode, MissionCameraLockOptions newCameraLock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Export(string exportFileName, bool overwrite = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node, bool simple = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ProcessMappedVesselNodeMap(uint mappedPersistentId = 0u, uint newPersistentId = 0u)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemapVesselId(uint currentVslId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private uint UpgradeMappedVesselId(uint currentVslId, MENode currentNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint CurrentVesselID(MENode node, uint vesselID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENode GetNodeById(Guid guid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Guid GetNodeGuidByVesselID(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Guid GetNodeGuidByVesselID(uint persistentId, bool processMappedVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PendingVesselLaunch(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation GetVesselSituationByVesselID(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSituation GetVesselSituationByVesselID(uint persistentId, bool processMappedVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Asteroid GetAsteroidByPersistentID(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Comet GetCometByPersistentID(uint PersistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateFlag GetActionCreateFlagByPersistentID(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateVessel GetActionCreateVessel(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateKerbal GetActionCreateKerbal(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateAsteroid GetActionCreateAsteroid(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateComet GetActionCreateComet(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateFlag GetActionCreateFlag(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<VesselSituation> GetAllVesselSituations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DictionaryValueList<VesselSituation, Guid> GetAllVesselSituationsGuid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> GetAllActionModules<T>() where T : ActionModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> GetAllTestModules<T>() where T : TestModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool MissionHasLaunchSite(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEBannerEntry GetBanner(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBanner(MEBannerEntry newBanner, MEBannerType bannerType)
	{
		throw null;
	}
}
