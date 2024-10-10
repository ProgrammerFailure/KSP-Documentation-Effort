using System.Collections.Generic;
using Smooth.Algebraics;

namespace Smooth.Collections;

public static class IDictionaryExtensions
{
	public static Option<U> TryGet<T, U>(this IDictionary<T, U> dictionary, T key)
	{
		if (!dictionary.TryGetValue(key, out var value))
		{
			return default(Option<U>);
		}
		return new Option<U>(value);
	}
}
