using System.Runtime.CompilerServices;

namespace Smooth.Slinq.Collections;

public struct Grouping<K, T>
{
	public readonly K key;

	public LinkedHeadTail<T> values;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Grouping(K key, LinkedHeadTail<T> values)
	{
		throw null;
	}
}
public struct Grouping<K, T, C>
{
	public readonly K key;

	public Slinq<T, C> values;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Grouping(K key, Slinq<T, C> values)
	{
		throw null;
	}
}
public static class Grouping
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Grouping<K, T> Create<K, T>(K key, LinkedHeadTail<T> values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Grouping<K, T, C> Create<K, T, C>(K key, Slinq<T, C> values)
	{
		throw null;
	}
}
