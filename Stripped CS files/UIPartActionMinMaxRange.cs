using System;
using TMPro;
using UnityEngine;

[UI_MinMaxRange]
public class UIPartActionMinMaxRange : UIPartActionFieldItem
{
	public GameObject slidersContainer;

	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldAmount;

	public GameObject numericContainer;

	public TextMeshProUGUI fieldNameNumeric;

	public TMP_InputField inputFieldMin;

	public TMP_InputField inputFieldMax;

	public DoubleSlider slider;

	public Vector2 fieldValue = Vector2.zero;

	public float lerpedValueMin;

	public float moddedValueMin;

	public float lerpedValueMax;

	public float moddedValueMax;

	public bool handlingChange;

	public UI_MinMaxRange progBarControl => (UI_MinMaxRange)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		SetSliderValue(GetFieldValue());
		DoubleSlider doubleSlider = slider;
		doubleSlider.onValueChanged = (DoubleSlider.OnValueChanged)Delegate.Combine(doubleSlider.onValueChanged, new DoubleSlider.OnValueChanged(OnValueChanged));
		inputFieldMin.onEndEdit.AddListener(SetNumericMinValue);
		inputFieldMax.onEndEdit.AddListener(SetNumericMaxValue);
		inputFieldMin.onSelect.AddListener(AddInputFieldLock);
		inputFieldMax.onSelect.AddListener(AddInputFieldLock);
		GameEvents.onPartActionNumericSlider.Add(ToggleNumericSlider);
		ToggleNumericSlider(GameSettings.PAW_NUMERIC_SLIDERS);
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionNumericSlider.Remove(ToggleNumericSlider);
	}

	public void OnValueChanged(float minValue, float maxValue)
	{
		if (handlingChange)
		{
			return;
		}
		handlingChange = true;
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float b = progBarControl.maxValueX - progBarControl.minValueX;
			float b2 = progBarControl.maxValueY - progBarControl.minValueY;
			lerpedValueMin = Mathf.Lerp(0f, b, slider.sliderMin.value);
			moddedValueMin = lerpedValueMin % progBarControl.stepIncrement;
			lerpedValueMax = Mathf.Lerp(0f, b2, slider.sliderMax.value);
			moddedValueMax = lerpedValueMax % progBarControl.stepIncrement;
			float x = fieldValue.x;
			if (moddedValueMin != 0f)
			{
				if (moddedValueMin < progBarControl.stepIncrement * 0.5f)
				{
					fieldValue.x = lerpedValueMin - moddedValueMin;
				}
				else
				{
					fieldValue.x = lerpedValueMin + (progBarControl.stepIncrement - moddedValueMin);
				}
			}
			else
			{
				fieldValue.x = lerpedValueMin;
			}
			fieldValue.x += progBarControl.minValueX;
			slider.sliderMin.value = Mathf.InverseLerp(progBarControl.minValueX, progBarControl.maxValueX, fieldValue.x);
			fieldValue.x = (float)Math.Round(fieldValue.x, 5);
			float y = fieldValue.y;
			if (moddedValueMax != 0f)
			{
				if (moddedValueMax < progBarControl.stepIncrement * 0.5f)
				{
					fieldValue.y = lerpedValueMax - moddedValueMax;
				}
				else
				{
					fieldValue.y = lerpedValueMax + (progBarControl.stepIncrement - moddedValueMax);
				}
			}
			else
			{
				fieldValue.y = lerpedValueMax;
			}
			fieldValue.y += progBarControl.minValueY;
			slider.sliderMax.value = Mathf.InverseLerp(progBarControl.minValueY, progBarControl.maxValueY, fieldValue.y);
			fieldValue.y = (float)Math.Round(fieldValue.y, 5);
			if (Mathf.Abs(fieldValue.y - y) > progBarControl.stepIncrement * 0.98f || Mathf.Abs(fieldValue.x - x) > progBarControl.stepIncrement * 0.98f)
			{
				SetFieldValue(fieldValue);
			}
		}
		handlingChange = false;
	}

	public Vector2 GetFieldValue()
	{
		return field.GetValue<Vector2>(field.host);
	}

	public void SetSliderValue(Vector2 rawValue)
	{
		slider.sliderMin.value = Mathf.Clamp01((rawValue.x - progBarControl.minValueX) / (progBarControl.maxValueX - progBarControl.minValueX));
		slider.sliderMax.value = Mathf.Clamp01((rawValue.y - progBarControl.minValueY) / (progBarControl.maxValueY - progBarControl.minValueY));
	}

	public override void UpdateItem()
	{
		fieldValue = GetFieldValue();
		fieldName.text = field.guiName;
		fieldAmount.text = KSPUtil.LocalizeNumber(fieldValue.x, field.guiFormat) + field.guiUnits + " , " + KSPUtil.LocalizeNumber(fieldValue.y, field.guiFormat) + field.guiUnits;
		fieldNameNumeric.text = field.guiName;
		if (!inputFieldMin.isFocused)
		{
			inputFieldMin.text = KSPUtil.LocalizeNumber(fieldValue.x, field.guiFormat);
		}
		if (!inputFieldMax.isFocused)
		{
			inputFieldMax.text = KSPUtil.LocalizeNumber(fieldValue.y, field.guiFormat);
		}
		SetSliderValue(fieldValue);
	}

	public void SetNumericMinValue(string input)
	{
		float result = fieldValue.x;
		if (float.TryParse(input, out result))
		{
			result = Mathf.Clamp(result, progBarControl.minValueX, fieldValue.y - progBarControl.stepIncrement);
			fieldValue.x = result;
			SetFieldValue(fieldValue);
			inputFieldMin.text = KSPUtil.LocalizeNumber(fieldValue.x, field.guiFormat);
		}
		RemoveInputfieldLock();
	}

	public void SetNumericMaxValue(string input)
	{
		float result = fieldValue.y;
		if (float.TryParse(input, out result))
		{
			result = Mathf.Clamp(result, fieldValue.x + progBarControl.stepIncrement, progBarControl.maxValueY);
			fieldValue.y = result;
			SetFieldValue(fieldValue);
			inputFieldMax.text = KSPUtil.LocalizeNumber(fieldValue.y, field.guiFormat);
		}
		RemoveInputfieldLock();
	}

	public void ToggleNumericSlider(bool numeric)
	{
		slidersContainer.SetActive(!numeric);
		numericContainer.SetActive(numeric);
	}
}
