using System;
using System.Collections.Generic;
using System.IO;
using ns11;
using Steamworks;
using UnityEngine;

public class KSPSteamUtils
{
	public static string steamCacheFolder = string.Empty;

	public static string SteamCacheFolder
	{
		get
		{
			if (steamCacheFolder == string.Empty)
			{
				steamCacheFolder = Path.GetFullPath(KSPUtil.ApplicationRootPath + "SteamCache/");
			}
			return steamCacheFolder;
		}
	}

	public static List<FileInfo> GatherCraftFilesFileInfo()
	{
		List<FileInfo> list = new List<FileInfo>();
		string kSPSteamWorkshopFolder = SteamManager.KSPSteamWorkshopFolder;
		if (Directory.Exists(kSPSteamWorkshopFolder))
		{
			DirectoryInfo[] directories = new DirectoryInfo(kSPSteamWorkshopFolder).GetDirectories();
			int num = directories.Length;
			for (int i = 0; i < num; i++)
			{
				FileInfo[] files = directories[i].GetFiles("*.craft");
				int num2 = files.Length;
				for (int j = 0; j < num2; j++)
				{
					list.Add(files[j]);
				}
			}
		}
		return list;
	}

	public static List<CraftEntry> GatherCraftFiles(List<CraftEntry> oldCraftEntries, Callback<CraftEntry> OnEntrySelected, bool excludeSteamUnsubscribed = false)
	{
		List<CraftEntry> list = new List<CraftEntry>();
		List<FileInfo> list2 = GatherCraftFilesFileInfo();
		for (int i = 0; i < list2.Count; i++)
		{
			CraftEntry craftEntry = null;
			try
			{
				craftEntry = CraftBrowserDialog.RemoveCreateCraftEntry(oldCraftEntries, list2[i], stock: false, OnEntrySelected, steamItem: true, null);
				craftEntry.steamItem = true;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("[KSPSteamUtils] Error loading steam subscribed craft: {0} message:- {1}", SteamManager.KSPSteamWorkshopFolder + list2[i].DirectoryName, ex.Message);
			}
			if (!(craftEntry != null))
			{
				continue;
			}
			if (excludeSteamUnsubscribed && craftEntry.steamItem)
			{
				string stateText = "";
				bool canBeUsed = false;
				bool subscribed = false;
				SteamManager.Instance.GetItemState(new PublishedFileId_t(craftEntry.GetSteamFileId()), out stateText, out canBeUsed, out subscribed);
				if (!subscribed)
				{
					craftEntry.gameObject.DestroyGameObject();
					continue;
				}
			}
			list.Add(craftEntry);
		}
		return list;
	}

	public static ulong GetSteamIDFromSteamFolder(string filePath)
	{
		ulong result = 0uL;
		if (Path.GetFullPath(filePath).StartsWith(Path.GetFullPath(SteamManager.KSPSteamWorkshopFolder)))
		{
			string fullPath = Path.GetFullPath(Path.GetDirectoryName(filePath));
			if (!string.IsNullOrEmpty(fullPath))
			{
				string[] array = fullPath.Split(Path.DirectorySeparatorChar);
				ulong.TryParse(array[array.Length - 1], out result);
			}
		}
		return result;
	}

	public static string GetSteamCacheLocation(string filePath)
	{
		filePath = Path.GetFullPath(filePath);
		return filePath.Replace(SteamManager.KSPSteamWorkshopFolder, SteamCacheFolder);
	}
}
