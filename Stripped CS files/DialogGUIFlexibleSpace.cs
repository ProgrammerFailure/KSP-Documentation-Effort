using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIFlexibleSpace : DialogGUISpace
{
	public DialogGUIFlexibleSpace()
		: base(-1f)
	{
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = new GameObject("FlexibleSpace");
		GameObject gameObject = layouts.Peek().gameObject;
		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		{
			uiItem.AddComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.LowerLeft;
			uiItem.AddComponent<LayoutElement>().flexibleHeight = 1f;
		}
		else if (gameObject.GetComponent<HorizontalLayoutGroup>() != null)
		{
			uiItem.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperRight;
			uiItem.AddComponent<LayoutElement>().flexibleWidth = 1f;
		}
		uiItem.GetComponent<HorizontalOrVerticalLayoutGroup>().padding = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>().padding;
		uiItem.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing;
		uiItem.GetComponent<HorizontalOrVerticalLayoutGroup>().childForceExpandHeight = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>().childForceExpandHeight;
		uiItem.GetComponent<HorizontalOrVerticalLayoutGroup>().childForceExpandWidth = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>().childForceExpandWidth;
		uiItem.transform.SetParent(gameObject.transform, worldPositionStays: false);
		return base.Create(ref layouts, skin);
	}
}
