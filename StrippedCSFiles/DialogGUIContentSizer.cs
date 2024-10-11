using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIContentSizer : DialogGUIBase
{
	protected ContentSizeFitter.FitMode widthMode;

	protected ContentSizeFitter.FitMode heightMode;

	protected bool useParentSize;

	protected ContentSizeFitter fitter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIContentSizer(ContentSizeFitter.FitMode widthMode, ContentSizeFitter.FitMode heightMode, bool useParentSize = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
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
}
