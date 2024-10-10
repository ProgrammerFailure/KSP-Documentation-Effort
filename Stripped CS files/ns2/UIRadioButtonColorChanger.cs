using UnityEngine;
using UnityEngine.EventSystems;

namespace ns2;

[RequireComponent(typeof(UIRadioButton))]
public class UIRadioButtonColorChanger : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IRadioButton
{
	public UIRadioButton button;

	public ButtonColorState stateTrue;

	public ButtonColorState stateFalse;

	public bool isHovering;

	[SerializeField]
	public int hoverRadioGroup;

	public RadioButtonGroup group;

	public int HoverRadioGroup
	{
		get
		{
			return hoverRadioGroup;
		}
		set
		{
			hoverRadioGroup = value;
		}
	}

	public bool Value
	{
		get
		{
			return isHovering;
		}
		set
		{
			OnPointerExit(null);
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
		button = GetComponent<UIRadioButton>();
		button.onTrue.AddListener(OnTrue);
		button.onFalse.AddListener(OnFalse);
		if (hoverRadioGroup != 0)
		{
			SetGroup(hoverRadioGroup, pop: false);
		}
	}

	public void Start()
	{
		if (button.Value)
		{
			button.Image.color = stateTrue.normalColor;
		}
		else
		{
			button.Image.color = stateFalse.normalColor;
		}
	}

	public void OnDestroy()
	{
		if (group != null)
		{
			group.buttons.Remove(this);
			group = null;
		}
	}

	public void OnTrue(PointerEventData eventData, UIRadioButton.CallType callType)
	{
		button.Image.color = stateTrue.normalColor;
	}

	public void OnFalse(PointerEventData eventData, UIRadioButton.CallType callType)
	{
		button.Image.color = stateFalse.normalColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (button.CurrentState == UIRadioButton.State.True)
		{
			button.Image.color = stateTrue.pressedColor;
		}
		else
		{
			button.Image.color = stateFalse.pressedColor;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (button.CurrentState == UIRadioButton.State.True)
		{
			button.Image.color = stateTrue.highlightColor;
		}
		else
		{
			button.Image.color = stateFalse.highlightColor;
		}
		isHovering = true;
		PopOtherButtonsInGroup();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (button.CurrentState == UIRadioButton.State.True)
		{
			button.Image.color = stateTrue.normalColor;
		}
		else
		{
			button.Image.color = stateFalse.normalColor;
		}
		isHovering = false;
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
			UIRadioButtonColorChanger uIRadioButtonColorChanger = (UIRadioButtonColorChanger)group.buttons[count];
			if (uIRadioButtonColorChanger != this && uIRadioButtonColorChanger.HoverRadioGroup != 0)
			{
				uIRadioButtonColorChanger.Value = false;
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
		HoverRadioGroup = groupID;
		group = RadioButtonGroup.GetGroup(groupID);
		group.buttons.Add(this);
		if (Value && pop)
		{
			PopOtherButtonsInGroup();
		}
	}
}
