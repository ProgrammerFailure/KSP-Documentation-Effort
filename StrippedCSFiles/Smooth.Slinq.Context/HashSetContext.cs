using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Slinq.Context;

public struct HashSetContext<T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly Disposable<HashSet<T>> hashSet;

	private readonly bool release;

	private BacktrackDetector bd;

	private static readonly Mutator<T, HashSetContext<T, C>> dispose;

	private static readonly Mutator<T, HashSetContext<T, C>> distinctSkip;

	private static readonly Mutator<T, HashSetContext<T, C>> distinctRemove;

	private static readonly Mutator<T, HashSetContext<T, C>> exceptSkip;

	private static readonly Mutator<T, HashSetContext<T, C>> exceptRemove;

	private static readonly Mutator<T, HashSetContext<T, C>> intersectSkip;

	private static readonly Mutator<T, HashSetContext<T, C>> intersectRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private HashSetContext(Slinq<T, C> chained, Disposable<HashSet<T>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static HashSetContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Distinct(Slinq<T, C> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Except(Slinq<T, C> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<T, C>> Intersect(Slinq<T, C> slinq, Disposable<HashSet<T>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctSkip(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctRemove(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptSkip(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptRemove(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectSkip(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectRemove(ref HashSetContext<T, C> context, out Option<T> next)
	{
		throw null;
	}
}
public struct HashSetContext<K, T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, K> selector;

	private readonly Disposable<HashSet<K>> hashSet;

	private readonly bool release;

	private BacktrackDetector bd;

	private static readonly Mutator<T, HashSetContext<K, T, C>> dispose;

	private static readonly Mutator<T, HashSetContext<K, T, C>> distinctSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C>> distinctRemove;

	private static readonly Mutator<T, HashSetContext<K, T, C>> exceptSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C>> exceptRemove;

	private static readonly Mutator<T, HashSetContext<K, T, C>> intersectSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C>> intersectRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private HashSetContext(Slinq<T, C> chained, DelegateFunc<T, K> selector, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static HashSetContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Distinct(Slinq<T, C> slinq, DelegateFunc<T, K> selector, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Except(Slinq<T, C> slinq, DelegateFunc<T, K> selector, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C>> Intersect(Slinq<T, C> slinq, DelegateFunc<T, K> selector, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctSkip(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctRemove(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptSkip(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptRemove(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectSkip(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectRemove(ref HashSetContext<K, T, C> context, out Option<T> next)
	{
		throw null;
	}
}
public struct HashSetContext<K, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, P, K> selector;

	private readonly P parameter;

	private readonly Disposable<HashSet<K>> hashSet;

	private readonly bool release;

	private BacktrackDetector bd;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> dispose;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> distinctSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> distinctRemove;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> exceptSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> exceptRemove;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> intersectSkip;

	private static readonly Mutator<T, HashSetContext<K, T, C, P>> intersectRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private HashSetContext(Slinq<T, C> chained, DelegateFunc<T, P, K> selector, P parameter, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static HashSetContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Distinct(Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Except(Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, HashSetContext<K, T, C, P>> Intersect(Slinq<T, C> slinq, DelegateFunc<T, P, K> selector, P parameter, Disposable<HashSet<K>> hashSet, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctSkip(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DistinctRemove(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptSkip(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ExceptRemove(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectSkip(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IntersectRemove(ref HashSetContext<K, T, C, P> context, out Option<T> next)
	{
		throw null;
	}
}
