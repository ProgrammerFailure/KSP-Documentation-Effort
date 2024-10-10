using ns12;
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
	public LimitOptions limitType;

	[SerializeField]
	public TooltipController_Text toolTip;

	public float deadZone = 1f;

	public Vector3 localPosCache;

	public RoboticControllerWindowAxis Axis { get; set; }

	public LimitOptions LimitType => limitType;

	public void Setup(RoboticControllerWindowAxis axis, float deadzone, float initialValue)
	{
		Axis = axis;
		deadZone = deadzone;
		SetToolTipValue(initialValue);
	}

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void OnDestroy()
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Axis.BeginDragAxisLimit(this, eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Axis.EndDragAxisLimit(this, eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Axis.OnDragAxisLimit(this, eventData);
	}

	public void SetToolTipValue(float newValue)
	{
		toolTip.SetText(newValue.ToString("F1"));
	}

	public void UpdateYPosition(float newY)
	{
		localPosCache = base.transform.localPosition;
		localPosCache.y = newY + deadZone;
		base.transform.localPosition = localPosCache;
	}
}
