using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro;

public class TMP_ScrollbarEventHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
{
	public bool isSelected;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ScrollbarEventHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSelect(BaseEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDeselect(BaseEventData eventData)
	{
		throw null;
	}
}
