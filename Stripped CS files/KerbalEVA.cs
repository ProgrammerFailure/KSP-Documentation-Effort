using System;
using System.Collections;
using System.Collections.Generic;
using Expansions;
using FinePrint.Utilities;
using ns36;
using ns9;
using UnityEngine;

public class KerbalEVA : PartModule
{
	public class ResourceListItem
	{
		public ProtoPartResourceSnapshot pPResourceSnapshot;

		public int priority;
	}

	[Serializable]
	public class HelmetColliderSetup
	{
		public float helmetOnRadius = 0.27f;

		public Vector3 helmetOnCenter = Vector3.zero;

		public float helmetOffRadius = 0.19f;

		public Vector3 helmetOffCenter = new Vector3(0.03f, 0.03f, 0f);
	}

	public class ClamberPath
	{
		public Vector3 edgeFaceNormal;

		public Vector3 srfNormal;

		public Vector3 p1;

		public Vector3 p2;

		public Vector3 p3;

		public float clamberHeight;

		public float standOffDistance;

		public ClamberPath(Vector3 edgeFaceNormal, Vector3 srfNormal, Vector3 p1, Vector3 p2, Vector3 p3, float height, float standOff)
		{
			this.edgeFaceNormal = edgeFaceNormal;
			this.srfNormal = srfNormal;
			clamberHeight = height;
			standOffDistance = standOff;
			this.p1 = p1 + edgeFaceNormal * standOff;
			this.p2 = p2 + (edgeFaceNormal + srfNormal) * standOff;
			this.p3 = p3 + srfNormal * standOff;
		}
	}

	public enum VisorStates
	{
		Raised,
		Lowered,
		Raising,
		Lowering
	}

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001402")]
	public float lightR = 1f;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001403")]
	public float lightG = 0.5176f;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001404")]
	public float lightB;

	[UI_ColorPicker]
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006093")]
	public string colorChanger;

	[KSPField]
	public float walkSpeed = 0.8f;

	[KSPField]
	public float strafeSpeed = 0.5f;

	[KSPField]
	public float runSpeed = 2.2f;

	[KSPField]
	public float turnRate = 4f;

	[KSPField]
	public float maxJumpForce = 10f;

	[KSPField]
	public float jumpMultiplier = 0.5f;

	[KSPField]
	public float boundForce = 1f;

	[KSPField]
	public float boundSpeed = 0.8f;

	[KSPField]
	public float boundThreshold;

	public float boundForceMassFactor = 0.533f;

	public Vector2 boundColliderYOffset = new Vector2(0.01f, 0.04f);

	public float boundColliderModifierThresholdTime = 0.25f;

	public float boundColliderModifierCounter;

	[KSPField]
	public float SlopeForce = 1.2f;

	public int slopeMovementDirection;

	[KSPField]
	public float swimSpeed = 0.8f;

	[KSPField]
	public float waterAngularDragMultiplier = 0.01f;

	[KSPField]
	public float ladderClimbSpeed = 0.6f;

	[KSPField]
	public float ladderPushoffForce = 3f;

	[KSPField]
	public float minWalkingGee = 0.17f;

	[KSPField]
	public float minRunningGee = 0.6f;

	[KSPField]
	public float initialMass = 1.5f;

	[KSPField]
	public float massMultiplier = 0.03f;

	[KSPField]
	public float onFallHeightFromTerrain = 0.3f;

	[KSPField]
	public float clamberMaxAlt = 100f;

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

	public AdvancedRagdoll advRagdoll;

	public KerbalFSM fsm;

	public bool DebugFSMState;

	public bool HasJetpack;

	[KSPField(isPersistant = true)]
	public bool JetpackDeployed;

	public bool jetpackDeployedForWelding;

	public bool JetpackIsThrusting;

	public bool autoGrabLadderOnStart = true;

	[KSPField(isPersistant = true)]
	public bool lampOn;

	public GameObject headLamp;

	[KSPField]
	public bool splatEnabled = true;

	[KSPField]
	public float splatSpeed = 50f;

	public GameObject splatPrefab;

	public bool CharacterFrameMode = true;

	public bool CharacterFrameModeToggle;

	public bool loadedFromSFS;

	public string loadedStateName = "";

	public Vector3 ejectDirection = Vector3.up;

	[KSPField]
	public string propellantResourceName = "EVA Propellant";

	[Obsolete("EVA Fuel is now carried in jetpacks and tanks, the kerbal has no fuel on their person")]
	[KSPField]
	public double propellantResourceDefaultAmount = 5.0;

	public PartResource propellantResource;

	public List<ResourceListItem> inventoryPropellantResources;

	public DockedVesselInfo kerbalVesselInfo;

	public Rigidbody _rigidbody;

	public Animation _animation;

	public List<ModuleLiftingSurface> chuteLiftingSurfaces = new List<ModuleLiftingSurface>();

	public ModuleEvaChute evaChute;

	public bool HasParachute;

	public Transform helmetTransform;

	public Transform neckRingTransform;

	public SphereCollider helmetCollider;

	public HelmetColliderSetup helmetColliderSetup;

	public CapsuleCollider[] bodyColliders;

	public bool helmetTransformExists;

	public bool neckRingTransformExists;

	[KSPField(isPersistant = true)]
	public bool isHelmetEnabled = true;

	[KSPField(isPersistant = true)]
	public bool isNeckRingEnabled = true;

	[KSPField]
	public float helmetOffMinSafePressureAtm = 0.177f;

	[KSPField]
	public float helmetOffMaxSafePressureAtm = 20f;

	[KSPField]
	public float helmetOffMinSafeTempK = 223f;

	[KSPField]
	public float helmetOffMaxSafeTempK = 333f;

	[KSPField]
	public float helmetOffMaxOceanPressureAtm = 5.8f;

	[KSPField]
	public float helmetOffMinSafePressureAtmMargin = 0.038f;

	[KSPField]
	public float helmetOffMaxSafePressureAtmMargin = 5f;

	[KSPField]
	public float helmetOffMinSafeTempKMargin = 10f;

	[KSPField]
	public float helmetOffMaxSafeTempKMargin = 10f;

	[KSPField]
	public float helmetOffMaxOceanPressureAtmMargin = 1f;

	[KSPField]
	public float evaExitTemperature = 303f;

	public float feetToPivotDistance = 0.25f;

	public bool partPlacementMode;

	public ModuleInventoryPart moduleInventoryPartReference;

	public static bool alwaysShowInventory;

	public kerbalExpressionSystem myExpressionSystem;

	public IEnumerator returnIDLE;

	public IEnumerator startExpression;

	[SerializeField]
	public float newidleTime = 10f;

	public float newidleCounter;

	public bool startTimer;

	public int idleAnimationsIndex;

	public KerbalPortrait portrait;

	public bool visibleInPortrait;

	public Camera kerbalCamSkyBox;

	public Camera kerbalCamAtmos;

	public Camera kerbalCam01;

	public Camera kerbalCam00;

	public Camera kerbalPortraitCamera;

	public RenderTexture AvatarTexture;

	public float KerbalAvatarUpdateInterval = 0.1f;

	public WaitForSeconds updIntervalYield;

	public Coroutine updateAvatarCoroutine;

	public bool isActiveVessel;

	public Vector3 cameraPosition;

	[SerializeField]
	public float standardCameraDistance = 0.519f;

	[SerializeField]
	public float swimmingCameraDistance = 0.612f;

	[SerializeField]
	public float runningCameraDistance = 0.654f;

	[SerializeField]
	public float ragdollCameraDistance = 1f;

	public List<ModuleScienceExperiment> moduleScienceExperiments;

	public ModuleScienceExperiment moduleScienceExperimentROC;

	public ModuleColorChanger suitColorChanger;

	public bool sciencePanelAnimPlaying;

	public float sciencePanelAnimCooldown;

	public bool pickRocSampleAnimPlaying;

	public float pickRocSampleAnimCooldown;

	public List<KerbalProp> kerbalObjects;

	public GameObject hammerPrefab;

	public Transform hammerAnchor;

	public GameObject hammer;

	public MeshRenderer hammerMesh;

	public Animation hammerAnimation;

	public float hammerAnimTimer;

	public ModuleGroundSciencePart sciencePart;

	public KerbalProp golfClub;

	public KerbalProp bananaProp;

	public KerbalProp wingnutProp;

	public SkinnedMeshRenderer helmetMesh;

	public SkinnedMeshRenderer bodyMesh;

	public SkinnedMeshRenderer neckringMesh;

	public GameObject weldToolPrefab;

	public Transform weldToolAnchor;

	public GameObject weldTool;

	public WeldFX weldFX;

	public AudioSource weldingLasersFX;

	public FXGroup weldingLaserGroup;

	public SuitCombos suitCombos;

	public bool alternateIdleDisabled;

	[KSPField(isPersistant = true)]
	public bool isVisorEnabled;

	public Transform JetpackTransform;

	public Transform BackpackTransform;

	public Transform BackpackStTransform;

	public Transform StorageTransform;

	public Transform StorageSlimTransform;

	public Transform ChuteJetpackTransform;

	public Transform ChuteStTransform;

	public Transform ChuteContainerTransform;

	public const string JetpackPartName = "evaJetpack";

	public const string ChutePartName = "evaChute";

