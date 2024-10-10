using System;
using UnityEngine;

[Serializable]
public class UIStyle
{
	public string name;

	public UIStyleState normal;

	public UIStyleState active;

	public UIStyleState highlight;

	public UIStyleState disabled;

	public Font font;

	public int fontSize;

	public FontStyle fontStyle;

	public bool wordWrap;

	public bool richText;

	public TextAnchor alignment;

	public TextClipping clipping;

	public float lineHeight;

	public bool stretchHeight;

	public bool stretchWidth;

	public float fixedHeight;

	public float fixedWidth;

	public UIStyle()
	{
	}

	public UIStyle(UIStyle s)
	{
		name = s.name;
		normal = s.normal;
		active = s.active;
		highlight = s.highlight;
		disabled = s.disabled;
		font = s.font;
		fontSize = s.fontSize;
		fontStyle = s.fontStyle;
		wordWrap = s.wordWrap;
		richText = s.richText;
		alignment = s.alignment;
		clipping = s.clipping;
		lineHeight = s.lineHeight;
		stretchHeight = s.stretchHeight;
		stretchWidth = s.stretchWidth;
		fixedHeight = s.fixedHeight;
		fixedWidth = s.fixedWidth;
	}
}
