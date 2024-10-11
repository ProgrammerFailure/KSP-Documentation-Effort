using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct ZipAllContext<T2, C2, T, C>
{
	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> skip;

	private static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> remove;

	private static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipAllContext(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipAllContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, C>> ZipAll(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipAllContext<T2, C2, T, C> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipAllContext<T2, C2, T, C> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipAllContext<T2, C2, T, C> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		throw null;
	}
}
public struct ZipAllContext<U, T2, C2, T, C>
{
	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly DelegateFunc<Option<T>, Option<T2>, U> selector;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C>> skip;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C>> remove;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipAllContext(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<Option<T>, Option<T2>, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipAllContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C>> ZipAll(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<Option<T>, Option<T2>, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipAllContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipAllContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipAllContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct ZipAllContext<U, T2, C2, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly DelegateFunc<Option<T>, Option<T2>, P, U> selector;

	private readonly P parameter;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C, P>> skip;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C, P>> remove;

	private static readonly Mutator<U, ZipAllContext<U, T2, C2, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipAllContext(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<Option<T>, Option<T2>, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipAllContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipAllContext<U, T2, C2, T, C, P>> ZipAll(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<Option<T>, Option<T2>, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipAllContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipAllContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipAllContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
