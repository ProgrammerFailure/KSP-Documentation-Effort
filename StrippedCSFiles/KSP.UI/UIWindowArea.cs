using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class UIWindowArea : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
{
	public bool moveWindow;

	public bool resizeWindow;

	public UIWindow.ResizeWidth resizeWidth;

	public UIWindow.ResizeHeight resizeHeight;

	public RectTransform targetWindow;

	private UIWindow uiWindow;

	private CanvasPixelPerfectHandler pixelPerfectHandler;

	[SerializeField]
	private bool pixelPerfectZeroDelay;

	private float originalDelay;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIWindowArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrag(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnMove(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnResize(PointerEventData data)
	{
		throw null;
	}
}
