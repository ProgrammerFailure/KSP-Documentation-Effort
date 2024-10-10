using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class RUIutils
{
	public class FuncComparer<T> : IComparer<T>
	{
		public readonly Comparison<T> comparison;

		public FuncComparer(Comparison<T> comparison)
		{
			this.comparison = comparison;
		}

		public int Compare(T x, T y)
		{
			return comparison(x, y);
		}
	}

	public static string CutString(string s, int l, string add)
	{
		if (s.Length > l)
		{
			return s.Substring(0, l - add.Length) + add;
		}
		return s;
	}

	public static string DecToHex(int n, bool doubleDigit = true)
	{
		if (doubleDigit)
		{
			return n.ToString("X2");
		}
		return n.ToString("X");
	}

	public static char HexToChar(string hex, bool doubleDigit = true)
	{
		return (char)int.Parse(hex, NumberStyles.AllowHexSpecifier);
	}

	public static string ColorToHex(Color32 c, bool alpha = false)
	{
		return DecToHex(c.r) + DecToHex(c.g) + DecToHex(c.b) + (alpha ? DecToHex(c.a) : "");
	}

	public static string Repeat(string repeat, int count)
	{
		string text = "";
		for (int i = 0; i < count; i++)
		{
			text += repeat;
		}
		return text;
	}

	public static int SortAscDescPrimarySecondary(bool asc, int comp1, int comp2)
	{
		if (asc)
		{
			if (comp1 != 0)
			{
				return comp1;
			}
			return comp2;
		}
		if (comp1 != 0)
		{
			return -comp1;
		}
		return comp2;
	}

	public static int SortAscDescPrimarySecondary(bool asc, params int[] comp)
	{
		int num = comp.Length;
		int num2 = 0;
		while (true)
		{
			if (num2 < num)
			{
				if (comp[num2] != 0)
				{
					break;
				}
				num2++;
				continue;
			}
			return 0;
		}
		if (!asc)
		{
			return -comp[num2];
		}
		return comp[num2];
	}

	public static Vector3 CamToCamReposition(Vector3 originalPos, Camera fromCam, Camera toCam)
	{
		return toCam.ViewportToWorldPoint(fromCam.WorldToViewportPoint(originalPos));
	}

	public static string StringListToString(List<string> list)
	{
		string text = "";
		foreach (string item in list)
		{
			text += item;
		}
		return text;
	}

	public static string GetYesNoUIString(bool yes)
	{
		if (yes)
		{
			return "<color=" + XKCDColors.HexFormat.Green + ">V</color>";
		}
		return "<color=" + XKCDColors.HexFormat.Red + ">X</color>";
	}

	public static Func<T, bool> And<T>(params Func<T, bool>[] predicates)
	{
		return delegate(T item)
		{
			int num = predicates.Length;
			int num2 = 0;
			while (true)
			{
				if (num2 >= num)
				{
					return true;
				}
				if (!predicates[num2](item))
				{
					break;
				}
				num2++;
			}
			return false;
		};
	}

	public static bool All<T>(List<T> list, params Func<T, bool>[] predicates)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			T arg = list[i];
			int num = predicates.Length;
			for (int j = 0; j < num; j++)
			{
				if (!predicates[j](arg))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool All<T>(List<T> list, Func<T, bool> predicate)
	{
		int count = list.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				T arg = list[num];
				if (!predicate(arg))
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public static bool All<T>(T[] list, Func<T, bool> predicate)
	{
		int num = list.Length;
		int num2 = 0;
		while (true)
		{
			if (num2 < num)
			{
				T arg = list[num2];
				if (!predicate(arg))
				{
					break;
				}
				num2++;
				continue;
			}
			return true;
		}
		return false;
	}

	public static bool Any<T>(List<T> list, Func<T, bool> predicate)
	{
		int count = list.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				T arg = list[num];
				if (predicate(arg))
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

	public static bool Any<T>(T[] list, Func<T, bool> predicate)
	{
		int num = list.Length;
		int num2 = 0;
		while (true)
		{
			if (num2 < num)
			{
				T arg = list[num2];
				if (predicate(arg))
				{
					break;
				}
				num2++;
				continue;
			}
			return false;
		}
		return true;
	}

	public static int Count<T>(T[] list, Func<T, bool> predicate)
	{
		int num = 0;
		int num2 = list.Length;
		for (int i = 0; i < num2; i++)
		{
			T arg = list[i];
			if (predicate(arg))
			{
				num++;
			}
		}
		return num;
	}

	public static List<T> Where<T>(List<T> list, Func<T, bool> predicate)
	{
		List<T> list2 = new List<T>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			T val = list[i];
			if (predicate(val))
			{
				list2.Add(val);
			}
		}
		return list2;
	}

	public static List<T> WhereMutating<T>(ref List<T> list, Func<T, bool> predicate)
	{
		int count = list.Count;
		while (count-- > 0)
		{
			if (!predicate(list[count]))
			{
				list.RemoveAt(count);
			}
		}
		return list;
	}

	public static int Sum<T>(List<T> list, Func<T, int> predicate)
	{
		int num = 0;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			T arg = list[i];
			num += predicate(arg);
		}
		return num;
	}

	public static double Sum<T>(List<T> list, Func<T, double> predicate)
	{
		double num = 0.0;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			T arg = list[i];
			num += predicate(arg);
		}
		return num;
	}
}
