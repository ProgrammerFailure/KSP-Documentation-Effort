using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Cursors;

[Serializable]
public class TextureCursor : CustomCursor
{
	public Texture2D texture;

	public Vector2 hotspot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextureCursor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetCursor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Unset()
	{
		throw null;
	}
}
