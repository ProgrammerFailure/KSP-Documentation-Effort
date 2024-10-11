using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI;

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

	private Vector3 mouseOffset;

	private Vector3 startLocalPosition;

	private Vector3 startPosition;

	private Transform startParent;

	public bool reparentOnDrag;

	private Transform tmpDragThingy;

	private bool isDraggable;

	public bool dragEnabled;

	public DragDirection dragDirection;

	public DragEvent<PointerEventData> beforeBeginDrag;

	public DragEvent<PointerEventData> beforeEndDrag;

	public Vector3 manualDragOffset;

	public bool overrideDragPlaneParentReset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIDragPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}
}
