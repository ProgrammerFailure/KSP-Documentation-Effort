using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct ZipAllContext<T2, C2, T, U>
{
	public bool needsMove;

	public Slinq<T, U> left;

	public Slinq<T2, C2> right;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> skip = Skip;

	public static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> remove = Remove;

	public static readonly Mutator<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> dispose = Dispose;

	public ZipAllContext(Slinq<T, U> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> ZipAll(Slinq<T, U> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		return new Slinq<Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>>(skip, remove, dispose, new ZipAllContext<T2, C2, T, U>(left, right, removeFlags));
	}

	public static void Skip(ref ZipAllContext<T2, C2, T, U> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		if (context.needsMove)
		{
			if (context.left.current.isSome)
			{
				context.left.skip(ref context.left.context, out context.left.current);
				if (context.right.current.isSome)
				{
					context.right.skip(ref context.right.context, out context.right.current);
				}
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.needsMove = true;
		}
		if (!context.left.current.isSome && !context.right.current.isSome)
		{
			next = default(Option<Tuple<Option<T>, Option<T2>>>);
		}
		else
		{
			next = new Option<Tuple<Option<T>, Option<T2>>>(new Tuple<Option<T>, Option<T2>>(context.left.current, context.right.current));
		}
	}

	public static void Remove(ref ZipAllContext<T2, C2, T, U> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		context.needsMove = false;
		if (context.left.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
			{
				context.left.remove(ref context.left.context, out context.left.current);
			}
			else
			{
				context.left.skip(ref context.left.context, out context.left.current);
			}
		}
		if (context.right.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
			{
				context.right.remove(ref context.right.context, out context.right.current);
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipAllContext<T2, C2, T, U> context, out Option<Tuple<Option<T>, Option<T2>>> next)
	{
		next = default(Option<Tuple<Option<T>, Option<T2>>>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
			if (context.right.current.isSome)
			{
				context.right.dispose(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}
}
public struct ZipAllContext<T, T2, C2, U, V>
{
	public bool needsMove;

	public Slinq<U, V> left;

	public Slinq<T2, C2> right;

	public readonly DelegateFunc<Option<U>, Option<T2>, T> selector;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V>> skip = Skip;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V>> remove = Remove;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V>> dispose = Dispose;

	public ZipAllContext(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<Option<U>, Option<T2>, T> selector, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.selector = selector;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V>> ZipAll(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<Option<U>, Option<T2>, T> selector, ZipRemoveFlags removeFlags)
	{
		return new Slinq<T, ZipAllContext<T, T2, C2, U, V>>(skip, remove, dispose, new ZipAllContext<T, T2, C2, U, V>(left, right, selector, removeFlags));
	}

	public static void Skip(ref ZipAllContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			if (context.left.current.isSome)
			{
				context.left.skip(ref context.left.context, out context.left.current);
				if (context.right.current.isSome)
				{
					context.right.skip(ref context.right.context, out context.right.current);
				}
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.needsMove = true;
		}
		if (!context.left.current.isSome && !context.right.current.isSome)
		{
			next = default(Option<T>);
		}
		else
		{
			next = new Option<T>(context.selector(context.left.current, context.right.current));
		}
	}

	public static void Remove(ref ZipAllContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		context.needsMove = false;
		if (context.left.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
			{
				context.left.remove(ref context.left.context, out context.left.current);
			}
			else
			{
				context.left.skip(ref context.left.context, out context.left.current);
			}
		}
		if (context.right.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
			{
				context.right.remove(ref context.right.context, out context.right.current);
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipAllContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		next = default(Option<T>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
			if (context.right.current.isSome)
			{
				context.right.dispose(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}
}
public struct ZipAllContext<T, T2, C2, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> left;

	public Slinq<T2, C2> right;

	public readonly DelegateFunc<Option<U>, Option<T2>, W, T> selector;

	public readonly W parameter;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V, W>> skip = Skip;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V, W>> remove = Remove;

	public static readonly Mutator<T, ZipAllContext<T, T2, C2, U, V, W>> dispose = Dispose;

	public ZipAllContext(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<Option<U>, Option<T2>, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.selector = selector;
		this.parameter = parameter;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V, W>> ZipAll(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<Option<U>, Option<T2>, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		return new Slinq<T, ZipAllContext<T, T2, C2, U, V, W>>(skip, remove, dispose, new ZipAllContext<T, T2, C2, U, V, W>(left, right, selector, parameter, removeFlags));
	}

	public static void Skip(ref ZipAllContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			if (context.left.current.isSome)
			{
				context.left.skip(ref context.left.context, out context.left.current);
				if (context.right.current.isSome)
				{
					context.right.skip(ref context.right.context, out context.right.current);
				}
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.needsMove = true;
		}
		if (!context.left.current.isSome && !context.right.current.isSome)
		{
			next = default(Option<T>);
		}
		else
		{
			next = new Option<T>(context.selector(context.left.current, context.right.current, context.parameter));
		}
	}

	public static void Remove(ref ZipAllContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		if (context.left.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
			{
				context.left.remove(ref context.left.context, out context.left.current);
			}
			else
			{
				context.left.skip(ref context.left.context, out context.left.current);
			}
		}
		if (context.right.current.isSome)
		{
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
			{
				context.right.remove(ref context.right.context, out context.right.current);
			}
			else
			{
				context.right.skip(ref context.right.context, out context.right.current);
			}
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipAllContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
			if (context.right.current.isSome)
			{
				context.right.dispose(ref context.right.context, out context.right.current);
			}
		}
		else
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}
}
