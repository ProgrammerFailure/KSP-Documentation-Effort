using System;
using ns2;
using TMPro;
using UnityEngine.UI;

[UI_ScaleEdit]
public class UIPartActionScaleEdit : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle inc;

	public UIButtonToggle dec;

	public Slider slider;

	public int intervalIndex;

	public UI_ScaleEdit scaleControl => (UI_ScaleEdit)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		float value = GetFieldValue();
		intervalIndex = FindInterval(value);
		value = UpdateSlider(value);
		UpdateDisplay(value, null);
		fieldName.text = field.guiName;
		inc.onToggle.AddListener(OnTap_inc);
		dec.onToggle.AddListener(OnTap_dec);
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public float GetFieldValue()
	{
		return field.GetValue<float>(field.host);
	}

	public int FindInterval(float value)
	{
		int result = 0;
		for (int i = 0; i < scaleControl.intervals.Length - 2; i++)
		{
			if (value >= scaleControl.intervals[i])
			{
				result = i;
			}
		}
		return result;
	}

	public float GetStep()
	{
		int num = scaleControl.incrementSlide.Length;
		int num2 = Math.Min(intervalIndex, num - 1);
		if (num2 >= 0)
		{
			return scaleControl.incrementSlide[num2];
		}
		return 0f;
	}

	public float UpdateSlider(float value)
	{
		float num = scaleControl.intervals[intervalIndex];
		float num2 = scaleControl.intervals[intervalIndex + 1];
		float step = GetStep();
		if (step > 0f)
		{
			value = num + (float)Math.Round((value - num) / step) * step;
		}
		value = Math.Max(num, Math.Min(value, num2));
		slider.maxValue = num2;
		slider.minValue = num;
		slider.value = value;
		return value;
	}

	public void UpdateDisplay(float value, UIButtonToggle button)
	{
		string unit = scaleControl.unit;
		int sigFigs = scaleControl.sigFigs;
		string text = ((!scaleControl.useSI) ? (KSPUtil.LocalizeNumber(value, "F" + sigFigs) + unit) : KSPUtil.PrintSI(value, unit, sigFigs));
		fieldValue.text = text;
		if ((bool)button)
		{
			button.SetState(on: false);
		}
	}

	public void UpdateInterval(bool up, UIButtonToggle button)
	{
		float num = GetFieldValue();
		if (up)
		{
			if (intervalIndex < scaleControl.intervals.Length - 2)
			{
				if (num == scaleControl.intervals[intervalIndex + 1])
				{
					num = scaleControl.intervals[intervalIndex + 2];
				}
				intervalIndex++;
			}
		}
		else if (intervalIndex > 0)
		{
			if (num == scaleControl.intervals[intervalIndex])
			{
				num = scaleControl.intervals[intervalIndex - 1];
			}
			intervalIndex--;
		}
		num = UpdateSlider(num);
		UpdateDisplay(num, button);
		SetFieldValue(num);
	}

	public void OnTap_inc()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			UpdateInterval(up: true, inc);
		}
	}

	public void OnTap_dec()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			UpdateInterval(up: false, dec);
		}
	}

	public void OnValueChanged(float obj)
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = slider.value;
			value = UpdateSlider(value);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}
}
