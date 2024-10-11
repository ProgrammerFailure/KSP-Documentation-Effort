using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

	[CompilerGenerated]
	private sealed class _003CActiveVesselGoOffRails_003Ed__166 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CActiveVesselGoOffRails_003Ed__166(int _003C_003E1__state)
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

	public static bool ready;

	public List<CelestialBody> bodies;

	private static FlightGlobals _fetch;

	public float srfAttachStiffNess;

	public float stackAttachStiffNess;

	public Vessel activeVessel;

	public List<Vessel> vessels;

	public List<Vessel> vesselsLoaded;

	public List<Vessel> vesselsUnloaded;

	public DictionaryValueList<uint, Vessel> persistentVesselIds;

	public DictionaryValueList<uint, Part> persistentLoadedPartIds;

	public DictionaryValueList<uint, ProtoPartSnapshot> persistentUnloadedPartIds;

	private static Dictionary<GameObject, Part> objectToPartUpwardsCache;

	private static Dictionary<GameObject, PartPointer> objectToPartPointerUpwardsCache;

	private bool setPositionInProgress;

	private uint setPositionInProgressVesselID;

	public static double ship_temp;

	public static double ship_dns;

	public static Quaternion ship_orientation;

	public static Quaternion ship_orientation_offset;

	public static float ship_heading;

	public static double camera_altitude;

	public static Vector3d camera_position;

	public static CelestialBody currentMainBody;

	public Camera mainCameraRef;

	private double lastAltitude;

	private double lastVS;

	private Vector3d lastVel;

	private Vector3d lastCoM;

	private ScreenMessage vesselSwitchFailMessage;

	private ITargetable _vesselTarget;

	private ScreenMessage targetSelectMessage;

	public Transform vesselTargetTransform;

	[HideInInspector]
	public Vector3 vesselTargetDelta;

	[HideInInspector]
	public Vector3 vesselTargetDirection;

	public static Vector3d ship_tgtVelocity;

	public static double ship_tgtSpeed;

	public static List<physicalObject> physicalObjects;

	private static Quaternion rotationOffset;

	public static bool overrideOrbit;

	public static bool warpDriveActive;

	private static double tempDouble;

	public static FoRModes FoRMode;

	private Vector3 targetDirection;

	private Vector3 endDirection;

	private Vector3 FoRupAxis;

	private Dictionary<string, CelestialBody> bodyNames;

	private CelestialBody homeBody;

	private int homeBodyIndex;

	private CelestialBody cometPerturber;

	private int cometPerturberIndex;

	public static float TargetSwitchSqrThresh;

	private SpeedDisplayModes commandedSpeedMode;

	private static SpeedDisplayModes speedMode;

	public static List<CelestialBody> Bodies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FlightGlobals fetch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float vacuumTemperature
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float SrfAttachStiffNess
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float StackAttachStiffNess
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Part activeTarget
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vessel ActiveVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool PartPlacementMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<Vessel> Vessels
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<Vessel> VesselsLoaded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<Vessel> VesselsUnloaded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DictionaryValueList<uint, Vessel> PersistentVesselIds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DictionaryValueList<uint, Part> PersistentLoadedPartIds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DictionaryValueList<uint, ProtoPartSnapshot> PersistentUnloadedPartIds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_altitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_verticalSpeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_geeForce
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_velocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_obtVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_obtSpeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_srfVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_srfSpeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_acceleration
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_angularVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_position
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_CoM
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ship_upAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3 ship_MOI
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3 ship_angularMomentum
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Quaternion ship_rotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Orbit ship_orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_latitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_longitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double ship_stP
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool RefFrameIsRotating
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ITargetable VesselTarget
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VesselTargetModes vesselTargetMode
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

	public static Vector3d upAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static SpeedDisplayModes speedDisplayMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part GetPartUpwardsCached(GameObject go)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartPointer GetPartPointerUpwardsCached(GameObject go)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetObjectPartUpwardsCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetObjectPartPointerUpwardsCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SortVessel(Vessel vessel, bool loaded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vessel FindVessel(Guid id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindVessel(uint id, out Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int CountUntrackedSpaceObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int CountSpaceObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int CountSpaceObjects(bool untrackedOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int UntrackedObjectsCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindLoadedPart(uint id, out Part partout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindUnloadedPart(uint id, out ProtoPartSnapshot partout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetUniquepersistentId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint CheckVesselpersistentId(uint persistentId, Vessel vessel, bool removeOldId, bool addNewId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint CheckPartpersistentId(uint persistentId, Part part, bool removeOldId, bool addNewId, uint vesselId = 0u)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint CheckProtoPartSnapShotpersistentId(uint persistentId, ProtoPartSnapshot partSnapshot, bool removeOldId, bool addNewId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool ClearpersistentIdDictionaries()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVesselTarget(ITargetable tgt, bool overrideInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneChange(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateInformation(bool fixedUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HookVesselEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnhookVesselEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEvent(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselEvent(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEventTargetAction(GameEvents.HostTargetAction<Part, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool SetActiveVessel(Vessel v, bool clearDeadVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetActiveVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ForceSetActiveVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool setActiveVessel(Vessel v, bool force, bool clearDeadVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearDeadVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void ClearAllVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void addPhysicalObject(physicalObject pObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void removePhysicalObject(physicalObject pObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetShipOrbit(int selBodyIndex, double ecc, double sma, double inc, double LAN, double mna, double argPe, double ObT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetShipOrbitRendezvous(Vessel target, Vector3d relPosition, Vector3d relVelocity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PrepForOrbitSet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PostOrbitSet(CelestialBody oldBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVesselPosition(int selBodyIndex, double latitude, double longitude, double altitude, double inclination, double heading, bool asl, bool easeToSurface = false, double gravityMultiplier = 0.1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVesselPosition(int selBodyIndex, double latitude, double longitude, double altitude, Vector3 rotation, bool asl, bool easeToSurface = false, double easeInMultiplier = 0.1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameUnpaused()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CActiveVesselGoOffRails_003Ed__166))]
	private IEnumerator ActiveVesselGoOffRails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SurfaceVesselEaseIn(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ToggleVesselEaseIn(Vessel vessel, bool enableEaseIn, double easeInMultiplier = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void clearInverseRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearInverseRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void disableOverride()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody getMainBody(Vector3d refPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static CelestialBody inSOI(Vector3d pos, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody getMainBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getGeeForceAtPosition(Vector3d pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getGeeForceAtPosition(Vector3d pos, CelestialBody mainBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getCentrifugalAcc(Vector3d pos, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getCoriolisAcc(Vector3d vel, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getExternalTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getExternalTemperature(double altitude, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getExternalTemperature(Vector3d pos, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getStaticPressure(double altitude, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getStaticPressure(Vector3d position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getStaticPressure(Vector3d position, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getStaticPressure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getAtmDensity(double pressure, double temperature, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getAltitudeAtPos(Vector3d position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float getAltitudeAtPos(Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float getAltitudeAtPos(Vector3 position, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double getAltitudeAtPos(Vector3d position, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetSqrAltitude(Vector3d position, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getUpAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getUpAxis(Vector3d position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d getUpAxis(CelestialBody body, Vector3d position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ClearToSaveStatus ClearToSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ClearToSaveStatus ClearToSave(bool logMsg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetNotClearToSaveStatusReason(ClearToSaveStatus status, string attempt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion getFoR(FoRModes mode, Transform referenceTransform, Orbit orbit, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion GetFoR(FoRModes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion GetFoR(FoRModes mode, Transform referenceTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion GetFoR(FoRModes mode, Transform referenceTransform, Orbit orbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindPartByID(uint flightID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ProtoPartSnapshot FindProtoPartByID(uint flightID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vessel FindNearestControllableVessel(Vessel currentVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Vessel> FindNearestVesselWhere(Vector3d refPos, Func<Vessel, bool> criteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody GetBodyByName(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetBodyIndex(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetHomeBodyIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody GetHomeBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetHomeBodyName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetHomeBodyDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetCometPerturberIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody GetCometPerturber()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static CelestialBody GetMidSystemPlanet(CelestialBody parentBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCometPerturberName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GGetCometPerturberDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSpeedMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetSpeedMode(SpeedDisplayModes newSpeedMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CycleSpeedModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetDisplaySpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d GetDisplayVelocity()
	{
		throw null;
	}
}
