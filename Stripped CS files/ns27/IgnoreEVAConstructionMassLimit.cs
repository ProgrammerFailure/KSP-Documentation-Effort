using ns26;

namespace ns27;

public class IgnoreEVAConstructionMassLimit : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.IgnoreEVAConstructionMassLimit);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.IgnoreEVAConstructionMassLimit = state;
	}
}
