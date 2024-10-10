using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectDragOverride : ScrollRect
{
	public DragPanel targetDragPanel;

	public override void Awake()
	{
		base.Awake();
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (targetDragPanel != null)
		{
			targetDragPanel.OnDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
	}
}
