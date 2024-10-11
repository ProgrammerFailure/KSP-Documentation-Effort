using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Slinq.Context;

namespace Smooth.Slinq.Collections;

public struct LinkedHeadTail<T> : IEquatable<LinkedHeadTail<T>>
{
	public Linked<T> head;

	public Linked<T> tail;

	public int count;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail(Linked<T> head)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(LinkedHeadTail<T> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(LinkedHeadTail<T> lhs, LinkedHeadTail<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(LinkedHeadTail<T> lhs, LinkedHeadTail<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(Linked<T> node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(LinkedHeadTail<T> other)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<T>> SlinqAndKeep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<T>> SlinqAndKeep(BacktrackDetector bd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<T>> SlinqAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> AddTo<K>(Lookup<K, T> lookup, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> AddTo<K, P>(Lookup<K, T> lookup, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> ToLookup<K>(DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> ToLookup<K>(DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> ToLookup<K, P>(DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> ToLookup<K, P>(DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}
}
public struct LinkedHeadTail<K, T> : IEquatable<LinkedHeadTail<K, T>>
{
	public Linked<K, T> head;

	public Linked<K, T> tail;

	public int count;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail(K key, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail(Linked<K, T> head)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(LinkedHeadTail<K, T> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(LinkedHeadTail<K, T> lhs, LinkedHeadTail<K, T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(LinkedHeadTail<K, T> lhs, LinkedHeadTail<K, T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(K key, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(Linked<K, T> node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Append(LinkedHeadTail<K, T> other)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<K, T>> SlinqAndKeep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<K, T>> SlinqAndKeep(BacktrackDetector bd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, LinkedContext<K, T>> SlinqAndDispose()
	{
		throw null;
	}
}
