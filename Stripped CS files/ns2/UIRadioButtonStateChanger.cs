using System;
using UnityEngine;

namespace ns2;

[RequireComponent(typeof(UIRadioButton))]
public class UIRadioButtonStateChanger : MonoBehaviour
{
	[Serializable]
	public class RadioButtonState
	{
		public string name;

		public ButtonState stateTrue;

		public ButtonState stateFalse;
	}

	public UIRadioButton button;

	public RadioButtonState[] states;

	[NonSerialized]
	public string currentState = "";

	public void Awake()
	{
		button = GetComponent<UIRadioButton>();
	}

	public void SetState(int index)
	{
		if (index >= 0 && index < states.Length && button != null)
		{
			button.stateTrue.normal = states[index].stateTrue.normal;
			button.stateTrue.highlight = states[index].stateTrue.highlight;
			button.stateTrue.pressed = states[index].stateTrue.pressed;
			button.stateTrue.disabled = states[index].stateTrue.disabled;
			button.stateFalse.normal = states[index].stateFalse.normal;
			button.stateFalse.highlight = states[index].stateFalse.highlight;
			button.stateFalse.pressed = states[index].stateFalse.pressed;
			button.stateFalse.disabled = states[index].stateFalse.disabled;
			currentState = states[index].name;
			button.SetState(button.CurrentState, UIRadioButton.CallType.APPLICATIONSILENT, null);
		}
	}

	public void SetState(string name)
	{
		int num = states.Length;
		while (num-- > 0)
		{
			if (states[num].name == name)
			{
				SetState(num);
			}
		}
	}
}
