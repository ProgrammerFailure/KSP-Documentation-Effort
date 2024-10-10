using ns26;
using ns9;

namespace ns27;

public class StringsShowKeysToggle : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(Localizer.ShowKeysOnScreen);
	}

	public override void OnToggleChanged(bool state)
	{
		Localizer.ShowKeysOnScreen = state;
	}
}
