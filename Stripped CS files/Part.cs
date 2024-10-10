using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CommNet;
using Expansions;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;
using Experience;
using Highlighting;
using ModuleWheels;
using ns11;
using ns2;
using ns8;
using ns9;
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

		public ReflectedAttributes(Type type)
		{
			className = type.Name;
			classID = className.GetHashCode();
			kspModules = (KSPModule[])type.GetType().GetCustomAttributes(typeof(KSPModule), inherit: true);
			partInfo = (PartInfo[])type.GetCustomAttributes(typeof(PartInfo), inherit: true);
			publicFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
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

		public ForceHolder(Vector3d f, Vector3d p)
		{
			force = f;
			pos = p;
		}
	}

	public static LayerMask layerMask = LayerUtil.DefaultEquivalent | 0x4000000 | 2;

	public static List<Part> allParts = new List<Part>();

	public Vessel vessel;

	public ShipConstruct ship;

	public List<Part> editorLinks = new List<Part>();

	public bool editorFirstTimePlaced;

	[UI_Toggle(disabledText = "#autoLOC_439840", scene = UI_Scene.All, enabledText = "#autoLOC_439839", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8002375")]
	public bool sameVesselCollision;

	public List<Part> symmetryCounterparts = new List<Part>();

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

	public List<ModuleAnimateGeneric> moduleAnimateGenerics;

	[Header("IDs")]
	public uint persistentId;

	public uint craftID;

	public uint flightID;

	public uint missionID;

	public uint launchID;

	public Part parent;

	public List<Part> fuelLookupTargets = new List<Part>();

	public List<Part> children = new List<Part>();

	public Transform partTransform;

	public CrashObjectName crashObjectName;

	[SerializeField]
	public PartStates state;

	public PartStates ResumeState;

	public PartStates PreFailState;

	[Space(10f)]
	[Header("Staging")]
	public int stageOffset;

	public int childStageOffset;

	public int manualStageOffset = -1;

	public int defaultInverseStage;

	public int inverseStage;

	public int inStageIndex = -1;

	public int originalStage;

	public int separationIndex;

	public string stagingIcon = "";

	public bool inverseStageCarryover = true;

	public ProtoStageIcon stackIcon;

	public StackIconGrouping stackIconGrouping = StackIconGrouping.SYM_COUNTERPARTS;

	public bool connected;

	public bool frozen;

	public bool attached;

	public bool compund;

	public Vessel.ControlLevel isControlSource;

	public int PhysicsSignificance = -1;

	public PhysicalSignificance physicalSignificance;

	public PhysicalSignificance previousPhysicalSignificance;

	public int collisionEnhancerSkipFrames = 5;

	public bool isPersistent;

	public bool started;

	public bool editorStarted;

	public string InternalModelName = "";

	public int CrewCapacity;

	public double crewRespawnTime;

	public float habitableVolume;

	public bool crewTransferAvailable = true;

	public List<ProtoCrewMember> protoModuleCrew = new List<ProtoCrewMember>();

	public InternalModel internalModel;

	public Transform airlock;

	public bool noAutoEVAAny;

	public bool noAutoEVAMulti;

	public bool hasKerbalOnLadder;

	public float hatchObstructionCheckOutwardDistance = 1f;

	public float hatchObstructionCheckInwardOffset = 1f;

	public float hatchObstructionCheckInwardDistance = 1.1f;

	public float hatchObstructionCheckSphereRadius = 0.22f;

	public float hatchObstructionCheckSphereOffset = 0.1f;

	public CollisionEnhancer collisionEnhancer;

	public PQS_PartCollider terrainCollider;

	public PartBuoyancy partBuoyancy;

	public List<PartModule> dockingPorts = new List<PartModule>();

	public Vector3 orgPos;

	public Quaternion orgRot;

	public Vector3 attPos = Vector3.zero;

	public Vector3 attPos0 = Vector3.zero;

	public Quaternion attRotation = Quaternion.identity;

	public Quaternion attRotation0 = Quaternion.identity;

	public Quaternion initRotation = Quaternion.identity;

	public PartJoint attachJoint;

	public float breakingForce = 22f;

	public float breakingTorque = 22f;

	public float crashTolerance = 9f;

	public double gTolerance = 50.0;

	public bool rigidAttachment;

	public float tempExplodeChance = 0.8f;

	public float gExplodeChance = 0.8f;

	public float presExplodeChance = 0.8f;

	[KSPField]
	public bool applyKerbalMassModification = true;

	public float mass = 2f;

	public float prefabMass;

	public bool needPrefabMass = true;

	public double resourceThermalMass;

	public float resourceMass;

	public double physicsMass;

	public double thermalMass;

	public double thermalMassReciprocal = 1.0;

	public double thermalMassModifier = 1.0;

	public float moduleMass;

	public float inventoryMass;

	public float kerbalMass;

	public float kerbalInventoryMass;

	public float kerbalResourceMass;

	public float partSeatMassReduction;

	public double temperature = -1.0;

	public double maxTemp = 2000.0;

	public double maxPressure = 4000.0;

	public float gaugeThresholdMult = 1f;

	public float edgeHighlightThresholdMult = 1f;

	public float blackBodyRadiationAlphaMult = 1f;

	public double heatConductivity = 0.12;

	public double heatConvectiveConstant = 1.0;

	public double emissiveConstant = 0.4;

	public double absorptiveConstant = -1.0;

	public PartThermalData ptd;

	public double skinMassPerArea = 1.0;

	public double skinThermalMassModifier = 1.0;

	public double skinThermalMass;

	public double skinThermalMassRecip;

	public double skinMaxTemp = -1.0;

	public double skinUnexposedTemperature;

	public double skinUnexposedExternalTemp = 4.0;

	public double skinExposedArea;

	public double skinExposedAreaFrac;

	public double skinExposedMassMult = 1.0;

	public double skinUnexposedMassMult;

	public double skinSkinConductionMult = 1.0;

	public double skinInternalConductionMult = 1.0;

	public double skinToInternalFlux;

	public bool explodeOverheat;

	public double radiatorMax = 0.25;

	public double radiatorCritical = 0.75;

	public double radiatorHeadroom = 0.25;

	public Vector3 CoMOffset = Vector3.zero;

	public Vector3 CoPOffset = Vector3.zero;

	public Vector3 CoLOffset = Vector3.zero;

	[KSPField(guiFormat = "S", guiActive = false, guiName = "Part")]
	public string FailureState = "Failed";

	public bool _canFail = true;

	public Vector3 CenterOfBuoyancy = Vector3.zero;

	public Vector3 CenterOfDisplacement = Vector3.zero;

	public float buoyancy = 1f;

	public bool buoyancyUseSine = true;

	public bool angularDragByFI = true;

	public float boundsMultiplier = 1f;

	public Vector3 boundsCentroidOffset = Vector3.zero;

	public string buoyancyUseCubeNamed = "";

	public double submergedPortion;

	public double submergedDynamicPressurekPa;

	public double minDepth;

	public double maxDepth;

	public double depth;

	public double submergedDragScalar = 1.0;

	public double submergedLiftScalar;

	public Rigidbody rb;

	public Rigidbody servoRb;

	public bool packed;

	[Header("DragCubes")]
	[Space(10f)]
	public DragModel dragModel = DragModel.CUBE;

	public float maximum_drag = 0.1f;

	public float minimum_drag = 0.1f;

	[SerializeField]
	public DragCubeList dragCubes = new DragCubeList();

	[SerializeField]
	public bool drawDragCubeGizmo;

	public Vector3 dragReferenceVector = Vector3.up;

	public Vector3 surfaceAreas = Vector3.one;

	public float angularDrag = 2f;

	public float waterAngularDragMultiplier = 1f;

	public Vector3 inertiaTensor;

	public double dynamicPressurekPa;

	public double staticPressureAtm;

	public double atmDensity;

	public double skinTemperature;

	public double analyticSkinInsulationFactor = 1.0;

	public double analyticInternalInsulationFactor = 1.0;

	public Vector3 dragVector = Vector3.up;

	public float dragVectorSqrMag;

	public float dragVectorMag;

	public Vector3 dragVectorDir = Vector3.up;

	public Vector3 dragVectorDirLocal = Vector3.up;

	public float dragScalar;

	public double machNumber;

	public bool hasLiftModule;

	public float bodyLiftMultiplier = 1f;

	public bool bodyLiftOnlyUnattachedLift;

	public bool bodyLiftOnlyUnattachedLiftActual;

	public string bodyLiftOnlyAttachName;

	public ILiftProvider bodyLiftOnlyProvider;

	public float bodyLiftScalar;

	public Vector3 bodyLiftLocalPosition = Vector3.zero;

	public Vector3 bodyLiftLocalVector = Vector3.zero;

	public double radiativeArea;

	public double exposedArea;

	public double aerodynamicArea;

	public string customPartData = "";

	public List<FXGroup> fxGroups = new List<FXGroup>();

	public float explosionPotential = 0.5f;

	public bool ActivatesEvenIfDisconnected = true;

	public bool fuelCrossFeed = true;

	public Part editorCollision;

	public Collider collider;

	public AsteroidCollider asteroidCollider;

	public bool partTweakerSelected;

	public float scaleFactor = 1f;

	public float rescaleFactor = 1.25f;

	public Callback OnJustAboutToDie = delegate
	{
	};

	public Callback OnJustAboutToBeDestroyed = delegate
	{
	};

	public Callback OnEditorAttach = delegate
	{
	};

	public Callback OnEditorDetach = delegate
	{
	};

	public Callback OnEditorDestroy = delegate
	{
	};

	public bool stageAfter;

	public bool stageBefore;

	public bool skipColliderIgnores;

	public bool listenersSet;

	public bool hasHeiarchyModel;

	public string initialVesselName;

	public VesselType vesselType;

	public string flagURL = "";

	public PartValues partValues = new PartValues();

	public MaterialColorUpdater temperatureRenderer;

	public bool overrideSkillUpdate;

	public string overrideSkillUpdateModules = "ModuleSAS, ModuleWheelSteering";

	public List<string> partRendererBoundsIgnore;

	public List<IConstruction> constructionModules;

	public Transform referenceTransform;

	public UIPartActionWindow _partActionWindow;

	public GameObject surfaceAttachGO;

	[Space(10f)]
	[Header("Contacts/Landed")]
	public DictionaryValueList<Collider, int> currentCollisions = new DictionaryValueList<Collider, int>();

	public bool GroundContact;

	public bool PermanentGroundContact;

	public bool WaterContact;

	public const string untagged = "Untagged";

	public Vector3 vel;

	public List<Collider> cargoColliders = new List<Collider>();

	public bool checkAfterResetCollisions;

	public Vector3 posBackup;

	public Vector3 velBackup;

	public Vector3 angVelBackup;

	public double partWeight;

	public ModuleCargoPart cpCache;

	public float cpMass;

	public ProtoCrewMember[] partCrew;

	public static voidPartDelegate CheckPartTemp = _CheckPartTemp;

	public static voidPartDelegate CheckPartPressure = _CheckPartPressure;

	public static voidPartDelegate CheckPartG = _CheckPartG;

	public ProtoStageIconInfo tempIndicator;

	public bool needsCollidersReset;

	[Header("Fuel")]
	[Space(10f)]
	public static uint fuelRequestID = 0u;

	public uint lastFuelRequestId;

	public string NoCrossFeedNodeKey = "";

	public List<Part> resourceTargets = new List<Part>();

	public List<MeshRenderer> modelMeshRenderersCache;

	public List<SkinnedMeshRenderer> modelSkinnedMeshRenderersCache;

	public List<Renderer> modelRenderersCache;

	[HideInInspector]
	[SerializeField]
	public BaseEventList events;

	[SerializeField]
	[HideInInspector]
	public BaseFieldList fields;

	[SerializeField]
	[HideInInspector]
	public BaseActionList actions;

	[HideInInspector]
	[SerializeField]
	public PartModuleList modules;

	public static Dictionary<Type, ReflectedAttributes> reflectedAttributeCache = new Dictionary<Type, ReflectedAttributes>();

	public Dictionary<Type, PartModule> cachedModules;

	public Dictionary<Type, List<PartModule>> cachedModuleLists;

	[Space(10f)]
	[Header("AttachNodes")]
	public AttachRules attachRules;

	public List<AttachNode> attachNodes = new List<AttachNode>();

	public AttachNode srfAttachNode;

	public AttachNodeMethod attachMethod;

	public AttachNode topNode;

	[SerializeField]
	public bool drawAttachNodeGizmo;

	public EffectList effects;

	public string partName;

	public int lastRequestID;

	public static int ResourceRequestID;

	[Space(10f)]
	[Header("Resources")]
	public PartSet crossfeedPartSet;

	[SerializeField]
	public PartSet simulationCrossfeedPartSet;

	[SerializeField]
	public PartResourceList _resources;

	public PartResourceList _simulationResources;

	public int resourcePriorityOffset;

	public bool resourcePriorityUseParentInverseStage;

	public bool alwaysShowResourcePriority;

	public bool fuelFlowOverlayEnabled;

	public double resourceRequestRemainingThreshold = 1E-12;

	public double stackPriThreshold = 0.001;

	public Vector3 mirrorAxis = Vector3.zero;

	public Vector3 mirrorRefAxis;

	public Vector3 mirrorVector = Vector3.one;

	public bool isMirrored;

	public Highlighter hl;

	public static Color defaultHighlightPart = Highlighter.colorPartHighlightDefault;

	public static Color defaultHighlightNone = new Color32(0, 0, 0, 0);

	public static float defaultHighlightFalloff = 2f;

	public HighlightType highlightType = HighlightType.OnMouseOver;

	public Color highlightColor = defaultHighlightPart;

	public bool highlightActive;

	public bool recurseHighlight;

	public List<Renderer> highlightRenderer;

	public MaterialPropertyBlock _mpb;

	public bool rendererlistscreated;

	public Color currentHighlightColor = defaultHighlightNone;

	public bool highlightBlocked;

	public bool mouseEntered;

	public bool mouseOver;

	public OnActionDelegate onMouseEnter;

	public OnActionDelegate onMouseExit;

	public OnActionDelegate onMouseDown;

	public Vector3 moduleSize = Vector3.zero;

	public Vector3 prefabSize = Vector3.zero;

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

	public string isShieldedDisplay = "";

	public BaseField shieldedFld;

	public static bool AlwaysRecheckShielding = true;

	public bool recheckShielding = true;

	public bool partIsShielded;

	[SerializeField]
	public List<IAirstreamShield> airstreamShields = new List<IAirstreamShield>();

	public bool canAimCamera = true;

	public bool stagingOn = true;

	public bool stagingIconAlwaysShown;

	public static voidPartDelegate UpgradeStatsDel;

	public List<PartJoint> autoStrutJoints = new List<PartJoint>();

	public List<VectorLine> autoStrutLines;

	public Vessel autoStrutVessel;

	public bool autoStrutCycling;

	public bool autoStrutQuickViz;

	public float autoStrutQuickVizStart = float.MinValue;

	public const float autoStrutQuickVizDuration = 0.2f;

	public const float autoStrutQuickVizHold = 0.3f;

	public const float autoStrutQuickVizFade = 0.5f;

	public float autoStrutHighMass;

	public Vessel autoStrutCacheVessel;

	public float autoStrutCacheTick = -1f;

	public Vector3 strutOffset = Vector3.zero;

	public AutoStrutMode autoStrutMode;

	public bool autoStrutExcludeParent = true;

	public bool autoStrutEnableOptionFlight = true;

	public bool autoStrutEnableOptionEditor = true;

	public Coroutine cyclingAutoStruts;

	public ShowRigidAttachmentOption showRigidOption = ShowRigidAttachmentOption.Editor;

	public List<ForceHolder> forces = new List<ForceHolder>(2);

	public Vector3d force = Vector3d.zero;

	public Vector3d torque = Vector3d.zero;

	public VesselNaming vesselNaming;

	public VesselRenameDialog renameDialog;

	[UI_Label]
	[KSPField(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003392")]
	public string vesselNamingDisplayName;

	[KSPField(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003393")]
	[UI_Label]
	public string vesselNamingDisplayPriority;

	public static string cacheAutoLOC_439839;

	public static string cacheAutoLOC_439840;

	public static string cacheAutoLOC_7001406;

	public static string cacheAutoLOC_211269;

	public static string cacheAutoLOC_211272;

	public static string cacheAutoLOC_211274;

	public static string cacheAutoLOC_7000027;

	public static string cacheAutoLOC_211097;

	public bool isVesselEVA
	{
		get
		{
			if (vessel == null)
			{
				return false;
			}
			return vessel.isEVA;
		}
	}

	public List<ModuleAnimateGeneric> ModuleAnimateGenerics
	{
		get
		{
			if (moduleAnimateGenerics == null)
			{
				moduleAnimateGenerics = modules.GetModules<ModuleAnimateGeneric>();
			}
			return moduleAnimateGenerics;
		}
	}

	public Part localRoot
	{
		get
		{
			if (attached && (bool)parent && parent.state != PartStates.DEAD)
			{
				return parent.localRoot;
			}
			return this;
		}
	}

	public PartStates State
	{
		get
		{
			return state;
		}
		set
		{
			if (value == PartStates.FAILED)
			{
				if (CanFail)
				{
					if (state == PartStates.ACTIVE)
					{
						PreFailState = PartStates.IDLE;
					}
					else if (state != PartStates.FAILED)
					{
						PreFailState = state;
					}
					state = value;
				}
			}
			else
			{
				state = value;
			}
		}
	}

	public bool hasStagingIcon => stagingIcon != string.Empty;

	public bool isAttached
	{
		get
		{
			return attached;
		}
		set
		{
			attached = value;
		}
	}

	public bool isCompund => compund;

	public bool isAttachable
	{
		get
		{
			if (attachMode == AttachModes.SRF_ATTACH)
			{
				return true;
			}
			int num = 0;
			int count = attachNodes.Count;
			while (true)
			{
				if (num < count)
				{
					if (attachNodes[num].attachedPart == null)
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
	}

	public bool isControllable
	{
		get
		{
			if (vessel != null)
			{
				return vessel.IsControllable;
			}
			return false;
		}
	}

	public bool NoAutoEVA
	{
		get
		{
			if (!noAutoEVAAny && !noAutoEVAMulti)
			{
				return false;
			}
			if (noAutoEVAAny)
			{
				return true;
			}
			if (noAutoEVAMulti && (vessel == null || vessel.Parts.Count > 1))
			{
				return true;
			}
			return false;
		}
	}

	public Vector3 WCoM
	{
		get
		{
			if (rb != null)
			{
				if (servoRb != null)
				{
					return (rb.worldCenterOfMass * rb.mass + servoRb.worldCenterOfMass * servoRb.mass) / (rb.mass + servoRb.mass);
				}
				return rb.worldCenterOfMass;
			}
			return partTransform.position + partTransform.rotation * CoMOffset;
		}
	}

	public bool CanFail
	{
		get
		{
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
			{
				return _canFail;
			}
			return false;
		}
		set
		{
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
			{
				_canFail = value;
			}
		}
	}

	public Rigidbody Rigidbody
	{
		get
		{
			if (!rb)
			{
				if (!parent)
				{
					return null;
				}
				return parent.Rigidbody;
			}
			return rb;
		}
	}

	public DragCubeList DragCubes => dragCubes;

	public bool PartTweakerSelected
	{
		get
		{
			return partTweakerSelected;
		}
		set
		{
			partTweakerSelected = value;
		}
	}

	public Orbit orbit => vessel.orbit;

	public PartValues PartValues => partValues;

	public UIPartActionWindow PartActionWindow => _partActionWindow;

	public bool Landed => vessel.Landed;

	public bool Splashed => vessel.Splashed;

	public string ClassName => PartAttributes.className;

	public int ClassID => PartAttributes.classID;

	public BaseEventList Events => events;

	public BaseFieldList Fields => fields;

	public BaseActionList Actions => actions;

	public PartModuleList Modules => modules;

	public ReflectedAttributes PartAttributes { get; set; }

	public EffectList Effects => effects;

	public PartResourceList Resources => _resources;

	public PartResourceList SimulationResources => _simulationResources;

	public Highlighter highlighter
	{
		get
		{
			return hl;
		}
		set
		{
			hl = value;
		}
	}

	public bool HighlightActive
	{
		get
		{
			return highlightActive;
		}
		set
		{
			highlightActive = value;
		}
	}

	public bool RecurseHighlight
	{
		get
		{
			return recurseHighlight;
		}
		set
		{
			recurseHighlight = value;
		}
	}

	public List<Renderer> HighlightRenderer
	{
		get
		{
			return highlightRenderer;
		}
		set
		{
			highlightRenderer = value;
		}
	}

	public MaterialPropertyBlock mpb
	{
		get
		{
			if (_mpb == null)
			{
				_mpb = new MaterialPropertyBlock();
			}
			return _mpb;
		}
	}

	public bool MouseOver => mouseOver;

	public bool ShieldedFromAirstream
	{
		get
		{
			return partIsShielded;
		}
		set
		{
			partIsShielded = value;
			isShieldedDisplay = (partIsShielded ? cacheAutoLOC_439839 : cacheAutoLOC_439840);
		}
	}

	public Part RigidBodyPart
	{
		get
		{
			if (!rb)
			{
				if (!parent)
				{
					return null;
				}
				return parent.RigidBodyPart;
			}
			return this;
		}
	}

	[KSPAction("#autoLOC_6005080")]
	public void ToggleSameVesselInteraction(KSPActionParam act)
	{
		sameVesselCollision = !sameVesselCollision;
		GameEvents.OnCollisionIgnoreUpdate.Fire();
	}

	[KSPAction("#autoLOC_6005078")]
	public void SetSameVesselInteraction(KSPActionParam act)
	{
		if (!sameVesselCollision)
		{
			sameVesselCollision = true;
			GameEvents.OnCollisionIgnoreUpdate.Fire();
		}
	}

	[KSPAction("#autoLOC_6005079")]
	public void RemoveSameVesselInteraction(KSPActionParam act)
	{
		if (sameVesselCollision)
		{
			sameVesselCollision = false;
			GameEvents.OnCollisionIgnoreUpdate.Fire();
		}
	}

	public Part getSymmetryCounterPart(int index)
	{
		index %= symmetryCounterparts.Count + 1;
		if (index == 0)
		{
			return this;
		}
		return symmetryCounterparts[index - 1];
	}

	public bool isSymmetryCounterPart(Part cPart)
	{
		int count = symmetryCounterparts.Count;
		Part part;
		do
		{
			if (count-- > 0)
			{
				part = symmetryCounterparts[count];
				continue;
			}
			return false;
		}
		while (!(cPart == part));
		return true;
	}

	public bool isKerbalEVA()
	{
		return HasModuleImplementing<KerbalEVA>();
	}

	public bool isCargoPart()
	{
		return HasModuleImplementing<ModuleCargoPart>();
	}

	public bool isInventoryPart()
	{
		return HasModuleImplementing<ModuleInventoryPart>();
	}

	public bool isGroundDeployable()
	{
		return HasModuleImplementing<ModuleGroundPart>();
	}

	public bool isLaunchClamp()
	{
		return HasModuleImplementing<LaunchClamp>();
	}

	public bool isAnchoredDecoupler(out ModuleAnchoredDecoupler moduleAnchoredDecoupler)
	{
		moduleAnchoredDecoupler = FindModuleImplementing<ModuleAnchoredDecoupler>();
		return moduleAnchoredDecoupler != null;
	}

	public bool isDecoupler(out ModuleDecouple moduleDecoupler)
	{
		moduleDecoupler = FindModuleImplementing<ModuleDecouple>();
		return moduleDecoupler != null;
	}

	public bool isDockingPort(out ModuleDockingNode dockingPort)
	{
		dockingPort = FindModuleImplementing<ModuleDockingNode>();
		return dockingPort != null;
	}

	public bool isAirIntake(out ModuleResourceIntake intake)
	{
		intake = FindModuleImplementing<ModuleResourceIntake>();
		if (intake != null && intake.resourceName != "IntakeAir")
		{
			intake = null;
		}
		return intake != null;
	}

	public bool isSolarPanel(out ModuleDeployableSolarPanel solarPanel)
	{
		solarPanel = FindModuleImplementing<ModuleDeployableSolarPanel>();
		return solarPanel != null;
	}

	public bool isRadiator(out ModuleDeployableRadiator radiator)
	{
		radiator = FindModuleImplementing<ModuleDeployableRadiator>();
		return radiator != null;
	}

	public bool isAntenna(out ModuleDeployableAntenna antenna)
	{
		antenna = FindModuleImplementing<ModuleDeployableAntenna>();
		return antenna != null;
	}

	public bool isGenerator(out ModuleGenerator generator)
	{
		generator = FindModuleImplementing<ModuleGenerator>();
		return generator != null;
	}

	public bool isFairing()
	{
		return HasModuleImplementing<ModuleProceduralFairing>();
	}

	public bool isEngine()
	{
		return HasModuleImplementing<ModuleEngines>();
	}

	public bool isEngine(out List<ModuleEngines> engines)
	{
		engines = FindModulesImplementing<ModuleEngines>();
		if (engines != null)
		{
			return engines.Count > 0;
		}
		return false;
	}

	public bool isParachute()
	{
		return HasModuleImplementing<ModuleParachute>();
	}

	public bool isRobotic()
	{
		IRoboticServo servo;
		return isRobotic(out servo);
	}

	public bool isRobotic(out IRoboticServo servo)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			servo = FindModuleImplementing<IRoboticServo>();
			return servo != null;
		}
		servo = null;
		return false;
	}

	public bool isRoboticRotor()
	{
		ModuleRoboticServoRotor rotor;
		return isRoboticRotor(out rotor);
	}

	public bool isRoboticRotor(out ModuleRoboticServoRotor rotor)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			rotor = FindModuleImplementing<ModuleRoboticServoRotor>();
			return rotor != null;
		}
		rotor = null;
		return false;
	}

	public bool isRoboticHinge()
	{
		ModuleRoboticServoHinge hinge;
		return isRoboticHinge(out hinge);
	}

	public bool isRoboticHinge(out ModuleRoboticServoHinge hinge)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			hinge = FindModuleImplementing<ModuleRoboticServoHinge>();
			return hinge != null;
		}
		hinge = null;
		return false;
	}

	public bool isRoboticPiston()
	{
		ModuleRoboticServoPiston piston;
		return isRoboticPiston(out piston);
	}

	public bool isRoboticPiston(out ModuleRoboticServoPiston piston)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			piston = FindModuleImplementing<ModuleRoboticServoPiston>();
			return piston != null;
		}
		piston = null;
		return false;
	}

	public bool isRoboticRotationServo()
	{
		ModuleRoboticRotationServo rotationServo;
		return isRoboticRotationServo(out rotationServo);
	}

	public bool isRoboticRotationServo(out ModuleRoboticRotationServo rotationServo)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			rotationServo = FindModuleImplementing<ModuleRoboticRotationServo>();
			return rotationServo != null;
		}
		rotationServo = null;
		return false;
	}

	public bool isChildOfRoboticRotor()
	{
		ModuleRoboticServoRotor rotor;
		return isChildOfRoboticRotor(out rotor);
	}

	public bool isChildOfRoboticRotor(out ModuleRoboticServoRotor rotor)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			rotor = FindParentModuleImplementing<ModuleRoboticServoRotor>();
			return rotor != null;
		}
		rotor = null;
		return false;
	}

	public bool isControlSurface()
	{
		ModuleControlSurface controlSurface;
		return isControlSurface(out controlSurface);
	}

	public bool isControlSurface(out ModuleControlSurface controlSurface)
	{
		controlSurface = FindModuleImplementing<ModuleControlSurface>();
		return controlSurface != null;
	}

	public bool isBaseServo(out BaseServo servo)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			servo = FindModuleImplementing<BaseServo>();
			return servo != null;
		}
		servo = null;
		return false;
	}

	public bool isRoboticController()
	{
		ModuleRoboticController controller;
		return isRoboticController(out controller);
	}

	public bool isRoboticController(out ModuleRoboticController controller)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			controller = FindModuleImplementing<ModuleRoboticController>();
			return controller != null;
		}
		controller = null;
		return false;
	}

	public bool isKerbalSeat()
	{
		return HasModuleImplementing<KerbalSeat>();
	}

	public bool hasWheelDamage()
	{
		ModuleWheelDamage wheelDamage;
		return hasWheelDamage(out wheelDamage);
	}

	public bool hasWheelDamage(out ModuleWheelDamage wheelDamage)
	{
		wheelDamage = FindModuleImplementing<ModuleWheelDamage>();
		return wheelDamage != null;
	}

	public bool isTrackingShipConstructIDChanges(out List<IShipConstructIDChanges> modules)
	{
		modules = FindModulesImplementing<IShipConstructIDChanges>();
		if (modules != null)
		{
			return modules.Count > 0;
		}
		return false;
	}

	public FXGroup findFxGroup(string groupID)
	{
		int num = 0;
		int count = fxGroups.Count;
		FXGroup fXGroup;
		while (true)
		{
			if (num < count)
			{
				fXGroup = fxGroups[num];
				if (fXGroup.name == groupID)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return fXGroup;
	}

	public virtual Transform GetReferenceTransform()
	{
		return referenceTransform;
	}

	public virtual Part GetReferenceParent()
	{
		return parent;
	}

	public virtual Collider[] GetPartColliders()
	{
		return partTransform.Find("model").GetComponentsInChildren<Collider>();
	}

	public virtual Renderer[] GetPartRenderers()
	{
		return partTransform.Find("model").GetComponentsInChildren<Renderer>();
	}

	public void SetReferenceTransform(Transform t)
	{
		if (t == null)
		{
			referenceTransform = base.transform;
		}
		else if (!(t == base.transform) && !(FindModelTransform(t.name) != null) && !(base.transform.Find(t.name) != null))
		{
			Debug.LogError("Part Error: Trying to set a reference transform, but transform cannot be found by any method used during loading", base.gameObject);
			if (Application.isEditor)
			{
				Debug.LogError("Here's the transform that was attempted (double click this message)", t);
			}
		}
		else
		{
			referenceTransform = t;
		}
	}

	public virtual void SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		base.transform.position = newPosition;
		base.transform.rotation = newRotation;
	}

	public void OnLoad()
	{
		fxGroups.Add(new FXGroup("prelaunch"));
		fxGroups.Add(new FXGroup("activate"));
		fxGroups.Add(new FXGroup("active"));
		fxGroups.Add(new FXGroup("deactivate"));
		Fields.SetOriginalValue();
		onPartLoad();
	}

	public void RelinkPrefab()
	{
		if (partInfo != null)
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(partInfo.name);
			if (partInfoByName != null)
			{
				partInfo = partInfoByName;
			}
		}
	}

	public void Awake()
	{
		RelinkPrefab();
		partTransform = base.transform;
		craftID = (uint)GetInstanceID();
		ResetAllModelComponentCacheLists();
		ModularSetup();
		modules = new PartModuleList(this);
		cachedModules = new Dictionary<Type, PartModule>();
		cachedModuleLists = new Dictionary<Type, List<PartModule>>();
		if (partRendererBoundsIgnore == null)
		{
			partRendererBoundsIgnore = new List<string>();
		}
		AttributeSetup();
		SetupResources();
		SetupSimulationResources();
		EffectSetup();
		dragCubes.SetPart(this);
		if (collider != null)
		{
			collider.isTrigger = true;
		}
		defaultHighlightPart = Highlighter.colorPartHighlightDefault;
		highlightColor = defaultHighlightPart;
		SetupHighlighter();
		onPartAwake();
		connected = true;
		attached = true;
		originalStage = inverseStage;
		stackIcon = new ProtoStageIcon(this);
		ProtoStageIcon protoStageIcon = stackIcon;
		protoStageIcon.onStageIconDestroy = (Callback)Delegate.Remove(protoStageIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			if (componentsInChildren[i].gameObject.CompareTag("Airlock"))
			{
				airlock = componentsInChildren[i].transform;
				break;
			}
		}
		referenceTransform = base.transform;
		dockingPorts.Clear();
		isShieldedDisplay = Localizer.Format(partIsShielded ? cacheAutoLOC_439839 : cacheAutoLOC_439840);
	}

	public IEnumerator Start()
	{
		uint vesselId = 0u;
		if (vessel != null)
		{
			if (vessel.persistentId != 0)
			{
				vesselId = vessel.persistentId;
			}
			else if (vessel.protoVessel != null)
			{
				vesselId = vessel.protoVessel.persistentId;
			}
		}
		persistentId = FlightGlobals.CheckPartpersistentId(persistentId, this, removeOldId: false, addNewId: true, vesselId);
		partTransform = base.transform;
		if (skinMaxTemp < 0.0)
		{
			skinMaxTemp = maxTemp;
		}
		if (absorptiveConstant < 0.0)
		{
			absorptiveConstant = emissiveConstant;
		}
		if (!hasHeiarchyModel)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				SetShader("KerbalSpacePart_VAB");
			}
			else if (HighLogic.LoadedSceneIsFlight)
			{
				if (isVesselEVA)
				{
					SetShader("KSP/Scenery/Decal/Multiply");
				}
				else
				{
					SetShader("KerbalSpacePart_Flight");
				}
			}
		}
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		audioSource.playOnAwake = false;
		audioSource.loop = true;
		audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		audioSource.dopplerLevel = 0f;
		audioSource.volume = GameSettings.SHIP_VOLUME;
		audioSource.spatialBlend = 1f;
		if (dragModel == DragModel.CUBE && !dragCubes.Procedural && !dragCubes.None && dragCubes.Cubes.Count == 0 && !isVesselEVA)
		{
			DragCubeSystem.Instance.LoadDragCubes(this);
		}
		CreateRendererLists();
		if (!HighLogic.LoadedSceneIsEditor && (ResumeState == PartStates.PLACEMENT || ResumeState == PartStates.CARGO))
		{
			State = ResumeState;
			SetCollisionIgnores();
			yield break;
		}
		ModulesOnStart();
		CheckTransferDialog();
		if (stagingIcon != string.Empty)
		{
			try
			{
				stackIcon.SetIcon((DefaultIcons)Enum.Parse(typeof(DefaultIcons), stagingIcon));
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message, base.gameObject);
				stagingIcon = "";
			}
		}
		if (HighLogic.LoadedScene != 0)
		{
			onPartStart();
		}
		UpdateStageability(propagate: false, iconUpdate: true);
		if (stagingOn)
		{
			stackIcon.CreateIcon();
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			rb = GetComponent<Rigidbody>();
			if (rb == null)
			{
				rb = base.gameObject.AddComponent<Rigidbody>();
			}
			rb.isKinematic = true;
			rb.useGravity = false;
			SetMirror(Vector3.one);
			if (attachRules.allowSrfAttach)
			{
				surfaceAttachGO = new GameObject("Surface Attach Collider");
				surfaceAttachGO.transform.SetParent(partTransform.Find("model"), worldPositionStays: false);
				SphereCollider sphereCollider = surfaceAttachGO.AddComponent<SphereCollider>();
				sphereCollider.radius = 0.05f;
				sphereCollider.transform.position = partTransform.TransformPoint(srfAttachNode.position);
				surfaceAttachGO.SetActive(value: false);
			}
			if (!isClone)
			{
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
				int num = componentsInChildren.Length;
				while (num-- > 0)
				{
					if (componentsInChildren[num].gameObject.layer == 21)
					{
						componentsInChildren[num].gameObject.SetActive(value: false);
					}
				}
			}
		}
		mirrorAxis = mirrorRefAxis;
		int count = fxGroups.Count;
		while (count-- > 0)
		{
			fxGroups[count].begin(audioSource);
		}
		if (PhysicsSignificance != -1)
		{
			if (HighLogic.LoadedSceneIsFlight && parent != null && parent.isRobotic())
			{
				PromoteToPhysicalPart();
				if (prefabMass < 0.0065f)
				{
					prefabMass = 0.0065f;
					mass = 0.0065f;
				}
			}
			else
			{
				PhysicsSignificance = Mathf.Max(0, PhysicsSignificance);
				physicalSignificance = (PhysicalSignificance)PhysicsSignificance;
				previousPhysicalSignificance = physicalSignificance;
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (!listenersSet)
			{
				GameEvents.onGamePause.Add(partPause);
				GameEvents.onGameUnpause.Add(partUnpause);
				GameEvents.onHideUI.Add(partHideUI);
				GameEvents.onShowUI.Add(partShowUI);
				GameEvents.onVesselWasModified.Add(VesselModified);
				GameEvents.onPartCouple.Add(OnPartEventFromToAction);
				GameEvents.onPartDie.Add(OnPartEvent);
				GameEvents.onPartUndock.Add(OnPartEvent);
				GameEvents.OnInventoryPartOnMouseChanged.Add(OnCurrentMousePartChanged);
				listenersSet = true;
			}
			topNode = FindAttachNode("top");
			if (collider != null)
			{
				collider.isTrigger = false;
			}
			if (temperature < 0.0)
			{
				temperature = (float)FlightGlobals.getExternalTemperature(FlightGlobals.getAltitudeAtPos(partTransform.position, FlightGlobals.getMainBody(partTransform.position)), FlightGlobals.getMainBody(partTransform.position));
			}
			if (physicalSignificance != 0 && vessel != null && vessel.rootPart == this)
			{
				physicalSignificance = PhysicalSignificance.FULL;
				previousPhysicalSignificance = physicalSignificance;
			}
			if (physicalSignificance == PhysicalSignificance.FULL)
			{
				partTransform.parent = null;
			}
			Events["AimCamera"].guiActive = (Events["AimCamera"].guiActiveUncommand = (Events["AimCamera"].guiActiveUnfocused = canAimCamera));
			Events["ToggleAutoStrut"].guiActive = AllowAutoStruts();
			Events["ToggleRigidAttachment"].guiActive = physicalSignificance == PhysicalSignificance.FULL && (PhysicsGlobals.ShowRigidJointTweakable == ShowRigidAttachmentOption.Always || showRigidOption == ShowRigidAttachmentOption.Always);
			resourceMass = GetResourceMass(out resourceThermalMass);
			thermalMass = (double)mass * PhysicsGlobals.StandardSpecificHeatCapacity * thermalMassModifier + resourceThermalMass;
			thermalMassReciprocal = 1.0 / Math.Max(thermalMass, 0.001);
			if (thermalMass > 0.0 && temperature == -1.0)
			{
				Part part = this;
				Part part2 = this;
				Part part3 = this;
				double num2 = 288.0;
				part3.skinUnexposedTemperature = 288.0;
				double num3 = num2;
				num2 = 288.0;
				part2.skinTemperature = num3;
				part.temperature = num2;
			}
			hasLiftModule = false;
			int count2 = Modules.Count;
			while (count2-- > 0)
			{
				if (Modules[count2] is ILiftProvider && (Modules[count2] as ILiftProvider).DisableBodyLift)
				{
					hasLiftModule = true;
					break;
				}
			}
			CheckBodyLiftAttachment();
			yield return null;
			if (physicalSignificance == PhysicalSignificance.FULL)
			{
				rb = GetComponent<Rigidbody>();
				if (rb == null)
				{
					rb = base.gameObject.AddComponent<Rigidbody>();
				}
				rb.useGravity = false;
				rb.drag = 0f;
				if (!isVesselEVA)
				{
					rb.centerOfMass = rb.centerOfMass;
				}
				rb.isKinematic = packed;
				rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
				collisionEnhancer = base.gameObject.GetComponent<CollisionEnhancer>() ?? base.gameObject.AddComponent<CollisionEnhancer>();
				partBuoyancy = base.gameObject.GetComponent<PartBuoyancy>() ?? base.gameObject.AddComponent<PartBuoyancy>();
				partBuoyancy.enabled = !packed;
			}
			SetCollisionIgnores();
			yield return null;
			inertiaTensor = (rb ? (rb.inertiaTensor / Mathf.Max(1f, rb.mass)) : Vector3.zero);
			ModulesBeforePartAttachJoint();
			if (physicalSignificance == PhysicalSignificance.FULL)
			{
				CreateAttachJoint(attachMode);
			}
			onFlightStart();
			if (vessel != null && vessel.situation == Vessel.Situations.PRELAUNCH)
			{
				onFlightStartAtLaunchPad();
			}
			switch (ResumeState)
			{
			case PartStates.ACTIVE:
				force_activate(playFX: false);
				break;
			case PartStates.DEACTIVATED:
				deactivate();
				break;
			case PartStates.DEAD:
				Die();
				break;
			}
			InitializeEffects();
			temperatureRenderer = new MaterialColorUpdater(partTransform, PhysicsGlobals.TemperaturePropertyID, this);
			string[] array = ParseExtensions.ParseArray(overrideSkillUpdateModules);
			int num4 = array.Length;
			while (num4-- > 0)
			{
				if (Modules.Contains(array[num4]))
				{
					overrideSkillUpdate = true;
				}
			}
			started = true;
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			Events["ToggleAutoStrut"].guiActiveEditor = AllowAutoStruts();
			Events["ToggleRigidAttachment"].guiActiveEditor = physicalSignificance == PhysicalSignificance.FULL && (PhysicsGlobals.ShowRigidJointTweakable == ShowRigidAttachmentOption.Editor || PhysicsGlobals.ShowRigidJointTweakable == ShowRigidAttachmentOption.Always) && (showRigidOption == ShowRigidAttachmentOption.Editor || showRigidOption == ShowRigidAttachmentOption.Always);
		}
		if (events.Contains("SetVesselNaming"))
		{
			bool flag = partInfo != null && partInfo.showVesselNaming;
			events["SetVesselNaming"].guiActiveEditor = partInfo != null && partInfo.showVesselNaming;
			events["SetVesselNaming"].guiActive = GameSettings.SHOW_VESSEL_NAMING_IN_FLIGHT && flag;
			events["SetVesselNaming"].guiActiveUncommand = GameSettings.SHOW_VESSEL_NAMING_IN_FLIGHT && flag;
			RefreshVesselNamingPAWDisplay();
			GameEvents.onVesselRename.Add(OnVesselRename);
		}
		editorStarted = HighLogic.LoadedSceneIsEditor;
		if (HighLogic.LoadedScene != 0)
		{
			onStartComplete();
		}
		ModulesOnStartFinished();
		UpdateAutoStrut();
		SetupRigidAttachmentUI();
		SetRemoveSymmetryVisibililty();
		if (HighLogic.LoadedSceneIsFlight && AlwaysRecheckShielding && recheckShielding)
		{
			StartCoroutine(RecheckShielding());
		}
		if (physicalSignificance != 0)
		{
			Fields["sameVesselCollision"].guiActive = false;
			Fields["sameVesselCollision"].guiActiveEditor = false;
		}
		else
		{
			Fields["sameVesselCollision"].OnValueModified += SameVesselCollision;
		}
	}

	public void CheckBodyLiftAttachment()
	{
		bodyLiftOnlyUnattachedLiftActual = false;
		if (!bodyLiftOnlyUnattachedLift || string.IsNullOrEmpty(bodyLiftOnlyAttachName))
		{
			return;
		}
		Part attachedPart = FindAttachNode(bodyLiftOnlyAttachName).attachedPart;
		if (!(attachedPart != null))
		{
			return;
		}
		bodyLiftOnlyProvider = null;
		int count = attachedPart.Modules.Count;
		do
		{
			if (count-- <= 0)
			{
				return;
			}
		}
		while (!(attachedPart.Modules[count] is ILiftProvider));
		bodyLiftOnlyProvider = attachedPart.modules[count] as ILiftProvider;
		bodyLiftOnlyUnattachedLiftActual = true;
	}

	public void CheckTransferDialog()
	{
		Events["SpawnTransferDialog"].active = CrewCapacity > 0 && crewTransferAvailable;
	}

	public void OnEnable()
	{
		allParts.AddUnique(this);
	}

	public void OnDisable()
	{
		allParts.Remove(this);
	}

	public void OnDestroy()
	{
		OnJustAboutToBeDestroyed();
		ReleaseAutoStruts();
		CheckAutoStruts();
		ClearModuleReferenceCache();
		if (listenersSet)
		{
			GameEvents.onGamePause.Remove(partPause);
			GameEvents.onGameUnpause.Remove(partUnpause);
			GameEvents.onHideUI.Remove(partHideUI);
			GameEvents.onShowUI.Remove(partShowUI);
			GameEvents.onVesselWasModified.Remove(VesselModified);
			GameEvents.onPartCouple.Remove(OnPartEventFromToAction);
			GameEvents.onPartDie.Remove(OnPartEvent);
			GameEvents.onPartUndock.Remove(OnPartEvent);
			GameEvents.OnInventoryPartOnMouseChanged.Remove(OnCurrentMousePartChanged);
		}
		GameEvents.onVesselRename.Remove(OnVesselRename);
		GameEvents.onPartDestroyed.Fire(this);
		onPartDestroy();
		ProtoStageIcon protoStageIcon = stackIcon;
		protoStageIcon.onStageIconDestroy = (Callback)Delegate.Remove(protoStageIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		stackIcon.RemoveIcon();
		int i = 0;
		for (int count = attachNodes.Count; i < count; i++)
		{
			if (attachNodes[i].icon != null)
			{
				attachNodes[i].DestroyNodeIcon();
			}
		}
		int count2 = currentCollisions.Count;
		while (count2-- > 0)
		{
			if (!(currentCollisions.KeyAt(count2) == null))
			{
				Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(currentCollisions.KeyAt(count2).gameObject);
				if ((bool)partUpwardsCached && partUpwardsCached != this)
				{
					partUpwardsCached.currentCollisions.Clear();
					partUpwardsCached.vessel.checkLanded();
				}
			}
		}
		if (protoModuleCrew != null)
		{
			UnregisterCrew();
		}
		if (physicalSignificance == PhysicalSignificance.FULL)
		{
			UnityEngine.Object.Destroy(GetComponent<CollisionEnhancer>());
			UnityEngine.Object.Destroy(GetComponent<PQS_PartCollider>());
			if ((bool)GetComponent<PartBuoyancy>())
			{
				UnityEngine.Object.Destroy(GetComponent<PartBuoyancy>());
			}
			if ((bool)GetComponent<Joint>())
			{
				UnityEngine.Object.Destroy(GetComponent<Joint>());
			}
		}
		allParts.Remove(this);
		if (Performance.Instance != null)
		{
			Performance.Instance.requireCleanup = true;
		}
		ResetAllModelComponentCacheLists();
		state = PartStates.DEAD;
		Fields["sameVesselCollision"].OnValueModified -= SameVesselCollision;
	}

	public void SameVesselCollision(object field)
	{
		SetCollisionIgnores();
	}

	public virtual void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
	}

	public virtual void OnInventoryModeDisable()
	{
	}

	public virtual void OnInventoryModeEnable()
	{
	}

	public virtual void OnConstructionModeUpdate()
	{
	}

	public virtual void OnConstructionModeFixedUpdate()
	{
	}

	public void OnWillBeCopied(bool asSymCounterpart)
	{
		onWillBeCopied(asSymCounterpart);
		for (int i = 0; i < modules.Count; i++)
		{
			modules[i].OnWillBeCopied(asSymCounterpart);
		}
		for (int j = 0; j < children.Count; j++)
		{
			children[j].OnWillBeCopied(asSymCounterpart);
		}
	}

	public void OnWasCopied(Part newPart, bool asSymCounterpart)
	{
		onWasCopied(newPart, asSymCounterpart);
		for (int i = 0; i < modules.Count; i++)
		{
			PartModule partModule = modules[i];
			PartModule partModule2 = null;
			if (newPart.modules.Count >= i)
			{
				partModule2 = newPart.modules[i];
				if (partModule.GetType() != partModule2.GetType())
				{
					partModule2 = null;
				}
			}
			modules[i].OnWasCopied(partModule2, asSymCounterpart);
		}
		for (int j = 0; j < children.Count; j++)
		{
			children[j].OnWasCopied(newPart.children[j], asSymCounterpart);
		}
	}

	public void OnCopy(Part original, bool asSymCounterpart)
	{
		isClone = true;
		originalPart = original;
		int count = Resources.Count;
		while (count-- > 0)
		{
			if (!original.Resources.Contains(Resources[count].info.id))
			{
				Resources.Remove(Resources[count].info.id);
			}
		}
		bool flag = false;
		for (int i = 0; i < original.Resources.Count; i++)
		{
			PartResource partResource = Resources.Get(original.Resources[i].info.id);
			if (partResource == null)
			{
				Resources.Add(original.Resources[i]);
				SimulationResources.Add(original.SimulationResources[i]);
			}
			else
			{
				partResource.amount = original.Resources[i].amount;
				partResource.maxAmount = original.Resources[i].maxAmount;
				flag = true;
			}
		}
		if (flag)
		{
			ResetSimulationResources();
		}
		int count2 = original.modules.Count;
		while (count2-- > 0)
		{
			modules[count2].Copy(original.modules[count2]);
		}
		if (original.variants != null && original.variants.SelectedVariant != null)
		{
			variants.SetVariant(original.variants.SelectedVariant.Name);
		}
		onCopy(original, asSymCounterpart);
		int j = 0;
		for (int count3 = children.Count; j < count3; j++)
		{
			children[j].OnCopy(original.children[j], asSymCounterpart);
		}
	}

	public bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		bool flag = false;
		for (int i = 0; i < modules.Count; i++)
		{
			flag = flag || modules[i].OnWillBeMirrored(ref rotation, selPartNode, partParent);
		}
		return flag;
	}

	public void CreateAttachJoint(AttachModes mode)
	{
		if ((bool)parent && attached)
		{
			AttachNode nodeToParent = FindAttachNodeByPart(parent);
			AttachNode nodeFromParent = parent.FindAttachNodeByPart(this);
			if (attachJoint != null)
			{
				attachJoint.DestroyJoint();
			}
			attachJoint = PartJoint.Create(this, parent, nodeToParent, nodeFromParent, mode);
		}
	}

	[ContextMenu("Reset Collision Ignores")]
	public void ResetCollisionIgnores()
	{
		SetCollisionIgnores();
	}

	public void SetCollisionIgnores()
	{
		if (physicalSignificance == PhysicalSignificance.FULL && !skipColliderIgnores)
		{
			GameEvents.OnCollisionIgnoreUpdate.Fire();
		}
	}

	public void SpawnIVA()
	{
		if (CrewCapacity <= 0)
		{
			return;
		}
		if (internalModel == null)
		{
			CreateInternalModel();
			if (internalModel == null)
			{
				if (!modules.Contains("KerbalSeat") && !modules.Contains("KerbalEVA"))
				{
					Debug.LogWarning("[Part]: " + base.name + " holds crew but has no interior model defined!");
				}
				return;
			}
		}
		if (internalModel != null)
		{
			internalModel.Initialize(this);
			internalModel.SpawnCrew();
			internalModel.SetVisible(visible: false);
		}
	}

	public void DespawnIVA()
	{
		if (!(internalModel == null))
		{
			internalModel.DespawnCrew();
			UnityEngine.Object.Destroy(internalModel.gameObject);
		}
	}

	public void RemoveCrewmember(ProtoCrewMember crew)
	{
		if (protoModuleCrew.Contains(crew))
		{
			crew.UnregisterExperienceTraits(this);
			crew.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			protoModuleCrew.Remove(crew);
			vessel.RemoveCrew(crew);
			if (internalModel != null)
			{
				internalModel.UnseatKerbal(crew);
			}
		}
		else
		{
			Debug.LogWarning("Cannot remove crewmember " + crew.name + " from " + base.name + ". Crewmember is not in this part.", crew.KerbalRef);
		}
	}

	public bool AddCrewmember(ProtoCrewMember crew)
	{
		if (!protoModuleCrew.Contains(crew))
		{
			if (protoModuleCrew.Count < CrewCapacity)
			{
				crew.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
				protoModuleCrew.Add(crew);
				crew.RegisterExperienceTraits(this);
				if (internalModel != null && internalModel.GetNextAvailableSeat() != null)
				{
					internalModel.SitKerbalAt(crew, internalModel.GetNextAvailableSeat());
				}
				else
				{
					crew.seatIdx = -1;
					crew.seat = null;
				}
				if (vessel != null)
				{
					vessel.CrewListSetDirty();
				}
				return true;
			}
			Debug.LogWarning("Cannot add crewmember " + crew.name + " to " + base.name + ". Part Crew Capacity Exceeded.", crew.KerbalRef);
			return false;
		}
		Debug.LogWarning("Cannot add crewmember " + crew.name + " to " + base.name + ". Crewmember is already in this part.", crew.KerbalRef);
		return false;
	}

	public bool AddCrewmemberAt(ProtoCrewMember crew, int seatIndex)
	{
		if (!protoModuleCrew.Contains(crew))
		{
			if (protoModuleCrew.Count < CrewCapacity)
			{
				if (internalModel != null)
				{
					if (seatIndex >= 0 && internalModel.seats.Count > seatIndex)
					{
						crew.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
						protoModuleCrew.Add(crew);
						crew.RegisterExperienceTraits(this);
						internalModel.SitKerbalAt(crew, internalModel.seats[seatIndex]);
						if (vessel != null)
						{
							vessel.CrewListSetDirty();
						}
						return true;
					}
					Debug.LogError("Cannot add crewmember " + crew.name + " to " + base.name + ". Seat index " + seatIndex + " is out of range", base.gameObject);
					return false;
				}
				crew.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
				protoModuleCrew.Add(crew);
				crew.RegisterExperienceTraits(this);
				crew.seatIdx = seatIndex;
				crew.seat = null;
				if (vessel != null)
				{
					vessel.CrewListSetDirty();
				}
				return true;
			}
			Debug.LogError("Cannot add crewmember " + crew.name + " to " + base.name + ". Part Crew Capacity Exceeded.", crew.KerbalRef);
			return false;
		}
		Debug.LogError("Cannot add crewmember " + crew.name + " to " + base.name + ". Crewmember is already in this part.", crew.KerbalRef);
		return false;
	}

	public void RegisterCrew()
	{
		if (protoModuleCrew != null)
		{
			int i = 0;
			for (int count = protoModuleCrew.Count; i < count; i++)
			{
				protoModuleCrew[i].RegisterExperienceTraits(this);
			}
		}
	}

	public void UnregisterCrew()
	{
		int i = 0;
		for (int count = protoModuleCrew.Count; i < count; i++)
		{
			protoModuleCrew[i].UnregisterExperienceTraits(this);
		}
	}

	public int GetCrewCountOfExperienceEffect<T>() where T : ExperienceEffect
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = protoModuleCrew[i];
			if (protoCrewMember.HasEffect<T>() && !protoCrewMember.inactive)
			{
				num++;
			}
		}
		return num;
	}

	public List<ProtoCrewMember> GetCrewOfExperienceEffect<T>() where T : ExperienceEffect
	{
		List<ProtoCrewMember> list = new List<ProtoCrewMember>();
		int count = protoModuleCrew.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = protoModuleCrew[i];
			if (protoCrewMember.HasEffect<T>() && !protoCrewMember.inactive)
			{
				list.Add(protoCrewMember);
			}
		}
		return list;
	}

	public void OnLevelLoaded()
	{
		SetHighlightDefault();
	}

	public void freeze()
	{
		if (stackIcon != null)
		{
			stackIcon.Freeze();
		}
		frozen = true;
		SetHighlight(active: true, recursive: true);
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].freeze();
		}
	}

	public void unfreeze()
	{
		if (stackIcon != null)
		{
			stackIcon.Unfreeze();
		}
		frozen = false;
		SetHighlight(active: false, recursive: true);
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].unfreeze();
		}
	}

	public void setParent(Part p = null)
	{
		if (p != null)
		{
			GameEvents.onPartAttach.Fire(new GameEvents.HostTargetAction<Part, Part>(this, p));
			p.addChild(this);
		}
		else if (parent != null)
		{
			GameEvents.onPartRemove.Fire(new GameEvents.HostTargetAction<Part, Part>(p, this));
			parent.removeChild(this);
		}
		parent = p;
		potentialParent = p;
		connected = p != null;
	}

	public void clearParent()
	{
		parent = null;
		potentialParent = null;
		connected = false;
	}

	public void addChild(Part child)
	{
		children.Add(child);
		SendMessage("OnChildAdd", child, SendMessageOptions.DontRequireReceiver);
	}

	public void removeChild(Part child)
	{
		if (children.IndexOf(child) != -1)
		{
			children.Remove(child);
			SendMessage("OnChildRemove", child, SendMessageOptions.DontRequireReceiver);
		}
		else if (child != null)
		{
			Debug.LogWarning("Part Warning: part " + child.name + " is not a child of " + base.name, base.gameObject);
		}
		else
		{
			Debug.LogError("Part Error: trying to remove a null child from " + base.name, base.gameObject);
		}
	}

	public bool hasIndirectChild(Part tgtPart)
	{
		if (tgtPart == this)
		{
			return true;
		}
		int num = 0;
		int count = children.Count;
		while (true)
		{
			if (num < count)
			{
				if (children[num].hasIndirectChild(tgtPart))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool hasIndirectParent(Part tgtPart)
	{
		if (tgtPart == this)
		{
			return true;
		}
		if (parent != null)
		{
			return parent.hasIndirectParent(parent);
		}
		return false;
	}

	public void onAttach(Part parent, bool first = true)
	{
		onPartAttach(parent);
		OnEditorAttach();
		connected = true;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].onAttach(this, first: false);
		}
		RefreshHighlighter();
		CycleAutoStrut();
		SetRemoveSymmetryVisibililty();
		if (first)
		{
			StageManager.GenerateStagingSequence(localRoot);
		}
	}

	public void OnAttachFlight(Part parent)
	{
		if (parent.vessel == null)
		{
			return;
		}
		this.parent = parent;
		vessel = parent.vessel;
		onPartAttach(parent);
		vessel.Parts.Add(this);
		parent.addChild(this);
		if ((vessel.vesselType <= VesselType.Debris || vessel.vesselType == VesselType.DroppedPart) && vesselType > VesselType.Debris)
		{
			vessel.vesselType = vesselType;
			if (CommNetScenario.CommNetEnabled && CommNetScenario.Instance != null)
			{
				CommNetVessel commNetVessel = vessel.FindVesselModuleImplementing<CommNetVessel>();
				if (commNetVessel == null)
				{
					commNetVessel = vessel.gameObject.AddComponent<CommNetVessel>();
					vessel.vesselModules.Add(commNetVessel);
				}
			}
			ModuleCommand moduleCommand = FindModuleImplementing<ModuleCommand>();
			if (moduleCommand != null)
			{
				moduleCommand.UpdateControlState();
			}
		}
		if (vessel.rootPart != null)
		{
			flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		}
		if ((bool)rb)
		{
			if (physicalSignificance == PhysicalSignificance.FULL)
			{
				rb.isKinematic = false;
			}
			else
			{
				UnityEngine.Object.Destroy(rb);
			}
		}
		state = PartStates.IDLE;
		ResumeState = PartStates.IDLE;
		PreFailState = PartStates.IDLE;
		connected = true;
		UpdateOrgPosAndRot(parent.vessel.rootPart);
		OnInventoryModeEnable();
		for (int i = 0; i < modules.Count; i++)
		{
			modules[i].enabled = true;
			modules[i].OnInventoryModeEnable();
		}
		if (!sameVesselCollision)
		{
			Collider[] partColliders = GetPartColliders();
			CollisionManager.IgnoreCollidersOnVessel(parent.vessel, partColliders);
		}
		base.enabled = true;
		InitializeModules();
		SetupHighlighter();
		RefreshHighlighter();
		CycleAutoStrut();
		if (physicalSignificance == PhysicalSignificance.NONE)
		{
			base.transform.SetParent(parent.transform);
		}
		else
		{
			base.transform.SetParent(null);
		}
		GameEvents.onVesselWasModified.Fire(vessel);
		GameEvents.onVesselPartCountChanged.Fire(vessel);
		StartCoroutine(WaitForStart());
	}

	public IEnumerator WaitForStart()
	{
		while (!started)
		{
			yield return null;
		}
		if (physicalSignificance == PhysicalSignificance.NONE && base.transform.parent == null && parent != null)
		{
			base.transform.SetParent(parent.transform, worldPositionStays: true);
		}
		Unpack();
		ResumeVelocity();
		yield return null;
		stagingOn = false;
		UpdateStageability(propagate: false, iconUpdate: false);
		if (stagingOn && vessel.currentStage <= 0)
		{
			vessel.currentStage = 1;
		}
	}

	public void onDetach(bool first = true)
	{
		onPartDetach();
		OnEditorDetach();
		connected = false;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].onDetach(first: false);
		}
		ReleaseAutoStruts();
	}

	public void OnDetachFlight()
	{
		if (!HighLogic.LoadedSceneIsFlight && Mouse.HoveredPart.children.Count <= 0)
		{
			return;
		}
		if (FlightCamera.fetch != null && FlightCamera.fetch.targetMode == FlightCamera.TargetMode.Part && FlightCamera.fetch.partTarget == this)
		{
			if (parent != null)
			{
				FlightCamera.SetTarget(parent);
			}
			else
			{
				FlightCamera.ClearTarget();
			}
		}
		onPartDetach();
		state = PartStates.CARGO;
		ResumeState = PartStates.CARGO;
		PreFailState = PartStates.CARGO;
		Decoupling(0f);
		if ((bool)collisionEnhancer)
		{
			UnityEngine.Object.Destroy(collisionEnhancer);
		}
		if ((bool)partBuoyancy)
		{
			UnityEngine.Object.Destroy(partBuoyancy);
		}
		if (parent != null)
		{
			parent.removeChild(this);
		}
		RemoveFromSymmetry();
		ReleaseAutoStruts();
		RefreshHighlighter();
		OnInventoryModeDisable();
		base.transform.SetParent(null);
		for (int i = 0; i < modules.Count; i++)
		{
			modules[i].OnInventoryModeDisable();
			modules[i].enabled = false;
		}
		if ((bool)rb)
		{
			rb.isKinematic = true;
		}
		else
		{
			rb = base.gameObject.AddComponent<Rigidbody>();
			rb.isKinematic = true;
			rb.useGravity = false;
		}
		CollisionManager.SetCollidersOnVessel(vessel, ignore: false, GetPartColliders());
		base.enabled = false;
		GameEvents.onVesselWasModified.Fire(vessel);
		vessel = null;
	}

	public void OnDelete()
	{
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Remove(persistentId);
		}
		onPartDelete();
		OnEditorDestroy();
	}

	[ContextMenu("Activate")]
	public void force_activate()
	{
		force_activate(playFX: true);
	}

	public void force_activate(bool playFX)
	{
		state = PartStates.ACTIVE;
		if (!onPartActivate())
		{
			state = PartStates.IDLE;
			MonoBehaviour.print("[" + base.name + "]: Did not activate");
			return;
		}
		if (findFxGroup("activate") != null && playFX)
		{
			findFxGroup("activate").Burst();
		}
		if (findFxGroup("active") != null)
		{
			findFxGroup("active").setActive(value: true);
		}
		ModulesOnActivate();
	}

	public bool activate(int currentStage, Vessel activeVessel)
	{
		if (state == PartStates.DEAD)
		{
			return false;
		}
		if (currentStage != inverseStage)
		{
			return false;
		}
		if (vessel != activeVessel && !ActivatesEvenIfDisconnected)
		{
			return false;
		}
		state = PartStates.ACTIVE;
		if (!onPartActivate())
		{
			state = PartStates.IDLE;
			MonoBehaviour.print("[" + base.name + "]: Did not activate");
			return false;
		}
		if (findFxGroup("activate") != null)
		{
			findFxGroup("activate").Burst();
		}
		if (findFxGroup("active") != null)
		{
			findFxGroup("active").setActive(value: true);
		}
		ModulesOnActivate();
		if (GameSettings.VERBOSE_DEBUG_LOG)
		{
			MonoBehaviour.print("[" + base.name + "]: Activated");
		}
		return true;
	}

	public void deactivate()
	{
		if (findFxGroup("active") != null)
		{
			findFxGroup("active").setActive(value: false);
		}
		if (findFxGroup("deactivate") != null)
		{
			findFxGroup("deactivate").Burst();
		}
		state = PartStates.DEACTIVATED;
		onPartDeactivate();
		ModulesOnDeactivate();
		if (GameSettings.VERBOSE_DEBUG_LOG)
		{
			MonoBehaviour.print("[" + base.name + "]: Deactivated");
		}
	}

	public void decouple(float breakForce = 0f)
	{
		Decoupling(breakForce);
		Vessel vessel = this.vessel;
		vessel.initialPosVelSet = false;
		string vesselName = this.vessel.vesselName;
		Vessel vessel2 = base.gameObject.AddComponent<Vessel>();
		vessel2.id = Guid.NewGuid();
		if (vessel2.Initialize())
		{
			if (!vessel2.UpdateVesselNaming())
			{
				vessel2.vesselName = Vessel.AutoRename(vessel2, vesselName);
			}
			vessel2.IgnoreGForces(10);
			vessel2.ctrlState.CopyFrom(vessel.ctrlState);
			vessel2.ActionGroups.CopyFrom(vessel.ActionGroups);
			vessel2.CopyOverrides(vessel);
			vessel2.currentStage = StageManager.RecalculateVesselStaging(vessel2);
			vessel2.launchedFrom = vessel.launchedFrom;
			vessel2.landedAt = vessel.landedAt;
			setParent();
		}
		vessel.checkLanded();
		vessel.FallBackReferenceTransform();
		CleanSymmetryVesselReferencesRecursively();
		vessel2.DiscoveryInfo.SetUntrackedObjectSize(vessel.DiscoveryInfo.objectSize);
		GameEvents.onVesselWasModified.Fire(vessel);
		GameEvents.onPartDeCoupleComplete.Fire(this);
		GameEvents.onPartDeCoupleNewVesselComplete.Fire(vessel, vessel2);
		CreateRendererLists();
		CheckBodyLiftAttachment();
		vessel.CheckAirstreamShields();
		vessel2.CheckAirstreamShields();
	}

	public void Decoupling(float breakForce)
	{
		GameEvents.onPartDeCouple.Fire(this);
		if (physicalSignificance != 0)
		{
			PromoteToPhysicalPart();
		}
		onDecouple(breakForce);
		if ((bool)attachJoint)
		{
			attachJoint.DestroyJoint();
		}
		disconnect(breakForce == 0f);
		GameEvents.onVesselPartCountChanged.Fire(vessel);
		attached = false;
		AttachNode attachNode = FindAttachNodeByPart(parent);
		if (attachNode != null)
		{
			attachNode.attachedPart = null;
			int i = 0;
			for (int count = modules.Count; i < count; i++)
			{
				if (modules[i] is IActivateOnDecouple activateOnDecouple)
				{
					activateOnDecouple.DecoupleAction(attachNode.id, weDecouple: true);
				}
			}
		}
		if (!(parent != null))
		{
			return;
		}
		AttachNode attachNode2 = parent.FindAttachNodeByPart(this);
		if (attachNode2 == null)
		{
			return;
		}
		int j = 0;
		for (int count2 = parent.Modules.Count; j < count2; j++)
		{
			if (parent.Modules[j] is IActivateOnDecouple activateOnDecouple2)
			{
				activateOnDecouple2.DecoupleAction(attachNode2.id, weDecouple: false);
			}
		}
		attachNode2.attachedPart = null;
	}

	public void FindNonPhysicslessChildren(ref List<Part> parts)
	{
		if (parts == null)
		{
			parts = new List<Part>();
		}
		for (int i = 0; i < children.Count; i++)
		{
			if (children[i] != null && children[i].physicalSignificance == PhysicalSignificance.FULL)
			{
				parts.Add(children[i]);
				if (children[i].children.Count > 0)
				{
					children[i].FindNonPhysicslessChildren(ref parts);
				}
			}
		}
	}

	public Part FindNonPhysicslessParent()
	{
		if (parent == null)
		{
			return this;
		}
		if (physicalSignificance == PhysicalSignificance.FULL)
		{
			return this;
		}
		return parent.FindNonPhysicslessParent();
	}

	public void PromoteToPhysicalPart()
	{
		if (physicalSignificance == PhysicalSignificance.FULL)
		{
			return;
		}
		FlightGlobals.ResetObjectPartUpwardsCache();
		FlightGlobals.ResetObjectPartPointerUpwardsCache();
		List<Part> parts = new List<Part>();
		FindNonPhysicslessChildren(ref parts);
		physicalSignificance = PhysicalSignificance.FULL;
		if (!HighLogic.LoadedSceneIsEditor)
		{
			partTransform.parent = null;
		}
		if (!GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.drag = 0f;
		rb.centerOfMass = Vector3.zero + CoMOffset;
		if (HighLogic.LoadedSceneIsEditor)
		{
			rb.isKinematic = true;
		}
		else
		{
			rb.isKinematic = false;
		}
		if ((bool)parent && parent.Rigidbody != null)
		{
			Rigidbody rigidbody = parent.Rigidbody;
			rb.velocity = rigidbody.velocity;
			rb.angularVelocity = rigidbody.angularVelocity;
		}
		inertiaTensor = rb.inertiaTensor / Mathf.Max(1f, rb.mass);
		collisionEnhancer = base.gameObject.GetComponent<CollisionEnhancer>();
		if (collisionEnhancer == null)
		{
			collisionEnhancer = base.gameObject.AddComponent<CollisionEnhancer>();
			collisionEnhancer.framesToSkip = collisionEnhancerSkipFrames;
		}
		partBuoyancy = base.gameObject.GetComponent<PartBuoyancy>() ?? base.gameObject.AddComponent<PartBuoyancy>();
		partBuoyancy.enabled = !packed;
		int i = 0;
		for (int count = parts.Count; i < count; i++)
		{
			if ((bool)parts[i].attachJoint)
			{
				parts[i].attachJoint.DestroyJoint();
			}
			AttachNode nodeToParent = parts[i].FindPartThroughNodes(this);
			AttachNode nodeFromParent = FindPartThroughNodes(parts[i]);
			parts[i].attachJoint = PartJoint.Create(parts[i], this, nodeToParent, nodeFromParent, parts[i].attachMode);
		}
		if (GameSettings.ADVANCED_TWEAKABLES)
		{
			Fields["sameVesselCollision"].guiActive = true;
			Fields["sameVesselCollision"].guiActiveEditor = true;
			Fields["sameVesselCollision"].OnValueModified += SameVesselCollision;
		}
		SetCollisionIgnores();
		for (int j = 0; j < modules.Count; j++)
		{
			modules[j].PromoteToPhysicalPart();
		}
	}

	public void DemoteToPhysicslessPart()
	{
		if (physicalSignificance != PhysicalSignificance.NONE && !(parent == null))
		{
			for (int i = 0; i < modules.Count; i++)
			{
				modules[i].DemoteToPhysicslessPart();
			}
			partTransform.SetParent(parent.transform);
			FlightGlobals.ResetObjectPartUpwardsCache();
			FlightGlobals.ResetObjectPartPointerUpwardsCache();
			previousPhysicalSignificance = physicalSignificance;
			physicalSignificance = PhysicalSignificance.NONE;
			ReleaseAutoStruts();
			if ((bool)collisionEnhancer)
			{
				UnityEngine.Object.Destroy(collisionEnhancer);
			}
			if ((bool)terrainCollider)
			{
				UnityEngine.Object.Destroy(terrainCollider);
			}
			if ((bool)partBuoyancy)
			{
				UnityEngine.Object.Destroy(partBuoyancy);
			}
			if ((bool)attachJoint)
			{
				attachJoint.DestroyJoint();
			}
			if ((bool)rb && FindModuleImplementing<ModuleWheelBase>() == null)
			{
				UnityEngine.Object.Destroy(rb);
			}
		}
	}

	public float GetPhysicslessChildMass()
	{
		float num = 0f;
		int count = children.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = children[i];
			if (part.rb == null)
			{
				float num2 = part.GetResourceMass();
				num += part.mass + num2;
			}
		}
		return num;
	}

	public AttachNode FindPartThroughNodes(Part tgtPart, Part src = null)
	{
		if (src == this)
		{
			return null;
		}
		int num = 0;
		int count = attachNodes.Count;
		AttachNode attachNode;
		while (true)
		{
			if (num < count)
			{
				attachNode = attachNodes[num];
				if (!(attachNode.attachedPart == tgtPart))
				{
					if (!(attachNode.attachedPart == null) && !(attachNode.attachedPart == src) && attachNode.attachedPart.FindPartThroughNodes(tgtPart, this) != null)
					{
						break;
					}
					num++;
					continue;
				}
				return attachNode;
			}
			if (srfAttachNode != null)
			{
				if (srfAttachNode.attachedPart == tgtPart)
				{
					return srfAttachNode;
				}
				if (!(srfAttachNode.attachedPart == null) && !(srfAttachNode.attachedPart == src) && srfAttachNode.attachedPart.FindPartThroughNodes(tgtPart, this) != null)
				{
					return srfAttachNode;
				}
			}
			return null;
		}
		return attachNode;
	}

	public void disconnect(bool controlledSeparation = false)
	{
		connected = false;
		onDisconnect();
		vessel.parts.Remove(this);
		vessel.RemoveCrewList(protoModuleCrew, updatePartCount: true);
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].disconnect(controlledSeparation);
		}
		if (controlledSeparation)
		{
			stackIcon.RemoveIcon();
		}
	}

	public void CleanSymmetryVesselReferencesRecursively()
	{
		CleanSymmetryVesselReferences();
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].CleanSymmetryVesselReferencesRecursively();
		}
		SetRemoveSymmetryVisibililty();
		SetCollisionIgnores();
	}

	public void CleanSymmetryReferences()
	{
		int count = symmetryCounterparts.Count;
		while (count-- > 0)
		{
			Part part = symmetryCounterparts[count];
			if (!(part == null))
			{
				part.symmetryCounterparts.Remove(this);
				part.SetRemoveSymmetryVisibililty();
				symmetryCounterparts.RemoveAt(count);
			}
		}
		SetRemoveSymmetryVisibililty();
	}

	public void CleanSymmetryVesselReferences()
	{
		int count = symmetryCounterparts.Count;
		while (count-- > 0)
		{
			Part part = symmetryCounterparts[count];
			if (part == null)
			{
				symmetryCounterparts.RemoveAt(count);
			}
			else if (part.vessel != vessel)
			{
				part.symmetryCounterparts.Remove(this);
				part.SetRemoveSymmetryVisibililty();
				symmetryCounterparts.RemoveAt(count);
			}
		}
		SetRemoveSymmetryVisibililty();
	}

	public void SetLayer(GameObject obj, int layer)
	{
		obj.layer = layer;
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			SetLayer(child.gameObject, layer);
		}
	}

	public void OnPartJointBreak(float breakForce)
	{
		if (!(vessel.rootPart == this))
		{
			if (breakForce > 0f && parent != null)
			{
				GameEvents.onJointBreak.Fire(new EventReport(FlightEvents.JOINTBREAK, this, partInfo.title, parent.partInfo.title));
			}
			decouple(breakForce);
		}
	}

	public void Couple(Part tgtPart)
	{
		GameEvents.onPartCouple.Fire(new GameEvents.FromToAction<Part, Part>(this, tgtPart));
		Vessel vessel = this.vessel;
		vessel.ClearStaging();
		tgtPart.vessel.ClearStaging();
		int i = 0;
		for (int count = this.vessel.parts.Count; i < count; i++)
		{
			Part part = this.vessel.parts[i];
			part.attached = true;
			part.connected = true;
			part.UpdateOrgPosAndRot(tgtPart.vessel.rootPart);
		}
		int j = 0;
		for (int count2 = tgtPart.vessel.parts.Count; j < count2; j++)
		{
			Part part2 = tgtPart.vessel.parts[j];
			part2.attached = true;
			part2.connected = true;
		}
		SetHierarchyRoot(this);
		setParent(tgtPart);
		CreateAttachJoint(tgtPart.attachMode);
		ResetJoints();
		SetVessel(tgtPart.vessel);
		GameEvents.onVesselPartCountChanged.Fire(tgtPart.vessel);
		this.vessel.vesselType = this.vessel.FindDefaultVesselType();
		vessel.OnJustAboutToBeDestroyed();
		GameEvents.onVesselPersistentIdChanged.Fire(vessel.persistentId, tgtPart.vessel.persistentId);
		UnityEngine.Object.DestroyImmediate(vessel);
		this.vessel.checkLanded();
		int k = 0;
		for (int count3 = this.vessel.parts.Count; k < count3; k++)
		{
			this.vessel.parts[k].SetCollisionIgnores();
		}
		this.vessel.currentStage = StageManager.RecalculateVesselStaging(this.vessel) + 1;
		GameEvents.onVesselWasModified.Fire(this.vessel);
		GameEvents.onPartCoupleComplete.Fire(new GameEvents.FromToAction<Part, Part>(this, tgtPart));
		CheckBodyLiftAttachment();
	}

	public void SetHierarchyRoot(Part root)
	{
		Part part = parent;
		bool flag = srfAttachNode.attachedPart == part;
		if (root == this)
		{
			parent = null;
		}
		else
		{
			parent = root;
		}
		if (physicalSignificance == PhysicalSignificance.FULL && !HighLogic.LoadedSceneIsEditor)
		{
			if ((bool)attachJoint)
			{
				attachJoint.DestroyJoint();
			}
			CreateAttachJoint(root.attachMode);
			ResetJoints();
		}
		else
		{
			base.transform.parent = ((parent != null) ? parent.transform : null);
		}
		if (part != null)
		{
			addChild(part);
			part.removeChild(this);
			part.SetHierarchyRoot(this);
			if (flag && srfAttachNode.nodeType == AttachNode.NodeType.Surface)
			{
				part.srfAttachNode.ReverseSrfNodeDirection(srfAttachNode);
				part.attachMode = AttachModes.SRF_ATTACH;
			}
			else
			{
				part.attachMode = AttachModes.STACK;
			}
			srfAttachNode.ChangeSrfNodePosition();
		}
	}

	public void UpdateOrgPosAndRot(Part newRoot)
	{
		orgPos = newRoot.partTransform.InverseTransformPoint(partTransform.position);
		orgRot = Quaternion.Inverse(newRoot.partTransform.rotation) * partTransform.rotation;
	}

	public void SetVessel(Vessel v)
	{
		v.parts.Add(this);
		vessel = v;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].SetVessel(v);
		}
	}

	public void Undock(DockedVesselInfo newVesselInfo)
	{
		GameEvents.onPartUndock.Fire(this);
		if ((bool)attachJoint)
		{
			attachJoint.DestroyJoint();
		}
		Vessel oldVessel = this.vessel;
		oldVessel.ClearStaging();
		Part part = ((newVesselInfo != null) ? this.vessel[newVesselInfo.rootPartUId] : null);
		setParent();
		bool flag;
		if (flag = part != null && hasIndirectChild(part))
		{
			part.SetHierarchyRoot(part);
		}
		else
		{
			part = this;
		}
		Vessel vessel = part.gameObject.AddComponent<Vessel>();
		vessel.id = Guid.NewGuid();
		vessel.Initialize();
		vessel.orbit.referenceBody = FlightGlobals.getMainBody(part.partTransform.position);
		if (!vessel.UpdateVesselNaming())
		{
			if (!flag && (newVesselInfo == null || newVesselInfo.vesselType != VesselType.const_11))
			{
				vessel.vesselName = Vessel.AutoRename(vessel, oldVessel.vesselName);
			}
			else
			{
				vessel.vesselName = newVesselInfo.name;
				vessel.vesselType = newVesselInfo.vesselType;
				if (vessel.vesselType <= VesselType.Unknown)
				{
					VesselType vesselType = VesselType.Debris;
					int count = vessel.parts.Count;
					while (count-- > 0)
					{
						if (vessel.parts[count].vesselType > vesselType)
						{
							vesselType = vessel.parts[count].vesselType;
						}
					}
					if (vesselType > vessel.vesselType)
					{
						vessel.vesselType = vesselType;
					}
				}
			}
		}
		if (vessel.mapObject != null)
		{
			vessel.mapObject.gameObject.name = vessel.vesselName;
		}
		vessel.IgnoreGForces(10);
		vessel.currentStage = StageManager.RecalculateVesselStaging(vessel) + 1;
		vessel.launchedFrom = oldVessel.launchedFrom;
		int i = 0;
		for (int count2 = vessel.parts.Count; i < count2; i++)
		{
			oldVessel.parts.Remove(vessel.parts[i]);
		}
		GameEvents.onVesselPartCountChanged.Fire(oldVessel);
		int j = 0;
		for (int count3 = vessel.parts.Count; j < count3; j++)
		{
			vessel.parts[j].SetCollisionIgnores();
		}
		oldVessel.StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			oldVessel.ResumeStaging();
		}));
		oldVessel.checkLanded();
		oldVessel.FallBackReferenceTransform();
		GameEvents.onVesselWasModified.Fire(oldVessel);
		GameEvents.onPartUndockComplete.Fire(this);
		GameEvents.onVesselsUndocking.Fire(oldVessel, vessel);
	}

	public void OnCollisionEnter(Collision c)
	{
		if (!HighLogic.LoadedSceneIsFlight || state == PartStates.DEAD || state == PartStates.PLACEMENT || state == PartStates.CARGO)
		{
			return;
		}
		if (checkAfterResetCollisions && c.contacts.Length != 0)
		{
			Collider otherCollider = c.contacts[0].otherCollider;
			Collider thisCollider = c.contacts[0].thisCollider;
			List<Collider> list = FindModelComponents<Collider>();
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				if ((otherCollider != null && list[i] == otherCollider) || (thisCollider != null && list[i] == thisCollider))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (currentCollisions.ContainsKey(c.collider))
		{
			currentCollisions[c.collider]++;
		}
		else
		{
			currentCollisions.Add(c.collider, 1);
		}
		if (c.gameObject.layer == 0)
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(c.gameObject);
			if (partUpwardsCached != null && (partUpwardsCached.state == PartStates.PLACEMENT || partUpwardsCached.state == PartStates.CARGO))
			{
				return;
			}
		}
		LandedCollisionChecks(c);
		if (vessel != null && vessel.IgnoreCollisionsFrames > 0)
		{
			return;
		}
		crashObjectName = GetComponentUpwards<CrashObjectName>(c.gameObject);
		if (vessel != null)
		{
			vessel.crashObjectName = crashObjectName;
		}
		if (c.gameObject.CompareTag("ROC") && ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			GClass0 componentInParent = c.gameObject.GetComponentInParent<GClass0>();
			if (componentInParent != null)
			{
				componentInParent.CheckCollision(vessel);
			}
		}
		CheckCollision(c);
	}

	public void LandedCollisionChecks(Collision c)
	{
		if (c.gameObject.layer == 15 && state != PartStates.DEAD)
		{
			GroundContact = true;
			if (!vessel.GroundContacts.Contains(this))
			{
				vessel.GroundContacts.Add(this);
			}
			vessel.Landed = true;
			if (!PermanentGroundContact && !c.gameObject.CompareTag("Untagged"))
			{
				if (ROCManager.Instance != null && c.gameObject.CompareTag("ROC"))
				{
					GameObject terrainObj = null;
					string terrainTag = ROCManager.Instance.GetTerrainTag(c.gameObject, out terrainObj);
					vessel.SetLandedAt(terrainTag, terrainObj);
				}
				else
				{
					vessel.SetLandedAt(c.gameObject.tag, c.gameObject);
				}
			}
		}
		if (c.gameObject.layer != 0)
		{
			return;
		}
		Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(c.gameObject);
		if (partUpwardsCached != null && !CheckOtherGroundContact(c) && partUpwardsCached.state != PartStates.PLACEMENT && partUpwardsCached.state != PartStates.CARGO && !vessel.parts.Contains(partUpwardsCached) && partUpwardsCached.vessel.LandedOrSplashed)
		{
			if (isVesselEVA)
			{
				vessel.Landed = true;
			}
			else if (!partUpwardsCached.isVesselEVA)
			{
				vessel.Landed = partUpwardsCached.vessel.Landed;
			}
			else
			{
				vessel.Landed = partUpwardsCached.vessel.Landed && !partUpwardsCached.vessel.Splashed;
			}
			if (partUpwardsCached.vessel.Splashed)
			{
				vessel.Splashed = true;
			}
			if (!PermanentGroundContact)
			{
				vessel.SetLandedAt(partUpwardsCached.vessel.landedAt, null, partUpwardsCached.vessel.displaylandedAt);
			}
		}
	}

	public void OnCollisionExit(Collision c)
	{
		if (currentCollisions.ContainsKey(c.collider))
		{
			currentCollisions[c.collider]--;
			if (currentCollisions[c.collider] > 0)
			{
				return;
			}
			currentCollisions.Remove(c.collider);
		}
		else if (HighLogic.LoadedSceneIsFlight && vessel != null && !vessel.isEVA)
		{
			Debug.Log("Part " + base.name + " exited collision with " + c.collider.gameObject.name + ", but it wasn't in collision count list!");
		}
		Part part = null;
		if (c.gameObject.layer == 0)
		{
			part = FlightGlobals.GetPartUpwardsCached(c.gameObject);
			if (part != null && part.state != PartStates.PLACEMENT && part.state != PartStates.CARGO && vessel != null && !vessel.parts.Contains(part) && part.vessel != null && part.vessel.Splashed)
			{
				vessel.Splashed = false;
			}
		}
		if (vessel != null && (c.gameObject.layer == 15 || (c.gameObject.layer == 0 && vessel != null && !vessel.parts.Contains(part))) && !CheckOtherGroundContact(c))
		{
			GroundContact = false;
			if (vessel.GroundContacts.Contains(this))
			{
				vessel.GroundContacts.Remove(this);
				vessel.ResetRBAnchor();
			}
			for (int i = 0; i < vessel.parts.Count; i++)
			{
				for (int j = 0; j < vessel.parts[i].Modules.Count; j++)
				{
					vessel.parts[i].modules[j].ResetWheelGroundCheck();
				}
			}
			vessel.UpdateLandedSplashed();
		}
		crashObjectName = null;
		if (vessel != null)
		{
			vessel.crashObjectName = crashObjectName;
		}
		currentCollisions.Remove(c.collider);
	}

	public bool CheckOtherGroundContact(Collision c)
	{
		int num = 0;
		while (true)
		{
			if (num < currentCollisions.Count)
			{
				if (!(currentCollisions.KeyAt(num) == null) && !(currentCollisions.KeyAt(num) == c.collider) && currentCollisions.KeyAt(num).gameObject.layer == 15)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool CheckCollision(Collision c)
	{
		if (GameSettings.WHEEL_DAMAGE_IMPACTCOLLIDER_ENABLED)
		{
			int count = modules.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleWheelDamage moduleWheelDamage = modules[i] as ModuleWheelDamage;
				if (moduleWheelDamage != null)
				{
					moduleWheelDamage.HandleCollision(c);
				}
			}
		}
		int num = 0;
		int num2 = c.contacts.Length;
		while (true)
		{
			if (num < num2)
			{
				ContactPoint contactPoint = c.contacts[num];
				if (collider == contactPoint.thisCollider || collider == contactPoint.otherCollider)
				{
					break;
				}
				if ((bool)asteroidCollider)
				{
					for (int j = 0; j < asteroidCollider.transform.childCount; j++)
					{
						MeshCollider component = asteroidCollider.transform.GetChild(j).GetComponent<MeshCollider>();
						if (component == contactPoint.thisCollider || component == contactPoint.otherCollider)
						{
							HandleCollision(c);
							return true;
						}
					}
				}
				else if (HasModuleImplementing<ModuleProceduralFairing>())
				{
					ModuleProceduralFairing moduleProceduralFairing = FindModuleImplementing<ModuleProceduralFairing>();
					for (int k = 0; k < moduleProceduralFairing.ClosedColliders.Count; k++)
					{
						if (moduleProceduralFairing.ClosedColliders[k] == contactPoint.thisCollider || moduleProceduralFairing.ClosedColliders[k] == contactPoint.otherCollider)
						{
							HandleCollision(c);
							return true;
						}
					}
				}
				num++;
				continue;
			}
			int num3 = 0;
			int count2 = children.Count;
			while (true)
			{
				if (num3 < count2)
				{
					Part part = children[num3];
					if (part.physicalSignificance != 0 && part.attached && part.state != PartStates.DEAD && part.CheckCollision(c))
					{
						break;
					}
					num3++;
					continue;
				}
				return false;
			}
			return true;
		}
		HandleCollision(c);
		return true;
	}

	public void HandleCollision(Collision c)
	{
		if (!c.collider.enabled || !c.collider.gameObject.activeInHierarchy || c.collider.isTrigger || c.contacts[0].thisCollider.CompareTag("Wheel_Piston_Collider") || c.relativeVelocity.magnitude <= crashTolerance || ((bool)c.collider.attachedRigidbody && c.relativeVelocity.magnitude * c.collider.attachedRigidbody.mass <= crashTolerance))
		{
			return;
		}
		GClass3 component = c.gameObject.GetComponent<GClass3>();
		CelestialBody celestialBody = null;
		Vector3d vector3d = c.relativeVelocity;
		if (component != null)
		{
			celestialBody = component.sphereRoot.gameObject.transform.parent.gameObject.GetComponent<CelestialBody>();
		}
		if (GameSettings.VERBOSE_DEBUG_LOG)
		{
			Debug.Log("[F: " + Time.frameCount + "]: " + base.name + " collided into " + c.gameObject.name + " - relative velocity: " + c.relativeVelocity.magnitude.ToString() + (c.collider.attachedRigidbody ? (" - momentum: " + (vector3d * c.collider.attachedRigidbody.mass).magnitude.ToString("0.0")) : ((celestialBody != null) ? (" - momentum: " + (vector3d * (float)celestialBody.Mass).magnitude.ToString("0.0")) : " - no momentum (no RB)")), base.gameObject);
		}
		if (component != null)
		{
			GameEvents.onPartExplodeGroundCollision.Fire(this);
		}
		if (!CheatOptions.NoCrashDamage)
		{
			explode();
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(c.gameObject);
			if (component != null)
			{
				GameEvents.onCrash.Fire(new EventReport(FlightEvents.CRASH, this, partInfo.title, Localizer.Format("#autoLOC_438839"), 0, "", c.relativeVelocity.magnitude));
			}
			else if (partUpwardsCached != null)
			{
				GameEvents.onCollision.Fire(new EventReport(FlightEvents.COLLISION, this, partInfo.title, partUpwardsCached.partInfo.title, 0, "", c.relativeVelocity.magnitude));
			}
			else if ((bool)crashObjectName)
			{
				GameEvents.onCrash.Fire(new EventReport(FlightEvents.CRASH, this, partInfo.title, crashObjectName.objectName, 0, "", c.relativeVelocity.magnitude));
			}
			else
			{
				GameEvents.onCrash.Fire(new EventReport(FlightEvents.CRASH, this, partInfo.title, c.gameObject.name, 0, "", c.relativeVelocity.magnitude));
			}
		}
	}

	public void OnTouchDown()
	{
		onPartTouchdown();
	}

	public void OnSplashDown()
	{
		onPartSplashdown();
	}

	public void OnLiftOff()
	{
		onPartLiftOff();
	}

	public bool checkLanded()
	{
		if (!PermanentGroundContact && (!GroundContact || Rigidbody.velocity.sqrMagnitude >= 0.25f))
		{
			int num = 0;
			int count = children.Count;
			while (true)
			{
				if (num < count)
				{
					Part part = children[num];
					if (part.attached && part.checkLanded())
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
		return true;
	}

	public bool checkSplashed()
	{
		if (WaterContact)
		{
			return true;
		}
		int num = 0;
		int count = children.Count;
		while (true)
		{
			if (num < count)
			{
				Part part = children[num];
				if (part.attached && part.checkSplashed())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void checkPermanentLandedAt()
	{
		if (vessel == null || vessel.objectUnderVessel == null)
		{
			return;
		}
		if (!vessel.GroundContacts.Contains(this))
		{
			vessel.GroundContacts.Add(this);
		}
		if (!vessel.objectUnderVessel.CompareTag("Untagged"))
		{
			if (ROCManager.Instance != null && vessel.objectUnderVessel.CompareTag("ROC"))
			{
				GameObject terrainObj = null;
				string terrainTag = ROCManager.Instance.GetTerrainTag(vessel.objectUnderVessel, out terrainObj);
				vessel.SetLandedAt(terrainTag, terrainObj);
			}
			else
			{
				vessel.SetLandedAt(vessel.objectUnderVessel.tag, vessel.objectUnderVessel);
			}
		}
	}

	public void Pack()
	{
		if (packed || state == PartStates.DEAD)
		{
			return;
		}
		if ((bool)rb)
		{
			rb.isKinematic = true;
			if ((bool)GetComponent<PartBuoyancy>())
			{
				GetComponent<PartBuoyancy>().enabled = false;
			}
			if ((bool)terrainCollider)
			{
				UnityEngine.Object.Destroy(terrainCollider);
			}
		}
		if ((bool)servoRb)
		{
			servoRb.isKinematic = true;
		}
		int i = 0;
		for (int count = protoModuleCrew.Count; i < count; i++)
		{
			protoModuleCrew[i].OnPartPack(this);
		}
		onPack();
		SendMessage("OnPartPack", SendMessageOptions.DontRequireReceiver);
		GameEvents.onPartPack.Fire(this);
		if ((bool)attachJoint)
		{
			attachJoint.SetUnbreakable(unbreakable: true, rigidAttachment);
		}
		packed = true;
	}

	public void Unpack()
	{
		ScheduleSetCollisionIgnores();
		if ((bool)rb && state != PartStates.DEAD)
		{
			rb.isKinematic = false;
			if ((bool)GetComponent<PartBuoyancy>())
			{
				GetComponent<PartBuoyancy>().enabled = true;
			}
			if ((bool)servoRb && state != PartStates.DEAD)
			{
				servoRb.isKinematic = false;
			}
			onUnpack();
		}
		int i = 0;
		for (int count = protoModuleCrew.Count; i < count; i++)
		{
			protoModuleCrew[i].OnPartUnpack(this);
		}
		ResetJoints();
		packed = false;
		SendMessage("OnPartUnpack", SendMessageOptions.DontRequireReceiver);
		GameEvents.onPartUnpack.Fire(this);
	}

	public void ResetJoints()
	{
		if ((bool)attachJoint)
		{
			if (CheatOptions.UnbreakableJoints)
			{
				attachJoint.SetUnbreakable(unbreakable: true, rigidAttachment);
			}
			else
			{
				attachJoint.SetUnbreakable(unbreakable: false, rigidAttachment);
			}
		}
		CycleAutoStrut();
	}

	public void ResumeVelocity()
	{
		vel = Vector3.zero;
		if (!vessel.LandedOrSplashed)
		{
			vel = orbit.GetVel() - (orbit.referenceBody.inverseRotation ? orbit.referenceBody.getRFrmVel(partTransform.position) : Vector3d.zero) - Krakensbane.GetFrameVelocity();
		}
		if (rb != null && state != PartStates.DEAD)
		{
			rb.velocity = vel;
		}
		if (servoRb != null && state != PartStates.DEAD)
		{
			servoRb.velocity = vel;
		}
	}

	public void EnableCollisions()
	{
		SetDetectCollisions(setState: true);
	}

	public void DisableCollisions()
	{
		SetDetectCollisions(setState: false);
	}

	public void SetDetectCollisions(bool setState)
	{
		if ((bool)rb)
		{
			rb.detectCollisions = setState;
		}
		if ((bool)servoRb)
		{
			servoRb.detectCollisions = setState;
		}
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].SetDetectCollisions(setState);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			editorCollision = FlightGlobals.GetPartUpwardsCached(other.gameObject);
		}
		if (HighLogic.LoadedSceneIsFlight && vessel != null)
		{
			if (!PermanentGroundContact && other.gameObject.layer == 1 && Landed && vessel.landedAt == string.Empty && !other.gameObject.CompareTag("Untagged"))
			{
				vessel.SetLandedAt(other.gameObject.tag, other.gameObject);
			}
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(other.gameObject);
			if (partUpwardsCached != null)
			{
				if (partUpwardsCached.state == PartStates.PLACEMENT && !partUpwardsCached.currentCollisions.ContainsKey(other))
				{
					partUpwardsCached.currentCollisions.Add(other, 1);
				}
				if (partUpwardsCached.state == PartStates.CARGO && base.gameObject.layer != 1)
				{
					partUpwardsCached.cargoColliders.AddUnique(collider);
				}
			}
			if (state == PartStates.PLACEMENT)
			{
				currentCollisions.Add(other, 1);
			}
		}
		else if (HighLogic.LoadedSceneIsFlight && state == PartStates.CARGO && other.gameObject.layer != 1)
		{
			cargoColliders.AddUnique(other);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			editorCollision = null;
		}
		else
		{
			if (!HighLogic.LoadedSceneIsFlight)
			{
				return;
			}
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(other.gameObject);
			if (partUpwardsCached != null)
			{
				if (partUpwardsCached.state == PartStates.PLACEMENT)
				{
					partUpwardsCached.currentCollisions.Remove(other);
				}
				if (partUpwardsCached.state == PartStates.CARGO)
				{
					partUpwardsCached.cargoColliders.Remove(collider);
				}
			}
			if (state == PartStates.PLACEMENT)
			{
				currentCollisions.Remove(other);
			}
			if (state == PartStates.CARGO)
			{
				cargoColliders.Remove(other);
			}
		}
	}

	public void OnCollisionStay(Collision c)
	{
		if (checkAfterResetCollisions)
		{
			OnCollisionEnter(c);
		}
	}

	public void ResetCollisions()
	{
		currentCollisions.Clear();
		GroundContact = false;
		if (vessel.GroundContacts.Contains(this))
		{
			vessel.GroundContacts.Remove(this);
			vessel.ResetRBAnchor();
		}
		if (vessel != null)
		{
			vessel.UpdateLandedSplashed();
		}
		checkAfterResetCollisions = true;
		StartCoroutine(CallbackUtil.DelayedCallback(3, ResetCollisionsCheck));
	}

	public void ResetCollisionsCheck()
	{
		checkAfterResetCollisions = false;
	}

	public void SetOpacity(float opacity)
	{
		CreateRendererLists();
		mpb.SetFloat(PropertyIDs._Opacity, opacity);
		int count = highlightRenderer.Count;
		while (count-- > 0)
		{
			highlightRenderer[count].SetPropertyBlock(mpb);
		}
	}

	public void SetShader(string shader)
	{
		Transform transform = partTransform.Find("model");
		if (transform != null)
		{
			MeshRenderer[] componentsInChildren = transform.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				componentsInChildren[num].material.shader = Shader.Find(shader);
			}
		}
	}

	public void propagateControlUpdate(FlightCtrlState st)
	{
		onCtrlUpd(st);
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			children[i].propagateControlUpdate(st);
		}
	}

	public void partPause()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (component != null)
		{
			component.volume = 0f;
		}
		onGamePause();
	}

	public void partUnpause()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (component != null)
		{
			component.volume = GameSettings.SHIP_VOLUME;
		}
		onGameResume();
	}

	public void UpdateMass()
	{
		int count = modules.Count;
		float num = 0f;
		moduleMass = 0f;
		inventoryMass = 0f;
		if (needPrefabMass)
		{
			prefabMass = mass;
			if (partInfo != null && partInfo.partPrefab != null)
			{
				prefabMass = partInfo.partPrefab.mass;
			}
			needPrefabMass = false;
		}
		for (int i = 0; i < count; i++)
		{
			if (modules[i] is IPartMassModifier partMassModifier)
			{
				num = partMassModifier.GetModuleMass(prefabMass, ModifierStagingSituation.CURRENT);
				moduleMass += num;
				if (modules[i] is ModuleInventoryPart)
				{
					inventoryMass += num;
				}
			}
		}
		mass = prefabMass + moduleMass;
		if (mass < partInfo.MinimumMass)
		{
			mass = partInfo.MinimumMass;
		}
	}

	public bool IsUnderConstructionWeightLimit()
	{
		if (CheatOptions.IgnoreEVAConstructionMassLimit)
		{
			return true;
		}
		cpCache = FindModuleImplementing<ModuleCargoPart>();
		if (cpCache != null)
		{
			cpMass = cpCache.MassForWeightTesting;
		}
		else
		{
			cpMass = mass;
		}
		double num = PhysicsGlobals.GravitationalAcceleration;
		if (vessel != null)
		{
			num = EVAConstructionUtil.GetConstructionGee(vessel);
		}
		else if (FlightGlobals.ActiveVessel != null)
		{
			num = EVAConstructionUtil.GetConstructionGee(FlightGlobals.ActiveVessel);
		}
		partWeight = (double)((cpMass + GetResourceMass()) * 1000f) * num;
		if (HighLogic.LoadedSceneIsFlight && GameSettings.EVA_CONSTRUCTION_COMBINE_ENABLED && EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.evaEditor != null)
		{
			return partWeight <= EVAConstructionModeController.Instance.evaEditor.CombinedConstructionWeightLimit;
		}
		return partWeight <= PhysicsGlobals.ConstructionWeightLimit;
	}

	public void FixedUpdate()
	{
		float num = 0f;
		kerbalMass = 0f;
		float num2 = 0f;
		kerbalInventoryMass = 0f;
		float num3 = 0f;
		kerbalResourceMass = 0f;
		if (HighLogic.LoadedSceneIsEditor && editorStarted)
		{
			UpdateMass();
			resourceMass = GetResourceMass();
			if (!(CrewAssignmentDialog.Instance != null) || CrewAssignmentDialog.Instance.CurrentManifestUnsafe == null)
			{
				return;
			}
			PartCrewManifest partCrewManifest = CrewAssignmentDialog.Instance.CurrentManifestUnsafe.GetPartCrewManifest(craftID);
			if (partCrewManifest == null)
			{
				return;
			}
			partCrewManifest.GetPartCrew(ref partCrew);
			if (modules.Contains<KerbalSeat>())
			{
				for (int i = 0; i < partCrew.Length; i++)
				{
					if (partCrew[i] != null)
					{
						KerbalEVA component = PartLoader.getPartInfoByName(partCrew[i].GetKerbalEVAPartName()).partPrefab.GetComponent<KerbalEVA>();
						if (component != null)
						{
							num = component.initialMass * component.massMultiplier;
							mass += num;
							kerbalMass += num;
							num3 = partCrew[i].ResourceMass();
							mass += num3;
							kerbalResourceMass += num3;
							num2 = partCrew[i].InventoryMass();
							mass += num2;
							kerbalInventoryMass += num2;
						}
					}
				}
				return;
			}
			for (int j = 0; j < partCrew.Length; j++)
			{
				if (partCrew[j] != null)
				{
					num = PhysicsGlobals.KerbalCrewMass;
					mass += num;
					kerbalMass += num;
					num3 = partCrew[j].ResourceMass();
					mass += num3;
					kerbalResourceMass += num3;
					num2 = partCrew[j].InventoryMass();
					mass += num2;
					kerbalInventoryMass += num2;
				}
			}
		}
		else
		{
			if (FlightDriver.Pause || state == PartStates.DEAD || state == PartStates.PLACEMENT || state == PartStates.CARGO || !HighLogic.LoadedSceneIsFlight || !started)
			{
				return;
			}
			ValidateInertiaTensor();
			onPartFixedUpdate();
			if (state == PartStates.ACTIVE)
			{
				onActiveFixedUpdate();
				ModulesOnFixedUpdate();
				InternalFixedUpdate();
			}
			else
			{
				UpdateMass();
			}
			if (protoModuleCrew != null)
			{
				if (!isKerbalEVA())
				{
					kerbalMass = (float)protoModuleCrew.Count * PhysicsGlobals.KerbalCrewMass;
					mass += kerbalMass;
				}
				int k = 0;
				for (int count = protoModuleCrew.Count; k < count; k++)
				{
					protoModuleCrew[k].ActiveFixedUpdate(this);
					if (!isKerbalEVA())
					{
						num3 = protoModuleCrew[k].ResourceMass();
						mass += num3;
						kerbalResourceMass += num3;
						num2 = protoModuleCrew[k].InventoryMass();
						mass += num2;
						inventoryMass += num2;
					}
				}
			}
			HeatGaugeUpdate();
			if (state != PartStates.DEAD)
			{
				CheckPartTemp(this);
			}
			if (state != PartStates.DEAD)
			{
				CheckPartPressure(this);
			}
			if (state != PartStates.DEAD)
			{
				CheckPartG(this);
			}
			if (PermanentGroundContact)
			{
				checkPermanentLandedAt();
			}
		}
	}

	public static void _CheckPartTemp(Part p)
	{
		if ((p.temperature > p.maxTemp || p.skinTemperature > p.skinMaxTemp) && !CheatOptions.IgnoreMaxTemperature)
		{
			double num = Math.Max(p.temperature / p.maxTemp, p.skinTemperature / p.skinMaxTemp);
			if ((double)UnityEngine.Random.Range(0f, 1f) <= (double)(p.tempExplodeChance * TimeWarp.fixedDeltaTime) * num)
			{
				string text = cacheAutoLOC_7001406;
				GameEvents.onOverheat.Fire(new EventReport(FlightEvents.OVERHEAT, p, p.partInfo.title, "", 0, (p.temperature > p.maxTemp) ? (p.temperature.ToString("F0") + " / " + p.maxTemp.ToString("F0") + text) : (p.skinTemperature.ToString("F0") + " / " + p.skinMaxTemp.ToString("F0") + text), p.explosionPotential));
				p.explodeOverheat = true;
				p.explode();
			}
		}
	}

	public static void _CheckPartPressure(Part p)
	{
		double num = p.staticPressureAtm * 101.325 + p.dynamicPressurekPa;
		if (!p.ShieldedFromAirstream && num > p.maxPressure * Vessel.warningThresholdPres && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PressurePartLimits)
		{
			double num2 = num / p.maxPressure;
			if (num2 > p.vessel.partMaxPresThresh)
			{
				p.vessel.partMaxPresThresh = num2;
			}
			if (!CheatOptions.NoCrashDamage && num > p.maxPressure && (double)UnityEngine.Random.Range(0f, 1f) <= (double)(p.presExplodeChance * TimeWarp.fixedDeltaTime) * num2)
			{
				GameEvents.onOverPressure.Fire(new EventReport(FlightEvents.OVERPRESSURE, p, p.partInfo.title, "", 0, Localizer.Format("#autoLOC_207654", (p.staticPressureAtm * 101.325 + p.dynamicPressurekPa).ToString("N0") + " / " + p.maxPressure.ToString("N0")), p.explosionPotential));
				p.explode();
			}
		}
	}

	public static void _CheckPartG(Part p)
	{
		if (p.vessel != null && p.vessel.geeForce > p.gTolerance * Vessel.warningThresholdG && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GPartLimits)
		{
			double num = p.vessel.geeForce / p.gTolerance;
			if (num > p.vessel.partMaxGThresh)
			{
				p.vessel.partMaxGThresh = num;
			}
			if (!CheatOptions.NoCrashDamage && p.vessel.geeForce > p.gTolerance && (double)UnityEngine.Random.Range(0f, 1f) <= (double)(p.gExplodeChance * TimeWarp.fixedDeltaTime) * num)
			{
				GameEvents.onOverG.Fire(new EventReport(FlightEvents.OVERG, p, p.partInfo.title, "", 0, p.vessel.geeForce.ToString("F0") + " / " + p.gTolerance.ToString("F0") + " G", p.explosionPotential));
				p.explode();
			}
		}
	}

	public void ValidateInertiaTensor()
	{
		if ((bool)rb && rb.inertiaTensor != inertiaTensor * Mathf.Max(1f, rb.mass) && inertiaTensor.x > 1E-06f && inertiaTensor.y > 1E-06f && inertiaTensor.z > 1E-06f)
		{
			rb.inertiaTensor = inertiaTensor * Mathf.Max(1f, rb.mass);
		}
		if ((bool)servoRb && servoRb.inertiaTensor != inertiaTensor * Mathf.Max(1f, servoRb.mass) && inertiaTensor.x > 1E-06f && inertiaTensor.y > 1E-06f && inertiaTensor.z > 1E-06f)
		{
			servoRb.inertiaTensor = inertiaTensor * Mathf.Max(1f, servoRb.mass);
		}
	}

	public void HeatGaugeUpdate()
	{
		float num = Mathf.Clamp01((float)Math.Max(temperature / maxTemp, skinTemperature / skinMaxTemp));
		float num2 = 0.7f * gaugeThresholdMult;
		if (num >= num2)
		{
			if ((bool)stackIcon.StageIcon)
			{
				if (tempIndicator == null)
				{
					tempIndicator = stackIcon.DisplayInfo();
					tempIndicator.SetMsgBgColor(XKCDColors.DarkRed.smethod_0(0.6f));
					tempIndicator.SetMsgTextColor(XKCDColors.OrangeYellow.smethod_0(0.6f));
					tempIndicator.SetMessage(cacheAutoLOC_7000027);
					tempIndicator.SetProgressBarBgColor(XKCDColors.DarkRed.smethod_0(0.6f));
					tempIndicator.SetProgressBarColor(XKCDColors.OrangeYellow.smethod_0(0.6f));
				}
				tempIndicator.SetValue(num, num2, 1f);
			}
		}
		else if (tempIndicator != null)
		{
			stackIcon.RemoveInfo(tempIndicator);
			tempIndicator = null;
		}
	}

	public void ResetMPB()
	{
		temperatureRenderer = new MaterialColorUpdater(partTransform, PhysicsGlobals.TemperaturePropertyID, this);
	}

	public void Update()
	{
		if (FlightDriver.Pause || state == PartStates.DEAD || state == PartStates.PLACEMENT || state == PartStates.CARGO)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch != null)
		{
			onEditorUpdate();
			updateMirroring();
		}
		UpdateMouseOver();
		if (!HighLogic.LoadedSceneIsFlight || !started || !FlightGlobals.ready)
		{
			return;
		}
		if (temperatureRenderer != null)
		{
			Color blackBodyRadiation = PhysicsGlobals.GetBlackBodyRadiation((float)skinTemperature, this);
			temperatureRenderer.Update(blackBodyRadiation);
		}
		if (overrideSkillUpdate || CrewCapacity > 0)
		{
			partValues.Update();
		}
		onPartUpdate();
		if (state == PartStates.ACTIVE)
		{
			onActiveUpdate();
		}
		ModulesOnUpdate();
		InternalOnUpdate();
		UpdateAeroDisplay();
		if (protoModuleCrew != null)
		{
			int i = 0;
			for (int count = protoModuleCrew.Count; i < count; i++)
			{
				protoModuleCrew[i].ActiveUpdate(this);
			}
		}
	}

	public void ScheduleSetCollisionIgnores()
	{
		needsCollidersReset = true;
	}

	public virtual void LateUpdate()
	{
		if (needsCollidersReset)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				SetCollisionIgnores();
			}
			needsCollidersReset = false;
		}
		DrawAutoStrutLine();
	}

	public void explode()
	{
		explode(0f);
	}

	public void explode(float offset)
	{
		if (state != PartStates.DEAD)
		{
			float num = 0f;
			if (FlightGlobals.ship_srfSpeed > 1000.0 && FlightGlobals.ship_srfSpeed < 200.0)
			{
				num = 0.12f;
			}
			else if (FlightGlobals.ship_srfSpeed >= 2000.0)
			{
				num = 0.25f;
			}
			partTransform.up = FlightGlobals.getUpAxis(FlightGlobals.currentMainBody, partTransform.position);
			partTransform.Translate(0f, offset, 0f);
			ModuleComet moduleComet = FindModuleImplementing<ModuleComet>();
			if (moduleComet == null || (moduleComet != null && (!moduleComet.canSpawnFragments || explodeOverheat)))
			{
				FXMonger.Explode(this, partTransform.position, explosionPotential + num);
			}
			Debug.Log(base.name + " Exploded!! - blast awesomeness: " + explosionPotential);
			GameEvents.onPartExplode.Fire(new GameEvents.ExplosionReaction((this == FlightGlobals.ActiveVessel) ? 0f : Vector3.Distance(partTransform.position, FlightGlobals.ActiveVessel.vesselTransform.position), explosionPotential));
			ModuleAsteroid moduleAsteroid = FindModuleImplementing<ModuleAsteroid>();
			if (moduleAsteroid != null)
			{
				AnalyticsUtil.LogAsteroidEvent(moduleAsteroid, AnalyticsUtil.SpaceObjectEventTypes.exploded, HighLogic.CurrentGame, vessel);
			}
			if (moduleComet != null)
			{
				AnalyticsUtil.LogCometEvent(moduleComet, AnalyticsUtil.SpaceObjectEventTypes.exploded, HighLogic.CurrentGame, vessel);
			}
			Die();
		}
	}

	[ContextMenu("Die")]
	public void Die()
	{
		if (state == PartStates.DEAD)
		{
			return;
		}
		GameEvents.onPartWillDie.Fire(this);
		Part[] array = children.ToArray();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			Part part = array[i];
			if (part.physicalSignificance == PhysicalSignificance.FULL)
			{
				if (part.State != PartStates.DEAD)
				{
					part.decouple();
				}
			}
			else
			{
				part.Die();
			}
		}
		disconnect();
		GameEvents.onVesselPartCountChanged.Fire(vessel);
		if (FindAttachNodeByPart(parent) != null)
		{
			FindAttachNodeByPart(parent).attachedPart = null;
		}
		if ((bool)parent && parent.FindAttachNodeByPart(this) != null)
		{
			parent.FindAttachNodeByPart(this).attachedPart = null;
		}
		vessel.checkLanded();
		vessel.FallBackReferenceTransform();
		setParent();
		CleanSymmetryReferences();
		deactivate();
		attached = false;
		OnJustAboutToDie();
		BroadcastMessage("OnPartDie", SendMessageOptions.DontRequireReceiver);
		GameEvents.onVesselWasModified.Fire(vessel);
		if (vessel.rootPart == this)
		{
			vessel.Die();
		}
		if (FlightCamera.fetch.targetMode == FlightCamera.TargetMode.Part && FlightCamera.fetch.partTarget == this)
		{
			FlightCamera.ClearTarget();
		}
		base.gameObject.SetActive(value: false);
		onPartExplode();
		stackIcon.ClearInfoBoxes();
		stackIcon.RemoveIcon();
		GameEvents.onPartDie.Fire(this);
		if (physicalSignificance == PhysicalSignificance.FULL)
		{
			UnityEngine.Object.Destroy(GetComponent<CollisionEnhancer>());
			if ((bool)terrainCollider)
			{
				UnityEngine.Object.Destroy(terrainCollider);
			}
			if ((bool)partBuoyancy)
			{
				UnityEngine.Object.Destroy(GetComponent<PartBuoyancy>());
			}
			if ((bool)attachJoint)
			{
				attachJoint.DestroyJoint();
			}
		}
		partTransform.parent = null;
		if (protoModuleCrew.Count > 0)
		{
			int j = 0;
			for (int count = protoModuleCrew.Count; j < count; j++)
			{
				ProtoCrewMember protoCrewMember = protoModuleCrew[j];
				protoCrewMember.UnregisterExperienceTraits(this);
				protoCrewMember.Die();
				if (HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn)
				{
					if (crewRespawnTime > 0.0)
					{
						protoCrewMember.StartRespawnPeriod(crewRespawnTime);
					}
					else if (crewRespawnTime == 0.0)
					{
						protoCrewMember.StartRespawnPeriod();
					}
				}
			}
			Vessel.CrewWasModified(vessel);
		}
		DespawnIVA();
		IOUtils.RemoveFlightData(vessel.id);
		if (vessel.rootPart != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		state = PartStates.DEAD;
	}

	public virtual void onBackup()
	{
	}

	public virtual void onFlightStateSave(Dictionary<string, KSPParseable> partDataCollection)
	{
	}

	public virtual void onFlightStateLoad(Dictionary<string, KSPParseable> parsedData)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void onEditorStartTweak()
	{
	}

	public virtual void onEditorEndTweak()
	{
	}

	public virtual void onStartComplete()
	{
	}

	public virtual void onPartLoad()
	{
	}

	public virtual void onPartAwake()
	{
	}

	public virtual void onCopy(Part original, bool asSymCounterpart)
	{
	}

	public virtual void onWillBeCopied(bool asSymCounterpart)
	{
	}

	public virtual void onWasCopied(Part copyPart, bool asSymCounterpart)
	{
	}

	public virtual void onPartStart()
	{
	}

	public virtual void onPartAttach(Part parent)
	{
	}

	public virtual void onPartDetach()
	{
	}

	public virtual void onFlightStart()
	{
	}

	public virtual void onFlightStartAtLaunchPad()
	{
	}

	public virtual void onDecouple(float breakForce)
	{
	}

	public virtual void onDisconnect()
	{
	}

	public virtual void onPack()
	{
	}

	public virtual void onUnpack()
	{
	}

	public virtual void onJointDisable()
	{
	}

	public virtual void onJointReset()
	{
	}

	public virtual void onPartLiftOff()
	{
	}

	public virtual void onPartTouchdown()
	{
	}

	public virtual void onPartSplashdown()
	{
	}

	public virtual bool onPartActivate()
	{
		return true;
	}

	public virtual void onCtrlUpd(FlightCtrlState s)
	{
	}

	public virtual void onGamePause()
	{
	}

	public virtual void onGameResume()
	{
	}

	public virtual void onPartFixedUpdate()
	{
	}

	public virtual void onPartUpdate()
	{
	}

	public virtual void onEditorUpdate()
	{
	}

	public virtual void onActiveFixedUpdate()
	{
	}

	public virtual void onActiveUpdate()
	{
	}

	public virtual void onPartDeactivate()
	{
	}

	public virtual void onPartExplode()
	{
	}

	public virtual void onPartDelete()
	{
	}

	public virtual void onPartDestroy()
	{
	}

	public static uint getFuelReqId()
	{
		fuelRequestID = (fuelRequestID + 1) % uint.MaxValue;
		return fuelRequestID;
	}

	[Obsolete("Use Part.RequestResource instead.")]
	public virtual bool RequestFuel(Part source, float amount, uint reqId)
	{
		if (reqId == lastFuelRequestId)
		{
			return false;
		}
		lastFuelRequestId = reqId;
		if (state == PartStates.DEAD)
		{
			return false;
		}
		if (fuelLookupTargets.Count > 0)
		{
			bool result = false;
			int count = fuelLookupTargets.Count;
			float amount2 = amount / (float)count;
			for (int i = 0; i < count; i++)
			{
				if ((fuelLookupTargets[i] == parent && attached) || (fuelLookupTargets[i] != parent && fuelLookupTargets[i].attached))
				{
					if (fuelLookupTargets[i].RequestFuel(source, amount2, reqId))
					{
						result = true;
					}
					else
					{
						i--;
					}
				}
			}
			return result;
		}
		if (!fuelCrossFeed && !(source == this))
		{
			return false;
		}
		resourceTargets.Clear();
		int count2 = attachNodes.Count;
		for (int j = 0; j < count2; j++)
		{
			AttachNode attachNode = attachNodes[j];
			if ((!(NoCrossFeedNodeKey != string.Empty) || !attachNode.id.Contains(NoCrossFeedNodeKey)) && !(attachNode.attachedPart == null) && attachNode.attachedPart.lastFuelRequestId != reqId)
			{
				resourceTargets.Add(attachNode.attachedPart);
			}
		}
		int count3 = resourceTargets.Count;
		if (count3 > 0)
		{
			bool flag = false;
			float amount3 = amount / (float)count3;
			for (int k = 0; k < count3; k++)
			{
				if (resourceTargets[k].RequestFuel(source, amount3, reqId))
				{
					flag = true;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		if (parent != null)
		{
			AttachNode attachNode2 = FindAttachNodeByPart(parent);
			if (attachNode2 != null && (NoCrossFeedNodeKey == string.Empty || !attachNode2.id.Contains(NoCrossFeedNodeKey)) && parent.RequestFuel(source, amount, reqId))
			{
				return true;
			}
		}
		return false;
	}

	[Obsolete("Use Part.GetConnectedResources instead.")]
	public virtual bool FindFuel(Part source, List<Part> fuelSources, uint reqId)
	{
		if (reqId == lastFuelRequestId)
		{
			return false;
		}
		lastFuelRequestId = reqId;
		if (state == PartStates.DEAD)
		{
			return false;
		}
		if (fuelLookupTargets.Count > 0)
		{
			bool flag = false;
			int i = 0;
			for (int count = fuelLookupTargets.Count; i < count; i++)
			{
				if (((fuelLookupTargets[i] == parent && attached) || (fuelLookupTargets[i] != parent && fuelLookupTargets[i].attached)) && fuelLookupTargets[i].FindFuel(source, fuelSources, reqId))
				{
					flag = true;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		if (!fuelCrossFeed && !(source == this))
		{
			return false;
		}
		resourceTargets.Clear();
		int j = 0;
		for (int count2 = attachNodes.Count; j < count2; j++)
		{
			AttachNode attachNode = attachNodes[j];
			if ((!(NoCrossFeedNodeKey != string.Empty) || !attachNode.id.Contains(NoCrossFeedNodeKey)) && !(attachNode.attachedPart == null) && attachNode.attachedPart.lastFuelRequestId != reqId)
			{
				resourceTargets.Add(attachNode.attachedPart);
			}
		}
		int count3 = resourceTargets.Count;
		if (count3 > 0)
		{
			bool flag2 = false;
			for (int k = 0; k < count3; k++)
			{
				if (resourceTargets[k].FindFuel(source, fuelSources, reqId))
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				return true;
			}
		}
		if (parent != null)
		{
			AttachNode attachNode2 = FindAttachNodeByPart(parent);
			if (attachNode2 != null && (NoCrossFeedNodeKey == string.Empty || !attachNode2.id.Contains(NoCrossFeedNodeKey)) && parent.FindFuel(source, fuelSources, reqId))
			{
				return true;
			}
		}
		return false;
	}

	[Obsolete("Use Part.TransferResource instead.")]
	public virtual bool DrainFuel(float amount)
	{
		return false;
	}

	[Obsolete("Use Part.RequestResource instead.")]
	public virtual bool RequestRCS(float amount, int earliestStage)
	{
		if (!connected)
		{
			return false;
		}
		int num = 0;
		int count = children.Count;
		while (true)
		{
			if (num < count)
			{
				if (children[num].RequestRCS(amount, earliestStage))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public string drawStats()
	{
		return OnGetStats() ?? "";
	}

	public virtual string OnGetStats()
	{
		return "";
	}

	[Obsolete("Functional behaviour should really be happening in PartModules now. In any case, this method's been replaced with OnGetStats, where you just return the string.")]
	public virtual void OnDrawStats()
	{
	}

	public void OnDrawGizmos()
	{
		if (HighLogic.LoadedScene == GameScenes.LOADING)
		{
			return;
		}
		if (drawDragCubeGizmo)
		{
			OnDrawGizmos_DragCube();
		}
		if (drawAttachNodeGizmo)
		{
			if (attachNodes != null)
			{
				int i = 0;
				for (int count = attachNodes.Count; i < count; i++)
				{
					AttachNode attachNode = attachNodes[i];
					Gizmos.color = ((attachNode.attachedPart == null) ? XKCDColors.RadioactiveGreen : XKCDColors.SteelGrey);
					Gizmos.DrawWireSphere(partTransform.TransformPoint(attachNode.position), attachNode.radius);
				}
			}
			if (attachRules.srfAttach)
			{
				Gizmos.color = XKCDColors.Amber;
				Gizmos.DrawWireSphere(partTransform.TransformPoint(srfAttachNode.position), srfAttachNode.radius - 0.1f);
			}
		}
		if (GroundContact || PermanentGroundContact)
		{
			Gizmos.color = ((!PermanentGroundContact) ? XKCDColors.KSPBadassGreen : XKCDColors.KSPNotSoGoodOrange).smethod_0(0.3f);
			Gizmos.DrawCube(partTransform.position, Vector3.one);
		}
	}

	public void OnDrawGizmos_DragCube()
	{
		dragCubes.DrawDragGizmos();
	}

	public Part FindChildPart(string childName)
	{
		return FindChildPart(childName, recursive: false);
	}

	public Part FindChildPart(string childName, bool recursive)
	{
		if (recursive)
		{
			return FindChildPart(this, childName);
		}
		int count = children.Count;
		int num = 0;
		Part part;
		while (true)
		{
			if (num < count)
			{
				part = children[num];
				if (part.gameObject.name == childName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return part;
	}

	public static Part FindChildPart(Part parent, string childName)
	{
		if (parent == null)
		{
			return null;
		}
		Part part = null;
		int count = parent.children.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Part part2 = parent.children[num];
				if (!(part2.gameObject.name == childName))
				{
					part = FindChildPart(part2, childName);
					if (part != null)
					{
						break;
					}
					num++;
					continue;
				}
				return part2;
			}
			return null;
		}
		return part;
	}

	public T FindChildPart<T>() where T : Part
	{
		return FindChildPart<T>(recursive: false);
	}

	public T FindChildPart<T>(bool recursive) where T : Part
	{
		if (recursive)
		{
			return FindChildPart<T>(this);
		}
		int count = children.Count;
		int num = 0;
		Part part;
		while (true)
		{
			if (num < count)
			{
				part = children[num];
				if (part is T)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return (T)part;
	}

	public static T FindChildPart<T>(Part parent) where T : Part
	{
		if (parent == null)
		{
			return null;
		}
		T val = null;
		int count = parent.children.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Part part = parent.children[num];
				if (!(part is T))
				{
					val = FindChildPart<T>(part);
					if (val != null)
					{
						break;
					}
					num++;
					continue;
				}
				return (T)part;
			}
			return null;
		}
		return val;
	}

	public T[] FindChildParts<T>() where T : Part
	{
		return FindChildParts<T>(recursive: false);
	}

	public T[] FindChildParts<T>(bool recursive) where T : Part
	{
		List<T> list = new List<T>();
		if (recursive)
		{
			FindChildParts(this, list);
		}
		else
		{
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = children[i];
				if (part is T)
				{
					list.Add((T)part);
				}
			}
		}
		return list.ToArray();
	}

	public static void FindChildParts<T>(Part parent, List<T> tList) where T : Part
	{
		if (parent == null)
		{
			return;
		}
		int count = parent.children.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parent.children[i];
			if (part is T)
			{
				tList.Add((T)part);
			}
			FindChildParts(part, tList);
		}
	}

	public Transform FindModelTransform(string childName)
	{
		return FindHeirarchyTransform(partTransform.Find("model"), childName);
	}

	public static Transform FindHeirarchyTransform(Transform parent, string childName)
	{
		if (parent == null)
		{
			return null;
		}
		if (parent.gameObject.name == childName)
		{
			return parent;
		}
		Transform transform = null;
		int num = 0;
		while (true)
		{
			if (num < parent.childCount)
			{
				transform = FindHeirarchyTransform(parent.GetChild(num), childName);
				if (transform != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return transform;
	}

	public Transform FindModelTransformByLayer(string layerName)
	{
		return FindHeirarchyTransformByLayer(partTransform.Find("model"), layerName);
	}

	public static Transform FindHeirarchyTransformByLayer(Transform parent, string layerName)
	{
		if (parent == null)
		{
			return null;
		}
		if (LayerMask.LayerToName(parent.gameObject.layer).Equals(layerName))
		{
			return parent;
		}
		Transform transform = null;
		int num = 0;
		while (true)
		{
			if (num < parent.childCount)
			{
				transform = FindHeirarchyTransformByLayer(parent.GetChild(num), layerName);
				if (transform != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return transform;
	}

	public Transform[] FindModelTransforms(string childName)
	{
		List<Transform> list = new List<Transform>();
		FindHeirarchyTransforms(partTransform.Find("model"), childName, list);
		return list.ToArray();
	}

	public Transform[] FindModelTransformsWithTag(string tag)
	{
		List<Transform> list = new List<Transform>();
		FindHeirarchyTransformsByTag(partTransform.Find("model"), tag, list);
		return list.ToArray();
	}

	public static void FindHeirarchyTransforms(Transform parent, string childName, List<Transform> tList)
	{
		if (!(parent == null))
		{
			if (parent.gameObject.name == childName)
			{
				tList.Add(parent);
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				FindHeirarchyTransforms(parent.GetChild(i), childName, tList);
			}
		}
	}

	public static void FindHeirarchyTransformsByTag(Transform parent, string tag, List<Transform> tList)
	{
		if (!(parent == null))
		{
			if (parent.CompareTag(tag))
			{
				tList.Add(parent);
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				FindHeirarchyTransformsByTag(parent.GetChild(i), tag, tList);
			}
		}
	}

	public T FindModelComponent<T>() where T : Component
	{
		if (HasModuleImplementing<KerbalEVA>())
		{
			T val = FindModelComponent<T>(partTransform.Find("model01"), "");
			if (val != null)
			{
				return val;
			}
		}
		if (HasModuleImplementing<ModuleAsteroid>())
		{
			T val2 = FindModelComponent<T>(partTransform.Find("Asteroid"), "");
			if (val2 != null)
			{
				return val2;
			}
		}
		if (HasModuleImplementing<ModuleComet>())
		{
			T val3 = FindModelComponent<T>(partTransform.Find("Comet"), "");
			if (val3 != null)
			{
				return val3;
			}
		}
		return FindModelComponent<T>(partTransform.Find("model"), "");
	}

	public T FindModelComponent<T>(string childName) where T : Component
	{
		if (HasModuleImplementing<KerbalEVA>())
		{
			T val = FindModelComponent<T>(partTransform.Find("model01"), childName);
			if (val != null)
			{
				return val;
			}
		}
		if (HasModuleImplementing<ModuleAsteroid>())
		{
			T val2 = FindModelComponent<T>(partTransform.Find("Asteroid"), childName);
			if (val2 != null)
			{
				return val2;
			}
		}
		if (HasModuleImplementing<ModuleComet>())
		{
			T val3 = FindModelComponent<T>(partTransform.Find("Comet"), childName);
			if (val3 != null)
			{
				return val3;
			}
		}
		return FindModelComponent<T>(partTransform.Find("model"), childName);
	}

	public static T FindModelComponent<T>(Transform parent, string childName) where T : Component
	{
		if (parent == null)
		{
			return null;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		int num = 0;
		T val;
		while (true)
		{
			if (num < parent.childCount)
			{
				val = FindModelComponent<T>(parent.GetChild(num), childName);
				if (val != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return val;
	}

	public List<T> FindModelComponents<T>() where T : Component
	{
		List<T> list = new List<T>();
		if (partTransform != null)
		{
			if (HasModuleImplementing<KerbalEVA>())
			{
				FindModelComponents(partTransform.Find("model01"), "", list);
			}
			if (HasModuleImplementing<ModuleAsteroid>())
			{
				FindModelComponents(partTransform.Find("Asteroid"), "", list);
			}
			if (HasModuleImplementing<ModuleComet>())
			{
				FindModelComponents(partTransform.Find("Comet"), "", list);
			}
			FindModelComponents(partTransform.Find("model"), "", list);
		}
		return list;
	}

	public List<T> FindModelComponents<T>(string childName) where T : Component
	{
		List<T> list = new List<T>();
		if (partTransform != null)
		{
			if (HasModuleImplementing<KerbalEVA>())
			{
				FindModelComponents(partTransform.Find("model01"), childName, list);
			}
			if (HasModuleImplementing<ModuleAsteroid>())
			{
				FindModelComponents(partTransform.Find("Asteroid"), childName, list);
			}
			if (HasModuleImplementing<ModuleComet>())
			{
				FindModelComponents(partTransform.Find("Comet"), childName, list);
			}
			FindModelComponents(partTransform.Find("model"), childName, list);
		}
		return list;
	}

	public static void FindModelComponents<T>(Transform parent, string childName, List<T> tList) where T : Component
	{
		if (parent == null)
		{
			return;
		}
		if (childName == string.Empty || parent.gameObject.name == childName)
		{
			T component = parent.gameObject.GetComponent<T>();
			if (component != null)
			{
				tList.Add(component);
			}
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			FindModelComponents(parent.GetChild(i), childName, tList);
		}
	}

	public void ResetAllModelComponentCacheLists()
	{
		modelRenderersCache = null;
		modelMeshRenderersCache = null;
		modelSkinnedMeshRenderersCache = null;
	}

	public List<MeshRenderer> FindModelMeshRenderersCached()
	{
		if (modelMeshRenderersCache != null)
		{
			for (int i = 0; i < modelMeshRenderersCache.Count; i++)
			{
				if (modelMeshRenderersCache[i] == null)
				{
					ResetModelMeshRenderersCache();
					break;
				}
			}
		}
		if (modelMeshRenderersCache == null)
		{
			modelMeshRenderersCache = FindModelComponents<MeshRenderer>();
		}
		return new List<MeshRenderer>(modelMeshRenderersCache);
	}

	public void ResetModelMeshRenderersCache()
	{
		modelMeshRenderersCache = null;
	}

	public List<SkinnedMeshRenderer> FindModelSkinnedMeshRenderersCached()
	{
		if (modelSkinnedMeshRenderersCache != null)
		{
			for (int i = 0; i < modelSkinnedMeshRenderersCache.Count; i++)
			{
				if (modelSkinnedMeshRenderersCache[i] == null)
				{
					ResetModelSkinnedMeshRenderersCache();
					break;
				}
			}
		}
		if (modelSkinnedMeshRenderersCache == null)
		{
			modelSkinnedMeshRenderersCache = FindModelComponents<SkinnedMeshRenderer>();
		}
		return new List<SkinnedMeshRenderer>(modelSkinnedMeshRenderersCache);
	}

	public void ResetModelSkinnedMeshRenderersCache()
	{
		modelSkinnedMeshRenderersCache = null;
	}

	public List<Renderer> FindModelRenderersCached()
	{
		if (modelRenderersCache != null)
		{
			for (int i = 0; i < modelRenderersCache.Count; i++)
			{
				if (modelRenderersCache[i] == null)
				{
					ResetModelRenderersCache();
					break;
				}
			}
		}
		if (modelRenderersCache == null)
		{
			modelRenderersCache = FindModelComponents<Renderer>();
		}
		return new List<Renderer>(modelRenderersCache);
	}

	public void ResetModelRenderersCache()
	{
		modelRenderersCache = null;
	}

	public Animation FindModelAnimator(string animatorName, string clipName)
	{
		List<Animation> list = FindModelComponents<Animation>();
		int count = list.Count;
		Animation animation;
		do
		{
			if (count-- > 0)
			{
				animation = list[count];
				continue;
			}
			return null;
		}
		while (animation.gameObject.name != animatorName);
		if (animation.GetClip(clipName) == null)
		{
			Debug.LogError("Cannot find clip '" + clipName + "' on animation '" + animatorName + "'");
			return null;
		}
		return animation;
	}

	public static Animation FindModelAnimator(Transform modelTransform, string animatorName, string clipName)
	{
		List<Animation> list = new List<Animation>();
		FindModelComponents(modelTransform, animatorName, list);
		int count = list.Count;
		Animation animation;
		do
		{
			if (count-- > 0)
			{
				animation = list[count];
				continue;
			}
			return null;
		}
		while (animation.gameObject.name != animatorName);
		if (animation.GetClip(clipName) == null)
		{
			Debug.LogError("Cannot find clip '" + clipName + "' on animation '" + animatorName + "'");
			return null;
		}
		return animation;
	}

	public Animation[] FindModelAnimators(string clipName)
	{
		List<Animation> list = FindModelComponents<Animation>();
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].GetClip(clipName) == null)
			{
				list.RemoveAt(count);
			}
		}
		return list.ToArray();
	}

	public Animation FindModelAnimator(string clipName)
	{
		List<Animation> list = FindModelComponents<Animation>();
		int num = 0;
		int count = list.Count;
		while (true)
		{
			if (num < count)
			{
				if (list[num].GetClip(clipName) != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return list[num];
	}

	public Animation[] FindModelAnimators()
	{
		return FindModelComponents<Animation>().ToArray();
	}

	public void ModularSetup()
	{
		PartAttributes = GetReflectedAttributes(GetType());
		events = new BaseEventList(this, null);
		actions = new BaseActionList(this, null);
		fields = new BaseFieldList(this);
	}

	public static ReflectedAttributes GetReflectedAttributes(Type partModuleType)
	{
		if (reflectedAttributeCache.TryGetValue(partModuleType, out var value))
		{
			return value;
		}
		value = new ReflectedAttributes(partModuleType);
		reflectedAttributeCache[partModuleType] = value;
		return value;
	}

	public void ModulesOnActivate()
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				if (modules[i].stagingEnabled)
				{
					modules[i].OnActive();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + modules[i].moduleName + " threw during OnActive " + ex);
			}
		}
	}

	public void ModulesOnDeactivate()
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				modules[i].OnInactive();
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + modules[i].moduleName + " threw during OnInactive: " + ex);
			}
		}
	}

	public PartModule.StartState GetModuleStartState()
	{
		PartModule.StartState startState = PartModule.StartState.None;
		if (HighLogic.LoadedSceneIsEditor)
		{
			startState |= PartModule.StartState.Editor;
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			if (vessel == null)
			{
				return startState;
			}
			if (vessel.situation == Vessel.Situations.PRELAUNCH)
			{
				startState |= PartModule.StartState.PreLaunch;
				startState |= PartModule.StartState.Landed;
			}
			if (vessel.situation == Vessel.Situations.DOCKED)
			{
				startState |= PartModule.StartState.Docked;
			}
			if (vessel.situation == Vessel.Situations.ORBITING || vessel.situation == Vessel.Situations.ESCAPING)
			{
				startState |= PartModule.StartState.Orbital;
			}
			if (vessel.situation == Vessel.Situations.SUB_ORBITAL)
			{
				startState |= PartModule.StartState.SubOrbital;
			}
			if (vessel.situation == Vessel.Situations.SPLASHED)
			{
				startState |= PartModule.StartState.Splashed;
			}
			if (vessel.situation == Vessel.Situations.FLYING)
			{
				startState |= PartModule.StartState.Flying;
			}
			if (vessel.situation == Vessel.Situations.LANDED)
			{
				startState |= PartModule.StartState.Landed;
			}
		}
		return startState;
	}

	public void ModulesOnStart()
	{
		PartModule.StartState moduleStartState = GetModuleStartState();
		bool flag = false;
		BaseEvent baseEvent = Events["ShowUpgradeStats"];
		Events["ShowUpgradeStats"].guiActive = false;
		baseEvent.guiActiveEditor = false;
		int count = modules.Count;
		float num = 0f;
		if (needPrefabMass)
		{
			prefabMass = mass;
			if (partInfo != null && partInfo.partPrefab != null)
			{
				prefabMass = partInfo.partPrefab.mass;
			}
			needPrefabMass = false;
		}
		for (int i = 0; i < count; i++)
		{
			PartModule partModule = modules[i];
			partModule.resHandler.SetPartModule(partModule);
			try
			{
				if (partModule.ApplyUpgrades(moduleStartState))
				{
					flag = true;
				}
				else if (partModule.AppliedUpgrades())
				{
					flag = true;
				}
				partModule.OnStart(moduleStartState);
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + partModule.moduleName + " threw during OnStart: " + ex);
			}
			partModule.UpdateStagingToggle();
			if (partModule is IPartMassModifier partMassModifier)
			{
				num += partMassModifier.GetModuleMass(prefabMass, ModifierStagingSituation.CURRENT);
			}
		}
		mass = prefabMass + num;
		if (mass < partInfo.MinimumMass)
		{
			mass = partInfo.MinimumMass;
		}
		if (moduleStartState == PartModule.StartState.Editor)
		{
			Events["ShowUpgradeStats"].guiActiveEditor = flag;
		}
		else
		{
			Events["ShowUpgradeStats"].guiActive = flag;
		}
	}

	public void ModulesOnStartFinished()
	{
		PartModule.StartState moduleStartState = GetModuleStartState();
		int i = 0;
		for (int count = Modules.Count; i < count; i++)
		{
			PartModule partModule = modules[i];
			modules[i].GetType();
			try
			{
				partModule.OnStartFinished(moduleStartState);
				partModule.ApplyAdjustersOnStart();
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + partModule.moduleName + " threw during OnStartFinished: " + ex);
			}
		}
	}

	public void ModulesBeforePartAttachJoint()
	{
		PartModule.StartState moduleStartState = GetModuleStartState();
		int i = 0;
		for (int count = Modules.Count; i < count; i++)
		{
			PartModule partModule = modules[i];
			try
			{
				partModule.OnStartBeforePartAttachJoint(moduleStartState);
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + partModule.moduleName + " threw during ModulesBeforePartAttachJoint: " + ex);
			}
		}
	}

	public void ModulesOnFixedUpdate()
	{
		int count = modules.Count;
		float num = 0f;
		if (needPrefabMass)
		{
			prefabMass = mass;
			if (partInfo != null && partInfo.partPrefab != null)
			{
				prefabMass = partInfo.partPrefab.mass;
			}
			needPrefabMass = false;
		}
		for (int i = 0; i < count; i++)
		{
			try
			{
				PartModule partModule = modules[i];
				if (partModule.isEnabled)
				{
					modules[i].OnFixedUpdate();
				}
				if (partModule is IPartMassModifier partMassModifier)
				{
					num += partMassModifier.GetModuleMass(prefabMass, ModifierStagingSituation.CURRENT);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + modules[i].moduleName + " threw during OnFixedUpdate: " + ex);
			}
		}
		mass = prefabMass + num;
		if (mass < partInfo.MinimumMass)
		{
			mass = partInfo.MinimumMass;
		}
	}

	public void ModulesOnUpdate()
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				if (modules[i].isEnabled)
				{
					modules[i].OnUpdate();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + modules[i].moduleName + " threw during OnUpdate: " + ex);
			}
		}
	}

	public void InitializeModules()
	{
		int count = modules.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				modules[i].OnInitialize();
				modules[i].UpdateStagingToggle();
			}
			catch (Exception ex)
			{
				Debug.LogError("Module " + modules[i].moduleName + " threw during OnInitialize: " + ex);
			}
		}
	}

	public PartModule AddModule(string moduleName, bool forceAwake = false)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(PartModule), moduleName);
		if (classByName == null)
		{
			Debug.LogError("Cannot find a PartModule of typename '" + moduleName + "'");
			return null;
		}
		if (classByName == typeof(ModuleCargoPart) && modules.Contains("ModuleInventoryPart"))
		{
			Debug.LogWarningFormat("[Part]: Cannot add {0} as this Part '{1}' already has a ModuleInventoryPart PartModule and they are mutually exclusive.", moduleName, base.name);
			return null;
		}
		if (classByName == typeof(ModuleInventoryPart) && modules.Contains("ModuleCargoPart"))
		{
			ModuleCargoPart moduleCargoPart = FindModuleImplementing<ModuleCargoPart>();
			if (moduleCargoPart != null && moduleCargoPart.packedVolume >= 0f)
			{
				RemoveModule(modules["ModuleCargoPart"]);
			}
			Debug.LogWarningFormat("[Part]: Cannot have ModuleCargoPart and ModuleInventoryPart on same Part [{0}'. Removed ModuleCargoPart", base.name);
		}
		PartModule partModule = (PartModule)base.gameObject.AddComponent(classByName);
		if (partModule == null)
		{
			Debug.LogError("Cannot create a PartModule of typename '" + moduleName + "'");
			return null;
		}
		if (forceAwake)
		{
			partModule.Awake();
		}
		modules.Add(partModule);
		return partModule;
	}

	public PartModule AddModule(ConfigNode node, bool forceAwake = false)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add a PartModule because the ConfigNode contains no module name");
			return null;
		}
		PartModule partModule = AddModule(node.GetValue("name"));
		if (partModule == null)
		{
			return null;
		}
		if (forceAwake)
		{
			partModule.Awake();
		}
		partModule.Load(node);
		return partModule;
	}

	public PartModule LoadModule(ConfigNode node, ref int moduleIndex)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot load PartModule #" + moduleIndex + " because ConfigNode contains no module name", base.gameObject);
			return null;
		}
		string value = node.GetValue("name");
		PartModule partModule;
		if (moduleIndex >= modules.Count)
		{
			Debug.LogWarning("[Part]: PartModule " + value + " at " + partInfo.name + ", index " + moduleIndex + ": index exceeds module count as defined in cfg.\nLooking for " + value + " in other indices...", base.gameObject);
			partModule = null;
		}
		else
		{
			partModule = modules[moduleIndex];
			if (partModule.moduleName != value)
			{
				Debug.LogWarning("[Part]: PartModule indexing mismatch at " + partInfo.name + ", index " + moduleIndex + ".\nNode '" + value + "' found in loaded data, but '" + partModule.moduleName + "' is defined in prefab.\nLooking for " + value + " in other indices...", base.gameObject);
				partModule = null;
			}
		}
		if (partModule == null)
		{
			int num = -1;
			int count = modules.Count;
			for (int i = 0; i < count; i++)
			{
				if (modules[i].moduleName == value)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				Debug.LogWarning("..." + value + " module found at index " + num + ".", base.gameObject);
				partModule = modules[num];
				moduleIndex++;
			}
			else
			{
				Debug.LogWarning("...no " + value + " module found on part definition. Skipping...", base.gameObject);
			}
		}
		else
		{
			moduleIndex++;
		}
		if (partModule == null)
		{
			return null;
		}
		partModule.Load(node);
		return partModule;
	}

	public void RemoveModule(PartModule module)
	{
		if (module.part != this)
		{
			Debug.LogError("Cannot destroy a module which does not belong to this part");
			return;
		}
		modules.Remove(module);
		UnityEngine.Object.DestroyImmediate(module);
		module = null;
	}

	public void RemoveModules()
	{
		modules.Clear();
	}

	public void ClearModuleReferenceCache()
	{
		if (cachedModules != null)
		{
			cachedModules.Clear();
		}
		if (cachedModuleLists != null)
		{
			cachedModuleLists.Clear();
		}
	}

	public T FindModuleImplementing<T>() where T : class
	{
		if (modules != null)
		{
			Type typeFromHandle = typeof(T);
			if (cachedModules == null)
			{
				cachedModules = new Dictionary<Type, PartModule>();
			}
			if (cachedModules.ContainsKey(typeFromHandle))
			{
				return cachedModules[typeFromHandle] as T;
			}
			int count = modules.Count;
			for (int i = 0; i < count; i++)
			{
				PartModule partModule = modules[i];
				if (partModule is T)
				{
					cachedModules.Add(typeFromHandle, partModule);
					return partModule as T;
				}
			}
		}
		return null;
	}

	public bool HasModuleImplementing<T>() where T : class
	{
		return FindModuleImplementing<T>() != null;
	}

	public T FindParentModuleImplementing<T>() where T : class
	{
		if (parent != null)
		{
			T val = parent.FindModuleImplementing<T>();
			if (val != null)
			{
				return val;
			}
			return parent.FindParentModuleImplementing<T>();
		}
		return null;
	}

	public List<T> FindModulesImplementing<T>() where T : class
	{
		List<T> list = new List<T>();
		if (modules != null)
		{
			Type typeFromHandle = typeof(T);
			if (cachedModuleLists.ContainsKey(typeFromHandle))
			{
				int count = cachedModuleLists[typeFromHandle].Count;
				for (int i = 0; i < count; i++)
				{
					list.Add(cachedModuleLists[typeFromHandle][i] as T);
				}
			}
			else
			{
				int count2 = modules.Count;
				for (int j = 0; j < count2; j++)
				{
					PartModule partModule = modules[j];
					if (partModule is T)
					{
						list.Add(partModule as T);
					}
				}
			}
		}
		return list;
	}

	public void AddAttachNode(ConfigNode node)
	{
		string value = node.GetValue("name");
		string value2 = node.GetValue("transform");
		if (value == null)
		{
			throw new Exception("Part: Cannot add attach node. Node requires a 'name' value.");
		}
		if (value2 == null)
		{
			throw new Exception("Part: Cannot add attach node. Node requires a 'transform' value.");
		}
		Transform transform = FindModelTransform(value2);
		if (transform == null)
		{
			throw new Exception("Part: Cannot add attach node. Transform of name '" + value2 + "' not found");
		}
		int result = 1;
		if (node.HasValue("size") && !int.TryParse(node.GetValue("size"), out result))
		{
			Debug.LogError("Part: Attach Node ('" + value + "') 'size' value is incorrect format");
			result = 1;
		}
		AttachNodeMethod attachNodeMethod = AttachNodeMethod.FIXED_JOINT;
		if (value == "srfAttach" || value == "attach")
		{
			attachNodeMethod = AttachNodeMethod.HINGE_JOINT;
		}
		if (node.HasValue("method"))
		{
			try
			{
				attachNodeMethod = (AttachNodeMethod)Enum.Parse(typeof(AttachNodeMethod), node.GetValue("method"), ignoreCase: true);
			}
			catch
			{
				Debug.LogError("Part: Attach Node ('" + value + "') 'method' value is incorrect format");
			}
		}
		bool value3 = true;
		node.TryGetValue("crossfeed", ref value3);
		bool value4 = false;
		node.TryGetValue("rigid", ref value4);
		AttachNode attachNode = new AttachNode(value, transform, result, attachNodeMethod, value3, value4);
		attachNode.owner = this;
		attachNodes.Add(attachNode);
	}

	public void UpdateAttachNodes()
	{
		int count = attachNodes.Count;
		for (int i = 0; i < count; i++)
		{
			AttachNode attachNode = attachNodes[i];
			if (!(attachNode.nodeTransform == null))
			{
				attachNode.position = base.transform.InverseTransformPoint(attachNode.nodeTransform.position);
				attachNode.orientation = base.transform.InverseTransformDirection(attachNode.nodeTransform.forward);
			}
		}
	}

	public void UpdateSrfAttachNode()
	{
		if (srfAttachNode != null && !(srfAttachNode.nodeTransform == null))
		{
			srfAttachNode.position = base.transform.InverseTransformPoint(srfAttachNode.nodeTransform.position);
			srfAttachNode.orientation = base.transform.InverseTransformDirection(srfAttachNode.nodeTransform.forward);
		}
	}

	public void SetupAttachNodes()
	{
		topNode = FindAttachNode("top");
		srfAttachNode = FindAttachNode("attach");
		if (srfAttachNode == null)
		{
			srfAttachNode = FindAttachNode("srfAttach");
		}
		else
		{
			srfAttachNode.id = "srfAttach";
		}
	}

	public AttachNode FindAttachNode(string nodeId)
	{
		int count = attachNodes.Count;
		int num = 0;
		AttachNode attachNode;
		while (true)
		{
			if (num < count)
			{
				attachNode = attachNodes[num];
				if (attachNode.id == nodeId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return attachNode;
	}

	public AttachNode[] FindAttachNodes(string partialNodeId)
	{
		List<AttachNode> list = new List<AttachNode>();
		int count = attachNodes.Count;
		for (int i = 0; i < count; i++)
		{
			AttachNode attachNode = attachNodes[i];
			if (attachNode.id.Contains(partialNodeId))
			{
				list.Add(attachNode);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public AttachNode FindAttachNodeByPart(Part connectedPart)
	{
		if (connectedPart == null)
		{
			return null;
		}
		int count = attachNodes.Count;
		int num = 0;
		AttachNode attachNode;
		while (true)
		{
			if (num < count)
			{
				attachNode = attachNodes[num];
				if (attachNode.attachedPart == connectedPart)
				{
					break;
				}
				num++;
				continue;
			}
			if (srfAttachNode.attachedPart == connectedPart)
			{
				return srfAttachNode;
			}
			return null;
		}
		return attachNode;
	}

	public void EffectSetup()
	{
		if (effects == null)
		{
			effects = new EffectList(this);
		}
	}

	public void InitializeEffects()
	{
		Effects.Initialize();
	}

	public void LoadEffects(ConfigNode node)
	{
		effects.OnLoad(node);
	}

	public void SaveEffects(ConfigNode node)
	{
		effects.OnSave(node);
	}

	public void Effect(string effectName, int transformIdx = -1)
	{
		Effects.Event(effectName, transformIdx);
	}

	public void Effect(string effectName, float effectPower, int transformIdx = -1)
	{
		Effects.Event(effectName, effectPower, transformIdx);
	}

	public void AttributeSetup()
	{
		if (PartAttributes.partInfo.Length != 0 && !string.IsNullOrEmpty(PartAttributes.partInfo[0].name))
		{
			partName = PartAttributes.partInfo[0].name;
		}
		else
		{
			partName = PartAttributes.className;
		}
	}

	public void SendEvent(string eventName)
	{
		SendEvent(eventName, null, int.MaxValue);
	}

	public void SendEvent(string eventName, BaseEventDetails data)
	{
		SendEvent(eventName, data, int.MaxValue);
	}

	public void SendEvent(string eventName, BaseEventDetails data, int maxDepth)
	{
		List<BaseEvent> list = CreateEventList(eventName.GetHashCode(), maxDepth);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list[i].Invoke(data);
		}
	}

	public List<BaseEvent> CreateEventList(int eventID, int maxDepth)
	{
		List<BaseEvent> list = new List<BaseEvent>();
		CreateEventListRecursive(eventID, NewRequestID(), list, 0, maxDepth);
		return list;
	}

	public void CreateEventListRecursive(int eventID, int requestID, List<BaseEvent> eventList, int depth, int maxDepth)
	{
		if (AlreadyProcessedRequest(requestID))
		{
			return;
		}
		BaseEvent baseEvent = Events[eventID];
		if (baseEvent != null)
		{
			eventList.Add(baseEvent);
		}
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			baseEvent = modules[i].Events[eventID];
			if (baseEvent != null && baseEvent.active)
			{
				eventList.Add(baseEvent);
			}
		}
		if (depth == maxDepth)
		{
			return;
		}
		depth++;
		if (parent != null)
		{
			parent.CreateEventListRecursive(eventID, requestID, eventList, depth, maxDepth);
		}
		int j = 0;
		for (int count2 = children.Count; j < count2; j++)
		{
			children[j].CreateEventListRecursive(eventID, requestID, eventList, depth, maxDepth);
		}
		int k = 0;
		for (int count3 = fuelLookupTargets.Count; k < count3; k++)
		{
			fuelLookupTargets[k].CreateEventListRecursive(eventID, requestID, eventList, depth, maxDepth);
		}
		int l = 0;
		for (int count4 = attachNodes.Count; l < count4; l++)
		{
			AttachNode attachNode = attachNodes[l];
			if (!(attachNode.attachedPart == null))
			{
				attachNode.attachedPart.CreateEventListRecursive(eventID, requestID, eventList, depth, maxDepth);
			}
		}
	}

	public static int NewRequestID()
	{
		if (ResourceRequestID < int.MaxValue)
		{
			ResourceRequestID++;
		}
		else
		{
			ResourceRequestID = 0;
		}
		return ResourceRequestID;
	}

	public bool AlreadyProcessedRequest(int requestID)
	{
		if (lastRequestID == requestID)
		{
			return true;
		}
		lastRequestID = requestID;
		return false;
	}

	public void SetupResources()
	{
		if (_resources == null || !_resources.IsValid)
		{
			_resources = new PartResourceList(this, (partInfo == null || !(partInfo.partPrefab != null)) ? null : partInfo.partPrefab._resources);
		}
	}

	public void SetupSimulationResources()
	{
		if (_resources == null || !_resources.IsValid)
		{
			SetupResources();
		}
		if (_simulationResources == null || !_simulationResources.IsValid)
		{
			_simulationResources = new PartResourceList(this, _resources, simulationSet: true);
		}
	}

	public void ResetSimulationResources()
	{
		_simulationResources.RefreshSimulationListAmounts(_resources);
	}

	public void ResetSimulationResources(PartResourceList sourceList)
	{
		_simulationResources.RefreshSimulationListAmounts(sourceList);
	}

	public void ResetSimulation()
	{
		if (_simulationResources != null && _simulationResources.IsValid)
		{
			ResetSimulationResources();
		}
		else
		{
			SetupSimulationResources();
		}
	}

	public PartResource AddResource(ConfigNode node)
	{
		SetupSimulationResources();
		string value = node.GetValue("name");
		PartResource partResource = _simulationResources.Get(value);
		if (partResource != null)
		{
			partResource.Load(node);
		}
		else
		{
			_simulationResources.Add(node);
		}
		return Resources.Add(node);
	}

	public void SetResource(ConfigNode node)
	{
		string value = node.GetValue("name");
		if (!string.IsNullOrEmpty(value))
		{
			PartResource partResource = Resources.Get(value);
			if (partResource != null)
			{
				partResource.Load(node);
			}
			else
			{
				AddResource(node);
			}
		}
	}

	public bool RemoveResource(PartResource res)
	{
		SetupSimulationResources();
		_simulationResources.Remove(res);
		return Resources.Remove(res);
	}

	public bool RemoveResource(string rName)
	{
		SetupSimulationResources();
		_simulationResources.Remove(rName);
		return Resources.Remove(rName);
	}

	public bool RemoveResource(int resID)
	{
		SetupSimulationResources();
		_simulationResources.Remove(resID);
		return Resources.Remove(resID);
	}

	[Obsolete]
	public virtual float RequestResource(int resourceID, float demand)
	{
		ResourceFlowMode defaultFlowMode = PartResourceLibrary.GetDefaultFlowMode(resourceID);
		if (defaultFlowMode != ResourceFlowMode.NULL)
		{
			return (float)requestResource(this, resourceID, defaultFlowMode, demand);
		}
		return 0f;
	}

	[Obsolete]
	public virtual float RequestResource(string resourceName, float demand)
	{
		ResourceFlowMode defaultFlowMode = PartResourceLibrary.GetDefaultFlowMode(resourceName);
		if (defaultFlowMode != ResourceFlowMode.NULL)
		{
			return (float)requestResource(this, resourceName.GetHashCode(), defaultFlowMode, demand);
		}
		return 0f;
	}

	public virtual double RequestResource(int resourceID, double demand)
	{
		return RequestResource(resourceID, demand, simulate: false);
	}

	public virtual double RequestResource(int resourceID, double demand, bool simulate)
	{
		ResourceFlowMode defaultFlowMode = PartResourceLibrary.GetDefaultFlowMode(resourceID);
		if (defaultFlowMode != ResourceFlowMode.NULL)
		{
			return requestResource(this, resourceID, defaultFlowMode, demand, simulate);
		}
		return 0.0;
	}

	public virtual double RequestResource(string resourceName, double demand)
	{
		return RequestResource(resourceName, demand, simulate: false);
	}

	public virtual double RequestResource(string resourceName, double demand, bool simulate)
	{
		ResourceFlowMode defaultFlowMode = PartResourceLibrary.GetDefaultFlowMode(resourceName);
		if (defaultFlowMode != ResourceFlowMode.NULL)
		{
			return requestResource(this, resourceName.GetHashCode(), defaultFlowMode, demand, simulate);
		}
		return 0.0;
	}

	public virtual double RequestResource(int resourceID, double demand, ResourceFlowMode flowMode)
	{
		return RequestResource(resourceID, demand, flowMode, simulate: false);
	}

	public virtual double RequestResource(int resourceID, double demand, ResourceFlowMode flowMode, bool simulate)
	{
		return requestResource(this, resourceID, flowMode, demand, simulate);
	}

	public virtual double RequestResource(string resourceName, double demand, ResourceFlowMode flowMode)
	{
		return RequestResource(resourceName, demand, flowMode, simulate: false);
	}

	public virtual double RequestResource(string resourceName, double demand, ResourceFlowMode flowMode, bool simulate)
	{
		return requestResource(this, resourceName.GetHashCode(), flowMode, demand, simulate);
	}

	public double requestResource(Part origin, int resourceID, ResourceFlowMode flowMode, double demand)
	{
		return requestResource(origin, resourceID, flowMode, demand, simulate: false);
	}

	public double requestResource(Part origin, int resourceID, ResourceFlowMode flowMode, double demand, bool simulate)
	{
		if (demand == 0.0)
		{
			return 0.0;
		}
		if (CheatOptions.InfiniteElectricity && resourceID == PartResourceLibrary.ElectricityHashcode && demand > 0.0)
		{
			return demand;
		}
		switch (flowMode)
		{
		default:
		{
			PartResource partResource = (simulate ? SimulationResources.GetFlowing(resourceID, demand > 0.0) : Resources.GetFlowing(resourceID, demand > 0.0));
			if (partResource != null)
			{
				return TransferResource(partResource, 0.0 - demand, this, simulate);
			}
			return 0.0;
		}
		case ResourceFlowMode.ALL_VESSEL:
		case ResourceFlowMode.ALL_VESSEL_BALANCE:
			if (vessel == null && ship == null)
			{
				return 0.0;
			}
			if (simulate && ship != null && HighLogic.LoadedSceneIsEditor)
			{
				return ship.RequestResource(this, resourceID, demand, usePriority: false, simulate);
			}
			if (vessel != null)
			{
				return vessel.RequestResource(this, resourceID, demand, usePriority: false, simulate);
			}
			return 0.0;
		case ResourceFlowMode.STAGE_PRIORITY_FLOW:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW_BALANCE:
			if (vessel == null && ship == null)
			{
				return 0.0;
			}
			if (simulate && ship != null && HighLogic.LoadedSceneIsEditor)
			{
				return ship.RequestResource(this, resourceID, demand, usePriority: true, simulate);
			}
			if (vessel != null)
			{
				return vessel.RequestResource(this, resourceID, demand, usePriority: true, simulate);
			}
			return 0.0;
		case ResourceFlowMode.STACK_PRIORITY_SEARCH:
		case ResourceFlowMode.STAGE_STACK_FLOW:
		case ResourceFlowMode.STAGE_STACK_FLOW_BALANCE:
			if (!simulate)
			{
				return crossfeedPartSet.RequestResource(this, resourceID, demand, usePri: true, simulate);
			}
			return simulationCrossfeedPartSet.RequestResource(this, resourceID, demand, usePri: true, simulate);
		}
	}

	public float GetResourceMass()
	{
		return GetResourceMass(simulate: false);
	}

	public float GetResourceMass(bool simulate)
	{
		double num = 0.0;
		int index = (simulate ? SimulationResources.Count : Resources.Count);
		while (index-- > 0)
		{
			PartResource partResource = (simulate ? SimulationResources[index] : Resources[index]);
			num += partResource.amount * (double)partResource.info.density;
		}
		return (float)num;
	}

	public float GetResourceMass(out float thermalMass)
	{
		float num = 0f;
		thermalMass = 0f;
		int count = Resources.Count;
		while (count-- > 0)
		{
			PartResource partResource = Resources[count];
			float num2 = (float)partResource.amount * partResource.info.density;
			num += num2;
			thermalMass += num2 * partResource.info.specificHeatCapacity;
		}
		return num;
	}

	public float GetResourceMass(out double thermalMass)
	{
		float num = 0f;
		thermalMass = 0.0;
		int count = Resources.Count;
		while (count-- > 0)
		{
			PartResource partResource = Resources[count];
			float num2 = (float)partResource.amount * partResource.info.density;
			num += num2;
			thermalMass += num2 * partResource.info.specificHeatCapacity;
		}
		return num;
	}

	public virtual double TransferResource(int resourceID, double amount)
	{
		PartResource partResource = Resources.Get(resourceID);
		if (partResource == null)
		{
			Debug.LogError("Resource System Error: Resource id " + resourceID + " does not exist in part", base.gameObject);
			return 0.0;
		}
		return TransferResource(partResource, amount, this);
	}

	public virtual double TransferResource(PartResource resource, double amount, Part other)
	{
		return TransferResource(resource, amount, other, simulate: false);
	}

	public virtual double TransferResource(PartResource resource, double amount, Part other, bool simulate)
	{
		PartResource.FlowMode flowMode = resource.flowMode;
		if (flowMode != 0 && amount != 0.0)
		{
			bool flag;
			if (flag = amount > 0.0)
			{
				if (flowMode == PartResource.FlowMode.Out)
				{
					return 0.0;
				}
			}
			else if (flowMode == PartResource.FlowMode.In)
			{
				return 0.0;
			}
			double amount2 = resource.amount;
			double num = resource.info.density;
			if (flag)
			{
				double num2 = resource.maxAmount - resource.amount;
				if (amount >= num2)
				{
					resource.amount = resource.maxAmount;
					if (num > 0.0 && HighLogic.LoadedSceneIsFlight && other != this && !simulate)
					{
						double num3 = num2 * num * (double)resource.info.specificHeatCapacity;
						temperature = (temperature * thermalMass + num3 * other.temperature) / (thermalMass + num3);
					}
					if (amount2 > 0.0)
					{
						if (num2 > 0.0 && !simulate)
						{
							GameEvents.onPartResourceNonemptyFull.Fire(resource);
						}
					}
					else if (!simulate)
					{
						GameEvents.onPartResourceEmptyFull.Fire(resource);
					}
					return 0.0 - num2;
				}
				resource.amount += amount;
				if (num > 0.0 && HighLogic.LoadedSceneIsFlight && other != this && !simulate)
				{
					double num4 = amount * num * (double)resource.info.specificHeatCapacity;
					temperature = (temperature * thermalMass + num4 * other.temperature) / (thermalMass + num4);
				}
				if (amount2 == 0.0 && !simulate)
				{
					GameEvents.onPartResourceEmptyNonempty.Fire(resource);
				}
				return 0.0 - amount;
			}
			amount = 0.0 - amount;
			if (amount >= resource.amount)
			{
				resource.amount = 0.0;
				if (num > 0.0 && HighLogic.LoadedSceneIsFlight && other != this && !simulate)
				{
					double num5 = amount2 * num * (double)resource.info.specificHeatCapacity;
					other.temperature = (other.temperature * other.thermalMass + num5 * temperature) / (other.thermalMass + num5);
				}
				if (amount2 < resource.maxAmount)
				{
					if (amount2 > 0.0 && !simulate)
					{
						GameEvents.onPartResourceNonemptyEmpty.Fire(resource);
					}
				}
				else if (!simulate)
				{
					GameEvents.onPartResourceFullEmpty.Fire(resource);
				}
				return amount2;
			}
			resource.amount -= amount;
			if (num > 0.0 && HighLogic.LoadedSceneIsFlight && other != this && !simulate)
			{
				double num6 = amount * num * (double)resource.info.specificHeatCapacity;
				other.temperature = (other.temperature * other.thermalMass + num6 * temperature) / (other.thermalMass + num6);
			}
			if (amount2 == resource.maxAmount && !simulate)
			{
				GameEvents.onPartResourceFullNonempty.Fire(resource);
			}
			return amount;
		}
		return 0.0;
	}

	public void GetConnectedResourceTotals(int resourceID, out double amount, out double maxAmount, bool pulling = true)
	{
		GetConnectedResourceTotals(resourceID, PartResourceLibrary.Instance.GetDefinition(resourceID).resourceFlowMode, out amount, out maxAmount, pulling);
	}

	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, out double amount, out double maxAmount, bool pulling = true)
	{
		GetConnectedResourceTotals(resourceID, flowMode, simulate: false, out amount, out maxAmount, pulling);
	}

	public void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		switch (flowMode)
		{
		default:
			if (simulate)
			{
				SimulationResources.GetFlowingTotals(resourceID, out amount, out maxAmount, pulling);
			}
			else
			{
				Resources.GetFlowingTotals(resourceID, out amount, out maxAmount, pulling);
			}
			break;
		case ResourceFlowMode.ALL_VESSEL:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW:
		case ResourceFlowMode.ALL_VESSEL_BALANCE:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW_BALANCE:
			if (ship != null && simulate && HighLogic.LoadedSceneIsEditor)
			{
				ship.GetConnectedResourceTotals(resourceID, simulate: true, out amount, out maxAmount, pulling);
				break;
			}
			if (vessel != null)
			{
				vessel.GetConnectedResourceTotals(resourceID, simulate, out amount, out maxAmount, pulling);
				break;
			}
			amount = 0.0;
			maxAmount = 0.0;
			break;
		case ResourceFlowMode.STACK_PRIORITY_SEARCH:
		case ResourceFlowMode.STAGE_STACK_FLOW:
		case ResourceFlowMode.STAGE_STACK_FLOW_BALANCE:
			if (simulate)
			{
				simulationCrossfeedPartSet.GetConnectedResourceTotals(resourceID, out amount, out maxAmount, pulling, simulate);
			}
			else
			{
				crossfeedPartSet.GetConnectedResourceTotals(resourceID, out amount, out maxAmount, pulling, simulate);
			}
			break;
		}
	}

	public void GetConnectedResourceTotals(int resourceID, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		GetConnectedResourceTotals(resourceID, PartResourceLibrary.Instance.GetDefinition(resourceID).resourceFlowMode, out amount, out maxAmount, threshold, pulling);
	}

	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		GetConnectedResourceTotals(resourceID, flowMode, simulate: false, out amount, out maxAmount, threshold, pulling);
	}

	public virtual void GetConnectedResourceTotals(int resourceID, ResourceFlowMode flowMode, bool simulate, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		switch (flowMode)
		{
		case ResourceFlowMode.ALL_VESSEL:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW:
		case ResourceFlowMode.ALL_VESSEL_BALANCE:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW_BALANCE:
			vessel.GetConnectedResourceTotals(resourceID, simulate, out amount, out maxAmount, threshold, pulling);
			return;
		case ResourceFlowMode.STACK_PRIORITY_SEARCH:
		case ResourceFlowMode.STAGE_STACK_FLOW:
		case ResourceFlowMode.STAGE_STACK_FLOW_BALANCE:
			if (simulate)
			{
				simulationCrossfeedPartSet.GetConnectedResourceTotals(resourceID, out amount, out maxAmount, threshold, pulling);
			}
			else
			{
				crossfeedPartSet.GetConnectedResourceTotals(resourceID, out amount, out maxAmount, threshold, pulling);
			}
			return;
		}
		if (simulate)
		{
			SimulationResources.GetFlowingTotals(resourceID, out amount, out maxAmount, pulling);
		}
		else
		{
			Resources.GetFlowingTotals(resourceID, out amount, out maxAmount, pulling);
		}
		if (pulling)
		{
			double num = maxAmount * threshold;
			amount -= num;
			if (amount < 1E-09)
			{
				amount = 0.0;
			}
			maxAmount *= 1.0 - threshold;
			if (maxAmount < 1E-09)
			{
				maxAmount = 0.0;
			}
		}
		else
		{
			maxAmount *= threshold;
			amount = maxAmount - amount;
			if (amount < 1E-09)
			{
				amount = 0.0;
			}
			if (maxAmount < 1E-09)
			{
				maxAmount = 0.0;
			}
		}
	}

	public virtual bool CanCrossfeed(Part target, string resName, ResourceFlowMode flow = ResourceFlowMode.NULL)
	{
		return CanCrossfeed(target, resName.GetHashCode(), flow);
	}

	public virtual bool CanCrossfeed(Part target, int resourceID, ResourceFlowMode flow = ResourceFlowMode.NULL)
	{
		if (flow == ResourceFlowMode.NULL)
		{
			flow = PartResourceLibrary.Instance.GetDefinition(resourceID).resourceFlowMode;
		}
		switch (flow)
		{
		default:
			return target == this;
		case ResourceFlowMode.ALL_VESSEL:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW:
		case ResourceFlowMode.ALL_VESSEL_BALANCE:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW_BALANCE:
			return target.vessel == vessel;
		case ResourceFlowMode.STACK_PRIORITY_SEARCH:
		case ResourceFlowMode.STAGE_STACK_FLOW:
		case ResourceFlowMode.STAGE_STACK_FLOW_BALANCE:
			return crossfeedPartSet.ContainsPart(target);
		}
	}

	public static Component GetComponentUpwards(string type, GameObject obj)
	{
		Component component = obj.GetComponent(type);
		if (component != null)
		{
			return component;
		}
		if (obj.transform.parent != null)
		{
			return GetComponentUpwards(type, obj.transform.parent.gameObject);
		}
		return null;
	}

	public static T GetComponentUpwards<T>(GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		if (obj.transform.parent != null)
		{
			return GetComponentUpwards<T>(obj.transform.parent.gameObject);
		}
		return null;
	}

	public static Part FromGO(GameObject obj)
	{
		return obj.GetComponent<Part>();
	}

	public static Quaternion PartToVesselSpaceRot(Quaternion rot, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => p.orgRot * rot, 
			_ => Quaternion.Inverse(v.transform.rotation) * p.partTransform.rotation * rot, 
		};
	}

	public static Vector3 PartToVesselSpacePos(Vector3 pos, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => p.orgPos + p.orgRot * pos, 
			_ => v.transform.InverseTransformPoint(p.partTransform.TransformPoint(pos)), 
		};
	}

	public static Vector3 PartToVesselSpaceDir(Vector3 dir, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => p.orgRot * dir, 
			_ => v.transform.InverseTransformDirection(p.partTransform.TransformDirection(dir)), 
		};
	}

	public static Quaternion VesselToPartSpaceRot(Quaternion rot, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => Quaternion.Inverse(p.orgRot) * rot, 
			_ => Quaternion.Inverse(p.partTransform.rotation) * v.transform.rotation * rot, 
		};
	}

	public static Vector3 VesselToPartSpacePos(Vector3 pos, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => Quaternion.Inverse(p.orgRot) * (pos - p.orgPos), 
			_ => p.partTransform.InverseTransformPoint(v.transform.TransformPoint(pos)), 
		};
	}

	public static Vector3 VesselToPartSpaceDir(Vector3 dir, Part p, Vessel v, PartSpaceMode space)
	{
		return space switch
		{
			PartSpaceMode.Pristine => Quaternion.Inverse(p.orgRot) * dir, 
			_ => p.partTransform.InverseTransformDirection(v.transform.TransformDirection(dir)), 
		};
	}

	public static void GetPartsOutTo(Part part, HashSet<Part> parts, int maxLinks)
	{
		if (parts.Contains(part))
		{
			return;
		}
		parts.Add(part);
		if (maxLinks <= 0)
		{
			return;
		}
		maxLinks--;
		int count = part.children.Count;
		while (count-- > 0)
		{
			Part part2 = part.children[count];
			if (part2 != null && part2 != part.parent)
			{
				GetPartsOutTo(part2, parts, maxLinks);
			}
		}
		if (part.parent != null)
		{
			GetPartsOutTo(part.parent, parts, maxLinks);
		}
	}

	public void updateMirroring()
	{
		if (mirrorRefAxis == Vector3.zero)
		{
			return;
		}
		Debug.DrawRay(partTransform.position, partTransform.rotation * mirrorAxis, Color.blue);
		Debug.DrawRay(partTransform.position, EditorLogic.VesselRotation * mirrorRefAxis, Color.cyan);
		if (Vector3.Dot(partTransform.rotation * mirrorAxis, EditorLogic.VesselRotation * mirrorRefAxis) < 0f)
		{
			if (mirrorRefAxis.x != 0f)
			{
				SetMirror(Vector3.Scale(mirrorVector, new Vector3(-1f, 1f, 1f)));
			}
			if (mirrorRefAxis.y != 0f)
			{
				SetMirror(Vector3.Scale(mirrorVector, new Vector3(1f, -1f, 1f)));
			}
			if (mirrorRefAxis.z != 0f)
			{
				SetMirror(Vector3.Scale(mirrorVector, new Vector3(1f, 1f, -1f)));
			}
		}
	}

	public void SetMirror(Vector3 mirrorVector)
	{
		this.mirrorVector = mirrorVector;
		isMirrored = mirrorVector != Vector3.one;
		mirrorAxis = Vector3.Scale(mirrorRefAxis, mirrorVector);
		Transform transform = partTransform.Find("model");
		if (transform != null)
		{
			transform.localScale = mirrorVector * rescaleFactor;
		}
		int i = 0;
		for (int count = attachNodes.Count; i < count; i++)
		{
			AttachNode attachNode = attachNodes[i];
			attachNode.position = Vector3.Scale(attachNode.originalPosition, mirrorVector);
			attachNode.orientation = Vector3.Scale(attachNode.originalOrientation, mirrorVector);
		}
	}

	public void SetupHighlighter()
	{
		if (!(hl != null) && state != PartStates.DEAD)
		{
			hl = GetComponent<Highlighter>();
			if (hl == null)
			{
				hl = base.gameObject.AddComponent<Highlighter>();
			}
		}
	}

	public void RefreshHighlighter()
	{
		if (hl != null)
		{
			hl.ReinitMaterials();
		}
	}

	public void SetHighlightType(HighlightType type)
	{
		highlightType = type;
		switch (type)
		{
		case HighlightType.Disabled:
			SetHighlightColor(defaultHighlightNone);
			break;
		case HighlightType.OnMouseOver:
			SetHighlight(mouseOver, recurseHighlight);
			break;
		case HighlightType.AlwaysOn:
			SetHighlightColor(highlightColor);
			break;
		}
	}

	public void SetHighlightColor(Color color)
	{
		highlightColor = color;
		switch (highlightType)
		{
		case HighlightType.AlwaysOn:
			SetHighlight(active: true, recurseHighlight);
			break;
		case HighlightType.OnMouseOver:
			SetHighlight(mouseOver, recurseHighlight);
			break;
		}
	}

	public void SetHighlightColor()
	{
		highlightColor = defaultHighlightPart;
	}

	public void SetHighlightDefault()
	{
		highlightColor = defaultHighlightPart;
		currentHighlightColor = defaultHighlightPart;
		highlightType = HighlightType.OnMouseOver;
		mouseOver = false;
		SetHighlight(mouseOver, recursive: false);
	}

	public void SetHighlight(bool active, bool recursive)
	{
		if (!recursive && (highlightActive == active || !recurseHighlight))
		{
			Highlight(active);
		}
		else
		{
			HighlightRecursive(active);
		}
		highlightActive = active;
		recurseHighlight = recursive && active;
		if (stackIcon != null && stackIcon.Highlighted != active)
		{
			stackIcon.Highlight(active);
		}
	}

	public void Highlight(bool active)
	{
		Highlight(active ? highlightColor : defaultHighlightNone);
	}

	public void Highlight(Color highlightColor)
	{
		if (currentHighlightColor == highlightColor)
		{
			return;
		}
		CreateRendererLists();
		currentHighlightColor = highlightColor;
		Color value = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * Highlighter.HighlighterLimit);
		if (!isVesselEVA)
		{
			mpb.SetColor(PropertyIDs._RimColor, value);
		}
		int count = highlightRenderer.Count;
		while (count-- > 0)
		{
			if (highlightRenderer[count] == null)
			{
				highlightRenderer.RemoveAt(count);
			}
			else
			{
				highlightRenderer[count].SetPropertyBlock(mpb);
			}
		}
		if (highlightColor == defaultHighlightNone)
		{
			hl.Off();
			if (temperatureRenderer != null)
			{
				Color blackBodyRadiation = PhysicsGlobals.GetBlackBodyRadiation((float)skinTemperature, this);
				temperatureRenderer.Update(blackBodyRadiation, force: true);
			}
		}
		else
		{
			hl.ConstantOn(highlightColor);
		}
	}

	public void CreateRendererLists()
	{
		if (rendererlistscreated)
		{
			return;
		}
		if (!isVesselEVA)
		{
			mpb.SetFloat(PropertyIDs._RimFalloff, 2f);
			mpb.SetColor(PropertyIDs._RimColor, defaultHighlightNone);
		}
		if (hasHeiarchyModel)
		{
			highlightRenderer = FindModelRenderersCached();
			highlightRenderer.RemoveNonHighlightableRenderers();
		}
		else
		{
			Transform transform = partTransform.Find("model");
			if (transform != null)
			{
				highlightRenderer = new List<Renderer>(transform.GetComponentsInChildren<MeshRenderer>());
				highlightRenderer.RemoveNonHighlightableRenderers();
			}
			else
			{
				highlightRenderer = new List<Renderer>(GetComponentsInChildren<Renderer>());
				highlightRenderer.RemoveNonHighlightableRenderers();
			}
		}
		rendererlistscreated = true;
	}

	public void HighlightRecursive(bool active)
	{
		Highlight(active);
		if (stackIcon != null && stackIcon.Highlighted)
		{
			stackIcon.Highlight(highlightState: false);
		}
		highlightActive = active;
		if (children.Count > 0)
		{
			int count = children.Count;
			while (count-- > 0)
			{
				children[count].HighlightRecursive(active);
				children[count].mouseOver = false;
			}
		}
	}

	public void HighlightRecursive(Color highlightColor)
	{
		Highlight(highlightColor);
		if (children.Count > 0)
		{
			int count = children.Count;
			while (count-- > 0)
			{
				children[count].HighlightRecursive(highlightColor);
				children[count].mouseOver = false;
			}
		}
	}

	public void VesselModified(Vessel v)
	{
		if (v == vessel)
		{
			RefreshHighlighter();
			rendererlistscreated = false;
			if (highlightRenderer != null)
			{
				highlightRenderer.Clear();
			}
			CreateRendererLists();
			CycleAutoStrut();
			CheckBodyLiftAttachment();
		}
		else if (v == autoStrutVessel)
		{
			CycleAutoStrut();
		}
	}

	public void partHideUI()
	{
		highlightBlocked = true;
		if (mouseOver && highlightType == HighlightType.OnMouseOver)
		{
			SetHighlight(active: false, HighLogic.LoadedSceneIsEditor);
		}
	}

	public void partShowUI()
	{
		highlightBlocked = false;
		if (mouseOver && (!HighLogic.LoadedSceneIsFlight || GameSettings.INFLIGHT_HIGHLIGHT) && highlightType == HighlightType.OnMouseOver)
		{
			SetHighlight(active: true, HighLogic.LoadedSceneIsEditor);
		}
	}

	public void OnCurrentMousePartChanged(Part p)
	{
		if (p == null)
		{
			highlightBlocked = false;
		}
		else
		{
			highlightBlocked = true;
		}
		if (!HighLogic.LoadedSceneIsFlight || !(EVAConstructionModeController.Instance != null) || !EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != 0 || physicalSignificance != PhysicalSignificance.NONE)
		{
			return;
		}
		if (p != null)
		{
			if (rb == null)
			{
				if (hasKerbalOnLadder)
				{
					return;
				}
				rb = base.gameObject.AddComponent<Rigidbody>();
				if (vessel != null)
				{
					CollisionManager.SetCollidersOnVessel(vessel, ignore: true, GetPartColliders());
				}
			}
			rb.isKinematic = true;
			rb.useGravity = false;
			rb.mass = 1E-06f;
		}
		else if (rb != null)
		{
			UnityEngine.Object.Destroy(rb);
			if (vessel != null)
			{
				CollisionManager.SetCollidersOnVessel(vessel, ignore: false, GetPartColliders());
			}
		}
	}

	public void OnMouseIsOver()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			return;
		}
		if (!highlightBlocked && (!HighLogic.LoadedSceneIsFlight || GameSettings.INFLIGHT_HIGHLIGHT) && highlightType == HighlightType.OnMouseOver)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				StartCoroutine(HandleMouseOver());
			}
			else
			{
				SetHighlight(active: true, HighLogic.LoadedSceneIsEditor);
			}
		}
		if (onMouseEnter != null)
		{
			onMouseEnter(this);
		}
	}

	public IEnumerator HandleMouseOver()
	{
		yield return new WaitForEndOfFrame();
		SetHighlight(active: true, HighLogic.LoadedSceneIsEditor);
	}

	public void OnMouseHasExited()
	{
		if (!highlightBlocked && highlightType == HighlightType.OnMouseOver)
		{
			SetHighlight(active: false, HighLogic.LoadedSceneIsEditor);
		}
		if (onMouseExit != null)
		{
			onMouseExit(this);
		}
	}

	public void UpdateMouseOver()
	{
		if (HighLogic.LoadedSceneIsFlight && (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3 || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal))
		{
			return;
		}
		mouseEntered = this == Mouse.HoveredPart;
		if ((bool)EditorActionGroups.Instance && EditorActionGroups.Instance.isMouseOver)
		{
			mouseEntered = false;
		}
		if (mouseEntered)
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				if (mouseOver)
				{
					mouseOver = false;
					OnMouseHasExited();
				}
			}
			else if (!mouseOver)
			{
				mouseOver = true;
				OnMouseIsOver();
			}
			if (Mouse.Left.GetDoubleClick() && HighLogic.LoadedSceneIsFlight)
			{
				ITargetable targetable = FindModuleImplementing<ITargetable>();
				if (targetable == null)
				{
					targetable = vessel;
				}
				if (!EventSystem.current.IsPointerOverGameObject())
				{
					FlightGlobals.fetch.SetVesselTarget(targetable);
				}
			}
		}
		else if (mouseOver)
		{
			mouseOver = false;
			OnMouseHasExited();
		}
	}

	public void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && onMouseDown != null)
		{
			onMouseDown(this);
		}
	}

	public void AddOnMouseEnter(OnActionDelegate method)
	{
		if (onMouseEnter == null)
		{
			onMouseEnter = method;
		}
		else
		{
			onMouseEnter = (OnActionDelegate)Delegate.Combine(onMouseEnter, method);
		}
	}

	public void RemoveOnMouseEnter(OnActionDelegate method)
	{
		if (onMouseEnter == method)
		{
			onMouseEnter = null;
		}
		else
		{
			onMouseEnter = (OnActionDelegate)Delegate.Remove(onMouseEnter, method);
		}
	}

	public void AddOnMouseExit(OnActionDelegate method)
	{
		if (onMouseExit == null)
		{
			onMouseExit = method;
		}
		else
		{
			onMouseExit = (OnActionDelegate)Delegate.Combine(onMouseExit, method);
		}
	}

	public void RemoveOnMouseExit(OnActionDelegate method)
	{
		if (onMouseExit == method)
		{
			onMouseExit = null;
		}
		else
		{
			onMouseExit = (OnActionDelegate)Delegate.Remove(onMouseExit, method);
		}
	}

	public void AddOnMouseDown(OnActionDelegate method)
	{
		if (onMouseDown == null)
		{
			onMouseDown = method;
		}
		else
		{
			onMouseDown = (OnActionDelegate)Delegate.Combine(onMouseDown, method);
		}
	}

	public void RemoveOnMouseDown(OnActionDelegate method)
	{
		if (onMouseDown == method)
		{
			onMouseDown = null;
		}
		else
		{
			onMouseDown = (OnActionDelegate)Delegate.Remove(onMouseDown, method);
		}
	}

	public InternalModel AddInternalPart(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add an InternalPart:ConfigNode contains no InternalPart name");
			return null;
		}
		if (internalModel != null)
		{
			UnityEngine.Object.DestroyImmediate(internalModel.gameObject);
		}
		InternalModel internalPart = PartLoader.GetInternalPart(node.GetValue("name"));
		if (internalPart == null)
		{
			Debug.LogError("Cannot add an InternalPart: Cannot find internal of name '" + node.GetValue("name") + "'");
			return null;
		}
		internalModel = UnityEngine.Object.Instantiate(internalPart);
		internalModel.gameObject.name = internalPart.internalName + " interior";
		internalModel.gameObject.SetActive(value: true);
		if (internalModel == null)
		{
			Debug.LogError("Cannot add an InternalPart: Cannot find instantiate internal of name '" + node.GetValue("name") + "'");
			return null;
		}
		internalModel.transform.parent = partTransform;
		internalModel.transform.localPosition = Vector3.zero;
		internalModel.transform.localRotation = Quaternion.identity;
		internalModel.part = this;
		internalModel.Load(node);
		return internalModel;
	}

	public void CreateInternalModel()
	{
		if (partInfo.internalConfig.HasData)
		{
			AddInternalPart(partInfo.internalConfig);
		}
	}

	public void InternalOnUpdate()
	{
		if (internalModel != null)
		{
			internalModel.OnUpdate();
		}
	}

	public void InternalFixedUpdate()
	{
		if (internalModel != null)
		{
			internalModel.OnFixedUpdate();
		}
	}

	public float GetModuleCosts(float defaultCost, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		float num = 0f;
		int i = 0;
		for (int count = Modules.Count; i < count; i++)
		{
			if (Modules[i] is IPartCostModifier partCostModifier)
			{
				num += partCostModifier.GetModuleCost(defaultCost, sit);
			}
		}
		return num;
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		float num = 0f;
		int i = 0;
		for (int count = Modules.Count; i < count; i++)
		{
			if (Modules[i] is IPartMassModifier partMassModifier)
			{
				num += partMassModifier.GetModuleMass(defaultMass, sit);
			}
		}
		return num;
	}

	public Vector3 GetModuleSize(Vector3 defaultSize, ModifierStagingSituation sit = ModifierStagingSituation.CURRENT)
	{
		prefabSize = defaultSize;
		moduleSize = Vector3.zero;
		int i = 0;
		for (int count = Modules.Count; i < count; i++)
		{
			if (Modules[i] is IPartSizeModifier partSizeModifier)
			{
				moduleSize += partSizeModifier.GetModuleSize(defaultSize, sit);
			}
		}
		return moduleSize;
	}

	public void UpdateAeroDisplay()
	{
		bool flag = false;
		if (PhysicsGlobals.AeroForceDisplay && staticPressureAtm > 0.0 && Rigidbody != null)
		{
			if (rb != null)
			{
				flag = true;
				if (dragArrowPtr == null)
				{
					dragArrowPtr = ArrowPointer.Create(base.transform, CoPOffset, dragVectorDirLocal, dragScalar * PhysicsGlobals.AeroForceDisplayScale, Color.red, worldSpace: false);
				}
				else
				{
					dragArrowPtr.Offset = CoPOffset;
					dragArrowPtr.Direction = dragVectorDirLocal;
					dragArrowPtr.Length = dragScalar * PhysicsGlobals.AeroForceDisplayScale;
				}
				if (bodyLiftArrowPtr == null)
				{
					bodyLiftArrowPtr = ArrowPointer.Create(base.transform, bodyLiftLocalPosition, bodyLiftLocalVector, PhysicsGlobals.AeroForceDisplayScale, Color.cyan, worldSpace: false);
				}
				else
				{
					float magnitude = bodyLiftLocalVector.magnitude;
					Vector3 direction = bodyLiftLocalVector;
					bodyLiftArrowPtr.Direction = direction;
					bodyLiftArrowPtr.Length = magnitude * PhysicsGlobals.AeroForceDisplayScale;
				}
			}
		}
		else
		{
			if (dragArrowPtr != null)
			{
				UnityEngine.Object.Destroy(dragArrowPtr.gameObject);
				dragArrowPtr = null;
			}
			if (bodyLiftArrowPtr != null)
			{
				UnityEngine.Object.Destroy(bodyLiftArrowPtr.gameObject);
				bodyLiftArrowPtr = null;
			}
		}
		if (flag != aeroDisplayWasActive)
		{
			RefreshHighlighter();
		}
		aeroDisplayWasActive = flag;
	}

	public void AddThermalFlux(double kilowatts)
	{
		thermalInternalFlux += kilowatts * (double)TimeWarp.fixedDeltaTime;
	}

	public void AddSkinThermalFlux(double kilowatts)
	{
		thermalSkinFlux += kilowatts * (double)TimeWarp.fixedDeltaTime;
	}

	public void AddExposedThermalFlux(double kilowatts)
	{
		thermalExposedFlux += kilowatts * (double)TimeWarp.fixedDeltaTime;
	}

	public Callback<IAirstreamShield> AddShield(IAirstreamShield shd)
	{
		airstreamShields.AddUnique(shd);
		OnShieldModified(shd);
		return OnShieldModified;
	}

	public void RemoveShield(IAirstreamShield shd)
	{
		airstreamShields.Remove(shd);
		OnShieldModified(shd);
	}

	public void OnShieldModified(IAirstreamShield shd)
	{
		int count = airstreamShields.Count;
		while (count-- > 0)
		{
			IAirstreamShield airstreamShield = airstreamShields[count];
			if (airstreamShield == null || (airstreamShield is PartModule && airstreamShield as PartModule == null))
			{
				Debug.LogWarning("[Part Airstream Shielding]: shield at i:" + count + " was null and was removed.", this);
				airstreamShields.RemoveAt(count);
			}
			if (airstreamShield != null && vessel != null && airstreamShield.GetVessel().persistentId != vessel.persistentId)
			{
				if (GameSettings.VERBOSE_DEBUG_LOG)
				{
					Debug.LogWarning("[Part Airstream Shielding]: shield from " + airstreamShield.GetPart().name + " does not belong to the same vessel as " + base.name + " and was removed.", this);
				}
				airstreamShields.RemoveAt(count);
			}
		}
		partIsShielded = airstreamShields != null && airstreamShields.Count > 0;
		if (!partIsShielded)
		{
			return;
		}
		partIsShielded = false;
		isShieldedDisplay = (partIsShielded ? cacheAutoLOC_439839 : cacheAutoLOC_439840);
		int count2 = airstreamShields.Count;
		do
		{
			if (count2-- <= 0)
			{
				return;
			}
		}
		while (!airstreamShields[count2].ClosedAndLocked());
		partIsShielded = true;
		isShieldedDisplay = (partIsShielded ? cacheAutoLOC_439839 : cacheAutoLOC_439840);
	}

	public IEnumerator RecheckShielding()
	{
		yield return null;
		OnShieldModified(null);
	}

	public void GainCameraAim()
	{
		BaseEvent baseEvent = Events["AimCamera"];
		BaseEvent baseEvent2 = Events["AimCamera"];
		Events["AimCamera"].guiActiveUnfocused = false;
		baseEvent2.guiActiveUncommand = false;
		baseEvent.guiActive = false;
		BaseEvent baseEvent3 = Events["ResetCamera"];
		BaseEvent baseEvent4 = Events["ResetCamera"];
		Events["ResetCamera"].guiActiveUnfocused = true;
		baseEvent4.guiActiveUncommand = true;
		baseEvent3.guiActive = true;
	}

	public void LoseCameraAim()
	{
		Events["AimCamera"].guiActive = (Events["AimCamera"].guiActiveUncommand = (Events["AimCamera"].guiActiveUnfocused = canAimCamera));
		BaseEvent baseEvent = Events["ResetCamera"];
		BaseEvent baseEvent2 = Events["ResetCamera"];
		BaseEvent baseEvent3 = Events["ResetCamera"];
		bool flag = false;
		baseEvent3.guiActiveUnfocused = false;
		flag = false;
		baseEvent2.guiActiveUncommand = false;
		baseEvent.guiActive = false;
	}

	[KSPEvent(advancedTweakable = true, guiActiveUncommand = true, guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = true, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6001315")]
	public void AimCamera()
	{
		if (canAimCamera)
		{
			FlightCamera.SetTarget(this);
		}
	}

	[KSPEvent(advancedTweakable = true, guiActiveUncommand = true, guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = false, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6001316")]
	public void ResetCamera()
	{
		FlightCamera.SetTarget(FlightGlobals.ActiveVessel);
	}

	[KSPEvent(guiActiveUncommand = true, guiActive = true, guiName = "#autoLOC_6001317")]
	public void SpawnTransferDialog()
	{
		if ((bool)CrewHatchController.fetch && protoModuleCrew.Count > 0)
		{
			if (!CrewHatchController.fetch.Active)
			{
				CrewHatchController.fetch.SpawnCrewDialog(this, showEVA: true, showTransfer: true);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_211097, 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public virtual int GetResourcePriority()
	{
		return ((!resourcePriorityUseParentInverseStage || !(parent != null)) ? inverseStage : parent.inverseStage) * 10 + resourcePriorityOffset;
	}

	public void ResetPri()
	{
		resourcePriorityOffset = 0;
		int count = symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (symmetryCounterparts[count] != this)
			{
				symmetryCounterparts[count].resourcePriorityOffset = 0;
			}
		}
		GameEvents.onPartPriorityChanged.Fire(this);
	}

	public void ChangeResourcePriority(int offset)
	{
		resourcePriorityOffset += offset;
		int count = symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (symmetryCounterparts[count] != this)
			{
				symmetryCounterparts[count].resourcePriorityOffset += offset;
			}
		}
		GameEvents.onPartPriorityChanged.Fire(this);
	}

	public void UpdateStageability(bool propagate, bool iconUpdate)
	{
		if (!hasStagingIcon || stagingIconAlwaysShown)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		int count = modules.Count;
		while (count-- > 0)
		{
			PartModule partModule = modules[count];
			if (partModule.IsStageable())
			{
				flag2 = true;
				if (partModule.StagingEnabled())
				{
					flag = true;
					break;
				}
			}
		}
		bool flag3 = false;
		if (flag2)
		{
			if (flag)
			{
				flag3 = !stagingOn;
				stagingOn = true;
				if (flag3 && stackIcon.StageIcon == null)
				{
					if (inverseStage > StageManager.StageCount)
					{
						inverseStage = StageManager.StageCount;
						GameEvents.onPartPriorityChanged.Fire(this);
					}
					if (iconUpdate)
					{
						stackIcon.CreateIcon();
						GameEvents.StageManager.OnPartUpdateStageability.Fire(this);
					}
				}
			}
			else
			{
				flag3 = stagingOn;
				stagingOn = false;
				if (stackIcon.StageIcon != null && iconUpdate)
				{
					stackIcon.RemoveIcon();
					GameEvents.StageManager.OnPartUpdateStageability.Fire(this);
				}
			}
			if (!(flag3 && propagate))
			{
				return;
			}
			int num = symmetryCounterparts.Count - 1;
			if (num < 0)
			{
				return;
			}
			for (int num2 = num; num2 >= 0; num2--)
			{
				Part part = symmetryCounterparts[num2];
				if (part != this && part.inverseStage == inverseStage)
				{
					int count2 = part.modules.Count;
					while (count2-- > 0)
					{
						part.modules[count2].SetStaging(stagingOn);
					}
					part.UpdateStageability(propagate: false, iconUpdate);
				}
			}
		}
		else if (stagingOn)
		{
			stagingOn = false;
			if (iconUpdate && stackIcon.StageIcon != null)
			{
				stackIcon.RemoveIcon();
				GameEvents.StageManager.OnPartUpdateStageability.Fire(this);
			}
		}
	}

	[KSPEvent(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6002279")]
	public virtual void ShowUpgradeStats()
	{
		if (UpgradeStatsDel != null)
		{
			UpgradeStatsDel(this);
			return;
		}
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		bool flag = true;
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			PartModule partModule = modules[i];
			if (partModule.upgradesApplied.Count != 0)
			{
				flag = false;
				stringBuilder.Append("<b>");
				string moduleDisplayName = partModule.GetModuleDisplayName();
				if (string.IsNullOrEmpty(moduleDisplayName))
				{
					stringBuilder.Append((partModule is IModuleInfo) ? (partModule as IModuleInfo).GetModuleTitle() : KSPUtil.PrintModuleName(partModule.moduleName));
				}
				else
				{
					stringBuilder.Append(moduleDisplayName);
				}
				stringBuilder.Append("</b>\n");
				stringBuilder.Append(partModule.GetUpgradeInfo());
				stringBuilder.Append("\n");
			}
		}
		string message = stringBuilder.ToStringAndRelease();
		if (flag)
		{
			message = cacheAutoLOC_211269;
		}
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Upgrade Stats", cacheAutoLOC_211272, message, cacheAutoLOC_211274, persistAcrossScenes: false, HighLogic.UISkin, isModal: true, craftID.ToString());
	}

	public bool AllowAutoStruts()
	{
		if (HighLogic.LoadedSceneIsFlight && !autoStrutEnableOptionFlight)
		{
			return false;
		}
		if (HighLogic.LoadedSceneIsEditor && !autoStrutEnableOptionEditor)
		{
			return false;
		}
		if (physicalSignificance != 0)
		{
			return false;
		}
		if ((HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX) && !string.IsNullOrEmpty(PhysicsGlobals.AutoStrutTechRequired) && ResearchAndDevelopment.GetTechnologyState(PhysicsGlobals.AutoStrutTechRequired) != RDTech.State.Available)
		{
			return false;
		}
		return true;
	}

	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001318")]
	public void ToggleAutoStrut()
	{
		AutoStrutMode autoStrutMode = this.autoStrutMode;
		switch (this.autoStrutMode)
		{
		case AutoStrutMode.Off:
			this.autoStrutMode = AutoStrutMode.Heaviest;
			break;
		case AutoStrutMode.Root:
			this.autoStrutMode = AutoStrutMode.Grandparent;
			break;
		case AutoStrutMode.Heaviest:
			this.autoStrutMode = AutoStrutMode.Root;
			break;
		case AutoStrutMode.Grandparent:
			this.autoStrutMode = AutoStrutMode.Off;
			break;
		}
		if (this.autoStrutMode == autoStrutMode)
		{
			return;
		}
		if (GameSettings.AUTOSTRUT_SYMMETRY)
		{
			int count = symmetryCounterparts.Count;
			while (count-- > 0)
			{
				Part part = symmetryCounterparts[count];
				part.autoStrutMode = this.autoStrutMode;
				part.UpdateAutoStrut();
				if (HighLogic.LoadedSceneIsEditor)
				{
					part.autoStrutQuickViz = true;
					part.autoStrutQuickVizStart = Time.time;
				}
			}
		}
		UpdateAutoStrut();
		if (HighLogic.LoadedSceneIsEditor)
		{
			autoStrutQuickViz = true;
			autoStrutQuickVizStart = Time.time;
		}
	}

	public void UpdateAutoStrut()
	{
		if (isVesselEVA)
		{
			autoStrutMode = AutoStrutMode.Off;
			BaseEvent baseEvent = Events["ToggleAutoStrut"];
			Events["ToggleAutoStrut"].guiActiveEditor = false;
			baseEvent.guiActive = false;
		}
		else if (autoStrutMode != 0)
		{
			CycleAutoStrut();
		}
		else
		{
			ReleaseAutoStruts();
			SetAutoStrutText();
		}
	}

	public void SetAutoStrutText()
	{
		switch (autoStrutMode)
		{
		case AutoStrutMode.Off:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001318";
			break;
		case AutoStrutMode.Root:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001320";
			break;
		case AutoStrutMode.Heaviest:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001319";
			break;
		case AutoStrutMode.Grandparent:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001321";
			break;
		case AutoStrutMode.ForceRoot:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001323";
			break;
		case AutoStrutMode.ForceHeaviest:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001322";
			break;
		case AutoStrutMode.ForceGrandparent:
			Events["ToggleAutoStrut"].guiName = "#autoLOC_6001324";
			break;
		}
		if (autoStrutJoints == null && autoStrutMode != 0 && HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			Events["ToggleAutoStrut"].guiName += "#autoLOC_6001328";
		}
	}

	public void OnPartEvent(Part p)
	{
		if (p != null && (p.vessel == vessel || p.vessel == autoStrutVessel))
		{
			CycleAutoStrut();
		}
	}

	public void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> action)
	{
		if (action.from != null && (action.from.vessel == vessel || action.from.vessel == autoStrutVessel))
		{
			CycleAutoStrut();
		}
		else if (action.to != null && (action.to.vessel == vessel || action.to.vessel == autoStrutVessel))
		{
			CycleAutoStrut();
		}
	}

	public void CycleAutoStrut()
	{
		if (autoStrutMode != 0 && !autoStrutCycling && (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor))
		{
			autoStrutCycling = true;
			ReleaseAutoStruts();
			if (base.isActiveAndEnabled && cyclingAutoStruts == null)
			{
				cyclingAutoStruts = StartCoroutine(SecureAutoStruts());
			}
		}
	}

	public bool HasAutoStrutDefined()
	{
		if (autoStrutJoints != null)
		{
			return autoStrutJoints.Count > 0;
		}
		return false;
	}

	public IEnumerator SecureAutoStruts()
	{
		yield return new WaitForFixedUpdate();
		AttachNode nodeToParent = null;
		AttachNode nodeFromParent = null;
		bool srfAttached = false;
		Part autoStrutAnchor = GetAutoStrutAnchor(out nodeToParent, out nodeFromParent, out srfAttached);
		if ((!(EVAConstructionModeController.Instance != null) || !(EVAConstructionModeController.Instance.evaEditor != null) || (EVAConstructionModeController.Instance.evaEditor.EVAConstructionMode != ConstructionMode.Move && EVAConstructionModeController.Instance.evaEditor.EVAConstructionMode != ConstructionMode.Rotate) || !(autoStrutAnchor != null) || !(EVAConstructionModeController.Instance.evaEditor.SelectedPart != null) || autoStrutAnchor.persistentId != EVAConstructionModeController.Instance.evaEditor.SelectedPart.persistentId) && autoStrutAnchor != null && autoStrutAnchor != this)
		{
			autoStrutVessel = autoStrutAnchor.vessel;
			autoStrutJoints.Add(SecureAutoStrut(autoStrutAnchor, nodeToParent, nodeFromParent, srfAttached));
			if (!isRobotic() && !autoStrutAnchor.isRobotic() && !autoStrutAnchor.symmetryCounterparts.Contains(this))
			{
				int count = autoStrutAnchor.symmetryCounterparts.Count;
				while (count-- > 0)
				{
					Part lastPart = null;
					if (!HasFreePivotBetweenParentPart(autoStrutAnchor.symmetryCounterparts[count], out lastPart, out nodeToParent, out nodeFromParent, out srfAttached))
					{
						if (symmetryCounterparts.Count >= count && autoStrutAnchor.symmetryCounterparts[count].isRobotic())
						{
							nodeToParent = AttachedPartNode(symmetryCounterparts[count], autoStrutAnchor.symmetryCounterparts[count], out srfAttached);
						}
						autoStrutJoints.Add(SecureAutoStrut(autoStrutAnchor.symmetryCounterparts[count], nodeToParent, nodeFromParent, srfAttached));
					}
				}
			}
		}
		SetAutoStrutText();
		autoStrutCycling = false;
		cyclingAutoStruts = null;
	}

	public PartJoint SecureAutoStrut(Part anchor, AttachNode nodeToParent, AttachNode nodeFromParent, bool srfAttached)
	{
		AttachNode attachNode = new AttachNode();
		attachNode.id = "AutoStrut";
		attachNode.attachedPart = anchor;
		attachNode.nodeType = AttachNode.NodeType.Stack;
		if (nodeToParent != null)
		{
			if (srfAttached)
			{
				attachNode.nodeType = AttachNode.NodeType.Surface;
				attachNode.srfAttachMeshName = nodeToParent.srfAttachMeshName;
			}
			else
			{
				attachNode.id = nodeToParent.id;
			}
		}
		attachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
		attachNode.breakingForce = float.MaxValue;
		attachNode.breakingTorque = float.MaxValue;
		attachNode.position = strutOffset;
		attachNode.orientation = anchor.partTransform.InverseTransformDirection((partTransform.position - anchor.partTransform.position).normalized);
		attachNode.ResourceXFeed = false;
		attachNode.overrideDragArea = 0f;
		attachNode.owner = this;
		attachNode.size = 1;
		return PartJoint.Create(this, anchor, attachNode, nodeFromParent, AttachModes.SRF_ATTACH);
	}

	public void DrawAutoStrutLine()
	{
		if (autoStrutJoints == null || autoStrutJoints.Count == 0 || UIMasterController.Instance == null || UIMasterController.Instance.mainCanvas == null)
		{
			return;
		}
		bool flag = true;
		if ((autoStrutQuickViz || PhysicsGlobals.AutoStrutDisplay) && !MapView.MapIsEnabled)
		{
			if (autoStrutLines == null)
			{
				autoStrutLines = new List<VectorLine>();
			}
			while (autoStrutLines.Count < autoStrutJoints.Count)
			{
				List<Vector2> points = new List<Vector2>
				{
					Vector3.zero,
					Vector3.zero
				};
				VectorLine vectorLine = new VectorLine("FPSLine", points, 2f);
				vectorLine.lineType = LineType.Continuous;
				vectorLine.rectTransform.SetParent(UIMasterController.Instance.mainCanvas.transform, worldPositionStays: false);
				autoStrutLines.Add(vectorLine);
			}
			int count = autoStrutJoints.Count;
			while (count-- > 0)
			{
				PartJoint partJoint = autoStrutJoints[count];
				VectorLine vectorLine2 = autoStrutLines[count];
				Camera camera = ((!HighLogic.LoadedSceneIsFlight) ? ((EditorCamera.Instance != null) ? EditorCamera.Instance.cam : null) : ((FlightCamera.fetch != null) ? FlightCamera.fetch.mainCamera : null));
				if (camera == null)
				{
					return;
				}
				if (!(Vector3.Dot(camera.transform.forward, partTransform.position + partTransform.TransformDirection(strutOffset) - camera.transform.position) >= 0f) || !(Vector3.Dot(camera.transform.forward, ((partJoint.Joint != null) ? partJoint.Joint.connectedBody.transform.position : partJoint.Parent.partTransform.position) - camera.transform.position) >= 0f))
				{
					continue;
				}
				Color32 color = XKCDColors.Orange;
				Vector3 vector = partTransform.position + partTransform.TransformDirection(strutOffset);
				Vector3 vector2 = ((partJoint.Joint != null) ? partJoint.Joint.connectedBody.transform.position : partJoint.Parent.partTransform.position);
				if (autoStrutQuickViz)
				{
					if (Time.time < autoStrutQuickVizStart + 0.2f)
					{
						float t = Mathf.InverseLerp(autoStrutQuickVizStart, autoStrutQuickVizStart + 0.2f, Time.time);
						vector2 = Vector3.Lerp(vector, vector2, t);
					}
					else if (PhysicsGlobals.AutoStrutDisplay || !(Time.time < autoStrutQuickVizStart + 0.2f + 0.3f))
					{
						if (!PhysicsGlobals.AutoStrutDisplay && Time.time < autoStrutQuickVizStart + 0.2f + 0.3f + 0.5f)
						{
							float num = Mathf.InverseLerp(autoStrutQuickVizStart + 0.2f + 0.3f, autoStrutQuickVizStart + 0.2f + 0.3f + 0.5f, Time.time);
							color.a = (byte)(255f * (1f - num));
						}
						else
						{
							autoStrutQuickViz = false;
							if (!PhysicsGlobals.AutoStrutDisplay)
							{
								flag = true;
								break;
							}
						}
					}
				}
				float num2 = 1f / UIMasterController.Instance.mainCanvas.scaleFactor;
				vectorLine2.points2[0] = camera.WorldToScreenPoint(vector) * num2;
				vectorLine2.points2[1] = camera.WorldToScreenPoint(vector2) * num2;
				if ((vectorLine2.points2[0].x > 0f && vectorLine2.points2[0].x < (float)Screen.width && vectorLine2.points2[0].y > 0f && vectorLine2.points2[0].y < (float)Screen.height) || (vectorLine2.points2[1].x > 0f && vectorLine2.points2[1].x < (float)Screen.width && vectorLine2.points2[1].y > 0f && vectorLine2.points2[1].y < (float)Screen.height))
				{
					flag = false;
					vectorLine2.color = color;
					vectorLine2.active = true;
					vectorLine2.Draw();
				}
			}
		}
		if (flag && autoStrutLines != null)
		{
			int count2 = autoStrutLines.Count;
			while (count2-- > 0)
			{
				autoStrutLines[count2].active = false;
			}
		}
	}

	public void ReleaseAutoStruts()
	{
		if (autoStrutJoints != null && autoStrutJoints.Count > 0)
		{
			int count = autoStrutJoints.Count;
			while (count-- > 0)
			{
				autoStrutJoints[count].DestroyJoint();
			}
			autoStrutJoints.Clear();
		}
		if (autoStrutLines != null)
		{
			int count2 = autoStrutLines.Count;
			while (count2-- > 0)
			{
				VectorLine line = autoStrutLines[count2];
				VectorLine.Destroy(ref line);
			}
			autoStrutLines.Clear();
		}
		if (autoStrutCycling && cyclingAutoStruts != null)
		{
			StopCoroutine(cyclingAutoStruts);
			cyclingAutoStruts = null;
			autoStrutCycling = false;
		}
	}

	public void CheckAutoStruts()
	{
		if (HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch && EditorLogic.fetch.ship != null)
		{
			for (int i = 0; i < EditorLogic.fetch.ship.parts.Count; i++)
			{
				EditorLogic.fetch.ship.parts[i].CheckTargetAutoStruts(this);
			}
		}
		else if (HighLogic.LoadedSceneIsFlight && vessel != null)
		{
			for (int j = 0; j < vessel.parts.Count; j++)
			{
				vessel.parts[j].CheckTargetAutoStruts(this);
			}
		}
	}

	public void CheckTargetAutoStruts(Part p)
	{
		if (autoStrutJoints == null || autoStrutJoints.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < autoStrutJoints.Count; i++)
		{
			PartJoint partJoint = autoStrutJoints[i];
			if (partJoint.Child.persistentId == p.persistentId || partJoint.Parent.persistentId == p.persistentId || partJoint.Target.persistentId == p.persistentId)
			{
				CycleAutoStrut();
			}
		}
	}

	public Part GetAutoStrutAnchor(out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		nodeToParent = (nodeFromParent = null);
		srfAttached = false;
		Part lastPart = null;
		bool flag = false;
		switch (autoStrutMode)
		{
		default:
			return null;
		case AutoStrutMode.Root:
		case AutoStrutMode.ForceRoot:
		{
			Part part = null;
			if (HighLogic.LoadedSceneIsFlight)
			{
				part = vessel.rootPart;
			}
			else
			{
				part = this;
				while (part.parent != null)
				{
					part = part.parent;
				}
			}
			flag = HasFreePivotBetweenRoot(out lastPart, out nodeToParent, out nodeFromParent, out srfAttached);
			if (!((autoStrutExcludeParent && parent != null && parent == part) || flag))
			{
				return part;
			}
			return lastPart;
		}
		case AutoStrutMode.Heaviest:
		case AutoStrutMode.ForceHeaviest:
			if (!autoStrutExcludeParent)
			{
				return GetMassivePart(this, out lastPart, out nodeToParent, out nodeFromParent, out srfAttached);
			}
			return GetMassivePart(this, out lastPart, out nodeToParent, out nodeFromParent, out srfAttached, parent);
		case AutoStrutMode.Grandparent:
		case AutoStrutMode.ForceGrandparent:
			if (parent != null && parent.parent != null)
			{
				if (flag = HasFreePivotBetweenParentPart(parent.parent, out lastPart, out nodeToParent, out nodeFromParent, out srfAttached))
				{
					return lastPart;
				}
				return parent.parent;
			}
			if (parent != null)
			{
				if (flag = HasFreePivotBetweenParentPart(parent, out lastPart, out nodeToParent, out nodeFromParent, out srfAttached))
				{
					return lastPart;
				}
				return parent;
			}
			return null;
		}
	}

	public Part GetMassivePart(Part start, out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached, params Part[] excluded)
	{
		float highestMass = float.MinValue;
		Part highestPart = null;
		lastPart = null;
		nodeToParent = (nodeFromParent = null);
		srfAttached = false;
		if (autoStrutCacheVessel == start.vessel && autoStrutCacheTick == Time.fixedTime)
		{
			highestMass = autoStrutHighMass;
		}
		if (start.isRobotic() && parent != null)
		{
			nodeToParent = AttachedPartNode(parent, start, out srfAttached);
		}
		GetMassiveParentPart(start, start, ref highestPart, ref highestMass, ref lastPart, ref nodeToParent, ref nodeFromParent, ref srfAttached, excluded);
		Part highestPart2 = highestPart;
		float highestMass2 = highestMass;
		Part lastPart2 = null;
		AttachNode nodeToParent2 = null;
		AttachNode nodeFromParent2 = null;
		bool srfAttached2 = false;
		GetMassiveChildPart(start, start, start.children, ref highestPart2, ref highestMass2, ref lastPart2, ref nodeToParent2, ref nodeFromParent2, ref srfAttached2, null, excluded);
		if ((highestPart2 != null && highestPart == null) || (highestPart != null && highestPart2.persistentId != highestPart.persistentId))
		{
			highestPart = highestPart2;
			highestMass = highestMass2;
			lastPart = lastPart2;
			nodeToParent = nodeToParent2;
			nodeFromParent = nodeFromParent2;
			srfAttached = srfAttached2;
		}
		autoStrutHighMass = highestMass;
		autoStrutCacheTick = Time.fixedTime;
		autoStrutCacheVessel = start.vessel;
		return highestPart;
	}

	public void GetMassiveParentPart(Part original, Part child, ref Part highestPart, ref float highestMass, ref Part lastPart, ref AttachNode nodeToParent, ref AttachNode nodeFromParent, ref bool srfAttached, params Part[] excluded)
	{
		if (child == null || child.parent == null)
		{
			return;
		}
		MassivePartCheck(original, child.parent, ref highestPart, ref highestMass, excluded);
		if (child.parent.HasFreePivot())
		{
			if (child.parent.isRobotic())
			{
				nodeFromParent = AttachedPartNode(child, child.parent, out srfAttached);
				lastPart = child.parent;
			}
			return;
		}
		GetMassiveParentPart(original, child.parent, ref highestPart, ref highestMass, ref lastPart, ref nodeToParent, ref nodeFromParent, ref srfAttached, excluded);
		Part highestPart2 = highestPart;
		float highestMass2 = highestMass;
		Part lastPart2 = null;
		AttachNode nodeToParent2 = null;
		AttachNode nodeFromParent2 = null;
		bool srfAttached2 = false;
		GetMassiveChildPart(original, child.parent, child.parent.children, ref highestPart2, ref highestMass2, ref lastPart2, ref nodeToParent2, ref nodeFromParent2, ref srfAttached2, child, excluded);
		if ((highestPart2 != null && highestPart == null) || (highestPart != null && highestPart2.persistentId != highestPart.persistentId))
		{
			highestPart = highestPart2;
			highestMass = highestMass2;
			lastPart = lastPart2;
			if (nodeToParent2 != null)
			{
				nodeToParent = nodeToParent2;
			}
			if (nodeFromParent2 != null)
			{
				nodeFromParent = nodeFromParent2;
			}
			srfAttached = srfAttached2;
		}
	}

	public void GetMassiveChildPart(Part original, Part parent, List<Part> children, ref Part highestPart, ref float highestMass, ref Part lastPart, ref AttachNode nodeToParent, ref AttachNode nodeFromParent, ref bool srfAttached, Part ignoredChild, params Part[] excluded)
	{
		if (children == null || children.Count == 0)
		{
			return;
		}
		Part part = null;
		int count = children.Count;
		while (count-- > 0)
		{
			part = children[count];
			if (original.isRobotic() && parent.persistentId == original.persistentId)
			{
				nodeFromParent = AttachedPartNode(original, part, out srfAttached);
			}
			if (part.persistentId == original.persistentId || part == ignoredChild)
			{
				continue;
			}
			MassivePartCheck(original, part, ref highestPart, ref highestMass, excluded);
			if (part.HasFreePivot())
			{
				if (part.isRobotic())
				{
					nodeFromParent = AttachedPartNode(part.parent, part, out srfAttached);
					lastPart = part;
				}
			}
			else
			{
				GetMassiveChildPart(original, part, part.children, ref highestPart, ref highestMass, ref lastPart, ref nodeToParent, ref nodeFromParent, ref srfAttached, null, excluded);
			}
		}
	}

	public void MassivePartCheck(Part original, Part p, ref Part highestPart, ref float highestMass, params Part[] excluded)
	{
		float num = ((HighLogic.LoadedSceneIsEditor || p.rb == null) ? (p.mass + p.GetResourceMass()) : p.rb.mass);
		if (Mathf.Approximately(num, highestMass))
		{
			Vector3 position = original.transform.position;
			Vector3 position2 = highestPart.transform.position;
			Vector3 position3 = p.transform.position;
			float num2 = Vector3.Distance(position2, position);
			float num3 = Vector3.Distance(position3, position);
			if ((double)Mathf.Abs(num2 - num3) < 0.05 && original.parent != null)
			{
				Vector3 lhs = position - original.parent.transform.position;
				Vector3 rhs = Vector3.Cross(lhs, position2);
				Vector3 rhs2 = Vector3.Cross(lhs, position3);
				float num4 = Vector3.Dot(original.transform.up, rhs);
				if (Vector3.Dot(original.transform.up, rhs2) < num4)
				{
					highestMass = -1f;
				}
				else
				{
					num = -1f;
				}
			}
			else if (num3 < num2)
			{
				highestMass = -1f;
			}
			else
			{
				num = -1f;
			}
		}
		if (!(num > highestMass))
		{
			return;
		}
		bool flag = false;
		int num5 = excluded.Length;
		while (num5-- > 0)
		{
			if (excluded[num5] == p)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			highestMass = num;
			highestPart = p;
		}
	}

	public bool HasFreePivot()
	{
		int count = modules.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(modules[count] is IJointLockState jointLockState) || !jointLockState.IsJointUnlocked());
		return true;
	}

	public bool HasFreePivotBetweenRoot(out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		Part previousPart = this;
		srfAttached = false;
		nodeToParent = (nodeFromParent = null);
		lastPart = this;
		nodeToParent = AttachedPartNode(parent, this, out srfAttached);
		Part part = parent;
		while (part != null && part.parent != null)
		{
			if (!part.HasFreePivot())
			{
				previousPart = part;
				part = part.parent;
				continue;
			}
			if (part.isRobotic())
			{
				nodeFromParent = AttachedPartNode(previousPart, part, out srfAttached);
				lastPart = part;
			}
			return true;
		}
		return false;
	}

	public AttachNode AttachedPartNode(Part previousPart, Part currentPart, out bool srfAttached)
	{
		AttachNode result = null;
		srfAttached = false;
		if (!(previousPart == null) && !(currentPart == null))
		{
			for (int i = 0; i < currentPart.attachNodes.Count; i++)
			{
				if (currentPart.attachNodes[i].attachedPart != null && currentPart.attachNodes[i].attachedPart.persistentId == previousPart.persistentId)
				{
					result = currentPart.attachNodes[i];
					break;
				}
			}
			if (currentPart.srfAttachNode != null && currentPart.srfAttachNode.attachedPart != null && currentPart.srfAttachNode.attachedPart.persistentId == previousPart.persistentId)
			{
				result = currentPart.srfAttachNode;
				srfAttached = true;
			}
			else if (previousPart.srfAttachNode != null && previousPart.srfAttachNode.attachedPart != null && previousPart.srfAttachNode.attachedPart.persistentId == currentPart.persistentId)
			{
				result = previousPart.srfAttachNode;
				srfAttached = true;
			}
			return result;
		}
		return result;
	}

	public bool HasFreePivotBetweenParentPart(Part endPart, out Part lastPart, out AttachNode nodeToParent, out AttachNode nodeFromParent, out bool srfAttached)
	{
		Part previousPart = this;
		srfAttached = false;
		nodeToParent = (nodeFromParent = null);
		lastPart = this;
		nodeToParent = AttachedPartNode(parent, this, out srfAttached);
		Part part = parent;
		while (part != null && part.parent != null)
		{
			if (!part.HasFreePivot())
			{
				if (part.parent.persistentId != endPart.persistentId)
				{
					previousPart = part;
					part = part.parent;
					continue;
				}
				if (!part.parent.HasFreePivot())
				{
					break;
				}
				if (part.parent.isRobotic())
				{
					nodeFromParent = AttachedPartNode(part, part.parent, out srfAttached);
					lastPart = part.parent;
				}
				return true;
			}
			if (part.isRobotic())
			{
				nodeFromParent = AttachedPartNode(previousPart, part, out srfAttached);
				lastPart = part;
			}
			return true;
		}
		return false;
	}

	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001325")]
	public void ToggleRigidAttachment()
	{
		rigidAttachment = !rigidAttachment;
		ApplyRigidAttachment();
		if (GameSettings.AUTOSTRUT_SYMMETRY)
		{
			int count = symmetryCounterparts.Count;
			while (count-- > 0)
			{
				Part part = symmetryCounterparts[count];
				part.rigidAttachment = rigidAttachment;
				part.ApplyRigidAttachment();
			}
		}
	}

	public void ApplyRigidAttachment()
	{
		SetupRigidAttachmentUI();
		if (HighLogic.LoadedSceneIsFlight)
		{
			ResetJoints();
			if ((bool)attachJoint)
			{
				attachJoint.SetUnbreakable(unbreakable: true, rigidAttachment);
				StartCoroutine(SetJoints());
			}
		}
	}

	public IEnumerator SetJoints()
	{
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		ResetJoints();
	}

	public void SetupRigidAttachmentUI()
	{
		Events["ToggleRigidAttachment"].guiName = (rigidAttachment ? "#autoLOC_6001327" : "#autoLOC_6001326");
	}

	public void AddForce(Vector3d vec)
	{
		if (RigidBodyPart != null)
		{
			RigidBodyPart.force += vec;
		}
	}

	public void AddImpulse(Vector3d vec)
	{
		if (RigidBodyPart != null)
		{
			RigidBodyPart.force += vec / TimeWarp.fixedDeltaTime;
		}
	}

	public void AddForceAtPosition(Vector3d vec, Vector3d pos)
	{
		RigidBodyPart.forces.Add(new ForceHolder(vec, pos));
	}

	public void AddTorque(Vector3d vec)
	{
		vessel.ResetRBAnchor();
		RigidBodyPart.torque += vec;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Click: " + eventData.button);
	}

	public static Transform FindTransformInChildrenExplicit(Transform parent, Transform find)
	{
		if (parent == find)
		{
			return parent;
		}
		int childCount = parent.childCount;
		Transform transform;
		do
		{
			if (childCount-- > 0)
			{
				transform = FindTransformInChildrenExplicit(parent.GetChild(childCount), find);
				continue;
			}
			return null;
		}
		while (!(transform != null));
		return transform;
	}

	public bool HasValidContractObjective(string objectiveType)
	{
		int count = Modules.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!Modules[count].IsValidContractObjective(objectiveType));
		return true;
	}

	public void OnStageIconDestroy()
	{
		tempIndicator = null;
	}

	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < Modules.Count; i++)
			{
				Modules[i].AddPartModuleAdjusterList(moduleAdjusters);
			}
		}
	}

	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < Modules.Count; i++)
			{
				Modules[i].RemovePartModuleAdjusterList(moduleAdjusters);
			}
		}
	}

	public void PartRepair()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			return;
		}
		for (int i = 0; i < Modules.Count; i++)
		{
			for (int j = 0; j < Modules[i].CurrentModuleAdjusterList.Count; j++)
			{
				if (Modules[i].CurrentModuleAdjusterList[j].canBeRepaired)
				{
					Modules[i].RemovePartModuleAdjuster(Modules[i].CurrentModuleAdjusterList[j]);
				}
			}
		}
		GameEvents.onPartRepair.Fire(this);
	}

	[KSPEvent(groupName = "VesselNaming", groupDisplayName = "#autoLOC_8003391", guiActiveUncommand = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003140")]
	public void SetVesselNaming()
	{
		if (!(renameDialog != null))
		{
			InputLockManager.SetControlLock("vesselRenameDialog");
			renameDialog = VesselRenameDialog.SpawnNameFromPart(this, onVesselNamingAccept, onVesselNamingDismiss, onVesselNamingRemove, allowTypeChange: true, VesselType.Debris);
		}
	}

	public void onVesselNamingAccept(string newVesselName, VesselType newVesselType, int newPriority)
	{
		if (Vessel.IsValidVesselName(newVesselName))
		{
			if (vesselNaming == null)
			{
				vesselNaming = new VesselNaming();
			}
			vesselNaming.vesselName = newVesselName;
			vesselNaming.vesselType = newVesselType;
			vesselNaming.namingPriority = newPriority;
			RefreshVesselNamingPAWDisplay();
			GameEvents.onPartVesselNamingChanged.Fire(this);
			onVesselNamingDismiss();
		}
	}

	public void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		if (vessel == nChg.host)
		{
			RefreshVesselNamingPAWDisplay();
		}
	}

	public void RefreshVesselNamingPAWDisplay()
	{
		if (vesselNaming != null)
		{
			BaseField baseField = Fields["vesselNamingDisplayName"];
			Fields["vesselNamingDisplayPriority"].guiActive = true;
			baseField.guiActive = true;
			BaseField baseField2 = Fields["vesselNamingDisplayName"];
			Fields["vesselNamingDisplayPriority"].guiActiveEditor = true;
			baseField2.guiActiveEditor = true;
			vesselNamingDisplayName = Localizer.Format(vesselNaming.vesselName);
			vesselNamingDisplayPriority = vesselNaming.namingPriority.ToString();
		}
		else
		{
			BaseField baseField3 = Fields["vesselNamingDisplayName"];
			Fields["vesselNamingDisplayPriority"].guiActive = false;
			baseField3.guiActive = false;
			BaseField baseField4 = Fields["vesselNamingDisplayName"];
			Fields["vesselNamingDisplayPriority"].guiActiveEditor = false;
			baseField4.guiActiveEditor = false;
		}
	}

	public void onVesselNamingDismiss()
	{
		InputLockManager.RemoveControlLock("vesselRenameDialog");
	}

	public void onVesselNamingRemove()
	{
		vesselNaming = null;
		RefreshVesselNamingPAWDisplay();
		onVesselNamingDismiss();
	}

	[KSPEvent(advancedTweakable = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_8003305")]
	public void RemoveFromSymmetry()
	{
		CleanSymmetryReferences();
		if (stackIcon != null)
		{
			stackIcon.RemoveIcon();
			stackIcon.CreateIcon();
			if (StageManager.Instance != null)
			{
				StageManager.Instance.SortIcons(instant: true);
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			EditorLogic.fetch.SetBackup();
		}
	}

	public void SetRemoveSymmetryVisibililty()
	{
		Events["RemoveFromSymmetry"].guiActive = (Events["RemoveFromSymmetry"].guiActiveEditor = symmetryCounterparts != null && symmetryCounterparts.Count > 0);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_439839 = Localizer.Format("#autoLOC_439839");
		cacheAutoLOC_439840 = Localizer.Format("#autoLOC_439840");
		cacheAutoLOC_7001406 = Localizer.Format("#autoLOC_7001406");
		cacheAutoLOC_211269 = Localizer.Format("#autoLOC_211269");
		cacheAutoLOC_211272 = Localizer.Format("#autoLOC_211272");
		cacheAutoLOC_211274 = Localizer.Format("#autoLOC_211274");
		cacheAutoLOC_7000027 = Localizer.Format("#autoLOC_7000027");
		cacheAutoLOC_211097 = Localizer.Format("#autoLOC_211097");
	}

	public float GetBoundsPoints(Vector3 groundNormal, out float centerPointOffset)
	{
		Vector3 vector = Vector3.zero;
		float num = float.PositiveInfinity;
		Quaternion rotation = base.transform.rotation;
		Quaternion rotation2 = Quaternion.FromToRotation(groundNormal, Vector3.forward) * rotation;
		partTransform.rotation = rotation2;
		GameObject gameObject = new GameObject("MeasureObject");
		gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		gameObject.transform.rotation = new Quaternion(base.transform.rotation.x, base.transform.rotation.y, base.transform.rotation.z, base.transform.rotation.w);
		gameObject.transform.position += -Vector3.forward * 1000f;
		Collider collider = null;
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool flag = true;
			if (componentsInChildren[i].gameObject.layer == 26 || componentsInChildren[i].gameObject.layer == 27 || componentsInChildren[i].gameObject.layer == 30)
			{
				flag = componentsInChildren[i].enabled;
				componentsInChildren[i].enabled = true;
			}
			if (componentsInChildren[i].gameObject.layer != 21 && componentsInChildren[i].gameObject.layer != 16 && componentsInChildren[i].gameObject.layer != 20 && componentsInChildren[i].enabled)
			{
				float num2 = 0f;
				num2 = ((componentsInChildren[i].gameObject.layer == 26 || componentsInChildren[i].gameObject.layer == 27 || componentsInChildren[i].gameObject.layer == 30) ? componentsInChildren[i].bounds.min.z : componentsInChildren[i].ClosestPoint(gameObject.transform.position).z);
				if (num2 < num)
				{
					collider = componentsInChildren[i];
				}
				num = Mathf.Min(num, num2);
				vector = ((!(vector == Vector3.zero)) ? Vector3.Lerp(vector, componentsInChildren[i].bounds.center, 0.5f) : componentsInChildren[i].bounds.center);
			}
			if (componentsInChildren[i].gameObject.layer == 26 || componentsInChildren[i].gameObject.layer == 27 || componentsInChildren[i].gameObject.layer == 30)
			{
				componentsInChildren[i].enabled = flag;
			}
		}
		if (collider != null)
		{
			if (collider.gameObject.name.Contains("WheelCollider"))
			{
				WheelCollider wheelCollider = collider as WheelCollider;
				if (wheelCollider != null)
				{
					num -= wheelCollider.radius;
				}
				num -= 0.15f;
			}
			else if (collider.gameObject.name.Contains("collisionEnhancer"))
			{
				num -= 0.15f;
			}
		}
		UnityEngine.Object.Destroy(gameObject);
		float result = base.transform.position.z - num;
		centerPointOffset = Mathf.Abs(partTransform.position.z - vector.z);
		partTransform.rotation = rotation;
		return result;
	}

	public void CacheIConstructionPartModules()
	{
		if (Modules == null || constructionModules != null)
		{
			return;
		}
		constructionModules = new List<IConstruction>();
		for (int i = 0; i < Modules.Count; i++)
		{
			IConstruction component = Modules[i].GetComponent<IConstruction>();
			if (component != null)
			{
				constructionModules.Add(component);
			}
		}
	}

	public bool PartCanBeDetached()
	{
		bool flag = true;
		CacheIConstructionPartModules();
		for (int i = 0; i < constructionModules.Count; i++)
		{
			if (!(flag = constructionModules[i].CanBeDetached()))
			{
				break;
			}
		}
		if (flag)
		{
			return !hasKerbalOnLadder;
		}
		return false;
	}

	public bool PartCanBeOffset()
	{
		bool flag = true;
		CacheIConstructionPartModules();
		if (vesselType != VesselType.DroppedPart && (vesselType != 0 || !(vessel != null) || vessel.parts == null || vessel.parts.Count != 1) && vesselType != VesselType.const_11)
		{
			for (int i = 0; i < constructionModules.Count; i++)
			{
				if (!(flag = constructionModules[i].CanBeOffset()))
				{
					break;
				}
			}
			if (flag)
			{
				return !hasKerbalOnLadder;
			}
			return false;
		}
		flag = false;
		return false;
	}

	public bool PartCanBeRotated()
	{
		bool flag = true;
		CacheIConstructionPartModules();
		if (vesselType != VesselType.DroppedPart && (vesselType != 0 || !(vessel != null) || vessel.parts == null || vessel.parts.Count != 1) && vesselType != VesselType.const_11)
		{
			for (int i = 0; i < constructionModules.Count; i++)
			{
				if (!(flag = constructionModules[i].CanBeOffset()))
				{
					break;
				}
			}
			if (flag)
			{
				return !hasKerbalOnLadder;
			}
			return false;
		}
		flag = false;
		return false;
	}
}
