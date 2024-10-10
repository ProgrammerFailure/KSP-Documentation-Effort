using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Slinq.Context;

public struct ZipContext<T2, C2, T, U>
{
	public bool needsMove;

	public Slinq<T, U> left;

	public Slinq<T2, C2> right;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, U>> skip = Skip;

	public static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, U>> remove = Remove;

	public static readonly Mutator<Tuple<T, T2>, ZipContext<T2, C2, T, U>> dispose = Dispose;

	public ZipContext(Slinq<T, U> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<Tuple<T, T2>, ZipContext<T2, C2, T, U>> Zip(Slinq<T, U> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags)
	{
		return new Slinq<Tuple<T, T2>, ZipContext<T2, C2, T, U>>(skip, remove, dispose, new ZipContext<T2, C2, T, U>(left, right, removeFlags));
	}

	public static void Skip(ref ZipContext<T2, C2, T, U> context, out Option<Tuple<T, T2>> next)
	{
		if (context.needsMove)
		{
			context.left.skip(ref context.left.context, out context.left.current);
			context.right.skip(ref context.right.context, out context.right.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.left.current.isSome && context.right.current.isSome)
		{
			next = new Option<Tuple<T, T2>>(new Tuple<T, T2>(context.left.current.value, context.right.current.value));
			return;
		}
		next = default(Option<Tuple<T, T2>>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
		}
		else if (context.right.current.isSome)
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}

	public static void Remove(ref ZipContext<T2, C2, T, U> context, out Option<Tuple<T, T2>> next)
	{
		context.needsMove = false;
		if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
		{
			context.left.remove(ref context.left.context, out context.left.current);
		}
		else
		{
			context.left.skip(ref context.left.context, out context.left.current);
		}
		if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
		{
			context.right.remove(ref context.right.context, out context.right.current);
		}
		else
		{
			context.right.skip(ref context.right.context, out context.right.current);
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipContext<T2, C2, T, U> context, out Option<Tuple<T, T2>> next)
	{
		next = default(Option<Tuple<T, T2>>);
		context.left.dispose(ref context.left.context, out context.left.current);
		context.right.dispose(ref context.right.context, out context.right.current);
	}
}
public struct ZipContext<T, T2, C2, U, V>
{
	public static readonly DelegateFunc<U, T2, Tuple<U, T2>> tuple = (U t, T2 t2) => new Tuple<U, T2>(t, t2);

	public bool needsMove;

	public Slinq<U, V> left;

	public Slinq<T2, C2> right;

	public readonly DelegateFunc<U, T2, T> selector;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V>> skip = Skip;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V>> remove = Remove;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V>> dispose = Dispose;

	public ZipContext(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<U, T2, T> selector, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.selector = selector;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V>> Zip(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<U, T2, T> selector, ZipRemoveFlags removeFlags)
	{
		return new Slinq<T, ZipContext<T, T2, C2, U, V>>(skip, remove, dispose, new ZipContext<T, T2, C2, U, V>(left, right, selector, removeFlags));
	}

	public static void Skip(ref ZipContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.left.skip(ref context.left.context, out context.left.current);
			context.right.skip(ref context.right.context, out context.right.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.left.current.isSome && context.right.current.isSome)
		{
			next = new Option<T>(context.selector(context.left.current.value, context.right.current.value));
			return;
		}
		next = default(Option<T>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
		}
		else if (context.right.current.isSome)
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}

	public static void Remove(ref ZipContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		context.needsMove = false;
		if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
		{
			context.left.remove(ref context.left.context, out context.left.current);
		}
		else
		{
			context.left.skip(ref context.left.context, out context.left.current);
		}
		if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
		{
			context.right.remove(ref context.right.context, out context.right.current);
		}
		else
		{
			context.right.skip(ref context.right.context, out context.right.current);
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipContext<T, T2, C2, U, V> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.left.dispose(ref context.left.context, out context.left.current);
		context.right.dispose(ref context.right.context, out context.right.current);
	}
}
public struct ZipContext<T, T2, C2, U, V, W>
{
	public bool needsMove;

	public Slinq<U, V> left;

	public Slinq<T2, C2> right;

	public readonly DelegateFunc<U, T2, W, T> selector;

	public readonly W parameter;

	public readonly ZipRemoveFlags removeFlags;

	public BacktrackDetector bd;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V, W>> skip = Skip;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V, W>> remove = Remove;

	public static readonly Mutator<T, ZipContext<T, T2, C2, U, V, W>> dispose = Dispose;

	public ZipContext(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<U, T2, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		needsMove = false;
		this.left = left;
		this.right = right;
		this.selector = selector;
		this.parameter = parameter;
		this.removeFlags = removeFlags;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V, W>> Zip(Slinq<U, V> left, Slinq<T2, C2> right, DelegateFunc<U, T2, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		return new Slinq<T, ZipContext<T, T2, C2, U, V, W>>(skip, remove, dispose, new ZipContext<T, T2, C2, U, V, W>(left, right, selector, parameter, removeFlags));
	}

	public static void Skip(ref ZipContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.left.skip(ref context.left.context, out context.left.current);
			context.right.skip(ref context.right.context, out context.right.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.left.current.isSome && context.right.current.isSome)
		{
			next = new Option<T>(context.selector(context.left.current.value, context.right.current.value, context.parameter));
			return;
		}
		next = default(Option<T>);
		if (context.left.current.isSome)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
		}
		else if (context.right.current.isSome)
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}

	public static void Remove(ref ZipContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left)
		{
			context.left.remove(ref context.left.context, out context.left.current);
		}
		else
		{
			context.left.skip(ref context.left.context, out context.left.current);
		}
		if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right)
		{
			context.right.remove(ref context.right.context, out context.right.current);
		}
		else
		{
			context.right.skip(ref context.right.context, out context.right.current);
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ZipContext<T, T2, C2, U, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.left.dispose(ref context.left.context, out context.left.current);
		context.right.dispose(ref context.right.context, out context.right.current);
	}
}
