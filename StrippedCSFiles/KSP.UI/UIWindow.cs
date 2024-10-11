using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI;

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

	public Vector2 minSize;

	public Vector2 maxSize;

	public bool maxSizeIsScreen;

	public float topBorder;

	public float bottomBorder;

	public float leftBorder;

	public float rightBorder;

	public bool invertTopBorder;

	public bool invertBottomBorder;

	public bool invertLeftBorder;

	public bool invertRightBorder;

	public Callback<RectTransform> OnWindowResize;

	public Callback<RectTransform> OnWindowMove;

	public RectTransform rectTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnScreenResolutionModified(int width, int height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MoveWindow(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void MoveRectTransform(RectTransform target, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResizeWindow(PointerEventData data, ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResizeRectTransform(RectTransform target, PointerEventData data, ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampToScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CutToScreen(ResizeHeight resizeHeight, ResizeWidth resizeWidth)
	{
		throw null;
	}
}
