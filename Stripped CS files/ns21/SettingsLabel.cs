using ns20;
using UnityEngine;

namespace ns21;

public class SettingsLabel : SettingsControlReflection
{
	[SettingsValue("#FFE805FF")]
	public Color textColor;

	[SettingsValue(14)]
	public int fontSize;

	public override void OnStart()
	{
		base.IgnoreEmptySetting = true;
		titleText.color = textColor;
		titleText.fontSize = fontSize;
	}
}
