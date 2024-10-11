using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using Steamworks;
using UnityEngine;

namespace Expansions.Missions;

public static class MissionsUtils
{
	internal class Objectives
	{
		internal class Graph<T>
		{
			public Dictionary<MENode, HashSet<MENode>> AdjacencyList
			{
				[MethodImpl(MethodImplOptions.NoInlining)]
				[CompilerGenerated]
				get
				{
					throw null;
				}
				[MethodImpl(MethodImplOptions.NoInlining)]
				[CompilerGenerated]
				private set
				{
					throw null;
				}
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public Graph()
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public Graph(IEnumerable<MENode> vertices, IEnumerable<MEEdge> edges)
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void AddVertex(MENode vertex)
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void AddEdge(MEEdge edge)
			{
				throw null;
			}
		}

		internal class MEEdge
		{
			public MENode from;

			public MENode to;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public MEEdge()
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Objectives()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static HashSet<MENode> BreadthSearch<T>(Graph<MENode> graph, MENode start, Action<MENode> preVisit = null)
		{
			throw null;
		}
	}

	internal static readonly List<string> ImageExtensions;

	internal static readonly string savesPath;

	internal static readonly string testsavesPath;

	public static readonly string fileExtension;

	private static Callback<SteamUGCQueryCompleted_t, bool, bool> onSteamQueryComplete;

	private static DictionaryValueList<string, MissionPack> missionPacks;

	public static DictionaryValueList<string, List<Type>> adjusterTypesSupportedByPartModule;

	public static DictionaryValueList<string, List<Type>> partModuleTypesSupportedByAdjuster;

	public static string ExpansionRootPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string StockMissionsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string BaseMissionsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string UsersMissionsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string MissionExportsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string MissionImportCompletedPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string TemplatesPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string SavesRootPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string SavesPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DictionaryValueList<string, MissionPack> MissionPacks
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MissionsUtils()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OpenMissionBuilder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject MEPrefab(string prefabPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D METexture(string prefabPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetFacilityLimit(string facilityName, int defaultLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetMissionsFolder(MissionTypes missionType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetUsersMissionsPath(string missionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetStockMissionsPath(string missionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetBaseMissionsPath(string missionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static List<MissionFileInfo> GatherMissionFiles(MissionTypes missionType, bool excludeSteamUnsubscribed = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int SortMissions(MissionFileInfo a, MissionFileInfo b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string VerifyUsersMissionsFolder(string missionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveMissionSaves(string folderName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void GetSteamWorkshopMissions(Callback<SteamUGCQueryCompleted_t, bool, bool> Callback, EUGCQuery queryType, string[] tags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void onSteamQueryItemsCallback(SteamUGCQueryCompleted_t results, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RebuildPacksList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void ParseMissionPacksCFG(string folderPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddMissionPackFromNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<MissionPack> GatherMissionPacks(MissionTypes missionType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void InitialiseAdjusterTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddMissionsScenarios()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static List<MENode> GetNodesOrdered(Mission m, bool objectivesOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowParser InstantiateMEFlowParser(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D GetTextureInExternalPath(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetFieldValueForDisplay(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void UpdatePartNames(ref List<string> partNames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string UpdatePartName(string partName)
	{
		throw null;
	}
}
