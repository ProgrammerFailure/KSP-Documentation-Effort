using System.Runtime.CompilerServices;

public class ModuleRotatingJoint : ModuleJointMotor
{
	public enum DriveModes
	{
		DRIVE,
		NEUTRAL,
		LOCKED
	}

	public enum ControlModes
	{
		ActionGroups,
		Throttle,
		Pitch,
		Roll,
		Yaw
	}

	public DriveModes driveMode;

	public ControlModes controlMode;

	public bool debug;

	[KSPField]
	public float maxTorque;

	[KSPField]
	public float minimumTorque;

	[KSPField]
	public float targetSpeed;

	[KSPField]
	public float maxSpeed;

	[KSPField]
	public float minimumSpeed;

	public float motorThrottle;

	[KSPField(isPersistant = true)]
	public float invertDrive;

	[KSPField(isPersistant = true)]
	public bool driveEngaged;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 1f, minValue = 0f)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001843")]
	public float torqueRatio;

	[KSPField(isPersistant = true)]
	public string savedMode;

	[KSPField(isPersistant = true)]
	public string savedControlMode;

	public float effectiveTopSpeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float effectiveTorque
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRotatingJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Engage Drive")]
	private void EngageAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Lock Drive")]
	private void LockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Neutral Drive")]
	private void NeutralAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Invert Drive")]
	private void InvertAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Rotate")]
	private void ForwardDrive(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Counter-Rotate")]
	private void ReverseDrive(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001838")]
	public void EngageDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001844")]
	public void LockDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001845")]
	public void NeutralDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001846")]
	public void InvertDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = " ")]
	public void ToggleControlMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSavedState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateGuiNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleStart(StartState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnJointInit(bool goodSetup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnMotorModeChanged(Mode mode)
	{
		throw null;
	}
}
