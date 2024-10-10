using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions.Runtime;
using ns9;
using SaveUpgradePipeline;
using UnityEngine;
using UnityEngine.Networking;

public class ShipConstruction
{
	public static List<ConfigNode> backups = new List<ConfigNode>();

	public static ConfigNode ShipConfig;

	public static VesselCrewManifest ShipManifest;

	public static EditorFacility ShipType;

	public const string saveFolder = "saves";

	public const string folderSeparator = "/";

	public const string shipsFolder = "Ships";

	public static void ClearBackups()
	{
		backups.Clear();
	}

	public static void CreateBackup(ShipConstruct ship)
	{
		ShipConfig = ship.SaveShip();
		ShipType = ship.shipFacility;
		backups.Add(ShipConfig);
	}

	public static void ShiftAndCreateBackup(ShipConstruct ship)
	{
		for (int i = 0; i < backups.Count - 1; i++)
		{
			backups[i] = backups[i + 1];
		}
		ShipConfig = ship.SaveShip();
		ShipType = ship.shipFacility;
		backups[backups.Count - 1] = ShipConfig;
	}

	public static ShipConstruct RestoreBackup(int index)
	{
		ShipConfig = backups[index];
		ShipConstruct shipConstruct = new ShipConstruct();
		shipConstruct.LoadShip(ShipConfig);
		for (int i = 0; i < shipConstruct.parts.Count; i++)
		{
			shipConstruct.parts[i].gameObject.SetLayerRecursive(0, 2097154);
		}
		ShipType = shipConstruct.shipFacility;
		return shipConstruct;
	}

	public static void DebugBackup()
	{
		int count = backups.Count;
		for (int i = 0; i < count; i++)
		{
			Debug.Log(backups[i].ToString());
		}
	}

	public static ShipConstruct LoadShip(string filePath)
	{
		ConfigNode configNode = ConfigNode.Load(filePath);
		if (configNode == null)
		{
			Debug.LogError("File '" + filePath + "' not found!");
			return null;
		}
		ShipConstruct shipConstruct = new ShipConstruct();
		if (!shipConstruct.LoadShip(configNode))
		{
			Debug.LogError("Ship file error!");
			return null;
		}
		if (shipConstruct.steamPublishedFileId == 0L)
		{
			shipConstruct.steamPublishedFileId = KSPSteamUtils.GetSteamIDFromSteamFolder(filePath);
		}
		AutoGenerateThumbnail(shipConstruct, filePath);
		ShipConfig = configNode;
		return shipConstruct;
	}

	public static ShipConstruct LoadShip()
	{
		if (ShipConfig == null)
		{
			Debug.LogError("Backup ship config not found!");
			return null;
		}
		ShipConstruct shipConstruct = new ShipConstruct();
		if (!shipConstruct.LoadShip(ShipConfig))
		{
			Debug.LogError("Ship file error!");
			return null;
		}
		return shipConstruct;
	}

	public static string SaveShip(ShipConstruct ship, string shipFilename)
	{
		string text = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "/" + shipFilename + ".craft";
		ship.SaveShip().Save(text);
		CaptureThumbnail(ship, "thumbs", HighLogic.SaveFolder + "_" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "_" + shipFilename);
		return text;
	}

	public static string SaveShip(string shipFilename)
	{
		if (ShipConfig == null)
		{
			return "";
		}
		string savePath = GetSavePath(shipFilename);
		ShipConfig.Save(savePath);
		CaptureThumbnail(EditorLogic.fetch.ship, "thumbs", HighLogic.SaveFolder + "_" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "_" + shipFilename);
		return savePath;
	}

