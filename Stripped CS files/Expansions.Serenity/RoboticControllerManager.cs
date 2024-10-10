using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity;

public class RoboticControllerManager : MonoBehaviour
{
	public class AxisUpdate
	{
		public int priority;

		public float newValue;

		public int values;

		public void Reset()
		{
			priority = 0;
			newValue = 0f;
			values = 0;
		}
	}

	public static RoboticControllerManager Instance;

	public DictionaryValueList<uint, RoboticControllerWindow> windows;

	public static FloatCurve copyCacheAxis;

	public static List<float> copyCacheAction;

	public static RoboticControllerWindowBaseRow.rowTypes copyCacheType;

	public DictionaryValueList<BaseAxisField, AxisUpdate> updateQueue;

	public float updateValueAverage;

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(this);
			return;
		}
		if ((bool)Instance)
		{
			Debug.LogError("[RoboticControllerManager]: Only one instance of this class may exist per scene. Destroying potential usurper.", base.gameObject);
			Object.Destroy(this);
			return;
		}
		Instance = this;
		windows = new DictionaryValueList<uint, RoboticControllerWindow>();
		updateQueue = new DictionaryValueList<BaseAxisField, AxisUpdate>();
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoad);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoad);
	}

	public void Update()
	{
		UpdateFieldValues();
	}

	public void OnSceneLoad(GameScenes scene)
	{
		CloseAllWindows();
		ClearUpdateQueue();
	}

	public static bool AnyWindowTextFieldHasFocus()
	{
		if (Instance == null)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < Instance.windows.Count)
			{
				if (Instance.windows.ValuesList[num].AnyTextFieldHasFocus())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void CloseAllWindows()
	{
		int count = windows.Count;
		while (count-- > 0)
		{
			Object.Destroy(windows[windows.KeysList[count]].gameObject);
		}
		windows.Clear();
	}

	public void QueueFieldUpdate(BaseAxisField axisField, float newValue, int priority)
	{
		if (!updateQueue.TryGetValue(axisField, out var val))
		{
			updateQueue.Add(axisField, new AxisUpdate());
			val = updateQueue[axisField];
		}
		if (priority >= val.priority)
		{
			if (priority > val.priority)
			{
				val.priority = priority;
				val.newValue = newValue;
				val.values = 1;
			}
			else
			{
				val.newValue += newValue;
				val.values++;
			}
		}
	}

	public void ClearUpdateQueue()
	{
		updateQueue.Clear();
	}

	public void UpdateFieldValues()
	{
		for (int i = 0; i < updateQueue.KeysList.Count; i++)
		{
			BaseAxisField baseAxisField = updateQueue.KeysList[i];
			if (updateQueue[baseAxisField].values > 0)
			{
				updateValueAverage = updateQueue[baseAxisField].newValue / (float)updateQueue[baseAxisField].values;
				baseAxisField.SetValue(updateValueAverage, baseAxisField.module);
			}
			updateQueue[baseAxisField].Reset();
		}
	}
}
