using ns9;
using TMPro;
using UnityEngine;

namespace ns21;

public class SettingsKeyboardLayoutOs : SettingsControlBase
{
	public TextMeshProUGUI keyboardLabel;

	public void Start()
	{
		KeyboardLayout keyboardLayout = KeyboardLayout.GetKeyboardLayout();
		Debug.Log("Locale is: " + keyboardLayout.Locale.DisplayName);
		keyboardLabel.text = Localizer.Format("#autoLOC_6001209", keyboardLayout.Type, keyboardLayout.Locale.NativeName);
	}
}
