using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct AggregateContext<U, T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private U acc;

	private readonly DelegateFunc<U, T, U> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<U, AggregateContext<U, T, C>> skip;

	private static readonly Mutator<U, AggregateContext<U, T, C>> remove;

	private static readonly Mutator<U, AggregateContext<U, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AggregateContext(Slinq<T, C> chained, U seed, DelegateFunc<U, T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AggregateContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, AggregateContext<U, T, C>> AggregateRunning(Slinq<T, C> slinq, U seed, DelegateFunc<U, T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref AggregateContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref AggregateContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref AggregateContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct AggregateContext<U, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private U acc;

	private readonly DelegateFunc<U, T, P, U> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<U, AggregateContext<U, T, C, P>> skip;

	private static readonly Mutator<U, AggregateContext<U, T, C, P>> remove;

	private static readonly Mutator<U, AggregateContext<U, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AggregateContext(Slinq<T, C> chained, U seed, DelegateFunc<U, T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AggregateContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, AggregateContext<U, T, C, P>> AggregateRunning(Slinq<T, C> slinq, U seed, DelegateFunc<U, T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref AggregateContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref AggregateContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref AggregateContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
