using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

public class UIWindow : MonoBehaviour
{
	public enum ResizeWidth
	{
		None,
		Left,
		Right
	}

	public enum ResizeHeight
	{
		None,
		Top,
		Bottom
	}

	public Vector2 minSize = new Vector2(320f, 240f);

	public Vector2 maxSize = new Vector2(640f, 480f);

	public bool maxSizeIsScreen;

	public float topBorder = 40f;

	public float bottomBorder = 40f;

	public float leftBorder = 40f;

	public float rightBorder = 40f;

	public bool invertTopBorder;

	public bool invertBottomBorder;

	public bool invertLeftBorder;

	public bool invertRightBorder;

	public Callback<RectTransform> OnWindowResize;

	public Callback<RectTransform> OnWindowMove;

	public RectTransform rectTransform { get; set; }

	public void Awake()
	{
		rectTransform = base.transform as RectTransform;
	}

	public void Start()
	{
		GameEvents.onScreenResolutionModified.Add(OnScreenResolutionModified);
	}

	public void OnDestroy()
	{
		GameEvents.onScreenResolutionModified.Remove(OnScreenResolutionModified);
	}

	public void OnScreenResolutionModified(int width, int height)
	{
		ClampToScreen();
	}

	public void MoveWindow(PointerEventData data)
	{
		rectTransform.localPosition += new Vector3(data.delta.x, data.delta.y);
		ClampToScreen();
		if (OnWindowMove != null)
		{
			OnWindowMove(rectTransform);
		}
	}

	public static void MoveRectTransform(RectTransform target, PointerEventData data)
	{
		target.localPosition += new Vector3(data.delta.x, data.delta.y);
	}

	public void ResizeWindow(PointerEventData data, ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		Vector2 delta = data.delta;
		Vector2 offsetMin = rectTransform.offsetMin;
		Vector2 offsetMax = rectTransform.offsetMax;
		switch (resizeHeight)
		{
		case ResizeHeight.Top:
		{
			offsetMax.y += delta.y;
			float num2 = offsetMax.y - offsetMin.y;
			if (num2 < minSize.y)
			{
				offsetMax.y = offsetMin.y + minSize.y;
			}
			if (num2 > maxSize.y)
			{
				offsetMax.y = offsetMin.y + maxSize.y;
			}
			break;
		}
		case ResizeHeight.Bottom:
		{
			offsetMin.y += delta.y;
			float num = offsetMax.y - offsetMin.y;
			if (num < minSize.y)
			{
				offsetMin.y = offsetMax.y - minSize.y;
			}
			if (num > maxSize.y)
			{
				offsetMin.y = offsetMax.y - maxSize.y;
			}
			break;
		}
		}
		switch (resizeWidth)
		{
		case ResizeWidth.Left:
		{
			offsetMin.x += delta.x;
			float num4 = offsetMax.x - offsetMin.x;
			if (num4 < minSize.x)
			{
				offsetMin.x = offsetMax.x - minSize.x;
			}
			if (num4 > maxSize.x)
			{
				offsetMin.x = offsetMax.x - maxSize.x;
			}
			break;
		}
		case ResizeWidth.Right:
		{
			offsetMax.x += delta.x;
			float num3 = offsetMax.x - offsetMin.x;
			if (num3 < minSize.x)
			{
				offsetMax.x = offsetMin.x + minSize.x;
			}
			if (num3 > maxSize.x)
			{
				offsetMax.x = offsetMin.x + maxSize.x;
			}
			break;
		}
		}
		rectTransform.offsetMin = offsetMin;
		rectTransform.offsetMax = offsetMax;
		CutToScreen(resizeHeight, resizeWidth);
		if (OnWindowResize != null)
		{
			OnWindowResize(rectTransform);
		}
	}

	public static void ResizeRectTransform(RectTransform target, PointerEventData data, ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		Vector2 offsetMin = target.offsetMin;
		Vector2 offsetMax = target.offsetMax;
		switch (resizeHeight)
		{
		case ResizeHeight.Top:
			offsetMax.y += data.delta.y;
			break;
		case ResizeHeight.Bottom:
			offsetMin.y += data.delta.y;
			break;
		}
		switch (resizeWidth)
		{
		case ResizeWidth.Left:
			offsetMin.x += data.delta.x;
			break;
		case ResizeWidth.Right:
			offsetMax.x += data.delta.x;
			break;
		}
		target.offsetMin = offsetMin;
		target.offsetMax = offsetMax;
	}

	public void ClampToScreen()
	{
		float num = topBorder;
		float num2 = bottomBorder;
		float num3 = leftBorder;
		float num4 = rightBorder;
		float x = rectTransform.sizeDelta.x;
		float y = rectTransform.sizeDelta.y;
		if (invertTopBorder)
		{
			num = 0f - (y - num);
		}
		if (invertBottomBorder)
		{
			num2 = 0f - (y - num2);
		}
		if (invertLeftBorder)
		{
			num3 = 0f - (x - num3);
		}
		if (invertRightBorder)
		{
			num4 = 0f - (x - num4);
		}
		UIMasterController.ClampToScreen(rectTransform, num, num2, num3, num4);
	}

	public void CutToScreen(ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		bool cutTop = resizeHeight == ResizeHeight.Top;
		bool cutBottom = resizeHeight == ResizeHeight.Bottom;
		bool cutLeft = resizeWidth == ResizeWidth.Left;
		bool cutRight = resizeWidth == ResizeWidth.Right;
		UIMasterController.CutToScreen(rectTransform, cutTop, cutBottom, cutLeft, cutRight);
	}
}
