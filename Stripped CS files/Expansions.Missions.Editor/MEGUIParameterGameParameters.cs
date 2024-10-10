using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_GameParameters]
public class MEGUIParameterGameParameters : MEGUIParameter, IMEHistoryTarget
{
	public Button openDifficultySettings;

	public DifficultyOptionsMenu difficultyPrefab;

	public override bool IsInteractable
	{
		get
		{
			return openDifficultySettings.interactable;
		}
		set
		{
			openDifficultySettings.interactable = value;
		}
	}

	public GameParameters FieldValue
	{
		get
		{
			return (GameParameters)field.GetValue();
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChanged);
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		openDifficultySettings.onClick.AddListener(OnDifficultySettingsOpen);
	}

	public override void ResetDefaultValue(string value)
	{
		FieldValue = new GameParameters();
	}

	public void OnDifficultySettingsOpen()
	{
		if (MissionEditorLogic.Instance.EditorMission.MissionInfo != null && MissionEditorLogic.Instance.EditorMission.MissionInfo.FileInfoObject != null && !string.IsNullOrEmpty(MissionEditorLogic.Instance.MissionNameAtLastSave))
		{
			OpenDifficultySettings();
		}
		else
		{
			MissionEditorLogic.Instance.SaveMission(OpenDifficultySettings);
		}
	}

	public void OpenDifficultySettings()
	{
		MissionEditorLogic.Instance.Lock(lockTopRight: true, lockSAPGAP: true, lockTool: true, lockKeystrokes: true, lockNodeCanvas: true, "missionBuilder_difficulty");
		difficultyPrefab = DifficultyOptionsMenu.Create(Game.Modes.MISSION, FieldValue, newGame: true, OnGameParametersChanged);
	}

	public void OnGameParametersChanged(GameParameters settings, bool changed)
	{
		if (changed)
		{
			FieldValue = settings;
		}
		if (settings.Difficulty.AllowStockVessels)
		{
			CopyDeleteStockCraft("VAB", copy: true);
			CopyDeleteStockCraft("SPH", copy: true);
			MissionEditorLogic.Instance.EditorMission.RebuildCraftFileList();
			GameEvents.Mission.onBuilderMissionDifficultyChanged.Fire();
		}
		else
		{
			CopyDeleteStockCraft("VAB", copy: false);
			CopyDeleteStockCraft("SPH", copy: false);
			MissionEditorLogic.Instance.EditorMission.RebuildCraftFileList();
			GameEvents.Mission.onBuilderMissionDifficultyChanged.Fire();
		}
		MissionEditorLogic.Instance.Unlock("missionBuilder_difficulty");
		Object.Destroy(difficultyPrefab);
	}

