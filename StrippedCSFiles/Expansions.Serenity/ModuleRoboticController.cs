using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticController : PartModule, IShipConstructIDChanges, IResourceConsumer
{
	public enum SequenceLoopOptions
	{
		Once,
		Repeat,
		PingPong,
		OnceRestart
	}

	public enum SequenceDirectionOptions
	{
		Forward,
		Reverse
	}

	private List<PartResourceDefinition> consumedResources;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003263")]
	public string displayName;

	[UI_Label]
	[KSPField(guiName = "#autoLOC_8003264")]
	protected int fieldsCount;

	[UI_Label]
	[KSPField(guiName = "#autoLOC_8003348")]
	protected int actionsCount;

	[KSPAxisField(unfocusedRange = 5f, isPersistant = true, guiActiveUnfocused = true, incrementalSpeed = 1f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, guiName = "#autoLOC_8003265", guiUnits = "s")]
	[UI_FloatRange(affectSymCounterparts = UI_Scene.None)]
	protected float sequencePosition;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
	[KSPAxisField(unfocusedRange = 5f, incrementalSpeed = 20f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, minValue = 0f, guiFormat = "F0", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_8003329", guiUnits = "%")]
	protected float sequencePlaySpeed;

	[UI_Label]
	[KSPField(guiFormat = "F1", isPersistant = true, guiName = "#autoLOC_8003266", guiUnits = "s")]
	protected float sequenceLength;

	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_6001072")]
	[UI_Toggle(disabledText = "#autoLOC_8005004", scene = UI_Scene.All, enabledText = "#autoLOC_8005003", affectSymCounterparts = UI_Scene.None)]
	public bool controllerEnabled;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003267")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003270", "#autoLOC_8003271" }, affectSymCounterparts = UI_Scene.None)]
	protected int sequencePlayingIndex;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003268")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003272", "#autoLOC_8003273" }, affectSymCounterparts = UI_Scene.None)]
	protected int sequenceDirectionIndex;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003274", "#autoLOC_8003275", "#autoLOC_8003276", "#autoLOC_8003620" }, affectSymCounterparts = UI_Scene.None)]
	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003269")]
	protected int sequenceLoopIndex;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5f, minValue = 1f)]
	[KSPField(unfocusedRange = 5f, guiActiveUnfocused = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003349")]
	protected float priorityField;

	protected int priority;

	[KSPField(isPersistant = true)]
	private Vector2 windowPosition;

	[KSPField(isPersistant = true)]
	private Vector2 windowSize;

	private string consumptionString;

	[SerializeField]
	private List<ControlledAxis> controlledAxes;

	[SerializeField]
	private List<ControlledAction> controlledActions;

	internal RoboticControllerWindow window;

	private bool hasEnoughResources;

	private bool leavingScene;

	private UI_FloatRange sequencePositionUIField;

	private UI_Cycle sequencePlayingIndexUIField;

	private UI_Cycle sequenceDirectionIndexUIField;

	private UI_Cycle sequenceLoopIndexUIField;

	private BaseAxisField sequenceAxisPositionAxis;

	private float updateTime;

	private float actionCheck1StartTime;

	private float actionCheck1EndTime;

	private float actionCheck2StartTime;

	private float actionCheck2EndTime;

	private KSPActionParam controllerActionParam;

	private List<BaseAction> actionsToFire;

	private bool pausedInWarp;

	private Keyframe resizeCache;

	private bool sequenceWasPlaying;

	public int Priority
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector2 WindowPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector2 WindowSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasWindowDimensions
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

	public uint PartPersistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ControlledAxis> ControlledAxes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Please use ControlledAxes going forward as Axes will be removed at some point")]
	public List<ControlledAxis> Axes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ControlledAction> ControlledActions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float SequencePosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float SequenceLength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float SequencePlaySpeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool SequenceIsPlaying
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

	public SequenceDirectionOptions SequenceDirection
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

	public SequenceLoopOptions SequenceLoop
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRoboticController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003277")]
	protected void TogglePlayAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003283")]
	protected void ToggleLoopModeAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003284")]
	protected void ToggleDirectionAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8014149")]
	protected void ToggleControllerEnabledAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8014150")]
	protected void ToggleControllerEnabledOn(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8014151")]
	protected void ToggleControllerEnabledOff(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003306")]
	protected void PlaySequenceAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003307")]
	protected void StopSequenceAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003308")]
	protected void SequenceForwardAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003309")]
	protected void SequenceReverseAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003310")]
	protected void SequenceLoopOnceAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003311")]
	protected void SequenceLoopRepeatAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003312")]
	protected void SequenceLoopPingPongAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003621")]
	protected void SequenceLoopOnceRestartAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003331")]
	protected void SequencePlaySpeedZeroAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003332")]
	protected void SequencePlaySpeedFullAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003278")]
	public void OpenControllerEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePlayingSequencePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckAndFireActions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWarpRateChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleControllerEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleControllerEnabled(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TogglePlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TogglePlay(bool play)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SequencePlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SequenceStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDirection(SequenceDirectionOptions newDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CycleLoopMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLoopMode(SequenceLoopOptions newMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDisplayName(string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLength(float newLength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLength(float newLength, bool maintainPointsTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSequencePosition(float newPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPlaySpeed(float newSpeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSequencePositionStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSequencePositionEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSequencePositionPrevKey()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSequencePositionNextKey()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartActionUIShown(UIPartActionWindow window, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartActionUICreate(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSequenceSliderInteraction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sequenceFieldChanged(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PriorityChanged(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sequenceAxisPositionChanged(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IShipConstructIDChanges.UpdatePersistentIDs(Dictionary<uint, uint> changedIDs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorPartEvent(ConstructionEventType evt, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartWillDie(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetWindowSizeAndPosition(RectTransform windowRect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartAxis(Part part, PartModule module, BaseAxisField axisField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartAction(Part part, PartModule module, BaseAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PartDeleted(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartAxis(Part part, BaseAxisField axisField, bool transferToSymPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartAction(Part part, BaseAction action, bool transferToSymPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartAxis(ControlledAxis axis, bool transferToSymPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartAction(ControlledAction action, bool transferToSymPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPartAxisForSymmetryPartner(ControlledAxis oldAxis, Part newPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPartActionForSymmetryPartner(ControlledAction oldAction, Part newPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPartAxisField(Part testPart, BaseAxisField testAxisField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPartSymmetryAxisField(Part testPart, BaseAxisField testAxisField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetPartAxisField(Part testPart, BaseAxisField testAxisField, out ControlledAxis axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetPartAxisField(Part testPart, BaseAxisField testAction, out ControlledAxis action, out bool symmetryPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPartAction(Part testPart, BaseAction testAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetPartAction(Part testPart, BaseAction testAction, out ControlledAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPartSymmetryAction(Part testPart, BaseAction testAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetPartAction(Part testPart, BaseAction testAction, out ControlledAction action, out bool symmetryPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPart(List<Part> testParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPart(Part testPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePartSymmetry(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartSymmetry(uint oldPartId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isSameActionId(Part testPart, ControlledAction cAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselModified(Vessel modifiedVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveOtherVesselItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Assign Ref Vars")]
	private void AssignReferenceVars_Debug()
	{
		throw null;
	}
}
