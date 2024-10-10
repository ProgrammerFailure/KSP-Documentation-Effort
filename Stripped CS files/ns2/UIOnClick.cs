using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UIOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public EventData<PointerEventData> onClick = new EventData<PointerEventData>("OnClick");

	public Selectable selectable;

	public bool interactable = true;

	public bool Interactable
	{
		get
		{
			if (selectable != null)
			{
				return selectable.interactable;
			}
			return interactable;
		}
		set
		{
			if (selectable != null)
			{
				selectable.interactable = value;
			}
			interactable = value;
		}
	}

	public void Awake()
	{
		selectable = GetComponent<Selectable>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Interactable)
		{
			onClick.Fire(eventData);
		}
	}
}
