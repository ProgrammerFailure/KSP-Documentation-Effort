using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct ConcatContext<C2, T, U>
{
	public bool needsMove;

	public Slinq<T, U> first;

	public Slinq<T, C2> second;

	public BacktrackDetector bd;

	public static readonly Mutator<T, ConcatContext<C2, T, U>> skip = Skip;

	public static readonly Mutator<T, ConcatContext<C2, T, U>> remove = Remove;

	public static readonly Mutator<T, ConcatContext<C2, T, U>> dispose = Dispose;

	public ConcatContext(Slinq<T, U> first, Slinq<T, C2> second)
	{
		needsMove = false;
		this.first = first;
		this.second = second;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, ConcatContext<C2, T, U>> Concat(Slinq<T, U> first, Slinq<T, C2> second)
	{
		return new Slinq<T, ConcatContext<C2, T, U>>(skip, remove, dispose, new ConcatContext<C2, T, U>(first, second));
	}

	public static void Skip(ref ConcatContext<C2, T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			if (context.first.current.isSome)
			{
				context.first.skip(ref context.first.context, out context.first.current);
			}
			else
			{
				context.second.skip(ref context.second.context, out context.second.current);
			}
		}
		else
		{
			context.needsMove = true;
		}
		next = (context.first.current.isSome ? context.first.current : context.second.current);
	}

	public static void Remove(ref ConcatContext<C2, T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		if (context.first.current.isSome)
		{
			context.first.remove(ref context.first.context, out context.first.current);
		}
		else
		{
			context.second.remove(ref context.second.context, out context.second.current);
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref ConcatContext<C2, T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
		if (context.first.current.isSome)
		{
			context.first.dispose(ref context.first.context, out context.first.current);
			if (context.second.current.isSome)
			{
				context.second.dispose(ref context.second.context, out context.second.current);
			}
		}
		else
		{
			context.second.dispose(ref context.second.context, out context.second.current);
		}
	}
}
