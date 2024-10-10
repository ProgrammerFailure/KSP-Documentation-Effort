using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIGridLayout : DialogGUILayoutBase
{
	public Vector2 cellSize { get; set; }

	public GridLayoutGroup.Constraint constraint { get; set; }

	public int constraintCount { get; set; }

	public new Vector2 spacing { get; set; }

	public GridLayoutGroup.Axis startAxis { get; set; }

	public GridLayoutGroup.Corner startCorner { get; set; }

	public TextAnchor childAlignment { get; set; }

	public DialogGUIGridLayout(params DialogGUIBase[] list)
		: base(list)
	{
	}

	public DialogGUIGridLayout()
	{
	}

	public DialogGUIGridLayout(RectOffset padding, Vector2 cellSize, Vector2 spacing, GridLayoutGroup.Corner startCorner, GridLayoutGroup.Axis startAxis, TextAnchor childAligment, GridLayoutGroup.Constraint constraint, int constraintCount, params DialogGUIBase[] list)
		: base(list)
	{
		this.spacing = spacing;
		base.padding = padding;
		this.cellSize = cellSize;
		this.constraint = constraint;
		this.constraintCount = constraintCount;
		this.startAxis = startAxis;
		this.startCorner = startCorner;
		childAlignment = childAlignment;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (!useParent)
		{
			uiItem = Object.Instantiate(UISkinManager.GetPrefab("UIGridLayoutPrefab"));
			uiItem.SetActive(value: true);
			uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
			uiItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
			uiItem.GetComponent<RectTransform>().localScale = Vector3.one;
			uiItem.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		}
		else
		{
			uiItem = layouts.Peek().gameObject;
			uiItem.AddComponent<GridLayoutGroup>();
		}
		GridLayoutGroup component = uiItem.GetComponent<GridLayoutGroup>();
		component.cellSize = cellSize;
		component.childAlignment = childAlignment;
		component.constraint = constraint;
		component.constraintCount = constraintCount;
		component.padding = padding;
		component.spacing = spacing;
		component.startAxis = startAxis;
		component.startCorner = startCorner;
		return base.Create(ref layouts, skin);
	}
}
