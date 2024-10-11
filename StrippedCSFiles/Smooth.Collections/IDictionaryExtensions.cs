using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Collections;

public static class IDictionaryExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<V> TryGet<K, V>(this IDictionary<K, V> dictionary, K key)
	{
		throw null;
	}
}
