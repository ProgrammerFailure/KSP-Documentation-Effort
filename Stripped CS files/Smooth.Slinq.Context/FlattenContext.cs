using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct FlattenContext<T, C1, C2>
{
	public bool needsMove;

	public Slinq<Slinq<T, C1>, C2> chained;

	public Slinq<T, C1> selected;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FlattenContext<T, C1, C2>> skip = Skip;

	public static readonly Mutator<T, FlattenContext<T, C1, C2>> remove = Remove;

	public static readonly Mutator<T, FlattenContext<T, C1, C2>> dispose = Dispose;

	public FlattenContext(Slinq<Slinq<T, C1>, C2> chained)
	{
		needsMove = false;
		this.chained = chained;
		selected = (chained.current.isSome ? chained.current.value : default(Slinq<T, C1>));
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FlattenContext<T, C1, C2>> Flatten(Slinq<Slinq<T, C1>, C2> slinq)
	{
		return new Slinq<T, FlattenContext<T, C1, C2>>(skip, remove, dispose, new FlattenContext<T, C1, C2>(slinq));
	}

	public static void Skip(ref FlattenContext<T, C1, C2> context, out Option<T> next)
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
				context.selected = context.chained.current.value;
				if (context.selected.current.isSome)
				{
					break;
				}
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
		}
		next = context.selected.current;
	}

	public static void Remove(ref FlattenContext<T, C1, C2> context, out Option<T> next)
	{
		context.needsMove = false;
		context.selected.remove(ref context.selected.context, out context.selected.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref FlattenContext<T, C1, C2> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		context.selected.dispose(ref context.selected.context, out context.selected.current);
	}
}
public struct FlattenContext<T, U>
{
	public bool needsMove;

	public Slinq<Option<T>, U> chained;

	public BacktrackDetector bd;

	public static readonly Mutator<T, FlattenContext<T, U>> skip = Skip;

	public static readonly Mutator<T, FlattenContext<T, U>> remove = Remove;

	public static readonly Mutator<T, FlattenContext<T, U>> dispose = Dispose;

	public FlattenContext(Slinq<Option<T>, U> chained)
	{
		needsMove = false;
		this.chained = chained;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, FlattenContext<T, U>> Flatten(Slinq<Option<T>, U> slinq)
	{
		return new Slinq<T, FlattenContext<T, U>>(skip, remove, dispose, new FlattenContext<T, U>(slinq));
	}

	public static void Skip(ref FlattenContext<T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		while (context.chained.current.isSome && !context.chained.current.value.isSome)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		if (context.chained.current.isSome)
		{
			next = context.chained.current.value;
		}
		else
		{
			next = default(Option<T>);
		}
	}

	public static void Remove(ref FlattenContext<T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref FlattenContext<T, U> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
	}
}
