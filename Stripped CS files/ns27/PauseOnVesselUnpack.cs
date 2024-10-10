using ns26;

namespace ns27;

public class PauseOnVesselUnpack : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.PauseOnVesselUnpack);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.PauseOnVesselUnpack = state;
	}
}
