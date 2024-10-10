using ns20;
using ns21;
using UnityEngine;

namespace ns22;

public class SettingsValueBoolTitleChange : MonoBehaviour
{
	[SettingsValue("True")]
	public string titleEnabled = "True";

	[SettingsValue("False")]
	public string titleDisabled = "False";

	public SettingsControlReflection control;

	public void Awake()
	{
		control = GetComponent<SettingsControlReflection>();
	}

	public void UpdateValue(object value)
	{
		if (value is bool && control.titleText != null)
		{
			control.titleText.text = (((bool)value) ? titleEnabled : titleDisabled);
		}
	}
}
