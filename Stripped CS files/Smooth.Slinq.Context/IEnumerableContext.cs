using System;
using System.Collections.Generic;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct IEnumerableContext<T>
{
	public readonly IEnumerator<T> enumerator;

	public BacktrackDetector bd;

	public static readonly Mutator<T, IEnumerableContext<T>> skip = Skip;

	public static readonly Mutator<T, IEnumerableContext<T>> remove = Remove;

	public static readonly Mutator<T, IEnumerableContext<T>> dispose = Dispose;

	public IEnumerableContext(IEnumerable<T> enumerable)
	{
		enumerator = enumerable.GetEnumerator();
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, IEnumerableContext<T>> Slinq(IEnumerable<T> enumerable)
	{
		return new Slinq<T, IEnumerableContext<T>>(skip, remove, dispose, new IEnumerableContext<T>(enumerable));
	}

	public static void Skip(ref IEnumerableContext<T> context, out Option<T> next)
	{
		if (context.enumerator.MoveNext())
		{
			next = new Option<T>(context.enumerator.Current);
			return;
		}
		next = default(Option<T>);
		context.enumerator.Dispose();
	}

	public static void Remove(ref IEnumerableContext<T> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref IEnumerableContext<T> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.enumerator.Dispose();
	}
}
