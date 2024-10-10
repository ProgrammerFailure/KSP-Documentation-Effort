using ns26;

namespace ns27;

public class IgnoreMaxTemperature : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.IgnoreMaxTemperature);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.IgnoreMaxTemperature = state;
	}
}
