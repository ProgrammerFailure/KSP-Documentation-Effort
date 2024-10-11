using System;
using System.Runtime.CompilerServices;

namespace Smooth.Slinq.Collections;

public class Linked<T> : IDisposable
{
	private static object poolLock;

	private static Linked<T> pool;

	public Linked<T> next;

	public T value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Linked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Linked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Linked<T> Borrow(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TrimAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisposeInBackground()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
public class Linked<K, T> : IDisposable
{
	private static object poolLock;

	private static Linked<K, T> pool;

	public Linked<K, T> next;

	public K key;

	public T value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Linked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Linked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Linked<K, T> Borrow(K key, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TrimAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisposeInBackground()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
public static class Linked
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<T> Reverse<T>(this LinkedHeadTail<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<K, T> Reverse<K, T>(this LinkedHeadTail<K, T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<T> Sort<T>(LinkedHeadTail<T> input, Comparison<T> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<K, T> Sort<K, T>(LinkedHeadTail<K, T> input, Comparison<K> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<T> Merge<T>(LinkedHeadTail<T> left, LinkedHeadTail<T> right, Comparison<T> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<K, T> Merge<K, T>(LinkedHeadTail<K, T> left, LinkedHeadTail<K, T> right, Comparison<K> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<T> InsertionSort<T>(LinkedHeadTail<T> input, Comparison<T> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LinkedHeadTail<K, T> InsertionSort<K, T>(LinkedHeadTail<K, T> input, Comparison<K> comparison, bool ascending)
	{
		throw null;
	}
}
