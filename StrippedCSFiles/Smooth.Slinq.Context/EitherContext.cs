using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct EitherContext<C2, T, C>
{
	private readonly bool isLeft;

	private bool needsMove;

	private Slinq<T, C> left;

	private Slinq<T, C2> right;

	private BacktrackDetector bd;

	private static readonly Mutator<T, EitherContext<C2, T, C>> skip;

	private static readonly Mutator<T, EitherContext<C2, T, C>> remove;

	private static readonly Mutator<T, EitherContext<C2, T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private EitherContext(Slinq<T, C> left, Slinq<T, C2> right, bool isLeft)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EitherContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, EitherContext<C2, T, C>> Left(Slinq<T, C> left)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, EitherContext<C2, T, C>> Right(Slinq<T, C2> right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref EitherContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref EitherContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref EitherContext<C2, T, C> context, out Option<T> next)
	{
		throw null;
	}
}
