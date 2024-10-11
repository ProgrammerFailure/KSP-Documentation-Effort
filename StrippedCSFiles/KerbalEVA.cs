using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight;
using UnityEngine;

public class KerbalEVA : PartModule
{
	protected class ResourceListItem
	{
		public ProtoPartResourceSnapshot pPResourceSnapshot;

		public int priority;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceListItem()
		{
			throw null;
		}
	}

	[Serializable]
	public class HelmetColliderSetup
	{
		public float helmetOnRadius;

		public Vector3 helmetOnCenter;

		public float helmetOffRadius;

		public Vector3 helmetOffCenter;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HelmetColliderSetup()
		{
			throw null;
		}
	}

	protected class ClamberPath
	{
		public Vector3 edgeFaceNormal;

		public Vector3 srfNormal;

		public Vector3 p1;

		public Vector3 p2;

		public Vector3 p3;

		public float clamberHeight;

		public float standOffDistance;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClamberPath(Vector3 edgeFaceNormal, Vector3 srfNormal, Vector3 p1, Vector3 p2, Vector3 p3, float height, float standOff)
		{
			throw null;
		}
	}

	public enum VisorStates
	{
		Raised,
		Lowered,
		Raising,
		Lowering
	}

	[CompilerGenerated]
	private sealed class _003CStartEVA_003Ed__193 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CStartEVA_003Ed__193(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CkerbalAvatarUpdateCycle_003Ed__199 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CkerbalAvatarUpdateCycle_003Ed__199(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CwaitAndHandleRagdollTimeWarp_003Ed__576 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int waitFrames;

		public KerbalEVA _003C_003E4__this;

		private int _003Ci_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CwaitAndHandleRagdollTimeWarp_003Ed__576(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAcquireRotation_003Ed__587 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA _003C_003E4__this;

		public float duration;

		public Quaternion tgtRot;

		private Quaternion _003CiRot_003E5__2;

		private float _003CstartTime_003E5__3;

		private float _003CendTime_003E5__4;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CAcquireRotation_003Ed__587(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAcquirePosition_003Ed__588 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA _003C_003E4__this;

		public float duration;

		public Vector3 tgtPos;

		private Vector3 _003CiPos_003E5__2;

		private float _003CstartTime_003E5__3;

		private float _003CendTime_003E5__4;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CAcquirePosition_003Ed__588(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStartNonCollidePeriod_003Ed__628 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA _003C_003E4__this;

		public Transform airlockTrf;

		public Part fromPart;

		public float standoff;

		public float duration;

		private float _003CT0_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CStartNonCollidePeriod_003Ed__628(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CrestoreVesselInfo_afterWait_003Ed__739 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delay;

		public KerbalEVA _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CrestoreVesselInfo_afterWait_003Ed__739(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CswichFocusIfActiveVesselUncontrollable_delay_003Ed__741 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float delay;

		public KerbalEVA _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CswichFocusIfActiveVesselUncontrollable_delay_003Ed__741(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CReturnToIdle_003Ed__797 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float time;

		public KerbalEVA _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CReturnToIdle_003Ed__797(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001402")]
	public float lightR;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001403")]
	public float lightG;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001404")]
	public float lightB;

	[UI_ColorPicker]
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006093")]
	public string colorChanger;

	[KSPField]
	public float walkSpeed;

	[KSPField]
	public float strafeSpeed;

	[KSPField]
	public float runSpeed;

	[KSPField]
	public float turnRate;

	[KSPField]
	public float maxJumpForce;

	[KSPField]
	public float jumpMultiplier;

	[KSPField]
	public float boundForce;

	[KSPField]
	public float boundSpeed;

	[KSPField]
	public float boundThreshold;

	protected float boundForceMassFactor;

	protected Vector2 boundColliderYOffset;

	protected float boundColliderModifierThresholdTime;

	protected float boundColliderModifierCounter;

	[KSPField]
	public float SlopeForce;

	protected int slopeMovementDirection;

	[KSPField]
	public float swimSpeed;

	[KSPField]
	public float waterAngularDragMultiplier;

	[KSPField]
	public float ladderClimbSpeed;

	[KSPField]
	public float ladderPushoffForce;

	[KSPField]
	public float minWalkingGee;

	[KSPField]
	public float minRunningGee;

	[KSPField]
	public float initialMass;

	[KSPField]
	public float massMultiplier;

	[KSPField]
	public float onFallHeightFromTerrain;

	[KSPField]
	public float clamberMaxAlt;

	public Transform upperTorso;

	public Transform footPivot;

	public Transform referenceTransform;

	public KerbalAnimationManager Animations;

	public KerbalRagdollNode[] ragdollNodes;

	public GameObject[] handNodes;

	public Collider[] characterColliders;

	public Collider[] otherRagdollColliders;

	public Collider ladderCollider;

	public Collider[] otherPhysicColliders;

	protected AdvancedRagdoll advRagdoll;

	public KerbalFSM fsm;

	public bool DebugFSMState;

	public bool HasJetpack;

	[KSPField(isPersistant = true)]
	public bool JetpackDeployed;

	private bool jetpackDeployedForWelding;

	public bool JetpackIsThrusting;

	public bool autoGrabLadderOnStart;

	[KSPField(isPersistant = true)]
	public bool lampOn;

	public GameObject headLamp;

	[KSPField]
	public bool splatEnabled;

	[KSPField]
	public float splatSpeed;

	public GameObject splatPrefab;

	public bool CharacterFrameMode;

	public bool CharacterFrameModeToggle;

	protected bool loadedFromSFS;

	protected string loadedStateName;

	protected Vector3 ejectDirection;

	[KSPField]
	public string propellantResourceName;

	[Obsolete("EVA Fuel is now carried in jetpacks and tanks, the kerbal has no fuel on their person")]
	[KSPField]
	public double propellantResourceDefaultAmount;

	protected PartResource propellantResource;

	protected List<ResourceListItem> inventoryPropellantResources;

	protected DockedVesselInfo kerbalVesselInfo;

	protected Rigidbody _rigidbody;

	protected Animation _animation;

	protected List<ModuleLiftingSurface> chuteLiftingSurfaces;

	protected ModuleEvaChute evaChute;

	public bool HasParachute;

	public Transform helmetTransform;

	public Transform neckRingTransform;

	public SphereCollider helmetCollider;

	public HelmetColliderSetup helmetColliderSetup;

	public CapsuleCollider[] bodyColliders;

	private bool helmetTransformExists;

	private bool neckRingTransformExists;

	[KSPField(isPersistant = true)]
	private bool isHelmetEnabled;

	[KSPField(isPersistant = true)]
	private bool isNeckRingEnabled;

	[KSPField]
	public float helmetOffMinSafePressureAtm;

	[KSPField]
	public float helmetOffMaxSafePressureAtm;

	[KSPField]
	public float helmetOffMinSafeTempK;

	[KSPField]
	public float helmetOffMaxSafeTempK;

	[KSPField]
	public float helmetOffMaxOceanPressureAtm;

	[KSPField]
	public float helmetOffMinSafePressureAtmMargin;

	[KSPField]
	public float helmetOffMaxSafePressureAtmMargin;

	[KSPField]
	public float helmetOffMinSafeTempKMargin;

	[KSPField]
	public float helmetOffMaxSafeTempKMargin;

	[KSPField]
	public float helmetOffMaxOceanPressureAtmMargin;

	[KSPField]
	public float evaExitTemperature;

	public float feetToPivotDistance;

	protected bool partPlacementMode;

	protected ModuleInventoryPart moduleInventoryPartReference;

	public static bool alwaysShowInventory;

	private kerbalExpressionSystem myExpressionSystem;

	private IEnumerator returnIDLE;

	private IEnumerator startExpression;

	[SerializeField]
	private float newidleTime;

	private float newidleCounter;

	private bool startTimer;

	private int idleAnimationsIndex;

	protected KerbalPortrait portrait;

	protected bool visibleInPortrait;

	public Camera kerbalCamSkyBox;

	public Camera kerbalCamAtmos;

	public Camera kerbalCam01;

	public Camera kerbalCam00;

	public Camera kerbalPortraitCamera;

	public RenderTexture AvatarTexture;

	public float KerbalAvatarUpdateInterval;

	protected WaitForSeconds updIntervalYield;

	protected Coroutine updateAvatarCoroutine;

	private bool isActiveVessel;

	private Vector3 cameraPosition;

	[SerializeField]
	private float standardCameraDistance;

	[SerializeField]
	private float swimmingCameraDistance;

	[SerializeField]
	private float runningCameraDistance;

	[SerializeField]
	private float ragdollCameraDistance;

	protected List<ModuleScienceExperiment> moduleScienceExperiments;

	protected ModuleScienceExperiment moduleScienceExperimentROC;

	protected ModuleColorChanger suitColorChanger;

	private bool sciencePanelAnimPlaying;

	private float sciencePanelAnimCooldown;

	private bool pickRocSampleAnimPlaying;

	private float pickRocSampleAnimCooldown;

	public List<KerbalProp> kerbalObjects;

	public GameObject hammerPrefab;

	public Transform hammerAnchor;

	private GameObject hammer;

	private MeshRenderer hammerMesh;

	private Animation hammerAnimation;

	private float hammerAnimTimer;

	private ModuleGroundSciencePart sciencePart;

	private KerbalProp golfClub;

	private KerbalProp bananaProp;

	private KerbalProp wingnutProp;

	public SkinnedMeshRenderer helmetMesh;

	public SkinnedMeshRenderer bodyMesh;

	public SkinnedMeshRenderer neckringMesh;

	public GameObject weldToolPrefab;

	public Transform weldToolAnchor;

	private GameObject weldTool;

	private WeldFX weldFX;

	private AudioSource weldingLasersFX;

	private FXGroup weldingLaserGroup;

	private SuitCombos suitCombos;

	private bool alternateIdleDisabled;

	[KSPField(isPersistant = true)]
	private bool isVisorEnabled;

	public Transform JetpackTransform;

	public Transform BackpackTransform;

	public Transform BackpackStTransform;

	public Transform StorageTransform;

	public Transform StorageSlimTransform;

	public Transform ChuteJetpackTransform;

	public Transform ChuteStTransform;

	public Transform ChuteContainerTransform;

	private const string JetpackPartName = "evaJetpack";

	private const string ChutePartName = "evaChute";

	[UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 10f)]
	[KSPAxisField(minValue = 10f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage;

	[SerializeField]
	private bool isLadderJointed;

	public KFSMState st_idle_gr;

	public KFSMState st_walk_acd;

	public KFSMState st_walk_fps;

	public KFSMState st_heading_acquire;

	public KFSMState st_bound_gr_acd;

	public KFSMState st_bound_gr_fps;

	public KFSMState st_bound_fl;

	public KFSMState st_run_acd;

	public KFSMState st_run_fps;

	public KFSMState st_ragdoll;

	public KFSMState st_recover;

	public KFSMState st_idle_fl;

	public KFSMState st_jump;

	public KFSMState st_land;

	public KFSMState st_packTransition;

	public KFSMState st_swim_idle;

	public KFSMState st_swim_fwd;

	public KFSMState st_ladder_idle;

	public KFSMState st_ladder_acquire;

	public KFSMState st_ladder_climb;

	public KFSMState st_ladder_descend;

	public KFSMState st_ladder_lean;

	public KFSMState st_ladder_pushoff;

	public KFSMState st_clamber_acquireP1;

	public KFSMState st_clamber_acquireP2;

	public KFSMState st_clamber_acquireP3;

	public KFSMState st_flagAcquireHeading;

	public KFSMState st_flagPlant;

	public KFSMState st_seated_cmd;

	public KFSMState st_grappled;

	public KFSMState st_semi_deployed_parachute;

	public KFSMState st_fully_deployed_parachute;

	public KFSMState st_idle_b_gr;

	public KFSMState st_controlPanel_identified;

	public KFSMState st_picking_roc_sample;

	public KFSMState st_ladder_end_reached;

	public KFSMState st_playing_golf;

	public KFSMState st_smashing_banana;

	public KFSMState st_spinning_wingnut;

	public KFSMState st_enteringConstruction;

	public KFSMState st_exitingConstruction;

	public KFSMState st_weldAcquireHeading;

	public KFSMState st_weld;

	public KFSMEvent On_MoveAcd;

	public KFSMEvent On_MoveFPS;

	public KFSMEvent On_startRun;

	public KFSMEvent On_endRun;

	public KFSMEvent On_stop;

	public KFSMEvent On_hdgAcquireStart;

	public KFSMEvent On_hdgAcquireComplete;

	public KFSMEvent On_stumble;

	public KFSMEvent On_recover_start;

	public KFSMEvent On_jump_start;

	public KFSMEvent On_fall;

	public KFSMEvent On_land_start;

	public KFSMEvent On_MoveLowG_Acd;

	public KFSMEvent On_MoveLowG_fps;

	public KFSMEvent On_bound;

	public KFSMEvent On_bound_land;

	public KFSMEvent On_bound_fall;

	public KFSMEvent On_packToggle;

	public KFSMEvent On_feet_wet;

	public KFSMEvent On_feet_dry;

	public KFSMEvent On_swim_fwd;

	public KFSMEvent On_swim_stop;

	public KFSMEvent On_ladderGrabStart;

	public KFSMEvent On_ladderGrabComplete;

	public KFSMEvent On_ladderClimb;

	public KFSMEvent On_ladderDescend;

	public KFSMEvent On_ladderStop;

	public KFSMEvent On_ladderLetGo;

	public KFSMEvent On_LadderLand;

	public KFSMEvent On_LadderTop;

	public KFSMEvent On_LadderEnd;

	public KFSMEvent On_LadderLeanStart;

	public KFSMEvent On_LadderLeanEnd;

	public KFSMEvent On_LadderPushOff;

	public KFSMEvent On_clamberGrabStart;

	public KFSMEvent On_clamberP1;

	public KFSMEvent On_clamberP2;

	public KFSMEvent On_clamberP3;

	public KFSMEvent On_boardPart;

	public KFSMEvent On_flagPlantStart;

	public KFSMEvent On_flagPlantHdgAcquire;

	public KFSMEvent On_flagPlantFailed;

	public KFSMEvent On_flagPlantComplete;

	public KFSMEvent On_seatBoard;

	public KFSMEvent On_seatDeboard;

	public KFSMEvent On_seatEject;

	public KFSMEvent On_grapple;

	public KFSMEvent On_grappleRelease;

	public KFSMEvent On_semi_deploy_parachute;

	public KFSMEvent On_fully_deploy_parachute;

	public KFSMEvent On_parachute_cut;

	public KFSMEvent On_idle_b_gr;

	public KFSMEvent On_return_idle;

	public KFSMEvent On_control_panel_search;

	public KFSMEvent On_control_panel_interacting;

	public KFSMEvent On_chopping_roc;

	public KFSMEvent On_roc_sample_stored;

	public KFSMEvent On_ladderEndReached;

	public KFSMEvent On_Playing_Golf;

	public KFSMEvent On_Golf_Complete;

	public KFSMEvent On_Smashing_Banana;

	public KFSMEvent On_Banana_Complete;

	public KFSMEvent On_Spinning_Wingnut;

	public KFSMEvent On_Wingnut_Complete;

	public KFSMEvent On_constructionModeEnter;

	public KFSMEvent On_constructionModeExit;

	public KFSMEvent On_constructionModeTrigger_gr_Complete;

	public KFSMEvent On_constructionModeTrigger_fl_Complete;

	public KFSMEvent On_weldStart;

	public KFSMEvent On_weldHdgAcquired;

	public KFSMEvent On_weldComplete;

	protected KFSMTimedEvent On_recover_complete;

	protected KFSMTimedEvent On_jump_complete;

	protected KFSMTimedEvent On_land_complete;

	protected KFSMTimedEvent On_LadderPushoff_complete;

	protected AnimationState tmpBoundState;

	[KSPField]
	public float boundFrequency;

	[KSPField]
	public float boundSharpness;

	[KSPField]
	public float boundAttack;

	[KSPField]
	public float boundRelease;

	[KSPField]
	public float boundFallThreshold;

	private float tgtBoundStep;

	private float boundStepSpeed;

	[KSPField(isPersistant = true)]
	public float lastBoundStep;

	public float halfHeight;

	[KSPField(isPersistant = true)]
	public int _flags;

	[KSPField]
	public float flagReach;

	protected FlagSite flag;

	protected KerbalSeat kerbalSeat;

	protected Vector3 tgtRpos;

	protected Vector3 lastTgtRPos;

	protected Vector3 packTgtRPos;

	protected Vector3 packRRot;

	protected Vector3 cmdRot;

	protected Vector3 cmdDir;

	protected Vector3 ladderTgtRPos;

	protected Vector3 flagSpot;

	protected Vector3 parachuteInput;

	protected float currentSpd;

	protected float tgtSpeed;

	protected float lastTgtSpeed;

	protected float deltaHdg;

	protected float lastDeltaHdg;

	protected float jumpForce;

	protected bool boundLeftFoot;

	protected bool manualAxisControl;

	protected Quaternion rd_rot;

	protected Quaternion rd_tgtRot;

	protected Quaternion manualRotation;

	protected Quaternion slopeRotation;

	protected float normalizedBoundTime;

	protected float colliderHeight;

	[KSPField]
	public float Kp;

	[KSPField]
	public float Ki;

	[KSPField]
	public float Kd;

	[KSPField]
	public float iC;

	protected Vector3 tgtFwd;

	protected Vector3 error;

	protected Vector3 integral;

	protected Vector3 derivative;

	protected Vector3 prev_error;

	protected Vector3 tgtUp;

	public bool thrustPercentagePIDBoost;

	public float pidBoostThreshold;

	public float pidBoostMultiplier;

	public float pidBoostExponent;

	protected FXGroup PitchPos;

	protected FXGroup PitchNeg;

	protected FXGroup YawPos;

	protected FXGroup YawNeg;

	protected FXGroup RollPos;

	protected FXGroup RollNeg;

	protected FXGroup xPos;

	protected FXGroup xNeg;

	protected FXGroup yPos;

	protected FXGroup yNeg;

	protected FXGroup zPos;

	protected FXGroup zNeg;

	public float linFXLatch;

	public float rotFXLatch;

	public float linFXMinPower;

	public float linFXMaxPower;

	public float rotFXMinPower;

	public float rotFXMaxPower;

	[KSPField]
	public float rotPower;

	[KSPField]
	public float linPower;

	protected Vector3 packLinear;

	[KSPField]
	public float PropellantConsumption;

	protected float fuelFlowRate;

	public Vector3 fFwd;

	public Vector3 fUp;

	public Vector3 fRgt;

	[KSPField]
	public float stumbleThreshold;

	protected Vector3 lastCollisionDirection;

	protected Vector3 lastCollisionNormal;

	private ROC availableROC;

	private ROC experimentROC;

	private bool isAnchored;

	private float kerbalAnchorTimeThreshold;

	private float kerbalAnchorTimeCounter;

	private FixedJoint anchorJoint;

	public bool isRagdoll;

	public bool canRecover;

	protected Vector3d geeForce;

	protected Vector3d coriolisForce;

	protected Vector3d centrifugalForce;

	protected int referenceFrameChanged_rdPhysHold;

	[KSPField]
	public float hopThreshold;

	[KSPField]
	public float recoverThreshold;

	[KSPField]
	public double recoverTime;

	protected double lastCollisionTime;

	[KSPField]
	public float splatThreshold;

	public LadderEndCheck topLadderEnd;

	public LadderEndCheck bottomLadderEnd;

	private bool canClimb;

	private bool canDescend;

	public Transform ladderPivot;

	protected List<Collider> currentLadderTriggers;

	protected Collider currentLadder;

	protected Collider secondaryLadder;

	protected Part _currentLadderPart;

	protected Vector3 ladderPos;

	protected Vector3 ladderUp;

	protected Vector3 ladderFwd;

	protected Vector3 Vtgt;

	protected bool invLadderAxis;

	protected bool onLadder;

	[Obsolete("No longer used.")]
	public double LadderVesselPerturbationMultiplier;

	[Obsolete("No longer used.")]
	public double LadderMinCorrectiveForceSqrMag;

	[SerializeField]
	[Tooltip("Dot of two vectors used for maximum angle between ladder forward vectors")]
	private float MinLadderForwardDot;

	[Tooltip("Dot of two vectors used for maximum angle between ladder right vectors")]
	[SerializeField]
	private float MinLadderRightDot;

	protected bool ladderTransition;

	protected Vector3 clamberOrigin;

	protected Vector3 clamberTarget;

	protected RaycastHit clamberHitInfo;

	protected ClamberPath clamberPath;

	[KSPField]
	public float clamberReach;

	[KSPField]
	public float clamberStandoff;

	protected Vector3 controlOrigin;

	protected Vector3 controlTarget;

	protected RaycastHit controlHitInfo;

	[KSPField]
	private float controlPanelReach;

	[KSPField]
	private float controlPanelStandoff;

	internal Part constructionTarget;

	internal float constructionTargetPivotOffset;

	public Renderer VisorRenderer;

	[KSPField]
	public float VisorAnimationSpeed;

	private bool wasHelmetEnabledBeforeWelding;

	private bool removeHelmetAfterRaisingVisor;

	private bool wasVisorEnabledBeforeWelding;

	private VisorStates visorState;

	[SerializeField]
	private float visorLoweredTargetOffset;

	private float visorRaisedTargetOffset;

	private float visorTargetOffset;

	private float visorCurrentOffset;

	private Vector2 visorTextureOffset;

	[SerializeField]
	private bool replacedGolfBall;

	[SerializeField]
	private Vector3 ballForceDir;

	public float ballForce;

	public float ballDrag;

	public float ballTime;

	public float ballAngle;

	public float golfSoundTime;

	private bool playingGolfAnimPlaying;

	private float playingGolfAnimCooldown;

	private Callback afterPlayGolf;

	private List<string> golfSoundFX;

	private string golfSound;

	private bool golfSoundPlayed;

	[SerializeField]
	private bool replacedBanana;

	[SerializeField]
	private float bananaTime;

	[SerializeField]
	private float bananaForce;

	[SerializeField]
	private float bananaSoundTime;

	private Callback afterBanana;

	private bool smashingBananaAnimPlaying;

	private float smashingBananaAnimCooldown;

	private GameObject bananaShards;

	private List<string> bananaSoundFX;

	private string bananaSound;

	private bool bananaSoundPlayed;

	[SerializeField]
	private float wingnutKerbalTorqueForce;

	[SerializeField]
	private float wingnutTransitionTime;

	[SerializeField]
	private float wingnutTorqueTime;

	private Callback afterWingnut;

	private bool spinningWingnutAnimPlaying;

	private float spinningWingnutAnimCooldown;

	private bool appliedTorque;

	private Rigidbody wingnutRB;

	private GameObject wingnut;

	protected Collider currentAirlockTrigger;

	protected Part currentAirlockPart;

	private static string cacheAutoLOC_114130;

	private static string cacheAutoLOC_114293;

	private static string cacheAutoLOC_114297;

	private static string cacheAutoLOC_114358;

	private static string cacheAutoLOC_115662;

	private static string cacheAutoLOC_115694;

	private static string cacheAutoLOC_6010008;

	private static string cacheAutoLOC_6010009;

	private static string cacheAutoLOC_6010010;

	private static string cacheAutoLOC_6010011;

	private static string cacheAutoLOC_8003204;

	private static string cacheAutoLOC_6010015;

	private static string cacheAutoLOC_8002357;

	private static string cacheAutoLOC_8002358;

	private static string cacheAutoLOC_8002359;

	private static string cacheAutoLOC_8002360;

	private static string cacheAutoLOC_6012032;

	private static string cacheAutoLOC_6006049;

	private string helmetUnsafeReason;

	private bool atmosExistence;

	private double kerbalStaticPressureAtm;

	private double kerbalSkinTemp;

	private double kerbalInternalTemp;

	[SerializeField]
	public int framesDelayForHelmetDeathCheck;

	private int framesDelayForHelmetDeathCounter;

	[SerializeField]
	private PhysicMaterial physicMaterial;

	[KSPField(isPersistant = true)]
	private bool useGlobalPhysicMaterial;

	public Vector3 ladderPosition;

	public bool Ready
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	internal PartResource PropellantResource
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsChuteState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PartPlacementMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleInventoryPart ModuleInventoryPartReference
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public WeldFX WeldFX
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InConstructionMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public bool IsLadderJointed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected virtual bool VesselUnderControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual int flagItems
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

	public float PIDBoost
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

	public virtual double Fuel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual double FuelCapacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected virtual Part currentLadderPart
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

	public Part CurrentLadderPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool OnALadder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part LadderPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VisorStates VisorState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsVisorEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string HelmetUnsafeReason
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal PhysicMaterial PhysicMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KerbalEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModuleInventoryChanged(ModuleInventoryPart moduleInventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePackModels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupEVAFuel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ProcessEVAFuel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSoundFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetNewTextures(ProtoCrewMember pCrew, Material helmetMaterial, Material bodyMaterial, Material neckringMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDefaultTextures(ProtoCrewMember pCrew, Material helmetMaterial, Material bodyMaterial, Material neckringMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextureScale(Material suitMaterial, Vector2 scaleUpdate, SuitCombo.MaterialProperty materialProperty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStartEVA_003Ed__193))]
	protected virtual IEnumerator StartEVA(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEvent(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInventoryPaw(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPortraitCameraDistance(float newDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CkerbalAvatarUpdateCycle_003Ed__199))]
	protected virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetVisibleInPortrait(bool visible)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reconfigure Animations")]
	public virtual void SetupAnimations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_b_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_b_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void walk_Acd_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void walk_ccd_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void walk_fps_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void walk_fps_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void heading_acquire_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void heading_acquire_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void run_acd_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void run_acd_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void run_fps_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void run_fps_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_acd_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_acd_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_acd_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_fps_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_fps_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_gr_fps_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_fl_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void bound_fl_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ragdoll_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ragdoll_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void recover_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void recover_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void controlPanel_identified_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void picking_roc_sample_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void spawnHammer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void jump_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_fl_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void idle_fl_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void land_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void land_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void swim_idle_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void swim_idle_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void swim_fwd_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void swim_fwd_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_acquire_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_acquire_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_idle_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_idle_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_lean_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_lean_OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_lean_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_climb_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_climb_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_descend_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_descend_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_end_reached_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_end_reached_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ladder_pushoff_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP1_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ZeroRBVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP1_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP2_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP2_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP3_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void clamber_acquireP3_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void flagAcquireHeading_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void flagAcquireHeading_OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void flagAcquireHeading_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void flagPlant_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void flagPlant_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void seated_cmd_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void seated_cmd_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void grappled_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void grappled_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void EnterConstructionMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ExitConstructionMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void enteringConstruction_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void exitingConstruction_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void enteringExitingConstruction_OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void exitingConstruction_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleWeldingGun(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_acquireHeading_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_acquireHeading_OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_acquireHeading_OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_acquireHeading_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasWeldLineOfSight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasWeldLineOfSight(Vector3 gunPoint, Vector3 partAttachPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InterruptWeld()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnWeldStart(KerbalEVA kerbalEVA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnWeldFinish(KerbalEVA kerbalEVA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void weld_OnLeave(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFSMStateChange(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFSMEventCalled(KFSMEvent fsmEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSuitColors(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void HandleMovementInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetWaypoint(Vector3 tgtPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool IsKerbalInStateAbleToDeployParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateParachuteInCommandSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnParachuteSemiDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSemiDeployedParachuteModeEntered(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSemiDeployedParachuteMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSemiDeployedParachuteModeLeft(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnParachuteFullyDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFullyDeployedParachuteModeEntered(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFullyDeployedParachuteMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFullyDeployedParachuteModeLeft(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnParachuteCut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateHeading()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateLowGBodyColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ResetOrientationPID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateOrientationPID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePackAngular()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void correctGroundedRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void StartGroundedRotationRecover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RecoverGroundedRotation(float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupJetpackEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFXGroupMinMaxPower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void updateJetpackEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ToggleLamp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ToggleJetpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ToggleJetpack(bool packState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePackLinear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePackFuel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void getCoordinateFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void drawCoordinateFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = false, guiActiveUncommand = false, guiActiveUnfocused = false, externalToEVAOnly = false, guiActive = false, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6011014")]
	public void PickUpROC()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnROCExperimentFinished(ScienceData experimentData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnROCExperimentReset(ScienceData experimentData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCollisionEnter(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int CompareContactsByNormalToSurface(ContactPoint c1, ContactPoint c2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCollisionStay(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCollisionExit(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool isSelfCollision(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool SurfaceContact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool SurfaceOrSplashed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool GetL19Contact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AnchorUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddRBAnchor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveRBAnchor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupRagdoll(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal virtual void EnableCharacterAndLadderColliders(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetRagdoll(bool ragDoll, bool preservePose = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void IntegrateRagdollRigidbodyForces()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void updateRagdollVelocities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateGroundLevelAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vcs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onReferencebodyChanged(GameEvents.FromToAction<CelestialBody, CelestialBody> rChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onRotatingFrameChanged(GameEvents.HostTargetAction<CelestialBody, bool> frm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onFrameVelocityChange(Vector3d velOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnVesselGoOnRails(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnVesselGoOffRails(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndHandleRagdollTimeWarp_003Ed__576))]
	protected virtual IEnumerator waitAndHandleRagdollTimeWarp(int waitFrames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ResetRagdollLinks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPartDie()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Splat(Vector3 point, Vector3 normal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CanRecover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetEjectDirection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void EjectFromSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CAcquireRotation_003Ed__587))]
	protected virtual IEnumerator AcquireRotation(Quaternion tgtRot, float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CAcquirePosition_003Ed__588))]
	protected virtual IEnumerator AcquirePosition(Vector3 tgtPos, float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void correctLadderPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void correctLadderRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateLadderMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void InterpolateLadders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AutoTransition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateCurrentLadder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateCurrentLadderIdle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckLadderTriggers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual int SortTriggersByDistance(Collider c1, Collider c2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual int SortTriggersByAlignment(Collider c1, Collider c2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStartNonCollidePeriod_003Ed__628))]
	public virtual IEnumerator StartNonCollidePeriod(float duration, float standoff, Part fromPart, Transform airlockTrf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool FindClamberSrf(float fwdOffset, float reach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool TestClamberSrf(RaycastHit clamberHitInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual ClamberPath GetClamberPath(float fwdOffset, float reach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool FindControlPanel(float fwdOffset, float reach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ControlPanelInteractionFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RandomControlPanelAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RocSampleStored()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Weld(Part targetPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LowerVisor(bool forceHelmet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RaiseVisor(bool restoreHelmet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisorPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVisorRaised(KerbalEVA kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisorEventStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003448")]
	public void LowerVisor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003449")]
	public void RaiseVisor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayGolf(Callback afterGolfCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void playing_Golf_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SpawnGolf()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PlayingGolfPhysicalBall()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FinishedPlayingGolf()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Banana(Callback afterBananaCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void smashing_banana_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SpawnBanana()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SmashingBananaParticles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FinishedSmashingBanana()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dzhanibekov(Callback afterWingnutCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void spinning_Wingnut_OnEnter(KFSMState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SpawnWingnut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ApplyWingnutTorque()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SpinningWingnutForever()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FinishedSpinningWingnut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupEVAScienceSoundFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void BoardPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool processInventory(Part p, bool storeParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool checkExperiments(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void proceedAndBoard(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6003095")]
	public virtual void PlantFlag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CanPlantFlag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void AddFlag(int flagCount = 1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001360")]
	public virtual void MakeReference()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool BoardSeat(KerbalSeat seat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsSeated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGrapple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001810")]
	public virtual void OnDeboardSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RestoreVesselInfo(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CrestoreVesselInfo_afterWait_003Ed__739))]
	protected virtual IEnumerator restoreVesselInfo_afterWait(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SwitchFocusIfActiveVesselUncontrollable(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CswichFocusIfActiveVesselUncontrollable_delay_003Ed__741))]
	protected virtual IEnumerator swichFocusIfActiveVesselUncontrollable_delay(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 GetEjectPoint(Vector3 ejectDirection, float maxDist, float capsuleRadius, float capsuleHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_900678")]
	public virtual void RenameVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTriggerStay(Collider o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTriggerExit(Collider o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PostInteractionScreenMessage(string message, float delay = 0.1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitHelmetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHelmet(bool enableHelmet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleNeckRing(bool enableNeckRing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHelmetAndNeckRing(bool enableHelmet, bool enableNeckRing, bool storeToPCM = true, bool suppressTransformMessages = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6010003")]
	public void ChangeHelmet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = false, guiName = "#autoLOC_6010005")]
	public void ChangeNeckRing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHelmetChanged(KerbalEVA changedKerbal, bool helmetVisible, bool neckRingVisible)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckHelmetOffSafe(bool includeSafetyMargins = true, bool startEVAChecks = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetPreLoadPressure(double loadValue, CelestialBody vesselBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanEVAWithoutHelmet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanSafelyRemoveHelmet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool WillDieWithoutHelmet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateHelmetOffChecks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetPhysicMaterial(PhysicMaterial newPhysicMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ResetPhysicMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CReturnToIdle_003Ed__797))]
	private IEnumerator ReturnToIdle(float time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetPartPlacementMode(bool mode, ModuleInventoryPart moduleInventoryPartReference)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetCurrentColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<Color> PresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnColorChanged(Color color, string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnColorChanged(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModifyBodyColliderHeight(float newHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Set Ladder Anti-Sliding Mechanism")]
	private void SetLadderHold()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Clear Ladder Anti-Sliding Mechanism")]
	private void ClearLadderHold()
	{
		throw null;
	}
}
