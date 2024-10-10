using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UIOnHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Button.ButtonClickedEvent onEnter = new Button.ButtonClickedEvent();

	public Button.ButtonClickedEvent onExit = new Button.ButtonClickedEvent();

	public void OnPointerEnter(PointerEventData eventData)
	{
		onEnter.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		onExit.Invoke();
	}
}
