using System.Runtime.CompilerServices;

namespace UniLinq;

internal static class Check
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Source(object source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Source1AndSource2(object source1, object source2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndFuncAndSelector(object source, object func, object selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndFunc(object source, object func)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndSelector(object source, object selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndPredicate(object source, object predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FirstAndSecond(object first, object second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndKeySelector(object source, object keySelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndKeyElementSelectors(object source, object keySelector, object elementSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndKeyResultSelectors(object source, object keySelector, object resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndCollectionSelectorAndResultSelector(object source, object collectionSelector, object resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SourceAndCollectionSelectors(object source, object collectionSelector, object selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void JoinSelectors(object outer, object inner, object outerKeySelector, object innerKeySelector, object resultSelector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GroupBySelectors(object source, object keySelector, object elementSelector, object resultSelector)
	{
		throw null;
	}
}
