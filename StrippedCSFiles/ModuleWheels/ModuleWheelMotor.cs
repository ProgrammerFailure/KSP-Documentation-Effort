using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;

namespace ModuleWheels;

public class ModuleWheelMotor : ModuleWheelSubmodule, IResourceConsumer
{
	public enum MotorState
	{
		Inoperable = -2,
		NotEnoughResources,
		Disabled,
		Idle,
		Running
	}

	[KSPField]
	public float driveResponse;

	[KSPField]
	public double idleDrain;

	[KSPField]
	public float wheelSpeedMax;

	[KSPField(isPersistant = true)]
	public bool autoTorque;

	[KSPField(guiActive = true, guiName = "#autoLOC_7000070")]
	public string motorStateString;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_7000070")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool motorEnabled;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001462")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001076", affectSymCounterparts = UI_Scene.All)]
	public bool motorInverted;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001463", guiUnits = "%")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float driveLimiter;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001464")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, maxValue = 5f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float tractionControlScale;

	[KSPField(guiFormat = "0.0", guiActive = true, guiName = "#autoLOC_7000070", guiUnits = "%")]
	[UI_ProgressBar(controlEnabled = true, scene = UI_Scene.Flight, maxValue = 100f, minValue = 0f)]
	public float driveOutput;

	protected float driveInput;

	public float maxTorque;

	public MotorState state;

	protected double resourceFraction;

	private BaseEvent evtAutoTorqueToggle;

	private BaseField fldDriveLimiter;

	private BaseField fldTractionControl;

	public double avgResRate;

	[KSPField]
	public FloatCurve torqueCurve;

	private List<PartResourceDefinition> consumedResources;

	private List<AdjusterWheelMotorBase> adjusterCache;

	private static string cacheAutoLOC_247970;

	private static string cacheAutoLOC_247987;

	private static string cacheAutoLOC_6001071;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelMotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
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
	protected virtual float OnDriveUpdate(float motorInput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMotorOrientationSign()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckMotorState(float driveInput, ref string stateString, out double rOutput, float torqueScalar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool GetMotorEnabled(bool baseMotorEnabled, ref string stateString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMaxSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoTorqueToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002419")]
	public void MotorToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002485")]
	public void MotorEnable(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002486")]
	public void MotorDisable(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001464")]
	public void ActAutoTorqueToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ActionUIUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ATsymPartUpdate()
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
	protected float ApplyTorqueAdjustments(float torque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
