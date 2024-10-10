using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public static class UIExtensions
{
	public delegate void OnEventDelegate(BaseEventData data);

	public static void AddEvent(this EventTrigger trigger, EventTriggerType eventID, OnEventDelegate method)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = eventID;
		entry.callback = new EventTrigger.TriggerEvent();
		UnityAction<BaseEventData> call = method.Invoke;
		entry.callback.AddListener(call);
		trigger.triggers.Add(entry);
	}

	public static void Lock(this Selectable b)
	{
		b.interactable = false;
	}

	public static void Unlock(this Selectable b)
	{
		b.interactable = true;
	}

	public static void CopyValues(this RectTransform copyTo, RectTransform copyFrom)
	{
		copyTo.anchoredPosition = Vector2.zero;
		copyTo.anchorMax = copyFrom.anchorMax;
		copyTo.anchorMin = copyFrom.anchorMin;
		copyTo.sizeDelta = copyFrom.sizeDelta;
	}

	public static void Clear(this LayoutGroup layoutGroup)
	{
		while (layoutGroup.transform.childCount > 0)
		{
			layoutGroup.transform.GetChild(0).gameObject.DestroyGameObject();
		}
	}

	public static void ClearChildrenImmediate(this Transform t)
	{
		while (t.childCount > 0)
		{
			t.GetChild(0).gameObject.DestroyGameObjectImmediate();
		}
	}

	public static void ClearChildren(this Transform t)
	{
		int childCount = t.childCount;
		while (childCount-- > 0)
		{
			t.GetChild(childCount).gameObject.DestroyGameObject();
		}
	}

	public static void SetLocalPositionZ(this Transform transform, float z = 0f)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}
}
