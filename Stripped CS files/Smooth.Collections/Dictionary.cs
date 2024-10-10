using System.Collections.Generic;

namespace Smooth.Collections;

public static class Dictionary
{
	public static Dictionary<T, U> Create<T, U>()
	{
		return new Dictionary<T, U>(EqualityComparer<T>.Default);
	}

	public static Dictionary<T, U> Create<T, U>(int capacity)
	{
		return new Dictionary<T, U>(capacity, EqualityComparer<T>.Default);
	}

	public static Dictionary<T, U> Create<T, U>(IDictionary<T, U> dictionary)
	{
		return new Dictionary<T, U>(dictionary, EqualityComparer<T>.Default);
	}
}
