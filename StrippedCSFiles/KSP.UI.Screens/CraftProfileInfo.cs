using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class CraftProfileInfo : IConfigNode
{
	public string shipName;

	public string description;

	public VersionCompareResult compatibility;

	public string version;

	public string saveMD5;

	public long lastWriteTime;

	public int partCount;

	public int stageCount;

	public float totalCost;

	public float totalMass;

	public Vector3 shipSize;

	public EditorFacility shipFacility;

	public ulong steamPublishedFileId;

	public List<string> partNames;

	public List<string> partModules;

	public bool shipPartsExperimental;

	public List<string> UnavailableShipParts;

	public List<string> UnavailableShipPartModules;

	public bool duplicatedParts;

	private static HashSet<string> excludedParts;

	public List<string> BrokenShipParts;

	public bool shipPartsUnlocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool shipPartModulesAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftProfileInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CraftProfileInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CraftProfileInfo LoadDetailsFromCraftFile(ConfigNode root, string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal CraftProfileInfo LoadDetailsFromCraftFile(ConfigNode root, string filePath, bool brokenParts, bool bypassTechCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadFromMetaFile(string loadmetaPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveToMetaFile(string loadmetaPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSFSMD5(string fullPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long GetLastWriteTime(string fullPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateExcludedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void PrepareCraftMetaFileLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static CraftProfileInfo GetSaveData(string fullPath, string loadMetaPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetErrorMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CraftHasErrors()
	{
		throw null;
	}
}
