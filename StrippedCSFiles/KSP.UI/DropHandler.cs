using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class DropHandler : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	[Serializable]
	public class DropEvent<PointerEventData> : UnityEvent<PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DropEvent()
		{
			throw null;
		}
	}

	public DropEvent<PointerEventData> onDrop;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DropHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrop(PointerEventData eventData)
	{
		throw null;
	}
}
