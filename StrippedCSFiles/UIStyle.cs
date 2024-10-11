using System;
using System.Runtime.CompilerServices;
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStyle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStyle(UIStyle s)
	{
		throw null;
	}
}
