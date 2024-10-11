using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DialogGUILabel : DialogGUIBase
{
	public class TextLabelOptions
	{
		public bool resizeBestFit;

		public int resizeMinFontSize;

		public int resizeMaxFontSize;

		public bool enableWordWrapping;

		public TextOverflowModes OverflowMode;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextLabelOptions()
		{
			throw null;
		}
	}

	public bool expandWidth;

	public bool expandHeight;

	public Func<string> GetString;

	public bool bypassTextStyleColor;

	public TextMeshProUGUI text;

	public TextLabelOptions textLabelOptions;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(string message, bool expandW = false, bool expandH = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(string message, float width, float height = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(string message, UIStyle style, bool expandW = false, bool expandH = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(Func<string> getString, UIStyle style, bool expandW = false, bool expandH = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(Func<string> getString, bool expandW = false, bool expandH = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(bool flexH, Func<string> getString, float width, float height = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILabel(Func<string> getString, float width, float height = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		throw null;
	}
}
