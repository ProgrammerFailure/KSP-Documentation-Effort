using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public static class UIExtensions
{
	public delegate void OnEventDelegate(BaseEventData data);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddEvent(this EventTrigger trigger, EventTriggerType eventID, OnEventDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Lock(this Selectable b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Unlock(this Selectable b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CopyValues(this RectTransform copyTo, RectTransform copyFrom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear(this LayoutGroup layoutGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearChildrenImmediate(this Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearChildren(this Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetLocalPositionZ(this Transform transform, float z = 0f)
	{
		throw null;
	}
}
