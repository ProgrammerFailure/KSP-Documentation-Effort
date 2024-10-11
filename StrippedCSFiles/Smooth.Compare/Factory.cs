using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Compare;

public static class Factory
{
	private const int hashCodeSeed = 17;

	private const int hashCodeStepMultiplier = 29;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<IComparer<T>> Comparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<IEqualityComparer<T>> EqualityComparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Expression HashCodeSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Expression HashCodeStepMultiplier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<Expression> EqualsExpression(Expression l, Expression r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Smooth.Algebraics.Tuple<Expression, MethodInfo> ExistingComparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Smooth.Algebraics.Tuple<Expression, MethodInfo> ExistingComparer(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> ExistingEqualityComparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> ExistingEqualityComparer(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static IComparer<T> KeyValuePairComparer<T>(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static IEqualityComparer<T> KeyValuePairEqualityComparer<T>(Type type)
	{
		throw null;
	}
}
