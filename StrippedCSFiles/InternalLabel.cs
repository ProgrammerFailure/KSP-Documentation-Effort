using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalLabel : InternalModule
{
	[KSPField]
	public string transformName;

	[KSPField]
	public string text;

	[KSPField]
	public string textFont;

	[KSPField]
	public float textSize;

	[KSPField]
	public bool textWrapping;

	[KSPField]
	public Color textColor;

	[KSPField]
	public string textAlign;

	public Transform labelTransform;

	public InternalText textObj;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}
}
