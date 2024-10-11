using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro;

[Serializable]
public class KerningPair
{
	[FormerlySerializedAs("AscII_Left")]
	[SerializeField]
	private uint m_FirstGlyph;

	[SerializeField]
	private GlyphValueRecord m_FirstGlyphAdjustments;

	[SerializeField]
	[FormerlySerializedAs("AscII_Right")]
	private uint m_SecondGlyph;

	[SerializeField]
	private GlyphValueRecord m_SecondGlyphAdjustments;

	[FormerlySerializedAs("XadvanceOffset")]
	public float xOffset;

	public uint firstGlyph
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public GlyphValueRecord firstGlyphAdjustments
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint secondGlyph
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public GlyphValueRecord secondGlyphAdjustments
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerningPair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerningPair(uint left, uint right, float offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerningPair(uint firstGlyph, GlyphValueRecord firstGlyphAdjustments, uint secondGlyph, GlyphValueRecord secondGlyphAdjustments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ConvertLegacyKerningData()
	{
		throw null;
	}
}
