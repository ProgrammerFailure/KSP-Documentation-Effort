using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class UIDragPanel : DragHandler
{
	public enum DragDirection
	{
		HORIZONTAL,
		VERTICAL,
		BOTH
	}

	public Transform dragAnchor;

	public GameObject tmpReplaceOnDrag;

	public CanvasGroup canvasGroup;

	public Vector3 mouseOffset;

	public Vector3 startLocalPosition;

	public Vector3 startPosition;

	public Transform startParent;

	public bool reparentOnDrag = true;

	public Transform tmpDragThingy;

	public bool isDraggable;

	public bool dragEnabled = true;

	public DragDirection dragDirection = DragDirection.BOTH;

	public DragEvent<PointerEventData> beforeBeginDrag = new DragEvent<PointerEventData>();

	public DragEvent<PointerEventData> beforeEndDrag = new DragEvent<PointerEventData>();

	public Vector3 manualDragOffset;

	public bool overrideDragPlaneParentReset;

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (dragEnabled)
		{
			beforeBeginDrag.Invoke(eventData);
			InputLockManager.SetControlLock(ControlTypes.UI_DRAGGING, "UIDragPanel");
			if (canvasGroup != null)
			{
				canvasGroup.blocksRaycasts = false;
			}
			mouseOffset = Input.mousePosition - dragAnchor.position;
			startLocalPosition = dragAnchor.localPosition;
			startPosition = dragAnchor.position;
			startParent = dragAnchor.parent;
			isDraggable = UIDragAndDropController.Register(dragAnchor as RectTransform, base.transform as RectTransform, reparentOnDrag);
			if (!isDraggable)
			{
				Debug.LogError("UIDragPanel not draggable");
			}
			if (tmpReplaceOnDrag != null)
			{
				tmpDragThingy = Object.Instantiate(tmpReplaceOnDrag).transform;
				tmpDragThingy.SetParent(base.transform, worldPositionStays: false);
				tmpDragThingy.localPosition = new Vector3(tmpDragThingy.localPosition.x, tmpDragThingy.localPosition.y, 0f);
			}
			base.OnBeginDrag(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (dragEnabled)
		{
			Vector3 position = Input.mousePosition - mouseOffset;
			if (!reparentOnDrag)
			{
				position.z = startPosition.z;
			}
			switch (dragDirection)
			{
			case DragDirection.HORIZONTAL:
				dragAnchor.position = new Vector3(position.x, startPosition.y - manualDragOffset.y, startPosition.z);
				break;
			case DragDirection.VERTICAL:
				dragAnchor.position = new Vector3(startPosition.x - manualDragOffset.x, position.y, startPosition.z);
				break;
			case DragDirection.BOTH:
				dragAnchor.position = position;
				break;
			}
			float z = 0f;
			if (!reparentOnDrag)
			{
				z = startLocalPosition.z;
			}
			dragAnchor.localPosition = new Vector3(dragAnchor.localPosition.x, dragAnchor.localPosition.y, z);
			base.OnDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (!dragEnabled)
		{
			return;
		}
		beforeEndDrag.Invoke(eventData);
		if (canvasGroup != null)
		{
			canvasGroup.blocksRaycasts = true;
		}
		if (tmpReplaceOnDrag != null)
		{
			tmpDragThingy.SetParent(null);
			tmpDragThingy.gameObject.DestroyGameObject();
		}
		if (dragAnchor.parent == UIDragAndDropController.Instance.dragPlane)
		{
			if (reparentOnDrag)
			{
				dragAnchor.SetParent(startParent);
			}
			if (!overrideDragPlaneParentReset)
			{
				dragAnchor.localPosition = startLocalPosition;
			}
		}
		UIDragAndDropController.Unregister(dragAnchor as RectTransform);
		InputLockManager.RemoveControlLock("UIDragPanel");
		base.OnEndDrag(eventData);
	}
}
