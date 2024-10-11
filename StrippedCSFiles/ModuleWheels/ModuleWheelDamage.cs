using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelDamage : ModuleWheelSubmodule, IConstruction
{
	[KSPField]
	public float stressTolerance;

	[KSPField]
	public float impactTolerance;

	[KSPField]
	public float deflectionMagnitude;

	[KSPField]
	public float slipMagnitude;

	[KSPField]
	public float deflectionSharpness;

	[KSPField]
	public float slipSharpness;

	[KSPField]
	public string damagedTransformName;

	[KSPField]
	public string undamagedTransformName;

	[KSPField]
	public bool isRepairable;

	[KSPField]
	public float explodeMultiplier;

	public float currentDeflection;

	public float lastDeflection;

	public float currentSlip;

	public float lastSlip;

	public float currentDownForce;

	public float lastDownForce;

	private const float repairImmunityMin = 30f;

	private const float repairImmunityMax = 90f;

	private float repairImmunityTimeTotal;

	private float repairImmunityTimeLeft;

	internal float startupTime;

	[SerializeField]
	internal int repairKitsNecessary;

	public bool initialized;

	[KSPField(isPersistant = true)]
	public bool isDamaged;

	public float totalStress;

	private float stressTime;

	private Transform dmgTransform;

	private Transform undmgTransform;

	[KSPField(guiFormat = "0.0", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001459")]
	[UI_ProgressBar(scene = UI_Scene.Flight, maxValue = 100f, minValue = 0f)]
	public float stressPercent;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6003063")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string brokenStatusWarning;

	private UI_Label brokenStatusWarningField;

	public float stressVariability;

	private BaseEvent eventRepairExternal;

	private WheelSubsystem subsystemDamage;

	private BaseField stressField;

	private static string cacheAutoLOC_6005093;

	[KSPField]
	public float impactDamageVelocity;

	[KSPField]
	public string impactDamageColliderName;

	private Collider impactDamageCollider;

	public bool IsImpactDamageEnable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelDamage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetDamageTransforms(bool logError)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartCouple(GameEvents.FromToAction<Part, Part> partAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartDecouple(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselModified(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetImmunity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDamaged(bool damaged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselFocusChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Damage Wheel")]
	public void DamageWheel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_6001882")]
	public void EventRepairExternal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeRotated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HandleCollision(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HandleCollisionVelocity(GameObject otherObject, Vector3 velocity, Vector3 normal)
	{
		throw null;
	}
}
