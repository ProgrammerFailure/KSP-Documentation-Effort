using ns20;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsAxisScale : SettingsControlReflection
{
	[SettingsValue(true)]
	public bool isPrimaryAxis = true;

	[SettingsValue(0f)]
	public float minValue;

	[SettingsValue(1f)]
	public float maxValue = 1f;

	[SettingsValue(1f)]
	public float displayMultiply = 1f;

	[SettingsValue("F0")]
	public string displayFormat = "F0";

	[SettingsValue("")]
	public string displayUnits = "";

	[SettingsValue(-1)]
	public int roundToPlaces = -1;

	public Slider slider;

	public float axisSensitivity;

	public override void OnStart()
	{
		slider.onValueChanged.AddListener(SliderValueChange);
		slider.minValue = minValue;
		slider.maxValue = maxValue;
	}

	public void SliderValueChange(float newValue)
	{
		if (roundToPlaces > -1)
		{
			newValue = ((roundToPlaces != 0) ? (Mathf.Round(newValue * Mathf.Pow(10f, roundToPlaces)) / Mathf.Pow(10f, roundToPlaces)) : Mathf.Round(newValue));
		}
		axisSensitivity = newValue;
		ValueUpdated();
	}

	public override void ValueInitialized()
	{
		if (isPrimaryAxis)
		{
			axisSensitivity = (base.Value as AxisBinding).primary.scale;
		}
		else
		{
			axisSensitivity = (base.Value as AxisBinding).secondary.scale;
		}
		slider.value = axisSensitivity;
	}

	public override void ValueUpdated()
	{
		if (valueText != null)
		{
			valueText.text = (axisSensitivity * displayMultiply).ToString(displayFormat) + displayUnits;
		}
	}

	public override void PreApply()
	{
		if (isPrimaryAxis)
		{
			(base.Value as AxisBinding).primary.scale = axisSensitivity;
		}
		else
		{
			(base.Value as AxisBinding).secondary.scale = axisSensitivity;
		}
	}
}
