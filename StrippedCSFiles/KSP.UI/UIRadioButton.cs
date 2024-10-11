using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

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
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClickEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class StateChangeEvent<PointerEventData, CallType> : UnityEvent<PointerEventData, CallType>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StateChangeEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class StateChangeEvent2<UIRadioButton, CallType, PointerEventData> : UnityEvent<UIRadioButton, CallType, PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StateChangeEvent2()
		{
			throw null;
		}
	}

	public object Data;

	public ClickType leftClick;

	public ClickType rightClick;

	public ButtonState stateTrue;

	public ButtonState stateFalse;

	public TextMeshProUGUI textLabel;

	[SerializeField]
	private State startState;

	protected State currentState;

	public bool unselectable;

	public ClickEvent<PointerEventData, State, CallType> onClick;

	public StateChangeEvent<PointerEventData, CallType> onFalse;

	public StateChangeEvent<PointerEventData, CallType> onTrue;

	public StateChangeEvent2<UIRadioButton, CallType, PointerEventData> onTrueBtn;

	public StateChangeEvent2<UIRadioButton, CallType, PointerEventData> onFalseBtn;

	private bool interactable;

	[SerializeField]
	private int radioGroup;

	protected RadioButtonGroup group;

	public State StartState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public State CurrentState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Button Button
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public Image Image
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

	public bool Interactable
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

	public virtual bool Value
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

	public int RadioGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIRadioButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MenuNavigationInputListener(MenuNavInput input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(State state, CallType callType, PointerEventData data, bool popButtonsInGroup = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleState(CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PopOtherButtonsInGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGroup(int groupID, bool pop = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	string IRadioButton.get_name()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	void IRadioButton.set_name(string value)
	{
		throw null;
	}
}
