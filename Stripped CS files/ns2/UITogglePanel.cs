using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class UITogglePanel : MonoBehaviour
{
	public List<EventTrigger> interactableA;

	public List<EventTrigger> interactableB;

	public GameObject panelA;

	public GameObject panelB;

	public void Awake()
	{
		int i = 0;
		for (int count = interactableA.Count; i < count; i++)
		{
			interactableA[i].AddEvent(EventTriggerType.PointerEnter, Hover);
		}
		int j = 0;
		for (int count2 = interactableB.Count; j < count2; j++)
		{
			interactableB[j].AddEvent(EventTriggerType.PointerExit, HoverOut);
		}
		HoverOut(null);
	}

	public void Hover(BaseEventData data)
	{
		panelA.SetActive(value: false);
		panelB.SetActive(value: true);
	}

	public void HoverOut(BaseEventData data)
	{
		panelA.SetActive(value: true);
		panelB.SetActive(value: false);
	}
}
