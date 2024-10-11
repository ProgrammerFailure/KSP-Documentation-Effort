using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Serenity;

public class AxisLimitLine : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public enum LimitOptions
	{
		Min,
		Max
	}

	[SerializeField]
	private LimitOptions limitType;

	[SerializeField]
	private TooltipController_Text toolTip;

	internal float deadZone;

	private Vector3 localPosCache;

	public RoboticControllerWindowAxis Axis
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

	public LimitOptions LimitType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisLimitLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(RoboticControllerWindowAxis axis, float deadzone, float initialValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetToolTipValue(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateYPosition(float newY)
	{
		throw null;
	}
}
