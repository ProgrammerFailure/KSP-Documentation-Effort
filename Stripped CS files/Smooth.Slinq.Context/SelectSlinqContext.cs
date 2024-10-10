using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct SelectSlinqContext<T, U, V, W>
{
	public bool needsMove;

	public Slinq<V, W> chained;

	public Slinq<T, U> selected;

	public readonly DelegateFunc<V, Slinq<T, U>> selector;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W>> skip = Skip;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W>> remove = Remove;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W>> dispose = Dispose;

	public SelectSlinqContext(Slinq<V, W> chained, DelegateFunc<V, Slinq<T, U>> selector)
	{
		needsMove = false;
		this.chained = chained;
		selected = (chained.current.isSome ? selector(chained.current.value) : default(Slinq<T, U>));
		this.selector = selector;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectSlinqContext<T, U, V, W>> SelectMany(Slinq<V, W> slinq, DelegateFunc<V, Slinq<T, U>> selector)
	{
		return new Slinq<T, SelectSlinqContext<T, U, V, W>>(skip, remove, dispose, new SelectSlinqContext<T, U, V, W>(slinq, selector));
	}

	public static void Skip(ref SelectSlinqContext<T, U, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.selected.skip(ref context.selected.context, out context.selected.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (!context.selected.current.isSome && context.chained.current.isSome)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
			while (context.chained.current.isSome)
			{
				context.selected = context.selector(context.chained.current.value);
				if (context.selected.current.isSome)
				{
					break;
				}
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
		}
		next = context.selected.current;
	}

	public static void Remove(ref SelectSlinqContext<T, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.selected.remove(ref context.selected.context, out context.selected.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectSlinqContext<T, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		context.selected.dispose(ref context.selected.context, out context.selected.current);
	}
}
public struct SelectSlinqContext<T, U, V, W, X>
{
	public bool needsMove;

	public Slinq<V, W> chained;

	public Slinq<T, U> selected;

	public readonly DelegateFunc<V, X, Slinq<T, U>> selector;

	public readonly X parameter;

	public BacktrackDetector bd;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W, X>> skip = Skip;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W, X>> remove = Remove;

	public static readonly Mutator<T, SelectSlinqContext<T, U, V, W, X>> dispose = Dispose;

	public SelectSlinqContext(Slinq<V, W> chained, DelegateFunc<V, X, Slinq<T, U>> selector, X parameter)
	{
		needsMove = false;
		this.chained = chained;
		selected = (chained.current.isSome ? selector(chained.current.value, parameter) : default(Slinq<T, U>));
		this.selector = selector;
		this.parameter = parameter;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, SelectSlinqContext<T, U, V, W, X>> SelectMany(Slinq<V, W> slinq, DelegateFunc<V, X, Slinq<T, U>> selector, X parameter)
	{
		return new Slinq<T, SelectSlinqContext<T, U, V, W, X>>(skip, remove, dispose, new SelectSlinqContext<T, U, V, W, X>(slinq, selector, parameter));
	}

	public static void Skip(ref SelectSlinqContext<T, U, V, W, X> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.selected.skip(ref context.selected.context, out context.selected.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (!context.selected.current.isSome && context.chained.current.isSome)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
			while (context.chained.current.isSome)
			{
				context.selected = context.selector(context.chained.current.value, context.parameter);
				if (context.selected.current.isSome)
				{
					break;
				}
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
		}
		next = context.selected.current;
	}

	public static void Remove(ref SelectSlinqContext<T, U, V, W, X> context, out Option<T> next)
	{
		context.needsMove = false;
		context.selected.remove(ref context.selected.context, out context.selected.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref SelectSlinqContext<T, U, V, W, X> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		context.selected.dispose(ref context.selected.context, out context.selected.current);
	}
}
