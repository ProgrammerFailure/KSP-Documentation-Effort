using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleJointMotorTest : PartModule, IActiveJointHost, IJointLockState
{
	[KSPField(guiActive = true, guiName = "#autoLOC_6001836")]
	public string motorState;

	[KSPField(guiActive = true, guiName = "#autoLOC_900381")]
	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 15f, minValue = 0f)]
	public float motorSpeed;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0.1f)]
	[KSPField(guiActive = true, guiName = "#autoLOC_6001837")]
	public float motorForce;

	private float lastSpeed;

	private float lastForce;

	private bool driveReverse;

	private bool linkSpeedToThrottle;

	private ActiveJoint motorJoint;

	private bool jointStarted;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleJointMotorTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnJointInit(ActiveJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetHostPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDriveModeChanged(ActiveJoint.DriveMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001838")]
	public void MotorDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001839")]
	public void MotorNeutral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001840")]
	public void MotorPark()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001841")]
	public void Reverse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001842")]
	public void LinkToThrottle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMotorSpeed(float speed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetMotorSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMotorForce(float force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetMotorForce()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetLocalTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsJointUnlocked()
	{
		throw null;
	}
}
