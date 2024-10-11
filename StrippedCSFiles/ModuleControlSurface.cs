using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;
using UnityEngine;

public class ModuleControlSurface : ModuleLiftingSurface, IMultipleDragCube, ITorqueProvider
{
	[KSPField]
	public new string transformName;

	[KSPField]
	public float ctrlSurfaceRange;

	[KSPField]
	public float ctrlSurfaceArea;

	[KSPField]
	public float actuatorSpeed;

	private float originalActuatorSpeed;

	[KSPField]
	public bool useExponentialSpeed;

	[KSPField]
	public float actuatorSpeedNormScale;

	[KSPField]
	public bool alwaysRecomputeLift;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool mirrorDeploy;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool usesMirrorDeploy;

	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	public bool ignorePitch;

	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001331")]
	public bool ignoreYaw;

	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001332")]
	public bool ignoreRoll;

	[UI_Toggle(disabledText = "#autoLOC_6001081", scene = UI_Scene.All, enabledText = "#autoLOC_6001080", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001333")]
	public bool deploy;

	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001334")]
	public bool deployInvert;

	[KSPField(unfocusedRange = 25f, guiActiveUnfocused = true, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001335")]
	[UI_Toggle(disabledText = "#autoLOC_6001075", enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.None)]
	public bool partDeployInvert;

	[Obsolete("Do not use as per #24665")]
	private bool precessionControl;

	protected Vector3 potentialBladeControlTorque;

	protected Vector3 vesselBladeLiftReference;

	[KSPAxisField(unfocusedRange = 25f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, incrementalSpeed = 50f, guiFormat = "0", axisMode = KSPAxisMode.Incremental, minValue = -100f, guiActive = true, guiName = "#autoLOC_6013041", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = -100f, affectSymCounterparts = UI_Scene.All)]
	public float deployAngle;

	public Vector2 deployAngleLimits;

	[KSPAxisField(minValue = -150f, incrementalSpeed = 60f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 150f, guiActive = false, guiName = "#autoLOC_6001336")]
	public float authorityLimiter;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 150f, minValue = -150f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, guiFormat = "0", isPersistant = false, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001336", guiUnits = "°")]
	public float authorityLimiterUI;

	private float maxAuthority;

	protected Vector3 inputVector;

	protected Vector3 rotatingControlInput;

	protected Quaternion airflowIncidence;

	protected Vector3 baseLiftForce;

	protected float action;

	protected float deflection;

	protected float roll;

	protected float deflectionDirection;

	protected Rigidbody referenceRigidBody;

	protected bool partActionWindowOpen;

	[SerializeField]
	protected Quaternion neutral;

	[SerializeField]
	protected Transform ctrlSurface;

	[SerializeField]
	protected double bladeUpdateInterval;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012000", guiUnits = "°")]
	public float angleOfAttack;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012001", guiUnits = "#autoLOC_6002488")]
	public float forwardLift;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012002", guiUnits = "#autoLOC_6002488")]
	public float verticalLift;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012003", guiUnits = "#autoLOC_6002487")]
	public float airSpeed;

	private double lastBladeInfoUpdate;

	[SerializeField]
	private ModuleRoboticServoRotor parentRotor;

	private IRoboticServo parentRotorServo;

	private float bladePitch;

	protected BaseField verticalLiftField;

	protected BaseField forwardLiftField;

	protected BaseField airSpeedField;

	protected BaseField aoaField;

	protected BaseField bladePitchField;

	private bool comCalculated;

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003384")]
	public string PitchCtrlState;

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003385")]
	public string YawCtrlState;

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003386")]
	public string RollCtrlState;

	protected List<AdjusterControlSurfaceBase> adjusterCache;

	private static string cacheAutoLOC_6003032;

	private static string cacheAutoLOC_8003402;

	private static string cacheAutoLOC_8003403;

	private static string cacheAutoLOC_8003404;

	public float currentDeployAngle
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

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleControlSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantApplied(Part eventPart, PartVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIShown(UIPartActionWindow paw, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIDismiss(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyAuthorityLimiter(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyAuthorityLimiterUI(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselReferenceTransformSwitch(Transform from, Transform to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalcAngleOfAttack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPotentialLift(bool positiveDeflection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ActionSign(float action, float epsilon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RotatingCtrlSurfaceUpdate(Vector3 vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedCtrlSurfaceUpdate(Vector3 vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CtrlSurfaceUpdate(Vector3 vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CtrlSurfaceEditorUpdate(Vector3 CoM)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001337", KSPActionGroup.None)]
	public void ActionToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001338", KSPActionGroup.None)]
	public void ActionExtend(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001339", KSPActionGroup.None)]
	public void ActionRetract(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006035", KSPActionGroup.None)]
	public void PitchActive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006036", KSPActionGroup.None)]
	public void PitchInactive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006037", KSPActionGroup.None)]
	public void TogglePitch(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006038", KSPActionGroup.None)]
	public void YawActive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006039", KSPActionGroup.None)]
	public void YawInactive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006040", KSPActionGroup.None)]
	public void ToggleYaw(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006041", KSPActionGroup.None)]
	public void RollActive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006042", KSPActionGroup.None)]
	public void RollInactive(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006043", KSPActionGroup.None)]
	public void ToggleRoll(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006044", KSPActionGroup.None)]
	public void ActivateAllControls(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6006045", KSPActionGroup.None)]
	public void DeactivateAllControls(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetDragCubeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeDragCubePosition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UsesProceduralDragCubes()
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
	protected float ApplyActuatorSpeedAdjustments(float actuatorSpeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateCoM()
	{
		throw null;
	}
}
