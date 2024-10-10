using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectContext<T, U, V>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, T> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectContext<T, U, V>> skip = Skip;

	public static readonly Mutator<T, SelectContext<T, U, V>> remove = Remove;

	public static readonly Mutator<T, SelectContext<T, U, V>> dispose = Dispose;

	public SelectContext(Slinq<U, V> chained, DelegateFunc<U, T> selector)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectContext<T, U, V>> Select(Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return new Slinq<T, SelectContext<T, U, V>>(skip, remove, dispose, new SelectContext<T, U, V>(slinq, selector));
	}

	public static void Skip(ref SelectContext<T, U, V> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		next = (context.chained.current.isSome ? new Option<T>(context.selector(context.chained.current.value)) : default(Option<T>));
	}

	public static void Remove(ref SelectContext<T, U, V> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectContext<T, U, V> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
public struct SelectContext<T, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, W, T> selector;

	public readonly W parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectContext<T, U, V, W>> skip = Skip;

	public static readonly Mutator<T, SelectContext<T, U, V, W>> remove = Remove;

	public static readonly Mutator<T, SelectContext<T, U, V, W>> dispose = Dispose;

	public SelectContext(Slinq<U, V> chained, DelegateFunc<U, W, T> selector, W parameter)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectContext<T, U, V, W>> Select(Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return new Slinq<T, SelectContext<T, U, V, W>>(skip, remove, dispose, new SelectContext<T, U, V, W>(slinq, selector, parameter));
	}

	public static void Skip(ref SelectContext<T, U, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		next = (context.chained.current.isSome ? new Option<T>(context.selector(context.chained.current.value, context.parameter)) : default(Option<T>));
	}

	public static void Remove(ref SelectContext<T, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectContext<T, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
