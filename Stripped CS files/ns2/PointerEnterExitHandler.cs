using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ns2;

public class PointerEnterExitHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[Serializable]
	public class PointerDataEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
	}

	[Serializable]
	public class PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> : UnityEvent<PointerEventData, PointerEnterExitHandler>
	{
	}

	public PointerDataEvent<PointerEventData> onPointerEnter = new PointerDataEvent<PointerEventData>();

	public PointerDataEvent<PointerEventData> onPointerExit = new PointerDataEvent<PointerEventData>();

	public PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> onPointerEnterObj = new PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler>();

	public PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> onPointerExitObj = new PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler>();

	public bool IsOver { get; set; }

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		IsOver = true;
		onPointerEnter.Invoke(eventData);
		onPointerEnterObj.Invoke(eventData, this);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		IsOver = false;
		onPointerExit.Invoke(eventData);
		onPointerExitObj.Invoke(eventData, this);
	}
}
