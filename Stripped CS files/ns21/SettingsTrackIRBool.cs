using System;
using ns2;
using ns20;
using UnityEngine;

namespace ns21;

public class SettingsTrackIRBool : SettingsControlReflection
{
	public UIButtonToggle toggle;

	[SettingsValue("True")]
	public string valueEnabled = "";

	[SettingsValue("False")]
	public string valueDisabled = "";

	public override void OnStart()
	{
		toggle.onToggle.AddListener(OnToggled);
	}

	public void OnToggled()
	{
		base.Value = toggle.state;
	}

	public override void ValueInitialized()
	{
		toggle.SetState((bool)base.Value);
		EnableInteraction(GameSettings.TRACKIR_ENABLED);
	}

	public override void ValueUpdated()
	{
		if (valueText == null)
		{
			return;
		}
		if ((bool)base.Value)
		{
			if (!string.IsNullOrEmpty(valueEnabled))
			{
				valueText.text = valueEnabled;
			}
		}
		else if (!string.IsNullOrEmpty(valueEnabled))
		{
			valueText.text = valueDisabled;
		}
	}

	public override AccessorBase GetAccessor(string settingName)
	{
		try
		{
			AccessorBase accessorBase = AccessorBase.Create(typeof(TrackIR), TrackIR.Instance, settingName);
			if (accessorBase == null)
			{
				Debug.LogError("SettingControl '" + titleText.text + "': Cannot find TrackIR field named '" + settingName + "'", base.gameObject);
				return null;
			}
			return accessorBase;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		return null;
	}

	public override void GetValue()
	{
		if (TrackIR.Instance != null)
		{
			base.Value = accessor.Value;
		}
		else
		{
			base.Value = false;
		}
	}

	public override void SetValue()
	{
		if (TrackIR.Instance != null)
		{
			accessor.Value = base.Value;
		}
	}

	public void SettingChanged(string settingString)
	{
		if (settingString.StartsWith("TRACKIR_ENABLED"))
		{
			EnableInteraction(settingString.EndsWith("True"));
		}
	}

	public void EnableInteraction(bool enabled)
	{
		if (enabled)
		{
			toggle.interactable = true;
			valueText.color = Color.white;
		}
		else
		{
			toggle.interactable = false;
			valueText.color = Color.grey;
		}
	}
}
