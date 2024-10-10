using System;
using System.Collections.Generic;

namespace UniLinq;

public class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
{
	public OrderedEnumerable<TElement> parent;

	public Func<TElement, TKey> selector;

	public IComparer<TKey> comparer;

	public SortDirection direction;

	public OrderedSequence(IEnumerable<TElement> source, Func<TElement, TKey> key_selector, IComparer<TKey> comparer, SortDirection direction)
		: base(source)
	{
		selector = key_selector;
		this.comparer = comparer ?? Comparer<TKey>.Default;
		this.direction = direction;
	}

	public OrderedSequence(OrderedEnumerable<TElement> parent, IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, SortDirection direction)
		: this(source, keySelector, comparer, direction)
	{
		this.parent = parent;
	}

	public override IEnumerator<TElement> GetEnumerator()
	{
		return base.GetEnumerator();
	}

	public override SortContext<TElement> CreateContext(SortContext<TElement> current)
	{
		SortContext<TElement> sortContext = new SortSequenceContext<TElement, TKey>(selector, comparer, direction, current);
		if (parent != null)
		{
			return parent.CreateContext(sortContext);
		}
		return sortContext;
	}

	public override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
	{
		return QuickSort<TElement>.Sort(source, CreateContext(null));
	}
}
