using System;
using System.IO;
using Expansions.Missions;
using UnityEngine;

[Serializable]
public class MEBannerEntry
{
	public string fileName;

	public bool isInMissionFolder;

	public Texture2D texture;

	public DialogGUIToggleButton buttonReference;

	public string relativePath;

	public string missionFolderPath;

	public bool isDuplicate;

	public bool IsDuplicate
	{
		set
		{
			isDuplicate = value;
		}
	}

	public string DisplayName => string.Concat(str1: (!isDuplicate) ? (isInMissionFolder ? "<b><color=#FFA500> (Missions Folder)</b></color></b>" : "<b><color=#4DC44D> (GameData Folder)</b></color></b>") : "<b><color=#FFA500> (Missions Folder)</b></color></b><b><color=#4DC44D> (GameData Folder)</b></color></b>", str0: fileName);

	public string FullPath
	{
		get
		{
			string empty = string.Empty;
			if (isInMissionFolder && missionFolderPath != string.Empty)
			{
				return missionFolderPath + relativePath + fileName;
			}
			return KSPUtil.ApplicationRootPath + "GameData/" + relativePath + fileName;
		}
	}

	public MEBannerEntry(MEBannerType bannerType)
	{
		SetToDefault(bannerType);
	}

	public void LoadFromMissionFolder(string newFileName, MEBannerType bannerType, Mission mission)
	{
		if (!string.IsNullOrEmpty(newFileName))
		{
			isInMissionFolder = true;
			fileName = newFileName;
			relativePath = bannerType.ToString() + "/";
			missionFolderPath = mission.BannersPath;
			string text = "";
			if (!File.Exists(FullPath))
			{
				texture = GetBannerDefaultTexture(bannerType, out text);
			}
			else
			{
				texture = MissionsUtils.GetTextureInExternalPath(FullPath);
			}
		}
	}

	public bool CopySource(string newPath)
	{
		string fullPath = FullPath;
		bool result = false;
		if (File.Exists(fullPath) && fullPath != newPath)
		{
			File.Copy(fullPath, newPath, overwrite: true);
			result = true;
		}
		return result;
	}

	public void DeleteSource()
	{
		string fullPath = FullPath;
		if (File.Exists(fullPath))
		{
			File.Delete(fullPath);
		}
	}

	public bool HasValidSource()
	{
		return File.Exists(FullPath);
	}

	public void SetToDefault(MEBannerType bannerType)
	{
		isInMissionFolder = false;
		fileName = "default.png";
		texture = GetBannerDefaultTexture(bannerType, out relativePath);
	}

	public static Texture2D GetBannerDefaultTexture(MEBannerType bannerType, out string relativePath)
	{
		Texture2D textureInExternalPath = MissionsUtils.GetTextureInExternalPath(KSPUtil.ApplicationRootPath + "GameData/" + GetBannerGameDataPath(bannerType) + "default.png");
		if (textureInExternalPath != null)
		{
			relativePath = GetBannerGameDataPath(bannerType);
			return textureInExternalPath;
		}
		textureInExternalPath = MissionsUtils.GetTextureInExternalPath(KSPUtil.ApplicationRootPath + "GameData/" + GetStockBannerGameDataPath(bannerType) + "default.png");
		relativePath = GetStockBannerGameDataPath(bannerType);
		return textureInExternalPath;
	}

	public static string GetBannerGameDataPath(MEBannerType bannerType)
	{
		return "SquadExpansion/MakingHistory/Banners/" + bannerType.ToString() + "/";
	}

	public static string GetStockBannerGameDataPath(MEBannerType bannerType)
	{
		return "Squad/Missions/Banners/" + bannerType.ToString() + "/";
	}

	public void DestroyTexture()
	{
		if (isInMissionFolder)
		{
			UnityEngine.Object.Destroy(texture);
		}
	}
}
