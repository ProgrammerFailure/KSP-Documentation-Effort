using ns20;
using UnityEngine.UI;

namespace ns21;

public class SettingsIntButtons : SettingsControlReflection
{
	public Button buttonUp;

	public Button buttonDown;

	[SettingsValue(0)]
	public int minValue;

	[SettingsValue(1)]
	public int maxValue;

	public override void OnStart()
	{
		buttonUp.onClick.AddListener(OnButtonUp);
		buttonDown.onClick.AddListener(OnButtonDown);
	}

	public void OnButtonUp()
	{
		int num = (int)base.Value;
		if (num < maxValue)
		{
			base.Value = num + 1;
		}
	}

	public void OnButtonDown()
	{
		int num = (int)base.Value;
		if (num > minValue)
		{
			base.Value = num - 1;
		}
	}
}
