using Smooth.Algebraics;

namespace Smooth.Delegates;

public static class Tupled
{
	public static void Call<T1>(this DelegateAction<T1> action, Tuple<T1> t)
	{
		action(t.Item1);
	}

	public static void Call<T1, T2>(this DelegateAction<T1, T2> action, Tuple<T1, T2> t)
	{
		action(t.Item1, t.Item2);
	}

	public static void Call<T1, T2, T3>(this DelegateAction<T1, T2, T3> action, Tuple<T1, T2, T3> t)
	{
		action(t.Item1, t.Item2, t.Item3);
	}

	public static void Call<T1, T2, T3, T4>(this DelegateAction<T1, T2, T3, T4> action, Tuple<T1, T2, T3, T4> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4);
	}

	public static void Call<T1, T2, T3, T4, T5>(this DelegateAction<T1, T2, T3, T4, T5> action, Tuple<T1, T2, T3, T4, T5> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
	}

	public static void Call<T1, T2, T3, T4, T5, T6>(this DelegateAction<T1, T2, T3, T4, T5, T6> action, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
	}

	public static void Call<T1, T2, T3, T4, T5, T6, T7>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7> action, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
	}

	public static void Call<T1, T2, T3, T4, T5, T6, T7, T8>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8> action, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);
	}

	public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t)
	{
		action(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8, t.Item9);
	}

	public static void Call<T, T1>(this DelegateAction<T, T1> action, T first, Tuple<T1> t)
	{
		action(first, t.Item1);
	}

	public static void Call<T, T1, T2>(this DelegateAction<T, T1, T2> action, T first, Tuple<T1, T2> t)
	{
		action(first, t.Item1, t.Item2);
	}

	public static void Call<T, T1, T2, T3>(this DelegateAction<T, T1, T2, T3> action, T first, Tuple<T1, T2, T3> t)
	{
		action(first, t.Item1, t.Item2, t.Item3);
	}

	public static void Call<T, T1, T2, T3, T4>(this DelegateAction<T, T1, T2, T3, T4> action, T first, Tuple<T1, T2, T3, T4> t)
	{
		action(first, t.Item1, t.Item2, t.Item3, t.Item4);
	}

	public static void Call<T, T1, T2, T3, T4, T5>(this DelegateAction<T, T1, T2, T3, T4, T5> action, T first, Tuple<T1, T2, T3, T4, T5> t)
	{
		action(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
	}

	public static void Call<T, T1, T2, T3, T4, T5, T6>(this DelegateAction<T, T1, T2, T3, T4, T5, T6> action, T first, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		action(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
	}

	public static void Call<T, T1, T2, T3, T4, T5, T6, T7>(this DelegateAction<T, T1, T2, T3, T4, T5, T6, T7> action, T first, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		action(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
	}

	public static void Call<T, T1, T2, T3, T4, T5, T6, T7, T8>(this DelegateAction<T, T1, T2, T3, T4, T5, T6, T7, T8> action, T first, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		action(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);
	}

	public static T Call<T1, T>(this DelegateFunc<T1, T> func, Tuple<T1> t)
	{
		return func(t.Item1);
	}

	public static T Call<T1, T2, T>(this DelegateFunc<T1, T2, T> func, Tuple<T1, T2> t)
	{
		return func(t.Item1, t.Item2);
	}

	public static T Call<T1, T2, T3, T>(this DelegateFunc<T1, T2, T3, T> func, Tuple<T1, T2, T3> t)
	{
		return func(t.Item1, t.Item2, t.Item3);
	}

	public static T Call<T1, T2, T3, T4, T>(this DelegateFunc<T1, T2, T3, T4, T> func, Tuple<T1, T2, T3, T4> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4);
	}

	public static T Call<T1, T2, T3, T4, T5, T>(this DelegateFunc<T1, T2, T3, T4, T5, T> func, Tuple<T1, T2, T3, T4, T5> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
	}

	public static T Call<T1, T2, T3, T4, T5, T6, T>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T> func, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
	}

	public static T Call<T1, T2, T3, T4, T5, T6, T7, T>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, T> func, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
	}

	public static T Call<T1, T2, T3, T4, T5, T6, T7, T8, T>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, T8, T> func, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);
	}

	public static T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> func, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t)
	{
		return func(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8, t.Item9);
	}

	public static U Call<T, T1, U>(this DelegateFunc<T, T1, U> func, T first, Tuple<T1> t)
	{
		return func(first, t.Item1);
	}

	public static U Call<T, T1, T2, U>(this DelegateFunc<T, T1, T2, U> func, T first, Tuple<T1, T2> t)
	{
		return func(first, t.Item1, t.Item2);
	}

	public static U Call<T, T1, T2, T3, U>(this DelegateFunc<T, T1, T2, T3, U> func, T first, Tuple<T1, T2, T3> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3);
	}

	public static U Call<T, T1, T2, T3, T4, U>(this DelegateFunc<T, T1, T2, T3, T4, U> func, T first, Tuple<T1, T2, T3, T4> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3, t.Item4);
	}

	public static U Call<T, T1, T2, T3, T4, T5, U>(this DelegateFunc<T, T1, T2, T3, T4, T5, U> func, T first, Tuple<T1, T2, T3, T4, T5> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
	}

	public static U Call<T, T1, T2, T3, T4, T5, T6, U>(this DelegateFunc<T, T1, T2, T3, T4, T5, T6, U> func, T first, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
	}

	public static U Call<T, T1, T2, T3, T4, T5, T6, T7, U>(this DelegateFunc<T, T1, T2, T3, T4, T5, T6, T7, U> func, T first, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
	}

	public static U Call<T, T1, T2, T3, T4, T5, T6, T7, T8, U>(this DelegateFunc<T, T1, T2, T3, T4, T5, T6, T7, T8, U> func, T first, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		return func(first, t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);
	}
}
