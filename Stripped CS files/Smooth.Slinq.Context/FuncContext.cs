using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct FuncContext<T>
{
	public bool needsMove;

	public T acc;

	public readonly DelegateFunc<T, T> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FuncContext<T>> skip = Skip;

	public static readonly Mutator<T, FuncContext<T>> remove = skip;

	public static readonly Mutator<T, FuncContext<T>> dispose = Dispose;

	public FuncContext(T seed, DelegateFunc<T, T> selector)
	{
		needsMove = false;
		acc = seed;
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FuncContext<T>> Sequence(T seed, DelegateFunc<T, T> selector)
	{
		return new Slinq<T, FuncContext<T>>(skip, remove, dispose, new FuncContext<T>(seed, selector));
	}

	public static void Skip(ref FuncContext<T> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.acc = context.selector(context.acc);
		}
		else
		{
			context.needsMove = true;
		}
		next = new Option<T>(context.acc);
	}

	public static void Remove(ref FuncContext<T> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref FuncContext<T> context, out Option<T> next)
	{
		next = default(Option<T>);
	}
}
public struct FuncContext<T, U>
{
	public bool needsMove;

	public T acc;

	public readonly DelegateFunc<T, U, T> selector;

	public readonly U parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FuncContext<T, U>> skip = Skip;

	public static readonly Mutator<T, FuncContext<T, U>> remove = skip;

	public static readonly Mutator<T, FuncContext<T, U>> dispose = Dispose;

	public FuncContext(T seed, DelegateFunc<T, U, T> selector, U parameter)
	{
		needsMove = false;
		acc = seed;
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FuncContext<T, U>> Sequence(T seed, DelegateFunc<T, U, T> selector, U parameter)
	{
		return new Slinq<T, FuncContext<T, U>>(skip, remove, dispose, new FuncContext<T, U>(seed, selector, parameter));
	}

	public static void Skip(ref FuncContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.acc = context.selector(context.acc, context.parameter);
		}
		else
		{
			context.needsMove = true;
		}
		next = new Option<T>(context.acc);
	}

	public static void Remove(ref FuncContext<T, U> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref FuncContext<T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
	}
}
