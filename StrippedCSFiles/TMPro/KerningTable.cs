using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TMPro;

[Serializable]
public class KerningTable
{
	public List<KerningPair> kerningPairs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerningTable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddKerningPair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int AddKerningPair(uint first, uint second, float offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int AddGlyphPairAdjustmentRecord(uint first, GlyphValueRecord firstAdjustments, uint second, GlyphValueRecord secondAdjustments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveKerningPair(int left, int right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveKerningPair(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortKerningPairs()
	{
		throw null;
	}
}
