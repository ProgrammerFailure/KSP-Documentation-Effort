using System;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Runtime;
using ns11;
using Steamworks;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class MissionFileInfo
{
	public string idName;

	public string folderName;

	public MissionTypes missionType;

	public string title;

	public string briefing;

	public string author;

	public string modsBriefing;

	public string packName;

	public int order;

	public bool hardIcon;

	public MissionDifficulty difficulty = MissionDifficulty.Intermediate;

	public List<string> tags;

	public int steamState;

	public bool subscribed;

	public bool canBeUsed;

	public string steamStateText;

	public Mission simpleMissionCache;

	public MissionPlayDialog.MissionProfileInfo metaMission;

	public LoadGameDialog.PlayerProfileInfo metaSavedMission;

	public Mission SimpleMission
	{
		get
		{
			if (HighLogic.LoadedSceneIsMissionBuilder)
			{
				return MissionSystem.LoadMission(this, loadSavedMission: false, initMission: false, simple: true);
			}
			if (simpleMissionCache == null)
			{
				simpleMissionCache = MissionSystem.LoadMission(this, HasSave, initMission: false, simple: true);
			}
			return simpleMissionCache;
		}
	}

	public MissionPlayDialog.MissionProfileInfo MetaMission
	{
		get
		{
			if (metaMission == null || metaMission.id == Guid.Empty)
			{
				metaMission = MissionSystem.LoadMetaMission(this);
			}
			return metaMission;
		}
	}

	public Mission savedMission => MissionSystem.LoadMission(this, loadSavedMission: true);

	public LoadGameDialog.PlayerProfileInfo MetaSavedMission
	{
		get
		{
			if (metaSavedMission == null)
			{
				metaSavedMission = new LoadGameDialog.PlayerProfileInfo();
				metaSavedMission.name = title;
				metaSavedMission = LoadGameDialog.GetSaveData(metaSavedMission, "persistent", "missions/" + folderName);
			}
			return metaSavedMission;
		}
	}

	public bool IsTutorialMission => packName == "squad_MakingHistory_Tutorial";

	public string FolderPath => MissionsUtils.GetMissionsFolder(missionType) + folderName + "/";

	public string ShipFolderPath => FolderPath + "Ships/";

	public string FilePath => FolderPath + "persistent" + MissionsUtils.fileExtension;

	public FileInfo FileInfoObject
	{
		get
		{
			if (File.Exists(FilePath))
			{
				return new FileInfo(FilePath);
			}
			return null;
		}
	}

	public string BannerPath => FolderPath + "Banners/";

	public string SaveFolderPath => MissionsUtils.SavesRootPath + folderName + "/";

	public string SavePath => SaveFolderPath + "persistent.sfs";

	public string SaveShipFolderPath => SaveFolderPath + "Ships/";

	public bool HasSave => File.Exists(SavePath);

	public MissionFileInfo(string folderName, MissionTypes missionType)
	{
		this.folderName = folderName;
		this.missionType = missionType;
		if (MetaMission != null)
		{
			title = MetaMission.title;
			idName = MetaMission.idName;
			briefing = MetaMission.briefing;
			author = MetaMission.author;
			modsBriefing = MetaMission.modsBriefing;
			packName = MetaMission.packName;
			order = MetaMission.order;
			hardIcon = MetaMission.hardIcon;
			difficulty = MetaMission.difficulty;
			tags = MetaMission.tags;
		}
		UpdateSteamState();
	}

	public static MissionFileInfo CreateFromPath(string fullPath)
	{
		FileInfo fileInfo = new FileInfo(fullPath);
		string name = fileInfo.Directory.Name;
		MissionTypes missionTypes = MissionTypes.Stock;
		if (fileInfo.Directory.Parent.FullName == Path.GetFullPath(MissionsUtils.GetMissionsFolder(MissionTypes.User)).TrimEnd(Path.DirectorySeparatorChar))
		{
			missionTypes = MissionTypes.User;
		}
		else if (fileInfo.Directory.Parent.FullName == Path.GetFullPath(MissionsUtils.GetMissionsFolder(MissionTypes.Steam)).TrimEnd(Path.DirectorySeparatorChar))
		{
			missionTypes = MissionTypes.Steam;
		}
		else if (fileInfo.Directory.Parent.FullName == Path.GetFullPath(MissionsUtils.GetMissionsFolder(MissionTypes.Base)).TrimEnd(Path.DirectorySeparatorChar))
		{
			missionTypes = MissionTypes.Base;
		}
		return new MissionFileInfo(name, missionTypes);
	}

	public bool IsCompatible()
	{
		if (string.IsNullOrEmpty(MetaMission.missionExpansionVersion))
		{
			return false;
		}
		if (metaSavedMission != null && !MetaSavedMission.gameCompatible)
		{
			return false;
		}
		bool result = true;
		System.Version version = new System.Version(Mission.lastCompatibleVersion);
		System.Version version2 = null;
		if (metaSavedMission != null && !string.IsNullOrEmpty(metaSavedMission.missionExpansionVersion))
		{
			version2 = new System.Version(metaSavedMission.missionExpansionVersion);
		}
		else if (metaMission != null && !string.IsNullOrEmpty(metaMission.missionExpansionVersion))
		{
			version2 = new System.Version(MetaMission.missionExpansionVersion);
		}
		string text = ExpansionsLoader.GetExpansionVersion("MakingHistory");
		if (string.IsNullOrEmpty(text) && MetaMission.packName == "squad_ESA")
		{
			text = "1.10.0";
		}
		System.Version version3 = null;
		if (!string.IsNullOrEmpty(text))
		{
			version3 = new System.Version(text);
		}
		if (version2 == null || version3 == null || version2 < version || version2 > version3)
		{
			Debug.LogWarning("Mission compatibility version check failed: " + MetaMission.missionExpansionVersion + " vs " + Mission.lastCompatibleVersion + " and " + ExpansionsLoader.GetExpansionVersion("MakingHistory"));
			result = false;
		}
		List<string> list = ((metaSavedMission == null) ? MetaMission.actionModules : metaSavedMission.actionModules);
		for (int i = 0; i < list.Count; i++)
		{
			if (AssemblyLoader.GetClassByName(typeof(ActionModule), list[i]) == null)
			{
				Debug.LogWarning("Mission compatibility action module check failed: " + list[i] + " not present!");
				result = false;
			}
		}
		List<string> list2 = ((metaSavedMission == null) ? MetaMission.testModules : metaSavedMission.testModules);
		for (int j = 0; j < list2.Count; j++)
		{
			if (AssemblyLoader.GetClassByName(typeof(TestModule), list2[j]) == null)
			{
				Debug.LogWarning("Mission compatibility test module check failed: " + list2[j] + " not present!");
				result = false;
			}
		}
		return result;
	}

	public bool IsCraftCompatible(ref string errorString, ref HashSet<string> incompatibleCraftHashSet, bool checkSaveIfAvailable)
	{
		string empty = string.Empty;
		empty = ((!checkSaveIfAvailable || !HasSave) ? (ShipFolderPath + ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_1)) : (SaveShipFolderPath + ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_1)));
		FileInfo[] fileInfoList = new FileInfo[0];
		if (Directory.Exists(empty))
		{
			fileInfoList = new DirectoryInfo(empty).GetFiles("*.craft");
		}
		bool num = IsIndividualFolderCraftCompatible(fileInfoList, ref errorString, ref incompatibleCraftHashSet);
		empty = ((!checkSaveIfAvailable || !HasSave) ? (ShipFolderPath + ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_2)) : (SaveShipFolderPath + ShipConstruction.GetShipsSubfolderFor(EditorFacility.const_2)));
		if (Directory.Exists(empty))
		{
			fileInfoList = new DirectoryInfo(empty).GetFiles("*.craft");
		}
		return num & IsIndividualFolderCraftCompatible(fileInfoList, ref errorString, ref incompatibleCraftHashSet);
	}

	public bool IsIndividualFolderCraftCompatible(FileInfo[] fileInfoList, ref string errorString, ref HashSet<string> incompatibleCraftHashSet)
	{
		int num = fileInfoList.Length;
		bool result = true;
		for (int i = 0; i < num; i++)
		{
			FileInfo fileInfo = fileInfoList[i];
			try
			{
				string text = fileInfo.FullName.Replace(fileInfo.Extension, ".loadmeta");
				if (metaMission.steamPublishedFileId != 0L)
				{
					text = KSPSteamUtils.GetSteamCacheLocation(text);
				}
				CraftProfileInfo saveData = CraftProfileInfo.GetSaveData(fileInfo.FullName, text);
				if (saveData.compatibility != VersionCompareResult.COMPATIBLE || !saveData.shipPartsUnlocked || !saveData.shipPartModulesAvailable)
				{
					result = false;
					incompatibleCraftHashSet.Add(fileInfo.Name);
					errorString += saveData.GetErrorMessage();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MissionFileInfo.IsCraftCompatible] : " + ex.Message);
			}
		}
		return result;
	}

	public ulong GetSteamFileId()
	{
		ulong value = 0uL;
		if (missionType != MissionTypes.Steam)
		{
			return value;
		}
		if (File.Exists(FilePath))
		{
			ConfigNode configNode = ConfigNode.Load(FilePath);
			if (configNode != null)
			{
				ConfigNode node = new ConfigNode();
				configNode.TryGetNode("MISSION", ref node);
				node.TryGetValue("steamPublishedFileId", ref value);
				if (value == 0L)
				{
					value = KSPSteamUtils.GetSteamIDFromSteamFolder(FilePath);
				}
			}
		}
		return value;
	}

	public void UpdateSteamState()
	{
		ulong steamFileId = GetSteamFileId();
		if (SteamManager.Initialized && missionType == MissionTypes.Steam && steamFileId != 0L)
		{
			steamState = SteamManager.Instance.GetItemState(new PublishedFileId_t(steamFileId), out steamStateText, out canBeUsed, out subscribed);
			return;
		}
		steamStateText = "";
		canBeUsed = false;
		subscribed = false;
		steamState = 0;
	}
}
