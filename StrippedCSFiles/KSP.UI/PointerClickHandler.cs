using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class PointerClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	[Serializable]
	public class PointerClickEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public PointerClickEvent()
		{
			throw null;
		}
	}

	public object Data;

	public PointerClickEvent<PointerEventData> onPointerClick;

	public PointerClickEvent<PointerEventData> onPointerDown;

	public PointerClickEvent<PointerEventData> onPointerUp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PointerClickHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}
}
