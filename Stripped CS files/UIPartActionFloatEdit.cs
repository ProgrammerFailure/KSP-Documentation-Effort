using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_FloatEdit]
public class UIPartActionFloatEdit : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle incLarge;

	public UIButtonToggle incSmall;

	public UIButtonToggle decLarge;

	public UIButtonToggle decSmall;

	public Slider slider;

	public bool blockSliderUpdate;

	public UI_FloatEdit floatControl => (UI_FloatEdit)control;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		float value = GetFieldValue();
		value = Clamp(value);
		value = UpdateSlider(value);
		SetFieldValue(value);
		UpdateDisplay(value, null);
		fieldName.text = field.guiName;
		incLarge.onToggle.AddListener(OnTap_incLarge);
		incSmall.onToggle.AddListener(OnTap_incSmall);
		decLarge.onToggle.AddListener(OnTap_decLarge);
		decSmall.onToggle.AddListener(OnTap_decSmall);
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public float GetFieldValue()
	{
		return field.GetValue<float>(field.host);
	}

	public float UpdateSlider(float value)
	{
		if (floatControl.incrementSlide != 0f)
		{
			value = Mathf.Round(value / floatControl.incrementSlide) * floatControl.incrementSlide;
		}
		return value;
	}

	public float IntervalBase(float value, float increment)
	{
		float num = Mathf.Floor((value + floatControl.incrementSlide / 2f) / increment) * increment;
		if (num > floatControl.maxValue - increment)
		{
			num = floatControl.maxValue - increment;
		}
		return num;
	}

	public void SliderInterval(float value, out float min, out float max)
	{
		if (floatControl.incrementLarge == 0f)
		{
			min = floatControl.minValue;
			max = floatControl.maxValue;
		}
		else if (floatControl.incrementSmall == 0f)
		{
			min = IntervalBase(value, floatControl.incrementLarge);
			max = min + floatControl.incrementLarge;
		}
		else
		{
			min = IntervalBase(value, floatControl.incrementSmall);
			max = min + floatControl.incrementSmall;
		}
		min = Mathf.Max(min, floatControl.minValue);
		max = Mathf.Min(max, floatControl.maxValue);
	}

	public void UpdateControlStates()
	{
		RectTransform component = slider.gameObject.GetComponent<RectTransform>();
		Vector2 sizeDelta = default(Vector2);
		sizeDelta.y = 0f;
		bool active;
		bool active2;
		if (floatControl.incrementLarge == 0f)
		{
			active = false;
			active2 = false;
			sizeDelta.x = 0f;
		}
		else if (floatControl.incrementSmall == 0f)
		{
			active = true;
			active2 = false;
			sizeDelta.x = -44f;
		}
		else
		{
			active = true;
			active2 = true;
			sizeDelta.x = -76f;
		}
		incLarge.gameObject.SetActive(active);
		decLarge.gameObject.SetActive(active);
		incSmall.gameObject.SetActive(active2);
		decSmall.gameObject.SetActive(active2);
		if (component.sizeDelta.x != sizeDelta.x)
		{
			component.sizeDelta = sizeDelta;
		}
	}

	public void UpdateDisplay(float value, UIButtonToggle button)
	{
		string unit = floatControl.unit;
		int sigFigs = floatControl.sigFigs;
		string text = ((!floatControl.useSI) ? (KSPUtil.LocalizeNumber(value, "F" + sigFigs) + unit) : KSPUtil.PrintSI(value, unit, sigFigs));
		fieldValue.text = text;
		SliderInterval(value, out var min, out var max);
		blockSliderUpdate = true;
		slider.minValue = min;
		slider.maxValue = max;
		slider.value = value;
		blockSliderUpdate = false;
		if ((bool)button)
		{
			button.SetState(on: false);
		}
		UpdateControlStates();
	}

	public float Clamp(float value)
	{
		value = Mathf.Min(value, floatControl.maxValue);
		value = Mathf.Max(value, floatControl.minValue);
		return value;
	}

	public float AdjustValue(float value, bool up, float increment)
	{
		if (increment == 0f)
		{
			return value;
		}
		float num = value % increment;
		if (num < 0f)
		{
			num += increment;
		}
		value -= num;
		if (up)
		{
			value += increment;
			if (increment - num < floatControl.incrementSlide / 2f)
			{
				value += increment;
			}
		}
		else if (num < floatControl.incrementSlide / 2f)
		{
			value -= increment;
		}
		value = Clamp(value);
		return value;
	}

	public void OnTap_incLarge()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = GetFieldValue();
			value = AdjustValue(value, up: true, floatControl.incrementLarge);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}

	public void OnTap_incSmall()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = GetFieldValue();
			value = AdjustValue(value, up: true, floatControl.incrementSmall);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}

	public void OnTap_decLarge()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = GetFieldValue();
			value = AdjustValue(value, up: false, floatControl.incrementLarge);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}

	public void OnTap_decSmall()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = GetFieldValue();
			value = AdjustValue(value, up: false, floatControl.incrementSmall);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}

	public void OnValueChanged(float obj)
	{
		if (!blockSliderUpdate && InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			float value = slider.value;
			value = UpdateSlider(value);
			UpdateDisplay(value, null);
			SetFieldValue(value);
		}
	}
}
