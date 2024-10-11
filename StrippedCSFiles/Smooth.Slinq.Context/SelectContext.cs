using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectContext<U, T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, U> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectContext<U, T, C>> skip;

	private static readonly Mutator<U, SelectContext<U, T, C>> remove;

	private static readonly Mutator<U, SelectContext<U, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectContext(Slinq<T, C> chained, DelegateFunc<T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectContext<U, T, C>> Select(Slinq<T, C> slinq, DelegateFunc<T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct SelectContext<U, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, P, U> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectContext<U, T, C, P>> skip;

	private static readonly Mutator<U, SelectContext<U, T, C, P>> remove;

	private static readonly Mutator<U, SelectContext<U, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectContext(Slinq<T, C> chained, DelegateFunc<T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectContext<U, T, C, P>> Select(Slinq<T, C> slinq, DelegateFunc<T, P, U> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
