using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq.Context;

namespace Smooth.Slinq;

public static class Slinqable
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, Unit> Empty<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IEnumerableContext<T>> Slinq<T>(this IEnumerable<T> enumerable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<Tuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list, int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedListNode<T> node, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedListNode<T> node, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, LinkedListContext<T>> SlinqDescending<T>(this LinkedList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodesDescending<T>(this LinkedList<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, OptionContext<T>> Slinq<T>(this Option<T> option)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<int, FuncOptionContext<int, int>> Range(int start, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, OptionContext<T>> Repeat<T>(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, IntContext<T, OptionContext<T>>> Repeat<T>(T value, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncContext<T>> Sequence<T>(T seed, DelegateFunc<T, T> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncContext<T, P>> Sequence<T, P>(T seed, DelegateFunc<T, P, T> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncOptionContext<T>> Sequence<T>(T seed, DelegateFunc<T, Option<T>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<T, FuncOptionContext<T, P>> Sequence<T, P>(T seed, DelegateFunc<T, P, Option<T>> selector, P parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<byte, FuncContext<byte, byte>> Sequence(byte start, byte step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<sbyte, FuncContext<sbyte, sbyte>> Sequence(sbyte start, sbyte step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<short, FuncContext<short, short>> Sequence(short start, short step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<ushort, FuncContext<ushort, ushort>> Sequence(ushort start, ushort step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<int, FuncContext<int, int>> Sequence(int start, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<uint, FuncContext<uint, uint>> Sequence(uint start, uint step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<long, FuncContext<long, long>> Sequence(long start, long step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<ulong, FuncContext<ulong, ulong>> Sequence(ulong start, ulong step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<float, FuncContext<float, float>> Sequence(float start, float step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<double, FuncContext<double, double>> Sequence(double start, double step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Slinq<decimal, FuncContext<decimal, decimal>> Sequence(decimal start, decimal step)
	{
		throw null;
	}
}
