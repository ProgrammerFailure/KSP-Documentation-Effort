using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Smooth.Algebraics;
using Smooth.Delegates;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class SlinqTest : MonoBehaviour
{
	public class Equals_1<T1, T2> : IEquatable<Equals_1<T1, T2>>, IEqualityComparer<Smooth.Algebraics.Tuple<T1, T2>>
	{
		public readonly IEqualityComparer<T1> equalityComparer;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Equals_1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Equals_1(IEqualityComparer<T1> equalityComparer)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override bool Equals(object o)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Equals(Equals_1<T1, T2> other)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override int GetHashCode()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool operator ==(Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool operator !=(Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Equals(Smooth.Algebraics.Tuple<T1, T2> a, Smooth.Algebraics.Tuple<T1, T2> b)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetHashCode(Smooth.Algebraics.Tuple<T1, T2> a)
		{
			throw null;
		}
	}

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int>, Smooth.Algebraics.Tuple<int, int>, bool> eq;

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int, int>, Smooth.Algebraics.Tuple<int, int, int>, bool> eq_t3;

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int>, int> to_1;

	public static readonly Func<Smooth.Algebraics.Tuple<int, int>, int> to_1f;

	public static readonly IEqualityComparer<Smooth.Algebraics.Tuple<int, int>> eq_1;

	public static readonly StringBuilder stringBuilder;

	public static List<Smooth.Algebraics.Tuple<int, int>> tuples1;

	public static List<Smooth.Algebraics.Tuple<int, int>> tuples2;

	public static int loops;

	public int minCount;

	public int maxCount;

	public int minValue;

	public int maxValue;

	public int speedLoops;

	public bool testCorrectness;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SlinqTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SlinqTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TestCorrectness()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Smooth.Algebraics.Tuple<int, int>> RemovableList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LinkedList<Smooth.Algebraics.Tuple<int, int>> RemovableLinkedList()
	{
		throw null;
	}
}
