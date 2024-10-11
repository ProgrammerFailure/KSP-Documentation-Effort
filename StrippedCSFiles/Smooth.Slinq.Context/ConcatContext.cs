using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct ConcatContext<C2, T, C>
{
	private bool needsMove;

	private Slinq<T, C> first;

	private Slinq<T, C2> second;

	private BacktrackDetector bd;

	private static readonly Mutator<T, ConcatContext<C2, T, C>> skip;

	private static readonly Mutator<T, ConcatContext<C2, T, C>> remove;

	private static readonly Mutator<T, ConcatContext<C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConcatContext(Slinq<T, C> first, Slinq<T, C2> second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ConcatContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, ConcatContext<C2, T, C>> Concat(Slinq<T, C> first, Slinq<T, C2> second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref ConcatContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref ConcatContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref ConcatContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}
}
