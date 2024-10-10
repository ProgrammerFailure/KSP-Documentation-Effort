using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Button))]
public class UIButtonToggle : MonoBehaviour
{
	public Image toggleImage;

	public Sprite spriteOn;

	public Sprite spriteOff;

	public bool startOn = true;

	public bool canToggleOn = true;

	public bool canToggleOff;

	public Button.ButtonClickedEvent onToggle = new Button.ButtonClickedEvent();

	public Button.ButtonClickedEvent onToggleOff = new Button.ButtonClickedEvent();

	public Button.ButtonClickedEvent onToggleOn = new Button.ButtonClickedEvent();

	public Button button;

	public bool hasBeenSet;

	public bool _state;

	public Button ButtonCtrl => button;

	public bool state
	{
		get
		{
			return _state;
		}
		set
		{
			SetState(value);
		}
	}

	public bool interactable
	{
		get
		{
			return button.interactable;
		}
		set
		{
			button.interactable = value;
		}
	}

	public void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		if (!hasBeenSet)
		{
			SetState(startOn);
		}
	}

	public void OnClick()
	{
		if (state)
		{
			if (canToggleOff)
			{
				SetState(on: false);
				onToggle.Invoke();
				onToggleOff.Invoke();
			}
		}
		else if (canToggleOn)
		{
			SetState(on: true);
			onToggle.Invoke();
			onToggleOn.Invoke();
		}
	}

	public void SetState(bool on)
	{
		_state = on;
		hasBeenSet = true;
		if (_state)
		{
			toggleImage.sprite = spriteOn;
		}
		else
		{
			toggleImage.sprite = spriteOff;
		}
	}
}
