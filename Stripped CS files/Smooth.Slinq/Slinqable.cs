using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq.Context;

namespace Smooth.Slinq;

public static class Slinqable
{
	public static Slinq<T, Unit> Empty<T>()
	{
		return default(Slinq<T, Unit>);
	}

	public static Slinq<T, IEnumerableContext<T>> Slinq<T>(this IEnumerable<T> enumerable)
	{
		return IEnumerableContext<T>.Slinq(enumerable);
	}

	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex, int step)
	{
		return IListContext<T>.Slinq(list, startIndex, step);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex, int step)
	{
		return IListContext<T>.SlinqWithIndex(list, startIndex, step);
	}

	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list)
	{
		return IListContext<T>.Slinq(list, 0, 1);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list)
	{
		return IListContext<T>.SlinqWithIndex(list, 0, 1);
	}

	public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex)
	{
		return IListContext<T>.Slinq(list, startIndex, 1);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex)
	{
		return IListContext<T>.SlinqWithIndex(list, startIndex, 1);
	}

	public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list)
	{
		return IListContext<T>.Slinq(list, list.Count - 1, -1);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list)
	{
		return IListContext<T>.SlinqWithIndex(list, list.Count - 1, -1);
	}

	public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list, int startIndex)
	{
		return IListContext<T>.Slinq(list, startIndex, -1);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list, int startIndex)
	{
		return IListContext<T>.SlinqWithIndex(list, startIndex, -1);
	}

	public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedListNode<T> node, int step)
	{
		return LinkedListContext<T>.Slinq(node, step);
	}

	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedListNode<T> node, int step)
	{
		return LinkedListContext<T>.SlinqNodes(node, step);
	}

	public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedList<T> list)
	{
		return LinkedListContext<T>.Slinq(list.First, 1);
	}

	public static Slinq<T, LinkedListContext<T>> SlinqDescending<T>(this LinkedList<T> list)
	{
		return LinkedListContext<T>.Slinq(list.Last, -1);
	}

	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedList<T> list)
	{
		return LinkedListContext<T>.SlinqNodes(list.First, 1);
	}

	public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodesDescending<T>(this LinkedList<T> list)
	{
		return LinkedListContext<T>.SlinqNodes(list.Last, -1);
	}

	public static Slinq<T, OptionContext<T>> Slinq<T>(this Option<T> option)
	{
		return OptionContext<T>.Slinq(option);
	}

	public static Slinq<int, FuncOptionContext<int, int>> Range(int start, int count)
	{
		if (count > 0)
		{
			int num = start + count - 1;
			if (num >= start)
			{
				return FuncOptionContext<int, int>.Sequence(start, (int acc, int last) => (++acc <= last) ? new Option<int>(acc) : default(Option<int>), num);
			}
			throw new ArgumentOutOfRangeException();
		}
		if (count != 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		return default(Slinq<int, FuncOptionContext<int, int>>);
	}

	public static Slinq<T, OptionContext<T>> Repeat<T>(T value)
	{
		return OptionContext<T>.Repeat(value);
	}

	public static Slinq<T, IntContext<T, OptionContext<T>>> Repeat<T>(T value, int count)
	{
		if (count > 0)
		{
			return OptionContext<T>.Repeat(value).Take(count);
		}
		if (count != 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		return default(Slinq<T, IntContext<T, OptionContext<T>>>);
	}

	public static Slinq<T, FuncContext<T>> Sequence<T>(T seed, DelegateFunc<T, T> selector)
	{
		return FuncContext<T>.Sequence(seed, selector);
	}

	public static Slinq<T, FuncContext<T, U>> Sequence<T, U>(T seed, DelegateFunc<T, U, T> selector, U parameter)
	{
		return FuncContext<T, U>.Sequence(seed, selector, parameter);
	}

	public static Slinq<T, FuncOptionContext<T>> Sequence<T>(T seed, DelegateFunc<T, Option<T>> selector)
	{
		return FuncOptionContext<T>.Sequence(seed, selector);
	}

	public static Slinq<T, FuncOptionContext<T, U>> Sequence<T, U>(T seed, DelegateFunc<T, U, Option<T>> selector, U parameter)
	{
		return FuncOptionContext<T, U>.Sequence(seed, selector, parameter);
	}

	public static Slinq<byte, FuncContext<byte, byte>> Sequence(byte start, byte step)
	{
		return FuncContext<byte, byte>.Sequence(start, (byte x, byte s) => (byte)(x + s), step);
	}

	public static Slinq<sbyte, FuncContext<sbyte, sbyte>> Sequence(sbyte start, sbyte step)
	{
		return FuncContext<sbyte, sbyte>.Sequence(start, (sbyte x, sbyte s) => (sbyte)(x + s), step);
	}

	public static Slinq<short, FuncContext<short, short>> Sequence(short start, short step)
	{
		return FuncContext<short, short>.Sequence(start, (short x, short s) => (short)(x + s), step);
	}

	public static Slinq<ushort, FuncContext<ushort, ushort>> Sequence(ushort start, ushort step)
	{
		return FuncContext<ushort, ushort>.Sequence(start, (ushort x, ushort s) => (ushort)(x + s), step);
	}

	public static Slinq<int, FuncContext<int, int>> Sequence(int start, int step)
	{
		return FuncContext<int, int>.Sequence(start, (int x, int s) => x + s, step);
	}

	public static Slinq<uint, FuncContext<uint, uint>> Sequence(uint start, uint step)
	{
		return FuncContext<uint, uint>.Sequence(start, (uint x, uint s) => x + s, step);
	}

	public static Slinq<long, FuncContext<long, long>> Sequence(long start, long step)
	{
		return FuncContext<long, long>.Sequence(start, (long x, long s) => x + s, step);
	}

	public static Slinq<ulong, FuncContext<ulong, ulong>> Sequence(ulong start, ulong step)
	{
		return FuncContext<ulong, ulong>.Sequence(start, (ulong x, ulong s) => x + s, step);
	}

	public static Slinq<float, FuncContext<float, float>> Sequence(float start, float step)
	{
		return FuncContext<float, float>.Sequence(start, (float x, float s) => x + s, step);
	}

	public static Slinq<double, FuncContext<double, double>> Sequence(double start, double step)
	{
		return FuncContext<double, double>.Sequence(start, (double x, double s) => x + s, step);
	}

	public static Slinq<decimal, FuncContext<decimal, decimal>> Sequence(decimal start, decimal step)
	{
		return FuncContext<decimal, decimal>.Sequence(start, (decimal x, decimal s) => x + s, step);
	}
}
