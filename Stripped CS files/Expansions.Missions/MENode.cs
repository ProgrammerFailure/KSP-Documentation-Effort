using System;
using System.Collections.Generic;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using Expansions.Missions.Runtime;
using Expansions.Missions.Tests;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MENode : MonoBehaviour, IConfigNode, IMEFlowBlock, IDiscoverable
{
	[SerializeField]
	public string _title = "";

	[MEGUI_ColorPicker(group = "Options", checkpointValidation = CheckpointValidationType.None, onValueChange = "SetNodeColor", order = 99, groupDisplayName = "#autoLOC_8100302", resetValue = "#888888", guiName = "#autoLOC_8100040", Tooltip = "#autoLOC_8100041")]
	public Color nodeColor;

	[MEGUI_TextArea(tabStop = true, order = 3, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100046", Tooltip = "#autoLOC_8100047")]
	public string description = "";

	public string basicNodeSource = "";

	public string basicNodeIconURL = "";

	public Vector2 editorPosition;

	[SerializeField]
	public bool isStartNode;

	[SerializeField]
	public bool isPaused;

	public int dockedIndex;

	[MEGUI_Checkbox(hideWhenStartNode = true, hideWhenDocked = false, order = 4, guiName = "#autoLOC_8100050", Tooltip = "#autoLOC_8100051")]
	public bool isObjective;

	[MEGUI_Checkbox(hideWhenInputConnected = true, hideWhenNoTestModules = true, onControlCreated = "CatchAllControlCreated", onValueChange = "ToggleCatchAllNode", order = 5, hideWhenStartNode = true, hideWhenDocked = true, guiName = "#autoLOC_8004161", Tooltip = "#autoLOC_8004162")]
	public bool isCatchAll;

	public MEGUIParameterCheckbox catchAllParameterReference;

	[MEGUI_Checkbox(hideWhenStartNode = true, hideWhenDocked = true, order = 6, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100042", Tooltip = "#autoLOC_8100043")]
	public bool showScreenMessage;

	[MEGUI_NumberRange(checkpointValidation = CheckpointValidationType.None, canBeReset = true, groupStartCollapsed = true, maxValue = 20f, groupDisplayName = "#autoLOC_8100303", hideWhenDocked = true, minValue = 2f, group = "MissionMessage", order = 8, hideWhenStartNode = true, resetValue = "5", roundToPlaces = 0, guiName = "#autoLOC_8100044", Tooltip = "#autoLOC_8100045")]
	public int msgTime = 5;

	[MEGUI_Checkbox(hideWhenDocked = true, hideWhenStartNode = true, guiName = "#autoLOC_8003017", Tooltip = "#autoLOC_8003018")]
	public bool isEvent;

	[MEGUI_Checkbox(hideWhenStartNode = true, onValueChange = "SetEndNode", order = 7, resetValue = "false", guiName = "#autoLOC_8100048", Tooltip = "#autoLOC_8100049")]
	public bool isEndNode;

	[MEGUI_Dropdown(groupStartCollapsed = true, group = "MissionEnd", order = 9, groupDisplayName = "#autoLOC_8100304", resetValue = "Success", guiName = "#autoLOC_8100052")]
	public MissionEndOptions missionEndOptions;

	public string message = "";

	[MEGUI_ListOrder(groupStartCollapsed = true, group = "Options", nameField = "title", order = 100, groupDisplayName = "#autoLOC_8100302", checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8100055")]
	public List<MENode> toNodes;

	[MEGUI_Checkbox(group = "Options", checkpointValidation = CheckpointValidationType.None, order = 200, groupDisplayName = "#autoLOC_8100302", resetValue = "True", guiName = "#autoLOC_8003070", Tooltip = "#autoLOC_8003071")]
	public bool activateOnceOnly = true;

	public List<Color> fromNodesConnectionColor;

	[SerializeField]
	public List<Guid> fromNodeIDs;

	public List<MENode> fromNodes;

	[SerializeField]
	public List<Guid> toNodeIDs;

	public Mission mission;

	public List<TestGroup> testGroups;

	public List<IActionModule> actionModules;

	public List<DisplayParameter> nodeBodyParameters;

	public double activatedUT;

	public MEGUINode guiNode;

	[SerializeField]
	public bool isLogicNode;

	public List<MENode> dockedNodes;

	public List<Guid> dockedNodesIDsOnLoad;

	public MENode dockParentNode;

	[SerializeField]
	public Guid dockParentNodeID = Guid.Empty;

	public OrbitDriver orbitDriver;

	public MissionOrbitRenderer orbitRenderer;

	public MapObject mapObject;

	public bool hasMapObject;

	public Orbit createVesselOrbit;

	public Orbit testOrbit;

	public IActionModule cachedActionCreateModule;

	public bool UpdateInfoOnRefresh;

	public Callback<MEFlowParser> OnUpdateFlowUI;

	public DiscoveryInfo discoveryInfo;

	public Guid id { get; set; }

	public string title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			base.gameObject.name = "Node (" + Localizer.Format(value) + ")";
		}
	}

	[MEGUI_InputField(tabStop = true, checkpointValidation = CheckpointValidationType.None, order = 0, resetValue = "Node Title", guiName = "#autoLOC_8100000", Tooltip = "#autoLOC_8100016")]
	public string Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
			if (guiNode != null)
			{
				guiNode.SetTitleText(value);
			}
		}
	}

	public string ObjectiveString => title;

	public bool HasBeenActivated
	{
		get
		{
			if (!(activatedUT > 0.0) && !isStartNode)
			{
				return IsDockedToStartNode;
			}
			return true;
		}
	}

	public bool IsActiveNode => mission.activeNode == this;

	public bool IsReachable
	{
		get
		{
			if (mission.isStarted && mission.flow != null && mission.flow.NodeReversePaths.ContainsKey(this))
			{
				MENodePathInfo mENodePathInfo = mission.flow.NodeReversePaths[this];
				int num = 0;
				while (true)
				{
					if (num < mENodePathInfo.paths.Count)
					{
						if (mENodePathInfo.paths[num].Nodes.Contains(mission.activeNode))
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
			return true;
		}
	}

	public bool IsHiddenByEvent
	{
		get
		{
			if (mission.flow != null && mission.flow.NodeReversePaths.ContainsKey(this))
			{
				MENodePathInfo mENodePathInfo = mission.flow.NodeReversePaths[this];
				for (int i = 0; i < mENodePathInfo.paths.Count; i++)
				{
					MEPath mEPath = mENodePathInfo.paths[i];
					for (int j = 0; j < mEPath.Nodes.Count; j++)
					{
						MENode mENode = mEPath.Nodes[j];
						if (!mENode.IsActiveNode)
						{
							if (mENode.isEvent && !mENode.HasBeenActivated)
							{
								return true;
							}
							continue;
						}
						return false;
					}
				}
			}
			return false;
		}
	}

	public string category { get; set; }

	public bool IsDockedToStartNode
	{
		get
		{
			if (dockParentNode == null)
			{
				return false;
			}
			return dockParentNode.id == mission.startNode.id;
		}
	}

	public bool IsOrphanNode
	{
		get
		{
			if (fromNodeIDs.Count < 1 && !isStartNode && isCatchAll)
			{
				return !IsDocked;
			}
			return false;
		}
	}

	public bool isNextObjective => mission.IsNextObjective(this);

	public bool IsDocked => dockParentNode != null;

	public bool HasPendingVesselLaunch
	{
		get
		{
			int num = 0;
			while (true)
			{
				if (num < actionModules.Count)
				{
					ActionCreateVessel actionCreateVessel = actionModules[num] as ActionCreateVessel;
					if (actionCreateVessel != null && !actionCreateVessel.vesselSituation.launched)
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
	}

	public bool IsActiveAndPendingVesselLaunch
	{
		get
		{
			if (IsVesselNode && HasBeenActivated)
			{
				return HasPendingVesselLaunch;
			}
			return false;
		}
	}

	public bool HasConnectedOutput
	{
		get
		{
			if (toNodes.Count < 1)
			{
				return false;
			}
			int num = 0;
			while (true)
			{
				if (num < toNodes.Count)
				{
					if (toNodes[num].fromNodeIDs.Contains(id))
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
	}

	public bool IsVesselNode
	{
		get
		{
			if (ActionsContainModule<ActionCreateVessel>())
			{
				return true;
			}
			if (ActionsContainModule<ActionCreateAsteroid>())
			{
				return true;
			}
			if (ActionsContainModule<ActionCreateKerbal>())
			{
				return true;
			}
			if (ActionsContainModule<ActionCreateFlag>())
			{
				return true;
			}
			if (ActionsContainModule<ActionCreateComet>())
			{
				return true;
			}
			return false;
		}
	}

	public bool IsLogicNode => isLogicNode;

	public bool IsScoringNode
	{
		get
		{
			if (ActionsContainModule<ActionMissionScore>())
			{
				return true;
			}
			return false;
		}
	}

	public bool IsLaunchPadNode
	{
		get
		{
			if (ActionsContainModule<ActionCreateLaunchSite>())
			{
				return true;
			}
			return false;
		}
	}

	public bool IsNodeLabelNode
	{
		get
		{
			for (int i = 0; i < testGroups.Count; i++)
			{
				for (int j = 0; j < testGroups[i].testModules.Count; j++)
				{
					if (testGroups[i].testModules[j] is ITestNodeLabel)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool HasTestModules
	{
		get
		{
			if (testGroups.Count > 0)
			{
				return testGroups[0].testModules.Count > 0;
			}
			return false;
		}
	}

	public bool HasVisibleChildren => false;

	public bool HasReachableObjectives => false;

	public bool HasObjectives => false;

	public int CountObjectives => 0;

	public DiscoveryInfo DiscoveryInfo => discoveryInfo;

	public void Awake()
	{
		id = Guid.NewGuid();
		nodeColor = Color.gray;
		editorPosition = Vector2.zero;
		testGroups = new List<TestGroup>();
		actionModules = new List<IActionModule>();
		fromNodesConnectionColor = new List<Color>();
		fromNodeIDs = new List<Guid>();
		toNodeIDs = new List<Guid>();
		nodeBodyParameters = new List<DisplayParameter>();
		dockedNodes = new List<MENode>();
		dockedNodesIDsOnLoad = new List<Guid>();
		category = "";
		discoveryInfo = new DiscoveryInfo(this);
		fromNodes = new List<MENode>();
		toNodes = new List<MENode>();
	}

	public void Start()
	{
		byte[] hashBytes = null;
		string signature = null;
		if (mission != null)
		{
			hashBytes = mission.HashBytes;
			signature = mission.Signature;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			GameEvents.onLevelWasLoaded.Add(OnLevelLoaded);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(OnLevelLoaded);
	}

	public static MENode Spawn(Mission mission)
	{
		GameObject gameObject = new GameObject("Node");
		MENode mENode = gameObject.AddComponent<MENode>();
		mENode.mission = mission;
		if (mission != null)
		{
			gameObject.transform.SetParent(mission.transform);
		}
		return mENode;
	}

	public static MENode Spawn(Mission mission, string title)
	{
		MENode mENode = Spawn(mission);
		mENode.title = title;
		return mENode;
	}

	public static MENode Spawn(Mission mission, MEBasicNode basicNode)
	{
		MENode mENode = Spawn(mission);
		mENode.title = basicNode.displayName;
		mENode.nodeColor = basicNode.headerColor;
		mENode.description = basicNode.description;
		mENode.basicNodeSource = basicNode.name;
		mENode.basicNodeIconURL = basicNode.iconURL;
		mENode.category = basicNode.category;
		mENode.testGroups.Clear();
		TestGroup testGroup = new TestGroup(mENode);
		for (int i = 0; i < basicNode.tests.Count; i++)
		{
			testGroup.AddTestModule(basicNode.tests[i], null);
		}
		mENode.testGroups.Add(testGroup);
		if (mENode.testGroups[0].testModules.Count > 0)
		{
			mENode.SetCatchAllNode(newCatchAll: true);
			if (mission != null)
			{
				mission.UpdateOrphanNodeState(mENode, makeOrphan: true);
			}
		}
		mENode.actionModules.Clear();
		for (int j = 0; j < basicNode.actions.Count; j++)
		{
			mENode.AddAction(basicNode.actions[j], null);
		}
		mENode.isLogicNode = basicNode.isLogicNode;
		mENode.isObjective = basicNode.isObjective;
		mENode.AddParametersDisplayedInNodeBody(basicNode.defaultNodeBodyParameters);
		mENode.AddParametersDisplayedInSAP(basicNode.defaultSAPParameters);
		return mENode;
	}

	public static MENode Clone(MENode nodeRef, Vector2 nodePos)
	{
		MENode mENode = Spawn(nodeRef.mission);
		mENode.title = nodeRef.Title;
		mENode.nodeColor = nodeRef.nodeColor;
		mENode.description = nodeRef.description;
		mENode.basicNodeSource = nodeRef.basicNodeSource;
		mENode.basicNodeIconURL = nodeRef.basicNodeIconURL;
		mENode.editorPosition = nodePos;
		for (int i = 0; i < nodeRef.testGroups.Count; i++)
		{
			TestGroup testGroup = new TestGroup(mENode);
			ConfigNode node = new ConfigNode();
			nodeRef.testGroups[i].Save(node);
			testGroup.Load(node);
			mENode.testGroups.Add(testGroup);
			testGroup.OnCloned(nodeRef.testGroups[i]);
		}
		if (nodeRef.isCatchAll)
		{
			mENode.SetCatchAllNode(newCatchAll: true);
			if (mENode.mission != null)
			{
				mENode.mission.UpdateOrphanNodeState(mENode, makeOrphan: true);
			}
		}
		ActionModule[] components = nodeRef.GetComponents<ActionModule>();
		for (int j = 0; j < components.Length; j++)
		{
			ConfigNode configNode = new ConfigNode();
			components[j].Save(configNode);
			mENode.AddAction(components[j].GetType().Name, configNode)?.OnCloned(ref components[j]);
		}
		mENode.isLogicNode = nodeRef.IsLogicNode;
		mENode.isObjective = nodeRef.isObjective;
		mENode.AddParametersDisplayedInNodeBody(nodeRef.nodeBodyParameters);
		mENode.AddParametersDisplayedInSAP(nodeRef.nodeBodyParameters);
		return mENode;
	}

	public bool ActionsContainModule<T>() where T : ActionModule
	{
		int num = 0;
		while (true)
		{
			if (num < actionModules.Count)
			{
				if (actionModules[num] is T)
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

	public void CatchAllControlCreated(MEGUIParameterCheckbox sender)
	{
		catchAllParameterReference = sender;
	}

	public void SetEndNode(bool value)
	{
		if (guiNode != null)
		{
			guiNode.SetEndNode(value);
		}
	}

	public void ToggleCatchAllNode(bool value)
	{
		SetCatchAllNode(value);
		if (mission != null)
		{
			mission.UpdateOrphanNodeState(this, IsOrphanNode);
		}
	}

	public void SetCatchAllNode(bool newCatchAll)
	{
		isCatchAll = newCatchAll;
		if (guiNode != null)
		{
			guiNode.UpdateInputConnectorImage();
		}
		if (catchAllParameterReference != null)
		{
			catchAllParameterReference.RefreshUI();
		}
	}

	public MENode ActivateNode(bool Loading = false)
	{
		if (!Loading)
		{
			activatedUT = Planetarium.GetUniversalTime();
			GameEvents.Mission.onNodeActivated.Fire(this);
			Activated();
		}
		if (HasTestModules || !IsDocked)
		{
			mission.SetNextObjectives(this);
		}
		if (isEvent)
		{
			mission.RemoveInactiveEvent(this);
		}
		for (int i = 0; i < toNodes.Count; i++)
		{
			MENode mENode = toNodes[i];
			if (!mENode.IsOrphanNode)
			{
				mENode.InitializeTestGroups();
			}
		}
		if (!Loading)
		{
			int j = 0;
			for (int count = actionModules.Count; j < count; j++)
			{
				MissionSystem.Instance.StartCoroutine(actionModules[j].Fire());
			}
			for (int k = 0; k < toNodes.Count; k++)
			{
				MENode mENode2 = toNodes[k];
				if (mENode2.showScreenMessage && !string.IsNullOrEmpty(mENode2.description) && !mENode2.isStartNode && !mENode2.IsOrphanNode && !mENode2.IsDocked)
				{
					ScreenMessages.PostScreenMessage(mENode2.description, mENode2.msgTime, ScreenMessageStyle.UPPER_LEFT, persist: true);
				}
			}
		}
		return this;
	}

	public MENode DeactivateNode()
	{
		GameEvents.Mission.onNodeDeactivated.Fire(this);
		Deactivated();
		for (int i = 0; i < toNodes.Count; i++)
		{
			MENode mENode = toNodes[i];
			if (!mENode.IsOrphanNode)
			{
				mENode.ClearTestGroups();
			}
		}
		ClearTestGroups();
		if (IsOrphanNode)
		{
			InitializeTestGroups();
		}
		return this;
	}

	public void InitializeTestGroups()
	{
		for (int i = 0; i < testGroups.Count; i++)
		{
			testGroups[i].initializeGroup();
		}
	}

	public void ClearTestGroups()
	{
		for (int i = 0; i < testGroups.Count; i++)
		{
			testGroups[i].ClearGroup();
		}
	}

	public bool RunTests()
	{
		if (activateOnceOnly && activatedUT > 0.0)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < testGroups.Count)
			{
				if (testGroups[num].Test())
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

	public IActionModule AddAction(string actionName, ConfigNode cfg)
	{
		Type type = Type.GetType("Expansions.Missions.Actions." + actionName, throwOnError: false, ignoreCase: false);
		IActionModule actionModule = null;
		if (type == null)
		{
			type = AssemblyLoader.GetClassByName(typeof(ActionModule), actionName);
		}
		if (type != null)
		{
			actionModule = (IActionModule)base.gameObject.AddComponent(type);
			actionModule.Initialize(this);
			if (cfg != null)
			{
				actionModule?.Load(cfg);
			}
			actionModules.Add(actionModule);
		}
		else
		{
			Debug.Log($"Error! Action module {actionName} type was not found in Assembly");
		}
		return actionModule;
	}

	public void AddParametersDisplayedInSAP(List<DisplayParameter> parameters)
	{
		for (int i = 0; i < testGroups.Count; i++)
		{
			for (int j = 0; j < testGroups[i].testModules.Count; j++)
			{
				for (int k = 0; k < parameters.Count; k++)
				{
					if (testGroups[i].testModules[j].GetName() == parameters[k].module)
					{
						testGroups[i].testModules[j].AddParameterToSAP(parameters[k].parameter);
					}
				}
			}
		}
		for (int l = 0; l < actionModules.Count; l++)
		{
			for (int m = 0; m < parameters.Count; m++)
			{
				if (actionModules[l].GetName() == parameters[m].module)
				{
					actionModules[l].AddParameterToSAP(parameters[m].parameter);
				}
			}
		}
		if (!isStartNode)
		{
			return;
		}
		for (int n = 0; n < parameters.Count; n++)
		{
			if (mission.situation.GetName() == parameters[n].module)
			{
				mission.situation.AddParameterToSAP(parameters[n].parameter);
			}
		}
	}

	public void AddParametersDisplayedInNodeBody(List<DisplayParameter> parameters)
	{
		nodeBodyParameters = parameters;
	}

	public bool HasNodeBodyParameter(string module, string parameter)
	{
		int num = 0;
		while (true)
		{
			if (num < nodeBodyParameters.Count)
			{
				if (nodeBodyParameters[num].module == module && nodeBodyParameters[num].parameter == parameter)
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

	public void AddParameterToNodeBody(string module, string parameter)
	{
		DisplayParameter item = default(DisplayParameter);
		item.module = module;
		item.parameter = parameter;
		nodeBodyParameters.AddUnique(item);
	}

	public void RemoveParameterFromNodeBody(string module, string parameter)
	{
		int num = nodeBodyParameters.Count - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (nodeBodyParameters[num].module == module && nodeBodyParameters[num].parameter == parameter)
				{
					break;
				}
				num--;
				continue;
			}
			return;
		}
		nodeBodyParameters.RemoveAt(num);
	}

	public void SetNodeColor(Color nodeColor)
	{
		if (guiNode != null)
		{
			guiNode.SetNodeColor(nodeColor);
		}
	}

	public bool HasTestOrbit(out Orbit testOrbit, out MissionOrbitRenderer testOrbitRenderer, bool drawOrbit = false)
	{
		testOrbitRenderer = null;
		testOrbit = null;
		for (int i = 0; i < testGroups.Count; i++)
		{
			for (int j = 0; j < testGroups[i].testModules.Count; j++)
			{
				if (!(testGroups[i].testModules[j] is INodeOrbit nodeOrbit) || !nodeOrbit.HasNodeOrbit())
				{
					continue;
				}
				testOrbit = nodeOrbit.GetNodeOrbit();
				if (testOrbit == null)
				{
					continue;
				}
				TestOrbit testOrbit2 = testGroups[i].testModules[j] as TestOrbit;
				if (!(testOrbit2 != null))
				{
					testOrbit = null;
					continue;
				}
				if (drawOrbit)
				{
					testOrbit2.DrawOrbit();
					if (testOrbit2.orbitRenderer != null)
					{
						testOrbitRenderer = testOrbit2.orbitRenderer;
					}
				}
				return true;
			}
		}
		return false;
	}

	public void OnLevelLoaded(GameScenes scene)
	{
		if (scene != GameScenes.TRACKSTATION && scene != GameScenes.FLIGHT)
		{
			return;
		}
		bool drawOrbit = HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER;
		if (IsVesselNode)
		{
			for (int i = 0; i < actionModules.Count; i++)
			{
				if (actionModules[i] is INodeOrbit)
				{
					cachedActionCreateModule = actionModules[i];
					INodeOrbit nodeOrbit = actionModules[i] as INodeOrbit;
					createVesselOrbit = nodeOrbit.GetNodeOrbit();
				}
			}
		}
		HasTestOrbit(out testOrbit, out var testOrbitRenderer, drawOrbit);
		if ((createVesselOrbit != null || testOrbit != null) && MapView.fetch != null && HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			if (createVesselOrbit != null)
			{
				orbitDriver = GetComponent<OrbitDriver>() ?? base.gameObject.AddComponent<OrbitDriver>();
				orbitDriver.orbit = createVesselOrbit;
			}
			else if (testOrbit != null)
			{
				orbitDriver = testOrbitRenderer.driver;
			}
			if (mapObject == null)
			{
				mapObject = ScaledMovement.Create(Title, this);
			}
			if (createVesselOrbit != null && orbitRenderer == null)
			{
				orbitRenderer = testOrbitRenderer;
			}
		}
	}

	public Vector3 GetNodeLocationInWorld()
	{
		Vector3 result = Vector3.zero;
		if (IsVesselNode && cachedActionCreateModule != null)
		{
			result = cachedActionCreateModule.ActionLocation();
		}
		if (testOrbit != null)
		{
			result = testOrbit.getPositionAtUT((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.UniversalTime : 0.0);
		}
		return result;
	}

	public List<T> GetAllActionModules<T>() where T : ActionModule
	{
		List<T> list = new List<T>();
		Type typeFromHandle = typeof(T);
		if (actionModules != null)
		{
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

	public void RunValidationWrapper(MissionEditorValidator validator)
	{
		if (!isEndNode && !IsDockedToStartNode && !ActionsContainModule<ActionCreateLaunchSite>() && (!IsDocked || !IsScoringNode) && !HasConnectedOutput)
		{
			validator.AddNodeFail(this, Localizer.Format("#autoLOC_8006040"));
		}
		RunValidation(validator);
		for (int i = 0; i < testGroups.Count; i++)
		{
			testGroups[i].RunValidationWrapper(validator);
		}
		for (int j = 0; j < actionModules.Count; j++)
		{
			actionModules[j].RunValidationWrapper(validator);
		}
	}

	public virtual void RunValidation(MissionEditorValidator validator)
	{
	}

	public virtual void Activated()
	{
	}

	public virtual void Deactivated()
	{
	}

	public void UpdateMissionFlowUI(MEFlowParser parser)
	{
		if (OnUpdateFlowUI != null)
		{
			OnUpdateFlowUI(parser);
		}
	}

	public string RevealName()
	{
		return title;
	}

	public string RevealDisplayName()
	{
		return title;
	}

	public double RevealSpeed()
	{
		return 0.0;
	}

	public double RevealAltitude()
	{
		return 0.0;
	}

	public string RevealSituationString()
	{
		return title;
	}

	public string RevealType()
	{
		return description;
	}

	public float RevealMass()
	{
		return 0f;
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("id"))
		{
			id = new Guid(node.GetValue("id"));
		}
		string value = "";
		node.TryGetValue("title", ref value);
		title = value;
		node.TryGetValue("description", ref description);
		node.TryGetValue("showScreenMessage", ref showScreenMessage);
		node.TryGetValue("msgTime", ref msgTime);
		if (node.HasValue("category"))
		{
			category = node.GetValue("category");
		}
		node.TryGetValue("editorPosition", ref editorPosition);
		node.TryGetValue("isStartNode", ref isStartNode);
		if (string.IsNullOrEmpty(category) && isStartNode)
		{
			category = "Start";
		}
		node.TryGetValue("isEndNode", ref isEndNode);
		node.TryGetValue("message", ref message);
		node.TryGetValue("dockedIndex", ref dockedIndex);
		node.TryGetValue("isObjective", ref isObjective);
		node.TryGetValue("isCatchAll", ref isCatchAll);
		node.TryGetValue("isEvent", ref isEvent);
		node.TryGetValue("activateOnceOnly", ref activateOnceOnly);
		if (node.HasValue("activatedUT"))
		{
			activatedUT = double.Parse(node.GetValue("activatedUT"));
		}
		else
		{
			activatedUT = 0.0;
		}
		node.TryGetValue("isLogicNode", ref isLogicNode);
		node.TryGetValue("basicNodeSource", ref basicNodeSource);
		node.TryGetValue("basicNodeIconURL", ref basicNodeIconURL);
		if (((basicNodeIconURL == "") & (basicNodeSource != "")) && MissionEditorLogic.Instance != null)
		{
			int count = MissionEditorLogic.Instance.basicNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (basicNodeSource == MissionEditorLogic.Instance.basicNodes[i].name)
				{
					basicNodeIconURL = MissionEditorLogic.Instance.basicNodes[i].iconURL;
					break;
				}
			}
		}
		string value2 = "";
		if (node.TryGetValue("nodeColor", ref value2) && ColorUtility.TryParseHtmlString(value2, out nodeColor))
		{
			SetNodeColor(nodeColor);
		}
		node.TryGetValue("dockParentNodeID", ref dockParentNodeID);
		if (node.HasNode("FROMNODES"))
		{
			ConfigNode[] nodes = node.GetNode("FROMNODES").GetNodes();
			for (int j = 0; j < nodes.Length; j++)
			{
				string value3 = "";
				if (nodes[j].TryGetValue("nodeID", ref value3))
				{
					Guid item = new Guid(value3);
					if (!fromNodeIDs.Contains(item))
					{
						fromNodeIDs.Add(item);
					}
				}
				string value4 = "";
				if (nodes[j].TryGetValue("connectorColor", ref value4) && ColorUtility.TryParseHtmlString(value4, out var color))
				{
					fromNodesConnectionColor.Add(color);
				}
			}
		}
		if (node.HasNode("TONODEIDS"))
		{
			string[] values = node.GetNode("TONODEIDS").GetValues("nodeID");
			for (int k = 0; k < values.Length; k++)
			{
				Guid item2 = new Guid(values[k]);
				if (!toNodeIDs.Contains(item2))
				{
					toNodeIDs.Add(item2);
				}
			}
		}
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("TESTGROUPS", ref node2))
		{
			ConfigNode[] nodes2 = node2.GetNodes("TESTGROUP");
			for (int l = 0; l < nodes2.Length; l++)
			{
				TestGroup testGroup = new TestGroup(this);
				testGroup.Load(nodes2[l]);
				testGroups.Add(testGroup);
			}
		}
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("ACTIONS", ref node3))
		{
			ConfigNode[] nodes3 = node3.GetNodes("ACTIONMODULE");
			for (int m = 0; m < nodes3.Length; m++)
			{
				AddAction(nodes3[m].GetValue("name"), nodes3[m]);
			}
		}
		ConfigNode node4 = new ConfigNode();
		if (node.TryGetNode("NODEBODY_PARAMETERS", ref node4))
		{
			ConfigNode[] nodes4 = node4.GetNodes("MODULE_PARAMETER");
			for (int n = 0; n < nodes4.Length; n++)
			{
				DisplayParameter item3 = default(DisplayParameter);
				item3.Load(nodes4[n]);
				nodeBodyParameters.AddUnique(item3);
			}
		}
		if (node.HasNode("DOCKEDNODEIDS"))
		{
			string[] values2 = node.GetNode("DOCKEDNODEIDS").GetValues("nodeID");
			for (int num = 0; num < values2.Length; num++)
			{
				dockedNodesIDsOnLoad.Add(new Guid(values2[num]));
			}
		}
		node.TryGetEnum("missionEndOptions", ref missionEndOptions, MissionEndOptions.Fail);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("title", title);
		node.AddValue("id", id.ToString());
		string text = "";
		text = description.Replace("\n", "\\n");
		text = text.Replace("\t", "\\t");
		node.AddValue("description", text);
		if (!string.IsNullOrEmpty(category))
		{
			node.AddValue("category", category);
		}
		node.AddValue("showScreenMessage", showScreenMessage);
		node.AddValue("msgTime", msgTime);
		node.AddValue("editorPosition", editorPosition);
		node.AddValue("isStartNode", isStartNode);
		node.AddValue("isEndNode", isEndNode);
		string text2 = message.Replace("\n", "\\n");
		text2 = text2.Replace("\t", "\\t");
		node.AddValue("message", text2);
		node.AddValue("dockedIndex", dockedIndex);
		node.AddValue("isObjective", isObjective);
		node.AddValue("isCatchAll", isCatchAll);
		node.AddValue("isEvent", isEvent);
		node.AddValue("activateOnceOnly", activateOnceOnly);
		node.AddValue("activatedUT", activatedUT);
		node.AddValue("isLogicNode", isLogicNode);
		node.AddValue("basicNodeSource", basicNodeSource);
		node.AddValue("basicNodeIconURL", basicNodeIconURL);
		node.AddValue("nodeColor", "#" + ColorUtility.ToHtmlStringRGB(nodeColor));
		if (dockParentNode != null)
		{
			node.AddValue("dockParentNodeID", dockParentNode.id);
		}
		if (fromNodes.Count > 0)
		{
			ConfigNode configNode = node.AddNode("FROMNODES");
			for (int i = 0; i < fromNodes.Count; i++)
			{
				MENode mENode = fromNodes[i];
				ConfigNode configNode2 = configNode.AddNode("FROMNODE");
				configNode2.AddValue("nodeID", mENode.id);
				if (!(mENode.guiNode != null))
				{
					continue;
				}
				int j = 0;
				for (int count = mENode.guiNode.OutputConnectors.Count; j < count; j++)
				{
					if (mENode.guiNode.OutputConnectors[j].IsConnectedToNode(guiNode))
					{
						configNode2.AddValue("connectorColor", "#" + ColorUtility.ToHtmlStringRGB(mENode.guiNode.OutputConnectors[j].LineColour));
						break;
					}
				}
			}
		}
		if (toNodes.Count > 0)
		{
			ConfigNode configNode3 = node.AddNode("TONODEIDS");
			for (int k = 0; k < toNodes.Count; k++)
			{
				if (!toNodes[k].IsOrphanNode)
				{
					configNode3.AddValue("nodeID", toNodes[k].id);
				}
			}
		}
		if (testGroups.Count > 0)
		{
			ConfigNode configNode4 = node.AddNode("TESTGROUPS");
			for (int l = 0; l < testGroups.Count; l++)
			{
				ConfigNode node2 = configNode4.AddNode("TESTGROUP");
				testGroups[l].Save(node2);
			}
		}
		if (actionModules.Count > 0)
		{
			ConfigNode configNode5 = node.AddNode("ACTIONS");
			for (int m = 0; m < actionModules.Count; m++)
			{
				ConfigNode node3 = configNode5.AddNode("ACTIONMODULE");
				actionModules[m].Save(node3);
			}
		}
		if (nodeBodyParameters.Count > 0)
		{
			ConfigNode configNode6 = node.AddNode("NODEBODY_PARAMETERS");
			for (int n = 0; n < nodeBodyParameters.Count; n++)
			{
				ConfigNode configNode7 = configNode6.AddNode("MODULE_PARAMETER");
				configNode7.AddValue("module", nodeBodyParameters[n].module);
				configNode7.AddValue("parameter", nodeBodyParameters[n].parameter);
			}
		}
		if (dockedNodes.Count > 0)
		{
			ConfigNode configNode8 = node.AddNode("DOCKEDNODEIDS");
			for (int num = 0; num < dockedNodes.Count; num++)
			{
				configNode8.AddValue("nodeID", dockedNodes[num].id);
			}
		}
		node.AddValue("missionEndOptions", missionEndOptions);
	}
}
