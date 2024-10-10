using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Compare.Comparers;
using Smooth.Comparisons;
using Smooth.Delegates;
using Smooth.Events;
using UnityEngine;

namespace Smooth.Compare;

public static class Finder
{
	public const string customConfigurationClassName = "Smooth.Compare.CustomConfiguration";

	public static GenericEvent<ComparerType, EventType, Type> OnEvent;

	public static readonly IEqualityComparer<Type> typeComparer;

	public static readonly Dictionary<Type, object> comparers;

	public static readonly Dictionary<Type, object> equalityComparers;

	public static readonly Configuration config;

	static Finder()
	{
		typeComparer = new FuncEqualityComparer<Type>((Type a, Type b) => a == b);
		comparers = new Dictionary<Type, object>(typeComparer);
		equalityComparers = new Dictionary<Type, object>(typeComparer) { 
		{
			typeof(Type),
			typeComparer
		} };
		try
		{
			Type type = Type.GetType("Smooth.Compare.CustomConfiguration", throwOnError: false);
			if (type != null)
			{
				if (typeof(Configuration).IsAssignableFrom(type))
				{
					if (type.GetConstructor(Type.EmptyTypes) != null)
					{
						config = (Configuration)type.GetConstructor(Type.EmptyTypes).Invoke(null);
					}
					else
					{
						Debug.LogError("A Smooth.Compare.CustomConfiguration class exists in your project, but will not be used because it does have a default constructor.");
					}
				}
				else
				{
					Debug.LogError("A Smooth.Compare.CustomConfiguration class exists in your project, but will not be used because it does not inherit from " + typeof(Configuration).FullName + ".");
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		finally
		{
			config = config ?? new Configuration();
		}
		try
		{
			config.RegisterComparers();
		}
		catch (Exception message2)
		{
			Debug.LogError(message2);
		}
	}

	public static void RegisterKeyValuePair<T, U>()
	{
		Register(new KeyValuePairComparer<T, U>());
		Register(new KeyValuePairEqualityComparer<T, U>());
	}

	public static void RegisterIComparable<T>() where T : IComparable<T>
	{
		Register(new IComparableComparer<T>());
	}

	public static void RegisterIEquatable<T>() where T : IEquatable<T>
	{
		Register(new IEquatableEqualityComparer<T>());
	}

	public static void RegisterIComparableIEquatable<T>() where T : IComparable<T>, IEquatable<T>
	{
		Register(new IComparableComparer<T>());
		Register(new IEquatableEqualityComparer<T>());
	}

	public static void Register<T>(Comparison<T> comparison, DelegateFunc<T, T, bool> equals)
	{
		Register(new FuncComparer<T>(comparison));
		Register(new FuncEqualityComparer<T>(equals));
	}

	public static void Register<T>(Comparison<T> comparison, DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		Register(new FuncComparer<T>(comparison));
		Register(new FuncEqualityComparer<T>(equals, hashCode));
	}

	public static void Register<T>(IComparer<T> comparer, IEqualityComparer<T> equalityComparer)
	{
		Register(comparer);
		Register(equalityComparer);
	}

	public static void Register<T>(Comparison<T> comparison)
	{
		Register(new FuncComparer<T>(comparison));
	}

	public static void Register<T>(IComparer<T> comparer)
	{
		ComparerType t = ComparerType.Comparer;
		Type typeFromHandle = typeof(T);
		if (comparer == null)
		{
			Debug.LogError("Tried to register a null comparer for: " + typeFromHandle.FullName);
			return;
		}
		lock (comparers)
		{
			if (comparers.ContainsKey(typeFromHandle))
			{
				OnEvent.Raise(t, EventType.AlreadyRegistered, typeFromHandle);
				return;
			}
			comparers.Add(typeFromHandle, comparer);
			OnEvent.Raise(t, EventType.Registered, typeFromHandle);
		}
	}

	public static IComparer<T> Comparer<T>()
	{
		ComparerType t = ComparerType.Comparer;
		Type typeFromHandle = typeof(T);
		lock (comparers)
		{
			if (comparers.TryGetValue(typeFromHandle, out var value))
			{
				OnEvent.Raise(t, EventType.FindRegistered, typeFromHandle);
				return (IComparer<T>)value;
			}
		}
		OnEvent.Raise(t, EventType.FindUnregistered, typeFromHandle);
		if (typeof(IComparable<T>).IsAssignableFrom(typeFromHandle))
		{
			OnEvent.Raise(t, EventType.EfficientDefault, typeFromHandle);
			return System.Collections.Generic.Comparer<T>.Default;
		}
		if (config.UseJit)
		{
			Option<IComparer<T>> option = config.Comparer<T>();
			if (option.isSome)
			{
				OnEvent.Raise(t, EventType.CustomJit, typeFromHandle);
				return option.value;
			}
		}
		if (typeof(IComparable).IsAssignableFrom(typeFromHandle))
		{
			OnEvent.Raise(t, typeFromHandle.IsValueType ? EventType.InefficientDefault : EventType.EfficientDefault, typeFromHandle);
			return System.Collections.Generic.Comparer<T>.Default;
		}
		OnEvent.Raise(t, EventType.InvalidDefault, typeFromHandle);
		return System.Collections.Generic.Comparer<T>.Default;
	}

	public static void Register<T>(DelegateFunc<T, T, bool> equals)
	{
		Register(new FuncEqualityComparer<T>(equals));
	}

	public static void Register<T>(DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode)
	{
		Register(new FuncEqualityComparer<T>(equals, hashCode));
	}

	public static void Register<T>(IEqualityComparer<T> equalityComparer)
	{
		ComparerType t = ComparerType.EqualityComparer;
		Type typeFromHandle = typeof(T);
		if (equalityComparer == null)
		{
			Debug.LogError("Tried to register a null equality comparer for: " + typeFromHandle.FullName);
			return;
		}
		lock (equalityComparers)
		{
			if (equalityComparers.ContainsKey(typeFromHandle))
			{
				OnEvent.Raise(t, EventType.AlreadyRegistered, typeFromHandle);
				return;
			}
			equalityComparers.Add(typeFromHandle, equalityComparer);
			OnEvent.Raise(t, EventType.Registered, typeFromHandle);
		}
	}

	public static IEqualityComparer<T> EqualityComparer<T>()
	{
		ComparerType t = ComparerType.EqualityComparer;
		Type typeFromHandle = typeof(T);
		lock (equalityComparers)
		{
			if (equalityComparers.TryGetValue(typeFromHandle, out var value))
			{
				OnEvent.Raise(t, EventType.FindRegistered, typeFromHandle);
				return (IEqualityComparer<T>)value;
			}
		}
		OnEvent.Raise(t, EventType.FindUnregistered, typeFromHandle);
		if (typeof(IEquatable<T>).IsAssignableFrom(typeFromHandle))
		{
			OnEvent.Raise(t, EventType.EfficientDefault, typeFromHandle);
			return System.Collections.Generic.EqualityComparer<T>.Default;
		}
		if (config.UseJit)
		{
			Option<IEqualityComparer<T>> option = config.EqualityComparer<T>();
			if (option.isSome)
			{
				OnEvent.Raise(t, EventType.CustomJit, typeFromHandle);
				return option.value;
			}
		}
		OnEvent.Raise(t, typeFromHandle.IsValueType ? EventType.InefficientDefault : EventType.EfficientDefault, typeFromHandle);
		return System.Collections.Generic.EqualityComparer<T>.Default;
	}
}
