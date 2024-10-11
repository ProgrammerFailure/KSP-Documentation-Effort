using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct FlattenContext<T, C1, C2>
{
	private bool needsMove;

	private Slinq<Slinq<T, C1>, C2> chained;

	private Slinq<T, C1> selected;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FlattenContext<T, C1, C2>> skip;

	private static readonly Mutator<T, FlattenContext<T, C1, C2>> remove;

	private static readonly Mutator<T, FlattenContext<T, C1, C2>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FlattenContext(Slinq<Slinq<T, C1>, C2> chained)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlattenContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FlattenContext<T, C1, C2>> Flatten(Slinq<Slinq<T, C1>, C2> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FlattenContext<T, C1, C2> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FlattenContext<T, C1, C2> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FlattenContext<T, C1, C2> context, out Option<T> next)
	{
		throw null;
	}
}
public struct FlattenContext<T, C>
{
	private bool needsMove;

	private Slinq<Option<T>, C> chained;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FlattenContext<T, C>> skip;

	private static readonly Mutator<T, FlattenContext<T, C>> remove;

	private static readonly Mutator<T, FlattenContext<T, C>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FlattenContext(Slinq<Option<T>, C> chained)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlattenContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FlattenContext<T, C>> Flatten(Slinq<Option<T>, C> slinq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FlattenContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FlattenContext<T, C> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FlattenContext<T, C> context, out Option<T> next)
	{
		throw null;
	}
}
