using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelDeployment : ModuleWheelSubmodule, IMultipleDragCube
{
	[UI_Toggle(disabledText = "#autoLOC_6001071", invertButton = true, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001460")]
	public bool shieldedCanDeploy;

	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLOC_6001372")]
	public string stateDisplayString;

	[KSPField(isPersistant = true)]
	public string stateString;

	[KSPField]
	public string animationTrfName;

	[KSPField]
	public string animationStateName;

	[KSPField]
	public float deployedPosition;

	public float retractedPosition;

	[KSPField]
	public float extendDurationFactor;

	[KSPField]
	public float retractDurationFactor;

	[KSPField]
	public float TsubSys;

	[KSPField]
	public string deployTargetTransformName;

	[KSPField]
	public string retractTransformName;

	[KSPField]
	public string fxDeploy;

	[KSPField]
	public string fxDeployed;

	[KSPField]
	public string fxRetract;

	[KSPField]
	public string fxRetracted;

	private bool retractStarted;

	private BaseField fieldState;

	private BaseEvent eventToggle;

	private BaseEvent eventInitialStateToggle;

	[NonSerialized]
	private Animation anim;

	[NonSerialized]
	private AnimationState animationState;

	[NonSerialized]
	private Transform deployTgt;

	[NonSerialized]
	private Transform retractTransform;

	public KerbalFSM fsm;

	public float position;

	public float animLength;

	[NonSerialized]
	private List<IScalarModule> slaveModules;

	public int[] slaveModuleIndices;

	public WheelSubsystem deploymentSubsystems;

	public bool subsystemsDisabled;

	[SerializeField]
	private SphereCollider standInCollider;

	[KSPField]
	public bool useStandInCollider;

	private ModuleLight lightModule;

	public KFSMState st_deployed;

	public KFSMState st_retracted;

	public KFSMState st_deploying;

	public KFSMState st_retracting;

	public KFSMState st_inoperable;

	public KFSMEvent on_deploy;

	public KFSMEvent on_deployed;

	public KFSMEvent on_retract;

	public KFSMEvent on_retracted;

	public KFSMEvent on_inoperable;

	private static string cacheAutoLOC_6002269;

	private static string cacheAutoLOC_6002270;

	private static string cacheAutoLOC_234856;

	private static string cacheAutoLOC_234861;

	private static string cacheAutoLOC_7003226;

	private static string cacheAutoLOC_247601;

	private static string cacheAutoLOC_6002271;

	private static string cacheAutoLOC_7003224;

	private static string cacheAutoLOC_7003225;

	public float Position
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
	public ModuleWheelDeployment()
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
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetInopSubsystems(bool disable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected SphereCollider CreateStandInCollider(KSPWheelController w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FSMSetup(string startStateName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFSMStateChange(KFSMState from, KFSMState to, KFSMEvent evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001329")]
	public void EventToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ToggleDeployment(bool affectSymCParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetToggleEventName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001461", KSPActionGroup.Gear)]
	public void ActionToggle(KSPActionParam kAct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActionToggle(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
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
	public float FindTransformAnimationTime(Transform t)
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
