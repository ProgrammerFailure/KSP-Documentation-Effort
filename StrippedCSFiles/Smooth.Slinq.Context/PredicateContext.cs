using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct PredicateContext<T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, bool> predicate;

	private BacktrackDetector bd;

	private static readonly Mutator<T, PredicateContext<T, C>> dispose;

	private static readonly Mutator<T, PredicateContext<T, C>> takeWhileSkip;

	private static readonly Mutator<T, PredicateContext<T, C>> takeWhileRemove;

	private static readonly Mutator<T, PredicateContext<T, C>> whereSkip;

	private static readonly Mutator<T, PredicateContext<T, C>> whereRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PredicateContext(Slinq<T, C> chained, DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PredicateContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C>> TakeWhile(Slinq<T, C> slinq, DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C>> Where(Slinq<T, C> slinq, DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref PredicateContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void TakeWhileSkip(ref PredicateContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void TakeWhileRemove(ref PredicateContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WhereSkip(ref PredicateContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WhereRemove(ref PredicateContext<T, C> context, out Option<T> next)
	{
		throw null;
	}
}
public struct PredicateContext<T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private readonly DelegateFunc<T, P, bool> predicate;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<T, PredicateContext<T, C, P>> dispose;

	private static readonly Mutator<T, PredicateContext<T, C, P>> takeWhileSkip;

	private static readonly Mutator<T, PredicateContext<T, C, P>> takeWhileRemove;

	private static readonly Mutator<T, PredicateContext<T, C, P>> whereSkip;

	private static readonly Mutator<T, PredicateContext<T, C, P>> whereRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PredicateContext(Slinq<T, C> chained, DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PredicateContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C, P>> TakeWhile(Slinq<T, C> slinq, DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, PredicateContext<T, C, P>> Where(Slinq<T, C> slinq, DelegateFunc<T, P, bool> predicate, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref PredicateContext<T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void TakeWhileSkip(ref PredicateContext<T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void TakeWhileRemove(ref PredicateContext<T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WhereSkip(ref PredicateContext<T, C, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WhereRemove(ref PredicateContext<T, C, P> context, out Option<T> next)
	{
		throw null;
	}
}
