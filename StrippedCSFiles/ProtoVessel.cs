using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProtoVessel
{
	public List<ProtoPartSnapshot> protoPartSnapshots;

	public ConfigNode vesselModules;

	public Dictionary<string, KSPParseable> vesselStateValues;

	public OrbitSnapshot orbitSnapShot;

	public Guid vesselID;

	public uint persistentId;

	public bool skipGroundPositioning;

	public bool skipGroundPositioningForDroppedPart;

	public bool vesselSpawning;

	public string launchedFrom;

	public bool landed;

	public string landedAt;

	public string displaylandedAt;

	public bool splashed;

	public string vesselName;

	public int rootIndex;

	public int stage;

	public double distanceTraveled;

	public uint refTransform;

	public Vector3d position;

	public double altitude;

	public double latitude;

	public double longitude;

	public float height;

	public Vector3 normal;

	public Quaternion rotation;

	public Vector3 CoM;

	public Vessel vesselRef;

	public Vessel.Situations situation;

	public VesselType vesselType;

	public double missionTime;

	public double launchTime;

	public double lastUT;

	public bool autoClean;

	public string autoCleanReason;

	public bool persistent;

	public int GroupOverride;

	public bool[] OverrideDefault;

	public KSPActionGroup[] OverrideActionControl;

	public KSPAxisGroup[] OverrideAxisControl;

	public string[] OverrideGroupNames;

	public ConfigNode actionGroups;

	public ConfigNode discoveryInfo;

	public ConfigNode flightPlan;

	public ConfigNode ctrlState;

	public ProtoTargetInfo targetInfo;

	public AltimeterDisplayState altimeterDisplayState;

	public ProtoWaypointInfo waypointInfo;

	public bool wasControllable;

	private List<ProtoCrewMember> crew;

	public int crewedParts;

	public int crewableParts;

	public int PQSminLevel;

	public int PQSmaxLevel;

	public static int MissingPartsWindowIndex;

	private static string cacheAutoLOC_145488;

	private static string cacheAutoLOC_417274;

	private static string cacheAutoLOC_145785;

	private static string cacheAutoLOC_145786;

	private static string cacheAutoLOC_145789;

	private static string cacheAutoLOC_145790;

	private static string cacheAutoLOC_145792;

	private static string cacheAutoLOC_145793;

	private static string cacheAutoLOC_145800;

	private static string cacheAutoLOC_145801;

	private static string cacheAutoLOC_6002161;

	private static string cacheAutoLOC_145803;

	private static string cacheAutoLOC_145804;

	private static string cacheAutoLOC_145805;

	private static string cacheAutoLOC_145806;

	private static string cacheAutoLOC_145807;

	private static string cacheAutoLOC_145808;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoVessel(Vessel VesselRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoVessel(Vessel VesselRef, bool preCreate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoVessel(ConfigNode node, Game st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ProtoVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void verifyCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoCrewMember> GetVesselCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RebuildCrewCounts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddCrew(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveCrew(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveVesselModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetProtoPartSnapShots()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(FlightState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Load(FlightState st, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateVesselNode(string vesselName, VesselType vesselType, Orbit orbit, int rootPartIndex, ConfigNode[] partNodes, params ConfigNode[] additionalNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreatePartNode(string partName, uint id, params ProtoCrewMember[] crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static ConfigNode CreatePartNode(Part part, string partName, uint id, params ProtoCrewMember[] crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateOrbitNode(Orbit orbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateDiscoveryNode(DiscoveryLevels level, UntrackedObjectClass size, double lifeTime, double maxLifeTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSituationString(ProtoVessel pv, List<CelestialBody> bodies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoPartSnapshot> GetAllProtoPartsIncludingCargo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoPartSnapshot> GetAllProtoPartsFromCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clean(string reason = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float CurrentFunds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float CurrentScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float CurrentReputation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