	public static string SaveShipToPath(string shipName, string path)
	{
		if (ShipConfig == null)
		{
			return "";
		}
		shipName = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true);
		ShipConfig.Save(path + "/" + shipName + ".craft");
		string playerCraftThumbnailName = GetPlayerCraftThumbnailName(path, shipName);
		string text = "thumbs";
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			text += "/test_missions";
		}
		else if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			text += "/missions";
		}
		CaptureThumbnail(EditorLogic.fetch.ship, text, playerCraftThumbnailName);
		return path;
	}

	public static string SaveShipToPath(string gameFolder, EditorFacility editorFacility, string localPath, string shipName)
	{
		if (ShipConfig == null)
		{
			return "";
		}
		shipName = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true);
		string text = GetShipsPathFor(gameFolder, editorFacility) + "/" + localPath;
		ShipConfig.Save(text + "/" + shipName + ".craft");
		string playerCraftThumbnailName = GetPlayerCraftThumbnailName(text, shipName);
		CaptureThumbnail(EditorLogic.fetch.ship, "thumbs", playerCraftThumbnailName);
		return text;
	}

	public static string GetPlayerCraftThumbnailName(string fullpath, string craftFileName)
	{
		try
		{
			craftFileName = KSPUtil.SanitizeString(craftFileName, '_', replaceEmpty: true);
			FileInfo fileInfo = new FileInfo(fullpath + "/" + craftFileName + ".craft");
			EditorFacility facility = CheckCraftFileType(fileInfo.FullName);
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (HighLogic.CurrentGame.IsMissionMode)
			{
				if (!MissionSystem.HasMissions)
				{
					return string.Empty;
				}
				empty = MissionSystem.missions[0].MissionInfo.folderName;
				empty2 = "/" + GetShipsSubfolderFor(facility);
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GetRootCraftSavePath());
				DirectoryInfo directoryInfo2 = new DirectoryInfo(fullpath);
				DirectoryInfo directoryInfo3 = null;
				while (directoryInfo2.FullName != directoryInfo.FullName)
				{
					directoryInfo3 = directoryInfo2;
					directoryInfo2 = directoryInfo2.Parent;
				}
				empty = directoryInfo3.Name;
				DirectoryInfo parent = new DirectoryInfo(GetShipsPathFor(empty, facility)).Parent;
				empty2 = fileInfo.Directory.FullName.Substring(parent.FullName.Length);
			}
			return GetPlayerCraftThumbnailName(empty, empty2, craftFileName);
		}
		catch
		{
			return string.Empty;
		}
	}

	public static string GetPlayerCraftThumbnailName(string gameName, string subDirectoryPath, string shipName)
	{
		string text = KSPUtil.SanitizeFilename(gameName);
		subDirectoryPath = subDirectoryPath.Replace("\\n", "/n").Replace("\n", "/n").Replace("\\t", "/t")
			.Replace("\t", "/t");
		string text2 = KSPUtil.SanitizeFilename(subDirectoryPath);
		shipName = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true);
		return text + text2 + "_" + shipName;
	}

	public static string GetSavePath(string shipName)
	{
		if (string.IsNullOrEmpty(shipName))
		{
			return "";
		}
		shipName = KSPUtil.SanitizeString(shipName, '_', replaceEmpty: true);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionSystem.missions.Count > 0)
		{
			return KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "/" + shipName + ".craft";
		}
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionSystem.missions.Count > 0)
		{
			return MissionSystem.missions[0].MissionInfo.FolderPath + "Ships/" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "/" + shipName + ".craft";
		}
		return EditorDriver.DefaultCraftSavePath + "/" + shipName + ".craft";
	}

	public static string GetCurrentGameShipsPathFor(EditorFacility facility)
	{
		return GetShipsPathFor(HighLogic.SaveFolder, facility);
	}

	public static string GetShipsPathFor(string gameName, EditorFacility facility)
	{
		return GetShipsPathFor(gameName) + "Ships/" + GetShipsSubfolderFor(facility) + "/";
	}

	public static string GetShipsPathFor(string gameName)
	{
		return GetRootCraftSavePath() + "/" + gameName + "/";
	}

	public static string GetRootCraftSavePath()
	{
		return KSPUtil.ApplicationRootPath + "saves";
	}

	public static EditorFacility CheckCraftFileType(string filePath)
	{
		ConfigNode configNode = ConfigNode.Load(filePath);
		if (configNode == null)
		{
			return EditorFacility.None;
		}
		if (!configNode.HasValue("type"))
		{
			return EditorFacility.None;
		}
		string value = configNode.GetValue("type");
		if (!(value == "SPH"))
		{
			if (value == "VAB")
			{
			}
			return EditorFacility.const_1;
		}
		return EditorFacility.const_2;
	}

	public static string GetShipsSubfolderFor(EditorFacility facility)
	{
		return facility switch
		{
			EditorFacility.const_1 => "VAB", 
			EditorFacility.const_2 => "SPH", 
			_ => "", 
		};
	}

	public static void AssembleForLaunch(ShipConstruct ship, string landedAt, string displaylandedAt, string flagURL, Game sceneState, VesselCrewManifest crewManifest)
	{
		AssembleForLaunch(ship, landedAt, displaylandedAt, flagURL, sceneState, crewManifest, fromShipAssembly: true, setActiveVessel: true, isLanded: true, preCreate: false, null, orbiting: false, isSplashed: false);
	}

	public static Vessel AssembleForLaunch(ShipConstruct ship, string landedAt, string displaylandedAt, string flagURL, Game sceneState, VesselCrewManifest crewManifest, bool fromShipAssembly, bool setActiveVessel, bool isLanded, bool preCreate, Orbit orbit, bool orbiting, bool isSplashed)
	{
		Part localRoot = ship.parts[0].localRoot;
		Vessel vessel = localRoot.gameObject.GetComponent<Vessel>();
		if (vessel == null)
		{
			vessel = localRoot.gameObject.AddComponent<Vessel>();
		}
		vessel.id = Guid.NewGuid();
		vessel.vesselName = ship.shipName;
		vessel.persistentId = ship.persistentId;
		if (ship.OverrideDefault != null)
		{
			vessel.OverrideDefault = new bool[ship.OverrideDefault.Length];
			for (int i = 0; i < vessel.OverrideDefault.Length; i++)
			{
				vessel.OverrideDefault[i] = ship.OverrideDefault[i];
			}
		}
		else
		{
			vessel.OverrideDefault = new bool[Vessel.NumOverrideGroups];
		}
		if (ship.OverrideActionControl != null)
		{
			vessel.OverrideActionControl = new KSPActionGroup[ship.OverrideActionControl.Length];
			for (int j = 0; j < vessel.OverrideActionControl.Length; j++)
			{
				vessel.OverrideActionControl[j] = ship.OverrideActionControl[j];
			}
		}
		else
		{
			vessel.OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		}
		if (ship.OverrideAxisControl != null)
		{
			vessel.OverrideAxisControl = new KSPAxisGroup[ship.OverrideAxisControl.Length];
			for (int k = 0; k < vessel.OverrideAxisControl.Length; k++)
			{
				vessel.OverrideAxisControl[k] = ship.OverrideAxisControl[k];
			}
		}
		else
		{
			vessel.OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		}
		if (ship.OverrideGroupNames != null)
		{
			vessel.OverrideGroupNames = new string[ship.OverrideGroupNames.Length];
			for (int l = 0; l < vessel.OverrideGroupNames.Length; l++)
			{
				vessel.OverrideGroupNames[l] = ship.OverrideGroupNames[l];
			}
		}
		else
		{
			vessel.OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		}
		if (orbit != null)
		{
			(vessel.gameObject.GetComponent<OrbitDriver>() ?? vessel.gameObject.AddComponent<OrbitDriver>()).orbit = orbit;
		}
		vessel.Initialize(fromShipAssembly, preCreate, orbiting, setActiveVessel);
		vessel.UpdateVesselNaming();
		vessel.IgnoreGForces(10);
		if (fromShipAssembly && !string.IsNullOrEmpty(landedAt))
		{
			if (PSystemSetup.Instance.GetSpaceCenterFacility(landedAt) == null)
			{
				PSystemSetup.Instance.IsStockLaunchSite(landedAt);
			}
			vessel.vesselSpawning = true;
			vessel.launchedFrom = landedAt;
		}
		if (isLanded)
		{
			if (!isSplashed)
			{
				vessel.Landed = true;
				vessel.SetLandedAt(landedAt, null, displaylandedAt);
				vessel.skipGroundPositioning = false;
			}
			else
			{
				vessel.Landed = false;
				vessel.Splashed = true;
				vessel.skipGroundPositioning = true;
			}
		}
		else
		{
			vessel.Landed = false;
		}
		uint hashCode = (uint)Guid.NewGuid().GetHashCode();
		uint launchID = HighLogic.CurrentGame.launchID++;
		int m = 0;
		for (int count = vessel.parts.Count; m < count; m++)
		{
			Part part = vessel.parts[m];
			part.flightID = GetUniqueFlightID(sceneState.flightState);
			part.missionID = hashCode;
			part.launchID = launchID;
			part.flagURL = flagURL;
		}
		crewManifest.AssignCrewToVessel(ship);
		vessel.RebuildCrewList();
		if (preCreate)
		{
			for (int n = 0; n < vessel.protoVessel.protoPartSnapshots.Count; n++)
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[n];
				for (int num = 0; num < protoPartSnapshot.partRef.protoModuleCrew.Count; num++)
				{
					ProtoCrewMember protoCrewMember = protoPartSnapshot.partRef.protoModuleCrew[num];
					protoPartSnapshot.protoModuleCrew.Add(protoCrewMember);
					protoPartSnapshot.protoCrewNames.Add(protoCrewMember.name);
					vessel.protoVessel.AddCrew(protoCrewMember);
				}
			}
			vessel.protoVessel.RebuildCrewCounts();
		}
		if (setActiveVessel)
		{
			FlightGlobals.SetActiveVessel(vessel);
		}
		if (localRoot.isControlSource == Vessel.ControlLevel.NONE)
		{
			Part part2 = findFirstCrewablePart(ship.parts[0]);
			vessel.SetReferenceTransform(part2 ?? findFirstControlSource(vessel) ?? localRoot);
		}
		else
		{
			vessel.SetReferenceTransform(localRoot);
		}
		AnalyticsUtil.LogVesselLaunched(HighLogic.CurrentGame, vessel, landedAt, crewManifest, ship.steamPublishedFileId);
		Debug.Log("Vessel assembly complete!");
		return vessel;
	}

	public static uint GetUniqueFlightID(FlightState flightState)
	{
		uint hashCode;
		do
		{
			hashCode = (uint)Guid.NewGuid().GetHashCode();
		}
		while (flightState != null && flightState.ContainsFlightID(hashCode) && hashCode != 0);
		return hashCode;
	}

	public static Part findFirstCrewablePart(Part part)
	{
		if (part.CrewCapacity > 0 && part.protoModuleCrew.Count > 0 && part.isControlSource > Vessel.ControlLevel.NONE)
		{
			return part;
		}
		int num = 0;
		int count = part.children.Count;
		Part part2;
		while (true)
		{
			if (num < count)
			{
				part2 = findFirstCrewablePart(part.children[num]);
				if (part2 != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return part2;
	}

	public static Part findFirstPod_Placeholder(Part part)
	{
		if (part.CrewCapacity > 0 && part.isControlSource > Vessel.ControlLevel.NONE)
		{
			return part;
		}
		int num = 0;
		int count = part.children.Count;
		Part part2;
		while (true)
		{
			if (num < count)
			{
				part2 = findFirstPod_Placeholder(part.children[num]);
				if (part2 != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return part2;
	}

	public static Part findFirstControlSource(Vessel v)
	{
		int num = 0;
		int count = v.parts.Count;
		Part part;
		while (true)
		{
			if (num < count)
			{
				part = v.parts[num];
				if (part.isControlSource > Vessel.ControlLevel.NONE)
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

	public static void PutShipToGround(ShipConstruct ship, Transform spawnPoint)
	{
		PartHeightQuery partHeightQuery = new PartHeightQuery(float.MaxValue);
		int count = ship.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			partHeightQuery.lowestOnParts.Add(part, float.MaxValue);
			Collider[] componentsInChildren = part.GetComponentsInChildren<Collider>();
			int num = componentsInChildren.Length;
			for (int j = 0; j < num; j++)
			{
				Collider collider = componentsInChildren[j];
				if (collider.gameObject.layer != 21 && collider.enabled)
				{
					partHeightQuery.lowestPoint = Mathf.Min(partHeightQuery.lowestPoint, collider.bounds.min.y);
					partHeightQuery.lowestOnParts[part] = Mathf.Min(partHeightQuery.lowestOnParts[part], collider.bounds.min.y);
				}
			}
		}
		count = ship.parts.Count;
		for (int k = 0; k < count; k++)
		{
			Part part = ship[k];
			part.SendMessage("OnPutToGround", partHeightQuery, SendMessageOptions.DontRequireReceiver);
		}
		Debug.Log("putting ship to ground: " + partHeightQuery.lowestPoint);
		spawnPoint.rotation.ToAngleAxis(out var angle, out var axis);
		Vector3 translation = new Vector3(0f, 0f - partHeightQuery.lowestPoint, 0f);
		translation += spawnPoint.position;
		ship.parts[0].localRoot.transform.Translate(translation, Space.World);
		ship.parts[0].localRoot.transform.RotateAround(spawnPoint.position, axis, angle);
	}

	public static Vector3 CalculateCraftSize(ShipConstruct ship)
	{
		if (ship.parts.Count == 0)
		{
			return Vector3.zero;
		}
		return CalculateCraftSize(ship.parts, ship.parts[0]);
	}

	public static Vector3 CalculateCraftSize(List<Part> parts, Part rootPart)
	{
		if (parts.Count != 0 && !(rootPart == null))
		{
			Bounds bounds = default(Bounds);
			Vector3 orgPos = rootPart.orgPos;
			bounds.center = orgPos;
			List<Bounds> list = new List<Bounds>();
			int count = parts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = parts[i];
				if (!(part.Modules.GetModule<LaunchClamp>() != null))
				{
					Bounds[] partRendererBounds = PartGeometryUtil.GetPartRendererBounds(part);
					int num = partRendererBounds.Length;
					for (int j = 0; j < num; j++)
					{
						Bounds item = partRendererBounds[j];
						item.size *= part.boundsMultiplier;
						Vector3 size = item.size;
						item.Expand(part.GetModuleSize(size));
						list.Add(item);
					}
				}
			}
			if (list.Count < 1)
			{
				return Vector3.zero;
			}
			return PartGeometryUtil.MergeBounds(list.ToArray(), rootPart.transform.root).size;
		}
		return Vector3.zero;
	}

	public static Vector3 CalculateCraftSize(ShipTemplate ship)
	{
		if (ship.shipSize != Vector3.zero)
		{
			return ship.shipSize;
		}
		return Vector3.zero;
	}

	public static Vector3 FindCraftCenter(ShipConstruct ship)
	{
		return FindCraftCenter(ship, excludeClamps: false);
	}

	public static Vector3 FindCraftCenter(ShipConstruct ship, bool excludeClamps)
	{
		List<Bounds> list = new List<Bounds>();
		int count = ship.parts.Count;
		for (int i = 0; i < count; i++)
		{
			if (!excludeClamps || !(ship.parts[i].Modules.GetModule<LaunchClamp>() != null))
			{
				list.AddRange(PartGeometryUtil.GetPartRendererBounds(ship.parts[i]));
			}
		}
		if (list.Count < 1 && ship.parts.Count > 0)
		{
			list.AddRange(PartGeometryUtil.GetPartRendererBounds(ship.parts[0]));
		}
		return PartGeometryUtil.FindBoundsCentroid(list.ToArray(), null);
	}

	public static Vector3 FindCraftMOI(Part rootPart, Vector3 CoM)
	{
		return FindCraftMOI(rootPart, CoM, Vector3.zero);
	}

	public static Vector3 FindCraftMOI(Part part, Vector3 CoM, Vector3 vector3_0)
	{
		Vector3 vector = Quaternion.Inverse(part.localRoot.GetReferenceTransform().rotation) * (part.WCoM - CoM);
		float num = part.mass + part.GetResourceMass();
		vector3_0 += num * new Vector3(vector.y * vector.y + vector.z * vector.z, vector.x * vector.x + vector.z * vector.z, vector.x * vector.x + vector.y * vector.y);
		int i = 0;
		for (int count = part.children.Count; i < count; i++)
		{
			Part part2 = part.children[i];
			if (part2.isAttached && part2.State != PartStates.DEAD)
			{
				vector3_0 = FindCraftMOI(part2, CoM, vector3_0);
			}
		}
		return vector3_0;
	}

	[Obsolete("Use FindVesselsAtLaunchSite and RecoverVesselFromFlight instead")]
	public static bool CheckLaunchSiteClear(FlightState flightState, string launchSiteName, bool doCleanup)
	{
		int firstVesselOnPadIndex;
		return CheckLaunchSiteClear(flightState, launchSiteName, doCleanup, out firstVesselOnPadIndex);
	}

	[Obsolete("Use FindVesselsAtLaunchSite and RecoverVesselFromFlight instead")]
	public static bool CheckLaunchSiteClear(FlightState flightState, string launchSiteName, bool doCleanup, out int firstVesselOnPadIndex)
	{
		bool flag = true;
		firstVesselOnPadIndex = -1;
		if (flightState == null)
		{
			firstVesselOnPadIndex = -1;
			return true;
		}
		List<ProtoVessel> list = new List<ProtoVessel>();
		VesselType vesselType = VesselType.Debris;
		int count = flightState.protoVessels.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoVessel protoVessel = flightState.protoVessels[i];
			if (protoVessel.landedAt.Contains(launchSiteName))
			{
				if (protoVessel.vesselType > vesselType || firstVesselOnPadIndex == -1)
				{
					vesselType = protoVessel.vesselType;
					firstVesselOnPadIndex = flightState.protoVessels.IndexOf(protoVessel);
				}
				list.Add(protoVessel);
				flag = false;
			}
		}
		if (!flag && doCleanup)
		{
			Debug.Log("[Ship Assembly]: WARNING! - " + launchSiteName + " has vessels on it. - Removing before something stupid happens.");
			count = list.Count;
			for (int j = 0; j < count; j++)
			{
				ProtoVessel protoVessel = list[j];
				GameEvents.onVesselRecovered.Fire(protoVessel, data1: false);
				flightState.protoVessels.Remove(protoVessel);
			}
		}
		return flag;
	}

	public static List<ProtoVessel> FindVesselsLandedAt(FlightState flightState, string landedAt)
	{
		List<ProtoVessel> list = new List<ProtoVessel>();
		if (flightState != null)
		{
			int count = flightState.protoVessels.Count;
			for (int i = 0; i < count; i++)
			{
				if (flightState.protoVessels[i].landedAt.Contains(landedAt))
				{
					list.Add(flightState.protoVessels[i]);
				}
			}
		}
		return list;
	}

	public static void FindVesselsLandedAt(FlightState flightState, string landedAt, out int count, out string name, out int idx, out VesselType vType)
	{
		count = 0;
		name = string.Empty;
		idx = 0;
		vType = VesselType.Debris;
		bool flag = true;
		if (flightState == null)
		{
			return;
		}
		int count2 = flightState.protoVessels.Count;
		for (int i = 0; i < count2; i++)
		{
			ProtoVessel protoVessel = flightState.protoVessels[i];
			if (protoVessel.landedAt.Contains(landedAt))
			{
				count++;
				if (flag || protoVessel.vesselType > VesselType.Debris)
				{
					vType = protoVessel.vesselType;
					name = protoVessel.vesselName;
					idx = i;
					flag = false;
				}
			}
		}
	}

	[Obsolete("Use FindVesselsLandedAt instead.")]
	public static List<ProtoVessel> FindVesselsAtLaunchSite(FlightState flightState, string launchSiteName)
	{
		return FindVesselsAtLaunchSite(flightState, launchSiteName);
	}

	public static void RecoverVesselFromFlight(ProtoVessel vessel, FlightState fromState)
	{
		RecoverVesselFromFlight(vessel, fromState, quick: false);
	}

	public static void RecoverVesselFromFlight(ProtoVessel vessel, FlightState fromState, bool quick)
	{
		GameEvents.onVesselRecovered.Fire(vessel, quick);
		fromState.protoVessels.Remove(vessel);
		if (vessel.vesselRef != null)
		{
			if (vessel.vesselRef.loaded)
			{
				if (vessel.vesselRef == FlightGlobals.ActiveVessel)
				{
					Debug.LogError("[Vessel Removal]: " + vessel.GetDisplayName() + " is the active vessel. Cannot remove.");
					return;
				}
				Debug.LogWarning("[Vessel Removal]: " + vessel.GetDisplayName() + " is loaded. Unloading before removal. ");
				vessel.vesselRef.Unload();
			}
			UnityEngine.Object.DestroyImmediate(vessel.vesselRef.gameObject);
		}
		Debug.Log("Vessel " + vessel.GetDisplayName() + " recovered");
	}

	public static void AutoGenerateThumbnail(ShipConstruct newShip, string filePath)
	{
		bool isStock = false;
		bool isExpansion = false;
		if (!HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		HighLogic.fetch.StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			isStock = !filePath.Contains("/saves/") && !filePath.Contains("\\saves\\");
			if (isStock)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
				string name = new FileInfo(filePath).Directory.Name;
				isExpansion = filePath.Contains("SquadExpansion");
				string text = "";
				if (isExpansion)
				{
					string[] separator = new string[1] { "SquadExpansion" };
					string[] array = filePath.Split(separator, StringSplitOptions.None);
					int num = array[1].IndexOf(Path.DirectorySeparatorChar, 3);
					text = array[1].Substring(1, num - 1);
				}
				string text2 = (isExpansion ? (KSPExpansionsUtils.ExpansionsGameDataPath + text + "/Ships/@thumbs/" + name + "/" + fileNameWithoutExtension + ".png") : (KSPUtil.ApplicationRootPath + "Ships/@thumbs/" + name + "/" + fileNameWithoutExtension + ".png"));
				Debug.Log(("Autogen thumbnail for " + text2 + " (Stock) from " + filePath) ?? "");
				if (!File.Exists(text2) || ThumbnailIsOutdated(filePath, text2))
				{
					CaptureStockThumbnail(newShip, text2, isExpansion);
				}
			}
			else
			{
				string text3 = HighLogic.SaveFolder + "_" + GetShipsSubfolderFor(EditorDriver.editorFacility) + "_" + Path.GetFileNameWithoutExtension(filePath);
				string text4 = KSPUtil.ApplicationRootPath + "thumbs/" + text3 + ".png";
				Debug.Log(("Autogen thumbnail for " + text4 + " from " + filePath) ?? "");
				if (!File.Exists(text4) || ThumbnailIsOutdated(filePath, text4))
				{
					CaptureThumbnail(newShip, "thumbs", text3);
				}
			}
		}));
	}

	public static bool ThumbnailIsOutdated(string filepath, string thumbPath)
	{
		DateTime lastWriteTime = File.GetLastWriteTime(filepath);
		return File.GetLastWriteTime(thumbPath).CompareTo(lastWriteTime) < 0;
	}

	public static void CaptureStockThumbnail(ShipConstruct ship, string craftPath, bool expansion)
	{
		if (ship != null && ship.parts != null && ship.parts.Count > 0)
		{
			ship.parts[0].HighlightRecursive(active: false);
		}
		EditorFacility editorFacility = EditorDriver.editorFacility;
		if (editorFacility != EditorFacility.const_1 && editorFacility == EditorFacility.const_2)
		{
			CraftThumbnail.TakeStockSnaphot(ship, 256, "SPH", expansion, craftPath, 35f, 135f, 35f, 135f, 0.9f);
		}
		else
		{
			CraftThumbnail.TakeStockSnaphot(ship, 256, "VAB", expansion, craftPath, 45f, 45f, 45f, 45f, 0.9f);
		}
	}

	public static void CaptureThumbnail(ShipConstruct ship, string baseFolder, string thumbURL)
	{
		if (ship != null && ship.parts != null && ship.parts.Count > 0)
		{
			ship.parts[0].HighlightRecursive(active: false);
		}
		EditorFacility editorFacility = EditorDriver.editorFacility;
		if (editorFacility != EditorFacility.const_1 && editorFacility == EditorFacility.const_2)
		{
			CraftThumbnail.TakeSnaphot(ship, 256, baseFolder, thumbURL, 35f, 135f, 35f, 135f, 0.9f);
		}
		else
		{
			CraftThumbnail.TakeSnaphot(ship, 256, baseFolder, thumbURL, 45f, 45f, 45f, 45f, 0.9f);
		}
	}

	public static Texture2D GetThumbnail(string thumbURL)
	{
		return GetThumbnail(thumbURL, fullPath: false, addFileExt: true);
	}

	public static Texture2D GetThumbnail(string thumbURL, bool fullPath, bool addFileExt)
	{
		Texture2D texture2D = UnityEngine.Object.Instantiate(AssetBase.GetTexture("craftThumbGeneric"));
		string text = "";
		text = ((!fullPath) ? (KSPUtil.ApplicationRootPath + thumbURL) : thumbURL);
		if (addFileExt)
		{
			text += ".png";
		}
		if (File.Exists(text))
		{
			HighLogic.fetch.StartCoroutine(LoadThumbnail(texture2D, "file:///" + Uri.EscapeDataString(text)));
		}
		else
		{
			Debug.LogWarning("[ShipConstruction]: No thumbnail image exists for " + thumbURL);
		}
		return texture2D;
	}

	public static Texture2D GetThumbnail(string thumbURL, bool fullPath, bool addFileExt, FileInfo fInfo)
	{
		Texture2D texture2D = UnityEngine.Object.Instantiate(AssetBase.GetTexture("craftThumbGeneric"));
		string text = "";
		text = ((!fullPath) ? (KSPUtil.ApplicationRootPath + thumbURL) : thumbURL);
		if (addFileExt)
		{
			text += ".png";
		}
		if (File.Exists(text))
		{
			HighLogic.fetch.StartCoroutine(LoadThumbnail(texture2D, "file:///" + Uri.EscapeDataString(text)));
		}
		else
		{
			Debug.Log("[ShipConstruction]: No thumbnail image exists for " + thumbURL + ". Attempting loading stock thumbnails.");
			string fullName = fInfo.FullName;
			string name = new FileInfo(fullName).Directory.Name;
			string text2 = "";
			text2 = KSPUtil.ApplicationRootPath + "Ships/@thumbs/" + name + "/" + Path.GetFileNameWithoutExtension(fullName);
			text2 += ".png";
			if (File.Exists(text2))
			{
				HighLogic.fetch.StartCoroutine(LoadThumbnail(texture2D, "file:///" + Uri.EscapeDataString(text2)));
				return texture2D;
			}
		}
		return texture2D;
	}

	public static IEnumerator LoadThumbnail(Texture2D tex, string url)
	{
		using UnityWebRequest texLoad = UnityWebRequestTexture.GetTexture(url);
		yield return texLoad.SendWebRequest();
		while (!texLoad.isDone)
		{
			yield return null;
		}
		if (!texLoad.isNetworkError && !texLoad.isHttpError && string.IsNullOrEmpty(texLoad.error))
		{
			byte[] data = DownloadHandlerTexture.GetContent(texLoad).EncodeToPNG();
			tex.LoadImage(data);
			yield break;
		}
		Debug.LogError("LoadThumbnail - WWW error in " + url + " : " + texLoad.error);
	}

	public static ShipTemplate LoadTemplate(string shipFilename)
	{
		ConfigNode configNode = ConfigNode.Load(shipFilename);
		if (configNode == null)
		{
			Debug.LogError("File '" + shipFilename + "' not found!");
			return null;
		}
		ShipTemplate shipTemplate = new ShipTemplate();
		shipTemplate.LoadShip(shipFilename, configNode);
		return shipTemplate;
	}

	public static string SaveSubassembly(ShipConstruct ship, string shipFilename)
	{
		string text = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Subassemblies/";
		Directory.CreateDirectory(text);
		string text2 = text + shipFilename + ".craft";
		ship.SaveShip().Save(text2);
		return text2;
	}

	public static bool SubassemblyExists(string shipFilename)
	{
		string text = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Subassemblies/";
		Directory.CreateDirectory(text);
		return File.Exists(text + shipFilename + ".craft");
	}

	public static ShipConstruct LoadSubassembly(string shipFilename)
	{
		ConfigNode configNode = ConfigNode.Load(shipFilename);
		if (configNode == null)
		{
			Debug.LogError("File '" + shipFilename + "' not found!");
			return null;
		}
		ShipConstruct shipConstruct = new ShipConstruct();
		if (!shipConstruct.LoadShip(configNode))
		{
			Debug.LogError("Ship file error!");
			return null;
		}
		if (shipConstruct.steamPublishedFileId == 0L)
		{
			shipConstruct.steamPublishedFileId = KSPSteamUtils.GetSteamIDFromSteamFolder(shipFilename);
		}
		return shipConstruct;
	}

	public static Texture2D CreateSubassemblyIcon(ShipConstruct sa, int size)
	{
		GameObject gameObject = new GameObject("SAParent");
		int count = sa.parts.Count;
		for (int i = 0; i < count; i++)
		{
			sa.parts[i].transform.parent = gameObject.transform;
		}
		StripPartComponents(gameObject);
		SetLayerRecursively(gameObject, LayerMask.NameToLayer("UIAdditional"));
		Bounds sABounds = GetSABounds(gameObject);
		float num = 1f / Mathf.Max(Mathf.Abs(sABounds.size.x), Mathf.Max(Mathf.Abs(sABounds.size.y), Mathf.Abs(sABounds.size.z)));
		GameObject gameObject2 = new GameObject("SAParentParent");
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localScale = Vector3.one * num;
		gameObject.transform.localPosition = sABounds.center * (0f - num);
		GameObject gameObject3 = new GameObject();
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = new Vector3(0f, 0f, -10f);
		gameObject3.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.forward);
		Camera camera = gameObject3.AddComponent<Camera>();
		camera.cullingMask = 1 << LayerMask.NameToLayer("UIAdditional");
		camera.clearFlags = CameraClearFlags.Depth;
		camera.orthographic = true;
		camera.orthographicSize = 0.5f;
		Texture2D result = RenderCamera(camera, size, size, 24, RenderTextureReadWrite.Default);
		SetLayerRecursively(gameObject2, LayerMask.NameToLayer("Part Triggers"));
		gameObject.transform.localPosition = new Vector3(0f, 1000f, 0f);
		gameObject2.SetActive(value: false);
		UnityEngine.Object.DestroyImmediate(gameObject2);
		return result;
	}

	public static Texture2D RenderCamera(Camera cam, int width, int height, int depth, RenderTextureReadWrite rtReadWrite)
	{
		RenderTexture renderTexture = new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32, rtReadWrite);
		renderTexture.Create();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		cam.targetTexture = renderTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: true);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
		texture2D.Apply();
		RenderTexture.active = active;
		cam.targetTexture = null;
		renderTexture.Release();
		UnityEngine.Object.DestroyImmediate(renderTexture);
		renderTexture = null;
		return texture2D;
	}

	public static void StripPartComponents(GameObject newPart)
	{
		newPart.SetActive(value: true);
		PartLoader.StripComponent<Part>(newPart);
		PartLoader.StripComponent<PartModule>(newPart);
		PartLoader.StripComponent<EffectBehaviour>(newPart);
		PartLoader.StripGameObject<Collider>(newPart, "collider");
		PartLoader.StripComponent<Collider>(newPart);
		PartLoader.StripComponent<WheelCollider>(newPart);
		PartLoader.StripComponent<SmokeTrailControl>(newPart);
		PartLoader.StripComponent<ParticleSystem>(newPart);
		PartLoader.StripComponent<Light>(newPart);
		PartLoader.StripComponent<Animation>(newPart);
		PartLoader.StripComponent<DAE>(newPart);
		PartLoader.StripComponent<MeshRenderer>(newPart, "Icon_Hidden");
		PartLoader.StripComponent<MeshFilter>(newPart, "Icon_Hidden");
	}

	public static Bounds GetSABounds(GameObject sc)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		MeshFilter[] componentsInChildren = sc.GetComponentsInChildren<MeshFilter>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			MeshFilter meshFilter = componentsInChildren[i];
			if (!(meshFilter.mesh == null))
			{
				Matrix4x4 localToWorldMatrix = meshFilter.transform.localToWorldMatrix;
				Bounds bounds = default(Bounds);
				bounds.center = localToWorldMatrix.MultiplyPoint(meshFilter.mesh.bounds.center);
				bounds.size = localToWorldMatrix.MultiplyVector(meshFilter.mesh.bounds.size);
				if (flag)
				{
					result.Encapsulate(bounds);
					continue;
				}
				result = new Bounds(bounds.center, bounds.size);
				flag = true;
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = sc.GetComponentsInChildren<SkinnedMeshRenderer>();
		num = componentsInChildren2.Length;
		for (int j = 0; j < num; j++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren2[j];
			if (!(skinnedMeshRenderer.sharedMesh == null))
			{
				Matrix4x4 localToWorldMatrix2 = skinnedMeshRenderer.transform.localToWorldMatrix;
				Bounds bounds2 = default(Bounds);
				bounds2.center = localToWorldMatrix2.MultiplyPoint(skinnedMeshRenderer.sharedMesh.bounds.center);
				bounds2.size = localToWorldMatrix2.MultiplyVector(skinnedMeshRenderer.sharedMesh.bounds.size);
				if (flag)
				{
					result.Encapsulate(bounds2);
					continue;
				}
				result = new Bounds(bounds2.center, bounds2.size);
				flag = true;
			}
		}
		return result;
	}

	public static void SetLayerRecursively(GameObject root, int layer)
	{
		root.layer = layer;
		for (int i = 0; i < root.transform.childCount; i++)
		{
			SetLayerRecursively(root.transform.GetChild(i).gameObject, layer);
		}
	}

	public static void CreateConstructFromTemplate(ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		KSPUpgradePipeline.Process(template.config, template.shipName, LoadContext.Craft, delegate(ConfigNode n)
		{
			OnPipelineFinished(n, template, onComplete);
		}, delegate(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n)
		{
			OnPipelineFailed(opt, n, template, onComplete);
		});
	}

	public static void OnPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		if (opt != 0 && opt == KSPUpgradePipeline.UpgradeFailOption.LoadAnyway)
		{
			OnPipelineFinished(n, template, onComplete);
		}
	}

	public static void OnPipelineFinished(ConfigNode node, ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		if (node != template.config)
		{
			template.config.Save(template.filename + ".original");
			node.Save(template.filename);
			template.config = node;
		}
		ShipConstruct shipConstruct = new ShipConstruct();
		if (!shipConstruct.LoadShip(template.config))
		{
			onComplete(null);
			return;
		}
		int count = shipConstruct.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = shipConstruct.parts[i];
			part.craftID = (uint)part.GetInstanceID();
		}
		onComplete(shipConstruct);
	}

	public static void SanitizeCraftIDs(List<Part> parts, bool preserveIDsOnGivenParts)
	{
		List<Part> list = new List<Part>(UnityEngine.Object.FindObjectsOfType<Part>());
		int count = list.Count;
		while (count-- > 0)
		{
			if (!list[count].isActiveAndEnabled)
			{
				list.RemoveAt(count);
			}
		}
		int num = list.Count - 1;
		int count2 = parts.Count;
		while (count2-- > 0)
		{
			Part part = parts[count2];
			for (int num2 = num; num2 >= 0; num2--)
			{
				Part part2 = list[num2];
				if (!(part == part2) && part.craftID == part2.craftID)
				{
					if (preserveIDsOnGivenParts)
					{
						part2.craftID = (uint)part2.GetInstanceID();
					}
					else
					{
						part.craftID = (uint)part.GetInstanceID();
					}
					if (preserveIDsOnGivenParts)
					{
						Debug.LogWarning("Part " + part2.name + "(p2) craft ID reset as it matched that of " + part.name + "(p1). New id: " + part2.craftID + ".", part2);
					}
					else
					{
						Debug.LogWarning("Part " + part.name + "(p1) craft ID reset as it matched that of " + part2.name + "(p2) [" + part2.craftID + "]. New id: " + part.craftID + ".", part);
					}
				}
			}
		}
	}

	public static float GetPartCosts(ProtoPartSnapshot protoPart, AvailablePart aP, out float dryCost, out float fuelCost)
	{
		return GetPartCosts(protoPart, includeModuleCosts: true, aP, out dryCost, out fuelCost);
	}

	public static float GetPartCosts(ProtoPartSnapshot protoPart, bool includeModuleCosts, AvailablePart aP, out float dryCost, out float fuelCost)
	{
		dryCost = (includeModuleCosts ? (aP.cost + protoPart.moduleCosts) : aP.cost);
		fuelCost = 0f;
		double num = dryCost;
		double num2 = 0.0;
		if (aP == null)
		{
			string name = protoPart.partInfo.name;
			aP = PartLoader.getPartInfoByName(name);
			if (aP == null)
			{
				Debug.LogError("[ShipConstruct]: No AvailablePart found for " + name);
				return 0f;
			}
		}
		int count = protoPart.resources.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoPartResourceSnapshot protoPartResourceSnapshot = protoPart.resources[i];
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(protoPartResourceSnapshot.resourceName);
			if (definition != null)
			{
				num -= (double)definition.unitCost * protoPartResourceSnapshot.maxAmount;
				num2 += (double)definition.unitCost * protoPartResourceSnapshot.amount;
			}
			else
			{
				Debug.LogError("[ShipTemplate]: No Resource definition found for " + protoPartResourceSnapshot.resourceName);
			}
		}
		dryCost = (float)num;
		fuelCost = (float)num2;
		return (float)(num + num2);
	}

	public static float GetPartCostsAndMass(ConfigNode partNode, AvailablePart aP, out float dryCost, out float fuelCost, out float dryMass, out float fuelMass)
	{
		dryCost = 0f;
		fuelCost = 0f;
		dryMass = 0f;
		fuelMass = 0f;
		if (aP == null)
		{
			string value = partNode.GetValue("part");
			value = value.Substring(0, value.IndexOf('_'));
			aP = PartLoader.getPartInfoByName(value);
			if (aP == null)
			{
				Debug.LogError("[ShipConstruct]: No AvailablePart found for " + value);
				return 0f;
			}
		}
		dryCost = aP.cost;
		if (aP.partPrefab != null)
		{
			dryMass = aP.partPrefab.mass;
		}
		if (partNode.HasValue("modCost"))
		{
			dryCost += float.Parse(partNode.GetValue("modCost"));
		}
		if (partNode.HasValue("modMass"))
		{
			dryMass += float.Parse(partNode.GetValue("modMass"));
		}
		int count = partNode.nodes.Count;
		while (count-- > 0)
		{
			ConfigNode configNode = partNode.nodes[count];
			if (!(configNode.name != "RESOURCE"))
			{
				PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(configNode.GetValue("name"));
				if (definition != null && configNode.HasValue("amount"))
				{
					float num = float.Parse(configNode.GetValue("amount"));
					float num2 = float.Parse(configNode.GetValue("maxAmount"));
					dryCost -= definition.unitCost * num2;
					fuelCost += definition.unitCost * num;
					fuelMass += definition.density * num;
				}
				else
				{
					Debug.LogError("[ShipTemplate]: No Resource definition found for " + configNode.name);
				}
			}
		}
		return dryCost + fuelCost;
	}

	public static void SanitizePartCosts(AvailablePart aP, ConfigNode partNode)
	{
		float dryCost = aP.cost;
		float fuelCost = 0f;
		GetPartCostsAndMass(partNode, aP, out dryCost, out fuelCost, out var _, out var _);
		if (dryCost < 0f)
		{
			Debug.LogWarning("[ShipConstruct for " + aP.name + "]: part cost (" + aP.cost.ToString("0.0") + ") is less than the cost of its resources (" + fuelCost.ToString("0.0") + ")");
			aP.cost = fuelCost;
		}
	}

	public static Part FindPartWithCraftID(uint craftID)
	{
		return EditorLogic.fetch.ship.parts.Find((Part p) => p.craftID == craftID);
	}

	public static bool AllPartsFound(ConfigNode root, ref string error)
	{
		if (root != null && error != null)
		{
			ConfigNode[] nodes = root.GetNodes("PART");
			string empty = string.Empty;
			bool result = true;
			for (int i = 0; i < nodes.Length; i++)
			{
				empty = nodes[i].GetValue("part");
				if (empty != null)
				{
					empty = empty.Substring(0, empty.IndexOf('_'));
					if (!PartLoader.DoesPartExist(empty) && !PartLoader.DoesPartHaveReplacement(empty) && !PartLoader.DoesPartExist(PartLoader.GetPartReplacementName(empty)))
					{
						string value = root.GetValue("ship");
						error += Localizer.Format("#autoLOC_8004243", value, empty);
						result = false;
					}
				}
			}
			return result;
		}
		return false;
	}

	public static bool IsCompatible(ConfigNode root, ref string error)
	{
		if (root != null && error != null)
		{
			if (KSPUtil.CheckVersion(root.GetValue("version"), ShipConstruct.lastCompatibleMajor, ShipConstruct.lastCompatibleMinor, ShipConstruct.lastCompatibleRev) != VersionCompareResult.COMPATIBLE)
			{
				string value = root.GetValue("ship");
				error += Localizer.Format("#autoLOC_8004244", value);
				return false;
			}
			return true;
		}
		return false;
	}
}
