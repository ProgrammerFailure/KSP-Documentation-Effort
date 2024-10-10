using System.Collections.Generic;

namespace Smooth.Collections;

public static class SortedDictionary
{
	public static SortedDictionary<T, U> Create<T, U>()
	{
		return new SortedDictionary<T, U>(Comparer<T>.Default);
	}

	public static SortedDictionary<T, U> Create<T, U>(IDictionary<T, U> dictionary)
	{
		return new SortedDictionary<T, U>(dictionary, Comparer<T>.Default);
	}
}
