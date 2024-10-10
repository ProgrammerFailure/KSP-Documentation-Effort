using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ColorSlider : MonoBehaviour
{
	public ColorPicker hsvpicker;

	public ColorValues type;

	public Slider slider;

	public bool listen = true;

	public void Start()
	{
		slider = GetComponent<Slider>();
		ColorChanged(hsvpicker.CurrentColor);
		HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(hsvpicker.CurrentColor);
		HSVChanged(hsvColor.normalizedH, hsvColor.normalizedS, hsvColor.normalizedV);
		hsvpicker.onValueChanged.AddListener(ColorChanged);
		hsvpicker.onInternalValueChanged.AddListener(ColorChanged);
		hsvpicker.onHSVChanged.AddListener(HSVChanged);
		slider.onValueChanged.AddListener(SliderChanged);
	}

	public void OnDestroy()
	{
		hsvpicker.onValueChanged.RemoveListener(ColorChanged);
		hsvpicker.onInternalValueChanged.RemoveListener(ColorChanged);
		hsvpicker.onHSVChanged.RemoveListener(HSVChanged);
		if (slider != null)
		{
			slider.onValueChanged.RemoveListener(SliderChanged);
		}
	}

	public void ColorChanged(Color newColor)
	{
		listen = false;
		switch (type)
		{
		case ColorValues.const_0:
			slider.normalizedValue = newColor.r;
			break;
		case ColorValues.const_1:
			slider.normalizedValue = newColor.g;
			break;
		case ColorValues.const_2:
			slider.normalizedValue = newColor.b;
			break;
		case ColorValues.const_3:
			slider.normalizedValue = newColor.a;
			break;
		}
		listen = true;
	}

	public void HSVChanged(float hue, float saturation, float value)
	{
		listen = false;
		switch (type)
		{
		case ColorValues.Hue:
			slider.normalizedValue = hue;
			break;
		case ColorValues.Saturation:
			slider.normalizedValue = saturation;
			break;
		case ColorValues.Value:
			slider.normalizedValue = value;
			break;
		}
		listen = true;
	}

	public void SliderChanged(float newValue)
	{
		if (listen)
		{
			newValue = slider.normalizedValue;
			hsvpicker.AssignColor(type, newValue);
		}
		listen = true;
	}
}
