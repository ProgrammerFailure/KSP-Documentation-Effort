using System;
using System.Collections.Generic;

namespace UniLinq;

public class QuickSort<TElement>
{
	public TElement[] elements;

	public int[] indexes;

	public SortContext<TElement> context;

	public QuickSort(IEnumerable<TElement> source, SortContext<TElement> context)
	{
		List<TElement> list = new List<TElement>();
		foreach (TElement item in source)
		{
			list.Add(item);
		}
		elements = list.ToArray();
		indexes = CreateIndexes(elements.Length);
		this.context = context;
	}

	public static int[] CreateIndexes(int length)
	{
		int[] array = new int[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = i;
		}
		return array;
	}

	public void PerformSort()
	{
		if (elements.Length > 1)
		{
			context.Initialize(elements);
			Array.Sort(indexes, context);
		}
	}

	public static IEnumerable<TElement> Sort(IEnumerable<TElement> source, SortContext<TElement> context)
	{
		QuickSort<TElement> sorter = new QuickSort<TElement>(source, context);
		sorter.PerformSort();
		for (int i = 0; i < sorter.elements.Length; i++)
		{
			yield return sorter.elements[sorter.indexes[i]];
		}
	}
}
