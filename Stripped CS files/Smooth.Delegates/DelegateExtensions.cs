namespace Smooth.Delegates;

public static class DelegateExtensions
{
	public static void Apply<T>(this T t, DelegateAction<T> a)
	{
		a(t);
	}

	public static void Apply<T, U>(this T t, DelegateAction<T, U> a, U p)
	{
		a(t, p);
	}

	public static U Apply<T, U>(this T t, DelegateFunc<T, U> f)
	{
		return f(t);
	}

	public static U Apply<T, U, V>(this T t, DelegateFunc<T, V, U> f, V p)
	{
		return f(t, p);
	}
}
