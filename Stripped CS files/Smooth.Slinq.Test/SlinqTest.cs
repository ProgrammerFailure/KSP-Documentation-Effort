using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smooth.Algebraics;
using Smooth.Collections;
using Smooth.Delegates;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class SlinqTest : MonoBehaviour
{
	public class Equals_1<T1, T2> : IEquatable<Equals_1<T1, T2>>, IEqualityComparer<Smooth.Algebraics.Tuple<T1, T2>>
	{
		public readonly IEqualityComparer<T1> equalityComparer;

		public Equals_1()
		{
			equalityComparer = Smooth.Collections.EqualityComparer<T1>.Default;
		}

		public Equals_1(IEqualityComparer<T1> equalityComparer)
		{
			this.equalityComparer = equalityComparer;
		}

		public override bool Equals(object o)
		{
			if (o is Equals_1<T1, T2>)
			{
				return Equals((Equals_1<T1, T2>)o);
			}
			return false;
		}

		public bool Equals(Equals_1<T1, T2> other)
		{
			return equalityComparer == other.equalityComparer;
		}

		public override int GetHashCode()
		{
			return equalityComparer.GetHashCode();
		}

		public static bool operator ==(Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs)
		{
			return !lhs.Equals(rhs);
		}

		public bool Equals(Smooth.Algebraics.Tuple<T1, T2> a, Smooth.Algebraics.Tuple<T1, T2> b)
		{
			return equalityComparer.Equals(a.Item1, b.Item1);
		}

		public int GetHashCode(Smooth.Algebraics.Tuple<T1, T2> a)
		{
			return equalityComparer.GetHashCode(a.Item1);
		}
	}

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int>, Smooth.Algebraics.Tuple<int, int>, bool> eq = (Smooth.Algebraics.Tuple<int, int> a, Smooth.Algebraics.Tuple<int, int> b) => a == b;

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int, int>, Smooth.Algebraics.Tuple<int, int, int>, bool> eq_t3 = (Smooth.Algebraics.Tuple<int, int, int> a, Smooth.Algebraics.Tuple<int, int, int> b) => a == b;

	public static readonly DelegateFunc<Smooth.Algebraics.Tuple<int, int>, int> to_1 = (Smooth.Algebraics.Tuple<int, int> t) => t.Item1;

	public static readonly Func<Smooth.Algebraics.Tuple<int, int>, int> to_1f = (Smooth.Algebraics.Tuple<int, int> t) => t.Item1;

	public static readonly IEqualityComparer<Smooth.Algebraics.Tuple<int, int>> eq_1 = new Equals_1<int, int>();

	public static readonly StringBuilder stringBuilder = new StringBuilder();

	public static List<Smooth.Algebraics.Tuple<int, int>> tuples1 = new List<Smooth.Algebraics.Tuple<int, int>>();

	public static List<Smooth.Algebraics.Tuple<int, int>> tuples2 = new List<Smooth.Algebraics.Tuple<int, int>>();

	public static int loops = 1;

	public int minCount = 50;

	public int maxCount = 100;

	public int minValue = 1;

	public int maxValue = 100;

	public int speedLoops = 10;

	public bool testCorrectness;

	public void Start()
	{
		tuples1 = new List<Smooth.Algebraics.Tuple<int, int>>(maxCount);
		tuples2 = new List<Smooth.Algebraics.Tuple<int, int>>(maxCount);
		loops = speedLoops;
		Debug.Log("Element count: " + minCount + " to " + maxCount + ", value range: " + minValue + " to " + maxValue + ", loops: " + loops);
		if (testCorrectness)
		{
			Debug.Log("Testing Correctness.");
		}
	}

	public void Update()
	{
		if (testCorrectness)
		{
			TestCorrectness();
		}
	}

	public void LateUpdate()
	{
		loops = speedLoops;
		tuples1.Clear();
		tuples2.Clear();
		int num = UnityEngine.Random.Range(minCount, maxCount + 1);
		for (int i = 0; i < num; i++)
		{
			tuples1.Add(new Smooth.Algebraics.Tuple<int, int>(UnityEngine.Random.Range(minValue, maxValue + 1), i));
			tuples2.Add(new Smooth.Algebraics.Tuple<int, int>(UnityEngine.Random.Range(minValue, maxValue + 1), i));
		}
	}

	public void TestCorrectness()
	{
		Smooth.Algebraics.Tuple<int, int> value = tuples2.Slinq().FirstOrDefault();
		int testInt = value.Item1;
		int testInt2 = testInt * (maxValue - minValue + 1) / 25;
		int midSkip = ((UnityEngine.Random.value < 0.5f) ? testInt : 0);
		if (tuples1.Slinq().Aggregate(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => acc + next.Item1) != tuples1.Aggregate(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => acc + next.Item1))
		{
			Debug.LogError("Aggregate failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().Aggregate(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => acc + next.Item1, (int acc) => -acc) != tuples1.Aggregate(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => acc + next.Item1, (int acc) => -acc))
		{
			Debug.LogError("Aggregate failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().AggregateWhile(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => (acc >= testInt2) ? default(Option<int>) : new Option<int>(acc + next.Item1)) != (from acc in tuples1.Slinq().AggregateRunning(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => acc + next.Item1)
			where acc >= testInt2
			select acc).FirstOrDefault())
		{
			Debug.LogError("AggregateWhile / AggregateRunning failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().All((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2) ^ tuples1.All((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2))
		{
			Debug.LogError("All failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().All((Smooth.Algebraics.Tuple<int, int> x, int c) => x.Item1 < c, testInt2) ^ tuples1.All((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2))
		{
			Debug.LogError("All failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().Any((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 > testInt2) ^ tuples1.Any((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 > testInt2))
		{
			Debug.LogError("All failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().Any((Smooth.Algebraics.Tuple<int, int> x, int c) => x.Item1 > c, testInt2) ^ tuples1.Any((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 > testInt2))
		{
			Debug.LogError("All failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Select(to_1).AverageOrNone()
			.Cata((double avg) => avg == tuples1.Select(to_1f).Average(), tuples1.Count == 0))
		{
			Debug.LogError("AverageOrNone failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Concat(tuples2.Slinq()).SequenceEqual(tuples1.Concat(tuples2).Slinq(), eq))
		{
			Debug.LogError("Concat failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().Contains(value, eq_1) ^ tuples1.Contains(value, eq_1))
		{
			Debug.LogError("Contains failed.");
			testCorrectness = false;
		}
		if ((from x in tuples1.Slinq()
			where x.Item1 < testInt
			select x).Count() != tuples1.Where((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt).Count())
		{
			Debug.LogError("Count failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Distinct(eq_1).SequenceEqual(tuples1.Distinct(eq_1).Slinq(), eq))
		{
			Debug.LogError("Distinct failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Except(tuples2.Slinq(), eq_1).SequenceEqual(tuples1.Except(tuples2, eq_1).Slinq(), eq))
		{
			Debug.LogError("Except failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().FirstOrNone().Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.First(), !tuples1.Any()))
		{
			Debug.LogError("FirstOrNone failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().FirstOrNone((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt).Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.First((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt), !tuples1.Where((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt).Any()))
		{
			Debug.LogError("FirstOrNone(predicate) failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().FirstOrNone((Smooth.Algebraics.Tuple<int, int> x, int t) => x.Item1 < t, testInt).Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.First((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt), !tuples1.Where((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt).Any()))
		{
			Debug.LogError("FirstOrNone(predicate, parameter) failed.");
			testCorrectness = false;
		}
		if (!(from x in new List<Smooth.Algebraics.Tuple<int, int>>[2] { tuples1, tuples2 }.Slinq()
			select x.Slinq()).Flatten().SequenceEqual(tuples1.Concat(tuples2).Slinq(), eq))
		{
			Debug.LogError("Flatten failed.");
			testCorrectness = false;
		}
		int feAcc = 0;
		tuples1.Slinq().ForEach(delegate(Smooth.Algebraics.Tuple<int, int> x)
		{
			feAcc += x.Item1;
		});
		if (feAcc != tuples1.Slinq().Select(to_1).Sum())
		{
			Debug.LogError("ForEach failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Intersect(tuples2.Slinq(), eq_1).SequenceEqual(tuples1.Intersect(tuples2, eq_1).Slinq(), eq))
		{
			Debug.LogError("Intersect failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().GroupBy(to_1).SequenceEqual(tuples1.GroupBy(to_1f).Slinq(), (Grouping<int, Smooth.Algebraics.Tuple<int, int>, LinkedContext<Smooth.Algebraics.Tuple<int, int>>> s, IGrouping<int, Smooth.Algebraics.Tuple<int, int>> l) => s.key == l.Key && s.values.SequenceEqual(l.Slinq(), eq)))
		{
			Debug.LogError("GroupBy failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().GroupJoin(tuples2.Slinq(), to_1, to_1, (Smooth.Algebraics.Tuple<int, int> a, Slinq<Smooth.Algebraics.Tuple<int, int>, LinkedContext<Smooth.Algebraics.Tuple<int, int>>> bs) => Smooth.Algebraics.Tuple.Create(a.Item1, a.Item2, bs.Count())).SequenceEqual(tuples1.GroupJoin(tuples2, to_1f, to_1f, (Smooth.Algebraics.Tuple<int, int> a, IEnumerable<Smooth.Algebraics.Tuple<int, int>> bs) => Smooth.Algebraics.Tuple.Create(a.Item1, a.Item2, bs.Count())).Slinq(), eq_t3))
		{
			Debug.LogError("GroupJoin failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Join(tuples2.Slinq(), to_1, to_1, (Smooth.Algebraics.Tuple<int, int> a, Smooth.Algebraics.Tuple<int, int> b) => Smooth.Algebraics.Tuple.Create(a.Item1, a.Item2, b.Item2)).SequenceEqual(tuples1.Join(tuples2, to_1f, to_1f, (Smooth.Algebraics.Tuple<int, int> a, Smooth.Algebraics.Tuple<int, int> b) => Smooth.Algebraics.Tuple.Create(a.Item1, a.Item2, b.Item2)).Slinq(), eq_t3))
		{
			Debug.LogError("Join failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().LastOrNone().Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.Last(), !tuples1.Any()))
		{
			Debug.LogError("LastOrNone failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().LastOrNone((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt).Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.Last((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt), !tuples1.Where((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt).Any()))
		{
			Debug.LogError("LastOrNone(predicate) failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().LastOrNone((Smooth.Algebraics.Tuple<int, int> x, int t) => x.Item1 < t, testInt).Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.Last((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt), !tuples1.Where((Smooth.Algebraics.Tuple<int, int> z) => z.Item1 < testInt).Any()))
		{
			Debug.LogError("LastOrNone(predicate, parameter) failed.");
			testCorrectness = false;
		}
		if (tuples1.Count > 0 && tuples1.Slinq().Max() != tuples1.Max())
		{
			Debug.LogError("Max failed.");
			testCorrectness = false;
		}
		if (tuples1.Count > 0 && tuples1.Slinq().Min() != tuples1.Min())
		{
			Debug.LogError("Min failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().MaxOrNone().Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.Max(), tuples1.Count == 0))
		{
			Debug.LogError("MaxOrNone failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().MinOrNone().Cata((Smooth.Algebraics.Tuple<int, int> x) => x == tuples1.Min(), tuples1.Count == 0))
		{
			Debug.LogError("MinOrNone failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderBy(to_1).SequenceEqual(tuples1.OrderBy(to_1f).Slinq(), eq))
		{
			Debug.LogError("OrderBy failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderByDescending(to_1).SequenceEqual(tuples1.OrderByDescending(to_1f).Slinq(), eq))
		{
			Debug.LogError("OrderByDescending failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderBy().SequenceEqual(tuples1.OrderBy((Smooth.Algebraics.Tuple<int, int> x) => x).Slinq(), eq))
		{
			Debug.LogError("OrderBy keyless failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderByDescending().SequenceEqual(tuples1.OrderByDescending((Smooth.Algebraics.Tuple<int, int> x) => x).Slinq(), eq))
		{
			Debug.LogError("OrderByDescending keyless failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderByGroup(to_1).SequenceEqual(tuples1.OrderBy(to_1f).Slinq(), eq))
		{
			Debug.LogError("OrderByGroup failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().OrderByGroupDescending(to_1).SequenceEqual(tuples1.OrderByDescending(to_1f).Slinq(), eq))
		{
			Debug.LogError("OrderByGroupDescending failed.");
			testCorrectness = false;
		}
		List<Smooth.Algebraics.Tuple<int, int>> list = RemovableList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, IListContext<Smooth.Algebraics.Tuple<int, int>>> slinq = list.Slinq().Skip(midSkip);
		for (int i = 0; i < testInt; i++)
		{
			slinq.Remove();
		}
		if (!list.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove failed.");
			testCorrectness = false;
		}
		List<Smooth.Algebraics.Tuple<int, int>> list2 = RemovableList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, IListContext<Smooth.Algebraics.Tuple<int, int>>> slinq2 = list2.SlinqDescending().Skip(midSkip);
		for (int j = 0; j < testInt; j++)
		{
			slinq2.Remove();
		}
		if (!list2.SlinqDescending().Skip(midSkip).SequenceEqual(tuples1.SlinqDescending().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove descending failed.");
			testCorrectness = false;
		}
		List<Smooth.Algebraics.Tuple<int, int>> list3 = RemovableList();
		list3.Slinq().Skip(midSkip).Remove(testInt);
		if (!list3.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove(int) failed.");
			testCorrectness = false;
		}
		List<Smooth.Algebraics.Tuple<int, int>> list4 = RemovableList();
		list4.Slinq().Skip(midSkip).RemoveWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2);
		if (!list4.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).SkipWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2), eq))
		{
			Debug.LogError("RemoveWhile failed.");
			testCorrectness = false;
		}
		List<Smooth.Algebraics.Tuple<int, int>> list5 = RemovableList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, IListContext<Smooth.Algebraics.Tuple<int, int>>> other = tuples1.Slinq().Skip(midSkip);
		int num = list5.Slinq().Skip(midSkip).RemoveWhile(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => (acc >= testInt2) ? default(Option<int>) : new Option<int>(acc + next.Item1));
		int num2 = other.SkipWhile(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => (acc >= testInt2) ? default(Option<int>) : new Option<int>(acc + next.Item1));
		if (num != num2 || !list5.Slinq().Skip(midSkip).SequenceEqual(other, eq))
		{
			Debug.LogError("RemoveWhile aggregating failed.");
			testCorrectness = false;
		}
		LinkedList<Smooth.Algebraics.Tuple<int, int>> list6 = RemovableLinkedList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, LinkedListContext<Smooth.Algebraics.Tuple<int, int>>> slinq3 = list6.Slinq().Skip(midSkip);
		for (int k = 0; k < testInt; k++)
		{
			slinq3.Remove();
		}
		if (!list6.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove LL failed.");
			testCorrectness = false;
		}
		LinkedList<Smooth.Algebraics.Tuple<int, int>> list7 = RemovableLinkedList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, LinkedListContext<Smooth.Algebraics.Tuple<int, int>>> slinq4 = list7.SlinqDescending().Skip(midSkip);
		for (int m = 0; m < testInt; m++)
		{
			slinq4.Remove();
		}
		if (!list7.SlinqDescending().Skip(midSkip).SequenceEqual(tuples1.SlinqDescending().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove descending LL failed.");
			testCorrectness = false;
		}
		LinkedList<Smooth.Algebraics.Tuple<int, int>> list8 = RemovableLinkedList();
		list8.Slinq().Skip(midSkip).Remove(testInt);
		if (!list8.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).Skip(testInt), eq))
		{
			Debug.LogError("Remove(int) LL failed.");
			testCorrectness = false;
		}
		LinkedList<Smooth.Algebraics.Tuple<int, int>> list9 = RemovableLinkedList();
		list9.Slinq().Skip(midSkip).RemoveWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2);
		if (!list9.Slinq().Skip(midSkip).SequenceEqual(tuples1.Slinq().Skip(midSkip).SkipWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2), eq))
		{
			Debug.LogError("RemoveWhile LL failed.");
			testCorrectness = false;
		}
		LinkedList<Smooth.Algebraics.Tuple<int, int>> list10 = RemovableLinkedList();
		Slinq<Smooth.Algebraics.Tuple<int, int>, IListContext<Smooth.Algebraics.Tuple<int, int>>> other2 = tuples1.Slinq().Skip(midSkip);
		int num3 = list10.Slinq().Skip(midSkip).RemoveWhile(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => (acc >= testInt2) ? default(Option<int>) : new Option<int>(acc + next.Item1));
		int num4 = other2.SkipWhile(0, (int acc, Smooth.Algebraics.Tuple<int, int> next) => (acc >= testInt2) ? default(Option<int>) : new Option<int>(acc + next.Item1));
		if (num3 != num4 || !list10.Slinq().Skip(midSkip).SequenceEqual(other2, eq))
		{
			Debug.LogError("RemoveWhile aggregating LL failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Reverse().SequenceEqual(Enumerable.Reverse(tuples1).Slinq(), eq))
		{
			Debug.LogError("Reverse failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Select(to_1).SequenceEqual(tuples1.Select(to_1f).Slinq()))
		{
			Debug.LogError("Select failed.");
			testCorrectness = false;
		}
		if (!new List<Smooth.Algebraics.Tuple<int, int>>[2] { tuples1, tuples2 }.Slinq().SelectMany((List<Smooth.Algebraics.Tuple<int, int>> x) => x.Slinq()).SequenceEqual(tuples1.Concat(tuples2).Slinq(), eq))
		{
			Debug.LogError("SelectMany failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().SelectMany((Smooth.Algebraics.Tuple<int, int> x) => (x.Item1 >= testInt) ? default(Option<int>) : new Option<int>(x.Item1)).SequenceEqual((from x in tuples1
			where x.Item1 < testInt
			select x.Item1).Slinq()))
		{
			Debug.LogError("SelectMany option failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().SequenceEqual(tuples1.Slinq(), eq))
		{
			Debug.LogError("SequenceEqual failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().SequenceEqual(tuples2.Slinq(), eq) ^ tuples1.SequenceEqual(tuples2))
		{
			Debug.LogError("SequenceEqual failed.");
			testCorrectness = false;
		}
		int[] list11 = new int[3] { 0, 0, testInt };
		if (list11.Slinq().SingleOrNone().isSome || list11.Slinq().Skip(1).SingleOrNone()
			.isSome || !list11.Slinq().Skip(2).SingleOrNone()
			.Contains(testInt) || list11.Slinq().Skip(3).SingleOrNone()
			.isSome)
		{
			Debug.LogError("SingleOrNone failed.");
			testCorrectness = false;
		}
		Slinq<Smooth.Algebraics.Tuple<int, int>, IListContext<Smooth.Algebraics.Tuple<int, int>>> other3 = tuples1.Slinq();
		for (int n = 0; n < testInt; n++)
		{
			other3.Skip();
		}
		if (!tuples1.Slinq().Skip(testInt).SequenceEqual(other3, eq))
		{
			Debug.LogError("Skip failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().SkipWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2).SequenceEqual(tuples1.SkipWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2).Slinq(), eq))
		{
			Debug.LogError("SkipWhile failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Reverse().SequenceEqual(tuples1.SlinqDescending(), eq))
		{
			Debug.LogError("SlinqDescending failed.");
			testCorrectness = false;
		}
		if (!tuples1.SlinqDescending().SequenceEqual(RemovableLinkedList().SlinqDescending(), eq))
		{
			Debug.LogError("SlinqDescending LL failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().SequenceEqual(from x in RemovableLinkedList().SlinqNodes()
			select x.Value, eq))
		{
			Debug.LogError("SlinqNodes LL failed.");
			testCorrectness = false;
		}
		if (!tuples1.SlinqDescending().SequenceEqual(from x in RemovableLinkedList().SlinqNodesDescending()
			select x.Value, eq))
		{
			Debug.LogError("SlinqNodesDescending LL failed.");
			testCorrectness = false;
		}
		if (!tuples1.SlinqWithIndex().All((Smooth.Algebraics.Tuple<Smooth.Algebraics.Tuple<int, int>, int> x) => x.Item1.Item2 == x.Item2) || !(from x in tuples1.SlinqWithIndex()
			select x.Item1).SequenceEqual(tuples1.Slinq(), eq))
		{
			Debug.LogError("SlinqWithIndex failed.");
			testCorrectness = false;
		}
		if (!tuples1.SlinqWithIndexDescending().All((Smooth.Algebraics.Tuple<Smooth.Algebraics.Tuple<int, int>, int> x) => x.Item1.Item2 == x.Item2) || !(from x in tuples1.SlinqWithIndexDescending()
			select x.Item1).SequenceEqual(tuples1.SlinqDescending(), eq))
		{
			Debug.LogError("SlinqWithIndexDescending failed.");
			testCorrectness = false;
		}
		if (tuples1.Slinq().Select(to_1).Sum() != tuples1.Select(to_1f).Sum())
		{
			Debug.LogError("Sum failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Take(testInt).SequenceEqual(tuples1.Take(testInt).Slinq(), eq))
		{
			Debug.LogError("Take failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().TakeRight(testInt).SequenceEqual(tuples1.Slinq().Skip(tuples1.Count - testInt), eq))
		{
			Debug.LogError("TakeRight failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().TakeWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2).SequenceEqual(tuples1.TakeWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2).Slinq(), eq))
		{
			Debug.LogError("TakeWhile failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().TakeWhile((Smooth.Algebraics.Tuple<int, int> x, int c) => x.Item1 < c, testInt2).SequenceEqual(tuples1.TakeWhile((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt2).Slinq(), eq))
		{
			Debug.LogError("TakeWhile failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Union(tuples2.Slinq(), eq_1).SequenceEqual(tuples1.Union(tuples2, eq_1).Slinq(), eq))
		{
			Debug.LogError("Union failed.");
			testCorrectness = false;
		}
		if (!(from x in tuples1.Slinq()
			where x.Item1 < testInt
			select x).SequenceEqual(tuples1.Where((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt).Slinq(), eq))
		{
			Debug.LogError("Where failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Where((Smooth.Algebraics.Tuple<int, int> x, int c) => x.Item1 < c, testInt).SequenceEqual(tuples1.Where((Smooth.Algebraics.Tuple<int, int> x) => x.Item1 < testInt).Slinq(), eq))
		{
			Debug.LogError("Where failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Zip(tuples2.Slinq()).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count && x < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create(tuples1[x], tuples2[x])))
		{
			Debug.LogError("Zip tuples failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Zip(tuples2.Slinq(), (Smooth.Algebraics.Tuple<int, int> a, Smooth.Algebraics.Tuple<int, int> b) => Smooth.Algebraics.Tuple.Create(a.Item1, b.Item1)).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count && x < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create(tuples1[x].Item1, tuples2[x].Item1), eq))
		{
			Debug.LogError("Zip failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().ZipAll(tuples2.Slinq()).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count || x < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create((x < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll tuples failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Skip(midSkip).ZipAll(tuples2.Slinq())
			.SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x + midSkip < tuples1.Count || x < tuples2.Count)
				select Smooth.Algebraics.Tuple.Create((x + midSkip < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x + midSkip]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll tuples failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().ZipAll(tuples2.Slinq().Skip(midSkip)).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count || x + midSkip < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create((x < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x + midSkip < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x + midSkip]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll tuples failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().ZipAll(tuples2.Slinq(), (Option<Smooth.Algebraics.Tuple<int, int>> a, Option<Smooth.Algebraics.Tuple<int, int>> b) => Smooth.Algebraics.Tuple.Create(a, b)).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count || x < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create((x < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().Skip(midSkip).ZipAll(tuples2.Slinq(), (Option<Smooth.Algebraics.Tuple<int, int>> a, Option<Smooth.Algebraics.Tuple<int, int>> b) => Smooth.Algebraics.Tuple.Create(a, b))
			.SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x + midSkip < tuples1.Count || x < tuples2.Count)
				select Smooth.Algebraics.Tuple.Create((x + midSkip < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x + midSkip]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().ZipAll(tuples2.Slinq().Skip(midSkip), (Option<Smooth.Algebraics.Tuple<int, int>> a, Option<Smooth.Algebraics.Tuple<int, int>> b) => Smooth.Algebraics.Tuple.Create(a, b)).SequenceEqual(from x in Slinqable.Sequence(0, 1).TakeWhile((int x) => x < tuples1.Count || x + midSkip < tuples2.Count)
			select Smooth.Algebraics.Tuple.Create((x < tuples1.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples1[x]) : default(Option<Smooth.Algebraics.Tuple<int, int>>), (x + midSkip < tuples2.Count) ? new Option<Smooth.Algebraics.Tuple<int, int>>(tuples2[x + midSkip]) : default(Option<Smooth.Algebraics.Tuple<int, int>>))))
		{
			Debug.LogError("ZipAll failed.");
			testCorrectness = false;
		}
		if (!tuples1.Slinq().ZipWithIndex().All((Smooth.Algebraics.Tuple<Smooth.Algebraics.Tuple<int, int>, int> x) => x.Item1.Item2 == x.Item2) || !(from x in tuples1.Slinq().ZipWithIndex()
			select x.Item1).SequenceEqual(tuples1.Slinq(), eq))
		{
			Debug.LogError("ZipWithIndex failed.");
			testCorrectness = false;
		}
	}

	public List<Smooth.Algebraics.Tuple<int, int>> RemovableList()
	{
		return new List<Smooth.Algebraics.Tuple<int, int>>(tuples1);
	}

	public LinkedList<Smooth.Algebraics.Tuple<int, int>> RemovableLinkedList()
	{
		return new LinkedList<Smooth.Algebraics.Tuple<int, int>>(tuples1);
	}
}
