using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct IntContext<T, C>
{
	private bool needsMove;

	private Slinq<T, C> chained;

	private int count;

	private BacktrackDetector bd;

	private static readonly Mutator<T, IntContext<T, C>> skip;

	private static readonly Mutator<T, IntContext<T, C>> remove;

	private static readonly Mutator<T, IntContext<T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IntContext(Slinq<T, C> chained, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static IntContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IntContext<T, C>> Take(Slinq<T, C> slinq, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref IntContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref IntContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref IntContext<T, C> context, out Option<T> next)
	{
		throw null;
	}
}
