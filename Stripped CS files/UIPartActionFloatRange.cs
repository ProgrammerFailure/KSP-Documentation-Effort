using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_FloatRange]
public class UIPartActionFloatRange : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldAmount;

	public Slider slider;

	public GameObject sliderBackground;

	public GameObject numericContainer;

	public TextMeshProUGUI fieldNameNumeric;

	public TMP_InputField inputField;

	public float fieldValue;

	public float lerpedValue;

	public float moddedValue;

	public bool handlingChange;

	public UI_FloatRange progBarControl => (UI_FloatRange)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		SetSliderValue(GetFieldValue());
		slider.onValueChanged.AddListener(OnValueChanged);
		inputField.onEndEdit.AddListener(OnFieldInput);
		inputField.onSelect.AddListener(AddInputFieldLock);
		GameEvents.onPartActionNumericSlider.Add(ToggleNumericSlider);
		ToggleNumericSlider(GameSettings.PAW_NUMERIC_SLIDERS);
		window.usingNumericValue = true;
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionNumericSlider.Remove(ToggleNumericSlider);
	}

	public void OnValueChanged(float obj)
	{
		if (handlingChange)
		{
			return;
		}
		handlingChange = true;
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float b = progBarControl.maxValue - progBarControl.minValue;
			lerpedValue = Mathf.Lerp(0f, b, slider.value);
			moddedValue = lerpedValue % progBarControl.stepIncrement;
			float num = fieldValue;
			if (moddedValue != 0f)
			{
				if (moddedValue < progBarControl.stepIncrement * 0.5f)
				{
					fieldValue = lerpedValue - moddedValue;
				}
				else
				{
					fieldValue = lerpedValue + (progBarControl.stepIncrement - moddedValue);
				}
			}
			else
			{
				fieldValue = lerpedValue;
			}
			fieldValue += progBarControl.minValue;
			slider.value = Mathf.InverseLerp(progBarControl.minValue, progBarControl.maxValue, fieldValue);
			fieldValue = (float)Math.Round(fieldValue, 5);
			if (Mathf.Abs(fieldValue - num) > progBarControl.stepIncrement * 0.98f)
			{
				SetFieldValue(fieldValue);
			}
		}
		handlingChange = false;
	}

	public float GetFieldValue()
	{
		return field.GetValue<float>(field.host);
	}

	public void SetSliderValue(float rawValue)
	{
		slider.value = Mathf.Clamp01((rawValue - progBarControl.minValue) / (progBarControl.maxValue - progBarControl.minValue));
	}

	public void OnFieldInput(string input)
	{
		float result = GetFieldValue();
		if (float.TryParse(input, out result))
		{
			slider.value = Mathf.Clamp01((result - progBarControl.minValue) / (progBarControl.maxValue - progBarControl.minValue));
			inputField.text = GetFieldValue().ToString();
		}
		RemoveInputfieldLock();
	}

	public void ToggleNumericSlider(bool numeric)
	{
		sliderBackground.SetActive(!numeric);
		numericContainer.SetActive(numeric);
		fieldNameNumeric.gameObject.SetActive(numeric);
		if (inputField.gameObject.activeInHierarchy)
		{
			inputField.text = GetFieldValue().ToString();
		}
	}

	public override void UpdateItem()
	{
		fieldValue = GetFieldValue();
		fieldName.text = field.guiName;
		fieldAmount.text = KSPUtil.LocalizeNumber(fieldValue, field.guiFormat) + field.guiUnits;
		fieldNameNumeric.text = field.guiName;
		if (!inputField.isFocused)
		{
			inputField.text = fieldValue.ToString();
		}
		SetSliderValue(fieldValue);
	}
}
