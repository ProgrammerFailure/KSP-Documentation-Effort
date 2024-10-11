using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelMotorSteering : ModuleWheelMotor
{
	[KSPField]
	public float steeringTorque;

	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001465")]
	public bool steeringEnabled;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001466")]
	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	public bool steeringInvert;

	public Vector3 wFwd;

	public Vector3 Rcom;

	public Vector3 wXcom;

	public Vector3 steerAxisInput;

	public float wDot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelMotorSteering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override float OnDriveUpdate(float motorInput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool GetMotorEnabled(bool baseMotorEnabled, ref string stateString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}
}
