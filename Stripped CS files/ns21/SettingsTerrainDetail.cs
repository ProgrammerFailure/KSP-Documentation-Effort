using ns20;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsTerrainDetail : SettingsControlBase
{
	public Slider slider;

	public TextMeshProUGUI valueText;

	[SettingsValue(new int[] { 0, 1 })]
	public int[] values = new int[2] { 0, 1 };

	[SettingsValue(new string[] { "A", "B" })]
	public string[] stringValues = new string[2] { "A", "B" };

	[SettingsValue(false)]
	public bool displayStringValue;

	public int value;

	public void Start()
	{
		if (PQSCache.Instance != null)
		{
			slider.onValueChanged.AddListener(OnSliderValueChange);
			slider.minValue = 0f;
			slider.maxValue = PQSCache.PresetList.presets.Count - 1;
			slider.wholeNumbers = true;
			value = PQSCache.PresetList.presetIndex;
			if (displayStringValue)
			{
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
			if (displayStringValue)
			{
				valueText.text = stringValues[value];
			}
			else
			{
				valueText.text = PQSCache.PresetList.presets[value].name;
			}
			slider.value = value;
		}
		else
		{
			valueText.text = "PQSCache null";
		}
	}

	public void OnSliderValueChange(float newValue)
	{
		if (!(PQSCache.Instance == null))
		{
			value = Mathf.FloorToInt(newValue);
			if (displayStringValue)
			{
				valueText.text = stringValues[value];
			}
			else
			{
				valueText.text = PQSCache.PresetList.presets[value].name;
			}
		}
	}

	public override void OnApply()
	{
		if (!(PQSCache.Instance == null))
		{
			PQSCache.PresetList.SetPreset(value);
			GameEvents.OnScenerySettingChanged.Fire();
		}
	}

	public override void OnRevert()
	{
		if (!(PQSCache.Instance == null))
		{
			slider.value = value;
			if (displayStringValue)
			{
				valueText.text = stringValues[value];
			}
			else
			{
				valueText.text = PQSCache.PresetList.presets[value].name;
			}
		}
	}
}
