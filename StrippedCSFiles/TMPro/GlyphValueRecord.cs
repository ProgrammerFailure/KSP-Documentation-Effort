using System;
using System.Runtime.CompilerServices;

namespace TMPro;

[Serializable]
public struct GlyphValueRecord
{
	public float xPlacement;

	public float yPlacement;

	public float xAdvance;

	public float yAdvance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GlyphValueRecord operator +(GlyphValueRecord a, GlyphValueRecord b)
	{
		throw null;
	}
}
