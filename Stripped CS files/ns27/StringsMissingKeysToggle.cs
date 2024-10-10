using ns26;
using ns9;

namespace ns27;

public class StringsMissingKeysToggle : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(Localizer.debugWriteMissingKeysToLog);
	}

	public override void OnToggleChanged(bool state)
	{
		Localizer.debugWriteMissingKeysToLog = state;
	}
}
