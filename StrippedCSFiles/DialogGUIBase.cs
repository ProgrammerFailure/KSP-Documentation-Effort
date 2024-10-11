using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;

public class DialogGUIBase
{
	public GameObject uiItem;

	public List<DialogGUIBase> children;

	public string OptionText;

	public float width;

	public float height;

	public Vector2 size;

	public Vector2 position;

	public bool useColor;

	public Color tint;

	public UIStyle guiStyle;

	public string tooltipText;

	public bool flexibleHeight;

	private TooltipController_Text toolTip;

	protected bool dirty;

	public Func<bool> OptionEnabledCondition;

	public Func<bool> OptionInteractableCondition;

	public Callback OnUpdate;

	public Callback OnFixedUpdate;

	public Callback OnLateUpdate;

	public Callback OnRenderObject;

	public Callback OnResize;

	protected bool lastEnabledState;

	protected bool lastInteractibleState;

	public bool Dirty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIBase(params DialogGUIBase[] list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddChild(DialogGUIBase child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddChildren(DialogGUIBase[] c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Resize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupTransformAndLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetUpTextObject(TextMeshProUGUI text, string value, UIStyle style, UISkinDef skin, bool ignoreStyleColor = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SelectFirstItem(DialogGUIBase[] items)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOptionText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual TextMeshProUGUI GetTextObject()
	{
		throw null;
	}
}
