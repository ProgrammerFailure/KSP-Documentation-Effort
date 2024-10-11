using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIScrollList : DialogGUIBase
{
	public DialogGUILayoutBase layout;

	public bool hScrollBar;

	public bool vScrollBar;

	protected ScrollRect scrollRect;

	protected GameObject content;

	protected Vector2 contentSize;

	internal GameObject Content
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIScrollList(Vector2 size, bool hScroll, bool vScroll, DialogGUILayoutBase layout)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIScrollList(Vector2 size, Vector2 contentSize, bool hScroll, bool vScroll, DialogGUILayoutBase layout)
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
