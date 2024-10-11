using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public static class HashSetPool<T>
{
	private static readonly KeyedPoolWithDefaultKey<IEqualityComparer<T>, HashSet<T>> _Instance;

	public static KeyedPoolWithDefaultKey<IEqualityComparer<T>, HashSet<T>> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static HashSetPool()
	{
		throw null;
	}
}
