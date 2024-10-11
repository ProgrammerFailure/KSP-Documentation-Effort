using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class PointerEnterExitHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[Serializable]
	public class PointerDataEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public PointerDataEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> : UnityEvent<PointerEventData, PointerEnterExitHandler>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public PointerDataObjectEvent()
		{
			throw null;
		}
	}

	public PointerDataEvent<PointerEventData> onPointerEnter;

	public PointerDataEvent<PointerEventData> onPointerExit;

	public PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> onPointerEnterObj;

	public PointerDataObjectEvent<PointerEventData, PointerEnterExitHandler> onPointerExitObj;

	public bool IsOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PointerEnterExitHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
