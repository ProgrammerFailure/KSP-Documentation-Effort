using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogGUISlider : DialogGUIBase
{
	public float min;

	public float max;

	public bool wholeNumbers;

	public UnityAction<float> actionCallback;

	public Func<float> setValue;

	public Slider slider;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUISlider(Func<float> setValue, float min, float max, bool wholeNumbers, float width, float height, UnityAction<float> setCallback)
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
}
