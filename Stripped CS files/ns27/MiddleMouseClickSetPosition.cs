using ns26;

namespace ns27;

public class MiddleMouseClickSetPosition : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.MiddleMouseClickSetPosition);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.MiddleMouseClickSetPosition = state;
	}
}
