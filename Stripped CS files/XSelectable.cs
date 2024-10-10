using UnityEngine.EventSystems;

public class XSelectable : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IMoveHandler
{
	public bool hover;

	public bool down;

	public bool selected;

	public bool Hover => hover;

	public bool Down => down;

	public bool Selected => selected;

	public event Callback<XSelectable, PointerEventData> onPointerEnter;

	public event Callback<XSelectable, PointerEventData> onPointerExit;

	public event Callback<XSelectable, PointerEventData> onPointerDown;

	public event Callback<XSelectable, PointerEventData> onPointerUp;

	public event Callback<XSelectable, BaseEventData> onSelect;

	public event Callback<XSelectable, BaseEventData> onDeselect;

	public event Callback<XSelectable, AxisEventData> onMove;

	public override void Awake()
	{
		base.Awake();
		this.onPointerDown = delegate
		{
		};
		this.onPointerEnter = delegate
		{
		};
		this.onPointerExit = delegate
		{
		};
		this.onPointerUp = delegate
		{
		};
		this.onSelect = delegate
		{
		};
		this.onDeselect = delegate
		{
		};
		this.onMove = delegate
		{
		};
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
		this.onPointerEnter(this, eventData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
		this.onPointerExit(this, eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		down = true;
		this.onPointerDown(this, eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		down = false;
		this.onPointerUp(this, eventData);
	}

	public void OnMove(AxisEventData eventData)
	{
		this.onMove(this, eventData);
	}

	public void OnSelect(BaseEventData eventData)
	{
		selected = true;
		this.onSelect(this, eventData);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		selected = false;
		this.onDeselect(this, eventData);
	}

	public void Clear()
	{
		selected = false;
		hover = false;
		down = false;
	}
}
