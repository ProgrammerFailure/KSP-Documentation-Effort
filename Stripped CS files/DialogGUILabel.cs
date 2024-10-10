using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogGUILabel : DialogGUIBase
{
	public class TextLabelOptions
	{
		public bool resizeBestFit;

		public int resizeMinFontSize;

		public int resizeMaxFontSize;

		public bool enableWordWrapping = true;

		public TextOverflowModes OverflowMode;
	}

	public bool expandWidth;

	public bool expandHeight;

	public Func<string> GetString;

	public bool bypassTextStyleColor;

	public TextMeshProUGUI text;

	public TextLabelOptions textLabelOptions;

	public DialogGUILabel(string message, bool expandW = false, bool expandH = false)
	{
		OptionText = message;
		expandWidth = expandW;
		expandHeight = expandH;
		size = new Vector2(expandW ? (-1f) : 0f, expandH ? (-1f) : 0f);
	}

	public DialogGUILabel(string message, float width, float height = 0f)
	{
		OptionText = message;
		base.width = width;
		base.height = height;
		size = new Vector2(width, height);
	}

	public DialogGUILabel(string message, UIStyle style, bool expandW = false, bool expandH = false)
	{
		OptionText = message;
		expandWidth = expandW;
		expandHeight = expandH;
		guiStyle = style;
		size = new Vector2(expandW ? (-1f) : 0f, expandH ? (-1f) : 0f);
	}

	public DialogGUILabel(Func<string> getString, UIStyle style, bool expandW = false, bool expandH = false)
	{
		expandWidth = expandW;
		expandHeight = expandH;
		guiStyle = style;
		GetString = getString;
		size = new Vector2(expandW ? (-1f) : 0f, expandH ? (-1f) : 0f);
	}

	public DialogGUILabel(Func<string> getString, bool expandW = false, bool expandH = false)
	{
		expandWidth = expandW;
		expandHeight = expandH;
		GetString = getString;
		size = new Vector2(expandW ? (-1f) : 0f, expandH ? (-1f) : 0f);
	}

	public DialogGUILabel(bool flexH, Func<string> getString, float width, float height = 0f)
	{
		GetString = getString;
		base.width = width;
		base.height = height;
		size = new Vector2(width, height);
		flexibleHeight = flexH;
	}

	public DialogGUILabel(Func<string> getString, float width, float height = 0f)
	{
		GetString = getString;
		base.width = width;
		base.height = height;
		size = new Vector2(width, height);
	}

	public override void Update()
	{
		base.Update();
		if (GetString != null)
		{
			text.text = GetString();
		}
		else if (text != null)
		{
			text.text = OptionText;
		}
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UITextPrefab"));
		text = uiItem.GetComponent<TextMeshProUGUI>();
		if (textLabelOptions != null)
		{
			text.enableAutoSizing = textLabelOptions.resizeBestFit;
			text.fontSizeMax = textLabelOptions.resizeMaxFontSize;
			text.fontSizeMin = textLabelOptions.resizeMinFontSize;
			text.enableWordWrapping = textLabelOptions.enableWordWrapping;
			text.overflowMode = textLabelOptions.OverflowMode;
		}
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		DialogGUIBase.SetUpTextObject(text, OptionText, guiStyle ?? skin.label, skin, bypassTextStyleColor);
		return base.Create(ref layouts, skin);
	}
}
