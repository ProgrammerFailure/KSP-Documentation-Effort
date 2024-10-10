using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using ns11;
using ns16;
using ns9;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionPlayDialog : MonoBehaviour
{
	public class dialogList
	{
		public bool steamItem;

		public MissionFileInfo missionFileInfo;

		public SteamMissionFileInfo steamMissionFileInfo;
	}

	public delegate void FinishedCallback(MissionFileInfo missionInfo);

	public enum SteamQueryFilters
	{
		VOTE,
		FEATURED,
		NEWEST,
		SUBSCRIBERS
	}

	public class MissionProfileInfo : IConfigNode
	{
		public Guid id;

		public string idName;

		public string title = "";

		public string briefing;

		public string author;

		public string modsBriefing;

		public string packName;

		public int order;

		public bool hardIcon;

		public MissionDifficulty difficulty = MissionDifficulty.Intermediate;

		public int vesselCount;

		public double startUT = -1.0;

		public int nodeCount;

		public List<string> tags = new List<string>();

		public bool errorAccess;

		public string errorDetails = "";

		public string saveMD5 = "";

		public long lastWriteTime;

		public string missionExpansionVersion;

		public ulong steamPublishedFileId;

		public List<string> testModules = new List<string>();

		public List<string> actionModules = new List<string>();

		public MissionProfileInfo()
		{
			id = Guid.Empty;
			order = int.MaxValue;
		}

		public MissionProfileInfo(string filename, string saveFolder, Mission mission)
		{
			LoadDetailsFromMission(mission);
			lastWriteTime = GetLastWriteTime(filename, saveFolder);
		}

		public MissionProfileInfo LoadDetailsFromMission(Mission mission)
		{
			if (mission != null)
			{
				id = mission.id;
				idName = mission.idName;
				title = mission.title;
				briefing = mission.briefing;
				author = mission.author;
				modsBriefing = mission.modsBriefing;
				packName = mission.packName;
				order = mission.order;
				hardIcon = mission.hardIcon;
				difficulty = mission.difficulty;
				startUT = mission.situation.startUT;
				vesselCount = mission.GetAllVesselSituations().Count;
				nodeCount = mission.nodes.Count;
				tags = mission.tags;
				missionExpansionVersion = mission.expansionVersion;
				steamPublishedFileId = mission.steamPublishedFileId;
				for (int i = 0; i < mission.nodes.ValuesList.Count; i++)
				{
					MENode mENode = mission.nodes.ValuesList[i];
					for (int j = 0; j < mENode.actionModules.Count; j++)
					{
						actionModules.AddUnique(mENode.actionModules[j].GetName());
					}
					for (int k = 0; k < mENode.testGroups.Count; k++)
					{
						for (int l = 0; l < mENode.testGroups[k].testModules.Count; l++)
						{
							testModules.AddUnique(mENode.testGroups[k].testModules[l].GetName());
						}
					}
				}
			}
			else
			{
				Debug.Log("[MissionSystem]: Mission was null!");
			}
			return this;
		}

		public void Load(ConfigNode node)
		{
			if (node.HasValue("saveMD5"))
			{
				saveMD5 = node.GetValue("saveMD5");
			}
			node.TryGetValue("lastWriteTime", ref lastWriteTime);
			node.TryGetValue("id", ref id);
			node.TryGetValue("idName", ref idName);
			if (node.HasValue("title"))
			{
				title = node.GetValue("title");
			}
			if (node.HasValue("briefing"))
			{
				briefing = node.GetValue("briefing");
			}
			if (node.HasValue("author"))
			{
				author = node.GetValue("author");
			}
			node.TryGetValue("modsBriefing", ref modsBriefing);
			node.TryGetValue("packName", ref packName);
			node.TryGetValue("order", ref order);
			node.TryGetValue("hardIcon", ref hardIcon);
			node.TryGetEnum("difficulty", ref difficulty, MissionDifficulty.Intermediate);
			if (node.HasValue("vesselCount"))
			{
				vesselCount = Convert.ToInt32(node.GetValue("vesselCount"));
			}
			if (node.HasValue("nodeCount"))
			{
				nodeCount = Convert.ToInt32(node.GetValue("nodeCount"));
			}
			if (node.HasValue("startUT"))
			{
				startUT = Convert.ToDouble(node.GetValue("startUT"));
			}
			if (node.HasValue("tags"))
			{
				tags = node.GetValuesList("tags");
			}
			if (node.HasValue("missionExpansionVersion"))
			{
				missionExpansionVersion = node.GetValue("missionExpansionVersion");
			}
			node.TryGetValue("steamPublishedFileId", ref steamPublishedFileId);
			if (node.HasValue("testModules"))
			{
				testModules = node.GetValuesList("testModules");
			}
			if (node.HasValue("actionModules"))
			{
				actionModules = node.GetValuesList("actionModules");
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("saveMD5", saveMD5);
			node.AddValue("lastWriteTime", lastWriteTime);
			node.AddValue("id", id);
			if (!string.IsNullOrEmpty(idName))
			{
				node.AddValue("idName", idName);
			}
			node.AddValue("title", title);
			string text = briefing.Replace("\n", "\\n");
			text = text.Replace("\t", "\\t");
			node.AddValue("briefing", text);
			node.AddValue("author", author);
			if (!string.IsNullOrEmpty(modsBriefing))
			{
				string text2 = modsBriefing.Replace("\n", "\\n");
				text2 = text2.Replace("\t", "\\t");
				node.AddValue("modsBriefing", text2);
			}
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
			node.AddValue("vesselCount", vesselCount);
			node.AddValue("nodeCount", nodeCount);
			node.AddValue("startUT", startUT);
			for (int i = 0; i < tags.Count; i++)
			{
				node.AddValue("tags", tags[i]);
			}
			node.AddValue("missionExpansionVersion", missionExpansionVersion);
			node.AddValue("steamPublishedFileId", steamPublishedFileId);
			for (int j = 0; j < testModules.Count; j++)
			{
				node.AddValue("testModules", testModules[j]);
			}
			for (int k = 0; k < actionModules.Count; k++)
			{
				node.AddValue("actionModules", actionModules[k]);
			}
		}

		public void LoadFromMetaFile(string filename, string saveFolder)
		{
			string filePath = saveFolder + "/" + filename + ".loadmeta";
			filePath = KSPSteamUtils.GetSteamCacheLocation(filePath);
			if (!File.Exists(filePath))
			{
				Debug.Log("No meta file found for path: " + filePath);
				return;
			}
			ConfigNode configNode = ConfigNode.Load(filePath, bypassLocalization: true);
			if (!configNode.HasValue("difficulty"))
			{
				Debug.Log("[MissionSystem] Meta file has no difficulty, forcing rebuild: " + filePath);
			}
			else if (!configNode.HasValue("missionExpansionVersion"))
			{
				Debug.Log("[MissionSystem] Meta file has no compatibility information, forcing rebuild: " + filePath);
			}
			else if ((configNode.HasValue("testModules") || configNode.HasValue("actionModules")) && configNode.GetValue("missionExpansionVersion") == "")
			{
				Debug.Log("[MissionSystem] Meta file has no compatibility information, forcing rebuild: " + filePath);
			}
			else if (!configNode.HasValue("steamPublishedFileId"))
			{
				Debug.Log("[MissionSystem] Meta file has no steam information, forcing rebuild: " + filePath);
			}
			else
			{
				Load(configNode);
			}
		}

		public void SaveToMetaFile(string filename, string saveFolder)
		{
			ConfigNode configNode = new ConfigNode();
			Save(configNode);
			string filePath = saveFolder + "/" + filename + ".loadmeta";
			filePath = KSPSteamUtils.GetSteamCacheLocation(filePath);
			if (steamPublishedFileId != 0L)
			{
				string directoryName = Path.GetDirectoryName(filePath);
				if (directoryName == null || directoryName == string.Empty)
				{
					Debug.LogWarning("[MissionSystem] Path.GetDirectoryName(loadmetaPath) returned an invalid path!");
					return;
				}
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			configNode.Save(filePath);
		}

		public static string GetSFSMD5(string filename, string saveFolder)
		{
			string text = saveFolder + "/" + filename + ".mission";
			if (File.Exists(text))
			{
				return Versioning.FileMD5String(text);
			}
			return "";
		}

		public static long GetLastWriteTime(string filename, string saveFolder)
		{
			string path = saveFolder + "/" + filename + ".mission";
			if (File.Exists(path))
			{
				return File.GetLastWriteTimeUtc(path).Ticks;
			}
			return -1L;
		}
	}

	public static MissionPlayDialog Instance;

	public Texture2D currentIcon;

	public Material iconMaterial;

	public Color scnIconColor;

	[SerializeField]
	[Header("UI Components")]
	public Button btnLoad;

	[SerializeField]
	public Button btnCancel;

	[SerializeField]
	public Button btnRestart;

	[SerializeField]
	public Button btnEdit;

	[SerializeField]
	public Button btnCreate;

	[SerializeField]
	public Button btnSteamOverlay;

	[SerializeField]
	public Button btnSteamItemDetails;

	[SerializeField]
	public Button btnSteamSubUnSub;

	[SerializeField]
	public Button btnMainSteamSubUnSub;

	[SerializeField]
	public TextMeshProUGUI MainSteamSubUnsubText;

	[SerializeField]
	public Toggle tabCommunity;

	[SerializeField]
	public Toggle tabStock;

	[SerializeField]
	public Toggle tabSteam;

	[SerializeField]
	public TextMeshProUGUI textTitle;

	[SerializeField]
	public TextMeshProUGUI textDescription;

	[SerializeField]
	public TextMeshProUGUI btnLoadCaption;

	[SerializeField]
	public TextMeshProUGUI textModsDescription;

	[SerializeField]
	public TextMeshProUGUI textAuthor;

	[SerializeField]
	public RawImage imgScnBanner;

	[SerializeField]
	public RectTransform dialogRectTransform;

	[SerializeField]
	public RectTransform containerDefault;

	[SerializeField]
	public RectTransform containerInfo;

	[SerializeField]
	public RectTransform scrollListContent;

	[SerializeField]
	public ToggleGroup listGroup;

	[SerializeField]
	public GameObject modsSection;

	[SerializeField]
	public AwardWidget awardWidgetPrefab;

	[SerializeField]
	public Transform awardsContent;

	[SerializeField]
	public TextMeshProUGUI textHighScore;

	[SerializeField]
	public TextMeshProUGUI textMaxScore;

	[SerializeField]
	public GameObject maxScoreField;

	[SerializeField]
	public GameObject highScoreField;

	[SerializeField]
	public GameObject awardsHeader;

	[SerializeField]
	public GameObject awardsHolder;

	[SerializeField]
	public GameObject objectivesHeader;

	[SerializeField]
	public GameObject progressField;

	[SerializeField]
	public GameObject SteamFilterHolder;

	[SerializeField]
	public Toggle SteamFilterToggleOne;

	[SerializeField]
	public Toggle SteamFilterToggleTwo;

	[SerializeField]
	public Toggle SteamFilterToggleThree;

	[SerializeField]
	public Toggle SteamFilterToggleFour;

	[SerializeField]
	public GameObject SteamSection;

	[SerializeField]
	public TextMeshProUGUI AuthorTitleText;

	[SerializeField]
	public TextMeshProUGUI SteamFileSizeText;

	[SerializeField]
	public TextMeshProUGUI SteamUpVotesText;

	[SerializeField]
	public TextMeshProUGUI SteamSubscribersText;

	[SerializeField]
	public TextMeshProUGUI SteamDownVotesText;

	[SerializeField]
	public TextMeshProUGUI SteamSubUnsubText;

	[SerializeField]
	public TextMeshProUGUI SteamLoadingText;

	[SerializeField]
	public GameObject SteamButtonSection;

	[SerializeField]
	public GameObject ButtonSection;

	[SerializeField]
	public Transform MEFlowObjectsParent;

	[SerializeField]
	public TMP_Text MEFlowEmptyObjectivesMessage;

	[SerializeField]
	public GameObject difficultyField;

	[SerializeField]
	public TextMeshProUGUI textDifficulty;

	[SerializeField]
	public Image difficultyHead;

	public Texture2D missionNormalIcon;

	public Texture2D missionHardIcon;

	public Texture2D steamIcon;

	public Texture2D steamSub;

	public Texture2D steamUnsub;

	[Header("Other Variables")]
	public DictionaryValueList<DialogGUIToggleButton, dialogList> items = new DictionaryValueList<DialogGUIToggleButton, dialogList>();

	public DictionaryValueList<MissionSteamItemWidget, dialogList> steamItems = new DictionaryValueList<MissionSteamItemWidget, dialogList>();

	[SerializeField]
	public PublishedFileId_t[] steamSubscribedItems;

	public FinishedCallback OnFinishedCallback = delegate
	{
	};

	public Callback OnDialogDismissCallback;

	public UISkinDef skin;

	public bool confirmGameRestart;

	public List<MissionFileInfo> missionList;

	public List<MissionPack> missionPacks;

	public MissionFileInfo selectedMission;

	public SteamMissionFileInfo _selectedSteamMissionItem;

	public MissionTypes selectedType = MissionTypes.User;

	public SteamQueryFilters selectedSteamQueryFilter;

	public MissionScoreInfo missionScoreInfo;

	public MEFlowParser flowParser;

	public UISkinDefSO guiSkinDef;

	[SerializeField]
	public List<Texture2D> missionDifficultyButtons;

	[SerializeField]
	public List<Sprite> difficultyHeads;

	public string authorNormalTitle = "";

	public string authorSteamTitle = "";

	public Coroutine buildListCoroutine;

	public DictionaryValueList<string, MissionListGroup> packGroups;

	public static float maxFrameTime = 0.25f;

	public bool dialogFullExpansionMode;

	public static MissionPlayDialog Create(Callback onDismiss)
	{
		MissionSystem.IsTestMode = false;
		UnityEngine.Object @object = null;
		bool flag;
		if (flag = ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			if (!Directory.Exists(MissionsUtils.GetMissionsFolder(MissionTypes.User)))
			{
				Directory.CreateDirectory(MissionsUtils.GetMissionsFolder(MissionTypes.User));
			}
			MissionImporting.ImportNewZips();
			@object = MissionsUtils.MEPrefab("_UI5/Dialogs/MissionPlayDialog/prefabs/MissionPlayDialog.prefab");
		}
		if (@object == null || !flag)
		{
			@object = AssetBase.GetPrefab("BaseMissionPlayDialog");
			if (@object == null)
			{
				Debug.LogError("[MissionPlayDialog]: Unable to load the Asset");
				return null;
			}
			flag = false;
		}
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(@object);
		obj.name = "MissionPlayDialog";
		obj.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		obj.transform.localPosition = Vector3.zero;
		MissionPlayDialog missionPlayDialog = (Instance = obj.GetComponent<MissionPlayDialog>());
		missionPlayDialog.dialogFullExpansionMode = flag;
		missionPlayDialog.OnDialogDismissCallback = onDismiss;
		missionPlayDialog.OnFinishedCallback = missionPlayDialog.OnMissionLoadDismiss;
		missionPlayDialog.skin = missionPlayDialog.guiSkinDef.SkinDef;
		if (flag)
		{
			missionPlayDialog.selectedType = MissionTypes.User;
		}
		else
		{
			missionPlayDialog.selectedType = MissionTypes.Base;
		}
		missionPlayDialog.containerDefault.gameObject.SetActive(value: true);
		missionPlayDialog.containerInfo.gameObject.SetActive(value: false);
		missionPlayDialog.dialogRectTransform = missionPlayDialog.GetComponent<RectTransform>();
		if (SteamManager.Initialized && flag)
		{
			missionPlayDialog.dialogRectTransform.sizeDelta = new Vector2(900f, missionPlayDialog.dialogRectTransform.rect.height);
		}
		if (flag)
		{
			missionPlayDialog.missionScoreInfo = MissionScoreInfo.LoadScores();
		}
		UpdateListOfMissions();
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "playMissionDialog");
		return missionPlayDialog;
	}

	public void OnMissionLoadDismiss(MissionFileInfo missionInfo)
	{
		InputLockManager.RemoveControlLock("playMissionDialog");
		MissionSystem.RemoveMissionObjects(removeAll: true);
		if (missionInfo != null)
		{
			StartCoroutine(MissionSystem.Instance.SetupMissionGame(missionInfo, playMission: true, testMode: false, onMissionLoadSuccess, onMissionLoadFail));
		}
	}

	public void onMissionLoadSuccess()
	{
		HighLogic.CurrentGame.Start();
		MissionSystem.RemoveMissionObjects();
		Terminate();
	}

	public void onMissionLoadFail()
	{
		Debug.LogError("[MissionsExpansion] Unable to start Mission.");
		MissionSystem.RemoveMissionObjects();
		Terminate();
	}

	public void Start()
	{
		GameEvents.Mission.onMissionImported.Add(onMissionImported);
		btnRestart.interactable = false;
		btnLoad.interactable = false;
		btnRestart.onClick.AddListener(OnButtonRestart);
		btnCancel.onClick.AddListener(OnBtnCancel);
		btnLoad.onClick.AddListener(OnButtonLoad);
		if (dialogFullExpansionMode)
		{
			btnEdit.onClick.AddListener(OnButtonEdit);
			btnCreate.onClick.AddListener(OnButtonCreate);
			tabStock.onValueChanged.AddListener(OnTabStock);
			tabCommunity.onValueChanged.AddListener(OnTabCommunity);
			btnSteamOverlay.gameObject.SetActive(value: false);
			SteamButtonSection.SetActive(value: false);
			ButtonSection.SetActive(value: true);
		}
		if (SteamManager.Initialized && dialogFullExpansionMode)
		{
			btnSteamOverlay.onClick.AddListener(OnBtnSteamOverlay);
			tabSteam.onValueChanged.AddListener(OnTabSteam);
			btnSteamItemDetails.onClick.AddListener(onBtnSteamItemDetails);
			btnSteamSubUnSub.onClick.AddListener(onBtnSteamSubUnSub);
			btnMainSteamSubUnSub.onClick.AddListener(onBtnSteamSubUnSub);
			SteamFilterToggleOne.onValueChanged.AddListener(onSteamFilterOne);
			SteamFilterToggleTwo.onValueChanged.AddListener(onSteamFilterTwo);
			SteamFilterToggleThree.onValueChanged.AddListener(onSteamFilterThree);
			SteamFilterToggleFour.onValueChanged.AddListener(onSteamFilterFour);
			UpdateSteamSubscribedItems();
		}
		else if (tabSteam != null)
		{
			tabSteam.gameObject.SetActive(value: false);
		}
		if (dialogFullExpansionMode)
		{
			tabCommunity.interactable = GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED;
			tabStock.interactable = GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED;
			tabSteam.interactable = GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED;
			btnCreate.interactable = GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED;
			btnEdit.interactable = false;
		}
		textTitle.text = "Select a mission to load";
		textDescription.text = "No description available.";
		OnSelectionChanged(haveSelection: false);
		textHighScore.text = "0";
		textMaxScore.text = "0";
		authorNormalTitle = Localizer.Format("#autoLOC_8006112");
		authorSteamTitle = Localizer.Format("#autoLOC_8002161");
		if (dialogFullExpansionMode)
		{
			if (!GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED)
			{
				OnTabStockInCode();
				ConfigNode configNode = new ConfigNode("SCENARIO");
				configNode.AddValue("name", typeof(MissionScreenTutorial).Name);
				configNode.AddValue("scene", "2");
				ScenarioRunner.SetProtoModules(new ProtoScenarioModule(configNode));
				AddTutorialLock();
			}
			else if (missionList.Count <= 0)
			{
				OnTabStockInCode();
			}
		}
		else if (!GameSettings.TUTORIALS_ESA_MISSION_SCREEN_TUTORIAL_COMPLETED)
		{
			OnTabStockInCode();
			ConfigNode configNode2 = new ConfigNode("SCENARIO");
			configNode2.AddValue("name", typeof(ESAMissionScreenTutorial).Name);
			configNode2.AddValue("scene", "2");
			ScenarioRunner.SetProtoModules(new ProtoScenarioModule(configNode2));
			AddTutorialLock();
		}
		else
		{
			OnTabStockInCode();
		}
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic, SliderFocusType.Scrollbar);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnBtnCancel();
		}
	}

	public void UpdateSteamSubscribedItems()
	{
		if (SteamManager.Initialized)
		{
			uint numberSubscribedItems = SteamManager.Instance.GetNumberSubscribedItems();
			steamSubscribedItems = SteamManager.Instance.GetSubscribedItems(numberSubscribedItems);
		}
	}

	public bool SteamItemSubscribed(ulong fileId)
	{
		int num = 0;
		while (true)
		{
			if (num < steamSubscribedItems.Length)
			{
				if (steamSubscribedItems[num].m_PublishedFileId == fileId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void AddTutorialLock()
	{
		GameEvents.OnGameSettingsWritten.Add(OnGameSettingsWritten);
		SetLocked(locked: true);
	}

	public void OnGameSettingsWritten()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			if (GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED)
			{
				RemoveTutorialLock();
			}
		}
		else if (GameSettings.TUTORIALS_ESA_MISSION_SCREEN_TUTORIAL_COMPLETED)
		{
			RemoveTutorialLock();
		}
	}

	public void RemoveTutorialLock()
	{
		GameEvents.OnGameSettingsWritten.Remove(OnGameSettingsWritten);
		SetLocked(locked: false);
	}

	public void Terminate()
	{
		if (buildListCoroutine != null)
		{
			StopCoroutine(buildListCoroutine);
		}
		GameEvents.Mission.onMissionImported.Remove(onMissionImported);
		btnRestart.onClick.RemoveListener(OnButtonRestart);
		btnCancel.onClick.RemoveListener(OnBtnCancel);
		btnLoad.onClick.RemoveListener(OnButtonLoad);
		if (dialogFullExpansionMode)
		{
			btnEdit.onClick.RemoveListener(OnButtonEdit);
			btnCreate.onClick.RemoveListener(OnButtonCreate);
			tabStock.onValueChanged.RemoveListener(OnTabStock);
			tabCommunity.onValueChanged.RemoveListener(OnTabCommunity);
		}
		if (SteamManager.Initialized && dialogFullExpansionMode)
		{
			btnSteamOverlay.onClick.RemoveListener(OnBtnSteamOverlay);
			tabSteam.onValueChanged.RemoveListener(OnTabSteam);
			btnSteamItemDetails.onClick.RemoveListener(onBtnSteamItemDetails);
			btnSteamSubUnSub.onClick.RemoveListener(onBtnSteamSubUnSub);
			btnMainSteamSubUnSub.onClick.RemoveListener(onBtnSteamSubUnSub);
			SteamFilterToggleOne.onValueChanged.RemoveListener(onSteamFilterOne);
			SteamFilterToggleTwo.onValueChanged.RemoveListener(onSteamFilterTwo);
			SteamFilterToggleThree.onValueChanged.RemoveListener(onSteamFilterThree);
			SteamFilterToggleFour.onValueChanged.RemoveListener(onSteamFilterFour);
		}
		RemoveTutorialLock();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void UpdateListOfMissions()
	{
		MissionSystem.RemoveMissionObjects(removeAll: true);
		Instance.selectedMission = null;
		Instance.missionList = MissionsUtils.GatherMissionFiles(Instance.selectedType);
		if (Instance.selectedType == MissionTypes.Stock)
		{
			Instance.missionList.AddRange(MissionsUtils.GatherMissionFiles(MissionTypes.Base, excludeSteamUnsubscribed: true));
		}
		if (Instance.selectedType == MissionTypes.User && SteamManager.Initialized)
		{
			Instance.missionList.AddRange(MissionsUtils.GatherMissionFiles(MissionTypes.Steam, excludeSteamUnsubscribed: true));
		}
		try
		{
			Instance.missionList.Sort(MissionsUtils.SortMissions);
		}
		catch (Exception)
		{
		}
		Instance.missionPacks = MissionsUtils.GatherMissionPacks(Instance.selectedType);
		if (Instance.buildListCoroutine != null)
		{
			Instance.StopCoroutine(Instance.buildListCoroutine);
		}
		Instance.buildListCoroutine = Instance.StartCoroutine(Instance.BuildList());
	}

	public static void UpdateSteamListOfMissions(SteamQueryFilters selectedQueryType)
	{
		MissionSystem.RemoveMissionObjects(removeAll: true);
		Instance.selectedMission = null;
		Instance._selectedSteamMissionItem = null;
		EUGCQuery eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
		string[] tags = new string[1] { "Mission" };
		switch (selectedQueryType)
		{
		default:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
			break;
		case SteamQueryFilters.FEATURED:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByVote;
			tags = new string[2] { "Mission", "Featured" };
			break;
		case SteamQueryFilters.NEWEST:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByPublicationDate;
			break;
		case SteamQueryFilters.SUBSCRIBERS:
			eUGCQuery = EUGCQuery.k_EUGCQuery_RankedByTotalUniqueSubscriptions;
			break;
		}
		Instance.missionPacks = MissionsUtils.GatherMissionPacks(Instance.selectedType);
		MissionsUtils.GetSteamWorkshopMissions(Instance.BuildSteamList, eUGCQuery, tags);
	}

	public void onMissionImported()
	{
		UpdateListOfMissions();
	}

	public IEnumerator BuildList()
	{
		SetHidden(hide: false);
		ClearListItems();
		BuildPacksList();
		CraftProfileInfo.PrepareCraftMetaFileLoad();
		float num = Time.realtimeSinceStartup + maxFrameTime;
		missionList = missionList.OrderByAlphaNumeric((MissionFileInfo m) => m.title);
		int c = missionList.Count;
		int i = 0;
		while (i < c)
		{
			MissionFileInfo mission = missionList[i];
			DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(set: false, string.Empty, delegate(bool b)
			{
				if (b)
				{
					selectedMission = mission;
					if (Mouse.Left.GetDoubleClick(isDelegate: true) && !confirmGameRestart)
					{
						ConfirmLoadGame();
					}
					else
					{
						if (selectedMission != null && selectedMission.missionType == MissionTypes.Steam)
						{
							selectedMission.UpdateSteamState();
						}
						OnSelectionChanged(selectedMission != null);
					}
				}
			}, -1f, 62f);
			dialogGUIToggleButton.guiStyle = skin.customStyles[8];
			dialogGUIToggleButton.OptionInteractableCondition = () => selectedMission != null;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(0, 0, 0, 0), TextAnchor.UpperLeft);
			CreateWidget(mission, dialogGUIVerticalLayout);
			if (mission.IsTutorialMission)
			{
				currentIcon = missionNormalIcon;
			}
			else
			{
				currentIcon = missionDifficultyButtons[(int)mission.difficulty];
			}
			if (mission.missionType == MissionTypes.Steam)
			{
				dialogGUIToggleButton.AddChild(new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(0, 8, 5, 5), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(58f, 58f), Vector2.zero, Color.white, currentIcon), dialogGUIVerticalLayout, new DialogGUIImage(new Vector2(30f, 30f), Vector2.zero, Color.white, steamIcon)));
			}
			else
			{
				dialogGUIToggleButton.AddChild(new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(0, 8, 5, 5), TextAnchor.MiddleLeft, new DialogGUIImage(new Vector2(58f, 58f), Vector2.zero, Color.white, currentIcon), dialogGUIVerticalLayout));
			}
			items.Add(dialogGUIToggleButton, new dialogList());
			items[dialogGUIToggleButton].missionFileInfo = mission;
			items[dialogGUIToggleButton].steamItem = false;
			if (!(Time.realtimeSinceStartup <= num))
			{
				DisplayAddedMissions();
				yield return null;
				num = Time.realtimeSinceStartup + maxFrameTime;
			}
			int num2 = i + 1;
			i = num2;
		}
		DisplayAddedMissions();
		OnSelectionChanged(haveSelection: false);
	}

	public void DisplayAddedMissions()
	{
		for (int i = 0; i < items.KeysList.Count; i++)
		{
			MissionFileInfo missionFileInfo = items.ValuesList[i].missionFileInfo;
			if ((missionFileInfo.packName == null || !packGroups.ContainsKey(missionFileInfo.packName) || !packGroups[missionFileInfo.packName].ContainsMission(missionFileInfo)) && (!packGroups.ContainsKey(MissionListGroup.defaultPackName) || !packGroups[MissionListGroup.defaultPackName].ContainsMission(missionFileInfo)) && (missionFileInfo.packName != null || packGroups.ContainsKey(MissionListGroup.defaultPackName)))
			{
				Stack<Transform> layouts = new Stack<Transform>();
				layouts.Push(base.transform);
				GameObject gameObject = items.KeysList[i].Create(ref layouts, skin);
				if (missionFileInfo.packName != null && packGroups.ContainsKey(missionFileInfo.packName))
				{
					gameObject.transform.SetParent(packGroups[missionFileInfo.packName].containerChildren, worldPositionStays: false);
					packGroups[missionFileInfo.packName].AddMission(missionFileInfo);
				}
				else
				{
					gameObject.transform.SetParent(packGroups[MissionListGroup.defaultPackName].containerChildren, worldPositionStays: false);
					packGroups[MissionListGroup.defaultPackName].AddMission(missionFileInfo);
				}
				items.KeysList[i].toggle.group = listGroup;
			}
		}
		for (int j = 0; j < packGroups.Count; j++)
		{
			if (packGroups.ValuesList[j].Missions.Count > 0 && packGroups.ValuesList[j].transform.parent == null)
			{
				packGroups.ValuesList[j].transform.SetParent(scrollListContent, worldPositionStays: false);
			}
		}
	}

	public void BuildSteamList(SteamUGCQueryCompleted_t results, bool bIOFailure, bool morePages)
	{
		if (bIOFailure || results.m_eResult != EResult.k_EResultOK)
		{
			Debug.LogFormat("[MissionPlayDialog]: Build Steam Items List. Steam Error: {0}", results.m_eResult.ToString());
		}
		if (selectedType != MissionTypes.Steam)
		{
			SteamLoadingText.gameObject.SetActive(value: false);
			OnSelectionChanged(haveSelection: false);
			SteamUGC.ReleaseQueryUGCRequest(results.m_handle);
			return;
		}
		SetHidden(hide: false);
		ClearListItems();
		for (int i = 0; i < results.m_unNumResultsReturned; i++)
		{
			dialogList dialogList = new dialogList();
			SteamUGCDetails_t uGCItemDetails = SteamManager.Instance.GetUGCItemDetails(results.m_handle, (uint)i);
			if (!uGCItemDetails.m_rgchTags.Contains("Mission") || (selectedSteamQueryFilter == SteamQueryFilters.FEATURED && !uGCItemDetails.m_rgchTags.Contains("Featured")))
			{
				continue;
			}
			SteamMissionFileInfo steamMissionFileInfo = new SteamMissionFileInfo(uGCItemDetails);
			dialogList.steamMissionFileInfo = steamMissionFileInfo;
			SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumFavorites, out steamMissionFileInfo.totalFavorites);
			SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumFollowers, out steamMissionFileInfo.totalFollowers);
			SteamUGC.GetQueryUGCStatistic(results.m_handle, (uint)i, EItemStatistic.k_EItemStatistic_NumSubscriptions, out steamMissionFileInfo.totalSubscriptions);
			SteamUGC.GetQueryUGCPreviewURL(results.m_handle, (uint)i, out steamMissionFileInfo.previewURL, (uint)uGCItemDetails.m_nPreviewFileSize);
			string pchMetadata = "";
			SteamUGC.GetQueryUGCMetadata(results.m_handle, (uint)i, out pchMetadata, 5000u);
			steamMissionFileInfo.ProcessMetaData(pchMetadata);
			MissionSteamItemWidget missionSteamItemWidget = MissionSteamItemWidget.Create(delegate(bool b)
			{
				if (b)
				{
					_selectedSteamMissionItem = steamMissionFileInfo;
					OnSelectionChanged(haveSelection: true, steamView: true);
				}
			});
			CreateSteamWidget(uGCItemDetails, steamMissionFileInfo, missionSteamItemWidget);
			currentIcon = missionDifficultyButtons[(int)steamMissionFileInfo.missionDifficulty];
			missionSteamItemWidget.missionIcon.texture = currentIcon;
			steamItems.Add(missionSteamItemWidget, dialogList);
			steamItems[missionSteamItemWidget].missionFileInfo = new MissionFileInfo("", MissionTypes.Steam);
			steamItems[missionSteamItemWidget].missionFileInfo.title = uGCItemDetails.m_rgchTitle;
			steamItems[missionSteamItemWidget].missionFileInfo.briefing = uGCItemDetails.m_rgchDescription;
			steamItems[missionSteamItemWidget].steamItem = true;
			steamMissionFileInfo.SetMissionFileInfo(ref steamItems[missionSteamItemWidget].missionFileInfo);
		}
		for (int j = 0; j < steamItems.KeysList.Count; j++)
		{
			new Stack<Transform>().Push(base.transform);
			steamItems.KeysList[j].gameObject.transform.SetParent(scrollListContent, worldPositionStays: false);
			steamItems.KeysList[j].gameObject.GetComponent<Toggle>().group = listGroup;
		}
		SteamLoadingText.gameObject.SetActive(value: false);
		OnSelectionChanged(haveSelection: false, steamView: true);
		SteamUGC.ReleaseQueryUGCRequest(results.m_handle);
	}

	public void BuildPacksList()
	{
		ClearPacksList();
		if (selectedType == MissionTypes.User || selectedType == MissionTypes.Steam)
		{
			MissionListGroup missionListGroup = MissionListGroup.CreateDefault();
			packGroups.Add(missionListGroup.Pack.name, missionListGroup);
		}
		for (int i = 0; i < missionPacks.Count; i++)
		{
			MissionListGroup missionListGroup = MissionListGroup.Create(missionPacks[i]);
			if (missionListGroup != null)
			{
				packGroups.Add(missionListGroup.Pack.name, missionListGroup);
			}
		}
		if (selectedType == MissionTypes.Stock)
		{
			MissionListGroup missionListGroup = MissionListGroup.CreateDefault();
			packGroups.Add(missionListGroup.Pack.name, missionListGroup);
		}
	}

	public void ClearPacksList()
	{
		if (packGroups == null)
		{
			packGroups = new DictionaryValueList<string, MissionListGroup>();
			return;
		}
		int count = packGroups.Count;
		while (count-- > 0)
		{
			MissionListGroup missionListGroup = packGroups.ValuesList[count];
			if (missionListGroup != null)
			{
				UnityEngine.Object.Destroy(missionListGroup.gameObject);
			}
		}
		packGroups.Clear();
	}

	public void CreateWidget(MissionFileInfo mission, DialogGUIVerticalLayout vLayout)
	{
		DialogGUILabel.TextLabelOptions textLabelOptions = new DialogGUILabel.TextLabelOptions
		{
			enableWordWrapping = false,
			OverflowMode = TextOverflowModes.Ellipsis,
			resizeBestFit = true,
			resizeMinFontSize = 11,
			resizeMaxFontSize = 12
		};
		DialogGUILabel.TextLabelOptions textLabelOptions2 = new DialogGUILabel.TextLabelOptions
		{
			enableWordWrapping = false,
			OverflowMode = TextOverflowModes.Overflow,
			resizeBestFit = true,
			resizeMinFontSize = 11,
			resizeMaxFontSize = 12
		};
		DialogGUILabel dialogGUILabel;
		if (mission == null)
		{
			dialogGUILabel = new DialogGUILabel(mission.title, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = textLabelOptions2;
			vLayout.AddChild(dialogGUILabel);
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8006019"), skin.customStyles[1], expandW: true));
			return;
		}
		dialogGUILabel = new DialogGUILabel((!string.IsNullOrEmpty(mission.title)) ? mission.title : mission.folderName, skin.customStyles[0], expandW: true);
		dialogGUILabel.textLabelOptions = textLabelOptions;
		vLayout.AddChild(dialogGUILabel);
		int num = 0;
		if (mission.HasSave)
		{
			if (mission.MetaSavedMission == null)
			{
				useMissionData(mission, vLayout, textLabelOptions2);
				return;
			}
			string text = KSPUtil.PrintDate(mission.MetaSavedMission.double_0, includeTime: true);
			if (mission.MetaSavedMission.missionIsEnded && dialogFullExpansionMode)
			{
				ScoreInfo missionScore = missionScoreInfo.GetMissionScore(mission.MetaMission.id, 0);
				string text2 = ((missionScore == null) ? "0" : missionScore.score.ToString());
				vLayout.AddChild(new DialogGUILabel(string.Format("<color=" + XKCDColors.HexFormat.ElectricLime + ">{0, -21}</color>", Localizer.Format(Localizer.Format("#autoLOC_8003010"), mission.MetaSavedMission.missionCurrentScore, text2)), skin.customStyles[1], expandW: true)
				{
					textLabelOptions = textLabelOptions2
				});
				if (mission.missionType == MissionTypes.Steam && !string.IsNullOrEmpty(mission.steamStateText))
				{
					vLayout.AddChild(new DialogGUILabel(mission.steamStateText, skin.customStyles[2], expandW: true)
					{
						textLabelOptions = textLabelOptions2
					});
				}
				else
				{
					vLayout.AddChild(new DialogGUILabel(Localizer.Format(Localizer.Format("#autoLOC_8003011"), text), skin.customStyles[2], expandW: true)
					{
						textLabelOptions = textLabelOptions2
					});
				}
				return;
			}
			string errorString = string.Empty;
			HashSet<string> incompatibleCraftHashSet = new HashSet<string>();
			if (!mission.IsCompatible())
			{
				vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004248"), skin.customStyles[1], expandW: true)
				{
					textLabelOptions = textLabelOptions2
				});
				return;
			}
			if (!mission.IsCraftCompatible(ref errorString, ref incompatibleCraftHashSet, checkSaveIfAvailable: true))
			{
				vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004256"), skin.customStyles[1], expandW: true)
				{
					textLabelOptions = textLabelOptions2
				});
				return;
			}
			num = mission.MetaSavedMission.vesselCount;
			vLayout.AddChild(new DialogGUILabel(string.Format("<color=" + XKCDColors.HexFormat.ElectricLime + ">{0, -21}</color>", Localizer.Format("#autoLOC_8004149", mission.MetaSavedMission.missionCurrentScore)), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = textLabelOptions2
			});
			if (mission.missionType == MissionTypes.Steam && !string.IsNullOrEmpty(mission.steamStateText))
			{
				vLayout.AddChild(new DialogGUILabel(mission.steamStateText, skin.customStyles[2], expandW: true)
				{
					textLabelOptions = textLabelOptions2
				});
			}
			else
			{
				vLayout.AddChild(new DialogGUILabel(text + " " + Localizer.Format("#autoLOC_8004150", num), skin.customStyles[2], expandW: true)
				{
					textLabelOptions = textLabelOptions2
				});
			}
		}
		else
		{
			useMissionData(mission, vLayout, textLabelOptions2);
		}
	}

	public void CreateSteamWidget(SteamUGCDetails_t itemDetails, SteamMissionFileInfo steamMissionFileInfo, MissionSteamItemWidget toggle)
	{
		toggle.SetHeaderText(itemDetails.m_rgchTitle);
		toggle.favoritetext.text = steamMissionFileInfo.totalFavorites.ToString();
		toggle.thumbsUptext.text = itemDetails.m_unVotesUp.ToString();
		toggle.thumbsDowntext.text = itemDetails.m_unVotesDown.ToString();
		toggle.SetSubIconState(SteamItemSubscribed(itemDetails.m_nPublishedFileId.m_PublishedFileId));
	}

	public void useMissionData(MissionFileInfo mission, DialogGUIVerticalLayout vLayout, DialogGUILabel.TextLabelOptions labelOptsForSaveDetails)
	{
		string errorString = string.Empty;
		HashSet<string> incompatibleCraftHashSet = new HashSet<string>();
		ScoreInfo scoreInfo = null;
		if (dialogFullExpansionMode)
		{
			scoreInfo = missionScoreInfo.GetMissionScore(mission.MetaMission.id, 0);
		}
		if (!mission.IsCompatible())
		{
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004248"), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = labelOptsForSaveDetails
			});
		}
		else if (!mission.IsCraftCompatible(ref errorString, ref incompatibleCraftHashSet, checkSaveIfAvailable: false))
		{
			vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004256"), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = labelOptsForSaveDetails
			});
		}
		else if (scoreInfo == null)
		{
			if (mission.MetaMission.packName == null || (mission.MetaMission.packName != null && !mission.MetaMission.packName.ToLower().Contains("tutorial")))
			{
				vLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8003012"), skin.customStyles[2], expandW: true)
				{
					textLabelOptions = labelOptsForSaveDetails
				});
			}
		}
		else
		{
			string text = ((scoreInfo == null) ? "0" : scoreInfo.score.ToString());
			vLayout.AddChild(new DialogGUILabel(string.Format("<color=" + XKCDColors.HexFormat.ElectricLime + ">{0, -21}</color>", Localizer.Format(Localizer.Format("#autoLOC_8003013"), text)), skin.customStyles[1], expandW: true)
			{
				textLabelOptions = labelOptsForSaveDetails
			});
		}
		if (mission.missionType == MissionTypes.Steam && !string.IsNullOrEmpty(mission.steamStateText))
		{
			vLayout.AddChild(new DialogGUILabel(mission.steamStateText, skin.customStyles[2], expandW: true)
			{
				textLabelOptions = labelOptsForSaveDetails
			});
		}
	}

	public void ClearListItems()
	{
		for (int i = 0; i < items.Count; i++)
		{
			UnityEngine.Object.Destroy(items.KeysList[i].uiItem);
		}
		items.Clear();
		for (int j = 0; j < steamItems.Count; j++)
		{
			UnityEngine.Object.Destroy(steamItems.KeysList[j].gameObject);
		}
		steamItems.Clear();
		selectedMission = null;
		OnSelectionChanged(haveSelection: false);
	}

	public void SetHidden(bool hide)
	{
		if (hide)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void SetLocked(bool locked)
	{
		for (int i = 0; i < items.KeysList.Count; i++)
		{
			if (items.KeysList[i].toggle != null)
			{
				items.KeysList[i].toggle.interactable = !locked;
			}
		}
		btnCancel.interactable = !locked;
		btnLoad.interactable = !locked && selectedMission != null;
		if (btnCreate != null)
		{
			btnCreate.interactable = !locked;
		}
	}

	public void OnTabStock(bool value)
	{
		if (dialogFullExpansionMode && selectedType != 0)
		{
			selectedType = MissionTypes.Stock;
			btnSteamOverlay.gameObject.SetActive(value: false);
			SteamFilterHolder.SetActive(value: false);
			ButtonSection.SetActive(value: true);
			SteamButtonSection.SetActive(value: false);
			UpdateListOfMissions();
		}
	}

	public void OnTabCommunity(bool value)
	{
		if (dialogFullExpansionMode && selectedType != MissionTypes.User)
		{
			selectedType = MissionTypes.User;
			btnSteamOverlay.gameObject.SetActive(value: false);
			SteamFilterHolder.SetActive(value: false);
			ButtonSection.SetActive(value: true);
			SteamButtonSection.SetActive(value: false);
			UpdateSteamSubscribedItems();
			UpdateListOfMissions();
		}
	}

	public void OnTabSteam(bool value)
	{
		if (dialogFullExpansionMode && selectedType != MissionTypes.Steam)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			selectedType = MissionTypes.Steam;
			btnSteamOverlay.gameObject.SetActive(value: true);
			SteamFilterHolder.SetActive(value: true);
			ClearListItems();
			ClearPacksList();
			ButtonSection.SetActive(value: false);
			SteamButtonSection.SetActive(value: true);
			UpdateSteamSubscribedItems();
			UpdateSteamListOfMissions(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterOne(bool value)
	{
		if (dialogFullExpansionMode && selectedSteamQueryFilter != 0)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearListItems();
			ClearPacksList();
			selectedSteamQueryFilter = SteamQueryFilters.VOTE;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfMissions(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterTwo(bool value)
	{
		if (dialogFullExpansionMode && selectedSteamQueryFilter != SteamQueryFilters.FEATURED)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearListItems();
			ClearPacksList();
			selectedSteamQueryFilter = SteamQueryFilters.FEATURED;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfMissions(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterThree(bool value)
	{
		if (dialogFullExpansionMode && selectedSteamQueryFilter != SteamQueryFilters.NEWEST)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearListItems();
			ClearPacksList();
			selectedSteamQueryFilter = SteamQueryFilters.NEWEST;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfMissions(selectedSteamQueryFilter);
		}
	}

	public void onSteamFilterFour(bool value)
	{
		if (dialogFullExpansionMode && selectedSteamQueryFilter != SteamQueryFilters.SUBSCRIBERS)
		{
			SteamLoadingText.gameObject.SetActive(value: true);
			ClearListItems();
			ClearPacksList();
			selectedSteamQueryFilter = SteamQueryFilters.SUBSCRIBERS;
			UpdateSteamSubscribedItems();
			UpdateSteamListOfMissions(selectedSteamQueryFilter);
		}
	}

	public void OnBtnSteamOverlay()
	{
		if (dialogFullExpansionMode)
		{
			string text = "https://steamcommunity.com/workshop/browse/?appid=220200&searchtext=&childpublishedfileid=0&section=readytouseitems&days=-1";
			switch (selectedSteamQueryFilter)
			{
			case SteamQueryFilters.VOTE:
				text += "&section=readytouseitems&days=-1&browsesort=trend&requiredtags%5B%5D=Mission";
				break;
			case SteamQueryFilters.FEATURED:
				text += "&browsesort=trend&requiredtags%5B%5D=Mission&requiredtags%5B%5D=Featured";
				break;
			case SteamQueryFilters.NEWEST:
				text += "&section=readytouseitems&days=-1&browsesort=mostrecent&requiredtags%5B%5D=Mission";
				break;
			case SteamQueryFilters.SUBSCRIBERS:
				text += "&section=readytouseitems&days=-1&browsesort=totaluniquesubscribers&requiredtags%5B%5D=Mission";
				break;
			}
			SteamFriends.ActivateGameOverlayToWebPage(text);
		}
	}

	public void onBtnSteamItemDetails()
	{
		if (dialogFullExpansionMode)
		{
			string text = "steam://url/CommunityFilePage/" + _selectedSteamMissionItem.itemDetails.m_nPublishedFileId.m_PublishedFileId;
			Debug.Log("Opening: " + text);
			SteamFriends.ActivateGameOverlayToWebPage(text);
		}
	}

	public void onBtnSteamSubUnSub()
	{
		if (!dialogFullExpansionMode)
		{
			return;
		}
		ulong fileId = 0uL;
		if (selectedType == MissionTypes.Steam)
		{
			fileId = _selectedSteamMissionItem.itemDetails.m_nPublishedFileId.m_PublishedFileId;
		}
		else
		{
			fileId = selectedMission.GetSteamFileId();
		}
		if (SteamItemSubscribed(fileId))
		{
			UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8002132"), Localizer.Format("#autoLOC_8002133"), Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_360842"), delegate(bool b)
			{
				SaveMissionSteamUnsubscribeWarning(b);
				SteamManager.Instance.unsubscribeItem(new PublishedFileId_t(fileId), onUnsubscribeItemCallback);
			}, delegate
			{
			});
		}
		else
		{
			SteamManager.Instance.subscribeItem(new PublishedFileId_t(fileId), onSubscribeItemCallback);
		}
	}

	public void onUnsubscribeItemCallback(RemoteStorageUnsubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		if (!bIOFailure && callResult.m_eResult == EResult.k_EResultOK)
		{
			AnalyticsUtil.LogSteamItemUnsubscribed(AnalyticsUtil.steamItemTypes.mission, callResult.m_nPublishedFileId);
			SteamSubUnsubText.text = Localizer.Format("#autoLOC_8002134");
			UpdateSteamSubscribedItems();
			if (selectedType == MissionTypes.Steam)
			{
				UpdateSteamListOfMissions(selectedSteamQueryFilter);
			}
			else
			{
				UpdateListOfMissions();
			}
		}
		else
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.unsubscribe, AnalyticsUtil.steamItemTypes.mission, callResult.m_nPublishedFileId, callResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002136", SteamManager.Instance.GetUGCFailureReason(callResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), skin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, skin);
		}
	}

	public void onSubscribeItemCallback(RemoteStorageSubscribePublishedFileResult_t callResult, bool bIOFailure)
	{
		if (!bIOFailure && callResult.m_eResult == EResult.k_EResultOK)
		{
			AnalyticsUtil.LogSteamItemSubscribed(AnalyticsUtil.steamItemTypes.mission, callResult.m_nPublishedFileId);
			SteamSubUnsubText.text = Localizer.Format("#autoLOC_8002135");
			UpdateSteamSubscribedItems();
			if (selectedType == MissionTypes.Steam)
			{
				UpdateSteamListOfMissions(selectedSteamQueryFilter);
			}
			else
			{
				UpdateListOfMissions();
			}
		}
		else
		{
			AnalyticsUtil.LogSteamError(AnalyticsUtil.steamActions.subscribe, AnalyticsUtil.steamItemTypes.mission, callResult.m_nPublishedFileId, callResult.m_eResult);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SteamError", Localizer.Format("#autoLOC_8002137", SteamManager.Instance.GetUGCFailureReason(callResult.m_eResult)), Localizer.Format("#autoLOC_8002125"), skin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_226975"), null, dismissOnSelect: true)), persistAcrossScenes: false, skin);
		}
	}

	public void SaveMissionSteamUnsubscribeWarning(bool dontShowAgain)
	{
		if (dontShowAgain == GameSettings.MISSION_STEAM_UNSUBSCRIBE_WARNING)
		{
			GameSettings.MISSION_STEAM_UNSUBSCRIBE_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
	}

	public void OnTabStockInCode()
	{
		if (dialogFullExpansionMode)
		{
			tabCommunity.isOn = false;
			tabStock.isOn = true;
			tabSteam.isOn = false;
			OnTabStock(tabStock.isOn);
		}
	}

	public void OnButtonEdit()
	{
		if (dialogFullExpansionMode)
		{
			MissionSystem.RemoveMissionObjects(removeAll: true);
			MissionEditorLogic.StartUpMissionEditor(selectedMission.FilePath);
			OnCloseWindow();
		}
	}

	public void OnButtonCreate()
	{
		if (dialogFullExpansionMode)
		{
			MissionSystem.RemoveMissionObjects(removeAll: true);
			MissionsUtils.OpenMissionBuilder();
			OnCloseWindow();
		}
	}

	public void OnButtonRestart()
	{
		confirmGameRestart = true;
		SetLocked(locked: true);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8006014"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_8006013"), ConfirmRestart, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), DismissRestartGame, dismissOnSelect: true)), persistAcrossScenes: false, skin).OnDismiss = DismissRestartGame;
	}

	public void OnBtnCancel()
	{
		CancelLoadGame();
	}

	public void OnButtonLoad()
	{
		if (selectedMission.IsCompatible())
		{
			CraftProfileInfo.PrepareCraftMetaFileLoad();
			string errorString = string.Empty;
			HashSet<string> incompatibleCraftHashSet = new HashSet<string>();
			if (selectedMission.IsCraftCompatible(ref errorString, ref incompatibleCraftHashSet, checkSaveIfAvailable: true))
			{
				ConfirmLoadGame();
				return;
			}
			SetLocked(locked: true);
			DialogGUILabel dialogGUILabel = new DialogGUILabel(errorString, skin.customStyles[0]);
			dialogGUILabel.bypassTextStyleColor = true;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 11, 4, 4), TextAnchor.UpperLeft, dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004242"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUISpace(6f), new DialogGUIScrollList(new Vector2(-1f, 80f), hScroll: false, vScroll: true, dialogGUIVerticalLayout), new DialogGUISpace(6f), new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				SetLocked(locked: false);
			}, 150f, -1f, true))), persistAcrossScenes: false, skin);
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004241"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				SetLocked(locked: false);
			}, 150f, -1f, true))), persistAcrossScenes: false, skin);
		}
	}

	public void DismissRestartGame()
	{
		listGroup.SetAllTogglesOff();
		confirmGameRestart = false;
		selectedMission = null;
		OnSelectionChanged(haveSelection: false);
		SetLocked(locked: false);
	}

	public void ConfirmLoadGame()
	{
		SetLocked(locked: false);
		if (selectedMission.SimpleMission == null)
		{
			Debug.LogError("[Mission] Unable to start mission, invalid or broken game save file.");
		}
		else
		{
			if (selectedMission.SimpleMission.BlockPlayMission(showDialog: true))
			{
				return;
			}
			if (selectedMission.IsTutorialMission)
			{
				OnButtonEdit();
				return;
			}
			if (OnDialogDismissCallback != null)
			{
				OnDialogDismissCallback();
			}
			OnFinishedCallback(selectedMission);
		}
	}

	public void CancelLoadGame()
	{
		OnCloseWindow();
	}

	public void OnCloseWindow()
	{
		if (OnDialogDismissCallback != null)
		{
			OnDialogDismissCallback();
		}
		OnFinishedCallback(null);
		MissionSystem.RemoveMissionObjects();
		Terminate();
	}

	public void ConfirmRestart()
	{
		if (Directory.Exists(selectedMission.SaveFolderPath))
		{
			FileInfo[] files = new DirectoryInfo(selectedMission.SaveFolderPath).GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
		}
		MissionFileInfo missionFileInfo = selectedMission;
		UpdateListOfMissions();
		DismissRestartGame();
		for (int j = 0; j < items.KeysList.Count; j++)
		{
			MissionFileInfo missionFileInfo2 = items.ValuesList[j].missionFileInfo;
			if (missionFileInfo2.FilePath == missionFileInfo.FilePath)
			{
				selectedMission = missionFileInfo2;
				items.KeysList[j].toggle.isOn = true;
				OnSelectionChanged(selectedMission != null);
				break;
			}
		}
		int count = missionList.Count;
		int num = 0;
		MissionFileInfo missionFileInfo3;
		while (true)
		{
			if (num < count)
			{
				missionFileInfo3 = missionList[num];
				if (missionFileInfo3.FilePath == missionFileInfo.FilePath)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		selectedMission = missionFileInfo3;
		OnSelectionChanged(selectedMission != null);
	}

	public void OnSelectionChanged(bool haveSelection, bool steamView = false)
	{
		if (dialogFullExpansionMode)
		{
			UpdateSteamSubscribedItems();
			btnMainSteamSubUnSub.gameObject.SetActive(value: false);
		}
		if (haveSelection && !steamView && (!dialogFullExpansionMode || GameSettings.TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED))
		{
			containerDefault.gameObject.SetActive(value: false);
			containerInfo.gameObject.SetActive(value: true);
			btnLoad.interactable = true;
			textTitle.text = selectedMission.title;
			textDescription.text = selectedMission.briefing;
			AuthorTitleText.text = authorNormalTitle;
			textAuthor.text = selectedMission.author;
			if (selectedMission.IsTutorialMission)
			{
				difficultyField.SetActive(value: false);
			}
			else
			{
				difficultyField.SetActive(value: true);
				difficultyHead.sprite = difficultyHeads[(int)selectedMission.difficulty];
				textDifficulty.text = selectedMission.difficulty.displayDescription();
			}
			if (!string.IsNullOrEmpty(selectedMission.modsBriefing))
			{
				modsSection.SetActive(value: true);
				textModsDescription.text = selectedMission.modsBriefing;
			}
			else
			{
				modsSection.SetActive(value: false);
			}
			btnLoadCaption.text = (selectedMission.HasSave ? "#autoLOC_900660" : "#autoLOC_900675");
			btnLoad.interactable = !selectedMission.HasSave || !selectedMission.MetaSavedMission.missionIsEnded;
			btnRestart.interactable = selectedMission.HasSave;
			if (btnEdit != null)
			{
				btnEdit.interactable = !selectedMission.IsTutorialMission;
			}
			if (dialogFullExpansionMode)
			{
				highScoreField.SetActive(value: true);
				awardsHeader.SetActive(value: true);
				awardsHolder.SetActive(value: true);
			}
			progressField.SetActive(value: true);
			objectivesHeader.SetActive(value: true);
			SteamSection.SetActive(value: false);
			Mission simpleMission = selectedMission.SimpleMission;
			if (simpleMission != null && dialogFullExpansionMode)
			{
				textMaxScore.text = simpleMission.maxScore.ToString("F0");
				maxScoreField.SetActive(Mathf.Abs(simpleMission.maxScore) > float.Epsilon);
				ScoreInfo missionScore = missionScoreInfo.GetMissionScore(simpleMission, 0);
				textHighScore.text = ((missionScore != null) ? missionScore.score.ToString("F0") : "0");
			}
			else
			{
				maxScoreField.SetActive(value: false);
			}
			if (dialogFullExpansionMode)
			{
				int i = 0;
				for (int childCount = awardsContent.childCount; i < childCount; i++)
				{
					UnityEngine.Object.Destroy(awardsContent.GetChild(i).gameObject);
				}
				List<string> missionAwards = missionScoreInfo.GetMissionAwards(simpleMission);
				int j = 0;
				for (int count = missionAwards.Count; j < count; j++)
				{
					awardWidgetPrefab.Create(missionAwards[j], awardsContent);
				}
			}
			if (simpleMission != null)
			{
				if (flowParser == null)
				{
					flowParser = MEFlowParser.Create(base.gameObject.transform, MEFlowObjectsParent, MEFlowEmptyObjectivesMessage);
				}
				flowParser.CreateMissionFlowUI_Button(simpleMission);
				imgScnBanner.texture = simpleMission.GetBanner(MEBannerType.Menu).texture;
			}
			else
			{
				Debug.LogWarning("Mission " + selectedMission.title + " failed to initialize. Cannot display Objectives.");
			}
			if (selectedMission.missionType == MissionTypes.Steam)
			{
				btnMainSteamSubUnSub.gameObject.SetActive(value: true);
				if (selectedMission.subscribed)
				{
					MainSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002135");
				}
				else
				{
					MainSteamSubUnsubText.text = Localizer.Format("#autoLOC_8002134");
				}
			}
		}
		else if (dialogFullExpansionMode && haveSelection && steamView)
		{
			containerDefault.gameObject.SetActive(value: false);
			containerInfo.gameObject.SetActive(value: true);
			difficultyField.SetActive(value: true);
			modsSection.SetActive(value: false);
			btnSteamItemDetails.interactable = true;
			btnSteamSubUnSub.interactable = true;
			maxScoreField.SetActive(value: false);
			highScoreField.SetActive(value: false);
			awardsHeader.SetActive(value: false);
			awardsHolder.SetActive(value: false);
			progressField.SetActive(value: false);
			objectivesHeader.SetActive(value: false);
			SteamSection.SetActive(value: true);
			AuthorTitleText.text = authorSteamTitle;
			imgScnBanner.texture = GameDatabase.Instance.GetTexture("SquadExpansion/MakingHistory/Banners/Menu/default.png", asNormalMap: false);
			if (_selectedSteamMissionItem != null)
			{
				textTitle.text = _selectedSteamMissionItem.itemDetails.m_rgchTitle;
				textDescription.text = _selectedSteamMissionItem.itemDetails.m_rgchDescription;
				textAuthor.text = SteamManager.Instance.getUserName(_selectedSteamMissionItem.itemDetails.m_ulSteamIDOwner, textAuthor);
				difficultyHead.sprite = difficultyHeads[(int)_selectedSteamMissionItem.missionDifficulty];
				textDifficulty.text = _selectedSteamMissionItem.missionDifficulty.displayDescription();
				if (!string.IsNullOrEmpty(_selectedSteamMissionItem.modsBriefing))
				{
					modsSection.SetActive(value: true);
					textModsDescription.text = _selectedSteamMissionItem.modsBriefing;
				}
				float num = (float)_selectedSteamMissionItem.itemDetails.m_nFileSize / 1024f / 1024f;
				SteamFileSizeText.text = num.ToString("N2");
				SteamUpVotesText.text = _selectedSteamMissionItem.itemDetails.m_unVotesUp.ToString();
				SteamDownVotesText.text = _selectedSteamMissionItem.itemDetails.m_unVotesDown.ToString();
				SteamSubscribersText.text = _selectedSteamMissionItem.totalSubscriptions.ToString();
				if (SteamItemSubscribed(_selectedSteamMissionItem.itemDetails.m_nPublishedFileId.m_PublishedFileId))
				{
					SteamSubUnsubText.text = Localizer.Format("#autoLOC_8002135");
				}
				else
				{
					SteamSubUnsubText.text = Localizer.Format("#autoLOC_8002134");
				}
				StartCoroutine(LoadSteamItemPreviewURL());
			}
			else
			{
				btnSteamItemDetails.interactable = false;
				btnSteamSubUnSub.interactable = false;
			}
			int k = 0;
			for (int childCount2 = awardsContent.childCount; k < childCount2; k++)
			{
				UnityEngine.Object.Destroy(awardsContent.GetChild(k).gameObject);
			}
		}
		else
		{
			btnLoad.interactable = false;
			if (btnEdit != null)
			{
				btnEdit.interactable = false;
			}
			if (btnSteamItemDetails != null)
			{
				btnSteamItemDetails.interactable = false;
			}
			if (btnSteamSubUnSub != null)
			{
				btnSteamSubUnSub.interactable = false;
			}
			if (selectedType == MissionTypes.Steam)
			{
				textTitle.text = Localizer.Format("#autoLOC_8002138");
			}
			else
			{
				textTitle.text = Localizer.Format("#autoLOC_8200122");
			}
			textDescription.text = Localizer.Format("#autoLOC_464935");
			textAuthor.text = Localizer.Format("#autoLOC_8006111");
			containerDefault.gameObject.SetActive(value: true);
			containerInfo.gameObject.SetActive(value: false);
			btnRestart.interactable = false;
		}
	}

	public IEnumerator LoadSteamItemPreviewURL()
	{
		if (_selectedSteamMissionItem == null)
		{
			yield break;
		}
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(_selectedSteamMissionItem.previewURL);
		yield return www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (!www.isNetworkError && !www.isHttpError && string.IsNullOrEmpty(www.error))
		{
			Texture2D content = DownloadHandlerTexture.GetContent(www);
			Texture2D texture2D = new Texture2D(content.width, content.height, TextureFormat.RGBA32, mipChain: false);
			byte[] data = content.EncodeToPNG();
			texture2D.LoadImage(data);
			imgScnBanner.texture = texture2D;
			yield break;
		}
		Debug.LogError("LoadSteamItemPreviewURL - WWW error in " + _selectedSteamMissionItem.previewURL + " : " + www.error);
	}
}
