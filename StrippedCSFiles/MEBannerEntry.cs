using System;
using System.Runtime.CompilerServices;
using Expansions.Missions;
using UnityEngine;

[Serializable]
public class MEBannerEntry
{
	public string fileName;

	public bool isInMissionFolder;

	public Texture2D texture;

	public DialogGUIToggleButton buttonReference;

	private string relativePath;

	private string missionFolderPath;

	private bool isDuplicate;

	public bool IsDuplicate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string DisplayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FullPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEBannerEntry(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadFromMissionFolder(string newFileName, MEBannerType bannerType, Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CopySource(string newPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteSource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValidSource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetToDefault(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D GetBannerDefaultTexture(MEBannerType bannerType, out string relativePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string GetBannerGameDataPath(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string GetStockBannerGameDataPath(MEBannerType bannerType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyTexture()
	{
		throw null;
	}
}
