using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectOptionContext<T, U, V>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, Option<T>> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectOptionContext<T, U, V>> skip = Skip;

	public static readonly Mutator<T, SelectOptionContext<T, U, V>> remove = Remove;

	public static readonly Mutator<T, SelectOptionContext<T, U, V>> dispose = Dispose;

	public SelectOptionContext(Slinq<U, V> chained, DelegateFunc<U, Option<T>> selector)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectOptionContext<T, U, V>> SelectMany(Slinq<U, V> slinq, DelegateFunc<U, Option<T>> selector)
	{
		return new Slinq<T, SelectOptionContext<T, U, V>>(skip, remove, dispose, new SelectOptionContext<T, U, V>(slinq, selector));
	}

	public static void Skip(ref SelectOptionContext<T, U, V> context, out Option<T> next)
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
			Option<T> option = context.selector(context.chained.current.value);
			while (!option.isSome)
			{
				context.chained.skip(ref context.chained.context, out context.chained.current);
				if (!context.chained.current.isSome)
				{
					break;
				}
				option = context.selector(context.chained.current.value);
			}
			next = option;
		}
		else
		{
			next = default(Option<T>);
		}
	}

	public static void Remove(ref SelectOptionContext<T, U, V> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectOptionContext<T, U, V> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
public struct SelectOptionContext<T, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> chained;

	public readonly DelegateFunc<U, W, Option<T>> selector;

	public readonly W parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectOptionContext<T, U, V, W>> skip = Skip;

	public static readonly Mutator<T, SelectOptionContext<T, U, V, W>> remove = Remove;

	public static readonly Mutator<T, SelectOptionContext<T, U, V, W>> dispose = Dispose;

	public SelectOptionContext(Slinq<U, V> chained, DelegateFunc<U, W, Option<T>> selector, W parameter)
	{
		needsMove = false;
		this.chained = chained;
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectOptionContext<T, U, V, W>> SelectMany(Slinq<U, V> slinq, DelegateFunc<U, W, Option<T>> selector, W parameter)
	{
		return new Slinq<T, SelectOptionContext<T, U, V, W>>(skip, remove, dispose, new SelectOptionContext<T, U, V, W>(slinq, selector, parameter));
	}

	public static void Skip(ref SelectOptionContext<T, U, V, W> context, out Option<T> next)
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
			Option<T> option = context.selector(context.chained.current.value, context.parameter);
			while (!option.isSome)
			{
				context.chained.skip(ref context.chained.context, out context.chained.current);
				if (!context.chained.current.isSome)
				{
					break;
				}
				option = context.selector(context.chained.current.value, context.parameter);
			}
			next = option;
		}
		else
		{
			next = default(Option<T>);
		}
	}

	public static void Remove(ref SelectOptionContext<T, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectOptionContext<T, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
