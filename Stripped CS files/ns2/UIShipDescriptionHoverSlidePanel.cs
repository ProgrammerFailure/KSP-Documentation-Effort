using TMPro;
using UnityEngine.EventSystems;

namespace ns2;

public class UIShipDescriptionHoverSlidePanel : UIHoverSlidePanel
{
	public XSelectable inputFieldSelectable;

	public TMP_InputField shipTextField;

	public TMP_InputField descriptionTextField;

	public bool nameFieldSelected;

	public bool descriptionFieldHover;

	public new void Start()
	{
		base.Start();
		inputFieldSelectable.onPointerEnter += InputFieldSelectable_onPointerEnter;
		inputFieldSelectable.onPointerExit += InputFieldSelectable_onPointerExit;
		shipTextField.onEndEdit.AddListener(OnEndEdit);
		descriptionTextField.onEndEdit.AddListener(OnEndEdit);
	}

	public void InputFieldSelectable_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		nameFieldSelected = false;
		UpdateHoverState();
	}

	public void InputFieldSelectable_onPointerEnter(XSelectable arg1, PointerEventData arg2)
	{
		if (EditorLogic.SelectedPart == null)
		{
			shipTextField.interactable = true;
			descriptionTextField.interactable = true;
			nameFieldSelected = true;
			UpdateHoverState();
		}
		else
		{
			shipTextField.interactable = false;
			descriptionTextField.interactable = false;
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		descriptionFieldHover = true;
		UpdateHoverState();
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		descriptionFieldHover = false;
		UpdateHoverState();
	}

	public void OnEndEdit(string id)
	{
		UpdateHoverState();
	}

	public void UpdateHoverState()
	{
		if (EditorLogic.SelectedPart != null)
		{
			shipTextField.interactable = false;
			descriptionTextField.interactable = false;
		}
		else
		{
			shipTextField.interactable = true;
			descriptionTextField.interactable = true;
		}
		pointOver = descriptionFieldHover || nameFieldSelected || shipTextField.isFocused || descriptionTextField.isFocused;
		if (pointOver)
		{
			if (coroutine == null && EditorLogic.SelectedPart == null)
			{
				coroutine = StartCoroutine(MoveToState(0f, newState: true));
			}
		}
		else if (coroutine == null)
		{
			coroutine = StartCoroutine(MoveToState(0.7f, newState: false));
		}
	}
}
