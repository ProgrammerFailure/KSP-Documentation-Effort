using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Platform;
using UnityEngine;

namespace Smooth.Compare;

public class Configuration
{
	public virtual bool UseJit => Runtime.HasJit;

	public bool NoJit => !UseJit;

	public Configuration()
	{
		Finder.OnEvent.Handle += HandleFinderEvent;
	}

	public virtual void RegisterComparers()
	{
		Finder.Register((Color32 a, Color32 b) => Color32ToInt(a) == Color32ToInt(b), Color32ToInt);
		if (NoJit)
		{
			Finder.RegisterIComparableIEquatable<bool>();
			Finder.RegisterIComparableIEquatable<char>();
			Finder.RegisterIComparableIEquatable<byte>();
			Finder.RegisterIComparableIEquatable<sbyte>();
			Finder.RegisterIComparableIEquatable<short>();
			Finder.RegisterIComparableIEquatable<ushort>();
			Finder.RegisterIComparableIEquatable<int>();
			Finder.RegisterIComparableIEquatable<uint>();
			Finder.RegisterIComparableIEquatable<long>();
			Finder.RegisterIComparableIEquatable<ulong>();
			Finder.RegisterIComparableIEquatable<float>();
			Finder.RegisterIComparableIEquatable<double>();
			Finder.RegisterIComparableIEquatable<decimal>();
			Finder.Register((RuntimeTypeHandle a, RuntimeTypeHandle b) => a.Equals(b));
			Finder.Register((RuntimeFieldHandle a, RuntimeFieldHandle b) => a == b);
			Finder.Register((RuntimeMethodHandle a, RuntimeMethodHandle b) => a == b);
			Finder.Register((Color a, Color b) => a == b);
			Finder.Register((Vector2 a, Vector2 b) => a == b);
			Finder.Register((Vector3 a, Vector3 b) => a == b);
			Finder.Register((Vector4 a, Vector4 b) => a == b);
			Finder.Register((Quaternion a, Quaternion b) => a == b);
		}
	}

	public virtual void HandleFinderEvent(ComparerType comparerType, EventType eventType, Type type)
	{
		switch (eventType)
		{
		case EventType.AlreadyRegistered:
			Debug.LogWarning("Tried to register a " + comparerType.ToStringCached() + " over an existing registration.\nType: " + type.FullName);
			break;
		case EventType.FindUnregistered:
			if (NoJit && type.IsValueType)
			{
				Debug.LogWarning("A " + comparerType.ToStringCached() + " has been requested for a non-registered value type with JIT disabled, this is a fragile operation and may result in a JIT exception.\nType:" + type.FullName);
			}
			break;
		case EventType.FindRegistered:
		case EventType.EfficientDefault:
			break;
		case EventType.InefficientDefault:
			Debug.LogWarning("A " + comparerType.ToStringCached() + " has been requested that will perform inefficient comparisons and/or cause boxing allocations.\nType:" + type.FullName);
			break;
		case EventType.InvalidDefault:
			Debug.LogWarning("A " + comparerType.ToStringCached() + " has been requested for a non-comparable type.  Using the comparer will cause exceptions.\nType:" + type.FullName);
			break;
		}
	}

	public virtual Option<IComparer<T>> Comparer<T>()
	{
		return Factory.Comparer<T>();
	}

	public virtual Option<IEqualityComparer<T>> EqualityComparer<T>()
	{
		return Factory.EqualityComparer<T>();
	}

	public static int Color32ToInt(Color32 c)
	{
		return (c.r << 24) | (c.g << 16) | (c.b << 8) | c.a;
	}
}
