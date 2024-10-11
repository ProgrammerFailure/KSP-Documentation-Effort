using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleDeployablePart : PartModule, IMultipleDragCube, IScalarModule, IConstruction
{
	public enum DeployState
	{
		RETRACTED,
		EXTENDED,
		RETRACTING,
		EXTENDING,
		BROKEN
	}

	public enum TrackingMode
	{
		SUN,
		HOME,
		CURRENT,
		VESSEL,
		NONE
	}

	public enum PanelAlignType
	{
		PIVOT,
		X,
		Y,
		Z
	}

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001352")]
	public string status;

	[KSPField]
	public bool showStatus;

	[KSPField]
	public Quaternion originalRotation;

	[KSPField(isPersistant = true)]
	public Quaternion currentRotation;

	[KSPField]
	public bool runOnce;

	[KSPField(isPersistant = true)]
	public float storedAnimationTime;

	[KSPField(isPersistant = true)]
	public float storedAnimationSpeed;

	[KSPField]
	public bool isTracking;

	[KSPField]
	public bool applyShielding;

	[KSPField]
	public bool applyShieldingExtend;

	[KSPField]
	public float TrackingAlignmentOffset;

	[KSPField]
	public bool retractable;

	[KSPField]
	public bool isBreakable;

	[KSPField]
	public float windResistance;

	[KSPField]
	public double gResistance;

	[KSPField]
	public float impactResistance;

	[KSPField]
	public float impactResistanceRetracted;

	[KSPField]
	public float subPartMass;

	[KSPField]
	public float trackingSpeed;

	[KSPField]
	public string pivotName;

	[KSPField]
	public string breakName;

	[KSPField]
	public PanelAlignType alignType;

	[KSPField]
	public string secondaryTransformName;

	[KSPField]
	public string animationName;

	[KSPField]
	public float editorAnimationSpeedMult;

	[KSPField]
	public bool useAnimation;

	private bool bypassSetupAnimation;

	[KSPField]
	public float panelDrag;

	[KSPField]
	public bool useCurve;

	[KSPField(isPersistant = true)]
	public DeployState deployState;

	[KSPField]
	public TrackingMode trackingMode;

	[KSPField]
	public bool eventsInSymmwtryAlways;

	[KSPField]
	public bool eventsInSymmwtryEditor;

	[KSPField]
	public string extendActionName;

	[KSPField]
	public string retractActionName;

	[KSPField]
	public string extendpanelsActionName;

	[KSPField]
	public string subPartName;

	[KSPField]
	public string partType;

	public Transform panelRotationTransform;

	public Transform panelBreakTransform;

	public bool hasPivot;

	public CelestialBody trackingBody;

	public Vessel trackingVessel;

	public string vesselID;

	public Transform trackingTransformLocal;

	public Transform trackingTransformScaled;

	protected Transform secondaryTransform;

	protected Animation anim;

	protected bool stopAnimation;

	private List<GameObject> breakObjects;

	protected EventData<float, float> onMove;

	protected EventData<float> onStop;

	internal bool playAnimationOnStart;

	[SerializeField]
	internal int repairKitsNecessary;

	private BaseEvent eventRepairExternal;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6003063")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string brokenStatusWarning;

	private UI_Label brokenStatusWarningField;

	public List<string> destroyOnBreakObjects;

	protected bool trackingLOS;

	protected string blockingObject;

	[KSPField]
	public string moduleID;

	protected bool overrideUIWriteState;

	private List<AdjusterDeployablePartBase> adjusterCache;

	protected static string cacheAutoLOC_234828;

	protected static string cacheAutoLOC_234841;

	protected static string cacheAutoLOC_234856;

	protected static string cacheAutoLOC_234861;

	protected static string cacheAutoLOC_234868;

	protected static string cacheAutoLOC_6001415;

	protected static string cacheAutoLOC_6001017;

	protected static string cacheAutoLOC_6005093;

	public virtual float MinAoAForQCheck
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
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
	public ModuleDeployablePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001800")]
	public void ExtendPanelsAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ControlPanelsWithoutUsingSymmetry(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001801")]
	public void ExtendAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001802")]
	public void RetractAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001803")]
	public virtual void Extend()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DoExtend()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001804")]
	public virtual void Retract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DoRetract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_8003453")]
	public void EventRepairExternal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatRepair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool DoRepair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselFocusChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindAnimations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCollisionEnter(Collision collision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void startFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void recurse(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void breakPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void breakNotifications(string partTitle, string breakMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PostFSMUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void updateFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ShouldBreakFromPressure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ShouldBreakFromG()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PostCalculateTracking(bool trackingLOS, Vector3 trackingDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CalculateTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetTrackingBodyTransforms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsMoving()
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
	private void SetDragCubes(float retracted, float angle)
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
	protected bool IsDeployablePartStuck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanBeRotated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool AllowConstructionDeployState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
