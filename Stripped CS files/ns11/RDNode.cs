using System;
using System.Collections.Generic;
using ns12;
using ns4;
using ns9;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

namespace ns11;

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
		const_0 = 1,
		BOTTOM,
		RIGHT,
		LEFT
	}

	[Serializable]
	public class ParentAnchor
	{
		public RDNode node;

		public Anchor anchor;

		public ParentAnchor(RDNode node, Anchor anchor)
		{
			this.node = node;
			this.anchor = anchor;
		}

		public Vector3 GetPosition(Vector3 toAnchorPos)
		{
			return node.GetPosition(anchor, toAnchorPos);
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

		public Parent(ParentAnchor parent, Anchor anchor)
		{
			this.parent = parent;
			this.anchor = anchor;
		}

		public Parent(ConfigNode node, List<RDNode> nodes)
		{
			Load(node, nodes);
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("parentID", parent.node.tech.techID);
			node.AddValue("lineFrom", parent.anchor);
			node.AddValue("lineTo", anchor);
		}

		public void Load(ConfigNode node, List<RDNode> nodes)
		{
			string text = "";
			RDNode rDNode = null;
			Anchor anchor = Anchor.RIGHT;
			Anchor anchor2 = Anchor.LEFT;
			if (node.HasValue("parentID"))
			{
				text = node.GetValue("parentID");
			}
			if (text != string.Empty)
			{
				rDNode = FindNodeByID(text, nodes);
			}
			if (rDNode != null)
			{
				if (node.HasValue("lineFrom"))
				{
					anchor = (Anchor)Enum.Parse(typeof(Anchor), node.GetValue("lineFrom"));
				}
				if (node.HasValue("lineTo"))
				{
					anchor2 = (Anchor)Enum.Parse(typeof(Anchor), node.GetValue("lineTo"));
				}
				parent = new ParentAnchor(rDNode, anchor);
				this.anchor = anchor2;
			}
			else
			{
				Debug.LogError("No RDNode registered with id " + text + "!");
			}
		}

		public RDNode FindNodeByID(string techID, List<RDNode> nodes)
		{
			int count = nodes.Count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					if (nodes[num].tech != null && nodes[num].tech.techID == techID)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return nodes[num];
		}
	}

	public RDController controller;

	public float scale = 1f;

	public bool treeNode = true;

	public string iconRef = "RDicon_generic";

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
	public List<RDNode> children = new List<RDNode>();

	[NonSerialized]
	public RDNodePrefab graphics;

	public TooltipController_TitleAndText tooltip;

	public bool selectable = true;

	public float width;

	public float height;

	public Vector3 pos;

	public bool setup;

	public bool arrows_setup;

	public static EventData<RDNode> OnNodeSelected = new EventData<RDNode>("OnRDNodeSelected");

	public static EventData<RDNode> OnNodeUnselected = new EventData<RDNode>("OnRDNodeUnselected");

	public bool IsResearched => tech.state == RDTech.State.Available;

	public bool hideIfNoParts => tech.hideIfNoParts;

	public Vector3 anchor_top => base.transform.position - base.transform.TransformDirection(-Vector3.right * (width / 2f - 1f));

	public Vector3 anchor_bottom => base.transform.position - base.transform.TransformDirection(-Vector3.right * (width / 2f - 1f) + base.transform.TransformDirection(Vector3.up * height));

	public Vector3 anchor_left => base.transform.position - base.transform.TransformDirection(Vector3.up * (height / 2f - 1f));

	public Vector3 anchor_right => base.transform.position - base.transform.TransformDirection(Vector3.up * (height / 2f - 1f)) + base.transform.TransformDirection(Vector3.right * width);

	public State state { get; set; }

	public void Awake()
	{
		Warmup(GetComponent<RDTech>());
	}

	public void Warmup(RDTech rdTech)
	{
		pos = base.transform.localPosition;
		tech = rdTech;
		if (tech == null)
		{
			name = base.gameObject.name;
			description = "Nothing is known about this technology.";
		}
		else
		{
			name = tech.title;
			description = tech.description;
		}
	}

	public void Start()
	{
		Setup();
	}

	public void Setup()
	{
		if (!setup)
		{
			setup = true;
			selectable = treeNode;
			graphics = UnityEngine.Object.Instantiate(RDController.Instance.RDNodePrefab).GetComponent<RDNodePrefab>();
			graphics.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 1f);
			graphics.transform.SetParent(base.transform);
			graphics.SetScale(scale);
			graphics.Setup();
			width = (graphics.width - (float)graphics.offset.right) * scale;
			height = (graphics.height - (float)graphics.offset.bottom) * scale;
			graphics.AddInputDelegate(NodeInput);
			tooltip = graphics.tooltip;
			if (treeNode)
			{
				controller = RDController.Instance;
				base.transform.parent = RDController.Instance.RDNodeRoot;
				base.transform.localPosition = pos;
				InitializeArrows();
				controller.RegisterNode(this);
				StartCoroutine(CallbackUtil.DelayedCallback(1, UpdateGraphics));
			}
			else
			{
				UnityEngine.Object.Destroy(tooltip);
			}
			SetIconState(RDController.Instance.iconLoader.GetIcon(iconRef));
		}
	}

	public string GetTooltipCaption()
	{
		if (tech == null)
		{
			return "";
		}
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDTechResearch, 0f, -tech.scienceCost, 0f);
		switch (state)
		{
		case State.RESEARCHED:
			return "<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + Localizer.Format("#autoLOC_469953") + "</color>";
		case State.RESEARCHABLE:
			if (currencyModifierQuery.CanAfford())
			{
				return currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: true);
			}
			return "<color=" + XKCDColors.HexFormat.Orange + ">" + currencyModifierQuery.GetCostLine() + Localizer.Format("#autoLOC_469963");
		default:
			return "<color=" + XKCDColors.HexFormat.KSPNeutralUIGrey + ">" + currencyModifierQuery.GetCostLine() + "</color>";
		case State.FADED:
			if ((float)tech.scienceCost > controller.ScienceCap)
			{
				return "<color=" + XKCDColors.HexFormat.Orange + ">" + currencyModifierQuery.GetCostLine() + " " + Localizer.Format("#autoLOC_469970", controller.ScienceCap.ToString("N0")) + "</color>";
			}
			return "<color=" + XKCDColors.HexFormat.Orange + ">" + currencyModifierQuery.GetCostLine() + Localizer.Format("#autoLOC_469975");
		}
	}

	public void SetButtonState(State state)
	{
		Setup();
		this.state = state;
		switch (state)
		{
		case State.RESEARCHED:
			graphics.button.SetState("RESEARCHED");
			graphics.SetIconColor(Color.white);
			break;
		case State.RESEARCHABLE:
			graphics.button.SetState("RESEARCHABLE");
			graphics.SetIconColor(Color.white);
			break;
		case State.FADED:
		{
			graphics.button.SetState("FADED");
			Color iconColor = graphics.GetIconColor();
			graphics.SetIconColor(new Color(iconColor.r, iconColor.g, iconColor.b, 0.5f));
			break;
		}
		}
		graphics.SetViewable(state != State.HIDDEN);
		selectable = treeNode && state != State.HIDDEN;
	}

	public void SetIconState(Icon icon)
	{
		iconRef = icon.GetName();
		this.icon = icon;
		graphics.SetIcon(icon);
	}

	public void InitializeArrows()
	{
		if (tech != null)
		{
			ShowArrows(show: false, controller.gridArea.LineMaterial, null);
		}
	}

	public void UpdateGraphics()
	{
		if (tech != null)
		{
			if (IsResearched)
			{
				SetButtonState(State.RESEARCHED);
			}
			else
			{
				CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDTechResearch, 0f, -tech.scienceCost, 0f);
				if (0f - (currencyModifierQuery.GetInput(Currency.Science) + currencyModifierQuery.GetEffectDelta(Currency.Science)) < controller.ScienceCap)
				{
					SetButtonState(State.RESEARCHABLE);
				}
				else
				{
					SetButtonState(State.FADED);
				}
			}
			if (hideIfNoParts && PartsInTotal() == 0)
			{
				SetButtonState(State.HIDDEN);
			}
			else
			{
				bool flag = true;
				int num = 0;
				int num2 = 0;
				List<Parent> list = new List<Parent>();
				List<Parent> list2 = new List<Parent>();
				int i = 0;
				for (int num3 = parents.Length; i < num3; i++)
				{
					Parent parent = parents[i];
					if (parent.parent.node.IsResearched)
					{
						num++;
						list.Add(parent);
					}
					else
					{
						flag = false;
						num2++;
						list2.Add(parent);
					}
				}
				if (flag)
				{
					ShowArrows(show: true, controller.gridArea.LineMaterial, null);
					graphics.SetAvailablePartsCircle(PartsNotUnlocked());
				}
				else if (num2 > 0 && num > 0)
				{
					if (AnyParentToUnlock)
					{
						ShowArrows(show: true, controller.gridArea.LineMaterial, list);
						ShowArrows(show: true, controller.gridArea.LineMaterialGray, list2);
						graphics.SetAvailablePartsCircle(PartsNotUnlocked());
					}
					else
					{
						ShowArrows(show: true, controller.gridArea.LineMaterialGray, null);
						SetButtonState(State.FADED);
						graphics.HideAvailablePartsCircle();
					}
				}
				else
				{
					ShowArrows(show: true, controller.gridArea.LineMaterialGray, null);
					SetButtonState(State.FADED);
				}
			}
			if (RnDDebugUtil.showPartsInNodeTooltips)
			{
				if (tooltip != null)
				{
					tooltip.titleString = name;
					tooltip.textString = GetTooltipCaption() + "\n" + KSPUtil.PrintCollection(tech.partsAssigned, "\n", (AvailablePart a) => a.title);
				}
			}
			else if (tooltip != null)
			{
				tooltip.titleString = name;
				tooltip.textString = GetTooltipCaption();
			}
		}
		else
		{
			UnityEngine.Object.Destroy(tooltip);
		}
	}

	public int PartsNotUnlocked()
	{
		if (tech != null)
		{
			return Mathf.Clamp(GetVisiblePartsCount(tech.partsAssigned) - GetVisiblePartsCount(tech.partsPurchased), 0, int.MaxValue);
		}
		return 0;
	}

	public int PartsInTotal()
	{
		if (tech != null)
		{
			return GetVisiblePartsCount(tech.partsAssigned) + GetVisiblePartsCount(tech.partsPurchased);
		}
		return 0;
	}

	public int GetVisiblePartsCount(List<AvailablePart> parts)
	{
		int num = 0;
		for (int i = 0; i < parts.Count; i++)
		{
			if (!parts[i].TechHidden)
			{
				num++;
			}
		}
		return num;
	}

	public void SelectNode()
	{
		selected = true;
		graphics.SelectNode();
		OnNodeSelected.Fire(this);
	}

	public void UnselectNode()
	{
		selected = false;
		graphics.UnselectNode();
		OnNodeUnselected.Fire(this);
	}

	public void NodeInput()
	{
		if (!selectable)
		{
			return;
		}
		if (controller.node_selected != this)
		{
			if (controller.node_selected != null)
			{
				controller.node_selected.UnselectNode();
			}
			controller.node_selected = this;
			SelectNode();
			controller.ShowNodePanel(this);
			RDTechTreeSearchBar.Instance.SelectPartIcons();
		}
		else if (selected)
		{
			controller.ShowNothingPanel();
			UnselectNode();
		}
		else
		{
			controller.ShowNodePanel(this);
			SelectNode();
			RDTechTreeSearchBar.Instance.SelectPartIcons();
		}
	}

	public Vector3 GetPosition(Anchor anchor, Vector3 toAnchorPos)
	{
		float num = 5f;
		switch (anchor)
		{
		default:
			return Vector3.zero;
		case Anchor.const_0:
			return anchor_top;
		case Anchor.BOTTOM:
			return anchor_bottom;
		case Anchor.RIGHT:
		{
			if (toAnchorPos == Vector3.zero)
			{
				return anchor_right;
			}
			float num3 = (float)Math.Sign((toAnchorPos - anchor_right).y) * num;
			return new Vector3(anchor_right.x, anchor_right.y + num3, anchor_right.z);
		}
		case Anchor.LEFT:
		{
			if (toAnchorPos == Vector3.zero)
			{
				return anchor_left;
			}
			float num2 = (float)Math.Sign((toAnchorPos - anchor_left).y) * num;
			return new Vector3(anchor_left.x, anchor_left.y + num2, anchor_left.z);
		}
		}
	}

	public void ShowArrows(bool show, Material mat, List<Parent> list)
	{
		if (!arrows_setup)
		{
			DrawArrow(mat);
		}
		Parent[] array = ((list == null) ? parents : list.ToArray());
		if (show)
		{
			int num = array.Length;
			while (num-- > 0)
			{
				array[num].line.rectTransform.gameObject.SetActive(value: true);
				graphics.SetArrowHeadState(array[num], show: true, mat);
			}
		}
		else
		{
			int num2 = array.Length;
			while (num2-- > 0)
			{
				array[num2].line.rectTransform.gameObject.SetActive(value: false);
				graphics.SetArrowHeadState(array[num2], show: false, mat);
			}
		}
	}

	public void DrawArrow(Material mat)
	{
		int num = parents.Length;
		for (int i = 0; i < num; i++)
		{
			Parent parent = parents[i];
			parent.parent.node.Setup();
			parent.line = new VectorLine(base.gameObject.name, GetVectorArray(parent), controller.gridArea.lineThickness, LineType.Continuous, Joins.None);
			parent.line.lineWidth = controller.gridArea.lineThickness;
			parent.line.Draw();
			parent.line.rectTransform.SetParent(parent.parent.node.transform);
			parent.line.rectTransform.localPosition = new Vector3(parent.line.rectTransform.localPosition.x, parent.line.rectTransform.localPosition.y, 0f);
			Vector2 vector = parent.line.points2[parent.line.points2.Count - 1];
			graphics.InstantiateArrowHeadAtPos(parent, vector - Vector2.left, mat);
		}
		arrows_setup = true;
	}

	public List<Vector2> GetVectorArray(Parent a)
	{
		if (a.parent.anchor == Anchor.const_0 && a.anchor == Anchor.BOTTOM)
		{
			if (Mathf.Abs(a.parent.GetPosition(Vector3.zero).x - GetPosition(a.anchor, Vector3.zero).x) < 1f)
			{
				return new List<Vector2>(2)
				{
					a.parent.GetPosition(Vector3.zero),
					GetPosition(a.anchor, Vector3.zero)
				};
			}
			float num = (GetPosition(a.anchor, Vector3.zero).y - a.parent.GetPosition(Vector3.zero).y) / 2f;
			Vector2[] array = chamferCorner(a.parent.GetPosition(Vector3.zero), a.parent.GetPosition(Vector3.zero) + base.transform.TransformDirection(Vector3.up * num), GetPosition(a.anchor, Vector3.zero) + base.transform.TransformDirection(-Vector3.up * num), controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
			Vector2[] array2 = chamferCorner(a.parent.GetPosition(Vector3.zero) + base.transform.TransformDirection(Vector3.up * num), GetPosition(a.anchor, Vector3.zero) + base.transform.TransformDirection(-Vector3.up * num), GetPosition(a.anchor, Vector3.zero), controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
			List<Vector2> list = new List<Vector2>(array.Length + array2.Length + 2);
			list.Add(a.parent.GetPosition(Vector3.zero));
			int i = 0;
			for (int num2 = array.Length; i < num2; i++)
			{
				list.Add(array[i]);
			}
			int j = 0;
			for (int num3 = array2.Length; j < num3; j++)
			{
				list.Add(array2[j]);
			}
			list.Add(GetPosition(a.anchor, Vector3.zero));
			return list;
		}
		if ((a.parent.anchor == Anchor.LEFT || a.parent.anchor == Anchor.RIGHT) && a.anchor == Anchor.BOTTOM)
		{
			float num4 = GetPosition(a.anchor, Vector3.zero).x - a.parent.GetPosition(Vector3.zero).x;
			Vector2[] array3 = chamferCorner(a.parent.GetPosition(Vector3.zero), a.parent.GetPosition(Vector3.zero) + base.transform.TransformDirection(Vector3.right * num4), GetPosition(a.anchor, Vector3.zero), controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
			List<Vector2> list2 = new List<Vector2>(array3.Length + 2);
			list2.Add(a.parent.GetPosition(Vector3.zero));
			int k = 0;
			for (int num5 = array3.Length; k < num5; k++)
			{
				list2.Add(array3[k]);
			}
			list2.Add(GetPosition(a.anchor, Vector3.zero));
			return list2;
		}
		if (a.parent.anchor == Anchor.const_0 && (a.anchor == Anchor.LEFT || a.anchor == Anchor.RIGHT))
		{
			float num6 = GetPosition(a.anchor, Vector3.zero).y - a.parent.GetPosition(Vector3.zero).y;
			Vector2[] array4 = chamferCorner(a.parent.GetPosition(Vector3.zero), a.parent.GetPosition(Vector3.zero) + base.transform.TransformDirection(Vector3.up * num6), GetPosition(a.anchor, Vector3.zero), controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
			List<Vector2> list3 = new List<Vector2>(array4.Length + 2);
			list3.Add(a.parent.GetPosition(Vector3.zero));
			int l = 0;
			for (int num7 = array4.Length; l < num7; l++)
			{
				list3.Add(array4[l]);
			}
			list3.Add(GetPosition(a.anchor, Vector3.zero));
			return list3;
		}
		if ((a.parent.anchor != Anchor.LEFT && a.parent.anchor != Anchor.RIGHT) || (a.anchor != Anchor.LEFT && a.anchor != Anchor.RIGHT))
		{
			return new List<Vector2>(2)
			{
				Vector2.zero,
				Vector2.zero
			};
		}
		if (Mathf.Abs(a.parent.GetPosition(Vector3.zero).y - GetPosition(a.anchor, Vector3.zero).y) < 1f)
		{
			return new List<Vector2>(2)
			{
				a.parent.GetPosition(Vector2.zero),
				GetPosition(a.anchor, Vector2.zero)
			};
		}
		float num8 = (GetPosition(a.anchor, Vector2.zero).x - a.parent.GetPosition(Vector2.zero).x) / 2f;
		Vector3 position = a.parent.GetPosition(GetPosition(a.anchor, Vector2.zero));
		Vector3 position2 = GetPosition(a.anchor, position);
		Vector2[] array5 = chamferCorner(position, position + base.transform.TransformDirection(Vector3.right * num8), position2 + base.transform.TransformDirection(-Vector3.right * num8), controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
		Vector2[] array6 = chamferCorner(position + base.transform.TransformDirection(Vector3.right * num8), position2 + base.transform.TransformDirection(-Vector3.right * num8), position2, controller.gridArea.roundedCornerRadius, controller.gridArea.lineCornerSegments);
		List<Vector2> list4 = new List<Vector2>(array5.Length + array6.Length + 2);
		list4.Add(position);
		int m = 0;
		for (int num9 = array5.Length; m < num9; m++)
		{
			list4.Add(array5[m]);
		}
		int n = 0;
		for (int num10 = array6.Length; n < num10; n++)
		{
			list4.Add(array6[n]);
		}
		list4.Add(position2);
		return list4;
	}

	public Vector2[] chamferCorner(Vector2 inPoint, Vector2 cornerPoint, Vector2 outPoint, float chamferDist, int subdivs)
	{
		Vector2[] array = new Vector2[1] { cornerPoint };
		for (int i = 0; i < subdivs; i++)
		{
			Vector2[] array2 = new Vector2[array.Length * 2];
			int j = 0;
			for (int num = array2.Length; j < num; j += 2)
			{
				int num2 = j / 2;
				Vector2 vector = array[num2];
				Vector2 normalized = (((num2 > 0) ? array[num2 - 1] : inPoint) - vector).normalized;
				Vector2 normalized2 = (((num2 < array.Length - 1) ? array[num2 + 1] : outPoint) - vector).normalized;
				array2[j] = vector + normalized * (chamferDist / Mathf.Pow(2.5f, i + 1));
				array2[j + 1] = vector + normalized2 * (chamferDist / Mathf.Pow(2.5f, i + 1));
			}
			array = array2;
		}
		return array;
	}

	public void OnDrawGizmos()
	{
		float num = 42f;
		float num2 = 42f;
		Vector3 position = base.transform.position;
		Vector3 vector = base.transform.position - base.transform.TransformDirection(Vector3.up * num2);
		Vector3 vector2 = base.transform.position + base.transform.TransformDirection(Vector3.right * num);
		Vector3 vector3 = base.transform.position - base.transform.TransformDirection(Vector3.up * num2) + base.transform.TransformDirection(Vector3.right * num);
		Gizmos.color = (AnyParentToUnlock ? XKCDColors.LightPeriwinkle : XKCDColors.ElectricLime);
		Gizmos.DrawLine(position, vector);
		Gizmos.DrawLine(vector, vector3);
		Gizmos.DrawLine(vector3, vector2);
		Gizmos.DrawLine(vector2, position);
		FakeGizmos();
		int num3 = parents.Length;
		for (int i = 0; i < num3; i++)
		{
			Parent parent = parents[i];
			parent.parent.node.FakeGizmos();
			List<Vector2> vectorArray = GetVectorArray(parent);
			int j = 0;
			for (int count = vectorArray.Count; j < count - 1; j++)
			{
				Gizmos.DrawLine(vectorArray[j], vectorArray[j + 1]);
			}
		}
	}

	public void FakeGizmos()
	{
		width = 42f;
		height = 42f;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("nodeName", base.gameObject.name);
		node.AddValue("anyToUnlock", AnyParentToUnlock);
		node.AddValue("icon", icon.GetName());
		node.AddValue("pos", KSPUtil.WriteVector(base.transform.localPosition));
		node.AddValue("scale", scale);
		int num = parents.Length;
		for (int i = 0; i < num; i++)
		{
			parents[i].Save(node.AddNode("Parent"));
		}
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("nodeName"))
		{
			base.gameObject.name = node.GetValue("nodeName");
		}
		if (node.HasValue("anyToUnlock"))
		{
			AnyParentToUnlock = bool.Parse(node.GetValue("anyToUnlock"));
		}
		if (node.HasValue("pos"))
		{
			pos = KSPUtil.ParseVector3(node.GetValue("pos"));
		}
		if (node.HasValue("icon"))
		{
			iconRef = node.GetValue("icon");
			icon = controller.iconLoader.GetIcon(iconRef);
		}
		if (node.HasValue("scale"))
		{
			scale = float.Parse(node.GetValue("scale"));
		}
	}

	public void LoadLinks(ConfigNode node, List<RDNode> nodes)
	{
		List<Parent> list = new List<Parent>();
		ConfigNode[] nodes2 = node.GetNodes("Parent");
		int num = nodes2.Length;
		for (int i = 0; i < num; i++)
		{
			list.Add(new Parent(nodes2[i], nodes));
		}
		parents = list.ToArray();
	}
}
