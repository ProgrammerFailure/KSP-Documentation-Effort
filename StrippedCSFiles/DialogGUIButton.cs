using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DialogGUIButton : DialogGUIBase
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

	public Callback onOptionSelected;

	public bool DismissOnSelect;

	public Func<string> GetString;

	public Sprite image;

	private TextMeshProUGUI textItem;

	public TextLabelOptions textLabelOptions;

	private bool clearButtonImage;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected, bool dismissOnSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected, float w, float h, bool dismissOnSelect, UIStyle style)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(Func<string> getString, Callback onSelected, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(Func<string> getString, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected, Func<bool> EnabledCondition, bool dismissOnSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, UIStyle style = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(Func<string> getString, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, UIStyle style)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(Sprite image, Callback onSelected, float w, float h, bool dismissOnSelect = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(Sprite image, string text, Callback onSelected, float w, float h, bool dismissOnSelect = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OptionSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearButtonImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Resize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		throw null;
	}
}
public class DialogGUIButton<T> : DialogGUIButton
{
	public Callback<T> onParamOptionSelected;

	public T parameter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter, bool dismissOnSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter, Func<bool> EnabledCondition, bool dismissOnSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OptionSelected()
	{
		throw null;
	}
}
