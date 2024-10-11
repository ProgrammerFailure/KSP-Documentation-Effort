using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class UIGridArea : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	public ScrollRect scrollRect;

	public RectTransform gridBackground;

	public Button zoom_up;

	public Button zoom_down;

	public TextMeshProUGUI zoom_level;

	public float zoomMin;

	public float zoomMax;

	public float zoomSpeed;

	public Material LineMaterial;

	public Material LineMaterialGray;

	public float roundedCornerRadius;

	public int lineCornerSegments;

	public float lineThickness;

	[NonSerialized]
	public float gridWidth;

	[NonSerialized]
	public float gridHeight;

	[NonSerialized]
	public float width;

	[NonSerialized]
	public float height;

	internal float zoom;

	protected Rect snapBounds;

	internal Callback zoomCallback;

	private float scaledWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private float scaledHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIGridArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnScroll(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SnapToNode(RDNode node, Vector2 offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateSnapBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Snap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Zoom(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ZoomTo(float level, bool center)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetZoomButtonState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetZoomLevelText(float level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void InputZoomUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void InputZoomDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector2 MousePointOnGrid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector2 CenterOfGrid()
	{
		throw null;
	}
}
