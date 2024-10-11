using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class MessageSystemAppFrame : MonoBehaviour
{
	public PointerEnterExitHandler hoverController;

	public DragHandler dragHeader;

	public DragHandler dragFooter;

	public TextMeshProUGUI header;

	public UIList scrollList;

	public int minHeight;

	public int maxHeight;

	private ApplicationLauncherButton appLauncherButton;

	private RectTransform rectTransform;

	private ScrollRect scrollRect;

	private bool scaleHeightToContainList;

	public bool anchorToAppButton;

	private int width;

	private int height;

	public PointerClickHandler deleteHandler;

	public TextMeshProUGUI deleteButtonText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MessageSystemAppFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string displayName, int width, int height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddGlobalInputDelegate(UnityAction<PointerEventData> pointerEnter, UnityAction<PointerEventData> pointerExit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDragFooter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDragFooter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDragFooter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDraggingBounds(int height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDragHeader(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDragHeader(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDragHeader(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDraggingBoundsAtBottom(float height, float pos)
	{
		throw null;
	}
}
