using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/BoxSlider", 35)]
public class BoxSlider : Selectable, IDragHandler, IEventSystemHandler, IInitializePotentialDragHandler, ICanvasElement
{
	public enum Direction
	{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom
	}

	[Serializable]
	public class BoxSliderEvent : UnityEvent<float, float>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public BoxSliderEvent()
		{
			throw null;
		}
	}

	private enum Axis
	{
		Horizontal,
		Vertical
	}

	[SerializeField]
	private RectTransform m_HandleRect;

	[SerializeField]
	[Space(6f)]
	private float m_MinValue;

	[SerializeField]
	private float m_MaxValue;

	[SerializeField]
	private bool m_WholeNumbers;

	[SerializeField]
	private float m_Value;

	[SerializeField]
	private float m_ValueY;

	[SerializeField]
	[Space(6f)]
	private BoxSliderEvent m_OnValueChanged;

	private Transform m_HandleTransform;

	private RectTransform m_HandleContainerRect;

	private Vector2 m_Offset;

	private DrivenRectTransformTracker m_Tracker;

	public RectTransform handleRect
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float minValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float maxValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool wholeNumbers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float normalizedValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float valueY
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float normalizedValueY
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public BoxSliderEvent onValueChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private float stepSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected BoxSlider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Rebuild(CanvasUpdate executing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LayoutComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GraphicUpdateComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCachedReferences()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Set(float input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Set(float input, bool sendCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetY(float input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetY(float input, bool sendCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRectTransformDimensionsChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisuals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDrag(PointerEventData eventData, Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MayDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	Transform ICanvasElement.get_transform()
	{
		throw null;
	}
}
