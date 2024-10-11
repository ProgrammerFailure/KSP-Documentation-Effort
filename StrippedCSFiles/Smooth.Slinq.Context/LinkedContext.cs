using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct LinkedContext<T>
{
	private readonly LinkedHeadTail<T> list;

	private readonly bool release;

	private Linked<T> runner;

	private BacktrackDetector bd;

	private static readonly Mutator<T, LinkedContext<T>> skip;

	private static readonly Mutator<T, LinkedContext<T>> remove;

	private static readonly Mutator<T, LinkedContext<T>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LinkedContext(LinkedHeadTail<T> list, BacktrackDetector bd, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LinkedContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> Slinq(LinkedHeadTail<T> list, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<T>> Slinq(LinkedHeadTail<T> list, BacktrackDetector bd, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref LinkedContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref LinkedContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref LinkedContext<T> context, out Option<T> next)
	{
		throw null;
	}
}
public struct LinkedContext<K, T>
{
	private readonly LinkedHeadTail<K, T> list;

	private readonly bool release;

	private Linked<K, T> runner;

	private BacktrackDetector bd;

	private static readonly Mutator<T, LinkedContext<K, T>> skip;

	private static readonly Mutator<T, LinkedContext<K, T>> remove;

	private static readonly Mutator<T, LinkedContext<K, T>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LinkedContext(LinkedHeadTail<K, T> list, BacktrackDetector bd, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LinkedContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> Slinq(LinkedHeadTail<K, T> list, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedContext<K, T>> Slinq(LinkedHeadTail<K, T> list, BacktrackDetector bd, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref LinkedContext<K, T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref LinkedContext<K, T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref LinkedContext<K, T> context, out Option<T> next)
	{
		throw null;
	}
}
