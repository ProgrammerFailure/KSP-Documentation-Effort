using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Compare;

public class Configuration
{
	public virtual bool UseJit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool NoJit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Configuration()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RegisterComparers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void HandleFinderEvent(ComparerType comparerType, EventType eventType, Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Option<IComparer<T>> Comparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Option<IEqualityComparer<T>> EqualityComparer<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Color32ToInt(Color32 c)
	{
		throw null;
	}
}
