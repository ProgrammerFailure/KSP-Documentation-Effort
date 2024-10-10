using System;
using System.Collections.Generic;
using ns9;
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

	public List<PartResourceDefinition> consumedResources;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003263")]
	public string displayName;

	[UI_Label]
	[KSPField(guiName = "#autoLOC_8003264")]
	public int fieldsCount;

	[UI_Label]
	[KSPField(guiName = "#autoLOC_8003348")]
	public int actionsCount;

	[KSPAxisField(unfocusedRange = 5f, isPersistant = true, guiActiveUnfocused = true, incrementalSpeed = 1f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, guiName = "#autoLOC_8003265", guiUnits = "s")]
	[UI_FloatRange(affectSymCounterparts = UI_Scene.None)]
	public float sequencePosition;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.None)]
	[KSPAxisField(unfocusedRange = 5f, incrementalSpeed = 20f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, minValue = 0f, guiFormat = "F0", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_8003329", guiUnits = "%")]
	public float sequencePlaySpeed = 100f;

	[UI_Label]
	[KSPField(guiFormat = "F1", isPersistant = true, guiName = "#autoLOC_8003266", guiUnits = "s")]
	public float sequenceLength = 5f;

	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_6001072")]
	[UI_Toggle(disabledText = "#autoLOC_8005004", scene = UI_Scene.All, enabledText = "#autoLOC_8005003", affectSymCounterparts = UI_Scene.None)]
	public bool controllerEnabled = true;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003267")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003270", "#autoLOC_8003271" }, affectSymCounterparts = UI_Scene.None)]
	public int sequencePlayingIndex = 1;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003268")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003272", "#autoLOC_8003273" }, affectSymCounterparts = UI_Scene.None)]
	public int sequenceDirectionIndex;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_8003274", "#autoLOC_8003275", "#autoLOC_8003276", "#autoLOC_8003620" }, affectSymCounterparts = UI_Scene.None)]
	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003269")]
	public int sequenceLoopIndex;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5f, minValue = 1f)]
	[KSPField(unfocusedRange = 5f, guiActiveUnfocused = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003349")]
	public float priorityField = 3f;

	public int priority;

	[KSPField(isPersistant = true)]
	public Vector2 windowPosition;

	[KSPField(isPersistant = true)]
	public Vector2 windowSize;

	public string consumptionString;

	[SerializeField]
	public List<ControlledAxis> controlledAxes;

	[SerializeField]
	public List<ControlledAction> controlledActions;

	public RoboticControllerWindow window;

	public bool hasEnoughResources;

	public bool leavingScene;

	public UI_FloatRange sequencePositionUIField;

	public UI_Cycle sequencePlayingIndexUIField;

	public UI_Cycle sequenceDirectionIndexUIField;

	public UI_Cycle sequenceLoopIndexUIField;

	public BaseAxisField sequenceAxisPositionAxis;

	public float updateTime;

	public float actionCheck1StartTime;

	public float actionCheck1EndTime;

	public float actionCheck2StartTime;

	public float actionCheck2EndTime;

	public KSPActionParam controllerActionParam = new KSPActionParam(KSPActionGroup.None, KSPActionType.Toggle);

	public List<BaseAction> actionsToFire;

	public bool pausedInWarp;

	public Keyframe resizeCache;

	public bool sequenceWasPlaying;

	public int Priority => priority;

	public Vector2 WindowPosition => windowPosition;

	public Vector2 WindowSize => windowSize;

	public bool HasWindowDimensions { get; set; }

	public uint PartPersistentId
	{
		get
		{
			if (!(base.part == null))
			{
				return base.part.persistentId;
			}
			return 0u;
		}
	}

	public List<ControlledAxis> ControlledAxes => controlledAxes;

	[Obsolete("Please use ControlledAxes going forward as Axes will be removed at some point")]
	public List<ControlledAxis> Axes => controlledAxes;

	public List<ControlledAction> ControlledActions => controlledActions;

	public float SequencePosition => sequencePosition;

	public float SequenceLength => sequenceLength;

	public float SequencePlaySpeed => sequencePlaySpeed;

	public bool SequenceIsPlaying
	{
		get
		{
			return sequencePlayingIndex == 0;
		}
		set
		{
			sequencePlayingIndex = ((!value) ? 1 : 0);
		}
	}

	public SequenceDirectionOptions SequenceDirection
	{
		get
		{
			return (SequenceDirectionOptions)sequenceDirectionIndex;
		}
		set
		{
			sequenceDirectionIndex = (int)value;
		}
	}

	public SequenceLoopOptions SequenceLoop
	{
		get
		{
			return (SequenceLoopOptions)sequenceLoopIndex;
		}
		set
		{
			sequenceLoopIndex = (int)value;
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	[KSPAction("#autoLOC_8003277")]
	public void TogglePlayAction(KSPActionParam param)
	{
		TogglePlay();
	}

	[KSPAction("#autoLOC_8003283")]
	public void ToggleLoopModeAction(KSPActionParam param)
	{
		CycleLoopMode();
	}

	[KSPAction("#autoLOC_8003284")]
	public void ToggleDirectionAction(KSPActionParam param)
	{
		ToggleDirection();
	}

	[KSPAction("#autoLOC_8014149")]
	public void ToggleControllerEnabledAction(KSPActionParam param)
	{
		ToggleControllerEnabled();
	}

	[KSPAction("#autoLOC_8014150")]
	public void ToggleControllerEnabledOn(KSPActionParam param)
	{
		ToggleControllerEnabled(enabled: true);
	}

	[KSPAction("#autoLOC_8014151")]
	public void ToggleControllerEnabledOff(KSPActionParam param)
	{
		ToggleControllerEnabled(enabled: false);
	}

	[KSPAction("#autoLOC_8003306")]
	public void PlaySequenceAction(KSPActionParam param)
	{
		TogglePlay(play: true);
	}

	[KSPAction("#autoLOC_8003307")]
	public void StopSequenceAction(KSPActionParam param)
	{
		TogglePlay(play: false);
	}

	[KSPAction("#autoLOC_8003308")]
	public void SequenceForwardAction(KSPActionParam param)
	{
		SetDirection(SequenceDirectionOptions.Forward);
	}

	[KSPAction("#autoLOC_8003309")]
	public void SequenceReverseAction(KSPActionParam param)
	{
		SetDirection(SequenceDirectionOptions.Reverse);
	}

	[KSPAction("#autoLOC_8003310")]
	public void SequenceLoopOnceAction(KSPActionParam param)
	{
		SetLoopMode(SequenceLoopOptions.Once);
	}

	[KSPAction("#autoLOC_8003311")]
	public void SequenceLoopRepeatAction(KSPActionParam param)
	{
		SetLoopMode(SequenceLoopOptions.Repeat);
	}

	[KSPAction("#autoLOC_8003312")]
	public void SequenceLoopPingPongAction(KSPActionParam param)
	{
		SetLoopMode(SequenceLoopOptions.PingPong);
	}

	[KSPAction("#autoLOC_8003621")]
	public void SequenceLoopOnceRestartAction(KSPActionParam param)
	{
		SetLoopMode(SequenceLoopOptions.OnceRestart);
	}

	[KSPAction("#autoLOC_8003331")]
	public void SequencePlaySpeedZeroAction(KSPActionParam param)
	{
		SetPlaySpeed(0f);
	}

	[KSPAction("#autoLOC_8003332")]
	public void SequencePlaySpeedFullAction(KSPActionParam param)
	{
		SetPlaySpeed(100f);
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003278")]
	public void OpenControllerEditor()
	{
		RoboticControllerWindow.Spawn(this);
	}

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
			UnityEngine.Object.Destroy(this);
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		controlledAxes = new List<ControlledAxis>();
		controlledActions = new List<ControlledAction>();
		window = null;
		leavingScene = false;
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public void OnSceneLoadRequested(GameScenes scene)
	{
		leavingScene = true;
	}

	public void OnDestroy()
	{
		if (window != null)
		{
			window.CloseWindow();
		}
		GameEvents.onPartPersistentIdChanged.Remove(PartPersistentIdChanged);
		GameEvents.onEditorPartEvent.Remove(onEditorPartEvent);
		GameEvents.onPartWillDie.Remove(OnPartWillDie);
		GameEvents.onPartActionUIShown.Remove(onPartActionUIShown);
		GameEvents.onPartActionUICreate.Remove(onPartActionUICreate);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
		GameEvents.onTimeWarpRateChanged.Remove(OnWarpRateChanged);
		GameEvents.onVesselWasModified.Remove(VesselModified);
		base.Fields["sequencePosition"].OnValueModified -= sequenceFieldChanged;
		base.Fields["sequencePlaySpeed"].OnValueModified -= sequenceFieldChanged;
		base.Fields["sequencePlayingIndex"].OnValueModified -= sequenceFieldChanged;
		base.Fields["sequenceDirectionIndex"].OnValueModified -= sequenceFieldChanged;
		base.Fields["sequenceLoopIndex"].OnValueModified -= sequenceFieldChanged;
		base.Fields["priorityField"].OnValueModified -= PriorityChanged;
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (RoboticControllerManager.Instance == null && !HighLogic.LoadedSceneIsMissionBuilder)
		{
			Debug.LogError("[ModuleRoboticController]: There is no RoboticControllerManager running");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onPartPersistentIdChanged.Add(PartPersistentIdChanged);
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorPartEvent.Add(onEditorPartEvent);
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onPartWillDie.Add(OnPartWillDie);
		}
		GameEvents.onPartActionUIShown.Add(onPartActionUIShown);
		GameEvents.onPartActionUICreate.Add(onPartActionUICreate);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
		GameEvents.onTimeWarpRateChanged.Add(OnWarpRateChanged);
		GameEvents.onVesselWasModified.Add(VesselModified);
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("sequencePosition", out sequencePositionUIField);
		base.Fields.TryGetFieldUIControl<UI_Cycle>("sequencePlayingIndex", out sequencePlayingIndexUIField);
		base.Fields.TryGetFieldUIControl<UI_Cycle>("sequenceDirectionIndex", out sequenceDirectionIndexUIField);
		base.Fields.TryGetFieldUIControl<UI_Cycle>("sequenceLoopIndex", out sequenceLoopIndexUIField);
		sequenceAxisPositionAxis = base.Fields["sequencePosition"] as BaseAxisField;
		sequenceAxisPositionAxis.maxValue = sequenceLength;
		base.Fields["sequencePosition"].OnValueModified += sequenceFieldChanged;
		base.Fields["sequencePlaySpeed"].OnValueModified += sequenceFieldChanged;
		base.Fields["sequencePlayingIndex"].OnValueModified += sequenceFieldChanged;
		base.Fields["sequenceDirectionIndex"].OnValueModified += sequenceFieldChanged;
		base.Fields["sequenceLoopIndex"].OnValueModified += sequenceFieldChanged;
		base.Fields["priorityField"].OnValueModified += PriorityChanged;
		if (string.IsNullOrEmpty(displayName) && base.part != null && base.part.partInfo != null)
		{
			displayName = base.part.partInfo.title;
		}
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			if (!controlledAxes[count].AssignReferenceVars())
			{
				Debug.LogWarningFormat("[ModuleRoboticController]: Removing the axis we couldnt bind from the controller");
				controlledAxes.RemoveAt(count);
			}
		}
		int count2 = controlledActions.Count;
		while (count2-- > 0)
		{
			if (!controlledActions[count2].AssignReferenceVars())
			{
				Debug.LogWarningFormat("[ModuleRoboticController]: Removing the action we couldnt bind from the controller");
				controlledActions.RemoveAt(count2);
			}
		}
		PriorityChanged(null);
		UpdateUIElements();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!leavingScene && SequenceIsPlaying && !pausedInWarp)
		{
			UpdatePlayingSequencePosition();
		}
	}

	public void Update()
	{
		sequenceWasPlaying = SequenceIsPlaying;
		if (HighLogic.LoadedSceneIsEditor && SequenceIsPlaying)
		{
			UpdatePlayingSequencePosition();
		}
	}

	public void FixedUpdate()
	{
		if (SequenceIsPlaying && controllerEnabled)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				hasEnoughResources = true;
			}
			else
			{
				hasEnoughResources = resHandler.UpdateModuleResourceInputs(ref consumptionString, 1.0, 0.999, returnOnFirstLack: false, average: false, stringOps: true) > 0.0;
			}
		}
	}

	public void UpdatePlayingSequencePosition()
	{
		if (hasEnoughResources)
		{
			updateTime = Time.deltaTime;
			if (SequenceDirection == SequenceDirectionOptions.Reverse)
			{
				updateTime *= -1f;
			}
			updateTime *= sequencePlaySpeed * 0.01f;
			SetSequencePosition(SequencePosition + updateTime);
			CheckAndFireActions();
		}
	}

	public void CheckAndFireActions()
	{
		if (actionsToFire == null)
		{
			actionsToFire = new List<BaseAction>();
		}
		bool flag = false;
		for (int i = 0; i < controlledActions.Count; i++)
		{
			for (int j = 0; j < controlledActions[i].times.Count; j++)
			{
				flag = false;
				if (controlledActions[i].times[j] >= actionCheck1StartTime / SequenceLength && controlledActions[i].times[j] < actionCheck1EndTime / SequenceLength)
				{
					flag = true;
				}
				if (controlledActions[i].times[j] >= actionCheck2StartTime / SequenceLength && controlledActions[i].times[j] < actionCheck2EndTime / SequenceLength)
				{
					flag = true;
				}
				if ((SequenceLoop == SequenceLoopOptions.Once || SequenceLoop == SequenceLoopOptions.OnceRestart) && controlledActions[i].times[j] == actionCheck1EndTime / SequenceLength)
				{
					flag = true;
				}
				if (!flag)
				{
					continue;
				}
				actionsToFire.Add(controlledActions[i].Action);
				if (controlledActions[i].SymmetryActions != null)
				{
					for (int k = 0; k < controlledActions[i].SymmetryActions.Count; k++)
					{
						actionsToFire.Add(controlledActions[i].SymmetryActions[k]);
					}
				}
			}
		}
		int count = actionsToFire.Count;
		while (count-- > 0)
		{
			if (actionsToFire[count].active && (!actionsToFire[count].requireFullControl || !InputLockManager.IsLocked(ControlTypes.TWEAKABLES_FULLONLY)) && (actionsToFire[count].activeEditor || !HighLogic.LoadedSceneIsEditor))
			{
				actionsToFire[count].Invoke(controllerActionParam);
			}
		}
		if (actionsToFire.Count > 0)
		{
			actionsToFire.Clear();
		}
	}

	public void OnWarpRateChanged()
	{
		if (TimeWarp.CurrentRateIndex > 0 && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			pausedInWarp = true;
		}
		else if (pausedInWarp)
		{
			pausedInWarp = false;
		}
	}

	public void ToggleControllerEnabled()
	{
		ToggleControllerEnabled(!controllerEnabled);
	}

	public void ToggleControllerEnabled(bool enabled)
	{
		controllerEnabled = enabled;
	}

	public void TogglePlay()
	{
		TogglePlay(!SequenceIsPlaying);
	}

	public void TogglePlay(bool play)
	{
		if (!play)
		{
			SequenceStop();
		}
		else
		{
			SequencePlay();
		}
	}

	public void SequencePlay()
	{
		bool num = !SequenceIsPlaying;
		if (num && SequenceLoop == SequenceLoopOptions.OnceRestart && sequencePosition >= SequenceLength)
		{
			SetSequencePositionStart();
		}
		SequenceIsPlaying = true;
		if (!num)
		{
			GameEvents.onRoboticControllerSequencePlayed.Fire(this);
		}
		UpdateUIElements();
	}

	public void SequenceStop()
	{
		bool sequenceIsPlaying = SequenceIsPlaying;
		SequenceIsPlaying = false;
		if (!sequenceIsPlaying)
		{
			GameEvents.onRoboticControllerSequenceStopped.Fire(this);
		}
		UpdateUIElements();
	}

	public void ToggleDirection()
	{
		SequenceDirectionOptions sequenceDirection = SequenceDirection;
		if (sequenceDirection != 0 && sequenceDirection == SequenceDirectionOptions.Reverse)
		{
			SetDirection(SequenceDirectionOptions.Forward);
		}
		else
		{
			SetDirection(SequenceDirectionOptions.Reverse);
		}
	}

	public void SetDirection(SequenceDirectionOptions newDirection)
	{
		bool num = SequenceDirection != newDirection;
		SequenceDirection = newDirection;
		if (num)
		{
			GameEvents.onRoboticControllerSequenceDirectionChanged.Fire(this, SequenceDirection);
		}
		UpdateUIElements();
	}

	public void CycleLoopMode()
	{
		switch (SequenceLoop)
		{
		case SequenceLoopOptions.Once:
			SetLoopMode(SequenceLoopOptions.Repeat);
			break;
		case SequenceLoopOptions.Repeat:
			SetLoopMode(SequenceLoopOptions.PingPong);
			break;
		case SequenceLoopOptions.PingPong:
			SetLoopMode(SequenceLoopOptions.OnceRestart);
			break;
		default:
			SetLoopMode(SequenceLoopOptions.Once);
			break;
		}
		GameEvents.onRoboticControllerSequenceLoopModeChanged.Fire(this, SequenceLoop);
		UpdateUIElements();
	}

	public void SetLoopMode(SequenceLoopOptions newMode)
	{
		bool num = SequenceLoop != newMode;
		SequenceLoop = newMode;
		if (num)
		{
			GameEvents.onRoboticControllerSequenceLoopModeChanged.Fire(this, SequenceLoop);
		}
		UpdateUIElements();
	}

	public void SetDisplayName(string newName)
	{
		displayName = newName;
		UpdateUIElements();
	}

	public void SetLength(float newLength)
	{
		if (newLength < 0.01f)
		{
			newLength = 0.01f;
		}
		float num = SequencePosition;
		if (num > 0f)
		{
			num = num / sequenceLength * newLength;
		}
		sequenceLength = newLength;
		sequencePosition = num;
		sequenceAxisPositionAxis.maxValue = sequenceLength;
		UpdateUIElements();
	}

	public void SetLength(float newLength, bool maintainPointsTime)
	{
		if (maintainPointsTime)
		{
			for (int i = 0; i < ControlledAxes.Count; i++)
			{
				ControlledAxes[i].RescaleCurveTime(SequenceLength / newLength, 0.01f / newLength);
			}
			for (int j = 0; j < ControlledActions.Count; j++)
			{
				ControlledActions[j].RescaleTimes(SequenceLength / newLength, 0.01f / newLength);
			}
		}
		SetLength(newLength);
	}

	public void SetSequencePosition(float newPosition)
	{
		float num = -1f;
		actionCheck2EndTime = -1f;
		float num2 = num;
		num = -1f;
		actionCheck2StartTime = num2;
		float num3 = num;
		num = -1f;
		actionCheck1EndTime = num3;
		actionCheck1StartTime = num;
		if (!(newPosition > SequenceLength) && newPosition >= 0f)
		{
			if (SequenceDirection == SequenceDirectionOptions.Forward)
			{
				actionCheck1StartTime = SequencePosition;
				actionCheck1EndTime = newPosition;
			}
			else
			{
				actionCheck1StartTime = newPosition;
				actionCheck1EndTime = sequencePosition;
			}
		}
		else
		{
			switch (SequenceLoop)
			{
			case SequenceLoopOptions.Repeat:
				if (newPosition < 0f)
				{
					newPosition = SequenceLength + newPosition;
					actionCheck1StartTime = 0f;
					actionCheck1EndTime = SequencePosition;
					actionCheck2StartTime = newPosition;
					actionCheck2EndTime = SequenceLength + 1f;
				}
				else
				{
					newPosition -= SequenceLength;
					actionCheck1StartTime = 0f;
					actionCheck1EndTime = newPosition;
					actionCheck2StartTime = sequencePosition;
					actionCheck2EndTime = SequenceLength + 1f;
				}
				break;
			case SequenceLoopOptions.PingPong:
				if (newPosition < 0f)
				{
					newPosition = 0f - newPosition;
					actionCheck1StartTime = 0f;
					actionCheck1EndTime = sequencePosition;
					actionCheck2StartTime = 0f;
					actionCheck2EndTime = newPosition;
				}
				else
				{
					newPosition = SequenceLength - (newPosition - SequenceLength);
					actionCheck1StartTime = sequencePosition;
					actionCheck1EndTime = sequenceLength + 1f;
					actionCheck2StartTime = newPosition;
					actionCheck2EndTime = sequenceLength + 1f;
				}
				ToggleDirection();
				break;
			default:
				if (newPosition < 0f)
				{
					newPosition = 0f;
					actionCheck1StartTime = 0f;
					actionCheck1EndTime = sequencePosition;
				}
				else
				{
					newPosition = SequenceLength;
					actionCheck1StartTime = sequencePosition;
					actionCheck1EndTime = newPosition;
				}
				SequenceStop();
				break;
			}
		}
		newPosition = Mathf.Clamp(newPosition, 0f, SequenceLength);
		sequencePosition = newPosition;
		if (controllerEnabled)
		{
			for (int i = 0; i < controlledAxes.Count; i++)
			{
				ControlledAxis controlledAxis = controlledAxes[i];
				if (controlledAxis.AxisField == null)
				{
					continue;
				}
				ModuleRoboticController moduleRoboticController = controlledAxis.AxisField.module as ModuleRoboticController;
				if (moduleRoboticController != null)
				{
					float valueMultiple = moduleRoboticController.sequenceLength;
					if (controlledAxis.axisName.Equals("sequencePlaySpeed"))
					{
						valueMultiple = 100f;
					}
					controlledAxis.UpdateFieldValue(newPosition / SequenceLength * controlledAxis.timeValue.maxTime, valueMultiple);
				}
				else
				{
					controlledAxis.UpdateFieldValue(newPosition / SequenceLength * controlledAxis.timeValue.maxTime);
				}
			}
		}
		UpdateUIElements();
	}

	public void SetPlaySpeed(float newSpeed)
	{
		sequencePlaySpeed = Mathf.Clamp(newSpeed, 0f, 100f);
		UpdateUIElements();
	}

	public void SetSequencePositionStart()
	{
		SetSequencePosition(0f);
	}

	public void SetSequencePositionEnd()
	{
		SetSequencePosition(SequenceLength);
	}

	public void SetSequencePositionPrevKey()
	{
		float a = 0f;
		float num = 0f;
		for (int i = 0; i < controlledAxes.Count; i++)
		{
			ControlledAxis controlledAxis = controlledAxes[i];
			for (int j = 0; j < controlledAxis.timeValue.Curve.keys.Length; j++)
			{
				num = controlledAxis.timeValue.Curve.keys[j].time / controlledAxis.timeValue.maxTime * SequenceLength;
				if (num < SequencePosition)
				{
					a = Mathf.Max(a, num);
				}
			}
		}
		for (int k = 0; k < controlledActions.Count; k++)
		{
			ControlledAction controlledAction = controlledActions[k];
			for (int l = 0; l < controlledAction.times.Count; l++)
			{
				num = controlledAction.times[l] * SequenceLength;
				if (num < SequencePosition)
				{
					a = Mathf.Max(a, num);
				}
			}
		}
		SetSequencePosition(a);
	}

	public void SetSequencePositionNextKey()
	{
		float a = SequenceLength;
		float num = 0f;
		for (int i = 0; i < controlledAxes.Count; i++)
		{
			ControlledAxis controlledAxis = controlledAxes[i];
			for (int j = 0; j < controlledAxis.timeValue.Curve.keys.Length; j++)
			{
				num = controlledAxis.timeValue.Curve.keys[j].time / controlledAxis.timeValue.maxTime * SequenceLength;
				if (num > SequencePosition)
				{
					a = Mathf.Min(a, num);
				}
			}
		}
		for (int k = 0; k < controlledActions.Count; k++)
		{
			ControlledAction controlledAction = controlledActions[k];
			for (int l = 0; l < controlledAction.times.Count; l++)
			{
				num = controlledAction.times[l] * SequenceLength;
				if (num < SequencePosition)
				{
					a = Mathf.Min(a, num);
				}
			}
		}
		SetSequencePosition(a);
	}

	public void onPartActionUIShown(UIPartActionWindow window, Part part)
	{
		if (part.persistentId == base.part.persistentId)
		{
			UpdateUIElements();
		}
	}

	public void onPartActionUICreate(Part part)
	{
		if (part.persistentId == base.part.persistentId)
		{
			SetSequenceSliderInteraction();
		}
	}

	public void UpdateUIElements()
	{
		if (base.part.PartActionWindow != null)
		{
			fieldsCount = controlledAxes.Count;
			actionsCount = controlledActions.Count;
			if (sequencePositionUIField != null)
			{
				sequencePositionUIField.minValue = 0f;
				sequencePositionUIField.maxValue = sequenceLength;
				SetSequenceSliderInteraction();
			}
			base.part.PartActionWindow.UpdateWindow();
		}
		if (window != null)
		{
			window.OnControllerChanged();
		}
	}

	public void SetSequenceSliderInteraction()
	{
		if (sequencePositionUIField != null && sequencePositionUIField.partActionItem != null)
		{
			(sequencePositionUIField.partActionItem as UIPartActionFloatRange).slider.interactable = !SequenceIsPlaying;
		}
	}

	public void sequenceFieldChanged(object field)
	{
		if (!sequenceWasPlaying && SequenceIsPlaying && SequenceLoop == SequenceLoopOptions.OnceRestart && sequencePosition >= SequenceLength)
		{
			SetSequencePositionStart();
		}
		SetSequencePosition(sequencePosition);
	}

	public void PriorityChanged(object field)
	{
		priority = (int)Mathf.Clamp(priorityField, 1f, 5f);
	}

	public void sequenceAxisPositionChanged(object field)
	{
	}

	public void PartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		if (controlledAxes != null && controlledAxes.Count > 0)
		{
			for (int i = 0; i < controlledAxes.Count; i++)
			{
				if (controlledAxes[i].partId == oldId)
				{
					controlledAxes[i].partId = newId;
					controlledAxes[i].AssignReferenceVars();
				}
			}
		}
		if (controlledActions == null || controlledActions.Count <= 0)
		{
			return;
		}
		for (int j = 0; j < controlledActions.Count; j++)
		{
			if (controlledActions[j].partId == oldId)
			{
				controlledActions[j].partId = newId;
				controlledActions[j].AssignReferenceVars();
			}
		}
	}

	void IShipConstructIDChanges.UpdatePersistentIDs(Dictionary<uint, uint> changedIDs)
	{
		for (int i = 0; i < controlledAxes.Count; i++)
		{
			if (changedIDs.ContainsKey(controlledAxes[i].partId))
			{
				controlledAxes[i].partId = changedIDs[controlledAxes[i].partId];
			}
		}
		for (int j = 0; j < controlledActions.Count; j++)
		{
			if (changedIDs.ContainsKey(controlledActions[j].partId))
			{
				controlledActions[j].partId = changedIDs[controlledActions[j].partId];
			}
		}
	}

	public void onEditorPartEvent(ConstructionEventType evt, Part part)
	{
		switch (evt)
		{
		case ConstructionEventType.PartDeleted:
			PartDeleted(part);
			break;
		case ConstructionEventType.PartSymmetryDeleted:
			PartDeleted(part);
			break;
		case ConstructionEventType.PartAttached:
			UpdatePartSymmetry(part);
			break;
		}
	}

	public void OnPartWillDie(Part part)
	{
		PartDeleted(part);
	}

	public override string GetInfo()
	{
		string text = "";
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002363") + "</color>";
		text = text + base.part.partInfo.cost + " " + Localizer.Format("#autoLOC_7001031") + "\n";
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002364") + "</color>";
		text = text + Localizer.Format("#autoLOC_5050023", base.part.partInfo.partPrefab.mass) + "\n";
		text += "\n";
		text += resHandler.PrintModuleResources();
		return text + "\n";
	}

	public void SetWindowSizeAndPosition(RectTransform windowRect)
	{
		windowPosition = windowRect.anchoredPosition;
		windowSize = windowRect.sizeDelta;
		HasWindowDimensions = true;
	}

	public void AddPartAxis(Part part, PartModule module, BaseAxisField axisField)
	{
		if (!HasPartAxisField(part, axisField) && !HasPartSymmetryAxisField(part, axisField))
		{
			ControlledAxis controlledAxis = new ControlledAxis(part, module, axisField, this);
			GameEvents.onRoboticControllerAxesAdding.Fire(this, controlledAxis);
			controlledAxes.Add(controlledAxis);
			GameEvents.onRoboticControllerAxesChanged.Fire(this);
			UpdateUIElements();
		}
	}

	public void AddPartAction(Part part, PartModule module, BaseAction action)
	{
		if (!HasPartAction(part, action) && !HasPartSymmetryAction(part, action))
		{
			ControlledAction controlledAction = new ControlledAction(part, module, action, this);
			GameEvents.onRoboticControllerActionsAdding.Fire(this, controlledAction);
			controlledActions.Add(controlledAction);
			GameEvents.onRoboticControllerActionsChanged.Fire(this);
			UpdateUIElements();
		}
	}

	public void PartDeleted(Part part)
	{
		RemovePartAxis(part, null, transferToSymPartner: true);
		RemovePartAction(part, null, transferToSymPartner: true);
		RemovePartSymmetry(part.persistentId);
	}

	public void RemovePartAxis(Part part, BaseAxisField axisField, bool transferToSymPartner)
	{
		bool flag = false;
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			ControlledAxis controlledAxis = controlledAxes[count];
			if (!(controlledAxis.Part != null) || controlledAxis.Part.persistentId != part.persistentId || (axisField != null && (controlledAxis.AxisField == null || !(controlledAxis.AxisField.name == axisField.name))))
			{
				continue;
			}
			if (part.symmetryCounterparts.Count > 0 && transferToSymPartner)
			{
				bool flag2 = false;
				for (int i = 0; i < part.symmetryCounterparts.Count; i++)
				{
					if (part.symmetryCounterparts[i].vessel == base.vessel)
					{
						flag2 = true;
						AddPartAxisForSymmetryPartner(controlledAxis, part.symmetryCounterparts[i]);
						controlledAxes.RemoveAt(count);
						flag = true;
					}
				}
				if (!flag2)
				{
					GameEvents.onRoboticControllerAxesRemoving.Fire(this, controlledAxis);
					controlledAxes.RemoveAt(count);
					flag = true;
				}
			}
			else
			{
				GameEvents.onRoboticControllerAxesRemoving.Fire(this, controlledAxis);
				controlledAxes.RemoveAt(count);
				flag = true;
			}
		}
		if (flag)
		{
			GameEvents.onRoboticControllerAxesChanged.Fire(this);
		}
		UpdateUIElements();
	}

	public void RemovePartAction(Part part, BaseAction action, bool transferToSymPartner)
	{
		bool flag = false;
		int count = controlledActions.Count;
		while (count-- > 0)
		{
			ControlledAction controlledAction = controlledActions[count];
			if ((!(controlledAction.Part != null) || controlledAction.Part.persistentId != part.persistentId || action != null) && (controlledAction.Action == null || action == null || !(controlledAction.Action.name == action.name) || (!(controlledAction.Module == null) && action.listParent != null && !(action.listParent.module == null) && controlledAction.moduleId != action.listParent.module.PersistentId)))
			{
				continue;
			}
			if (part.symmetryCounterparts.Count > 0 && transferToSymPartner)
			{
				bool flag2 = false;
				for (int i = 0; i < part.symmetryCounterparts.Count; i++)
				{
					if (part.symmetryCounterparts[i].vessel == base.vessel)
					{
						flag2 = true;
						AddPartActionForSymmetryPartner(controlledAction, part.symmetryCounterparts[i]);
						controlledActions.RemoveAt(count);
						flag = true;
					}
				}
				if (!flag2)
				{
					GameEvents.onRoboticControllerActionsRemoving.Fire(this, controlledAction);
					controlledActions.RemoveAt(count);
					flag = true;
				}
			}
			else
			{
				GameEvents.onRoboticControllerActionsRemoving.Fire(this, controlledAction);
				controlledActions.RemoveAt(count);
				flag = true;
			}
		}
		if (flag)
		{
			GameEvents.onRoboticControllerActionsChanged.Fire(this);
		}
		UpdateUIElements();
	}

	public void RemovePartAxis(ControlledAxis axis, bool transferToSymPartner)
	{
		RemovePartAxis(axis.Part, axis.AxisField, transferToSymPartner);
	}

	public void RemovePartAction(ControlledAction action, bool transferToSymPartner)
	{
		RemovePartAction(action.Part, action.Action, transferToSymPartner);
	}

	public void AddPartAxisForSymmetryPartner(ControlledAxis oldAxis, Part newPart)
	{
		if (oldAxis.AxisField == null || oldAxis.AxisField.module == null || oldAxis.axisName == null)
		{
			return;
		}
		BaseAxisField baseAxisField = null;
		PartModule partModule = null;
		if (oldAxis.Module == null)
		{
			partModule = newPart.Modules[oldAxis.AxisField.module.ClassName];
			baseAxisField = partModule.Fields[oldAxis.axisName] as BaseAxisField;
		}
		else
		{
			int num = oldAxis.Part.Modules.IndexOf(oldAxis.Module);
			if (num > -1)
			{
				partModule = newPart.Modules[num];
				baseAxisField = partModule.Fields[oldAxis.axisName] as BaseAxisField;
			}
		}
		if (baseAxisField != null)
		{
			ControlledAxis controlledAxis = new ControlledAxis(newPart, partModule, baseAxisField, this);
			FloatCurve timeValue = new FloatCurve(oldAxis.timeValue.Curve.keys);
			controlledAxis.timeValue = timeValue;
			controlledAxes.Add(controlledAxis);
		}
	}

	public void AddPartActionForSymmetryPartner(ControlledAction oldAction, Part newPart)
	{
		if (oldAction == null || oldAction.actionName == null)
		{
			return;
		}
		BaseAction baseAction = null;
		PartModule partModule = null;
		if (oldAction.Module == null)
		{
			baseAction = newPart.Actions[oldAction.actionName];
		}
		else
		{
			int num = oldAction.Part.Modules.IndexOf(oldAction.Module);
			if (num > -1)
			{
				partModule = newPart.Modules[num];
				baseAction = partModule.Actions[oldAction.actionName];
			}
		}
		if (baseAction != null)
		{
			ControlledAction controlledAction = new ControlledAction(newPart, partModule, baseAction, this);
			controlledAction.times = new List<float>(oldAction.times);
			controlledActions.Add(controlledAction);
		}
	}

	public bool HasPartAxisField(Part testPart, BaseAxisField testAxisField)
	{
		ControlledAxis axis;
		return TryGetPartAxisField(testPart, testAxisField, out axis);
	}

	public bool HasPartSymmetryAxisField(Part testPart, BaseAxisField testAxisField)
	{
		bool symmetryPartner = false;
		ControlledAxis action;
		return TryGetPartAxisField(testPart, testAxisField, out action, out symmetryPartner) && symmetryPartner;
	}

	public bool TryGetPartAxisField(Part testPart, BaseAxisField testAxisField, out ControlledAxis axis)
	{
		bool symmetryPartner;
		bool result = TryGetPartAxisField(testPart, testAxisField, out axis, out symmetryPartner);
		if (symmetryPartner)
		{
			axis = null;
			return false;
		}
		return result;
	}

	public bool TryGetPartAxisField(Part testPart, BaseAxisField testAction, out ControlledAxis action, out bool symmetryPartner)
	{
		int count = controlledAxes.Count;
		ControlledAxis controlledAxis;
		do
		{
			if (count-- > 0)
			{
				controlledAxis = controlledAxes[count];
				if (controlledAxis.Part != null && controlledAxis.AxisField != null && controlledAxis.AxisField.name == testAction.name && controlledAxis.Part.isSymmetryCounterPart(testPart))
				{
					testAction.module.PersistentActionsId = testAction.module.GetPersistenActiontId();
					if (testAction.module == null || testAction.module.PersistentActionsId == controlledAxis.PersistentActionId)
					{
						action = controlledAxis;
						symmetryPartner = true;
						return true;
					}
				}
				continue;
			}
			action = null;
			symmetryPartner = false;
			return false;
		}
		while (!(controlledAxis.Part != null) || controlledAxis.AxisField == null || controlledAxis.Part.persistentId != testPart.persistentId || !(controlledAxis.AxisField.name == testAction.name) || (!(testAction.module == null) && testAction.module.PersistentId != controlledAxis.moduleId));
		action = controlledAxis;
		symmetryPartner = false;
		return true;
	}

	public bool HasPartAction(Part testPart, BaseAction testAction)
	{
		ControlledAction action;
		return TryGetPartAction(testPart, testAction, out action);
	}

	public bool TryGetPartAction(Part testPart, BaseAction testAction, out ControlledAction action)
	{
		bool symmetryPartner;
		bool result = TryGetPartAction(testPart, testAction, out action, out symmetryPartner);
		if (symmetryPartner)
		{
			action = null;
			return false;
		}
		return result;
	}

	public bool HasPartSymmetryAction(Part testPart, BaseAction testAction)
	{
		bool symmetryPartner = false;
		ControlledAction action;
		return TryGetPartAction(testPart, testAction, out action, out symmetryPartner) && symmetryPartner;
	}

	public bool TryGetPartAction(Part testPart, BaseAction testAction, out ControlledAction action, out bool symmetryPartner)
	{
		int count = controlledActions.Count;
		ControlledAction controlledAction;
		do
		{
			if (count-- > 0)
			{
				controlledAction = controlledActions[count];
				if (controlledAction.Part != null && controlledAction.Action != null && controlledAction.Action.name == testAction.name && controlledAction.Part.isSymmetryCounterPart(testPart))
				{
					if (testAction.listParent.module != null)
					{
						testAction.listParent.module.PersistentActionsId = testAction.listParent.module.GetPersistenActiontId();
					}
					if (testAction.listParent.module == null || testAction.listParent.module.PersistentActionsId == controlledAction.PersistentActionId)
					{
						action = controlledAction;
						symmetryPartner = true;
						return true;
					}
				}
				continue;
			}
			action = null;
			symmetryPartner = false;
			return false;
		}
		while (!(controlledAction.Part != null) || controlledAction.Action == null || controlledAction.Part.persistentId != testPart.persistentId || !(controlledAction.Action.name == testAction.name) || (!(testAction.listParent.module == null) && testAction.listParent.module.PersistentId != controlledAction.moduleId));
		action = controlledAction;
		symmetryPartner = false;
		return true;
	}

	public bool HasPart(List<Part> testParts)
	{
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			ControlledAxis controlledAxis = controlledAxes[count];
			for (int i = 0; i < testParts.Count; i++)
			{
				if (controlledAxis.Part != null && controlledAxis.Part.persistentId == testParts[i].persistentId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasPart(Part testPart)
	{
		int count = controlledAxes.Count;
		ControlledAxis controlledAxis;
		do
		{
			if (count-- > 0)
			{
				controlledAxis = controlledAxes[count];
				continue;
			}
			return false;
		}
		while (!(controlledAxis.Part != null) || controlledAxis.Part.persistentId != testPart.persistentId);
		return true;
	}

	public void UpdatePartSymmetry(Part part)
	{
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			ControlledAxis controlledAxis = controlledAxes[count];
			if (controlledAxis.Part != null && controlledAxis.Part.persistentId == part.persistentId)
			{
				controlledAxis.RebuildSymmetryList();
			}
		}
		int count2 = controlledActions.Count;
		while (count2-- > 0)
		{
			ControlledAction controlledAction = controlledActions[count2];
			if (controlledAction.Part != null && controlledAction.Part.persistentId == part.persistentId)
			{
				controlledAction.RebuildSymmetryList();
			}
		}
		UpdateUIElements();
	}

	public void RemovePartSymmetry(uint oldPartId)
	{
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			controlledAxes[count].RebuildSymmetryList(oldPartId);
		}
		int count2 = controlledActions.Count;
		while (count2-- > 0)
		{
			controlledActions[count2].RebuildSymmetryList(oldPartId);
		}
		UpdateUIElements();
	}

	public bool isSameActionId(Part testPart, ControlledAction cAction)
	{
		ModuleRoboticController component = testPart.gameObject.GetComponent<ModuleRoboticController>();
		int num = 0;
		while (true)
		{
			if (num < component.ControlledActions.Count)
			{
				if (component.ControlledActions[num].PersistentActionId == cAction.PersistentActionId)
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

	public void VesselModified(Vessel modifiedVessel)
	{
		if (HighLogic.LoadedSceneIsFlight && modifiedVessel != null && base.vessel != null && modifiedVessel.persistentId == base.vessel.persistentId)
		{
			RemoveOtherVesselItems();
		}
	}

	public void RemoveOtherVesselItems()
	{
		if (GameSettings.SERENITY_CONTROLLER_IGNORES_VESSEL)
		{
			return;
		}
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			if (controlledAxes[count].Part != null && controlledAxes[count].Part.vessel != null && base.vessel != null && controlledAxes[count].Part.vessel.persistentId != base.vessel.persistentId)
			{
				Debug.LogFormat("[ModuleRoboticController]: Removing the {0}({1}) axis from the controller as its on another vessel", controlledAxes[count].PartNickName, controlledAxes[count].partId);
				RemovePartAxis(controlledAxes[count], transferToSymPartner: true);
			}
		}
		int count2 = controlledActions.Count;
		while (count2-- > 0)
		{
			if (controlledActions[count2].Part != null && controlledActions[count2].Part.vessel != null && base.vessel != null && controlledActions[count2].Part.vessel.persistentId != base.vessel.persistentId)
			{
				Debug.LogFormat("[ModuleRoboticController]: Removing the {0}({1}) action from the controller as its on another vessel", controlledActions[count2].PartNickName, controlledActions[count2].partId);
				RemovePartAction(controlledActions[count2], transferToSymPartner: true);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (window != null)
		{
			SetWindowSizeAndPosition(window.transform as RectTransform);
		}
		base.OnSave(node);
		node.AddValue("sequenceIsPlaying", SequenceIsPlaying);
		node.AddValue("sequenceDirection", SequenceDirection);
		node.AddValue("sequenceLoopMode", SequenceLoop);
		ConfigNode configNode = node.AddNode("CONTROLLEDAXES");
		for (int i = 0; i < controlledAxes.Count; i++)
		{
			ConfigNode node2 = configNode.AddNode("AXIS");
			controlledAxes[i].Save(node2);
		}
		ConfigNode configNode2 = node.AddNode("CONTROLLEDACTIONS");
		for (int j = 0; j < controlledActions.Count; j++)
		{
			ConfigNode node3 = configNode2.AddNode("ACTION");
			controlledActions[j].Save(node3);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		HasWindowDimensions = node.HasValue("windowPosition");
		bool value = false;
		node.TryGetValue("sequenceIsPlaying", ref value);
		SequenceIsPlaying = value;
		SequenceDirectionOptions value2 = SequenceDirectionOptions.Forward;
		node.TryGetEnum("sequenceDirection", ref value2, SequenceDirectionOptions.Forward);
		SequenceDirection = value2;
		SequenceLoopOptions value3 = SequenceLoopOptions.Once;
		node.TryGetEnum("sequenceLoopMode", ref value3, SequenceLoopOptions.Once);
		SequenceLoop = value3;
		if (controlledAxes == null)
		{
			controlledAxes = new List<ControlledAxis>();
		}
		controlledAxes.Clear();
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("CONTROLLEDAXES", ref node2))
		{
			for (int i = 0; i < node2.nodes.Count; i++)
			{
				ControlledAxis controlledAxis = new ControlledAxis();
				controlledAxis.Load(node2.nodes[i]);
				controlledAxis.Controller = this;
				controlledAxes.Add(controlledAxis);
			}
		}
		if (controlledActions == null)
		{
			controlledActions = new List<ControlledAction>();
		}
		controlledActions.Clear();
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("CONTROLLEDACTIONS", ref node3))
		{
			for (int j = 0; j < node3.nodes.Count; j++)
			{
				ControlledAction controlledAction = new ControlledAction();
				controlledAction.Load(node3.nodes[j]);
				controlledAction.Controller = this;
				controlledActions.Add(controlledAction);
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6011075");
	}

	[ContextMenu("Assign Ref Vars")]
	public void AssignReferenceVars_Debug()
	{
		int count = controlledAxes.Count;
		while (count-- > 0)
		{
			if (!controlledAxes[count].AssignReferenceVars())
			{
				Debug.LogWarningFormat("[ModuleRoboticController]: Removing the axis we couldnt bind from the controller");
				controlledAxes.RemoveAt(count);
			}
		}
		int count2 = controlledActions.Count;
		while (count2-- > 0)
		{
			if (!controlledActions[count2].AssignReferenceVars())
			{
				Debug.LogWarningFormat("[ModuleRoboticController]: Removing the action we couldnt bind from the controller");
				controlledActions.RemoveAt(count2);
			}
		}
	}
}
