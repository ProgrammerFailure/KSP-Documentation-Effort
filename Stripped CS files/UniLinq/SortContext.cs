using System.Collections.Generic;

namespace UniLinq;

public abstract class SortContext<TElement> : IComparer<int>
{
	public SortDirection direction;

	public SortContext<TElement> child_context;

	public SortContext(SortDirection direction, SortContext<TElement> child_context)
	{
		this.direction = direction;
		this.child_context = child_context;
	}

	public abstract void Initialize(TElement[] elements);

	public abstract int Compare(int first_index, int second_index);
}
