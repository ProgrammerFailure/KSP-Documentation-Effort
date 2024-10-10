using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct IntContext<T, U>
{
	public bool needsMove;

	public Slinq<T, U> chained;

	public int count;

	public BacktrackDetector bd;

	public static readonly Mutator<T, IntContext<T, U>> skip = Skip;

	public static readonly Mutator<T, IntContext<T, U>> remove = Remove;

	public static readonly Mutator<T, IntContext<T, U>> dispose = Dispose;

	public IntContext(Slinq<T, U> chained, int count)
	{
		needsMove = false;
		this.chained = chained;
		this.count = count;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, IntContext<T, U>> Take(Slinq<T, U> slinq, int count)
	{
		return new Slinq<T, IntContext<T, U>>(skip, remove, dispose, new IntContext<T, U>(slinq, count));
	}

	public static void Skip(ref IntContext<T, U> context, out Option<T> next)
	{
		if (context.count-- > 0)
		{
			if (context.needsMove)
			{
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
			else
			{
				context.needsMove = true;
			}
		}
		else if (context.chained.current.isSome)
		{
			context.chained.dispose(ref context.chained.context, out context.chained.current);
		}
		next = context.chained.current;
	}

	public static void Remove(ref IntContext<T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref IntContext<T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
