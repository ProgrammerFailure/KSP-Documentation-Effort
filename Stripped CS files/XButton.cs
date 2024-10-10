using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XButton : Button
{
	public bool hover;

	public bool down;

	public bool selected;

	public bool Hover => hover;

	public bool Down => down;

	public bool Selected => selected;

	public event Callback<XButton, PointerEventData> onPointerEnter;

	public event Callback<XButton, PointerEventData> onPointerExit;

	public event Callback<XButton, PointerEventData> onPointerDown;

	public event Callback<XButton, PointerEventData> onPointerUp;

	public event Callback<XButton, BaseEventData> onSelect;

	public event Callback<XButton, BaseEventData> onDeselect;

	public event Callback<XButton, AxisEventData> onMove;

	public static XButton Create(GameObject host, XGraphic tgtGraphic)
	{
		XButton xButton = host.AddComponent<XButton>();
		xButton.targetGraphic = tgtGraphic;
		return xButton;
	}

	public void Terminate()
	{
		Object.Destroy(base.gameObject);
	}

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

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		this.onPointerEnter(this, eventData);
		hover = true;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		this.onPointerExit(this, eventData);
		hover = false;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		this.onPointerDown(this, eventData);
		down = true;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.onPointerUp(this, eventData);
		down = false;
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.onSelect(this, eventData);
		selected = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		this.onDeselect(this, eventData);
		selected = false;
	}

	public override void OnMove(AxisEventData eventData)
	{
		base.OnMove(eventData);
		this.onMove(this, eventData);
	}
}
