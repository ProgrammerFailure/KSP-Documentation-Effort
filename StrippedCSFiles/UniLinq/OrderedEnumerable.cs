using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniLinq;

internal abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
{
	private IEnumerable<TElement> source;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected OrderedEnumerable(IEnumerable<TElement> source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerator<TElement> GetEnumerator()
	{
		throw null;
	}

	public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

	protected abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
	{
		throw null;
	}
}
