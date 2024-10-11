using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleDockingNode : PartModule, ITargetable, IStageSeparator, IContractObjectiveModule, IResourceConsumer, IJointLockState
{
	[CompilerGenerated]
	private sealed class _003ClateFSMStart_003Ed__114 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleDockingNode _003C_003E4__this;

		public StartState st;

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
		public _003ClateFSMStart_003Ed__114(int _003C_003E1__state)
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
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitAndSwitchFocus_003Ed__158 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleDockingNode _003C_003E4__this;

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
		public _003CWaitAndSwitchFocus_003Ed__158(int _003C_003E1__state)
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
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[KSPField]
	public string nodeTransformName;

	[KSPField]
	public string controlTransformName;

	[KSPField]
	public float undockEjectionForce;

	[KSPField]
	public float minDistanceToReEngage;

	[KSPField]
	public float acquireRange;

	[KSPField]
	public float acquireMinFwdDot;

	[KSPField]
	public float acquireMinRollDot;

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002397", guiUnits = "%")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 5f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float acquireForceTweak;

	[KSPField]
	public float acquireForce;

	[KSPField]
	public float acquireTorque;

	[KSPField]
	public float acquireTorqueRoll;

	[KSPField]
	public float captureRange;

	[KSPField]
	public float captureMinFwdDot;

	[KSPField]
	public float captureMinRollDot;

	[KSPField]
	public float captureMaxRvel;

	[KSPField]
	public string referenceAttachNode;

	[KSPField]
	public bool useReferenceAttachNode;

	[KSPField]
	public string nodeType;

	[KSPField]
	public int deployAnimationController;

	[KSPField]
	public float deployAnimationTarget;

	[KSPField]
	public bool animReadyEnter;

	[KSPField]
	public bool animReadyExit;

	[KSPField]
	public bool animDisengageEnter;

	[KSPField]
	public bool animDisengageExit;

	[KSPField]
	public bool animDisabledEnter;

	[KSPField]
	public bool animDisabledExit;

	[KSPField]
	public bool animDisableIfNot1;

	[KSPField]
	public bool animEnableIf1;

	[KSPField]
	public bool animCaptureOff;

	[KSPField]
	public bool animUndockOn;

	[KSPField]
	public bool setAnimWrite;

	[KSPField]
	public bool gendered;

	[KSPField]
	public bool genderFemale;

	[KSPField]
	public bool snapRotation;

	[KSPField]
	public float snapOffset;

	[KSPField(isPersistant = true)]
	public bool crossfeed;

	[KSPField]
	public bool canRotate;

	[KSPField]
	public string rotationTransformName;

	private Transform rotationTransform;

	[KSPField]
	public string rotationAxis;

	private Vector3 initialRotation;

	private float cachedInitialAngle;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float traverseVelocity;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public Vector2 hardMinMaxLimits;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float efficiency;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float maxMotorOutput;

	[KSPField]
	public float baseResourceConsumptionRate;

	[KSPField]
	public float referenceConsumptionVelocity;

	protected float motorRate;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002700")]
	public bool nodeIsLocked;

	[KSPAxisField(isPersistant = true, incrementalSpeed = 30f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = false, guiActive = false, ignoreClampWhenIncremental = true, guiName = "#autoLOC_8014167")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 180f, minValue = -179.99f, affectSymCounterparts = UI_Scene.Editor)]
	public float targetAngle;

	private Quaternion targetRotation;

	[KSPField(advancedTweakable = false, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003304")]
	[UI_Toggle(disabledText = "#autoLOC_8003303", enabledText = "#autoLOC_8003302", tipText = "#autoLOC_8003301", affectSymCounterparts = UI_Scene.None)]
	public bool inverted;

	private float workingJointTargetAngle;

	[SerializeField]
	protected SoftJointLimit upperRotationLimit;

	[SerializeField]
	protected SoftJointLimit lowerRotationLimit;

	protected UI_FloatRange targetAngleUIField;

	protected BaseAxisField targetAngleAxisField;

	private bool rotationInitComplete;

	private bool partJointUnbreakable;

	public DockedVesselInfo vesselInfo;

	public string state;

	public Transform nodeTransform;

	public Transform controlTransform;

	public double TatUndock;

	public uint dockedPartUId;

	public int dockingNodeModuleIndex;

	public ModuleDockingNode otherNode;

	public HashSet<string> nodeTypes;

	public ModuleAnimateGeneric deployAnimator;

	public AttachNode referenceNode;

	protected bool setStagingState;

	private ModuleDockingNode sameVesselUndockNode;

	private ModuleDockingNode sameVesselUndockOtherNode;

	private bool sameVesselUndockRedock;

	public bool DebugFSMState;

	private bool partActionMenuOpen;

	private bool undockPreAttached;

	private bool physicsLessMode;

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

	protected bool hasEnoughResources;

	private List<PartResourceDefinition> consumedResources;

	[KSPField]
	public bool staged;

	private List<AdjusterDockingNodeBase> adjusterCache;

	public bool IsRotating
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

	public float JointTargetAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private float VisualTargetAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigurableJoint RotationJoint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private bool RotationJointHost
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsDisabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDockingNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002701", advancedTweakable = true)]
	protected void ToggleLockedAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002702", advancedTweakable = true)]
	protected void ServoEngageLockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002703", advancedTweakable = true)]
	protected void ServoDisgageLockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool EngageNodeLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisengageNodeLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyLocked(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PromoteToPhysicalPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartMenuClose(Part inpPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDockingNode FindOtherNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003ClateFSMStart_003Ed__114))]
	public IEnumerator lateFSMStart(StartState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFSMStateChange(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFSMEventCalled(KFSMEvent fsmEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePAWUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDockingNode FindNodeApproaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckDockContact(ModuleDockingNode m1, ModuleDockingNode m2, float minDist, float minFwdDot, float minRollDot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DockToVessel(ModuleDockingNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_8002396")]
	public void MakePrimary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001444", activeEditor = false)]
	public void UndockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001445")]
	public void Undock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndSwitchFocus_003Ed__158))]
	public IEnumerator WaitAndSwitchFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnOtherNodeUndock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateAlignmentRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation, Vector3 rotationAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 GetRotationAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetJointHighLowLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Apply Coords Update")]
	protected virtual void ApplyCoordsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RecurseCoordUpdate(Part p, Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDockingNode GetDominantNode(ModuleDockingNode m1, ModuleDockingNode m2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DockToSameVessel(ModuleDockingNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroySameVesselJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001445")]
	public void UndockSameVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnOtherNodeSameVesselDisconnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NodeIsTooFar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001446", noLongerAssignable = true, activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001446")]
	public void Decouple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnConstructionAttach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetObtVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetSrfVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetFwdVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit GetOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitDriver GetOrbitDriver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselTargetModes GetTargetingMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetActiveTargetable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = 200f, guiName = "#autoLOC_6001448")]
	public void SetAsTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = 200f, guiName = "#autoLOC_6001449")]
	public void UnsetTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236028")]
	public void EnableXFeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236030")]
	public void DisableXFeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001447")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001447")]
	public void MakeReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236028")]
	public void EnableXFeedAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236030")]
	public void DisableXFeedAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236032")]
	public void ToggleXFeedAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageIndex(int fallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingEnableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingDisableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedToggleableEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedToggleableFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckContractObjectiveValidity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsAdjusterBlockingUndock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsJointUnlocked()
	{
		throw null;
	}
}
