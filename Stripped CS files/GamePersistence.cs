using System;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions;
using ns9;
using UnityEngine;

public class GamePersistence
{
	public static string SaveGame(string saveFileName, string saveFolder, SaveMode saveMode, GameScenes startScene)
	{
		if (HighLogic.CurrentGame == null)
		{
			HighLogic.CurrentGame = new Game().Updated(startScene);
			return SaveGame(HighLogic.CurrentGame, saveFileName, saveFolder, saveMode);
		}
		return SaveGame(HighLogic.CurrentGame.Updated(startScene), saveFileName, saveFolder, saveMode);
	}

	public static string SaveGame(string saveFileName, string saveFolder, SaveMode saveMode)
	{
		return SaveGame(saveFileName, saveFolder, saveMode, GameScenes.LOADING);
	}

	public static string SaveGame(Game game, string saveFileName, string saveFolder, SaveMode saveMode)
	{
		if (saveFileName == "persistent" && !game.Parameters.Flight.CanAutoSave)
		{
			Debug.LogWarning("[GamePersistence]: Saving to persistent.sfs is disabled in FLIGHT scenario options (disabled autosave)");
			return string.Empty;
		}
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		foreach (char oldChar in invalidFileNameChars)
		{
			saveFileName = saveFileName.Replace(oldChar, '_');
		}
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/"))
		{
			Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/");
		}
		switch (saveMode)
		{
		case SaveMode.APPEND:
		{
			int num2 = 0;
			string text4;
			do
			{
				text4 = saveFileName + num2++.ToString("000");
			}
			while (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + text4 + ".sfs"));
			saveFileName = text4;
			break;
		}
		case SaveMode.ABORT:
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + saveFileName + ".sfs"))
			{
				Debug.LogWarning("Save Aborted! File already exists!");
				return "";
			}
			break;
		case SaveMode.BACKUP:
		{
			string text = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/";
			string text2 = text + "/Backup/";
			string text3 = text + saveFileName + ".sfs";
			string destFileName = text2 + saveFileName + " (" + KSPUtil.SystemDateTime.DateTimeNow().ToString("yyyy_MM_dd_HH_mm_ss") + ").sfs";
			if (GameSettings.SAVE_BACKUPS <= 0)
			{
				if (Directory.Exists(text2))
				{
					Directory.Delete(text2, recursive: true);
				}
			}
			else
			{
				if (!File.Exists(text3))
				{
					break;
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
					File.Copy(text3, destFileName, overwrite: true);
					break;
				}
				List<FileInfo> list = new List<FileInfo>(new DirectoryInfo(text2).GetFiles("*.sfs"));
				list.Sort(saveCompareToDate);
				if (list.Count < GameSettings.SAVE_BACKUPS)
				{
					File.Copy(text3, destFileName, overwrite: true);
					break;
				}
				File.Copy(text3, destFileName, overwrite: true);
				int num = list.Count + 1 - GameSettings.SAVE_BACKUPS;
				for (int j = 0; j < num; j++)
				{
					list[j].Delete();
				}
			}
			break;
		}
		}
		ConfigNode configNode = new ConfigNode();
		game.Save(configNode);
		configNode.Save(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + saveFileName + ".sfs");
		Debug.Log("Game State Saved to saves/" + saveFolder + "/" + saveFileName);
		try
		{
			new LoadGameDialog.PlayerProfileInfo(saveFileName, saveFolder, game).SaveToMetaFile(saveFileName, saveFolder);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Unable to save .loadmeta for filename\n" + ex.Message);
		}
		return saveFileName;
	}

	public static int saveCompareToDate(FileInfo a, FileInfo b)
	{
		try
		{
			return a.LastWriteTime.CompareTo(b.LastWriteTime);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public static string SaveGame(GameBackup game, string saveFileName, string saveFolder, SaveMode saveMode)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		foreach (char oldChar in invalidFileNameChars)
		{
			saveFileName = saveFileName.Replace(oldChar, '_');
		}
		if (!Directory.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/"))
		{
			Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/");
		}
		switch (saveMode)
		{
		case SaveMode.APPEND:
		{
			int num2 = 0;
			string text4;
			do
			{
				text4 = saveFileName + num2++.ToString("000");
			}
			while (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + text4 + ".sfs"));
			saveFileName = text4;
			break;
		}
		case SaveMode.ABORT:
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + saveFileName + ".sfs"))
			{
				Debug.LogWarning("Save Aborted! File already exists!");
				return "";
			}
			break;
		case SaveMode.BACKUP:
		{
			string text = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/";
			string text2 = text + "/Backup/";
			string text3 = text + saveFileName + ".sfs";
			string destFileName = text2 + saveFileName + " (" + KSPUtil.SystemDateTime.DateTimeNow().ToString("yyyy_MM_dd_HH_mm_ss") + ").sfs";
			if (GameSettings.SAVE_BACKUPS <= 0)
			{
				if (Directory.Exists(text2))
				{
					Directory.Delete(text2, recursive: true);
				}
			}
			else
			{
				if (!File.Exists(text3))
				{
					break;
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
					File.Copy(text3, destFileName, overwrite: true);
					break;
				}
				List<FileInfo> list = new List<FileInfo>(new DirectoryInfo(text2).GetFiles("*.sfs"));
				list.Sort(saveCompareToDate);
				if (list.Count < GameSettings.SAVE_BACKUPS)
				{
					File.Copy(text3, destFileName, overwrite: true);
					break;
				}
				File.Copy(text3, destFileName, overwrite: true);
				int num = list.Count + 1 - GameSettings.SAVE_BACKUPS;
				for (int j = 0; j < num; j++)
				{
					list[j].Delete();
				}
			}
			break;
		}
		}
		game.Config.Save(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + saveFileName + ".sfs");
		Debug.Log("Game State Saved as " + saveFileName);
		return saveFileName;
	}

	public static ConfigNode LoadSFSFile(string filename, string saveFolder)
	{
		string text = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs";
		if (!File.Exists(text))
		{
			Debug.Log("No save file found for path: " + KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs");
			return null;
		}
		return ConfigNode.Load(text);
	}

	public static Game LoadGameCfg(ConfigNode node, string saveName, bool nullIfIncompatible, bool suppressIncompatibleMessage)
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Smart people usually have the game running before trying to load it.");
			return null;
		}
		FlightGlobals.ClearpersistentIdDictionaries();
		Game game = new Game(node);
		if (!game.compatible && nullIfIncompatible)
		{
			if (!suppressIncompatibleMessage)
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Load Failed", Localizer.Format("#autoLOC_6002422"), Localizer.Format("#autoLOC_6002423", saveName), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, HighLogic.UISkin);
			}
			return null;
		}
		return game;
	}

	public static Game LoadGame(string filename, string saveFolder, bool nullIfIncompatible, bool suppressIncompatibleMessage)
	{
		ConfigNode configNode = LoadSFSFile(filename, saveFolder);
		FlightGlobals.ClearpersistentIdDictionaries();
		if (configNode != null)
		{
			return LoadGameCfg(configNode, filename, nullIfIncompatible, suppressIncompatibleMessage);
		}
		return null;
	}

	public static bool UpdateScenarioModules(Game game)
	{
		bool result = false;
		List<KSPScenarioType> allScenarioTypesInAssemblies = KSPScenarioType.GetAllScenarioTypesInAssemblies();
		int count = allScenarioTypesInAssemblies.Count;
		for (int i = 0; i < count; i++)
		{
			KSPScenarioType kSPScenarioType = allScenarioTypesInAssemblies[i];
			if (game.Mode == Game.Modes.SANDBOX)
			{
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToExistingSandboxGames) && game.AddProtoScenarioModule(kSPScenarioType.ModuleType, kSPScenarioType.ScenarioAttributes.TargetScenes) != null)
				{
					result = true;
				}
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.RemoveFromSandboxGames) && game.RemoveProtoScenarioModule(kSPScenarioType.ModuleType))
				{
					result = true;
				}
			}
			else if (game.Mode == Game.Modes.SCIENCE_SANDBOX)
			{
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToExistingScienceSandboxGames) && game.AddProtoScenarioModule(kSPScenarioType.ModuleType, kSPScenarioType.ScenarioAttributes.TargetScenes) != null)
				{
					result = true;
				}
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.RemoveFromScienceSandboxGames) && game.RemoveProtoScenarioModule(kSPScenarioType.ModuleType))
				{
					result = true;
				}
			}
			else if (game.Mode == Game.Modes.CAREER)
			{
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToExistingCareerGames) && game.AddProtoScenarioModule(kSPScenarioType.ModuleType, kSPScenarioType.ScenarioAttributes.TargetScenes) != null)
				{
					result = true;
				}
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.RemoveFromCareerGames) && game.RemoveProtoScenarioModule(kSPScenarioType.ModuleType))
				{
					result = true;
				}
			}
			else if (game.Mode == Game.Modes.MISSION || game.Mode == Game.Modes.MISSION_BUILDER)
			{
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToExistingMissionGames) && game.AddProtoScenarioModule(kSPScenarioType.ModuleType, kSPScenarioType.ScenarioAttributes.TargetScenes) != null)
				{
					result = true;
				}
				if (kSPScenarioType.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.RemoveFromMissionGames) && game.RemoveProtoScenarioModule(kSPScenarioType.ModuleType))
				{
					result = true;
				}
			}
		}
		return result;
	}

	public static Game CreateNewGame(string name, Game.Modes mode, GameParameters parameters, string flagURL, GameScenes startScene, EditorFacility editorFacility)
	{
		FlightGlobals.ClearpersistentIdDictionaries();
		string text = ((mode == Game.Modes.MISSION || mode == Game.Modes.MISSION_BUILDER) ? MissionsUtils.SavesPath : "");
		string text2 = KSPUtil.ApplicationRootPath + "saves/" + text + Localizer.Format(name);
		try
		{
			Directory.CreateDirectory(text2);
		}
		catch (UnauthorizedAccessException)
		{
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
		}
		Directory.CreateDirectory(text2 + "/Ships");
		Directory.CreateDirectory(text2 + "/Ships/VAB");
		Directory.CreateDirectory(text2 + "/Ships/SPH");
		HighLogic.SaveFolder = text + Localizer.Format(name);
		Game game = new Game();
		game.Mode = mode;
		game.Parameters = parameters;
		game.Title = name + " (" + mode.ToString() + ")";
		game.Description = "No description available.";
		game.startScene = startScene;
		game.editorFacility = editorFacility;
		game.flagURL = flagURL;
		game.CrewRoster = KerbalRoster.GenerateInitialCrewRoster(mode);
		Planetarium.SetUniversalTime(0.0);
		foreach (KSPScenarioType allScenarioTypesInAssembly in KSPScenarioType.GetAllScenarioTypesInAssemblies())
		{
			switch (mode)
			{
			case Game.Modes.CAREER:
				if (allScenarioTypesInAssembly.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToNewCareerGames))
				{
					game.AddProtoScenarioModule(allScenarioTypesInAssembly.ModuleType, allScenarioTypesInAssembly.ScenarioAttributes.TargetScenes);
				}
				break;
			case Game.Modes.SCIENCE_SANDBOX:
				if (allScenarioTypesInAssembly.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToNewScienceSandboxGames))
				{
					game.AddProtoScenarioModule(allScenarioTypesInAssembly.ModuleType, allScenarioTypesInAssembly.ScenarioAttributes.TargetScenes);
				}
				break;
			case Game.Modes.SANDBOX:
				if (allScenarioTypesInAssembly.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToNewSandboxGames))
				{
					game.AddProtoScenarioModule(allScenarioTypesInAssembly.ModuleType, allScenarioTypesInAssembly.ScenarioAttributes.TargetScenes);
				}
				break;
			case Game.Modes.MISSION:
			case Game.Modes.MISSION_BUILDER:
				if (allScenarioTypesInAssembly.ScenarioAttributes.HasCreateOption(ScenarioCreationOptions.AddToNewMissionGames) && (allScenarioTypesInAssembly.ModuleType != typeof(Funding) || parameters.CustomParams<MissionParamsGeneral>().enableFunding))
				{
					game.AddProtoScenarioModule(allScenarioTypesInAssembly.ModuleType, allScenarioTypesInAssembly.ScenarioAttributes.TargetScenes);
				}
				break;
			}
		}
		SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		return game;
	}
}
