using ns2;
using TMPro;
using UnityEngine.UI;

[UI_Toggle]
public class UIPartActionToggle : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldStatus;

	public UIButtonToggle toggle;

	public TextMeshProUGUI tipText;

	public LayoutElement layoutElement;

	public float heightNoTip = 14f;

	public float heightWithTip = 28f;

	public bool fieldValue;

	public UI_Toggle toggleControl => (UI_Toggle)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		SetButtonState(GetFieldValue(), forceButton: true);
		fieldName.text = field.guiName;
		toggle.onToggle.AddListener(OnTap);
	}

	public void Awake()
	{
		layoutElement = base.gameObject.GetComponent<LayoutElement>();
	}

	public bool GetFieldValue()
	{
		return field.GetValue<bool>(field.host);
	}

	public void SetButtonState(bool state, bool forceButton)
	{
		if (state)
		{
			if (forceButton)
			{
				toggle.SetState(!toggleControl.invertButton);
			}
			fieldStatus.text = toggleControl.displayEnabledText;
		}
		else
		{
			if (forceButton)
			{
				toggle.SetState(toggleControl.invertButton);
			}
			fieldStatus.text = toggleControl.displayDisabledText;
		}
	}

	public void OnTap()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			Mouse.Left.ClearMouseState();
			fieldValue = toggle.state ^ toggleControl.invertButton;
			SetButtonState(fieldValue, forceButton: false);
			SetFieldValue(fieldValue);
		}
	}

	public override void UpdateItem()
	{
		base.UpdateItem();
		SetButtonState(GetFieldValue(), forceButton: true);
		toggle.interactable = base.Field.guiInteractable;
		tipText.text = toggleControl.tipText;
		if (layoutElement != null)
		{
			layoutElement.preferredHeight = (string.IsNullOrEmpty(tipText.text) ? heightNoTip : heightWithTip);
		}
	}
}
