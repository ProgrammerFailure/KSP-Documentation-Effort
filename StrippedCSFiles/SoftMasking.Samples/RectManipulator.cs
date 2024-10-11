using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking.Samples;

[RequireComponent(typeof(RectTransform))]
public class RectManipulator : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Flags]
	public enum ManipulationType
	{
		None = 0,
		Move = 1,
		ResizeLeft = 2,
		ResizeUp = 4,
		ResizeRight = 8,
		ResizeDown = 0x10,
		ResizeUpLeft = 6,
		ResizeUpRight = 0xC,
		ResizeDownLeft = 0x12,
		ResizeDownRight = 0x18,
		Rotate = 0x20
	}

	public RectTransform targetTransform;

	public ManipulationType manipulation;

	public ShowOnHover showOnHover;

	[Header("Limits")]
	public Vector2 minSize;

	[Header("Display")]
	public Graphic icon;

	public float normalAlpha;

	public float selectedAlpha;

	public float transitionDuration;

	private bool _isManipulatedNow;

	private Vector2 _startAnchoredPosition;

	private Vector2 _startSizeDelta;

	private float _startRotation;

	private RectTransform parentTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RectManipulator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightIcon(bool highlight, bool instant = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RememberStartTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ToParentSpace(Vector2 position, Camera eventCamera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoMove(Vector2 parentSpaceMovement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool Is(ManipulationType expected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MoveTo(Vector2 desiredAnchoredPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ClampPosition(Vector2 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoRotate(Vector2 startParentPoint, Vector2 targetParentPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float DeltaRotation(Vector2 startLever, Vector2 endLever)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoResize(Vector2 parentSpaceMovement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ProjectResizeOffset(Vector2 localOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ResizeSign()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSizeDirected(Vector2 localOffset, Vector2 sizeSign)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 ClampSize(Vector2 size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}
}
