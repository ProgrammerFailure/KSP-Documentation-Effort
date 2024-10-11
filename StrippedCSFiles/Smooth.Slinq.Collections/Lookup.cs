using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Slinq.Context;

namespace Smooth.Slinq.Collections;

public class Lookup<K, T> : IDisposable
{
	private static readonly Stack<Lookup<K, T>> pool;

	public LinkedHeadTail<K> keys;

	public readonly Dictionary<K, LinkedHeadTail<T>> dictionary;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Lookup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Lookup(IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Lookup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Lookup<K, T> Borrow(IEqualityComparer<K> comparer)
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
	public void Add(K key, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(K key, Linked<T> value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(K key, LinkedHeadTail<T> values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> GetValues(K key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> RemoveValues(K key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Lookup<K, T> SortKeys(Comparison<K> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> FlattenAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> SlinqAndKeep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> SlinqAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<Grouping<K, T>, GroupByContext<K, T>> SlinqLinkedAndKeep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<Grouping<K, T>, GroupByContext<K, T>> SlinqLinkedAndDispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, GroupJoinContext<U, K, T, T2, C2>> GroupJoinAndKeep<U, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, K> outerSelector, DelegateFunc<T2, Slinq<T, LinkedContext<T>>, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, GroupJoinContext<U, K, T, T2, C2, P>> GroupJoinAndKeep<U, T2, C2, P>(Slinq<T2, C2> outer, DelegateFunc<T2, P, K> outerSelector, DelegateFunc<T2, Slinq<T, LinkedContext<T>>, P, U> resultSelector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, GroupJoinContext<U, K, T, T2, C2>> GroupJoinAndDispose<U, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, K> outerSelector, DelegateFunc<T2, Slinq<T, LinkedContext<T>>, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, GroupJoinContext<U, K, T, T2, C2, P>> GroupJoinAndDispose<U, T2, C2, P>(Slinq<T2, C2> outer, DelegateFunc<T2, P, K> outerSelector, DelegateFunc<T2, Slinq<T, LinkedContext<T>>, P, U> resultSelector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, JoinContext<U, K, T, T2, C2>> JoinAndKeep<U, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, K> outerSelector, DelegateFunc<T2, T, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, JoinContext<U, K, T, T2, C2, P>> JoinAndKeep<U, T2, C2, P>(Slinq<T2, C2> outer, DelegateFunc<T2, P, K> outerSelector, DelegateFunc<T2, T, P, U> resultSelector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, JoinContext<U, K, T, T2, C2>> JoinAndDispose<U, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, K> outerSelector, DelegateFunc<T2, T, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<U, JoinContext<U, K, T, T2, C2, P>> JoinAndDispose<U, T2, C2, P>(Slinq<T2, C2> outer, DelegateFunc<T2, P, K> outerSelector, DelegateFunc<T2, T, P, U> resultSelector, P parameter)
	{
		throw null;
	}
}
