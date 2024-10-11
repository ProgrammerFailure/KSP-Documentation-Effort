using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

public class RoboticControllerManager : MonoBehaviour
{
	private class AxisUpdate
	{
		public int priority;

		public float newValue;

		public int values;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AxisUpdate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Reset()
		{
			throw null;
		}
	}

	public static RoboticControllerManager Instance;

	internal DictionaryValueList<uint, RoboticControllerWindow> windows;

	internal static FloatCurve copyCacheAxis;

	internal static List<float> copyCacheAction;

	internal static RoboticControllerWindowBaseRow.rowTypes copyCacheType;

	private DictionaryValueList<BaseAxisField, AxisUpdate> updateQueue;

	private float updateValueAverage;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RoboticControllerManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoad(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool AnyWindowTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseAllWindows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void QueueFieldUpdate(BaseAxisField axisField, float newValue, int priority)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearUpdateQueue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFieldValues()
	{
		throw null;
	}
}
