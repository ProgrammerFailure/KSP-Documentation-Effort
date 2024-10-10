using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleDockingNode : PartModule, ITargetable, IStageSeparator, IContractObjectiveModule, IResourceConsumer, IJointLockState
{
	[KSPField]
	public string nodeTransformName = "dockingNode";

	[KSPField]
	public string controlTransformName = "";

	[KSPField]
	public float undockEjectionForce = 10f;

	[KSPField]
	public float minDistanceToReEngage = 1f;

	[KSPField]
	public float acquireRange = 0.5f;

	[KSPField]
	public float acquireMinFwdDot = 0.7f;

	[KSPField]
	public float acquireMinRollDot = float.MinValue;

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002397", guiUnits = "%")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 5f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float acquireForceTweak = 100f;

	[KSPField]
	public float acquireForce = 2f;

	[KSPField]
	public float acquireTorque = 2f;

	[KSPField]
	public float acquireTorqueRoll;

	[KSPField]
	public float captureRange = 0.06f;

	[KSPField]
	public float captureMinFwdDot = 0.998f;

	[KSPField]
	public float captureMinRollDot = float.MinValue;

	[KSPField]
	public float captureMaxRvel = 0.3f;

	[KSPField]
	public string referenceAttachNode = "";

	[KSPField]
	public bool useReferenceAttachNode;

	[KSPField]
	public string nodeType = "size1";

	[KSPField]
	public int deployAnimationController = -1;

	[KSPField]
	public float deployAnimationTarget = 1f;

	[KSPField]
	public bool animReadyEnter = true;

	[KSPField]
	public bool animReadyExit = true;

	[KSPField]
	public bool animDisengageEnter = true;

	[KSPField]
	public bool animDisengageExit = true;

	[KSPField]
	public bool animDisabledEnter = true;

	[KSPField]
	public bool animDisabledExit = true;

	[KSPField]
	public bool animDisableIfNot1 = true;

	[KSPField]
	public bool animEnableIf1 = true;

	[KSPField]
	public bool animCaptureOff;

	[KSPField]
	public bool animUndockOn;

	[KSPField]
	public bool setAnimWrite = true;

	[KSPField]
	public bool gendered;

	[KSPField]
	public bool genderFemale = true;

	[KSPField]
	public bool snapRotation;

	[KSPField]
	public float snapOffset = 90f;

	[KSPField(isPersistant = true)]
	public bool crossfeed = true;

	[KSPField]
	public bool canRotate;

	[KSPField]
	public string rotationTransformName;

	public Transform rotationTransform;

	[KSPField]
	public string rotationAxis = "Z";

	public Vector3 initialRotation;

	public float cachedInitialAngle;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float traverseVelocity = 2.5f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public Vector2 hardMinMaxLimits = new Vector2(-15f, 15f);

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float efficiency = 0.75f;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float maxMotorOutput = 100f;

	[KSPField]
	public float baseResourceConsumptionRate = 0.02f;

	[KSPField]
	public float referenceConsumptionVelocity = 180f;

	public float motorRate;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002700")]
	public bool nodeIsLocked;

	[KSPAxisField(isPersistant = true, incrementalSpeed = 30f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = false, guiActive = false, ignoreClampWhenIncremental = true, guiName = "#autoLOC_8014167")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 180f, minValue = -179.99f, affectSymCounterparts = UI_Scene.Editor)]
	public float targetAngle;

	public Quaternion targetRotation;

	[KSPField(advancedTweakable = false, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003304")]
	[UI_Toggle(disabledText = "#autoLOC_8003303", enabledText = "#autoLOC_8003302", tipText = "#autoLOC_8003301", affectSymCounterparts = UI_Scene.None)]
	public bool inverted;

	public float workingJointTargetAngle;

	[SerializeField]
	public SoftJointLimit upperRotationLimit;

	[SerializeField]
	public SoftJointLimit lowerRotationLimit;

	public UI_FloatRange targetAngleUIField;

	public BaseAxisField targetAngleAxisField;

	public bool rotationInitComplete;

	public bool partJointUnbreakable;

	public DockedVesselInfo vesselInfo;

	public string state = "Ready";

	public Transform nodeTransform;

	public Transform controlTransform;

	public double TatUndock;

	public uint dockedPartUId;

	public int dockingNodeModuleIndex;

	public ModuleDockingNode otherNode;

	public HashSet<string> nodeTypes;

	public ModuleAnimateGeneric deployAnimator;

	public AttachNode referenceNode;

	public bool setStagingState = true;

	public ModuleDockingNode sameVesselUndockNode;

	public ModuleDockingNode sameVesselUndockOtherNode;

	public bool sameVesselUndockRedock;

	public bool DebugFSMState;

	public bool partActionMenuOpen;

	public bool undockPreAttached;

	public bool physicsLessMode;

	public PartJoint sameVesselDockJoint;

	public KerbalFSM fsm;

	public KFSMState st_ready;

	public KFSMState st_acquire;

	public KFSMState st_acquire_dockee;

	public KFSMState st_docked_dockee;

	public KFSMState st_docked_docker;

	public KFSMState st_docker_sameVessel;

	public KFSMState st_disengage;

	public KFSMState st_disabled;

	public KFSMState st_preattached;

	public KFSMEvent on_nodeApproach;

	public KFSMEvent on_nodeDistance;

	public KFSMEvent on_capture;

	public KFSMEvent on_capture_dockee;

	public KFSMEvent on_capture_docker;

	public KFSMEvent on_capture_docker_sameVessel;

	public KFSMEvent on_undock;

	public KFSMEvent on_sameVessel_disconnect;

	public KFSMEvent on_disable;

	public KFSMEvent on_enable;

	public KFSMEvent on_decouple;

	public KFSMEvent on_preattachedDecouple;

	public KFSMEvent on_swapPrimary;

	public KFSMEvent on_swapSecondary;

	public KFSMEvent on_construction_Attach;

	public KFSMEvent on_construction_Detach;

	public BaseEvent evtSetAsTarget;

	public BaseEvent evtUnsetTarget;

	public float maxAnglePerFrame;

	public float driveTargetAngle;

	public float visualTargetAngle;

	public bool hasEnoughResources = true;

	public List<PartResourceDefinition> consumedResources;

	[KSPField]
	public bool staged = true;

	public List<AdjusterDockingNodeBase> adjusterCache = new List<AdjusterDockingNodeBase>();

	public bool IsRotating { get; set; }

	public float JointTargetAngle
	{
		get
		{
			workingJointTargetAngle = targetAngle;
			if (inverted)
			{
				workingJointTargetAngle = 0f - workingJointTargetAngle;
			}
			if ((bool)otherNode)
			{
				if (otherNode.inverted)
				{
					workingJointTargetAngle += 0f - otherNode.targetAngle;
				}
				else
				{
					workingJointTargetAngle += otherNode.targetAngle;
				}
			}
			return workingJointTargetAngle;
		}
	}

	public float VisualTargetAngle
	{
		get
		{
			float num = 0f;
			if (RotationJoint != null && RotationJoint == base.part.attachJoint.Joint)
			{
				num = 0f - targetAngle;
				if (inverted)
				{
					num = 0f - num;
				}
			}
			else
			{
				num = targetAngle;
				if (inverted)
				{
					num = 0f - num;
				}
			}
			return num;
		}
	}

	public ConfigurableJoint RotationJoint
	{
		get
		{
			if (fsm.Started && fsm.CurrentState == st_docked_docker)
			{
				if ((bool)base.part.attachJoint)
				{
					return base.part.attachJoint.Joint;
				}
			}
			else if (fsm.Started && fsm.CurrentState == st_docked_dockee)
			{
				if ((bool)otherNode && (bool)otherNode.part.attachJoint)
				{
					return otherNode.part.attachJoint.Joint;
				}
			}
			else if (fsm.Started && fsm.CurrentState == st_preattached && (bool)base.part.attachJoint && (bool)otherNode)
			{
				if ((bool)base.part.attachJoint.Parent && base.part.attachJoint.Parent.HasModuleImplementing<ModuleDockingNode>())
				{
					return base.part.attachJoint.Joint;
				}
				if ((bool)otherNode.part.attachJoint)
				{
					return otherNode.part.attachJoint.Joint;
				}
			}
			return null;
		}
	}

	public bool RotationJointHost
	{
		get
		{
			if (otherNode != null && GetDominantNode(this, otherNode) == this)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsDisabled
	{
		get
		{
			if (fsm != null && fsm.Started)
			{
				return fsm.CurrentState == st_disabled;
			}
			return false;
		}
	}

	[KSPAction("#autoLOC_6002701", advancedTweakable = true)]
	public void ToggleLockedAction(KSPActionParam param)
	{
		if (nodeIsLocked)
		{
			DisengageNodeLock();
		}
		else if (!EngageNodeLock())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("Cannot Lock Docking part, Docking Node is moving."), 5f);
		}
	}

	[KSPAction("#autoLOC_6002702", advancedTweakable = true)]
	public void ServoEngageLockAction(KSPActionParam param)
	{
		if (!EngageNodeLock())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("Cannot Lock Docking part, Servo is moving."), 5f);
		}
	}

	[KSPAction("#autoLOC_6002703", advancedTweakable = true)]
	public void ServoDisgageLockAction(KSPActionParam param)
	{
		DisengageNodeLock();
	}

	public bool EngageNodeLock()
	{
		if (nodeIsLocked)
		{
			return true;
		}
		if (!nodeIsLocked && !IsRotating)
		{
			base.Fields["nodeIsLocked"].SetValue(true, this);
			ModifyLocked(null);
			return true;
		}
		return false;
	}

	public void DisengageNodeLock()
	{
		if (nodeIsLocked)
		{
			base.Fields["nodeIsLocked"].SetValue(false, this);
			ModifyLocked(null);
		}
	}

	public void ModifyLocked(object obj)
	{
		base.Fields["targetAngle"].guiInteractable = !nodeIsLocked;
		base.Fields["inverted"].guiInteractable = !nodeIsLocked;
		if (nodeIsLocked && (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor))
		{
			RecurseCoordUpdate(base.part, HighLogic.LoadedSceneIsFlight ? base.part.vessel.rootPart : EditorLogic.fetch.ship.parts[0]);
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (base.vessel != null)
			{
				base.vessel.CycleAllAutoStrut();
			}
		}
		else if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch.ship != null)
		{
			for (int i = 0; i < EditorLogic.fetch.ship.parts.Count; i++)
			{
				EditorLogic.fetch.ship.parts[i].CycleAutoStrut();
			}
		}
	}

	public override void OnAwake()
	{
		nodeTypes = new HashSet<string>();
		if (setStagingState)
		{
			setStagingState = false;
			stagingEnabled = false;
		}
		base.part.dockingPorts.AddUnique(this);
		evtSetAsTarget = base.Events["SetAsTarget"];
		evtUnsetTarget = base.Events["UnsetTarget"];
		nodeTransform = base.part.FindModelTransform(nodeTransformName);
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		float num = baseResourceConsumptionRate * maxMotorOutput * (traverseVelocity / referenceConsumptionVelocity);
		motorRate = num / efficiency;
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			resHandler.inputResources[i].startUpHandler = true;
			resHandler.inputResources[i].startUpAmount = motorRate;
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("state"))
		{
			state = node.GetValue("state");
		}
		if (node.HasValue("dockUId"))
		{
			dockedPartUId = uint.Parse(node.GetValue("dockUId"));
		}
		if (node.HasValue("dockNodeIdx"))
		{
			dockingNodeModuleIndex = int.Parse(node.GetValue("dockNodeIdx"));
		}
		if (node.HasNode("DOCKEDVESSEL"))
		{
			vesselInfo = new DockedVesselInfo();
			vesselInfo.Load(node.GetNode("DOCKEDVESSEL"));
		}
		if (referenceAttachNode != string.Empty)
		{
			referenceNode = base.part.FindAttachNode(referenceAttachNode);
		}
		base.part.fuelCrossFeed = crossfeed;
		base.Events["EnableXFeed"].active = !crossfeed;
		base.Events["DisableXFeed"].active = crossfeed;
		string[] array = nodeType.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			nodeTypes.Add(array[i]);
		}
		hardMinMaxLimits.x = Mathf.Clamp(hardMinMaxLimits.x, -86f, 0f);
		hardMinMaxLimits.y = Mathf.Clamp(hardMinMaxLimits.y, 0f, 86f);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("state", (fsm == null || !fsm.Started) ? "Ready" : fsm.currentStateName);
		node.AddValue("dockUId", dockedPartUId);
		node.AddValue("dockNodeIdx", dockingNodeModuleIndex);
		if (vesselInfo != null)
		{
			vesselInfo.Save(node.AddNode("DOCKEDVESSEL"));
		}
		if (HighLogic.LoadedSceneIsFlight && base.vessel != null && base.vessel.loaded && canRotate && !nodeIsLocked && otherNode != null)
		{
			ApplyCoordsUpdate();
		}
	}

	public override void OnStart(StartState st)
	{
		base.Events["Undock"].active = false;
		base.Events["UndockSameVessel"].active = false;
		base.Events["MakePrimary"].active = false;
		nodeTransform = base.part.FindModelTransform(nodeTransformName);
		if (!nodeTransform)
		{
			Debug.LogWarning("[Docking Node Module]: WARNING - No node transform found with name " + nodeTransformName, base.part.gameObject);
			return;
		}
		if (controlTransformName == string.Empty)
		{
			controlTransform = base.part.transform;
		}
		else
		{
			controlTransform = base.part.FindModelTransform(controlTransformName);
			if (!controlTransform)
			{
				Debug.LogWarning("[Docking Node Module]: WARNING - No control transform found with name " + controlTransformName, base.part.gameObject);
				controlTransform = base.part.transform;
			}
		}
		if (base.part.physicalSignificance != 0)
		{
			Debug.LogWarning("[Docking Node Module]: WARNING - The part for a docking node module cannot be physicsless!", base.part.gameObject);
			base.part.physicalSignificance = Part.PhysicalSignificance.FULL;
		}
		if (deployAnimationController != -1)
		{
			deployAnimator = base.part.Modules.GetModule(deployAnimationController) as ModuleAnimateGeneric;
		}
		base.part.fuelCrossFeed = crossfeed;
		base.Events["EnableXFeed"].active = !crossfeed;
		base.Events["DisableXFeed"].active = crossfeed;
		StartCoroutine(lateFSMStart(st));
		if (rotationTransformName != string.Empty)
		{
			rotationTransform = base.part.FindModelTransform(rotationTransformName);
			if (!rotationTransform)
			{
				Debug.LogWarning("[Docking Node Module]: WARNING - No rotation transform found with name " + rotationTransformName, base.part.gameObject);
				canRotate = false;
			}
		}
		GameEvents.onPartActionUIShown.Add(OnPartMenuOpen);
		GameEvents.onPartActionUIDismiss.Add(OnPartMenuClose);
		if (!HighLogic.LoadedSceneIsMissionBuilder && canRotate)
		{
			initialRotation = rotationTransform.localRotation.eulerAngles;
			base.Fields.TryGetFieldUIControl<UI_FloatRange>("targetAngle", out targetAngleUIField);
			targetAngleUIField.minValue = hardMinMaxLimits.x;
			targetAngleUIField.maxValue = hardMinMaxLimits.y;
			targetAngleAxisField = base.Fields["targetAngle"] as BaseAxisField;
			targetAngleAxisField.minValue = hardMinMaxLimits.x;
			targetAngleAxisField.maxValue = hardMinMaxLimits.y;
		}
		base.Fields["targetAngle"].guiActive = false;
		base.Fields["inverted"].guiActive = false;
		base.Fields["targetAngle"].guiActiveEditor = false;
		base.Fields["inverted"].guiActiveEditor = false;
		if ((canRotate && otherNode == null && sameVesselDockJoint == null) || !canRotate)
		{
			nodeIsLocked = true;
		}
		base.Fields["nodeIsLocked"].OnValueModified += ModifyLocked;
		base.Fields["targetAngle"].guiInteractable = !nodeIsLocked;
	}

	public override void OnStartFinished(StartState state)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (otherNode != null)
		{
			if (canRotate && RotationJoint != null && base.part.attachJoint != null && rotationTransform != null)
			{
				RotationJoint.angularXMotion = ConfigurableJointMotion.Limited;
				driveTargetAngle = JointTargetAngle;
				initialRotation = rotationTransform.localRotation.eulerAngles;
				cachedInitialAngle = JointTargetAngle;
				if (RotationJoint == base.part.attachJoint.Joint)
				{
					Quaternion targetLocalRotation = SetTargetRotation(Quaternion.identity, JointTargetAngle - cachedInitialAngle, setRotation: true, Vector3.up);
					RotationJoint.SetTargetRotationLocal(targetLocalRotation, Quaternion.identity);
				}
				if (RotationJoint == base.part.attachJoint.Joint)
				{
					targetRotation = SetTargetRotation(Quaternion.Euler(initialRotation), VisualTargetAngle, setRotation: false);
				}
				else
				{
					targetRotation = SetTargetRotation(Quaternion.Euler(initialRotation), 0f - VisualTargetAngle, setRotation: false);
				}
				rotationTransform.localRotation = targetRotation;
				visualTargetAngle = VisualTargetAngle;
				rotationInitComplete = true;
				if (otherNode != null && !otherNode.rotationInitComplete)
				{
					otherNode.targetRotation = SetTargetRotation(Quaternion.Euler(otherNode.initialRotation), 0f - otherNode.VisualTargetAngle, setRotation: false);
					otherNode.rotationTransform.localRotation = otherNode.targetRotation;
					otherNode.visualTargetAngle = otherNode.VisualTargetAngle;
					otherNode.rotationInitComplete = true;
				}
			}
		}
		else
		{
			rotationInitComplete = true;
		}
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public void OnPartPack()
	{
		if (canRotate && !nodeIsLocked && otherNode != null)
		{
			ApplyCoordsUpdate();
		}
	}

	public void OnPartUnpack()
	{
		if (!canRotate || !(otherNode != null))
		{
			return;
		}
		if (!IsRotating)
		{
			driveTargetAngle = JointTargetAngle;
			visualTargetAngle = VisualTargetAngle;
			if (RotationJoint != null && base.part.attachJoint != null && RotationJoint == base.part.attachJoint.Joint)
			{
				Quaternion targetLocalRotation = SetTargetRotation(Quaternion.identity, JointTargetAngle - cachedInitialAngle, setRotation: true, Vector3.up);
				RotationJoint.SetTargetRotationLocal(targetLocalRotation, Quaternion.identity);
			}
		}
		SetJointHighLowLimits();
	}

	public override void OnInventoryModeEnable()
	{
		if (referenceAttachNode != string.Empty)
		{
			referenceNode = base.part.FindAttachNode(referenceAttachNode);
		}
		nodeTransform = base.part.FindModelTransform(nodeTransformName);
		if (referenceNode == null || !(referenceNode.attachedPart != null))
		{
			return;
		}
		int num = 0;
		int count = referenceNode.attachedPart.Modules.Count;
		ModuleDockingNode moduleDockingNode;
		while (true)
		{
			if (num < count)
			{
				moduleDockingNode = referenceNode.attachedPart.Modules[num] as ModuleDockingNode;
				if (!(moduleDockingNode == null) && moduleDockingNode.referenceNode != null && moduleDockingNode.referenceNode.attachedPart == base.part)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		otherNode = moduleDockingNode;
		if (fsm.Started)
		{
			fsm.RunEvent(on_construction_Attach);
		}
		otherNode.otherNode = this;
		otherNode.OnConstructionAttach();
	}

	public override void OnInventoryModeDisable()
	{
		if (referenceAttachNode != string.Empty)
		{
			referenceNode = base.part.FindAttachNode(referenceAttachNode);
		}
		nodeTransform = base.part.FindModelTransform(nodeTransformName);
		if (fsm.Started)
		{
			fsm.RunEvent(on_construction_Detach);
		}
	}

	public override void DemoteToPhysicslessPart()
	{
		physicsLessMode = true;
	}

	public override void PromoteToPhysicalPart()
	{
		physicsLessMode = false;
	}

	public void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		if (inpPart.persistentId == base.part.persistentId)
		{
			partActionMenuOpen = true;
			UpdatePAWUI();
		}
	}

	public void OnPartMenuClose(Part inpPart)
	{
		if (inpPart.persistentId == base.part.persistentId)
		{
			partActionMenuOpen = false;
		}
	}

	public ModuleDockingNode FindOtherNode()
	{
		Part part = FlightGlobals.FindPartByID(dockedPartUId);
		if (part != null)
		{
			return part.Modules.GetModule(dockingNodeModuleIndex) as ModuleDockingNode;
		}
		return null;
	}

	public IEnumerator lateFSMStart(StartState st)
	{
		yield return null;
		SetupFSM();
		if ((st & StartState.Editor) != 0)
		{
			yield break;
		}
		if (state.Contains("Docked"))
		{
			otherNode = FindOtherNode();
			if (otherNode == null)
			{
				state = "Ready";
			}
			else if (state.Contains("(dockee)"))
			{
				if (otherNode.state.Contains("Acquire"))
				{
					state = st_acquire_dockee.name;
				}
				else if (!otherNode.state.Contains("Docked") || (otherNode.part.parent != base.part && base.part.parent != otherNode.part))
				{
					state = "Ready";
				}
			}
		}
		if (otherNode == null && (state.Contains("Acquire") || state.Contains("Disengage")))
		{
			otherNode = FindOtherNode();
			if (otherNode == null)
			{
				state = "Ready";
			}
		}
		if (referenceNode != null && referenceNode.attachedPart != null)
		{
			int i = 0;
			for (int count = referenceNode.attachedPart.Modules.Count; i < count; i++)
			{
				ModuleDockingNode moduleDockingNode = referenceNode.attachedPart.Modules[i] as ModuleDockingNode;
				if (!(moduleDockingNode == null) && moduleDockingNode.referenceNode != null && moduleDockingNode.referenceNode.attachedPart == base.part)
				{
					otherNode = moduleDockingNode;
				}
			}
			fsm.StartFSM(st_preattached);
		}
		else
		{
			fsm.StartFSM(state);
		}
	}

	public void SetupFSM()
	{
		fsm = new KerbalFSM();
		KerbalFSM kerbalFSM = fsm;
		kerbalFSM.OnStateChange = (Callback<KFSMState, KFSMState, KFSMEvent>)Delegate.Combine(kerbalFSM.OnStateChange, new Callback<KFSMState, KFSMState, KFSMEvent>(OnFSMStateChange));
		KerbalFSM kerbalFSM2 = fsm;
		kerbalFSM2.OnEventCalled = (Callback<KFSMEvent>)Delegate.Combine(kerbalFSM2.OnEventCalled, new Callback<KFSMEvent>(OnFSMEventCalled));
		st_ready = new KFSMState("Ready");
		st_ready.OnEnter = delegate
		{
			if (animReadyEnter && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_ready.OnFixedUpdate = delegate
		{
			otherNode = FindNodeApproaches();
		};
		st_ready.OnLeave = delegate
		{
			if (animReadyExit && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_ready);
		st_acquire = new KFSMState("Acquire");
		st_acquire.OnFixedUpdate = delegate
		{
			Vector3 vector = otherNode.nodeTransform.position - nodeTransform.position;
			Vector3 vector2 = Vector3.Cross(nodeTransform.forward, -otherNode.nodeTransform.forward);
			Vector3 vector3 = Vector3.Cross(nodeTransform.up, -otherNode.nodeTransform.up);
			float num = 1f / Mathf.Max(vector.sqrMagnitude, 0.05f);
			base.part.AddForceAtPosition(vector * num * acquireForce * 0.5f * (acquireForceTweak * 0.01f), nodeTransform.position);
			base.part.AddTorque(vector2 * num * acquireTorque * 0.5f * (acquireForceTweak * 0.01f));
			base.part.AddTorque(vector3 * num * acquireTorqueRoll * 0.5f * (acquireForceTweak * 0.01f));
			otherNode.part.AddForceAtPosition(-(vector * num) * otherNode.acquireForce * 0.5f * (otherNode.acquireForceTweak * 0.01f), otherNode.nodeTransform.position);
			otherNode.part.AddTorque(-(vector2 * num) * otherNode.acquireTorque * 0.5f * (otherNode.acquireForceTweak * 0.01f));
			otherNode.part.AddTorque(-(vector3 * num) * otherNode.acquireTorqueRoll * 0.5f * (otherNode.acquireForceTweak * 0.01f));
		};
		fsm.AddState(st_acquire);
		st_acquire_dockee = new KFSMState("Acquire (dockee)");
		fsm.AddState(st_acquire_dockee);
		st_docked_dockee = new KFSMState("Docked (dockee)");
		st_docked_dockee.OnEnter = delegate
		{
			if (sameVesselUndockRedock && fsm.LastState == st_docker_sameVessel)
			{
				DestroySameVesselJoint();
				otherNode.DockToVessel(this);
				otherNode.fsm.RunEvent(otherNode.on_capture_docker);
			}
			if (deployAnimator != null && setAnimWrite)
			{
				deployAnimator.SetUIWrite(state: false);
			}
			if (fsm.LastState != st_ready)
			{
				base.Events["MakePrimary"].active = true;
			}
			sameVesselUndockRedock = false;
		};
		fsm.AddState(st_docked_dockee);
		st_docked_docker = new KFSMState("Docked (docker)");
		st_docked_docker.OnEnter = delegate
		{
			if (sameVesselUndockRedock && fsm.LastState == st_docker_sameVessel)
			{
				DestroySameVesselJoint();
				DockToVessel(otherNode);
				otherNode.fsm.RunEvent(otherNode.on_capture_dockee);
			}
			base.Events["Undock"].active = true;
			base.Events["MakePrimary"].active = false;
			base.part.fuelLookupTargets.Add(otherNode.part);
			otherNode.part.fuelLookupTargets.Add(base.part);
			GameEvents.onPartFuelLookupStateChange.Fire(new GameEvents.HostedFromToAction<bool, Part>(host: true, otherNode.part, base.part));
			if (deployAnimator != null && setAnimWrite)
			{
				deployAnimator.SetUIWrite(state: false);
			}
			sameVesselUndockRedock = false;
		};
		st_docked_docker.OnUpdate = delegate
		{
			base.Events["Undock"].active = !IsAdjusterBlockingUndock() && !otherNode.IsAdjusterBlockingUndock();
		};
		st_docked_docker.OnLeave = delegate
		{
			base.Events["Undock"].active = false;
			base.part.fuelLookupTargets.Remove(otherNode.part);
			otherNode.part.fuelLookupTargets.Remove(base.part);
			GameEvents.onPartFuelLookupStateChange.Fire(new GameEvents.HostedFromToAction<bool, Part>(host: true, base.part, otherNode.part));
		};
		fsm.AddState(st_docked_docker);
		st_docker_sameVessel = new KFSMState("Docked (same vessel)");
		st_docker_sameVessel.OnEnter = delegate
		{
			base.Events["UndockSameVessel"].active = true;
			base.Events["MakePrimary"].active = false;
			if (!base.vessel.packed)
			{
				DockToSameVessel(otherNode);
			}
			sameVesselUndockRedock = false;
		};
		st_docker_sameVessel.OnUpdate = delegate
		{
			if (sameVesselDockJoint == null && !base.vessel.packed)
			{
				DockToSameVessel(otherNode);
			}
			base.Events["UndockSameVessel"].active = !IsAdjusterBlockingUndock() && !otherNode.IsAdjusterBlockingUndock();
		};
		st_docker_sameVessel.OnLeave = delegate
		{
			base.Events["UndockSameVessel"].active = false;
			if (sameVesselUndockRedock)
			{
				if (animCaptureOff && (bool)deployAnimator)
				{
					deployAnimator.Events["Toggle"].active = false;
				}
			}
			else
			{
				DestroySameVesselJoint();
				if (fsm.LastEvent == on_sameVessel_disconnect)
				{
					otherNode = null;
					vesselInfo = null;
				}
				sameVesselUndockNode = null;
				sameVesselUndockOtherNode = null;
			}
		};
		fsm.AddState(st_docker_sameVessel);
		st_disengage = new KFSMState("Disengage");
		st_disengage.OnEnter = delegate
		{
			if (animDisengageEnter && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_disengage.OnLeave = delegate
		{
			if (animDisengageExit && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_disengage);
		st_disabled = new KFSMState("Disabled");
		st_disabled.OnEnter = delegate
		{
			if (animDisabledEnter && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_disabled.OnLeave = delegate
		{
			if (animDisabledExit && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_disabled);
		st_preattached = new KFSMState("PreAttached");
		st_preattached.OnEnter = delegate
		{
			base.Events["Undock"].active = true;
			undockPreAttached = true;
		};
		st_preattached.OnLeave = delegate
		{
			undockPreAttached = false;
		};
		fsm.AddState(st_preattached);
		on_nodeApproach = new KFSMEvent("Node Approach");
		on_nodeApproach.updateMode = KFSMUpdateMode.UPDATE;
		on_nodeApproach.GoToStateOnEvent = st_acquire;
		on_nodeApproach.OnCheckCondition = (KFSMState st) => otherNode != null;
		on_nodeApproach.OnEvent = delegate
		{
			dockedPartUId = otherNode.part.flightID;
			dockingNodeModuleIndex = otherNode.part.Modules.IndexOf(otherNode);
			if (otherNode.vessel != base.vessel)
			{
				if (Vessel.GetDominantVessel(base.vessel, otherNode.vessel) == base.vessel)
				{
					on_nodeApproach.GoToStateOnEvent = st_acquire_dockee;
				}
				else
				{
					on_nodeApproach.GoToStateOnEvent = st_acquire;
				}
			}
			else if (GetDominantNode(this, otherNode) == this)
			{
				on_nodeApproach.GoToStateOnEvent = st_docked_dockee;
			}
			else
			{
				on_nodeApproach.GoToStateOnEvent = st_acquire;
			}
		};
		fsm.AddEvent(on_nodeApproach, st_ready);
		on_capture = new KFSMEvent("Capture");
		on_capture.updateMode = KFSMUpdateMode.UPDATE;
		on_capture.OnCheckCondition = (KFSMState st) => (otherNode.vessel != base.vessel) ? (CheckDockContact(this, otherNode, captureRange, captureMinFwdDot, captureMinRollDot) && (base.part.rb.velocity - otherNode.part.rb.velocity).sqrMagnitude <= captureMaxRvel * captureMaxRvel) : CheckDockContact(this, otherNode, captureRange, captureMinFwdDot, captureMinRollDot);
		on_capture.OnEvent = delegate
		{
			if (otherNode.vessel != base.vessel)
			{
				if (Vessel.GetDominantVessel(base.vessel, otherNode.vessel) == base.vessel)
				{
					on_capture.GoToStateOnEvent = st_docked_dockee;
					otherNode.DockToVessel(this);
					otherNode.fsm.RunEvent(otherNode.on_capture_docker);
					return;
				}
				on_capture.GoToStateOnEvent = st_docked_docker;
				DockToVessel(otherNode);
				otherNode.fsm.RunEvent(otherNode.on_capture_dockee);
			}
			else
			{
				if (GetDominantNode(this, otherNode) == this)
				{
					on_capture.GoToStateOnEvent = st_docked_dockee;
					otherNode.fsm.RunEvent(otherNode.on_capture_docker_sameVessel);
					return;
				}
				on_capture.GoToStateOnEvent = st_docker_sameVessel;
				otherNode.fsm.RunEvent(otherNode.on_capture_dockee);
			}
			if (animCaptureOff && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddEvent(on_capture, st_acquire);
		on_capture_dockee = new KFSMEvent("Capture (dockee)");
		on_capture_dockee.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_capture_dockee.GoToStateOnEvent = st_docked_dockee;
		fsm.AddEvent(on_capture_dockee, st_acquire_dockee);
		fsm.AddEvent(on_capture_dockee, st_docker_sameVessel);
		fsm.AddEvent(on_capture_dockee, st_disengage);
		on_capture_docker = new KFSMEvent("Capture (docker)");
		on_capture_docker.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_capture_docker.OnEvent = delegate
		{
			if (animCaptureOff && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		on_capture_docker.GoToStateOnEvent = st_docked_docker;
		fsm.AddEvent(on_capture_docker, st_acquire);
		fsm.AddEvent(on_capture_docker, st_docker_sameVessel);
		fsm.AddEvent(on_capture_docker, st_ready);
		on_capture_docker_sameVessel = new KFSMEvent("Capture (docker same vessel)");
		on_capture_docker_sameVessel.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_capture_docker_sameVessel.OnEvent = delegate
		{
			if (animCaptureOff && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		on_capture_docker_sameVessel.GoToStateOnEvent = st_docker_sameVessel;
		fsm.AddEvent(on_capture_docker_sameVessel, st_acquire);
		fsm.AddEvent(on_capture_docker_sameVessel, st_acquire_dockee);
		on_nodeDistance = new KFSMEvent("Node Distanced");
		on_nodeDistance.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		on_nodeDistance.GoToStateOnEvent = st_ready;
		on_nodeDistance.OnCheckCondition = (KFSMState st) => NodeIsTooFar();
		on_nodeDistance.OnEvent = delegate
		{
			otherNode = null;
			vesselInfo = null;
		};
		fsm.AddEvent(on_nodeDistance, st_acquire, st_acquire_dockee, st_disengage, st_docked_dockee);
		on_undock = new KFSMEvent("Undock");
		on_undock.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_undock.GoToStateOnEvent = st_disengage;
		on_undock.OnEvent = delegate
		{
			if (deployAnimator != null && setAnimWrite)
			{
				deployAnimator.SetUIWrite(state: true);
			}
			if (animUndockOn && (bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
			on_undock.GoToStateOnEvent = (otherNode ? st_disengage : st_ready);
			base.Events["Undock"].active = false;
		};
		fsm.AddEvent(on_undock, st_docked_docker, st_docked_dockee, st_preattached, st_docker_sameVessel);
		on_sameVessel_disconnect = new KFSMEvent("Same Vessel Disconnect");
		on_sameVessel_disconnect.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		on_sameVessel_disconnect.GoToStateOnEvent = st_ready;
		on_sameVessel_disconnect.OnCheckCondition = delegate
		{
			if (otherNode.vessel != base.vessel && ((sameVesselUndockNode == null && sameVesselUndockOtherNode == null) || (sameVesselUndockNode != this && sameVesselUndockOtherNode != this)))
			{
				sameVesselUndockRedock = true;
				otherNode.sameVesselUndockRedock = true;
				if (Vessel.GetDominantVessel(base.vessel, otherNode.vessel) == base.vessel)
				{
					on_sameVessel_disconnect.GoToStateOnEvent = st_docked_dockee;
				}
				else
				{
					on_sameVessel_disconnect.GoToStateOnEvent = st_docked_docker;
				}
				return true;
			}
			sameVesselUndockNode = null;
			sameVesselUndockOtherNode = null;
			on_sameVessel_disconnect.GoToStateOnEvent = st_ready;
			return otherNode == null || otherNode.vessel != base.vessel;
		};
		on_sameVessel_disconnect.OnEvent = delegate
		{
			if ((bool)otherNode && !sameVesselUndockRedock)
			{
				otherNode.OnOtherNodeSameVesselDisconnect();
			}
		};
		fsm.AddEvent(on_sameVessel_disconnect, st_docker_sameVessel);
		on_disable = new KFSMEvent("Disable");
		on_disable.updateMode = KFSMUpdateMode.UPDATE;
		on_disable.OnCheckCondition = (KFSMState st) => (bool)deployAnimator && animDisableIfNot1 && deployAnimator.Progress != 1f;
		on_disable.GoToStateOnEvent = st_disabled;
		fsm.AddEvent(on_disable, st_ready, st_disengage);
		on_enable = new KFSMEvent("Enable");
		on_enable.updateMode = KFSMUpdateMode.UPDATE;
		on_enable.OnCheckCondition = (KFSMState st) => (bool)deployAnimator && animEnableIf1 && deployAnimator.Progress == 1f;
		on_enable.GoToStateOnEvent = st_ready;
		fsm.AddEvent(on_enable, st_disabled);
		on_preattachedDecouple = new KFSMEvent("Pre-Attached Part Decoupled");
		on_preattachedDecouple.updateMode = KFSMUpdateMode.UPDATE;
		on_preattachedDecouple.OnCheckCondition = (KFSMState st) => referenceNode.attachedPart == null;
		on_preattachedDecouple.GoToStateOnEvent = st_ready;
		fsm.AddEvent(on_preattachedDecouple, st_preattached);
		on_swapPrimary = new KFSMEvent("SwapPrimary");
		on_swapPrimary.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_swapPrimary.GoToStateOnEvent = st_docked_docker;
		fsm.AddEvent(on_swapPrimary, st_docked_dockee);
		on_swapSecondary = new KFSMEvent("SwapSecondary");
		on_swapSecondary.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_swapSecondary.GoToStateOnEvent = st_docked_dockee;
		fsm.AddEvent(on_swapSecondary, st_docked_docker);
		on_construction_Attach = new KFSMEvent("OnConstructionAttach");
		on_construction_Attach.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_construction_Attach.GoToStateOnEvent = st_preattached;
		on_construction_Attach.OnEvent = delegate
		{
		};
		fsm.AddEvent(on_construction_Attach, st_ready, st_acquire, st_acquire_dockee, st_docked_dockee, st_docked_docker, st_docker_sameVessel, st_disengage, st_disabled, st_preattached);
		on_construction_Detach = new KFSMEvent("OnConstructionDetach");
		on_construction_Detach.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_construction_Detach.GoToStateOnEvent = st_ready;
		on_construction_Detach.OnEvent = delegate
		{
		};
		fsm.AddEvent(on_construction_Detach, st_ready, st_acquire, st_acquire_dockee, st_docked_dockee, st_docked_docker, st_docker_sameVessel, st_disengage, st_disabled, st_preattached);
	}

	public void OnFSMStateChange(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		if (DebugFSMState)
		{
			Debug.LogFormat("[ModuleDockingNode]: Part:{0}-{1} FSM State Changed, Old State:{2} New State:{3} Event:{4}", base.part.partInfo.title, base.part.persistentId, oldStatea.name, newState.name, fsmEvent.name);
		}
	}

	public void OnFSMEventCalled(KFSMEvent fsmEvent)
	{
		if (DebugFSMState)
		{
			Debug.LogFormat("[ModuleDockingNode]: Part:{0}-{1} FSM Event Called, Event:{2}", base.part.partInfo.title, base.part.persistentId, fsmEvent.name);
		}
	}

	public void Update()
	{
		if (this == null || physicsLessMode)
		{
			return;
		}
		if (fsm != null && fsm.Started)
		{
			fsm.UpdateFSM();
			state = fsm.currentStateName;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (FlightGlobals.fetch.VesselTarget == this)
			{
				evtSetAsTarget.active = false;
				evtUnsetTarget.active = true;
				if (FlightGlobals.ActiveVessel == base.vessel)
				{
					FlightGlobals.fetch.SetVesselTarget(null);
				}
				else if ((FlightGlobals.ActiveVessel.transform.position - nodeTransform.position).sqrMagnitude > 40000f)
				{
					FlightGlobals.fetch.SetVesselTarget(base.vessel);
				}
			}
			else
			{
				evtSetAsTarget.active = true;
				evtUnsetTarget.active = false;
			}
		}
		if (partActionMenuOpen)
		{
			UpdatePAWUI();
		}
	}

	public void FixedUpdate()
	{
		if (physicsLessMode)
		{
			return;
		}
		if (fsm != null && fsm.Started && this != null)
		{
			fsm.FixedUpdateFSM();
		}
		if (canRotate && HighLogic.LoadedSceneIsFlight && !FlightDriver.Pause && !base.vessel.packed)
		{
			UpdateAlignmentRotation();
			if (IsRotating && hasEnoughResources)
			{
				CalculateResourceDrain();
			}
			else if (!hasEnoughResources && resHandler.HasEnoughResourcesToAutoStart())
			{
				hasEnoughResources = true;
			}
		}
	}

	public void LateUpdate()
	{
		if (!physicsLessMode && fsm != null && fsm.Started && this != null)
		{
			fsm.LateUpdateFSM();
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.onPartActionUIShown.Remove(OnPartMenuOpen);
		GameEvents.onPartActionUIDismiss.Remove(OnPartMenuClose);
		base.Fields["nodeIsLocked"].OnValueModified -= ModifyLocked;
	}

	public virtual void UpdatePAWUI()
	{
		if (HighLogic.LoadedSceneIsFlight && canRotate)
		{
			base.Fields["targetAngle"].guiActive = otherNode != null && sameVesselDockJoint == null;
			base.Fields["inverted"].guiActive = otherNode != null && sameVesselDockJoint == null;
			base.Fields["nodeIsLocked"].guiActive = otherNode != null && sameVesselDockJoint == null;
			base.Fields["targetAngle"].guiActiveEditor = otherNode != null && sameVesselDockJoint == null;
			base.Fields["inverted"].guiActiveEditor = otherNode != null && sameVesselDockJoint == null;
			base.Fields["nodeIsLocked"].guiActiveEditor = otherNode != null && sameVesselDockJoint == null;
		}
		else
		{
			base.Fields["targetAngle"].guiActive = false;
			base.Fields["inverted"].guiActive = false;
			base.Fields["nodeIsLocked"].guiActive = false;
			base.Fields["targetAngle"].guiActiveEditor = false;
			base.Fields["inverted"].guiActiveEditor = false;
			base.Fields["nodeIsLocked"].guiActiveEditor = otherNode != null && sameVesselDockJoint == null;
		}
	}

	public ModuleDockingNode FindNodeApproaches()
	{
		if (base.part.packed)
		{
			return null;
		}
		int count = FlightGlobals.VesselsLoaded.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.VesselsLoaded[count];
			if (vessel.packed)
			{
				continue;
			}
			int count2 = vessel.dockingPorts.Count;
			if (count2 == 0)
			{
				continue;
			}
			int index = count2;
			while (index-- > 0)
			{
				PartModule partModule = vessel.dockingPorts[index];
				if (partModule.part == base.part || partModule.part == null || partModule.part.State == PartStates.DEAD || !(partModule is ModuleDockingNode))
				{
					continue;
				}
				ModuleDockingNode moduleDockingNode = partModule as ModuleDockingNode;
				if (moduleDockingNode.state != st_ready.name)
				{
					continue;
				}
				bool flag = true;
				HashSet<string>.Enumerator enumerator = nodeTypes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					flag &= !moduleDockingNode.nodeTypes.Contains(enumerator.Current);
				}
				if (!flag && moduleDockingNode.gendered == gendered && (!gendered || moduleDockingNode.genderFemale != genderFemale) && moduleDockingNode.snapRotation == snapRotation && (!snapRotation || moduleDockingNode.snapOffset == snapOffset))
				{
					if (CheckDockContact(this, moduleDockingNode, acquireRange, acquireMinFwdDot, acquireMinRollDot))
					{
						Debug.DrawLine(nodeTransform.position, moduleDockingNode.nodeTransform.position, Color.green);
						return moduleDockingNode;
					}
					Debug.DrawLine(nodeTransform.position, moduleDockingNode.nodeTransform.position, Color.red);
				}
			}
		}
		return null;
	}

	public bool CheckDockContact(ModuleDockingNode m1, ModuleDockingNode m2, float minDist, float minFwdDot, float minRollDot)
	{
		if ((m1.nodeTransform.position - m2.nodeTransform.position).sqrMagnitude < minDist * minDist && Vector3.Dot(m1.nodeTransform.forward, -m2.nodeTransform.forward) > minFwdDot)
		{
			float num = Vector3.Dot(m1.nodeTransform.up, m2.nodeTransform.up);
			if (m1.snapRotation)
			{
				double num2 = Math.Acos(num);
				if (double.IsNaN(num2))
				{
					num2 = ((!(num > 0f)) ? Math.PI : 0.0);
				}
				else if (Vector3.Dot(m1.nodeTransform.up, m2.nodeTransform.right) > 0f)
				{
					num2 = Math.PI * 2.0 - num2;
				}
				double num3 = UtilMath.DegreesToRadians(m1.snapOffset);
				if (num3 > 0.0)
				{
					while (num2 > num3)
					{
						num2 -= num3;
					}
				}
				num = (float)Math.Cos(num2);
			}
			if (num > minRollDot)
			{
				return true;
			}
		}
		return false;
	}

	public void DockToVessel(ModuleDockingNode node)
	{
		Debug.Log("Docking to vessel " + node.vessel.GetDisplayName(), base.gameObject);
		vesselInfo = new DockedVesselInfo();
		vesselInfo.name = base.vessel.vesselName;
		vesselInfo.vesselType = base.vessel.vesselType;
		vesselInfo.rootPartUId = base.vessel.rootPart.flightID;
		node.vesselInfo = new DockedVesselInfo();
		node.vesselInfo.name = node.vessel.vesselName;
		node.vesselInfo.vesselType = node.vessel.vesselType;
		node.vesselInfo.rootPartUId = node.vessel.rootPart.flightID;
		Vessel vessel = base.vessel;
		uint data = base.vessel.persistentId;
		uint data2 = node.vessel.persistentId;
		GameEvents.onVesselDocking.Fire(data, data2);
		GameEvents.onActiveJointNeedUpdate.Fire(node.vessel);
		GameEvents.onActiveJointNeedUpdate.Fire(base.vessel);
		node.vessel.SetRotation(node.vessel.transform.rotation);
		base.vessel.SetRotation(Quaternion.FromToRotation(nodeTransform.forward, -node.nodeTransform.forward) * base.vessel.transform.rotation);
		base.vessel.SetPosition(base.vessel.transform.position - (nodeTransform.position - node.nodeTransform.position), usePristineCoords: true);
		base.vessel.IgnoreGForces(10);
		base.part.Couple(node.part);
		GameEvents.onVesselPersistentIdChanged.Fire(data, data2);
		if (vessel == FlightGlobals.ActiveVessel)
		{
			FlightGlobals.ForceSetActiveVessel(base.vessel);
			FlightInputHandler.SetNeutralControls();
		}
		else if (base.vessel == FlightGlobals.ActiveVessel)
		{
			base.vessel.MakeActive();
			FlightInputHandler.SetNeutralControls();
		}
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			FlightGlobals.PersistentLoadedPartIds.Add(vessel.parts[i].persistentId, vessel.parts[i]);
			if (vessel.parts[i].protoPartSnapshot != null)
			{
				FlightGlobals.PersistentUnloadedPartIds.Add(vessel.parts[i].protoPartSnapshot.persistentId, vessel.parts[i].protoPartSnapshot);
			}
		}
		GameEvents.onVesselWasModified.Fire(base.vessel);
		GameEvents.onDockingComplete.Fire(new GameEvents.FromToAction<Part, Part>(base.part, node.part));
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_8002396")]
	public void MakePrimary()
	{
		if ((bool)otherNode)
		{
			otherNode.fsm.RunEvent(otherNode.on_swapSecondary);
			fsm.RunEvent(on_swapPrimary);
		}
	}

	[KSPAction("#autoLOC_6001444", activeEditor = false)]
	public void UndockAction(KSPActionParam param)
	{
		if (base.Events["Undock"].active)
		{
			Undock();
		}
		if (base.Events["UndockSameVessel"].active)
		{
			UndockSameVessel();
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001445")]
	public void Undock()
	{
		Part parent = base.part.parent;
		Vessel vessel = base.vessel;
		uint referenceTransformId = base.vessel.referenceTransformId;
		sameVesselUndockNode = this;
		sameVesselUndockOtherNode = otherNode;
		if (undockPreAttached)
		{
			Decouple();
			fsm.RunEvent(on_undock);
			if (otherNode != null)
			{
				otherNode.OnOtherNodeUndock();
			}
			undockPreAttached = false;
			return;
		}
		if (parent != otherNode.part)
		{
			otherNode.Undock();
			return;
		}
		base.part.Undock(vesselInfo);
		base.part.AddForce(nodeTransform.forward * ((0f - undockEjectionForce) * 0.5f));
		parent.AddForce(nodeTransform.forward * (undockEjectionForce * 0.5f));
		if (vessel == FlightGlobals.ActiveVessel && vessel[referenceTransformId] == null)
		{
			StartCoroutine(WaitAndSwitchFocus());
		}
		fsm.RunEvent(on_undock);
		otherNode.OnOtherNodeUndock();
	}

	public IEnumerator WaitAndSwitchFocus()
	{
		yield return null;
		FlightGlobals.ForceSetActiveVessel(base.vessel);
		FlightInputHandler.SetNeutralControls();
	}

	public void OnOtherNodeUndock()
	{
		fsm.RunEvent(on_undock);
	}

	public void UpdateAlignmentRotation()
	{
		if (!canRotate || !hasEnoughResources || !rotationInitComplete || RotationJoint == null || targetAngle < hardMinMaxLimits.x || !(targetAngle <= hardMinMaxLimits.y))
		{
			return;
		}
		maxAnglePerFrame = traverseVelocity * Time.fixedDeltaTime;
		if (!driveTargetAngle.Equals(JointTargetAngle))
		{
			if (!IsRotating)
			{
				IsRotating = true;
				SetJointHighLowLimits();
			}
			driveTargetAngle = Mathf.MoveTowards(driveTargetAngle, JointTargetAngle, maxAnglePerFrame);
			visualTargetAngle = Mathf.MoveTowards(visualTargetAngle, VisualTargetAngle, maxAnglePerFrame);
			if (RotationJoint == base.part.attachJoint.Joint)
			{
				if (!partJointUnbreakable)
				{
					RotationJoint.breakTorque *= 10f;
					RotationJoint.breakForce *= 10f;
					partJointUnbreakable = true;
				}
				Quaternion targetLocalRotation = SetTargetRotation(Quaternion.identity, driveTargetAngle - cachedInitialAngle, setRotation: true, Vector3.up);
				RotationJoint.SetTargetRotationLocal(targetLocalRotation, Quaternion.identity);
				ApplyCoordsUpdate();
				targetRotation = SetTargetRotation(Quaternion.Euler(initialRotation), visualTargetAngle, setRotation: false);
				rotationTransform.localRotation = targetRotation;
			}
			else
			{
				targetRotation = SetTargetRotation(Quaternion.Euler(initialRotation), 0f - visualTargetAngle, setRotation: false);
				rotationTransform.localRotation = targetRotation;
			}
		}
		else
		{
			IsRotating = false;
			if (partJointUnbreakable)
			{
				RotationJoint.breakTorque /= 10f;
				RotationJoint.breakForce /= 10f;
				partJointUnbreakable = false;
			}
		}
	}

	public Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation)
	{
		return SetTargetRotation(startingRotation, rotationAngle, setRotation, GetRotationAxis());
	}

	public Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation, Vector3 rotationAxis)
	{
		Quaternion quaternion = startingRotation;
		if (setRotation)
		{
			return Quaternion.AngleAxis(rotationAngle, rotationAxis);
		}
		return quaternion * Quaternion.AngleAxis(rotationAngle, rotationAxis);
	}

	public Vector3 GetRotationAxis()
	{
		Vector3 zero = Vector3.zero;
		return rotationAxis switch
		{
			"Z-" => Vector3.back, 
			"Y" => Vector3.up, 
			"Y-" => Vector3.down, 
			"X" => Vector3.right, 
			"X-" => Vector3.left, 
			_ => Vector3.forward, 
		};
	}

	public virtual void CalculateResourceDrain()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			float num = motorRate;
			if ((double)Math.Abs(num) < 0.001)
			{
				num = 0f;
			}
			double rateMultiplier;
			double rateMultiplier2;
			if (num >= 0f)
			{
				rateMultiplier = num;
				rateMultiplier2 = 0.0;
			}
			else
			{
				rateMultiplier = 0.0;
				rateMultiplier2 = 0f - num;
			}
			string error = "";
			hasEnoughResources = resHandler.UpdateModuleResourceInputs(ref error, useFlowMode: true, rateMultiplier, 0.999, returnOnFirstLack: true, stringOps: false);
			resHandler.UpdateModuleResourceOutputs(rateMultiplier2);
			if (!hasEnoughResources)
			{
				num = 0f;
				IsRotating = false;
			}
		}
	}

	public void SetJointHighLowLimits()
	{
		if (RotationJoint != null)
		{
			lowerRotationLimit = RotationJoint.lowAngularXLimit;
			upperRotationLimit = RotationJoint.highAngularXLimit;
			RotationJoint.angularXMotion = ConfigurableJointMotion.Limited;
			lowerRotationLimit.limit = hardMinMaxLimits.x * 2f - 5f;
			upperRotationLimit.limit = hardMinMaxLimits.y * 2f + 5f;
			RotationJoint.lowAngularXLimit = lowerRotationLimit;
			RotationJoint.highAngularXLimit = upperRotationLimit;
		}
	}

	[ContextMenu("Apply Coords Update")]
	public virtual void ApplyCoordsUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && canRotate && !nodeIsLocked && otherNode != null && base.vessel != null && base.part.vessel != null)
		{
			RecurseCoordUpdate(base.part.vessel.rootPart, base.part.vessel.rootPart);
		}
	}

	public virtual void RecurseCoordUpdate(Part p, Part rootPart)
	{
		if (!(p == null) && !(rootPart == null))
		{
			p.UpdateOrgPosAndRot(rootPart);
			for (int i = 0; i < p.children.Count; i++)
			{
				RecurseCoordUpdate(p.children[i], rootPart);
			}
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public ModuleDockingNode GetDominantNode(ModuleDockingNode m1, ModuleDockingNode m2)
	{
		if (base.vessel.parts.IndexOf(m1.part) > base.vessel.parts.IndexOf(m2.part))
		{
			return m1;
		}
		return m2;
	}

	public void DockToSameVessel(ModuleDockingNode node)
	{
		sameVesselDockJoint = PartJoint.Create(base.part, node.part, referenceNode, node.referenceNode, node.part.attachMode);
		GameEvents.onSameVesselDock.Fire(new GameEvents.FromToAction<ModuleDockingNode, ModuleDockingNode>(this, node));
	}

	public void DestroySameVesselJoint()
	{
		if (sameVesselDockJoint != null)
		{
			sameVesselDockJoint.DestroyJoint();
		}
		if (!sameVesselUndockRedock)
		{
			otherNode.OnOtherNodeUndock();
			GameEvents.onSameVesselUndock.Fire(new GameEvents.FromToAction<ModuleDockingNode, ModuleDockingNode>(this, otherNode));
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001445")]
	public void UndockSameVessel()
	{
		fsm.RunEvent(on_undock);
		otherNode.OnOtherNodeUndock();
	}

	public void OnOtherNodeSameVesselDisconnect()
	{
		fsm.RunEvent(on_nodeDistance);
	}

	public bool NodeIsTooFar()
	{
		if (otherNode == null)
		{
			Debug.LogWarning("[DockingNode]: other node is null!", base.gameObject);
			return true;
		}
		if (otherNode.nodeTransform == null)
		{
			Debug.LogWarning("[DockingNode]: other node transform is null!", base.gameObject);
			return true;
		}
		return (nodeTransform.position - otherNode.nodeTransform.position).sqrMagnitude > minDistanceToReEngage * minDistanceToReEngage;
	}

	[KSPAction("#autoLOC_6001446", noLongerAssignable = true, activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		if (base.Events["Decouple"].active)
		{
			Decouple();
		}
	}

	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001446")]
	public void Decouple()
	{
		Part attachedPart = referenceNode.attachedPart;
		if (!(attachedPart == null))
		{
			sameVesselUndockNode = this;
			sameVesselUndockOtherNode = otherNode;
			if (referenceNode.attachedPart == base.part.parent)
			{
				base.part.decouple();
			}
			else
			{
				referenceNode.attachedPart.decouple();
			}
			base.part.AddForce(nodeTransform.forward * ((0f - undockEjectionForce) * 0.5f));
			attachedPart.AddForce(nodeTransform.forward * (undockEjectionForce * 0.5f));
			fsm.RunEvent(on_undock);
			if ((bool)otherNode)
			{
				otherNode.OnOtherNodeUndock();
			}
			if (stagingEnabled)
			{
				SetStaging(newValue: false);
			}
		}
	}

	public override void OnActive()
	{
		if (staged && stagingEnabled)
		{
			Decouple();
		}
	}

	public void OnConstructionAttach()
	{
		if (fsm.Started)
		{
			fsm.RunEvent(on_construction_Attach);
		}
	}

	public Transform GetTransform()
	{
		return nodeTransform;
	}

	public Vector3 GetObtVelocity()
	{
		return base.vessel.obt_velocity;
	}

	public Vector3 GetSrfVelocity()
	{
		return base.vessel.srf_velocity;
	}

	public Vector3 GetFwdVector()
	{
		return nodeTransform.forward;
	}

	public Vessel GetVessel()
	{
		return base.vessel;
	}

	public string GetName()
	{
		return Localizer.Format("#autoLOC_241797", base.vessel.vesselName);
	}

	public string GetDisplayName()
	{
		return GetName();
	}

	public Orbit GetOrbit()
	{
		return base.vessel.orbit;
	}

	public OrbitDriver GetOrbitDriver()
	{
		return base.vessel.orbitDriver;
	}

	public VesselTargetModes GetTargetingMode()
	{
		return VesselTargetModes.DirectionVelocityAndOrientation;
	}

	public bool GetActiveTargetable()
	{
		return false;
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = 200f, guiName = "#autoLOC_6001448")]
	public void SetAsTarget()
	{
		FlightGlobals.fetch.SetVesselTarget(this);
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = 200f, guiName = "#autoLOC_6001449")]
	public void UnsetTarget()
	{
		FlightGlobals.fetch.SetVesselTarget(null);
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236028")]
	public void EnableXFeed()
	{
		base.Events["EnableXFeed"].active = false;
		base.Events["DisableXFeed"].active = true;
		bool fuelCrossFeed = base.part.fuelCrossFeed;
		Part obj = base.part;
		crossfeed = true;
		obj.fuelCrossFeed = true;
		if (fuelCrossFeed != crossfeed)
		{
			GameEvents.onPartCrossfeedStateChange.Fire(base.part);
		}
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236030")]
	public void DisableXFeed()
	{
		base.Events["EnableXFeed"].active = true;
		base.Events["DisableXFeed"].active = false;
		bool fuelCrossFeed = base.part.fuelCrossFeed;
		Part obj = base.part;
		crossfeed = false;
		obj.fuelCrossFeed = false;
		if (fuelCrossFeed != crossfeed)
		{
			GameEvents.onPartCrossfeedStateChange.Fire(base.part);
		}
	}

	[KSPAction("#autoLOC_6001447")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		MakeReferenceTransform();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001447")]
	public void MakeReferenceTransform()
	{
		base.part.SetReferenceTransform(controlTransform);
		base.vessel.SetReferenceTransform(base.part);
	}

	[KSPAction("#autoLOC_236028")]
	public void EnableXFeedAction(KSPActionParam param)
	{
		EnableXFeed();
	}

	[KSPAction("#autoLOC_236030")]
	public void DisableXFeedAction(KSPActionParam param)
	{
		DisableXFeed();
	}

	[KSPAction("#autoLOC_236032")]
	public void ToggleXFeedAction(KSPActionParam param)
	{
		if (crossfeed)
		{
			DisableXFeed();
		}
		else
		{
			EnableXFeed();
		}
	}

	public override string GetInfo()
	{
		return string.Concat("" + Localizer.Format("#autoLOC_241887", captureRange.ToString("0.0###")), Localizer.Format("#autoLOC_241888", undockEjectionForce.ToString("0.0###")));
	}

	public int GetStageIndex(int fallback)
	{
		return fallback + 1;
	}

	public override bool IsStageable()
	{
		return staged;
	}

	public override bool StagingEnabled()
	{
		if (stagingEnabled)
		{
			return staged;
		}
		return false;
	}

	public override bool StagingToggleEnabledEditor()
	{
		return staged;
	}

	public override bool StagingToggleEnabledFlight()
	{
		return staged;
	}

	public override string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return Localizer.Format("#autoLOC_241913");
	}

	public override string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return Localizer.Format("#autoLOC_241914");
	}

	public bool CrossfeedToggleableEditor()
	{
		return true;
	}

	public bool CrossfeedToggleableFlight()
	{
		return true;
	}

	public string GetContractObjectiveType()
	{
		return "Dock";
	}

	public bool CheckContractObjectiveValidity()
	{
		return true;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterDockingNodeBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterDockingNodeBase item = adjuster as AdjusterDockingNodeBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public bool IsAdjusterBlockingUndock()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsBlockingUndock())
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

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003039");
	}

	public bool IsJointUnlocked()
	{
		if ((otherNode == null && sameVesselDockJoint == null) || !canRotate)
		{
			return false;
		}
		return !nodeIsLocked;
	}
}
