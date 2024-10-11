using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public struct VertexGradient
{
	public Color topLeft;

	public Color topRight;

	public Color bottomLeft;

	public Color bottomRight;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VertexGradient(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VertexGradient(Color color0, Color color1, Color color2, Color color3)
	{
		throw null;
	}
}
