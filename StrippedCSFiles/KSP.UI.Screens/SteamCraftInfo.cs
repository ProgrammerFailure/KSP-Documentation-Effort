using System.Runtime.CompilerServices;
using Steamworks;

namespace KSP.UI.Screens;

public class SteamCraftInfo
{
	public SteamUGCDetails_t itemDetails;

	public ulong totalSubscriptions;

	public ulong totalFavorites;

	public ulong totalFollowers;

	public string previewURL;

	public ConfigNode metaData;

	public string modsBriefing;

	public string vesselType;

	public float mass;

	public int partCount;

	public int stageCount;

	public float cost;

	public int crewCapacity;

	public string KSPversion;

	public int steamState;

	public bool subscribed;

	public bool canBeUsed;

	public string steamStateText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteamCraftInfo(SteamUGCDetails_t itemDetails)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProcessMetaData(string metaData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CreateMetaData(string modsBriefing, string vesselType, float mass, float cost, int partCount, int stageCount, int crewCapacity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSteamState()
	{
		throw null;
	}
}
