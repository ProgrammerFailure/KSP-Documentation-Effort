using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;

namespace ModuleWheels;

public class ModuleWheelBrakes : ModuleWheelSubmodule
{
	[KSPField]
	public float maxBrakeTorque;

	[KSPField]
	public float brakeResponse;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 5f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_148080", guiUnits = "%")]
	public float brakeTweakable;

	[KSPField]
	public int statusLightModuleIndex;

	[KSPField(isPersistant = true)]
	public float brakeInput;

	public ModuleStatusLight statusLight;

	private List<AdjusterWheelBrakesBase> adjusterCache;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelBrakes()
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
	[KSPAction("#autoLOC_6001458", KSPActionGroup.Brakes)]
	public void BrakeAction(KSPActionParam kPar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
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
}
