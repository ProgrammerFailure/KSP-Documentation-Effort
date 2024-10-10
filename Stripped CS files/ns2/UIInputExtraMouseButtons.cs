using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(EventTrigger))]
public class UIInputExtraMouseButtons : MonoBehaviour
{
	public bool mouseEntered;

	public Button.ButtonClickedEvent leftClick;

	public Button.ButtonClickedEvent rightClick;

	public Button.ButtonClickedEvent middleClick;

	public void Awake()
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(OnMouseEnter);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(OnMouseExit);
		EventTrigger component = GetComponent<EventTrigger>();
		component.triggers.Add(entry);
		component.triggers.Add(entry2);
	}

	public void OnMouseEnter(BaseEventData data)
	{
		mouseEntered = true;
	}

	public void OnMouseExit(BaseEventData data)
	{
		mouseEntered = false;
	}

	public void Update()
	{
		if (mouseEntered)
		{
			if (Input.GetMouseButtonUp(0))
			{
				leftClick.Invoke();
			}
			if (Input.GetMouseButtonUp(1))
			{
				rightClick.Invoke();
			}
			if (Input.GetMouseButtonUp(2))
			{
				middleClick.Invoke();
			}
		}
	}
}
