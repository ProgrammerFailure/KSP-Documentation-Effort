using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniLinq;

internal class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
{
	private K key;

	private IEnumerable<T> group;

	public K Key
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Grouping(K key, IEnumerable<T> group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
