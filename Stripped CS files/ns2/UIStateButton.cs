using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UIStateButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public class ClickEventData<PointerEventData> : UnityEvent<PointerEventData>
	{
	}

	[Serializable]
	public class ClickEvent<String> : UnityEvent<String>
	{
	}

	[Serializable]
	public class OnValueChangeEvent<UIStateButton> : UnityEvent<UIStateButton>
	{
	}

	[SerializeField]
	public bool clickChangesState;

	public ButtonState[] states = new ButtonState[0];

	public string startState = "";

	[NonSerialized]
	public string currentState = "";

	[NonSerialized]
	public int currentStateIndex;

	public bool stateSet;

	public Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

	public ClickEvent<string> onClickState = new ClickEvent<string>();

	public ClickEventData<PointerEventData> onClickEventData = new ClickEventData<PointerEventData>();

	public OnValueChangeEvent<UIStateButton> onValueChanged = new OnValueChangeEvent<UIStateButton>();

	public bool ClickChangesState => clickChangesState;

	public Button Button { get; set; }

	public Image Image { get; set; }

	public void Reset()
	{
		Button = GetComponent<Button>();
	}

	public void Awake()
	{
		Image = GetComponent<Image>();
		Button = GetComponent<Button>();
		if (!stateSet)
		{
			SetState(startState);
		}
		else
		{
			SetState(currentState);
		}
		Button.onClick.AddListener(delegate
		{
			onClick.Invoke();
			onClickState.Invoke(currentState);
			if (ClickChangesState)
			{
				ToggleState();
			}
		});
	}

	public void ToggleState()
	{
		if (currentStateIndex >= states.Length - 1)
		{
			SetState(0);
		}
		else
		{
			SetState(currentStateIndex + 1);
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		onClickEventData.Invoke(eventData);
	}

	public void SetState(int index, bool invokeChange = true)
	{
		if (index < 0 || index >= states.Length)
		{
			return;
		}
		currentState = states[index].name;
		currentStateIndex = index;
		stateSet = true;
		if (Button != null)
		{
			states[index].Setup(Button, Image);
			if (invokeChange)
			{
				onValueChanged.Invoke(this);
			}
		}
	}

	public void SetState(string name, bool invokeChange = true)
	{
		int num = states.Length;
		while (num-- > 0)
		{
			if (states[num].name == name)
			{
				SetState(num, invokeChange);
			}
		}
	}

	public void Enable(bool enable)
	{
		Button.interactable = enable;
	}
}
