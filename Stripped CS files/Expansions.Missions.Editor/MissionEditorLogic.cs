using System;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Actions;
using Expansions.Missions.Flow;
using Expansions.Missions.Runtime;
using KSPAssets.KSPedia;
using ns11;
using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionEditorLogic : MonoBehaviour, IMEHistoryTarget
{
	public static string startUpMissionPath;

	public static string MissionToTest;

	public TMP_InputField titleText;

	public TooltipController_Text titleTooltip;

	[NonSerialized]
	public Awards awardDefinitions;

	public Button buttonBrief;

	public Button buttonExit;

	public Button buttonSave;

	public Button buttonLoad;

	public Button buttonNew;

	public Button buttonLayout;

	public Button buttonLayoutGAP;

	public Button buttonExport;

	public Button buttonTest;

	public Button buttonKSPedia;

	public Button buttonUndo;

	public Button buttonRedo;

	public Toggle buttonMaximizeCanvas;

	public Toggle buttonMaximizeGAP;

	public Button buttonFit;

	public Toggle buttonSnap;

	public Button buttonArrange;

	public Button buttonTS;

	public UIStateImage maximizeCanvasImage;

	public UIStateImage snapGridImage;

	public UIStateImage maximizeGAPImage;

	public MissionsBrowserDialog loadMissionDialog;

	public bool disallowSave;

	public CheckpointBrowserDialog testMissionDialog;

	public ScreenMessage modeMsg;

	public MEGUIConnector CurrentConnector;

	public MEGUIConnector meConnectorPrefab;

	public MEGUINode MENodePrefab;

	public MENodeColors MENodeColorConfig;

	[SerializeField]
	public MEGUINode.ColoredConnectorOptions ColoredConnectorPins;

	[SerializeField]
	public GameObject prefabNodeCanvas;

	public TextMeshProUGUI zoom_level;

	public Button zoom_buttonPlus;

	public Button zoom_buttonMinus;

	public MENodeCanvas NodeCanvas;

	public RectTransform NodeCanvasUIRect;

	public Canvas canvas;

	public MEActionPane actionPane;

	public MEGUIPanel[] Panels;

	public MEGUINode _currentlySelectedNode;

	[HideInInspector]
	public List<MEGUINode> selectedNodes = new List<MEGUINode>();

	public List<MEGUINode> copiedGUINodes = new List<MEGUINode>();

	[HideInInspector]
	public List<MEGUIConnector> selectedConnectors = new List<MEGUIConnector>();

	[SerializeField]
	public Vector2 pastedNodeOffset = Vector2.zero;

	[HideInInspector]
	public Vector2 nodeDragPos;

	[HideInInspector]
	public Vector2 lastFramePos;

	public Vector2 cachePositionForPastedNodes;

	public int nodeGroupIndex;

	public int connectorGroupIndex;

	public float maxGridXoffset;

	public float connectorSelectionAllowance;

	public MEGUIConnector _currentlySelectedConnector;

	public List<MEGUIConnector> editorConnectorList;

	public List<MEGUINode> editorNodeList;

	public List<MEBasicNode> basicNodes;

	public bool actionPaneMoving;

	public bool isMouseOverGAPScroll;

	public string MissionNameAtLastSave;

	public bool briefingHasChanged;

	public float distanceForNodeDocking;

	public MECrewAssignmentDialog crewAssignmentDialog;

	public Dictionary<uint, ShipConstruct> vesselCache;

	public RectTransform toolBox;

	[SerializeField]
	public Button validationReport;

	[SerializeField]
	public Image validationButtonImage;

	[SerializeField]
	public Button validationRun;

	[SerializeField]
	public TMP_InputField searchInput;

	[SerializeField]
	public CanvasGroupInputLock nodeFilterLock;

	[SerializeField]
	public CanvasGroupInputLock nodeListLock;

	[SerializeField]
	public CanvasGroupInputLock settingsActionPaneLock;

	[SerializeField]
	public Color TutorialHighlighterColor = Color.white;

	public Color parameterSelectionOriginalColor;

	public const string BLUE_PARAMETER_SELECTION_COLOR_HEXA = "#193D9896";

	public const string ORIGINAL_PARAMETER_SELECTION_COLOR_HEXA = "#7B7F8996";

	public GameObject currentSelectedGameObject;

	public Action<GameObject> OnSelectedGameObjectChange;

	public Action OnLeave;

	public Action OnExitEditor;

	public List<MissionPack> userMissionPacks;

	public List<MissionPack> stockMissionPacks;

	public bool isCmd;

	public double startTime;

	public ConfigNode missionToLoadRoot;

	public MissionFileInfo missionToLoadMFI;

	public string missionToLoadFullPath;

	public HashSet<string> incompatibleCraft = new HashSet<string>();

	public MissionBriefingDialog onBriefBriefingDialog;

	public UIConfirmDialog openingMissionSaveConfirmDialog;

	public UIConfirmDialog newTestConfirmDialog;

	public MissionsBrowserDialog onLoadConfirmDialog;

	public MissionBriefingDialog onSaveConfirmDialog;

	public PopupDialog unableToStartMission;

	public PopupDialog onExitMissionBuilderDialog;

	public PopupDialog dialogSelectEditor;

	public MissionValidationDialog missionValidationDialog;

	public TMP_InputField tabNextInputfield;

	public Guid lastHistoryStateIDAtLastSave;

	public static MissionEditorLogic Instance { get; set; }

	public Canvas Canvas
	{
		get
		{
			return canvas;
		}
		set
		{
			canvas = value;
		}
	}

	public MEGUINode CurrentSelectedNode
	{
		get
		{
			return _currentlySelectedNode;
		}
		set
		{
			if (CurrentlySelectedConnector != null && value != null)
			{
				CurrentlySelectedConnector = null;
			}
			_currentlySelectedNode = value;
			actionPane.SAPDisplayNodeParameters(_currentlySelectedNode);
		}
	}

	public bool PreventNodeDestruction { get; set; }

	public MEGUIConnector CurrentlySelectedConnector
	{
		get
		{
			return _currentlySelectedConnector;
		}
		set
		{
			_currentlySelectedConnector = value;
		}
	}

	public Mission EditorMission { get; set; }

	public List<MissionPack> StockMissionPacks => stockMissionPacks;

	public bool HasUnsavedChanges
	{
		get
		{
			if ((!(lastHistoryStateIDAtLastSave != MissionEditorHistory.HistoryStateId) || !MissionEditorHistory.HasHistory) && !briefingHasChanged)
			{
				if (MissionNameAtLastSave != "")
				{
					return MissionNameAtLastSave != KSPUtil.SanitizeFilename(EditorMission.title);
				}
				return false;
			}
			return true;
		}
	}

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		Instance = this;
		awardDefinitions = UnityEngine.Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/MEAwards.prefab")).GetComponent<Awards>();
		modeMsg = new ScreenMessage("", 5f, ScreenMessageStyle.LOWER_CENTER);
		editorNodeList = new List<MEGUINode>();
		editorConnectorList = new List<MEGUIConnector>();
		vesselCache = new Dictionary<uint, ShipConstruct>();
		LoadBasicNodes();
		GameEvents.onInputLocksModified.Add(OnInputLocksModified);
		GameEvents.Mission.onLocalizationLockOverriden.Add(onLocalizationLockOverriden);
		NodeCanvas = UnityEngine.Object.Instantiate(prefabNodeCanvas).GetComponentInChildren<MENodeCanvas>();
		NodeCanvas.Initialize(this);
	}

	public void Start()
	{
		Time.timeScale = 1f;
		buttonExit.onClick.AddListener(OnExit);
		buttonSave.onClick.AddListener(OnSave);
		buttonLoad.onClick.AddListener(OnLoad);
		buttonNew.onClick.AddListener(OnNew);
		buttonLayout.onClick.AddListener(OnLayout);
		buttonLayoutGAP.onClick.AddListener(OnLayout);
		buttonBrief.onClick.AddListener(OnExport);
		buttonExport.onClick.AddListener(OnExport);
		buttonTest.onClick.AddListener(OnTest);
		buttonKSPedia.onClick.AddListener(OnKSPedia);
		buttonUndo.onClick.AddListener(OnUndo);
		buttonRedo.onClick.AddListener(OnRedo);
		buttonMaximizeCanvas.onValueChanged.AddListener(OnMaximizeCanvas);
		buttonMaximizeGAP.onValueChanged.AddListener(OnMaximizeGAP);
		buttonFit.onClick.AddListener(OnFitInView);
		buttonSnap.onValueChanged.AddListener(OnSnapToGrid);
		buttonArrange.onClick.AddListener(OnArrangeGraph);
		buttonTS.onClick.AddListener(OnTrackingStation);
		titleText.onValueChanged.AddListener(OnTitleChanged);
		titleTooltip.enabled = false;
		if (maximizeCanvasImage == null)
		{
			maximizeCanvasImage = buttonMaximizeCanvas.GetComponent<UIStateImage>();
		}
		if (snapGridImage == null)
		{
			snapGridImage = buttonSnap.GetComponent<UIStateImage>();
		}
		if (maximizeGAPImage == null)
		{
			maximizeGAPImage = buttonMaximizeGAP.GetComponent<UIStateImage>();
		}
		buttonSnap.isOn = GameSettings.MISSION_SNAP_TO_GRID;
		OnSnapToGrid(GameSettings.MISSION_SNAP_TO_GRID);
		buttonLayoutGAP.gameObject.SetActive(value: false);
		validationReport.onClick.AddListener(OnValidationReport);
		validationRun.onClick.AddListener(OnValidationRun);
		Canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
		MissionsUtils.AddMissionsScenarios();
		userMissionPacks = MissionsUtils.GatherMissionPacks(MissionTypes.User);
		if (SteamManager.Initialized)
		{
			userMissionPacks.AddUniqueRange(MissionsUtils.GatherMissionPacks(MissionTypes.Steam));
			userMissionPacks.Sort(MissionPack.CompareDisplayName);
		}
		stockMissionPacks = MissionsUtils.GatherMissionPacks(MissionTypes.Stock);
		GameEvents.Mission.onMissionImported.Add(OnMissionImported);
		if (!string.IsNullOrEmpty(startUpMissionPath))
		{
			MissionToLoadSelected(startUpMissionPath);
			startUpMissionPath = "";
		}
		else
		{
			CreateEmptyMission();
		}
		Planetarium.SetUniversalTime(0.0);
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			FlightGlobals.Bodies[i].inverseRotation = false;
			FlightGlobals.Bodies[i].CBUpdate();
		}
		if (!EditorMission.IsTutorialMission)
		{
			GameEvents.Mission.onMissionBriefingChanged.Add(OnMissionBriefingChanged);
		}
		UnlockAllOptionAtStartup();
		startTime = HighLogic.gameRunTimeAdditive;
		onExitMissionBuilderDialog = null;
	}

	public void UnlockAllOptionAtStartup()
	{
		GameEvents.onKerbalStatusChanged.Add(onKerbalStatusChange);
		GameEvents.onKerbalAddComplete.Add(onKerbalAdded);
		GameEvents.onKerbalRemoved.Add(onKerbalRemoved);
		GameEvents.onKerbalTypeChanged.Add(onKerbalTypeChanged);
		GameEvents.onKerbalNameChanged.Add(onKerbalNameChanged);
		SetParameterSelectionColor();
	}

	public void OnDestroy()
	{
		Instance = null;
		titleText.onValueChanged.RemoveListener(OnTitleChanged);
		GameEvents.Mission.onMissionBriefingChanged.Remove(OnMissionBriefingChanged);
		GameEvents.onKerbalStatusChanged.Remove(onKerbalStatusChange);
		GameEvents.onInputLocksModified.Remove(OnInputLocksModified);
		GameEvents.onKerbalAddComplete.Remove(onKerbalAdded);
		GameEvents.onKerbalRemoved.Remove(onKerbalRemoved);
		GameEvents.onKerbalTypeChanged.Remove(onKerbalTypeChanged);
		GameEvents.onKerbalNameChanged.Remove(onKerbalNameChanged);
	}

	public void LoadBasicNodes()
	{
		basicNodes = new List<MEBasicNode>();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("MEBASICNODE");
		for (int i = 0; i < configNodes.Length; i++)
		{
			MEBasicNode mEBasicNode = new MEBasicNode();
			mEBasicNode.Load(configNodes[i]);
			mEBasicNode.Initialize();
			basicNodes.Add(mEBasicNode);
		}
		GameEvents.Mission.onMissionImported.Remove(OnMissionImported);
	}

	public void OnMissionImported()
	{
		userMissionPacks = MissionsUtils.GatherMissionPacks(MissionTypes.User);
	}

	public void onLocalizationLockOverriden()
	{
		titleText.text = Localizer.Format(EditorMission.title);
		if (Localizer.Tags.ContainsKey(EditorMission.title) && !Localizer.OverrideMELock)
		{
			titleTooltip.enabled = true;
			titleText.interactable = false;
		}
		else
		{
			titleTooltip.enabled = false;
			titleText.interactable = true;
		}
	}

	public void OnTitleChanged(string value)
	{
		EditorMission.title = value;
		if (Localizer.Tags.ContainsKey(value))
		{
			titleText.text = Localizer.Format(value);
			if (!Localizer.OverrideMELock)
			{
				titleTooltip.enabled = true;
				titleText.interactable = false;
			}
		}
		else
		{
			EditorMission.title = value;
		}
	}

	public void OnRedo()
	{
		MissionEditorHistory.Redo();
		NodeCanvas.zoomChanged();
	}

	public void OnUndo()
	{
		MissionEditorHistory.Undo();
		NodeCanvas.zoomChanged();
	}

	public void OnMaximizeCanvas(bool newState)
	{
		if (maximizeCanvasImage != null)
		{
			if (newState)
			{
				maximizeCanvasImage.SetState("Minimize");
			}
			else
			{
				maximizeCanvasImage.SetState("Maximize");
			}
		}
	}

	public void OnMaximizeGAP(bool newState)
	{
		if (maximizeGAPImage != null)
		{
			if (newState)
			{
				maximizeGAPImage.SetState("Minimize");
				buttonLayoutGAP.gameObject.SetActive(value: true);
			}
			else
			{
				maximizeGAPImage.SetState("Maximize");
				buttonLayoutGAP.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnBrief()
	{
		onBriefBriefingDialog = MissionBriefingDialog.Display(EditorMission);
	}

	public void OnMissionBriefingChanged(Mission updatedMission)
	{
		EditorMission = updatedMission;
		EditorMission.isBriefingSet = true;
		titleText.text = EditorMission.title;
		briefingHasChanged = true;
	}

	public void OnSave()
	{
		SaveMission(delegate
		{
		});
	}

	public void SaveMission(Callback afterSave)
	{
		if (EditorMission.IsTutorialMission)
		{
			return;
		}
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_save");
		if (!EditorMission.isBriefingSet)
		{
			if (!GameSettings.MISSION_SHOW_NO_BRIEFING_WARNING)
			{
				onSaveConfirmDialog = MissionBriefingDialog.Display(EditorMission, delegate
				{
					SaveMission(afterSave);
				}, onSaveConfirmDismiss);
				return;
			}
			openingMissionSaveConfirmDialog = UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8100002"), Localizer.Format("#autoLOC_8100001"), "", Localizer.Format("#autoLOC_226975"), Localizer.Format("#autoLOC_360842"), delegate(bool dontShowAgain)
			{
				SaveMissionNoBriefingWarning(dontShowAgain, afterSave);
			}, delegate(bool dontShowAgain)
			{
				SaveMissionNoBriefingWarning(dontShowAgain, afterSave);
			}, showCancelBtn: false);
		}
		else if ((KSPUtil.SanitizeFilename(titleText.text) != MissionNameAtLastSave || EditorMission.MissionInfo.missionType == MissionTypes.Stock) && File.Exists(MissionsUtils.GetUsersMissionsPath(titleText.text)))
		{
			ScreenMessages.PostScreenMessage("", modeMsg);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirm File Overwrite", Localizer.Format("#autoLOC_8100003", titleText.text), Localizer.Format("#autoLOC_127866"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_127867"), delegate
			{
				onSaveConfirmDismiss();
				trytoSave(afterSave, overwrite: true);
			}), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), onSaveConfirmDismiss)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = onSaveConfirmDismiss;
		}
		else
		{
			trytoSave(afterSave);
		}
	}

	public void SaveMissionNoBriefingWarning(bool dontShowAgain, Callback afterSave)
	{
		if (dontShowAgain == GameSettings.MISSION_SHOW_NO_BRIEFING_WARNING)
		{
			GameSettings.MISSION_SHOW_NO_BRIEFING_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
		onBriefBriefingDialog = MissionBriefingDialog.Display(EditorMission, delegate
		{
			SaveMission(afterSave);
		}, onSaveConfirmDismiss);
	}

	public void trytoSave(Callback afterSave, bool overwrite = false)
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_save");
		if (overwrite && Directory.Exists(MissionsUtils.UsersMissionsPath + KSPUtil.SanitizeFilename(EditorMission.title)))
		{
			Directory.Delete(MissionsUtils.UsersMissionsPath + KSPUtil.SanitizeFilename(EditorMission.title), recursive: true);
		}
		bool num = onSaveConfirm();
		Unlock("missionBuilder_save");
		if (num)
		{
			afterSave?.Invoke();
		}
	}

	public bool onSaveConfirm()
	{
		if (string.IsNullOrEmpty(EditorMission.title))
		{
			Debug.LogWarning("[MissionSystem]: No mission name, so we can't save the mission!");
			return false;
		}
		string originalFilename = EditorMission.title;
		if (!GameSettings.MISSION_SHOW_STOCK_PACKS_IN_BRIEFING)
		{
			if (EditorMission.MissionInfo.missionType == MissionTypes.Stock)
			{
				originalFilename = Localizer.Format(EditorMission.title) + "_Copy";
				EditorMission.RegenerateMissionID();
			}
			else if (File.Exists(MissionsUtils.GetStockMissionsPath(EditorMission.title)))
			{
				originalFilename = Localizer.Format(EditorMission.title) + "_Copy";
			}
		}
		originalFilename = KSPUtil.SanitizeFilename(originalFilename);
		if (!string.IsNullOrEmpty(MissionNameAtLastSave) && (originalFilename != MissionNameAtLastSave || EditorMission.MissionInfo.missionType == MissionTypes.Stock))
		{
			if (Directory.Exists(MissionsUtils.UsersMissionsPath + originalFilename))
			{
				Directory.Delete(MissionsUtils.UsersMissionsPath + originalFilename, recursive: true);
			}
			string folderPath = EditorMission.MissionInfo.FolderPath;
			string newValue = MissionsUtils.UsersMissionsPath + originalFilename + "/";
			string[] directories = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				Directory.CreateDirectory(directories[i].Replace(folderPath, newValue));
			}
			string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
			for (int j = 0; j < files.Length; j++)
			{
				File.Copy(files[j], files[j].Replace(folderPath, newValue), overwrite: true);
			}
			EditorMission.steamPublishedFileId = 0uL;
		}
		string text = MissionsUtils.VerifyUsersMissionsFolder(originalFilename);
		string text2 = "persistent";
		string text3 = text + "/" + text2 + ".mission";
		ConfigNode configNode = new ConfigNode();
		ConfigNode node = configNode.AddNode("MISSION");
		CopyMissionBanners(text);
		MEFlowParser.ParseMission(EditorMission);
		EditorMission.Save(node);
		NodeCanvas.Save(configNode);
		configNode.Save(text3);
		try
		{
			new MissionPlayDialog.MissionProfileInfo(text2, text, EditorMission).SaveToMetaFile(text2, text);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[MissionSystem] Unable to save .loadmeta for filename\n" + ex.Message);
		}
		EditorMission.UpdateMissionFileInfo(text3);
		MissionNameAtLastSave = ((!EditorMission.IsTutorialMission) ? EditorMission.MissionInfo.folderName : "");
		lastHistoryStateIDAtLastSave = MissionEditorHistory.HistoryStateId;
		briefingHasChanged = false;
		if (MissionEditorValidator.Mode != 0)
		{
			MissionEditorValidator.RunValidation(EditorMission);
		}
		return true;
	}

	public void CopyMissionBanners(string missionDir)
	{
		MEBannerType[] array = (MEBannerType[])Enum.GetValues(typeof(MEBannerType));
		for (int i = 0; i < array.Length; i++)
		{
			MEBannerEntry banner = EditorMission.GetBanner(array[i]);
			string newPath = missionDir + "/Banners/" + array[i].ToString() + "/" + banner.fileName;
			banner.CopySource(newPath);
		}
	}

	public void onSaveConfirmDismiss()
	{
		Unlock("missionBuilder_save");
		Unlock("missionBuilder_test");
		Unlock("missionBuilder_ts");
		Unlock("missionBuilder_new");
	}

	public void OnExit()
	{
		if (OnExitEditor != null)
		{
			OnExitEditor();
		}
		if (HasUnsavedChanges)
		{
			onExitMissionBuilderDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Leave mission editor", Localizer.Format("#autoLOC_8100008"), Localizer.Format("#autoLOC_8100009"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton((!disallowSave) ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036"), delegate
			{
				if (!disallowSave)
				{
					SaveMission(onExitConfirm);
				}
			}, !disallowSave), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), null), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), onExitConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
		}
		else
		{
			onExitConfirm();
		}
	}

	public void onExitConfirm()
	{
		if (HighLogic.fetch != null)
		{
			if (OnLeave != null)
			{
				OnLeave();
			}
			AnalyticsUtil.LogMissionExitBuilder(EditorMission, HighLogic.gameRunTimeAdditive - startTime);
			if (PSystemSetup.Instance != null)
			{
				PSystemSetup.Instance.RemoveNonStockLaunchSites();
			}
			GameSettings.SaveSettings();
			EditorMission = null;
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
	}

	public void OnNew()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_new");
		if (HasUnsavedChanges)
		{
			ScreenMessages.PostScreenMessage("", modeMsg);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("New Mission", Localizer.Format("#autoLOC_8100013"), Localizer.Format("#autoLOC_8100014"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton((!disallowSave) ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036 "), delegate
			{
				if (!disallowSave)
				{
					SaveMission(OnNewConfirm);
				}
			}, !disallowSave), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), OnNewConfirmDismiss), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), OnNewConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OnNewConfirmDismiss;
		}
		else
		{
			OnNewConfirm();
		}
	}

	public void OnNewConfirm()
	{
		CreateEmptyMission();
		ResetPanels();
		actionPane.InitializeGapMinHeight();
		OnNewConfirmDismiss();
	}

	public void OnNewConfirmDismiss()
	{
		Unlock("missionBuilder_new");
	}

	public void CreateEmptyMission()
	{
		ClearEditor();
		titleTooltip.enabled = false;
		titleText.interactable = true;
		EditorMission = Mission.Spawn();
		titleText.text = Localizer.Format("#autoLOC_8100018");
		EditorMission.title = Localizer.Format("#autoLOC_8100018");
		MENode mENode = MENode.Spawn(EditorMission, Localizer.Format("#autoLOC_8100019"));
		mENode.nodeColor = GetStartNodeColor();
		EditorMission.SetStartNode(mENode);
		mENode.editorPosition = Vector2.zero;
		EditorMission.nodes.Add(mENode.id, mENode);
		for (int i = 0; i < basicNodes.Count; i++)
		{
			if (basicNodes[i].name == "CreateVessel")
			{
				MENode mENode2 = MENode.Spawn(EditorMission, basicNodes[i]);
				EditorMission.nodes.Add(mENode2.id, mENode2);
				mENode.dockedNodes.Add(mENode2);
				break;
			}
		}
		EditorMission.situation.AddParameterToNodeBody("startUT");
		EditorMission.situation.AddParameterToSAP("startUT");
		EditorMission.situation.AddParameterToSAP("partFilter");
		EditorMission.situation.AddParameterToSAP("autoGenerateCrew");
		EditorMission.situation.AddParameterToSAP("gameParameters");
		EditorMission.situation.AddParameterToSAP("resourceSeed");
		if (ResourceScenario.Instance != null)
		{
			ResourceScenario.Instance.gameSettings.GenerateNewSeed();
			EditorMission.situation.resourceSeed = ResourceScenario.Instance.gameSettings.Seed;
			ResourceScenario.Instance.gameSettings.GenerateNewROCMissionSeed();
			EditorMission.situation.rocMissionSeed = ResourceScenario.Instance.gameSettings.ROCMissionSeed;
		}
		EditorMission.UpdateMissionFileInfo(MissionsUtils.UsersMissionsPath + KSPUtil.SanitizeFilename(EditorMission.title) + "/");
		ResetValidator();
		SetupMission(EditorMission);
		NodeCanvas.FocusStartNode();
		validationButtonImage.color = MissionEditorValidator.GetStatusColor();
		MissionNameAtLastSave = "";
		briefingHasChanged = false;
		Unlock("missionBuilder_new");
	}

	public void OnTest()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_test");
		if (MissionNameAtLastSave == "")
		{
			briefingHasChanged = true;
		}
		if (HasUnsavedChanges)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("TestMissionSave", Localizer.Format("#autoLOC_8100020"), Localizer.Format("#autoLOC_8100021"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton((!disallowSave) ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036 "), delegate
			{
				if (!disallowSave)
				{
					SaveMission(OnTestCheckSFSInProgress);
				}
			}, !disallowSave), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				OnTestCancel();
			})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OnTestCancel;
		}
		else
		{
			OnTestCheckSFSInProgress();
		}
	}

	public void OnTestCancel()
	{
		if (MissionNameAtLastSave == "")
		{
			briefingHasChanged = false;
		}
		UnlockTestInputLocks();
	}

	public void OnTestCheckSFSInProgress()
	{
		if (EditorMission.BlockPlayMission(showDialog: true))
		{
			UnlockTestInputLocks();
			RunValidator();
			return;
		}
		if (EditorMission.MissionInfo.HasSave)
		{
			if (Directory.GetFiles(EditorMission.MissionInfo.SaveFolderPath, "checkpoint_*.sfs").Length > 1)
			{
				testMissionDialog.Spawn(CheckpointToLoadSelected, OnCheckpointToLoadCancelled);
				return;
			}
			File.Delete(EditorMission.MissionInfo.SavePath);
			File.Delete(EditorMission.MissionInfo.SaveFolderPath + "persistent.loadmeta");
			if (Directory.GetFiles(EditorMission.MissionInfo.SaveFolderPath, "lastcreatevesseleditor.missionsfs").Length == 1)
			{
				File.Delete(EditorMission.MissionInfo.SaveFolderPath + "lastcreatevesseleditor.missionsfs");
			}
			if (Directory.GetFiles(EditorMission.MissionInfo.SaveFolderPath, "lastcreatevesselspawn.missionsfs").Length == 1)
			{
				File.Delete(EditorMission.MissionInfo.SaveFolderPath + "lastcreatevesselspawn.missionsfs");
			}
			if (Directory.GetFiles(EditorMission.MissionInfo.SaveFolderPath, "startcreatevesselspawn.missionsfs").Length == 1)
			{
				File.Delete(EditorMission.MissionInfo.SaveFolderPath + "startcreatevesselspawn.missionsfs");
			}
		}
		if (GameSettings.MISSION_SHOW_TEST_MISSION_WARNING)
		{
			newTestConfirmDialog = UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_8006020"), Localizer.Format("#autoLOC_8006021"), OnTestConfirm, showCancelBtn: false);
		}
		else
		{
			LaunchNewTest();
		}
	}

	public void UnlockTestInputLocks()
	{
		Unlock("missionBuilder_test");
	}

	public void OnTestConfirm(bool dontShowAgain)
	{
		if (dontShowAgain == GameSettings.MISSION_SHOW_TEST_MISSION_WARNING)
		{
			GameSettings.MISSION_SHOW_TEST_MISSION_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
		LaunchNewTest();
	}

	public void LaunchNewTest()
	{
		UnlockTestInputLocks();
		StartCoroutine(MissionSystem.Instance.SetupMissionGame(EditorMission.MissionInfo, playMission: true, testMode: true, LaunchNewTestMissionSetupSuccess, LaunchNewTestMissionSetupFail));
	}

	public void LaunchNewTestMissionSetupSuccess()
	{
		AnalyticsUtil.LogMissionExitBuilder(EditorMission, HighLogic.gameRunTimeAdditive - startTime);
		MissionToTest = EditorMission.MissionInfo.FilePath;
		HighLogic.CurrentGame.Start();
	}

	public void LaunchNewTestMissionSetupFail()
	{
		Debug.LogError("[MissionsExpansion] Unable to start Mission.");
	}

	public void CheckpointToLoadSelected(string path)
	{
		UnlockTestInputLocks();
		if (string.IsNullOrEmpty(path))
		{
			File.Delete(EditorMission.MissionInfo.SavePath);
			File.Delete(EditorMission.MissionInfo.SaveFolderPath + "persistent.loadmeta");
		}
		StartCoroutine(MissionSystem.Instance.SetupMissionGame(EditorMission.MissionInfo, playMission: true, testMode: true, CheckpointTestMissionSetupSuccess, LaunchNewTestMissionSetupFail, string.IsNullOrEmpty(path) ? path : Path.GetFileNameWithoutExtension(path)));
	}

	public void CheckpointTestMissionSetupSuccess()
	{
		HighLogic.CurrentGame.Start();
	}

	public void OnCheckpointToLoadCancelled()
	{
		UnlockTestInputLocks();
	}

	public void OnKSPedia()
	{
		KSPediaSpawner.Show(null, buttonKSPedia);
	}

	public void OnTrackingStation()
	{
		if (HasUnsavedChanges)
		{
			Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_ts");
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("TSMissionSave", Localizer.Format("#autoLOC_8002001"), Localizer.Format("#autoLOC_8002002"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton((!disallowSave) ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036 "), delegate
			{
				if (!disallowSave)
				{
					SaveMission(OnTSConfirm);
				}
			}, !disallowSave), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				UnlockTSInputLocks();
			})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = UnlockTSInputLocks;
		}
		else
		{
			OnTSConfirm();
		}
	}

	public void UnlockTSInputLocks()
	{
		Unlock("missionBuilder_ts");
	}

	public void OnTSConfirm()
	{
		if (EditorMission.MissionInfo.HasSave)
		{
			File.Delete(EditorMission.MissionInfo.SavePath);
		}
		UnlockTSInputLocks();
		if (EditorMission.MissionInfo != null || EditorMission.MissionInfo.FileInfoObject != null || string.IsNullOrEmpty(MissionNameAtLastSave))
		{
			SaveMission(ContinueToTrackingStation);
		}
	}

	public void ContinueToTrackingStation()
	{
		MissionSystem.RemoveMissionObjects(removeAll: true);
		StartCoroutine(MissionSystem.Instance.SetupMissionGame(EditorMission.MissionInfo, playMission: true, testMode: true, TrackingStationMissionSetupSuccess, LaunchNewTestMissionSetupFail, null, trackingStation: true));
	}

	public void TrackingStationMissionSetupSuccess()
	{
		HighLogic.CurrentGame.Mode = Game.Modes.MISSION_BUILDER;
		SpaceTracking.missionFilePath = EditorMission.MissionInfo.FilePath;
		EditorMission = null;
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		HighLogic.LoadScene(GameScenes.TRACKSTATION);
	}

	public void OnExport()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_export");
		onBriefBriefingDialog = MissionBriefingDialog.Display(EditorMission, UnlockExport, UnlockExport);
	}

	public void UnlockExport()
	{
		Unlock("missionBuilder_export");
	}

	public void RunValidator()
	{
		MissionEditorValidator.RunValidation(EditorMission);
	}

	public void ResetValidator()
	{
		MissionEditorValidator.ResetValidator();
		UpdateValidationLed();
	}

	public static void UpdateValidationLed()
	{
		if (Instance != null)
		{
			Instance.validationButtonImage.color = MissionEditorValidator.GetStatusColor();
		}
	}

	public void OnValidationReport()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: false, "missionBuilder_validation");
		missionValidationDialog = MissionValidationDialog.Display(EditorMission, UnlockValidation, UnlockValidation);
	}

	public void UnlockValidation()
	{
		Unlock("missionBuilder_validation");
	}

	public void OnValidationRun()
	{
		RunValidator();
	}

	public void OnFitInView()
	{
		FitNodesInView();
	}

	public void OnSnapToGrid(bool newState)
	{
		NodeCanvas.SnapToGrid = newState;
		if (snapGridImage != null)
		{
			if (newState)
			{
				snapGridImage.SetState("SnapOn");
			}
			else
			{
				snapGridImage.SetState("SnapOff");
			}
		}
		GameSettings.MISSION_SNAP_TO_GRID = newState;
	}

	public void OnArrangeGraph()
	{
		ArrangeGraphNodes();
	}

	public void FitNodesInView()
	{
		NodeCanvas.FitCameraToArea();
	}

	public void ArrangeGraphNodes()
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryArrangeGraphChange);
		List<MEGUINode> list = new List<MEGUINode>();
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			if (editorNodeList[i].Node.isStartNode)
			{
				list.Insert(0, editorNodeList[i]);
			}
			else if (editorNodeList[i].InputConnectors.Count == 0 && editorNodeList[i].dockedParentNode == null)
			{
				list.Add(editorNodeList[i]);
			}
		}
		List<Guid> placedNodes = new List<Guid>();
		maxGridXoffset = 0f;
		PlaceArrangeNode(list, 0, 0f, ref placedNodes);
		NodeCanvas.CalculateBorders();
		FitNodesInView();
	}

	public void PlaceArrangeNode(List<MEGUINode> nodes, int level, float yOffset, ref List<Guid> placedNodes)
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			if (placedNodes.Contains(nodes[i].Node.id))
			{
				continue;
			}
			if (nodes[i].dockedParentNode == null)
			{
				nodes[i].rectTransform.anchoredPosition = new Vector2(level * 300, yOffset - (float)(i * 150));
				nodes[i].Node.editorPosition = nodes[i].rectTransform.anchoredPosition;
				nodes[i].UpdateConnectors();
				placedNodes.Add(nodes[i].Node.id);
			}
			List<MEGUINode> list = new List<MEGUINode>();
			float num = (float)(nodes[i].OutputConnectors.Count - 1) * 150f;
			int j = 0;
			for (int count2 = nodes[i].OutputConnectors.Count; j < count2; j++)
			{
				if (nodes[i].OutputConnectors[j].toNode != null)
				{
					list.Add(nodes[i].OutputConnectors[j].toNode);
				}
			}
			int k = 0;
			for (int childCount = nodes[i].dockedNodesParentTransform.childCount; k < childCount; k++)
			{
				list.Add(nodes[i].dockedNodesParentTransform.GetChild(k).GetComponent<MEGUINode>());
			}
			num = ((list.Count > 1) ? (num / 2f) : 0f);
			if (list.Count > 0)
			{
				if (nodes[i].dockedParentNode != null)
				{
					num -= nodes[i].dockedParentNode.rectTransform.rect.height - nodes[i].dockedParentNode.dockedNodesParentTransform.rect.height;
				}
				PlaceArrangeNode(list, level + 1, nodes[i].rectTransform.anchoredPosition.y + num, ref placedNodes);
			}
			maxGridXoffset = Mathf.Max(maxGridXoffset, nodes[i].rectTransform.anchoredPosition.x);
		}
	}

	public void OnLayout()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_layout");
		ScreenMessages.PostScreenMessage("", modeMsg);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Reset Mission Panels", Localizer.Format("#autoLOC_8006086"), Localizer.Format("#autoLOC_8006087"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton(Localizer.Format("#autoLOC_900305"), OnLayoutConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_900535"), OnLayoutConfirmDismiss)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OnLayoutConfirmDismiss;
	}

	public void OnLayoutConfirm()
	{
		ResetPanels();
		Unlock("missionBuilder_layout");
	}

	public void ResetPanels()
	{
		int num = Panels.Length;
		while (num-- > 0)
		{
			Panels[num].Show(enableUI: true);
			Panels[num].Reset();
		}
	}

	public void OnLayoutConfirmDismiss()
	{
		Unlock("missionBuilder_layout");
	}

	public void OnLoad()
	{
		Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_load");
		if (HasUnsavedChanges)
		{
			ScreenMessages.PostScreenMessage("", modeMsg);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Save and Continue", Localizer.Format("#autoLOC_8100032"), Localizer.Format("#autoLOC_8100033"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIButton((!disallowSave) ? Localizer.Format("#autoLOC_127975") : Localizer.Format("#autoLOC_6004036 "), delegate
			{
				if (!disallowSave)
				{
					OnLoadConfirmDismiss();
					SaveMission(OnLoadConfirm);
				}
			}, !disallowSave), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), OnLoadConfirmDismiss), new DialogGUIButton(Localizer.Format("#autoLOC_127985"), OnLoadConfirm)), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7")).OnDismiss = OnLoadConfirmDismiss;
		}
		else
		{
			OnLoadConfirm();
		}
	}

	public void OnLoadConfirm()
	{
		onLoadConfirmDialog = loadMissionDialog.Spawn(MissionToLoadSelected, OnMissionToLoadCancelled);
	}

	public void OnLoadConfirmDismiss()
	{
		Unlock("missionBuilder_load");
	}

	public void MissionToLoadSelected(string fullPath)
	{
		missionToLoadFullPath = fullPath;
		missionToLoadMFI = MissionFileInfo.CreateFromPath(fullPath);
		MissionSystem.RemoveMissionObjects();
		ClearEditor();
		missionToLoadRoot = ConfigNode.Load(fullPath, bypassLocalization: true);
		Mission mission = Mission.Spawn(missionToLoadMFI);
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			EditorMission = mission;
		}
		mission.Load(missionToLoadRoot);
		MissionSystem.Instance.ValidateCrewAssignments(mission);
		mission.InitMission();
		CraftProfileInfo.PrepareCraftMetaFileLoad();
		string errorString = string.Empty;
		missionToLoadMFI.IsCraftCompatible(ref errorString, ref incompatibleCraft, checkSaveIfAvailable: false);
		StartCoroutine(mission.GenerateMissionLaunchSites(onMissionLaunchsitesGenerated, createPQSObject: false));
	}

	public void onMissionLaunchsitesGenerated(Mission mission)
	{
		NodeCanvas.Load(missionToLoadRoot);
		if (mission != null)
		{
			ResetValidator();
			mission.UpdateMissionFileInfo(missionToLoadFullPath);
			SetupMission(mission);
			lastHistoryStateIDAtLastSave = MissionEditorHistory.HistoryStateId;
			briefingHasChanged = false;
		}
		Unlock("missionBuilder_load");
		ResetPanels();
		actionPane.InitializeGapMinHeight();
		if (mission != null && mission.IsTutorialMission)
		{
			SetupTutorial(missionToLoadMFI.folderName);
		}
	}

	public void SetupTutorial(string tutorialClass)
	{
		ConfigNode configNode = new ConfigNode("SCENARIO");
		configNode.AddValue("name", tutorialClass);
		configNode.AddValue("scene", "21");
		ScenarioRunner.SetProtoModules(new ProtoScenarioModule(configNode));
	}

	public void OnMissionToLoadCancelled()
	{
		MissionSystem.RemoveMissionObjects();
		Unlock("missionBuilder_load");
	}

	public void SetupMission(Mission mission)
	{
		DictionaryValueList<Guid, MEGUINode> dictionaryValueList = new DictionaryValueList<Guid, MEGUINode>();
		Dictionary<Guid, MENode>.Enumerator dictEnumerator = mission.nodes.GetDictEnumerator();
		while (dictEnumerator.MoveNext())
		{
			MENode value = dictEnumerator.Current.Value;
			MEGUINode val = CreateNodeAtPosition(value.editorPosition, value);
			dictionaryValueList.Add(value.id, val);
		}
		int count = editorNodeList.Count;
		while (count-- > 0)
		{
			foreach (MENode fromNode in editorNodeList[count].Node.fromNodes)
			{
				if (editorNodeList[count].Node.fromNodes.IndexOf(fromNode) < editorNodeList[count].Node.fromNodesConnectionColor.Count)
				{
					ConnectNodes(dictionaryValueList[fromNode.id], editorNodeList[count], editorNodeList[count].Node.fromNodesConnectionColor[editorNodeList[count].Node.fromNodes.IndexOf(fromNode)]);
				}
				else
				{
					ConnectNodes(dictionaryValueList[fromNode.id], editorNodeList[count]);
				}
			}
		}
		int count2 = editorNodeList.Count;
		while (count2-- > 0)
		{
			foreach (MENode dockedNode in editorNodeList[count2].Node.dockedNodes)
			{
				dockedNode.guiNode.DockNode(editorNodeList[count2]);
			}
		}
		if (Localizer.Tags.ContainsKey(mission.title) && !Localizer.OverrideMELock)
		{
			titleTooltip.enabled = !mission.IsTutorialMission;
			titleText.interactable = false;
		}
		EditorMission = mission;
		titleText.text = Localizer.Format(mission.title);
		MissionNameAtLastSave = ((!mission.IsTutorialMission) ? mission.MissionInfo.folderName : "");
		EditorMission = mission;
		MissionEditorHistory.Clear();
		lastHistoryStateIDAtLastSave = Guid.NewGuid();
		briefingHasChanged = false;
		GameEvents.Mission.onMissionLoaded.Fire();
	}

	public void ClearEditor()
	{
		ClearAllSelections();
		if (EditorMission != null)
		{
			EditorMission.gameObject.DestroyGameObject();
		}
		CurrentlySelectedConnector = null;
		CurrentSelectedNode = null;
		copiedGUINodes.Clear();
		if (editorNodeList != null && editorNodeList.Count > 0)
		{
			int count = editorNodeList.Count;
			while (count-- > 0)
			{
				editorNodeList[count].CleanUp();
			}
			editorNodeList.Clear();
		}
		if (editorConnectorList != null && editorConnectorList.Count > 0)
		{
			int count2 = editorConnectorList.Count;
			while (count2-- > 0)
			{
				editorConnectorList[count2].CleanUp();
			}
			editorConnectorList.Clear();
		}
		actionPane.ClearCache();
		MENodeCategorizer.Instance.ClearSearchField();
		MENodeCategorizer.Instance.ResetStoredIcons();
		foreach (ShipConstruct value in vesselCache.Values)
		{
			if (value.parts != null && value.parts[0] != null)
			{
				value.parts[0].gameObject.DestroyGameObject();
			}
		}
		vesselCache.Clear();
		FlightGlobals.ClearpersistentIdDictionaries();
		if (PSystemSetup.Instance != null)
		{
			PSystemSetup.Instance.RemoveNonStockLaunchSites();
		}
		incompatibleCraft.Clear();
	}

	public void ConnectorButtonClicked(MEGUINode node, MENodeConnectionType type)
	{
		if (CurrentConnector == null)
		{
			CurrentConnector = UnityEngine.Object.Instantiate(meConnectorPrefab);
			CurrentConnector.SetUpConnector(Canvas, NodeCanvas.NodeRoot, this);
			CurrentConnector.AddNewEnd(node, type);
			node.AddNewLine(CurrentConnector, type);
			editorConnectorList.Add(CurrentConnector);
		}
		else if (!CurrentConnector.IsConnectedToNode(node) && CurrentConnector.IsNewConnectionValid(type))
		{
			if (CurrentConnector.GetConnectedNode().DoesConnectionExist(node, type))
			{
				DestroyCurrentConnector();
				return;
			}
			CurrentConnector.AddNewEnd(node, type);
			node.AddNewLine(CurrentConnector, type);
			CurrentConnector.ConnectLogic();
			CurrentConnector = null;
		}
	}

	public void ConnectorButtonDragged(MEGUINode node, MENodeConnectionType type)
	{
		if (CurrentConnector == null)
		{
			CurrentConnector = UnityEngine.Object.Instantiate(meConnectorPrefab);
			CurrentConnector.SetUpConnector(Canvas, NodeCanvas.NodeRoot, this);
			CurrentConnector.AddNewEnd(node, type);
			node.AddNewLine(CurrentConnector, type);
			editorConnectorList.Add(CurrentConnector);
		}
	}

	public void ConnectorButtonDropped(MEGUINode node, MENodeConnectionType type)
	{
		if (!(CurrentConnector == null) && !CurrentConnector.IsConnectedToNode(node) && CurrentConnector.IsNewConnectionValid(type))
		{
			if (CurrentConnector.GetConnectedNode().DoesConnectionExist(node, type))
			{
				DestroyCurrentConnector();
				return;
			}
			CurrentConnector.AddNewEnd(node, type);
			node.AddNewLine(CurrentConnector, type);
			CurrentConnector.ConnectLogic();
			CurrentConnector = null;
		}
	}

	public MEGUIConnector ConnectNodes(MEGUINode fromNode, MEGUINode toNode)
	{
		MEGUIConnector mEGUIConnector = UnityEngine.Object.Instantiate(meConnectorPrefab);
		mEGUIConnector.SetUpConnector(Canvas, NodeCanvas.NodeRoot, this);
		mEGUIConnector.AddNewEnd(toNode, MENodeConnectionType.Input);
		toNode.AddNewLine(mEGUIConnector, MENodeConnectionType.Input);
		mEGUIConnector.AddNewEnd(fromNode, MENodeConnectionType.Output);
		fromNode.AddNewLine(mEGUIConnector, MENodeConnectionType.Output);
		editorConnectorList.Add(mEGUIConnector);
		return mEGUIConnector;
	}

	public MEGUIConnector ConnectNodes(MEGUINode fromNode, MEGUINode toNode, Color connectionColor)
	{
		MEGUIConnector mEGUIConnector = UnityEngine.Object.Instantiate(meConnectorPrefab);
		mEGUIConnector.SetUpConnector(Canvas, NodeCanvas.NodeRoot, this);
		mEGUIConnector.LineColour = connectionColor;
		mEGUIConnector.AddNewEnd(toNode, MENodeConnectionType.Input);
		toNode.AddNewLine(mEGUIConnector, MENodeConnectionType.Input);
		mEGUIConnector.AddNewEnd(fromNode, MENodeConnectionType.Output);
		fromNode.AddNewLine(mEGUIConnector, MENodeConnectionType.Output);
		editorConnectorList.Add(mEGUIConnector);
		return mEGUIConnector;
	}

	public void DestroyCurrentConnector()
	{
		if (!(CurrentConnector == null))
		{
			CurrentConnector.Destroy();
			CurrentConnector = null;
		}
	}

	public void AddConnectorToConnectorList(MEGUIConnector connector)
	{
		editorConnectorList.Add(connector);
	}

	public void ClearConnector(MEGUIConnector connector)
	{
		if (CurrentlySelectedConnector != null && CurrentlySelectedConnector.Equals(connector))
		{
			CurrentlySelectedConnector = null;
		}
		if (CurrentConnector != null && CurrentConnector.Equals(connector))
		{
			CurrentConnector = null;
		}
		if (selectedConnectors.Contains(connector))
		{
			selectedConnectors.Remove(connector);
		}
		editorConnectorList.Remove(connector);
	}

	public MEGUINode CreateNodeAtPosition(Vector2 position)
	{
		MENode node = MENode.Spawn(EditorMission);
		return CreateNodeAtPosition(position, node);
	}

	public MEGUINode CreateNodeAtPosition(Vector2 position, MEBasicNode basicNode)
	{
		MENode node = MENode.Spawn(EditorMission, basicNode);
		return CreateNodeAtPosition(position, node);
	}

	public MEGUINode CreateNodeAtPosition(Vector2 position, MENode node)
	{
		MEGUINode mEGUINode = UnityEngine.Object.Instantiate(MENodePrefab);
		mEGUINode.rectTransform.SetParent(NodeCanvas.NodeRoot.transform, worldPositionStays: false);
		mEGUINode.rectTransform.anchoredPosition = position;
		mEGUINode.rectTransform.localScale = Vector3.one;
		mEGUINode.SetNode(node);
		AddNode(mEGUINode);
		return mEGUINode;
	}

	public void CopySelectedNodes()
	{
		if (copiedGUINodes.Count >= 0)
		{
			copiedGUINodes.Clear();
		}
		for (int i = 0; i < selectedNodes.Count; i++)
		{
			if (selectedNodes[i].Node != EditorMission.startNode)
			{
				copiedGUINodes.Add(selectedNodes[i]);
			}
		}
	}

	public void PasteCopiedNodes()
	{
		if (copiedGUINodes.Count <= 0)
		{
			return;
		}
		ClearSelectedNodesList();
		for (int i = 0; i < copiedGUINodes.Count; i++)
		{
			if (copiedGUINodes[i].dockedParentNode == null)
			{
				cachePositionForPastedNodes = new Vector2(copiedGUINodes[i].rectTransform.anchoredPosition.x + pastedNodeOffset.x, copiedGUINodes[i].rectTransform.anchoredPosition.y + pastedNodeOffset.y);
			}
			else
			{
				cachePositionForPastedNodes = new Vector2(copiedGUINodes[i].dockedParentNode.rectTransform.anchoredPosition.x + pastedNodeOffset.x, copiedGUINodes[i].dockedParentNode.rectTransform.anchoredPosition.y + pastedNodeOffset.y);
				for (int j = 0; j < copiedGUINodes[i].transform.parent.childCount; j++)
				{
					cachePositionForPastedNodes = new Vector2(cachePositionForPastedNodes.x, cachePositionForPastedNodes.y - copiedGUINodes[i].transform.parent.GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta.y);
					if (j == copiedGUINodes[i].transform.GetSiblingIndex())
					{
						break;
					}
				}
			}
			MENode node = MENode.Clone(copiedGUINodes[i].Node, cachePositionForPastedNodes);
			MEGUINode mEGUINode = CreateNodeAtPosition(cachePositionForPastedNodes, node);
			if (mEGUINode.dockedParentNode != null)
			{
				mEGUINode.UndockNode();
			}
			List<ActionCreateVessel> allActionModules = mEGUINode.Node.GetAllActionModules<ActionCreateVessel>();
			for (int k = 0; k < allActionModules.Count; k++)
			{
				allActionModules[k].vesselSituation.vesselCrew.Clear();
				allActionModules[k].UpdateNodeBodyUI();
			}
			mEGUINode.Select(deselectOtherNodes: false, copiedGUINodes.Count > 1);
			mEGUINode.PushUndoActionOnPasteNode();
		}
	}

	public MEGUINode CreateNodeAtMousePosition()
	{
		return CreateNodeAtPosition(GetGridMousePosition());
	}

	public MEGUINode CreateNodeAtMousePosition(MEBasicNode basicNode)
	{
		return CreateNodeAtPosition(GetGridMousePosition(), basicNode);
	}

	public Vector3 GetGridMousePosition()
	{
		return NodeCanvas.GetMousePointOnGrid();
	}

	public void CanvasClicked(Vector2 clickPosition, bool dragging)
	{
		if (dragging || CurrentConnector != null)
		{
			return;
		}
		MEGUIConnector connector = null;
		for (int i = 0; i < editorConnectorList.Count; i++)
		{
			if (editorConnectorList[i].WasMouseClickWithinTolerance(clickPosition, connectorSelectionAllowance))
			{
				connector = editorConnectorList[i];
				break;
			}
		}
		ConnectorSelectionChange(connector);
	}

	public void NodeSelectionChange(MEGUINode node)
	{
		CurrentSelectedNode = node;
		GameEvents.Mission.onBuilderNodeSelectionChanged.Fire((node == null) ? null : node.Node);
	}

	public void AddNodeToSelectedList(MEGUINode node)
	{
		selectedNodes.Add(node);
		if (selectedNodes.Count > 1)
		{
			DisplayMultipleSelectedItems(selectedNodes.Count, Localizer.Format("#autoLOC_8006024"));
		}
	}

	public void RemoveNodeFromSelectedList(MEGUINode node)
	{
		selectedNodes.Remove(node);
		if (selectedNodes.Count > 1)
		{
			DisplayMultipleSelectedItems(selectedNodes.Count, Localizer.Format("#autoLOC_8006024"));
		}
		else if (selectedNodes.Count == 1)
		{
			NodeSelectionChange(selectedNodes[0]);
		}
	}

	public void ClearSelectedNodesList()
	{
		if (selectedNodes.Count > 0)
		{
			nodeGroupIndex = selectedNodes.Count;
			for (int i = 0; i < nodeGroupIndex; i++)
			{
				selectedNodes[0].Deselect();
			}
		}
	}

	public void UpdateSelectedNodesPosition(MEGUINode node, Vector2 pos)
	{
		for (int i = 0; i < selectedNodes.Count; i++)
		{
			if (selectedNodes[i] != node && selectedNodes[i].dockedParentNode == null)
			{
				selectedNodes[i].rectTransform.anchoredPosition += new Vector2(0f - pos.x, 0f - pos.y);
				selectedNodes[i].UpdateConnectors();
			}
		}
	}

	public void DisplayMultipleSelectedItems(int amount, string itemName)
	{
		actionPane.SAPDisplayMultipleSelectedItemsMessage(amount, itemName);
	}

	public void AddConnectorToSelectedList()
	{
		selectedConnectors.Add(CurrentlySelectedConnector);
		CurrentlySelectedConnector.Select();
		if (selectedConnectors.Count > 1 && selectedConnectors.Count > 1)
		{
			DisplayMultipleSelectedItems(selectedConnectors.Count, Localizer.Format("#autoLOC_8006023"));
		}
	}

	public void RemoveConnectorFromSelectedList()
	{
		selectedConnectors.Remove(CurrentlySelectedConnector);
		CurrentlySelectedConnector.Deselect();
		if (selectedConnectors.Count > 1)
		{
			if (selectedConnectors.Count > 1)
			{
				DisplayMultipleSelectedItems(selectedConnectors.Count, Localizer.Format("#autoLOC_8006023"));
			}
		}
		else
		{
			actionPane.Clean();
		}
	}

	public void ClearConnectorGroupSelection()
	{
		if (selectedConnectors.Count > 0)
		{
			connectorGroupIndex = selectedConnectors.Count;
			for (int i = 0; i < connectorGroupIndex; i++)
			{
				selectedConnectors[i].Deselect();
			}
			selectedConnectors.Clear();
		}
	}

	public void ClearAllSelections()
	{
		ClearConnectorGroupSelection();
		ClearSelectedNodesList();
		NodeSelectionChange(null);
	}

	public bool IsNodeDockable(MEGUINode testNode, ref MEGUINode nodeToDockTo, Vector3 nodePositionCheckOverride)
	{
		if (testNode != null)
		{
			for (int i = 0; i < editorNodeList.Count; i++)
			{
				if (!(testNode != editorNodeList[i]) || !(editorNodeList[i].dockedParentNode == null))
				{
					continue;
				}
				Vector2 anchoredPosition = editorNodeList[i].rectTransform.anchoredPosition;
				anchoredPosition.y += editorNodeList[i].dockedNodesParentTransform.offsetMax.y;
				if (editorNodeList[i].Node.dockedNodes.Count > 0)
				{
					for (int j = 0; j < editorNodeList[i].Node.dockedNodes.Count; j++)
					{
						if (editorNodeList[i].Node.dockedNodes[j].guiNode != testNode)
						{
							anchoredPosition.y -= editorNodeList[i].Node.dockedNodes[j].guiNode.rectTransform.rect.height;
						}
					}
				}
				Vector2 vector = testNode.rectTransform.anchoredPosition;
				if (nodePositionCheckOverride != Vector3.zero)
				{
					vector = nodePositionCheckOverride;
				}
				else if (testNode.dockedParentNode != null)
				{
					vector.x -= testNode.rectTransform.rect.width / 2f;
					vector += testNode.dockedParentNode.rectTransform.anchoredPosition;
					vector.y += testNode.dockedParentNode.dockedNodesParentTransform.offsetMax.y;
				}
				if (!((vector - anchoredPosition).SqrMagnitude() >= distanceForNodeDocking * distanceForNodeDocking))
				{
					nodeToDockTo = editorNodeList[i];
					return true;
				}
			}
		}
		return false;
	}

	public void ConnectorSelectionChange(MEGUIConnector connector)
	{
		ClearSelectedNodesList();
		_ = CurrentlySelectedConnector != connector;
		CurrentlySelectedConnector = connector;
		if (CurrentlySelectedConnector != null)
		{
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				ClearConnectorGroupSelection();
				AddConnectorToSelectedList();
			}
			else if (CurrentlySelectedConnector.isSelected)
			{
				RemoveConnectorFromSelectedList();
			}
			else
			{
				AddConnectorToSelectedList();
			}
			if (selectedConnectors.Count == 1 && _currentlySelectedConnector != null)
			{
				actionPane.SAPDisplayConnectorParameters(_currentlySelectedConnector);
			}
		}
		else
		{
			ClearConnectorGroupSelection();
			actionPane.Clean();
		}
	}

	public void Update()
	{
		InputUpdate();
		if (CurrentConnector != null)
		{
			CurrentConnector.UpdateLine();
		}
	}

	public void LockLocalizedTitle()
	{
		if (Localizer.Tags.ContainsKey(EditorMission.title) && !Localizer.OverrideMELock)
		{
			titleTooltip.enabled = true;
			titleText.interactable = false;
		}
		else
		{
			titleTooltip.enabled = false;
			titleText.interactable = true;
		}
	}

	public void InputUpdate()
	{
		SetCurrentSelectedGameObject();
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH) && (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && !PreventCanvasObjectChanges())
		{
			if (CurrentConnector != null)
			{
				DestroyCurrentConnector();
			}
			else
			{
				if (!PreventNodeDestruction && selectedNodes.Count > 0)
				{
					for (int i = 0; i < selectedNodes.Count; i++)
					{
						if (selectedNodes[i].Node != EditorMission.startNode)
						{
							actionPane.CheckDeletedNodeFromGap(selectedNodes[i].Node);
							selectedNodes[i].Destroy();
						}
					}
				}
				if (selectedConnectors.Count > 0)
				{
					for (int num = selectedConnectors.Count - 1; num >= 0; num--)
					{
						selectedConnectors[num].Destroy();
					}
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (CurrentConnector != null)
			{
				DestroyCurrentConnector();
			}
			else if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_EXIT))
			{
				if (buttonMaximizeGAP.isOn)
				{
					buttonMaximizeGAP.isOn = false;
				}
				else
				{
					CheckDialogsBeforeExiting();
				}
			}
		}
		bool num2;
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_UNDO_REDO) && !PreventCanvasObjectChanges())
		{
			if (!Application.isEditor)
			{
				if (Application.platform == RuntimePlatform.OSXPlayer)
				{
					if (Input.GetKey(KeyCode.LeftCommand))
					{
						num2 = Input.GetKey(KeyCode.Z);
						goto IL_01b8;
					}
				}
				else if (Input.GetKey(KeyCode.LeftControl))
				{
					num2 = Input.GetKeyDown(KeyCode.Z);
					goto IL_01b8;
				}
				goto IL_01c0;
			}
			num2 = Input.GetKeyDown(KeyCode.Z);
			goto IL_01b8;
		}
		goto IL_020e;
		IL_0206:
		bool num3;
		if (num3 != 0)
		{
			OnRedo();
		}
		goto IL_020e;
		IL_01c0:
		if (!Application.isEditor)
		{
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				if (Input.GetKey(KeyCode.LeftCommand))
				{
					num3 = Input.GetKey(KeyCode.Y);
					goto IL_0206;
				}
			}
			else if (Input.GetKey(KeyCode.LeftControl))
			{
				num3 = Input.GetKeyDown(KeyCode.Y);
				goto IL_0206;
			}
			goto IL_020e;
		}
		num3 = Input.GetKeyDown(KeyCode.Y);
		goto IL_0206;
		IL_01b8:
		if (num2)
		{
			OnUndo();
		}
		goto IL_01c0;
		IL_020e:
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			searchInput.Select();
			searchInput.ActivateInputField();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				TabInputField(direction: false);
			}
			else
			{
				TabInputField(direction: true);
			}
		}
		if (!InputLockManager.IsUnlocked(ControlTypes.EDITOR_MODE_SWITCH) || (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)))
		{
			return;
		}
		if (EditorMission.IsTutorialMission)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003182"));
			return;
		}
		if (Input.GetKeyDown(KeyCode.C) && EventSystem.current != null && (EventSystem.current.currentSelectedGameObject == null || (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() == null)))
		{
			CopySelectedNodes();
		}
		if (Input.GetKeyDown(KeyCode.V) && EventSystem.current != null && (EventSystem.current.currentSelectedGameObject == null || (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() == null)))
		{
			PasteCopiedNodes();
		}
	}

	public void CheckDialogsBeforeExiting()
	{
		if (!onBriefBriefingDialog && !onSaveConfirmDialog && !openingMissionSaveConfirmDialog && !onLoadConfirmDialog && !unableToStartMission && !dialogSelectEditor && !missionValidationDialog)
		{
			if (!PopupDialog.CheckForOpenDialogs() && !onExitMissionBuilderDialog && !newTestConfirmDialog)
			{
				OnExit();
			}
		}
		else
		{
			onSaveConfirmDismiss();
		}
	}

	public void TabInputField(bool direction)
	{
		tabNextInputfield = actionPane.GetNextInputTabStop(direction);
		if (!(tabNextInputfield == null))
		{
			if (!tabNextInputfield.enabled)
			{
				tabNextInputfield.enabled = true;
			}
			tabNextInputfield.Select();
		}
	}

	public bool PreventCanvasObjectChanges()
	{
		if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponentInParent<MEActionPane>() != null)
		{
			return true;
		}
		if (EventSystem.current.currentSelectedGameObject == titleText.gameObject)
		{
			return true;
		}
		return false;
	}

	public void ScrollSettingToBottom()
	{
		ScrollBar(0f);
	}

	public void ScrollSettingToTop()
	{
		ScrollBar(1f);
	}

	public void ScrollBar(float value)
	{
		if (!(CurrentSelectedNode == null) && !(actionPane == null))
		{
			actionPane.ScrollBar(value);
		}
	}

	public void ScrollToParameterGroup(int index)
	{
		MEGUIParameterGroup[] parentParameterGroup = GetParentParameterGroup();
		if (parentParameterGroup == null || index >= parentParameterGroup.Length)
		{
			return;
		}
		if (index == 0)
		{
			ScrollSettingToTop();
			return;
		}
		float num = 0f;
		for (int i = 1; i < parentParameterGroup.Length; i++)
		{
			RectTransform rectTransform = parentParameterGroup[i - 1].transform as RectTransform;
			num += rectTransform.rect.height;
			if (i == index)
			{
				break;
			}
		}
		ScrollPanel(num);
	}

	public MEGUIParameterGroup[] GetParentParameterGroup()
	{
		Transform contentRoot = actionPane.SAPPanel.ContentRoot;
		int childCount = contentRoot.childCount;
		List<MEGUIParameterGroup> list = new List<MEGUIParameterGroup>();
		for (int i = 0; i < childCount; i++)
		{
			Transform child = contentRoot.GetChild(i);
			list.Add(child.GetComponent<MEGUIParameterGroup>());
		}
		return list.ToArray();
	}

	public void ScrollPanel(float height)
	{
		if (!(CurrentSelectedNode == null) && !(actionPane == null))
		{
			ScrollSettingToTop();
			actionPane.ScrollPanel(height);
		}
	}

	public void SetCurrentSelectedGameObject()
	{
		GameObject pointerPress = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleCustom>().GetLastPointerEventDataPublic(-1).pointerPress;
		if (!(currentSelectedGameObject == pointerPress))
		{
			currentSelectedGameObject = pointerPress;
			if (pointerPress != null && OnSelectedGameObjectChange != null)
			{
				OnSelectedGameObjectChange(pointerPress);
			}
		}
	}

	public void AddNode(MEGUINode node)
	{
		if (!EditorMission.nodes.ContainsKey(node.Node.id))
		{
			EditorMission.nodes.Add(node.Node.id, node.Node);
		}
		if (!editorNodeList.Contains(node))
		{
			editorNodeList.Add(node);
		}
		else
		{
			Debug.Log("[MissionEditorLogic.AddNode] Node " + node.Node.Title + " was not added to editor node list because it already existed!");
		}
		GameEvents.Mission.onBuilderNodeAdded.Fire(node.Node);
	}

	public void RemoveNode(MEGUINode node)
	{
		if (node.Node.IsLaunchPadNode)
		{
			ActionCreateLaunchSite actionCreateLaunchSite = node.Node.actionModules[0] as ActionCreateLaunchSite;
			if (actionCreateLaunchSite != null)
			{
				actionCreateLaunchSite.launchSiteSituation.RemoveLaunchSite();
			}
		}
		bool flag = false;
		for (int i = 0; i < node.Node.actionModules.Count; i++)
		{
			if (node.Node.actionModules[i] as ActionCreateVessel != null)
			{
				flag = true;
			}
			if (node.Node.actionModules[i] is IMissionKerbal missionKerbal)
			{
				missionKerbal.NodeDeleted();
			}
		}
		for (int j = 0; j < node.Node.testGroups.Count; j++)
		{
			for (int k = 0; k < node.Node.testGroups[j].testModules.Count; k++)
			{
				if (node.Node.testGroups[j].testModules[k] is IMissionKerbal)
				{
					(node.Node.testGroups[j].testModules[k] as IMissionKerbal).NodeDeleted();
				}
			}
		}
		if (node.Node.IsDocked && node.Node.dockParentNode.dockedNodes.Contains(node.Node))
		{
			node.Node.dockParentNode.dockedNodes.Remove(node.Node);
		}
		editorNodeList.Remove(node);
		EditorMission.nodes.Remove(node.Node.id);
		if (CurrentSelectedNode != null && CurrentSelectedNode.Equals(node))
		{
			CurrentSelectedNode = null;
		}
		if (flag)
		{
			GameEvents.Mission.onVesselSituationChanged.Fire();
		}
		GameEvents.Mission.onBuilderNodeDeleted.Fire(node.Node);
		MENodeCanvas.Instance.CalculateBorders();
	}

	public MEGUINode GetNodeFromID(int nodeID)
	{
		if (nodeID == 0)
		{
			return null;
		}
		int count = editorNodeList.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (editorNodeList[count].GetInstanceID() != nodeID);
		return editorNodeList[count];
	}

	public float GetToolboxWidth()
	{
		return toolBox.rect.width;
	}

	public ShipConstruct LoadVessel(MissionCraft vessel)
	{
		return LoadVessel(vessel.CraftNode, vessel.persistentId);
	}

	public ShipConstruct LoadVessel(ConfigNode craftNode, uint persistentID)
	{
		ShipConstruct shipConstruct = null;
		if (vesselCache.ContainsKey(persistentID))
		{
			shipConstruct = vesselCache[persistentID];
		}
		if (shipConstruct == null || shipConstruct.Parts[0] == null)
		{
			if (craftNode == null)
			{
				return shipConstruct;
			}
			shipConstruct = new ShipConstruct();
			if (!shipConstruct.LoadShip(craftNode, persistentID))
			{
				return null;
			}
			if (string.IsNullOrEmpty(shipConstruct.missionFlag))
			{
				shipConstruct.missionFlag = EditorMission.flagURL;
			}
			for (int i = 0; i < shipConstruct.parts.Count; i++)
			{
				shipConstruct.parts[i].flagURL = shipConstruct.missionFlag;
				for (int j = 0; j < shipConstruct.parts[i].Modules.Count; j++)
				{
					if (shipConstruct.parts[i].Modules[j] is FlagDecal)
					{
						FlagDecal flagDecal = shipConstruct.parts[i].Modules[j] as FlagDecal;
						if (flagDecal != null)
						{
							flagDecal.UpdateFlagTexture();
						}
					}
				}
			}
			vesselCache[persistentID] = shipConstruct;
		}
		return shipConstruct;
	}

	public void onKerbalStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			for (int j = 0; j < editorNodeList[i].Node.actionModules.Count; j++)
			{
				if (editorNodeList[i].Node.actionModules[j] is IMissionKerbal)
				{
					(editorNodeList[i].Node.actionModules[j] as IMissionKerbal).KerbalRosterStatusChange(kerbal, oldStatus, newStatus);
				}
			}
			for (int k = 0; k < editorNodeList[i].Node.testGroups.Count; k++)
			{
				for (int l = 0; l < editorNodeList[i].Node.testGroups[k].testModules.Count; l++)
				{
					if (editorNodeList[i].Node.testGroups[k].testModules[l] is IMissionKerbal)
					{
						(editorNodeList[i].Node.testGroups[k].testModules[l] as IMissionKerbal).KerbalRosterStatusChange(kerbal, oldStatus, newStatus);
					}
				}
			}
		}
	}

	public void onKerbalAdded(ProtoCrewMember kerbal)
	{
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			for (int j = 0; j < editorNodeList[i].Node.actionModules.Count; j++)
			{
				if (editorNodeList[i].Node.actionModules[j] is IMissionKerbal)
				{
					(editorNodeList[i].Node.actionModules[j] as IMissionKerbal).KerbalAdded(kerbal);
				}
			}
			for (int k = 0; k < editorNodeList[i].Node.testGroups.Count; k++)
			{
				for (int l = 0; l < editorNodeList[i].Node.testGroups[k].testModules.Count; l++)
				{
					if (editorNodeList[i].Node.testGroups[k].testModules[l] is IMissionKerbal)
					{
						(editorNodeList[i].Node.testGroups[k].testModules[l] as IMissionKerbal).KerbalAdded(kerbal);
					}
				}
			}
		}
	}

	public void onKerbalTypeChanged(ProtoCrewMember kerbal, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			for (int j = 0; j < editorNodeList[i].Node.actionModules.Count; j++)
			{
				if (editorNodeList[i].Node.actionModules[j] is IMissionKerbal)
				{
					(editorNodeList[i].Node.actionModules[j] as IMissionKerbal).KerbalTypeChange(kerbal, oldType, newType);
				}
			}
			for (int k = 0; k < editorNodeList[i].Node.testGroups.Count; k++)
			{
				for (int l = 0; l < editorNodeList[i].Node.testGroups[k].testModules.Count; l++)
				{
					if (editorNodeList[i].Node.testGroups[k].testModules[l] is IMissionKerbal)
					{
						(editorNodeList[i].Node.testGroups[k].testModules[l] as IMissionKerbal).KerbalTypeChange(kerbal, oldType, newType);
					}
				}
			}
		}
	}

	public void onKerbalNameChanged(ProtoCrewMember kerbal, string oldName, string newName)
	{
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			for (int j = 0; j < editorNodeList[i].Node.actionModules.Count; j++)
			{
				if (editorNodeList[i].Node.actionModules[j] is IMissionKerbal)
				{
					(editorNodeList[i].Node.actionModules[j] as IMissionKerbal).KerbalNameChange(kerbal, oldName, newName);
				}
			}
			for (int k = 0; k < editorNodeList[i].Node.testGroups.Count; k++)
			{
				for (int l = 0; l < editorNodeList[i].Node.testGroups[k].testModules.Count; l++)
				{
					if (editorNodeList[i].Node.testGroups[k].testModules[l] is IMissionKerbal)
					{
						(editorNodeList[i].Node.testGroups[k].testModules[l] as IMissionKerbal).KerbalNameChange(kerbal, oldName, newName);
					}
				}
			}
		}
	}

	public void onKerbalRemoved(ProtoCrewMember kerbal)
	{
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			for (int j = 0; j < editorNodeList[i].Node.actionModules.Count; j++)
			{
				if (editorNodeList[i].Node.actionModules[j] is IMissionKerbal)
				{
					(editorNodeList[i].Node.actionModules[j] as IMissionKerbal).KerbalRemoved(kerbal);
				}
			}
			for (int k = 0; k < editorNodeList[i].Node.testGroups.Count; k++)
			{
				for (int l = 0; l < editorNodeList[i].Node.testGroups[k].testModules.Count; l++)
				{
					if (editorNodeList[i].Node.testGroups[k].testModules[l] is IMissionKerbal)
					{
						(editorNodeList[i].Node.testGroups[k].testModules[l] as IMissionKerbal).KerbalRemoved(kerbal);
					}
				}
			}
		}
	}

	public static void StartUpMissionEditor(string missionPath = "")
	{
		MissionsUtils.OpenMissionBuilder();
		startUpMissionPath = missionPath;
	}

	public static Color GetStartNodeColor()
	{
		if (Instance != null && Instance.MENodeColorConfig != null)
		{
			return Instance.MENodeColorConfig.startNodeColor;
		}
		Debug.LogWarning("[MissionEditorLogic] Not instantiated - returning gray");
		return Color.gray;
	}

	public static Color GetCategoryColor(string categoryName)
	{
		if (Instance != null && Instance.MENodeColorConfig != null)
		{
			int num = 0;
			while (true)
			{
				if (num < Instance.MENodeColorConfig.categoryColors.Count)
				{
					if (Instance.MENodeColorConfig.categoryColors[num].name == categoryName)
					{
						break;
					}
					num++;
					continue;
				}
				return Instance.MENodeColorConfig.categoryDefaultColor;
			}
			return Instance.MENodeColorConfig.categoryColors[num].headerColor;
		}
		Debug.LogWarning("[MissionEditorLogic] Not instantiated - returning gray");
		return Color.gray;
	}

	public void OnHistorySelectionChange(ConfigNode data, HistoryType type)
	{
		string value = "none";
		if (data.TryGetValue("selectionType", ref value))
		{
			if (value.Equals("node"))
			{
				OnHistoryNodeSelectionChange(data, type);
				return;
			}
			if (value.Equals("connector"))
			{
				OnHistoryConnectorSelectionChange(data, type);
				return;
			}
			CurrentSelectedNode = null;
			CurrentlySelectedConnector = null;
		}
	}

	public void OnHistoryNodeSelectionChange(ConfigNode data, HistoryType type)
	{
		int value = 0;
		if (!data.TryGetValue("currentNode", ref value))
		{
			return;
		}
		if (value != 0)
		{
			int count = editorNodeList.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (editorNodeList[count].GetInstanceID() != value);
			CurrentSelectedNode = editorNodeList[count];
		}
		else
		{
			CurrentSelectedNode = null;
		}
	}

	public void OnHistoryConnectorSelectionChange(ConfigNode data, HistoryType type)
	{
		int value = 0;
		if (!data.TryGetValue("currentConnector", ref value))
		{
			return;
		}
		if (value != 0)
		{
			int count = editorConnectorList.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (editorConnectorList[count].GetHashCode() != value);
			CurrentlySelectedConnector = editorConnectorList[count];
		}
		else
		{
			CurrentlySelectedConnector = null;
		}
	}

	public void OnHistoryArrangeGraphChange(ConfigNode data, HistoryType type)
	{
		ConfigNode node = new ConfigNode();
		if (!data.TryGetNode("NODES", ref node))
		{
			return;
		}
		ConfigNode[] nodes = node.GetNodes("NODE");
		foreach (ConfigNode configNode in nodes)
		{
			Guid value = default(Guid);
			if (!configNode.TryGetValue("nodeID", ref value))
			{
				continue;
			}
			Vector2 value2 = Vector2.zero;
			if (!configNode.TryGetValue("editorPosition", ref value2))
			{
				continue;
			}
			for (int j = 0; j < editorNodeList.Count; j++)
			{
				if (editorNodeList[j].Node.id == value)
				{
					editorNodeList[j].rectTransform.anchoredPosition = value2;
					editorNodeList[j].Node.editorPosition = value2;
					editorNodeList[j].UpdateConnectors();
					break;
				}
			}
		}
	}

	public ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("selectionType", (_currentlySelectedNode != null) ? "node" : ((_currentlySelectedConnector != null) ? "connector" : "none"));
		configNode.AddValue("currentNode", (_currentlySelectedNode != null) ? _currentlySelectedNode.GetInstanceID() : 0);
		configNode.AddValue("currentConnector", (_currentlySelectedConnector != null) ? _currentlySelectedConnector.GetHashCode() : 0);
		ConfigNode configNode2 = configNode.AddNode("NODES");
		for (int i = 0; i < editorNodeList.Count; i++)
		{
			ConfigNode configNode3 = configNode2.AddNode("NODE");
			configNode3.AddValue("nodeID", editorNodeList[i].Node.id);
			configNode3.AddValue("editorPosition", editorNodeList[i].Node.editorPosition);
		}
		return configNode;
	}

	public void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ct)
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		buttonLoad.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LOAD);
		buttonSave.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_SAVE);
		buttonNew.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_NEW);
		buttonTest.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LAUNCH);
		buttonBrief.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LAUNCH);
		buttonExport.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_LAUNCH);
		buttonExit.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EXIT);
		buttonLayout.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_UI_TOPRIGHT);
		buttonLayoutGAP.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_UI_TOPRIGHT);
		searchInput.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_UI_TOPRIGHT);
		buttonUndo.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonRedo.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonMaximizeCanvas.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonMaximizeGAP.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonFit.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonSnap.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonArrange.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		buttonTS.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		validationRun.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
		validationReport.interactable = InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS);
	}

	public void Lock(bool lockTopRight, bool lockSAPGAP, bool lockTool, bool lockKeystrokes, bool lockNodeCanvas, string lockID, bool lockTopRightExceptLeave = false)
	{
		ControlTypes controlTypes = ControlTypes.None;
		if (lockTopRight)
		{
			controlTypes |= ControlTypes.EDITOR_UI_TOPRIGHT;
		}
		if (lockSAPGAP)
		{
			controlTypes |= ControlTypes.EDITOR_EDIT_NAME_FIELDS;
			controlTypes |= ControlTypes.EDITOR_GIZMO_TOOLS;
		}
		if (lockTool)
		{
			controlTypes |= ControlTypes.EDITOR_EDIT_NAME_FIELDS;
		}
		if (lockKeystrokes)
		{
			controlTypes |= ControlTypes.EDITOR_MODE_SWITCH;
		}
		if (lockNodeCanvas)
		{
			controlTypes |= ControlTypes.EDITOR_ICON_HOVER;
		}
		if (lockTopRightExceptLeave)
		{
			controlTypes |= ControlTypes.EDITOR_LOAD;
			controlTypes |= ControlTypes.EDITOR_SAVE;
			controlTypes |= ControlTypes.EDITOR_NEW;
			controlTypes |= ControlTypes.EDITOR_LAUNCH;
		}
		InputLockManager.SetControlLock(controlTypes, lockID);
	}

	public void Unlock(string lockID)
	{
		InputLockManager.RemoveControlLock(lockID);
	}

	public void SetLock(ControlTypes type, bool add, string lockId)
	{
		ControlTypes controlTypes = InputLockManager.GetControlLock(lockId);
		if (add)
		{
			if ((controlTypes & type) != type)
			{
				controlTypes |= type;
			}
		}
		else if ((controlTypes & type) == type)
		{
			controlTypes ^= type;
		}
		InputLockManager.SetControlLock(controlTypes, lockId);
	}

	public List<string> GetDisplayedNodeNames()
	{
		return MENodeCategorizer.Instance.GetDisplayedNodeNames();
	}

	public List<MEGUINode> GetDisplayedNodes()
	{
		return editorNodeList;
	}

	public void HighLightDisplayedNode(bool isActive)
	{
		MENodeCategorizer.Instance.HighLightDisplayedNodeIcon(isActive);
	}

	public void FilterSideBarNode(string nodeName, Action callback)
	{
		MENodeCategorizer.Instance.searchField.text = nodeName;
		MENodeCategorizer.Instance.SearchField_OnValueChange(nodeName, callback);
	}

	public PointerClickHandler.PointerClickEvent<PointerEventData> GetOnClickOnSearchListener()
	{
		return MENodeCategorizer.Instance.searchFieldClickHandler.onPointerClick;
	}

	public void SetOnClickOnSearchListener(PointerClickHandler.PointerClickEvent<PointerEventData> aListener)
	{
		MENodeCategorizer.Instance.searchFieldClickHandler.onPointerClick = aListener;
	}

	public void SimulateOnNodeClick(int nodeId)
	{
		editorNodeList[nodeId].Select(deselectOtherNodes: true);
	}

	public int GetNodeListCout()
	{
		return editorNodeList.Count;
	}

	public void SetParameterSelectionColor()
	{
		ColorUtility.TryParseHtmlString("#7B7F8996", out parameterSelectionOriginalColor);
	}

	public void ShowTutorialIndicator(MEGUIParameter param)
	{
		if (!(param == null))
		{
			param.ChangeColor(TutorialHighlighterColor);
			param.ShowColor();
		}
	}

	public void HideTutorialIndicator(MEGUIParameter param)
	{
		if (!(param == null))
		{
			param.ChangeColor(parameterSelectionOriginalColor);
			param.HideColor();
		}
	}

	public void SetNodeFilterInputMask(List<string> mask)
	{
		nodeFilterLock.SetInputMask(mask);
	}

	public void SetNodeFilterInputMaskToDefault()
	{
		nodeFilterLock.SetInputMaskToDefault();
	}

	public void SetNodeListInputMask(List<string> mask)
	{
		nodeListLock.SetInputMask(mask);
	}

	public void SetNodeListInputMaskToDefault()
	{
		nodeListLock.SetInputMaskToDefault();
	}

	public void SetSettingsActionPaneInputMask(List<string> mask)
	{
		settingsActionPaneLock.SetInputMask(mask);
	}

	public void SetSettingsActionPaneInputMaskToDefault()
	{
		settingsActionPaneLock.SetInputMaskToDefault();
	}
}
