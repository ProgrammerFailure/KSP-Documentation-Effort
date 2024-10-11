using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class RnDDebugUtil
{
	private enum Page
	{
		TechTree,
		Node,
		Tech,
		Science,
		Parts,
		Files,
		Settings
	}

	public class NodeMetrics
	{
		public RDNode rdNode;

		public RDTech rdTech;

		public List<PartMetrics> partMetrics;

		public float avgPartCostVsEntryCostRatio;

		public float avgPartEntryCostVsScienceCostRatio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public NodeMetrics(RDNode node)
		{
			throw null;
		}
	}

	public class PartMetrics
	{
		public AvailablePart part;

		public RDTech assignedNode;

		public float costVsEntryCostRatio;

		public float costVsNodeScienceCostRatio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartMetrics(AvailablePart p, RDTech techNode)
		{
			throw null;
		}
	}

	private float wWidth;

	private RDTechTree techTree;

	private RDTech selectedTech;

	private RDNode selectedNode;

	private NodeMetrics selectedNodeMetrics;

	private Vector2 scrollPos;

	private List<AvailablePart> moddedParts;

	private List<RDNode> rdNodes;

	private RDController treeController;

	public string textEditor;

	public string treeCfgPath;

	public static bool showPartsInNodeTooltips;

	private Page page;

	private string[] pages;

	private float yThreshold;

	private Vector2 scrollPosition;

	private int selGridInt;

	private string outputText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RnDDebugUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RnDDebugUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(RDTechTree tecTree, float wWidth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTreeSpawn(RDController treeController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTreeDespawn(RDController treeController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeSelected(RDNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeUnselected(RDNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawWindow(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawTreePage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReloadParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildTree()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReloadPartAssignments()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<RDNode> GetSortedRDNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int CompareRDNodes(RDNode a, RDNode b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawNodePage(RDNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawTechPage(RDTech t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawSciencePage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawPartsPage(NodeMetrics n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawFilesPage(List<AvailablePart> moddedParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SavePart(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DiscardPart(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawSettingsPage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasTextEditor()
	{
		throw null;
	}
}
