using ns20;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsIntArrayRange : SettingsControlReflection
{
	[SettingsValue(new int[] { 0, 1 })]
	public int[] values = new int[2] { 0, 1 };

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues = new string[2] { "A", "B" };

	[SettingsValue(false)]
	public bool displayStringValue;

	public Slider slider;

	public int currentValue;

	public override void OnStart()
	{
		slider.onValueChanged.AddListener(OnSliderValueChange);
		slider.minValue = 0f;
		slider.maxValue = values.Length - 1;
		slider.wholeNumbers = true;
		if (!displayStringValue)
		{
			return;
		}
		if (stringValues.Length != 0 && stringValues.Length != values.Length)
		{
			string[] array = Localizer.Format(stringValues[0]).Split(',');
			if (array.Length == values.Length)
			{
				stringValues = array;
			}
			else
			{
				displayStringValue = false;
			}
		}
		else if (stringValues.Length != values.Length)
		{
			displayStringValue = false;
		}
	}

	public void OnSliderValueChange(float newValue)
	{
		currentValue = Mathf.RoundToInt(newValue);
		base.Value = values[currentValue];
	}

	public override void ValueInitialized()
	{
		int num = (int)base.Value;
		currentValue = values.IndexOf(num);
		if (currentValue == -1)
		{
			currentValue = 0;
		}
		slider.value = currentValue;
	}

	public override void ValueUpdated()
	{
		if (!(valueText == null))
		{
			if (displayStringValue)
			{
				valueText.text = stringValues[currentValue];
			}
			else
			{
				valueText.text = ((int)base.Value).ToString();
			}
		}
	}
}
