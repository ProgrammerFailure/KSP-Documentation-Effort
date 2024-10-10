using System;
using System.Collections.Generic;
using Smooth.Delegates;
using Smooth.Dispose;
using Smooth.Slinq.Context;
using UnityEngine;

namespace Smooth.Slinq.Collections;

public class Lookup<T, U> : IDisposable
{
	public static readonly Stack<Lookup<T, U>> pool = new Stack<Lookup<T, U>>();

	public LinkedHeadTail<T> keys;

	public readonly Dictionary<T, LinkedHeadTail<U>> dictionary;

	public Lookup()
	{
	}

	public Lookup(IEqualityComparer<T> comparer)
	{
		dictionary = new Dictionary<T, LinkedHeadTail<U>>(comparer);
		keys = default(LinkedHeadTail<T>);
	}

	public static Lookup<T, U> Borrow(IEqualityComparer<T> comparer)
	{
		lock (pool)
		{
			return (pool.Count > 0) ? pool.Pop() : new Lookup<T, U>(comparer);
		}
	}

	public void DisposeInBackground()
	{
		DisposalQueue.Enqueue(this);
	}

	public void Dispose()
	{
		LinkedHeadTail<U> linkedHeadTail = default(LinkedHeadTail<U>);
		for (Linked<T> linked = keys.head; linked != null; linked = linked.next)
		{
			linkedHeadTail.Append(RemoveValues(linked.value));
		}
		if (dictionary.Count > 0)
		{
			Debug.LogWarning("Lookup had dictionary keys that were not in the key list.");
			foreach (LinkedHeadTail<U> value in dictionary.Values)
			{
				linkedHeadTail.Append(value);
			}
			dictionary.Clear();
		}
		keys.Dispose();
		linkedHeadTail.Dispose();
		lock (pool)
		{
			pool.Push(this);
		}
	}

	public void Add(T key, U value)
	{
		if (!dictionary.TryGetValue(key, out var value2))
		{
			keys.Append(key);
		}
		value2.Append(value);
		dictionary[key] = value2;
	}

	public void Add(T key, Linked<U> value)
	{
		if (!dictionary.TryGetValue(key, out var value2))
		{
			keys.Append(key);
		}
		value2.Append(value);
		dictionary[key] = value2;
	}

	public void Add(T key, LinkedHeadTail<U> values)
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			value.Append(values);
			dictionary[key] = value;
		}
		else
		{
			keys.Append(key);
			dictionary[key] = values;
		}
	}

	public LinkedHeadTail<U> GetValues(T key)
	{
		dictionary.TryGetValue(key, out var value);
		return value;
	}

	public LinkedHeadTail<U> RemoveValues(T key)
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			dictionary.Remove(key);
		}
		return value;
	}

	public Lookup<T, U> SortKeys(Comparison<T> comparison, bool ascending)
	{
		keys = Linked.Sort(keys, comparison, ascending);
		return this;
	}

	public LinkedHeadTail<U> FlattenAndDispose()
	{
		LinkedHeadTail<U> result = default(LinkedHeadTail<U>);
		for (Linked<T> linked = keys.head; linked != null; linked = linked.next)
		{
			result.Append(RemoveValues(linked.value));
		}
		keys.DisposeInBackground();
		DisposeInBackground();
		return result;
	}

	public Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> SlinqAndKeep()
	{
		return GroupByContext<T, U>.Slinq(this, release: false);
	}

	public Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> SlinqAndDispose()
	{
		return GroupByContext<T, U>.Slinq(this, release: true);
	}

	public Slinq<Grouping<T, U>, GroupByContext<T, U>> SlinqLinkedAndKeep()
	{
		return GroupByContext<T, U>.SlinqLinked(this, release: false);
	}

	public Slinq<Grouping<T, U>, GroupByContext<T, U>> SlinqLinkedAndDispose()
	{
		return GroupByContext<T, U>.SlinqLinked(this, release: true);
	}

	public Slinq<V, GroupJoinContext<V, T, U, T2, C2>> GroupJoinAndKeep<V, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, T> outerSelector, DelegateFunc<T2, Slinq<U, LinkedContext<U>>, V> resultSelector)
	{
		return GroupJoinContext<V, T, U, T2, C2>.GroupJoin(this, outer, outerSelector, resultSelector, release: false);
	}

	public Slinq<V, GroupJoinContext<V, T, U, T2, C2, W>> GroupJoinAndKeep<V, T2, C2, W>(Slinq<T2, C2> outer, DelegateFunc<T2, W, T> outerSelector, DelegateFunc<T2, Slinq<U, LinkedContext<U>>, W, V> resultSelector, W parameter)
	{
		return GroupJoinContext<V, T, U, T2, C2, W>.GroupJoin(this, outer, outerSelector, resultSelector, parameter, release: false);
	}

	public Slinq<V, GroupJoinContext<V, T, U, T2, C2>> GroupJoinAndDispose<V, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, T> outerSelector, DelegateFunc<T2, Slinq<U, LinkedContext<U>>, V> resultSelector)
	{
		return GroupJoinContext<V, T, U, T2, C2>.GroupJoin(this, outer, outerSelector, resultSelector, release: true);
	}

	public Slinq<V, GroupJoinContext<V, T, U, T2, C2, W>> GroupJoinAndDispose<V, T2, C2, W>(Slinq<T2, C2> outer, DelegateFunc<T2, W, T> outerSelector, DelegateFunc<T2, Slinq<U, LinkedContext<U>>, W, V> resultSelector, W parameter)
	{
		return GroupJoinContext<V, T, U, T2, C2, W>.GroupJoin(this, outer, outerSelector, resultSelector, parameter, release: true);
	}

	public Slinq<V, JoinContext<V, T, U, T2, C2>> JoinAndKeep<V, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, T> outerSelector, DelegateFunc<T2, U, V> resultSelector)
	{
		return JoinContext<V, T, U, T2, C2>.Join(this, outer, outerSelector, resultSelector, release: false);
	}

	public Slinq<V, JoinContext<V, T, U, T2, C2, W>> JoinAndKeep<V, T2, C2, W>(Slinq<T2, C2> outer, DelegateFunc<T2, W, T> outerSelector, DelegateFunc<T2, U, W, V> resultSelector, W parameter)
	{
		return JoinContext<V, T, U, T2, C2, W>.Join(this, outer, outerSelector, resultSelector, parameter, release: false);
	}

	public Slinq<V, JoinContext<V, T, U, T2, C2>> JoinAndDispose<V, T2, C2>(Slinq<T2, C2> outer, DelegateFunc<T2, T> outerSelector, DelegateFunc<T2, U, V> resultSelector)
	{
		return JoinContext<V, T, U, T2, C2>.Join(this, outer, outerSelector, resultSelector, release: true);
	}

	public Slinq<V, JoinContext<V, T, U, T2, C2, W>> JoinAndDispose<V, T2, C2, W>(Slinq<T2, C2> outer, DelegateFunc<T2, W, T> outerSelector, DelegateFunc<T2, U, W, V> resultSelector, W parameter)
	{
		return JoinContext<V, T, U, T2, C2, W>.Join(this, outer, outerSelector, resultSelector, parameter, release: true);
	}
}
