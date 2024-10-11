using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct GroupJoinContext<U, K, T2, T, C>
{
	private bool needsMove;

	private readonly Lookup<K, T2> lookup;

	private readonly DelegateFunc<T, K> outerSelector;

	private readonly DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, U> resultSelector;

	private readonly bool release;

	private Slinq<T, C> chained;

	private BacktrackDetector bd;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C>> skip;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C>> remove;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GroupJoinContext(Lookup<K, T2> lookup, Slinq<T, C> outer, DelegateFunc<T, K> outerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, U> resultSelector, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GroupJoinContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C>> GroupJoin(Lookup<K, T2> lookup, Slinq<T, C> outer, DelegateFunc<T, K> outerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, U> resultSelector, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref GroupJoinContext<U, K, T2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref GroupJoinContext<U, K, T2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref GroupJoinContext<U, K, T2, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct GroupJoinContext<U, K, T2, T, C, P>
{
	private bool needsMove;

	private readonly Lookup<K, T2> lookup;

	private readonly DelegateFunc<T, P, K> outerSelector;

	private readonly DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, P, U> resultSelector;

	private readonly P parameter;

	private readonly bool release;

	private Slinq<T, C> chained;

	private BacktrackDetector bd;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C, P>> skip;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C, P>> remove;

	private static readonly Mutator<U, GroupJoinContext<U, K, T2, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GroupJoinContext(Lookup<K, T2> lookup, Slinq<T, C> outer, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, P, U> resultSelector, P parameter, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GroupJoinContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, GroupJoinContext<U, K, T2, T, C, P>> GroupJoin(Lookup<K, T2> lookup, Slinq<T, C> outer, DelegateFunc<T, P, K> outerSelector, DelegateFunc<T, Slinq<T2, LinkedContext<T2>>, P, U> resultSelector, P parameter, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref GroupJoinContext<U, K, T2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref GroupJoinContext<U, K, T2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref GroupJoinContext<U, K, T2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
