using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using RUI.Icons.Simple;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

namespace KSP.UI.Screens;

public class RDNode : MonoBehaviour
{
	public enum State
	{
		RESEARCHED = 1,
		RESEARCHABLE,
		HIDDEN,
		FADED
	}

	public enum Anchor
	{
		TOP = 1,
		BOTTOM,
		RIGHT,
		LEFT
	}

	[Serializable]
	public class ParentAnchor
	{
		public RDNode node;

		public Anchor anchor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ParentAnchor(RDNode node, Anchor anchor)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetPosition(Vector3 toAnchorPos)
		{
			throw null;
		}
	}

	[Serializable]
	public class Parent
	{
		public ParentAnchor parent;

		public Anchor anchor;

		[NonSerialized]
		public VectorLine line;

		[NonSerialized]
		public Image arrowHead;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Parent(ParentAnchor parent, Anchor anchor)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Parent(ConfigNode node, List<RDNode> nodes)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node, List<RDNode> nodes)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RDNode FindNodeByID(string techID, List<RDNode> nodes)
		{
			throw null;
		}
	}

	public RDController controller;

	public float scale;

	public bool treeNode;

	public string iconRef;

	public Icon icon;

	public bool AnyParentToUnlock;

	public Parent[] parents;

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public string description;

	[NonSerialized]
	public RDTech tech;

	[NonSerialized]
	public bool selected;

	[NonSerialized]
	public List<RDNode> children;

	[NonSerialized]
	public RDNodePrefab graphics;

	private TooltipController_TitleAndText tooltip;

	private bool selectable;

	private float width;

	private float height;

	private Vector3 pos;

	private bool setup;

	private bool arrows_setup;

	public static EventData<RDNode> OnNodeSelected;

	public static EventData<RDNode> OnNodeUnselected;

	public bool IsResearched
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool hideIfNoParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Vector3 anchor_top
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Vector3 anchor_bottom
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Vector3 anchor_left
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Vector3 anchor_right
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public State state
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
	public RDNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RDNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Warmup(RDTech rdTech)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetTooltipCaption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetButtonState(State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconState(Icon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeArrows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateGraphics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PartsNotUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PartsInTotal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetVisiblePartsCount(List<AvailablePart> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnselectNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NodeInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPosition(Anchor anchor, Vector3 toAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowArrows(bool show, Material mat, List<Parent> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawArrow(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Vector2> GetVectorArray(Parent a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2[] chamferCorner(Vector2 inPoint, Vector2 cornerPoint, Vector2 outPoint, float chamferDist, int subdivs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FakeGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadLinks(ConfigNode node, List<RDNode> nodes)
	{
		throw null;
	}
}
