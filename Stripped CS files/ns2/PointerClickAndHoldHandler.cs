using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class PointerClickAndHoldHandler : PointerClickHandler, IPointerExitHandler, IEventSystemHandler
{
	public PointerClickEvent<PointerEventData> onPointerDownHold = new PointerClickEvent<PointerEventData>();

	public Coroutine pointerRoutine;

	public override void OnPointerDown(PointerEventData eventData)
	{
		onPointerDown.Invoke(eventData);
		pointerRoutine = StartCoroutine(OnPointerDownHold(eventData));
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (pointerRoutine != null)
		{
			onPointerUp.Invoke(eventData);
			StopCoroutine(pointerRoutine);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (pointerRoutine != null)
		{
			StopCoroutine(pointerRoutine);
		}
	}

	public IEnumerator OnPointerDownHold(PointerEventData eventData)
	{
		while (true)
		{
			onPointerDownHold.Invoke(eventData);
			yield return null;
		}
	}
}
