using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UIStateToggleButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public class ButtonState
	{
		public Sprite normal;

		public Sprite highlight;

		public Sprite pressed;

		public Sprite disabled;

		public void Setup(Button button, Image image)
		{
			SpriteState spriteState = button.spriteState;
			spriteState.highlightedSprite = highlight;
			spriteState.pressedSprite = pressed;
			spriteState.disabledSprite = disabled;
			if (spriteState.selectedSprite == null)
			{
				spriteState.selectedSprite = highlight;
			}
			button.spriteState = spriteState;
			image.sprite = normal;
		}
	}

	public enum BtnState
	{
		True,
		False
	}

	public enum ClickType
	{
		Left,
		Middle,
		Right
	}

	public ButtonState stateTrue;

	public ButtonState stateFalse;

	public Button.ButtonClickedEvent onTrue;

	public Button.ButtonClickedEvent onTrueLeft;

	public Button.ButtonClickedEvent onTrueRight;

	public Button.ButtonClickedEvent onFalse;

	public Button.ButtonClickedEvent onFalseLeft;

	public Button.ButtonClickedEvent onFalseRight;

	public Button.ButtonClickedEvent onClick;

	public Button.ButtonClickedEvent onClickLeft;

	public Button.ButtonClickedEvent onClickRight;

	[SerializeField]
	public bool autoStateChange;

	[SerializeField]
	public BtnState state;

	public BtnState previousState;

	public ButtonState currentState;

	public bool AutoStateChange => autoStateChange;

	public BtnState State => state;

	public bool StateBool => state == BtnState.True;

	public ButtonState CurrentState => currentState;

	public Button Button { get; set; }

	public Image Image { get; set; }

	public EventTrigger Trigger { get; set; }

	public bool interactable
	{
		get
		{
			return Button.interactable;
		}
		set
		{
			Button.interactable = value;
		}
	}

	public void Awake()
	{
		Image = GetComponent<Image>();
		Button = GetComponent<Button>();
	}

	public void Start()
	{
		SetState(state);
	}

	public void SetState(bool state)
	{
		if (StateBool != state)
		{
			SetState((!state) ? BtnState.False : BtnState.True);
		}
	}

	public void SetState(BtnState state)
	{
		this.state = state;
		switch (state)
		{
		case BtnState.False:
			currentState = stateTrue;
			stateFalse.Setup(Button, Image);
			break;
		case BtnState.True:
			currentState = stateTrue;
			stateTrue.Setup(Button, Image);
			break;
		}
	}

	public void ToggleState(int button)
	{
		if (state == BtnState.False)
		{
			onTrue.Invoke();
			if (button == 0)
			{
				onTrueLeft.Invoke();
			}
			else
			{
				onTrueRight.Invoke();
			}
			SetState(BtnState.True);
		}
		else
		{
			onFalse.Invoke();
			if (button == 0)
			{
				onFalseLeft.Invoke();
			}
			else
			{
				onFalseRight.Invoke();
			}
			SetState(BtnState.False);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (interactable)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				onClick.Invoke();
				onClickLeft.Invoke();
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				onClick.Invoke();
				onClickRight.Invoke();
			}
		}
	}
}
