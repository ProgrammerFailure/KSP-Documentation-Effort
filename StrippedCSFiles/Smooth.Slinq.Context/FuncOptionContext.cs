using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct FuncOptionContext<T>
{
	private bool needsMove;

	private Option<T> acc;

	private readonly DelegateFunc<T, Option<T>> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FuncOptionContext<T>> skip;

	private static readonly Mutator<T, FuncOptionContext<T>> remove;

	private static readonly Mutator<T, FuncOptionContext<T>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FuncOptionContext(T seed, DelegateFunc<T, Option<T>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FuncOptionContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncOptionContext<T>> Sequence(T seed, DelegateFunc<T, Option<T>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FuncOptionContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FuncOptionContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FuncOptionContext<T> context, out Option<T> next)
	{
		throw null;
	}
}
public struct FuncOptionContext<T, P>
{
	private bool needsMove;

	private Option<T> acc;

	private readonly DelegateFunc<T, P, Option<T>> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FuncOptionContext<T, P>> skip;

	private static readonly Mutator<T, FuncOptionContext<T, P>> remove;

	private static readonly Mutator<T, FuncOptionContext<T, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FuncOptionContext(T seed, DelegateFunc<T, P, Option<T>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FuncOptionContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncOptionContext<T, P>> Sequence(T seed, DelegateFunc<T, P, Option<T>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FuncOptionContext<T, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FuncOptionContext<T, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FuncOptionContext<T, P> context, out Option<T> next)
	{
		throw null;
	}
}
