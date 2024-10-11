using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MultiOptionDialog : DialogGUIBase
{
	public DialogGUIBase[] Options;

	public UISkinDef UISkinDef;

	public string name;

	public string title;

	public string message;

	public Rect dialogRect;

	public Callback DrawCustomContent;

	public int id;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, params DialogGUIBase[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, float width, params DialogGUIBase[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, Rect rct, params DialogGUIBase[] options)
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
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void OnRenderObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void Resize()
	{
		throw null;
	}
}
