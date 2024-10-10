using System;
using System.Collections.Generic;
using System.IO;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class MissionImporting
{
	public static void ImportNewZips()
	{
		if (!Directory.Exists(MissionsUtils.UsersMissionsPath))
		{
			Debug.Log("[MissionImporting]:Skipping as Missions Path not yet created");
			return;
		}
		string[] files = Directory.GetFiles(MissionsUtils.UsersMissionsPath, "*.zip");
		if (files.Length >= 1)
		{
			if (!Directory.Exists(MissionsUtils.MissionImportCompletedPath))
			{
				Directory.CreateDirectory(MissionsUtils.MissionImportCompletedPath);
			}
			int length = MissionsUtils.UsersMissionsPath.Length;
			int num = 0;
			string[] array = files;
			foreach (string text in array)
			{
				ImportZip(text, MissionsUtils.MissionImportCompletedPath + text.Remove(0, length), num);
				num++;
			}
		}
	}

	public static bool ImportZip(string filePath, string moveDestination, int fileNumber)
	{
		List<string> directories;
		try
		{
			if (KSPCompression.FilesAtRoot(filePath))
			{
				Debug.LogError("[MissionImporting]:Zip file " + filePath + " has files at the root and will not be imported into the Missions Folder");
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005323"), 6f, ScreenMessageStyle.UPPER_LEFT);
				MoveImportedFile(filePath, moveDestination);
				return false;
			}
			directories = KSPCompression.GetTopLevelDirectories(filePath);
			if (directories.Count < 1)
			{
				Debug.LogError("[MissionImporting]:Zip file " + filePath + " has no directories. We wont import this into the Missions Folder");
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005323"), 6f, ScreenMessageStyle.UPPER_LEFT);
				MoveImportedFile(filePath, moveDestination);
				return false;
			}
		}
		catch (Exception ex)
		{
			ScreenMessages.PostScreenMessage("<color=red>[MissionImporting]:Zip File Parsing Failed. Check the log.</color>", 6f, ScreenMessageStyle.UPPER_LEFT);
			Debug.LogError("[MissionImporting]:Zip file " + filePath + " parsing Failed\nError: " + ex.Message);
			MoveImportedFile(filePath, moveDestination);
			return false;
		}
		string text = "";
		for (int i = 0; i < directories.Count; i++)
		{
			if (Directory.Exists(MissionsUtils.UsersMissionsPath + directories[i]))
			{
				text = text + "\t" + directories[i] + "\n";
			}
		}
		if (text != "")
		{
			string text2 = filePath.Substring(filePath.LastIndexOf("/") + 1);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ConfirmMissionOverwrite" + fileNumber, Localizer.Format("#autoLOC_8005324", text2, text), Localizer.Format("#autoLOC_8005325"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_8005326"), delegate
			{
				ImportZipConfirmed(filePath, directories, moveDestination);
			}), new DialogGUIButton(Localizer.Format("#autoLOC_8005327"), delegate
			{
				Debug.LogWarning("Zip file " + filePath + " bypassed. Moved to Imported folder");
				MoveImportedFile(filePath, moveDestination);
			})), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"));
		}
		else
		{
			ImportZipConfirmed(filePath, directories, moveDestination);
		}
		return true;
	}

	public static void ImportZipConfirmed(string filePath, List<string> directories, string moveDestination)
	{
		for (int i = 0; i < directories.Count; i++)
		{
			if (Directory.Exists(MissionsUtils.UsersMissionsPath + directories[i]))
			{
				Directory.Delete(MissionsUtils.UsersMissionsPath + directories[i], recursive: true);
			}
		}
		try
		{
			KSPCompression.DecompressFile(filePath, MissionsUtils.UsersMissionsPath);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005328"), 5f, ScreenMessageStyle.UPPER_LEFT);
			Debug.Log("[MissionImporting]:Mission File " + filePath + " imported to " + MissionsUtils.UsersMissionsPath);
			MoveImportedFile(filePath, moveDestination);
			MissionsUtils.ParseMissionPacksCFG(MissionsUtils.UsersMissionsPath);
			GameEvents.Mission.onMissionImported.Fire();
		}
		catch (Exception ex)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005323"), 6f, ScreenMessageStyle.UPPER_LEFT);
			Debug.LogError("[MissionImporting]:Zip file " + filePath + " import Failed\nError: " + ex.Message);
			MoveImportedFile(filePath, moveDestination);
		}
	}

	public static void MoveImportedFile(string filePath, string destinationPath)
	{
		if (File.Exists(destinationPath))
		{
			string text = destinationPath.Substring(destinationPath.LastIndexOf("/") + 1);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ConfirmImportedFileOverwrite", Localizer.Format("#autoLOC_8005331", text), Localizer.Format("#autoLOC_8005332"), HighLogic.UISkin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_8005333"), delegate
			{
				KSPUtil.MoveFile(filePath, destinationPath, overwrite: true);
			}), new DialogGUIButton(Localizer.Format("#autoLOC_8005334"), delegate
			{
				KSPUtil.MoveFile(filePath, KSPUtil.GenerateFilePathWithDate(destinationPath), overwrite: true);
			})), persistAcrossScenes: false, null);
		}
		else if (KSPUtil.MoveFile(filePath, destinationPath, overwrite: true))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005329"), 5f);
			Debug.Log("[MissionImporting]:Imported Mission File " + filePath + " moved to " + destinationPath);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8005330"), 10f, ScreenMessageStyle.UPPER_LEFT);
			Debug.LogWarning("[MissionImporting]:Imported Mission File Move Failed");
		}
	}
}
