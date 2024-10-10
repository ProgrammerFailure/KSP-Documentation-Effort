using ns20;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsIntRange : SettingsControlReflection
{
	[SettingsValue(0)]
	public int minValue;

	[SettingsValue(1)]
	public int maxValue;

	public Slider slider;

	public override void OnStart()
	{
		slider.onValueChanged.AddListener(OnSliderValueChange);
		slider.minValue = minValue;
		slider.maxValue = maxValue;
		slider.wholeNumbers = true;
	}

	public void OnSliderValueChange(float newValue)
	{
		base.Value = Mathf.FloorToInt(newValue);
	}

	public override void ValueInitialized()
	{
		slider.value = (int)base.Value;
	}

	public override void ValueUpdated()
	{
		valueText.text = ((int)base.Value).ToString();
	}
}
