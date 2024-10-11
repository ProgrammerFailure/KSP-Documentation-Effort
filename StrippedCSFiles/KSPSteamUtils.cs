using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;

internal class KSPSteamUtils
{
	private static string steamCacheFolder;

	public static string SteamCacheFolder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPSteamUtils()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KSPSteamUtils()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static List<FileInfo> GatherCraftFilesFileInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static List<CraftEntry> GatherCraftFiles(List<CraftEntry> oldCraftEntries, Callback<CraftEntry> OnEntrySelected, bool excludeSteamUnsubscribed = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static ulong GetSteamIDFromSteamFolder(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GetSteamCacheLocation(string filePath)
	{
		throw null;
	}
}
