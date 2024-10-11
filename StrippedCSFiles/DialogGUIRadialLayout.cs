using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIRadialLayout : DialogGUILayoutBase
{
	public float radius;

	public float increment;

	public float startangle;

	protected Image[] backgrounds;

	private int selectedIndex;

	private static float minMag;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIRadialLayout(float radius, float increment, float startangle, params DialogGUIBase[] items)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DialogGUIRadialLayout()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int SelectedItemAtAngle(float angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}
}
