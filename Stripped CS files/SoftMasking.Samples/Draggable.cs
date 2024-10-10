using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples;

[RequireComponent(typeof(RectTransform))]
public class Draggable : UIBehaviour, IDragHandler, IEventSystemHandler
{
	public RectTransform _rectTransform;

	public override void Awake()
	{
		base.Awake();
		_rectTransform = GetComponent<RectTransform>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		_rectTransform.anchoredPosition += eventData.delta;
	}
}
