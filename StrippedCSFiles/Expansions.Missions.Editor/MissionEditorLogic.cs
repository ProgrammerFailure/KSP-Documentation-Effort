using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionEditorLogic : MonoBehaviour, IMEHistoryTarget
{
	protected static string startUpMissionPath;

	internal static string MissionToTest;

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

	private UIStateImage maximizeCanvasImage;

	private UIStateImage snapGridImage;

	private UIStateImage maximizeGAPImage;

	public MissionsBrowserDialog loadMissionDialog;

	public bool disallowSave;

	public CheckpointBrowserDialog testMissionDialog;

	private ScreenMessage modeMsg;

	private MEGUIConnector CurrentConnector;

	public MEGUIConnector meConnectorPrefab;

	public MEGUINode MENodePrefab;

	public MENodeColors MENodeColorConfig;

	[SerializeField]
	internal MEGUINode.ColoredConnectorOptions ColoredConnectorPins;

	[SerializeField]
	private GameObject prefabNodeCanvas;

	public TextMeshProUGUI zoom_level;

	public Button zoom_buttonPlus;

	public Button zoom_buttonMinus;

	public MENodeCanvas NodeCanvas;

	public RectTransform NodeCanvasUIRect;

	private Canvas canvas;

	public MEActionPane actionPane;

	public MEGUIPanel[] Panels;

	private MEGUINode _currentlySelectedNode;

	[HideInInspector]
	public List<MEGUINode> selectedNodes;

	private List<MEGUINode> copiedGUINodes;

	[HideInInspector]
	public List<MEGUIConnector> selectedConnectors;

	[SerializeField]
	private Vector2 pastedNodeOffset;

	[HideInInspector]
	public Vector2 nodeDragPos;

	[HideInInspector]
	public Vector2 lastFramePos;

	private Vector2 cachePositionForPastedNodes;

	private int nodeGroupIndex;

	private int connectorGroupIndex;

	private float maxGridXoffset;

	public float connectorSelectionAllowance;

	private MEGUIConnector _currentlySelectedConnector;

	internal List<MEGUIConnector> editorConnectorList;

	internal List<MEGUINode> editorNodeList;

	public List<MEBasicNode> basicNodes;

	protected bool actionPaneMoving;

	public bool isMouseOverGAPScroll;

	internal string MissionNameAtLastSave;

	internal bool briefingHasChanged;

	public float distanceForNodeDocking;

	public MECrewAssignmentDialog crewAssignmentDialog;

	private Dictionary<uint, ShipConstruct> vesselCache;

	public RectTransform toolBox;

	[SerializeField]
	private Button validationReport;

	[SerializeField]
	private Image validationButtonImage;

	[SerializeField]
	private Button validationRun;

	[SerializeField]
	private TMP_InputField searchInput;

	[SerializeField]
	private CanvasGroupInputLock nodeFilterLock;

	[SerializeField]
	private CanvasGroupInputLock nodeListLock;

	[SerializeField]
	private CanvasGroupInputLock settingsActionPaneLock;

	[SerializeField]
	internal Color TutorialHighlighterColor;

	private Color parameterSelectionOriginalColor;

	private const string BLUE_PARAMETER_SELECTION_COLOR_HEXA = "#193D9896";

	private const string ORIGINAL_PARAMETER_SELECTION_COLOR_HEXA = "#7B7F8996";

	private GameObject currentSelectedGameObject;

	public Action<GameObject> OnSelectedGameObjectChange;

	public Action OnLeave;

	public Action OnExitEditor;

	public List<MissionPack> userMissionPacks;

	private List<MissionPack> stockMissionPacks;

	private bool isCmd;

	private double startTime;

	private ConfigNode missionToLoadRoot;

	private MissionFileInfo missionToLoadMFI;

	private string missionToLoadFullPath;

	internal HashSet<string> incompatibleCraft;

	private MissionBriefingDialog onBriefBriefingDialog;

	private UIConfirmDialog openingMissionSaveConfirmDialog;

	private UIConfirmDialog newTestConfirmDialog;

	private MissionsBrowserDialog onLoadConfirmDialog;

	private MissionBriefingDialog onSaveConfirmDialog;

	public PopupDialog unableToStartMission;

	private PopupDialog onExitMissionBuilderDialog;

	public PopupDialog dialogSelectEditor;

	public MissionValidationDialog missionValidationDialog;

	private TMP_InputField tabNextInputfield;

	private Guid lastHistoryStateIDAtLastSave;

	public static MissionEditorLogic Instance
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

	public Canvas Canvas
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

	public MEGUINode CurrentSelectedNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected set
		{
			throw null;
		}
	}

	internal bool PreventNodeDestruction
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public MEGUIConnector CurrentlySelectedConnector
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected set
		{
			throw null;
		}
	}

	public Mission EditorMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public List<MissionPack> StockMissionPacks
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private bool HasUnsavedChanges
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionEditorLogic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnlockAllOptionAtStartup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadBasicNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionImported()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLocalizationLockOverriden()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTitleChanged(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRedo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUndo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMaximizeCanvas(bool newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMaximizeGAP(bool newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBrief()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionBriefingChanged(Mission updatedMission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SaveMission(Callback afterSave)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveMissionNoBriefingWarning(bool dontShowAgain, Callback afterSave)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void trytoSave(Callback afterSave, bool overwrite = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool onSaveConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CopyMissionBanners(string missionDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSaveConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onExitConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateEmptyMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTestCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTestCheckSFSInProgress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnlockTestInputLocks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTestConfirm(bool dontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LaunchNewTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LaunchNewTestMissionSetupSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LaunchNewTestMissionSetupFail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckpointToLoadSelected(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckpointTestMissionSetupSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCheckpointToLoadCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKSPedia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrackingStation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnlockTSInputLocks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTSConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContinueToTrackingStation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TrackingStationMissionSetupSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnlockExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RunValidator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetValidator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void UpdateValidationLed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidationReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnlockValidation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidationRun()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFitInView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSnapToGrid(bool newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnArrangeGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FitNodesInView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ArrangeGraphNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PlaceArrangeNode(List<MEGUINode> nodes, int level, float yOffset, ref List<Guid> placedNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLayoutConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLayoutConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoadConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoadConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void MissionToLoadSelected(string fullPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionLaunchsitesGenerated(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupTutorial(string tutorialClass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionToLoadCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupMission(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConnectorButtonClicked(MEGUINode node, MENodeConnectionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConnectorButtonDragged(MEGUINode node, MENodeConnectionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConnectorButtonDropped(MEGUINode node, MENodeConnectionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIConnector ConnectNodes(MEGUINode fromNode, MEGUINode toNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIConnector ConnectNodes(MEGUINode fromNode, MEGUINode toNode, Color connectionColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyCurrentConnector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddConnectorToConnectorList(MEGUIConnector connector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearConnector(MEGUIConnector connector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode CreateNodeAtPosition(Vector2 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode CreateNodeAtPosition(Vector2 position, MEBasicNode basicNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode CreateNodeAtPosition(Vector2 position, MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopySelectedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PasteCopiedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode CreateNodeAtMousePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode CreateNodeAtMousePosition(MEBasicNode basicNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetGridMousePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CanvasClicked(Vector2 clickPosition, bool dragging)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NodeSelectionChange(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddNodeToSelectedList(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNodeFromSelectedList(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSelectedNodesList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSelectedNodesPosition(MEGUINode node, Vector2 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisplayMultipleSelectedItems(int amount, string itemName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddConnectorToSelectedList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveConnectorFromSelectedList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearConnectorGroupSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearAllSelections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsNodeDockable(MEGUINode testNode, ref MEGUINode nodeToDockTo, Vector3 nodePositionCheckOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConnectorSelectionChange(MEGUIConnector connector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LockLocalizedTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckDialogsBeforeExiting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TabInputField(bool direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PreventCanvasObjectChanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollSettingToBottom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollSettingToTop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollBar(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollToParameterGroup(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameterGroup[] GetParentParameterGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollPanel(float height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCurrentSelectedGameObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddNode(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNode(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode GetNodeFromID(int nodeID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetToolboxWidth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct LoadVessel(MissionCraft vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct LoadVessel(ConfigNode craftNode, uint persistentID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalAdded(ProtoCrewMember kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalTypeChanged(ProtoCrewMember kerbal, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalNameChanged(ProtoCrewMember kerbal, string oldName, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalRemoved(ProtoCrewMember kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StartUpMissionEditor(string missionPath = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GetStartNodeColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GetCategoryColor(string categoryName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistorySelectionChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryNodeSelectionChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryConnectorSelectionChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryArrangeGraphChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Lock(bool lockTopRight, bool lockSAPGAP, bool lockTool, bool lockKeystrokes, bool lockNodeCanvas, string lockID, bool lockTopRightExceptLeave = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unlock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLock(ControlTypes type, bool add, string lockId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDisplayedNodeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUINode> GetDisplayedNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighLightDisplayedNode(bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FilterSideBarNode(string nodeName, Action callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PointerClickHandler.PointerClickEvent<PointerEventData> GetOnClickOnSearchListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOnClickOnSearchListener(PointerClickHandler.PointerClickEvent<PointerEventData> aListener)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SimulateOnNodeClick(int nodeId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetNodeListCout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetParameterSelectionColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowTutorialIndicator(MEGUIParameter param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideTutorialIndicator(MEGUIParameter param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeFilterInputMask(List<string> mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeFilterInputMaskToDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeListInputMask(List<string> mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeListInputMaskToDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSettingsActionPaneInputMask(List<string> mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSettingsActionPaneInputMaskToDefault()
	{
		throw null;
	}
}
