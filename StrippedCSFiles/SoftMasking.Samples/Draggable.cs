using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples;

[RequireComponent(typeof(RectTransform))]
public class Draggable : UIBehaviour, IDragHandler, IEventSystemHandler
{
	private RectTransform _rectTransform;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Draggable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}
}
