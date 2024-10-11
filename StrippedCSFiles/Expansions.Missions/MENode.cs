using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MENode : MonoBehaviour, IConfigNode, IMEFlowBlock, IDiscoverable
{
	[SerializeField]
	private string _title;

	[MEGUI_ColorPicker(group = "Options", checkpointValidation = CheckpointValidationType.None, onValueChange = "SetNodeColor", order = 99, groupDisplayName = "#autoLOC_8100302", resetValue = "#888888", guiName = "#autoLOC_8100040", Tooltip = "#autoLOC_8100041")]
	public Color nodeColor;

	[MEGUI_TextArea(tabStop = true, order = 3, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100046", Tooltip = "#autoLOC_8100047")]
	public string description;

	public string basicNodeSource;

	internal string basicNodeIconURL;

	public Vector2 editorPosition;

	[SerializeField]
	internal bool isStartNode;

	[SerializeField]
	internal bool isPaused;

	public int dockedIndex;

	[MEGUI_Checkbox(hideWhenStartNode = true, hideWhenDocked = false, order = 4, guiName = "#autoLOC_8100050", Tooltip = "#autoLOC_8100051")]
	public bool isObjective;

	[MEGUI_Checkbox(hideWhenInputConnected = true, hideWhenNoTestModules = true, onControlCreated = "CatchAllControlCreated", onValueChange = "ToggleCatchAllNode", order = 5, hideWhenStartNode = true, hideWhenDocked = true, guiName = "#autoLOC_8004161", Tooltip = "#autoLOC_8004162")]
	private bool isCatchAll;

	private MEGUIParameterCheckbox catchAllParameterReference;

	[MEGUI_Checkbox(hideWhenStartNode = true, hideWhenDocked = true, order = 6, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100042", Tooltip = "#autoLOC_8100043")]
	public bool showScreenMessage;

	[MEGUI_NumberRange(checkpointValidation = CheckpointValidationType.None, canBeReset = true, groupStartCollapsed = true, maxValue = 20f, groupDisplayName = "#autoLOC_8100303", hideWhenDocked = true, minValue = 2f, group = "MissionMessage", order = 8, hideWhenStartNode = true, resetValue = "5", roundToPlaces = 0, guiName = "#autoLOC_8100044", Tooltip = "#autoLOC_8100045")]
	public int msgTime;

	[MEGUI_Checkbox(hideWhenDocked = true, hideWhenStartNode = true, guiName = "#autoLOC_8003017", Tooltip = "#autoLOC_8003018")]
	public bool isEvent;

	[MEGUI_Checkbox(hideWhenStartNode = true, onValueChange = "SetEndNode", order = 7, resetValue = "false", guiName = "#autoLOC_8100048", Tooltip = "#autoLOC_8100049")]
	public bool isEndNode;

	[MEGUI_Dropdown(groupStartCollapsed = true, group = "MissionEnd", order = 9, groupDisplayName = "#autoLOC_8100304", resetValue = "Success", guiName = "#autoLOC_8100052")]
	public MissionEndOptions missionEndOptions;

	public string message;

	[MEGUI_ListOrder(groupStartCollapsed = true, group = "Options", nameField = "title", order = 100, groupDisplayName = "#autoLOC_8100302", checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100055")]
	public List<MENode> toNodes;

	[MEGUI_Checkbox(group = "Options", checkpointValidation = CheckpointValidationType.None, order = 200, groupDisplayName = "#autoLOC_8100302", resetValue = "True", guiName = "#autoLOC_8003070", Tooltip = "#autoLOC_8003071")]
	public bool activateOnceOnly;

	public List<Color> fromNodesConnectionColor;

	[SerializeField]
	internal List<Guid> fromNodeIDs;

	public List<MENode> fromNodes;

	[SerializeField]
	internal List<Guid> toNodeIDs;

	public Mission mission;

	public List<TestGroup> testGroups;

	public List<IActionModule> actionModules;

	public List<DisplayParameter> nodeBodyParameters;

	public double activatedUT;

	public MEGUINode guiNode;

	[SerializeField]
	private bool isLogicNode;

	public List<MENode> dockedNodes;

	public List<Guid> dockedNodesIDsOnLoad;

	public MENode dockParentNode;

	[SerializeField]
	internal Guid dockParentNodeID;

	public OrbitDriver orbitDriver;

	public MissionOrbitRenderer orbitRenderer;

	public MapObject mapObject;

	public bool hasMapObject;

	private Orbit createVesselOrbit;

	private Orbit testOrbit;

	private IActionModule cachedActionCreateModule;

	public bool UpdateInfoOnRefresh;

	internal Callback<MEFlowParser> OnUpdateFlowUI;

	private DiscoveryInfo discoveryInfo;

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

	private string title
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

	[MEGUI_InputField(tabStop = true, checkpointValidation = CheckpointValidationType.None, order = 0, resetValue = "Node Title", guiName = "#autoLOC_8100000", Tooltip = "#autoLOC_8100016")]
	public string Title
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

	public string ObjectiveString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasBeenActivated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsActiveNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsReachable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsHiddenByEvent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string category
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

	internal bool IsDockedToStartNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsOrphanNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isNextObjective
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsDocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasPendingVesselLaunch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsActiveAndPendingVesselLaunch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal bool HasConnectedOutput
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsVesselNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsLogicNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsScoringNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsLaunchPadNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsNodeLabelNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasTestModules
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasVisibleChildren
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasReachableObjectives
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasObjectives
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CountObjectives
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DiscoveryInfo DiscoveryInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENode()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MENode Spawn(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MENode Spawn(Mission mission, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MENode Spawn(Mission mission, MEBasicNode basicNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MENode Clone(MENode nodeRef, Vector2 nodePos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ActionsContainModule<T>() where T : ActionModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CatchAllControlCreated(MEGUIParameterCheckbox sender)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEndNode(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleCatchAllNode(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCatchAllNode(bool newCatchAll)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal MENode ActivateNode(bool Loading = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal MENode DeactivateNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void InitializeTestGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearTestGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RunTests()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IActionModule AddAction(string actionName, ConfigNode cfg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParametersDisplayedInSAP(List<DisplayParameter> parameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParametersDisplayedInNodeBody(List<DisplayParameter> parameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeBodyParameter(string module, string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParameterToNodeBody(string module, string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameterFromNodeBody(string module, string parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetNodeColor(Color nodeColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasTestOrbit(out Orbit testOrbit, out MissionOrbitRenderer testOrbitRenderer, bool drawOrbit = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetNodeLocationInWorld()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> GetAllActionModules<T>() where T : ActionModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RunValidationWrapper(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Activated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Deactivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateMissionFlowUI(MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealAltitude()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealSituationString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float RevealMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
