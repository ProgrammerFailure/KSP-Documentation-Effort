using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectOptionContext<U, T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, Option<U>> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectOptionContext<U, T, C>> skip;

	private static readonly Mutator<U, SelectOptionContext<U, T, C>> remove;

	private static readonly Mutator<U, SelectOptionContext<U, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectOptionContext(Slinq<T, C> chained, DelegateFunc<T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectOptionContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectOptionContext<U, T, C>> SelectMany(Slinq<T, C> slinq, DelegateFunc<T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectOptionContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectOptionContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectOptionContext<U, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct SelectOptionContext<U, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, P, Option<U>> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<U, SelectOptionContext<U, T, C, P>> skip;

	private static readonly Mutator<U, SelectOptionContext<U, T, C, P>> remove;

	private static readonly Mutator<U, SelectOptionContext<U, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SelectOptionContext(Slinq<T, C> chained, DelegateFunc<T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SelectOptionContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, SelectOptionContext<U, T, C, P>> SelectMany(Slinq<T, C> slinq, DelegateFunc<T, P, Option<U>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref SelectOptionContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref SelectOptionContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref SelectOptionContext<U, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
