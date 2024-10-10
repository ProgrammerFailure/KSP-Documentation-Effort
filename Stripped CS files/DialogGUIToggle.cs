using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIToggle : DialogGUIBase
{
	public bool setting;

	public string label;

	public Callback<bool> onToggled = delegate
	{
	};

	public Func<bool> setValue;

	public Func<string> setLabel;

	public Func<Sprite> setCheck;

	public Sprite overlayImage;

	public Toggle toggle;

	public TextMeshProUGUI textItem;

	public DialogGUIToggle(bool set, string lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		setting = set;
		label = lbel;
		onToggled = selected;
		size = new Vector2(w, h);
	}

	public DialogGUIToggle(bool set, Func<string> lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		setting = set;
		setLabel = lbel;
		onToggled = selected;
		size = new Vector2(w, h);
	}

	public DialogGUIToggle(Func<bool> set, string lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		setting = set();
		setValue = set;
		label = lbel;
		onToggled = selected;
		size = new Vector2(w, h);
	}

	public DialogGUIToggle(Func<bool> set, Func<string> lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		setting = set();
		setValue = set;
		setLabel = lbel;
		onToggled = selected;
		size = new Vector2(w, h);
	}

	public DialogGUIToggle(Func<bool> set, Func<Sprite> checkSet, Callback<bool> selected, Sprite overImage, float w = -1f, float h = -1f)
	{
		setting = set();
		setValue = set;
		onToggled = selected;
		size = new Vector2(w, h);
		setCheck = checkSet;
		overlayImage = overImage;
	}

	public override void Update()
	{
		base.Update();
		if (uiItem != null && uiItem.activeInHierarchy)
		{
			if (setValue != null)
			{
				bool isOn = setValue();
				toggle.isOn = isOn;
			}
			if (setLabel != null)
			{
				textItem.text = setLabel();
			}
			if (setCheck != null)
			{
				((Image)toggle.targetGraphic).sprite = setCheck();
			}
		}
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (!(this is DialogGUIToggleButton))
		{
			uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UITogglePrefab"));
			guiStyle = guiStyle ?? skin.toggle;
			uiItem.SetActive(value: true);
			uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
			SetupTransformAndLayout();
			toggle = uiItem.GetComponent<Toggle>();
			if (setCheck == null)
			{
				SpriteState spriteState = default(SpriteState);
				spriteState.disabledSprite = guiStyle.disabled.background;
				spriteState.pressedSprite = guiStyle.active.background;
				spriteState.highlightedSprite = guiStyle.highlight.background;
				if (spriteState.selectedSprite == null)
				{
					spriteState.selectedSprite = guiStyle.highlight.background;
				}
				toggle.spriteState = spriteState;
			}
			else
			{
				toggle.transition = Selectable.Transition.None;
				((Image)toggle.targetGraphic).sprite = setCheck();
				toggle.graphic.gameObject.SetActive(value: false);
				RectTransform obj = toggle.targetGraphic.transform as RectTransform;
				obj.anchorMin = Vector2.one * 0.5f;
				obj.anchorMax = Vector2.one * 0.5f;
				obj.localScale = Vector3.one;
				obj.localPosition = Vector3.one;
				obj.sizeDelta = new Vector2(size.x + 2f, size.y + 2f);
			}
			if (overlayImage != null)
			{
				Image image = new GameObject("Overlay").AddComponent<Image>();
				image.sprite = overlayImage;
				image.transform.SetParent(toggle.transform, worldPositionStays: false);
				RectTransform obj2 = image.transform as RectTransform;
				obj2.anchorMin = Vector2.one * 0.5f;
				obj2.anchorMax = Vector2.one * 0.5f;
				obj2.localScale = Vector3.one;
				obj2.localPosition = Vector3.one;
				obj2.sizeDelta = size;
			}
			toggle.isOn = setting;
			toggle.onValueChanged.AddListener(delegate(bool b)
			{
				onToggled(b);
			});
			textItem = uiItem.GetChild("Label").GetComponent<TextMeshProUGUI>();
			DialogGUIBase.SetUpTextObject(textItem, label, guiStyle, skin);
		}
		return base.Create(ref layouts, skin);
	}
}
