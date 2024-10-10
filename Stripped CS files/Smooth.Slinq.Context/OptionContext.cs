using System;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct OptionContext<T>
{
	public Option<T> option;

	public BacktrackDetector bd;

	public static readonly Mutator<T, OptionContext<T>> remove = Remove;

	public static readonly Mutator<T, OptionContext<T>> dispose = Dispose;

	public static readonly Mutator<T, OptionContext<T>> optionSkip = OptionSkip;

	public static readonly Mutator<T, OptionContext<T>> repeatSkip = RepeatSkip;

	public OptionContext(Option<T> option)
	{
		this.option = option;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, OptionContext<T>> Slinq(Option<T> option)
	{
		return new Slinq<T, OptionContext<T>>(optionSkip, remove, dispose, new OptionContext<T>(option));
	}

	public static Slinq<T, OptionContext<T>> Repeat(T value)
	{
		return new Slinq<T, OptionContext<T>>(repeatSkip, remove, dispose, new OptionContext<T>(new Option<T>(value)));
	}

	public static void Remove(ref OptionContext<T> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref OptionContext<T> context, out Option<T> next)
	{
		next = default(Option<T>);
	}

	public static void OptionSkip(ref OptionContext<T> context, out Option<T> next)
	{
		next = context.option;
		if (context.option.isSome)
		{
			context.option = default(Option<T>);
		}
	}

	public static void RepeatSkip(ref OptionContext<T> context, out Option<T> next)
	{
		next = context.option;
	}
}
