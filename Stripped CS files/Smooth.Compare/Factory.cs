using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Smooth.Algebraics;
using Smooth.Collections;
using Smooth.Comparisons;
using Smooth.Delegates;
using UnityEngine;

namespace Smooth.Compare;

public static class Factory
{
	public const int hashCodeSeed = 17;

	public const int hashCodeStepMultiplier = 29;

	public static Option<IComparer<T>> Comparer<T>()
	{
		Type typeFromHandle = typeof(T);
		if (!typeFromHandle.IsValueType)
		{
			return Option<IComparer<T>>.None;
		}
		if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
		{
			return new Option<IComparer<T>>(KeyValuePairComparer<T>(typeFromHandle));
		}
		return Option<IComparer<T>>.None;
	}

	public static Option<IEqualityComparer<T>> EqualityComparer<T>()
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle.IsValueType && !typeFromHandle.IsEnum)
		{
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
			{
				return new Option<IEqualityComparer<T>>(KeyValuePairEqualityComparer<T>(typeFromHandle));
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "l");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle, "r");
			Option<Expression> option = EqualsExpression(parameterExpression, parameterExpression2);
			if (!option.isSome)
			{
				return Option<IEqualityComparer<T>>.None;
			}
			return new Option<IEqualityComparer<T>>(new FuncEqualityComparer<T>(Expression.Lambda<DelegateFunc<T, T, bool>>(option.value, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile()));
		}
		return Option<IEqualityComparer<T>>.None;
	}

	public static Expression HashCodeSeed()
	{
		return Expression.Constant(17, typeof(int));
	}

	public static Expression HashCodeStepMultiplier()
	{
		return Expression.Constant(29, typeof(int));
	}

	public static Option<Expression> EqualsExpression(Expression l, Expression r)
	{
		try
		{
			BinaryExpression binaryExpression = Expression.Equal(l, r);
			MethodInfo method = binaryExpression.Method;
			if (method != null)
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters[0].ParameterType == l.Type && parameters[1].ParameterType == r.Type)
				{
					return new Option<Expression>(binaryExpression);
				}
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		try
		{
			MethodInfo method2 = l.Type.GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new Type[1] { r.Type }, null);
			if (method2 != null && method2.GetParameters()[0].ParameterType == r.Type)
			{
				return new Option<Expression>(Expression.Call(l, method2, r));
			}
		}
		catch (Exception message2)
		{
			Debug.LogError(message2);
		}
		return Option<Expression>.None;
	}

	public static Smooth.Algebraics.Tuple<Expression, MethodInfo> ExistingComparer<T>()
	{
		return ExistingComparer(typeof(T));
	}

	public static Smooth.Algebraics.Tuple<Expression, MethodInfo> ExistingComparer(Type type)
	{
		PropertyInfo property = typeof(Smooth.Collections.Comparer<>).MakeGenericType(type).GetProperty("Default", BindingFlags.Static | BindingFlags.Public, null, typeof(IComparer<>).MakeGenericType(type), Type.EmptyTypes, null);
		MemberExpression memberExpression = Expression.Property(null, property);
		return new Smooth.Algebraics.Tuple<Expression, MethodInfo>(memberExpression, memberExpression.Type.GetMethod("Compare", BindingFlags.Instance | BindingFlags.Public, null, new Type[2] { type, type }, null));
	}

	public static Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> ExistingEqualityComparer<T>()
	{
		return ExistingEqualityComparer(typeof(T));
	}

	public static Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> ExistingEqualityComparer(Type type)
	{
		PropertyInfo property = typeof(Smooth.Collections.EqualityComparer<>).MakeGenericType(type).GetProperty("Default", BindingFlags.Static | BindingFlags.Public, null, typeof(IEqualityComparer<>).MakeGenericType(type), Type.EmptyTypes, null);
		MemberExpression memberExpression = Expression.Property(null, property);
		return new Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo>(memberExpression, memberExpression.Type.GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new Type[2] { type, type }, null), memberExpression.Type.GetMethod("GetHashCode", BindingFlags.Instance | BindingFlags.Public, null, new Type[1] { type }, null));
	}

	public static IComparer<T> KeyValuePairComparer<T>(Type type)
	{
		ParameterExpression parameterExpression = Expression.Parameter(type, "l");
		ParameterExpression parameterExpression2 = Expression.Parameter(type, "r");
		MemberExpression memberExpression = Expression.Property(parameterExpression, "Key");
		MemberExpression arg = Expression.Property(parameterExpression2, "Key");
		MemberExpression memberExpression2 = Expression.Property(parameterExpression, "Value");
		MemberExpression arg2 = Expression.Property(parameterExpression2, "Value");
		Smooth.Algebraics.Tuple<Expression, MethodInfo> tuple = ExistingComparer(memberExpression.Type);
		Smooth.Algebraics.Tuple<Expression, MethodInfo> tuple2 = ExistingComparer(memberExpression2.Type);
		Comparison<T> keysCompared = Expression.Lambda<Comparison<T>>(Expression.Call(tuple.Item1, tuple.Item2, memberExpression, arg), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
		Comparison<T> valuesCompared = Expression.Lambda<Comparison<T>>(Expression.Call(tuple2.Item1, tuple2.Item2, memberExpression2, arg2), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
		return new FuncComparer<T>(delegate(T lhs, T rhs)
		{
			int num = keysCompared(lhs, rhs);
			return (num != 0) ? num : valuesCompared(lhs, rhs);
		});
	}

	public static IEqualityComparer<T> KeyValuePairEqualityComparer<T>(Type type)
	{
		ParameterExpression parameterExpression = Expression.Parameter(type, "l");
		ParameterExpression parameterExpression2 = Expression.Parameter(type, "r");
		MemberExpression memberExpression = Expression.Property(parameterExpression, "Key");
		MemberExpression arg = Expression.Property(parameterExpression2, "Key");
		MemberExpression memberExpression2 = Expression.Property(parameterExpression, "Value");
		MemberExpression arg2 = Expression.Property(parameterExpression2, "Value");
		Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> tuple = ExistingEqualityComparer(memberExpression.Type);
		Smooth.Algebraics.Tuple<Expression, MethodInfo, MethodInfo> tuple2 = ExistingEqualityComparer(memberExpression2.Type);
		MethodCallExpression left = Expression.Call(tuple.Item1, tuple.Item2, memberExpression, arg);
		MethodCallExpression right = Expression.Call(tuple2.Item1, tuple2.Item2, memberExpression2, arg2);
		BinaryExpression body = Expression.And(left, right);
		MethodCallExpression left2 = Expression.Call(tuple.Item1, tuple.Item3, memberExpression);
		MethodCallExpression left3 = Expression.Call(tuple2.Item1, tuple2.Item3, memberExpression2);
		Expression left4 = HashCodeSeed();
		Expression right2 = HashCodeStepMultiplier();
		left4 = Expression.Add(left2, Expression.Multiply(left4, right2));
		left4 = Expression.Add(left3, Expression.Multiply(left4, right2));
		return new FuncEqualityComparer<T>(Expression.Lambda<DelegateFunc<T, T, bool>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile(), Expression.Lambda<DelegateFunc<T, int>>(left4, new ParameterExpression[1] { parameterExpression }).Compile());
	}
}
