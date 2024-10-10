using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIBox : DialogGUIBase
{
	public DialogGUIBox(string message, float w, float h, Func<bool> EnabledCondition = null, params DialogGUIBase[] options)
		: base(options)
	{
		OptionText = message;
		size = new Vector2(w, h);
		width = w;
		height = h;
		OptionEnabledCondition = EnabledCondition;
	}

	public DialogGUIBox(string message, UIStyle style, float w, float h, Func<bool> EnabledCondition = null, params DialogGUIBase[] options)
		: base(options)
	{
		OptionText = message;
		size = new Vector2(w, h);
		width = w;
		height = h;
		OptionEnabledCondition = EnabledCondition;
		guiStyle = style;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UIBoxPrefab"));
		if (uiItem.GetComponent<CanvasRenderer>() == null)
		{
			uiItem.AddComponent<CanvasRenderer>();
		}
		if (uiItem.GetComponent<Image>() == null)
		{
			uiItem.AddComponent<Image>();
		}
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		uiItem.GetComponent<Image>().sprite = ((guiStyle != null) ? guiStyle.normal.background : skin.box.normal.background);
		if (!string.IsNullOrEmpty(OptionText))
		{
			children.Add(new DialogGUIHorizontalLayout(true, true, 0f, new RectOffset(), TextAnchor.UpperCenter, new DialogGUILabel(OptionText, guiStyle ?? skin.box)));
		}
		return base.Create(ref layouts, skin);
	}
}
