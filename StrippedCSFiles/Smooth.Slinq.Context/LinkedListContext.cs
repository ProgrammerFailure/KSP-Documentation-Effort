using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public struct LinkedListContext<T>
{
	private bool needsMove;

	private LinkedListNode<T> node;

	private readonly int step;

	private BacktrackDetector bd;

	private static readonly Mutator<T, LinkedListContext<T>> skip;

	private static readonly Mutator<T, LinkedListContext<T>> remove;

	private static readonly Mutator<T, LinkedListContext<T>> dispose;

	private static readonly Mutator<LinkedListNode<T>, LinkedListContext<T>> skipNodes;

	private static readonly Mutator<LinkedListNode<T>, LinkedListContext<T>> removeNodes;

	private static readonly Mutator<LinkedListNode<T>, LinkedListContext<T>> disposeNodes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LinkedListContext(LinkedListNode<T> node, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LinkedListContext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedListContext<T>> Slinq(LinkedListNode<T> node, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes(LinkedListNode<T> node, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Skip(ref LinkedListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Remove(ref LinkedListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose(ref LinkedListContext<T> context, out Option<T> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SkipNodes(ref LinkedListContext<T> context, out Option<LinkedListNode<T>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RemoveNodes(ref LinkedListContext<T> context, out Option<LinkedListNode<T>> next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DisposeNodes(ref LinkedListContext<T> context, out Option<LinkedListNode<T>> next)
	{
		throw null;
	}
}
