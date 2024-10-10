using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

[RequireComponent(typeof(EventTrigger))]
public class EventTriggerForwarder : MonoBehaviour
{
	public bool forwardDrop;

	public bool forwardPointerEnter;

	public bool forwardPointerExit;

	public bool forwardScroll;

	public void Awake()
	{
		EventTrigger component = GetComponent<EventTrigger>();
		if (forwardDrop)
		{
			component.AddEvent(EventTriggerType.Drop, Drop);
		}
		if (forwardScroll)
		{
			component.AddEvent(EventTriggerType.Scroll, Scroll);
		}
		if (forwardPointerEnter)
		{
			component.AddEvent(EventTriggerType.PointerEnter, PointerEnter);
		}
		if (forwardPointerExit)
		{
			component.AddEvent(EventTriggerType.PointerExit, PointerExit);
		}
	}

	public void Drop(BaseEventData data)
	{
		ExecuteEvents.ExecuteHierarchy(base.transform.parent.gameObject, data, delegate(IDropHandler x, BaseEventData y)
		{
			x.OnDrop(null);
		});
	}

	public void Scroll(BaseEventData data)
	{
		ExecuteEvents.ExecuteHierarchy(base.transform.parent.gameObject, data, delegate(IScrollHandler x, BaseEventData y)
		{
			x.OnScroll((PointerEventData)data);
		});
	}

	public void PointerEnter(BaseEventData data)
	{
		ExecuteEvents.ExecuteHierarchy(base.transform.parent.gameObject, data, delegate(IPointerEnterHandler x, BaseEventData y)
		{
			x.OnPointerEnter((PointerEventData)data);
		});
	}

	public void PointerExit(BaseEventData data)
	{
		ExecuteEvents.ExecuteHierarchy(base.transform.parent.gameObject, data, delegate(IPointerExitHandler x, BaseEventData y)
		{
			x.OnPointerExit((PointerEventData)data);
		});
	}
}
