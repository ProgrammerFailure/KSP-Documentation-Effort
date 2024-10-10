using System;
using System.Collections.Generic;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

public class ProtoVessel
{
	public List<ProtoPartSnapshot> protoPartSnapshots;

	public ConfigNode vesselModules;

	public Dictionary<string, KSPParseable> vesselStateValues;

	public OrbitSnapshot orbitSnapShot;

	public Guid vesselID = Guid.Empty;

	public uint persistentId;

	public bool skipGroundPositioning;

	public bool skipGroundPositioningForDroppedPart;

	public bool vesselSpawning;

	public string launchedFrom = "";

	public bool landed;

	public string landedAt = "";

	public string displaylandedAt = "";

	public bool splashed;

	public string vesselName = "Unnamed Vessel";

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

	public double lastUT = -1.0;

	public bool autoClean;

	public string autoCleanReason = string.Empty;

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

	public List<ProtoCrewMember> crew;

	public int crewedParts;

	public int crewableParts;

	public int PQSminLevel;

	public int PQSmaxLevel;

	public static int MissingPartsWindowIndex;

	public static string cacheAutoLOC_145488;

	public static string cacheAutoLOC_417274;

	public static string cacheAutoLOC_145785;

	public static string cacheAutoLOC_145786;

	public static string cacheAutoLOC_145789;

	public static string cacheAutoLOC_145790;

	public static string cacheAutoLOC_145792;

	public static string cacheAutoLOC_145793;

	public static string cacheAutoLOC_145800;

	public static string cacheAutoLOC_145801;

	public static string cacheAutoLOC_6002161;

	public static string cacheAutoLOC_145803;

	public static string cacheAutoLOC_145804;

	public static string cacheAutoLOC_145805;

	public static string cacheAutoLOC_145806;

	public static string cacheAutoLOC_145807;

	public static string cacheAutoLOC_145808;

	public ProtoVessel(Vessel VesselRef)
		: this(VesselRef, preCreate: true)
	{
	}

	public ProtoVessel(Vessel VesselRef, bool preCreate)
	{
		vesselRef = VesselRef;
		protoPartSnapshots = new List<ProtoPartSnapshot>();
		vesselStateValues = new Dictionary<string, KSPParseable>();
		orbitSnapShot = new OrbitSnapshot(VesselRef.orbit);
		crew = new List<ProtoCrewMember>();
		vesselID = VesselRef.id;
		persistentId = vesselRef.persistentId;
		refTransform = vesselRef.referenceTransformId;
		vesselType = VesselRef.vesselType;
		situation = VesselRef.situation;
		landed = VesselRef.Landed;
		skipGroundPositioning = vesselRef.skipGroundPositioning;
		skipGroundPositioningForDroppedPart = vesselRef.skipGroundPositioningForDroppedPart;
		vesselSpawning = vesselRef.vesselSpawning;
		launchedFrom = vesselRef.launchedFrom;
		landedAt = VesselRef.landedAt;
		displaylandedAt = VesselRef.displaylandedAt;
		splashed = VesselRef.Splashed;
		vesselName = VesselRef.vesselName;
		distanceTraveled = VesselRef.distanceTraveled;
		missionTime = VesselRef.missionTime;
		launchTime = VesselRef.launchTime;
		lastUT = VesselRef.lastUT;
		autoClean = VesselRef.AutoClean;
		autoCleanReason = VesselRef.AutoCleanReason;
		wasControllable = VesselRef.IsControllable;
		for (int i = 0; i < VesselRef.parts.Count; i++)
		{
			Part part = VesselRef.parts[i];
			if (part.State != PartStates.DEAD)
			{
				protoPartSnapshots.Add(new ProtoPartSnapshot(part, this));
			}
		}
		int j = 0;
		for (int count = protoPartSnapshots.Count; j < count; j++)
		{
			protoPartSnapshots[j].storePartRefs();
		}
		CoM = vesselRef.localCoM;
		latitude = vesselRef.latitude;
		longitude = vesselRef.longitude;
		altitude = vesselRef.altitude;
		height = vesselRef.heightFromTerrain;
		normal = vesselRef.terrainNormal;
		altimeterDisplayState = vesselRef.altimeterDisplayState;
		rotation = vesselRef.srfRelRotation;
		PQSminLevel = vesselRef.PQSminLevel;
		PQSmaxLevel = vesselRef.PQSmaxLevel;
		stage = vesselRef.currentStage;
		persistent = vesselRef.isPersistent;
		vesselRef.protoVessel = this;
		actionGroups = new ConfigNode();
		vesselRef.ActionGroups.Save(actionGroups);
		GroupOverride = vesselRef.GroupOverride;
		OverrideDefault = new bool[vesselRef.OverrideDefault.Length];
		for (int k = 0; k < OverrideDefault.Length; k++)
		{
			OverrideDefault[k] = vesselRef.OverrideDefault[k];
		}
		OverrideActionControl = new KSPActionGroup[vesselRef.OverrideActionControl.Length];
		for (int l = 0; l < OverrideActionControl.Length; l++)
		{
			OverrideActionControl[l] = vesselRef.OverrideActionControl[l];
		}
		OverrideAxisControl = new KSPAxisGroup[vesselRef.OverrideAxisControl.Length];
		for (int m = 0; m < OverrideAxisControl.Length; m++)
		{
			OverrideAxisControl[m] = vesselRef.OverrideAxisControl[m];
		}
		OverrideGroupNames = new string[vesselRef.OverrideGroupNames.Length];
		for (int n = 0; n < OverrideGroupNames.Length; n++)
		{
			OverrideGroupNames[n] = vesselRef.OverrideGroupNames[n];
		}
		discoveryInfo = new ConfigNode();
		vesselRef.DiscoveryInfo.Save(discoveryInfo);
		flightPlan = new ConfigNode();
		if (vesselRef.PatchedConicsAttached)
		{
			vesselRef.flightPlanNode.ClearData();
			vesselRef.patchedConicSolver.Save(vesselRef.flightPlanNode);
		}
		vesselRef.flightPlanNode.CopyTo(flightPlan);
		targetInfo = new ProtoTargetInfo(vesselRef.targetObject);
		waypointInfo = new ProtoWaypointInfo(vesselRef.navigationWaypoint);
		ctrlState = new ConfigNode();
		vesselRef.ctrlState.Save(ctrlState);
		SaveVesselModules();
		vesselRef.OnSaveFlightState(vesselStateValues);
		RebuildCrewCounts();
		GameEvents.onProtoVesselSave.Fire(new GameEvents.FromToAction<ProtoVessel, ConfigNode>(this, null));
	}

