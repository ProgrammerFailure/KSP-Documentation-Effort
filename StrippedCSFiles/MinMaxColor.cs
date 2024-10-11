using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MinMaxColor
{
	public Color min;

	public Color max;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MinMaxColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color GetLerp(float t)
	{
		throw null;
	}
}
