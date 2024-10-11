using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct FuncContext<T>
{
	private bool needsMove;

	private T acc;

	private readonly DelegateFunc<T, T> selector;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FuncContext<T>> skip;

	private static readonly Mutator<T, FuncContext<T>> remove;

	private static readonly Mutator<T, FuncContext<T>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FuncContext(T seed, DelegateFunc<T, T> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FuncContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncContext<T>> Sequence(T seed, DelegateFunc<T, T> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FuncContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FuncContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FuncContext<T> context, out Option<T> next)
	{
		throw null;
	}
}
public struct FuncContext<T, P>
{
	private bool needsMove;

	private T acc;

	private readonly DelegateFunc<T, P, T> selector;

	private readonly P parameter;

	private BacktrackDetector bd;

	private static readonly Mutator<T, FuncContext<T, P>> skip;

	private static readonly Mutator<T, FuncContext<T, P>> remove;

	private static readonly Mutator<T, FuncContext<T, P>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FuncContext(T seed, DelegateFunc<T, P, T> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FuncContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncContext<T, P>> Sequence(T seed, DelegateFunc<T, P, T> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref FuncContext<T, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref FuncContext<T, P> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref FuncContext<T, P> context, out Option<T> next)
	{
		throw null;
	}
}
