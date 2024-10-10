using ns2;
using ns20;

namespace ns21;

public class SettingsBoolToggle : SettingsControlReflection
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
}
