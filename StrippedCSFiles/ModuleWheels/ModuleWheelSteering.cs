using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelSteering : ModuleWheelSubmodule
{
	[KSPField]
	public string caliperTransformName;

	[KSPField]
	public FloatCurve steeringCurve;

	[KSPField]
	public FloatCurve steeringMaxAngleCurve;

	[KSPField]
	public float steeringResponse;

	[KSPField(isPersistant = true)]
	public bool autoSteeringAdjust;

	public Transform caliperTransform;

	private bool caliperUpdating;

	private Quaternion caliperRot0;

	[KSPField]
	public float steeringRange;

	public float steeringInput;

	public float steeringInputLast;

	private Vector3 upAxis;

	private Vector3 fwdAxis;

	private Vector3 leftAxis;

	public float steerAngle;

	public float steerRange;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001467")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool steeringEnabled;

	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001468")]
	public bool steeringInvert;

	private Vector3 CoM;

	private Vector3 wCenter;

	private Vector3 wLeft0;

	private Vector3 wLeft;

	private Vector3 wRight;

	private Vector3 wAxis;

	private Vector3 sAxis;

	private float CoMfwdLength;

	private BaseEvent evtAutoSteeringAdjustToggle;

	private BaseField fldSteeringResponseTweakable;

	private BaseField fldSteeringAngleTweakable;

	private bool partActionWindowOpen;

	private bool steeringAngleCurveDefined;

	private bool steeringCurveDefined;

	private Vector2 prevLocalWheelVelocity;

	[KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002672")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 1f, maxValue = 10f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	public float angleTweakable;

	[KSPAxisField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002673")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 10f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	public float responseTweakable;

	private Vector3 updateCoordFrameReferenceForward;

	public Vector3 tPivot;

	public Vector3 tOrt;

	public Vector3 tCenter;

	public float h;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelSteering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoSteeringAdjustToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ActionSteeringAdjustUIUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ATsymPartUpdate()
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
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002418")]
	public void SteeringToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetSteeringResponseScale(float steerDelta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateCoordFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float updateSteering(float input, float steeringRange)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float findCoMfwdLength(Vector3 vesselCoM, Vector3 wheelCenter, Vector3 fwdAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetCoMFwdLength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 findTurnCenter(float steerAngle, float length, Vector3 CoM)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIShown(UIPartActionWindow window, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSteeringTweakablesSliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIDismiss(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSubsystemsModified(WheelSubsystems s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCaliperUpdate(bool update)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetCaliper(bool resetTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWheelRepaired(Part p)
	{
		throw null;
	}
}
