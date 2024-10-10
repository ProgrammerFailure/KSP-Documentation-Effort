using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIToggleButton : DialogGUIToggle
{
	public DialogGUIToggleButton(bool set, string lbel, Callback<bool> selected, float w = -1f, float h = 1f)
		: base(set, lbel, selected, w, h)
	{
	}

	public DialogGUIToggleButton(Func<bool> set, string lbel, Callback<bool> selected, float w = -1f, float h = 1f)
		: base(set, lbel, selected, w, h)
	{
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UIToggleButtonPrefab"));
		guiStyle = guiStyle ?? skin.button;
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		toggle = uiItem.GetComponent<Toggle>();
		SpriteState spriteState = default(SpriteState);
		spriteState.disabledSprite = ((guiStyle.disabled.background != null) ? guiStyle.disabled.background : guiStyle.normal.background);
		spriteState.pressedSprite = guiStyle.active.background;
		spriteState.highlightedSprite = guiStyle.highlight.background;
		if (spriteState.selectedSprite == null)
		{
			spriteState.selectedSprite = guiStyle.highlight.background;
		}
		toggle.gameObject.GetChild("Background").GetComponent<Image>().sprite = guiStyle.normal.background;
		toggle.gameObject.GetChild("Checkmark").GetComponent<Image>().sprite = guiStyle.active.background;
		toggle.spriteState = spriteState;
		toggle.isOn = setting;
		toggle.onValueChanged.AddListener(delegate(bool b)
		{
			onToggled(b);
		});
		if (!string.IsNullOrEmpty(label))
		{
			children.Add(new DialogGUIHorizontalLayout(true, true, 0f, new RectOffset(), TextAnchor.UpperCenter, new DialogGUILabel(label, guiStyle)));
		}
		return base.Create(ref layouts, skin);
	}
}
