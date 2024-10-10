using System.Collections.Generic;
using Smooth.Collections;

namespace Smooth.Pools;

public static class HashSetPool<T>
{
	public static readonly KeyedPoolWithDefaultKey<IEqualityComparer<T>, HashSet<T>> _Instance = new KeyedPoolWithDefaultKey<IEqualityComparer<T>, HashSet<T>>((IEqualityComparer<T> comparer) => new HashSet<T>(comparer), delegate(HashSet<T> hashSet)
	{
		hashSet.Clear();
		return hashSet.Comparer;
	}, () => Smooth.Collections.EqualityComparer<T>.Default);

	public static KeyedPoolWithDefaultKey<IEqualityComparer<T>, HashSet<T>> Instance => _Instance;
}
