using System;

public static class DelegateExtensions
{
	public static void Clear<T>(this Func<T> f, T defaultValue)
	{
		f = () => defaultValue;
	}

	public static void Clear<T, U>(this Func<T, U> f, U defaultValue)
	{
		f = (T t) => defaultValue;
	}

	public static void Clear<T, U, V>(this Func<T, U, V> f, V defaultValue)
	{
		f = (T t, U u) => defaultValue;
	}

	public static void Clear<T, U, V, W>(this Func<T, U, V, W> f, W defaultValue)
	{
		f = (T t, U u, V v) => defaultValue;
	}

	public static void Clear<T, U, V, W, X>(this Func<T, U, V, W, X> f, X defaultValue)
	{
		f = (T t, U u, V v, W w) => defaultValue;
	}
}
