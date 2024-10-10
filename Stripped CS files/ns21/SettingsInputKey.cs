using System;
using ns20;
using ns9;
using TMPro;
using UnityEngine.UI;

namespace ns21;

public class SettingsInputKey : SettingsControlReflection
{
	public Button buttonPrimary;

	public TextMeshProUGUI buttonPrimaryText;

	public Button buttonSecondary;

	public TextMeshProUGUI buttonSecondaryText;

	public EventData<object, string, SettingsInputBinding.BindingType, SettingsInputBinding.BindingVariant> OnTryBind = new EventData<object, string, SettingsInputBinding.BindingType, SettingsInputBinding.BindingVariant>("OnTryBind");

	public KeyBinding KeyBinding => base.Value as KeyBinding;

	public bool useModes { get; set; }

	public override void OnStart()
	{
		buttonPrimary.onClick.AddListener(OnClickPrimary);
		buttonSecondary.onClick.AddListener(OnClickSecondary);
	}

	public override void ValueInitialized()
	{
	}

	public override void ValueUpdated()
	{
		buttonPrimaryText.text = ((KeyBinding.primary.ToString() == "None") ? Localizer.Format("#autoLOC_6003000") : SettingsScreen.Instance.GetKeyLayoutMap(KeyBinding.primary.ToString()).ToString());
		buttonSecondaryText.text = ((KeyBinding.secondary.ToString() == "None") ? Localizer.Format("#autoLOC_6003000") : SettingsScreen.Instance.GetKeyLayoutMap(KeyBinding.secondary.ToString()).ToString());
	}

	public override void GetValue()
	{
		object obj = accessor.Value;
		if (SettingsLayoutConfig.LayoutConfig != null)
		{
			ConfigNode node = SettingsLayoutConfig.LayoutConfig.GetNode(settingName);
			if (node != null)
			{
				KeyBinding keyBinding = new KeyBinding();
				keyBinding.Load(node);
				base.Value = keyBinding;
			}
			else
			{
				base.Value = obj;
			}
		}
		else if (obj != null && obj is ICloneable)
		{
			base.Value = (obj as ICloneable).Clone();
		}
		else
		{
			base.Value = obj;
		}
	}

	public void OnClickPrimary()
	{
		string data = Localizer.Format(titleText.text);
		OnTryBind.Fire(base.Value, data, SettingsInputBinding.BindingType.Key, SettingsInputBinding.BindingVariant.Primary);
	}

	public void OnClickSecondary()
	{
		string data = Localizer.Format(titleText.text);
		OnTryBind.Fire(base.Value, data, SettingsInputBinding.BindingType.Key, SettingsInputBinding.BindingVariant.Secondary);
	}

	public void UpdateInputSettings()
	{
		ValueUpdated();
	}
}
