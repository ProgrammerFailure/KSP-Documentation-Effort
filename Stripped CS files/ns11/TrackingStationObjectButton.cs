using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

[RequireComponent(typeof(Button))]
public class TrackingStationObjectButton : MonoBehaviour
{
	public Button.ButtonClickedEvent OnClickLeft = new Button.ButtonClickedEvent();

	public Button.ButtonClickedEvent OnClickRight = new Button.ButtonClickedEvent();

	public Image image;

	public Sprite spriteTrue;

	public Sprite spriteFalse;

	public TextMeshProUGUI textCount;

	public bool state = true;

	public Button button;

	public bool hover;

	public bool hoverOnLastDown;

	public void Awake()
	{
		button = GetComponent<Button>();
		EventTrigger eventTrigger = GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = base.gameObject.AddComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(OnMouseEnter);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(OnMouseExit);
		eventTrigger.triggers.Add(entry);
		eventTrigger.triggers.Add(entry2);
		SetState(state);
	}

	public void SetState(bool state)
	{
		this.state = state;
		if (state)
		{
			image.sprite = spriteTrue;
		}
		else
		{
			image.sprite = spriteFalse;
		}
	}

	public void SetCount(int count)
	{
		textCount.text = count.ToString();
	}

	public void ShowCount(bool showCount)
	{
		textCount.gameObject.SetActive(showCount);
	}

	public void Lock()
	{
		button.interactable = false;
	}

	public void Unlock()
	{
		button.interactable = true;
	}

	[ContextMenu("UpdateState")]
	public void UpdateState()
	{
		SetState(state);
	}

	public void OnMouseEnter(BaseEventData data)
	{
		hover = true;
	}

	public void OnMouseExit(BaseEventData data)
	{
		hover = false;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			hoverOnLastDown = hover;
		}
		if (hover)
		{
			if (Input.GetMouseButtonUp(0) && hoverOnLastDown)
			{
				SetState(!state);
				OnClickLeft.Invoke();
			}
			if (Input.GetMouseButtonUp(1) && hoverOnLastDown)
			{
				SetState(!state);
				OnClickRight.Invoke();
			}
		}
	}
}
