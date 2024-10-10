using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ns9;
using UniLinq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlightGlobals : MonoBehaviour
{
	public enum SpeedDisplayModes
	{
		[Description("#autoLOC_7001217")]
		Orbit,
		[Description("#autoLOC_7001218")]
		Surface,
		[Description("#autoLOC_7001219")]
		Target
	}

	public static bool ready;

	public List<CelestialBody> bodies = new List<CelestialBody>();

	public static FlightGlobals _fetch;

	public float srfAttachStiffNess = 1f;

	public float stackAttachStiffNess = 1f;

	public Vessel activeVessel;

	public List<Vessel> vessels = new List<Vessel>();

	public List<Vessel> vesselsLoaded = new List<Vessel>();

	public List<Vessel> vesselsUnloaded = new List<Vessel>();

	public DictionaryValueList<uint, Vessel> persistentVesselIds = new DictionaryValueList<uint, Vessel>();

	public DictionaryValueList<uint, Part> persistentLoadedPartIds = new DictionaryValueList<uint, Part>();

	public DictionaryValueList<uint, ProtoPartSnapshot> persistentUnloadedPartIds = new DictionaryValueList<uint, ProtoPartSnapshot>();

	public static Dictionary<GameObject, Part> objectToPartUpwardsCache = new Dictionary<GameObject, Part>();

	public static Dictionary<GameObject, PartPointer> objectToPartPointerUpwardsCache = new Dictionary<GameObject, PartPointer>();

	public bool setPositionInProgress;

	public uint setPositionInProgressVesselID;

	public static double ship_temp;

	public static double ship_dns;

	public static Quaternion ship_orientation;

	public static Quaternion ship_orientation_offset;

	public static float ship_heading;

	public static double camera_altitude;

	public static Vector3d camera_position;

	public static CelestialBody currentMainBody;

	public Camera mainCameraRef;

	public double lastAltitude;

	public double lastVS;

	public Vector3d lastVel;

	public Vector3d lastCoM;

	public ScreenMessage vesselSwitchFailMessage;

	public ITargetable _vesselTarget;

	public ScreenMessage targetSelectMessage;

	public Transform vesselTargetTransform;

	[HideInInspector]
	public Vector3 vesselTargetDelta;

	[HideInInspector]
	public Vector3 vesselTargetDirection;

	public static Vector3d ship_tgtVelocity;

	public static double ship_tgtSpeed;

	public static List<physicalObject> physicalObjects = new List<physicalObject>();

	public static Quaternion rotationOffset = Quaternion.Euler(90f, 0f, 0f);

	public static bool overrideOrbit = false;

	public static bool warpDriveActive = false;

	public static double tempDouble;

	public static FoRModes FoRMode = FoRModes.SRF_NORTH;

	public Vector3 targetDirection;

	public Vector3 endDirection;

	public Vector3 FoRupAxis;

	public Dictionary<string, CelestialBody> bodyNames = new Dictionary<string, CelestialBody>();

	public CelestialBody homeBody;

	public int homeBodyIndex;

	public CelestialBody cometPerturber;

	public int cometPerturberIndex;

	public static float TargetSwitchSqrThresh = 3.6E+09f;

	public SpeedDisplayModes commandedSpeedMode = SpeedDisplayModes.Surface;

	public static SpeedDisplayModes speedMode = SpeedDisplayModes.Surface;

	public static List<CelestialBody> Bodies => fetch.bodies;

	public static FlightGlobals fetch
	{
		get
		{
			if (_fetch == null)
			{
				_fetch = (FlightGlobals)UnityEngine.Object.FindObjectOfType(typeof(FlightGlobals));
			}
			return _fetch;
		}
	}

	public static float vacuumTemperature => 0f;

	public static float SrfAttachStiffNess => fetch.srfAttachStiffNess;

	public static float StackAttachStiffNess => fetch.stackAttachStiffNess;

	public static Part activeTarget => fetch.activeVessel.rootPart;

	public static Vessel ActiveVessel => fetch.activeVessel;

	public static bool PartPlacementMode
	{
		get
		{
			if (ActiveVessel != null && ActiveVessel.isEVA && ActiveVessel.evaController != null && ActiveVessel.evaController.PartPlacementMode)
			{
				return true;
			}
			if (UIPartActionController.Instance != null)
			{
				return UIPartActionController.Instance.InventoryAndCargoPartExist();
			}
			return false;
		}
	}

	public static List<Vessel> Vessels => fetch.vessels;

	public static List<Vessel> VesselsLoaded => fetch.vesselsLoaded;

	public static List<Vessel> VesselsUnloaded => fetch.vesselsUnloaded;

	public static DictionaryValueList<uint, Vessel> PersistentVesselIds => fetch.persistentVesselIds;

	public static DictionaryValueList<uint, Part> PersistentLoadedPartIds => fetch.persistentLoadedPartIds;

	public static DictionaryValueList<uint, ProtoPartSnapshot> PersistentUnloadedPartIds => fetch.persistentUnloadedPartIds;

	public static double ship_altitude => ActiveVessel.altitude;

	public static double ship_verticalSpeed => ActiveVessel.verticalSpeed;

	public static double ship_geeForce => ActiveVessel.geeForce;

	public static Vector3d ship_velocity => ActiveVessel.rb_velocity;

	public static Vector3d ship_obtVelocity => ActiveVessel.obt_velocity;

	public static double ship_obtSpeed => ActiveVessel.obt_speed;

	public static Vector3d ship_srfVelocity => ActiveVessel.srf_velocity;

	public static double ship_srfSpeed => ActiveVessel.srfSpeed;

	public static Vector3d ship_acceleration => ActiveVessel.acceleration;

	public static Vector3d ship_angularVelocity => ActiveVessel.angularVelocity;

	public static Vector3d ship_position => ActiveVessel.vesselTransform.position;

	public static Vector3d ship_CoM => ActiveVessel.CoM;

	public static Vector3d ship_upAxis => ActiveVessel.upAxis;

	public static Vector3 ship_MOI => ActiveVessel.vector3_0;

	public static Vector3 ship_angularMomentum => ActiveVessel.angularMomentum;

	public static Quaternion ship_rotation => ActiveVessel.ReferenceTransform.rotation;

	public static Orbit ship_orbit => ActiveVessel.orbit;

	public static double ship_latitude => ActiveVessel.latitude;

	public static double ship_longitude => ActiveVessel.longitude;

	public static double ship_stP => ActiveVessel.staticPressurekPa;

	public static bool RefFrameIsRotating
	{
		get
		{
			if (currentMainBody.rotates)
			{
				return currentMainBody.inverseRotation;
			}
			return false;
		}
	}

	public ITargetable VesselTarget => _vesselTarget;

	public VesselTargetModes vesselTargetMode { get; set; }

	public static Vector3d upAxis => getUpAxis(ActiveVessel.mainBody, ActiveVessel.transform.position);

	public static SpeedDisplayModes speedDisplayMode => speedMode;

	public static Part GetPartUpwardsCached(GameObject go)
	{
		if (!objectToPartUpwardsCache.ContainsKey(go))
		{
			objectToPartUpwardsCache[go] = go.GetComponentUpwards<Part>();
		}
		return objectToPartUpwardsCache[go];
	}

	public static PartPointer GetPartPointerUpwardsCached(GameObject go)
	{
		if (!objectToPartPointerUpwardsCache.ContainsKey(go))
		{
			objectToPartPointerUpwardsCache[go] = go.GetComponentUpwards<PartPointer>();
		}
		return objectToPartPointerUpwardsCache[go];
	}

	public static void ResetObjectPartUpwardsCache()
	{
		objectToPartUpwardsCache.Clear();
	}

	public static void ResetObjectPartPointerUpwardsCache()
	{
		objectToPartPointerUpwardsCache.Clear();
	}

	public static void AddVessel(Vessel vessel)
	{
		if (!fetch)
		{
			return;
		}
		fetch.vessels.Add(vessel);
		if (vessel.loaded)
		{
			fetch.vesselsLoaded.Add(vessel);
		}
		else
		{
			fetch.vesselsUnloaded.Add(vessel);
		}
		PersistentVesselIds.Add(vessel.persistentId, vessel);
		if (vessel.loaded)
		{
			for (int i = 0; i < vessel.parts.Count; i++)
			{
				PersistentLoadedPartIds.Add(vessel.parts[i].persistentId, vessel.parts[i]);
			}
		}
		else if (vessel.protoVessel != null && vessel.protoVessel.protoPartSnapshots != null)
		{
			for (int j = 0; j < vessel.protoVessel.protoPartSnapshots.Count; j++)
			{
				PersistentUnloadedPartIds.Add(vessel.protoVessel.protoPartSnapshots[j].persistentId, vessel.protoVessel.protoPartSnapshots[j]);
			}
		}
		GameEvents.onFlightGlobalsAddVessel.Fire(vessel);
	}

	public static void RemoveVessel(Vessel vessel)
	{
		if (!fetch)
		{
			return;
		}
		fetch.vessels.Remove(vessel);
		if (vessel.loaded)
		{
			fetch.vesselsLoaded.Remove(vessel);
		}
		else
		{
			fetch.vesselsUnloaded.Remove(vessel);
		}
		PersistentVesselIds.Remove(vessel.persistentId);
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			PersistentLoadedPartIds.Remove(vessel.parts[i].persistentId);
		}
		if (vessel.protoVessel != null && vessel.protoVessel.protoPartSnapshots != null)
		{
			for (int j = 0; j < vessel.protoVessel.protoPartSnapshots.Count; j++)
			{
				PersistentUnloadedPartIds.Remove(vessel.protoVessel.protoPartSnapshots[j].persistentId);
			}
		}
		GameEvents.onFlightGlobalsRemoveVessel.Fire(vessel);
	}

	public static void SortVessel(Vessel vessel, bool loaded)
	{
		if (!fetch)
		{
			return;
		}
		if (loaded)
		{
			if (fetch.vesselsUnloaded.Contains(vessel))
			{
				fetch.vesselsUnloaded.Remove(vessel);
				fetch.vesselsLoaded.Add(vessel);
				PersistentVesselIds.Add(vessel.persistentId, vessel);
				for (int i = 0; i < vessel.parts.Count; i++)
				{
					PersistentLoadedPartIds.Add(vessel.parts[i].persistentId, vessel.parts[i]);
				}
			}
		}
		else if (fetch.vesselsLoaded.Contains(vessel))
		{
			fetch.vesselsLoaded.Remove(vessel);
			fetch.vesselsUnloaded.Add(vessel);
			for (int j = 0; j < vessel.protoVessel.protoPartSnapshots.Count; j++)
			{
				PersistentLoadedPartIds.Remove(vessel.protoVessel.protoPartSnapshots[j].persistentId);
				PersistentUnloadedPartIds.Add(vessel.protoVessel.protoPartSnapshots[j].persistentId, vessel.protoVessel.protoPartSnapshots[j]);
			}
		}
	}

	public static Vessel FindVessel(Guid id)
	{
		if (!fetch)
		{
			return null;
		}
		int count = fetch.vessels.Count;
		Vessel vessel;
		do
		{
			if (count-- > 0)
			{
				vessel = fetch.vessels[count];
				continue;
			}
			return null;
		}
		while (!(vessel.id == id));
		return vessel;
	}

	public static bool FindVessel(uint id, out Vessel vessel)
	{
		if (!fetch)
		{
			vessel = null;
			return false;
		}
		if (PersistentVesselIds.Contains(id))
		{
			vessel = PersistentVesselIds[id];
			return true;
		}
		vessel = null;
		return false;
	}

	public static int CountUntrackedSpaceObjects()
	{
		return CountSpaceObjects(untrackedOnly: true);
	}

	public static int CountSpaceObjects()
	{
		return CountSpaceObjects(untrackedOnly: false);
	}

	public static int CountSpaceObjects(bool untrackedOnly)
	{
		int num = 0;
		for (int num2 = Vessels.Count - 1; num2 >= 0; num2--)
		{
			Vessel vessel = Vessels[num2];
			if (!untrackedOnly || (vessel != null && !vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors)))
			{
				num++;
			}
		}
		return num;
	}

	public int UntrackedObjectsCount()
	{
		int num = 0;
		for (int num2 = Vessels.Count - 1; num2 >= 0; num2--)
		{
			Vessel vessel = Vessels[num2];
			if (vessel != null && !vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				num++;
			}
		}
		return num;
	}

	public static bool FindLoadedPart(uint id, out Part partout)
	{
		if ((bool)fetch && id != 0)
		{
			if (PersistentLoadedPartIds.Contains(id))
			{
				partout = PersistentLoadedPartIds[id];
				return true;
			}
			partout = null;
			return false;
		}
		partout = null;
		return false;
	}

	public static bool FindUnloadedPart(uint id, out ProtoPartSnapshot partout)
	{
		if (!fetch)
		{
			partout = null;
			return false;
		}
		if (PersistentUnloadedPartIds.Contains(id))
		{
			partout = PersistentUnloadedPartIds[id];
			return true;
		}
		partout = null;
		return false;
	}

	public static uint GetUniquepersistentId()
	{
		uint hashCode = (uint)Guid.NewGuid().GetHashCode();
		if (!_fetch)
		{
			return hashCode;
		}
		while (PersistentLoadedPartIds.Contains(hashCode) || PersistentUnloadedPartIds.Contains(hashCode) || PersistentVesselIds.Contains(hashCode) || hashCode == 0)
		{
			hashCode = (uint)Guid.NewGuid().GetHashCode();
		}
		return hashCode;
	}

	public static uint CheckVesselpersistentId(uint persistentId, Vessel vessel, bool removeOldId, bool addNewId)
	{
		if (persistentId == 0)
		{
			persistentId = GetUniquepersistentId();
			if (!fetch)
			{
				return persistentId;
			}
			if (addNewId)
			{
				PersistentVesselIds.Add(persistentId, vessel);
			}
		}
		else
		{
			Vessel vessel2 = null;
			if (FindVessel(persistentId, out vessel2))
			{
				if (vessel2 == null || vessel == null || vessel2 != vessel)
				{
					uint num = persistentId;
					persistentId = GetUniquepersistentId();
					if (removeOldId || addNewId)
					{
						if ((bool)fetch)
						{
							if (removeOldId)
							{
								PersistentVesselIds.Remove(num);
							}
							if (addNewId)
							{
								PersistentVesselIds.Add(persistentId, vessel);
							}
						}
						GameEvents.onVesselPersistentIdChanged.Fire(num, persistentId);
						Debug.LogFormat("[FlightGlobals]: Vessel persistentId changed from {0} to {1}.", num, persistentId);
					}
				}
			}
			else if ((bool)fetch)
			{
				PersistentVesselIds.Add(persistentId, vessel);
			}
		}
		return persistentId;
	}

	public static uint CheckPartpersistentId(uint persistentId, Part part, bool removeOldId, bool addNewId, uint vesselId = 0u)
	{
		if (persistentId == 0)
		{
			persistentId = GetUniquepersistentId();
			if (!fetch)
			{
				return persistentId;
			}
			if (addNewId)
			{
				PersistentLoadedPartIds.Add(persistentId, part);
			}
		}
		else
		{
			Part partout = null;
			ProtoPartSnapshot partout2 = null;
			bool num = FindLoadedPart(persistentId, out partout);
			bool flag = FindUnloadedPart(persistentId, out partout2);
			bool flag2 = false;
			if (flag)
			{
				flag2 = part.vessel != null && partout2.pVesselRef != null && part.vessel.persistentId == partout2.pVesselRef.persistentId && partout2.pVesselRef.persistentId != 0;
			}
			bool flag3 = false;
			if (num && (partout == null || part == null || partout != part))
			{
				flag3 = true;
			}
			else if (flag && !flag2)
			{
				flag3 = true;
			}
			if (flag3)
			{
				uint num2 = persistentId;
				persistentId = GetUniquepersistentId();
				if ((bool)fetch)
				{
					if (removeOldId)
					{
						PersistentLoadedPartIds.Remove(num2);
					}
					if (addNewId)
					{
						PersistentLoadedPartIds.Add(persistentId, part);
					}
				}
				uint num3 = 0u;
				if (part.vessel != null)
				{
					num3 = part.vessel.persistentId;
				}
				if (num3 == 0 && vesselId != 0)
				{
					num3 = vesselId;
				}
				GameEvents.onPartPersistentIdChanged.Fire(num3, num2, persistentId);
			}
			else if ((bool)fetch)
			{
				PersistentLoadedPartIds.Add(persistentId, part);
			}
		}
		return persistentId;
	}

	public static uint CheckProtoPartSnapShotpersistentId(uint persistentId, ProtoPartSnapshot partSnapshot, bool removeOldId, bool addNewId)
	{
		if (persistentId == 0)
		{
			persistentId = GetUniquepersistentId();
			if (!fetch)
			{
				return persistentId;
			}
			if (addNewId)
			{
				PersistentUnloadedPartIds.Add(persistentId, partSnapshot);
			}
		}
		else
		{
			ProtoPartSnapshot partout = null;
			if (FindUnloadedPart(persistentId, out partout))
			{
				if (partout == null || partSnapshot == null || partout != partSnapshot)
				{
					uint num = persistentId;
					persistentId = GetUniquepersistentId();
					if ((bool)fetch)
					{
						if (removeOldId)
						{
							PersistentUnloadedPartIds.Remove(num);
						}
						if (addNewId)
						{
							PersistentUnloadedPartIds.Add(persistentId, partSnapshot);
						}
					}
					uint num2 = 0u;
					if (partSnapshot != null && partSnapshot.pVesselRef != null)
					{
						num2 = partSnapshot.pVesselRef.persistentId;
					}
					GameEvents.onPartPersistentIdChanged.Fire(num2, num, persistentId);
					Debug.LogFormat("[FlightGlobals]: ProtoPartSnapShot persistentId changed from {0} to {1}. Vessel persistentId {2}", num, persistentId, num2);
				}
			}
			else if ((bool)fetch)
			{
				PersistentUnloadedPartIds.Add(persistentId, partSnapshot);
			}
		}
		return persistentId;
	}

	public static bool ClearpersistentIdDictionaries()
	{
		if (!fetch)
		{
			Debug.LogWarning("[FlightGlobals] Warning: Tried to clear persistentId Dictionaries but instance is null");
			return false;
		}
		PersistentVesselIds.Clear();
		PersistentLoadedPartIds.Clear();
		PersistentUnloadedPartIds.Clear();
		return true;
	}

	public void SetVesselTarget(ITargetable tgt, bool overrideInputLock = false)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.TARGETING) || overrideInputLock)
		{
			_vesselTarget = tgt;
			vesselTargetTransform = tgt?.GetTransform();
			if (tgt is IDiscoverable && !(tgt as IDiscoverable).DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				vesselTargetMode = VesselTargetModes.None;
			}
			else if (tgt != null)
			{
				vesselTargetMode = tgt.GetTargetingMode();
			}
			else
			{
				vesselTargetMode = VesselTargetModes.None;
			}
			if ((bool)ActiveVessel)
			{
				if ((bool)ActiveVessel.orbitTargeter)
				{
					if (tgt != null && tgt.GetOrbitDriver() != null && tgt.GetOrbitDriver() != ActiveVessel.orbitDriver)
					{
						ActiveVessel.orbitTargeter.SetTarget(tgt.GetOrbitDriver());
					}
					else
					{
						ActiveVessel.orbitTargeter.SetTarget(null);
					}
				}
				ActiveVessel.targetObject = tgt;
			}
			if (tgt != null)
			{
				if (tgt is IDiscoverable)
				{
					targetSelectMessage.message = Localizer.Format("#autoLOC_339130", (tgt as IDiscoverable).DiscoveryInfo.displayName.Value);
				}
				else
				{
					targetSelectMessage.message = Localizer.Format("#autoLOC_339134", tgt.GetDisplayName());
				}
			}
			else
			{
				targetSelectMessage.message = Localizer.Format("#autoLOC_339139");
			}
		}
		else
		{
			targetSelectMessage.message = Localizer.Format("#autoLOC_339144");
		}
		ScreenMessages.PostScreenMessage(targetSelectMessage);
	}

	public void Awake()
	{
		_fetch = this;
		ready = false;
		activeVessel = null;
		vesselSwitchFailMessage = new ScreenMessage("", 2f, ScreenMessageStyle.UPPER_CENTER);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void Start()
	{
		lastVel = Vector3d.zero;
		lastCoM = Vector3d.zero;
		FlightLogger.IgnoreGeeForces(3f);
		targetSelectMessage = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
		overrideOrbit = true;
		Invoke("disableOverride", 1f);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
		GameEvents.onGameUnpause.Add(OnGameUnpaused);
		HookVesselEvents();
		CrewGenerator.Initialize();
	}

	public void OnSceneChange(GameScenes scene)
	{
		SetSpeedMode(commandedSpeedMode);
		ResetObjectPartUpwardsCache();
		ResetObjectPartPointerUpwardsCache();
		GameEvents.OnFlightGlobalsReady.Fire(data: false);
		ready = false;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes level)
	{
		lastVel.Zero();
		lastCoM.Zero();
		FlightLogger.IgnoreGeeForces(3f);
		overrideOrbit = true;
		Invoke("disableOverride", 1f);
	}

	public void OnDestroy()
	{
		activeVessel = null;
		currentMainBody = null;
		if (_fetch != null)
		{
			Vessels.Clear();
		}
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		GameEvents.onVesselSituationChange.Remove(SurfaceVesselEaseIn);
		GameEvents.onGameUnpause.Remove(OnGameUnpaused);
		SceneManager.sceneLoaded -= OnSceneLoaded;
		UnhookVesselEvents();
		if (_fetch != null && _fetch == this)
		{
			_fetch = null;
		}
	}

	public void FixedUpdate()
	{
		if (!(activeVessel == null))
		{
			UpdateInformation(fixedUpdate: true);
			if (!ready)
			{
				ready = true;
				GameEvents.OnFlightGlobalsReady.Fire(data: true);
			}
		}
	}

	public void Update()
	{
		if (!(activeVessel == null) && !(currentMainBody == null))
		{
			UpdateInformation(fixedUpdate: false);
			UpdateSpeedMode();
		}
	}

	public void UpdateInformation(bool fixedUpdate)
	{
		try
		{
			currentMainBody = activeVessel.orbitDriver.referenceBody;
			lastVel = ship_obtVelocity;
			camera_altitude = Vector3d.Distance(fetch.mainCameraRef.transform.position, currentMainBody.position) - currentMainBody.Radius;
			camera_position = fetch.mainCameraRef.transform.position;
			ship_temp = activeVessel.externalTemperature;
			ship_dns = activeVessel.atmDensity;
			ship_orientation_offset = rotationOffset * Quaternion.Inverse(activeVessel.ReferenceTransform.rotation);
			Vector3 normalized = Vector3.ProjectOnPlane(currentMainBody.position + (Vector3d)currentMainBody.transform.up * currentMainBody.Radius - activeVessel.transform.position, (activeVessel.transform.position - currentMainBody.position).normalized).normalized;
			Vector3 upwards = (activeVessel.transform.position - currentMainBody.position).normalized;
			ship_orientation = ship_orientation_offset * Quaternion.LookRotation(normalized, upwards);
			ship_heading = Quaternion.Inverse(ship_orientation).eulerAngles.y;
			vesselTargetDelta.Zero();
			vesselTargetDirection.Zero();
			ship_tgtVelocity.Zero();
			ship_tgtSpeed = 0.0;
			if (VesselTarget != null)
			{
				if (VesselTarget.GetOrbitDriver() == currentMainBody.GetOrbitDriver())
				{
					SetVesselTarget(null);
				}
				else if (VesselTarget.GetOrbitDriver() != null && VesselTarget.GetOrbitDriver().celestialBody != null && currentMainBody.HasParent(VesselTarget.GetOrbitDriver().celestialBody))
				{
					SetVesselTarget(null);
				}
				else if (VesselTarget.GetTransform() == null)
				{
					SetVesselTarget(null);
				}
				else if (!VesselTarget.GetActiveTargetable() && VesselTarget.GetVessel() == activeVessel)
				{
					SetVesselTarget(null);
				}
			}
			else if (vesselTargetMode != 0)
			{
				SetVesselTarget(null);
			}
			if (vesselTargetTransform != null)
			{
				Part referenceTransformPart = activeVessel.GetReferenceTransformPart();
				if (referenceTransformPart != null)
				{
					vesselTargetDelta = vesselTargetTransform.position - referenceTransformPart.partTransform.position;
				}
				else
				{
					vesselTargetDelta = vesselTargetTransform.position - activeVessel.transform.position;
				}
				vesselTargetDirection = vesselTargetDelta.normalized;
				if (vesselTargetMode >= VesselTargetModes.DirectionAndVelocity)
				{
					ship_tgtVelocity = ship_obtVelocity - VesselTarget.GetObtVelocity();
					ship_tgtSpeed = ship_tgtVelocity.magnitude;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[FlightGlobals]: threw during UpdateInformation, fixed=" + fixedUpdate.ToString() + ". Exception: " + ex);
		}
	}

	public void HookVesselEvents()
	{
		GameEvents.onPartAttach.Add(OnPartEventTargetAction);
		GameEvents.onPartRemove.Add(OnPartEventTargetAction);
		GameEvents.onPartCouple.Add(OnPartEventFromToAction);
		GameEvents.onPartDie.Add(OnPartEvent);
		GameEvents.onPartUndock.Add(OnPartEvent);
		GameEvents.onVesselWasModified.Add(OnVesselEvent);
		GameEvents.onVesselPartCountChanged.Add(OnVesselEvent);
	}

	public void UnhookVesselEvents()
	{
		GameEvents.onPartAttach.Remove(OnPartEventTargetAction);
		GameEvents.onPartRemove.Remove(OnPartEventTargetAction);
		GameEvents.onPartCouple.Remove(OnPartEventFromToAction);
		GameEvents.onPartDie.Remove(OnPartEvent);
		GameEvents.onPartUndock.Remove(OnPartEvent);
		GameEvents.onVesselWasModified.Remove(OnVesselEvent);
		GameEvents.onVesselPartCountChanged.Remove(OnVesselEvent);
	}

	public void OnPartEvent(Part part)
	{
		if (part != null && part.vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(part.vessel);
		}
	}

	public void OnVesselEvent(Vessel vessel)
	{
		if (vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(vessel);
		}
	}

	public void OnPartEventTargetAction(GameEvents.HostTargetAction<Part, Part> data)
	{
		if ((object)data.host != null && data.host.vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(data.host.vessel);
		}
		if ((object)data.host != null && data.target.vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(data.target.vessel);
		}
	}

	public void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> data)
	{
		if ((object)data.from != null && data.from.vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(data.from.vessel);
		}
		if ((object)data.to != null && data.to.vessel != null)
		{
			GameEvents.onVesselStandardModification.Fire(data.to.vessel);
		}
	}

	public static bool SetActiveVessel(Vessel v, bool clearDeadVessels)
	{
		if (!fetch)
		{
			return false;
		}
		return fetch.setActiveVessel(v, force: false, clearDeadVessels);
	}

	public static bool SetActiveVessel(Vessel v)
	{
		if (!fetch)
		{
			return false;
		}
		return fetch.setActiveVessel(v, force: false, clearDeadVessels: true);
	}

	public static bool ForceSetActiveVessel(Vessel v)
	{
		if (!fetch)
		{
			return false;
		}
		return fetch.setActiveVessel(v, force: true, clearDeadVessels: true);
	}

	public bool setActiveVessel(Vessel v, bool force, bool clearDeadVessels)
	{
		if (v == null)
		{
			return false;
		}
		if (v == activeVessel)
		{
			return false;
		}
		Vessel data = activeVessel;
		Debug.Log("[FLIGHT GLOBALS]: Switching To Vessel " + v.GetDisplayName() + " ---------------------- ", v.gameObject);
		GameEvents.OnFlightGlobalsReady.Fire(data: false);
		ready = false;
		if (ActiveVessel != null)
		{
			if (!force)
			{
				switch (ClearToSave())
				{
				case ClearToSaveStatus.NOT_IN_ATMOSPHERE:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002346");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				case ClearToSaveStatus.NOT_UNDER_ACCELERATION:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002347");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				case ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002348");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				case ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002349");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				case ClearToSaveStatus.NOT_WHILE_ON_A_LADDER:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002350");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				case ClearToSaveStatus.NOT_WHILE_THROTTLED_UP:
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002351");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				}
				if (v.DiscoveryInfo.Level != DiscoveryLevels.Owned)
				{
					vesselSwitchFailMessage.message = Localizer.Format("#autoLOC_6002352");
					ScreenMessages.PostScreenMessage(vesselSwitchFailMessage);
					return false;
				}
			}
			ActiveVessel.MakeInactive();
			ActiveVessel.ClearStaging();
			if (!v.loaded)
			{
				GameEvents.onVesselSwitchingToUnloaded.Fire(data, v);
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
				FlightDriver.StartAndFocusVessel(HighLogic.CurrentGame.Updated(), vessels.IndexOf(v));
				return true;
			}
			if (v.packed)
			{
				int count = Vessels.Count;
				for (int i = 0; i < count; i++)
				{
					Vessels[i].GoOnRails();
				}
			}
		}
		GameEvents.onVesselSwitching.Fire(data, v);
		activeVessel = v;
		currentMainBody = v.orbit.referenceBody;
		OrbitPhysicsManager.CheckReferenceFrame();
		OrbitPhysicsManager.HoldVesselUnpack();
		FlightCamera.SetTarget(v);
		v.MakeActive();
		if (clearDeadVessels)
		{
			ClearDeadVessels();
		}
		GameEvents.onVesselChange.Fire(v);
		return true;
	}

	public void ClearDeadVessels()
	{
		int count = vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = vessels[count];
			if (vessel.state == Vessel.State.DEAD)
			{
				RemoveVessel(vessel);
				UnityEngine.Object.Destroy(vessel);
			}
		}
	}

	public static void ClearAllVessels()
	{
		if (fetch != null)
		{
			int count = fetch.vessels.Count;
			while (count-- > 0)
			{
				Vessel vessel = fetch.vessels[count];
				RemoveVessel(vessel);
				UnityEngine.Object.Destroy(vessel);
			}
		}
	}

	public static void addPhysicalObject(physicalObject pObject)
	{
		physicalObjects.Add(pObject);
	}

	public static void removePhysicalObject(physicalObject pObject)
	{
		physicalObjects.Remove(pObject);
	}

	public void SetShipOrbit(int selBodyIndex, double ecc, double sma, double inc, double double_0, double mna, double argPe, double ObT)
	{
		if (ActiveVessel.easingInToSurface)
		{
			ToggleVesselEaseIn(ActiveVessel, enableEaseIn: false);
		}
		CelestialBody mainBody = ActiveVessel.mainBody;
		currentMainBody = bodies[selBodyIndex];
		PrepForOrbitSet();
		sma = Math.Min(currentMainBody.sphereOfInfluence * 0.99, sma);
		ActiveVessel.orbit.SetOrbit(inc, ecc, sma, double_0, argPe, mna, Planetarium.GetUniversalTime(), currentMainBody);
		PostOrbitSet(mainBody);
	}

	public void SetShipOrbitRendezvous(Vessel target, Vector3d relPosition, Vector3d relVelocity)
	{
		if (ActiveVessel.easingInToSurface)
		{
			ToggleVesselEaseIn(ActiveVessel, enableEaseIn: false);
		}
		CelestialBody mainBody = ActiveVessel.mainBody;
		currentMainBody = target.mainBody;
		PrepForOrbitSet();
		double universalTime = Planetarium.GetUniversalTime();
		Orbit currentOrbit = target.GetCurrentOrbit();
		Vector3d normalized = currentOrbit.vel.normalized;
		Vector3d orbitNormal = currentOrbit.GetOrbitNormal();
		Vector3d vector3d_ = Vector3d.Cross(normalized, orbitNormal);
		Vector3d pos = currentOrbit.pos + relPosition.Basis(vector3d_, normalized, orbitNormal);
		Vector3d vel = currentOrbit.vel + relVelocity.Basis(vector3d_, normalized, orbitNormal);
		ActiveVessel.orbit.UpdateFromStateVectors(pos, vel, currentOrbit.referenceBody, universalTime);
		PostOrbitSet(mainBody);
	}

	public void PrepForOrbitSet()
	{
		overrideOrbit = true;
		Invoke("disableOverride", 2f);
		ActiveVessel.Landed = false;
		ActiveVessel.Splashed = false;
		ActiveVessel.SetLandedAt(string.Empty, null, string.Empty);
		ActiveVessel.KillPermanentGroundContact();
		ActiveVessel.ResetGroundContact();
		int count = Vessels.Count;
		for (int i = 0; i < count; i++)
		{
			Vessel vessel = Vessels[i];
			if (!vessel.packed)
			{
				vessel.GoOnRails();
			}
		}
		clearInverseRotation();
		OrbitPhysicsManager.SetDominantBody(currentMainBody);
	}

	public void PostOrbitSet(CelestialBody oldBody)
	{
		ActiveVessel.orbitDriver.updateFromParameters();
		CollisionEnhancer.bypass = true;
		FloatingOrigin.SetOffset(ActiveVessel.transform.position);
		OrbitPhysicsManager.CheckReferenceFrame();
		OrbitPhysicsManager.HoldVesselUnpack(10);
		if (ActiveVessel.mainBody != oldBody)
		{
			GameEvents.onVesselSOIChanged.Fire(new GameEvents.HostedFromToAction<Vessel, CelestialBody>(ActiveVessel, oldBody, ActiveVessel.mainBody));
		}
		activeVessel.IgnoreGForces(20);
		activeVessel.IgnoreSpeed(20);
	}

	public void SetVesselPosition(int selBodyIndex, double latitude, double longitude, double altitude, double inclination, double heading, bool asl, bool easeToSurface = false, double gravityMultiplier = 0.1)
	{
		SetVesselPosition(selBodyIndex, latitude, longitude, altitude, new Vector3((float)inclination, 0f, (float)heading), asl, easeToSurface, gravityMultiplier);
	}

	public void SetVesselPosition(int selBodyIndex, double latitude, double longitude, double altitude, Vector3 rotation, bool asl, bool easeToSurface = false, double easeInMultiplier = 0.1)
	{
		CelestialBody mainBody = ActiveVessel.mainBody;
		currentMainBody = bodies[selBodyIndex];
		PrepForOrbitSet();
		Vector3d relSurfaceNVector = currentMainBody.GetRelSurfaceNVector(latitude, longitude);
		double num = 0.0;
		if (currentMainBody.pqsController != null)
		{
			num = currentMainBody.pqsController.GetSurfaceHeight(relSurfaceNVector) - currentMainBody.Radius;
		}
		else
		{
			easeToSurface = false;
		}
		if (asl && num < 0.0)
		{
			num = 0.0;
		}
		altitude = Math.Abs(altitude);
		if (altitude > GameSettings.DEBUG_MAX_SETPOSITION_ALTITUDE)
		{
			Debug.LogWarning($"[FlightGlobals]: SetPosition altitude limited to DEBUG_MAX_SETPOSITION_ALTITUDE ({GameSettings.DEBUG_MAX_SETPOSITION_ALTITUDE})");
			altitude = GameSettings.DEBUG_MAX_SETPOSITION_ALTITUDE;
		}
		if (latitude < -89.999)
		{
			latitude = -89.999;
		}
		if (latitude > 89.999)
		{
			latitude = 89.999;
		}
		if (longitude < -180.0)
		{
			longitude = -180.0;
		}
		if (longitude > 180.0)
		{
			longitude = 180.0;
		}
		Vector3d rhs = currentMainBody.GetWorldSurfacePosition(latitude, longitude, altitude + num) - currentMainBody.position;
		Vector3d vector3d = Vector3d.Cross(currentMainBody.angularVelocity, rhs);
		rhs = rhs.xzy;
		vector3d = vector3d.xzy;
		ActiveVessel.orbit.UpdateFromStateVectors(rhs, vector3d, currentMainBody, Planetarium.GetUniversalTime());
		PostOrbitSet(mainBody);
		Quaternion quaternion = default(Quaternion);
		ActiveVessel.SetRotation(Quaternion.identity);
		quaternion = Quaternion.LookRotation(Vector3.ProjectOnPlane(currentMainBody.position + (Vector3d)currentMainBody.transform.up * currentMainBody.Radius - ActiveVessel.transform.position, (ActiveVessel.transform.position - currentMainBody.position).normalized).normalized, (ActiveVessel.transform.position - currentMainBody.position).normalized);
		quaternion *= Quaternion.Inverse(ActiveVessel.ReferenceTransform.rotation);
		quaternion *= Quaternion.AngleAxis(90f, ActiveVessel.ReferenceTransform.right);
		quaternion *= Quaternion.AngleAxis(rotation.z, -ActiveVessel.ReferenceTransform.forward) * Quaternion.AngleAxis(rotation.x, -ActiveVessel.ReferenceTransform.right);
		ActiveVessel.SetRotation(quaternion);
		if (FlightDriver.Pause)
		{
			setPositionInProgress = true;
			setPositionInProgressVesselID = ActiveVessel.persistentId;
		}
		else
		{
			setPositionInProgress = false;
			StartCoroutine(ActiveVesselGoOffRails());
		}
		if (easeToSurface)
		{
			ToggleVesselEaseIn(ActiveVessel, enableEaseIn: true, easeInMultiplier);
		}
		else if (ActiveVessel.easingInToSurface)
		{
			ToggleVesselEaseIn(ActiveVessel, enableEaseIn: false);
		}
	}

	public void OnGameUnpaused()
	{
		if (setPositionInProgress)
		{
			if (setPositionInProgressVesselID == ActiveVessel.persistentId)
			{
				StartCoroutine(ActiveVesselGoOffRails());
			}
			setPositionInProgress = false;
		}
	}

	public IEnumerator ActiveVesselGoOffRails()
	{
		yield return null;
		ActiveVessel.GoOffRails();
	}

	public void SurfaceVesselEaseIn(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		if (data.host.easingInToSurface && data.to == Vessel.Situations.LANDED)
		{
			ToggleVesselEaseIn(data.host, enableEaseIn: false);
		}
	}

	public void ToggleVesselEaseIn(Vessel vessel, bool enableEaseIn, double easeInMultiplier = 1.0)
	{
		if (enableEaseIn)
		{
			if (!vessel.easingInToSurface)
			{
				vessel.easeInMultiplier = easeInMultiplier;
				vessel.gravityMultiplier *= easeInMultiplier;
				vessel.easingInToSurface = true;
				GameEvents.onVesselSituationChange.Add(SurfaceVesselEaseIn);
			}
			else
			{
				vessel.gravityMultiplier /= vessel.easeInMultiplier;
				vessel.easeInMultiplier = easeInMultiplier;
				vessel.gravityMultiplier *= easeInMultiplier;
			}
		}
		else if (vessel.easingInToSurface)
		{
			vessel.gravityMultiplier /= vessel.easeInMultiplier;
			vessel.easingInToSurface = false;
			GameEvents.onVesselSituationChange.Remove(SurfaceVesselEaseIn);
		}
	}

	public void clearInverseRotation()
	{
		for (int i = 0; i < bodies.Count; i++)
		{
			bodies[i].inverseRotation = false;
		}
	}

	public static void ClearInverseRotation()
	{
		fetch.clearInverseRotation();
	}

	public void disableOverride()
	{
		CollisionEnhancer.bypass = false;
		overrideOrbit = false;
	}

	public static CelestialBody getMainBody(Vector3d refPos)
	{
		return inSOI(refPos, fetch.bodies[0]);
	}

	public static CelestialBody inSOI(Vector3d pos, CelestialBody body)
	{
		int count = body.orbitingBodies.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				CelestialBody celestialBody = body.orbitingBodies[i];
				if (!((pos - celestialBody.position).sqrMagnitude >= celestialBody.sphereOfInfluence * celestialBody.sphereOfInfluence))
				{
					return inSOI(pos, celestialBody);
				}
			}
		}
		return body;
	}

	public static CelestialBody getMainBody()
	{
		if ((bool)ActiveVessel)
		{
			return ActiveVessel.orbitDriver.referenceBody;
		}
		return getMainBody(Vector3.zero);
	}

	public static Vector3d getGeeForceAtPosition(Vector3d pos)
	{
		return getGeeForceAtPosition(pos, getMainBody(pos));
	}

	public static Vector3d getGeeForceAtPosition(Vector3d pos, CelestialBody mainBody)
	{
		_ = Vector3d.zero;
		pos -= mainBody.position;
		double sqrMagnitude = pos.sqrMagnitude;
		sqrMagnitude *= Math.Sqrt(sqrMagnitude);
		return pos * (0.0 - mainBody.gMagnitudeAtCenter) / sqrMagnitude;
	}

	public static Vector3d getCentrifugalAcc(Vector3d pos, CelestialBody body)
	{
		if (!body.inverseRotation)
		{
			return Vector3d.zero;
		}
		pos = body.position - pos;
		return Vector3d.Cross(body.angularVelocity, Vector3d.Cross(body.angularVelocity, pos));
	}

	public static Vector3d getCoriolisAcc(Vector3d vel, CelestialBody body)
	{
		if (!body.inverseRotation)
		{
			return Vector3d.zero;
		}
		return -2.0 * Vector3d.Cross(body.angularVelocity, vel);
	}

	public static double getExternalTemperature()
	{
		return getExternalTemperature(ship_altitude);
	}

	public static double getExternalTemperature(double altitude, CelestialBody body = null)
	{
		if (body == null)
		{
			body = currentMainBody;
		}
		if (!body.atmosphere)
		{
			return 0.0;
		}
		return body.GetTemperature(altitude);
	}

	public static double getExternalTemperature(Vector3d pos, CelestialBody body = null)
	{
		if (body == null)
		{
			body = currentMainBody;
		}
		if (!body.atmosphere)
		{
			return 0.0;
		}
		return body.GetTemperature(getAltitudeAtPos(pos, body));
	}

	public static double getStaticPressure(double altitude, CelestialBody body = null)
	{
		if (body == null)
		{
			body = currentMainBody;
		}
		if (!body.atmosphere)
		{
			return 0.0;
		}
		return body.GetPressure(altitude);
	}

	public static double getStaticPressure(Vector3d position)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return 101.325;
		}
		CelestialBody mainBody = getMainBody(position);
		return getStaticPressure(getAltitudeAtPos(position, mainBody), mainBody);
	}

	public static double getStaticPressure(Vector3d position, CelestialBody body)
	{
		return body.GetPressure(getAltitudeAtPos(position, body));
	}

	public static double getStaticPressure()
	{
		if (_fetch != null && _fetch.activeVessel != null)
		{
			return _fetch.activeVessel.staticPressurekPa;
		}
		return getStaticPressure(ship_altitude);
	}

	public static double getAtmDensity(double pressure, double temperature, CelestialBody body = null)
	{
		if (body == null)
		{
			body = currentMainBody;
		}
		if (!body.atmosphere)
		{
			return 0.0;
		}
		return body.GetDensity(pressure, temperature);
	}

	public static double getAltitudeAtPos(Vector3d position)
	{
		return getAltitudeAtPos(position, getMainBody(position));
	}

	public static float getAltitudeAtPos(Vector3 position)
	{
		return (float)getAltitudeAtPos((Vector3d)position, getMainBody(position));
	}

	public static float getAltitudeAtPos(Vector3 position, CelestialBody body)
	{
		return (float)getAltitudeAtPos((Vector3d)position, body);
	}

	public static double getAltitudeAtPos(Vector3d position, CelestialBody body)
	{
		return Vector3d.Distance(position, body.position) - body.Radius;
	}

	public static double GetSqrAltitude(Vector3d position, CelestialBody body)
	{
		return (position - body.position).sqrMagnitude - body.Radius * body.Radius;
	}

	public static Vector3d getUpAxis()
	{
		return getUpAxis(ActiveVessel.mainBody, ActiveVessel.transform.position);
	}

	public static Vector3d getUpAxis(Vector3d position)
	{
		return (position - getMainBody(position).position).normalized;
	}

	public static Vector3d getUpAxis(CelestialBody body, Vector3d position)
	{
		return (position - body.position).normalized;
	}

	public static ClearToSaveStatus ClearToSave()
	{
		return ClearToSave(logMsg: true);
	}

	public static ClearToSaveStatus ClearToSave(bool logMsg)
	{
		if (ActiveVessel.state == Vessel.State.DEAD)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: Active Vessel is dead. Saving not impossible but not advisable unless leaving flight.", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.CLEAR;
		}
		if (ActiveVessel.isEVA && ActiveVessel.evaController != null && ActiveVessel.evaController.OnALadder)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: There are Kerbals on a ladder. Cannot save", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.NOT_WHILE_ON_A_LADDER;
		}
		if ((ActiveVessel.situation == Vessel.Situations.SUB_ORBITAL || ActiveVessel.situation == Vessel.Situations.FLYING) && ActiveVessel.heightFromTerrain < Mathf.Max(0.5f, GameSettings.QUICKSAVE_MINIMUM_ALTITUDE) && ActiveVessel.heightFromTerrain != -1f)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: Active Vessel is about to crash. Cannot save", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH;
		}
		if (ActiveVessel.situation == Vessel.Situations.FLYING)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: Active Vessel is in atmosphere. Cannot save.", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.NOT_IN_ATMOSPHERE;
		}
		if (ActiveVessel.geeForce > 0.1 && !ActiveVessel.LandedOrSplashed)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: Active Vessel is under acceleration (G = " + ActiveVessel.geeForce + "). Cannot save.", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.NOT_UNDER_ACCELERATION;
		}
		if (ActiveVessel.LandedOrSplashed && ActiveVessel.srf_velocity.sqrMagnitude > 0.09000000357627869)
		{
			if (logMsg)
			{
				Debug.Log("[FlightGlobals]: Active Vessel is moving (sqrVel = " + ActiveVessel.srf_velocity.sqrMagnitude + "). Cannot save.", ActiveVessel.gameObject);
			}
			return ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE;
		}
		int count = Vessels.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Vessel vessel = Vessels[num];
				if (vessel.isEVA && (bool)vessel.evaController && vessel.evaController.OnALadder)
				{
					break;
				}
				num++;
				continue;
			}
			return ClearToSaveStatus.CLEAR;
		}
		if (logMsg)
		{
			Debug.Log("[FlightGlobals]: There are Kerbals on a ladder. Cannot save", ActiveVessel.gameObject);
		}
		return ClearToSaveStatus.NOT_WHILE_ON_A_LADDER;
	}

	public static string GetNotClearToSaveStatusReason(ClearToSaveStatus status, string attempt)
	{
		return status switch
		{
			ClearToSaveStatus.NOT_IN_ATMOSPHERE => Localizer.Format("#autoLOC_7003248", attempt), 
			ClearToSaveStatus.NOT_UNDER_ACCELERATION => Localizer.Format("#autoLOC_7003249", attempt), 
			ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE => Localizer.Format("#autoLOC_7003251", attempt), 
			ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH => Localizer.Format("#autoLOC_7003250", attempt), 
			ClearToSaveStatus.NOT_WHILE_ON_A_LADDER => Localizer.Format("#autoLOC_7003252", attempt), 
			ClearToSaveStatus.NOT_WHILE_THROTTLED_UP => Localizer.Format("#autoLOC_7003253", attempt), 
			_ => Localizer.Format("#autoLOC_7003254", attempt), 
		};
	}

	public Quaternion getFoR(FoRModes mode, Transform referenceTransform, Orbit orbit, CelestialBody body = null)
	{
		if (body == null)
		{
			FoRupAxis = getUpAxis(referenceTransform.position);
		}
		else
		{
			FoRupAxis = getUpAxis(body, referenceTransform.position);
		}
		switch (mode)
		{
		case FoRModes.SRF_NORTH:
			return Quaternion.LookRotation(Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up, FoRupAxis)) * -FoRupAxis, FoRupAxis);
		case FoRModes.SRF_VEL:
			endDirection = Vector3.ProjectOnPlane((orbit.GetVel() + referenceTransform.forward * 0.1f).normalized, FoRupAxis).normalized;
			targetDirection = Vector3.Slerp(targetDirection, endDirection, GameSettings.FLT_CAMERA_CHASE_SHARPNESS * Time.deltaTime);
			return Quaternion.LookRotation(targetDirection, upAxis);
		case FoRModes.SRF_HDG:
			endDirection = Vector3.ProjectOnPlane((referenceTransform.up + referenceTransform.forward * 0.1f).normalized, FoRupAxis).normalized;
			targetDirection = Vector3.Slerp(targetDirection, endDirection, GameSettings.FLT_CAMERA_CHASE_SHARPNESS * Time.deltaTime);
			return Quaternion.LookRotation(targetDirection, FoRupAxis);
		default:
			return Quaternion.identity;
		case FoRModes.OBT_PLN:
			return Quaternion.LookRotation(orbit.GetVel(), orbit.h);
		case FoRModes.OBT_VEL:
			return Quaternion.LookRotation(orbit.GetVel().normalized, Vector3.up);
		case FoRModes.OBT_CTR:
			return Quaternion.LookRotation(-FoRupAxis, Vector3.up);
		case FoRModes.SHP_REL:
			return Quaternion.LookRotation(referenceTransform.up, -referenceTransform.forward);
		}
	}

	public static Quaternion GetFoR(FoRModes mode)
	{
		return fetch.getFoR(mode, ActiveVessel.ReferenceTransform, ActiveVessel.orbit, ActiveVessel.mainBody);
	}

	public static Quaternion GetFoR(FoRModes mode, Transform referenceTransform)
	{
		if (mode == FoRModes.SRF_VEL)
		{
			mode = FoRModes.SRF_HDG;
		}
		if (mode == FoRModes.OBT_PLN || mode == FoRModes.OBT_VEL)
		{
			mode = FoRModes.OBT_ABS;
		}
		return fetch.getFoR(mode, referenceTransform, null);
	}

	public static Quaternion GetFoR(FoRModes mode, Transform referenceTransform, Orbit orbit)
	{
		return fetch.getFoR(mode, referenceTransform, orbit);
	}

	public static Part FindPartByID(uint flightID)
	{
		if (!fetch)
		{
			return null;
		}
		if (flightID == 0)
		{
			Debug.LogWarning("Warning: Tried to search for a part with ID 0");
			return null;
		}
		int count = VesselsLoaded.Count;
		while (count-- > 0)
		{
			Vessel vessel = VesselsLoaded[count];
			int count2 = vessel.parts.Count;
			while (count2-- > 0)
			{
				Part part = vessel.parts[count2];
				if (part.flightID == flightID)
				{
					return part;
				}
			}
		}
		return null;
	}

	public static ProtoPartSnapshot FindProtoPartByID(uint flightID)
	{
		if (flightID == 0)
		{
			Debug.LogWarning("Warning: Tried to search for a part with ID 0");
			return null;
		}
		int count = Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = Vessels[count];
			int count2 = vessel.protoVessel.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[count2];
				if (protoPartSnapshot.flightID == flightID)
				{
					return protoPartSnapshot;
				}
			}
		}
		return null;
	}

	public static Vessel FindNearestControllableVessel(Vessel currentVessel)
	{
		Vessel result = null;
		float num = float.MaxValue;
		int count = VesselsLoaded.Count;
		for (int i = 0; i < count; i++)
		{
			Vessel vessel = VesselsLoaded[i];
			if (!vessel.packed && vessel.IsControllable && vessel.state != Vessel.State.DEAD && vessel != currentVessel)
			{
				float sqrMagnitude = (vessel.transform.position - currentVessel.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = vessel;
				}
			}
		}
		return result;
	}

	public static List<Vessel> FindNearestVesselWhere(Vector3d refPos, Func<Vessel, bool> criteria)
	{
		List<Vessel> list = Vessels.Where(criteria).ToList();
		if (list.Count > 0)
		{
			list = list.OrderBy((Vessel v) => (v.GetWorldPos3D() - refPos).sqrMagnitude).ToList();
		}
		return list;
	}

	public static CelestialBody GetBodyByName(string bodyName)
	{
		if (!(fetch == null) && fetch.bodyNames != null)
		{
			if (fetch.bodyNames.ContainsKey(bodyName))
			{
				CelestialBody celestialBody = fetch.bodyNames[bodyName];
				if (celestialBody != null)
				{
					return celestialBody;
				}
			}
			int count = Bodies.Count;
			CelestialBody celestialBody2;
			string text;
			do
			{
				if (count-- > 0)
				{
					celestialBody2 = Bodies[count];
					text = celestialBody2.name;
					continue;
				}
				return null;
			}
			while (text != bodyName);
			fetch.bodyNames[text] = celestialBody2;
			return celestialBody2;
		}
		return null;
	}

	public static int GetBodyIndex(CelestialBody body)
	{
		int count = Bodies.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (Bodies[num] == body)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public static int GetHomeBodyIndex()
	{
		if (fetch.homeBody == null)
		{
			fetch.homeBody = GetHomeBody();
		}
		return fetch.homeBodyIndex;
	}

	public static CelestialBody GetHomeBody()
	{
		if (fetch == null || fetch.homeBody == null)
		{
			int count = Bodies.Count;
			for (int i = 0; i < count; i++)
			{
				if (Bodies[i].isHomeWorld)
				{
					if (fetch == null)
					{
						return Bodies[i];
					}
					fetch.homeBody = Bodies[i];
					fetch.homeBodyIndex = i;
					break;
				}
			}
		}
		if (fetch == null)
		{
			return Bodies[0];
		}
		return fetch.homeBody;
	}

	public static string GetHomeBodyName()
	{
		if (fetch.homeBody == null)
		{
			fetch.homeBody = GetHomeBody();
		}
		return fetch.homeBody.name;
	}

	public static string GetHomeBodyDisplayName()
	{
		if (fetch.homeBody == null)
		{
			fetch.homeBody = GetHomeBody();
		}
		return fetch.homeBody.displayName;
	}

	public static int GetCometPerturberIndex()
	{
		if (fetch.cometPerturber == null)
		{
			fetch.cometPerturber = GetHomeBody();
		}
		return fetch.cometPerturberIndex;
	}

	public static CelestialBody GetCometPerturber()
	{
		if (fetch == null || fetch.cometPerturber == null)
		{
			int count = Bodies.Count;
			for (int i = 0; i < count; i++)
			{
				if (Bodies[i].isCometPerturber)
				{
					if (fetch == null)
					{
						return Bodies[i];
					}
					fetch.cometPerturber = Bodies[i];
					fetch.cometPerturberIndex = i;
					break;
				}
			}
		}
		if (fetch == null)
		{
			CelestialBody sun = Planetarium.fetch.Sun;
			if (sun == null)
			{
				return Bodies[0];
			}
			return GetMidSystemPlanet(sun);
		}
		return fetch.cometPerturber;
	}

	public static CelestialBody GetMidSystemPlanet(CelestialBody parentBody)
	{
		int count = Bodies.Count;
		double num = double.MaxValue;
		double num2 = double.MinValue;
		for (int i = 0; i < count; i++)
		{
			if (!(Bodies[i].orbit.referenceBody != parentBody))
			{
				if (Bodies[i].orbit.semiMajorAxis > num2)
				{
					num2 = Bodies[i].orbit.semiMajorAxis;
				}
				if (Bodies[i].orbit.semiMajorAxis < num)
				{
					num = Bodies[i].orbit.semiMajorAxis;
				}
			}
		}
		double num3 = (num + num2) / 2.0;
		CelestialBody result = Bodies[0];
		double num4 = double.MaxValue;
		for (int j = 0; j < count; j++)
		{
			if (Math.Abs(Bodies[j].orbit.semiMajorAxis - num3) < num4)
			{
				num4 = Math.Abs(Bodies[j].orbit.semiMajorAxis - num3);
				result = Bodies[j];
			}
		}
		return result;
	}

	public static string GetCometPerturberName()
	{
		if (fetch.cometPerturber == null)
		{
			fetch.cometPerturber = GetHomeBody();
		}
		return fetch.cometPerturber.name;
	}

	public static string GGetCometPerturberDisplayName()
	{
		if (fetch.cometPerturber == null)
		{
			fetch.cometPerturber = GetHomeBody();
		}
		return fetch.cometPerturber.displayName;
	}

	public void UpdateSpeedMode()
	{
		if (commandedSpeedMode == SpeedDisplayModes.Target && _vesselTarget == null)
		{
			commandedSpeedMode = SpeedDisplayModes.Orbit;
			SetSpeedMode(commandedSpeedMode);
		}
		else if (commandedSpeedMode != SpeedDisplayModes.Target && _vesselTarget != null && vesselTargetMode >= VesselTargetModes.DirectionAndVelocity && (activeVessel.transform.position - _vesselTarget.GetTransform().position).sqrMagnitude < TargetSwitchSqrThresh && (!activeVessel.Autopilot.Enabled || activeVessel.Autopilot.Mode == VesselAutopilot.AutopilotMode.StabilityAssist || activeVessel.Autopilot.Mode >= VesselAutopilot.AutopilotMode.Target))
		{
			commandedSpeedMode = SpeedDisplayModes.Target;
			SetSpeedMode(commandedSpeedMode);
		}
		else if (commandedSpeedMode == SpeedDisplayModes.Surface)
		{
			if (ship_altitude > currentMainBody.Radius * currentMainBody.navballSwitchRadiusMult)
			{
				commandedSpeedMode = SpeedDisplayModes.Orbit;
				SetSpeedMode(commandedSpeedMode);
			}
		}
		else if (commandedSpeedMode == SpeedDisplayModes.Orbit && ship_altitude < currentMainBody.Radius * currentMainBody.navballSwitchRadiusMultLow)
		{
			commandedSpeedMode = SpeedDisplayModes.Surface;
			SetSpeedMode(commandedSpeedMode);
		}
	}

	public static void SetSpeedMode(SpeedDisplayModes newSpeedMode)
	{
		speedMode = newSpeedMode;
		GameEvents.onSetSpeedMode.Fire(speedDisplayMode);
	}

	public static void CycleSpeedModes()
	{
		speedMode = (SpeedDisplayModes)((int)(speedMode + 1) % 3);
		if (speedDisplayMode == SpeedDisplayModes.Target && fetch.VesselTarget == null)
		{
			CycleSpeedModes();
		}
		else
		{
			SetSpeedMode(speedMode);
		}
	}

	public static double GetDisplaySpeed()
	{
		return speedMode switch
		{
			SpeedDisplayModes.Surface => ship_srfSpeed, 
			SpeedDisplayModes.Target => ship_tgtSpeed, 
			_ => ship_obtSpeed, 
		};
	}

	public static Vector3d GetDisplayVelocity()
	{
		return speedMode switch
		{
			SpeedDisplayModes.Surface => ActiveVessel.srf_velocity, 
			SpeedDisplayModes.Target => ship_tgtVelocity, 
			_ => ActiveVessel.obt_velocity, 
		};
	}
}
