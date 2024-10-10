using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(EventTrigger))]
public class UITextButton : MonoBehaviour
{
	public Color normal = Color.white;

	public Color highlight = Color.white;

	public Color pressed = Color.white;

	public Color disabled = Color.white;

	public Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

	public bool mouseOver;

	[SerializeField]
	public bool _interactable;

	public Text Text { get; set; }

	public EventTrigger Trigger { get; set; }

	public bool interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			_interactable = value;
			if (!_interactable)
			{
				SetState(disabled);
			}
			else if (mouseOver)
			{
				SetState(highlight);
			}
		}
	}

	public void Awake()
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(OnMouseEnter);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(OnMouseExit);
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.PointerClick;
		entry3.callback.AddListener(OnMouseClick);
		EventTrigger.Entry entry4 = new EventTrigger.Entry();
		entry4.eventID = EventTriggerType.PointerDown;
		entry4.callback.AddListener(OnMouseDown);
		EventTrigger.Entry entry5 = new EventTrigger.Entry();
		entry5.eventID = EventTriggerType.PointerUp;
		entry5.callback.AddListener(OnMouseUp);
		EventTrigger component = GetComponent<EventTrigger>();
		component.triggers.Add(entry);
		component.triggers.Add(entry2);
		component.triggers.Add(entry3);
		component.triggers.Add(entry4);
		component.triggers.Add(entry5);
		Text = GetComponent<Text>();
	}

	public void Start()
	{
		if (!_interactable)
		{
			SetState(normal);
		}
		else
		{
			SetState(disabled);
		}
	}

	public void SetState(Color color)
	{
		Text.color = color;
	}

	public void OnMouseEnter(BaseEventData data)
	{
		if (_interactable)
		{
			SetState(highlight);
		}
		mouseOver = true;
	}

	public void OnMouseExit(BaseEventData data)
	{
		if (_interactable)
		{
			SetState(normal);
		}
		mouseOver = false;
	}

	public void OnMouseClick(BaseEventData data)
	{
		if (_interactable)
		{
			onClick.Invoke();
		}
	}

	public void OnMouseDown(BaseEventData data)
	{
		if (_interactable)
		{
			SetState(pressed);
		}
	}

	public void OnMouseUp(BaseEventData data)
	{
		if (_interactable)
		{
			SetState(highlight);
		}
	}
}
