using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CommNet;
using Contracts;
using Expansions.Missions.Scenery.Scripts;
using FinePrint;
using ns11;
using ns3;
using ns9;
using UnityEngine;
using VehiclePhysics;

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

	public Guid id;

	public uint persistentId;

	public string vesselName;

	public List<Part> parts;

	public Part rootPart;

	[NonSerialized]
	public Part referenceTransformPart;

	[NonSerialized]
	public Part referenceTransformPartRecall;

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

	public float groundLevelAngle = -1f;

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
	public CrashObjectName crashObjectName;

	public List<Part> GroundContacts;

	public FixedJoint joint;

	public float anchorTimeCounter;

	public float anchorForceAngle;

	public float prevAnchorForceAngle = -1f;

	public float deltaAngle;

	public float anchorDeltaAngleTimeCounter;

	public static List<string> VesselAnchorExceptions;

	public bool skipGroundPositioning;

	public bool skipGroundPositioningForDroppedPart;

	public bool vesselSpawning = true;

	public string launchedFrom = "";

	public string landedAt = "";

	public string displaylandedAt = "";

	public string landedAtLast = "";

	public Callback OnJustAboutToBeDestroyed = delegate
	{
	};

	public bool bouncedOffCheck;

	public float camhdg;

	public float campth;

	public float camMode = -1f;

	public bool persistent;

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

	public Vector3d[] accelSmoothing;

	public int accelSmoothingCursor;

	public static int accelSmoothingLength = 20;

	public double accelSmoothingLengthRecip;

	public Vector3d perturbation;

	public Vector3d perturbation_immediate;

	public double specificAcceleration;

	public Vector3d upAxis;

	public Vector3 CoM;

	public Vector3 vector3_0;

	public Vector3d CoMD;

	public Vector3 angularVelocity = Vector3.zero;

	public Vector3d angularVelocityD = Vector3d.zero;

	public Vector3 angularMomentum = Vector3.zero;

	public double geeForce;

	public double geeForce_immediate;

	public double gravityMultiplier = 1.0;

	public double easeInMultiplier = 1.0;

	public int ignoreGeeforceFrames;

	public int ignoreSpeedFrames;

	public Vector3d graviticAcceleration;

	public Vector3d gravityForPos;

	public Vector3d gravityTrue;

	public int ignoreCollisionsFrames;

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

	public double lastUT = -1.0;

	public double waterOffset;

	public Vector3d lastVel;

	public Vector3d nextVel;

	public CelestialBody lastBody;

	public VesselType vesselType;

	public KerbalEVA evaController;

	public bool wasLadder;

	public VesselAutopilot autopilot;

	public List<ProtoCrewMember> crew = new List<ProtoCrewMember>();

	public int crewCachedPartCount = -1;

	public int crewedParts;

	public int crewableParts;

	public bool isControllable;

	public ControlLevel controlLevel;

	public ControlLevel maxControlLevel = ControlLevel.FULL;

	public static bool PartialControlHasSASRCS = true;

	public bool controlLockSet;

	public bool controlLockPartial;

	public bool EditableNodes;

	public ConfigNode flightPlanNode;

	public ITargetable targetObject;

	public Waypoint navigationWaypoint;

	public VesselValues vesselValues;

	public VesselDeltaV VesselDeltaV;

	public List<VesselModule> vesselModules = new List<VesselModule>();

	public CommNetVessel connection;

	public CometVessel comet;

	public SuspensionLoadBalancer _suspensionLoadBalancer;

	public bool useFramesAtStartupOverride;

	public int hold_count_override = 5;

	public int framesAtStartupOverride;

	public Vector3 rootOrgPos;

	public Quaternion rootOrgRot;

	public bool isUnloading;

	public bool isBackingUp;

	public FlightInputCallback OnPreAutopilotUpdate = delegate
	{
	};

	public FlightInputCallback OnAutopilotUpdate = delegate
	{
	};

	public FlightInputCallback OnPostAutopilotUpdate = delegate
	{
	};

	public FlightInputCallback OnFlyByWire = delegate
	{
	};

	public ProtoTargetInfo pTI;

	public ProtoWaypointInfo pWPI;

	public VesselRanges vesselRanges;

	public bool physicsHoldLock;

	public int framesAtStartup;

	public RaycastHit groundCollisionHit;

	public static double HeightFromPartOffsetGlobal = 0.5;

	public double heightFromPartOffsetLocal;

	public float heightFromTerrain = -1f;

	public float heightFromSurface = -1f;

	public double terrainAltitude;

	public double pqsAltitude;

	public Vector3 terrainNormal;

	public GameObject objectUnderVessel;

	public RaycastHit heightFromTerrainHit;

	public RaycastHit heightFromSurfaceHit;

	public double FG_geeForce;

	public Transform vesselTransform;

	public bool initialPosVelSet;

	public float gThresh;

	public float presThresh;

	public double partMaxGThresh;

	public double partMaxPresThresh;

	public static double warningThresholdG = 0.8;

	public static double warningThresholdPres = 0.8;

	public static ScreenMessage gMessage;

	public static ScreenMessage presMessage;

	public static bool messageObjectsCached = false;

	public int oldPartCount = -1;

	public List<PartModule> dockingPorts = new List<PartModule>();

	public Situations lastSituation;

	public Vector3d krakensbaneAcc;

	public Vector3 localCoM;

	public const float maxVisibleDistanceSqr = 25000000f;

	[NonSerialized]
	public PartSet resourcePartSet;

	[NonSerialized]
	public PartSet simulationResourcePartSet;

	public StackFlowGraph flowGraph;

	public bool updateResourcesOnEvent = true;

	public bool resourcesDirty;

	public List<PartSet> crossfeedSets = new List<PartSet>();

	public List<PartSet> simulationCrossfeedSets = new List<PartSet>();

	public ActionGroupList actionGroups;

	public static int NumOverrideGroups = 4;

	public int GroupOverride;

	public bool[] OverrideDefault;

	public KSPActionGroup[] OverrideActionControl;

	public KSPAxisGroup[] OverrideAxisControl;

	public string[] OverrideGroupNames;

	public VesselRenameDialog renameDialog;

	public Part vesselNamedBy;

	public FlightIntegrator _fi;

	public DiscoveryInfo discoveryInfo;

	public bool autoClean;

	public string autoCleanReason = string.Empty;

	public static string cacheAutoLOC_6002161;

	public static string cacheAutoLOC_6002162;

	public static string cacheAutoLOC_6002163;

	public static string cacheAutoLOC_6002164;

	public static string cacheAutoLOC_6002165;

	public static string cacheAutoLOC_6002166;

	public static string cacheAutoLOC_6002167;

	public static string cacheAutoLOC_6002168;

	public static string cacheAutoLOC_6002169;

	public static string cacheAutoLOC_348557;

	public static string cacheAutoLOC_348558;

	public static string cacheAutoLOC_348559;

	public static string cacheAutoLOC_348560;

	public static string cacheAutoLOC_348561;

	public static string cacheAutoLOC_348562;

	public static string cacheAutoLOC_348563;

	public static string cacheAutoLOC_348564;

	public static string cacheAutoLOC_348565;

	public static string cacheAutoLOC_145785;

	public static string cacheAutoLOC_145801;

	public static string cacheAutoLOC_145790;

	public static string cacheAutoLOC_145792;

	public static string cacheAutoLOC_145793;

	public Part this[uint flightID]
	{
		get
		{
			int count = parts.Count;
			Part part;
			do
			{
				if (count-- > 0)
				{
					part = parts[count];
					continue;
				}
				return null;
			}
			while (part.flightID != flightID);
			return part;
		}
	}

	public Part this[int index]
	{
		get
		{
			if (index >= 0 && parts.Count > index)
			{
				return parts[index];
			}
			return null;
		}
	}

	public List<Part> Parts => parts;

	public Transform ReferenceTransform
	{
		get
		{
			if (!(referenceTransformPart != null))
			{
				return vesselTransform;
			}
			return referenceTransformPart.GetReferenceTransform();
		}
	}

	public Orbit orbit => orbitDriver.orbit;

	public CelestialBody mainBody => orbit.referenceBody;

	public float GroundLevelAngle => groundLevelAngle;

	public bool LandedOrSplashed
	{
		get
		{
			if (!Landed)
			{
				return Splashed;
			}
			return true;
		}
	}

	public bool IsAnchored => joint != null;

	public bool LandedInKSC
	{
		get
		{
			string text = (loaded ? landedAt : protoVessel.landedAt);
			if (!text.Contains("KSC") && !text.Contains("LaunchPad"))
			{
				return text.Contains("Runway");
			}
			return true;
		}
	}

	public bool LandedInStockLaunchSite
	{
		get
		{
			string text = (loaded ? landedAt : protoVessel.landedAt);
			int num = 0;
			while (true)
			{
				if (num < PSystemSetup.Instance.StockLaunchSites.Length)
				{
					if (text.Contains(PSystemSetup.Instance.StockLaunchSites[num].name))
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

	public bool HoldPhysics => physicsHoldLock;

	public bool isCommandable => vesselType > VesselType.Debris;

	public bool isPersistent
	{
		get
		{
			if (loaded)
			{
				int count = parts.Count;
				int num = 0;
				while (true)
				{
					if (num < count)
					{
						if (parts[num].isPersistent)
						{
							break;
						}
						num++;
						continue;
					}
					persistent = false;
					return false;
				}
				persistent = true;
				return true;
			}
			return persistent;
		}
		set
		{
			persistent = value;
		}
	}

	public bool isActiveVessel => FlightGlobals.ActiveVessel == this;

	public bool IsRecoverable
	{
		get
		{
			if (LandedOrSplashed)
			{
				return mainBody.isHomeWorld;
			}
			return false;
		}
	}

	public Situations BestSituation
	{
		get
		{
			if (lastSituation == Situations.FLYING)
			{
				return lastSituation;
			}
			return situation;
		}
	}

	public string SituationString => situation switch
	{
		Situations.SUB_ORBITAL => cacheAutoLOC_6002165, 
		Situations.FLYING => cacheAutoLOC_6002164, 
		Situations.LANDED => cacheAutoLOC_6002161, 
		Situations.SPLASHED => cacheAutoLOC_6002162, 
		Situations.PRELAUNCH => cacheAutoLOC_6002163, 
		Situations.DOCKED => cacheAutoLOC_6002168, 
		Situations.ESCAPING => cacheAutoLOC_6002167, 
		Situations.ORBITING => cacheAutoLOC_6002166, 
		_ => cacheAutoLOC_6002169, 
	};

	public bool IgnoreSpeedActive => ignoreSpeedFrames > 0;

	public int IgnoreCollisionsFrames => ignoreCollisionsFrames;

	public Vector3 CurrentCoM => CoM;

	public bool isEVA => vesselType == VesselType.const_11;

	public Vessel EVALadderVessel
	{
		get
		{
			if (isEVA && evaController != null && evaController.LadderPart != null && evaController.LadderPart.vessel != null)
			{
				return evaController.LadderPart.vessel;
			}
			return this;
		}
	}

	public VesselAutopilot Autopilot => autopilot;

	public ControlLevel CurrentControlLevel => controlLevel;

	public bool IsControllable => isControllable;

	public VesselValues VesselValues => vesselValues;

	public CommNetVessel Connection
	{
		get
		{
			return connection;
		}
		set
		{
			connection = value;
		}
	}

	public CometVessel Comet
	{
		get
		{
			return comet;
		}
		set
		{
			comet = value;
		}
	}

	public SuspensionLoadBalancer suspensionLoadBalancer
	{
		get
		{
			return _suspensionLoadBalancer;
		}
		set
		{
			_suspensionLoadBalancer = value;
		}
	}

	public bool PatchedConicsAttached { get; set; }

	public RaycastHit HeightFromSurfaceHit => heightFromSurfaceHit;

	public Vessel VesselSurface { get; set; }

	public ActionGroupList ActionGroups => actionGroups;

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distancePackThreshold
	{
		get
		{
			return vesselRanges.orbit.pack;
		}
		set
		{
			vesselRanges.orbit.pack = value;
			vesselRanges.subOrbital.pack = value;
			vesselRanges.flying.pack = value;
			vesselRanges.escaping.pack = value;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceUnpackThreshold
	{
		get
		{
			return vesselRanges.orbit.unpack;
		}
		set
		{
			vesselRanges.orbit.unpack = value;
			vesselRanges.subOrbital.unpack = value;
			vesselRanges.flying.unpack = value;
			vesselRanges.escaping.unpack = value;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceLandedPackThreshold
	{
		get
		{
			return vesselRanges.landed.pack;
		}
		set
		{
			vesselRanges.landed.pack = value;
			vesselRanges.splashed.pack = value;
			vesselRanges.prelaunch.pack = value;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public float distanceLandedUnpackThreshold
	{
		get
		{
			return vesselRanges.landed.unpack;
		}
		set
		{
			vesselRanges.landed.unpack = value;
			vesselRanges.splashed.unpack = value;
			vesselRanges.prelaunch.unpack = value;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public static float loadDistance
	{
		get
		{
			return PhysicsGlobals.Instance.VesselRangesDefault.orbit.load;
		}
		set
		{
			PhysicsGlobals.Instance.VesselRangesDefault.landed.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.splashed.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.prelaunch.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.flying.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.orbit.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.subOrbital.load = value;
			PhysicsGlobals.Instance.VesselRangesDefault.escaping.load = value;
		}
	}

	[Obsolete("Use Vessel.vesselRanges instead")]
	public static float unloadDistance
	{
		get
		{
			return PhysicsGlobals.Instance.VesselRangesDefault.orbit.unload;
		}
		set
		{
			PhysicsGlobals.Instance.VesselRangesDefault.landed.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.splashed.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.prelaunch.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.flying.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.orbit.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.subOrbital.unload = value;
			PhysicsGlobals.Instance.VesselRangesDefault.escaping.unload = value;
		}
	}

	public DiscoveryInfo DiscoveryInfo => discoveryInfo;

	public bool AutoClean => autoClean;

	public string AutoCleanReason => autoCleanReason;

	public Part PartsContain(uint partPersistentId)
	{
		int count = parts.Count;
		Part part;
		do
		{
			if (count-- > 0)
			{
				part = parts[count];
				continue;
			}
			return null;
		}
		while (part.persistentId != partPersistentId);
		return part;
	}

	public Part GetReferenceTransformPart()
	{
		return referenceTransformPart;
	}

	public void SetReferenceTransform(Part p, bool storeRecall = true)
	{
		if (p != null)
		{
			GameEvents.onVesselReferenceTransformSwitch.Fire(ReferenceTransform, p.GetReferenceTransform());
			referenceTransformId = p.flightID;
			referenceTransformPart = p;
		}
		else
		{
			GameEvents.onVesselReferenceTransformSwitch.Fire(ReferenceTransform, vesselTransform);
			referenceTransformId = 0u;
			referenceTransformPart = null;
		}
		if (storeRecall)
		{
			referenceTransformIdRecall = referenceTransformId;
			referenceTransformPartRecall = referenceTransformPart;
		}
	}

	public void RecallReferenceTransform()
	{
		if ((referenceTransformPartRecall == null && referenceTransformIdRecall == 0) || referenceTransformPartRecall.vessel == this)
		{
			SetReferenceTransform(referenceTransformPartRecall);
			return;
		}
		referenceTransformIdRecall = referenceTransformId;
		referenceTransformPartRecall = referenceTransformPart;
	}

	public Part FindReferenceTransform(uint refId)
	{
		Part part = this[refId];
		if (part == null)
		{
			Debug.LogWarning("Couldn't find part with id " + refId, base.gameObject);
			referenceTransformId = 0u;
			return null;
		}
		return part;
	}

	public void FallBackReferenceTransform()
	{
		if (this[referenceTransformId] == null)
		{
			SetReferenceTransform(ShipConstruction.findFirstCrewablePart(rootPart));
		}
	}

	public Orbit GetCurrentOrbit()
	{
		if (orbitDriver != null)
		{
			return orbit;
		}
		Debug.LogError("Vessel.GetCurrentOrbit: orbit driver is null!");
		double universalTime = Planetarium.GetUniversalTime();
		Orbit obj = protoVessel.orbitSnapShot.Load();
		obj.UpdateFromUT(universalTime);
		return obj;
	}

	public Vector3d GetWorldPos3D()
	{
		if (precalc != null && !precalc.firstStatsRunComplete)
		{
			precalc.CalculatePhysicsStats();
		}
		return CoMD;
	}

	public void SetLoaded(bool loaded)
	{
		this.loaded = loaded;
		UpdateVesselModuleActivation();
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.SortVessel(this, loaded);
		}
	}

	public bool checkLanded()
	{
		if (loaded && !packed && !precalc.isEasingGravity)
		{
			Landed = false;
			anchorTimeCounter = 0f;
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = parts[i];
				if (part.GroundContact || part.PermanentGroundContact)
				{
					Landed = true;
					break;
				}
			}
			if (!Landed)
			{
				SetLandedAt("");
			}
			if (Landed && mainBody != null)
			{
				PQSminLevel = mainBody.pqsController.minLevel;
				PQSmaxLevel = mainBody.pqsController.maxLevel;
			}
		}
		return Landed;
	}

	public bool checkSplashed()
	{
		if (loaded)
		{
			Splashed = false;
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				if (parts[i].WaterContact)
				{
					Splashed = true;
					Landed = false;
					break;
				}
			}
		}
		return Splashed;
	}

	[ContextMenu("Print Ground Contacts")]
	public void printGroundContacts()
	{
		bool flag = false;
		for (int i = 0; i < GroundContacts.Count; i++)
		{
			Debug.Log("Ground Contact on " + GroundContacts[i].name, GroundContacts[i].gameObject);
			flag = true;
		}
		if (!flag)
		{
			Debug.Log("No Ground Contacts.");
		}
	}

	[ContextMenu("Print All Collisions")]
	public void printCollisions()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			int count2 = part.currentCollisions.Count;
			for (int j = 0; j < count2; j++)
			{
				Collider collider = part.currentCollisions.KeyAt(j);
				Debug.Log(part.currentCollisions.ValuesList[j] + " collisions registered between " + part.name + "...", part.gameObject);
				Debug.Log("...and " + collider.name + " in " + collider.transform.root.name, collider.gameObject);
			}
		}
	}

	[ContextMenu("Reset Collision Ignores")]
	public void ResetCollisionIgnores()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			parts[i].SetCollisionIgnores();
		}
	}

	public void AddRBAnchor()
	{
		if (isEVA || vesselType == VesselType.Flag || state == State.DEAD)
		{
			return;
		}
		for (int i = 0; i < GroundContacts.Count; i++)
		{
			for (int j = 0; j < VesselAnchorExceptions.Count; j++)
			{
				if (GroundContacts[i].Modules.Contains(VesselAnchorExceptions[j]))
				{
					ResetRBAnchor();
					return;
				}
			}
		}
		anchorTimeCounter += Time.deltaTime;
		if (joint != null)
		{
			if (state == State.ACTIVE && (ctrlState.wheelThrottle != 0f || ctrlState.mainThrottle > 0f))
			{
				ResetRBAnchor();
				return;
			}
			anchorForceAngle = Vector3.Angle(joint.currentForce, Vector3.up);
			if (anchorDeltaAngleTimeCounter >= GameSettings.VESSEL_ANCHOR_ANGLE_TIME_THRESHOLD)
			{
				if (prevAnchorForceAngle > 0f)
				{
					deltaAngle = anchorForceAngle - prevAnchorForceAngle;
				}
				else
				{
					deltaAngle = 0f;
				}
				if (Mathf.Abs(deltaAngle) >= GameSettings.VESSEL_ANCHOR_ANGLE_CHANGE_THRESHOLD)
				{
					ResetRBAnchor();
					return;
				}
				anchorDeltaAngleTimeCounter = 0f;
				prevAnchorForceAngle = anchorForceAngle;
			}
			anchorDeltaAngleTimeCounter += Time.deltaTime;
			if (Vector3.Dot(joint.currentForce, FlightGlobals.getUpAxis(mainBody, base.transform.position)) < 0f)
			{
				ResetRBAnchor();
			}
			else
			{
				joint.breakForce = Mathf.Max((float)totalMass, PhysicsGlobals.PartRBMassMin) * (float)graviticAcceleration.magnitude * GameSettings.VESSEL_ANCHOR_BREAK_FORCE_FACTOR;
			}
		}
		else if (loaded && !packed && !precalc.isEasingGravity && Landed && rb_velocity.sqrMagnitude < GameSettings.VESSEL_ANCHOR_VELOCITY_THRESHOLD && anchorTimeCounter >= GameSettings.VESSEL_ANCHOR_TIME_THRESHOLD && !GameSettings.WHEEL_THROTTLE_DOWN.GetKey() && !GameSettings.WHEEL_THROTTLE_UP.GetKey())
		{
			ResetRBAnchor();
			joint = base.gameObject.AddComponent<FixedJoint>();
			joint.breakForce = Mathf.Max((float)totalMass, PhysicsGlobals.PartRBMassMin) * (float)graviticAcceleration.magnitude * GameSettings.VESSEL_ANCHOR_BREAK_FORCE_FACTOR;
			joint.breakTorque = GameSettings.VESSEL_ANCHOR_BREAK_TORQUE;
			joint.enablePreprocessing = false;
		}
	}

	public void ResetRBAnchor()
	{
		if (joint != null)
		{
			UnityEngine.Object.Destroy(joint);
		}
		anchorTimeCounter = 0f;
		anchorDeltaAngleTimeCounter = 0f;
		prevAnchorForceAngle = -1f;
	}

	public void OnJointBreak(float breakForce)
	{
		ResetRBAnchor();
	}

	public void SetLandedAt(string landedAt, GameObject gO = null, string inputdisplaylandedAt = "")
	{
		if (this.landedAt == landedAt || landedAt == "Untagged")
		{
			return;
		}
		this.landedAt = landedAt;
		displaylandedAt = landedAt;
		if (!string.IsNullOrEmpty(landedAt))
		{
			landedAtLast = landedAt;
		}
		else if (landedAtLast == null)
		{
			landedAtLast = string.Empty;
		}
		if (inputdisplaylandedAt != string.Empty)
		{
			displaylandedAt = inputdisplaylandedAt;
		}
		else
		{
			if (!(landedAt != string.Empty))
			{
				return;
			}
			bool isFacility = false;
			if (!PSystemSetup.Instance.IsFacilityOrLaunchSite(landedAt, out isFacility, out displaylandedAt) && gO != null)
			{
				if (ROCManager.Instance != null && gO.CompareTag("ROC"))
				{
					GameObject terrainObj = null;
					if (!string.IsNullOrEmpty(ROCManager.Instance.GetTerrainTag(gO.gameObject, out terrainObj)))
					{
						displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(terrainObj.tag, formatted: false);
					}
				}
				else
				{
					displaylandedAt = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(gO.tag, formatted: false);
				}
			}
			if (displaylandedAt == string.Empty)
			{
				displaylandedAt = ScienceUtil.GetBiomedisplayName(mainBody, GetLandedAtString(landedAt), formatted: false);
			}
		}
	}

	public List<ProtoCrewMember> GetVesselCrew()
	{
		if (loaded)
		{
			if (crewCachedPartCount != parts.Count)
			{
				RebuildCrewList();
			}
			return crew;
		}
		return protoVessel.GetVesselCrew();
	}

	public void CrewListSetDirty()
	{
		crewCachedPartCount = -1;
	}

	public void RebuildCrewList()
	{
		if (loaded)
		{
			crew.Clear();
			crewableParts = 0;
			crewedParts = 0;
			crewCachedPartCount = parts.Count;
			for (int i = 0; i < crewCachedPartCount; i++)
			{
				Part part = parts[i];
				int count = part.protoModuleCrew.Count;
				if (count > 0)
				{
					crewedParts++;
					crewableParts++;
					for (int j = 0; j < count; j++)
					{
						crew.Add(part.protoModuleCrew[j]);
					}
				}
				else if (part.CrewCapacity > 0)
				{
					crewableParts++;
				}
			}
		}
		else
		{
			List<ProtoCrewMember> vesselCrew = protoVessel.GetVesselCrew();
			int count2 = vesselCrew.Count;
			for (int k = 0; k < count2; k++)
			{
				crew.Add(vesselCrew[k]);
			}
			crewCachedPartCount = protoVessel.protoPartSnapshots.Count;
			crewedParts = protoVessel.crewedParts;
			crewableParts = protoVessel.crewableParts;
		}
	}

	public bool RemoveCrew(ProtoCrewMember pcm)
	{
		return crew.Remove(pcm);
	}

	public bool RemoveCrew(string name)
	{
		int count = crew.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(crew[count].name == name));
		crew.RemoveAt(count);
		return true;
	}

	public void RemoveCrewList(List<ProtoCrewMember> pcmList, bool updatePartCount)
	{
		if (updatePartCount)
		{
			crewCachedPartCount = parts.Count;
		}
		int num = 0;
		int count = pcmList.Count;
		int count2 = crew.Count;
		while (count2-- > 0)
		{
			int index = count;
			while (index-- > 0)
			{
				if (crew[count2] == pcmList[index])
				{
					crew.RemoveAt(count2);
					num++;
					break;
				}
			}
			if (num == count)
			{
				break;
			}
		}
	}

	public int GetCrewCapacity()
	{
		int num = 0;
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			num += part.CrewCapacity;
		}
		return num;
	}

	public int GetCrewCount()
	{
		return crew.Count;
	}

	public static void CrewWasModified(Vessel vessel)
	{
		if (vessel != null)
		{
			vessel.RebuildCrewList();
			GameEvents.onVesselCrewWasModified.Fire(vessel);
		}
	}

	public static void CrewWasModified(Vessel vessel1, Vessel vessel2)
	{
		CrewWasModified(vessel1);
		CrewWasModified(vessel2);
	}

	public void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		RebuildCrewList();
	}

	public ControlLevel GetControlLevel()
	{
		if (isEVA && crew.Count > 0)
		{
			if (!crew[0].inactive)
			{
				return ControlLevel.PARTIAL_MANNED;
			}
			return ControlLevel.NONE;
		}
		if (connection != null && CommNetScenario.Instance != null && CommNetScenario.CommNetEnabled)
		{
			return connection.GetControlLevel();
		}
		ControlLevel controlLevel = ControlLevel.NONE;
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			if (part.isControlSource > controlLevel)
			{
				controlLevel = part.isControlSource;
			}
		}
		return controlLevel;
	}

	public void CheckControllable()
	{
		if (!loaded)
		{
			isControllable = false;
			controlLevel = ControlLevel.NONE;
			return;
		}
		controlLevel = GetControlLevel();
		if (maxControlLevel < controlLevel)
		{
			controlLevel = maxControlLevel;
		}
		bool flag = isControllable;
		isControllable = controlLevel > ControlLevel.NONE;
		if (isControllable != flag)
		{
			GameEvents.onVesselControlStateChange.Fire(this, isControllable);
		}
		if (state != State.ACTIVE)
		{
			return;
		}
		switch (controlLevel)
		{
		case ControlLevel.PARTIAL_UNMANNED:
			if (controlLockSet && !controlLockPartial)
			{
				InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
				controlLockSet = false;
			}
			if (!controlLockSet)
			{
				ControlTypes controlTypes = (PartialControlHasSASRCS ? ControlTypes.PARTIAL_SHIP_CONTROLS : ControlTypes.PARTIAL_SHIP_CONTROLS_SASRCS);
				if (!EditableNodes)
				{
					controlTypes = controlTypes | ControlTypes.MANNODE_ADDEDIT | ControlTypes.MANNODE_DELETE;
				}
				InputLockManager.SetControlLock(controlTypes, "vessel_noControl_" + id.ToString());
				controlLockSet = true;
				controlLockPartial = true;
			}
			return;
		case ControlLevel.PARTIAL_MANNED:
			if (controlLockSet && !controlLockPartial)
			{
				InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
				controlLockSet = false;
			}
			if (!controlLockSet && !EditableNodes)
			{
				InputLockManager.SetControlLock(ControlTypes.MANNODE_ADDEDIT | ControlTypes.MANNODE_DELETE, "vessel_noControl_" + id.ToString());
				controlLockSet = true;
				controlLockPartial = true;
			}
			return;
		case ControlLevel.FULL:
			if (controlLockSet)
			{
				InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
				controlLockSet = false;
				controlLockPartial = false;
			}
			return;
		}
		if (controlLockSet && controlLockPartial)
		{
			InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
			controlLockSet = false;
		}
		if (!controlLockSet)
		{
			ControlTypes locks = ControlTypes.ALL_SHIP_CONTROLS;
			if (!EditableNodes)
			{
				locks = ControlTypes.ALL_SHIP_CONTROLS | ControlTypes.MANNODE_ADDEDIT | ControlTypes.MANNODE_DELETE;
			}
			InputLockManager.SetControlLock(locks, "vessel_noControl_" + id.ToString());
			controlLockSet = true;
			controlLockPartial = false;
		}
	}

	public void LoadVesselModules(ConfigNode node)
	{
		if (node == null)
		{
			return;
		}
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			VesselModule vesselModule = vesselModules[i];
			string text = vesselModule.GetType().Name;
			ConfigNode node2 = node.GetNode(text);
			if (node2 != null)
			{
				vesselModule.Load(node2);
			}
		}
	}

	public void UpdateVesselModuleActivation()
	{
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			VesselModule vesselModule = vesselModules[i];
			if (vesselModule.enabled != vesselModule.ShouldBeActive())
			{
				vesselModule.enabled = !vesselModule.enabled;
			}
		}
	}

	public void Awake()
	{
		if (!messageObjectsCached)
		{
			CacheScreenMessageObjects();
		}
		vesselTransform = base.transform;
		SetStaticPressureAndExternalTemp();
		if (parts != null)
		{
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				parts[i].partTransform = parts[i].transform;
			}
		}
		framesAtStartup = Time.frameCount;
		accelSmoothing = new Vector3d[accelSmoothingLength];
		accelSmoothingCursor = 0;
		accelSmoothingLengthRecip = 1.0 / (double)accelSmoothingLength;
		actionGroups = new ActionGroupList(this);
		OverrideDefault = new bool[NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[NumOverrideGroups];
		OverrideGroupNames = new string[NumOverrideGroups];
		ctrlState = new FlightCtrlState();
		setControlStates = new FlightCtrlState[NumOverrideGroups + 1];
		setControlStates[0] = ctrlState;
		for (int j = 1; j < setControlStates.Length; j++)
		{
			setControlStates[j] = new FlightCtrlState(ctrlState.custom_axes);
		}
		vesselValues = new VesselValues(this);
		autopilot = new VesselAutopilot(this);
		discoveryInfo = new DiscoveryInfo(this);
		flightPlanNode = new ConfigNode("FLIGHTPLAN");
		if (PhysicsGlobals.Instance != null)
		{
			vesselRanges = new VesselRanges(PhysicsGlobals.Instance.VesselRangesDefault);
		}
		else
		{
			vesselRanges = new VesselRanges();
		}
		GroundContacts = new List<Part>();
		VesselAnchorExceptions = new List<string>();
		VesselAnchorExceptions.Add("ModuleWheelBase");
		VesselAnchorExceptions.Add("LaunchClamp");
		VesselAnchorExceptions.Add("FlagSite");
		VesselAnchorExceptions.Add("ModuleGroundPart");
		GameEvents.onVesselPrecalcAssign.Fire(this);
		precalc = GetComponent<VesselPrecalculate>() ?? base.gameObject.AddComponent<VesselPrecalculate>();
		GameEvents.onCrewTransferred.Add(onCrewTransferred);
		GameEvents.onVesselPartCountChanged.Add(OnVesselPartCountChanged);
		GameEvents.onPartResourceListChange.Add(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartCrossfeedStateChange.Add(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartFuelLookupStateChange.Add(UpdateCrossfeedResourceSetFL);
		GameEvents.onPartPriorityChanged.Add(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartVesselNamingChanged.Add(OnPartVesselNamingChanged);
		GameEvents.onVesselWasModified.Add(OnVesselNamingVesselWasModified);
		VesselModuleManager.AddModulesToVessel(this, vesselModules);
	}

	public void SetStaticPressureAndExternalTemp()
	{
		CelestialBody celestialBody = FlightGlobals.getMainBody(base.transform.position);
		staticPressurekPa = FlightGlobals.getStaticPressure(base.transform.position, celestialBody);
		if (staticPressurekPa > 0.0)
		{
			externalTemperature = (atmosphericTemperature = celestialBody.GetFullTemperature(base.transform.position));
		}
		else
		{
			externalTemperature = (atmosphericTemperature = PhysicsGlobals.SpaceTemperature);
		}
	}

	public void Start()
	{
		GameEvents.OnCollisionIgnoreUpdate.Fire();
		if (orbitDriver != null && orbit != null)
		{
			lastBody = orbit.referenceBody;
		}
		RebuildCrewList();
		UpdateVesselModuleActivation();
		if (!loaded)
		{
			SetStaticPressureAndExternalTemp();
		}
	}

	public static void CacheScreenMessageObjects()
	{
		gMessage = new ScreenMessage(Localizer.Format("#autoLOC_6002420", "orange"), 1f, ScreenMessageStyle.UPPER_CENTER);
		presMessage = new ScreenMessage(Localizer.Format("#autoLOC_6002421", "orange"), 1f, ScreenMessageStyle.UPPER_CENTER);
		messageObjectsCached = true;
	}

	public void AddPhysicsHoldLock()
	{
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.flag_68 | ControlTypes.PAUSE | ControlTypes.CAMERACONTROLS | ControlTypes.QUICKLOAD | ControlTypes.flag_53), "physicsHold");
		physicsHoldLock = true;
	}

	public void RemovePhysicsHoldLock()
	{
		if (physicsHoldLock)
		{
			InputLockManager.RemoveControlLock("physicsHold");
			physicsHoldLock = false;
		}
	}

	public bool PhysicsHoldExpired()
	{
		if (useFramesAtStartupOverride)
		{
			useFramesAtStartupOverride = Time.frameCount - framesAtStartupOverride < hold_count_override;
			return !useFramesAtStartupOverride;
		}
		int num = (Landed ? 75 : 25);
		return Time.frameCount - framesAtStartup >= num;
	}

	public void SetPhysicsHoldExpiryOverride(int newHoldCountValue = 5)
	{
		framesAtStartup = 0;
		framesAtStartupOverride = Time.frameCount;
		hold_count_override = newHoldCountValue;
		useFramesAtStartupOverride = true;
	}

	public bool Initialize(bool fromShipAssembly = false)
	{
		return Initialize(fromShipAssembly, preCreate: false, orbiting: false, setActiveVessel: true);
	}

	public bool Initialize(bool fromShipAssembly, bool preCreate, bool orbiting, bool setActiveVessel)
	{
		updateResourcesOnEvent = false;
		rootPart = GetComponent<Part>();
		evaController = GetComponent<KerbalEVA>();
		rootOrgPos = rootPart.orgPos;
		rootOrgRot = rootPart.orgRot;
		SetLandedAt("");
		if (persistentId == 0)
		{
			persistentId = FlightGlobals.GetUniquepersistentId();
		}
		parts = new List<Part>();
		findVesselParts(rootPart, !fromShipAssembly);
		if (!preCreate)
		{
			GameEvents.onVesselPartCountChanged.Fire(this);
		}
		int count = parts.Count;
		if (!preCreate)
		{
			for (int i = 0; i < count; i++)
			{
				parts[i].InitializeModules();
			}
		}
		if (vesselTransform == null)
		{
			Debug.LogWarning("[Vessel]: " + base.name + " failed to initialize! Transform is null!", base.gameObject);
			return false;
		}
		orbitDriver = GetComponent<OrbitDriver>() ?? base.gameObject.AddComponent<OrbitDriver>();
		if (orbit != null && orbiting)
		{
			orbitDriver.referenceBody = orbit.referenceBody;
		}
		else
		{
			orbitDriver.referenceBody = FlightGlobals.getMainBody(vesselTransform.position);
		}
		if (orbitDriver.referenceBody.pqsController != null)
		{
			PQSminLevel = orbitDriver.referenceBody.pqsController.minLevel;
			PQSmaxLevel = orbitDriver.referenceBody.pqsController.maxLevel;
		}
		vesselType = FindDefaultVesselType();
		if (vesselType > VesselType.Debris)
		{
			for (int j = 0; j < count; j++)
			{
				parts[j].isAttached = true;
			}
		}
		orbitDriver.updateMode = (fromShipAssembly ? OrbitDriver.UpdateMode.IDLE : OrbitDriver.UpdateMode.TRACK_Phys);
		AddOrbitRenderer();
		if (!preCreate)
		{
			FlightGlobals.AddVessel(this);
		}
		launchTime = Planetarium.GetUniversalTime();
		missionTime = 0.0;
		SetLoaded(loaded: true);
		int k = 0;
		for (int count2 = vesselModules.Count; k < count2; k++)
		{
			vesselModules[k].OnLoadVessel();
		}
		persistent = isPersistent;
		MakeInactive();
		if (fromShipAssembly)
		{
			mainBody.GetLatLonAlt(vesselTransform.position, out latitude, out longitude, out altitude);
			srfRelRotation = Quaternion.Inverse(mainBody.bodyTransform.rotation) * vesselTransform.rotation;
			upAxis = FlightGlobals.getUpAxis(mainBody, vesselTransform.position);
			heightFromTerrain = Vector3.Dot(vesselTransform.position, upAxis);
			terrainNormal = vesselTransform.InverseTransformDirection(upAxis);
			lastSituation = Situations.PRELAUNCH;
			situation = Situations.PRELAUNCH;
			packed = true;
			if (!preCreate)
			{
				MonoBehaviour.print("[" + GetDisplayName() + "]: Ready to Launch - waiting to start physics...");
				if (setActiveVessel)
				{
					AddPhysicsHoldLock();
				}
				GameEvents.onVesselCreate.Fire(this);
			}
		}
		else
		{
			packed = false;
			lastUT = Planetarium.GetUniversalTime();
			checkLanded();
			checkSplashed();
			FallBackReferenceTransform();
			updateSituation();
			if (!preCreate)
			{
				GameEvents.onVesselCreate.Fire(this);
				GameEvents.onVesselWasModified.Fire(this);
			}
			RebuildCrewList();
		}
		protoVessel = new ProtoVessel(this, preCreate);
		updateResourcesOnEvent = true;
		if (!preCreate)
		{
			UpdateResourceSets();
			VesselDeltaV = VesselDeltaV.Create(this);
		}
		if (!fromShipAssembly)
		{
			precalc.RunFirst();
		}
		return true;
	}

	public void findVesselParts(Part part, bool checkAttached)
	{
		parts.Add(part);
		part.vessel = this;
		part.orgPos = Quaternion.Inverse(rootOrgRot) * (part.orgPos - rootOrgPos);
		part.orgRot = Quaternion.Inverse(rootOrgRot) * part.orgRot;
		int count = part.children.Count;
		for (int i = 0; i < count; i++)
		{
			Part part2 = part.children[i];
			if (!checkAttached || (part2.isAttached && !(part2.GetComponent<Vessel>() != null)))
			{
				findVesselParts(part2, checkAttached);
			}
		}
	}

	public void StartFromBackup(ProtoVessel pv)
	{
		StartFromBackup(pv, null);
	}

	public void StartFromBackup(ProtoVessel pv, FlightState st)
	{
		parts = new List<Part>();
		protoVessel = pv;
		if (st != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && pv.situation == Situations.PRELAUNCH)
		{
			orbitDriver = GetComponent<OrbitDriver>() ?? base.gameObject.AddComponent<OrbitDriver>();
			orbitDriver.referenceBody = FlightGlobals.getMainBody(vesselTransform.position);
			orbitDriver.updateMode = OrbitDriver.UpdateMode.IDLE;
		}
		AddOrbitRenderer();
		VesselDeltaV = VesselDeltaV.Create(this);
	}

	[ContextMenu("Load")]
	public void Load()
	{
		if (loaded)
		{
			return;
		}
		updateResourcesOnEvent = false;
		if (!packed)
		{
			GoOnRails();
		}
		protoVessel.LoadObjects();
		base.name = base.name + " (" + GetDisplayName() + ")";
		SetLoaded(loaded: true);
		RebuildCrewList();
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			vesselModules[i].OnLoadVessel();
		}
		if (Landed)
		{
			Debug.Log("[" + GetDisplayName() + "]: landed - waiting for ground contact to resume physics...", base.gameObject);
			altitude = getCorrectedLandedAltitude(latitude, longitude, altitude, mainBody);
			if (isActiveVessel)
			{
				AddPhysicsHoldLock();
			}
		}
		if (referenceTransformId != 0)
		{
			SetReferenceTransform(FindReferenceTransform(referenceTransformId));
		}
		staticPressurekPa = FlightGlobals.getStaticPressure(base.transform.position, mainBody);
		updateSituation();
		SendMessage("OnVesselLoad", SendMessageOptions.DontRequireReceiver);
		GameEvents.onVesselLoaded.Fire(this);
		updateResourcesOnEvent = true;
		UpdateResourceSets();
		if (targetObject == null)
		{
			ITargetable targetable = protoVessel.targetInfo.FindTarget();
			if (targetable != null)
			{
				pTI = new ProtoTargetInfo(protoVessel.targetInfo);
				targetObject = targetable;
			}
		}
		if (navigationWaypoint == null)
		{
			pWPI = protoVessel.waypointInfo;
		}
	}

	[ContextMenu("Unload")]
	public void Unload()
	{
		if (!loaded)
		{
			return;
		}
		isUnloading = true;
		Debug.Log(GetDisplayName() + " Unloaded");
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			vesselModules[i].OnUnloadVessel();
		}
		protoVessel = new ProtoVessel(this);
		int count2 = parts.Count;
		for (int j = 0; j < count2; j++)
		{
			Part part = parts[j];
			if (part != rootPart)
			{
				UnityEngine.Object.Destroy(part.gameObject);
				continue;
			}
			int count3 = part.Modules.Count;
			for (int k = 0; k < count3; k++)
			{
				UnityEngine.Object.Destroy(part.Modules[k]);
			}
		}
		if (rootPart != null)
		{
			rootPart.OnJustAboutToBeDestroyed();
		}
		if (this == FlightGlobals.ActiveVessel)
		{
			OnJustAboutToBeDestroyed();
		}
		UnityEngine.Component component = null;
		component = GetComponent<KSPWheelController>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<KerbalEVA>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<CollisionEnhancer>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<PartBuoyancy>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		Joint[] components = GetComponents<Joint>();
		int num = components.Length;
		while (num-- > 0)
		{
			UnityEngine.Object.Destroy(components[num]);
		}
		component = GetComponent<PartJoint>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<Rigidbody>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<AudioSource>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		UnityEngine.Object.Destroy(rootPart);
		Transform[] componentsInChildren = base.transform.GetComponentsInChildren<Transform>();
		int num2 = componentsInChildren.Length;
		while (num2-- > 0)
		{
			if (componentsInChildren[num2] != base.transform)
			{
				UnityEngine.Object.Destroy(componentsInChildren[num2].gameObject);
			}
		}
		parts.Clear();
		if (!packed)
		{
			GoOnRails();
		}
		SetLoaded(loaded: false);
		base.name = GetDisplayName() + " (Unloaded)";
		isUnloading = false;
		SendMessage("OnVesselUnload", SendMessageOptions.DontRequireReceiver);
		GameEvents.onVesselUnloaded.Fire(this);
	}

	public ProtoVessel BackupVessel()
	{
		isBackingUp = true;
		if (loaded)
		{
			protoVessel = new ProtoVessel(this);
		}
		else
		{
			protoVessel.vesselName = vesselName;
			protoVessel.vesselType = vesselType;
			protoVessel.orbitSnapShot = new OrbitSnapshot(orbit);
			protoVessel.discoveryInfo = new ConfigNode("DISCOVERY");
			discoveryInfo.Save(protoVessel.discoveryInfo);
			if (PatchedConicsAttached)
			{
				protoVessel.flightPlan = new ConfigNode("FLIGHTPLAN");
				patchedConicSolver.Save(protoVessel.flightPlan);
			}
			protoVessel.lastUT = lastUT;
		}
		isBackingUp = false;
		return protoVessel;
	}

	public void SetGroupOverride(int newGroup)
	{
		if (newGroup != GroupOverride)
		{
			KSPAxisGroup kSPAxisGroup = ((GroupOverride == 0) ? (~BlockedMask(newGroup)) : ((newGroup != 0) ? (~BlockedMask(newGroup)) : KSPAxisGroup.None));
			if (kSPAxisGroup != 0)
			{
				setControlStates[newGroup].MaskedCopyFrom(setControlStates[0], kSPAxisGroup);
			}
			GroupOverride = newGroup;
			GameEvents.OnVesselOverrideGroupChanged.Fire(this);
		}
	}

	public KSPAxisGroup BlockedMask(int overrideGroup)
	{
		if (overrideGroup == 0)
		{
			return KSPAxisGroup.None;
		}
		return OverrideAxisControl[overrideGroup - 1];
	}

	public void SetControlState(FlightCtrlState state)
	{
		setControlStates[GroupOverride].CopyFrom(state);
		if (GroupOverride > 0)
		{
			KSPAxisGroup kSPAxisGroup = ~BlockedMask(GroupOverride);
			if (kSPAxisGroup != 0)
			{
				setControlStates[0].MaskedCopyFrom(state, kSPAxisGroup);
			}
		}
	}

	public void GetControlState(FlightCtrlState state)
	{
		state.CopyFrom(setControlStates[GroupOverride]);
	}

	public void FeedInputFeed()
	{
		OnPreAutopilotUpdate(ctrlState);
		OnAutopilotUpdate(ctrlState);
		OnPostAutopilotUpdate(ctrlState);
		OnFlyByWire(ctrlState);
		if (loaded && !packed && !physicsHoldLock && isControllable)
		{
			rootPart.propagateControlUpdate(ctrlState);
		}
	}

	public void MakeActive()
	{
		state = State.ACTIVE;
		if (!loaded)
		{
			Load();
		}
		AttachPatchedConicsSolver();
		if (camMode != -1f)
		{
			FlightCamera.SetModeImmediate((FlightCamera.Modes)camMode);
			FlightCamera.CamPitch = campth;
			FlightCamera.CamHdg = camhdg;
		}
		else
		{
			FlightCamera.CamPitch = 0f;
			FlightCamera.CamHdg = 0f;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			part.partTransform = part.transform;
			part.SendMessage("OnVesselActive", SendMessageOptions.DontRequireReceiver);
		}
		SendMessage("OnVesselActive", SendMessageOptions.DontRequireReceiver);
		autopilot.SetupModules();
		SpawnCrew();
		ResumeStaging();
		ResumeTarget();
		ResumeNavigation();
	}

	public void MakeInactive()
	{
		if (controlLockSet)
		{
			InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
			controlLockSet = false;
		}
		if (state != State.DEAD)
		{
			if (state == State.ACTIVE)
			{
				camhdg = FlightCamera.CamHdg;
				campth = FlightCamera.CamPitch;
				camMode = FlightCamera.CamMode;
			}
			state = State.INACTIVE;
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				parts[i].SendMessage("OnVesselInactive", SendMessageOptions.DontRequireReceiver);
			}
			SendMessage("OnVesselInactive", SendMessageOptions.DontRequireReceiver);
			DetachPatchedConicsSolver();
			DespawnCrew();
			if (orbitTargeter != null)
			{
				orbitTargeter.SetTarget(null);
			}
		}
	}

	public void DespawnCrew()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			parts[i].DespawnIVA();
		}
	}

	public void SpawnCrew()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			parts[i].SpawnIVA();
		}
	}

	public void ClearStaging()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			parts[i].stackIcon.RemoveIcon(alertStagingSequencer: false);
		}
		GameEvents.onVesselClearStaging.Fire(this);
	}

	public void ResumeStaging()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			part.UpdateStageability(propagate: false, iconUpdate: false);
			if (part.stagingOn)
			{
				part.stackIcon.CreateIcon(alertStagingSequencer: false);
			}
		}
		GameEvents.onVesselResumeStaging.Fire(this);
	}

	public void ResumeTarget()
	{
		ITargetable targetable = protoVessel.targetInfo.FindTarget();
		if (targetObject == null)
		{
			if (targetable != null)
			{
				targetObject = targetable;
				pTI = new ProtoTargetInfo(protoVessel.targetInfo);
			}
		}
		else if (pTI == null)
		{
			pTI = new ProtoTargetInfo(targetObject);
		}
		targetable = targetObject;
		FlightGlobals.fetch.SetVesselTarget(null, overrideInputLock: true);
		StopCoroutine("waitAndResumeTarget");
		if (targetable != null)
		{
			StartCoroutine(waitAndResumeTargetInfo(120, pTI));
		}
	}

	public IEnumerator waitAndResumeTargetInfo(int framesToWait, ProtoTargetInfo protoTarget)
	{
		ITargetable savedTgt2 = protoTarget.FindTarget();
		yield return null;
		int i = framesToWait;
		while (i-- > 0 && (!PhysicsHoldExpired() || savedTgt2 == null || !(savedTgt2.GetOrbitDriver().Renderer != null)))
		{
			yield return null;
		}
		if (savedTgt2 != null && savedTgt2.GetOrbitDriver().Renderer != null && FlightGlobals.ActiveVessel == this)
		{
			savedTgt2 = protoTarget.FindTarget();
			FlightGlobals.fetch.SetVesselTarget(savedTgt2, overrideInputLock: true);
			pTI = null;
		}
	}

	public void ResumeNavigation()
	{
		if (pWPI != null && !(pWPI.navigationId == Guid.Empty))
		{
			StopCoroutine("waitAndResumeNavigationInfo");
			StartCoroutine(waitAndResumeNavigationInfo(120));
		}
	}

	public IEnumerator waitAndResumeNavigationInfo(int framesToWait)
	{
		yield return null;
		int i = framesToWait;
		while (i-- > 0 && (pWPI == null || !ContractSystem.loaded))
		{
			yield return null;
		}
		if (pWPI == null)
		{
			yield break;
		}
		Waypoint waypoint = pWPI.FindWaypoint();
		if (waypoint != null)
		{
			NavWaypoint fetch = NavWaypoint.fetch;
			if (fetch != null)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003247", waypoint.FullName), 2.5f, ScreenMessageStyle.UPPER_CENTER);
				fetch.Setup(waypoint);
				fetch.Activate();
			}
		}
		else
		{
			navigationWaypoint = null;
		}
		pWPI = null;
	}

	public static Vessel GetDominantVessel(Vessel v1, Vessel v2)
	{
		if (v1.vesselType > v2.vesselType)
		{
			return v1;
		}
		if (v1.vesselType < v2.vesselType)
		{
			return v2;
		}
		v1.GetTotalMass();
		v2.GetTotalMass();
		if (Math.Abs(v1.totalMass - v2.totalMass) < 0.009999999776482582)
		{
			if (v1.id.CompareTo(v2.id) > 0)
			{
				return v1;
			}
			return v2;
		}
		if (v1.totalMass > v2.totalMass)
		{
			return v1;
		}
		return v2;
	}

	public void AddOrbitRenderer()
	{
		if (MapView.fetch != null)
		{
			if (mapObject == null)
			{
				mapObject = ScaledMovement.Create(GetDisplayName(), this);
			}
			if (orbitRenderer == null)
			{
				orbitRenderer = GetComponent<OrbitRenderer>() ?? base.gameObject.AddComponent<OrbitRenderer>();
				orbitRenderer.driver = orbitDriver;
				orbitRenderer.vessel = this;
				orbitDriver.Renderer = orbitRenderer;
			}
		}
	}

	public void AttachPatchedConicsSolver()
	{
		if (patchedConicsUnlocked())
		{
			if (!PatchedConicsAttached)
			{
				PatchedConicsAttached = true;
				orbitRenderer.drawIcons = OrbitRendererBase.DrawIcons.const_1;
				orbitRenderer.drawMode = OrbitRendererBase.DrawMode.const_0;
				orbitRenderer.drawNodes = true;
				patchedConicSolver = base.gameObject.AddComponent<PatchedConicSolver>();
				patchedConicRenderer = base.gameObject.AddComponent<PatchedConicRenderer>();
				orbitTargeter = base.gameObject.AddComponent<OrbitTargeter>();
				if (FlightGlobals.ActiveVessel != this)
				{
					patchedConicSolver.DecreasePatchLimit();
				}
				patchedConicSolver.Load(flightPlanNode);
			}
		}
		else if (vesselOrbitsUnlocked())
		{
			orbitRenderer.drawIcons = OrbitRendererBase.DrawIcons.const_1;
			orbitRenderer.drawNodes = false;
		}
		else
		{
			orbitRenderer.drawNodes = false;
		}
	}

	public void DetachPatchedConicsSolver()
	{
		if (PatchedConicsAttached)
		{
			PatchedConicsAttached = false;
			if ((bool)patchedConicSolver)
			{
				flightPlanNode.ClearData();
				patchedConicSolver.Save(flightPlanNode);
			}
			orbitRenderer.drawIcons = OrbitRendererBase.DrawIcons.const_1;
			orbitRenderer.drawMode = OrbitRendererBase.DrawMode.REDRAW_AND_RECALCULATE;
			if ((bool)orbitTargeter)
			{
				UnityEngine.Object.Destroy(orbitTargeter);
			}
			if ((bool)patchedConicRenderer)
			{
				UnityEngine.Object.Destroy(patchedConicRenderer);
			}
			if ((bool)patchedConicSolver)
			{
				UnityEngine.Object.Destroy(patchedConicSolver);
			}
		}
	}

	public bool patchedConicsUnlocked()
	{
		if (ScenarioUpgradeableFacilities.Instance == null)
		{
			return true;
		}
		return GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) == GameVariables.OrbitDisplayMode.PatchedConics;
	}

	public bool vesselOrbitsUnlocked()
	{
		if (ScenarioUpgradeableFacilities.Instance == null)
		{
			return true;
		}
		return GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) >= GameVariables.OrbitDisplayMode.AllOrbits;
	}

	public void GoOnRails()
	{
		if (packed)
		{
			return;
		}
		precalc.GoOnRails();
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			vesselModules[i].OnGoOnRails();
		}
		GameEvents.onVesselGoOnRails.Fire(this);
		ctrlState.Neutralize();
		MonoBehaviour.print("Packing " + GetDisplayName() + " for orbit");
		if (loaded)
		{
			int count2 = parts.Count;
			for (int j = 0; j < count2; j++)
			{
				parts[j].Pack();
			}
			if (Landed)
			{
				GetHeightFromTerrain();
			}
		}
		packed = true;
		if (LandedOrSplashed)
		{
			orbitDriver.SetOrbitMode(OrbitDriver.UpdateMode.IDLE);
		}
		else
		{
			orbitDriver.SetOrbitMode(OrbitDriver.UpdateMode.UPDATE);
		}
		SendMessage("OnVesselPack", SendMessageOptions.DontRequireReceiver);
	}

	public void GoOffRails()
	{
		if (!packed || !loaded || (!PhysicsHoldExpired() && FindPartModuleImplementing<ModuleGroundPart>() == null))
		{
			return;
		}
		if (Landed && !Splashed)
		{
			if (PQSminLevel != 0 && PQSmaxLevel != 0 && situation != Situations.PRELAUNCH)
			{
				if (PQSminLevel == mainBody.pqsController.minLevel && PQSmaxLevel == mainBody.pqsController.maxLevel)
				{
					skipGroundPositioning = true;
				}
				else
				{
					skipGroundPositioning = false;
				}
			}
			if (Parts.Count == 1 && !skipGroundPositioningForDroppedPart)
			{
				skipGroundPositioning = false;
			}
			if ((vesselSpawning || !skipGroundPositioning) && !CheckGroundCollision())
			{
				vesselSpawning = false;
				return;
			}
		}
		vesselSpawning = false;
		skipGroundPositioningForDroppedPart = false;
		RemovePhysicsHoldLock();
		precalc.GoOffRails();
		if (LandedOrSplashed && !FlightGlobals.RefFrameIsRotating)
		{
			return;
		}
		int i = 0;
		for (int count = vesselModules.Count; i < count; i++)
		{
			vesselModules[i].OnGoOffRails();
		}
		MonoBehaviour.print("Unpacking " + GetDisplayName());
		if (this == FlightGlobals.ActiveVessel)
		{
			Krakensbane.AddFrameVelocity(orbit.GetVel() - (orbit.referenceBody.inverseRotation ? orbit.referenceBody.getRFrmVel(vesselTransform.position) : Vector3d.zero) - Krakensbane.GetFrameVelocity());
		}
		packed = false;
		int count2 = parts.Count;
		for (int j = 0; j < count2; j++)
		{
			parts[j].Unpack();
			parts[j].InitializeModules();
			if (parts[j].PermanentGroundContact)
			{
				Landed = true;
			}
		}
		orbitDriver.SetOrbitMode(OrbitDriver.UpdateMode.TRACK_Phys);
		for (int k = 0; k < count2; k++)
		{
			parts[k].ResumeVelocity();
		}
		if (LandedOrSplashed)
		{
			int num = accelSmoothingLength;
			while (num-- > 0)
			{
				accelSmoothing[num].Zero();
			}
		}
		else
		{
			int num2 = accelSmoothingLength;
			while (num2-- > 0)
			{
				acceleration = graviticAcceleration;
			}
		}
		if (CheatOptions.PauseOnVesselUnpack)
		{
			Debug.Break();
		}
		if (FlightGlobals.ActiveVessel == this)
		{
			if (situation == Situations.PRELAUNCH)
			{
				FlightInputHandler.SetLaunchCtrlState();
			}
			else
			{
				FlightInputHandler.ResumeVesselCtrlState(this);
			}
		}
		SendMessage("OnVesselUnpack", SendMessageOptions.DontRequireReceiver);
		GameEvents.onVesselGoOffRails.Fire(this);
	}

	public bool CheckGroundCollision()
	{
		if (skipGroundPositioning)
		{
			return true;
		}
		Vector3 position = vesselTransform.position;
		Vector3d vector3d = position;
		Vector3d vector3d2 = FlightGlobals.getUpAxis(mainBody, vesselTransform.position);
		Vector3d relSurfaceNVector = mainBody.GetRelSurfaceNVector(latitude, longitude);
		GetHeightFromTerrain(overRidePacked: true);
		double num = mainBody.pqsController.GetSurfaceHeight(relSurfaceNVector, overrideQuadBuildCheck: true) - mainBody.Radius - mainBody.GetAltitude(vesselTransform.position);
		float num2 = Mathf.Max(500f, (float)num + 500f);
		Vector3 vector = position + vector3d2 * num2;
		IgnoreGForces(1);
		Translate(vector3d2 * 5000.0);
		if (Physics.Raycast(maxDistance: (float)(vector - mainBody.position).magnitude, origin: vector, direction: -vector3d2, hitInfo: out groundCollisionHit, layerMask: 0x8000 | LayerUtil.DefaultEquivalent, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
		{
			SetPosition(vector3d);
			double num3;
			if (FlightGlobals.GetPartUpwardsCached(groundCollisionHit.collider.transform.gameObject) != null)
			{
				num3 = HeightFromPartOffsetGlobal + heightFromPartOffsetLocal;
				MonoBehaviour.print("[" + GetDisplayName() + "]: ground contact from a Part! Moving upwards " + num3.ToString("0.000") + "m");
			}
			else
			{
				if (heightFromTerrain < 0f)
				{
					heightFromTerrain = 0f;
				}
				num3 = (double)heightFromTerrain - ((double)groundCollisionHit.distance - (double)num2);
				float num4 = Mathf.Abs(getLowestPoint());
				if (!float.IsInfinity(num4))
				{
					float num5 = Mathf.Cos(Mathf.Abs((float)Vector3d.Angle(groundCollisionHit.normal, vector3d2)) * ((float)Math.PI / 180f)) * num4;
					if (Mathf.Abs(num4 - num5) > 0.1f)
					{
						num4 = num5;
					}
					if (heightFromTerrain > num4)
					{
						num4 = heightFromTerrain - num4;
						num3 -= (double)num4;
					}
					else
					{
						num4 -= heightFromTerrain;
						num3 += (double)num4;
						CollisionEnhancer.bypass = true;
						ignoreCollisionsFrames = 60;
					}
				}
				if (groundCollisionHit.transform.GetComponentInParent<PositionMobileLaunchPad>() != null)
				{
					Quaternion rotation = Quaternion.FromToRotation(groundCollisionHit.normal, upAxis) * vesselTransform.rotation;
					SetRotation(rotation);
					CollisionEnhancer.bypass = true;
					ignoreCollisionsFrames = 60;
				}
				else if (Vector3.Angle(vesselTransform.up, groundCollisionHit.normal) > 1f)
				{
					SetRotation(vesselTransform.rotation * Quaternion.FromToRotation(terrainNormal, vesselTransform.InverseTransformDirection(groundCollisionHit.normal)));
					CollisionEnhancer.bypass = true;
					ignoreCollisionsFrames = 60;
				}
			}
			ModuleGroundPart moduleGroundPart = rootPart.FindModuleImplementing<ModuleGroundPart>();
			if (Math.Abs(num3) < 0.10000000149011612 && moduleGroundPart == null)
			{
				SetPosition(vector3d);
			}
			else
			{
				MonoBehaviour.print("[" + GetDisplayName() + "]: ground contact! - error. Moving Vessel " + ((num3 < 0.0) ? " down " : " up ") + num3.ToString("0.000") + "m");
				SetPosition(vector3d + vector3d2 * num3);
			}
			RemovePhysicsHoldLock();
			IgnoreGForces(20 * (int)Mathf.Max(Mathf.RoundToInt((float)num3), 1f));
			return true;
		}
		SetPosition(vector3d);
		return false;
	}

	public float getLowestPoint()
	{
		float num = float.PositiveInfinity;
		Quaternion rotation = vesselTransform.rotation;
		Quaternion rotation2 = Quaternion.FromToRotation(groundCollisionHit.normal, Vector3.forward) * rotation;
		SetRotation(rotation2);
		GameObject gameObject = new GameObject("MeasureObject");
		gameObject.transform.position = new Vector3(vesselTransform.position.x, vesselTransform.position.y, vesselTransform.position.z);
		gameObject.transform.rotation = new Quaternion(vesselTransform.rotation.x, vesselTransform.rotation.y, vesselTransform.rotation.z, vesselTransform.rotation.w);
		gameObject.transform.position += -Vector3.forward * 1000f;
		Collider collider = null;
		for (int i = 0; i < parts.Count; i++)
		{
			ModuleWheelBase component = parts[i].GetComponent<ModuleWheelBase>();
			if (component != null)
			{
				component.onPartUnpack(parts[i]);
			}
			Collider[] componentsInChildren = parts[i].GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				bool flag = true;
				if (componentsInChildren[j].gameObject.layer == 26 || componentsInChildren[j].gameObject.layer == 27 || componentsInChildren[j].gameObject.layer == 30)
				{
					flag = componentsInChildren[j].enabled;
					componentsInChildren[j].enabled = true;
				}
				if (componentsInChildren[j].gameObject.layer != 21 && componentsInChildren[j].gameObject.layer != 16 && componentsInChildren[j].gameObject.layer != 20 && !componentsInChildren[j].isTrigger && componentsInChildren[j].enabled)
				{
					float num2 = 0f;
					num2 = ((componentsInChildren[j].gameObject.layer == 26 || componentsInChildren[j].gameObject.layer == 27 || componentsInChildren[j].gameObject.layer == 30) ? componentsInChildren[j].bounds.min.z : componentsInChildren[j].ClosestPoint(gameObject.transform.position).z);
					if (num2 < num)
					{
						collider = componentsInChildren[j];
						_ = parts[i];
					}
					num = Mathf.Min(num, num2);
				}
				if (componentsInChildren[j].gameObject.layer == 26 || componentsInChildren[j].gameObject.layer == 27 || componentsInChildren[j].gameObject.layer == 30)
				{
					componentsInChildren[j].enabled = flag;
				}
			}
		}
		if (collider != null)
		{
			if (collider.gameObject.name.Contains("WheelCollider"))
			{
				VPWheelCollider component2 = collider.GetComponent<VPWheelCollider>();
				if (component2 != null)
				{
					num -= component2.radius;
				}
				else
				{
					WheelCollider wheelCollider = collider as WheelCollider;
					if (wheelCollider != null)
					{
						num -= wheelCollider.radius;
					}
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
		SetRotation(rotation);
		return result;
	}

	public float GetHeightFromTerrain(bool overRidePacked = false)
	{
		if (loaded && (overRidePacked || !packed))
		{
			heightFromTerrain = -1f;
			Vector3 vector = FlightGlobals.getUpAxis(mainBody, vesselTransform.position);
			terrainNormal = vesselTransform.InverseTransformDirection(vector);
			float num = FlightGlobals.getAltitudeAtPos(vesselTransform.position, mainBody);
			if (num < 0f)
			{
				num = 0f - (float)PQSAltitude();
			}
			num += 600f;
			if (Physics.Raycast(vesselTransform.position, -vector, out heightFromTerrainHit, num, 32768, QueryTriggerInteraction.Ignore))
			{
				heightFromTerrain = heightFromTerrainHit.distance;
				terrainNormal = vesselTransform.InverseTransformDirection(heightFromTerrainHit.normal);
				objectUnderVessel = heightFromTerrainHit.collider.gameObject;
			}
		}
		return heightFromTerrain;
	}

	public float GetHeightFromSurface()
	{
		if (loaded && !packed)
		{
			heightFromSurface = -1f;
			float num = FlightGlobals.getAltitudeAtPos(vesselTransform.position, mainBody);
			if (num < 0f)
			{
				num = 0f;
			}
			num += 1000f;
			if (Physics.Raycast(vesselTransform.position, -FlightGlobals.getUpAxis(mainBody, vesselTransform.position), out heightFromSurfaceHit, num, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000, QueryTriggerInteraction.Ignore))
			{
				heightFromSurface = heightFromSurfaceHit.distance;
			}
		}
		return heightFromSurface;
	}

	public RaycastHit GetHitFromSurface()
	{
		VesselSurface = null;
		GetHeightFromSurface();
		if (heightFromSurfaceHit.distance != 0f)
		{
			Part component = HeightFromSurfaceHit.collider.gameObject.GetComponent<Part>();
			if (component != null)
			{
				VesselSurface = component.vessel;
			}
		}
		return heightFromSurfaceHit;
	}

	public virtual void GetGroundLevelAngle()
	{
		groundLevelAngle = Vector3.Angle(FlightGlobals.getUpAxis(), GetHitFromSurface().normal);
	}

	public double getCorrectedLandedAltitude(double lat, double lon, double alt, CelestialBody body)
	{
		Vector3d relSurfaceNVector = body.GetRelSurfaceNVector(lat, lon);
		double val = body.pqsController.GetSurfaceHeight(relSurfaceNVector, overrideQuadBuildCheck: true) - body.Radius;
		return Math.Max(alt, val);
	}

	public double PQSAltitude()
	{
		if (mainBody.pqsController != null)
		{
			return mainBody.pqsController.GetSurfaceHeight(mainBody.GetRelSurfaceNVector(latitude, longitude)) - mainBody.Radius;
		}
		return -1.0;
	}

	public void FixedUpdate()
	{
		if (state != State.DEAD)
		{
			UpdateCaches();
			if (ignoreCollisionsFrames > 0)
			{
				ignoreCollisionsFrames--;
			}
			if (ignoreSpeedFrames > 0)
			{
				ignoreSpeedFrames--;
			}
		}
	}

	public void CheckKill()
	{
		if (FlightGlobals.ready && packed && this != FlightGlobals.ActiveVessel && !FlightGlobals.overrideOrbit && !LandedOrSplashed && mainBody.GetPressure(altitude) > 1.0)
		{
			bool flag = true;
			if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
			{
				float pack = vesselRanges.GetSituationRanges(BestSituation).pack;
				pack *= pack;
				if ((vesselTransform.position - FlightGlobals.ActiveVessel.transform.position).sqrMagnitude < pack)
				{
					flag = false;
				}
			}
			if (flag)
			{
				Debug.Log("[F: " + Time.frameCount + "]: Vessel " + GetDisplayName() + " was on-rails at " + FlightGlobals.getStaticPressure(vesselTransform.position, mainBody).ToString("0.0") + " kPa pressure and was destroyed.");
				MurderCrew();
				Die();
				return;
			}
		}
		if (!LandedOrSplashed && PhysicsHoldExpired() && altitude < ((terrainAltitude != -1.0) ? terrainAltitude : (loaded ? (-250.0) : (-0.1))))
		{
			Debug.LogWarning("[F: " + Time.frameCount + "]: Vessel " + GetDisplayName() + " crashed through terrain on " + mainBody.bodyName + ".");
			GameEvents.onVesselExplodeGroundCollision.Fire(this);
			if (loaded)
			{
				GameEvents.onCrash.Fire(new EventReport(FlightEvents.CRASH, rootPart, GetDisplayName(), mainBody.displayName));
				float offset = (float)(terrainAltitude - altitude);
				rootPart.explode(offset);
			}
			else
			{
				MurderCrew();
				Die();
			}
		}
	}

	public void UpdatePosVel()
	{
		if (!LandedOrSplashed || !packed)
		{
			if (loaded)
			{
				mainBody.GetLatLonAlt(vesselTransform.position, out latitude, out longitude, out altitude);
			}
			else
			{
				mainBody.GetLatLonAltOrbital(orbit.pos, out latitude, out longitude, out altitude);
			}
			srfRelRotation = Quaternion.Inverse(mainBody.bodyTransform.rotation) * vesselTransform.rotation;
		}
		if (loaded && !packed)
		{
			GetHeightFromTerrain();
		}
		else if (!LandedOrSplashed)
		{
			heightFromTerrain = -1f;
		}
		if (heightFromTerrain != -1f)
		{
			terrainAltitude = altitude - (double)heightFromTerrain;
			pqsAltitude = terrainAltitude;
			if (!(altitude < 0.0) && (double)heightFromTerrain >= altitude)
			{
				radarAltitude = altitude;
			}
			else
			{
				radarAltitude = heightFromTerrain;
			}
		}
		else if (!GameSettings.RADAR_ALTIMETER_EXTENDED_CALCS && (!(altitude < (double)mainBody.inverseRotThresholdAltitude) || altitude >= mainBody.minOrbitalDistance - mainBody.Radius))
		{
			radarAltitude = altitude;
		}
		else
		{
			pqsAltitude = PQSAltitude();
			terrainAltitude = pqsAltitude;
			radarAltitude = altitude - terrainAltitude;
			heightFromTerrain = (float)altitude;
			if (pqsAltitude < 0.0)
			{
				radarAltitude = altitude;
			}
		}
		if (!FlightGlobals.ready)
		{
			return;
		}
		obt_velocity = orbit.GetRelativeVel();
		obt_speed = obt_velocity.magnitude;
		srf_velocity = orbit.GetVel() - mainBody.getRFrmVelOrbit(orbit);
		upAxis = FlightGlobals.getUpAxis(mainBody, CoMD);
		verticalSpeed = Vector3d.Dot(obt_velocity, upAxis);
		double sqrMagnitude = srf_velocity.sqrMagnitude;
		if (sqrMagnitude > 0.0)
		{
			srfSpeed = Math.Sqrt(sqrMagnitude);
			srf_vel_direction = srf_velocity / srfSpeed;
			double num = sqrMagnitude - verticalSpeed * verticalSpeed;
			if (!(num <= 0.0) && !double.IsNaN(num))
			{
				horizontalSrfSpeed = Math.Sqrt(num);
			}
			else
			{
				horizontalSrfSpeed = 0.0;
			}
		}
		else
		{
			srfSpeed = 0.0;
			horizontalSrfSpeed = 0.0;
			srf_vel_direction.Zero();
		}
		up = (CoM - mainBody.position).normalized;
		north = Vector3d.Exclude(up, mainBody.position + mainBody.transform.up * (float)mainBody.Radius - CoM).normalized;
		east = mainBody.getRFrmVel(CoM).normalized;
		UpdateDistanceTraveled();
		initialPosVelSet = true;
	}

	public void UpdateDistanceTraveled()
	{
		double num = 0.0;
		switch (situation)
		{
		default:
			num = obt_speed;
			break;
		case Situations.FLYING:
			num = srfSpeed;
			break;
		case Situations.LANDED:
		case Situations.SPLASHED:
		case Situations.PRELAUNCH:
			return;
		}
		distanceTraveled += num * (double)Time.deltaTime;
	}

	public void UpdateAcceleration(double fdtRecip, bool fromUpdate)
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		bool flag = mainBody.rotates && mainBody.inverseRotation;
		if (loaded && !packed)
		{
			if (ignoreGeeforceFrames == 0)
			{
				if (!(lastBody != orbit.referenceBody) && (!orbit.referenceBody.rotates || frameWasRotating == flag))
				{
					Vessel eVALadderVessel = EVALadderVessel;
					if (eVALadderVessel == this)
					{
						acceleration_immediate = (accelSmoothing[accelSmoothingCursor++] = (obt_velocity - lastVel) * fdtRecip);
						if (accelSmoothingCursor >= accelSmoothing.Length)
						{
							accelSmoothingCursor = 0;
						}
						acceleration.Zero();
						int num = accelSmoothingLength;
						while (num-- > 0)
						{
							acceleration += accelSmoothing[num];
						}
						acceleration *= accelSmoothingLengthRecip;
						perturbation = acceleration - graviticAcceleration;
						perturbation_immediate = acceleration_immediate - graviticAcceleration;
						geeForce = perturbation.magnitude * (1.0 / PhysicsGlobals.GravitationalAcceleration);
						if (state == State.ACTIVE && ctrlState.mainThrottle > 0f)
						{
							specificAcceleration = geeForce * PhysicsGlobals.GravitationalAcceleration / (double)ctrlState.mainThrottle;
						}
						geeForce_immediate = ((obt_velocity - lastVel) * fdtRecip - graviticAcceleration).magnitude * (1.0 / PhysicsGlobals.GravitationalAcceleration);
					}
					else
					{
						int num2 = accelSmoothingLength;
						while (num2-- > 0)
						{
							acceleration = eVALadderVessel.accelSmoothing[num2];
						}
						perturbation = eVALadderVessel.perturbation;
						perturbation_immediate = eVALadderVessel.perturbation_immediate;
						geeForce = eVALadderVessel.geeForce;
						geeForce_immediate = eVALadderVessel.geeForce_immediate;
					}
				}
			}
			else
			{
				ignoreGeeforceFrames--;
			}
		}
		else
		{
			if (LandedOrSplashed)
			{
				perturbation = (perturbation_immediate = graviticAcceleration * (-1.0 / PhysicsGlobals.GravitationalAcceleration));
				geeForce = (geeForce_immediate = perturbation.magnitude);
			}
			else
			{
				geeForce = 0.0;
				geeForce_immediate = 0.0;
				perturbation.Zero();
				perturbation_immediate.Zero();
			}
			acceleration_immediate.Zero();
			acceleration.Zero();
			if (ignoreGeeforceFrames > 0)
			{
				ignoreGeeforceFrames--;
			}
		}
		lastVel = obt_velocity;
		frameWasRotating = flag;
		krakensbaneAcc.Zero();
		lastBody = orbit.referenceBody;
	}

	public void IgnoreGForces(int frames)
	{
		if (ignoreGeeforceFrames < frames)
		{
			ignoreGeeforceFrames = frames;
		}
	}

	public void IgnoreSpeed(int frames)
	{
		if (ignoreSpeedFrames < frames)
		{
			ignoreSpeedFrames = frames;
		}
	}

	public void Update()
	{
		if (state == State.DEAD)
		{
			return;
		}
		if (situation == Situations.PRELAUNCH)
		{
			launchTime = Planetarium.GetUniversalTime();
		}
		missionTime = Planetarium.GetUniversalTime() - launchTime;
		UpdateVesselModuleActivation();
		if (autoClean && !loaded && HighLogic.CurrentGame.CurrenciesAvailable)
		{
			Clean(autoCleanReason);
		}
		else
		{
			if (!FlightGlobals.ready)
			{
				return;
			}
			if (this != FlightGlobals.ActiveVessel)
			{
				if (loaded && Vector3.Distance(vesselTransform.position, FlightGlobals.ActiveVessel.vesselTransform.position) > vesselRanges.GetSituationRanges(situation).unload)
				{
					Unload();
				}
				if (!loaded && Vector3.Distance(vesselTransform.position, FlightGlobals.ActiveVessel.vesselTransform.position) < vesselRanges.GetSituationRanges(situation).load)
				{
					Load();
					if (loaded && vesselType == VesselType.SpaceObject)
					{
						if (comet != null)
						{
							AnalyticsUtil.LogCometVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.reached, HighLogic.CurrentGame, this);
						}
						else
						{
							AnalyticsUtil.LogAsteroidVesselEvent(AnalyticsUtil.SpaceObjectEventTypes.reached, HighLogic.CurrentGame, this);
						}
					}
				}
			}
			else if (mainBody != null && mainBody.pqsController != null)
			{
				double num = mainBody.pqsController.radiusMax - mainBody.pqsController.radius;
				if (FlightGlobals.camera_altitude > num && CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Flight)
				{
					float num2 = Vector3.Dot((base.transform.position - mainBody.bodyTransform.position).normalized, FlightCamera.fetch.mainCamera.transform.forward);
					float num3 = Mathf.Cos(0.5f * FlightCamera.fetch.mainCamera.fieldOfView * FlightCamera.fetch.mainCamera.aspect * ((float)Math.PI / 180f));
					if (num2 > num3)
					{
						FloatingOrigin.ResetTerrainShaderOffset();
					}
				}
			}
			if (IsControllable && !packed)
			{
				autopilot.Update();
			}
		}
	}

	public void LateUpdate()
	{
		if (!FlightGlobals.ready || state == State.DEAD)
		{
			return;
		}
		if (!bouncedOffCheck && Time.timeSinceLevelLoad > 5f)
		{
			bouncedOffCheck = true;
			UpdateLandedSplashed();
		}
		CheckControllable();
		if (Time.timeSinceLevelLoad > 0.5f)
		{
			updateSituation();
		}
		if (loaded)
		{
			if (partMaxGThresh > warningThresholdG && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GPartLimits)
			{
				if (FlightGlobals.ActiveVessel == this)
				{
					ScreenMessages.PostScreenMessage(gMessage);
				}
				gThresh = (float)((partMaxGThresh - warningThresholdG) * (1.0 / (1.0 - warningThresholdG)));
			}
			else
			{
				gThresh = 0f;
			}
			if (partMaxPresThresh > warningThresholdPres && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().PressurePartLimits)
			{
				if (FlightGlobals.ActiveVessel == this)
				{
					ScreenMessages.PostScreenMessage(presMessage);
				}
				presThresh = (float)((partMaxPresThresh - warningThresholdPres) * (1.0 / (1.0 - warningThresholdPres)));
			}
			else
			{
				presThresh = 0f;
			}
			AddRBAnchor();
		}
		double num = 0.0;
		partMaxPresThresh = 0.0;
		partMaxGThresh = num;
	}

	public void UpdateCaches()
	{
		int count = parts.Count;
		if ((loaded && crewCachedPartCount != count) || (!loaded && crewCachedPartCount != protoVessel.protoPartSnapshots.Count))
		{
			RebuildCrewList();
		}
		if (resourcesDirty)
		{
			UpdateResourceSets();
		}
		if (oldPartCount == count)
		{
			return;
		}
		oldPartCount = count;
		dockingPorts.Clear();
		int index = count;
		while (index-- > 0)
		{
			Part part = parts[index];
			if (part.PermanentGroundContact)
			{
				permanentGroundContact = true;
			}
			dockingPorts.AddRange(part.dockingPorts);
		}
	}

	public void updateSituation()
	{
		bool flag;
		if (EVALadderVessel != this)
		{
			situation = evaController.LadderPart.vessel.situation;
			flag = true;
		}
		else
		{
			flag = false;
			if (Landed)
			{
				if (situation == Situations.PRELAUNCH && (!(srfSpeed > 2.5) || precalc.isEasingGravity || vesselSpawning))
				{
					situation = Situations.PRELAUNCH;
				}
				else
				{
					situation = Situations.LANDED;
				}
			}
			else if (Splashed)
			{
				situation = Situations.SPLASHED;
			}
			else if (staticPressurekPa > 0.0)
			{
				situation = Situations.FLYING;
			}
			else if (orbit.eccentricity < 1.0 && orbit.ApR < mainBody.sphereOfInfluence)
			{
				if (orbit.PeA < (mainBody.atmosphere ? mainBody.atmosphereDepth : 0.0))
				{
					situation = Situations.SUB_ORBITAL;
				}
				else
				{
					situation = Situations.ORBITING;
				}
			}
			else
			{
				situation = Situations.ESCAPING;
			}
		}
		if (situation != lastSituation)
		{
			GameEvents.onVesselSituationChange.Fire(new GameEvents.HostedFromToAction<Vessel, Situations>(this, lastSituation, situation));
			lastSituation = situation;
		}
		if (wasLadder != flag)
		{
			wasLadder = flag;
		}
	}

	public void UpdateLandedSplashed()
	{
		checkLanded();
		checkSplashed();
		updateSituation();
	}

	public void ChangeWorldVelocity(Vector3d velOffset)
	{
		if (state == State.DEAD || packed)
		{
			return;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part.State != PartStates.DEAD && !(part.rb == null))
			{
				part.rb.velocity = (Vector3d)part.rb.velocity + velOffset;
				if (part.servoRb != null)
				{
					part.servoRb.velocity = (Vector3d)part.servoRb.velocity + velOffset;
				}
			}
		}
		krakensbaneAcc += velOffset * (1.0 / (double)TimeWarp.fixedDeltaTime);
	}

	public void SetWorldVelocity(Vector3d vel)
	{
		if (state == State.DEAD || packed)
		{
			return;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part.State != PartStates.DEAD && !(part.rb == null))
			{
				part.rb.velocity = vel;
				if (part.servoRb != null)
				{
					part.servoRb.velocity = vel;
				}
			}
		}
	}

	public void Die()
	{
		GameEvents.onVesselWillDestroy.Fire(this);
		if (FlightGlobals.fetch.VesselTarget != null && FlightGlobals.fetch.VesselTarget.GetVessel() == this)
		{
			FlightGlobals.fetch.SetVesselTarget(null);
		}
		SendMessage("OnVesselDie", SendMessageOptions.DontRequireReceiver);
		state = State.DEAD;
		obt_velocity.Zero();
		srf_velocity.Zero();
		rb_velocity.Zero();
		rb_velocityD.Zero();
		verticalSpeed = 0.0;
		horizontalSrfSpeed = 0.0;
		angularVelocity = Vector3.zero;
		acceleration.Zero();
		geeForce = 0.0;
		DestroyVesselComponents();
		if (FlightGlobals.ActiveVessel != this)
		{
			FlightGlobals.RemoveVessel(this);
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				UnityEngine.Object.Destroy(parts[i].gameObject);
			}
			OnJustAboutToBeDestroyed();
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			OnJustAboutToBeDestroyed();
			if (rootPart != null)
			{
				rootPart.OnJustAboutToBeDestroyed();
			}
			MapView.ExitMapView();
			CameraManager.Instance.SetCameraFlight();
			FlightGlobals.fetch.SetVesselTarget(null);
		}
		if (!loaded)
		{
			Debug.Log("[Vessel " + GetDisplayName() + "]: Vessel was destroyed.");
			MurderCrew();
		}
		RemovePhysicsHoldLock();
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER && KSCVesselMarkers.fetch != null)
		{
			KSCVesselMarkers.fetch.RefreshMarkers();
		}
	}

	public void DestroyVesselComponents()
	{
		UnityEngine.Component component = null;
		component = GetComponent<OrbitTargeter>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<PatchedConicRenderer>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<PatchedConicSolver>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<OrbitRenderer>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<OrbitDriver>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = GetComponent<VesselPrecalculate>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		VesselModule[] components = GetComponents<VesselModule>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			if (components[i] != null)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
		if (VesselDeltaV != null)
		{
			UnityEngine.Object.Destroy(VesselDeltaV);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVesselPartCountChanged.Remove(OnVesselPartCountChanged);
		GameEvents.onPartResourceListChange.Remove(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartCrossfeedStateChange.Remove(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartFuelLookupStateChange.Remove(UpdateCrossfeedResourceSetFL);
		GameEvents.onPartPriorityChanged.Remove(UpdateResourceSetsEventCheckPart);
		GameEvents.onPartVesselNamingChanged.Remove(OnPartVesselNamingChanged);
		GameEvents.onVesselWasModified.Remove(OnVesselNamingVesselWasModified);
		GameEvents.onCrewTransferred.Remove(onCrewTransferred);
		if (base.gameObject.name.Contains("Drag Rendering Clone"))
		{
			DestroyVesselComponents();
			return;
		}
		GameEvents.onVesselDestroy.Fire(this);
		if (FlightGlobals.fetch != null && FlightGlobals.Vessels.Contains(this))
		{
			FlightGlobals.RemoveVessel(this);
		}
		RemovePhysicsHoldLock();
		if (controlLockSet)
		{
			InputLockManager.RemoveControlLock("vessel_noControl_" + id.ToString());
			controlLockSet = false;
		}
		if (mapObject != null)
		{
			mapObject.Terminate();
		}
		DestroyVesselComponents();
		autopilot.Destroy();
		autopilot = null;
	}

	public void MurderCrew()
	{
		List<ProtoCrewMember> vesselCrew = GetVesselCrew();
		if (vesselCrew.Count > 0)
		{
			Debug.Log("[Vessel " + GetDisplayName() + "]: " + KSPUtil.PrintCollection(vesselCrew, ", ", (ProtoCrewMember pcm) => pcm.name) + " are now dead.");
			int i = 0;
			for (int count = vesselCrew.Count; i < count; i++)
			{
				vesselCrew[i].Die();
			}
			CrewWasModified(this);
		}
	}

	public void KillPermanentGroundContact()
	{
		if (parts == null)
		{
			return;
		}
		for (int num = parts.Count - 1; num >= 0; num--)
		{
			Part part = parts[num];
			if (part.PermanentGroundContact)
			{
				part.Die();
			}
		}
		Debug.Log("Killed all parts in permanent ground contact on vessel " + GetDisplayName() + ".");
	}

	public void ResetGroundContact()
	{
		if (parts != null)
		{
			for (int num = parts.Count - 1; num >= 0; num--)
			{
				parts[num].GroundContact = false;
				GroundContacts.Remove(parts[num]);
				parts[num].currentCollisions.Clear();
			}
			ResetRBAnchor();
		}
	}

	public float GetTotalMass()
	{
		if (!loaded)
		{
			totalMass = GetUnloadedVesselMass();
		}
		else
		{
			totalMass = 0.0;
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = parts[i];
				totalMass += part.mass + part.GetResourceMass();
			}
		}
		return (float)totalMass;
	}

	public double GetUnloadedVesselMass()
	{
		double num = 0.0;
		for (int i = 0; i < protoVessel.protoPartSnapshots.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = protoVessel.protoPartSnapshots[i];
			num += (double)protoPartSnapshot.mass;
			for (int j = 0; j < protoPartSnapshot.resources.Count; j++)
			{
				ProtoPartResourceSnapshot protoPartResourceSnapshot = protoPartSnapshot.resources[j];
				if (protoPartResourceSnapshot != null && protoPartResourceSnapshot.definition != null)
				{
					num += protoPartResourceSnapshot.amount * (double)protoPartResourceSnapshot.definition.density;
				}
			}
		}
		return num;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = (loaded ? Color.black : XKCDColors.DarkRed);
		Gizmos.DrawWireSphere(vesselTransform.position, 0.5f);
	}

	public static string GetSituationString(Situations situation)
	{
		return situation switch
		{
			Situations.SUB_ORBITAL => cacheAutoLOC_348562, 
			Situations.FLYING => cacheAutoLOC_348558, 
			Situations.LANDED => cacheAutoLOC_348559, 
			Situations.SPLASHED => cacheAutoLOC_348561, 
			Situations.PRELAUNCH => cacheAutoLOC_348557, 
			Situations.DOCKED => cacheAutoLOC_348564, 
			Situations.ESCAPING => cacheAutoLOC_348563, 
			Situations.ORBITING => cacheAutoLOC_348560, 
			_ => cacheAutoLOC_348565, 
		};
	}

	public static string GetSituationString(Vessel v)
	{
		switch (v.situation)
		{
		case Situations.SUB_ORBITAL:
			return cacheAutoLOC_145790;
		case Situations.FLYING:
			return cacheAutoLOC_145801;
		case Situations.LANDED:
			if (v.landedAt == string.Empty)
			{
				return Localizer.Format("#autoLOC_6100012", v.mainBody.displayName);
			}
			if (v.displaylandedAt == string.Empty)
			{
				return Localizer.Format("#autoLOC_6100012", ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(GetLandedAtString(v.landedAt), formatted: true));
			}
			return Localizer.Format("#autoLOC_6100012", Localizer.Format(v.displaylandedAt));
		case Situations.SPLASHED:
			if (v.landedAt == string.Empty)
			{
				return Localizer.Format("#autoLOC_6100016", v.mainBody.displayName);
			}
			if (v.displaylandedAt == string.Empty)
			{
				return Localizer.Format("#autoLOC_6100016", ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(GetLandedAtString(v.landedAt), formatted: true));
			}
			return Localizer.Format("#autoLOC_6100016", Localizer.Format(v.displaylandedAt));
		case Situations.PRELAUNCH:
			return cacheAutoLOC_145785;
		default:
			return cacheAutoLOC_145793;
		case Situations.DOCKED:
			return cacheAutoLOC_145792;
		case Situations.ESCAPING:
			return Localizer.Format("#autoLOC_145791", v.mainBody.displayName);
		case Situations.ORBITING:
			return Localizer.Format("#autoLOC_145788", v.mainBody.displayName);
		}
	}

	public static string GetLandedAtString(string landedAt)
	{
		return MiniBiome.ConvertTagtoLandedAt(landedAt);
	}

	public static string GetMETString(Vessel v)
	{
		return KSPUtil.PrintTime(Math.Abs(v.missionTime), 5, explicitPositive: false);
	}

	public static double GetNextManeuverTime(Vessel v, out bool hasManeuver)
	{
		hasManeuver = true;
		if (v != null && v.patchedConicSolver != null && v.patchedConicSolver.maneuverNodes != null)
		{
			if (v.patchedConicSolver.maneuverNodes.Count > 0)
			{
				return v.patchedConicSolver.maneuverNodes[0].double_0;
			}
			hasManeuver = false;
			return 0.0;
		}
		if (!(v == null) && v.flightPlanNode != null && v.flightPlanNode.CountNodes >= 1)
		{
			ConfigNode node = v.flightPlanNode.GetNode("MANEUVER");
			if (node != null && node.HasValue("UT") && double.TryParse(node.GetValue("UT"), out var result))
			{
				return result;
			}
			hasManeuver = false;
			return 0.0;
		}
		hasManeuver = false;
		return 0.0;
	}

	public static string AutoRename(Vessel v, string baseName)
	{
		int num = int.MaxValue;
		IVesselAutoRename vesselAutoRename = null;
		int count = v.Parts.Count;
		while (count-- > 0)
		{
			int count2 = v.Parts[count].Modules.Count;
			while (count2-- > 0)
			{
				if (v.Parts[count].Modules[count2] is IVesselAutoRename vesselAutoRename2)
				{
					int num2 = (int)vesselAutoRename2.GetVesselType();
					if (num2 < num)
					{
						num = num2;
						vesselAutoRename = vesselAutoRename2;
					}
				}
			}
		}
		if (vesselAutoRename != null)
		{
			return vesselAutoRename.GetVesselName();
		}
		if (v.vesselType > VesselType.Debris)
		{
			return baseName.Contains(v.vesselType.displayDescription()) ? Localizer.Format(baseName) : (Localizer.Format(baseName) + " " + Localizer.Format(v.vesselType.displayDescription()));
		}
		if (baseName == null)
		{
			return Localizer.Format("#autoLOC_900676");
		}
		return baseName.Contains(VesselType.Debris.displayDescription()) ? Localizer.Format(baseName) : Localizer.Format("#autoLOC_7001353", Localizer.Format(baseName) + "^N");
	}

	public void CheckAirstreamShields()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].OnShieldModified(null);
			}
		}
	}

	public void OnSaveFlightState(Dictionary<string, KSPParseable> dataPool)
	{
		if (FlightGlobals.ActiveVessel == this)
		{
			camhdg = FlightCamera.CamHdg;
			campth = FlightCamera.CamPitch;
			camMode = FlightCamera.CamMode;
		}
		dataPool.Add("cPch", new KSPParseable(campth, KSPParseable.Type.FLOAT));
		dataPool.Add("cHdg", new KSPParseable(camhdg, KSPParseable.Type.FLOAT));
		dataPool.Add("cMod", new KSPParseable(camMode, KSPParseable.Type.const_3));
	}

	public void OnLoadFlightState(Dictionary<string, KSPParseable> dataPool)
	{
		if (dataPool.ContainsKey("cPch"))
		{
			campth = dataPool["cPch"].value_float;
		}
		if (dataPool.ContainsKey("cHdg"))
		{
			camhdg = dataPool["cHdg"].value_float;
		}
		if (dataPool.ContainsKey("cMod"))
		{
			camMode = dataPool["cMod"].value_int;
		}
	}

	public ClearToSaveStatus IsClearToSave()
	{
		if (loaded && !packed)
		{
			if (situation == Situations.FLYING)
			{
				return ClearToSaveStatus.NOT_IN_ATMOSPHERE;
			}
			return ClearToSaveStatus.CLEAR;
		}
		return ClearToSaveStatus.CLEAR;
	}

	public bool checkVisibility()
	{
		int count = FlightGlobals.Vessels.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Vessel vessel = FlightGlobals.Vessels[num];
				if (!(vessel == this) && vessel.isCommandable && !((vessel.vesselTransform.position - vesselTransform.position).sqrMagnitude >= 25000000f))
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

	public void SetPosition(Vector3d position)
	{
		SetPosition(position, packed);
	}

	public void SetPosition(Vector3d position, bool usePristineCoords)
	{
		if (!loaded)
		{
			vesselTransform.position = position;
			return;
		}
		if (usePristineCoords)
		{
			QuaternionD quaternionD = vesselTransform.rotation;
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = parts[i];
				part.partTransform.position = position + quaternionD * part.orgPos;
			}
			return;
		}
		Vector3d vector3d = position - (Vector3d)vesselTransform.position;
		int count2 = parts.Count;
		for (int j = 0; j < count2; j++)
		{
			Part part2 = parts[j];
			if (part2.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				part2.partTransform.position = (Vector3d)part2.partTransform.position + vector3d;
			}
		}
	}

	public void Translate(Vector3d offset)
	{
		SetPosition(offset + (Vector3d)vesselTransform.position);
	}

	public void SetRotation(Quaternion rotation)
	{
		SetRotation(rotation, setPos: true);
	}

	public void SetRotation(Quaternion rotation, bool setPos)
	{
		if (!loaded)
		{
			vesselTransform.rotation = rotation;
			return;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			part.partTransform.rotation = rotation * part.orgRot;
		}
		if (setPos)
		{
			SetPosition(vesselTransform.position, usePristineCoords: true);
		}
	}

	public void OffsetVelocity(Vector3d correction)
	{
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = Parts[count];
			if (part.rb != null)
			{
				Vector3d vector3d = (Vector3d)part.rb.velocity + correction;
				part.rb.velocity = vector3d;
			}
			if (part.servoRb != null)
			{
				Vector3d vector3d = (Vector3d)part.servoRb.velocity + correction;
				part.servoRb.velocity = vector3d;
			}
		}
	}

	public Transform GetTransform()
	{
		return ReferenceTransform;
	}

	public Vector3 GetObtVelocity()
	{
		return obt_velocity;
	}

	public Vector3 GetSrfVelocity()
	{
		return srf_velocity;
	}

	public Vector3 GetFwdVector()
	{
		return ReferenceTransform.forward;
	}

	public Vessel GetVessel()
	{
		return this;
	}

	public string GetName()
	{
		return vesselName;
	}

	public string GetDisplayName()
	{
		return Localizer.Format(vesselName);
	}

	public Orbit GetOrbit()
	{
		return orbit;
	}

	public OrbitDriver GetOrbitDriver()
	{
		return orbitDriver;
	}

	public VesselTargetModes GetTargetingMode()
	{
		return VesselTargetModes.DirectionAndVelocity;
	}

	public bool GetActiveTargetable()
	{
		return false;
	}

	public List<Part> GetActiveParts()
	{
		List<Part> list = new List<Part>();
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part.State == PartStates.ACTIVE && currentStage <= part.inverseStage)
			{
				list.Add(part);
			}
		}
		return list;
	}

	public List<T> FindPartModulesImplementing<T>() where T : class
	{
		List<T> list = new List<T>();
		int i = 0;
		for (int count = parts.Count; i < count; i++)
		{
			Part part = parts[i];
			int j = 0;
			for (int count2 = part.Modules.Count; j < count2; j++)
			{
				PartModule partModule = part.Modules[j];
				if (partModule is T)
				{
					list.Add(partModule as T);
				}
			}
		}
		return list;
	}

	public T FindPartModuleImplementing<T>() where T : class
	{
		int i = 0;
		for (int count = parts.Count; i < count; i++)
		{
			Part part = parts[i];
			int j = 0;
			for (int count2 = part.Modules.Count; j < count2; j++)
			{
				PartModule partModule = part.Modules[j];
				if (partModule is T)
				{
					return partModule as T;
				}
			}
		}
		return null;
	}

	public T FindVesselModuleImplementing<T>() where T : class
	{
		int num = 0;
		int count = vesselModules.Count;
		VesselModule vesselModule;
		while (true)
		{
			if (num < count)
			{
				vesselModule = vesselModules[num];
				if (vesselModule is T)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return vesselModule as T;
	}

	public void OnVesselPartCountChanged(Vessel v)
	{
		if (v == this)
		{
			if (updateResourcesOnEvent)
			{
				UpdateResourceSets();
			}
			else
			{
				resourcesDirty = true;
			}
			UpdateVesselSize();
		}
	}

	public void UpdateVesselSize()
	{
		vesselSize = ShipConstruction.CalculateCraftSize(parts, rootPart);
	}

	public void UpdateResourceSets()
	{
		resourcesDirty = false;
		if (resourcePartSet == null)
		{
			resourcePartSet = new PartSet(this);
		}
		else
		{
			resourcePartSet.RebuildVessel(this);
		}
		if (simulationResourcePartSet == null)
		{
			simulationResourcePartSet = new PartSet(this);
		}
		else
		{
			simulationResourcePartSet.RebuildVessel(this);
		}
		BuildCrossfeedPartSets();
	}

	public void UpdateResourceSetsIfDirty()
	{
		if (resourcesDirty)
		{
			UpdateResourceSets();
		}
	}

	public void UpdateCrossfeedResourceSetFL(GameEvents.HostedFromToAction<bool, Part> data)
	{
		if (data.from.vessel == this)
		{
			resourcesDirty = true;
		}
	}

	public void UpdateResourceSetsEventCheckPart(Part p)
	{
		if (p.vessel == this)
		{
			if (updateResourcesOnEvent)
			{
				UpdateResourceSets();
			}
			else
			{
				resourcesDirty = true;
			}
		}
	}

	public void BuildCrossfeedPartSets()
	{
		if (GameSettings.VERBOSE_DEBUG_LOG)
		{
			Debug.Log("[PartSet]: Recreating part sets for vessel " + (string.IsNullOrEmpty(vesselName) ? "[empty name]" : GetDisplayName()));
		}
		PartSet.BuildPartSets(parts, this, PartSet.PartBuildSetOptions.Both);
	}

	public double RequestResource(Part part, int id, double demand, bool usePriority)
	{
		return RequestResource(part, id, demand, usePriority, simulate: false);
	}

	public double RequestResource(Part part, int id, double demand, bool usePriority, bool simulate)
	{
		if (simulate)
		{
			return simulationResourcePartSet.RequestResource(part, id, demand, usePriority, simulate);
		}
		return resourcePartSet.RequestResource(part, id, demand, usePriority, simulate);
	}

	public void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling = true)
	{
		GetConnectedResourceTotals(id, simulate: false, out amount, out maxAmount, pulling);
	}

	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		if (simulate)
		{
			if (simulationResourcePartSet != null)
			{
				simulationResourcePartSet.GetConnectedResourceTotals(id, out amount, out maxAmount, pulling, simulate);
				return;
			}
			amount = 0.0;
			maxAmount = 0.0;
		}
		else if (resourcePartSet != null)
		{
			resourcePartSet.GetConnectedResourceTotals(id, out amount, out maxAmount, pulling, simulate);
		}
		else
		{
			amount = 0.0;
			maxAmount = 0.0;
		}
	}

	public void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		GetConnectedResourceTotals(id, simulate: false, out amount, out maxAmount, threshold, pulling);
	}

	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, double threshold, bool pulling = true)
	{
		if (simulate)
		{
			if (simulationResourcePartSet != null)
			{
				simulationResourcePartSet.GetConnectedResourceTotals(id, out amount, out maxAmount, threshold, pulling, simulate);
				return;
			}
			amount = 0.0;
			maxAmount = 0.0;
		}
		else if (resourcePartSet != null)
		{
			resourcePartSet.GetConnectedResourceTotals(id, out amount, out maxAmount, threshold, pulling, simulate);
		}
		else
		{
			amount = 0.0;
			maxAmount = 0.0;
		}
	}

	public bool ActionControlBlocked(KSPActionGroup actionGroup)
	{
		if (GroupOverride > 0 && GroupOverride <= OverrideActionControl.Length)
		{
			return (OverrideActionControl[GroupOverride - 1] & actionGroup) != 0;
		}
		return false;
	}

	public bool AxisControlBlocked(KSPAxisGroup axisGroup)
	{
		if (GroupOverride > 0 && GroupOverride <= OverrideAxisControl.Length)
		{
			return (OverrideAxisControl[GroupOverride - 1] & axisGroup) != 0;
		}
		return false;
	}

	public void CopyOverrides(Vessel v)
	{
		GroupOverride = v.GroupOverride;
		OverrideDefault = new bool[v.OverrideDefault.Length];
		Array.Copy(v.OverrideDefault, OverrideDefault, OverrideDefault.Length);
		OverrideActionControl = new KSPActionGroup[v.OverrideActionControl.Length];
		Array.Copy(v.OverrideActionControl, OverrideActionControl, OverrideActionControl.Length);
		OverrideAxisControl = new KSPAxisGroup[v.OverrideAxisControl.Length];
		Array.Copy(v.OverrideAxisControl, OverrideAxisControl, OverrideAxisControl.Length);
		OverrideGroupNames = new string[v.OverrideGroupNames.Length];
		Array.Copy(v.OverrideGroupNames, OverrideGroupNames, OverrideGroupNames.Length);
		GameEvents.OnVesselOverrideGroupChanged.Fire(this);
	}

	[ContextMenu("Rename Vessel")]
	public void RenameVessel()
	{
		if (renameDialog != null)
		{
			return;
		}
		InputLockManager.SetControlLock("vesselRenameDialog");
		VesselType lowestType;
		if (loaded)
		{
			lowestType = VesselType.Debris;
			int count = parts.Count;
			while (count-- > 0)
			{
				if (parts[count].vesselType == VesselType.SpaceObject)
				{
					lowestType = VesselType.SpaceObject;
					break;
				}
			}
		}
		else
		{
			lowestType = VesselType.Debris;
			int count2 = protoVessel.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				if (protoVessel.protoPartSnapshots[count2].partInfo.partPrefab.vesselType == VesselType.SpaceObject)
				{
					lowestType = VesselType.SpaceObject;
					break;
				}
			}
		}
		renameDialog = VesselRenameDialog.Spawn(this, onVesselRenameAccept, onVesselRenameDismiss, discoveryInfo.Level == DiscoveryLevels.Owned, lowestType);
	}

	public void onVesselRenameAccept(string newVesselName, VesselType newVesselType)
	{
		if (IsValidVesselName(newVesselName))
		{
			string from = vesselName;
			vesselName = newVesselName;
			vesselType = newVesselType;
			if (vesselNamedBy != null)
			{
				vesselNamedBy.vesselNaming.vesselName = newVesselName;
				vesselNamedBy.vesselNaming.vesselType = newVesselType;
			}
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(this, from, vesselName));
			onVesselRenameDismiss();
		}
	}

	public void onVesselRenameDismiss()
	{
		InputLockManager.RemoveControlLock("vesselRenameDialog");
	}

	public static bool IsValidVesselName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		return true;
	}

	public void OnPartVesselNamingChanged(Part p)
	{
		if (p.vessel == this)
		{
			UpdateVesselNaming();
		}
	}

	public void OnVesselNamingVesselWasModified(Vessel v)
	{
		if (v == this)
		{
			UpdateVesselNaming();
		}
	}

	public bool UpdateVesselNaming(bool noGameEvent = false)
	{
		Part part = VesselNaming.FindPriorityNamePart(this);
		if (part == null)
		{
			vesselNamedBy = null;
			return false;
		}
		RunVesselNamingUpdates(part, noGameEvent);
		return true;
	}

	public void RunVesselNamingUpdates(Part pNewName, bool noGameEvent)
	{
		string from = vesselName;
		bool flag = false;
		if (vesselNamedBy != pNewName)
		{
			vesselName = pNewName.vesselNaming.vesselName;
			vesselType = pNewName.vesselNaming.vesselType;
			vesselNamedBy = pNewName;
			flag = true;
		}
		else if (pNewName.vesselNaming.vesselName != vesselNamedBy.vesselNaming.vesselName)
		{
			vesselName = pNewName.vesselNaming.vesselName;
			vesselType = pNewName.vesselNaming.vesselType;
			flag = true;
		}
		else if (pNewName.vesselNaming.vesselType != vesselNamedBy.vesselNaming.vesselType)
		{
			vesselType = pNewName.vesselNaming.vesselType;
			flag = true;
		}
		if (flag && !noGameEvent)
		{
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(this, from, vesselName));
		}
	}

	public VesselType FindDefaultVesselType()
	{
		VesselType vesselType = this.vesselType;
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part.vesselType > vesselType)
			{
				vesselType = part.vesselType;
			}
		}
		return vesselType;
	}

	public void SetActiveInternalSpace(Part visiblePart)
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part == visiblePart)
			{
				if (part.internalModel != null)
				{
					part.internalModel.SetVisible(visible: true);
				}
			}
			else if (part.internalModel != null)
			{
				part.internalModel.SetVisible(visible: false);
			}
		}
	}

	public void ClearActiveInternalSpace()
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part.internalModel != null)
			{
				part.internalModel.SetVisible(visible: false);
			}
		}
	}

	public void SetActiveInternalSpaces(HashSet<Part> visibleParts)
	{
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (visibleParts.Contains(part))
			{
				if (part.internalModel != null)
				{
					part.internalModel.SetVisible(visible: true);
				}
			}
			else if (part.internalModel != null)
			{
				part.internalModel.SetVisible(visible: false);
			}
		}
	}

	public bool ContainsCollider(Collider c)
	{
		if (!loaded)
		{
			return false;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Collider[] componentsInChildren = parts[i].GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (c == componentsInChildren[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CycleAllAutoStrut()
	{
		if (parts == null || parts.Count < 1)
		{
			return;
		}
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			if (part != null)
			{
				part.CycleAutoStrut();
			}
		}
	}

	public bool HasValidContractObjectives(params string[] objectiveTypes)
	{
		return HasValidContractObjectives(new List<string>(objectiveTypes), copy: false);
	}

	public bool HasValidContractObjectives(List<string> objectiveTypes, bool copy = true)
	{
		if (objectiveTypes != null && objectiveTypes.Count > 0)
		{
			List<string> list = (copy ? new List<string>(objectiveTypes) : objectiveTypes);
			if (loaded)
			{
				int count = Parts.Count;
				while (count-- > 0)
				{
					for (int num = list.Count - 1; num >= 0; num--)
					{
						if (Parts[count].HasValidContractObjective(list[num]))
						{
							list.RemoveAt(num);
							if (list.Count <= 0)
							{
								return true;
							}
						}
					}
				}
			}
			else
			{
				int count2 = protoVessel.protoPartSnapshots.Count;
				while (count2-- > 0)
				{
					Part partPrefab = protoVessel.protoPartSnapshots[count2].partPrefab;
					if (partPrefab == null)
					{
						continue;
					}
					for (int num2 = list.Count - 1; num2 >= 0; num2--)
					{
						if (partPrefab.HasValidContractObjective(list[num2]))
						{
							list.RemoveAt(num2);
							if (list.Count <= 0)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	public bool IsFirstFrame()
	{
		if (this.GetComponentCached(ref _fi) == null)
		{
			return false;
		}
		return _fi.firstFrame;
	}

	public string RevealName()
	{
		return vesselName;
	}

	public string RevealDisplayName()
	{
		return Localizer.Format(vesselName);
	}

	public double RevealSpeed()
	{
		if (!LandedOrSplashed)
		{
			return obt_speed;
		}
		return horizontalSrfSpeed;
	}

	public double RevealAltitude()
	{
		return altitude;
	}

	public string RevealSituationString()
	{
		return GetSituationString(this);
	}

	public string RevealType()
	{
		return vesselType.ToString().PrintSpacedStringFromCamelcase();
	}

	public float RevealMass()
	{
		return GetTotalMass();
	}

	public void SetAutoClean(string reason = "")
	{
		if (!autoClean)
		{
			autoClean = true;
			autoCleanReason = reason;
		}
	}

	public void Clean(string reason = "")
	{
		if (protoVessel != null)
		{
			protoVessel.Clean(reason);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002161 = Localizer.Format(Situations.LANDED.Description());
		cacheAutoLOC_6002162 = Localizer.Format(Situations.SPLASHED.Description());
		cacheAutoLOC_6002163 = Localizer.Format(Situations.PRELAUNCH.Description());
		cacheAutoLOC_6002164 = Localizer.Format(Situations.FLYING.Description());
		cacheAutoLOC_6002165 = Localizer.Format(Situations.SUB_ORBITAL.Description());
		cacheAutoLOC_6002166 = Localizer.Format(Situations.ORBITING.Description());
		cacheAutoLOC_6002167 = Localizer.Format(Situations.ESCAPING.Description());
		cacheAutoLOC_6002168 = Localizer.Format(Situations.DOCKED.Description());
		cacheAutoLOC_6002169 = Localizer.Format("#autoLoc_6002169");
		cacheAutoLOC_348557 = Localizer.Format("#autoLOC_348557");
		cacheAutoLOC_348558 = Localizer.Format("#autoLOC_348558");
		cacheAutoLOC_348559 = Localizer.Format("#autoLOC_348559");
		cacheAutoLOC_348560 = Localizer.Format("#autoLOC_348560");
		cacheAutoLOC_348561 = Localizer.Format("#autoLOC_348561");
		cacheAutoLOC_348562 = Localizer.Format("#autoLOC_348562");
		cacheAutoLOC_348563 = Localizer.Format("#autoLOC_348563");
		cacheAutoLOC_348564 = Localizer.Format("#autoLOC_348564");
		cacheAutoLOC_348565 = Localizer.Format("#autoLOC_348565");
		cacheAutoLOC_145785 = Localizer.Format("#autoLOC_145785");
		cacheAutoLOC_145801 = Localizer.Format("#autoLOC_145801");
		cacheAutoLOC_145790 = Localizer.Format("#autoLOC_145790");
		cacheAutoLOC_145792 = Localizer.Format("#autoLOC_145792");
		cacheAutoLOC_145793 = Localizer.Format("#autoLOC_145793");
	}

	public bool HasMakingHistoryParts()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].partInfo != null && parts[num].partInfo.partUrl.IndexOf("SquadExpansion/MakingHistory") > -1)
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

	public bool HasSerenityParts()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].partInfo != null && parts[num].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
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

	public bool HasSerenityRoboticParts()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].isRobotic() && parts[num].partInfo != null && parts[num].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
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

	public bool HasSerenityRoboticController()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].isRoboticController(out var _) && parts[num].partInfo != null && parts[num].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
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

	public int CountSerenityRoboticParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRobotic() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityModRoboticParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRobotic() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") < 0)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityRotorParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticRotor() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityHingeParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticHinge() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityPistonParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticPiston() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityRotationServoParts()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticRotationServo() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int CountSerenityRoboticController()
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticController() && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				num++;
			}
		}
		return num;
	}

	public bool CountSerenityControllerFields(out int countControllers, out int countAxes, out int countActions)
	{
		countControllers = 0;
		countAxes = 0;
		countActions = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].isRoboticController(out var controller) && parts[i].partInfo != null && parts[i].partInfo.partUrl.IndexOf("SquadExpansion/Serenity") > -1)
			{
				countControllers++;
				if (controller.ControlledAxes != null)
				{
					countAxes += controller.ControlledAxes.Count;
				}
				if (controller.ControlledActions != null)
				{
					countActions += controller.ControlledActions.Count;
				}
			}
		}
		return countControllers > 0;
	}

	public bool HasModParts()
	{
		int num = 0;
		while (true)
		{
			if (num < parts.Count)
			{
				if (parts[num].partInfo != null && parts[num].partInfo.partUrl.IndexOf("SquadExpansion") < 0 && parts[num].partInfo.partUrl.IndexOf("Squad") < 0)
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
