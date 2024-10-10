using System.Collections.Generic;

namespace Smooth.Collections;

public static class HashSet
{
	public static HashSet<T> Create<T>()
	{
		return new HashSet<T>(EqualityComparer<T>.Default);
	}

	public static HashSet<T> Create<T>(IEnumerable<T> collection)
	{
		return new HashSet<T>(collection, EqualityComparer<T>.Default);
	}
}
