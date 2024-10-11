using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRoboticServoPiston : BaseServo, IMultipleDragCube
{
	[KSPField(guiFormat = "F2", isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6013028")]
	[UI_MinMaxRange(maxValueY = 100f, maxValueX = 99f, stepIncrement = 1f, affectSymCounterparts = UI_Scene.All, minValueY = 1f, minValueX = 0f)]
	public Vector2 softMinMaxExtension;

	protected UI_MinMaxRange softRangeField;

	[KSPField]
	public bool hideUISoftMinMaxExtension;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiFormat = "F2", isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6013029")]
	public float targetExtension;

	public float powerLossExtension;

	protected UI_FloatRange targetExtensionField;

	protected BaseAxisField targetExtensionAxisField;

	[KSPField(guiFormat = "F2", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6013033", guiUnits = "#autoLOC_7001411")]
	public float currentExtension;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 5f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 1f, isPersistant = true, maxValue = 5f, minValue = 0.05f, guiFormat = "F2", axisMode = KSPAxisMode.Absolute, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_8005419")]
	public float traverseVelocity;

	[KSPField]
	public bool hideUITraverseVelocity;

	[KSPAxisField(incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6013030")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float pistonDamping;

	[KSPField]
	public bool hideUIDamping;

	[KSPField]
	public float linearLimitBounce;

	[KSPField]
	public float linearLimitSpringSpring;

	[KSPField]
	public float linearLimitSpringDamper;

	[KSPField]
	public float linearLimitContactDistance;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float positionDampingMutliplier;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public float positionSpringMutliplier;

	[KSPField]
	public string slaveTransformNames;

	[SerializeField]
	protected SoftJointLimit linearLimit;

	[SerializeField]
	protected SoftJointLimitSpring linearLimitSpring;

	[SerializeField]
	protected JointDrive jointDrive;

	[SerializeField]
	protected Vector3 editorPostion;

	[SerializeField]
	protected float previousTargetPosition;

	public float driveTargetPosition;

	private float targetPosition;

	protected Transform[] slaveTransforms;

	protected bool initComplete;

	private static string outputUnit;

	private float positionDiff;

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRoboticServoPiston()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new static void CacheLocalStrings()
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
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateTargetPosition(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateTraverseVelocity(object field)
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
	protected override void OnVisualizeServo(bool moveBase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetJointTargetPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModifyServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCurrentExtension()
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
	protected override void ResetLaunchPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSaveShip(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdatePAWUI(UI_Scene currentScene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 CalcTargetPosition(Vector3 startingPosition, float offset, bool relative)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6013034")]
	protected void ExtendPistonAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6013035")]
	protected void RetractPistonAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6013036")]
	public void TogglePistonAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ExtendPiston()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RetractPiston()
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
	private void SetDragCubes(float extension)
	{
		throw null;
	}
}
