using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public static class Dictionary
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Dictionary<K, V> Create<K, V>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Dictionary<K, V> Create<K, V>(int capacity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Dictionary<K, V> Create<K, V>(IDictionary<K, V> dictionary)
	{
		throw null;
	}
}
