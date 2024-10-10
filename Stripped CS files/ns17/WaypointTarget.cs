using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns17;

public class WaypointTarget : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	public RectTransform parent;

	public RectTransform child;

	public float scaleMagnitude = 2f;

	public float scaleSpeed = 15f;

	public float traversalSpeed = 375f;

	public Vector2 padding;

	public Vector3 stationaryScale;

	public Vector3 movingScale;

	public Vector3 initialPosition;

	public Vector3 traversePosition;

	public bool isDragging { get; set; }

	public bool isTraversing { get; set; }

	public void Awake()
	{
		padding = new Vector2(Mathf.Ceil(child.sizeDelta.x / 2f), Mathf.Ceil(child.sizeDelta.y / 2f));
		initialPosition = child.localPosition;
		stationaryScale = child.localScale;
		movingScale = stationaryScale * scaleMagnitude;
		isDragging = false;
		isTraversing = false;
	}

	public void OnDisable()
	{
		child.localScale = stationaryScale;
		isDragging = false;
		isTraversing = false;
	}

	public void Update()
	{
		CheckTraverseClick();
		UpdateTraversal();
		UpdateScale();
	}

	public void UpdateScale()
	{
		if (!isDragging && !isTraversing)
		{
			child.localScale = Vector3.MoveTowards(child.localScale, stationaryScale, Time.deltaTime * scaleSpeed);
		}
		else
		{
			child.localScale = Vector3.MoveTowards(child.localScale, movingScale, Time.deltaTime * scaleSpeed);
		}
	}

	public void CheckTraverseClick()
	{
		if (Mouse.Left.GetClick() && (bool)UIMasterController.Instance && RectTransformUtility.RectangleContainsScreenPoint(parent, Input.mousePosition, UIMasterController.Instance.uiCamera))
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, UIMasterController.Instance.uiCamera, out var localPoint);
			traversePosition = ChildClampedLocalPosition(localPoint);
			isTraversing = true;
		}
	}

	public void UpdateTraversal()
	{
		if (isDragging)
		{
			isTraversing = false;
		}
		if (isTraversing)
		{
			child.localPosition = Vector3.MoveTowards(child.localPosition, traversePosition, Time.deltaTime * traversalSpeed);
			if (child.localPosition == traversePosition)
			{
				isTraversing = false;
			}
			Target();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, eventData.position, eventData.pressEventCamera, out var worldPoint);
		child.position = ChildClampedWorldPosition(worldPoint);
		Target();
	}

	public void Target()
	{
		KerbNetDialog.ChangeMapPosition(child.localPosition);
	}

	public void Reset()
	{
		traversePosition = initialPosition;
		isTraversing = true;
	}

	public Vector3 ChildClampedLocalPosition(Vector3 localPosition)
	{
		Vector3[] array = new Vector3[4];
		parent.GetLocalCorners(array);
		localPosition.x = Mathf.Clamp(localPosition.x, array[0].x + padding.x, array[2].x - padding.x);
		localPosition.y = Mathf.Clamp(localPosition.y, array[0].y + padding.y, array[2].y - padding.y);
		localPosition.z = child.localPosition.z;
		return localPosition;
	}

	public Vector3 ChildClampedWorldPosition(Vector3 position)
	{
		Vector3[] array = new Vector3[4];
		parent.GetWorldCorners(array);
		position.x = Mathf.Clamp(position.x, array[0].x + padding.x, array[2].x - padding.x);
		position.y = Mathf.Clamp(position.y, array[0].y + padding.y, array[2].y - padding.y);
		position.z = child.position.z;
		return position;
	}
}
