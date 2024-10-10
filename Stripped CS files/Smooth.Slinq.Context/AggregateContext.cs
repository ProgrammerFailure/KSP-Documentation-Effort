using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct AggregateContext<T, U, V>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public T acc;

	public readonly DelegateFunc<T, U, T> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, AggregateContext<T, U, V>> skip = Skip;

	public static readonly Mutator<T, AggregateContext<T, U, V>> remove = Remove;

	public static readonly Mutator<T, AggregateContext<T, U, V>> dispose = Dispose;

	public AggregateContext(Slinq<U, V> chained, T seed, DelegateFunc<T, U, T> selector)
	{
		needsMove = false;
		this.chained = chained;
		acc = seed;
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, AggregateContext<T, U, V>> AggregateRunning(Slinq<U, V> slinq, T seed, DelegateFunc<T, U, T> selector)
	{
		return new Slinq<T, AggregateContext<T, U, V>>(skip, remove, dispose, new AggregateContext<T, U, V>(slinq, seed, selector));
	}

	public static void Skip(ref AggregateContext<T, U, V> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.chained.current.isSome)
		{
			context.acc = context.selector(context.acc, context.chained.current.value);
			next = new Option<T>(context.acc);
		}
		else
		{
			next = default(Option<T>);
		}
	}

	public static void Remove(ref AggregateContext<T, U, V> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref AggregateContext<T, U, V> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
public struct AggregateContext<T, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public T acc;

	public readonly DelegateFunc<T, U, W, T> selector;

	public readonly W parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, AggregateContext<T, U, V, W>> skip = Skip;

	public static readonly Mutator<T, AggregateContext<T, U, V, W>> remove = Remove;

	public static readonly Mutator<T, AggregateContext<T, U, V, W>> dispose = Dispose;

	public AggregateContext(Slinq<U, V> chained, T seed, DelegateFunc<T, U, W, T> selector, W parameter)
	{
		needsMove = false;
		this.chained = chained;
		acc = seed;
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, AggregateContext<T, U, V, W>> AggregateRunning(Slinq<U, V> slinq, T seed, DelegateFunc<T, U, W, T> selector, W parameter)
	{
		return new Slinq<T, AggregateContext<T, U, V, W>>(skip, remove, dispose, new AggregateContext<T, U, V, W>(slinq, seed, selector, parameter));
	}

	public static void Skip(ref AggregateContext<T, U, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.chained.current.isSome)
		{
			context.acc = context.selector(context.acc, context.chained.current.value, context.parameter);
			next = new Option<T>(context.acc);
		}
		else
		{
			next = default(Option<T>);
		}
	}

	public static void Remove(ref AggregateContext<T, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref AggregateContext<T, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
