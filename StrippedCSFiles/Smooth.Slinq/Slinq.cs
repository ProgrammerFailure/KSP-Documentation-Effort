using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;

namespace Smooth.Slinq;

public struct Slinq<T, C>
{
	public readonly Mutator<T, C> skip;

	public readonly Mutator<T, C> remove;

	public readonly Mutator<T, C> dispose;

	public C context;

	public Option<T> current;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq(Mutator<T, C> skip, Mutator<T, C> remove, Mutator<T, C> dispose, C context)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Skip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> SkipAndReturn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveAndReturn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SkipAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> Skip(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> SkipWhile(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> SkipWhile<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U SkipWhile<U>(U seed, DelegateFunc<U, T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U SkipWhile<U, P>(U seed, DelegateFunc<U, T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int RemoveAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int RemoveAll(DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int RemoveAll<P>(DelegateAction<T, P> then, P thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> Remove(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> Remove(int count, DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> Remove<P>(int count, DelegateAction<T, P> then, P thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile(DelegateFunc<T, bool> predicate, DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile<P>(DelegateFunc<T, bool> predicate, DelegateAction<T, P> then, P thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile<P>(DelegateFunc<T, P, bool> predicate, P parameter, DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Slinq<T, C> RemoveWhile<P, P2>(DelegateFunc<T, P, bool> predicate, P parameter, DelegateAction<T, P2> then, P2 thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U>(U seed, DelegateFunc<U, T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U>(U seed, DelegateFunc<U, T, Option<U>> selector, DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U, P>(U seed, DelegateFunc<U, T, Option<U>> selector, DelegateAction<T, P> then, P thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U, P>(U seed, DelegateFunc<U, T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U, P>(U seed, DelegateFunc<U, T, P, Option<U>> selector, P parameter, DelegateAction<T> then)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U RemoveWhile<U, P, P2>(U seed, DelegateFunc<U, T, P, Option<U>> selector, P parameter, DelegateAction<T, P2> then, P2 thenParameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Aggregate(DelegateFunc<T, T, T> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> AggregateOrNone(DelegateFunc<T, T, T> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> AggregateOrNone<P>(DelegateFunc<T, T, P, T> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Aggregate<U>(U seed, DelegateFunc<U, T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public V Aggregate<U, V>(U seed, DelegateFunc<U, T, U> selector, DelegateFunc<U, V> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Aggregate<U, P>(U seed, DelegateFunc<U, T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U AggregateWhile<U>(U seed, DelegateFunc<U, T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U AggregateWhile<U, P>(U seed, DelegateFunc<U, T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool All(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool All<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Any()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Any(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Any<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(T value, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Count()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T ElementAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T ElementAtOrDefault(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> ElementAtOrNone(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T First()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T First(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FirstOrDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T FirstOrDefault(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> FirstOrNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> FirstOrNone(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> FirstOrNone<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach(DelegateAction<T> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach<P>(DelegateAction<T, P> action, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Last()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Last(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T LastOrDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T LastOrDefault(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> LastOrNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> LastOrNone(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> LastOrNone<P>(DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Max()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone(IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone(Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K>(DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K>(DelegateFunc<T, K> selector, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K>(DelegateFunc<T, K> selector, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MaxOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Min()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Min(IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone(Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K>(DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K>(DelegateFunc<T, K> selector, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K>(DelegateFunc<T, K> selector, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> MinOrNone<K, P>(DelegateFunc<T, P, K> selector, P parameter, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SequenceEqual<C2>(Slinq<T, C2> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SequenceEqual<C2>(Slinq<T, C2> other, EqualityComparer<T> equalityComparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SequenceEqual<T2, C2>(Slinq<T2, C2> other, DelegateFunc<T, T2, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Single()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T SingleOrDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> SingleOrNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>> SplitRight(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IC AddTo<IC>(IC collection) where IC : ICollection<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<IC> AddTo<IC>(Disposable<IC> collection) where IC : ICollection<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IC AddTo<U, IC>(IC collection, DelegateFunc<T, U> selector) where IC : ICollection<U>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<IC> AddTo<U, IC>(Disposable<IC> collection, DelegateFunc<T, U> selector) where IC : ICollection<U>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IC AddTo<U, IC, P>(IC collection, DelegateFunc<T, P, U> selector, P parameter) where IC : ICollection<U>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<IC> AddTo<U, IC, P>(Disposable<IC> collection, DelegateFunc<T, P, U> selector, P parameter) where IC : ICollection<U>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> AddTo(LinkedHeadTail<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> AddToReverse(LinkedHeadTail<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> AddTo<K>(LinkedHeadTail<K, T> list, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> AddTo<K, P>(LinkedHeadTail<K, T> list, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> AddToReverse<K>(LinkedHeadTail<K, T> list, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> AddToReverse<K, P>(LinkedHeadTail<K, T> list, DelegateFunc<T, P, K> selector, P parameter)
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
	public LinkedHeadTail<T> ToLinked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<T> ToLinkedReverse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> ToLinked<K>(DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> ToLinked<K, P>(DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> ToLinkedReverse<K>(DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedHeadTail<K, T> ToLinkedReverse<K, P>(DelegateFunc<T, P, K> selector, P parameter)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> ToList()
	{
		throw null;
	}
}
public static class Slinq
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, AggregateContext<U, T, C>> AggregateRunning<U, T, C>(this Slinq<T, C> slinq, U seed, DelegateFunc<U, T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, AggregateContext<U, T, C, P>> AggregateRunning<U, T, C, P>(this Slinq<T, C> slinq, U seed, DelegateFunc<U, T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, ConcatContext<C2, T, C>> Concat<C2, T, C>(this Slinq<T, C> first, Slinq<T, C2> second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, EitherContext<OptionContext<T>, T, C>> DefaultIfEmpty<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, EitherContext<OptionContext<T>, T, C>> DefaultIfEmpty<T, C>(this Slinq<T, C> slinq, T defaultValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Distinct<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Distinct<T, C>(this Slinq<T, C> slinq, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Distinct<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Distinct<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Distinct<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Distinct<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Except<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Except<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Except<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Except<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Except<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Except<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FlattenContext<T, C1, C2>> Flatten<T, C1, C2>(this Slinq<Slinq<T, C1>, C2> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FlattenContext<T, C>> Flatten<T, C>(this Slinq<Option<T>, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> GroupBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> GroupBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> GroupBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> GroupBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C>> GroupJoin<U, K, T2, C2, T, C>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, K> outerSelector, DelegateFunc<T2, K> innerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C>> GroupJoin<U, K, T2, C2, T, C>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, K> outerSelector, DelegateFunc<T2, K> innerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, U> resultSelector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C, P>> GroupJoin<U, K, T2, C2, T, C, P>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T2, P, K> innerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, P, U> resultSelector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C, P>> GroupJoin<U, K, T2, C2, T, C, P>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T2, P, K> innerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, P, U> resultSelector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Intersect<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Intersect<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Intersect<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Intersect<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Intersect<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Intersect<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, JoinContext<U, K, T2, T, C>> Join<U, K, T2, C2, T, C>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, K> outerSelector, DelegateFunc<T2, K> innerSelector, DelegateFunc<T, T2, U> resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, JoinContext<U, K, T2, T, C>> Join<U, K, T2, C2, T, C>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, K> outerSelector, DelegateFunc<T2, K> innerSelector, DelegateFunc<T, T2, U> resultSelector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, JoinContext<U, K, T2, T, C, P>> Join<U, K, T2, C2, T, C, P>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T2, P, K> innerSelector, DelegateFunc<T, T2, P, U> resultSelector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, JoinContext<U, K, T2, T, C, P>> Join<U, K, T2, C2, T, C, P>(this Slinq<T, C> outer, Slinq<T2, C2> inner, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T2, P, K> innerSelector, DelegateFunc<T, T2, P, U> resultSelector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderBy<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderBy<T, C>(this Slinq<T, C> slinq, IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderBy<T, C>(this Slinq<T, C> slinq, Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, C>(this Slinq<T, C> slinq, IComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, C>(this Slinq<T, C> slinq, Comparison<T> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderBy<T, C>(this Slinq<T, C> slinq, Comparison<T> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, Comparison<K> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderByDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> OrderBy<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Comparison<K> comparison, bool ascending)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> equalityComparer, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> equalityComparer, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> equalityComparer, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, K> selector, IEqualityComparer<K> equalityComparer, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> equalityComparer, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroup<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> equalityComparer, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> equalityComparer, IComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> OrderByGroupDescending<K, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> equalityComparer, Comparison<K> comparison)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> Reverse<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectContext<U, T, C>> Select<U, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectContext<U, T, C, P>> Select<U, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectSlinqContext<U, UC, T, C>> SelectMany<U, UC, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, Slinq<U, UC>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectSlinqContext<U, UC, T, C, P>> SelectMany<U, UC, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, Slinq<U, UC>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectOptionContext<U, T, C>> SelectMany<U, T, C>(this Slinq<T, C> slinq, DelegateFunc<T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectOptionContext<U, T, C, P>> SelectMany<U, T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IntContext<T, C>> Take<T, C>(this Slinq<T, C> slinq, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> TakeRight<T, C>(this Slinq<T, C> slinq, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C>> TakeWhile<T, C>(this Slinq<T, C> slinq, DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C, P>> TakeWhile<T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C>> Where<T, C>(this Slinq<T, C> slinq, DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C, P>> Where<T, C, P>(this Slinq<T, C> slinq, DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, ConcatContext<C2, T, C>>> Union<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, ConcatContext<C2, T, C>>> Union<C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, ConcatContext<C2, T, C>>> Union<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, ConcatContext<C2, T, C>>> Union<K, C2, T, C>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, K> selector, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, ConcatContext<C2, T, C>, P>> Union<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, ConcatContext<C2, T, C>, P>> Union<K, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T, C2> other, DelegateFunc<T, P, K> selector, P parameter, IEqualityComparer<K> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Smooth.Algebraics.Tuple<T, T2>, ZipContext<T2, C2, T, C>> Zip<T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Smooth.Algebraics.Tuple<T, T2>, ZipContext<T2, C2, T, C>> Zip<T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C>> Zip<U, T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<T, T2, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C>> Zip<U, T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<T, T2, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C, P>> Zip<U, T2, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<T, T2, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C, P>> Zip<U, T2, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<T, T2, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Smooth.Algebraics.Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> ZipAll<T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Smooth.Algebraics.Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> ZipAll<T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C>> ZipAll<U, T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<Option<T>, Option<T2>, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C>> ZipAll<U, T2, C2, T, C>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<Option<T>, Option<T2>, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C, P>> ZipAll<U, T2, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<Option<T>, Option<T2>, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C, P>> ZipAll<U, T2, C2, T, C, P>(this Slinq<T, C> slinq, Slinq<T2, C2> with, DelegateFunc<Option<T>, Option<T2>, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Smooth.Algebraics.Tuple<T, int>, ZipContext<int, FuncContext<int, int>, T, C>> ZipWithIndex<T, C>(this Slinq<T, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Average<C>(this Slinq<int, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Average<C>(this Slinq<long, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float Average<C>(this Slinq<float, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Average<C>(this Slinq<double, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<double> AverageOrNone<C>(this Slinq<int, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<double> AverageOrNone<C>(this Slinq<long, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<float> AverageOrNone<C>(this Slinq<float, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<double> AverageOrNone<C>(this Slinq<double, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Sum<C>(this Slinq<int, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long Sum<C>(this Slinq<long, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float Sum<C>(this Slinq<float, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Sum<C>(this Slinq<double, C> slinq)
	{
		throw null;
	}
}
