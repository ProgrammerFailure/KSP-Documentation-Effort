using UnityEngine.UI;

namespace ns21;

public class SettingsKeyboardLayoutApply : SettingsControlBase
{
	public Button buttonApply;

	public void Start()
	{
		if (GameSettings.KeyboardLayouts.Count == 0)
		{
			buttonApply.interactable = false;
		}
	}
}
