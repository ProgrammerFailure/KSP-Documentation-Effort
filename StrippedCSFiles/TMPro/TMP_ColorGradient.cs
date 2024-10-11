using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_ColorGradient : ScriptableObject
{
	public Color topLeft;

	public Color topRight;

	public Color bottomLeft;

	public Color bottomRight;

	private static Color k_defaultColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ColorGradient()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ColorGradient(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ColorGradient(Color color0, Color color1, Color color2, Color color3)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_ColorGradient()
	{
		throw null;
	}
}
