using System;
using System.Collections.Generic;
using Smooth.Collections;
using Smooth.Delegates;

namespace Smooth.Comparisons;

public static class Comparisons
{
	public static Comparison<T> Reverse<T>(Comparison<T> comparison)
	{
		return (T a, T b) => comparison(b, a);
	}

	public static Comparison<T> NullsFirst<T>(Comparison<T> comparison) where T : class
	{
		return delegate(T a, T b)
		{
			if (a != null)
			{
				if (b != null)
				{
					return comparison(a, b);
				}
				return 1;
			}
			return (b != null) ? (-1) : 0;
		};
	}

	public static Comparison<T> NullsLast<T>(Comparison<T> comparison) where T : class
	{
		return delegate(T a, T b)
		{
			if (a != null)
			{
				if (b != null)
				{
					return comparison(a, b);
				}
				return -1;
			}
			return (b != null) ? 1 : 0;
		};
	}

	public static Comparison<T?> NullableNullsFirst<T>(Comparison<T> comparison) where T : struct
	{
		return delegate(T? a, T? b)
		{
			if (a.HasValue)
			{
				if (b.HasValue)
				{
					return comparison(a.Value, b.Value);
				}
				return 1;
			}
			return b.HasValue ? (-1) : 0;
		};
	}

	public static Comparison<T?> NullableNullsLast<T>(Comparison<T> comparison) where T : struct
	{
		return delegate(T? a, T? b)
		{
			if (a.HasValue)
			{
				if (b.HasValue)
				{
					return comparison(a.Value, b.Value);
				}
				return -1;
			}
			return b.HasValue ? 1 : 0;
		};
	}
}
public static class Comparisons<T>
{
	public static Dictionary<IComparer<T>, Comparison<T>> toComparison = new Dictionary<IComparer<T>, Comparison<T>>();

	public static Dictionary<IEqualityComparer<T>, DelegateFunc<T, T, bool>> toPredicate = new Dictionary<IEqualityComparer<T>, DelegateFunc<T, T, bool>>();

	public static Comparison<T> Default => ToComparison(Smooth.Collections.Comparer<T>.Default);

	public static DelegateFunc<T, T, bool> DefaultPredicate => ToPredicate(Smooth.Collections.EqualityComparer<T>.Default);

	public static Comparison<T> ToComparison(IComparer<T> comparer)
	{
		Comparison<T> value;
		lock (toComparison)
		{
			if (!toComparison.TryGetValue(comparer, out value))
			{
				value = comparer.Compare;
				toComparison[comparer] = value;
			}
		}
		return value;
	}

	public static DelegateFunc<T, T, bool> ToPredicate(IEqualityComparer<T> equalityComparer)
	{
		DelegateFunc<T, T, bool> value;
		lock (toPredicate)
		{
			if (!toPredicate.TryGetValue(equalityComparer, out value))
			{
				value = equalityComparer.Equals;
				toPredicate[equalityComparer] = value;
			}
		}
		return value;
	}
}
