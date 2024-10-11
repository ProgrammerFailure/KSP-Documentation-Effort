using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoRotor : BaseServo
{
	[KSPAxisField(incrementalSpeed = 100f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005424")]
	[UI_FloatRange(stepIncrement = 5f, maxValue = 460f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float rpmLimit;

	[KSPField(guiFormat = "F1", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8005437")]
	public float currentRPM;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005425")]
	[UI_Toggle(disabledText = "#autoLOC_8005426", scene = UI_Scene.All, enabledText = "#autoLOC_8005427", affectSymCounterparts = UI_Scene.All)]
	public bool rotateCounterClockwise;

	[UI_Toggle(disabledText = "#autoLOC_8003303", enabledText = "#autoLOC_8003302", tipText = "#autoLOC_8003301", affectSymCounterparts = UI_Scene.None)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003304")]
	public bool inverted;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_8002243", "#autoLOC_8005426", "#autoLOC_8005427" })]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8005428")]
	public int ratcheted;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001458")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 200f, minValue = 0f)]
	public float brakePercentage;

	[KSPField]
	public float rotationMatch;

	[KSPField]
	public float LFPerkN;

	[KSPField]
	public float OxidizerPerkN;

	[KSPField]
	public float rotorSpoolTime;

	[KSPField]
	public float brakeTorque;

	[KSPField]
	public float maxTorque;

	[KSPField]
	public float angularPositionDamper;

	[KSPField]
	public float angularPositionSpring;

	[KSPField]
	public string angularDriveMode;

	[SerializeField]
	private SoftJointLimitSpring xSpringLimit;

	[SerializeField]
	private SoftJointLimit lowLimits;

	[SerializeField]
	private SoftJointLimit highLimits;

	[SerializeField]
	private JointDrive xDrive;

	[SerializeField]
	private Quaternion editorRotation;

	private bool initComplete;

	private UI_Cycle ratchetedField;

	private UI_Toggle rotationDirectionField;

	private UI_FloatRange rpmLimitUIField;

	private BaseAxisField rpmLimitAxisField;

	private UI_FloatRange motorPowerField;

	private bool brakingMode;

	private float freespinCurrentRotation;

	private float freespinPreviousRotation;

	private Quaternion ratchetLockRotation;

	[SerializeField]
	private float ratchetTolerance;

	private bool resourceStateChange;

	private float positiveAngularVelocity;

	private float brakingTorque;

	private float totalTorque;

	private float tempAngularVelocity;

	private float workingRPM;

	private float loadMass;

	[SerializeField]
	private float rpmRefreshSeconds;

	private float timeSinceRPMRefresh;

	private bool refreshRPMReadouts;

	private float rotationDiff;

	public float normalizedOutput
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRoboticServoRotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyRPMLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyDirection(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001458", KSPActionGroup.Brakes)]
	public void BrakeAction(KSPActionParam kPar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8005434")]
	public void MotorPowerAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8005435")]
	public void MotorDirectionAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleMotorPower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleMotorDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnJointInit(bool goodSetup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPostStartJointInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPreModifyServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnVisualizeServo(bool rotateBase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModifyServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnServoLockApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnServoLockRemoved()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnServoMotorEngaged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnServoMotorDisengaged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RatchetLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool IsMoving()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override float GetFrameDisplacement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetInitialDisplacement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSaveShip(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ResetLaunchPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLoadMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetJointDrive(float torque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion RotateOnMainAxis(Quaternion originalRotation, float rotationAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdatePAWUI(UI_Scene currentScene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnResourceNonemptyEmpty(PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void InitAxisFieldLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateAxisFieldHardLimit(string fieldName, Vector2 newlimits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateAxisFieldSoftLimit(string fieldName, Vector2 newlimits)
	{
		throw null;
	}
}
