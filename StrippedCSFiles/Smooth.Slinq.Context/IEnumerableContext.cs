using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct IEnumerableContext<T>
{
	private readonly IEnumerator<T> enumerator;

	private BacktrackDetector bd;

	private static readonly Mutator<T, IEnumerableContext<T>> skip;

	private static readonly Mutator<T, IEnumerableContext<T>> remove;

	private static readonly Mutator<T, IEnumerableContext<T>> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IEnumerableContext(IEnumerable<T> enumerable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static IEnumerableContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IEnumerableContext<T>> Slinq(IEnumerable<T> enumerable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref IEnumerableContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref IEnumerableContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref IEnumerableContext<T> context, out Option<T> next)
	{
		throw null;
	}
}
