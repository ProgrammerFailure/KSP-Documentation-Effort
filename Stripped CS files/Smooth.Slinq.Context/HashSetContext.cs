using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Slinq.Context;

public struct HashSetContext<T, U>
{
	public bool needsMove;

	public Slinq<T, U> chained;

	public readonly Disposable<HashSet<T>> hashSet;

	public readonly bool release;

	public BacktrackDetector bd;

	public static readonly Mutator<T, HashSetContext<T, U>> dispose = Dispose;

	public static readonly Mutator<T, HashSetContext<T, U>> distinctSkip = DistinctSkip;

	public static readonly Mutator<T, HashSetContext<T, U>> distinctRemove = DistinctRemove;

	public static readonly Mutator<T, HashSetContext<T, U>> exceptSkip = ExceptSkip;

	public static readonly Mutator<T, HashSetContext<T, U>> exceptRemove = ExceptRemove;

	public static readonly Mutator<T, HashSetContext<T, U>> intersectSkip = IntersectSkip;

	public static readonly Mutator<T, HashSetContext<T, U>> intersectRemove = IntersectRemove;

	public HashSetContext(Slinq<T, U> chained, Disposable<HashSet<T>> hashSet, bool release)
	{
		needsMove = false;
		this.chained = chained;
		this.hashSet = hashSet;
		this.release = release;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, HashSetContext<T, U>> Distinct(Slinq<T, U> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<T, HashSetContext<T, U>>(distinctSkip, distinctRemove, dispose, new HashSetContext<T, U>(slinq, hashSet, release));
	}

	public static Slinq<T, HashSetContext<T, U>> Except(Slinq<T, U> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<T, HashSetContext<T, U>>(exceptSkip, exceptRemove, dispose, new HashSetContext<T, U>(slinq, hashSet, release));
	}

	public static Slinq<T, HashSetContext<T, U>> Intersect(Slinq<T, U> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<T, HashSetContext<T, U>>(intersectSkip, intersectRemove, dispose, new HashSetContext<T, U>(slinq, hashSet, release));
	}

	public static void Dispose(ref HashSetContext<T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.hashSet.Dispose();
		}
	}

	public static void DistinctSkip(ref HashSetContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Add(context.chained.current.value))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void DistinctRemove(ref HashSetContext<T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		DistinctSkip(ref context, out next);
	}

	public static void ExceptSkip(ref HashSetContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && context.hashSet.value.Contains(context.chained.current.value))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void ExceptRemove(ref HashSetContext<T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		ExceptSkip(ref context, out next);
	}

	public static void IntersectSkip(ref HashSetContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Remove(context.chained.current.value))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void IntersectRemove(ref HashSetContext<T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		IntersectSkip(ref context, out next);
	}
}
public struct HashSetContext<T, U, V>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, T> selector;

	public readonly Disposable<HashSet<T>> hashSet;

	public readonly bool release;

	public BacktrackDetector bd;

	public static readonly Mutator<U, HashSetContext<T, U, V>> dispose = Dispose;

	public static readonly Mutator<U, HashSetContext<T, U, V>> distinctSkip = DistinctSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V>> distinctRemove = DistinctRemove;

	public static readonly Mutator<U, HashSetContext<T, U, V>> exceptSkip = ExceptSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V>> exceptRemove = ExceptRemove;

	public static readonly Mutator<U, HashSetContext<T, U, V>> intersectSkip = IntersectSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V>> intersectRemove = IntersectRemove;

	public HashSetContext(Slinq<U, V> chained, DelegateFunc<U, T> selector, Disposable<HashSet<T>> hashSet, bool release)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		this.hashSet = hashSet;
		this.release = release;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<U, HashSetContext<T, U, V>> Distinct(Slinq<U, V> slinq, DelegateFunc<U, T> selector, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V>>(distinctSkip, distinctRemove, dispose, new HashSetContext<T, U, V>(slinq, selector, hashSet, release));
	}

	public static Slinq<U, HashSetContext<T, U, V>> Except(Slinq<U, V> slinq, DelegateFunc<U, T> selector, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V>>(exceptSkip, exceptRemove, dispose, new HashSetContext<T, U, V>(slinq, selector, hashSet, release));
	}

	public static Slinq<U, HashSetContext<T, U, V>> Intersect(Slinq<U, V> slinq, DelegateFunc<U, T> selector, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V>>(intersectSkip, intersectRemove, dispose, new HashSetContext<T, U, V>(slinq, selector, hashSet, release));
	}

	public static void Dispose(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		next = default(Option<U>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.hashSet.Dispose();
		}
	}

	public static void DistinctSkip(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Add(context.selector(context.chained.current.value)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void DistinctRemove(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		DistinctSkip(ref context, out next);
	}

	public static void ExceptSkip(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && context.hashSet.value.Contains(context.selector(context.chained.current.value)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void ExceptRemove(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		ExceptSkip(ref context, out next);
	}

	public static void IntersectSkip(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Remove(context.selector(context.chained.current.value)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void IntersectRemove(ref HashSetContext<T, U, V> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		IntersectSkip(ref context, out next);
	}
}
public struct HashSetContext<T, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, W, T> selector;

	public readonly W parameter;

	public readonly Disposable<HashSet<T>> hashSet;

	public readonly bool release;

	public BacktrackDetector bd;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> dispose = Dispose;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> distinctSkip = DistinctSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> distinctRemove = DistinctRemove;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> exceptSkip = ExceptSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> exceptRemove = ExceptRemove;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> intersectSkip = IntersectSkip;

	public static readonly Mutator<U, HashSetContext<T, U, V, W>> intersectRemove = IntersectRemove;

	public HashSetContext(Slinq<U, V> chained, DelegateFunc<U, W, T> selector, W parameter, Disposable<HashSet<T>> hashSet, bool release)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		this.parameter = parameter;
		this.hashSet = hashSet;
		this.release = release;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Distinct(Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V, W>>(distinctSkip, distinctRemove, dispose, new HashSetContext<T, U, V, W>(slinq, selector, parameter, hashSet, release));
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Except(Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V, W>>(exceptSkip, exceptRemove, dispose, new HashSetContext<T, U, V, W>(slinq, selector, parameter, hashSet, release));
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Intersect(Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Disposable<HashSet<T>> hashSet, bool release)
	{
		return new Slinq<U, HashSetContext<T, U, V, W>>(intersectSkip, intersectRemove, dispose, new HashSetContext<T, U, V, W>(slinq, selector, parameter, hashSet, release));
	}

	public static void Dispose(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		next = default(Option<U>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.hashSet.Dispose();
		}
	}

	public static void DistinctSkip(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Add(context.selector(context.chained.current.value, context.parameter)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void DistinctRemove(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		DistinctSkip(ref context, out next);
	}

	public static void ExceptSkip(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && context.hashSet.value.Contains(context.selector(context.chained.current.value, context.parameter)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void ExceptRemove(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		ExceptSkip(ref context, out next);
	}

	public static void IntersectSkip(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.hashSet.value.Remove(context.selector(context.chained.current.value, context.parameter)))
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (!context.chained.current.isSome && context.release)
		{
			context.hashSet.Dispose();
		}
		next = context.chained.current;
	}

	public static void IntersectRemove(ref HashSetContext<T, U, V, W> context, out Option<U> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		IntersectSkip(ref context, out next);
	}
}
