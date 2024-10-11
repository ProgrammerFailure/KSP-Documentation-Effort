using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ModuleWheels;
using UnityEngine;
using VehiclePhysics;

public class ModuleWheelBase : PartModule, IModuleInfo, IContractObjectiveModule
{
	public enum DriftCorrectionState
	{
		Idle,
		Acquire,
		Fix
	}

	[SerializeField]
	private KSPWheelController wheel;

	[SerializeField]
	private GameObject wheelHost;

	[KSPField(isPersistant = true)]
	public WheelType wheelType;

	[KSPField(isPersistant = true)]
	public bool isGrounded;

	[KSPField]
	public bool FitWheelColliderToMesh;

	[KSPField]
	public float radius;

	[KSPField]
	public Vector3 center;

	[KSPField]
	public float mass;

	[KSPField]
	public float frictionSharpness;

	[KSPField]
	public float wheelDamping;

	[KSPField]
	public float wheelMaxSpeed;

	[KSPField]
	public string clipObject;

	private GameObject clipGameObject;

	[KSPField]
	public float adherentStart;

	[KSPField]
	public float frictionAdherent;

	[KSPField]
	public float peakStart;

	[KSPField]
	public float frictionPeak;

	[KSPField]
	public float limitStart;

	[KSPField]
	public float frictionLimit;

	[KSPField]
	public bool autoFrictionAvailable;

	[KSPField(isPersistant = true)]
	public bool autoFriction;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001457")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.01f, maxValue = 10f, minValue = 0.01f, affectSymCounterparts = UI_Scene.All)]
	public float frictionMultiplier;

	[KSPField]
	public float geeBias;

	public bool suspensionEnabled;

	[KSPField]
	public float groundHeightOffset;

	[KSPField]
	public int inactiveSubsteps;

	[KSPField]
	public int activeSubsteps;

	[KSPField]
	public float tireForceSharpness;

	[KSPField]
	public float suspensionForceSharpness;

	[KSPField]
	public bool ApplyForcesToParent;

	[KSPField]
	public string wheelColliderTransformName;

	[KSPField]
	public string wheelTransformName;

	[KSPField]
	public string TooltipTitle;

	[KSPField]
	public string TooltipPrimaryField;

	[KSPField]
	public float springSlerpRate;

	[KSPField]
	public float minimumDownforce;

	[KSPField]
	public bool useNewFrictionModel;

	public Transform wheelColliderHost;

	public Transform wheelTransform;

	private List<ModuleWheelSubmodule> subModules;

	private bool setup;

	private BaseEvent evtAutoFrictionToggle;

	private BaseField fldFrictionMultiplier;

	[SerializeField]
	private bool driftCorrection;

	[NonSerialized]
	private Collider gCollider;

	[NonSerialized]
	private Collider gColliderPrev;

	[NonSerialized]
	private Vessel vSrfContact;

	[NonSerialized]
	private Part tgtParent;

	[NonSerialized]
	private string vLandedAt;

	public Vector2 slipDisplacement;

	private WheelSubsystems inopSystems;

	private WheelSubsystem inopOnRails;

	private WheelSubsystem inopSuspension;

	private ModuleWheelBrakes brakesSubmodule;

	private ModuleWheelLock wheelLockSubmodule;

	private ModuleWheelDamage wheelDamageSubmodule;

	[SerializeField]
	private bool rbBrakeConstraints;

	private float schloompaTime;

	private ModuleWheelDamage moduleWheelDamage;

	private float acquireMaxSpeed;

	private Vector3 fixFwd;

	private Vector3 error;

	private Vector3 errorLast;

	[SerializeField]
	private float kd;

	[SerializeField]
	private float ki;

	private DriftCorrectionState driftCorrectionState;

	public KSPWheelController Wheel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WheelOrgPosR
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public Quaternion WheelOrgRotR
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public WheelSubsystems InopSystems
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelBase()
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
	private void wheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ApplyGeeBias(float gee)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFriction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckSubsteps()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LandingDetectionUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool LandedDetectionNeedsUpdate(Collider hitCollider, Collider hitColliderPrev, Vessel vContact, bool isGrounded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IgnoreForcesOnSameVesselContact(VehicleBase.WheelState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LSchloomphaVPPProcessing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableWheelCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableWheelCollider(bool immediate = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void ResetWheelGroundCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPutToGround(PartHeightQuery qry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterSubmodule(ModuleWheelSubmodule m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterSubmodule(ModuleWheelSubmodule m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWheelSetup(KSPWheelController w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartPack(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void onPartUnpack(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onDockingComplete(GameEvents.FromToAction<Part, Part> FromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void resetBrakeInput(float prevInput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselUndocking(Vessel fromVessel, Vessel toVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void toggleRbConstraints(bool freeze)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void holdWheelDamage(float seconds = 3f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> FromTo)
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
	public override void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PromoteToPhysicalPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSubsystemsModified(WheelSubsystems sub)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InopUpdate(bool force = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InopWheelCollider(bool inop, bool force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InopWheelTransform(bool disable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EnableSuspension(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableSuspension(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckSuspensionToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void updateDriftFix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getFixFwd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void getFixTorque(Vector3 fixOrt, Vector3 refOrt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoFrictionToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001457")]
	public void ActAutoFrictionToggle(KSPActionParam act)
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
	public string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckContractObjectiveValidity()
	{
		throw null;
	}
}
