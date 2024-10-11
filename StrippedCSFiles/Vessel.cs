using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommNet;
using FinePrint;
using KSP.UI.Screens;
using RUI.Algorithms;
using UnityEngine;

[SelectionBase]
public class Vessel : MonoBehaviour, IShipconstruct, ITargetable, IDiscoverable
{
	public enum State
	{
		INACTIVE,
		ACTIVE,
		DEAD
	}

	public enum Situations
	{
		[Description("#autoLoc_6002161")]
		LANDED = 1,
		[Description("#autoLoc_6002162")]
		SPLASHED = 2,
		[Description("#autoLoc_6002163")]
		PRELAUNCH = 4,
		[Description("#autoLoc_6002164")]
		FLYING = 8,
		[Description("#autoLoc_6002165")]
		SUB_ORBITAL = 0x10,
		[Description("#autoLoc_6002166")]
		ORBITING = 0x20,
		[Description("#autoLoc_6002167")]
		ESCAPING = 0x40,
		[Description("#autoLoc_6002168")]
		DOCKED = 0x80
	}

	public enum ControlLevel
	{
		NONE,
		PARTIAL_UNMANNED,
		PARTIAL_MANNED,
		FULL
	}

	[CompilerGenerated]
	private sealed class _003CwaitAndResumeTargetInfo_003Ed__287 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ProtoTargetInfo protoTarget;

		public int framesToWait;

		public Vessel _003C_003E4__this;

		private ITargetable _003CsavedTgt_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CwaitAndResumeTargetInfo_003Ed__287(int _003C_003E1__state)
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
	private sealed class _003CwaitAndResumeNavigationInfo_003Ed__290 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int framesToWait;

		public Vessel _003C_003E4__this;

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
		public _003CwaitAndResumeNavigationInfo_003Ed__290(int _003C_003E1__state)
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

	public Guid id;

	public uint persistentId;

	public string vesselName;

	public List<Part> parts;

	public Part rootPart;

	[NonSerialized]
	private Part referenceTransformPart;

	[NonSerialized]
	private Part referenceTransformPartRecall;

	public uint referenceTransformId;

	public uint referenceTransformIdRecall;

	public VesselPrecalculate precalc;

	public OrbitDriver orbitDriver;

	public PatchedConicSolver patchedConicSolver;

	public PatchedConicRenderer patchedConicRenderer;

	public OrbitTargeter orbitTargeter;

	public OrbitRenderer orbitRenderer;

	public MapObject mapObject;

	public FlightCtrlState ctrlState;

	public FlightCtrlState[] setControlStates;

	public int PQSminLevel;

	public int PQSmaxLevel;

	public int currentStage;

	private float groundLevelAngle;

	public Quaternion srfRelRotation;

	public double longitude;

	public double latitude;

	public double altitude;

	public double radarAltitude;

	public ProtoVessel protoVessel;

	public bool loaded;

	public bool easingInToSurface;

	public double distanceTraveled;

	public Vector3 vesselSize;

	public AltimeterDisplayState altimeterDisplayState;

	public State state;

	public bool packed;

	public bool Landed;

	public bool Splashed;

	public bool permanentGroundContact;

	[SerializeField]
	internal CrashObjectName crashObjectName;

	public List<Part> GroundContacts;

	private FixedJoint joint;

	private float anchorTimeCounter;

	private float anchorForceAngle;

	private float prevAnchorForceAngle;

	private float deltaAngle;

	private float anchorDeltaAngleTimeCounter;

	public static List<string> VesselAnchorExceptions;

	public bool skipGroundPositioning;

	public bool skipGroundPositioningForDroppedPart;

	public bool vesselSpawning;

	public string launchedFrom;

	public string landedAt;

	public string displaylandedAt;

	public string landedAtLast;

	public Callback OnJustAboutToBeDestroyed;

	private bool bouncedOffCheck;

	private float camhdg;

	private float campth;

	private float camMode;

	private bool persistent;

	public Situations situation;

	public double missionTime;

	public double launchTime;

	public Vector3d obt_velocity;

	public Vector3d srf_velocity;

	public Vector3d srf_vel_direction;

	public Vector3 rb_velocity;

	public Vector3d rb_velocityD;

	public Vector3d velocityD;

	public double obt_speed;

	public Vector3d acceleration;

	public Vector3d acceleration_immediate;

	private Vector3d[] accelSmoothing;

	private int accelSmoothingCursor;

