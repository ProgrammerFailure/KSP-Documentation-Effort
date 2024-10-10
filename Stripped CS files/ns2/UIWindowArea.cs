using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class UIWindowArea : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
{
	public bool moveWindow;

	public bool resizeWindow;

	public UIWindow.ResizeWidth resizeWidth;

	public UIWindow.ResizeHeight resizeHeight;

	public RectTransform targetWindow;

	public UIWindow uiWindow;

	public CanvasPixelPerfectHandler pixelPerfectHandler;

	[SerializeField]
	public bool pixelPerfectZeroDelay;

	public float originalDelay;

	public void Start()
	{
		if (!(targetWindow == null))
		{
			uiWindow = targetWindow.GetComponent<UIWindow>();
			pixelPerfectHandler = base.gameObject.GetComponentUpwards<CanvasPixelPerfectHandler>();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (uiWindow != null && uiWindow.maxSizeIsScreen)
		{
			uiWindow.maxSize = new Vector2(Screen.width, Screen.height);
		}
	}

	public virtual void OnDrag(PointerEventData data)
	{
		if (targetWindow == null)
		{
			return;
		}
		if (pixelPerfectHandler != null)
		{
			if (pixelPerfectZeroDelay)
			{
				originalDelay = pixelPerfectHandler.delay;
				pixelPerfectHandler.delay = 0f;
			}
			pixelPerfectHandler.TemporaryDisable();
			if (pixelPerfectZeroDelay)
			{
				pixelPerfectHandler.delay = originalDelay;
			}
		}
		if (moveWindow)
		{
			OnMove(data);
		}
		if (resizeWindow)
		{
			OnResize(data);
		}
	}

	public virtual void OnMove(PointerEventData data)
	{
		if (uiWindow != null)
		{
			uiWindow.MoveWindow(data);
		}
		else
		{
			targetWindow.localPosition += new Vector3(data.delta.x, data.delta.y);
		}
	}

	public virtual void OnResize(PointerEventData data)
	{
		if (uiWindow != null)
		{
			uiWindow.ResizeWindow(data, resizeHeight, resizeWidth);
		}
		else
		{
			UIWindow.ResizeRectTransform(targetWindow, data, resizeHeight, resizeWidth);
		}
	}
}
