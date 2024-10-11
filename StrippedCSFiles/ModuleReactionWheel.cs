using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleReactionWheel : PartModule, IResourceConsumer, ITorqueProvider
{
	public enum WheelState
	{
		[Description("#autoLOC_218513")]
		Active,
		[Description("#autoLOC_6003062")]
		Disabled,
		[Description("#autoLOC_6003063")]
		Broken
	}

	[KSPField]
	public string actionGUIName;

	[KSPField]
	public float PitchTorque;

	[KSPField]
	public float YawTorque;

	[KSPField]
	public float RollTorque;

	[KSPField]
	public float torqueResponseSpeed;

	public WheelState wheelState;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001306")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_6001075", "#autoLOC_6001307", "#autoLOC_6001308" }, controlEnabled = true, scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All)]
	public int actuatorModeCycle;

	public bool operational;

	[KSPField(guiFormat = "0", isPersistant = true, guiActive = true, guiName = "#autoLOC_6001309", guiUnits = "%")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float authorityLimiter;

	public Vector3 inputVector;

	public float inputSum;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001306")]
	public string stateString;

	private List<PartResourceDefinition> consumedResources;

	private Vector3 cmdInput;

	private Vector3 inputVectorTemp;

	private List<AdjusterReactionWheelBase> adjusterCache;

	private static string cacheAutoLOC_236147;

	private static string cacheAutoLOC_7001223;

	private static string cacheAutoLOC_218716;

	private static string cacheAutoLOC_6003052;

	public WheelState State
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleReactionWheel()
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
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetAppliedTorque()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ActiveUpdate(VesselActuatorMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001301")]
	public void CycleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001302")]
	public void Activate(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001303")]
	public void Deactivate(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001304")]
	public void Toggle(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001310")]
	public void OnToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
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
	protected bool IsAdjusterBreakingReactionWheel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 ApplyTorqueAdjustments(Vector3 torque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
