using ns2;
using ns9;
using TMPro;

[UI_Cycle]
public class UIPartActionCycle : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldStatus;

	public UIButtonToggle toggle;

	public int fieldValue;

	public UI_Cycle toggleControl => (UI_Cycle)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		fieldValue = GetFieldValue();
		SetButtonState(fieldValue);
		fieldName.text = field.guiName;
		toggle.onToggle.AddListener(OnTap);
	}

	public int GetFieldValue()
	{
		return field.GetValue<int>(field.host);
	}

	public void SetButtonState(int state)
	{
		fieldStatus.text = Localizer.Format(toggleControl.stateNames[state]);
		toggle.SetState(on: false);
	}

	public void OnTap()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			fieldValue = (fieldValue + 1) % toggleControl.stateNames.Length;
			SetButtonState(fieldValue);
			SetFieldValue(fieldValue);
		}
	}

	public override void UpdateItem()
	{
		fieldValue = GetFieldValue();
		SetButtonState(fieldValue);
	}
}
