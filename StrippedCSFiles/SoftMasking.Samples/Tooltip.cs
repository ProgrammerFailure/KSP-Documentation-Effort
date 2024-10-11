using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public RectTransform tooltip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Tooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
