using System.Collections.Generic;

namespace Smooth.Collections;

public static class SortedList
{
	public static SortedList<T, U> Create<T, U>()
	{
		return new SortedList<T, U>(Comparer<T>.Default);
	}

	public static SortedList<T, U> Create<T, U>(int capacity)
	{
		return new SortedList<T, U>(capacity, Comparer<T>.Default);
	}

	public static SortedList<T, U> Create<T, U>(IDictionary<T, U> dictionary)
	{
		return new SortedList<T, U>(dictionary, Comparer<T>.Default);
	}
}
