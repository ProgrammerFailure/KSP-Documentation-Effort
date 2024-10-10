using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq;

public abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
{
	public IEnumerable<TElement> source;

	public OrderedEnumerable(IEnumerable<TElement> source)
	{
		this.source = source;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual IEnumerator<TElement> GetEnumerator()
	{
		return Sort(source).GetEnumerator();
	}

	public abstract SortContext<TElement> CreateContext(SortContext<TElement> current);

	public abstract IEnumerable<TElement> Sort(IEnumerable<TElement> source);

	public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending)
	{
		return new OrderedSequence<TElement, TKey>(this, source, selector, comparer, descending ? SortDirection.Descending : SortDirection.Ascending);
	}
}
