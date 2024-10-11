using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class DragHandler : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Serializable]
	public class DragEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DragEvent()
		{
			throw null;
		}
	}

	public DragEvent<PointerEventData> onInitializePotentialDrag;

	public DragEvent<PointerEventData> onBeginDrag;

	public DragEvent<PointerEventData> onDrag;

	public DragEvent<PointerEventData> onEndDrag;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEvents(UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEvents(UnityAction<PointerEventData> onInitializePotentialDrag, UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveEvents(UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveEvents(UnityAction<PointerEventData> onInitializePotentialDrag, UnityAction<PointerEventData> onBeginDrag, UnityAction<PointerEventData> onDrag, UnityAction<PointerEventData> onEndDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearEvents()
	{
		throw null;
	}
}
