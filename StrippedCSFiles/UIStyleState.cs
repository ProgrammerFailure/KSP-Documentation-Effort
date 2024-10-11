using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class UIStyleState
{
	public Sprite background;

	public Color textColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStyleState()
	{
		throw null;
	}
}
