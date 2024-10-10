using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUISpace : DialogGUIBase
{
	public float space;

	public DialogGUISpace(float v)
	{
		space = v;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (!(this is DialogGUIFlexibleSpace))
		{
			uiItem = new GameObject("Space");
			uiItem.AddComponent<RectTransform>();
			uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
			if (uiItem.transform.parent.GetComponent<VerticalLayoutGroup>() != null)
			{
				size.x = 0f;
				size.y = space;
			}
			else if (uiItem.transform.parent.GetComponent<HorizontalLayoutGroup>() != null)
			{
				size.y = 0f;
				size.x = space;
			}
			SetupTransformAndLayout();
		}
		return base.Create(ref layouts, skin);
	}
}
