using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class RDTechTree : MonoBehaviour
{
	public RDController controller;

	public static EventData<RDTechTree> OnTechTreeSpawn;

	public static EventData<RDTechTree> OnTechTreeDespawn;

	public static string backupTechTreeUrl;

	private static List<ProtoRDNode> rdNodes;

	private static ProtoTechNode[] TechTreeTechs;

	private static ProtoRDNode[] TechTreeNodes;

	private static ConfigNode treeNode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDTechTree()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RDTechTree()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTechNode[] GetTreeTechs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoRDNode[] GetTreeNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetTreeConfigNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetRDScreenPrefab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PreLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ReLoad(string filePath, bool loadFromDatabase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnTechTreeNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveTechTree(List<RDNode> nodes, string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void WipeTechTree(List<RDNode> nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadTechTree(string filePath, List<RDNode> rdNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadTechTree(string filePath, List<RDNode> rdNodes, bool loadFromDatabase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadTechTree(string filePath, List<ProtoRDNode> rdNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadTechTree(string filePath, List<ProtoRDNode> rdNodes, bool loadFromDatabase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadTechTitles(string filePath, Dictionary<string, string> dict)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadTechTitles(string filePath, Dictionary<string, string> dict, bool loadFromDatabase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool GetTechNodesFromDB(out ConfigNode treeNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoTechNode> GetCheapestUnavailableNodes(int tolerance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoTechNode> GetNextUnavailableNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void recurseForNextTechs(ProtoRDNode node, List<ProtoTechNode> techs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTechNode FindTech(string requiredTechId)
	{
		throw null;
	}
}
