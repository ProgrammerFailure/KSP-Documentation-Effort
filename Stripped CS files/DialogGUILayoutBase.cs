using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUILayoutBase : DialogGUIBase
{
	public bool stretchWidth;

	public bool stretchHeight;

	public float spacing = 4f;

	public RectOffset padding = new RectOffset();

	public TextAnchor anchor;

	public float minWidth;

	public float minHeight;

	public bool useParent;

	public DialogGUILayoutBase(params DialogGUIBase[] list)
		: base(list)
	{
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (!(this is DialogGUIGridLayout) && !(this is DialogGUIRadialLayout))
		{
			if (!useParent)
			{
				uiItem = Object.Instantiate((this is DialogGUIVerticalLayout) ? UISkinManager.GetPrefab("UIVerticalLayoutPrefab") : UISkinManager.GetPrefab("UIHorizontalLayoutPrefab"));
			}
			else
			{
				uiItem = layouts.Peek().gameObject;
				if (this is DialogGUIVerticalLayout)
				{
					uiItem.AddComponent<VerticalLayoutGroup>();
				}
				else
				{
					uiItem.AddComponent<HorizontalLayoutGroup>();
				}
			}
			uiItem.SetActive(value: true);
			uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
			RectTransform component = uiItem.GetComponent<RectTransform>();
			component.localPosition = Vector3.zero;
			component.localScale = Vector3.one;
			component.sizeDelta = Vector2.zero;
			HorizontalOrVerticalLayoutGroup component2 = uiItem.GetComponent<HorizontalOrVerticalLayoutGroup>();
			component2.childAlignment = anchor;
			component2.childForceExpandHeight = stretchHeight;
			component2.childForceExpandWidth = stretchWidth;
			component2.spacing = spacing;
			component2.padding = padding;
			if (minWidth != 0f || minHeight != 0f)
			{
				LayoutElement layoutElement = uiItem.AddComponent<LayoutElement>();
				if (minHeight != -1f)
				{
					layoutElement.minHeight = minHeight;
				}
				else
				{
					layoutElement.flexibleHeight = minHeight;
				}
				if (minWidth != -1f)
				{
					layoutElement.minWidth = 1f;
				}
				else
				{
					layoutElement.flexibleWidth = 1f;
				}
			}
		}
		if (children.Count == 0)
		{
			layouts.Push(uiItem.transform);
		}
		return base.Create(ref layouts, skin);
	}
}
