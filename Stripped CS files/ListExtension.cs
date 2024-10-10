using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ListExtension
{
	public static void Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = UnityEngine.Random.Range(0, num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static T AddUnique<T>(this IList<T> list, T item)
	{
		if (!list.Contains(item))
		{
			list.Add(item);
		}
		return item;
	}

	public static void AddUniqueRange<T>(this IList<T> list, IEnumerable<T> items)
	{
		IEnumerator<T> enumerator = items.GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			if (!list.Contains(current))
			{
				list.Add(current);
			}
		}
	}

	public static void AddUniqueRange<T>(this IList<T> list, IList<T> items)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			T item = items[i];
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
	}

	public static T MaxAt<T, TKey>(this IList<T> list, Func<T, TKey> sortBy)
	{
		int count = list.Count;
		if (count > 0)
		{
			T val = list[0];
			TKey y = sortBy(val);
			Comparer<TKey> @default = Comparer<TKey>.Default;
			for (int num = count; num >= 0; num--)
			{
				T val2 = list[num];
				TKey val3 = sortBy(val2);
				if (@default.Compare(val3, y) > 0)
				{
					val = val2;
					y = val3;
				}
			}
			return val;
		}
		throw new ArgumentNullException("list", "collection cannot be empty");
	}

	public static T MinAt<T, TKey>(this IList<T> list, Func<T, TKey> sortBy)
	{
		int count = list.Count;
		if (count > 0)
		{
			T val = list[0];
			TKey y = sortBy(val);
			Comparer<TKey> @default = Comparer<TKey>.Default;
			for (int num = count; num >= 0; num--)
			{
				T val2 = list[num];
				TKey val3 = sortBy(val2);
				if (@default.Compare(val3, y) < 0)
				{
					val = val2;
					y = val3;
				}
			}
			return val;
		}
		throw new ArgumentNullException("list", "collection cannot be empty");
	}

	public static void SetRange<T>(this List<T> list, T[] range, int startIndex)
	{
		int count = list.Count;
		int num = range.Length;
		int num2 = 0;
		int num3 = startIndex;
		while (num2 < num && num3 < count)
		{
			list[num3] = range[num2];
			num2++;
			num3++;
		}
	}

	public static void SetRange<T>(this List<T> list, List<T> range, int startIndex)
	{
		int count = list.Count;
		int count2 = range.Count;
		int num = 0;
		int num2 = startIndex;
		while (num < count2 && num2 < count)
		{
			list[num2] = range[num];
			num++;
			num2++;
		}
	}

	public static void SetRange<T>(this T[] list, List<T> range, int startIndex)
	{
		int num = list.Length;
		int count = range.Count;
		int num2 = 0;
		int num3 = startIndex;
		while (num2 < count && num3 < num)
		{
			list[num3] = range[num2];
			num2++;
			num3++;
		}
	}

	public static void SetRange<T>(this T[] list, T[] range, int startIndex)
	{
		int num = list.Length;
		int num2 = range.Length;
		int num3 = 0;
		int num4 = startIndex;
		while (num3 < num2 && num4 < num)
		{
			list[num4] = range[num3];
			num3++;
			num4++;
		}
	}

	public static bool ContainsId(this List<Part> list, Part p)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].persistentId == p.persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public static bool ContainsId(this List<Part> list, uint id)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].persistentId == id)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public static bool ContainsId(this HashSet<Part> list, Part p)
	{
		HashSet<Part>.Enumerator enumerator = list.GetEnumerator();
		do
		{
			if (!enumerator.MoveNext())
			{
				enumerator.Dispose();
				return false;
			}
		}
		while (!(enumerator.Current != null) || enumerator.Current.persistentId != p.persistentId);
		enumerator.Dispose();
		return true;
	}

	public static List<T> OrderByAlphaNumeric<T>(this List<T> source, Func<T, string> selector)
	{
		Regex regex = new Regex("\\d+");
		int maxNumberLength = 0;
		for (int j = 0; j < source.Count; j++)
		{
			MatchCollection matchCollection = regex.Matches(selector(source[j]));
			for (int k = 0; k < matchCollection.Count; k++)
			{
				if (matchCollection[k].Value.Length > maxNumberLength)
				{
					maxNumberLength = matchCollection[k].Value.Length;
				}
			}
		}
		return source.OrderBy((T i) => Regex.Replace(selector(i), "\\d+", (Match m) => m.Value.PadLeft(maxNumberLength, '0'))).ToList();
	}

	public static T[] OrderByAlphanumeric<T>(this T[] source, Func<T, string> selector)
	{
		return source.ToList().OrderByAlphaNumeric(selector).ToArray();
	}
}
