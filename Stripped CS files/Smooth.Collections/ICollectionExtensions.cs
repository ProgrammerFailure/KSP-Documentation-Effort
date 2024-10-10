using System.Collections.Generic;

namespace Smooth.Collections;

public static class ICollectionExtensions
{
	public static T AddAll<T, U>(this T collection, params U[] values) where T : ICollection<U>
	{
		for (int i = 0; i < values.Length; i++)
		{
			collection.Add(values[i]);
		}
		return collection;
	}

	public static T AddAll<T, U>(this T collection, IList<U> values) where T : ICollection<U>
	{
		for (int i = 0; i < values.Count; i++)
		{
			collection.Add(values[i]);
		}
		return collection;
	}

	public static T AddAll<T, U>(this T collection, IEnumerable<U> values) where T : ICollection<U>
	{
		foreach (U value in values)
		{
			collection.Add(value);
		}
		return collection;
	}
}
