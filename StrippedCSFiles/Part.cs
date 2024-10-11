using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;
using Experience;
using Highlighting;
using KSP.UI.Screens;
using ModuleWheels;
using UnityEngine;
using UnityEngine.EventSystems;
using Vectrosity;

public class Part : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public enum PhysicalSignificance
	{
		FULL,
		NONE
	}

	public enum DragModel
	{
		DEFAULT,
		CONIC,
		CYLINDRICAL,
		SPHERICAL,
		CUBE,
		NONE
	}

	public class ReflectedAttributes
	{
		public string className;

		public int classID;

		public KSPModule[] kspModules;

		public PartInfo[] partInfo;

		public FieldInfo[] publicFields;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReflectedAttributes(Type type)
		{
			throw null;
		}
	}

	public enum HighlightType
	{
		Disabled,
		OnMouseOver,
		AlwaysOn
	}

	public delegate void OnActionDelegate(Part p);

	public delegate void voidPartDelegate(Part p);

	public enum AutoStrutMode
	{
		Off,
		Root,
		Heaviest,
		Grandparent,
		ForceRoot,
		ForceHeaviest,
		ForceGrandparent
	}

	public enum ShowRigidAttachmentOption
	{
		Never,
		Editor,
		Always
	}

	public struct ForceHolder
	{
		public Vector3d force;

		public Vector3d pos;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ForceHolder(Vector3d f, Vector3d p)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__323 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CStart_003Ed__323(int _003C_003E1__state)
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
	private sealed class _003CWaitForStart_003Ed__362 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CWaitForStart_003Ed__362(int _003C_003E1__state)
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
	private sealed class _003CHandleMouseOver_003Ed__743 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CHandleMouseOver_003Ed__743(int _003C_003E1__state)
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
	private sealed class _003CRecheckShielding_003Ed__794 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CRecheckShielding_003Ed__794(int _003C_003E1__state)
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
	private sealed class _003CSecureAutoStruts_003Ed__837 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CSecureAutoStruts_003Ed__837(int _003C_003E1__state)
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
	private sealed class _003CSetJoints_003Ed__856 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part _003C_003E4__this;

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
		public _003CSetJoints_003Ed__856(int _003C_003E1__state)
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

	public static LayerMask layerMask;

	public static List<Part> allParts;

	public Vessel vessel;

	public ShipConstruct ship;

	public List<Part> editorLinks;

	public bool editorFirstTimePlaced;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002375")]
	public bool sameVesselCollision;

	public List<Part> symmetryCounterparts;

	public bool isClone;

	public Part originalPart;

	public int stackSymmetry;

	public SymmetryMethod symMethod;

	public Part potentialParent;

	public AttachModes attachMode;

	public ProtoPartSnapshot protoPartSnapshot;

	public AvailablePart partInfo;

	public ModulePartVariants variants;

	public PartVariant baseVariant;

	private List<ModuleAnimateGeneric> moduleAnimateGenerics;

	[Header("IDs")]
	public uint persistentId;

	public uint craftID;

	public uint flightID;

	public uint missionID;

	public uint launchID;

	public Part parent;

	public List<Part> fuelLookupTargets;

	public List<Part> children;

	public Transform partTransform;

	private CrashObjectName crashObjectName;

	[SerializeField]
	protected PartStates state;

	public PartStates ResumeState;

	public PartStates PreFailState;

	[Space(10f)]
	[Header("Staging")]
	public int stageOffset;

	public int childStageOffset;

	public int manualStageOffset;

	public int defaultInverseStage;

	public int inverseStage;

	public int inStageIndex;

	public int originalStage;

	public int separationIndex;

	public string stagingIcon;

	public bool inverseStageCarryover;

	public ProtoStageIcon stackIcon;

	public StackIconGrouping stackIconGrouping;

	protected bool connected;

	public bool frozen;

	protected bool attached;

	protected bool compund;

	public Vessel.ControlLevel isControlSource;

	public int PhysicsSignificance;

	public PhysicalSignificance physicalSignificance;

	public PhysicalSignificance previousPhysicalSignificance;

	public int collisionEnhancerSkipFrames;

	public bool isPersistent;

	public bool started;

	public bool editorStarted;

	public string InternalModelName;

	public int CrewCapacity;

	public double crewRespawnTime;

	public float habitableVolume;

	public bool crewTransferAvailable;

	public List<ProtoCrewMember> protoModuleCrew;

	public InternalModel internalModel;

	public Transform airlock;

	public bool noAutoEVAAny;

	public bool noAutoEVAMulti;

	public bool hasKerbalOnLadder;

	public float hatchObstructionCheckOutwardDistance;

	public float hatchObstructionCheckInwardOffset;

	public float hatchObstructionCheckInwardDistance;

	public float hatchObstructionCheckSphereRadius;

	public float hatchObstructionCheckSphereOffset;

	public CollisionEnhancer collisionEnhancer;

	public PQS_PartCollider terrainCollider;

	public PartBuoyancy partBuoyancy;

	public List<PartModule> dockingPorts;

	public Vector3 orgPos;

	public Quaternion orgRot;

	public Vector3 attPos;

	public Vector3 attPos0;

	public Quaternion attRotation;

	public Quaternion attRotation0;

	public Quaternion initRotation;

	public PartJoint attachJoint;

	public float breakingForce;

	public float breakingTorque;

	public float crashTolerance;

	public double gTolerance;

	public bool rigidAttachment;

	public float tempExplodeChance;

	public float gExplodeChance;

	public float presExplodeChance;

	[KSPField]
	public bool applyKerbalMassModification;

	public float mass;

	public float prefabMass;

	public bool needPrefabMass;

	public double resourceThermalMass;

	public float resourceMass;

	public double physicsMass;

	public double thermalMass;

	public double thermalMassReciprocal;

	public double thermalMassModifier;

	internal float moduleMass;

	internal float inventoryMass;

	internal float kerbalMass;

	internal float kerbalInventoryMass;

	internal float kerbalResourceMass;

	public float partSeatMassReduction;

	public double temperature;

	public double maxTemp;

	public double maxPressure;

	public float gaugeThresholdMult;

	public float edgeHighlightThresholdMult;

	public float blackBodyRadiationAlphaMult;

	public double heatConductivity;

	public double heatConvectiveConstant;

	public double emissiveConstant;

	public double absorptiveConstant;

	public PartThermalData ptd;

	public double skinMassPerArea;

	public double skinThermalMassModifier;

	public double skinThermalMass;

	public double skinThermalMassRecip;

	public double skinMaxTemp;

	public double skinUnexposedTemperature;

	public double skinUnexposedExternalTemp;

	public double skinExposedArea;

	public double skinExposedAreaFrac;

	public double skinExposedMassMult;

	public double skinUnexposedMassMult;

	public double skinSkinConductionMult;

	public double skinInternalConductionMult;

	public double skinToInternalFlux;

	protected bool explodeOverheat;

	public double radiatorMax;

	public double radiatorCritical;

	public double radiatorHeadroom;

	public Vector3 CoMOffset;

	public Vector3 CoPOffset;

	public Vector3 CoLOffset;

	[KSPField(guiFormat = "S", guiActive = false, guiName = "Part")]
	public string FailureState;

	private bool _canFail;

	public Vector3 CenterOfBuoyancy;

	public Vector3 CenterOfDisplacement;

	public float buoyancy;

	public bool buoyancyUseSine;

	public bool angularDragByFI;

	public float boundsMultiplier;

	public Vector3 boundsCentroidOffset;

	public string buoyancyUseCubeNamed;

	public double submergedPortion;

	public double submergedDynamicPressurekPa;

	public double minDepth;

	public double maxDepth;

	public double depth;

	public double submergedDragScalar;

	public double submergedLiftScalar;

	public Rigidbody rb;

	public Rigidbody servoRb;

	public bool packed;

	[Header("DragCubes")]
	[Space(10f)]
	public DragModel dragModel;

	public float maximum_drag;

	public float minimum_drag;

	[SerializeField]
	private DragCubeList dragCubes;

	[SerializeField]
	private bool drawDragCubeGizmo;

	public Vector3 dragReferenceVector;

	public Vector3 surfaceAreas;

	public float angularDrag;

	public float waterAngularDragMultiplier;

	private Vector3 inertiaTensor;

	public double dynamicPressurekPa;

	public double staticPressureAtm;

	public double atmDensity;

	public double skinTemperature;

	public double analyticSkinInsulationFactor;

	public double analyticInternalInsulationFactor;

	public Vector3 dragVector;

	public float dragVectorSqrMag;

	public float dragVectorMag;

	public Vector3 dragVectorDir;

	public Vector3 dragVectorDirLocal;

	public float dragScalar;

	public double machNumber;

	public bool hasLiftModule;

	public float bodyLiftMultiplier;

	public bool bodyLiftOnlyUnattachedLift;

	public bool bodyLiftOnlyUnattachedLiftActual;

	public string bodyLiftOnlyAttachName;

	public ILiftProvider bodyLiftOnlyProvider;

	public float bodyLiftScalar;

	public Vector3 bodyLiftLocalPosition;

	public Vector3 bodyLiftLocalVector;

	public double radiativeArea;

	public double exposedArea;

	public double aerodynamicArea;

	public string customPartData;

	public List<FXGroup> fxGroups;

	public float explosionPotential;

	public bool ActivatesEvenIfDisconnected;

	public bool fuelCrossFeed;

	public Part editorCollision;

	public Collider collider;

	public AsteroidCollider asteroidCollider;

	private bool partTweakerSelected;

	public float scaleFactor;

	public float rescaleFactor;

	public Callback OnJustAboutToDie;

	public Callback OnJustAboutToBeDestroyed;

	public Callback OnEditorAttach;

	public Callback OnEditorDetach;

	public Callback OnEditorDestroy;

	public bool stageAfter;

	public bool stageBefore;

	public bool skipColliderIgnores;

	private bool listenersSet;

	public bool hasHeiarchyModel;

	public string initialVesselName;

	public VesselType vesselType;

	public string flagURL;

	private PartValues partValues;

	private MaterialColorUpdater temperatureRenderer;

	public bool overrideSkillUpdate;

	public string overrideSkillUpdateModules;

	public List<string> partRendererBoundsIgnore;

	public List<IConstruction> constructionModules;

	private Transform referenceTransform;

	internal UIPartActionWindow _partActionWindow;

	public GameObject surfaceAttachGO;

	[Space(10f)]
	[Header("Contacts/Landed")]
	public DictionaryValueList<Collider, int> currentCollisions;

	public bool GroundContact;

	public bool PermanentGroundContact;

	public bool WaterContact;

	private const string untagged = "Untagged";

	public Vector3 vel;

	internal List<Collider> cargoColliders;

	private bool checkAfterResetCollisions;

	protected Vector3 posBackup;

	protected Vector3 velBackup;

	protected Vector3 angVelBackup;

	internal double partWeight;

	private ModuleCargoPart cpCache;

	private float cpMass;

	private ProtoCrewMember[] partCrew;

	public static voidPartDelegate CheckPartTemp;

	public static voidPartDelegate CheckPartPressure;

	public static voidPartDelegate CheckPartG;

	private ProtoStageIconInfo tempIndicator;

	private bool needsCollidersReset;

	[Header("Fuel")]
	[Space(10f)]
	public static uint fuelRequestID;

	public uint lastFuelRequestId;

	public string NoCrossFeedNodeKey;

	protected List<Part> resourceTargets;

	private List<MeshRenderer> modelMeshRenderersCache;

	private List<SkinnedMeshRenderer> modelSkinnedMeshRenderersCache;

	private List<Renderer> modelRenderersCache;

	[HideInInspector]
	[SerializeField]
	private BaseEventList events;

	[SerializeField]
	[HideInInspector]
	private BaseFieldList fields;

	[SerializeField]
	[HideInInspector]
	private BaseActionList actions;

	[HideInInspector]
	[SerializeField]
	private PartModuleList modules;

	protected static Dictionary<Type, ReflectedAttributes> reflectedAttributeCache;

	private Dictionary<Type, PartModule> cachedModules;

	private Dictionary<Type, List<PartModule>> cachedModuleLists;

	[Space(10f)]
	[Header("AttachNodes")]
	public AttachRules attachRules;

	public List<AttachNode> attachNodes;

	public AttachNode srfAttachNode;

	public AttachNodeMethod attachMethod;

	public AttachNode topNode;

	[SerializeField]
	private bool drawAttachNodeGizmo;

	private EffectList effects;

	public string partName;

	private int lastRequestID;

	private static int ResourceRequestID;

	[Space(10f)]
	[Header("Resources")]
	public PartSet crossfeedPartSet;

	[SerializeField]
	public PartSet simulationCrossfeedPartSet;

	[SerializeField]
	private PartResourceList _resources;

	private PartResourceList _simulationResources;

	public int resourcePriorityOffset;

	public bool resourcePriorityUseParentInverseStage;

	public bool alwaysShowResourcePriority;

	public bool fuelFlowOverlayEnabled;

	public double resourceRequestRemainingThreshold;

	public double stackPriThreshold;

	public Vector3 mirrorAxis;

	public Vector3 mirrorRefAxis;

	public Vector3 mirrorVector;

	public bool isMirrored;

	private Highlighter hl;

	public static Color defaultHighlightPart;

	public static Color defaultHighlightNone;

	public static float defaultHighlightFalloff;

	public HighlightType highlightType;

	public Color highlightColor;

	private bool highlightActive;

	private bool recurseHighlight;

	private List<Renderer> highlightRenderer;

	private MaterialPropertyBlock _mpb;

	private bool rendererlistscreated;

	private Color currentHighlightColor;

	private bool highlightBlocked;

	private bool mouseEntered;

	private bool mouseOver;

	private OnActionDelegate onMouseEnter;

	private OnActionDelegate onMouseExit;

	private OnActionDelegate onMouseDown;

	public Vector3 moduleSize;

	public Vector3 prefabSize;

	public ArrowPointer dragArrowPtr;

	public ArrowPointer bodyLiftArrowPtr;

	public bool aeroDisplayWasActive;

	public double thermalConvectionFlux;

	public double thermalConductionFlux;

	public double thermalRadiationFlux;

	public double thermalInternalFlux;

	public double thermalInternalFluxPrevious;

	public double thermalSkinFlux;

	public double thermalSkinFluxPrevious;

	public double thermalExposedFlux;

	public double thermalExposedFluxPrevious;

	public string isShieldedDisplay;

	private BaseField shieldedFld;

	public static bool AlwaysRecheckShielding;

	public bool recheckShielding;

	private bool partIsShielded;

	[SerializeField]
	public List<IAirstreamShield> airstreamShields;

	public bool canAimCamera;

	public bool stagingOn;

	public bool stagingIconAlwaysShown;

	public static voidPartDelegate UpgradeStatsDel;

	private List<PartJoint> autoStrutJoints;

	private List<VectorLine> autoStrutLines;

	private Vessel autoStrutVessel;

	private bool autoStrutCycling;

	private bool autoStrutQuickViz;

	private float autoStrutQuickVizStart;

	private const float autoStrutQuickVizDuration = 0.2f;

	private const float autoStrutQuickVizHold = 0.3f;

	private const float autoStrutQuickVizFade = 0.5f;

	private float autoStrutHighMass;

	private Vessel autoStrutCacheVessel;

	private float autoStrutCacheTick;

	public Vector3 strutOffset;

	public AutoStrutMode autoStrutMode;

	public bool autoStrutExcludeParent;

	public bool autoStrutEnableOptionFlight;

	public bool autoStrutEnableOptionEditor;

	public Coroutine cyclingAutoStruts;

	public ShowRigidAttachmentOption showRigidOption;

	public List<ForceHolder> forces;

	public Vector3d force;

	public Vector3d torque;

	public VesselNaming vesselNaming;

	private VesselRenameDialog renameDialog;

	[UI_Label]
	[KSPField(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003392")]
	protected string vesselNamingDisplayName;

	[KSPField(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003393")]
	[UI_Label]
	protected string vesselNamingDisplayPriority;

	private static string cacheAutoLOC_439839;

	private static string cacheAutoLOC_439840;

	private static string cacheAutoLOC_7001406;

	private static string cacheAutoLOC_211269;

	private static string cacheAutoLOC_211272;

	private static string cacheAutoLOC_211274;

	private static string cacheAutoLOC_7000027;

	private static string cacheAutoLOC_211097;

	public bool isVesselEVA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ModuleAnimateGeneric> ModuleAnimateGenerics
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part localRoot
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartStates State
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

	public bool hasStagingIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isAttached
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

	public bool isCompund
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isAttachable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isControllable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool NoAutoEVA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WCoM
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanFail
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

	public Rigidbody Rigidbody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DragCubeList DragCubes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PartTweakerSelected
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

	public Orbit orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartValues PartValues
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIPartActionWindow PartActionWindow
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Landed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Splashed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ClassName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ClassID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseEventList Events
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseFieldList Fields
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseActionList Actions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartModuleList Modules
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ReflectedAttributes PartAttributes
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

	public EffectList Effects
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartResourceList Resources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartResourceList SimulationResources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Highlighter highlighter
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

	public bool HighlightActive
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

	public bool RecurseHighlight
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

	public List<Renderer> HighlightRenderer
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

	public MaterialPropertyBlock mpb
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool MouseOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ShieldedFromAirstream
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

	public Part RigidBodyPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Part()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6005080")]
	public void ToggleSameVesselInteraction(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6005078")]
	public void SetSameVesselInteraction(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6005079")]
	public void RemoveSameVesselInteraction(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part getSymmetryCounterPart(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isSymmetryCounterPart(Part cPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isKerbalEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isCargoPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isInventoryPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isGroundDeployable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isLaunchClamp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isAnchoredDecoupler(out ModuleAnchoredDecoupler moduleAnchoredDecoupler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isDecoupler(out ModuleDecouple moduleDecoupler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isDockingPort(out ModuleDockingNode dockingPort)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isAirIntake(out ModuleResourceIntake intake)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isSolarPanel(out ModuleDeployableSolarPanel solarPanel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRadiator(out ModuleDeployableRadiator radiator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isAntenna(out ModuleDeployableAntenna antenna)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isGenerator(out ModuleGenerator generator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isEngine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isEngine(out List<ModuleEngines> engines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRobotic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isRobotic(out IRoboticServo servo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticRotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isRoboticRotor(out ModuleRoboticServoRotor rotor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticHinge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isRoboticHinge(out ModuleRoboticServoHinge hinge)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticPiston()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isRoboticPiston(out ModuleRoboticServoPiston piston)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticRotationServo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isRoboticRotationServo(out ModuleRoboticRotationServo rotationServo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isChildOfRoboticRotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isChildOfRoboticRotor(out ModuleRoboticServoRotor rotor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isControlSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isControlSurface(out ModuleControlSurface controlSurface)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isBaseServo(out BaseServo servo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isRoboticController(out ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isKerbalSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool hasWheelDamage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool hasWheelDamage(out ModuleWheelDamage wheelDamage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isTrackingShipConstructIDChanges(out List<IShipConstructIDChanges> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXGroup findFxGroup(string groupID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Transform GetReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Part GetReferenceParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Collider[] GetPartColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Renderer[] GetPartRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetReferenceTransform(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RelinkPrefab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__323))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckBodyLiftAttachment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckTransferDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SameVesselCollision(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnConstructionModeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnConstructionModeFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnWillBeCopied(bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnWasCopied(Part newPart, bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCopy(Part original, bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateAttachJoint(AttachModes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Collision Ignores")]
	public void ResetCollisionIgnores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCollisionIgnores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnIVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnIVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrewmember(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddCrewmember(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddCrewmemberAt(ProtoCrewMember crew, int seatIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewCountOfExperienceEffect<T>() where T : ExperienceEffect
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoCrewMember> GetCrewOfExperienceEffect<T>() where T : ExperienceEffect
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void freeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void unfreeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setParent(Part p = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void clearParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void addChild(Part child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void removeChild(Part child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool hasIndirectChild(Part tgtPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool hasIndirectParent(Part tgtPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onAttach(Part parent, bool first = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAttachFlight(Part parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitForStart_003Ed__362))]
	private IEnumerator WaitForStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onDetach(bool first = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDetachFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Activate")]
	public void force_activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void force_activate(bool playFX)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool activate(int currentStage, Vessel activeVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void decouple(float breakForce = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Decoupling(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindNonPhysicslessChildren(ref List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part FindNonPhysicslessParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PromoteToPhysicalPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetPhysicslessChildMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode FindPartThroughNodes(Part tgtPart, Part src = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void disconnect(bool controlledSeparation = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanSymmetryVesselReferencesRecursively()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanSymmetryReferences()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanSymmetryVesselReferences()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetLayer(GameObject obj, int layer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartJointBreak(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Couple(Part tgtPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHierarchyRoot(Part root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateOrgPosAndRot(Part newRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Undock(DockedVesselInfo newVesselInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCollisionEnter(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LandedCollisionChecks(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCollisionExit(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckOtherGroundContact(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckCollision(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HandleCollision(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTouchDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSplashDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLiftOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkLanded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkSplashed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void checkPermanentLandedAt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Pack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetJoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResumeVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableCollisions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableCollisions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDetectCollisions(bool setState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnTriggerEnter(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnTriggerExit(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCollisionStay(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCollisions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCollisionsCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOpacity(float opacity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetShader(string shader)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void propagateControlUpdate(FlightCtrlState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsUnderConstructionWeightLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void _CheckPartTemp(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void _CheckPartPressure(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void _CheckPartG(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ValidateInertiaTensor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HeatGaugeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetMPB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScheduleSetCollisionIgnores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void explode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void explode(float offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Die")]
	public void Die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void onBackup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void onFlightStateSave(Dictionary<string, KSPParseable> partDataCollection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void onFlightStateLoad(Dictionary<string, KSPParseable> parsedData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void onEditorStartTweak()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void onEditorEndTweak()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onStartComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onCopy(Part original, bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onWillBeCopied(bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onWasCopied(Part copyPart, bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartAttach(Part parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartDetach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onFlightStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onFlightStartAtLaunchPad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onDecouple(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onDisconnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onJointDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onJointReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartLiftOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartTouchdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartSplashdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool onPartActivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onCtrlUpd(FlightCtrlState s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onGameResume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onEditorUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onActiveFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onActiveUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartDeactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartExplode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onPartDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint getFuelReqId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use Part.RequestResource instead.")]
	public virtual bool RequestFuel(Part source, float amount, uint reqId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use Part.GetConnectedResources instead.")]
	public virtual bool FindFuel(Part source, List<Part> fuelSources, uint reqId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use Part.TransferResource instead.")]
	public virtual bool DrainFuel(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use Part.RequestResource instead.")]
	public virtual bool RequestRCS(float amount, int earliestStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string drawStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string OnGetStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Functional behaviour should really be happening in PartModules now. In any case, this method's been replaced with OnGetStats, where you just return the string.")]
	public virtual void OnDrawStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos_DragCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part FindChildPart(string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part FindChildPart(string childName, bool recursive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindChildPart(Part parent, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindChildPart<T>() where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindChildPart<T>(bool recursive) where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T FindChildPart<T>(Part parent) where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] FindChildParts<T>() where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] FindChildParts<T>(bool recursive) where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindChildParts<T>(Part parent, List<T> tList) where T : Part
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform FindModelTransform(string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindHeirarchyTransform(Transform parent, string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform FindModelTransformByLayer(string layerName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindHeirarchyTransformByLayer(Transform parent, string layerName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform[] FindModelTransforms(string childName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform[] FindModelTransformsWithTag(string tag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindHeirarchyTransforms(Transform parent, string childName, List<Transform> tList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindHeirarchyTransformsByTag(Transform parent, string tag, List<Transform> tList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindModelComponent<T>() where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindModelComponent<T>(string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T FindModelComponent<T>(Transform parent, string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> FindModelComponents<T>() where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> FindModelComponents<T>(string childName) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindModelComponents<T>(Transform parent, string childName, List<T> tList) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetAllModelComponentCacheLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MeshRenderer> FindModelMeshRenderersCached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetModelMeshRenderersCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<SkinnedMeshRenderer> FindModelSkinnedMeshRenderersCached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetModelSkinnedMeshRenderersCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Renderer> FindModelRenderersCached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetModelRenderersCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation FindModelAnimator(string animatorName, string clipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Animation FindModelAnimator(Transform modelTransform, string animatorName, string clipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation[] FindModelAnimators(string clipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation FindModelAnimator(string clipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Animation[] FindModelAnimators()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModularSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ReflectedAttributes GetReflectedAttributes(Type partModuleType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnActivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnDeactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule.StartState GetModuleStartState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnStartFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesBeforePartAttachJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModulesOnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitializeModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule AddModule(string moduleName, bool forceAwake = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule AddModule(ConfigNode node, bool forceAwake = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule LoadModule(ConfigNode node, ref int moduleIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveModule(PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearModuleReferenceCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindModuleImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasModuleImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindParentModuleImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> FindModulesImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddAttachNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAttachNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSrfAttachNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupAttachNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode FindAttachNode(string nodeId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode[] FindAttachNodes(string partialNodeId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode FindAttachNodeByPart(Part connectedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EffectSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitializeEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadEffects(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveEffects(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Effect(string effectName, int transformIdx = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Effect(string effectName, float effectPower, int transformIdx = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttributeSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendEvent(string eventName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendEvent(string eventName, BaseEventDetails data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendEvent(string eventName, BaseEventDetails data, int maxDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<BaseEvent> CreateEventList(int eventID, int maxDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateEventListRecursive(int eventID, int requestID, List<BaseEvent> eventList, int depth, int maxDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int NewRequestID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AlreadyProcessedRequest(int requestID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupSimulationResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetSimulationResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetSimulationResources(PartResourceList sourceList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetSimulation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResource AddResource(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetResource(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResource(PartResource res)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResource(string rName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResource(int resID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete]
	public virtual float RequestResource(int resourceID, float demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete]
	public virtual float RequestResource(string resourceName, float demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(int resourceID, double demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(int resourceID, double demand, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(string resourceName, double demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(string resourceName, double demand, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(int resourceID, double demand, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(int resourceID, double demand, ResourceFlowMode flowMode, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(string resourceName, double demand, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(string resourceName, double demand, ResourceFlowMode flowMode, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double requestResource(Part origin, int resourceID, ResourceFlowMode flowMode, double demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double requestResource(Part origin, int resourceID, ResourceFlowMode flowMode, double demand, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetResourceMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetResourceMass(bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetResourceMass(out float thermalMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetResourceMass(out double thermalMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double TransferResource(int resourceID, double amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double TransferResource(PartResource resource, double amount, Part other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double TransferResource(PartResource resource, double amount, Part other, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int resourceID, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int resourceID, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, bool simulate, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanCrossfeed(Part target, string resName, ResourceFlowMode flow = ResourceFlowMode.NULL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanCrossfeed(Part target, int resourceID, ResourceFlowMode flow = ResourceFlowMode.NULL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Component GetComponentUpwards(string type, GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetComponentUpwards<T>(GameObject obj) where T : Component
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FromGO(GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion PartToVesselSpaceRot(Quaternion rot, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 PartToVesselSpacePos(Vector3 pos, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 PartToVesselSpaceDir(Vector3 dir, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion VesselToPartSpaceRot(Quaternion rot, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 VesselToPartSpacePos(Vector3 pos, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 VesselToPartSpaceDir(Vector3 dir, Part p, Vessel v, PartSpaceMode space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GetPartsOutTo(Part part, HashSet<Part> parts, int maxLinks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateMirroring()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMirror(Vector3 mirrorVector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlightType(HighlightType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlightColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlightColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlightDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlight(bool active, bool recursive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Highlight(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Highlight(Color highlightColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateRendererLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighlightRecursive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighlightRecursive(Color highlightColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partHideUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partShowUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCurrentMousePartChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseIsOver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CHandleMouseOver_003Ed__743))]
	private IEnumerator HandleMouseOver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseHasExited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMouseOver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnMouseEnter(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnMouseEnter(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnMouseExit(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnMouseExit(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnMouseDown(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnMouseDown(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalModel AddInternalPart(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateInternalModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalOnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCosts(float defaultCost, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetModuleSize(Vector3 defaultSize, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAeroDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddThermalFlux(double kilowatts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSkinThermalFlux(double kilowatts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddExposedThermalFlux(double kilowatts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<IAirstreamShield> AddShield(IAirstreamShield shd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveShield(IAirstreamShield shd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnShieldModified(IAirstreamShield shd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRecheckShielding_003Ed__794))]
	protected IEnumerator RecheckShielding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GainCameraAim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoseCameraAim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActiveUncommand = true, guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = true, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6001315")]
	public void AimCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActiveUncommand = true, guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6001316")]
	public void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUncommand = true, guiActive = true, guiName = "#autoLOC_6001317")]
	public void SpawnTransferDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetResourcePriority()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetPri()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeResourcePriority(int offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateStageability(bool propagate, bool iconUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6002279")]
	public virtual void ShowUpgradeStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AllowAutoStruts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001318")]
	public void ToggleAutoStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAutoStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAutoStrutText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEvent(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CycleAutoStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasAutoStrutDefined()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSecureAutoStruts_003Ed__837))]
	private IEnumerator SecureAutoStruts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartJoint SecureAutoStrut(Part anchor, AttachNode nodeToParent, AttachNode nodeFromParent, bool srfAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawAutoStrutLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReleaseAutoStruts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckAutoStruts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckTargetAutoStruts(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part GetAutoStrutAnchor(out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetMassivePart(Part start, out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached, params Part[] excluded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetMassiveParentPart(Part original, Part child, ref Part highestPart, ref float highestMass, ref Part lastPart, ref AttachNode nodeToParent, ref AttachNode nodeFromParent, ref bool srfAttached, params Part[] excluded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetMassiveChildPart(Part original, Part parent, List<Part> children, ref Part highestPart, ref float highestMass, ref Part lastPart, ref AttachNode nodeToParent, ref AttachNode nodeFromParent, ref bool srfAttached, Part ignoredChild, params Part[] excluded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MassivePartCheck(Part original, Part p, ref Part highestPart, ref float highestMass, params Part[] excluded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasFreePivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasFreePivotBetweenRoot(out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AttachNode AttachedPartNode(Part previousPart, Part currentPart, out bool srfAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasFreePivotBetweenParentPart(Part endPart, out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001325")]
	public void ToggleRigidAttachment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyRigidAttachment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetJoints_003Ed__856))]
	private IEnumerator SetJoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupRigidAttachmentUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddForce(Vector3d vec)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddImpulse(Vector3d vec)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddForceAtPosition(Vector3d vec, Vector3d pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddTorque(Vector3d vec)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindTransformInChildrenExplicit(Transform parent, Transform find)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValidContractObjective(string objectiveType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStageIconDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PartRepair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActiveUncommand = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003140")]
	public void SetVesselNaming()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselNamingAccept(string newVesselName, VesselType newVesselType, int newPriority)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RefreshVesselNamingPAWDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselNamingDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselNamingRemove()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003305")]
	public void RemoveFromSymmetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetRemoveSymmetryVisibililty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float GetBoundsPoints(Vector3 groundNormal, out float centerPointOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CacheIConstructionPartModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PartCanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PartCanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PartCanBeRotated()
	{
		throw null;
	}
}