	private static int accelSmoothingLength;

	private double accelSmoothingLengthRecip;

	public Vector3d perturbation;

	public Vector3d perturbation_immediate;

	public double specificAcceleration;

	public Vector3d upAxis;

	public Vector3 CoM;

	public Vector3 MOI;

	public Vector3d CoMD;

	public Vector3 angularVelocity;

	public Vector3d angularVelocityD;

	public Vector3 angularMomentum;

	public double geeForce;

	public double geeForce_immediate;

	public double gravityMultiplier;

	internal double easeInMultiplier;

	private int ignoreGeeforceFrames;

	private int ignoreSpeedFrames;

	public Vector3d graviticAcceleration;

	public Vector3d gravityForPos;

	public Vector3d gravityTrue;

	internal int ignoreCollisionsFrames;

	public bool frameWasRotating;

	public double verticalSpeed;

	public double horizontalSrfSpeed;

	public double srfSpeed;

	public double indicatedAirSpeed;

	public double mach;

	public double speed;

	public double externalTemperature;

	public double atmosphericTemperature;

	public double staticPressurekPa;

	public double dynamicPressurekPa;

	public double atmDensity;

	public double speedOfSound;

	public double distanceToSun;

	public Vector3d up;

	public Vector3d north;

	public Vector3d east;

	public double convectiveMachFlux;

	public double convectiveCoefficient;

	public double solarFlux;

	public bool directSunlight;

	public double totalMass;

	public double lastUT;

	public double waterOffset;

	public Vector3d lastVel;

	public Vector3d nextVel;

	public CelestialBody lastBody;

	public VesselType vesselType;

	public KerbalEVA evaController;

	private bool wasLadder;

	private VesselAutopilot autopilot;

	protected List<ProtoCrewMember> crew;

	protected int crewCachedPartCount;

	public int crewedParts;

	public int crewableParts;

	private bool isControllable;

	private ControlLevel controlLevel;

	public ControlLevel maxControlLevel;

	public static bool PartialControlHasSASRCS;

	private bool controlLockSet;

	private bool controlLockPartial;

	public bool EditableNodes;

	public ConfigNode flightPlanNode;

	public ITargetable targetObject;

	public Waypoint navigationWaypoint;

	private VesselValues vesselValues;

	public VesselDeltaV VesselDeltaV;

	public List<VesselModule> vesselModules;

	public CommNetVessel connection;

	private CometVessel comet;

	private SuspensionLoadBalancer _suspensionLoadBalancer;

	private bool useFramesAtStartupOverride;

	private int hold_count_override;

	private int framesAtStartupOverride;

	private Vector3 rootOrgPos;

	private Quaternion rootOrgRot;

	internal bool isUnloading;

	internal bool isBackingUp;

	public FlightInputCallback OnPreAutopilotUpdate;

	public FlightInputCallback OnAutopilotUpdate;

	public FlightInputCallback OnPostAutopilotUpdate;

	public FlightInputCallback OnFlyByWire;

	public ProtoTargetInfo pTI;

	private ProtoWaypointInfo pWPI;

	public VesselRanges vesselRanges;

	private bool physicsHoldLock;

	private int framesAtStartup;

	private RaycastHit groundCollisionHit;

	public static double HeightFromPartOffsetGlobal;

	public double heightFromPartOffsetLocal;

	public float heightFromTerrain;

	public float heightFromSurface;

	public double terrainAltitude;

	public double pqsAltitude;

	public Vector3 terrainNormal;

	public GameObject objectUnderVessel;

	private RaycastHit heightFromTerrainHit;

	private RaycastHit heightFromSurfaceHit;

	private double FG_geeForce;

	public Transform vesselTransform;

	internal bool initialPosVelSet;

	public float gThresh;

	public float presThresh;

	public double partMaxGThresh;

	public double partMaxPresThresh;

	public static double warningThresholdG;

	public static double warningThresholdPres;

	private static ScreenMessage gMessage;

	private static ScreenMessage presMessage;

	private static bool messageObjectsCached;

	private int oldPartCount;

	public List<PartModule> dockingPorts;

	private Situations lastSituation;

	public Vector3d krakensbaneAcc;

	public Vector3 localCoM;

	private const float maxVisibleDistanceSqr = 25000000f;

	[NonSerialized]
	public PartSet resourcePartSet;

	[NonSerialized]
	public PartSet simulationResourcePartSet;

	public StackFlowGraph flowGraph;

