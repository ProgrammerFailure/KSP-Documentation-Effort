using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Smooth.Compare.Utilities;

public class LogUnregisteredOnDestroy : MonoBehaviour
{
	public bool destroyOnLoad;

	protected HashSet<Type> comparers;

	protected HashSet<Type> equalityComparers;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LogUnregisteredOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void HandleFinderEvent(ComparerType comparerType, EventType eventType, Type type)
	{
		throw null;
	}
}
