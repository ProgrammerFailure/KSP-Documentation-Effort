using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Serenity.RobotArmFX;
using UnityEngine;

namespace Expansions.Serenity;

public class ModuleRobotArmScanner : ModuleDeployablePart
{
	public enum ArmDeployState
	{
		RETRACTED,
		UNPACKING,
		EXTENDING,
		SCANNING,
		RETRACTING,
		PACKING,
		BROKEN,
		PREVIEWRANGE
	}

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001352")]
	public string scanStatus;

	[KSPField]
	public string unpackAnimationName;

	[KSPField]
	public string editorReachAnimationName;

	private ArmDeployState _deployState;

	[KSPField]
	public string firstJointTransformName;

	protected Transform firstJointTransform;

	protected Quaternion firstJointOrigRot;

	[KSPField]
	public float firstJointAlignOffset;

	[KSPField]
	public string secondJointTransformName;

	protected Transform secondJointTransform;

	protected Quaternion secondJointOrigRot;

	[KSPField]
	public float secondJointAlignOffset;

	[KSPField]
	public string gimbalTransformName;

	protected Transform gimbalTransform;

	protected Quaternion gimbalOrigRot;

	[KSPField]
	public float gimbalAlignOffset;

	[KSPField]
	public string instTransformName;

	protected Transform instTransform;

	protected Quaternion instOrigRot;

	[KSPField]
	public float instAlignOffset;

	[KSPField]
	public string instCentreTransformName;

	protected Transform instCentreTransform;

	protected Quaternion instCentreOrigRot;

	protected bool emergencyRetractScanner;

	public List<Vector2> safeRetractPeriods;

	[KSPField]
	public string rangeTriggerColliderTransformName;

	protected Transform rangeTriggerTransform;

	protected MeshRenderer rangeTriggerRenderer;

	protected Material rangeTriggerMaterial;

	[KSPField]
	public float distanceFromSurface;

	[KSPField]
	public float emergencyStopDistanceFromSurface;

	protected List<GameObject> rocsInRange;

	protected float firstArmLength;

	protected float secondArmLength;

	protected float maxArmLength;

	protected Vector3 scanPosition;

	protected Vector3 instTargetPos;

	protected ROC scanROC;

	protected string scanROCDisplayName;

	public Quaternion baseExtTargetRot;

	public Quaternion firstJointExtTargetRot;

	public Quaternion secondJointExtTargetRot;

	public Quaternion gimbalExtTargetRot;

	public Quaternion instExtTargetRot;

	[KSPField]
	public float cancelScanDistance;

	[KSPField]
	public float firstJointRotStartAngleModifier;

	private Vector3 scanStartPosition;

	protected ModuleScienceExperiment moduleScienceExperiment;

	protected Vector3 closestScanPoint;

	[KSPField]
	public float sphereCastRadius;

	[KSPField]
	public float rangeTriggerRadius;

	[KSPField]
	public string rangeTriggerParentTransformName;

	[KSPField]
	public float firstJointRotationLimit;

	private RaycastHit hitInfo;

	private static RaycastHit[] hits;

	[KSPField]
	public string unpackEffectName;

	[KSPField]
	public string extendEffectName;

	[KSPField]
	public string scanEffectName;

	[KSPField]
	public string retractEffectName;

	[KSPField]
	public string packEffectName;

	protected AudioFX packAudioFX;

	[SerializeField]
	public List<RobotArmScannerFX> scannerEffectList;

	private bool extendingUpwards;

	private bool startGoingBackwards;

	protected float animationStartTime;

	protected float pauseStartTime;

	private float paused_anim_speed;

	private string playingAnimation;

	protected static string cacheAutoLOC_8004274;

	protected static string cacheAutoLOC_8004432;

	protected string cacheAutoLOC_8004426;

	protected static string cacheAutoLOC_8004427;

	public new ArmDeployState deployState
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
	public ModuleRobotArmScanner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ModuleRobotArmScanner()
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
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupScannerTransforms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupScannerEffectList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
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
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTriggerEnter(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTriggerExit(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCollisionEnter(Collision collision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnPartActionUICreate(Part actionPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePartActionUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DoExtend()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual ROC SelectROCToScan(out Vector3 closestScanPoint, bool useRaycast)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RayCastToROC(Vector3 origin, Vector3 traceDirection, float traceDistance, ref RaycastHit hit, bool performSphereCast, float radiusForSphereCast = 0.35f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateExtendedTargetRotations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DoRetract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void startFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void updateFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PerformScanROCNullCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateAnimationTime(string playingAnimationName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayUnpackAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UnpackArm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ExtendArm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayScanAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PerformExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ReturnArmToPrePackState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayPackAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PackArm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void breakPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool DoRepair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CanScan()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTimeWarpRateChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCurrentMousePartChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_8004430")]
	private void PlayEditorAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
