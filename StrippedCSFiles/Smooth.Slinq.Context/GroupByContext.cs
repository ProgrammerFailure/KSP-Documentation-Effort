using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct GroupByContext<K, T>
{
	private readonly Lookup<K, T> lookup;

	private readonly bool release;

	private Linked<K> runner;

	private BacktrackDetector bd;

	private static readonly Mutator<Grouping<K, T>, GroupByContext<K, T>> linkedSkip;

	private static readonly Mutator<Grouping<K, T>, GroupByContext<K, T>> linkedRemove;

	private static readonly Mutator<Grouping<K, T>, GroupByContext<K, T>> linkedDispose;

	private static readonly Mutator<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> slinqedSkip;

	private static readonly Mutator<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> slinqedRemove;

	private static readonly Mutator<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> slinqedDispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GroupByContext(Lookup<K, T> lookup, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GroupByContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T, LinkedContext<T>>, GroupByContext<K, T>> Slinq(Lookup<K, T> lookup, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Grouping<K, T>, GroupByContext<K, T>> SlinqLinked(Lookup<K, T> lookup, bool release)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LinkedSkip(ref GroupByContext<K, T> context, out Option<Grouping<K, T>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LinkedRemove(ref GroupByContext<K, T> context, out Option<Grouping<K, T>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LinkedDispose(ref GroupByContext<K, T> context, out Option<Grouping<K, T>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SlinqedSkip(ref GroupByContext<K, T> context, out Option<Grouping<K, T, LinkedContext<T>>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SlinqedRemove(ref GroupByContext<K, T> context, out Option<Grouping<K, T, LinkedContext<T>>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SlinqedDispose(ref GroupByContext<K, T> context, out Option<Grouping<K, T, LinkedContext<T>>> next)
	{
		throw null;
	}
}
