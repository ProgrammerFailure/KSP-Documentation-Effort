using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct IListContext<T>
{
	private IList<T> list;

	private int size;

	private int index;

	private readonly int step;

	private BacktrackDetector bd;

	private static readonly Mutator<T, IListContext<T>> skip;

	private static readonly Mutator<T, IListContext<T>> remove;

	private static readonly Mutator<T, IListContext<T>> dispose;

	private static readonly Mutator<Tuple<T, int>, IListContext<T>> skipWithIndex;

	private static readonly Mutator<Tuple<T, int>, IListContext<T>> removeWithIndex;

	private static readonly Mutator<Tuple<T, int>, IListContext<T>> disposeWithIndex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IListContext(IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static IListContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> Slinq(IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndex(IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref IListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref IListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref IListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SkipWithIndex(ref IListContext<T> context, out Option<Tuple<T, int>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RemoveWithIndex(ref IListContext<T> context, out Option<Tuple<T, int>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DisposeWithIndex(ref IListContext<T> context, out Option<Tuple<T, int>> next)
	{
		throw null;
	}
}
