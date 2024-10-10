using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns21;

public class SettingsInputAxis : SettingsControlReflection
{
	public Button primaryButton;

	public TextMeshProUGUI primaryButtonText;

	public UIButtonToggle primaryInvert;

	public Slider primarySensitivity;

	public TextMeshProUGUI primarySensitivityText;

	public Slider primaryDeadzone;

	public TextMeshProUGUI primaryDeadzoneText;

	public Button secondaryButton;

	public TextMeshProUGUI secondaryButtonText;

	public UIButtonToggle secondaryInvert;

	public Slider secondarySensitivity;

	public TextMeshProUGUI secondarySensitivityText;

	public Slider secondaryDeadzone;

	public TextMeshProUGUI secondaryDeadzoneText;

	public EventData<object, string, SettingsInputBinding.BindingType, SettingsInputBinding.BindingVariant> OnTryBind = new EventData<object, string, SettingsInputBinding.BindingType, SettingsInputBinding.BindingVariant>("OnTryBind");

	public AxisBinding axis;

	public override void OnStart()
	{
		primaryButton.onClick.AddListener(OnClickPrimary);
		primaryInvert.onToggle.AddListener(OnTogglePrimaryInvert);
		primarySensitivity.onValueChanged.AddListener(OnChangePrimarySensitivity);
		primaryDeadzone.onValueChanged.AddListener(OnChangePrimaryDeadzone);
		secondaryButton.onClick.AddListener(OnClickSecondary);
		secondaryInvert.onToggle.AddListener(OnToggleSecondaryInvert);
		secondarySensitivity.onValueChanged.AddListener(OnChangeSecondarySensitivity);
		secondaryDeadzone.onValueChanged.AddListener(OnChangeSecondaryDeadzone);
	}

	public override void ValueInitialized()
	{
		axis = (AxisBinding)base.Value;
	}

	public override void ValueUpdated()
	{
		if (axis.primary.deviceIdx != -1)
		{
			primaryButtonText.text = axis.primary.idTag;
		}
		else
		{
			primaryButtonText.text = "<";
		}
		primaryInvert.state = axis.primary.inverted;
		primarySensitivity.value = 1f - Mathf.InverseLerp(GameSettings.AxisSensitivityMin, GameSettings.AxisSensitivityMax, axis.primary.sensitivity);
		primarySensitivityText.text = KSPUtil.LocalizeNumber(primarySensitivity.value, "F2");
		primaryDeadzone.value = axis.primary.deadzone;
		primaryDeadzoneText.text = KSPUtil.LocalizeNumber(axis.primary.deadzone, "F2");
		if (axis.secondary.deviceIdx != -1)
		{
			secondaryButtonText.text = axis.secondary.idTag;
		}
		else
		{
			secondaryButtonText.text = "<";
		}
		secondaryInvert.state = axis.secondary.inverted;
		secondarySensitivity.value = 1f - Mathf.InverseLerp(GameSettings.AxisSensitivityMin, GameSettings.AxisSensitivityMax, axis.secondary.sensitivity);
		secondarySensitivityText.text = KSPUtil.LocalizeNumber(secondarySensitivity.value, "F2");
		secondaryDeadzone.value = axis.secondary.deadzone;
		secondaryDeadzoneText.text = KSPUtil.LocalizeNumber(axis.secondary.deadzone, "F2");
	}

	public void OnClickPrimary()
	{
		string data = Localizer.Format(titleText.text);
		OnTryBind.Fire(base.Value, data, SettingsInputBinding.BindingType.Axis, SettingsInputBinding.BindingVariant.Primary);
	}

	public void OnTogglePrimaryInvert()
	{
		axis.primary.inverted = primaryInvert.state;
	}

	public void OnChangePrimarySensitivity(float value)
	{
		primarySensitivityText.text = KSPUtil.LocalizeNumber(value, "F2");
		axis.primary.sensitivity = Mathf.Lerp(GameSettings.AxisSensitivityMin, GameSettings.AxisSensitivityMax, 1f - value);
	}

	public void OnChangePrimaryDeadzone(float value)
	{
		primaryDeadzoneText.text = KSPUtil.LocalizeNumber(value, "F2");
		axis.primary.deadzone = value;
	}

	public void OnClickSecondary()
	{
		string data = Localizer.Format(titleText.text);
		OnTryBind.Fire(base.Value, data, SettingsInputBinding.BindingType.Axis, SettingsInputBinding.BindingVariant.Secondary);
	}

	public void OnToggleSecondaryInvert()
	{
		axis.secondary.inverted = secondaryInvert.state;
	}

	public void OnChangeSecondarySensitivity(float value)
	{
		secondarySensitivityText.text = KSPUtil.LocalizeNumber(value, "F2");
		axis.secondary.sensitivity = Mathf.Lerp(GameSettings.AxisSensitivityMin, GameSettings.AxisSensitivityMax, 1f - value);
	}

	public void OnChangeSecondaryDeadzone(float value)
	{
		secondaryDeadzoneText.text = KSPUtil.LocalizeNumber(value, "F2");
		axis.secondary.deadzone = value;
	}

	public void UpdateInputSettings()
	{
		ValueUpdated();
	}
}