	public bool updateResourcesOnEvent;

	public bool resourcesDirty;

	public List<PartSet> crossfeedSets;

	public List<PartSet> simulationCrossfeedSets;

	private ActionGroupList actionGroups;

	public static int NumOverrideGroups;

	public int GroupOverride;

	public bool[] OverrideDefault;

	public KSPActionGroup[] OverrideActionControl;

	public KSPAxisGroup[] OverrideAxisControl;

	public string[] OverrideGroupNames;

	private VesselRenameDialog renameDialog;

	private Part vesselNamedBy;

	private FlightIntegrator _fi;

	private DiscoveryInfo discoveryInfo;

	private bool autoClean;

	private string autoCleanReason;

	private static string cacheAutoLOC_6002161;

	private static string cacheAutoLOC_6002162;

	private static string cacheAutoLOC_6002163;

	private static string cacheAutoLOC_6002164;

	private static string cacheAutoLOC_6002165;

	private static string cacheAutoLOC_6002166;

	private static string cacheAutoLOC_6002167;

	private static string cacheAutoLOC_6002168;

	private static string cacheAutoLOC_6002169;

	private static string cacheAutoLOC_348557;

	private static string cacheAutoLOC_348558;

	private static string cacheAutoLOC_348559;

	private static string cacheAutoLOC_348560;

	private static string cacheAutoLOC_348561;

	private static string cacheAutoLOC_348562;

	private static string cacheAutoLOC_348563;

	private static string cacheAutoLOC_348564;

	private static string cacheAutoLOC_348565;

	private static string cacheAutoLOC_145785;

	private static string cacheAutoLOC_145801;

	private static string cacheAutoLOC_145790;

	private static string cacheAutoLOC_145792;

	private static string cacheAutoLOC_145793;

	public Part this[uint flightID]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<Part> Parts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform ReferenceTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
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

	public CelestialBody mainBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GroundLevelAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool LandedOrSplashed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAnchored
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool LandedInKSC
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool LandedInStockLaunchSite
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HoldPhysics
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isCommandable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isPersistent
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

	public bool isActiveVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsRecoverable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Situations BestSituation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SituationString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IgnoreSpeedActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int IgnoreCollisionsFrames
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 CurrentCoM
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isEVA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel EVALadderVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VesselAutopilot Autopilot
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ControlLevel CurrentControlLevel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsControllable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VesselValues VesselValues
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CommNetVessel Connection
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

