using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniLinq;

internal class SortSequenceContext<TElement, TKey> : SortContext<TElement>
{
	private Func<TElement, TKey> selector;

	private IComparer<TKey> comparer;

	private TKey[] keys;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SortSequenceContext(Func<TElement, TKey> selector, IComparer<TKey> comparer, SortDirection direction, SortContext<TElement> child_context)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(TElement[] elements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int Compare(int first_index, int second_index)
	{
		throw null;
	}
}
