using ns20;
using ns21;
using UnityEngine;

namespace ns22;

public class SettingsValueIntToString : MonoBehaviour
{
	[SettingsValue(new string[] { "A", "B" })]
	public string[] values = new string[2] { "A", "B" };

	public SettingsControlReflection control;

	public void Awake()
	{
		control = GetComponent<SettingsControlReflection>();
	}

	public void UpdateValue(object value)
	{
		if (control.valueText != null && value is int num && num >= 0 && num < values.Length)
		{
			control.valueText.text = values[num];
		}
	}
}