	public void CopyDeleteStockCraft(string folder, bool copy)
	{
		string stockPath = KSPUtil.ApplicationRootPath + "Ships/" + folder + "/";
		string stockPath2 = KSPUtil.ApplicationRootPath + "Ships/@thumbs/" + folder + "/";
		string stockPath3 = KSPExpansionsUtils.ExpansionsGameDataPath + "MakingHistory/Ships/" + folder + "/";
		string stockPath4 = KSPExpansionsUtils.ExpansionsGameDataPath + "MakingHistory/Ships/@thumbs/" + folder + "/";
		FileInfo[] stockFiles = getStockFiles(stockPath, "*.craft");
		FileInfo[] stockFiles2 = getStockFiles(stockPath2, "*.png");
		FileInfo[] stockFiles3 = getStockFiles(stockPath3, "*.craft");
		FileInfo[] stockFiles4 = getStockFiles(stockPath4, "*.png");
		string text = MissionEditorLogic.Instance.EditorMission.MissionInfo.FolderPath + "Ships/" + folder + "/";
		string text2 = KSPUtil.ApplicationRootPath + "thumbs/" + MissionsUtils.savesPath;
		string text3 = KSPUtil.ApplicationRootPath + "thumbs/" + MissionsUtils.testsavesPath;
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		if (!Directory.Exists(text3))
		{
			Directory.CreateDirectory(text3);
		}
		List<VesselSituation> allVesselSituations = MissionEditorLogic.Instance.EditorMission.GetAllVesselSituations();
		if (stockFiles != null)
		{
			int num = stockFiles.Length;
			for (int i = 0; i < num; i++)
			{
				if (copy)
				{
					File.Copy(stockFiles[i].FullName, text + stockFiles[i].Name, overwrite: true);
				}
				else if (File.Exists(text + stockFiles[i].Name) && !craftFileInUse(allVesselSituations, stockFiles[i].Name))
				{
					File.Delete(text + stockFiles[i].Name);
				}
			}
		}
		if (stockFiles3 != null)
		{
			int num2 = stockFiles3.Length;
			for (int j = 0; j < num2; j++)
			{
				if (copy)
				{
					File.Copy(stockFiles3[j].FullName, text + stockFiles3[j].Name, overwrite: true);
				}
				else if (File.Exists(text + stockFiles3[j].Name) && !craftFileInUse(allVesselSituations, stockFiles3[j].Name))
				{
					File.Delete(text + stockFiles3[j].Name);
				}
			}
		}
		if (stockFiles2 != null)
		{
			int num3 = stockFiles2.Length;
			for (int k = 0; k < num3; k++)
			{
				if (copy)
				{
					File.Copy(stockFiles2[k].FullName, text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles2[k].Name, overwrite: true);
					File.Copy(stockFiles2[k].FullName, text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles2[k].Name, overwrite: true);
				}
				else if (File.Exists(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles2[k].Name) && !craftFileInUse(allVesselSituations, stockFiles2[k].Name))
				{
					File.Delete(text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles2[k].Name);
					File.Delete(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles2[k].Name);
				}
			}
		}
		if (stockFiles4 != null)
		{
			int num4 = stockFiles4.Length;
			for (int l = 0; l < num4; l++)
			{
				if (copy)
				{
					File.Copy(stockFiles4[l].FullName, text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles4[l].Name, overwrite: true);
					File.Copy(stockFiles4[l].FullName, text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles4[l].Name, overwrite: true);
				}
				else if (File.Exists(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles4[l].Name) && !craftFileInUse(allVesselSituations, stockFiles2[l].Name))
				{
					File.Delete(text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles4[l].Name);
					File.Delete(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles4[l].Name);
				}
			}
		}
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return;
		}
		string stockPath5 = KSPExpansionsUtils.ExpansionsGameDataPath + "Serenity/Ships/" + folder + "/";
		string stockPath6 = KSPExpansionsUtils.ExpansionsGameDataPath + "Serenity/Ships/@thumbs/" + folder + "/";
		FileInfo[] stockFiles5 = getStockFiles(stockPath5, "*.craft");
		FileInfo[] stockFiles6 = getStockFiles(stockPath6, "*.png");
		if (stockFiles5 != null)
		{
			int num5 = stockFiles5.Length;
			for (int m = 0; m < num5; m++)
			{
				if (copy)
				{
					File.Copy(stockFiles5[m].FullName, text + stockFiles5[m].Name, overwrite: true);
				}
				else if (File.Exists(text + stockFiles5[m].Name) && !craftFileInUse(allVesselSituations, stockFiles5[m].Name))
				{
					File.Delete(text + stockFiles5[m].Name);
				}
			}
		}
		if (stockFiles6 == null)
		{
			return;
		}
		int num6 = stockFiles6.Length;
		for (int n = 0; n < num6; n++)
		{
			if (copy)
			{
				File.Copy(stockFiles6[n].FullName, text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles6[n].Name, overwrite: true);
				File.Copy(stockFiles6[n].FullName, text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles6[n].Name, overwrite: true);
			}
			else if (File.Exists(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles6[n].Name) && !craftFileInUse(allVesselSituations, stockFiles2[n].Name))
			{
				File.Delete(text2 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles6[n].Name);
				File.Delete(text3 + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName + "_" + folder + "_" + stockFiles6[n].Name);
			}
		}
	}

	public FileInfo[] getStockFiles(string stockPath, string fileType)
	{
		if (Directory.Exists(stockPath))
		{
			return new DirectoryInfo(stockPath).GetFiles(fileType);
		}
		return null;
	}

	public bool craftFileInUse(List<VesselSituation> vesselSituations, string fileName)
	{
		int num = 0;
		while (true)
		{
			if (num < vesselSituations.Count)
			{
				if (vesselSituations[num].craftFile == fileName)
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

	public void OnHistoryValueChanged(ConfigNode data, HistoryType type)
	{
		ConfigNode node = new ConfigNode();
		if (data.TryGetNode("value", ref node))
		{
			FieldValue.Load(node);
		}
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		ConfigNode node = new ConfigNode();
		FieldValue.Save(node);
		configNode.AddNode("value", node);
		return configNode;
	}
}
