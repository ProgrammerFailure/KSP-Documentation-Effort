using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct EitherContext<C2, T, U>
{
	public readonly bool isLeft;

	public bool needsMove;

	public Slinq<T, U> left;

	public Slinq<T, C2> right;

	public BacktrackDetector bd;

	public static readonly Mutator<T, EitherContext<C2, T, U>> skip = Skip;

	public static readonly Mutator<T, EitherContext<C2, T, U>> remove = Remove;

	public static readonly Mutator<T, EitherContext<C2, T, U>> dispose = Dispose;

	public EitherContext(Slinq<T, U> left, Slinq<T, C2> right, bool isLeft)
	{
		needsMove = false;
		this.isLeft = isLeft;
		this.left = left;
		this.right = right;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, EitherContext<C2, T, U>> Left(Slinq<T, U> left)
	{
		return new Slinq<T, EitherContext<C2, T, U>>(skip, remove, dispose, new EitherContext<C2, T, U>(left, default(Slinq<T, C2>), isLeft: true));
	}

	public static Slinq<T, EitherContext<C2, T, U>> Right(Slinq<T, C2> right)
	{
		return new Slinq<T, EitherContext<C2, T, U>>(skip, remove, dispose, new EitherContext<C2, T, U>(default(Slinq<T, U>), right, isLeft: false));
	}

	public static void Skip(ref EitherContext<C2, T, U> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			if (context.isLeft)
			{
				context.left.skip(ref context.left.context, out context.left.current);
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
		next = (context.isLeft ? context.left.current : context.right.current);
	}

	public static void Remove(ref EitherContext<C2, T, U> context, out Option<T> next)
	{
		context.needsMove = false;
		if (context.isLeft)
		{
			context.left.remove(ref context.left.context, out context.left.current);
		}
		else
		{
			context.right.remove(ref context.right.context, out context.right.current);
		}
		Skip(ref context, out next);
	}

	public static void Dispose(ref EitherContext<C2, T, U> context, out Option<T> next)
	{
		next = Option<T>.None;
		if (context.isLeft)
		{
			context.left.dispose(ref context.left.context, out context.left.current);
		}
		else
		{
			context.right.dispose(ref context.right.context, out context.right.current);
		}
	}
}