	public ProtoVessel(ConfigNode node, Game st)
	{
		protoPartSnapshots = new List<ProtoPartSnapshot>();
		vesselStateValues = new Dictionary<string, KSPParseable>();
		discoveryInfo = new ConfigNode();
		flightPlan = new ConfigNode();
		targetInfo = new ProtoTargetInfo();
		waypointInfo = new ProtoWaypointInfo();
		ctrlState = new ConfigNode();
		crew = new List<ProtoCrewMember>();
		vesselModules = null;
		GameEvents.onProtoVesselLoad.Fire(new GameEvents.FromToAction<ProtoVessel, ConfigNode>(this, node));
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "cln":
				autoClean = bool.Parse(value.value);
				break;
			case "ctrl":
				wasControllable = bool.Parse(value.value);
				break;
			case "PQSMin":
				PQSminLevel = int.Parse(value.value);
				break;
			case "root":
				rootIndex = int.Parse(value.value);
				break;
			case "alt":
				altitude = double.Parse(value.value);
				break;
			case "PQSMax":
				PQSmaxLevel = int.Parse(value.value);
				break;
			case "launchedFrom":
				launchedFrom = value.value;
				break;
			case "vesselSpawning":
				vesselSpawning = bool.Parse(value.value);
				break;
			case "lon":
				longitude = double.Parse(value.value);
				break;
			case "nrm":
				normal = KSPUtil.ParseVector3(value.value);
				break;
			case "rot":
				rotation = KSPUtil.ParseQuaternion(value.value);
				break;
			case "ref":
				refTransform = uint.Parse(value.value);
				break;
			case "type":
				vesselType = (VesselType)Enum.Parse(typeof(VesselType), value.value);
				break;
			case "lat":
				latitude = double.Parse(value.value);
				break;
			case "lct":
				launchTime = double.Parse(value.value);
				break;
			case "GroupOverride":
				GroupOverride = int.Parse(value.value);
				break;
			case "pos":
				position = KSPUtil.ParseVector3d(value.value);
				break;
			case "persistentId":
				uint.TryParse(value.value, out persistentId);
				break;
			case "OverrideGroupNames":
				OverrideGroupNames = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, ParseExtensions.ParseArray(value.value, StringSplitOptions.None));
				break;
			case "pid":
				vesselID = new Guid(value.value);
				break;
			case "clnRsn":
				autoCleanReason = value.value;
				break;
			case "CoM":
				CoM = KSPUtil.ParseVector3(value.value);
				break;
			case "skipGroundPositioningForDroppedPart":
				skipGroundPositioningForDroppedPart = bool.Parse(value.value);
				break;
			case "OverrideAxisControl":
				ParseExtensions.TryParseEnumIntArray<KSPAxisGroup>(value.value, out OverrideAxisControl);
				OverrideAxisControl = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideAxisControl);
				break;
			case "distanceTraveled":
				distanceTraveled = double.Parse(value.value);
				break;
			case "landedAt":
				landedAt = value.value;
				break;
			case "name":
				vesselName = value.value;
				break;
			case "landed":
				landed = bool.Parse(value.value);
				break;
			case "skipGroundPositioning":
				skipGroundPositioning = bool.Parse(value.value);
				break;
			case "stg":
				stage = int.Parse(value.value);
				break;
			case "altDispState":
				node.TryGetEnum("altDispState", ref altimeterDisplayState, AltimeterDisplayState.DEFAULT);
				break;
			case "sit":
				situation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), value.value);
				break;
			case "splashed":
				splashed = bool.Parse(value.value);
				break;
			case "displaylandedAt":
				displaylandedAt = value.value;
				break;
			case "met":
				missionTime = double.Parse(value.value);
				break;
			case "lastUT":
				lastUT = double.Parse(value.value);
				break;
			case "OverrideActionControl":
				ParseExtensions.TryParseEnumIntArray<KSPActionGroup>(value.value, out OverrideActionControl);
				OverrideActionControl = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideActionControl);
				break;
			case "hgt":
				height = float.Parse(value.value);
				break;
			case "OverrideDefault":
				ParseExtensions.TryParseBoolArray(value.value, out OverrideDefault);
				OverrideDefault = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideDefault);
				break;
			default:
				vesselStateValues.Add(value.name, new KSPParseable(value.value));
				break;
			case "prst":
				persistent = bool.Parse(value.value);
				break;
			}
		}
		if (OverrideDefault == null)
		{
			OverrideDefault = new bool[Vessel.NumOverrideGroups];
		}
		if (OverrideActionControl == null)
		{
			OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		}
		if (OverrideAxisControl == null)
		{
			OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		}
		if (OverrideGroupNames == null)
		{
			OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		}
		persistentId = FlightGlobals.CheckVesselpersistentId(persistentId, null, removeOldId: false, addNewId: true);
		for (int j = 0; j < node.nodes.Count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			switch (configNode.name)
			{
			case "PART":
				protoPartSnapshots.Add(new ProtoPartSnapshot(configNode, this, st));
				break;
			case "WAYPOINT":
				waypointInfo = new ProtoWaypointInfo(configNode);
				break;
			case "VESSELMODULES":
				vesselModules = configNode;
				break;
			case "DISCOVERY":
				discoveryInfo = configNode;
				break;
			case "CTRLSTATE":
				ctrlState = configNode;
				break;
			case "FLIGHTPLAN":
				flightPlan = configNode;
				break;
			case "TARGET":
				targetInfo = new ProtoTargetInfo(configNode);
				break;
			case "ACTIONGROUPS":
				actionGroups = configNode;
				break;
			case "ORBIT":
				orbitSnapShot = new OrbitSnapshot(configNode);
				break;
			}
		}
		RebuildCrewCounts();
	}

	public void verifyCrew()
	{
		int count = crew.Count;
		while (count-- > 0)
		{
			if (crew[count] == null)
			{
				crew.RemoveAt(count);
			}
		}
	}

	public List<ProtoCrewMember> GetVesselCrew()
	{
		verifyCrew();
		return crew;
	}

	public void RebuildCrewCounts()
	{
		crewableParts = 0;
		crewedParts = 0;
		int count = protoPartSnapshots.Count;
		while (count-- > 0)
		{
			ProtoPartSnapshot protoPartSnapshot = protoPartSnapshots[count];
			if (protoPartSnapshot.protoModuleCrew.Count > 0)
			{
				crewableParts++;
				crewedParts++;
			}
			else if (protoPartSnapshot.partPrefab != null && protoPartSnapshot.partPrefab.CrewCapacity > 0)
			{
				crewableParts++;
			}
		}
		if (vesselRef != null)
		{
			vesselRef.crewedParts = crewedParts;
			vesselRef.crewableParts = crewableParts;
		}
	}

	public void AddCrew(ProtoCrewMember pcm)
	{
		crew.Add(pcm);
		RebuildCrewCounts();
	}

	public bool RemoveCrew(ProtoCrewMember pcm)
	{
		RebuildCrewCounts();
		return crew.Remove(pcm);
	}

	public string GetDisplayName()
	{
		return Localizer.Format(vesselName);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("pid", vesselID.ToString("N"));
		node.AddValue("persistentId", persistentId);
		node.AddValue("name", vesselName);
		node.AddValue("type", vesselType);
		node.AddValue("sit", situation);
		node.AddValue("landed", landed);
		node.AddValue("skipGroundPositioning", skipGroundPositioning);
		node.AddValue("skipGroundPositioningForDroppedPart", skipGroundPositioningForDroppedPart);
		node.AddValue("vesselSpawning", vesselSpawning);
		node.AddValue("launchedFrom", launchedFrom);
		node.AddValue("landedAt", landedAt);
		node.AddValue("displaylandedAt", displaylandedAt);
		node.AddValue("splashed", splashed);
		node.AddValue("met", missionTime);
		node.AddValue("lct", launchTime);
		node.AddValue("lastUT", lastUT);
		node.AddValue("distanceTraveled", distanceTraveled);
		if (autoClean)
		{
			node.AddValue("cln", value: true);
			if (autoCleanReason != string.Empty)
			{
				node.AddValue("clnRsn", autoCleanReason);
			}
		}
		node.AddValue("root", rootIndex);
		node.AddValue("lat", latitude);
		node.AddValue("lon", longitude);
		node.AddValue("alt", altitude);
		node.AddValue("hgt", height);
		node.AddValue("nrm", KSPUtil.WriteVector(normal));
		node.AddValue("rot", KSPUtil.WriteQuaternion(rotation));
		node.AddValue("CoM", KSPUtil.WriteVector(CoM));
		node.AddValue("stg", stage);
		node.AddValue("prst", persistent);
		node.AddValue("ref", refTransform);
		node.AddValue("ctrl", wasControllable);
		node.AddValue("PQSMin", PQSminLevel);
		node.AddValue("PQSMax", PQSmaxLevel);
		node.AddValue("GroupOverride", GroupOverride);
		node.AddValue("OverrideDefault", ConfigNode.WriteBoolArray(OverrideDefault));
		node.AddValue("OverrideActionControl", ConfigNode.WriteEnumIntArray(OverrideActionControl));
		node.AddValue("OverrideAxisControl", ConfigNode.WriteEnumIntArray(OverrideAxisControl));
		node.AddValue("OverrideGroupNames", ConfigNode.WriteStringArray(OverrideGroupNames));
		node.AddValue("altDispState", altimeterDisplayState);
		Dictionary<string, KSPParseable>.Enumerator enumerator = vesselStateValues.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, KSPParseable> current = enumerator.Current;
			node.AddValue(current.Key, current.Value.value);
		}
		orbitSnapShot.Save(node.AddNode("ORBIT"));
		for (int i = 0; i < protoPartSnapshots.Count; i++)
		{
			protoPartSnapshots[i].Save(node.AddNode("PART"));
		}
		ConfigNode node2 = node.AddNode("ACTIONGROUPS");
		actionGroups.CopyTo(node2);
		ConfigNode node3 = node.AddNode("DISCOVERY");
		discoveryInfo.CopyTo(node3);
		ConfigNode node4 = node.AddNode("FLIGHTPLAN");
		flightPlan.CopyTo(node4);
		ConfigNode configNode = new ConfigNode("TARGET");
		targetInfo.Save(configNode);
		if (configNode.HasData)
		{
			node.AddNode(configNode);
		}
		if (waypointInfo.navigationId != Guid.Empty)
		{
			ConfigNode node5 = new ConfigNode("WAYPOINT");
			waypointInfo.Save(node5);
			node.AddNode(node5);
		}
		ConfigNode node6 = node.AddNode("CTRLSTATE");
		ctrlState.CopyTo(node6);
		SaveVesselModules();
		ConfigNode node7 = node.AddNode("VESSELMODULES");
		vesselModules.CopyTo(node7);
		GameEvents.onProtoVesselSave.Fire(new GameEvents.FromToAction<ProtoVessel, ConfigNode>(this, node));
	}

	public void SaveVesselModules()
	{
		if (vesselModules == null)
		{
			vesselModules = new ConfigNode("VESSELMODULES");
		}
		if (vesselRef != null)
		{
			vesselModules.ClearData();
			for (int i = 0; i < vesselRef.vesselModules.Count; i++)
			{
				VesselModule vesselModule = vesselRef.vesselModules[i];
				ConfigNode node = vesselModules.AddNode(vesselModule.GetType().Name);
				vesselModule.Save(node);
			}
		}
	}

	public void ResetProtoPartSnapShots()
	{
		protoPartSnapshots.Clear();
		for (int i = 0; i < vesselRef.parts.Count; i++)
		{
			Part part = vesselRef.parts[i];
			if (part.State != PartStates.DEAD)
			{
				protoPartSnapshots.Add(new ProtoPartSnapshot(part, this));
			}
		}
		int j = 0;
		for (int count = protoPartSnapshots.Count; j < count; j++)
		{
			protoPartSnapshots[j].storePartRefs();
		}
	}

	public void Load(FlightState st)
	{
		Load(st, null);
	}

	public void Load(FlightState st, Vessel vessel)
	{
		GameEvents.onProtoVesselLoad.Fire(new GameEvents.FromToAction<ProtoVessel, ConfigNode>(this, null));
		List<string> list = new List<string>();
		for (int i = 0; i < protoPartSnapshots.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = protoPartSnapshots[i];
			AvailablePart partInfo = protoPartSnapshot.partInfo;
			if ((partInfo == null || partInfo.partPrefab == null) && !list.Contains(protoPartSnapshot.partName))
			{
				list.Add(protoPartSnapshot.partName);
			}
		}
		if (list.Count > 0)
		{
			string text = "";
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				text = text + list[j] + "\n";
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Vessel Loading Error", cacheAutoLOC_145488, Localizer.Format("#autoLOC_145489", GetDisplayName(), text), cacheAutoLOC_417274, persistAcrossScenes: true, HighLogic.UISkin, isModal: true, "_" + MissingPartsWindowIndex++);
			int k = 0;
			for (int count2 = crew.Count; k < count2; k++)
			{
				crew[k].StartRespawnPeriod();
			}
			return;
		}
		if (vessel == null)
		{
			vesselRef = new GameObject().AddComponent<Vessel>();
			if (vesselID == Guid.Empty)
			{
				vesselRef.id = Guid.NewGuid();
			}
			else
			{
				vesselRef.id = vesselID;
			}
			if (persistentId == 0)
			{
				persistentId = FlightGlobals.GetUniquepersistentId();
			}
			vesselRef.persistentId = persistentId;
			vesselRef.IgnoreGForces(10);
		}
		else
		{
			vesselRef = vessel;
		}
		vesselRef.name = GetDisplayName() + " (unloaded)";
		vesselRef.vesselName = vesselName;
		vesselRef.vesselType = vesselType;
		vesselRef.situation = situation;
		vesselRef.Landed = landed;
		vesselRef.skipGroundPositioning = skipGroundPositioning;
		vesselRef.skipGroundPositioningForDroppedPart = skipGroundPositioningForDroppedPart;
		vesselRef.vesselSpawning = vesselSpawning;
		vesselRef.launchedFrom = launchedFrom;
		vesselRef.Splashed = splashed;
		vesselRef.packed = true;
		vesselRef.localCoM = CoM;
		vesselRef.missionTime = missionTime;
		vesselRef.launchTime = launchTime;
		vesselRef.lastUT = lastUT;
		vesselRef.distanceTraveled = distanceTraveled;
		if (autoClean)
		{
			vesselRef.SetAutoClean(autoCleanReason);
		}
		if (vessel == null)
		{
			vesselRef.referenceTransformId = refTransform;
			vesselRef.orbitDriver = vesselRef.gameObject.AddComponent<OrbitDriver>();
			vesselRef.orbitDriver.orbit = orbitSnapShot.Load();
			vesselRef.orbitDriver.updateMode = ((!vesselRef.LandedOrSplashed) ? OrbitDriver.UpdateMode.UPDATE : OrbitDriver.UpdateMode.IDLE);
			vesselRef.latitude = latitude;
			vesselRef.longitude = longitude;
			vesselRef.altitude = altitude;
			vesselRef.heightFromTerrain = height;
			vesselRef.terrainNormal = normal;
			vesselRef.PQSminLevel = PQSminLevel;
			vesselRef.PQSmaxLevel = PQSmaxLevel;
			vesselRef.altimeterDisplayState = altimeterDisplayState;
			vesselRef.transform.position = vesselRef.orbit.referenceBody.GetWorldSurfacePosition(latitude, longitude, altitude);
			vesselRef.srfRelRotation = rotation;
			vesselRef.transform.rotation = vesselRef.orbit.referenceBody.bodyTransform.rotation * rotation;
		}
		vesselRef.currentStage = stage;
		vesselRef.isPersistent = persistent;
		vesselRef.GroupOverride = GroupOverride;
		vesselRef.OverrideDefault = new bool[OverrideDefault.Length];
		for (int l = 0; l < OverrideDefault.Length; l++)
		{
			vesselRef.OverrideDefault[l] = OverrideDefault[l];
		}
		vesselRef.OverrideActionControl = new KSPActionGroup[OverrideActionControl.Length];
		for (int m = 0; m < OverrideActionControl.Length; m++)
		{
			vesselRef.OverrideActionControl[m] = OverrideActionControl[m];
		}
		vesselRef.OverrideAxisControl = new KSPAxisGroup[OverrideAxisControl.Length];
		for (int n = 0; n < OverrideAxisControl.Length; n++)
		{
			vesselRef.OverrideAxisControl[n] = OverrideAxisControl[n];
		}
		vesselRef.OverrideGroupNames = new string[OverrideGroupNames.Length];
		for (int num = 0; num < OverrideGroupNames.Length; num++)
		{
			vesselRef.OverrideGroupNames[num] = OverrideGroupNames[num];
		}
		vesselRef.SetLandedAt(landedAt, null, displaylandedAt);
		if (actionGroups != null)
		{
			vesselRef.ActionGroups.Load(actionGroups);
		}
		if (discoveryInfo != null)
		{
			vesselRef.DiscoveryInfo.Load(discoveryInfo);
		}
		if (flightPlan != null)
		{
			vesselRef.flightPlanNode = flightPlan;
		}
		if (ctrlState != null)
		{
			vesselRef.ctrlState.Load(ctrlState);
		}
		if (vessel == null)
		{
			vesselRef.LoadVesselModules(vesselModules);
			vesselRef.StartFromBackup(this, st);
		}
		CometVessel[] components = vesselRef.GetComponents<CometVessel>();
		for (int num2 = 0; num2 < protoPartSnapshots.Count; num2++)
		{
			if (!protoPartSnapshots[num2].partName.Equals("PotatoComet") || (HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
			{
				continue;
			}
			CometVessel cometVessel = null;
			for (int num3 = 0; num3 < components.Length; num3++)
			{
				if (components[num3].cometPartId == protoPartSnapshots[num2].persistentId)
				{
					cometVessel = components[num3];
					break;
				}
			}
			if (cometVessel != null && vesselRef.mapObject != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("CometVFXController"));
				if (gameObject != null)
				{
					gameObject.transform.SetParent(vesselRef.mapObject.gameObject.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.identity;
					CometVFXController component = gameObject.GetComponent<CometVFXController>();
					if (component != null)
					{
						component.cometVessel = cometVessel;
						component.hostVessel = vesselRef;
						component.hostSM = vesselRef.mapObject.GetComponent<ScaledMovement>();
						cometVessel.cometVFX = component;
					}
				}
			}
			else
			{
				Debug.LogFormat("[ProtoVessel]: Unable to create Comet VFX.");
			}
		}
		FlightGlobals.AddVessel(vesselRef);
		vesselRef.OnLoadFlightState(vesselStateValues);
		if (vessel == null)
		{
			GameEvents.onVesselCreate.Fire(vesselRef);
		}
	}

	public void LoadObjects()
	{
		int i = 0;
		for (int count = protoPartSnapshots.Count; i < count; i++)
		{
			bool flag = i == rootIndex;
			Part part = protoPartSnapshots[i].Load(vesselRef, flag);
			if (part == null)
			{
				continue;
			}
			if (flag)
			{
				vesselRef.rootPart = part;
				if (vesselRef.isEVA)
				{
					vesselRef.evaController = vesselRef.GetComponent<KerbalEVA>();
				}
			}
			vesselRef.parts.Add(part);
		}
		GameEvents.onVesselPartCountChanged.Fire(vesselRef);
		int j = 0;
		for (int count2 = protoPartSnapshots.Count; j < count2; j++)
		{
			protoPartSnapshots[j].Init(vesselRef);
		}
		if (FlightGlobals.ActiveVessel == vesselRef)
		{
			FlightGlobals.ActiveVessel.rootPart = vesselRef.rootPart;
		}
	}

	public static ConfigNode CreateVesselNode(string vesselName, VesselType vesselType, Orbit orbit, int rootPartIndex, ConfigNode[] partNodes, params ConfigNode[] additionalNodes)
	{
		Vector3d positionAtUT = orbit.getPositionAtUT(Planetarium.GetUniversalTime());
		orbit.referenceBody.GetLatLonAlt(positionAtUT, out var lat, out var lon, out var alt);
		uint uniquepersistentId = FlightGlobals.GetUniquepersistentId();
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pid", Guid.NewGuid().ToString("N"));
		configNode.AddValue("name", vesselName);
		configNode.AddValue("type", vesselType);
		configNode.AddValue("persistentId", uniquepersistentId);
		configNode.AddValue("sit", Vessel.Situations.ORBITING);
		configNode.AddValue("landed", value: false);
		configNode.AddValue("skipGroundPositioning", value: false);
		configNode.AddValue("vesselSpawning", value: true);
		configNode.AddValue("landedAt", "");
		configNode.AddValue("splashed", value: false);
		configNode.AddValue("alt", alt);
		configNode.AddValue("lat", lat);
		configNode.AddValue("lon", lon);
		configNode.AddValue("hgt", -1f);
		configNode.AddValue("nrm", KSPUtil.WriteVector(Vector3.up));
		configNode.AddValue("rot", KSPUtil.WriteQuaternion(UnityEngine.Random.rotation));
		configNode.AddValue("met", Planetarium.GetUniversalTime());
		configNode.AddValue("lct", Planetarium.GetUniversalTime());
		configNode.AddValue("root", rootPartIndex);
		configNode.AddValue("PQSMin", 0);
		configNode.AddValue("PQSMax", 0);
		configNode.AddValue("altDispState", AltimeterDisplayState.DEFAULT);
		if (orbit != null)
		{
			configNode.AddNode(CreateOrbitNode(orbit));
		}
		int i = 0;
		for (int num = partNodes.Length; i < num; i++)
		{
			configNode.AddNode(partNodes[i]);
		}
		int j = 0;
		for (int num2 = additionalNodes.Length; j < num2; j++)
		{
			configNode.AddNode(additionalNodes[j]);
		}
		if (!configNode.HasNode("ACTIONGROUPS"))
		{
			configNode.AddNode("ACTIONGROUPS");
		}
		return configNode;
	}

	public static ConfigNode CreatePartNode(string partName, uint id, params ProtoCrewMember[] crew)
	{
		return CreatePartNode(null, partName, id, crew);
	}

	public static ConfigNode CreatePartNode(Part part, string partName, uint id, params ProtoCrewMember[] crew)
	{
		ConfigNode configNode = new ConfigNode("PART");
		Part part2 = part;
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(KSPUtil.SanitizeInstanceName(partName));
		if (part2 == null)
		{
			if (partInfoByName == null)
			{
				throw new Exception("[VesselSpawner Error]: No part in loader found with name: " + partName + ".");
			}
			part2 = partInfoByName.partPrefab;
		}
		uint uniquepersistentId = FlightGlobals.GetUniquepersistentId();
		configNode.AddValue("name", partInfoByName.name);
		configNode.AddValue("uid", id);
		configNode.AddValue("mid", id);
		configNode.AddValue("persistentId", uniquepersistentId);
		configNode.AddValue("parent", 0);
		configNode.AddValue("position", KSPUtil.WriteVector(Vector3.zero));
		configNode.AddValue("rotation", KSPUtil.WriteQuaternion(Quaternion.identity));
		configNode.AddValue("mirror", KSPUtil.WriteVector(Vector3.one));
		configNode.AddValue("srfN", "None, -1");
		configNode.AddValue("connected", value: true);
		configNode.AddValue("attached", value: true);
		configNode.AddValue("state", 0);
		configNode.AddValue("temp", part2.temperature);
		configNode.AddValue("mass", part2.mass);
		configNode.AddValue("expt", part2.explosionPotential);
		if (part2.attachNodes != null)
		{
			int i = 0;
			for (int count = part2.attachNodes.Count; i < count; i++)
			{
				AttachNode attachNode = part2.attachNodes[i];
				configNode.AddValue("attN", attachNode.id + ", -1");
			}
		}
		if (crew != null)
		{
			int j = 0;
			for (int num = crew.Length; j < num; j++)
			{
				ProtoCrewMember protoCrewMember = crew[j];
				if (protoCrewMember != null)
				{
					configNode.AddValue("crew", protoCrewMember.name);
				}
			}
		}
		if (part2.Resources != null && part2.Resources.IsValid)
		{
			int count2 = part2.Resources.Count;
			for (int k = 0; k < count2; k++)
			{
				new ProtoPartResourceSnapshot(part2.Resources[k]).Save(configNode.AddNode("RESOURCE"));
			}
		}
		return configNode;
	}

	public static ConfigNode CreateOrbitNode(Orbit orbit)
	{
		ConfigNode configNode = new ConfigNode("ORBIT");
		configNode.AddValue("SMA", orbit.semiMajorAxis);
		configNode.AddValue("ECC", orbit.eccentricity);
		configNode.AddValue("INC", orbit.inclination);
		configNode.AddValue("LPE", orbit.argumentOfPeriapsis);
		configNode.AddValue("LAN", orbit.double_0);
		configNode.AddValue("MNA", orbit.meanAnomalyAtEpoch);
		configNode.AddValue("EPH", orbit.epoch);
		configNode.AddValue("REF", FlightGlobals.Bodies.IndexOf(orbit.referenceBody));
		return configNode;
	}

	public static ConfigNode CreateDiscoveryNode(DiscoveryLevels level, UntrackedObjectClass size, double lifeTime, double maxLifeTime)
	{
		ConfigNode configNode = new ConfigNode("DISCOVERY");
		DiscoveryInfo obj = new DiscoveryInfo(null, lifeTime);
		obj.SetLevel(level);
		obj.SetUntrackedObjectSize(size);
		obj.SetLastObservedTime(Planetarium.GetUniversalTime());
		obj.SetReferenceLifetime(maxLifeTime);
		obj.Save(configNode);
		return configNode;
	}

	public static string GetSituationString(ProtoVessel pv, List<CelestialBody> bodies)
	{
		string text = "";
		if (bodies != null && bodies.Count > pv.orbitSnapShot.ReferenceBodyIndex)
		{
			text = bodies[pv.orbitSnapShot.ReferenceBodyIndex].displayName;
			switch (pv.situation)
			{
			case Vessel.Situations.SUB_ORBITAL:
				return cacheAutoLOC_145790;
			case Vessel.Situations.FLYING:
				return cacheAutoLOC_145786;
			case Vessel.Situations.LANDED:
				if (pv.landedAt == string.Empty)
				{
					return Localizer.Format("#autoLOC_6100012", text);
				}
				if (pv.displaylandedAt == string.Empty)
				{
					return Localizer.Format("#autoLOC_6100012", ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(Vessel.GetLandedAtString(pv.landedAt), formatted: true));
				}
				return Localizer.Format("#autoLOC_6100012", Localizer.Format(pv.displaylandedAt));
			case Vessel.Situations.SPLASHED:
				return cacheAutoLOC_145789;
			case Vessel.Situations.PRELAUNCH:
				return cacheAutoLOC_145785;
			default:
				return cacheAutoLOC_145793;
			case Vessel.Situations.DOCKED:
				return cacheAutoLOC_145792;
			case Vessel.Situations.ESCAPING:
				return Localizer.Format("#autoLOC_145791", text);
			case Vessel.Situations.ORBITING:
				return Localizer.Format("#autoLOC_145788", text);
			}
		}
		switch (pv.situation)
		{
		case Vessel.Situations.SUB_ORBITAL:
			return cacheAutoLOC_145805;
		case Vessel.Situations.FLYING:
			return cacheAutoLOC_145801;
		case Vessel.Situations.LANDED:
			if (pv.landedAt == string.Empty)
			{
				return cacheAutoLOC_6002161;
			}
			if (pv.displaylandedAt == string.Empty)
			{
				return Localizer.Format("#autoLOC_6100016", ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(Vessel.GetLandedAtString(pv.landedAt), formatted: true));
			}
			return Localizer.Format("#autoLOC_6100016", Localizer.Format(pv.displaylandedAt));
		case Vessel.Situations.SPLASHED:
			return cacheAutoLOC_145804;
		case Vessel.Situations.PRELAUNCH:
			return cacheAutoLOC_145800;
		default:
			return cacheAutoLOC_145808;
		case Vessel.Situations.DOCKED:
			return cacheAutoLOC_145807;
		case Vessel.Situations.ESCAPING:
			return cacheAutoLOC_145806;
		case Vessel.Situations.ORBITING:
			return cacheAutoLOC_145803;
		}
	}

	public List<ProtoPartSnapshot> GetAllProtoPartsIncludingCargo()
	{
		List<ProtoPartSnapshot> list = new List<ProtoPartSnapshot>();
		for (int i = 0; i < protoPartSnapshots.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = protoPartSnapshots[i];
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(protoPartSnapshot.partInfo.name);
			if (partInfoByName == null || string.Equals(partInfoByName.name, "kerbalEVA"))
			{
				continue;
			}
			bool flag = false;
			ConfigNode configNode = null;
			for (int j = 0; j < protoPartSnapshot.modules.Count; j++)
			{
				if (string.Equals(protoPartSnapshot.modules[j].moduleName, "ModuleInventoryPart"))
				{
					flag = true;
					configNode = protoPartSnapshot.modules[j].moduleValues;
					break;
				}
			}
			list.Add(protoPartSnapshot);
			if (!flag)
			{
				continue;
			}
			ConfigNode node = null;
			if (!configNode.TryGetNode("STOREDPARTS", ref node))
			{
				continue;
			}
			for (int k = 0; k < node.nodes.Count; k++)
			{
				ConfigNode configNode2 = node.nodes[k];
				if (configNode2 == null)
				{
					continue;
				}
				int value = 1;
				configNode2.TryGetValue("quantity", ref value);
				ConfigNode node2 = null;
				if (!configNode2.TryGetNode("PART", ref node2))
				{
					continue;
				}
				protoPartSnapshot = new ProtoPartSnapshot(node2, this, null);
				if (protoPartSnapshot != null)
				{
					for (int l = 0; l < value; l++)
					{
						list.Add(protoPartSnapshot);
					}
				}
			}
		}
		list.AddRange(GetAllProtoPartsFromCrew());
		return list;
	}

	public List<ProtoPartSnapshot> GetAllProtoPartsFromCrew()
	{
		List<ProtoPartSnapshot> list = new List<ProtoPartSnapshot>();
		for (int i = 0; i < crew.Count; i++)
		{
			ConfigNode inventoryNode = crew[i].InventoryNode;
			if (inventoryNode == null)
			{
				continue;
			}
			ConfigNode node = null;
			if (!inventoryNode.TryGetNode("STOREDPARTS", ref node))
			{
				continue;
			}
			for (int j = 0; j < node.nodes.Count; j++)
			{
				ConfigNode configNode = node.nodes[j];
				if (configNode == null)
				{
					continue;
				}
				int value = 1;
				configNode.TryGetValue("quantity", ref value);
				ConfigNode node2 = null;
				if (!configNode.TryGetNode("PART", ref node2))
				{
					continue;
				}
				ProtoPartSnapshot protoPartSnapshot = new ProtoPartSnapshot(node2, this, null);
				if (protoPartSnapshot != null)
				{
					for (int k = 0; k < value; k++)
					{
						list.Add(protoPartSnapshot);
					}
				}
			}
		}
		return list;
	}

	public void Clean(string reason = "")
	{
		if (HighLogic.CurrentGame == null)
		{
			return;
		}
		if (vesselRef != null && vesselRef.loaded)
		{
			if (!(vesselRef == FlightGlobals.ActiveVessel))
			{
				vesselRef.Unload();
				vesselRef.protoVessel.Clean(reason);
			}
			return;
		}
		CelestialBody celestialBody = FlightGlobals.Bodies[orbitSnapShot.ReferenceBodyIndex];
		bool flag = false;
		float funds = 0f;
		float science = 0f;
		float reputation = 0f;
		if (celestialBody.isHomeWorld)
		{
			funds = CurrentFunds();
			science = CurrentScience();
			reputation = CurrentReputation();
			GameEvents.onVesselRecovered.Fire(this, data1: true);
			flag = true;
			funds = Mathf.Round(CurrentFunds() - funds);
			science = Mathf.Round(CurrentScience() - science);
			reputation = Mathf.Round(CurrentReputation() - reputation);
		}
		flag = flag && (HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX);
		HighLogic.CurrentGame.flightState.protoVessels.Remove(this);
		if (vesselRef != null)
		{
			UnityEngine.Object.DestroyImmediate(vesselRef.gameObject);
		}
		string text = "Vessel " + ((vesselName == string.Empty) ? "was " : (GetDisplayName() + " was "));
		if (flag)
		{
			List<string> list = StringUtilities.FormattedCurrencies(funds, science, reputation, symbols: false, verbose: false, TransactionReasons.None, CurrencyModifierQuery.TextStyling.None);
			string text2 = StringUtilities.ThisThisAndThat(list);
			text += ((list.Count > 0) ? ("recovered for a refund of " + text2) : "removed from the game");
		}
		else
		{
			text += "removed from the game";
		}
		text += ((reason == string.Empty) ? "." : (": " + reason + "."));
		Debug.LogWarning(text);
	}

	public float CurrentFunds()
	{
		if (!(Funding.Instance != null))
		{
			return 0f;
		}
		return (float)Funding.Instance.Funds;
	}

	public float CurrentScience()
	{
		if (!(ResearchAndDevelopment.Instance != null))
		{
			return 0f;
		}
		return ResearchAndDevelopment.Instance.Science;
	}

	public float CurrentReputation()
	{
		if (!(Reputation.Instance != null))
		{
			return 0f;
		}
		return Reputation.Instance.reputation;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_145488 = Localizer.Format("#autoLOC_145488");
		cacheAutoLOC_417274 = Localizer.Format("#autoLOC_417274");
		cacheAutoLOC_145785 = Localizer.Format("#autoLOC_145785");
		cacheAutoLOC_145786 = Localizer.Format("#autoLOC_145786");
		cacheAutoLOC_145789 = Localizer.Format("#autoLOC_145789");
		cacheAutoLOC_145790 = Localizer.Format("#autoLOC_145790");
		cacheAutoLOC_145792 = Localizer.Format("#autoLOC_145792");
		cacheAutoLOC_145793 = Localizer.Format("#autoLOC_145793");
		cacheAutoLOC_145800 = Localizer.Format("#autoLOC_145800");
		cacheAutoLOC_145801 = Localizer.Format("#autoLOC_145801");
		cacheAutoLOC_6002161 = Localizer.Format("#autoLoc_6002161");
		cacheAutoLOC_145803 = Localizer.Format("#autoLOC_145803");
		cacheAutoLOC_145804 = Localizer.Format("#autoLOC_145804");
		cacheAutoLOC_145805 = Localizer.Format("#autoLOC_145805");
		cacheAutoLOC_145806 = Localizer.Format("#autoLOC_145806");
		cacheAutoLOC_145807 = Localizer.Format("#autoLOC_145807");
		cacheAutoLOC_145808 = Localizer.Format("#autoLOC_145808");
	}
}
