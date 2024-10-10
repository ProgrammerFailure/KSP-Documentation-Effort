using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UIRadioButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IRadioButton
{
	public enum StateSetWith
	{
		LEFT,
		RIGHT,
		BOTH
	}

	public enum State
	{
		True,
		False
	}

	public enum CallType
	{
		USER,
		APPLICATION,
		APPLICATIONSILENT
	}

	public enum ClickType
	{
		None,
		ClickOnly,
		ClickAndStateChange
	}

	[Serializable]
	public class ClickEvent<PointerEventData, State, CallType> : UnityEvent<PointerEventData, State, CallType>
	{
	}

	[Serializable]
	public class StateChangeEvent<PointerEventData, CallType> : UnityEvent<PointerEventData, CallType>
	{
	}

	[Serializable]
	public class StateChangeEvent2<UIRadioButton, CallType, PointerEventData> : UnityEvent<UIRadioButton, CallType, PointerEventData>
	{
	}

	public object Data;

	public ClickType leftClick;

	public ClickType rightClick;

	public ButtonState stateTrue;

	public ButtonState stateFalse;

	public TextMeshProUGUI textLabel;

	[SerializeField]
	public State startState;

	public State currentState;

	public bool unselectable = true;

	public ClickEvent<PointerEventData, State, CallType> onClick = new ClickEvent<PointerEventData, State, CallType>();

	public StateChangeEvent<PointerEventData, CallType> onFalse = new StateChangeEvent<PointerEventData, CallType>();

	public StateChangeEvent<PointerEventData, CallType> onTrue = new StateChangeEvent<PointerEventData, CallType>();

	public StateChangeEvent2<UIRadioButton, CallType, PointerEventData> onTrueBtn = new StateChangeEvent2<UIRadioButton, CallType, PointerEventData>();

	public StateChangeEvent2<UIRadioButton, CallType, PointerEventData> onFalseBtn = new StateChangeEvent2<UIRadioButton, CallType, PointerEventData>();

	public bool interactable = true;

	[SerializeField]
	public int radioGroup;

	public RadioButtonGroup group;

	public State StartState => startState;

	public State CurrentState => currentState;

	public Button Button { get; set; }

	public Image Image { get; set; }

	public bool Interactable
	{
		get
		{
			return interactable;
		}
		set
		{
			interactable = value;
			if (Button != null)
			{
				Button.interactable = value;
			}
		}
	}

	public virtual bool Value
	{
		get
		{
			return currentState == State.True;
		}
		set
		{
			if (value && currentState != 0)
			{
				SetState(State.True, CallType.APPLICATION, null);
			}
			else if (currentState != State.False)
			{
				SetState(State.False, CallType.APPLICATION, null);
			}
		}
	}

	public int RadioGroup
	{
		get
		{
			return radioGroup;
		}
		set
		{
			radioGroup = value;
		}
	}

	string IRadioButton.name
	{
		get
		{
			return base.name;
		}
		set
		{
			base.name = value;
		}
	}

	public void Awake()
	{
		Image = GetComponent<Image>();
		Button = GetComponent<Button>();
		currentState = startState;
		SetState(startState, CallType.APPLICATIONSILENT, null);
		if (radioGroup != 0)
		{
			SetGroup(radioGroup, pop: false);
		}
		GameEvents.onMenuNavGetInput.Add(MenuNavigationInputListener);
	}

	public void OnDestroy()
	{
		GameEvents.onMenuNavGetInput.Remove(MenuNavigationInputListener);
		if (group != null)
		{
			group.buttons.Remove(this);
			group = null;
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (!interactable)
		{
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (rightClick == ClickType.ClickOnly)
			{
				onClick.Invoke(eventData, currentState, CallType.USER);
				Button.onClick.Invoke();
				Button.OnSubmit(eventData);
			}
			else if (rightClick == ClickType.ClickAndStateChange)
			{
				ToggleState(CallType.USER, eventData);
				onClick.Invoke(eventData, currentState, CallType.USER);
				Button.onClick.Invoke();
				Button.OnSubmit(eventData);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (leftClick == ClickType.ClickOnly)
			{
				onClick.Invoke(eventData, currentState, CallType.USER);
			}
			if (leftClick == ClickType.ClickAndStateChange)
			{
				ToggleState(CallType.USER, eventData);
				onClick.Invoke(eventData, currentState, CallType.USER);
			}
		}
	}

	public void MenuNavigationInputListener(MenuNavInput input)
	{
		if (base.gameObject.activeInHierarchy && input == MenuNavInput.Accept && currentState == State.False && EventSystem.current.currentSelectedGameObject == base.gameObject)
		{
			Button.onClick.Invoke();
			Value = true;
		}
	}

	public void SetState(State state, CallType callType, PointerEventData data, bool popButtonsInGroup = true)
	{
		switch (state)
		{
		case State.False:
			if (callType != 0 || unselectable)
			{
				currentState = state;
				startState = state;
				stateFalse.Setup(Button, Image, textLabel);
				if (callType != CallType.APPLICATIONSILENT)
				{
					onFalse.Invoke(data, callType);
					onFalseBtn.Invoke(this, callType, data);
				}
			}
			break;
		case State.True:
			currentState = state;
			startState = state;
			stateTrue.Setup(Button, Image, textLabel);
			if (popButtonsInGroup)
			{
				PopOtherButtonsInGroup();
			}
			if (callType != CallType.APPLICATIONSILENT)
			{
				onTrue.Invoke(data, callType);
				onTrueBtn.Invoke(this, callType, data);
			}
			break;
		}
	}

	public void ToggleState(CallType callType, PointerEventData data)
	{
		if (currentState == State.True)
		{
			SetState(State.False, callType, data);
		}
		else
		{
			SetState(State.True, callType, data);
		}
	}

	public void PopOtherButtonsInGroup()
	{
		if (group == null)
		{
			return;
		}
		int count = group.buttons.Count;
		while (count-- > 0)
		{
			UIRadioButton uIRadioButton = (UIRadioButton)group.buttons[count];
			if (uIRadioButton != this && uIRadioButton.RadioGroup != 0)
			{
				uIRadioButton.Value = false;
			}
		}
	}

	public void SetGroup(int groupID, bool pop = true)
	{
		if (group != null)
		{
			group.buttons.Remove(this);
			group = null;
		}
		RadioGroup = groupID;
		group = RadioButtonGroup.GetGroup(groupID);
		group.buttons.Add(this);
		if (Value && pop)
		{
			PopOtherButtonsInGroup();
		}
	}
}