	public CometVessel Comet
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal set
		{
			throw null;
		}
	}

	public SuspensionLoadBalancer suspensionLoadBalancer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal set
		{
			throw null;
		}
	}

	public bool PatchedConicsAttached
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

	public RaycastHit HeightFromSurfaceHit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel VesselSurface
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

	public ActionGroupList ActionGroups
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distancePackThreshold
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

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceUnpackThreshold
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

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceLandedPackThreshold
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

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceLandedUnpackThreshold
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

	[Obsolete("Use Vessel.vesselRanges instead")]
	public static float loadDistance
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

	[Obsolete("Use Vessel.vesselRanges instead")]
	public static float unloadDistance
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

	public DiscoveryInfo DiscoveryInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool AutoClean
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string AutoCleanReason
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Vessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part PartsContain(uint partPersistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetReferenceTransformPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetReferenceTransform(Part p, bool storeRecall = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecallReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part FindReferenceTransform(uint refId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FallBackReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit GetCurrentOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetWorldPos3D()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLoaded(bool loaded)
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
	[ContextMenu("Print Ground Contacts")]
	public void printGroundContacts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Print All Collisions")]
	public void printCollisions()
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
	private void AddRBAnchor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetRBAnchor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnJointBreak(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLandedAt(string landedAt, GameObject gO = null, string inputdisplaylandedAt = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoCrewMember> GetVesselCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CrewListSetDirty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RebuildCrewList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCrew(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCrew(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrewList(List<ProtoCrewMember> pcmList, bool updatePartCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewCapacity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CrewWasModified(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CrewWasModified(Vessel vessel1, Vessel vessel2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ControlLevel GetControlLevel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckControllable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadVesselModules(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVesselModuleActivation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetStaticPressureAndExternalTemp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CacheScreenMessageObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPhysicsHoldLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemovePhysicsHoldLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PhysicsHoldExpired()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetPhysicsHoldExpiryOverride(int newHoldCountValue = 5)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Initialize(bool fromShipAssembly = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool Initialize(bool fromShipAssembly, bool preCreate, bool orbiting, bool setActiveVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void findVesselParts(Part part, bool checkAttached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartFromBackup(ProtoVessel pv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartFromBackup(ProtoVessel pv, FlightState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Load")]
	public void Load()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Unload")]
	public void Unload()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoVessel BackupVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGroupOverride(int newGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KSPAxisGroup BlockedMask(int overrideGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetControlState(FlightCtrlState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetControlState(FlightCtrlState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FeedInputFeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MakeActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MakeInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearStaging()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResumeStaging()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResumeTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndResumeTargetInfo_003Ed__287))]
	private IEnumerator waitAndResumeTargetInfo(int framesToWait, ProtoTargetInfo protoTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResumeNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndResumeNavigationInfo_003Ed__290))]
	private IEnumerator waitAndResumeNavigationInfo(int framesToWait)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vessel GetDominantVessel(Vessel v1, Vessel v2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddOrbitRenderer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AttachPatchedConicsSolver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DetachPatchedConicsSolver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool patchedConicsUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool vesselOrbitsUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GoOnRails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GoOffRails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckGroundCollision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float getLowestPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetHeightFromTerrain(bool overRidePacked = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetHeightFromSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RaycastHit GetHitFromSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetGroundLevelAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getCorrectedLandedAltitude(double lat, double lon, double alt, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double PQSAltitude()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckKill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePosVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDistanceTraveled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAcceleration(double fdtRecip, bool fromUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IgnoreGForces(int frames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IgnoreSpeed(int frames)
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
	public void UpdateCaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLandedSplashed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeWorldVelocity(Vector3d velOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWorldVelocity(Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyVesselComponents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MurderCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KillPermanentGroundContact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetGroundContact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetTotalMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetUnloadedVesselMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSituationString(Situations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSituationString(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetLandedAtString(string landedAt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetMETString(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetNextManeuverTime(Vessel v, out bool hasManeuver)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string AutoRename(Vessel v, string baseName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckAirstreamShields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSaveFlightState(Dictionary<string, KSPParseable> dataPool)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLoadFlightState(Dictionary<string, KSPParseable> dataPool)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ClearToSaveStatus IsClearToSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPosition(Vector3d position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPosition(Vector3d position, bool usePristineCoords)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Translate(Vector3d offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetRotation(Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetRotation(Quaternion rotation, bool setPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OffsetVelocity(Vector3d correction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetObtVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetSrfVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetFwdVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit GetOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitDriver GetOrbitDriver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselTargetModes GetTargetingMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetActiveTargetable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Part> GetActiveParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> FindPartModulesImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindPartModuleImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FindVesselModuleImplementing<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselPartCountChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVesselSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourceSets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourceSetsIfDirty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCrossfeedResourceSetFL(GameEvents.HostedFromToAction<bool, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceSetsEventCheckPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildCrossfeedPartSets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestResource(Part part, int id, double demand, bool usePriority)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestResource(Part part, int id, double demand, bool usePriority, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ActionControlBlocked(KSPActionGroup actionGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AxisControlBlocked(KSPAxisGroup axisGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyOverrides(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Rename Vessel")]
	public void RenameVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRenameAccept(string newVesselName, VesselType newVesselType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRenameDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsValidVesselName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartVesselNamingChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselNamingVesselWasModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool UpdateVesselNaming(bool noGameEvent = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RunVesselNamingUpdates(Part pNewName, bool noGameEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselType FindDefaultVesselType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActiveInternalSpace(Part visiblePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearActiveInternalSpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActiveInternalSpaces(HashSet<Part> visibleParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsCollider(Collider c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CycleAllAutoStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValidContractObjectives(params string[] objectiveTypes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValidContractObjectives(List<string> objectiveTypes, bool copy = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFirstFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealAltitude()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealSituationString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float RevealMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAutoClean(string reason = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clean(string reason = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool HasMakingHistoryParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool HasSerenityParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool HasSerenityRoboticParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool HasSerenityRoboticController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityRoboticParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityModRoboticParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityRotorParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityHingeParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityPistonParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityRotationServoParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int CountSerenityRoboticController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool CountSerenityControllerFields(out int countControllers, out int countAxes, out int countActions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool HasModParts()
	{
		throw null;
	}
}
