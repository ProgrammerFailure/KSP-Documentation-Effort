using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Delegates;

public static class Tupled
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1>(this DelegateAction<T1> action, Tuple<T1> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2>(this DelegateAction<T1, T2> action, Tuple<T1, T2> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3>(this DelegateAction<T1, T2, T3> action, Tuple<T1, T2, T3> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4>(this DelegateAction<T1, T2, T3, T4> action, Tuple<T1, T2, T3, T4> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4, T5>(this DelegateAction<T1, T2, T3, T4, T5> action, Tuple<T1, T2, T3, T4, T5> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4, T5, T6>(this DelegateAction<T1, T2, T3, T4, T5, T6> action, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4, T5, T6, T7>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7> action, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4, T5, T6, T7, T8>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8> action, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1>(this DelegateAction<F, T1> action, F first, Tuple<T1> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2>(this DelegateAction<F, T1, T2> action, F first, Tuple<T1, T2> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3>(this DelegateAction<F, T1, T2, T3> action, F first, Tuple<T1, T2, T3> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3, T4>(this DelegateAction<F, T1, T2, T3, T4> action, F first, Tuple<T1, T2, T3, T4> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3, T4, T5>(this DelegateAction<F, T1, T2, T3, T4, T5> action, F first, Tuple<T1, T2, T3, T4, T5> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3, T4, T5, T6>(this DelegateAction<F, T1, T2, T3, T4, T5, T6> action, F first, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3, T4, T5, T6, T7>(this DelegateAction<F, T1, T2, T3, T4, T5, T6, T7> action, F first, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Call<F, T1, T2, T3, T4, T5, T6, T7, T8>(this DelegateAction<F, T1, T2, T3, T4, T5, T6, T7, T8> action, F first, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, R>(this DelegateFunc<T1, R> func, Tuple<T1> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, R>(this DelegateFunc<T1, T2, R> func, Tuple<T1, T2> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, R>(this DelegateFunc<T1, T2, T3, R> func, Tuple<T1, T2, T3> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, R>(this DelegateFunc<T1, T2, T3, T4, R> func, Tuple<T1, T2, T3, T4> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, T5, R>(this DelegateFunc<T1, T2, T3, T4, T5, R> func, Tuple<T1, T2, T3, T4, T5> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, T5, T6, R>(this DelegateFunc<T1, T2, T3, T4, T5, T6, R> func, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, T5, T6, T7, R>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, R> func, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, T5, T6, T7, T8, R>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> func, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(this DelegateFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, R>(this DelegateFunc<F, T1, R> func, F first, Tuple<T1> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, R>(this DelegateFunc<F, T1, T2, R> func, F first, Tuple<T1, T2> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, R>(this DelegateFunc<F, T1, T2, T3, R> func, F first, Tuple<T1, T2, T3> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, T4, R>(this DelegateFunc<F, T1, T2, T3, T4, R> func, F first, Tuple<T1, T2, T3, T4> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, T4, T5, R>(this DelegateFunc<F, T1, T2, T3, T4, T5, R> func, F first, Tuple<T1, T2, T3, T4, T5> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, T4, T5, T6, R>(this DelegateFunc<F, T1, T2, T3, T4, T5, T6, R> func, F first, Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, T4, T5, T6, T7, R>(this DelegateFunc<F, T1, T2, T3, T4, T5, T6, T7, R> func, F first, Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static R Call<F, T1, T2, T3, T4, T5, T6, T7, T8, R>(this DelegateFunc<F, T1, T2, T3, T4, T5, T6, T7, T8, R> func, F first, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		throw null;
	}
}
