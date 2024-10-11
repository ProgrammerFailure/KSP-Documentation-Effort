using System.Runtime.CompilerServices;

namespace Smooth.Delegates;

public static class DelegateExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Apply<T>(this T t, DelegateAction<T> a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Apply<T, P>(this T t, DelegateAction<T, P> a, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static U Apply<T, U>(this T t, DelegateFunc<T, U> f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static U Apply<T, U, P>(this T t, DelegateFunc<T, P, U> f, P p)
	{
		throw null;
	}
}