	[UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 10f)]
	[KSPAxisField(minValue = 10f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage = 100f;

	[SerializeField]
	public bool isLadderJointed;

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

	public KFSMTimedEvent On_recover_complete;

	public KFSMTimedEvent On_jump_complete;

	public KFSMTimedEvent On_land_complete;

	public KFSMTimedEvent On_LadderPushoff_complete;

	public AnimationState tmpBoundState;

	[KSPField]
	public float boundFrequency = 0.15f;

	[KSPField]
	public float boundSharpness = 0.2f;

	[KSPField]
	public float boundAttack = 0.4f;

	[KSPField]
	public float boundRelease = 2f;

	[KSPField]
	public float boundFallThreshold = 1.5f;

	public float tgtBoundStep;

	public float boundStepSpeed;

	[KSPField(isPersistant = true)]
	public float lastBoundStep;

	public float halfHeight = 0.269f;

	[KSPField(isPersistant = true)]
	public int _flags = 1;

	[KSPField]
	public float flagReach = 0.3f;

	public FlagSite flag;

	public KerbalSeat kerbalSeat;

	public Vector3 tgtRpos;

	public Vector3 lastTgtRPos;

	public Vector3 packTgtRPos;

	public Vector3 packRRot;

	public Vector3 cmdRot;

	public Vector3 cmdDir;

	public Vector3 ladderTgtRPos;

	public Vector3 flagSpot;

	public Vector3 parachuteInput;

	public float currentSpd;

	public float tgtSpeed;

	public float lastTgtSpeed;

	public float deltaHdg;

	public float lastDeltaHdg;

	public float jumpForce;

	public bool boundLeftFoot;

	public bool manualAxisControl;

	public Quaternion rd_rot;

	public Quaternion rd_tgtRot;

	public Quaternion manualRotation;

	public Quaternion slopeRotation;

	public float normalizedBoundTime;

	public float colliderHeight;

	[KSPField]
	public float Kp = 0.7f;

	[KSPField]
	public float Ki = 0.25f;

	[KSPField]
	public float Kd = 0.3f;

	[KSPField]
	public float iC = 0.005f;

	public Vector3 tgtFwd;

	public Vector3 error;

	public Vector3 integral;

	public Vector3 derivative;

	public Vector3 prev_error;

	public Vector3 tgtUp;

	public bool thrustPercentagePIDBoost = true;

	public float pidBoostThreshold = 55f;

	public float pidBoostMultiplier = 0.00012f;

	public float pidBoostExponent = 2.7f;

	public FXGroup PitchPos;

	public FXGroup PitchNeg;

	public FXGroup YawPos;

	public FXGroup YawNeg;

	public FXGroup RollPos;

	public FXGroup RollNeg;

	public FXGroup xPos;

	public FXGroup xNeg;

	public FXGroup yPos;

	public FXGroup yNeg;

	public FXGroup zPos;

	public FXGroup zNeg;

	public float linFXLatch = 0.02f;

	public float rotFXLatch = 0.01f;

	public float linFXMinPower = 0.2f;

	public float linFXMaxPower = 0.9f;

	public float rotFXMinPower = 0.2f;

	public float rotFXMaxPower = 0.9f;

	[KSPField]
	public float rotPower = 1f;

	[KSPField]
	public float linPower = 10f;

	public Vector3 packLinear;

	[KSPField]
	public float PropellantConsumption = 0.025f;

	public float fuelFlowRate;

	public Vector3 fFwd = Vector3.forward;

	public Vector3 fUp = Vector3.up;

	public Vector3 fRgt = Vector3.right;

	[KSPField]
	public float stumbleThreshold = 3.5f;

	public Vector3 lastCollisionDirection;

	public Vector3 lastCollisionNormal;

	public GClass0 availableROC;

	public GClass0 experimentROC;

	public bool isAnchored;

	public float kerbalAnchorTimeThreshold = 0.5f;

	public float kerbalAnchorTimeCounter;

	public FixedJoint anchorJoint;

	public bool isRagdoll;

	public bool canRecover;

	public Vector3d geeForce;

	public Vector3d coriolisForce;

	public Vector3d centrifugalForce;

	public int referenceFrameChanged_rdPhysHold;

	[KSPField]
	public float hopThreshold = 2f;

	[KSPField]
	public float recoverThreshold = 0.6f;

	[KSPField]
	public double recoverTime = 3.0;

	public double lastCollisionTime;

	[KSPField]
	public float splatThreshold = 150f;

	public LadderEndCheck topLadderEnd;

	public LadderEndCheck bottomLadderEnd;

	public bool canClimb;

	public bool canDescend;

	public Transform ladderPivot;

	public List<Collider> currentLadderTriggers = new List<Collider>();

	public Collider currentLadder;

	public Collider secondaryLadder;

	public Part _currentLadderPart;

	public Vector3 ladderPos;

	public Vector3 ladderUp;

	public Vector3 ladderFwd;

	public Vector3 Vtgt;

	public bool invLadderAxis;

	public bool onLadder;

	[Obsolete("No longer used.")]
	public double LadderVesselPerturbationMultiplier = 1.0;

	[Obsolete("No longer used.")]
	public double LadderMinCorrectiveForceSqrMag = 0.1;

	[SerializeField]
	[Tooltip("Dot of two vectors used for maximum angle between ladder forward vectors")]
	public float MinLadderForwardDot = 0.5f;

	[Tooltip("Dot of two vectors used for maximum angle between ladder right vectors")]
	[SerializeField]
	public float MinLadderRightDot = 0.866f;

	public bool ladderTransition;

	public Vector3 clamberOrigin;

	public Vector3 clamberTarget;

	public RaycastHit clamberHitInfo;

	public ClamberPath clamberPath;

	[KSPField]
	public float clamberReach = 0.9f;

	[KSPField]
	public float clamberStandoff = 0.45f;

	public Vector3 controlOrigin;

	public Vector3 controlTarget;

	public RaycastHit controlHitInfo;

	[KSPField]
	public float controlPanelReach = 0.9f;

	[KSPField]
	public float controlPanelStandoff = 0.45f;

	public Part constructionTarget;

	public float constructionTargetPivotOffset = 0.05f;

	public Renderer VisorRenderer;

	[KSPField]
	public float VisorAnimationSpeed = 12f;

	public bool wasHelmetEnabledBeforeWelding = true;

	public bool removeHelmetAfterRaisingVisor = true;

	public bool wasVisorEnabledBeforeWelding;

	public VisorStates visorState;

	[SerializeField]
	public float visorLoweredTargetOffset = 0.5f;

	public float visorRaisedTargetOffset;

	public float visorTargetOffset = 0.5f;

	public float visorCurrentOffset;

	public Vector2 visorTextureOffset;

	[SerializeField]
	public bool replacedGolfBall;

	[SerializeField]
	public Vector3 ballForceDir = Vector3.forward;

	public float ballForce;

	public float ballDrag;

	public float ballTime;

	public float ballAngle;

	public float golfSoundTime;

	public bool playingGolfAnimPlaying;

	public float playingGolfAnimCooldown;

	public Callback afterPlayGolf;

	public List<string> golfSoundFX;

	public string golfSound;

	public bool golfSoundPlayed;

	[SerializeField]
	public bool replacedBanana;

	[SerializeField]
	public float bananaTime = 0.595f;

	[SerializeField]
	public float bananaForce = 1.5f;

	[SerializeField]
	public float bananaSoundTime = 0.59f;

	public Callback afterBanana;

	public bool smashingBananaAnimPlaying;

	public float smashingBananaAnimCooldown;

	public GameObject bananaShards;

	public List<string> bananaSoundFX;

	public string bananaSound;

	public bool bananaSoundPlayed;

	[SerializeField]
	public float wingnutKerbalTorqueForce = 0.5f;

	[SerializeField]
	public float wingnutTransitionTime = 4.5f;

	[SerializeField]
	public float wingnutTorqueTime = 0.71f;

	public Callback afterWingnut;

	public bool spinningWingnutAnimPlaying;

	public float spinningWingnutAnimCooldown;

	public bool appliedTorque;

	public Rigidbody wingnutRB;

	public GameObject wingnut;

	public Collider currentAirlockTrigger;

	public Part currentAirlockPart;

	public static string cacheAutoLOC_114130;

	public static string cacheAutoLOC_114293;

	public static string cacheAutoLOC_114297;

	public static string cacheAutoLOC_114358;

	public static string cacheAutoLOC_115662;

	public static string cacheAutoLOC_115694;

	public static string cacheAutoLOC_6010008;

	public static string cacheAutoLOC_6010009;

	public static string cacheAutoLOC_6010010;

	public static string cacheAutoLOC_6010011;

	public static string cacheAutoLOC_8003204;

	public static string cacheAutoLOC_6010015;

	public static string cacheAutoLOC_8002357;

	public static string cacheAutoLOC_8002358;

	public static string cacheAutoLOC_8002359;

	public static string cacheAutoLOC_8002360;

	public static string cacheAutoLOC_6012032;

	public static string cacheAutoLOC_6006049;

	public string helmetUnsafeReason = "";

	public bool atmosExistence;

	public double kerbalStaticPressureAtm;

	public double kerbalSkinTemp;

	public double kerbalInternalTemp;

	[SerializeField]
	public int framesDelayForHelmetDeathCheck = 5;

	public int framesDelayForHelmetDeathCounter;

	[SerializeField]
	public PhysicMaterial physicMaterial;

	[KSPField(isPersistant = true)]
	public bool useGlobalPhysicMaterial = true;

	public Vector3 ladderPosition;

	public bool Ready { get; set; }

	public PartResource PropellantResource => propellantResource;

	public bool IsChuteState
	{
		get
		{
			if (fsm.CurrentState != st_semi_deployed_parachute)
			{
				return fsm.CurrentState == st_fully_deployed_parachute;
			}
			return true;
		}
	}

	public bool PartPlacementMode => partPlacementMode;

	public ModuleInventoryPart ModuleInventoryPartReference => moduleInventoryPartReference;

	public WeldFX WeldFX => weldFX;

	public bool InConstructionMode { get; set; }

	public bool IsLadderJointed => isLadderJointed;

	public virtual bool VesselUnderControl
	{
		get
		{
			if (base.vessel.state == Vessel.State.ACTIVE)
			{
				return !base.vessel.packed;
			}
			return false;
		}
	}

	public virtual int flagItems
	{
		get
		{
			return _flags;
		}
		set
		{
			_flags = value;
			base.Events["PlantFlag"].guiName = Localizer.Format("#autoLOC_6003096", value);
		}
	}

	public float PIDBoost { get; set; }

	public virtual double Fuel => propellantResource.amount;

	public virtual double FuelCapacity => propellantResource.maxAmount;

	public virtual Part currentLadderPart
	{
		get
		{
			return _currentLadderPart;
		}
		set
		{
			if (_currentLadderPart != value)
			{
				if (_currentLadderPart != null)
				{
					GameEvents.onPartLadderExit.Fire(this, _currentLadderPart);
				}
				GameEvents.onPartLadderEnter.Fire(this, value);
				_currentLadderPart = value;
			}
		}
	}

	public Part CurrentLadderPart => _currentLadderPart;

	public bool OnALadder
	{
		get
		{
			if (!fsm.Started)
			{
				return false;
			}
			return onLadder;
		}
	}

	public Part LadderPart => currentLadderPart;

	public VisorStates VisorState => visorState;

	public bool IsVisorEnabled => isVisorEnabled;

	public string HelmetUnsafeReason => helmetUnsafeReason;

	public PhysicMaterial PhysicMaterial
	{
		get
		{
			if (!useGlobalPhysicMaterial && !(physicMaterial == null))
			{
				return physicMaterial;
			}
			return PhysicsGlobals.KerbalEVAPhysicMaterial;
		}
	}

	public void OnModuleInventoryChanged(ModuleInventoryPart moduleInventory)
	{
		if (moduleInventory != null && moduleInventory == moduleInventoryPartReference)
		{
			UpdatePackModels();
			if (evaChute != null && evaChute.deploymentState != 0 && !moduleInventory.ContainsPart("evaChute"))
			{
				evaChute.CutParachute();
			}
		}
	}

	public void UpdatePackModels()
	{
		BackpackTransform.gameObject.SetActive(value: false);
		BackpackStTransform.gameObject.SetActive(value: false);
		StorageTransform.gameObject.SetActive(value: false);
		StorageSlimTransform.gameObject.SetActive(value: false);
		ChuteJetpackTransform.gameObject.SetActive(value: false);
		ChuteStTransform.gameObject.SetActive(value: false);
		ChuteContainerTransform.gameObject.SetActive(value: false);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = evaChute != null && evaChute.CanCrewMemberUseParachute();
		if (moduleInventoryPartReference.InventoryItemCount > 0)
		{
			for (int i = 0; i < moduleInventoryPartReference.storedParts.Count; i++)
			{
				StoredPart storedPart = moduleInventoryPartReference.storedParts.ValuesList[i];
				if (storedPart != null)
				{
					if (storedPart.partName.Equals("evaJetpack"))
					{
						flag = true;
					}
					else if (storedPart.partName.Equals("evaChute"))
					{
						flag2 = true;
					}
				}
			}
			if (moduleInventoryPartReference.InventoryItemCount == 1)
			{
				if (flag2)
				{
					if (flag3)
					{
						ChuteStTransform.gameObject.SetActive(value: true);
						if (evaChute != null)
						{
							evaChute.SetCanopy(ChuteStTransform);
						}
					}
					else
					{
						BackpackStTransform.gameObject.SetActive(value: true);
					}
				}
				else
				{
					BackpackStTransform.gameObject.SetActive(!flag);
				}
			}
			else if (flag)
			{
				if (flag2 && flag3)
				{
					ChuteJetpackTransform.gameObject.SetActive(value: true);
					if (evaChute != null)
					{
						evaChute.SetCanopy(ChuteJetpackTransform);
					}
				}
				else
				{
					BackpackTransform.gameObject.SetActive(value: true);
				}
			}
			else if (flag2 && flag3)
			{
				StorageSlimTransform.gameObject.SetActive(value: true);
				ChuteContainerTransform.gameObject.SetActive(value: true);
				if (evaChute != null)
				{
					evaChute.SetCanopy(ChuteContainerTransform);
				}
			}
			else
			{
				StorageTransform.gameObject.SetActive(value: true);
			}
		}
		ProcessEVAFuel();
		JetpackTransform.gameObject.SetActive(flag);
		HasJetpack = flag;
		if (!flag)
		{
			ToggleJetpack(packState: false);
		}
		HasParachute = flag2;
		if (evaChute != null)
		{
			evaChute.SetEVAChuteActive(flag2 && flag3);
		}
		base.Fields["thrustPercentage"].guiActive = flag;
	}

	public void SetupEVAFuel()
	{
		if (base.part.Resources.Contains(propellantResourceName))
		{
			if (moduleInventoryPartReference.InventoryItemCount > 0)
			{
				for (int i = 0; i < moduleInventoryPartReference.storedParts.Count; i++)
				{
					StoredPart storedPart = moduleInventoryPartReference.storedParts.ValuesList[i];
					if (storedPart == null || !storedPart.partName.Equals("evaJetpack"))
					{
						continue;
					}
					for (int j = 0; j < storedPart.snapshot.resources.Count; j++)
					{
						if (storedPart.snapshot.resources[j].resourceName == propellantResourceName)
						{
							storedPart.snapshot.resources[j].amount = base.part.Resources[propellantResourceName].amount;
							break;
						}
					}
				}
			}
			base.part.Resources.Remove(propellantResourceName);
		}
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(propellantResourceName);
		if (definition != null)
		{
			propellantResource = base.part.AddResource(definition.Config);
			base.part.Resources.Remove(propellantResourceName);
		}
		propellantResource.amount = 0.0;
		propellantResource.maxAmount = 0.0;
	}

	public void ProcessEVAFuel()
	{
		if (propellantResource == null)
		{
			SetupEVAFuel();
		}
		inventoryPropellantResources.Clear();
		propellantResource.amount = 0.0;
		propellantResource.maxAmount = 0.0;
		if (moduleInventoryPartReference.InventoryItemCount > 0)
		{
			for (int i = 0; i < moduleInventoryPartReference.storedParts.Count; i++)
			{
				StoredPart storedPart = moduleInventoryPartReference.storedParts.ValuesList[i];
				if (storedPart == null)
				{
					continue;
				}
				for (int j = 0; j < storedPart.snapshot.resources.Count; j++)
				{
					if (storedPart.snapshot.resources[j].resourceName == propellantResourceName)
					{
						ProtoPartResourceSnapshot protoPartResourceSnapshot = storedPart.snapshot.resources[j];
						propellantResource.maxAmount += protoPartResourceSnapshot.maxAmount;
						propellantResource.amount += protoPartResourceSnapshot.amount;
						ResourceListItem resourceListItem = new ResourceListItem();
						resourceListItem.pPResourceSnapshot = protoPartResourceSnapshot;
						resourceListItem.priority = storedPart.snapshot.resourcePriorityOffset;
						inventoryPropellantResources.Add(resourceListItem);
					}
				}
			}
		}
		inventoryPropellantResources.Sort((ResourceListItem x, ResourceListItem y) => x.priority.CompareTo(y.priority));
	}

	public void OnDrawGizmosSelected()
	{
		if (base.part != null)
		{
			Vector3 center = base.part.partTransform.TransformPoint(PartGeometryUtil.FindBoundsCentroid(base.part.GetRendererBounds(), base.part.partTransform)) + base.part.partTransform.rotation * base.part.boundsCentroidOffset;
			Gizmos.color = XKCDColors.Red;
			Gizmos.DrawWireSphere(center, 0.15f);
			Gizmos.DrawSphere(center, 0.1f);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = node.GetValue("isCfg");
		if (!string.IsNullOrEmpty(value) && bool.Parse(value))
		{
			loadedFromSFS = false;
		}
		else
		{
			loadedFromSFS = true;
			Debug.Log("[KerbalEVA]: Loaded, added rigidbody", base.gameObject);
			base.gameObject.AddComponent<Rigidbody>();
			if (node.HasValue("state"))
			{
				loadedStateName = node.GetValue("state");
			}
			if (node.HasValue("step"))
			{
				lastBoundStep = float.Parse(node.GetValue("step"));
			}
		}
		if (node.HasValue("packExt"))
		{
			JetpackDeployed = bool.Parse(node.GetValue("packExt"));
		}
		if (node.HasValue("lightOn"))
		{
			lampOn = bool.Parse(node.GetValue("lightOn"));
		}
		if (node.HasValue("flags"))
		{
			flagItems = int.Parse(node.GetValue("flags"));
		}
		else
		{
			flagItems = _flags;
		}
		if (node.HasNode("vInfo"))
		{
			if (kerbalVesselInfo == null)
			{
				kerbalVesselInfo = new DockedVesselInfo();
			}
			kerbalVesselInfo.Load(node.GetNode("vInfo"));
		}
		if (node.HasValue("useGlobalPhysicMaterial"))
		{
			useGlobalPhysicMaterial = bool.Parse(node.GetValue("useGlobalPhysicMaterial"));
		}
		else
		{
			useGlobalPhysicMaterial = true;
		}
		if (!useGlobalPhysicMaterial)
		{
			physicMaterial = new PhysicMaterial();
			float value2 = PhysicsGlobals.KerbalEVADynamicFriction;
			if (node.TryGetValue("kerbalEVADynamicFriction", ref value2))
			{
				physicMaterial.dynamicFriction = value2;
			}
			float value3 = PhysicsGlobals.KerbalEVAStaticFriction;
			if (node.TryGetValue("kerbalEVAStaticFriction", ref value3))
			{
				physicMaterial.staticFriction = value3;
			}
			float value4 = PhysicsGlobals.KerbalEVABounciness;
			if (node.TryGetValue("kerbalEVABounciness", ref value4))
			{
				physicMaterial.bounciness = value4;
			}
			PhysicMaterialCombine value5 = PhysicsGlobals.KerbalEVAFrictionCombine;
			node.TryGetEnum("kerbalEVAFrictionCombine", ref value5, PhysicsGlobals.KerbalEVAFrictionCombine);
			PhysicMaterialCombine value6 = PhysicsGlobals.KerbalEVABounceCombine;
			node.TryGetEnum("kerbalEVABounceCombine", ref value6, PhysicsGlobals.KerbalEVABounceCombine);
			SetPhysicMaterial(physicMaterial);
		}
	}

	public override void OnStart(StartState state)
	{
		isEnabled = true;
		GameEvents.onVesselGoOnRails.Add(OnVesselGoOnRails);
		GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
		GameEvents.onKrakensbaneEngage.Add(onFrameVelocityChange);
		GameEvents.onKrakensbaneDisengage.Add(onFrameVelocityChange);
		GameEvents.onRotatingFrameTransition.Add(onRotatingFrameChanged);
		GameEvents.onDominantBodyChange.Add(onReferencebodyChanged);
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
		GameEvents.OnHelmetChanged.Add(OnHelmetChanged);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.OnROCExperimentStored.Add(OnROCExperimentFinished);
		GameEvents.OnROCExperimentReset.Add(OnROCExperimentReset);
		GameEvents.onPartDie.Add(OnPartEvent);
		GameEvents.OnVisorRaised.Add(OnVisorRaised);
		GameEvents.OnEVAConstructionWeldStart.Add(OnWeldStart);
		GameEvents.OnEVAConstructionWeldFinish.Add(OnWeldFinish);
		suitCombos = GameDatabase.Instance.GetComponent<SuitCombos>();
		if (!base.part.Modules.Contains("ModuleTripLogger"))
		{
			base.part.AddModule("ModuleTripLogger");
		}
		base.part.waterAngularDragMultiplier = waterAngularDragMultiplier;
		base.part.buoyancy = PhysicsGlobals.BuoyancyKerbals;
		if (PhysicsGlobals.Instance != null)
		{
			base.part.DragCubes.LoadCubes(PhysicsGlobals.KerbalEVADragCube);
		}
		moduleInventoryPartReference = GetComponent<ModuleInventoryPart>();
		evaChute = GetComponent<ModuleEvaChute>();
		GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
		ModuleLiftingSurface[] components = GetComponents<ModuleLiftingSurface>();
		bool flag = evaChute == null || (evaChute != null && evaChute.deploymentState != ModuleParachute.deploymentStates.DEPLOYED);
		if (components.Length != 0 && moduleInventoryPartReference != null && !moduleInventoryPartReference.InventoryIsEmpty)
		{
			flag = true;
		}
		for (int i = 0; i < components.Length; i++)
		{
			if (flag)
			{
				chuteLiftingSurfaces.Add(components[i]);
				chuteLiftingSurfaces[i].moduleIsEnabled = false;
				chuteLiftingSurfaces[i].enabled = false;
			}
		}
		StartCoroutine(StartEVA(state));
		myExpressionSystem = base.gameObject.GetComponent<kerbalExpressionSystem>();
		ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
		if (protoCrewMember.SuitTexturePath == null)
		{
			SetDefaultTextures(protoCrewMember, helmetMesh.material, bodyMesh.material, (neckringMesh != null) ? neckringMesh.material : null);
		}
		else
		{
			SetNewTextures(protoCrewMember, helmetMesh.material, bodyMesh.material, (neckringMesh != null) ? neckringMesh.material : null);
		}
		InitHelmetSetup();
		UnityEngine.Random.InitState(base.part.protoModuleCrew[0].name.GetHashCode_Net35());
		KerbalAvatarUpdateInterval = UnityEngine.Random.Range(0.1f, 0.15f);
		updIntervalYield = new WaitForSeconds(KerbalAvatarUpdateInterval);
		AvatarTexture = new RenderTexture(256, 256, 24, RenderTextureFormat.RGB565);
		if (FlightGlobals.ActiveVessel.id == base.part.vessel.id)
		{
			isActiveVessel = true;
		}
		if (kerbalPortraitCamera != null && GameSettings.EVA_SHOW_PORTRAIT)
		{
			kerbalPortraitCamera.targetTexture = AvatarTexture;
			kerbalPortraitCamera.clearFlags = CameraClearFlags.Depth;
			kerbalPortraitCamera.backgroundColor = Color.black;
			if (kerbalCam00 != null)
			{
				kerbalCam00.targetTexture = AvatarTexture;
				kerbalCam00.clearFlags = CameraClearFlags.Depth;
				kerbalCam00.backgroundColor = Color.black;
			}
			if (kerbalCamSkyBox != null)
			{
				kerbalCamSkyBox.targetTexture = AvatarTexture;
				kerbalCamSkyBox.clearFlags = CameraClearFlags.Color;
				kerbalCamSkyBox.backgroundColor = Color.black;
			}
			if (kerbalCamAtmos != null)
			{
				kerbalCamAtmos.targetTexture = AvatarTexture;
				kerbalCamAtmos.clearFlags = CameraClearFlags.Depth;
				kerbalCamAtmos.backgroundColor = Color.black;
				if ((bool)ScaledCamera.Instance)
				{
					kerbalCamAtmos.transform.SetParent(ScaledCamera.Instance.transform);
					kerbalCamAtmos.transform.localPosition = Vector3.zero;
				}
			}
			if (kerbalCam01 != null)
			{
				kerbalCam01.targetTexture = AvatarTexture;
				kerbalCam01.clearFlags = CameraClearFlags.Depth;
				kerbalCam01.backgroundColor = Color.black;
			}
			updateAvatarCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
		}
		moduleScienceExperiments = base.part.FindModulesImplementing<ModuleScienceExperiment>();
		for (int j = 0; j < moduleScienceExperiments.Count; j++)
		{
			if (moduleScienceExperiments[j].experimentID == "ROCScience")
			{
				moduleScienceExperimentROC = moduleScienceExperiments[j];
			}
		}
		if (moduleScienceExperimentROC != null)
		{
			moduleScienceExperimentROC.DeployEventDisabled = true;
		}
		suitColorChanger = base.part.FindModuleImplementing<ModuleColorChanger>();
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			if (protoCrewMember != null)
			{
				lightR = protoCrewMember.lightR;
				lightG = protoCrewMember.lightG;
				lightB = protoCrewMember.lightB;
			}
			if (suitColorChanger == null)
			{
				base.Fields["lightR"].guiActive = false;
				base.Fields["lightG"].guiActive = false;
				base.Fields["lightB"].guiActive = false;
			}
			else
			{
				base.Fields["lightR"].OnValueModified += UpdateSuitColors;
				base.Fields["lightG"].OnValueModified += UpdateSuitColors;
				base.Fields["lightB"].OnValueModified += UpdateSuitColors;
				UpdateSuitColors(null);
			}
		}
		else
		{
			base.Fields["lightR"].guiActive = false;
			base.Fields["lightG"].guiActive = false;
			base.Fields["lightB"].guiActive = false;
		}
		SetupEVAScienceSoundFX();
		SetupSoundFX();
	}

	public void SetupSoundFX()
	{
		weldingLaserGroup = base.part.findFxGroup("weldingLaser");
		if (weldingLaserGroup != null)
		{
			weldingLaserGroup.setActive(value: false);
			if (weldingLasersFX == null)
			{
				weldingLasersFX = base.gameObject.AddComponent<AudioSource>();
			}
			weldingLasersFX.playOnAwake = false;
			weldingLasersFX.loop = false;
			weldingLasersFX.rolloffMode = AudioRolloffMode.Linear;
			weldingLasersFX.dopplerLevel = 0f;
			weldingLasersFX.volume = GameSettings.SHIP_VOLUME;
			weldingLasersFX.spatialBlend = 1f;
			weldingLaserGroup.begin(weldingLasersFX);
		}
	}

	public override void OnStartFinished(StartState state)
	{
		base.OnStartFinished(state);
		UpdatePackModels();
	}

	public void SetNewTextures(ProtoCrewMember pCrew, Material helmetMaterial, Material bodyMaterial, Material neckringMaterial)
	{
		Vector2 scaleUpdate = new Vector2(1f, -1f);
		UpdateTextureScale(helmetMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		UpdateTextureScale(bodyMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		if (neckringMaterial != null)
		{
			UpdateTextureScale(neckringMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		}
		Texture texture = GameDatabase.Instance.GetTexture(pCrew.SuitTexturePath, asNormalMap: false);
		Texture texture2 = GameDatabase.Instance.GetTexture(pCrew.NormalTexturePath, asNormalMap: true);
		helmetMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Helmet, helmetMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
		helmetMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Normal, helmetMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		bodyMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Body, bodyMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
		bodyMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Normal, bodyMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		if (neckringMaterial != null)
		{
			neckringMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Body, neckringMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
			neckringMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(pCrew, SuitCombo.TextureTarget.Normal, neckringMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		}
	}

	public void SetDefaultTextures(ProtoCrewMember pCrew, Material helmetMaterial, Material bodyMaterial, Material neckringMaterial)
	{
		Vector2 scaleUpdate = new Vector2(1f, 1f);
		UpdateTextureScale(helmetMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		UpdateTextureScale(bodyMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		if (neckringMaterial != null)
		{
			UpdateTextureScale(neckringMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		}
		List<SuitCombo> stockCombos = GameDatabase.Instance.GetComponent<SuitCombos>().StockCombos;
		int num = 0;
		while (true)
		{
			if (num < stockCombos.Count)
			{
				if (pCrew.ComboId == stockCombos[num].name)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		helmetMaterial.mainTexture = stockCombos[num].defaultSuitTexture;
		helmetMaterial.SetTexture("_BumpMap", stockCombos[num].defaultNormalTexture);
		bodyMaterial.mainTexture = stockCombos[num].defaultSuitTexture;
		bodyMaterial.SetTexture("_BumpMap", stockCombos[num].defaultNormalTexture);
		if (neckringMaterial != null)
		{
			neckringMaterial.mainTexture = stockCombos[num].defaultSuitTexture;
			neckringMaterial.SetTexture("_BumpMap", stockCombos[num].defaultNormalTexture);
		}
	}

	public void UpdateTextureScale(Material suitMaterial, Vector2 scaleUpdate, SuitCombo.MaterialProperty materialProperty)
	{
		switch (materialProperty)
		{
		case SuitCombo.MaterialProperty.BumpMap:
			suitMaterial.SetTextureScale("_BumpMap", scaleUpdate);
			break;
		case SuitCombo.MaterialProperty.MainTex:
			suitMaterial.mainTextureScale = scaleUpdate;
			break;
		case SuitCombo.MaterialProperty.All:
			suitMaterial.mainTextureScale = scaleUpdate;
			suitMaterial.SetTextureScale("_BumpMap", scaleUpdate);
			break;
		}
	}

	public virtual IEnumerator StartEVA(StartState state)
	{
		advRagdoll = GetComponent<AdvancedRagdoll>() ?? base.gameObject.AddComponent<AdvancedRagdoll>();
		advRagdoll.ragdollRoot = base.transform;
		if (HighLogic.LoadedSceneIsFlight)
		{
			base.part.partInfo = new AvailablePart(base.part.partInfo);
			base.part.partInfo.name = ((base.part.protoModuleCrew[0] != null) ? base.part.protoModuleCrew[0].GetKerbalEVAPartName() : "kerbalEVA");
			base.part.partInfo.title = ((base.part.protoModuleCrew[0] != null) ? base.part.protoModuleCrew[0].name : Localizer.Format("#autoLOC_112486"));
		}
		if (kerbalVesselInfo == null)
		{
			kerbalVesselInfo = new DockedVesselInfo();
			kerbalVesselInfo.name = base.vessel.vesselName;
			kerbalVesselInfo.vesselType = VesselType.const_11;
			kerbalVesselInfo.rootPartUId = base.part.flightID;
		}
		SetupAnimations();
		SetupRagdoll(base.part);
		ladderPushoffForce *= massMultiplier;
		maxJumpForce *= massMultiplier;
		boundForce *= massMultiplier;
		linPower *= massMultiplier;
		ResetRagdollLinks();
		SetRagdoll(ragDoll: false);
		base.Events["PlantFlag"].active = false;
		base.Events["OnDeboardSeat"].active = false;
		base.Events["MakeReference"].active = false;
		base.Events["RenameVessel"].active = false;
		SetupFSM();
		SetupJetpackEffects();
		yield return null;
		if (loadedFromSFS && loadedStateName != string.Empty)
		{
			if (loadedStateName.Contains("Ladder"))
			{
				if (currentLadderTriggers.Count > 0)
				{
					loadedStateName = st_ladder_acquire.name;
				}
				else
				{
					loadedStateName = st_idle_fl.name;
				}
			}
			try
			{
				fsm.StartFSM(loadedStateName);
				Ready = true;
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message, base.gameObject);
				Ready = false;
			}
		}
		if (!Ready)
		{
			if (currentLadderTriggers.Count > 0 && autoGrabLadderOnStart)
			{
				fsm.StartFSM(st_ladder_acquire);
			}
			else if (SurfaceContact())
			{
				fsm.StartFSM(st_idle_gr);
			}
			else if (base.vessel.Splashed)
			{
				fsm.StartFSM(st_swim_idle);
			}
			else
			{
				fsm.StartFSM(st_idle_fl);
			}
			Ready = true;
		}
		KerbalFSM kerbalFSM = fsm;
		kerbalFSM.OnStateChange = (Callback<KFSMState, KFSMState, KFSMEvent>)Delegate.Combine(kerbalFSM.OnStateChange, new Callback<KFSMState, KFSMState, KFSMEvent>(UpdateInventoryPaw));
		ToggleJetpack(JetpackDeployed);
		headLamp.SetActive(lampOn);
		base.Fields["colorChanger"].guiActive = base.vessel.GetVesselCrew()[0].suit == ProtoCrewMember.KerbalSuit.Future;
		ResetOrientationPID();
		while (!base.part.started)
		{
			yield return null;
		}
		if ((bool)base.part.collisionEnhancer)
		{
			base.part.collisionEnhancer.OnTerrainPunchThrough = CollisionEnhancerBehaviour.TRANSLATE_BACK;
		}
		base.part.SetReferenceTransform(referenceTransform);
	}

	public override void OnSave(ConfigNode node)
	{
		if (fsm.Started)
		{
			node.AddValue("state", fsm.currentStateName);
			kerbalVesselInfo.Save(node.AddNode("vInfo"));
		}
		if (evaChute != null && base.part.protoModuleCrew.Count > 0)
		{
			base.part.protoModuleCrew[0].SaveEVAChute(evaChute);
		}
		if (moduleInventoryPartReference != null && base.part.protoModuleCrew.Count > 0)
		{
			base.part.protoModuleCrew[0].SaveInventory(moduleInventoryPartReference);
			if ((bool)KerbalInventoryScenario.Instance)
			{
				KerbalInventoryScenario.Instance.RemoveKerbalInventoryInstance(base.part.protoModuleCrew[0].name);
			}
		}
		if (!useGlobalPhysicMaterial)
		{
			node.AddValue("kerbalEVADynamicFriction", physicMaterial.dynamicFriction);
			node.AddValue("kerbalEVAStaticFriction", physicMaterial.staticFriction);
			node.AddValue("kerbalEVABounciness", physicMaterial.bounciness);
			node.AddValue("kerbalEVAFrictionCombine", physicMaterial.frictionCombine);
			node.AddValue("kerbalEVABounceCombine", physicMaterial.bounceCombine);
		}
	}

	public void OnVesselChange(Vessel vsl)
	{
		isActiveVessel = vsl.id == base.vessel.id;
		if (isActiveVessel)
		{
			if (updateAvatarCoroutine == null && GameSettings.EVA_SHOW_PORTRAIT)
			{
				updateAvatarCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
			}
		}
		else if (updateAvatarCoroutine != null)
		{
			StopCoroutine(updateAvatarCoroutine);
			updateAvatarCoroutine = null;
		}
	}

	public void OnPartEvent(Part part)
	{
		if (currentAirlockPart != null && part.persistentId == currentAirlockPart.persistentId)
		{
			currentAirlockPart = null;
		}
	}

	public void UpdateInventoryPaw(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		if (moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.KerbalStateChanged();
		}
	}

	public void SetPortraitCameraDistance(float newDistance)
	{
		if (kerbalPortraitCamera != null && GameSettings.EVA_SHOW_PORTRAIT)
		{
			cameraPosition = kerbalPortraitCamera.transform.localPosition;
			cameraPosition.z = newDistance;
			kerbalPortraitCamera.transform.localPosition = cameraPosition;
		}
	}

	public virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		while (isActiveVessel)
		{
			if (kerbalPortraitCamera != null)
			{
				try
				{
					RenderTexture.active = AvatarTexture;
					kerbalCamSkyBox.targetTexture = AvatarTexture;
					kerbalCamSkyBox.Render();
					kerbalCamAtmos.targetTexture = AvatarTexture;
					kerbalCamAtmos.Render();
					kerbalCam01.targetTexture = AvatarTexture;
					kerbalCam01.Render();
					kerbalCam00.targetTexture = AvatarTexture;
					kerbalCam00.Render();
					kerbalPortraitCamera.targetTexture = AvatarTexture;
					kerbalPortraitCamera.Render();
					RenderTexture.active = null;
				}
				catch (Exception ex)
				{
					Debug.LogWarning(ex.Message);
				}
			}
			yield return updIntervalYield;
		}
	}

	public virtual void SetVisibleInPortrait(bool visible)
	{
		if (visible)
		{
			visibleInPortrait = true;
			if (updateAvatarCoroutine == null)
			{
				updateAvatarCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
			}
			return;
		}
		visibleInPortrait = false;
		if (updateAvatarCoroutine != null)
		{
			StopCoroutine(updateAvatarCoroutine);
			updateAvatarCoroutine = null;
		}
	}

	[ContextMenu("Reconfigure Animations")]
	public virtual void SetupAnimations()
	{
		Animations.Start(this);
	}

	public virtual void idle_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.idle, 0.2f, PlayMode.StopSameLayer);
		tgtSpeed = 0f;
		ModifyBodyColliderHeight(0f);
		CalculateGroundLevelAngle();
		SetPortraitCameraDistance(standardCameraDistance);
		base.Events["PlantFlag"].active = CanPlantFlag();
		if (evaChute != null)
		{
			evaChute.AllowRepack(allowRepack: true);
		}
		if (!JetpackDeployed && !PartPlacementMode)
		{
			newidleCounter = newidleTime;
			startTimer = true;
		}
	}

	public virtual void idle_OnLeave(KFSMState st)
	{
		newidleCounter = newidleTime;
		startTimer = false;
		lastTgtSpeed = 0f;
		base.Events["PlantFlag"].active = false;
	}

	public virtual void idle_b_OnEnter(KFSMState st)
	{
		idleAnimationsIndex = 0;
		idleAnimationsIndex = UnityEngine.Random.Range(0, Animations.RandomIdleAnims.Count);
		if (!JetpackDeployed && !alternateIdleDisabled)
		{
			if ((Animations.RandomIdleAnims[idleAnimationsIndex].State.name == "idle_c" || Animations.RandomIdleAnims[idleAnimationsIndex].State.name == "idle_c_02") && !isHelmetEnabled)
			{
				idleAnimationsIndex++;
			}
			this.GetComponentCached(ref _animation).CrossFade(Animations.RandomIdleAnims[idleAnimationsIndex], 0f, PlayMode.StopAll);
			if (myExpressionSystem == null)
			{
				myExpressionSystem = base.gameObject.GetComponent<kerbalExpressionSystem>();
			}
			if (myExpressionSystem != null)
			{
				myExpressionSystem.startExpressionTime = Animations.RandomIdleAnims[idleAnimationsIndex].startExpressionTime;
				myExpressionSystem.newIdleWheeLevel = Animations.RandomIdleAnims[idleAnimationsIndex].wheeLevel;
				myExpressionSystem.newIdleFearFactor = Animations.RandomIdleAnims[idleAnimationsIndex].fearFactor;
				startExpression = myExpressionSystem.SetNewIdleExpression();
				StartCoroutine(startExpression);
			}
			tgtSpeed = 0f;
			base.Events["PlantFlag"].active = CanPlantFlag();
			if (evaChute != null)
			{
				evaChute.AllowRepack(allowRepack: true);
			}
			float num = 0f;
			num = Animations.RandomIdleAnims[idleAnimationsIndex].State.length - Animations.RandomIdleAnims[idleAnimationsIndex].CutAnimationTime;
			returnIDLE = ReturnToIdle(num);
			StopCoroutine(returnIDLE);
			StartCoroutine(returnIDLE);
			base.vessel.UpdateLandedSplashed();
		}
		else
		{
			myExpressionSystem.useNewIdleExpressions = false;
			fsm.RunEvent(On_return_idle);
		}
	}

	public virtual void idle_b_OnLeave(KFSMState st)
	{
		if (returnIDLE != null)
		{
			StopCoroutine(returnIDLE);
		}
		if (startExpression != null)
		{
			StopCoroutine(startExpression);
		}
		if (myExpressionSystem != null)
		{
			myExpressionSystem.useNewIdleExpressions = false;
		}
		lastTgtSpeed = 0f;
		base.Events["PlantFlag"].active = false;
	}

	public virtual void walk_Acd_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.walkFwd, 0.2f, PlayMode.StopSameLayer);
		this.GetComponentCached(ref _animation).Blend(Animations.walkLowGee, Mathf.InverseLerp(1f, minWalkingGee, (float)base.vessel.mainBody.GeeASL));
		Animations.walkLowGee.State.speed = 2.7f;
		tgtSpeed = walkSpeed;
	}

	public virtual void walk_ccd_OnLeave(KFSMState st)
	{
		lastTgtSpeed = walkSpeed;
	}

	public virtual void walk_fps_OnUpdate()
	{
		float num = Vector3.Dot(tgtRpos, base.transform.forward);
		float num2 = Mathf.Clamp01(num);
		float num3 = Mathf.Clamp01(0f - num);
		float num4 = Vector3.Dot(tgtRpos, base.transform.right);
		float num5 = Mathf.Clamp01(num4);
		float num6 = Mathf.Clamp01(0f - num4);
		tgtSpeed = walkSpeed * (num2 + num3) + strafeSpeed * (num6 + num5);
		if (num2 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.walkFwd, 0.2f);
			this.GetComponentCached(ref _animation).Blend(Animations.walkLowGee, Mathf.InverseLerp(1f, minWalkingGee, (float)base.vessel.mainBody.GeeASL));
			Animations.walkLowGee.State.speed = 2.7f;
			if (num5 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeRight, num5);
			}
			else if (num6 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeLeft, num6);
			}
		}
		else if (num3 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.walkBack, 0.2f);
			if (num5 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeRight, num5);
			}
			else if (num6 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeLeft, num6);
			}
		}
		else if (num5 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.strafeRight, 0.2f);
		}
		else if (num6 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.strafeLeft, 0.2f);
		}
	}

	public virtual void walk_fps_OnLeave(KFSMState st)
	{
		lastTgtSpeed = tgtSpeed;
	}

	public virtual void heading_acquire_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade((deltaHdg > 0f) ? Animations.turnRight : Animations.turnLeft, 0.3f, PlayMode.StopSameLayer);
	}

	public virtual void heading_acquire_OnLeave(KFSMState st)
	{
		lastTgtSpeed = (float)base.vessel.horizontalSrfSpeed;
	}

	public virtual void run_acd_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.run);
		this.GetComponentCached(ref _animation).Blend(Animations.walkLowGee, Mathf.InverseLerp(1f, minWalkingGee, (float)base.vessel.mainBody.GeeASL));
		Animations.walkLowGee.State.speed = 2.7f;
		SetPortraitCameraDistance(runningCameraDistance);
		tgtSpeed = runSpeed;
	}

	public virtual void run_acd_OnLeave(KFSMState st)
	{
		lastTgtSpeed = runSpeed;
	}

	public virtual void run_fps_OnUpdate()
	{
		float num = Vector3.Dot(tgtRpos, base.transform.forward);
		float num2 = Mathf.Clamp01(num);
		float num3 = Mathf.Clamp01(0f - num);
		float num4 = Vector3.Dot(tgtRpos, base.transform.right);
		float num5 = Mathf.Clamp01(num4);
		float num6 = Mathf.Clamp01(0f - num4);
		tgtSpeed = runSpeed * num2 + walkSpeed * num3 + strafeSpeed * (num6 + num5);
		if (num2 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.run, 0.2f);
			this.GetComponentCached(ref _animation).Blend(Animations.walkLowGee, Mathf.InverseLerp(1f, minWalkingGee, (float)base.vessel.mainBody.GeeASL));
			Animations.walkLowGee.State.speed = 2.7f;
			if (num5 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeRight, num5);
			}
			else if (num6 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeLeft, num6);
			}
		}
		else if (num3 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.walkBack, 0.2f);
			if (num5 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeRight, num5);
			}
			else if (num6 > 0f)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.strafeLeft, num6);
			}
		}
		else if (num5 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.strafeRight, 0.2f);
		}
		else if (num6 > 0.01f)
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.strafeLeft, 0.2f);
		}
	}

	public virtual void run_fps_OnLeave(KFSMState st)
	{
		lastTgtSpeed = runSpeed;
	}

	public virtual void bound_gr_acd_OnEnter(KFSMState st)
	{
		if (st != st_bound_fl)
		{
			boundColliderModifierCounter = 0f;
			tmpBoundState = this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.walkLowGee, boundAttack, QueueMode.PlayNow, PlayMode.StopSameLayer);
		}
		tgtSpeed = boundSpeed;
		CalculateGroundLevelAngle();
		lastBoundStep = Mathf.Clamp(lastBoundStep, boundFrequency, 100f);
		boundStepSpeed = Mathf.Clamp(Animations.walkLowGee.State.length * 0.5f / (lastBoundStep + boundFrequency), 0f, 1f);
		if ((bool)tmpBoundState)
		{
			tmpBoundState.speed = boundStepSpeed;
		}
		tgtBoundStep = (boundLeftFoot ? Animations.walkLowGee.end : Animations.walkLowGee.start);
	}

	public virtual void bound_gr_acd_OnUpdate()
	{
		boundColliderModifierCounter += Time.deltaTime;
	}

	public virtual void bound_gr_acd_OnLeave(KFSMState st)
	{
		lastTgtSpeed = boundSpeed;
	}

	public virtual void bound_gr_fps_OnEnter(KFSMState st)
	{
		boundForce = Mathf.Round(base.part.mass * boundForceMassFactor * 100f) / 100f;
		tgtBoundStep = (boundLeftFoot ? Animations.walkLowGee.end : Animations.walkLowGee.start);
		boundLeftFoot = !boundLeftFoot;
	}

	public virtual void bound_gr_fps_OnUpdate()
	{
		float num = Vector3.Dot(tgtRpos, base.transform.forward);
		float num2 = Mathf.Clamp01(num);
		float num3 = Mathf.Clamp01(0f - num);
		float num4 = Vector3.Dot(tgtRpos, base.transform.right);
		float num5 = Mathf.Clamp01(num4);
		float num6 = Mathf.Clamp01(0f - num4);
		tgtSpeed = boundSpeed * (num2 + num3) + strafeSpeed * (num6 + num5);
		AnimationState animationState = null;
		AnimationState animationState2 = null;
		if (num2 > 0.01f)
		{
			animationState = Animations.walkLowGee.State;
			if (num5 > 0f)
			{
				animationState2 = Animations.strafeRight.State;
			}
			else if (num6 > 0f)
			{
				animationState2 = Animations.strafeLeft.State;
			}
		}
		else if (num3 > 0.01f)
		{
			animationState = Animations.walkBack.State;
			if (num5 > 0f)
			{
				animationState2 = Animations.strafeRight.State;
			}
			else if (num6 > 0f)
			{
				animationState2 = Animations.strafeLeft.State;
			}
		}
		else if (num5 > 0.01f)
		{
			animationState = Animations.strafeRight.State;
		}
		else if (num6 > 0.01f)
		{
			animationState = Animations.strafeLeft.State;
		}
		if (animationState != null)
		{
			this.GetComponentCached(ref _animation).CrossFade(animationState.name, boundAttack * 0.5f, PlayMode.StopSameLayer);
			boundStepSpeed = Mathf.Clamp(animationState.length * 0.5f / lastBoundStep, 0f, 1f);
			animationState.speed = boundStepSpeed;
			if (animationState2 != null)
			{
				this.GetComponentCached(ref _animation).Blend(animationState2.name, boundAttack * 0.5f);
				animationState2.normalizedSpeed = animationState.normalizedSpeed;
				animationState2.normalizedTime = animationState.normalizedTime;
			}
		}
	}

	public virtual void bound_gr_fps_OnLeave(KFSMState st)
	{
		if (st != st_bound_fl)
		{
			boundLeftFoot = false;
		}
		lastTgtSpeed = boundSpeed;
	}

	public virtual void bound_fl_OnEnter(KFSMState st)
	{
		if (st == st_bound_gr_fps)
		{
			this.GetComponentCached(ref _animation).CrossFade(boundLeftFoot ? Animations.walkLowGeeSuspendedLeft : Animations.walkLowGeeSuspendedRight, lastBoundStep * boundRelease, PlayMode.StopSameLayer);
		}
	}

	public virtual void bound_fl_OnLeave(KFSMState st)
	{
		lastTgtSpeed = (float)base.vessel.horizontalSrfSpeed;
		lastBoundStep = (float)fsm.TimeAtCurrentState;
	}

	public virtual void ragdoll_OnEnter(KFSMState st)
	{
		if (!base.vessel.packed)
		{
			SetRagdoll(ragDoll: true);
		}
		isRagdoll = true;
		lastCollisionTime = 0.0;
		if (InConstructionMode && (bool)EVAConstructionModeController.Instance)
		{
			EVAConstructionModeController.Instance.ClosePanel();
			weldFX.Stop();
			alternateIdleDisabled = false;
			ToggleWeldingGun(toggle: false);
			InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
		}
		SetPortraitCameraDistance(ragdollCameraDistance);
		currentLadder = null;
		if (currentLadderPart != null)
		{
			currentLadderPart.hasKerbalOnLadder = false;
		}
		currentLadderPart = null;
		base.vessel.UpdateLandedSplashed();
		secondaryLadder = null;
		currentLadderTriggers.Clear();
		if (evaChute != null)
		{
			evaChute.AllowRepack(allowRepack: false);
		}
	}

	public virtual void ragdoll_OnLeave(KFSMState st)
	{
		SetRagdoll(ragDoll: false);
		canRecover = false;
		SetPortraitCameraDistance(standardCameraDistance);
		if (evaChute != null && base.vessel.LandedOrSplashed)
		{
			evaChute.AllowRepack(allowRepack: true);
		}
	}

	public virtual void recover_OnEnter(KFSMState st)
	{
		if (isRagdoll)
		{
			advRagdoll.SynchRagdollOut(base.transform);
		}
		if (Vector3.Dot(base.transform.forward, fUp) > 0f)
		{
			if (base.vessel.Splashed && !SurfaceContact())
			{
				this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.swimUpFaceUp, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
				On_recover_complete.TimerDuration = Animations.swimUpFaceUp.State.length - 0.6f;
			}
			else if (SurfaceContact())
			{
				this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.standUpFaceUp, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
				On_recover_complete.TimerDuration = Animations.standUpFaceUp.State.length - 0.6f;
			}
			else
			{
				this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.suspendedIdle, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
				On_recover_complete.TimerDuration = Animations.suspendedIdle.State.length - 0.6f;
			}
		}
		else if (base.vessel.Splashed && !SurfaceContact())
		{
			this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.swimUpFaceDown, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
			On_recover_complete.TimerDuration = Animations.swimUpFaceDown.State.length - 0.6f;
		}
		else if (SurfaceContact())
		{
			this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.standUpFaceDown, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
			On_recover_complete.TimerDuration = Animations.standUpFaceDown.State.length - 0.6f;
		}
		else
		{
			this.GetComponentCached(ref _animation).CrossFadeQueued(Animations.suspendedIdle, 0.5f, QueueMode.PlayNow, PlayMode.StopSameLayer);
			On_recover_complete.TimerDuration = Animations.suspendedIdle.State.length - 0.6f;
		}
		if (SurfaceOrSplashed())
		{
			StartGroundedRotationRecover();
		}
	}

	public virtual void recover_OnUpdate()
	{
		if (SurfaceOrSplashed())
		{
			RecoverGroundedRotation(0.5f);
			this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
		}
	}

	public virtual void controlPanel_identified_OnEnter(KFSMState st)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			tgtSpeed = 0f;
			SetPortraitCameraDistance(standardCameraDistance);
			Animations.controlPanelAnims[Animations.controlPanelAnimSelector].State.time = Animations.controlPanelAnims[Animations.controlPanelAnimSelector].start;
			this.GetComponentCached(ref _animation).CrossFade(Animations.controlPanelAnims[Animations.controlPanelAnimSelector], 0.2f, PlayMode.StopAll);
			sciencePanelAnimCooldown = Animations.controlPanelAnims[Animations.controlPanelAnimSelector].State.length;
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "ControlPanelLock_" + base.vessel.id);
			sciencePanelAnimPlaying = true;
			if (sciencePart != null)
			{
				UIPartActionController.Instance.SpawnPartActionWindow(sciencePart.part);
			}
		}
	}

	public virtual void picking_roc_sample_OnEnter(KFSMState st)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Animations.rockSample.State.time = Animations.rockSample.start;
			this.GetComponentCached(ref _animation).CrossFade(Animations.rockSample, 0.2f, PlayMode.StopAll);
			pickRocSampleAnimCooldown = Animations.rockSample.State.length;
			InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "ControlPanelLock_" + base.vessel.id);
			pickRocSampleAnimPlaying = true;
		}
	}

	public virtual void spawnHammer()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && !(hammerPrefab == null))
		{
			if (hammer == null)
			{
				hammer = UnityEngine.Object.Instantiate(hammerPrefab);
				hammer.transform.SetParent(hammerAnchor);
				hammer.transform.localPosition = Vector3.zero;
				hammer.transform.localRotation = Quaternion.identity;
				hammerMesh = hammerAnchor.GetComponentInChildren<MeshRenderer>();
				hammerAnimation = hammerAnchor.GetComponentInChildren<Animation>();
			}
			hammerAnimTimer += Time.deltaTime;
			if (!(hammerAnimTimer < hammerAnimation.clip.length * 0.04f) && hammerAnimTimer <= hammerAnimation.clip.length * 0.55f)
			{
				hammerMesh.enabled = true;
			}
			else
			{
				hammerMesh.enabled = false;
			}
			if (!hammerAnimation.isPlaying)
			{
				hammerAnimation.Play();
			}
		}
	}

	public virtual void jump_OnEnter(KFSMState st)
	{
		if (base.vessel.horizontalSrfSpeed < 0.20000000298023224)
		{
			On_jump_complete.TimerDuration = 0.20000000298023224;
			Animations.JumpStillStart.State.time = -0.2f;
			this.GetComponentCached(ref _animation).CrossFade(Animations.JumpStillStart, 0.2f, PlayMode.StopAll);
		}
		else
		{
			On_jump_complete.TimerDuration = Animations.JumpFwdStart.end;
			Animations.JumpFwdStart.State.time = Animations.JumpFwdStart.start;
			this.GetComponentCached(ref _animation).CrossFade(Animations.JumpFwdStart, 0.2f, PlayMode.StopAll);
		}
	}

	public virtual void idle_fl_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.suspendedIdle, 1.2f, PlayMode.StopSameLayer);
		tgtSpeed = 0f;
		currentSpd = 0f;
		ResetOrientationPID();
		if (evaChute != null)
		{
			evaChute.AllowRepack(allowRepack: false);
		}
	}

	public virtual void idle_fl_OnLeave(KFSMState st)
	{
		if (evaChute != null)
		{
			evaChute.AllowRepack(allowRepack: true);
		}
	}

	public virtual void land_OnEnter(KFSMState st)
	{
		lastTgtSpeed = (float)base.vessel.horizontalSrfSpeed;
		StartGroundedRotationRecover();
		if (base.vessel.horizontalSrfSpeed < 0.1)
		{
			Animations.JumpStillEnd.State.time = 0.1f;
			this.GetComponentCached(ref _animation).CrossFade(Animations.JumpStillEnd, 0.2f, PlayMode.StopSameLayer);
		}
		else
		{
			Animations.JumpFwdEnd.State.time = 0.3f;
			this.GetComponentCached(ref _animation).CrossFade(Animations.JumpFwdEnd, 0.2f, PlayMode.StopSameLayer);
		}
	}

	public virtual void land_OnUpdate()
	{
		RecoverGroundedRotation(0.2f);
	}

	public virtual void swim_idle_OnEnter(KFSMState st)
	{
		ToggleJetpack(packState: false);
		SetPortraitCameraDistance(swimmingCameraDistance);
		this.GetComponentCached(ref _animation).CrossFade(Animations.swimIdle, 0.5f, PlayMode.StopSameLayer);
		tgtSpeed = 0f;
		if (evaChute != null)
		{
			evaChute.AllowRepack(allowRepack: true);
		}
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
	}

	public virtual void swim_idle_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
	}

	public virtual void swim_fwd_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.swimFwd);
		tgtSpeed = swimSpeed;
	}

	public virtual void swim_fwd_OnLeave(KFSMState st)
	{
		lastTgtSpeed = swimSpeed;
	}

	public virtual void ladder_acquire_OnEnter(KFSMState st)
	{
		ToggleJetpack(packState: false);
		currentLadder = currentLadderTriggers[0];
		if (currentLadderPart != null)
		{
			currentLadderPart.hasKerbalOnLadder = false;
		}
		currentLadderPart = FlightGlobals.GetPartUpwardsCached(currentLadder.gameObject);
		if (currentLadderPart != null)
		{
			currentLadderPart.hasKerbalOnLadder = true;
		}
		invLadderAxis = Vector3.Dot(currentLadder.transform.up, base.transform.up) < 0f;
		lastTgtSpeed = 0f;
		StartCoroutine(AcquireRotation(Quaternion.LookRotation(currentLadder.transform.forward, currentLadder.transform.up * (invLadderAxis ? (-1f) : 1f)), 0.5f));
		if (SurfaceOrSplashed())
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.ladderGrabGrounded, 0.2f, PlayMode.StopAll);
		}
		else
		{
			this.GetComponentCached(ref _animation).CrossFade(Animations.ladderGrabSuspended, 0.2f, PlayMode.StopAll);
		}
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
		if (InConstructionMode)
		{
			ToggleWeldingGun(toggle: false);
		}
	}

	public virtual void ladder_acquire_OnLeave(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Blend(Animations.ladderGrabGrounded, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderGrabSuspended, 0f, 2f);
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
	}

	public virtual void ladder_idle_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.ladderIdle, 0.2f, PlayMode.StopAll);
		tgtSpeed = 0f;
		ladderTransition = false;
		onLadder = true;
		SetLadderHold();
	}

	public virtual void ladder_idle_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
		this.GetComponentCached(ref _animation).Blend(Animations.ladderIdle, 0f, 2f);
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
		ClearLadderHold();
	}

	public virtual void ladder_lean_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Play(Animations.ladderIdle, PlayMode.StopAll);
		Animations.ladderLeanFwd.State.enabled = true;
		Animations.ladderLeanBack.State.enabled = true;
		Animations.ladderLeanLeft.State.enabled = true;
		Animations.ladderLeanRight.State.enabled = true;
		Animations.ladderPushOff.State.enabled = true;
		tgtSpeed = 0f;
		ladderTransition = false;
		onLadder = true;
	}

	public virtual void ladder_lean_OnLateUpdate()
	{
		float num = Vector3.Dot(ladderTgtRPos, base.transform.up);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanFwd, Mathf.Clamp01(num));
		Animations.ladderLeanFwd.State.normalizedTime = Mathf.Lerp(Animations.ladderLeanFwd.start, Animations.ladderLeanFwd.end, Animations.ladderLeanFwd.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanBack, Mathf.Clamp01(0f - num));
		Animations.ladderLeanBack.State.normalizedTime = Mathf.Lerp(Animations.ladderLeanBack.start, Animations.ladderLeanBack.end, Animations.ladderLeanBack.State.weight);
		float num2 = Vector3.Dot(ladderTgtRPos, -base.transform.right);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanLeft, Mathf.Clamp01(num2));
		Animations.ladderLeanLeft.State.normalizedTime = Mathf.Lerp(Animations.ladderLeanLeft.start, Animations.ladderLeanLeft.end, Animations.ladderLeanLeft.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanRight, Mathf.Clamp01(0f - num2));
		Animations.ladderLeanRight.State.normalizedTime = Mathf.Lerp(Animations.ladderLeanRight.start, Animations.ladderLeanRight.end, Animations.ladderLeanRight.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderPushOff, 1f - Mathf.Clamp01(Mathf.Abs(num2) + Mathf.Abs(num)));
		Animations.ladderPushOff.State.normalizedTime = Mathf.Lerp(Animations.ladderPushOff.start, Animations.ladderPushOff.end, Animations.ladderPushOff.State.weight);
	}

	public virtual void ladder_lean_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
		if (st != st_ladder_pushoff)
		{
			this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanFwd, 0f, 2f);
			this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanBack, 0f, 2f);
			this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanLeft, 0f, 2f);
			this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanRight, 0f, 2f);
			this.GetComponentCached(ref _animation).Blend(Animations.ladderPushOff, 0f, 2f);
		}
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
	}

	public virtual void ladder_climb_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.ladderClimb, 0.2f, PlayMode.StopAll);
		tgtSpeed = ladderClimbSpeed;
		onLadder = true;
	}

	public virtual void ladder_climb_OnLeave(KFSMState st)
	{
		lastTgtSpeed = ladderClimbSpeed;
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
	}

	public virtual void ladder_descend_OnEnter(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.ladderDescend, 0.2f, PlayMode.StopAll);
		tgtSpeed = 0f - ladderClimbSpeed;
		onLadder = true;
	}

	public virtual void ladder_descend_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f - ladderClimbSpeed;
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
	}

	public virtual void ladder_end_reached_OnEnter(KFSMState st)
	{
		tgtSpeed = 0f;
		this.GetComponentCached(ref _animation).CrossFade(Animations.ladderIdle, 1f, PlayMode.StopAll);
		onLadder = true;
		SetLadderHold();
	}

	public virtual void ladder_end_reached_OnLeave(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Blend(Animations.ladderIdle, 0f, 1f);
		base.vessel.gravityMultiplier = 1.0;
		lastTgtSpeed = 0f;
		onLadder = false;
		ClearLadderHold();
	}

	public virtual void ladder_pushoff_OnLeave(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanFwd, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanBack, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanLeft, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderLeanRight, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.ladderPushOff, 0f, 2f);
		base.vessel.gravityMultiplier = 1.0;
		onLadder = false;
	}

	public virtual void clamber_acquireP1_OnEnter(KFSMState st)
	{
		currentLadder = null;
		ToggleJetpack(packState: false);
		lastTgtSpeed = 0f;
		this.GetComponentCached(ref _animation).CrossFade(Animations.clamber, 0.05f, PlayMode.StopSameLayer);
		Animations.clamber.State.normalizedTime = Animations.clamber.start;
		Animations.clamber.State.speed = Animations.clamber.speedAt1Gee;
		StartCoroutine(AcquireRotation(Quaternion.LookRotation(-clamberPath.edgeFaceNormal, fUp), Animations.clamber.State.length * 0.2f * Animations.clamber.speedAt1Gee));
		StartCoroutine(AcquirePosition(clamberPath.p1, Animations.clamber.State.length * 0.2f * Animations.clamber.speedAt1Gee));
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
	}

	public virtual void ZeroRBVelocity()
	{
		this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
	}

	public virtual void clamber_acquireP1_OnLeave(KFSMState st)
	{
		if (st != st_clamber_acquireP2)
		{
			clamberPath = null;
		}
	}

	public virtual void clamber_acquireP2_OnEnter(KFSMState st)
	{
		StartCoroutine(AcquireRotation(Quaternion.LookRotation(-clamberPath.edgeFaceNormal, fUp), Animations.clamber.State.length * 0.1f * Animations.clamber.speedAt1Gee));
		StartCoroutine(AcquirePosition(clamberPath.p2, Animations.clamber.State.length * 0.1f * Animations.clamber.speedAt1Gee));
	}

	public virtual void clamber_acquireP2_OnLeave(KFSMState st)
	{
		if (st != st_clamber_acquireP3)
		{
			clamberPath = null;
		}
	}

	public virtual void clamber_acquireP3_OnEnter(KFSMState st)
	{
		StartCoroutine(AcquireRotation(Quaternion.LookRotation(-clamberPath.edgeFaceNormal, fUp), Animations.clamber.State.length * 0.2f * Animations.clamber.speedAt1Gee));
		StartCoroutine(AcquirePosition(clamberPath.p3, Animations.clamber.State.length * 0.2f * Animations.clamber.speedAt1Gee));
	}

	public virtual void clamber_acquireP3_OnLeave(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Blend(Animations.clamber, 0f, 1f - Animations.clamber.end);
		clamberPath = null;
	}

	public virtual void flagAcquireHeading_OnEnter(KFSMState st)
	{
		tgtSpeed = 0f;
		flagSpot = FlagSite.ScanSurroundingTerrain(base.transform.position, base.transform.forward, flagReach);
		ToggleJetpack(packState: false);
		SetWaypoint(flagSpot);
		UpdateHeading();
		this.GetComponentCached(ref _animation).CrossFade((deltaHdg > 0f) ? Animations.turnRight : Animations.turnLeft, 0.3f, PlayMode.StopSameLayer);
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
	}

	public virtual void flagAcquireHeading_OnLateUpdate()
	{
		SetWaypoint(flagSpot);
	}

	public virtual void flagAcquireHeading_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
	}

	public virtual void flagPlant_OnEnter(KFSMState st)
	{
		deltaHdg = 0f;
		this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
		this.GetComponentCached(ref _animation).CrossFade(Animations.flagPlant, 0.2f, PlayMode.StopAll);
		Physics.Raycast(base.transform.position + base.transform.forward * flagReach + fUp * 10f, -fUp, out var hitInfo, 20f, 32768, QueryTriggerInteraction.Ignore);
		flagSpot = hitInfo.point;
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "FlagDeployLock_" + base.vessel.id);
		flag = FlagSite.CreateFlag(flagSpot, Quaternion.LookRotation(base.transform.forward, base.transform.up), base.part);
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
	}

	public virtual void flagPlant_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
		InputLockManager.RemoveControlLock("FlagDeployLock_" + base.vessel.id);
		if (fsm.LastEvent != On_flagPlantComplete)
		{
			flag.OnPlacementFail();
		}
		else
		{
			flag.OnPlacementComplete();
			base.part.protoModuleCrew[0].flightLog.AddEntryUnique(FlightLog.EntryType.PlantFlag, base.vessel.orbit.referenceBody.name);
			base.part.protoModuleCrew[0].UpdateExperience();
			int count = FlightGlobals.VesselsLoaded.Count;
			while (count-- > 0)
			{
				Vessel vessel = FlightGlobals.VesselsLoaded[count];
				if (!(vessel != null) || !vessel.loaded || !(vessel != FlightGlobals.ActiveVessel))
				{
					continue;
				}
				if (vessel.vesselType == VesselType.const_11)
				{
					ProtoCrewMember protoCrewMember = vessel.GetVesselCrew()[0];
					protoCrewMember.flightLog.AddEntryUnique(FlightLog.EntryType.PlantFlag, base.vessel.orbit.referenceBody.name);
					protoCrewMember.UpdateExperience();
				}
				else if (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.SPLASHED || vessel.situation == Vessel.Situations.PRELAUNCH)
				{
					List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
					int count2 = vesselCrew.Count;
					while (count2-- > 0)
					{
						ProtoCrewMember protoCrewMember2 = vesselCrew[count2];
						protoCrewMember2.flightLog.AddEntryUnique(FlightLog.EntryType.PlantFlag, base.vessel.orbit.referenceBody.name);
						protoCrewMember2.UpdateExperience();
					}
				}
			}
		}
		flag = null;
	}

	public virtual void seated_cmd_OnEnter(KFSMState st)
	{
		GameEvents.onCommandSeatInteractionEnter.Fire(this, data1: true);
		this.GetComponentCached(ref _animation).Play(Animations.seatIdle, PlayMode.StopSameLayer);
		base.Events["OnDeboardSeat"].active = true;
		base.Events["MakeReference"].active = true;
		base.Events["RenameVessel"].active = true;
		base.part.isControlSource = Vessel.ControlLevel.FULL;
		if (fsm.LastEvent != On_seatBoard)
		{
			SetEjectDirection();
		}
		GameEvents.onCommandSeatInteraction.Fire(this, data1: true);
		if (partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
		if (KerbalPortraitGallery.Instance != null)
		{
			portrait = KerbalPortraitGallery.Instance.RegisterActiveCrew(this, base.vessel != null && base.vessel.isActiveVessel);
		}
	}

	public virtual void seated_cmd_OnLeave(KFSMState st)
	{
		GameEvents.onCommandSeatInteractionEnter.Fire(this, data1: false);
		base.Events["OnDeboardSeat"].active = false;
		base.Events["MakeReference"].active = false;
		base.Events["RenameVessel"].active = false;
		base.part.isControlSource = Vessel.ControlLevel.NONE;
		if (fsm.LastEvent != On_seatDeboard)
		{
			if (base.vessel.rootPart != base.part)
			{
				EjectFromSeat();
			}
			RestoreVesselInfo(0.1f);
			SwitchFocusIfActiveVesselUncontrollable(0.1f);
		}
		if (st == st_ragdoll && evaChute != null && (evaChute.deploymentState == ModuleParachute.deploymentStates.SEMIDEPLOYED || evaChute.deploymentState == ModuleParachute.deploymentStates.DEPLOYED))
		{
			OnParachuteCut();
		}
		if (KerbalPortraitGallery.Instance != null)
		{
			KerbalPortraitGallery.Instance.UnregisterActiveCrew(this);
			portrait = null;
		}
		GameEvents.onCommandSeatInteraction.Fire(this, data1: false);
	}

	public virtual void grappled_OnEnter(KFSMState st)
	{
		if (st != st_ragdoll)
		{
			if (!base.vessel.packed)
			{
				SetRagdoll(ragDoll: true);
			}
			isRagdoll = true;
			lastCollisionTime = 0.0;
			if (InConstructionMode && (bool)EVAConstructionModeController.Instance)
			{
				EVAConstructionModeController.Instance.ClosePanel();
				weldFX.Stop();
				alternateIdleDisabled = false;
				ToggleWeldingGun(toggle: false);
				InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
			}
			currentLadder = null;
			if (currentLadderPart != null)
			{
				currentLadderPart.hasKerbalOnLadder = false;
			}
			currentLadderPart = null;
			base.vessel.UpdateLandedSplashed();
			secondaryLadder = null;
			currentLadderTriggers.Clear();
			if (partPlacementMode && moduleInventoryPartReference != null)
			{
				moduleInventoryPartReference.CancelPartPlacementMode();
			}
		}
	}

	public virtual void grappled_OnLeave(KFSMState st)
	{
		if (st != st_ragdoll)
		{
			SetRagdoll(ragDoll: false);
			canRecover = false;
		}
		updateRagdollVelocities();
		updateRagdollVelocities();
		RestoreVesselInfo(0.1f);
		SwitchFocusIfActiveVesselUncontrollable(0.1f);
	}

	public void EnterConstructionMode()
	{
		InConstructionMode = true;
		if (!OnALadder)
		{
			fsm.RunEvent(On_constructionModeEnter);
		}
	}

	public void ExitConstructionMode()
	{
		InConstructionMode = false;
		if (!OnALadder)
		{
			InterruptWeld();
			fsm.RunEvent(On_constructionModeExit);
			InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
		}
	}

	public virtual void enteringConstruction_OnEnter(KFSMState st)
	{
		alternateIdleDisabled = true;
		base.Events["PlantFlag"].active = CanPlantFlag();
		if (weldTool == null)
		{
			weldTool = UnityEngine.Object.Instantiate(weldToolPrefab);
			weldTool.transform.SetParent(weldToolAnchor);
			weldTool.transform.localPosition = Vector3.zero;
			weldTool.transform.localRotation = Quaternion.identity;
			weldFX = weldTool.GetComponent<WeldFX>();
			weldFX.evaController = this;
			weldTool.GetComponentInChildren<TrackRigObject>().target = handNodes[1].transform;
		}
		weldFX.Active = true;
		ToggleWeldingGun(toggle: true);
	}

	public virtual void exitingConstruction_OnEnter(KFSMState st)
	{
		alternateIdleDisabled = false;
		base.Events["PlantFlag"].active = CanPlantFlag();
		ToggleWeldingGun(toggle: false);
	}

	public virtual void enteringExitingConstruction_OnFixedUpdate()
	{
		if (SurfaceContact())
		{
			if ((fsm.CurrentState == st_enteringConstruction && fsm.TimeAtCurrentState >= (double)Animations.weldGunDraw.State.length) || (fsm.CurrentState == st_exitingConstruction && fsm.TimeAtCurrentState >= (double)Animations.weldGunPutAway.State.length))
			{
				fsm.RunEvent(On_constructionModeTrigger_gr_Complete);
				return;
			}
			UpdateMovement();
			correctGroundedRotation();
			if (fsm.LastState == st_idle_gr || fsm.LastState == st_idle_b_gr)
			{
				return;
			}
			UpdateHeading();
			if (!JetpackDeployed)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.weldGunLift, 1f, 0.1f);
			}
		}
		else
		{
			if ((fsm.CurrentState == st_enteringConstruction && fsm.TimeAtCurrentState >= (double)Animations.weldGunDraw.State.length) || (fsm.CurrentState == st_exitingConstruction && fsm.TimeAtCurrentState >= (double)Animations.weldGunPutAway.State.length))
			{
				fsm.RunEvent(On_constructionModeTrigger_fl_Complete);
				return;
			}
			UpdateOrientationPID();
		}
		updateRagdollVelocities();
	}

	public virtual void exitingConstruction_OnLeave(KFSMState st)
	{
		this.GetComponentCached(ref _animation).Stop(Animations.weldGunLift);
	}

	public void ToggleWeldingGun(bool toggle)
	{
		if (toggle)
		{
			Animations.weldGunDraw.State.time = Animations.weldGunDraw.start;
			this.GetComponentCached(ref _animation).CrossFade(Animations.weldGunDraw, 0.2f, PlayMode.StopSameLayer);
			if (SurfaceContact() && !JetpackDeployed)
			{
				this.GetComponentCached(ref _animation).Blend(Animations.weldGunLift, 1f, 0.1f);
			}
			weldFX.EnableMesh(0.5f);
			weldFX.IsFloating = !SurfaceContact();
		}
		else
		{
			Animations.weldGunPutAway.State.time = Animations.weldGunPutAway.start;
			this.GetComponentCached(ref _animation).CrossFade(Animations.weldGunPutAway, 0.2f, PlayMode.StopSameLayer);
			this.GetComponentCached(ref _animation).Blend(Animations.weldGunLift, 0f, 0.1f);
			if (weldFX != null)
			{
				weldFX.DisableMesh(Animations.weldGunPutAway.State.length - 1f);
			}
		}
	}

	public virtual void weld_acquireHeading_OnEnter(KFSMState st)
	{
		ZeroRBVelocity();
		tgtRpos = Vector3.zero;
		packTgtRPos = Vector3.zero;
		deltaHdg = 0f;
		tgtSpeed = 0f;
		InputLockManager.SetControlLock(~(ControlTypes.EDITOR_LOCK | ControlTypes.flag_2), "WeldLock_" + base.vessel.id);
		SetWaypoint(constructionTarget.transform.position);
		UpdateHeading();
		if (SurfaceContact())
		{
			this.GetComponentCached(ref _animation).CrossFade((deltaHdg > 0f) ? Animations.turnRight : Animations.turnLeft, 0.3f, PlayMode.StopSameLayer);
			return;
		}
		this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
		if (!JetpackDeployed)
		{
			jetpackDeployedForWelding = true;
			ToggleJetpack(packState: true);
		}
		else
		{
			jetpackDeployedForWelding = false;
		}
	}

	public virtual void weld_acquireHeading_OnFixedUpdate()
	{
		if (SurfaceContact())
		{
			correctGroundedRotation();
		}
		else
		{
			UpdateOrientationPID();
		}
		updateRagdollVelocities();
	}

	public virtual void weld_acquireHeading_OnLateUpdate()
	{
		if (SurfaceContact())
		{
			SetWaypoint(constructionTarget.transform.position);
			UpdateHeading();
		}
		else
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(constructionTarget.transform.position - base.transform.position), Time.deltaTime * 1.5f);
		}
	}

	public virtual void weld_acquireHeading_OnLeave(KFSMState st)
	{
		lastTgtSpeed = 0f;
	}

	public virtual void weld_OnEnter(KFSMState st)
	{
		Animations.weld.State.time = Animations.weld.start;
		Animations.weldSuspended.State.time = Animations.weldSuspended.start;
		if (HasWeldLineOfSight())
		{
			if (SurfaceContact())
			{
				this.GetComponentCached(ref _animation).CrossFade(Animations.weld, 0.2f, PlayMode.StopSameLayer);
			}
			else
			{
				this.GetComponentCached(ref _animation).CrossFade(Animations.weldSuspended, 0.2f, PlayMode.StopSameLayer);
			}
			wasVisorEnabledBeforeWelding = visorState == VisorStates.Lowered;
			LowerVisor(forceHelmet: true);
			if (weldFX != null)
			{
				weldFX.Play();
			}
		}
		else
		{
			fsm.RunEvent(On_weldComplete);
		}
	}

	public bool HasWeldLineOfSight()
	{
		Vector3 gunPoint = base.transform.position + base.transform.up * (halfHeight - 0.1f);
		Bounds rendererBoundsWithoutParticles = constructionTarget.gameObject.GetRendererBoundsWithoutParticles();
		if (constructionTarget.parent != null)
		{
			Bounds rendererBoundsWithoutParticles2 = constructionTarget.parent.gameObject.GetRendererBoundsWithoutParticles();
			if (Physics.Raycast(rendererBoundsWithoutParticles.center, (rendererBoundsWithoutParticles2.center - rendererBoundsWithoutParticles.center).normalized, out var hitInfo, (rendererBoundsWithoutParticles2.center - rendererBoundsWithoutParticles.center).magnitude, LayerUtil.DefaultEquivalent, QueryTriggerInteraction.Ignore))
			{
				Vector3 point = hitInfo.point;
				if (HasWeldLineOfSight(gunPoint, point))
				{
					return true;
				}
			}
		}
		Vector3 partAttachPoint = rendererBoundsWithoutParticles.center - constructionTarget.transform.forward * constructionTargetPivotOffset;
		if (HasWeldLineOfSight(gunPoint, partAttachPoint))
		{
			return true;
		}
		partAttachPoint = rendererBoundsWithoutParticles.center - constructionTarget.transform.right * constructionTargetPivotOffset;
		if (HasWeldLineOfSight(gunPoint, partAttachPoint))
		{
			return true;
		}
		partAttachPoint = rendererBoundsWithoutParticles.center - constructionTarget.transform.up * constructionTargetPivotOffset;
		if (HasWeldLineOfSight(gunPoint, partAttachPoint))
		{
			return true;
		}
		return false;
	}

	public bool HasWeldLineOfSight(Vector3 gunPoint, Vector3 partAttachPoint)
	{
		if (Physics.Raycast(gunPoint, (partAttachPoint - gunPoint).normalized, out var hitInfo, GameSettings.EVA_CONSTRUCTION_RANGE * 1.2f, LayerUtil.DefaultEquivalent, QueryTriggerInteraction.Ignore) && hitInfo.collider.gameObject.GetComponentInParent<Part>() == constructionTarget)
		{
			return true;
		}
		return false;
	}

	public void InterruptWeld()
	{
		if (fsm.CurrentState == st_weld)
		{
			weldFX.Stop();
			fsm.RunEvent(On_weldComplete);
			InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
		}
	}

	public virtual void OnWeldStart(KerbalEVA kerbalEVA)
	{
		if (weldingLasersFX != null && !weldingLasersFX.isPlaying)
		{
			weldingLasersFX.PlayDelayed(kerbalEVA.weldFX.StartDelay);
		}
	}

	public virtual void OnWeldFinish(KerbalEVA kerbalEVA)
	{
		if (weldingLasersFX != null)
		{
			weldingLasersFX.Stop();
		}
	}

	public virtual void weld_OnFixedUpdate()
	{
		if (SurfaceContact())
		{
			correctGroundedRotation();
			ZeroRBVelocity();
			UpdateMovement();
			this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
			float value = Vector3.SignedAngle(constructionTarget.transform.position - base.transform.position, base.transform.forward, base.transform.right);
			value = Mathf.Clamp(value, -20f, 45f);
			if (value > 0f)
			{
				value /= 45f;
				this.GetComponentCached(ref _animation).Blend(Animations.weld_aim_up, value, 0.15f);
				this.GetComponentCached(ref _animation).Blend(Animations.weld_aim_down, 0f, 0.15f);
				this.GetComponentCached(ref _animation).Blend(Animations.weld, 1f - value, 0.15f);
			}
			else
			{
				value /= 20f;
				this.GetComponentCached(ref _animation).Blend(Animations.weld_aim_down, 0f - value, 0.15f);
				this.GetComponentCached(ref _animation).Blend(Animations.weld_aim_up, 0f, 0.15f);
				this.GetComponentCached(ref _animation).Blend(Animations.weld, 1f + value, 0.15f);
			}
		}
		else
		{
			UpdateOrientationPID();
			base.transform.LookAt(constructionTarget.transform);
		}
		updateRagdollVelocities();
	}

	public virtual void weld_OnLeave(KFSMState st)
	{
		InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
		if (SurfaceContact())
		{
			this.GetComponentCached(ref _animation).Blend(Animations.weldGunLift, 1f, 0.1f);
		}
		this.GetComponentCached(ref _animation).Stop(Animations.weld_aim_up);
		this.GetComponentCached(ref _animation).Stop(Animations.weld_aim_down);
		this.GetComponentCached(ref _animation).Stop(Animations.weld);
		if (!wasVisorEnabledBeforeWelding)
		{
			RaiseVisor(restoreHelmet: true);
		}
		if (jetpackDeployedForWelding)
		{
			ToggleJetpack(packState: false);
		}
	}

	public virtual void SetupFSM()
	{
		fsm = new KerbalFSM();
		KerbalFSM kerbalFSM = fsm;
		kerbalFSM.OnStateChange = (Callback<KFSMState, KFSMState, KFSMEvent>)Delegate.Combine(kerbalFSM.OnStateChange, new Callback<KFSMState, KFSMState, KFSMEvent>(OnFSMStateChange));
		KerbalFSM kerbalFSM2 = fsm;
		kerbalFSM2.OnEventCalled = (Callback<KFSMEvent>)Delegate.Combine(kerbalFSM2.OnEventCalled, new Callback<KFSMEvent>(OnFSMEventCalled));
		st_idle_gr = new KFSMState("Idle (Grounded)");
		st_idle_gr.OnEnter = idle_OnEnter;
		KFSMState kFSMState = st_idle_gr;
		kFSMState.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState2 = st_idle_gr;
		kFSMState2.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState2.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState3 = st_idle_gr;
		kFSMState3.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState3.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState4 = st_idle_gr;
		kFSMState4.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState4.OnFixedUpdate, new KFSMCallback(UpdatePackLinear));
		KFSMState kFSMState5 = st_idle_gr;
		kFSMState5.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState5.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState6 = st_idle_gr;
		kFSMState6.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState6.OnFixedUpdate, new KFSMCallback(CheckLadderTriggers));
		st_idle_gr.OnLeave = idle_OnLeave;
		fsm.AddState(st_idle_gr);
		st_idle_b_gr = new KFSMState("Idle_b (Grounded)");
		st_idle_b_gr.OnEnter = idle_b_OnEnter;
		KFSMState kFSMState7 = st_idle_b_gr;
		kFSMState7.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState7.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState8 = st_idle_b_gr;
		kFSMState8.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState8.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState9 = st_idle_b_gr;
		kFSMState9.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState9.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState10 = st_idle_b_gr;
		kFSMState10.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState10.OnFixedUpdate, new KFSMCallback(UpdatePackLinear));
		KFSMState kFSMState11 = st_idle_b_gr;
		kFSMState11.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState11.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState12 = st_idle_b_gr;
		kFSMState12.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState12.OnFixedUpdate, new KFSMCallback(CheckLadderTriggers));
		st_idle_b_gr.OnLeave = idle_b_OnLeave;
		fsm.AddState(st_idle_b_gr);
		On_idle_b_gr = new KFSMEvent("New Idle (Grounded)");
		On_idle_b_gr.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_idle_b_gr.GoToStateOnEvent = st_idle_b_gr;
		fsm.AddEvent(On_idle_b_gr, st_idle_gr, st_idle_b_gr);
		On_return_idle = new KFSMEvent("Return to Idle (Grounded)");
		On_return_idle.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_return_idle.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_return_idle, st_idle_gr, st_idle_b_gr);
		st_walk_acd = new KFSMState("Walk (Arcade)");
		st_walk_acd.OnEnter = walk_Acd_OnEnter;
		KFSMState kFSMState13 = st_walk_acd;
		kFSMState13.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState13.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState14 = st_walk_acd;
		kFSMState14.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState14.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState15 = st_walk_acd;
		kFSMState15.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState15.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState16 = st_walk_acd;
		kFSMState16.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState16.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_walk_acd.OnLeave = walk_ccd_OnLeave;
		fsm.AddState(st_walk_acd);
		st_walk_fps = new KFSMState("Walk (FPS)");
		KFSMState kFSMState17 = st_walk_fps;
		kFSMState17.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState17.OnUpdate, new KFSMCallback(walk_fps_OnUpdate));
		KFSMState kFSMState18 = st_walk_fps;
		kFSMState18.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState18.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState19 = st_walk_fps;
		kFSMState19.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState19.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState20 = st_walk_fps;
		kFSMState20.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState20.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState21 = st_walk_fps;
		kFSMState21.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState21.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_walk_fps.OnLeave = walk_fps_OnLeave;
		fsm.AddState(st_walk_fps);
		st_heading_acquire = new KFSMState("Turn to Heading");
		st_heading_acquire.OnEnter = heading_acquire_OnEnter;
		KFSMState kFSMState22 = st_heading_acquire;
		kFSMState22.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState22.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState23 = st_heading_acquire;
		kFSMState23.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState23.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState24 = st_heading_acquire;
		kFSMState24.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState24.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_heading_acquire.OnLeave = heading_acquire_OnLeave;
		fsm.AddState(st_heading_acquire);
		st_run_acd = new KFSMState("Run (Arcade)");
		st_run_acd.OnEnter = run_acd_OnEnter;
		KFSMState kFSMState25 = st_run_acd;
		kFSMState25.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState25.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState26 = st_run_acd;
		kFSMState26.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState26.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState27 = st_run_acd;
		kFSMState27.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState27.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState28 = st_run_acd;
		kFSMState28.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState28.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_run_acd.OnLeave = run_acd_OnLeave;
		fsm.AddState(st_run_acd);
		st_run_fps = new KFSMState("Run (FPS)");
		KFSMState kFSMState29 = st_run_fps;
		kFSMState29.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState29.OnUpdate, new KFSMCallback(run_fps_OnUpdate));
		KFSMState kFSMState30 = st_run_fps;
		kFSMState30.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState30.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState31 = st_run_fps;
		kFSMState31.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState31.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState32 = st_run_fps;
		kFSMState32.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState32.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState33 = st_run_fps;
		kFSMState33.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState33.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_run_fps.OnLeave = run_fps_OnLeave;
		fsm.AddState(st_run_fps);
		st_bound_gr_acd = new KFSMState("Low G Bound (Grounded - Arcade)");
		st_bound_gr_acd.OnEnter = bound_gr_acd_OnEnter;
		KFSMState kFSMState34 = st_bound_gr_acd;
		kFSMState34.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState34.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState35 = st_bound_gr_acd;
		kFSMState35.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState35.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState36 = st_bound_gr_acd;
		kFSMState36.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState36.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState37 = st_bound_gr_acd;
		kFSMState37.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState37.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState38 = st_bound_gr_acd;
		kFSMState38.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState38.OnFixedUpdate, new KFSMCallback(UpdateLowGBodyColliders));
		KFSMState kFSMState39 = st_bound_gr_acd;
		kFSMState39.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState39.OnUpdate, new KFSMCallback(bound_gr_acd_OnUpdate));
		st_bound_gr_acd.OnLeave = bound_gr_acd_OnLeave;
		fsm.AddState(st_bound_gr_acd);
		st_bound_gr_fps = new KFSMState("Low G Bound (Grounded - FPS)");
		st_bound_gr_fps.OnEnter = bound_gr_fps_OnEnter;
		KFSMState kFSMState40 = st_bound_gr_fps;
		kFSMState40.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState40.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState41 = st_bound_gr_fps;
		kFSMState41.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState41.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState42 = st_bound_gr_fps;
		kFSMState42.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState42.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState43 = st_bound_gr_fps;
		kFSMState43.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState43.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState44 = st_bound_gr_acd;
		kFSMState44.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState44.OnFixedUpdate, new KFSMCallback(UpdateLowGBodyColliders));
		KFSMState kFSMState45 = st_bound_gr_fps;
		kFSMState45.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState45.OnUpdate, new KFSMCallback(bound_gr_fps_OnUpdate));
		st_bound_gr_fps.OnLeave = bound_gr_fps_OnLeave;
		fsm.AddState(st_bound_gr_fps);
		lastBoundStep = Animations.walkLowGee.State.length * 0.5f;
		st_bound_fl = new KFSMState("Low G Bound (floating)");
		st_bound_fl.OnEnter = bound_fl_OnEnter;
		KFSMState kFSMState46 = st_bound_fl;
		kFSMState46.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState46.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState47 = st_bound_fl;
		kFSMState47.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState47.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState48 = st_bound_fl;
		kFSMState48.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState48.OnFixedUpdate, new KFSMCallback(UpdateLowGBodyColliders));
		st_bound_fl.OnLeave = bound_fl_OnLeave;
		fsm.AddState(st_bound_fl);
		st_ragdoll = new KFSMState("Ragdoll");
		st_ragdoll.OnEnter = ragdoll_OnEnter;
		KFSMState kFSMState49 = st_ragdoll;
		kFSMState49.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState49.OnFixedUpdate, new KFSMCallback(IntegrateRagdollRigidbodyForces));
		st_ragdoll.OnLeave = ragdoll_OnLeave;
		fsm.AddState(st_ragdoll);
		st_recover = new KFSMState("Recover");
		st_recover.OnEnter = recover_OnEnter;
		st_recover.OnUpdate = recover_OnUpdate;
		KFSMState kFSMState50 = st_recover;
		kFSMState50.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState50.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_recover);
		st_jump = new KFSMState("Jumping");
		st_jump.OnEnter = jump_OnEnter;
		KFSMState kFSMState51 = st_jump;
		kFSMState51.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState51.OnUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState52 = st_jump;
		kFSMState52.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState52.OnUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState53 = st_jump;
		kFSMState53.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState53.OnUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState54 = st_jump;
		kFSMState54.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState54.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_jump);
		st_idle_fl = new KFSMState("Idle (Floating)");
		st_idle_fl.OnEnter = idle_fl_OnEnter;
		KFSMState kFSMState55 = st_idle_fl;
		kFSMState55.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState55.OnFixedUpdate, new KFSMCallback(UpdateOrientationPID));
		KFSMState kFSMState56 = st_idle_fl;
		kFSMState56.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState56.OnFixedUpdate, new KFSMCallback(UpdatePackAngular));
		KFSMState kFSMState57 = st_idle_fl;
		kFSMState57.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState57.OnFixedUpdate, new KFSMCallback(UpdatePackLinear));
		KFSMState kFSMState58 = st_idle_fl;
		kFSMState58.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState58.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_idle_fl.OnLeave = idle_fl_OnLeave;
		fsm.AddState(st_idle_fl);
		st_land = new KFSMState("Landing");
		st_land.OnEnter = land_OnEnter;
		KFSMState kFSMState59 = st_land;
		kFSMState59.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState59.OnUpdate, new KFSMCallback(land_OnUpdate));
		KFSMState kFSMState60 = st_land;
		kFSMState60.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState60.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_land);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			st_controlPanel_identified = new KFSMState("Control Panel Identified");
			st_controlPanel_identified.OnEnter = controlPanel_identified_OnEnter;
			KFSMState kFSMState61 = st_controlPanel_identified;
			kFSMState61.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState61.OnUpdate, new KFSMCallback(correctGroundedRotation));
			KFSMState kFSMState62 = st_controlPanel_identified;
			kFSMState62.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState62.OnUpdate, new KFSMCallback(UpdateMovement));
			KFSMState kFSMState63 = st_controlPanel_identified;
			kFSMState63.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState63.OnUpdate, new KFSMCallback(UpdateHeading));
			KFSMState kFSMState64 = st_controlPanel_identified;
			kFSMState64.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState64.OnUpdate, new KFSMCallback(ControlPanelInteractionFinished));
			KFSMState kFSMState65 = st_controlPanel_identified;
			kFSMState65.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState65.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
			fsm.AddState(st_controlPanel_identified);
		}
		st_swim_idle = new KFSMState("Swim (Idle)");
		st_swim_idle.OnEnter = swim_idle_OnEnter;
		KFSMState kFSMState66 = st_swim_idle;
		kFSMState66.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState66.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState67 = st_swim_idle;
		kFSMState67.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState67.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState68 = st_swim_idle;
		kFSMState68.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState68.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState69 = st_swim_idle;
		kFSMState69.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState69.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState70 = st_swim_idle;
		kFSMState70.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState70.OnFixedUpdate, new KFSMCallback(CheckLadderTriggers));
		st_swim_idle.OnLeave = swim_idle_OnLeave;
		fsm.AddState(st_swim_idle);
		st_swim_fwd = new KFSMState("Swim (fwd)");
		st_swim_fwd.OnEnter = swim_fwd_OnEnter;
		KFSMState kFSMState71 = st_swim_fwd;
		kFSMState71.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState71.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState72 = st_swim_fwd;
		kFSMState72.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState72.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState73 = st_swim_fwd;
		kFSMState73.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState73.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState74 = st_swim_fwd;
		kFSMState74.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState74.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_swim_fwd.OnLeave = swim_fwd_OnLeave;
		fsm.AddState(st_swim_fwd);
		st_ladder_acquire = new KFSMState("Ladder (Acquire)");
		st_ladder_acquire.OnEnter = ladder_acquire_OnEnter;
		KFSMState kFSMState75 = st_ladder_acquire;
		kFSMState75.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState75.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState76 = st_ladder_acquire;
		kFSMState76.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState76.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_ladder_acquire.OnLeave = ladder_acquire_OnLeave;
		fsm.AddState(st_ladder_acquire);
		st_ladder_idle = new KFSMState("Ladder (Idle)");
		st_ladder_idle.updateMode = KFSMUpdateMode.UPDATE;
		st_ladder_idle.OnEnter = ladder_idle_OnEnter;
		KFSMState kFSMState77 = st_ladder_idle;
		kFSMState77.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState77.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState78 = st_ladder_idle;
		kFSMState78.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState78.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState79 = st_ladder_idle;
		kFSMState79.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState79.OnFixedUpdate, new KFSMCallback(UpdateLadderMovement));
		KFSMState kFSMState80 = st_ladder_idle;
		kFSMState80.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState80.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState81 = st_ladder_idle;
		kFSMState81.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState81.OnUpdate, new KFSMCallback(InterpolateLadders));
		KFSMState kFSMState82 = st_ladder_idle;
		kFSMState82.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState82.OnUpdate, new KFSMCallback(UpdateCurrentLadderIdle));
		st_ladder_idle.OnLeave = ladder_idle_OnLeave;
		fsm.AddState(st_ladder_idle);
		st_ladder_lean = new KFSMState("Ladder (Lean)");
		st_ladder_lean.OnEnter = ladder_lean_OnEnter;
		KFSMState kFSMState83 = st_ladder_lean;
		kFSMState83.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState83.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState84 = st_ladder_lean;
		kFSMState84.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState84.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState85 = st_ladder_lean;
		kFSMState85.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState85.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState86 = st_ladder_lean;
		kFSMState86.OnLateUpdate = (KFSMCallback)Delegate.Combine(kFSMState86.OnLateUpdate, new KFSMCallback(ladder_lean_OnLateUpdate));
		st_ladder_lean.OnLeave = ladder_lean_OnLeave;
		fsm.AddState(st_ladder_lean);
		st_ladder_climb = new KFSMState("Ladder (Climb)");
		st_ladder_climb.OnEnter = ladder_climb_OnEnter;
		KFSMState kFSMState87 = st_ladder_climb;
		kFSMState87.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState87.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState88 = st_ladder_climb;
		kFSMState88.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState88.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState89 = st_ladder_climb;
		kFSMState89.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState89.OnFixedUpdate, new KFSMCallback(UpdateLadderMovement));
		KFSMState kFSMState90 = st_ladder_climb;
		kFSMState90.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState90.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState91 = st_ladder_climb;
		kFSMState91.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState91.OnUpdate, new KFSMCallback(InterpolateLadders));
		KFSMState kFSMState92 = st_ladder_climb;
		kFSMState92.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState92.OnUpdate, new KFSMCallback(UpdateCurrentLadder));
		st_ladder_climb.OnLeave = ladder_climb_OnLeave;
		fsm.AddState(st_ladder_climb);
		st_ladder_descend = new KFSMState("Ladder (Descend)");
		st_ladder_descend.OnEnter = ladder_descend_OnEnter;
		KFSMState kFSMState93 = st_ladder_descend;
		kFSMState93.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState93.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState94 = st_ladder_descend;
		kFSMState94.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState94.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState95 = st_ladder_descend;
		kFSMState95.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState95.OnFixedUpdate, new KFSMCallback(UpdateLadderMovement));
		KFSMState kFSMState96 = st_ladder_descend;
		kFSMState96.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState96.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState97 = st_ladder_descend;
		kFSMState97.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState97.OnUpdate, new KFSMCallback(InterpolateLadders));
		KFSMState kFSMState98 = st_ladder_descend;
		kFSMState98.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState98.OnUpdate, new KFSMCallback(UpdateCurrentLadder));
		st_ladder_descend.OnLeave = ladder_descend_OnLeave;
		fsm.AddState(st_ladder_descend);
		st_ladder_pushoff = new KFSMState("Ladder (Pushoff)");
		KFSMState kFSMState99 = st_ladder_pushoff;
		kFSMState99.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState99.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState100 = st_ladder_pushoff;
		kFSMState100.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState100.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState101 = st_ladder_pushoff;
		kFSMState101.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState101.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_ladder_pushoff.OnLeave = ladder_pushoff_OnLeave;
		fsm.AddState(st_ladder_pushoff);
		st_ladder_end_reached = new KFSMState("Ladder (End Reached)");
		st_ladder_end_reached.OnEnter = ladder_end_reached_OnEnter;
		KFSMState kFSMState102 = st_ladder_end_reached;
		kFSMState102.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState102.OnFixedUpdate, new KFSMCallback(correctLadderRotation));
		KFSMState kFSMState103 = st_ladder_end_reached;
		kFSMState103.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState103.OnFixedUpdate, new KFSMCallback(correctLadderPosition));
		KFSMState kFSMState104 = st_ladder_end_reached;
		kFSMState104.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState104.OnFixedUpdate, new KFSMCallback(UpdateLadderMovement));
		KFSMState kFSMState105 = st_ladder_end_reached;
		kFSMState105.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState105.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState106 = st_ladder_end_reached;
		kFSMState106.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState106.OnUpdate, new KFSMCallback(InterpolateLadders));
		KFSMState kFSMState107 = st_ladder_end_reached;
		kFSMState107.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState107.OnUpdate, new KFSMCallback(UpdateCurrentLadder));
		st_ladder_end_reached.OnLeave = ladder_end_reached_OnLeave;
		fsm.AddState(st_ladder_end_reached);
		st_clamber_acquireP1 = new KFSMState("Clamber (P1)");
		st_clamber_acquireP1.OnEnter = clamber_acquireP1_OnEnter;
		KFSMState kFSMState108 = st_clamber_acquireP1;
		kFSMState108.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState108.OnFixedUpdate, new KFSMCallback(ZeroRBVelocity));
		KFSMState kFSMState109 = st_clamber_acquireP1;
		kFSMState109.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState109.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState110 = st_clamber_acquireP1;
		kFSMState110.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState110.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_clamber_acquireP1.OnLeave = clamber_acquireP1_OnLeave;
		fsm.AddState(st_clamber_acquireP1);
		st_clamber_acquireP2 = new KFSMState("Clamber (P2)");
		st_clamber_acquireP2.OnEnter = clamber_acquireP2_OnEnter;
		KFSMState kFSMState111 = st_clamber_acquireP2;
		kFSMState111.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState111.OnFixedUpdate, new KFSMCallback(ZeroRBVelocity));
		KFSMState kFSMState112 = st_clamber_acquireP2;
		kFSMState112.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState112.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState113 = st_clamber_acquireP2;
		kFSMState113.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState113.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_clamber_acquireP2.OnLeave = clamber_acquireP2_OnLeave;
		fsm.AddState(st_clamber_acquireP2);
		st_clamber_acquireP3 = new KFSMState("Clamber (P3)");
		st_clamber_acquireP3.OnEnter = clamber_acquireP3_OnEnter;
		KFSMState kFSMState114 = st_clamber_acquireP3;
		kFSMState114.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState114.OnFixedUpdate, new KFSMCallback(ZeroRBVelocity));
		KFSMState kFSMState115 = st_clamber_acquireP3;
		kFSMState115.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState115.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState116 = st_clamber_acquireP3;
		kFSMState116.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState116.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_clamber_acquireP3.OnLeave = clamber_acquireP3_OnLeave;
		fsm.AddState(st_clamber_acquireP3);
		st_flagAcquireHeading = new KFSMState("Flag-plant Terrain Acquire");
		st_flagAcquireHeading.OnEnter = flagAcquireHeading_OnEnter;
		KFSMState kFSMState117 = st_flagAcquireHeading;
		kFSMState117.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState117.OnFixedUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState118 = st_flagAcquireHeading;
		kFSMState118.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState118.OnFixedUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState119 = st_flagAcquireHeading;
		kFSMState119.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState119.OnFixedUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState120 = st_flagAcquireHeading;
		kFSMState120.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState120.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		KFSMState kFSMState121 = st_flagAcquireHeading;
		kFSMState121.OnLateUpdate = (KFSMCallback)Delegate.Combine(kFSMState121.OnLateUpdate, new KFSMCallback(flagAcquireHeading_OnLateUpdate));
		st_flagAcquireHeading.OnLeave = flagAcquireHeading_OnLeave;
		fsm.AddState(st_flagAcquireHeading);
		st_flagPlant = new KFSMState("Planting Flag");
		st_flagPlant.OnEnter = flagPlant_OnEnter;
		KFSMState kFSMState122 = st_flagPlant;
		kFSMState122.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState122.OnFixedUpdate, new KFSMCallback(ZeroRBVelocity));
		KFSMState kFSMState123 = st_flagPlant;
		kFSMState123.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState123.OnUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState124 = st_flagPlant;
		kFSMState124.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState124.OnUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState125 = st_flagPlant;
		kFSMState125.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState125.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_flagPlant.OnLeave = flagPlant_OnLeave;
		fsm.AddState(st_flagPlant);
		st_seated_cmd = new KFSMState("Seated (Command)");
		st_seated_cmd.OnEnter = seated_cmd_OnEnter;
		KFSMState kFSMState126 = st_seated_cmd;
		kFSMState126.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState126.OnUpdate, new KFSMCallback(UpdateParachuteInCommandSeat));
		st_seated_cmd.OnLeave = seated_cmd_OnLeave;
		fsm.AddState(st_seated_cmd);
		st_grappled = new KFSMState("Grappled");
		st_grappled.OnEnter = grappled_OnEnter;
		KFSMState kFSMState127 = st_grappled;
		kFSMState127.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState127.OnFixedUpdate, new KFSMCallback(IntegrateRagdollRigidbodyForces));
		st_grappled.OnLeave = grappled_OnLeave;
		fsm.AddState(st_grappled);
		st_semi_deployed_parachute = new KFSMState("Semi Deployed Parachute");
		st_semi_deployed_parachute.OnEnter = OnSemiDeployedParachuteModeEntered;
		KFSMState kFSMState128 = st_semi_deployed_parachute;
		kFSMState128.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState128.OnFixedUpdate, new KFSMCallback(UpdateSemiDeployedParachuteMovement));
		KFSMState kFSMState129 = st_semi_deployed_parachute;
		kFSMState129.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState129.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_semi_deployed_parachute.OnLeave = OnSemiDeployedParachuteModeLeft;
		fsm.AddState(st_semi_deployed_parachute);
		st_fully_deployed_parachute = new KFSMState("Fully Deployed Parachute");
		st_fully_deployed_parachute.OnEnter = OnFullyDeployedParachuteModeEntered;
		KFSMState kFSMState130 = st_fully_deployed_parachute;
		kFSMState130.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState130.OnFixedUpdate, new KFSMCallback(UpdateFullyDeployedParachuteMovement));
		KFSMState kFSMState131 = st_fully_deployed_parachute;
		kFSMState131.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState131.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		st_fully_deployed_parachute.OnLeave = OnFullyDeployedParachuteModeLeft;
		fsm.AddState(st_fully_deployed_parachute);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			st_picking_roc_sample = new KFSMState("Picking Roc Sample");
			st_picking_roc_sample.OnEnter = picking_roc_sample_OnEnter;
			KFSMState kFSMState132 = st_picking_roc_sample;
			kFSMState132.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState132.OnUpdate, new KFSMCallback(correctGroundedRotation));
			KFSMState kFSMState133 = st_picking_roc_sample;
			kFSMState133.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState133.OnUpdate, new KFSMCallback(UpdateMovement));
			KFSMState kFSMState134 = st_picking_roc_sample;
			kFSMState134.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState134.OnUpdate, new KFSMCallback(UpdateHeading));
			KFSMState kFSMState135 = st_picking_roc_sample;
			kFSMState135.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState135.OnUpdate, new KFSMCallback(RocSampleStored));
			KFSMState kFSMState136 = st_picking_roc_sample;
			kFSMState136.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState136.OnUpdate, new KFSMCallback(spawnHammer));
			KFSMState kFSMState137 = st_picking_roc_sample;
			kFSMState137.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState137.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
			fsm.AddState(st_picking_roc_sample);
		}
		st_enteringConstruction = new KFSMState("Entering construction mode");
		st_enteringConstruction.OnEnter = enteringConstruction_OnEnter;
		KFSMState kFSMState138 = st_enteringConstruction;
		kFSMState138.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState138.OnFixedUpdate, new KFSMCallback(enteringExitingConstruction_OnFixedUpdate));
		fsm.AddState(st_enteringConstruction);
		st_exitingConstruction = new KFSMState("Exiting construction mode");
		st_exitingConstruction.OnEnter = exitingConstruction_OnEnter;
		KFSMState kFSMState139 = st_exitingConstruction;
		kFSMState139.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState139.OnFixedUpdate, new KFSMCallback(enteringExitingConstruction_OnFixedUpdate));
		st_exitingConstruction.OnLeave = exitingConstruction_OnLeave;
		fsm.AddState(st_exitingConstruction);
		st_weldAcquireHeading = new KFSMState("Weld Turn to face Part");
		st_weldAcquireHeading.OnEnter = weld_acquireHeading_OnEnter;
		KFSMState kFSMState140 = st_weldAcquireHeading;
		kFSMState140.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState140.OnFixedUpdate, new KFSMCallback(weld_acquireHeading_OnFixedUpdate));
		KFSMState kFSMState141 = st_weldAcquireHeading;
		kFSMState141.OnLateUpdate = (KFSMCallback)Delegate.Combine(kFSMState141.OnLateUpdate, new KFSMCallback(weld_acquireHeading_OnLateUpdate));
		st_weldAcquireHeading.OnLeave = weld_acquireHeading_OnLeave;
		fsm.AddState(st_weldAcquireHeading);
		st_weld = new KFSMState("Welding part");
		st_weld.OnEnter = weld_OnEnter;
		st_weld.OnFixedUpdate = weld_OnFixedUpdate;
		st_weld.OnLeave = weld_OnLeave;
		fsm.AddState(st_weld);
		st_playing_golf = new KFSMState("Playing Golf");
		st_playing_golf.OnEnter = playing_Golf_OnEnter;
		KFSMState kFSMState142 = st_playing_golf;
		kFSMState142.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState142.OnUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState143 = st_playing_golf;
		kFSMState143.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState143.OnUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState144 = st_playing_golf;
		kFSMState144.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState144.OnUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState145 = st_playing_golf;
		kFSMState145.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState145.OnUpdate, new KFSMCallback(PlayingGolfPhysicalBall));
		KFSMState kFSMState146 = st_playing_golf;
		kFSMState146.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState146.OnUpdate, new KFSMCallback(FinishedPlayingGolf));
		KFSMState kFSMState147 = st_playing_golf;
		kFSMState147.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState147.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_playing_golf);
		st_smashing_banana = new KFSMState("Smashing Banana");
		st_smashing_banana.OnEnter = smashing_banana_OnEnter;
		KFSMState kFSMState148 = st_smashing_banana;
		kFSMState148.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState148.OnUpdate, new KFSMCallback(correctGroundedRotation));
		KFSMState kFSMState149 = st_smashing_banana;
		kFSMState149.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState149.OnUpdate, new KFSMCallback(UpdateMovement));
		KFSMState kFSMState150 = st_smashing_banana;
		kFSMState150.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState150.OnUpdate, new KFSMCallback(UpdateHeading));
		KFSMState kFSMState151 = st_smashing_banana;
		kFSMState151.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState151.OnUpdate, new KFSMCallback(SmashingBananaParticles));
		KFSMState kFSMState152 = st_smashing_banana;
		kFSMState152.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState152.OnUpdate, new KFSMCallback(FinishedSmashingBanana));
		KFSMState kFSMState153 = st_smashing_banana;
		kFSMState153.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState153.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_smashing_banana);
		st_spinning_wingnut = new KFSMState("Spinning Wingnut");
		st_spinning_wingnut.OnEnter = spinning_Wingnut_OnEnter;
		KFSMState kFSMState154 = st_spinning_wingnut;
		kFSMState154.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState154.OnUpdate, new KFSMCallback(ApplyWingnutTorque));
		KFSMState kFSMState155 = st_spinning_wingnut;
		kFSMState155.OnUpdate = (KFSMCallback)Delegate.Combine(kFSMState155.OnUpdate, new KFSMCallback(FinishedSpinningWingnut));
		KFSMState kFSMState156 = st_spinning_wingnut;
		kFSMState156.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState156.OnFixedUpdate, new KFSMCallback(UpdateOrientationPID));
		KFSMState kFSMState157 = st_spinning_wingnut;
		kFSMState157.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState157.OnFixedUpdate, new KFSMCallback(UpdatePackAngular));
		KFSMState kFSMState158 = st_spinning_wingnut;
		kFSMState158.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState158.OnFixedUpdate, new KFSMCallback(UpdatePackLinear));
		KFSMState kFSMState159 = st_spinning_wingnut;
		kFSMState159.OnFixedUpdate = (KFSMCallback)Delegate.Combine(kFSMState159.OnFixedUpdate, new KFSMCallback(updateRagdollVelocities));
		fsm.AddState(st_spinning_wingnut);
		On_hdgAcquireStart = new KFSMEvent("Hdg Acquire Start");
		On_hdgAcquireStart.GoToStateOnEvent = st_heading_acquire;
		On_hdgAcquireStart.OnCheckCondition = (KFSMState st) => Mathf.Abs(deltaHdg) > 60f;
		fsm.AddEvent(On_hdgAcquireStart, st_bound_gr_acd, st_bound_gr_fps, st_walk_acd, st_walk_fps, st_run_acd, st_run_fps);
		On_hdgAcquireComplete = new KFSMEvent("Hdg Acquire Complete");
		On_hdgAcquireComplete.GoToStateOnEvent = st_idle_gr;
		On_hdgAcquireComplete.OnCheckCondition = (KFSMState st) => Mathf.Abs(deltaHdg) < 30f;
		fsm.AddEvent(On_hdgAcquireComplete, st_heading_acquire);
		On_MoveAcd = new KFSMEvent("Move (Arcade)");
		On_MoveAcd.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_MoveAcd.GoToStateOnEvent = st_walk_acd;
		On_MoveAcd.OnCheckCondition = (KFSMState currentState) => tgtRpos != Vector3.zero && base.vessel.mainBody.GeeASL > (double)minWalkingGee && !CharacterFrameMode && !EVAConstructionModeController.MovementRestricted && SurfaceContact();
		fsm.AddEvent(On_MoveAcd, st_idle_gr, st_idle_b_gr, st_walk_fps, st_run_fps, st_enteringConstruction, st_exitingConstruction);
		On_MoveFPS = new KFSMEvent("Move (FPS)");
		On_MoveFPS.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_MoveFPS.GoToStateOnEvent = st_walk_fps;
		On_MoveFPS.OnCheckCondition = (KFSMState currentState) => tgtRpos != Vector3.zero && base.vessel.mainBody.GeeASL > (double)minWalkingGee && CharacterFrameMode && !EVAConstructionModeController.MovementRestricted && SurfaceContact();
		fsm.AddEvent(On_MoveFPS, st_idle_gr, st_idle_b_gr, st_walk_acd, st_run_acd, st_enteringConstruction, st_exitingConstruction);
		On_MoveLowG_Acd = new KFSMEvent("Move Low G (Arcade)");
		On_MoveLowG_Acd.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_MoveLowG_Acd.GoToStateOnEvent = st_bound_gr_acd;
		On_MoveLowG_Acd.OnCheckCondition = (KFSMState currentState) => tgtRpos != Vector3.zero && base.vessel.mainBody.GeeASL <= (double)minWalkingGee && !CharacterFrameMode && !EVAConstructionModeController.MovementRestricted;
		On_MoveLowG_Acd.OnEvent = delegate
		{
		};
		fsm.AddEvent(On_MoveLowG_Acd, st_idle_gr, st_idle_b_gr, st_bound_gr_fps, st_enteringConstruction, st_exitingConstruction);
		On_MoveLowG_fps = new KFSMEvent("Move Low G (FPS)");
		On_MoveLowG_fps.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_MoveLowG_fps.GoToStateOnEvent = st_bound_gr_fps;
		On_MoveLowG_fps.OnCheckCondition = (KFSMState currentState) => tgtRpos != Vector3.zero && base.vessel.mainBody.GeeASL <= (double)minWalkingGee && CharacterFrameMode && !EVAConstructionModeController.MovementRestricted;
		On_MoveLowG_fps.OnEvent = delegate
		{
		};
		fsm.AddEvent(On_MoveLowG_fps, st_idle_gr, st_idle_b_gr, st_bound_gr_acd, st_enteringConstruction, st_exitingConstruction);
		On_bound = new KFSMEvent("Low G bound");
		On_bound.GoToStateOnEvent = st_bound_fl;
		On_bound.updateMode = KFSMUpdateMode.UPDATE;
		On_bound.OnCheckCondition = (KFSMState currentState) => SurfaceContact() && tgtRpos != Vector3.zero && Mathf.Abs(deltaHdg) < 5f && !base.vessel.packed && fsm.TimeAtCurrentState > (double)Mathf.Min(boundFrequency, lastBoundStep) && !EVAConstructionModeController.MovementRestricted;
		On_bound.OnEvent = delegate
		{
			deltaHdg = 0f;
			boundForce = Mathf.Round(base.part.mass * boundForceMassFactor * 100f) / 100f;
			Vector3 direction = base.transform.InverseTransformDirection(this.GetComponentCached(ref _rigidbody).velocity);
			direction.y = 0f;
			this.GetComponentCached(ref _rigidbody).velocity = base.transform.TransformDirection(direction);
			Vector3 vector = Quaternion.FromToRotation(tgtRpos.normalized, this.GetComponentCached(ref _rigidbody).velocity.normalized) * (base.transform.up * boundForce + tgtRpos.normalized * boundSpeed * massMultiplier);
			this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
			base.part.AddImpulse(vector);
		};
		fsm.AddEvent(On_bound, st_bound_gr_acd, st_bound_gr_fps);
		halfHeight -= Physics.defaultContactOffset * 2f;
		On_bound_land = new KFSMEvent("Low G bound land");
		On_bound_land.updateMode = KFSMUpdateMode.UPDATE;
		On_bound_land.OnCheckCondition = (KFSMState currentState) => base.vessel.Splashed || (Math.Abs(base.vessel.GetHeightFromSurface()) < halfHeight + boundThreshold && fsm.TimeAtCurrentState > 0.10000000149011612) || (SurfaceContact() && fsm.TimeAtCurrentState > 0.5);
		On_bound_land.OnEvent = delegate
		{
			boundLeftFoot = !boundLeftFoot;
			On_bound_land.GoToStateOnEvent = (CharacterFrameMode ? st_bound_gr_fps : st_bound_gr_acd);
		};
		fsm.AddEvent(On_bound_land, st_bound_fl);
		On_bound_fall = new KFSMEvent("Low G Bound Fall");
		On_bound_fall.GoToStateOnEvent = st_idle_fl;
		On_bound_fall.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_bound_fall.OnCheckCondition = (KFSMState currentState) => !SurfaceOrSplashed() && fsm.TimeAtCurrentState > (double)(lastBoundStep * boundFallThreshold) && !EVAConstructionModeController.MovementRestricted;
		On_bound_fall.OnEvent = delegate
		{
			ModifyBodyColliderHeight(0f);
		};
		fsm.AddEvent(On_bound_fall, st_bound_fl);
		On_startRun = new KFSMEvent("Start Run");
		On_startRun.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_startRun.OnCheckCondition = (KFSMState currentState) => GameSettings.EVA_Run.GetKey() && !JetpackDeployed && VesselUnderControl && !EVAConstructionModeController.MovementRestricted;
		On_startRun.OnEvent = delegate
		{
			if (base.vessel.mainBody.GeeASL > (double)minWalkingGee)
			{
				On_startRun.GoToStateOnEvent = (CharacterFrameMode ? st_run_fps : st_run_acd);
			}
		};
		fsm.AddEvent(On_startRun, st_walk_acd, st_walk_fps, st_enteringConstruction, st_exitingConstruction);
		On_endRun = new KFSMEvent("End Run");
		On_endRun.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_endRun.OnCheckCondition = (KFSMState currentState) => !GameSettings.EVA_Run.GetKey();
		On_endRun.OnEvent = delegate
		{
			On_endRun.GoToStateOnEvent = (CharacterFrameMode ? st_walk_fps : st_walk_acd);
		};
		fsm.AddEvent(On_endRun, st_run_acd, st_run_fps);
		On_stop = new KFSMEvent("Stop");
		On_stop.GoToStateOnEvent = st_idle_gr;
		On_stop.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_stop.OnCheckCondition = (KFSMState currentState) => tgtRpos == Vector3.zero;
		fsm.AddEvent(On_stop, st_walk_acd, st_run_acd, st_bound_gr_acd, st_bound_gr_fps, st_walk_fps, st_run_fps);
		On_jump_start = new KFSMEvent("Jump Start");
		On_jump_start.GoToStateOnEvent = st_jump;
		On_jump_start.OnCheckCondition = (KFSMState currentState) => GameSettings.EVA_Jump.GetKey() && VesselUnderControl && !PartPlacementMode && !EVAConstructionModeController.MovementRestricted;
		fsm.AddEvent(On_jump_start, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_run_acd, st_run_fps, st_bound_gr_acd, st_bound_gr_fps);
		On_jump_complete = new KFSMTimedEvent("Jump Launch", 0.3);
		On_jump_complete.GoToStateOnEvent = st_idle_fl;
		On_jump_complete.OnEvent = delegate
		{
			base.part.AddImpulse(base.transform.up * Mathf.Pow(base.part.mass / PhysicsGlobals.PerCommandSeatReduction, jumpMultiplier) * maxJumpForce + base.transform.forward * tgtSpeed * massMultiplier);
		};
		fsm.AddEvent(On_jump_complete, st_jump);
		On_land_start = new KFSMEvent("Landing");
		On_land_start.OnCheckCondition = (KFSMState currentState) => SurfaceContact() && fsm.FramesInCurrentState > 2;
		On_land_start.OnEvent = delegate
		{
			if (!(base.vessel.rb_velocity.sqrMagnitude > stumbleThreshold * stumbleThreshold) && Vector3.Dot(base.transform.up, lastCollisionDirection) <= 0.9f)
			{
				if (!(Mathf.Abs(Vector3.Dot(fUp, lastCollisionNormal)) > 0.4f) && Planetarium.GetUniversalTime() - lastCollisionTime <= 0.4)
				{
					On_land_start.GoToStateOnEvent = st_idle_fl;
				}
				else if (Vector3.Dot(base.transform.up, fUp) > 0.8f)
				{
					On_land_start.GoToStateOnEvent = st_land;
				}
				else
				{
					On_land_start.GoToStateOnEvent = st_recover;
				}
			}
			else
			{
				On_land_start.GoToStateOnEvent = st_ragdoll;
			}
			cmdDir = this.GetComponentCached(ref _rigidbody).velocity.normalized;
		};
		fsm.AddEvent(On_land_start, st_idle_fl, st_semi_deployed_parachute, st_fully_deployed_parachute);
		On_land_complete = new KFSMTimedEvent("Landed", 0.10000000149011612);
		On_land_complete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_land_complete, st_land);
		On_fall = new KFSMEvent("Fall");
		On_fall.GoToStateOnEvent = st_idle_fl;
		On_fall.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_fall.OnCheckCondition = (KFSMState currentState) => Math.Abs(base.vessel.heightFromTerrain) > onFallHeightFromTerrain && !SurfaceOrSplashed();
		fsm.AddEvent(On_fall, st_idle_gr, st_idle_b_gr, st_walk_acd, st_run_acd, st_swim_idle, st_swim_fwd);
		On_stumble = new KFSMEvent("Stumble");
		On_stumble.GoToStateOnEvent = st_ragdoll;
		On_stumble.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		fsm.AddEventExcluding(On_stumble, st_ragdoll, st_grappled, st_clamber_acquireP2, st_clamber_acquireP3);
		On_recover_start = new KFSMEvent("Recover Start");
		On_recover_start.GoToStateOnEvent = st_recover;
		On_recover_start.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_recover_start.OnCheckCondition = (KFSMState currentState) => CanRecover() && fsm.TimeAtCurrentState > 0.20000000298023224;
		fsm.AddEvent(On_recover_start, st_ragdoll);
		On_recover_complete = new KFSMTimedEvent("Recover End", 0.5);
		On_recover_complete.OnEvent = delegate
		{
			isRagdoll = false;
			lastTgtSpeed = 0f;
			CollisionManager.UpdateAllColliders();
			if (SurfaceContact())
			{
				On_recover_complete.GoToStateOnEvent = st_idle_gr;
			}
			else if (base.vessel.Splashed)
			{
				On_recover_complete.GoToStateOnEvent = st_swim_idle;
			}
			else
			{
				On_recover_complete.GoToStateOnEvent = st_idle_fl;
			}
		};
		fsm.AddEvent(On_recover_complete, st_recover);
		On_packToggle = new KFSMEvent("Pack Toggle");
		On_packToggle.updateMode = KFSMUpdateMode.UPDATE;
		On_packToggle.OnCheckCondition = delegate
		{
			if (GameSettings.EVA_TogglePack.GetKeyDown() && VesselUnderControl && !PartPlacementMode)
			{
				On_packToggle.GoToStateOnEvent = fsm.CurrentState;
				return true;
			}
			return false;
		};
		On_packToggle.OnEvent = delegate
		{
			ToggleJetpack(!JetpackDeployed);
		};
		fsm.AddEvent(On_packToggle, st_idle_gr, st_idle_b_gr, st_idle_fl, st_weldAcquireHeading);
		On_feet_wet = new KFSMEvent("Feet Wet");
		On_feet_wet.OnCheckCondition = (KFSMState st) => base.vessel.Splashed && !SurfaceContact();
		On_feet_wet.OnEvent = delegate
		{
			if (this.GetComponentCached(ref _rigidbody).velocity.sqrMagnitude < stumbleThreshold * stumbleThreshold)
			{
				if (Vector3.Dot(base.transform.up, fUp) > 0.75f)
				{
					On_feet_wet.GoToStateOnEvent = st_swim_idle;
				}
				else
				{
					On_feet_wet.GoToStateOnEvent = st_recover;
				}
			}
			else
			{
				On_feet_wet.GoToStateOnEvent = st_ragdoll;
			}
		};
		fsm.AddEvent(On_feet_wet, st_idle_fl, st_idle_gr, st_idle_b_gr, st_run_acd, st_walk_acd);
		On_feet_dry = new KFSMEvent("Feet Dry");
		On_feet_dry.OnCheckCondition = (KFSMState st) => SurfaceContact();
		On_feet_dry.OnEvent = delegate
		{
			if (Vector3.Dot(base.transform.up, fUp) > 0.3f)
			{
				On_feet_dry.GoToStateOnEvent = st_idle_fl;
			}
			else
			{
				On_feet_dry.GoToStateOnEvent = st_recover;
			}
		};
		fsm.AddEvent(On_feet_dry, st_swim_idle, st_swim_fwd);
		On_swim_fwd = new KFSMEvent("Swim Forward");
		On_swim_fwd.GoToStateOnEvent = st_swim_fwd;
		On_swim_fwd.OnCheckCondition = (KFSMState st) => tgtRpos != Vector3.zero;
		fsm.AddEvent(On_swim_fwd, st_swim_idle);
		On_swim_stop = new KFSMEvent("Swim Stop");
		On_swim_stop.GoToStateOnEvent = st_swim_idle;
		On_swim_stop.OnCheckCondition = (KFSMState st) => tgtRpos == Vector3.zero;
		fsm.AddEvent(On_swim_stop, st_swim_fwd);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			On_control_panel_search = new KFSMEvent("Control Panel Search");
			On_control_panel_search.updateMode = KFSMUpdateMode.FIXEDUPDATE;
			On_control_panel_search.OnCheckCondition = delegate
			{
				if (FindControlPanel(controlPanelStandoff, controlPanelReach) && VesselUnderControl && GameSettings.EVA_Use.GetKey())
				{
					RandomControlPanelAnim();
					return true;
				}
				return false;
			};
			On_control_panel_search.GoToStateOnEvent = st_controlPanel_identified;
			fsm.AddEvent(On_control_panel_search, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_heading_acquire, st_run_acd, st_run_fps, st_bound_gr_acd, st_idle_fl, st_bound_gr_fps, st_ladder_idle, st_controlPanel_identified);
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			On_control_panel_interacting = new KFSMEvent("Control Panel Interacting");
			On_control_panel_interacting.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			On_control_panel_interacting.GoToStateOnEvent = st_idle_gr;
			fsm.AddEvent(On_control_panel_interacting, st_controlPanel_identified, st_idle_gr);
		}
		On_ladderGrabStart = new KFSMEvent("Ladder Grab Start");
		On_ladderGrabStart.GoToStateOnEvent = st_ladder_acquire;
		On_ladderGrabStart.OnCheckCondition = delegate
		{
			if (currentLadderTriggers.Count > 0 && VesselUnderControl && !EVAConstructionModeController.MovementRestricted)
			{
				PostInteractionScreenMessage(cacheAutoLOC_114130);
				if (GameSettings.EVA_Use.GetKeyDown())
				{
					return true;
				}
			}
			return false;
		};
		fsm.AddEvent(On_ladderGrabStart, st_idle_fl, st_idle_gr, st_idle_b_gr, st_swim_idle);
		On_ladderGrabComplete = new KFSMTimedEvent("Ladder Grab Complete", 1.0);
		On_ladderGrabComplete.GoToStateOnEvent = st_ladder_idle;
		fsm.AddEvent(On_ladderGrabComplete, st_ladder_acquire);
		On_ladderClimb = new KFSMEvent("Ladder Climb");
		On_ladderClimb.GoToStateOnEvent = st_ladder_climb;
		On_ladderClimb.OnCheckCondition = (KFSMState st) => !(currentLadder == null) && (!GameSettings.EVA_LADDER_CHECK_END || canClimb) && !EVAConstructionModeController.MovementRestricted && Vector3.Dot(ladderTgtRPos, currentLadder.transform.up * (invLadderAxis ? (-1f) : 1f)) > 0.3f;
		fsm.AddEvent(On_ladderClimb, st_ladder_idle, st_ladder_descend, st_ladder_end_reached);
		On_ladderDescend = new KFSMEvent("Ladder Descend");
		On_ladderDescend.GoToStateOnEvent = st_ladder_descend;
		On_ladderDescend.OnCheckCondition = (KFSMState st) => !(currentLadder == null) && (!GameSettings.EVA_LADDER_CHECK_END || canDescend) && !EVAConstructionModeController.MovementRestricted && Vector3.Dot(ladderTgtRPos, currentLadder.transform.up * (invLadderAxis ? (-1f) : 1f)) < -0.3f;
		fsm.AddEvent(On_ladderDescend, st_ladder_idle, st_ladder_climb, st_ladder_end_reached);
		On_ladderStop = new KFSMEvent("Ladder Stop");
		On_ladderStop.GoToStateOnEvent = st_ladder_idle;
		On_ladderStop.OnCheckCondition = (KFSMState st) => ladderTgtRPos == Vector3.zero;
		fsm.AddEvent(On_ladderStop, st_ladder_climb, st_ladder_descend);
		On_ladderLetGo = new KFSMEvent("Ladder Let Go");
		On_ladderLetGo.GoToStateOnEvent = st_idle_fl;
		On_ladderLetGo.OnCheckCondition = (KFSMState st) => GameSettings.EVA_Jump.GetKeyDown() && VesselUnderControl && !EVAConstructionModeController.MovementRestricted;
		On_ladderLetGo.OnEvent = delegate
		{
			currentLadderTriggers.Clear();
			currentLadder = null;
			if (currentLadderPart != null)
			{
				currentLadderPart.hasKerbalOnLadder = false;
			}
			currentLadderPart = null;
			base.vessel.UpdateLandedSplashed();
			secondaryLadder = null;
		};
		fsm.AddEvent(On_ladderLetGo, st_ladder_idle, st_ladder_climb, st_ladder_descend, st_ladder_end_reached);
		On_LadderEnd = new KFSMEvent("Ladder End");
		On_LadderEnd.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_LadderEnd.GoToStateOnEvent = st_idle_fl;
		On_LadderEnd.OnCheckCondition = (KFSMState st) => currentLadder == null || currentLadderTriggers.Count == 0 || !currentLadder.gameObject.activeInHierarchy || !currentLadder.enabled;
		On_LadderEnd.OnEvent = delegate
		{
			currentLadderTriggers.Clear();
			currentLadder = null;
			if (currentLadderPart != null)
			{
				currentLadderPart.hasKerbalOnLadder = false;
			}
			currentLadderPart = null;
			base.vessel.UpdateLandedSplashed();
			secondaryLadder = null;
		};
		fsm.AddEvent(On_LadderEnd, st_ladder_idle, st_ladder_climb, st_ladder_descend, st_ladder_acquire, st_ladder_lean, st_ladder_end_reached);
		On_LadderLeanStart = new KFSMEvent("Ladder Lean (Start)");
		On_LadderLeanStart.GoToStateOnEvent = st_ladder_lean;
		On_LadderLeanStart.OnCheckCondition = (KFSMState st) => GameSettings.EVA_Run.GetKey() && VesselUnderControl && !EVAConstructionModeController.MovementRestricted;
		fsm.AddEvent(On_LadderLeanStart, st_ladder_idle);
		On_LadderLeanEnd = new KFSMEvent("Ladder Lean (End)");
		On_LadderLeanEnd.GoToStateOnEvent = st_ladder_idle;
		On_LadderLeanEnd.OnCheckCondition = (KFSMState st) => !GameSettings.EVA_Run.GetKey();
		fsm.AddEvent(On_LadderLeanEnd, st_ladder_lean);
		On_LadderPushOff = new KFSMEvent("Ladder Push Off Start");
		On_LadderPushOff.GoToStateOnEvent = st_ladder_pushoff;
		On_LadderPushOff.OnCheckCondition = (KFSMState st) => GameSettings.EVA_Jump.GetKey() && VesselUnderControl && !EVAConstructionModeController.MovementRestricted;
		fsm.AddEvent(On_LadderPushOff, st_ladder_lean);
		On_LadderPushoff_complete = new KFSMTimedEvent("Ladder Push Off Complete", 0.3);
		On_LadderPushoff_complete.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_LadderPushoff_complete.GoToStateOnEvent = st_idle_fl;
		On_LadderPushoff_complete.OnEvent = delegate
		{
			if (ladderTgtRPos != Vector3.zero)
			{
				base.part.AddImpulse(ladderTgtRPos * ladderPushoffForce);
			}
			else
			{
				base.part.AddImpulse(-base.transform.forward * ladderPushoffForce);
			}
		};
		fsm.AddEvent(On_LadderPushoff_complete, st_ladder_pushoff);
		On_ladderEndReached = new KFSMEvent("Ladder End Reached");
		On_ladderEndReached.GoToStateOnEvent = st_ladder_end_reached;
		On_ladderEndReached.OnCheckCondition = (KFSMState st) => !(currentLadder == null) && GameSettings.EVA_LADDER_CHECK_END && currentSpd == 0f && (!canClimb || !canDescend);
		fsm.AddEvent(On_ladderEndReached, st_ladder_idle, st_ladder_descend, st_ladder_climb, st_clamber_acquireP1, st_idle_fl);
		On_clamberGrabStart = new KFSMEvent("Clamber Grab Start");
		On_clamberGrabStart.GoToStateOnEvent = st_clamber_acquireP1;
		On_clamberGrabStart.updateMode = KFSMUpdateMode.UPDATE;
		On_clamberGrabStart.OnCheckCondition = delegate(KFSMState st)
		{
			if (base.vessel.heightFromTerrain == -1f)
			{
				return false;
			}
			if (((!(base.vessel.terrainAltitude < 0.0) || !base.vessel.mainBody.ocean) ? ((double)base.vessel.heightFromTerrain) : base.vessel.altitude) > (double)clamberMaxAlt)
			{
				return false;
			}
			if (PartPlacementMode)
			{
				return false;
			}
			if (FindClamberSrf(clamberStandoff, clamberReach) && VesselUnderControl && TestClamberSrf(clamberHitInfo))
			{
				if (st != st_ladder_idle && st != st_ladder_climb && st != st_ladder_end_reached)
				{
					PostInteractionScreenMessage(cacheAutoLOC_114297);
				}
				else
				{
					PostInteractionScreenMessage(cacheAutoLOC_114293);
				}
				if (GameSettings.EVA_Use.GetKey())
				{
					clamberPath = GetClamberPath(clamberStandoff, clamberReach);
					return clamberPath != null;
				}
			}
			return false;
		};
		fsm.AddEvent(On_clamberGrabStart, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_run_acd, st_run_fps, st_jump, st_land, st_bound_fl, st_idle_fl, st_swim_idle, st_swim_fwd, st_ladder_climb, st_ladder_idle, st_ladder_end_reached);
		On_clamberP1 = new KFSMEvent("Clamber Reach P1");
		On_clamberP1.GoToStateOnEvent = st_clamber_acquireP2;
		On_clamberP1.OnCheckCondition = (KFSMState st) => clamberPath != null && Animations.clamber.State.normalizedTime >= 0.2f;
		fsm.AddEvent(On_clamberP1, st_clamber_acquireP1);
		On_clamberP2 = new KFSMEvent("Clamber Reach P2");
		On_clamberP2.GoToStateOnEvent = st_clamber_acquireP3;
		On_clamberP2.OnCheckCondition = (KFSMState st) => clamberPath != null && Animations.clamber.State.normalizedTime >= 0.3f;
		fsm.AddEvent(On_clamberP2, st_clamber_acquireP2);
		On_clamberP3 = new KFSMEvent("Clamber Reach P3");
		On_clamberP3.GoToStateOnEvent = st_idle_gr;
		On_clamberP3.OnCheckCondition = (KFSMState st) => clamberPath != null && (Animations.clamber.State.normalizedTime >= Animations.clamber.end || fsm.TimeAtCurrentState > (double)(Animations.clamber.State.length * 0.5f));
		fsm.AddEvent(On_clamberP3, st_clamber_acquireP3);
		On_boardPart = new KFSMEvent("Boarding Part");
		On_boardPart.updateMode = KFSMUpdateMode.UPDATE;
		On_boardPart.OnCheckCondition = delegate(KFSMState st)
		{
			if (currentAirlockPart != null && VesselUnderControl && !EVAConstructionModeController.MovementRestricted && (st == st_ladder_idle || currentLadderTriggers.Count == 0))
			{
				PostInteractionScreenMessage(cacheAutoLOC_114358);
				if (GameSettings.EVA_Board.GetKeyUp())
				{
					return true;
				}
			}
			return false;
		};
		On_boardPart.OnEvent = delegate
		{
			On_boardPart.GoToStateOnEvent = fsm.CurrentState;
			BoardPart(currentAirlockPart);
		};
		fsm.AddEvent(On_boardPart, st_idle_fl, st_idle_gr, st_idle_b_gr, st_ladder_idle, st_swim_idle);
		On_flagPlantStart = new KFSMEvent("Flag Plant Started");
		On_flagPlantStart.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_flagPlantStart.GoToStateOnEvent = st_flagAcquireHeading;
		fsm.AddEvent(On_flagPlantStart, st_idle_gr, st_idle_b_gr);
		On_flagPlantHdgAcquire = new KFSMEvent("Flag Plant Heading Acquired");
		On_flagPlantHdgAcquire.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_flagPlantHdgAcquire.GoToStateOnEvent = st_flagPlant;
		On_flagPlantHdgAcquire.OnCheckCondition = (KFSMState st) => Mathf.Abs(deltaHdg + lastDeltaHdg) < 30f;
		fsm.AddEvent(On_flagPlantHdgAcquire, st_flagAcquireHeading);
		On_flagPlantComplete = new KFSMTimedEvent("Flag Plant Complete", Animations.flagPlant.State.length);
		On_flagPlantComplete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_flagPlantComplete, st_flagPlant);
		On_seatBoard = new KFSMEvent("Seat Board");
		On_seatBoard.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_seatBoard.GoToStateOnEvent = st_seated_cmd;
		On_seatBoard.OnEvent = delegate
		{
			base.vessel.SetRotation(kerbalSeat.SeatPivot.rotation, setPos: false);
			base.vessel.SetPosition(kerbalSeat.SeatPivot.position, usePristineCoords: true);
			base.vessel.IgnoreGForces(10);
			base.part.Couple(kerbalSeat.part);
			FlightGlobals.ForceSetActiveVessel(base.vessel);
			FlightInputHandler.ResumeVesselCtrlState(base.vessel);
			if (FlightGlobals.ActiveVessel.id == base.vessel.id)
			{
				isActiveVessel = true;
				if (updateAvatarCoroutine == null)
				{
					updateAvatarCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
				}
			}
			if (base.vessel.CurrentControlLevel == Vessel.ControlLevel.NONE)
			{
				MakeReference();
			}
			base.part.rb.drag = 0f;
		};
		fsm.AddEvent(On_seatBoard, st_idle_fl, st_idle_gr, st_idle_b_gr, st_swim_idle, st_ladder_idle);
		On_seatDeboard = new KFSMEvent("Seat Deboard");
		On_seatDeboard.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_seatDeboard.GoToStateOnEvent = st_idle_fl;
		On_seatDeboard.OnEvent = delegate
		{
			EjectFromSeat();
			FlightGlobals.ForceSetActiveVessel(base.vessel);
			FlightInputHandler.SetNeutralControls();
			if (evaChute != null && evaChute.deploymentState == ModuleParachute.deploymentStates.SEMIDEPLOYED)
			{
				On_seatDeboard.GoToStateOnEvent = st_semi_deployed_parachute;
			}
			else if (evaChute != null && evaChute.deploymentState == ModuleParachute.deploymentStates.DEPLOYED)
			{
				On_seatDeboard.GoToStateOnEvent = st_fully_deployed_parachute;
			}
			else
			{
				On_seatDeboard.GoToStateOnEvent = st_idle_fl;
			}
		};
		fsm.AddEvent(On_seatDeboard, st_seated_cmd);
		On_seatEject = new KFSMEvent("Seat Eject");
		On_seatEject.updateMode = KFSMUpdateMode.LATEUPDATE;
		On_seatEject.GoToStateOnEvent = st_ragdoll;
		On_seatEject.OnCheckCondition = (KFSMState st) => base.part.parent == null;
		fsm.AddEvent(On_seatEject, st_seated_cmd);
		On_grapple = new KFSMEvent("Grapple");
		On_grapple.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_grapple.GoToStateOnEvent = st_grappled;
		On_grapple.OnEvent = delegate
		{
		};
		fsm.AddEventExcluding(On_grapple, st_grappled);
		On_grappleRelease = new KFSMEvent("Grapple Release");
		On_grappleRelease.updateMode = KFSMUpdateMode.LATEUPDATE;
		On_grappleRelease.GoToStateOnEvent = st_ragdoll;
		On_grappleRelease.OnCheckCondition = (KFSMState st) => base.part.parent == null;
		fsm.AddEvent(On_grappleRelease, st_grappled);
		On_semi_deploy_parachute = new KFSMEvent("Semi-Deploy Parachute");
		On_semi_deploy_parachute.OnCheckCondition = delegate
		{
			if (evaChute != null && evaChute.enabled && evaChute.deploymentState == ModuleParachute.deploymentStates.STOWED && GameSettings.EVA_ChuteDeploy.GetKey() && VesselUnderControl)
			{
				if (JetpackDeployed)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004227"), 4f, ScreenMessageStyle.UPPER_CENTER);
				}
				else
				{
					evaChute.Deploy();
				}
			}
			return false;
		};
		On_semi_deploy_parachute.GoToStateOnEvent = st_semi_deployed_parachute;
		fsm.AddEvent(On_semi_deploy_parachute, st_ragdoll, st_idle_fl);
		On_fully_deploy_parachute = new KFSMEvent("Fully-Deploy Parachute");
		On_fully_deploy_parachute.GoToStateOnEvent = st_fully_deployed_parachute;
		On_grapple.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		fsm.AddEvent(On_fully_deploy_parachute, st_semi_deployed_parachute);
		On_parachute_cut = new KFSMEvent("Parachute Cut");
		On_parachute_cut.GoToStateOnEvent = st_idle_fl;
		On_grapple.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		fsm.AddEvent(On_parachute_cut, st_semi_deployed_parachute, st_fully_deployed_parachute);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			On_chopping_roc = new KFSMEvent("Chopping Roc");
			On_chopping_roc.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			On_chopping_roc.GoToStateOnEvent = st_picking_roc_sample;
			fsm.AddEvent(On_chopping_roc, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_heading_acquire, st_run_acd, st_run_fps, st_bound_gr_acd, st_idle_fl, st_bound_gr_fps, st_ladder_idle, st_picking_roc_sample);
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			On_roc_sample_stored = new KFSMEvent("Roc Sample Stored");
			On_roc_sample_stored.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			On_roc_sample_stored.GoToStateOnEvent = st_idle_gr;
			fsm.AddEvent(On_roc_sample_stored, st_idle_gr, st_picking_roc_sample);
		}
		On_constructionModeEnter = new KFSMEvent("Construction Mode Entered");
		On_constructionModeEnter.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_constructionModeEnter.GoToStateOnEvent = st_enteringConstruction;
		On_constructionModeEnter.OnCheckCondition = (KFSMState currentState) => ((InConstructionMode && weldFX != null && !weldFX.mesh.enabled) || (InConstructionMode && weldFX == null)) ? true : false;
		fsm.AddEvent(On_constructionModeEnter, st_exitingConstruction, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_run_acd, st_run_fps, st_heading_acquire, st_idle_fl);
		On_constructionModeExit = new KFSMEvent("Construction Mode Exited");
		On_constructionModeExit.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_constructionModeExit.GoToStateOnEvent = st_exitingConstruction;
		fsm.AddEvent(On_constructionModeExit, st_enteringConstruction, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_run_acd, st_run_fps, st_heading_acquire, st_idle_fl);
		On_constructionModeTrigger_gr_Complete = new KFSMEvent("Construction Mode Toggle finished on ground");
		On_constructionModeTrigger_gr_Complete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_constructionModeTrigger_gr_Complete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_constructionModeTrigger_gr_Complete, st_enteringConstruction, st_exitingConstruction);
		On_constructionModeTrigger_fl_Complete = new KFSMEvent("Construction Mode Toggle finished on space");
		On_constructionModeTrigger_fl_Complete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_constructionModeTrigger_fl_Complete.GoToStateOnEvent = st_idle_fl;
		fsm.AddEvent(On_constructionModeTrigger_fl_Complete, st_enteringConstruction, st_exitingConstruction);
		On_weldStart = new KFSMEvent("Weld Started");
		On_weldStart.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_weldStart.GoToStateOnEvent = st_weldAcquireHeading;
		fsm.AddEvent(On_weldStart, st_idle_gr, st_idle_b_gr, st_idle_fl);
		On_weldHdgAcquired = new KFSMEvent("Weld Heading Acquired");
		On_weldHdgAcquired.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		On_weldHdgAcquired.GoToStateOnEvent = st_weld;
		On_weldHdgAcquired.OnCheckCondition = delegate
		{
			Vector3 normalized = (constructionTarget.transform.position - base.transform.position).normalized;
			return Vector3.Angle((normalized - Vector3.Dot(normalized, base.transform.up) * base.transform.up).normalized, base.transform.forward) < 3f || fsm.TimeAtCurrentState > 2.0;
		};
		fsm.AddEvent(On_weldHdgAcquired, st_weldAcquireHeading);
		On_weldComplete = new KFSMTimedEvent("Weld Part Complete", Animations.weld.State.length);
		On_weldComplete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_weldComplete, st_weld);
		On_Playing_Golf = new KFSMEvent("Playing Golf");
		On_Playing_Golf.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Playing_Golf.GoToStateOnEvent = st_playing_golf;
		fsm.AddEvent(On_Playing_Golf, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_heading_acquire, st_run_acd, st_run_fps, st_bound_gr_acd, st_idle_fl, st_bound_gr_fps, st_ladder_idle, st_playing_golf);
		On_Golf_Complete = new KFSMEvent("Golf Complete");
		On_Golf_Complete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Golf_Complete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_Golf_Complete, st_idle_gr, st_playing_golf);
		On_Smashing_Banana = new KFSMEvent("Smashing Banana");
		On_Smashing_Banana.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Smashing_Banana.GoToStateOnEvent = st_smashing_banana;
		fsm.AddEvent(On_Smashing_Banana, st_idle_gr, st_idle_b_gr, st_walk_acd, st_walk_fps, st_heading_acquire, st_run_acd, st_run_fps, st_bound_gr_acd, st_idle_fl, st_bound_gr_fps, st_ladder_idle, st_smashing_banana);
		On_Banana_Complete = new KFSMEvent("Banana Complete");
		On_Banana_Complete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Banana_Complete.GoToStateOnEvent = st_idle_gr;
		fsm.AddEvent(On_Banana_Complete, st_idle_gr, st_smashing_banana);
		On_Spinning_Wingnut = new KFSMEvent("Spinning Wingnut");
		On_Spinning_Wingnut.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Spinning_Wingnut.GoToStateOnEvent = st_spinning_wingnut;
		fsm.AddEvent(On_Spinning_Wingnut, st_idle_fl, st_idle_b_gr, st_walk_acd, st_walk_fps, st_heading_acquire, st_run_acd, st_run_fps, st_bound_gr_acd, st_idle_fl, st_bound_gr_fps, st_ladder_idle, st_spinning_wingnut);
		On_Wingnut_Complete = new KFSMEvent("Wingnut Complete");
		On_Wingnut_Complete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		On_Wingnut_Complete.GoToStateOnEvent = st_idle_fl;
		fsm.AddEvent(On_Wingnut_Complete, st_idle_fl, st_spinning_wingnut);
	}

	public void OnFSMStateChange(KFSMState oldStatea, KFSMState newState, KFSMEvent fsmEvent)
	{
		if (DebugFSMState)
		{
			Debug.LogFormat("[KerbalEVA]: Part:{0}-{1} FSM State Changed, Old State:{2} New State:{3} Event:{4}", base.part.partInfo.title, base.part.persistentId, oldStatea.name, newState.name, fsmEvent.name);
		}
	}

	public void OnFSMEventCalled(KFSMEvent fsmEvent)
	{
		if (DebugFSMState)
		{
			Debug.LogFormat("[KerbalEVA]: Part:{0}-{1} FSM Event Called, Event:{2}", base.part.partInfo.title, base.part.persistentId, fsmEvent.name);
		}
	}

	public virtual void FixedUpdate()
	{
		if (!base.vessel.packed)
		{
			getCoordinateFrame();
			lastDeltaHdg = deltaHdg;
			deltaHdg = 0f;
			cmdRot = Vector3.zero;
			packLinear = Vector3.zero;
			fuelFlowRate = 0f;
			tgtRpos = Vector3.zero;
			ladderTgtRPos = Vector3.zero;
			packTgtRPos = Vector3.zero;
			packRRot = Vector3.zero;
			HandleMovementInput();
			fsm.FixedUpdateFSM();
			JetpackIsThrusting = fuelFlowRate > PropellantConsumption / 2f * (thrustPercentage * 0.01f);
			UpdatePackFuel();
			UpdateHelmetOffChecks();
			UpdateVisorPosition();
		}
	}

	public void UpdateSuitColors(object obj)
	{
		ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
		if (protoCrewMember == null)
		{
			return;
		}
		Color color = new Color(protoCrewMember.lightR, protoCrewMember.lightG, protoCrewMember.lightB);
		if (suitColorChanger != null)
		{
			if (!protoCrewMember.completedFirstEVA)
			{
				suitColorChanger.redColor = lightR;
				suitColorChanger.greenColor = lightG;
				suitColorChanger.blueColor = lightB;
			}
			else
			{
				suitColorChanger.redColor = color.r;
				suitColorChanger.greenColor = color.g;
				suitColorChanger.blueColor = color.b;
			}
			suitColorChanger.SetState(suitColorChanger.CurrentRateState);
			protoCrewMember.lightR = lightR;
			protoCrewMember.lightG = lightG;
			protoCrewMember.lightB = lightB;
		}
	}

	public virtual void Update()
	{
		if (base.vessel != null && base.vessel.vesselType == VesselType.Debris && base.part != null && base.part.parent == null)
		{
			base.vessel.vesselType = VesselType.const_11;
			if (base.part.protoModuleCrew.Count > 0)
			{
				base.vessel.vesselName = base.part.protoModuleCrew[0].name;
			}
			Debug.LogWarning("Kerbal " + base.vessel.vesselName + " was marked as debris, fixed.");
		}
		fsm.UpdateFSM();
		if (PartPlacementMode && moduleInventoryPartReference != null && moduleInventoryPartReference.SelectedPart != null)
		{
			PostInteractionScreenMessage(cacheAutoLOC_8002357);
			PostInteractionScreenMessage(cacheAutoLOC_8002358);
			if (!moduleInventoryPartReference.PlacementAllowXRotation && !moduleInventoryPartReference.PlacementAllowZRotation)
			{
				PostInteractionScreenMessage(cacheAutoLOC_8002359);
			}
			else
			{
				PostInteractionScreenMessage(cacheAutoLOC_8002360);
			}
		}
		if (base.vessel.packed)
		{
			return;
		}
		if (GameSettings.EVA_Lights.GetKeyDown())
		{
			ToggleLamp();
		}
		if (GameSettings.EVA_Helmet.GetKeyDown())
		{
			ChangeHelmet();
		}
		if (startTimer)
		{
			newidleCounter -= Time.deltaTime;
			if (newidleCounter <= 0f)
			{
				newidleCounter = newidleTime;
				startTimer = false;
				fsm.RunEvent(On_idle_b_gr);
			}
		}
		updateJetpackEffects();
		Animations.SyncAnimationLayers();
		if (kerbalCamAtmos != null && kerbalPortraitCamera != null)
		{
			Quaternion rotation = kerbalPortraitCamera.transform.rotation;
			kerbalCamAtmos.transform.rotation = rotation;
		}
		if (onLadder)
		{
			canClimb = !topLadderEnd.Reached;
			canDescend = !bottomLadderEnd.Reached;
			if (!canClimb && !canDescend)
			{
				canDescend = true;
				canClimb = true;
			}
		}
		else
		{
			canClimb = false;
			canDescend = false;
		}
	}

	public virtual void LateUpdate()
	{
		if (!base.vessel.packed)
		{
			fsm.LateUpdateFSM();
			AnchorUpdate();
		}
	}

	public virtual void HandleMovementInput()
	{
		if (!VesselUnderControl || (!SurfaceOrSplashed() && JetpackDeployed && EVAConstructionModeController.MovementRestricted))
		{
			return;
		}
		if (GameSettings.EVA_ToggleMovementMode.GetKeyDown() && !EVAConstructionModeController.MovementRestricted)
		{
			CharacterFrameModeToggle = !CharacterFrameModeToggle;
		}
		CharacterFrameMode = JetpackDeployed || CharacterFrameModeToggle;
		parachuteInput = Vector3.zero;
		if (GameSettings.EVA_forward.GetKey())
		{
			tgtRpos += (CharacterFrameMode ? base.transform.forward : fFwd);
			ladderTgtRPos += base.transform.up;
		}
		if (GameSettings.EVA_back.GetKey())
		{
			tgtRpos -= (CharacterFrameMode ? base.transform.forward : fFwd);
			ladderTgtRPos -= base.transform.up;
		}
		if (GameSettings.EVA_left.GetKey())
		{
			tgtRpos -= (CharacterFrameMode ? base.transform.right : fRgt);
			ladderTgtRPos -= base.transform.right;
		}
		if (GameSettings.EVA_right.GetKey())
		{
			tgtRpos += (CharacterFrameMode ? base.transform.right : fRgt);
			ladderTgtRPos += base.transform.right;
		}
		tgtRpos.Normalize();
		packTgtRPos += base.transform.forward * GameSettings.axis_EVA_translate_z.GetAxis();
		packTgtRPos += base.transform.right * GameSettings.axis_EVA_translate_x.GetAxis();
		packTgtRPos += base.transform.up * GameSettings.axis_EVA_translate_y.GetAxis();
		packRRot += base.transform.right * GameSettings.axis_EVA_pitch.GetAxis();
		packRRot += base.transform.up * GameSettings.axis_EVA_yaw.GetAxis();
		packRRot += base.transform.forward * GameSettings.axis_EVA_roll.GetAxis();
		if (FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL)
		{
			if (CharacterFrameMode)
			{
				tgtRpos += (base.transform.rotation * SpaceNavigator.Translation).normalized;
			}
			else
			{
				tgtRpos += Quaternion.LookRotation(fFwd, fUp) * SpaceNavigator.Translation;
			}
			if (SurfaceOrSplashed())
			{
				if (CharacterFrameMode)
				{
					tgtFwd = fFwd;
					tgtUp = fUp;
				}
				else
				{
					tgtFwd = tgtRpos;
					tgtUp = fUp;
				}
			}
			ladderTgtRPos += Quaternion.LookRotation(fFwd, fUp) * SpaceNavigator.Translation;
			packTgtRPos += Quaternion.LookRotation(fFwd, fUp) * SpaceNavigator.Translation * GameSettings.SPACENAV_FLIGHT_SENS_LIN;
		}
		if (packRRot != Vector3.zero)
		{
			manualAxisControl = true;
			cmdRot = packRRot;
		}
		if (GameSettings.EVA_Pack_forward.GetKey())
		{
			packTgtRPos += base.transform.forward;
			parachuteInput.x += 1f;
		}
		if (GameSettings.EVA_Pack_back.GetKey())
		{
			packTgtRPos -= base.transform.forward;
			parachuteInput.x -= 1f;
		}
		if (GameSettings.EVA_Pack_left.GetKey())
		{
			packTgtRPos -= base.transform.right;
			parachuteInput.y -= 1f;
		}
		if (GameSettings.EVA_Pack_right.GetKey())
		{
			packTgtRPos += base.transform.right;
			parachuteInput.y += 1f;
		}
		if (GameSettings.EVA_Pack_up.GetKey())
		{
			packTgtRPos += base.transform.up;
		}
		if (GameSettings.EVA_Pack_down.GetKey())
		{
			packTgtRPos -= base.transform.up;
		}
		manualRotation = Quaternion.identity;
		parachuteInput.x = Mathf.Max(Mathf.Min(parachuteInput.x - GameSettings.axis_EVA_pitch.GetAxis(), 1f), -1f);
		parachuteInput.y = Mathf.Max(Mathf.Min(GameSettings.axis_EVA_yaw.GetAxis() + parachuteInput.y, 1f), -1f);
		if (Input.GetMouseButton(0))
		{
			UIPartActionWindow item = UIPartActionController.Instance.GetItem(base.part);
			if (item == null || (item != null && !item.dragging))
			{
				manualRotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 57.29578f * Time.deltaTime, -base.transform.forward) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 57.29578f * Time.deltaTime, base.transform.right);
			}
		}
		if (!SurfaceOrSplashed() && JetpackDeployed)
		{
			if (GameSettings.EVA_yaw_left.GetKey())
			{
				manualRotation = Quaternion.AngleAxis((0f - turnRate) * 57.29578f * Time.deltaTime, base.transform.up);
			}
			if (GameSettings.EVA_yaw_right.GetKey())
			{
				manualRotation = Quaternion.AngleAxis(turnRate * 57.29578f * Time.deltaTime, base.transform.up);
			}
			if (SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice) && FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL)
			{
				manualRotation = Quaternion.AngleAxis(SpaceNavigator.Rotation.Pitch() * GameSettings.SPACENAV_FLIGHT_SENS_ROT, fRgt) * Quaternion.AngleAxis(SpaceNavigator.Rotation.Yaw() * GameSettings.SPACENAV_FLIGHT_SENS_ROT, fUp) * Quaternion.AngleAxis(SpaceNavigator.Rotation.Roll() * GameSettings.SPACENAV_FLIGHT_SENS_ROT, fFwd);
			}
		}
		if (manualRotation != Quaternion.identity)
		{
			if (Mathf.Acos(Vector3.Dot(tgtFwd, base.transform.forward)) < (float)Math.PI / 2f && Mathf.Acos(Vector3.Dot(tgtUp, base.transform.up)) < (float)Math.PI / 2f)
			{
				tgtUp = manualRotation * tgtUp;
				tgtFwd = manualRotation * tgtFwd;
			}
			else
			{
				tgtUp = base.transform.up;
				tgtFwd = base.transform.forward;
			}
		}
		if ((!manualAxisControl && (GameSettings.EVA_ROTATE_ON_MOVE || SurfaceOrSplashed()) && (GameSettings.EVA_forward.GetKey() || GameSettings.EVA_back.GetKey() || GameSettings.EVA_left.GetKey() || GameSettings.EVA_right.GetKey())) || GameSettings.EVA_Orient.GetKey())
		{
			manualAxisControl = false;
			if (CharacterFrameMode)
			{
				tgtFwd = fFwd;
				tgtUp = fUp;
			}
			else
			{
				tgtFwd = tgtRpos;
				tgtUp = fUp;
			}
		}
	}

	public virtual void SetWaypoint(Vector3 tgtPos)
	{
		tgtRpos = (tgtPos - base.transform.position).normalized;
		tgtFwd = tgtRpos;
		tgtUp = fUp;
	}

	public bool IsKerbalInStateAbleToDeployParachute()
	{
		if (fsm.CurrentState != st_idle_fl && fsm.CurrentState != st_ragdoll && fsm.CurrentState != st_semi_deployed_parachute && fsm.CurrentState != st_seated_cmd)
		{
			return false;
		}
		return true;
	}

	public void UpdateParachuteInCommandSeat()
	{
		if (evaChute != null && evaChute.enabled && evaChute.deploymentState == ModuleParachute.deploymentStates.STOWED && GameSettings.EVA_ChuteDeploy.GetKey() && VesselUnderControl)
		{
			if (evaChute.IsMovingFastEnoughToDeploy())
			{
				evaChute.Deploy();
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8004228"), 4f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		if (evaChute != null && evaChute.deploymentState == ModuleParachute.deploymentStates.const_4)
		{
			evaChute.AllowRepack(!evaChute.IsMovingFastEnoughToDeploy() && base.vessel.LandedOrSplashed);
		}
	}

	public void OnParachuteSemiDeployed()
	{
		fsm.RunEvent(On_semi_deploy_parachute);
	}

	public void OnSemiDeployedParachuteModeEntered(KFSMState st)
	{
		evaChute.Deploy();
		SetRagdoll(ragDoll: false);
		this.GetComponentCached(ref _animation).CrossFade(Animations.suspendedIdle, 1.2f, PlayMode.StopSameLayer);
	}

	public void UpdateSemiDeployedParachuteMovement()
	{
		evaChute.UpdateSemiDeployedParachuteMovement(parachuteInput, this.GetComponentCached(ref _rigidbody));
	}

	public void OnSemiDeployedParachuteModeLeft(KFSMState st)
	{
		if (st != st_fully_deployed_parachute)
		{
			evaChute.CutParachute();
		}
	}

	public void OnParachuteFullyDeployed()
	{
		fsm.RunEvent(On_fully_deploy_parachute);
		for (int i = 0; i < chuteLiftingSurfaces.Count; i++)
		{
			chuteLiftingSurfaces[i].moduleIsEnabled = true;
			chuteLiftingSurfaces[i].enabled = true;
		}
	}

	public void OnFullyDeployedParachuteModeEntered(KFSMState st)
	{
		this.GetComponentCached(ref _animation).CrossFade(Animations.chuteIdle, 1.2f, PlayMode.StopAll);
		Animations.chuteLeanForward.State.enabled = true;
		Animations.chuteLeanBackward.State.enabled = true;
		Animations.chuteLeanLeft.State.enabled = true;
		Animations.chuteLeanRight.State.enabled = true;
	}

	public void UpdateFullyDeployedParachuteMovement()
	{
		evaChute.UpdateFullyDeployedParachuteMovement(parachuteInput, this.GetComponentCached(ref _rigidbody));
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanForward, Mathf.Clamp01(parachuteInput.x));
		Animations.chuteLeanForward.State.normalizedTime = Mathf.Lerp(Animations.chuteLeanForward.start, Animations.chuteLeanForward.end, Animations.chuteLeanForward.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanBackward, Mathf.Clamp01(0f - parachuteInput.x));
		Animations.chuteLeanBackward.State.normalizedTime = Mathf.Lerp(Animations.chuteLeanBackward.start, Animations.chuteLeanBackward.end, Animations.chuteLeanBackward.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanLeft, Mathf.Clamp01(0f - parachuteInput.y));
		Animations.chuteLeanLeft.State.normalizedTime = Mathf.Lerp(Animations.chuteLeanLeft.start, Animations.chuteLeanLeft.end, Animations.chuteLeanLeft.State.weight);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanRight, Mathf.Clamp01(parachuteInput.y));
		Animations.chuteLeanRight.State.normalizedTime = Mathf.Lerp(Animations.chuteLeanRight.start, Animations.chuteLeanRight.end, Animations.chuteLeanRight.State.weight);
	}

	public void OnFullyDeployedParachuteModeLeft(KFSMState st)
	{
		evaChute.CutParachute();
		for (int i = 0; i < chuteLiftingSurfaces.Count; i++)
		{
			chuteLiftingSurfaces[i].moduleIsEnabled = false;
			chuteLiftingSurfaces[i].enabled = false;
			chuteLiftingSurfaces[i].DestroyLiftAndDragArrows();
		}
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanForward, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanBackward, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanLeft, 0f, 2f);
		this.GetComponentCached(ref _animation).Blend(Animations.chuteLeanRight, 0f, 2f);
	}

	public void OnParachuteCut()
	{
		if (fsm.currentStateName == fsm.CurrentState.name)
		{
			fsm.RunEvent(On_parachute_cut);
		}
		for (int i = 0; i < chuteLiftingSurfaces.Count; i++)
		{
			chuteLiftingSurfaces[i].moduleIsEnabled = false;
			chuteLiftingSurfaces[i].enabled = false;
			chuteLiftingSurfaces[i].DestroyLiftAndDragArrows();
		}
	}

	public virtual void UpdateMovement()
	{
		if (SurfaceOrSplashed() && !base.vessel.packed)
		{
			float num = (float)fsm.TimeAtCurrentState;
			num = ((num >= 0.3f) ? 1f : ((!(num > 0f)) ? 0f : (num * 3.3333333f)));
			currentSpd = Mathf.Lerp(lastTgtSpeed, tgtSpeed, num);
			if (tgtRpos != Vector3.zero)
			{
				cmdDir = Vector3.Lerp(cmdDir, CharacterFrameMode ? tgtRpos : base.transform.forward, num);
			}
			slopeRotation = Quaternion.FromToRotation(FlightGlobals.getUpAxis(), base.vessel.HeightFromSurfaceHit.normal);
			this.GetComponentCached(ref _rigidbody).velocity = cmdDir * currentSpd;
			this.GetComponentCached(ref _rigidbody).velocity = slopeRotation * this.GetComponentCached(ref _rigidbody).velocity + fUp * Vector3.Dot(this.GetComponentCached(ref _rigidbody).velocity, fUp);
		}
	}

	public virtual void UpdateHeading()
	{
		if (base.vessel.packed)
		{
			return;
		}
		if (tgtRpos != Vector3.zero)
		{
			float num = Vector3.Dot(base.transform.forward, tgtFwd);
			if (num <= -1f || !(num < 1f))
			{
				return;
			}
			deltaHdg = Mathf.Acos(num) * 57.29578f;
		}
		float num2 = Mathf.Sign((Quaternion.Inverse(base.transform.rotation) * tgtFwd).x);
		deltaHdg *= num2;
		if (Mathf.Abs(deltaHdg) < turnRate * 2f)
		{
			this.GetComponentCached(ref _rigidbody).angularVelocity = deltaHdg * 0.5f * fUp;
		}
		else
		{
			this.GetComponentCached(ref _rigidbody).angularVelocity = turnRate * num2 * fUp;
		}
	}

	public virtual void UpdateLowGBodyColliders()
	{
		if (!(boundColliderModifierCounter < boundColliderModifierThresholdTime) && tmpBoundState != null)
		{
			normalizedBoundTime = tmpBoundState.normalizedTime % 1f;
			if (normalizedBoundTime >= 0f && normalizedBoundTime < 0.25f)
			{
				colliderHeight = (boundColliderYOffset.y - boundColliderYOffset.x) * (-4f * normalizedBoundTime + 1f) + boundColliderYOffset.x;
			}
			else if (normalizedBoundTime >= 0.25f && normalizedBoundTime < 0.5f)
			{
				colliderHeight = (boundColliderYOffset.y - boundColliderYOffset.x) * (4f * normalizedBoundTime - 1f) + boundColliderYOffset.x;
			}
			else if (normalizedBoundTime >= 0.5f && normalizedBoundTime < 0.75f)
			{
				colliderHeight = (boundColliderYOffset.y - boundColliderYOffset.x) * (-4f * normalizedBoundTime + 3f) + boundColliderYOffset.x;
			}
			else
			{
				colliderHeight = (boundColliderYOffset.y - boundColliderYOffset.x) * (4f * normalizedBoundTime - 3f) + boundColliderYOffset.x;
			}
			ModifyBodyColliderHeight(colliderHeight);
		}
	}

	public virtual void ResetOrientationPID()
	{
		tgtFwd = base.transform.forward;
		tgtUp = fUp;
		integral = Vector3.zero;
		prev_error = error;
	}

	public virtual void UpdateOrientationPID()
	{
		if (!manualAxisControl)
		{
			if (thrustPercentagePIDBoost && thrustPercentage > pidBoostThreshold)
			{
				PIDBoost = 1f;
			}
			else
			{
				PIDBoost = 1f + Mathf.Pow(pidBoostThreshold - thrustPercentage, pidBoostExponent) * pidBoostMultiplier;
			}
			error = Vector3.ClampMagnitude(Vector3.Cross(base.transform.forward, tgtFwd), 0.5f) + Vector3.Cross(base.transform.up, tgtUp);
			integral = Vector3.ClampMagnitude(integral + error * Time.fixedDeltaTime, 1f);
			derivative = (error - prev_error) / Time.fixedDeltaTime;
			cmdRot = (Kp * error + Ki * integral + Kd * derivative) * PIDBoost;
			prev_error = error;
		}
	}

	public virtual void UpdatePackAngular()
	{
		if (JetpackDeployed && !(base.vessel == null) && !base.vessel.packed && cmdRot != Vector3.zero && Fuel > 0.0)
		{
			base.part.AddTorque(cmdRot * massMultiplier * rotPower * (thrustPercentage * 0.01f));
			fuelFlowRate += cmdRot.magnitude * Time.fixedDeltaTime;
		}
	}

	public virtual void correctGroundedRotation()
	{
		if (!base.vessel.packed)
		{
			Quaternion quaternion = Quaternion.FromToRotation(base.transform.up, fUp);
			if (tgtSpeed != 0f)
			{
				base.transform.position += base.transform.position - footPivot.position - quaternion * (base.transform.position - footPivot.position);
			}
			base.transform.rotation = quaternion * base.transform.rotation;
		}
	}

	public virtual void StartGroundedRotationRecover()
	{
		rd_rot = base.transform.rotation;
		rd_tgtRot = Quaternion.FromToRotation(base.transform.up, fUp) * base.transform.rotation;
	}

	public virtual void RecoverGroundedRotation(float duration)
	{
		if (!base.vessel.packed)
		{
			if ((rd_tgtRot.w == 0f && rd_tgtRot.x == 0f && rd_tgtRot.y == 0f && rd_tgtRot.z == 0f) || float.IsNaN(rd_tgtRot.w + rd_tgtRot.x + rd_tgtRot.y + rd_tgtRot.z))
			{
				Quaternion quaternion = rd_rot;
				StartGroundedRotationRecover();
				rd_rot = quaternion;
			}
			base.transform.rotation = Quaternion.Lerp(rd_rot, rd_tgtRot, Mathf.InverseLerp(0f, duration, (float)Math.Max(0.001, fsm.TimeAtCurrentState)));
		}
	}

	public virtual void SetupJetpackEffects()
	{
		PitchPos = base.part.findFxGroup("Pitch+");
		PitchNeg = base.part.findFxGroup("Pitch-");
		YawPos = base.part.findFxGroup("Yaw+");
		YawNeg = base.part.findFxGroup("Yaw-");
		RollPos = base.part.findFxGroup("Roll+");
		RollNeg = base.part.findFxGroup("Roll-");
		xPos = base.part.findFxGroup("X+");
		xNeg = base.part.findFxGroup("X-");
		yPos = base.part.findFxGroup("Y+");
		yNeg = base.part.findFxGroup("Y-");
		zPos = base.part.findFxGroup("Z+");
		zNeg = base.part.findFxGroup("Z-");
		SetFXGroupMinMaxPower();
	}

	public void SetFXGroupMinMaxPower()
	{
		PitchPos.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		PitchNeg.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		YawPos.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		YawNeg.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		RollPos.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		RollNeg.SetVisualMinMax(rotFXMinPower, rotFXMaxPower);
		xPos.SetVisualMinMax(linFXMinPower, linFXMaxPower);
		xNeg.SetVisualMinMax(linFXMinPower, linFXMaxPower);
		yPos.SetVisualMinMax(linFXMinPower, linFXMaxPower);
		yNeg.SetVisualMinMax(linFXMinPower, linFXMaxPower);
		zPos.SetVisualMinMax(linFXMinPower, linFXMaxPower);
		zNeg.SetVisualMinMax(linFXMinPower, linFXMaxPower);
	}

	public virtual void updateJetpackEffects()
	{
		if (!(this == null) && !(base.part == null))
		{
			SetFXGroupMinMaxPower();
			int count = base.part.fxGroups.Count;
			while (count-- > 0)
			{
				base.part.fxGroups[count].Unlatch();
			}
			if (JetpackDeployed && !base.vessel.packed && !isRagdoll && Fuel > 0.0)
			{
				Vector3 vector = Quaternion.Inverse(base.transform.rotation) * packLinear;
				xPos.SetLatch(vector.x > linFXLatch);
				xNeg.SetLatch(vector.x < 0f - linFXLatch);
				yPos.SetLatch(vector.y > linFXLatch);
				yNeg.SetLatch(vector.y < 0f - linFXLatch);
				zPos.SetLatch(vector.z > linFXLatch);
				zNeg.SetLatch(vector.z < 0f - linFXLatch);
				xPos.SetPowerLatch(vector.x);
				xNeg.SetPowerLatch(0f - vector.x);
				yPos.SetPowerLatch(vector.y);
				yNeg.SetPowerLatch(0f - vector.y);
				zPos.SetPowerLatch(vector.z);
				zNeg.SetPowerLatch(0f - vector.z);
				Vector3 vector2 = Quaternion.Inverse(base.transform.rotation) * Vector3.ClampMagnitude(cmdRot * (thrustPercentage * 0.01f), 1f);
				PitchPos.SetLatch(vector2.x > rotFXLatch);
				PitchNeg.SetLatch(vector2.x < 0f - rotFXLatch);
				YawPos.SetLatch(vector2.y > rotFXLatch);
				YawNeg.SetLatch(vector2.y < 0f - rotFXLatch);
				RollPos.SetLatch(vector2.z > rotFXLatch);
				RollNeg.SetLatch(vector2.z < 0f - rotFXLatch);
				PitchPos.SetPowerLatch(vector2.x);
				PitchNeg.SetPowerLatch(0f - vector2.x);
				YawPos.SetPowerLatch(vector2.y);
				YawNeg.SetPowerLatch(0f - vector2.y);
				RollPos.SetPowerLatch(vector2.z);
				RollNeg.SetPowerLatch(0f - vector2.z);
			}
		}
	}

	public virtual void ToggleLamp()
	{
		if (FlightGlobals.ActiveVessel == base.vessel)
		{
			if (lampOn)
			{
				lampOn = false;
				headLamp.SetActive(value: false);
			}
			else
			{
				lampOn = true;
				headLamp.SetActive(value: true);
			}
			if (suitColorChanger != null)
			{
				KSPActionParam param = new KSPActionParam(KSPActionGroup.Light, (!lampOn) ? KSPActionType.Deactivate : KSPActionType.Activate);
				suitColorChanger.ToggleAction(param);
			}
		}
	}

	public virtual void ToggleJetpack()
	{
		if (HasJetpack && FlightGlobals.ActiveVessel == base.vessel && VesselUnderControl)
		{
			On_packToggle.GoToStateOnEvent = fsm.CurrentState;
			fsm.RunEvent(On_packToggle);
		}
	}

	public virtual void ToggleJetpack(bool packState)
	{
		if (!HasJetpack && packState)
		{
			JetpackDeployed = false;
			return;
		}
		if (packState && !JetpackDeployed)
		{
			Animations.packExtend.State.time = 0.1f;
			this.GetComponentCached(ref _animation).Play(Animations.packExtend, PlayMode.StopAll);
			this.GetComponentCached(ref _animation).Blend(Animations.packExtend, 1f, 0.5f);
		}
		if (!packState && JetpackDeployed)
		{
			Animations.packStow.State.time = 0.1f;
			this.GetComponentCached(ref _animation).Play(Animations.packStow, PlayMode.StopAll);
			this.GetComponentCached(ref _animation).Blend(Animations.packStow, 1f, 0.5f);
		}
		JetpackDeployed = packState;
		if (InConstructionMode && !JetpackDeployed && SurfaceContact())
		{
			this.GetComponentCached(ref _animation).Blend(Animations.weldGunLift, 1f, 0.1f);
		}
	}

	public virtual void UpdatePackLinear()
	{
		if (JetpackDeployed && !base.vessel.packed && !isRagdoll && !EVAConstructionModeController.MovementRestricted)
		{
			packLinear = packTgtRPos * (thrustPercentage * 0.01f);
			if (packLinear != Vector3.zero && Fuel > 0.0)
			{
				base.part.AddForce(packLinear * linPower);
				fuelFlowRate += packLinear.magnitude * Time.fixedDeltaTime;
			}
		}
	}

	public virtual void UpdatePackFuel()
	{
		if (CheatOptions.InfinitePropellant)
		{
			fuelFlowRate = 0f;
		}
		else
		{
			if (propellantResource == null)
			{
				return;
			}
			double num = PropellantConsumption * fuelFlowRate;
			base.part.TransferResource(propellantResource, 0.0 - num, base.part);
			for (int i = 0; i < inventoryPropellantResources.Count; i++)
			{
				if (inventoryPropellantResources[i].pPResourceSnapshot.amount <= num)
				{
					num -= inventoryPropellantResources[i].pPResourceSnapshot.amount;
					inventoryPropellantResources[i].pPResourceSnapshot.amount = 0.0;
				}
				else
				{
					inventoryPropellantResources[i].pPResourceSnapshot.amount -= num;
					num = 0.0;
				}
				inventoryPropellantResources[i].pPResourceSnapshot.UpdateConfigNodeAmounts();
				if (num <= 0.0)
				{
					break;
				}
			}
		}
	}

	public virtual void getCoordinateFrame()
	{
		fUp = (FlightGlobals.ready ? (FlightCamera.fetch.getReferenceFrame() * Vector3.up) : Vector3.up);
		fFwd = Vector3.ProjectOnPlane(FlightCamera.fetch.mainCamera.transform.forward, fUp).normalized;
		fRgt = Vector3.Cross(fUp, fFwd);
	}

	public virtual void drawCoordinateFrame()
	{
		Debug.DrawRay(base.transform.position, fFwd, Color.blue);
		Debug.DrawRay(base.transform.position, -fFwd, Color.green);
		Debug.DrawRay(base.transform.position, fRgt, Color.cyan);
		Debug.DrawRay(base.transform.position, -fRgt, Color.green);
		Debug.DrawRay(base.transform.position, fUp, Color.grey);
	}

	[KSPEvent(advancedTweakable = false, guiActiveUncommand = false, guiActiveUnfocused = false, externalToEVAOnly = false, guiActive = false, unfocusedRange = float.MaxValue, guiName = "#autoLOC_6011014")]
	public void PickUpROC()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && Animations.rockSample.State != null)
		{
			if (availableROC != null)
			{
				experimentROC = availableROC;
				fsm.RunEvent(On_chopping_roc);
				hammerAnimTimer = 0f;
				base.Events["PickUpROC"].guiActive = false;
			}
			else
			{
				base.Events["PickUpROC"].guiActive = false;
			}
		}
	}

	public void OnROCExperimentFinished(ScienceData experimentData)
	{
		if (experimentROC != null && experimentROC.smallROC && experimentData.subjectID.Contains("ROCScience"))
		{
			experimentROC.PickUpROC();
			base.Events["PickUpROC"].guiActive = false;
			experimentROC = null;
		}
	}

	public void OnROCExperimentReset(ScienceData experimentData)
	{
		if (experimentROC != null && experimentROC.smallROC && experimentData.subjectID.Contains("ROCScience"))
		{
			base.Events["PickUpROC"].guiActive = true;
		}
	}

	public virtual void OnCollisionEnter(Collision c)
	{
		if (base.vessel.packed || !Ready)
		{
			return;
		}
		base.part.HandleCollision(c);
		RemoveRBAnchor();
		if (c.gameObject.layer == 0 || c.gameObject.layer == 15 || c.gameObject.layer == 19)
		{
			Array.Sort(c.contacts, CompareContactsByNormalToSurface);
			lastCollisionDirection = (c.contacts[0].point - base.transform.position).normalized;
			lastCollisionNormal = c.contacts[0].normal;
			Debug.DrawRay(base.transform.position, lastCollisionDirection, Color.red, 3f);
			Debug.DrawRay(base.transform.position, lastCollisionNormal, Color.yellow, 3f);
			if (!isSelfCollision(c))
			{
				lastCollisionTime = Planetarium.GetUniversalTime();
			}
			CalculateGroundLevelAngle();
		}
		if (base.part.State != PartStates.DEAD && c.relativeVelocity.sqrMagnitude > stumbleThreshold * stumbleThreshold)
		{
			fsm.RunEvent(On_stumble);
		}
		if (c.gameObject.CompareTag("ROC"))
		{
			GameObject gameObject = c.gameObject;
			availableROC = gameObject.GetComponentInParent<GClass0>();
			if (availableROC != null && availableROC.smallROC)
			{
				base.Events["PickUpROC"].guiActive = true;
				base.Events["PickUpROC"].guiName = Localizer.Format("#autoLOC_6011014", availableROC.displayName);
			}
		}
	}

	public virtual int CompareContactsByNormalToSurface(ContactPoint c1, ContactPoint c2)
	{
		if (Mathf.Abs(Vector3.Dot(c1.normal, base.vessel.upAxis)) > Mathf.Abs(Vector3.Dot(c2.normal, base.vessel.upAxis)))
		{
			return -1;
		}
		return 1;
	}

	public virtual void OnCollisionStay(Collision c)
	{
		base.part.LandedCollisionChecks(c);
	}

	public virtual void OnCollisionExit(Collision c)
	{
		base.part.OnCollisionExit(c);
		if (c.gameObject.CompareTag("ROC"))
		{
			base.Events["PickUpROC"].guiActive = false;
			availableROC = null;
		}
	}

	public virtual bool isSelfCollision(Collision c)
	{
		int num = ragdollNodes.Length;
		do
		{
			if (num-- > 0)
			{
				continue;
			}
			int num2 = characterColliders.Length;
			do
			{
				if (num2-- > 0)
				{
					continue;
				}
				int num3 = otherRagdollColliders.Length;
				do
				{
					if (num3-- <= 0)
					{
						return false;
					}
				}
				while (!(otherRagdollColliders[num3] == c.collider));
				return true;
			}
			while (!(characterColliders[num2] == c.collider));
			return true;
		}
		while (!(c.collider == ragdollNodes[num].collider));
		return true;
	}

	public virtual bool SurfaceContact()
	{
		if (!base.vessel.Landed)
		{
			return GetL19Contact();
		}
		return true;
	}

	public virtual bool SurfaceOrSplashed()
	{
		if (!base.vessel.LandedOrSplashed)
		{
			return GetL19Contact();
		}
		return true;
	}

	public virtual bool GetL19Contact()
	{
		if (!base.vessel.Splashed && base.vessel.heightFromTerrain < 1000f && base.vessel.srfSpeed < 10.0)
		{
			int count = base.part.currentCollisions.Count;
			while (count-- > 0)
			{
				if (base.part.currentCollisions.KeyAt(count) != null && base.part.currentCollisions.KeyAt(count).gameObject.layer == 19)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void AnchorUpdate()
	{
		if (base.vessel.loaded && !base.vessel.packed && base.vessel.Landed && base.vessel.rb_velocity.sqrMagnitude < 0.1f && (fsm.CurrentState == st_idle_gr || fsm.CurrentState == st_idle_b_gr || fsm.CurrentState == st_enteringConstruction || fsm.CurrentState == st_exitingConstruction || fsm.CurrentState == st_weld))
		{
			if (kerbalAnchorTimeCounter < kerbalAnchorTimeThreshold)
			{
				kerbalAnchorTimeCounter += Time.deltaTime;
			}
			else if (base.vessel.HeightFromSurfaceHit.distance != 0f && base.vessel.HeightFromSurfaceHit.distance > onFallHeightFromTerrain)
			{
				RemoveRBAnchor();
			}
			else if (base.vessel.HeightFromSurfaceHit.distance != 0f && base.vessel.VesselSurface != null && !base.vessel.VesselSurface.IsAnchored)
			{
				RemoveRBAnchor();
			}
			else if (JetpackDeployed && JetpackIsThrusting)
			{
				RemoveRBAnchor();
			}
			else if (base.vessel.GroundLevelAngle == -1f)
			{
				CalculateGroundLevelAngle();
				RemoveRBAnchor();
			}
			else if (base.vessel.GroundLevelAngle < GameSettings.EVA_MAX_SLOPE_ANGLE)
			{
				AddRBAnchor();
			}
			else
			{
				RemoveRBAnchor();
			}
		}
		else
		{
			RemoveRBAnchor();
		}
	}

	public void AddRBAnchor()
	{
		if (!isAnchored)
		{
			isAnchored = true;
			anchorJoint = base.gameObject.AddComponent<FixedJoint>();
		}
	}

	public void RemoveRBAnchor()
	{
		if (isAnchored)
		{
			isAnchored = false;
			UnityEngine.Object.Destroy(anchorJoint);
			kerbalAnchorTimeCounter = 0f;
		}
	}

	public virtual void SetupRagdoll(Part part)
	{
		if (!this.GetComponentCached(ref _rigidbody))
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		int num = ragdollNodes.Length;
		for (int i = 0; i < num; i++)
		{
			KerbalRagdollNode obj = ragdollNodes[i];
			obj.go.AddComponent<RDPartCollisionHandler>().eva = this;
			obj.rb.mass *= massMultiplier;
			obj.rb.useGravity = false;
			CharacterJoint component = obj.rb.GetComponent<CharacterJoint>();
			if (component != null)
			{
				component.enablePreprocessing = false;
			}
		}
		part.mass = initialMass * massMultiplier;
		part.prefabMass = part.mass;
		part.needPrefabMass = false;
		this.GetComponentCached(ref _rigidbody).mass = part.mass;
	}

	public virtual void EnableCharacterAndLadderColliders(bool enable)
	{
		int num = characterColliders.Length;
		while (num-- > 0)
		{
			characterColliders[num].enabled = enable;
		}
		if (ladderCollider != null)
		{
			ladderCollider.enabled = enable;
		}
	}

	public virtual void SetRagdoll(bool ragDoll, bool preservePose = false)
	{
		if (ragDoll && this.GetComponentCached(ref _animation).enabled)
		{
			this.GetComponentCached(ref _animation).enabled = false;
		}
		int num = ragdollNodes.Length;
		while (num-- > 0)
		{
			KerbalRagdollNode kerbalRagdollNode = ragdollNodes[num];
			kerbalRagdollNode.go.SetActive(ragDoll);
			if (ragDoll)
			{
				kerbalRagdollNode.rb.velocity = kerbalRagdollNode.velocity;
			}
			else
			{
				kerbalRagdollNode.velocity = this.GetComponentCached(ref _rigidbody).velocity;
			}
		}
		int num2 = characterColliders.Length;
		while (num2-- > 0)
		{
			characterColliders[num2].enabled = !ragDoll;
		}
		int num3 = otherRagdollColliders.Length;
		while (num3-- > 0)
		{
			otherRagdollColliders[num3].enabled = ragDoll;
		}
		if (!ragDoll && !this.GetComponentCached(ref _animation).enabled)
		{
			this.GetComponentCached(ref _animation).enabled = !preservePose;
		}
		if (ragDoll && partPlacementMode && moduleInventoryPartReference != null)
		{
			moduleInventoryPartReference.CancelPartPlacementMode();
		}
		if (ragDoll && (sciencePanelAnimPlaying || playingGolfAnimPlaying || smashingBananaAnimPlaying || spinningWingnutAnimPlaying))
		{
			sciencePanelAnimPlaying = false;
			playingGolfAnimPlaying = false;
			smashingBananaAnimPlaying = false;
			spinningWingnutAnimPlaying = false;
			InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
		}
	}

	public virtual void IntegrateRagdollRigidbodyForces()
	{
		if (base.vessel.packed)
		{
			return;
		}
		if (referenceFrameChanged_rdPhysHold != 0)
		{
			updateRagdollVelocities();
			referenceFrameChanged_rdPhysHold--;
			return;
		}
		geeForce = FlightGlobals.getGeeForceAtPosition(base.transform.position) * PhysicsGlobals.GraviticForceMultiplier;
		centrifugalForce = FlightGlobals.getCentrifugalAcc(base.transform.position, base.part.orbit.referenceBody) * PhysicsGlobals.GraviticForceMultiplier;
		coriolisForce = FlightGlobals.getCoriolisAcc(base.vessel.rb_velocity + Krakensbane.GetFrameVelocityV3f(), base.part.orbit.referenceBody) * PhysicsGlobals.GraviticForceMultiplier;
		int num = ragdollNodes.Length;
		for (int i = 0; i < num; i++)
		{
			KerbalRagdollNode kerbalRagdollNode = ragdollNodes[i];
			kerbalRagdollNode.rb.drag = this.GetComponentCached(ref _rigidbody).drag;
			kerbalRagdollNode.rb.angularDrag = this.GetComponentCached(ref _rigidbody).angularDrag;
			kerbalRagdollNode.rb.AddForce(geeForce, ForceMode.Acceleration);
			kerbalRagdollNode.rb.AddForce(centrifugalForce, ForceMode.Acceleration);
			kerbalRagdollNode.rb.AddForce(coriolisForce, ForceMode.Acceleration);
			if (kerbalRagdollNode.rb != base.part.rb)
			{
				kerbalRagdollNode.rb.AddForce(Krakensbane.GetLastCorrection(), ForceMode.VelocityChange);
			}
			if (base.vessel.Splashed && base.part.partBuoyancy != null)
			{
				float num2 = Mathf.Max(0f, 0f - FlightGlobals.getAltitudeAtPos(kerbalRagdollNode.go.transform.position, base.vessel.mainBody));
				Vector3 force = -FlightGlobals.getGeeForceAtPosition(base.part.partTransform.position) * ((base.part.partBuoyancy.displacement != 0.0) ? base.part.partBuoyancy.displacement : PhysicsGlobals.BuoyancyDefaultVolume) * base.part.vessel.mainBody.oceanDensity * (((double)num2 < PhysicsGlobals.BuoyancyScaleAboveDepth) ? ((double)num2 / PhysicsGlobals.BuoyancyScaleAboveDepth) : 1.0) * PhysicsGlobals.BuoyancyScalar * PhysicsGlobals.BuoyancyKerbalsRagdoll;
				kerbalRagdollNode.rb.AddForce(force, ForceMode.Acceleration);
			}
		}
	}

	public virtual void updateRagdollVelocities()
	{
		if (!base.vessel.packed)
		{
			int num = ragdollNodes.Length;
			while (num-- > 0)
			{
				ragdollNodes[num].updateVelocity(base.transform.position, base.part.rb.velocity, 1f / Time.fixedDeltaTime);
			}
		}
	}

	public virtual void CalculateGroundLevelAngle()
	{
		base.vessel.GetGroundLevelAngle();
		_ = (Vector3)(base.transform.position - FlightGlobals.getUpAxis() * halfHeight + base.transform.forward * 0.5f);
		Vector3 origin = base.transform.position - FlightGlobals.getUpAxis() * halfHeight;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, base.transform.forward, out hitInfo, 0.25f, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000, QueryTriggerInteraction.Ignore);
		RaycastHit hitInfo2;
		bool flag2 = Physics.Raycast(origin, -base.transform.forward, out hitInfo2, 0.25f, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000, QueryTriggerInteraction.Ignore);
		if (flag && !flag2)
		{
			slopeMovementDirection = 1;
		}
		else if (!flag && flag2)
		{
			slopeMovementDirection = -1;
		}
		else if (flag && flag2)
		{
			slopeMovementDirection = -1;
		}
		else
		{
			slopeMovementDirection = 0;
		}
	}

	public virtual void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vcs)
	{
		if (vcs.host == base.vessel && fsm.CurrentState == st_idle_gr)
		{
			base.Events["PlantFlag"].active = CanPlantFlag();
		}
	}

	public virtual void onReferencebodyChanged(GameEvents.FromToAction<CelestialBody, CelestialBody> rChg)
	{
		referenceFrameChanged_rdPhysHold = 2;
		Vector3d vel = base.vessel.orbit.GetVel();
		if (!Krakensbane.GetFrameVelocity().IsZero() && !(vel.sqrMagnitude < Krakensbane.SqrThreshold))
		{
			return;
		}
		vel -= Krakensbane.GetFrameVelocity();
		int num = ragdollNodes.Length;
		while (num-- > 0)
		{
			KerbalRagdollNode kerbalRagdollNode = ragdollNodes[num];
			if (kerbalRagdollNode.rb != base.part.rb)
			{
				kerbalRagdollNode.rb.AddForce(vel - kerbalRagdollNode.rb.velocity, ForceMode.VelocityChange);
			}
		}
	}

	public virtual void onRotatingFrameChanged(GameEvents.HostTargetAction<CelestialBody, bool> frm)
	{
		referenceFrameChanged_rdPhysHold = 2;
		Vector3d vector3d = base.part.rb.velocity + frm.host.getRFrmVel(base.part.partTransform.position) * (frm.target ? (-1.0) : 1.0) + Krakensbane.GetFrameVelocity();
		if (!Krakensbane.GetFrameVelocity().IsZero() && !(vector3d.sqrMagnitude < Krakensbane.SqrThreshold))
		{
			return;
		}
		int num = ragdollNodes.Length;
		while (num-- > 0)
		{
			KerbalRagdollNode kerbalRagdollNode = ragdollNodes[num];
			if (kerbalRagdollNode.rb != base.part.rb)
			{
				if (frm.target)
				{
					kerbalRagdollNode.rb.velocity = kerbalRagdollNode.rb.velocity - frm.host.getRFrmVel(base.part.partTransform.position);
				}
				else
				{
					kerbalRagdollNode.rb.velocity = kerbalRagdollNode.rb.velocity + frm.host.getRFrmVel(base.part.partTransform.position);
				}
			}
		}
	}

	public virtual void onFrameVelocityChange(Vector3d velOffset)
	{
		referenceFrameChanged_rdPhysHold = 2;
		int num = ragdollNodes.Length;
		while (num-- > 0)
		{
			KerbalRagdollNode kerbalRagdollNode = ragdollNodes[num];
			if (kerbalRagdollNode.rb != base.part.rb)
			{
				kerbalRagdollNode.rb.AddForce(velOffset, ForceMode.VelocityChange);
			}
		}
	}

	public virtual void OnVesselGoOnRails(Vessel v)
	{
		if (v == base.vessel)
		{
			if (fsm.currentStateName.Contains("Ladder"))
			{
				fsm.RunEvent(On_ladderLetGo);
			}
			if (isRagdoll)
			{
				SetRagdoll(ragDoll: false, preservePose: true);
			}
		}
	}

	public virtual void OnVesselGoOffRails(Vessel v)
	{
		if (v == base.vessel)
		{
			StartCoroutine(waitAndHandleRagdollTimeWarp(3));
			if (fsm.CurrentState == st_idle_gr)
			{
				base.Events["PlantFlag"].active = CanPlantFlag();
			}
		}
	}

	public virtual IEnumerator waitAndHandleRagdollTimeWarp(int waitFrames)
	{
		int i = 0;
		while (i < waitFrames)
		{
			yield return null;
			int num = i + 1;
			i = num;
		}
		Debug.Log("Unpacking " + base.name + ". Vel: " + this.GetComponentCached(ref _rigidbody).velocity.ToString(), base.gameObject);
		if (isRagdoll)
		{
			int num2 = ragdollNodes.Length;
			while (num2-- > 0)
			{
				ragdollNodes[num2].velocity = base.part.rb.velocity;
			}
			SetRagdoll(ragDoll: true);
			if (InConstructionMode && (bool)EVAConstructionModeController.Instance)
			{
				EVAConstructionModeController.Instance.ClosePanel();
				weldFX.Stop();
				alternateIdleDisabled = false;
				ToggleWeldingGun(toggle: false);
				InputLockManager.RemoveControlLock("WeldLock_" + base.vessel.id);
			}
		}
		else
		{
			SetRagdoll(ragDoll: false);
		}
	}

	public virtual void ResetRagdollLinks()
	{
		int num = ragdollNodes.Length;
		while (num-- > 0)
		{
			KerbalRagdollNode obj = ragdollNodes[num];
			obj.velocity = Vector3.zero;
			CharacterJoint component = obj.go.GetComponent<CharacterJoint>();
			if ((bool)component && component.connectedBody == null)
			{
				component.connectedBody = this.GetComponentCached(ref _rigidbody);
			}
		}
	}

	public virtual void OnPartDie()
	{
		RDPartCollisionHandler[] componentsInChildren = GetComponentsInChildren<RDPartCollisionHandler>(includeInactive: true);
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			UnityEngine.Object.Destroy(componentsInChildren[num]);
		}
	}

	public virtual void Splat(Vector3 point, Vector3 normal)
	{
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		UnityEngine.Object.Instantiate(splatPrefab, point + normal * 0.1f, rotation).SetActive(value: true);
	}

	public virtual bool CanRecover()
	{
		canRecover = this.GetComponentCached(ref _rigidbody).velocity.sqrMagnitude < recoverThreshold * recoverThreshold || (!base.vessel.LandedOrSplashed && Planetarium.GetUniversalTime() > lastCollisionTime + recoverTime);
		if (canRecover)
		{
			if (!(FlightGlobals.ActiveVessel == base.vessel))
			{
				return true;
			}
			return tgtRpos != Vector3.zero;
		}
		return false;
	}

	public virtual void SetEjectDirection()
	{
		ejectDirection = Vector3.up;
		int count = base.part.parent.Modules.Count;
		int num = 0;
		while (true)
		{
			if (num >= count)
			{
				return;
			}
			PartModule partModule = base.part.parent.Modules[num];
			if (partModule is KerbalSeat)
			{
				kerbalSeat = partModule as KerbalSeat;
				if (kerbalSeat.Occupant == base.part)
				{
					break;
				}
			}
			num++;
		}
		ejectDirection = kerbalSeat.ejectDirection;
	}

	public virtual void EjectFromSeat()
	{
		Vector3 ejectPoint = GetEjectPoint(ejectDirection, 50f, halfHeight, halfHeight);
		base.part.Undock(kerbalVesselInfo);
		base.vessel.SetPosition(ejectPoint, usePristineCoords: true);
		base.vessel.IgnoreGForces(1);
		if (kerbalSeat == null && base.part.parent != null)
		{
			kerbalSeat = base.part.parent.GetComponent<KerbalSeat>();
		}
		if (kerbalSeat != null && kerbalSeat.EjectionForce > 0f)
		{
			base.part.rb.AddForce(base.part.partTransform.TransformVector(kerbalSeat.ejectionForceDirection.normalized * kerbalSeat.EjectionForce));
		}
		if (KerbalPortraitGallery.Instance != null)
		{
			KerbalPortraitGallery.Instance.UnregisterActiveCrew(this);
			portrait = null;
		}
	}

	public virtual IEnumerator AcquireRotation(Quaternion tgtRot, float duration)
	{
		if (!base.vessel.packed)
		{
			this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
			Quaternion iRot = base.transform.rotation;
			float startTime = Time.time;
			float endTime = Time.time + duration;
			while (Time.time < endTime)
			{
				base.transform.rotation = Quaternion.Lerp(iRot, tgtRot, Mathf.InverseLerp(startTime, endTime, Time.time));
				yield return null;
			}
		}
	}

	public virtual IEnumerator AcquirePosition(Vector3 tgtPos, float duration)
	{
		if (!base.vessel.packed)
		{
			this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
			Vector3 iPos = base.transform.position;
			float startTime = Time.time;
			float endTime = Time.time + duration;
			while (Time.time < endTime)
			{
				base.transform.position = Vector3.Lerp(iPos, tgtPos, Mathf.InverseLerp(startTime, endTime, Time.time));
				yield return null;
			}
		}
	}

	public virtual void correctLadderPosition()
	{
		if (base.vessel.packed || currentLadder == null)
		{
			return;
		}
		Vtgt = (currentLadder.attachedRigidbody ? (currentLadder.attachedRigidbody.velocity + Vector3.Cross(currentLadder.attachedRigidbody.angularVelocity, this.GetComponentCached(ref _rigidbody).worldCenterOfMass - currentLadder.attachedRigidbody.worldCenterOfMass)) : Vector3.zero);
		this.GetComponentCached(ref _rigidbody).velocity = Vtgt;
		base.vessel.gravityMultiplier = 1.0;
		onLadder = true;
		Vector3 zero = Vector3.zero;
		if (isLadderJointed)
		{
			zero = ((currentLadderPart != null) ? currentLadderPart.transform.TransformPoint(ladderPosition) : currentLadder.transform.TransformPoint(ladderPosition));
			if (currentLadderPart != null && currentLadderPart.vessel.perturbation_immediate.magnitude > GameSettings.EVA_LADDER_JOINT_BREAK_ACCELERATION && currentLadderPart.vessel.velocityD.magnitude > GameSettings.EVA_LADDER_JOINT_BREAK_VELOCITY)
			{
				ClearLadderHold();
			}
		}
		else
		{
			ladderPos = Vector3.ProjectOnPlane(currentLadder.transform.position - ladderPivot.position, currentLadder.transform.up);
			zero = Vector3.Lerp(base.transform.position, ladderPos + base.transform.position, 10f * Time.deltaTime);
		}
		base.transform.position = zero;
	}

	public virtual void correctLadderRotation()
	{
		if (!base.vessel.packed && !(currentLadder == null))
		{
			ladderUp = currentLadder.transform.up * (invLadderAxis ? (-1f) : 1f);
			ladderFwd = currentLadder.transform.forward;
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.FromToRotation(base.transform.up, ladderUp) * base.transform.rotation, 10f * Time.deltaTime);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.FromToRotation(base.transform.forward, ladderFwd) * base.transform.rotation, 5f * Time.deltaTime);
		}
	}

	public virtual void UpdateLadderMovement()
	{
		if (!base.vessel.packed && !(currentLadder == null))
		{
			float num = (float)fsm.TimeAtCurrentState;
			num = ((num >= 0.3f) ? 1f : ((!(num > 0f)) ? 0f : (num * 3.3333333f)));
			currentSpd = Mathf.Lerp(lastTgtSpeed, tgtSpeed, num);
			if (((currentSpd < 0f && !canDescend) || (currentSpd > 0f && !canClimb)) && GameSettings.EVA_LADDER_CHECK_END)
			{
				currentSpd = 0f;
			}
			this.GetComponentCached(ref _rigidbody).velocity = (currentLadder.attachedRigidbody ? (currentLadder.attachedRigidbody.velocity + Vector3.Cross(currentLadder.attachedRigidbody.angularVelocity, this.GetComponentCached(ref _rigidbody).worldCenterOfMass - currentLadder.attachedRigidbody.worldCenterOfMass)) : Vector3.zero) + ladderUp * currentSpd;
		}
	}

	public virtual void InterpolateLadders()
	{
		if (currentLadderTriggers.Count > 1 && currentLadder != null && !ladderTransition && Vector3.Dot(currentLadder.transform.forward, currentLadderTriggers[1].transform.forward) > MinLadderForwardDot && Mathf.Abs(Vector3.Dot(currentLadder.transform.right, currentLadderTriggers[1].transform.right)) > MinLadderRightDot)
		{
			secondaryLadder = currentLadderTriggers[1];
		}
		else if (ladderTgtRPos != Vector3.zero)
		{
			secondaryLadder = null;
		}
	}

	public virtual void AutoTransition()
	{
		if (!(secondaryLadder != null) || ladderTransition)
		{
			return;
		}
		Vector3 lhs = currentLadder.transform.position - secondaryLadder.transform.position;
		float num = Vector3.Dot(lhs, secondaryLadder.transform.forward);
		float num2 = Vector3.Dot(lhs, currentLadder.transform.forward);
		if (num > 0f && num2 < 0f)
		{
			if (Vector3.Dot(ladderPivot.position - secondaryLadder.transform.position, secondaryLadder.transform.forward) < 0f)
			{
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = false;
				}
				currentLadder = secondaryLadder;
				currentLadderPart = FlightGlobals.GetPartUpwardsCached(currentLadder.gameObject);
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = true;
				}
				secondaryLadder = null;
				ladderTransition = true;
			}
		}
		else if (num < 0f && num2 > 0f && Vector3.Dot(ladderPivot.position - secondaryLadder.transform.position, secondaryLadder.transform.forward) > 0f)
		{
			if (currentLadderPart != null)
			{
				currentLadderPart.hasKerbalOnLadder = false;
			}
			currentLadder = secondaryLadder;
			currentLadderPart = FlightGlobals.GetPartUpwardsCached(currentLadder.gameObject);
			if (currentLadderPart != null)
			{
				currentLadderPart.hasKerbalOnLadder = true;
			}
			secondaryLadder = null;
			ladderTransition = true;
		}
	}

	public virtual void UpdateCurrentLadder()
	{
		if (base.vessel.packed || currentLadderTriggers.Count < 1)
		{
			return;
		}
		PostInteractionScreenMessage(cacheAutoLOC_115662);
		AutoTransition();
		if (ladderTgtRPos != Vector3.zero && !ladderTransition)
		{
			currentLadderTriggers.Sort(SortTriggersByAlignment);
			Collider collider = currentLadderTriggers[0];
			if (currentLadder != collider && Mathf.Abs(Vector3.Dot(ladderTgtRPos, collider.transform.up)) > 0.3f)
			{
				currentLadder = collider;
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = false;
				}
				currentLadderPart = FlightGlobals.GetPartUpwardsCached(currentLadder.gameObject);
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = true;
				}
				invLadderAxis = Vector3.Dot(ladderTgtRPos.normalized, currentLadder.transform.up) * ((tgtSpeed != 0f) ? Mathf.Sign(tgtSpeed) : 1f) < 0f;
				if (GameSettings.EVA_left.GetKey() || GameSettings.EVA_right.GetKey())
				{
					ladderTransition = true;
				}
			}
		}
		if (GameSettings.EVA_left.GetKeyUp() || GameSettings.EVA_right.GetKeyUp() || currentLadderTriggers.Count == 1)
		{
			ladderTransition = false;
		}
	}

	public virtual void UpdateCurrentLadderIdle()
	{
		if (base.vessel.packed || currentLadderTriggers.Count < 1)
		{
			return;
		}
		PostInteractionScreenMessage(cacheAutoLOC_115694);
		if (currentLadderTriggers.Count == 1)
		{
			ladderTransition = false;
			return;
		}
		AutoTransition();
		if (Mathf.Abs(Vector3.Dot(ladderTgtRPos, base.transform.right)) > 0.9f && !ladderTransition)
		{
			ladderTgtRPos = Vector3.ProjectOnPlane(ladderTgtRPos, base.transform.up);
			currentLadderTriggers.Sort(SortTriggersByAlignment);
			Collider collider = currentLadderTriggers[0];
			if (currentLadder != collider && Mathf.Abs(Vector3.Dot(ladderTgtRPos, collider.transform.up)) > 0.3f)
			{
				currentLadder = collider;
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = false;
				}
				currentLadderPart = FlightGlobals.GetPartUpwardsCached(currentLadder.gameObject);
				if (currentLadderPart != null)
				{
					currentLadderPart.hasKerbalOnLadder = true;
				}
				invLadderAxis = Vector3.Dot(ladderTgtRPos.normalized, currentLadder.transform.up) < 0f;
				ladderTransition = true;
			}
		}
		if (GameSettings.EVA_left.GetKeyUp() || GameSettings.EVA_right.GetKeyUp())
		{
			ladderTransition = false;
		}
	}

	public virtual void CheckLadderTriggers()
	{
		if (currentLadderTriggers.Count <= 0)
		{
			return;
		}
		List<Collider> list = new List<Collider>();
		for (int i = 0; i < currentLadderTriggers.Count; i++)
		{
			if (currentLadderTriggers[i] != null && !currentLadderTriggers[i].enabled)
			{
				list.Add(currentLadderTriggers[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			currentLadderTriggers.Remove(list[j]);
		}
	}

	public virtual int SortTriggersByDistance(Collider c1, Collider c2)
	{
		Vector3 vector = c1.ClosestPointOnBounds(ladderPivot.position);
		Vector3 vector2 = c2.ClosestPointOnBounds(ladderPivot.position);
		if ((vector - ladderPivot.position).sqrMagnitude <= (vector2 - ladderPivot.position).sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	public virtual int SortTriggersByAlignment(Collider c1, Collider c2)
	{
		if (Mathf.Abs(Vector3.Dot(ladderTgtRPos.normalized, c1.transform.up)) >= Mathf.Abs(Vector3.Dot(ladderTgtRPos.normalized, c2.transform.up)) - 0.001f)
		{
			return -1;
		}
		return 1;
	}

	public virtual IEnumerator StartNonCollidePeriod(float duration, float standoff, Part fromPart, Transform airlockTrf)
	{
		float T0 = Time.realtimeSinceStartup;
		int num = characterColliders.Length;
		while (num-- > 0)
		{
			characterColliders[num].isTrigger = true;
		}
		while (T0 + duration > Time.realtimeSinceStartup)
		{
			if (airlockTrf != null && fromPart != null)
			{
				base.transform.rotation = airlockTrf.rotation;
				base.transform.position = airlockTrf.position + airlockTrf.rotation * (Vector3.back * standoff);
				this.GetComponentCached(ref _rigidbody).velocity = fromPart.Rigidbody.velocity + Vector3.Cross(base.transform.position - fromPart.vessel.CurrentCoM, fromPart.vessel.angularVelocity);
				this.GetComponentCached(ref _rigidbody).angularVelocity = fromPart.vessel.angularVelocity;
				yield return new WaitForFixedUpdate();
			}
			int num2 = characterColliders.Length;
			while (num2-- > 0)
			{
				characterColliders[num2].isTrigger = false;
			}
		}
	}

	public virtual bool FindClamberSrf(float fwdOffset, float reach)
	{
		clamberOrigin = base.transform.position + base.transform.forward * fwdOffset + fUp * reach;
		clamberTarget = base.transform.position + base.transform.forward * fwdOffset;
		if (Physics.Raycast(clamberOrigin, (clamberTarget - clamberOrigin).normalized, out clamberHitInfo, reach, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000))
		{
			if (Physics.Raycast(new Ray(base.transform.position + fUp * halfHeight, fUp), out var _, reach - clamberHitInfo.distance + halfHeight * 3f))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public virtual bool TestClamberSrf(RaycastHit clamberHitInfo)
	{
		if (clamberHitInfo.collider.attachedRigidbody != null)
		{
			sciencePart = Part.GetComponentUpwards<ModuleGroundSciencePart>(clamberHitInfo.collider.gameObject);
			if (sciencePart != null)
			{
				return false;
			}
			if (clamberHitInfo.collider.attachedRigidbody.velocity.sqrMagnitude < 0.01f)
			{
				return clamberHitInfo.collider.attachedRigidbody.angularVelocity.sqrMagnitude < 0.01f;
			}
			return false;
		}
		return true;
	}

	public virtual ClamberPath GetClamberPath(float fwdOffset, float reach)
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector3 zero3 = Vector3.zero;
		Vector3 zero4 = Vector3.zero;
		Vector3 zero5 = Vector3.zero;
		clamberOrigin = base.transform.position + base.transform.forward * fwdOffset + fUp * reach;
		clamberTarget = base.transform.position + base.transform.forward * fwdOffset;
		Vector3 forward = base.transform.forward;
		float num = 0f;
		ClamberPath clamberPath = null;
		if (Physics.Raycast(clamberOrigin, (clamberTarget - clamberOrigin).normalized, out clamberHitInfo, reach, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000))
		{
			zero3 = clamberHitInfo.point;
			zero4 = clamberHitInfo.normal;
			num = Vector3.Dot(zero3 - base.transform.position, fUp);
			if (clamberHitInfo.collider.Raycast(new Ray(base.transform.position + fUp * (num - 0.01f), forward), out clamberHitInfo, reach))
			{
				zero2 = clamberHitInfo.point;
				zero5 = Vector3.ProjectOnPlane(clamberHitInfo.normal, fUp).normalized;
				Debug.DrawRay(zero3, zero5 * clamberHitInfo.distance, Color.cyan);
				zero = zero2 + Quaternion.AngleAxis(90f, Vector3.Cross(zero5, zero4)) * (zero3 - zero2);
				clamberPath = new ClamberPath(zero5, zero4, zero, zero2, zero3, num, halfHeight);
				DebugDrawUtil.DrawCrosshairs(clamberPath.p1, 0.2f, Color.green, 5f);
				DebugDrawUtil.DrawCrosshairs(clamberPath.p2, 0.2f, Color.yellow, 5f);
				DebugDrawUtil.DrawCrosshairs(clamberPath.p3, 0.2f, Color.blue, 5f);
			}
		}
		return clamberPath;
	}

	public virtual bool FindControlPanel(float fwdOffset, float reach)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return false;
		}
		controlOrigin = base.transform.position + base.transform.forward * fwdOffset + fUp * reach;
		controlTarget = base.transform.position + base.transform.forward * fwdOffset;
		if (Physics.Raycast(controlOrigin, (controlTarget - controlOrigin).normalized, out controlHitInfo, reach, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000))
		{
			sciencePart = Part.GetComponentUpwards<ModuleGroundSciencePart>(controlHitInfo.collider.gameObject);
			if (sciencePart != null && sciencePart.DeployedOnGround)
			{
				PostInteractionScreenMessage(cacheAutoLOC_6012032);
				return true;
			}
			return false;
		}
		return false;
	}

	public virtual void ControlPanelInteractionFinished()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && sciencePanelAnimPlaying)
		{
			if (sciencePanelAnimCooldown > 0f)
			{
				sciencePanelAnimCooldown -= Time.deltaTime;
				return;
			}
			sciencePanelAnimPlaying = false;
			InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
			fsm.RunEvent(On_control_panel_interacting);
		}
	}

	public void RandomControlPanelAnim()
	{
		Animations.controlPanelAnimSelector = UnityEngine.Random.Range(0, Animations.controlPanelAnims.Count);
	}

	public virtual void RocSampleStored()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") || !pickRocSampleAnimPlaying)
		{
			return;
		}
		if (pickRocSampleAnimCooldown > 0f)
		{
			pickRocSampleAnimCooldown -= Time.deltaTime;
			return;
		}
		pickRocSampleAnimPlaying = false;
		InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
		fsm.RunEvent(On_roc_sample_stored);
		if (experimentROC != null)
		{
			experimentROC.PerformExperiment(moduleScienceExperimentROC);
		}
	}

	public void Weld(Part targetPart)
	{
		if (!(EVAConstructionModeController.Instance == null))
		{
			if (VesselUtilities.VesselCrewWithTraitCount("Engineer") <= 0)
			{
				Debug.LogError("Kerbal is not an engineer!");
				return;
			}
			constructionTarget = targetPart;
			fsm.RunEvent(On_weldStart);
		}
	}

	public void LowerVisor(bool forceHelmet)
	{
		if (isHelmetEnabled || forceHelmet)
		{
			wasHelmetEnabledBeforeWelding = isHelmetEnabled;
			if (forceHelmet && !isHelmetEnabled)
			{
				ToggleHelmet(enableHelmet: true);
			}
			visorState = VisorStates.Lowering;
			UpdateVisorEventStates();
			GameEvents.OnVisorLowering.Fire(this);
		}
	}

	public void RaiseVisor(bool restoreHelmet)
	{
		removeHelmetAfterRaisingVisor = !wasHelmetEnabledBeforeWelding && restoreHelmet;
		visorState = VisorStates.Raising;
		UpdateVisorEventStates();
		GameEvents.OnVisorRaising.Fire(this);
	}

	public void UpdateVisorPosition()
	{
		if (visorState == VisorStates.Lowered || visorState == VisorStates.Raised)
		{
			return;
		}
		visorTextureOffset = VisorRenderer.material.mainTextureOffset;
		visorCurrentOffset = visorTextureOffset.y;
		visorTargetOffset = ((visorState == VisorStates.Lowering) ? visorLoweredTargetOffset : visorRaisedTargetOffset);
		visorCurrentOffset = Mathf.Lerp(visorCurrentOffset, visorTargetOffset, Time.deltaTime * VisorAnimationSpeed);
		visorTextureOffset.y = visorCurrentOffset;
		VisorRenderer.material.mainTextureOffset = visorTextureOffset;
		if (Mathf.Abs(visorTargetOffset - visorCurrentOffset) < 0.001f)
		{
			if (visorState == VisorStates.Lowering)
			{
				visorState = VisorStates.Lowered;
				isVisorEnabled = true;
				GameEvents.OnVisorLowered.Fire(this);
			}
			else
			{
				visorState = VisorStates.Raised;
				isVisorEnabled = false;
				GameEvents.OnVisorRaised.Fire(this);
			}
			ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
			if (protoCrewMember != null)
			{
				protoCrewMember.hasVisorDown = isVisorEnabled;
			}
		}
		UpdateVisorEventStates();
	}

	public void OnVisorRaised(KerbalEVA kerbal)
	{
		if (!(kerbal != this) && removeHelmetAfterRaisingVisor)
		{
			removeHelmetAfterRaisingVisor = false;
			ToggleHelmet(enableHelmet: false);
		}
	}

	public void UpdateVisorEventStates()
	{
		base.Events["LowerVisor"].guiActive = isHelmetEnabled && (visorState == VisorStates.Raising || visorState == VisorStates.Raised);
		base.Events["RaiseVisor"].guiActive = isHelmetEnabled && (visorState == VisorStates.Lowering || visorState == VisorStates.Lowered);
	}

	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003448")]
	public void LowerVisor()
	{
		LowerVisor(forceHelmet: false);
	}

	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8003449")]
	public void RaiseVisor()
	{
		RaiseVisor(restoreHelmet: false);
	}

	public void PlayGolf(Callback afterGolfCallback)
	{
		afterPlayGolf = afterGolfCallback;
		fsm.RunEvent(On_Playing_Golf);
	}

	public virtual void playing_Golf_OnEnter(KFSMState st)
	{
		Animations.playingGolf.State.time = Animations.playingGolf.start;
		this.GetComponentCached(ref _animation).CrossFade(Animations.playingGolf, 0.2f, PlayMode.StopAll);
		if (golfSoundFX.Count > 0)
		{
			golfSound = golfSoundFX[UnityEngine.Random.Range(0, golfSoundFX.Count - 1)];
		}
		playingGolfAnimCooldown = Animations.playingGolf.State.length;
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "ControlPanelLock_" + base.vessel.id);
		playingGolfAnimPlaying = true;
		replacedGolfBall = false;
		golfSoundPlayed = false;
		SpawnGolf();
	}

	public virtual void SpawnGolf()
	{
		if (golfClub == null)
		{
			for (int i = 0; i < kerbalObjects.Count; i++)
			{
				if (kerbalObjects[i].name == "GolfClub")
				{
					golfClub = kerbalObjects[i];
					break;
				}
			}
		}
		if (golfClub != null && golfClub.instance == null)
		{
			golfClub.instance = UnityEngine.Object.Instantiate(golfClub.prefab);
			golfClub.instance.transform.SetParent(golfClub.anchor);
			golfClub.instance.transform.localPosition = Vector3.zero;
			golfClub.instance.transform.localRotation = Quaternion.identity;
			golfClub.mesh = golfClub.instance.GetComponentInChildren<MeshRenderer>();
			golfClub.additionalObject = golfClub.instance.transform.FindChild("golfBall", findActiveChild: false);
			golfClub.animation = golfClub.instance.GetComponentInChildren<Animation>();
		}
		if (golfClub == null || !(golfClub.instance != null) || !(golfClub.animation != null) || golfClub.animation.isPlaying)
		{
			return;
		}
		golfClub.instance.SetActive(value: true);
		foreach (AnimationState item in golfClub.animation)
		{
			item.normalizedTime = 0f;
		}
		golfClub.animation.Play();
	}

	public virtual void PlayingGolfPhysicalBall()
	{
		if (!golfSoundPlayed && Animations.playingGolf.State.normalizedTime > golfSoundTime && !string.IsNullOrEmpty(golfSound))
		{
			base.part.Effect(golfSound);
			golfSoundPlayed = true;
		}
		if (Animations.playingGolf.State.normalizedTime > ballTime && !replacedGolfBall)
		{
			if (golfClub != null && golfClub.additionalObjectPrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(golfClub.additionalObjectPrefab);
				gameObject.transform.position = golfClub.additionalObject.position;
				physicalObject obj = physicalObject.ConvertToPhysicalObject(base.part, gameObject);
				Rigidbody rb = obj.rb;
				obj.maxDistance = 10000f;
				obj.origDrag = ballDrag;
				rb.mass = 0.01f;
				rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
				rb.angularVelocity = base.part.Rigidbody.angularVelocity;
				gameObject.transform.rotation = base.transform.rotation;
				gameObject.transform.Rotate(ballAngle, 0f, 0f, Space.Self);
				ballForceDir = gameObject.transform.forward;
				rb.drag = ballDrag;
				rb.useGravity = false;
				rb.AddForce(ballForceDir * ballForce, ForceMode.Force);
			}
			replacedGolfBall = true;
		}
	}

	public virtual void FinishedPlayingGolf()
	{
		if (!playingGolfAnimPlaying)
		{
			return;
		}
		if (playingGolfAnimCooldown > 0f)
		{
			playingGolfAnimCooldown -= Time.deltaTime;
			return;
		}
		if (golfClub != null)
		{
			if (golfClub.animation != null)
			{
				golfClub.animation.Stop();
			}
			golfClub.instance.SetActive(value: false);
		}
		playingGolfAnimPlaying = false;
		InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
		fsm.RunEvent(On_Golf_Complete);
		if (afterPlayGolf != null)
		{
			afterPlayGolf();
		}
	}

	public void Banana(Callback afterBananaCallback)
	{
		afterBanana = afterBananaCallback;
		fsm.RunEvent(On_Smashing_Banana);
	}

	public virtual void smashing_banana_OnEnter(KFSMState st)
	{
		Animations.smashBanana.State.time = Animations.smashBanana.start;
		this.GetComponentCached(ref _animation).CrossFade(Animations.smashBanana, 0.2f, PlayMode.StopAll);
		if (bananaSoundFX.Count > 0)
		{
			bananaSound = bananaSoundFX[UnityEngine.Random.Range(0, bananaSoundFX.Count - 1)];
		}
		smashingBananaAnimCooldown = Animations.smashBanana.State.length;
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "ControlPanelLock_" + base.vessel.id);
		smashingBananaAnimPlaying = true;
		replacedBanana = false;
		bananaSoundPlayed = false;
		SpawnBanana();
	}

	public virtual void SpawnBanana()
	{
		if (bananaProp == null)
		{
			for (int i = 0; i < kerbalObjects.Count; i++)
			{
				if (kerbalObjects[i].name == "Banana")
				{
					bananaProp = kerbalObjects[i];
					break;
				}
			}
		}
		if (bananaProp != null && bananaProp.instance == null)
		{
			bananaProp.instance = UnityEngine.Object.Instantiate(bananaProp.prefab);
			bananaProp.instance.transform.SetParent(bananaProp.anchor);
			bananaProp.instance.transform.localPosition = Vector3.zero;
			bananaProp.instance.transform.localRotation = Quaternion.identity;
			bananaProp.mesh = bananaProp.instance.GetComponentInChildren<MeshRenderer>();
			bananaProp.additionalObject = bananaProp.instance.transform.FindChild("bananaShattered", findActiveChild: false);
			bananaProp.animation = bananaProp.instance.GetComponentInChildren<Animation>();
		}
		if (bananaProp == null || !(bananaProp.instance != null) || !(bananaProp.animation != null))
		{
			return;
		}
		if (bananaProp.additionalObject != null)
		{
			bananaProp.additionalObject.gameObject.SetActive(value: true);
		}
		if (bananaProp.animation.isPlaying)
		{
			return;
		}
		bananaProp.instance.SetActive(value: true);
		foreach (AnimationState item in bananaProp.animation)
		{
			item.normalizedTime = 0f;
		}
		bananaProp.animation.Play();
	}

	public virtual void SmashingBananaParticles()
	{
		if (!bananaSoundPlayed && Animations.smashBanana.State.normalizedTime > bananaSoundTime && !string.IsNullOrEmpty(bananaSound))
		{
			base.part.Effect(bananaSound);
			bananaSoundPlayed = true;
		}
		if (!(Animations.smashBanana.State.normalizedTime > bananaTime) || replacedBanana)
		{
			return;
		}
		if (bananaProp != null && bananaProp.additionalObjectPrefab != null)
		{
			bananaShards = UnityEngine.Object.Instantiate(bananaProp.additionalObjectPrefab);
			bananaShards.transform.position = bananaProp.additionalObject.position;
			bananaProp.additionalObject.gameObject.SetActive(value: false);
			int childCount = bananaShards.transform.childCount;
			while (childCount-- > 0)
			{
				Transform child = bananaShards.transform.GetChild(childCount);
				if ((bool)child.gameObject.GetComponent<Renderer>())
				{
					if (!child.gameObject.GetComponent<Collider>())
					{
						child.gameObject.AddComponent<MeshCollider>();
					}
					physicalObject obj = physicalObject.ConvertToPhysicalObject(base.part, child.gameObject);
					Rigidbody rb = obj.rb;
					obj.maxDistance = 10000f;
					obj.origDrag = ballDrag;
					rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
					rb.mass = 0.01f;
					Vector3 vector = new Vector3(UnityEngine.Random.Range(0f, bananaForce), UnityEngine.Random.Range(0f, bananaForce), UnityEngine.Random.Range(0f, bananaForce));
					Vector3 vector2 = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
					rb.angularVelocity = base.part.Rigidbody.angularVelocity + vector2;
					Vector3 lhs = base.vessel.CurrentCoM - base.part.Rigidbody.worldCenterOfMass;
					rb.velocity = base.part.Rigidbody.velocity + vector + Vector3.Cross(lhs, rb.angularVelocity);
					rb.drag = ballDrag;
					rb.useGravity = false;
				}
			}
		}
		replacedBanana = true;
	}

	public virtual void FinishedSmashingBanana()
	{
		if (!smashingBananaAnimPlaying)
		{
			return;
		}
		if (smashingBananaAnimCooldown > 0f)
		{
			smashingBananaAnimCooldown -= Time.deltaTime;
			return;
		}
		if (bananaProp != null)
		{
			if (bananaProp.animation != null)
			{
				bananaProp.animation.Stop();
			}
			bananaProp.instance.SetActive(value: false);
		}
		if (bananaShards != null)
		{
			bananaShards.DestroyGameObject();
		}
		smashingBananaAnimPlaying = false;
		InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
		fsm.RunEvent(On_Banana_Complete);
		if (afterBanana != null)
		{
			afterBanana();
		}
	}

	public void Dzhanibekov(Callback afterWingnutCallback)
	{
		afterWingnut = afterWingnutCallback;
		fsm.RunEvent(On_Spinning_Wingnut);
	}

	public virtual void spinning_Wingnut_OnEnter(KFSMState st)
	{
		Animations.spinWingnut.State.time = Animations.spinWingnut.start;
		this.GetComponentCached(ref _animation).CrossFade(Animations.spinWingnut, 0.2f, PlayMode.StopAll);
		spinningWingnutAnimCooldown = wingnutTransitionTime;
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "ControlPanelLock_" + base.vessel.id);
		spinningWingnutAnimPlaying = true;
		appliedTorque = false;
		SpawnWingnut();
	}

	public virtual void SpawnWingnut()
	{
		if (wingnutProp == null)
		{
			for (int i = 0; i < kerbalObjects.Count; i++)
			{
				if (kerbalObjects[i].name == "Wingnut")
				{
					wingnutProp = kerbalObjects[i];
					break;
				}
			}
		}
		if (wingnutProp != null && wingnutProp.instance == null)
		{
			if (JetpackDeployed && wingnutProp.jetpackPrefab != null)
			{
				wingnutProp.instance = UnityEngine.Object.Instantiate(wingnutProp.jetpackPrefab);
			}
			else
			{
				wingnutProp.instance = UnityEngine.Object.Instantiate(wingnutProp.prefab);
			}
			wingnutProp.instance.transform.SetParent(wingnutProp.anchor);
			wingnutProp.instance.transform.localPosition = Vector3.zero;
			wingnutProp.instance.transform.localRotation = Quaternion.identity;
			wingnutProp.mesh = wingnutProp.instance.GetComponentInChildren<MeshRenderer>();
			wingnutProp.additionalObject = wingnutProp.instance.transform.FindChild("flynutSpin", findActiveChild: false);
			wingnutProp.animation = wingnutProp.instance.GetComponentInChildren<Animation>();
		}
		if (wingnutProp == null || !(wingnutProp.instance != null) || !(wingnutProp.animation != null) || wingnutProp.animation.isPlaying)
		{
			return;
		}
		wingnutProp.instance.SetActive(value: true);
		foreach (AnimationState item in wingnutProp.animation)
		{
			item.normalizedTime = 0f;
		}
		wingnutProp.animation.Play();
	}

	public virtual void ApplyWingnutTorque()
	{
		if (Animations.spinWingnut.State.normalizedTime > wingnutTorqueTime && !appliedTorque)
		{
			Vector3.Cross(base.transform.up, base.transform.right);
			base.part.AddTorque(base.transform.forward * wingnutKerbalTorqueForce * 1f);
			appliedTorque = true;
		}
	}

	public virtual void SpinningWingnutForever()
	{
		if (wingnutProp == null || !(wingnutProp.additionalObjectPrefab != null))
		{
			return;
		}
		wingnut = UnityEngine.Object.Instantiate(wingnutProp.additionalObjectPrefab);
		wingnut.transform.position = wingnutProp.additionalObject.position;
		wingnut.transform.rotation = wingnutProp.additionalObject.rotation;
		wingnutRB = wingnut.GetComponent<Rigidbody>();
		if (wingnutRB != null)
		{
			wingnutRB.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
		}
		Animation componentInChildren = wingnut.GetComponentInChildren<Animation>();
		if (!(componentInChildren != null))
		{
			return;
		}
		foreach (AnimationState item in componentInChildren)
		{
			item.normalizedTime = 0f;
			item.wrapMode = WrapMode.Loop;
		}
		componentInChildren.Play();
	}

	public virtual void FinishedSpinningWingnut()
	{
		if (!spinningWingnutAnimPlaying)
		{
			return;
		}
		if (spinningWingnutAnimCooldown > 0f)
		{
			spinningWingnutAnimCooldown -= Time.deltaTime;
			return;
		}
		if (wingnutProp != null)
		{
			if (wingnutProp.animation != null)
			{
				wingnutProp.animation.Stop();
			}
			SpinningWingnutForever();
			wingnutProp.instance.SetActive(value: false);
		}
		spinningWingnutAnimPlaying = false;
		InputLockManager.RemoveControlLock("ControlPanelLock_" + base.vessel.id);
		fsm.RunEvent(On_Wingnut_Complete);
		if (afterWingnut != null)
		{
			afterWingnut();
		}
	}

	public void SetupEVAScienceSoundFX()
	{
		golfSoundFX = base.part.Effects.EffectsStartingWith("golf");
		bananaSoundFX = base.part.Effects.EffectsStartingWith("banana");
	}

	public virtual void BoardPart(Part p)
	{
		if (!HighLogic.CurrentGame.Parameters.Flight.CanBoard)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_115948"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
		else if (p.protoModuleCrew.Count >= p.CrewCapacity)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_115954"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
		else if (!processInventory(p, storeParts: false))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8002271"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
		else if (checkExperiments(p))
		{
			proceedAndBoard(p);
		}
	}

	public bool processInventory(Part p, bool storeParts)
	{
		if (moduleInventoryPartReference == null)
		{
			return true;
		}
		moduleInventoryPartReference.RefillEVAPropellantOnBoarding(p);
		base.part.protoModuleCrew[0].SaveInventory(moduleInventoryPartReference);
		return true;
	}

	public virtual bool checkExperiments(Part p)
	{
		List<IScienceDataContainer> list = new List<IScienceDataContainer>();
		bool flag = false;
		int count = base.vessel.parts.Count;
		while (count-- > 0)
		{
			Part part = base.vessel.parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				PartModule partModule = part.Modules[count2];
				if (partModule is IScienceDataContainer)
				{
					IScienceDataContainer scienceDataContainer = partModule as IScienceDataContainer;
					list.Add(scienceDataContainer);
					if (!flag && scienceDataContainer.GetScienceCount() > 0)
					{
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			ModuleScienceContainer moduleScienceContainer = null;
			bool flag2 = false;
			int count3 = p.Modules.Count;
			for (int i = 0; i < count3; i++)
			{
				PartModule partModule = p.Modules[i];
				if (partModule is ModuleScienceContainer)
				{
					flag2 = true;
					moduleScienceContainer = partModule as ModuleScienceContainer;
				}
			}
			if (flag2 && moduleScienceContainer.StoreData(list, dumpRepeats: false))
			{
				return true;
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("StoreExperimentsIssue", Localizer.Format("#autoLOC_116006", p.partInfo.title), Localizer.Format("#autoLOC_116007"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_116008"), delegate
			{
				proceedAndBoard(p);
			}), new DialogGUIButton(Localizer.Format("#autoLOC_116009"), delegate
			{
			})), persistAcrossScenes: false, HighLogic.UISkin);
			return false;
		}
		return true;
	}

	public virtual void proceedAndBoard(Part p)
	{
		if (base.part.protoModuleCrew.Count != 0)
		{
			processInventory(p, storeParts: true);
			ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
			protoCrewMember.flightLog.AddEntryUnique(FlightLog.EntryType.BoardVessel, base.vessel.orbit.referenceBody.name);
			protoCrewMember.SaveEVAChute(evaChute);
			base.part.RemoveCrewmember(protoCrewMember);
			p.AddCrewmember(protoCrewMember);
			protoCrewMember.persistentID = FlightGlobals.GetUniquepersistentId();
			GameEvents.onCrewBoardVessel.Fire(new GameEvents.FromToAction<Part, Part>(base.part, p));
			GameEvents.onCrewTransferred.Fire(new GameEvents.HostedFromToAction<ProtoCrewMember, Part>(protoCrewMember, base.part, p));
			Vessel.CrewWasModified(base.part.vessel, p.vessel);
		}
		if (p.vessel.targetObject != null && p.vessel.targetObject.GetVessel() == base.vessel)
		{
			p.vessel.targetObject = null;
		}
		FlightGlobals.ForceSetActiveVessel(p.vessel);
		FlightInputHandler.ResumeVesselCtrlState(p.vessel);
		base.vessel.Die();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6003095")]
	public virtual void PlantFlag()
	{
		flagItems--;
		fsm.RunEvent(On_flagPlantStart);
	}

	public virtual bool CanPlantFlag()
	{
		if (base.vessel.state == Vessel.State.ACTIVE && base.part.GroundContact && flagItems > 0 && !isRagdoll && GameVariables.Instance.UnlockedEVAFlags(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
		{
			return !InConstructionMode;
		}
		return false;
	}

	public virtual void AddFlag(int flagCount = 1)
	{
		flagItems += flagCount;
		base.Events["PlantFlag"].active = CanPlantFlag();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001360")]
	public virtual void MakeReference()
	{
		if ((bool)referenceTransform)
		{
			base.vessel.SetReferenceTransform(base.part);
		}
		else
		{
			Debug.LogError("[KerbalEVA Error]: No referenceTransform reference defined!", base.gameObject);
		}
	}

	public virtual bool BoardSeat(KerbalSeat seat)
	{
		kerbalSeat = seat;
		fsm.RunEvent(On_seatBoard);
		if (fsm.LastEvent == On_seatBoard)
		{
			ejectDirection = ((seat != null) ? seat.ejectDirection : Vector3.up);
			return true;
		}
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_116092"), 2f, ScreenMessageStyle.UPPER_CENTER);
		return false;
	}

	public virtual bool IsSeated()
	{
		return fsm.CurrentState == st_seated_cmd;
	}

	public virtual void OnGrapple()
	{
		fsm.RunEvent(On_grapple);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001810")]
	public virtual void OnDeboardSeat()
	{
		if (base.part.protoModuleCrew[0].type != ProtoCrewMember.KerbalType.Tourist)
		{
			fsm.RunEvent(On_seatDeboard);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8012052"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public virtual void RestoreVesselInfo(float delay)
	{
		StartCoroutine(restoreVesselInfo_afterWait(delay));
	}

	public virtual IEnumerator restoreVesselInfo_afterWait(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (base.vessel.rootPart == base.part)
		{
			base.vessel.vesselName = kerbalVesselInfo.name;
			base.vessel.vesselType = VesselType.const_11;
		}
		else
		{
			Debug.LogError("KerbalEVA Error: Trying to restore EVA vessel data, but EVA does not own the vessel", base.gameObject);
		}
	}

	public virtual void SwitchFocusIfActiveVesselUncontrollable(float delay)
	{
		StartCoroutine(swichFocusIfActiveVesselUncontrollable_delay(delay));
	}

	public virtual IEnumerator swichFocusIfActiveVesselUncontrollable_delay(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!FlightGlobals.ActiveVessel.IsControllable)
		{
			FlightGlobals.ForceSetActiveVessel(base.vessel);
			FlightInputHandler.ResumeVesselCtrlState(base.vessel);
		}
	}

	public virtual Vector3 GetEjectPoint(Vector3 ejectDirection, float maxDist, float capsuleRadius, float capsuleHeight)
	{
		float num = capsuleHeight * 0.5f;
		Vector3 vector = base.transform.rotation * ejectDirection * maxDist;
		Vector3 vector2 = base.transform.position + vector + base.transform.up * num;
		Vector3 vector3 = base.transform.position + vector - base.transform.up * num;
		DebugDrawUtil.DrawCrosshairs(vector2, capsuleRadius, Color.magenta, 5f);
		DebugDrawUtil.DrawCrosshairs(vector3, capsuleRadius, Color.yellow, 5f);
		Vector3 vector4 = base.transform.position;
		RaycastHit[] array = PhysicsUtil.CapsuleCastAllIgnoreSelf(vector2, vector3, capsuleRadius, -vector.normalized, maxDist, LayerUtil.DefaultEquivalent, base.transform);
		if (array.Length != 0)
		{
			vector4 = base.transform.position + vector - vector.normalized * array[0].distance;
			DebugDrawUtil.DrawCrosshairs(vector4 + base.transform.up * num, capsuleRadius, Color.magenta, 5f);
			DebugDrawUtil.DrawCrosshairs(vector4 - base.transform.up * num, capsuleRadius, Color.yellow, 5f);
			DebugDrawUtil.DrawCrosshairs(vector4, 0.3f, Color.cyan, 5f);
		}
		Vector3 vector5 = vector4;
		RaycastHit[] array2 = PhysicsUtil.RaycastAllIgnoreSelf(vector4 + base.vessel.upAxis * maxDist, -base.vessel.upAxis, maxDist + num, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000, base.transform);
		if (array2.Length != 0)
		{
			vector5 = array2[0].point + base.vessel.upAxis * num;
			DebugDrawUtil.DrawCrosshairs(vector5, 0.2f, Color.green, 5f);
		}
		return vector5;
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_900678")]
	public virtual void RenameVessel()
	{
		base.vessel.RenameVessel();
	}

	public virtual void OnTriggerStay(Collider o)
	{
		if (o.CompareTag("Ladder"))
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(o.gameObject);
			if ((partUpwardsCached == null || partUpwardsCached.isAttached) && !currentLadderTriggers.Contains(o))
			{
				currentLadderTriggers.Add(o);
				currentLadderTriggers.Sort(SortTriggersByDistance);
			}
		}
		if (o.CompareTag("Airlock"))
		{
			if (o != currentAirlockTrigger)
			{
				currentAirlockPart = FlightGlobals.GetPartUpwardsCached(o.gameObject);
			}
			currentAirlockTrigger = o;
		}
	}

	public virtual void OnTriggerExit(Collider o)
	{
		if (o.CompareTag("Ladder"))
		{
			currentLadderTriggers.Remove(o);
			if (currentLadderTriggers.Count > 0)
			{
				currentLadderTriggers.Sort(SortTriggersByDistance);
			}
		}
		if (o.CompareTag("Airlock") && o == currentAirlockTrigger)
		{
			currentAirlockTrigger = null;
			currentAirlockPart = null;
		}
	}

	public virtual void PostInteractionScreenMessage(string message, float delay = 0.1f)
	{
		if (HighLogic.LoadedSceneIsFlight && !MapView.MapIsEnabled && base.vessel == FlightGlobals.ActiveVessel)
		{
			ScreenMessages.PostScreenMessage(message, delay, ScreenMessageStyle.KERBAL_EVA);
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		PIDBoost = 1f;
		CacheLocalStrings();
		GameEvents.onLanguageSwitched.Add(CacheLocalStrings);
		inventoryPropellantResources = new List<ResourceListItem>();
	}

	public virtual void OnDestroy()
	{
		if ((bool)advRagdoll)
		{
			UnityEngine.Object.Destroy(advRagdoll);
		}
		if (fsm != null && fsm.CurrentState != null && fsm.CurrentState == st_flagPlant)
		{
			InputLockManager.RemoveControlLock("FlagDeployLock_" + base.vessel.id);
		}
		GameEvents.OnHelmetChanged.Remove(OnHelmetChanged);
		GameEvents.onVesselGoOnRails.Remove(OnVesselGoOnRails);
		GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
		GameEvents.onKrakensbaneEngage.Remove(onFrameVelocityChange);
		GameEvents.onKrakensbaneDisengage.Remove(onFrameVelocityChange);
		GameEvents.onRotatingFrameTransition.Remove(onRotatingFrameChanged);
		GameEvents.onDominantBodyChange.Remove(onReferencebodyChanged);
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		GameEvents.onLanguageSwitched.Remove(CacheLocalStrings);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onPartDie.Remove(OnPartEvent);
		GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
		GameEvents.OnVisorRaised.Remove(OnVisorRaised);
		GameEvents.OnEVAConstructionWeldStart.Remove(OnWeldStart);
		GameEvents.OnEVAConstructionWeldFinish.Remove(OnWeldFinish);
		KerbalFSM kerbalFSM = fsm;
		kerbalFSM.OnStateChange = (Callback<KFSMState, KFSMState, KFSMEvent>)Delegate.Remove(kerbalFSM.OnStateChange, new Callback<KFSMState, KFSMState, KFSMEvent>(UpdateInventoryPaw));
		if (suitColorChanger != null)
		{
			base.Fields["lightR"].OnValueModified -= UpdateSuitColors;
			base.Fields["lightG"].OnValueModified -= UpdateSuitColors;
			base.Fields["lightB"].OnValueModified -= UpdateSuitColors;
		}
		if (updateAvatarCoroutine != null)
		{
			StopCoroutine(updateAvatarCoroutine);
			updateAvatarCoroutine = null;
		}
		if (kerbalCamAtmos != null && kerbalPortraitCamera != null && kerbalCamAtmos.transform.parent != kerbalPortraitCamera.transform)
		{
			kerbalCamAtmos.transform.SetParent(kerbalPortraitCamera.transform);
			kerbalCamAtmos.transform.localPosition = Vector3.zero;
		}
		if (currentLadderPart != null)
		{
			currentLadderPart.hasKerbalOnLadder = false;
		}
	}

	public override void OnInactive()
	{
		if (evaChute.deploymentState != 0)
		{
			evaChute.CutParachute();
			evaChute.Repack();
		}
		base.part.protoModuleCrew[0].SaveEVAChute(evaChute);
		if (moduleInventoryPartReference != null)
		{
			base.part.protoModuleCrew[0].SaveInventory(moduleInventoryPartReference);
		}
		base.OnInactive();
	}

	public void CacheLocalStrings()
	{
		cacheAutoLOC_114130 = Localizer.Format("#autoLOC_114130", GameSettings.EVA_Use.name);
		cacheAutoLOC_114293 = Localizer.Format("#autoLOC_114293", GameSettings.EVA_Use.name);
		cacheAutoLOC_114297 = Localizer.Format("#autoLOC_114297", GameSettings.EVA_Use.name);
		cacheAutoLOC_114358 = Localizer.Format("#autoLOC_114358", GameSettings.EVA_Board.name);
		cacheAutoLOC_115662 = Localizer.Format("#autoLOC_115662", GameSettings.EVA_Jump.name);
		cacheAutoLOC_115694 = Localizer.Format("#autoLOC_115694", GameSettings.EVA_Jump.name);
		cacheAutoLOC_6010008 = Localizer.Format("#autoLOC_6010008");
		cacheAutoLOC_6010009 = Localizer.Format("#autoLOC_6010009");
		cacheAutoLOC_6010010 = Localizer.Format("#autoLOC_6010010");
		cacheAutoLOC_6010011 = Localizer.Format("#autoLOC_6010011");
		cacheAutoLOC_8003204 = Localizer.Format("#autoLOC_8003204");
		cacheAutoLOC_6010015 = Localizer.Format("#autoLOC_6010015");
		cacheAutoLOC_8002357 = Localizer.Format("#autoLOC_8002357", GameSettings.EVA_Jump.name);
		cacheAutoLOC_8002358 = Localizer.Format("#autoLOC_8002358", GameSettings.PAUSE.name);
		cacheAutoLOC_8002359 = Localizer.Format("#autoLOC_8002359", GameSettings.TRANSLATE_LEFT.name, GameSettings.TRANSLATE_RIGHT.name);
		cacheAutoLOC_8002360 = Localizer.Format("#autoLOC_8002360", GameSettings.TRANSLATE_LEFT.name, GameSettings.TRANSLATE_RIGHT.name, GameSettings.TRANSLATE_FWD.name, GameSettings.TRANSLATE_BACK.name, GameSettings.TRANSLATE_DOWN.name, GameSettings.TRANSLATE_UP.name);
		cacheAutoLOC_6012032 = Localizer.Format("#autoLOC_6012032", GameSettings.EVA_Use.name);
		cacheAutoLOC_6006049 = Localizer.Format("#autoLOC_6006049");
	}

	public void InitHelmetSetup()
	{
		neckRingTransformExists = neckRingTransform != null;
		helmetTransformExists = helmetTransform != null;
		ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
		if (protoCrewMember != null)
		{
			if (!protoCrewMember.completedFirstEVA)
			{
				protoCrewMember.completedFirstEVA = true;
				protoCrewMember.hasHelmetOn = GameSettings.EVA_DEFAULT_HELMET_ON;
				protoCrewMember.hasNeckRingOn = GameSettings.EVA_DEFAULT_NECKRING_ON;
				protoCrewMember.hasVisorDown = false;
			}
			isHelmetEnabled = protoCrewMember.hasHelmetOn;
			isNeckRingEnabled = protoCrewMember.hasNeckRingOn;
			isVisorEnabled = protoCrewMember.hasVisorDown;
			if (!CanEVAWithoutHelmet())
			{
				isHelmetEnabled = true;
			}
			ToggleHelmetAndNeckRing(isHelmetEnabled, isNeckRingEnabled, storeToPCM: false, suppressTransformMessages: true);
			OnHelmetChanged(this, isHelmetEnabled, isNeckRingEnabled);
			visorState = (IsVisorEnabled ? VisorStates.Lowered : VisorStates.Raised);
			visorTextureOffset = VisorRenderer.material.mainTextureOffset;
			visorTargetOffset = ((visorState == VisorStates.Lowered) ? visorLoweredTargetOffset : visorRaisedTargetOffset);
			visorTextureOffset.y = visorTargetOffset;
			VisorRenderer.material.mainTextureOffset = visorTextureOffset;
			UpdateVisorEventStates();
		}
		else
		{
			Debug.LogError("Unable to get the Proto Crew Member on Kerbal Eva (InitHelmetSetup)");
		}
	}

	public void ToggleHelmet(bool enableHelmet)
	{
		ToggleHelmetAndNeckRing(enableHelmet, isNeckRingEnabled);
	}

	public void ToggleNeckRing(bool enableNeckRing)
	{
		ToggleHelmetAndNeckRing(isHelmetEnabled, enableNeckRing);
	}

	public void ToggleHelmetAndNeckRing(bool enableHelmet, bool enableNeckRing, bool storeToPCM = true, bool suppressTransformMessages = false)
	{
		if (Ready && FlightGlobals.ActiveVessel != base.vessel)
		{
			return;
		}
		if (!helmetTransformExists)
		{
			if (!suppressTransformMessages)
			{
				Debug.LogError("Unable to set Helmet visibility as transform is missing");
			}
			return;
		}
		if (!enableHelmet && isHelmetEnabled && !CanSafelyRemoveHelmet())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003205", HelmetUnsafeReason), 5f, ScreenMessageStyle.UPPER_CENTER, Color.yellow);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (helmetTransform.gameObject.activeSelf != enableHelmet)
		{
			helmetTransform.gameObject.SetActive(enableHelmet);
			if (helmetCollider != null)
			{
				if (enableHelmet)
				{
					helmetCollider.radius = helmetColliderSetup.helmetOnRadius;
					helmetCollider.center = helmetColliderSetup.helmetOnCenter;
				}
				else
				{
					helmetCollider.radius = helmetColliderSetup.helmetOffRadius;
					helmetCollider.center = helmetColliderSetup.helmetOffCenter;
				}
			}
			flag = true;
		}
		if (neckRingTransform != null && neckRingTransform.gameObject.activeSelf != (enableNeckRing || enableHelmet))
		{
			neckRingTransform.gameObject.SetActive(enableNeckRing || enableHelmet);
			flag2 = true;
		}
		isHelmetEnabled = enableHelmet;
		isNeckRingEnabled = enableNeckRing;
		if (storeToPCM)
		{
			ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
			if (protoCrewMember != null)
			{
				protoCrewMember.hasHelmetOn = isHelmetEnabled;
				protoCrewMember.hasNeckRingOn = isNeckRingEnabled;
			}
		}
		if (flag || flag2)
		{
			GameEvents.OnHelmetChanged.Fire(this, isHelmetEnabled, isNeckRingEnabled);
			UpdateVisorEventStates();
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6010003")]
	public void ChangeHelmet()
	{
		ToggleHelmet(!isHelmetEnabled);
	}

	[KSPEvent(active = true, guiActive = false, guiName = "#autoLOC_6010005")]
	public void ChangeNeckRing()
	{
		if (!(neckRingTransform == null))
		{
			ToggleNeckRing(!isNeckRingEnabled || isHelmetEnabled);
		}
	}

	public void OnHelmetChanged(KerbalEVA changedKerbal, bool helmetVisible, bool neckRingVisible)
	{
		if (this != changedKerbal)
		{
			return;
		}
		if (!helmetTransformExists)
		{
			base.Events["ChangeHelmet"].guiActive = false;
			base.Events["ChangeNeckRing"].guiActive = false;
			return;
		}
		if (!neckRingTransformExists)
		{
			base.Events["ChangeNeckRing"].guiActive = false;
		}
		if (helmetVisible)
		{
			base.Events["ChangeHelmet"].guiName = "#autoLOC_6010003";
			base.Events["ChangeNeckRing"].guiActive = false;
			return;
		}
		base.Events["ChangeHelmet"].guiName = "#autoLOC_6010004";
		base.Events["ChangeNeckRing"].guiActive = neckRingTransform != null;
		if (neckRingVisible)
		{
			base.Events["ChangeNeckRing"].guiName = "#autoLOC_6010005";
		}
		else
		{
			base.Events["ChangeNeckRing"].guiName = "#autoLOC_6010006";
		}
	}

	public bool CheckHelmetOffSafe(bool includeSafetyMargins = true, bool startEVAChecks = false)
	{
		if (!startEVAChecks && LoadingBufferMask.Instance.loadingObject.gameObject.activeInHierarchy)
		{
			return true;
		}
		CelestialBody mainBody = base.vessel.mainBody;
		atmosExistence = mainBody.atmosphere && base.vessel.altitude < mainBody.atmosphereDepth;
		if (!atmosExistence)
		{
			helmetUnsafeReason = cacheAutoLOC_6010008;
			return false;
		}
		if (!mainBody.atmosphereContainsOxygen)
		{
			helmetUnsafeReason = cacheAutoLOC_8003204;
			return false;
		}
		kerbalStaticPressureAtm = ((!startEVAChecks) ? base.part.staticPressureAtm : GetPreLoadPressure(base.part.staticPressureAtm, mainBody));
		if (kerbalStaticPressureAtm < (double)(helmetOffMinSafePressureAtm + (includeSafetyMargins ? helmetOffMinSafePressureAtmMargin : 0f)))
		{
			helmetUnsafeReason = cacheAutoLOC_6010009;
			return false;
		}
		if (kerbalStaticPressureAtm > (double)(helmetOffMaxSafePressureAtm - (includeSafetyMargins ? helmetOffMaxSafePressureAtmMargin : 0f)))
		{
			helmetUnsafeReason = cacheAutoLOC_6006049;
			return false;
		}
		if ((base.part.Splashed || (startEVAChecks && base.vessel.altitude < 0.0 && mainBody.ocean)) && kerbalStaticPressureAtm > (double)(helmetOffMaxOceanPressureAtm - (includeSafetyMargins ? helmetOffMaxOceanPressureAtmMargin : 0f)))
		{
			helmetUnsafeReason = cacheAutoLOC_6010015;
			return false;
		}
		kerbalSkinTemp = ((!startEVAChecks) ? base.part.skinTemperature : mainBody.GetTemperature(base.vessel.altitude));
		kerbalInternalTemp = ((!startEVAChecks) ? base.part.temperature : mainBody.GetTemperature(base.vessel.altitude));
		if (!(kerbalSkinTemp < (double)(helmetOffMinSafeTempK + (includeSafetyMargins ? helmetOffMinSafeTempKMargin : 0f))) && kerbalInternalTemp >= (double)helmetOffMinSafeTempK)
		{
			if (!(kerbalSkinTemp > (double)(helmetOffMaxSafeTempK - (includeSafetyMargins ? helmetOffMaxSafeTempKMargin : 0f))) && kerbalInternalTemp <= (double)helmetOffMaxSafeTempK)
			{
				helmetUnsafeReason = "";
				return true;
			}
			helmetUnsafeReason = cacheAutoLOC_6010011;
			return false;
		}
		helmetUnsafeReason = cacheAutoLOC_6010010;
		return false;
	}

	public double GetPreLoadPressure(double loadValue, CelestialBody vesselBody)
	{
		if (loadValue != 0.0)
		{
			return loadValue;
		}
		double pressureAtm = vesselBody.GetPressureAtm(base.vessel.altitude);
		if (!(base.vessel.altitude >= 0.0) && vesselBody.ocean)
		{
			double num = vesselBody.GeeASL * PhysicsGlobals.GravitationalAcceleration * vesselBody.Radius * vesselBody.Radius / ((vesselBody.Radius + base.vessel.altitude) * (vesselBody.Radius + base.vessel.altitude));
			return pressureAtm + num * (0.0 - base.vessel.altitude) * vesselBody.oceanDensity * 0.009869232667160128;
		}
		return pressureAtm;
	}

	public virtual bool CanEVAWithoutHelmet()
	{
		return CheckHelmetOffSafe(includeSafetyMargins: true, startEVAChecks: true);
	}

	public virtual bool CanSafelyRemoveHelmet()
	{
		return CheckHelmetOffSafe();
	}

	public virtual bool WillDieWithoutHelmet()
	{
		return !CheckHelmetOffSafe(includeSafetyMargins: false);
	}

	public void UpdateHelmetOffChecks()
	{
		if (GameSettings.EVA_DIES_WHEN_UNSAFE_HELMET && base.part.State != PartStates.DEAD && Ready)
		{
			if (framesDelayForHelmetDeathCounter < framesDelayForHelmetDeathCheck)
			{
				framesDelayForHelmetDeathCounter++;
			}
			else if (!isHelmetEnabled && WillDieWithoutHelmet())
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6010013", base.part.protoModuleCrew[0].displayName, helmetUnsafeReason), 5f, ScreenMessageStyle.UPPER_CENTER, Color.red);
				base.part.explode();
			}
		}
	}

	public virtual void SetPhysicMaterial(PhysicMaterial newPhysicMaterial)
	{
		physicMaterial = newPhysicMaterial;
		for (int i = 0; i < characterColliders.Length; i++)
		{
			characterColliders[i].material = physicMaterial;
		}
		for (int j = 0; j < otherRagdollColliders.Length; j++)
		{
			otherRagdollColliders[j].material = physicMaterial;
		}
		for (int k = 0; k < otherPhysicColliders.Length; k++)
		{
			otherPhysicColliders[k].material = physicMaterial;
		}
		useGlobalPhysicMaterial = false;
	}

	public virtual void ResetPhysicMaterial()
	{
		physicMaterial = null;
		for (int i = 0; i < characterColliders.Length; i++)
		{
			characterColliders[i].sharedMaterial = PhysicsGlobals.KerbalEVAPhysicMaterial;
		}
		for (int j = 0; j < otherRagdollColliders.Length; j++)
		{
			otherRagdollColliders[j].sharedMaterial = PhysicsGlobals.KerbalEVAPhysicMaterial;
		}
		for (int k = 0; k < otherPhysicColliders.Length; k++)
		{
			otherPhysicColliders[k].sharedMaterial = PhysicsGlobals.KerbalEVAPhysicMaterial;
		}
		useGlobalPhysicMaterial = true;
	}

	public IEnumerator ReturnToIdle(float time)
	{
		yield return new WaitForSeconds(time);
		fsm.RunEvent(On_return_idle);
	}

	public bool SetPartPlacementMode(bool mode, ModuleInventoryPart moduleInventoryPartReference)
	{
		if (mode)
		{
			if (!isRagdoll && VesselUnderControl && !OnALadder && (!(base.vessel != null) || base.vessel.Landed))
			{
				if (moduleInventoryPartReference != null)
				{
					this.moduleInventoryPartReference = moduleInventoryPartReference;
				}
				partPlacementMode = true;
				return true;
			}
			return false;
		}
		partPlacementMode = false;
		return true;
	}

	public override Color GetCurrentColor()
	{
		return new Color(lightR, lightG, lightB);
	}

	public override List<Color> PresetColors()
	{
		return GameSettings.GetLightPresetColors();
	}

	public override void OnColorChanged(Color color, string id)
	{
		OnColorChanged(color);
	}

	public override void OnColorChanged(Color color)
	{
		ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
		if (protoCrewMember != null)
		{
			protoCrewMember.lightR = color.r;
			protoCrewMember.lightG = color.g;
			protoCrewMember.lightB = color.b;
		}
		lightR = color.r;
		lightG = color.g;
		lightB = color.b;
		UpdateSuitColors(null);
	}

	public void ModifyBodyColliderHeight(float newHeight)
	{
		for (int i = 0; i < bodyColliders.Length; i++)
		{
			bodyColliders[i].center = Vector3.up * newHeight;
		}
	}

	[ContextMenu("Set Ladder Anti-Sliding Mechanism")]
	public void SetLadderHold()
	{
		if (GameSettings.EVA_LADDER_JOINT_WHEN_IDLE && onLadder && !isLadderJointed)
		{
			ladderPosition = ((currentLadderPart != null) ? currentLadderPart.transform.InverseTransformPoint(base.transform.position) : currentLadder.transform.InverseTransformPoint(base.transform.position));
			isLadderJointed = true;
		}
	}

	[ContextMenu("Clear Ladder Anti-Sliding Mechanism")]
	public void ClearLadderHold()
	{
		isLadderJointed = false;
	}
}
