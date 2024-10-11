using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelSuspension : ModuleWheelSubmodule
{
	[KSPField]
	public string suspensionTransformName;

	[KSPField]
	public float suspensionOffset;

	[KSPField]
	public float suspensionDistance;

	[SerializeField]
	private float spring;

	[SerializeField]
	private float damper;

	[KSPField]
	public float targetPosition;

	[KSPField]
	public float springRatio;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 3f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001469")]
	public float springTweakable;

	[KSPField]
	public float damperRatio;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 2f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001470")]
	public float damperTweakable;

	[KSPField(isPersistant = true)]
	public bool autoSpringDamper;

	[SerializeField]
	private bool autoSpringDamperSet;

	[KSPField]
	public float boostRatio;

	[KSPField]
	public float maximumLoad;

	[KSPField]
	public bool suppressModuleInfo;

	[KSPField(isPersistant = true)]
	public Vector3 suspensionPos;

	[KSPField(isPersistant = true)]
	public Vector3 suspensionDefaultPos;

	[KSPField]
	public bool useDistributedMass;

	[SerializeField]
	private float damperFudge;

	[SerializeField]
	private float boost;

	[SerializeField]
	private float vesselMass;

	private Transform suspensionTransform;

	public float suspCompression;

	public float suspMass;

	public float sprungMassGravity;

	[KSPField(isPersistant = true)]
	public float autoBoost;

	[KSPField]
	public bool useAutoBoost;

	[KSPField]
	public string suspensionColliderName;

	private Collider suspensionCollider;

	[KSPField]
	public bool adjustForHighGee;

	[KSPField]
	public float highGeeThreshold;

	[KSPField]
	public float highGeeSpringTweakable;

	[KSPField]
	public float highGeeDamperTweakable;

	[SerializeField]
	private bool highGeeOverride;

	[SerializeField]
	private bool prevUseAutoBoost;

	[SerializeField]
	private float prevSpringTweakable;

	[SerializeField]
	private float springClampMax;

	[SerializeField]
	private float damperClampMax;

	[SerializeField]
	private float damperLerpBase;

	[SerializeField]
	private float oscillationDamper;

	[SerializeField]
	private float boostClamp;

	[SerializeField]
	private float autoBoostClamp;

	private BaseEvent evtAutoSpringDamperToggle;

	private BaseField fldSpringTweakable;

	private BaseField fldDamperTweakable;

	[SerializeField]
	private bool debugSuspension;

	public bool HighGeeOverride
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelSuspension()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoSpringDamperToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ActionSpringDamperUIUpdate()
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
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
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
	private void onVesselPartCountChanged(Vessel changedVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Toggle Tweakables")]
	public void ToggleTweakables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float BoostCurve(float b, float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SuspensionSpringUpdate(float sprungMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateAutoBoost(float st)
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
	protected void ResetSuspensionCollider(float delay)
	{
		throw null;
	}
}
