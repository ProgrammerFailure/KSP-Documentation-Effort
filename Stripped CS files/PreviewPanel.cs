using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewPanel : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public bool previewHovered;

	public void OnPointerEnter(PointerEventData eventData)
	{
		previewHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		previewHovered = false;
	}
}
