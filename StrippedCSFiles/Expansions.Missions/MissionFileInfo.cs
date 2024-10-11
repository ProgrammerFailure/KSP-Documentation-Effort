using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Expansions.Missions.Runtime;

namespace Expansions.Missions;

[Serializable]
public class MissionFileInfo
{
	internal string idName;

	public string folderName;

	public MissionTypes missionType;

	public string title;

	public string briefing;

	public string author;

	public string modsBriefing;

	public string packName;

	public int order;

	public bool hardIcon;

	public MissionDifficulty difficulty;

	public List<string> tags;

	public int steamState;

	public bool subscribed;

	public bool canBeUsed;

	public string steamStateText;

	private Mission simpleMissionCache;

	private MissionPlayDialog.MissionProfileInfo metaMission;

	private LoadGameDialog.PlayerProfileInfo metaSavedMission;

	public Mission SimpleMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MissionPlayDialog.MissionProfileInfo MetaMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Mission savedMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal LoadGameDialog.PlayerProfileInfo MetaSavedMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsTutorialMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FolderPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ShipFolderPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FilePath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public FileInfo FileInfoObject
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string BannerPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SaveFolderPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SavePath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SaveShipFolderPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasSave
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionFileInfo(string folderName, MissionTypes missionType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionFileInfo CreateFromPath(string fullPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsCompatible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsCraftCompatible(ref string errorString, ref HashSet<string> incompatibleCraftHashSet, bool checkSaveIfAvailable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsIndividualFolderCraftCompatible(FileInfo[] fileInfoList, ref string errorString, ref HashSet<string> incompatibleCraftHashSet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ulong GetSteamFileId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSteamState()
	{
		throw null;
	}
}
