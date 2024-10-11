using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct OptionContext<T>
{
	private Option<T> option;

	private BacktrackDetector bd;

	private static readonly Mutator<T, OptionContext<T>> remove;

	private static readonly Mutator<T, OptionContext<T>> dispose;

	private static readonly Mutator<T, OptionContext<T>> optionSkip;

	private static readonly Mutator<T, OptionContext<T>> repeatSkip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private OptionContext(Option<T> option)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OptionContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, OptionContext<T>> Slinq(Option<T> option)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, OptionContext<T>> Repeat(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref OptionContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref OptionContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OptionSkip(ref OptionContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RepeatSkip(ref OptionContext<T> context, out Option<T> next)
	{
		throw null;
	}
}
