using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Comparisons;
using UnityEngine;

namespace Smooth.Collections;

public static class IListExtensions
{
	public static Option<T> Random<T>(this IList<T> list)
	{
		if (list.Count != 0)
		{
			return new Option<T>(list[UnityEngine.Random.Range(0, list.Count)]);
		}
		return Option<T>.None;
	}

	public static void Shuffle<T>(this IList<T> ts)
	{
		int count = ts.Count;
		int num = count - 1;
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(i, count);
			T value = ts[i];
			ts[i] = ts[index];
			ts[index] = value;
		}
	}

	public static void InsertionSort<T>(this IList<T> ts)
	{
		ts.InsertionSort(Comparisons<T>.Default);
	}

	public static void InsertionSort<T>(this IList<T> ts, IComparer<T> comparer)
	{
		ts.InsertionSort(Comparisons<T>.ToComparison(comparer));
	}

	public static void InsertionSort<T>(this IList<T> ts, Comparison<T> comparison)
	{
		for (int i = 1; i < ts.Count; i++)
		{
			T val = ts[i];
			int num = i - 1;
			while (num >= 0 && comparison(ts[num], val) > 0)
			{
				ts[num + 1] = ts[num];
				num--;
			}
			ts[num + 1] = val;
		}
	}
}
