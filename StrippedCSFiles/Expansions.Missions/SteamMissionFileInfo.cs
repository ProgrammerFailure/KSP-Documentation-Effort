using System.Runtime.CompilerServices;
using Steamworks;

namespace Expansions.Missions;

public class SteamMissionFileInfo
{
	public SteamUGCDetails_t itemDetails;

	public ulong totalSubscriptions;

	public ulong totalFavorites;

	public ulong totalFollowers;

	public string previewURL;

	public ConfigNode metaData;

	public MissionDifficulty missionDifficulty;

	public string modsBriefing;

	public string packName;

	public string packDisplayName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteamMissionFileInfo(SteamUGCDetails_t itemDetails)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProcessMetaData(string metaData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CreateMetaData(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMissionFileInfo(ref MissionFileInfo missionFileInfo)
	{
		throw null;
	}
}
