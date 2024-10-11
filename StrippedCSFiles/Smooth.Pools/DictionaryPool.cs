using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public static class DictionaryPool<K, V>
{
	private static readonly KeyedPoolWithDefaultKey<IEqualityComparer<K>, Dictionary<K, V>> _Instance;

	public static KeyedPoolWithDefaultKey<IEqualityComparer<K>, Dictionary<K, V>> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DictionaryPool()
	{
		throw null;
	}
}
