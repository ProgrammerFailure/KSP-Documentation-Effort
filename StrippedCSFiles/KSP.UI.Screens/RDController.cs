using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RUI.Icons.Simple;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

public class RDController : MonoBehaviour
{
	public static RDController Instance;

	public RDTechTree techTree;

	public UIStatePanel nodePanel;

	public RDGridArea gridArea;

	public RDNode node_inPanel;

	public TextMeshProUGUI node_name;

	public TextMeshProUGUI node_description;

	public UIStateButton actionButton;

	public TextMeshProUGUI actionButtonText;

	public RDPartList partList;

	public RDNodeList requiresList;

	public TextMeshProUGUI requiresCaption;

	public GameObject RDNodePrefab;

	public Transform RDNodeRoot;

	public List<RDNode> nodes;

	[NonSerialized]
	public RDNode node_selected;

	private float scienceCap;

	public static EventData<RDController> OnRDTreeSpawn;

	public static EventData<RDController> OnRDTreeDespawn;

	[NonSerialized]
	public IconLoader iconLoader;

	[SerializeField]
	private IconLoader iconLoaderPrefab;

	private static HashSet<AvailablePart> otherPartHashes;

	public float ScienceCap
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RDController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ActionButtonClick(string state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowNodePanel(RDNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePurchaseButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowNothingPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterNode(RDNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Snap to start node")]
	public void SnapToStartNode()
	{
		throw null;
	}
}
