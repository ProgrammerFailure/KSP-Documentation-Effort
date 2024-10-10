using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct FuncOptionContext<T>
{
	public bool needsMove;

	public Option<T> acc;

	public readonly DelegateFunc<T, Option<T>> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FuncOptionContext<T>> skip = Skip;

	public static readonly Mutator<T, FuncOptionContext<T>> remove = skip;

	public static readonly Mutator<T, FuncOptionContext<T>> dispose = Dispose;

	public FuncOptionContext(T seed, DelegateFunc<T, Option<T>> selector)
	{
		needsMove = false;
		acc = new Option<T>(seed);
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FuncOptionContext<T>> Sequence(T seed, DelegateFunc<T, Option<T>> selector)
	{
		return new Slinq<T, FuncOptionContext<T>>(skip, remove, dispose, new FuncOptionContext<T>(seed, selector));
	}

	public static void Skip(ref FuncOptionContext<T> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.acc = context.selector(context.acc.value);
		}
		else
		{
			context.needsMove = true;
		}
		next = context.acc;
	}

	public static void Remove(ref FuncOptionContext<T> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref FuncOptionContext<T> context, out Option<T> next)
	{
		next = default(Option<T>);
	}
}
public struct FuncOptionContext<T, U>
{
	public bool needsMove;

	public Option<T> acc;

	public readonly DelegateFunc<T, U, Option<T>> selector;

	public readonly U parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FuncOptionContext<T, U>> skip = Skip;

	public static readonly Mutator<T, FuncOptionContext<T, U>> remove = skip;

	public static readonly Mutator<T, FuncOptionContext<T, U>> dispose = Dispose;

	public FuncOptionContext(T seed, DelegateFunc<T, U, Option<T>> selector, U parameter)
	{
		needsMove = false;
		acc = new Option<T>(seed);
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FuncOptionContext<T, U>> Sequence(T seed, DelegateFunc<T, U, Option<T>> selector, U parameter)
	{
		return new Slinq<T, FuncOptionContext<T, U>>(skip, remove, dispose, new FuncOptionContext<T, U>(seed, selector, parameter));
	}

	public static void Skip(ref FuncOptionContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.acc = context.selector(context.acc.value, context.parameter);
		}
		else
		{
			context.needsMove = true;
		}
		next = context.acc;
	}

	public static void Remove(ref FuncOptionContext<T, U> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref FuncOptionContext<T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
	}
}
