using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ns2;

public class DragHandler : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Serializable]
	public class DragEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
	}

	public DragEvent<PointerEventData> onInitializePotentialDrag = new DragEvent<PointerEventData>();

	public DragEvent<PointerEventData> onBeginDrag = new DragEvent<PointerEventData>();

	public DragEvent<PointerEventData> onDrag = new DragEvent<PointerEventData>();

	public DragEvent<PointerEventData> onEndDrag = new DragEvent<PointerEventData>();

	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		onInitializePotentialDrag.Invoke(eventData);
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		onBeginDrag.Invoke(eventData);
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		onDrag.Invoke(eventData);
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		onEndDrag.Invoke(eventData);
	}

	public void AddEvents(UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		this.onBeginDrag.AddListener(onBeginDrag);
		this.onDrag.AddListener(onDrag);
		this.onEndDrag.AddListener(onEndDrag);
	}

	public void AddEvents(UnityAction<PointerEventData> onInitializePotentialDrag, UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		this.onInitializePotentialDrag.AddListener(onInitializePotentialDrag);
		AddEvents(onBeginDrag, onDrag, onEndDrag);
	}

	public void RemoveEvents(UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		this.onBeginDrag.RemoveListener(onBeginDrag);
		this.onDrag.RemoveListener(onDrag);
		this.onEndDrag.RemoveListener(onEndDrag);
	}

	public void RemoveEvents(UnityAction<PointerEventData> onInitializePotentialDrag, UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		this.onInitializePotentialDrag.RemoveListener(onInitializePotentialDrag);
		RemoveEvents(onBeginDrag, onDrag, onEndDrag);
	}

	public void ClearEvents()
	{
		onInitializePotentialDrag.RemoveAllListeners();
		onBeginDrag.RemoveAllListeners();
		onDrag.RemoveAllListeners();
		onEndDrag.RemoveAllListeners();
	}
}
