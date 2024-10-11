using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectSlinqContext<U, UC, T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private Slinq<U, UC> selected;

	private readonly DelegateFunc<T, Slinq<U, UC>> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C>> skip;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C>> remove;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectSlinqContext(Slinq<T, C> chained, DelegateFunc<T, Slinq<U, UC>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectSlinqContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectSlinqContext<U, UC, T, C>> SelectMany(Slinq<T, C> slinq, DelegateFunc<T, Slinq<U, UC>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectSlinqContext<U, UC, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectSlinqContext<U, UC, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectSlinqContext<U, UC, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct SelectSlinqContext<U, UC, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private Slinq<U, UC> selected;

	private readonly DelegateFunc<T, P, Slinq<U, UC>> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C, P>> skip;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C, P>> remove;

	private static readonly Mutator<U, SelectSlinqContext<U, UC, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectSlinqContext(Slinq<T, C> chained, DelegateFunc<T, P, Slinq<U, UC>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectSlinqContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectSlinqContext<U, UC, T, C, P>> SelectMany(Slinq<T, C> slinq, DelegateFunc<T, P, Slinq<U, UC>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectSlinqContext<U, UC, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectSlinqContext<U, UC, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectSlinqContext<U, UC, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
