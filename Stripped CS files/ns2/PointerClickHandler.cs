using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ns2;

public class PointerClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	[Serializable]
	public class PointerClickEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
	}

	public object Data;

	public PointerClickEvent<PointerEventData> onPointerClick = new PointerClickEvent<PointerEventData>();

	public PointerClickEvent<PointerEventData> onPointerDown = new PointerClickEvent<PointerEventData>();

	public PointerClickEvent<PointerEventData> onPointerUp = new PointerClickEvent<PointerEventData>();

	public void OnPointerClick(PointerEventData eventData)
	{
		if (onPointerClick != null)
		{
			onPointerClick.Invoke(eventData);
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (onPointerDown != null)
		{
			onPointerDown.Invoke(eventData);
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (onPointerUp != null)
		{
			onPointerUp.Invoke(eventData);
		}
	}
}
