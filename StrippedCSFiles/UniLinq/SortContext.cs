using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniLinq;

internal abstract class SortContext<TElement> : IComparer<int>
{
	protected SortDirection direction;

	protected SortContext<TElement> child_context;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected SortContext(SortDirection direction, SortContext<TElement> child_context)
	{
		throw null;
	}

	public abstract void Initialize(TElement[] elements);

	public abstract int Compare(int first_index, int second_index);
}
