using UnityEngine;
using UnityEngine.UI;

[UI_Vector2]
public class UIPartActionVector2 : UIPartActionFieldItem
{
	public Text fieldName;

	public Text fieldAmount;

	public Slider slider;

	public float fieldValue;

	public UI_Vector2 progBarControl => (UI_Vector2)control;

	public void Awake()
	{
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		SetSliderValue(GetFieldValue());
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public void OnValueChanged(float obj)
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			fieldValue = slider.value * (progBarControl.maxValueX - progBarControl.minValueX) + progBarControl.minValueX;
			Debug.LogError("FVSet: " + slider.value + " " + fieldValue);
			SetFieldValue(fieldValue);
		}
	}

	public float GetFieldValue()
	{
		return field.GetValue<float>(field.host);
	}

	public void SetSliderValue(float rawValue)
	{
		slider.value = Mathf.Clamp01((rawValue - progBarControl.minValueX) / (progBarControl.maxValueX - progBarControl.minValueX));
	}

	public override void UpdateItem()
	{
		fieldValue = GetFieldValue();
		Debug.LogError("FV: " + fieldValue + "   " + (fieldValue - progBarControl.minValueX) / (progBarControl.maxValueX - progBarControl.minValueX));
		fieldName.text = field.guiName;
		fieldAmount.text = KSPUtil.LocalizeNumber(fieldValue, field.guiFormat);
		SetSliderValue(fieldValue);
	}
}
