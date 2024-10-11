using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct ZipContext<T2, C2, T, C>
{
	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, C>> skip;

	private static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, C>> remove;

	private static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, T2>, ZipContext<T2, C2, T, C>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipContext<T2, C2, T, C> context, out Option<Tuple<T, T2>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipContext<T2, C2, T, C> context, out Option<Tuple<T, T2>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipContext<T2, C2, T, C> context, out Option<Tuple<T, T2>> next)
	{
		throw null;
	}
}
public struct ZipContext<U, T2, C2, T, C>
{
	public static readonly DelegateFunc<T, T2, Tuple<T, T2>> tuple;

	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly DelegateFunc<T, T2, U> selector;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> skip;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> remove;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<T, T2, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<T, T2, U> selector, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next)
	{
		throw null;
	}
}
public struct ZipContext<U, T2, C2, T, C, P>
{
	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T2, C2> right;

	private readonly DelegateFunc<T, T2, P, U> selector;

	private readonly P parameter;

	private readonly ZipRemoveFlags removeFlags;

	private BacktrackDetector bd;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> skip;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> remove;

	private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<T, T2, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ZipContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<U, ZipContext<U, T2, C2, T, C, P>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, DelegateFunc<T, T2, P, U> selector, P parameter, ZipRemoveFlags removeFlags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next)
	{
		throw null;
	}
}
