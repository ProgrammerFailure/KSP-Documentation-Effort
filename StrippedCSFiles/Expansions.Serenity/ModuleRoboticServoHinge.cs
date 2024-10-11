using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoHinge : BaseServo, IMultipleDragCube
{
	[UI_MinMaxRange(maxValueY = 177f, maxValueX = 176f, stepIncrement = 1f, affectSymCounterparts = UI_Scene.All, minValueY = 1f, minValueX = 0f)]
	[KSPField(guiFormat = "F1", isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8002344")]
	public Vector2 softMinMaxAngles;

	[KSPField]
	public bool hideUISoftMinMaxAngles;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 30f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005420")]
	public float targetAngle;

	[KSPField]
	public float modelInitialAngle;

	[KSPField(isPersistant = true)]
	public bool mirrorRotation;

	private bool checkSymmetry;

	private float workingJointTargetAngle;

	[KSPField(guiFormat = "N1", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002346")]
	public float currentAngle;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 180f, minValue = 1f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 30f, guiFormat = "F1", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8005419")]
	public float traverseVelocity;

	[KSPField]
	public bool hideUITraverseVelocity;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(advancedTweakable = true, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002347")]
	public float hingeDamping;

	[KSPField]
	public bool hideUIDamping;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float angularXLimitSpring;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float angularXLimitDamper;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float highAngularXLimitBounce;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float highAngularXLimitSurf;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float lowAngularXLimitBounce;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float lowAngularXLimitSurf;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float driveDampingMutliplier;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float driveSpringMutliplier;

	[KSPField(isPersistant = true)]
	public float previousTargetAngle;

	[SerializeField]
	private string pistonTransforms;

	[SerializeField]
	public float displacementLimitIgnoreRange;

	[SerializeField]
	public float displacementLimitIgnoreFrameDistance;

	[SerializeField]
	protected SoftJointLimit upperLimit;

	[SerializeField]
	protected SoftJointLimit lowerLimit;

	[SerializeField]
	protected SoftJointLimitSpring xSpringLimit;

	[SerializeField]
	protected JointDrive xDrive;

	[SerializeField]
	protected Quaternion editorRotation;

	[SerializeField]
	protected float lockedTargetAngle;

	protected bool initComplete;

	protected UI_FloatRange targetAngleUIField;

	protected BaseAxisField targetAngleAxisField;

	protected UI_FloatRange traverseVelocityUIField;

	protected BaseAxisField traverseVelocityAxisField;

	protected UI_MinMaxRange softRangeField;

	private float lastDragCubeAngle;

	private float powerLossLockAngle;

	public float maxAnglePerFrame;

	public float driveTargetAngle;

	private float angleDiff;

	private float lowerDisplacementLimit;

	private float upperDisplacementLimit;

	public float JointTargetAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
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
	public ModuleRoboticServoHinge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003236")]
	protected void MaximumAngleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003235")]
	protected void MinimumAngleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_8003282")]
	protected void ToggleHingeAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMinimumAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMaximumAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckSymmetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TogglePistons(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCopy(PartModule fromModule)
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
	protected override void ResetLaunchPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnFixedUpdate()
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
	protected override void UpdatePAWUI(UI_Scene currentScene)
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
	protected override void OnSaveShip(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyTraverseLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Quaternion SetTargetRotation(Quaternion startingRotation, float rotationAngle, bool setRotation)
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
	private void SetDragCubes(float angle)
	{
		throw null;
	}
}
