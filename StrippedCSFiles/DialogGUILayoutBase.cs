using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogGUILayoutBase : DialogGUIBase
{
	public bool stretchWidth;

	public bool stretchHeight;

	public float spacing;

	public RectOffset padding;

	public TextAnchor anchor;

	public float minWidth;

	public float minHeight;

	public bool useParent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUILayoutBase(params DialogGUIBase[] list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		throw null;
	}
}
