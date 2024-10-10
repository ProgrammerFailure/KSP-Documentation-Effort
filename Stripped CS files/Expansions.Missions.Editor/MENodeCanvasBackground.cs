using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MENodeCanvasBackground : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	[SerializeField]
	public MENodeCanvas nodeCanvas;

	public void OnPointerClick(PointerEventData eventData)
	{
		nodeCanvas.OnPointerClick(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		nodeCanvas.OnPointerDown(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		nodeCanvas.OnDrag(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		nodeCanvas.OnPointerUp(eventData);
	}
}
