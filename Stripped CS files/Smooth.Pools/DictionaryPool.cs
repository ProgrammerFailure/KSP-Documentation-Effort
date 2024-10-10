using System.Collections.Generic;
using Smooth.Collections;

namespace Smooth.Pools;

public static class DictionaryPool<T, U>
{
	public static readonly KeyedPoolWithDefaultKey<IEqualityComparer<T>, Dictionary<T, U>> _Instance = new KeyedPoolWithDefaultKey<IEqualityComparer<T>, Dictionary<T, U>>((IEqualityComparer<T> comparer) => new Dictionary<T, U>(comparer), delegate(Dictionary<T, U> dictionary)
	{
		dictionary.Clear();
		return dictionary.Comparer;
	}, () => Smooth.Collections.EqualityComparer<T>.Default);

	public static KeyedPoolWithDefaultKey<IEqualityComparer<T>, Dictionary<T, U>> Instance => _Instance;
}
