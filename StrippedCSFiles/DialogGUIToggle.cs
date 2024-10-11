using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIToggle : DialogGUIBase
{
	public bool setting;

	public string label;

	public Callback<bool> onToggled;

	public Func<bool> setValue;

	public Func<string> setLabel;

	public Func<Sprite> setCheck;

	public Sprite overlayImage;

	public Toggle toggle;

	protected TextMeshProUGUI textItem;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIToggle(bool set, string lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIToggle(bool set, Func<string> lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIToggle(Func<bool> set, string lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIToggle(Func<bool> set, Func<string> lbel, Callback<bool> selected, float w = -1f, float h = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIToggle(Func<bool> set, Func<Sprite> checkSet, Callback<bool> selected, Sprite overImage, float w = -1f, float h = -1f)
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
