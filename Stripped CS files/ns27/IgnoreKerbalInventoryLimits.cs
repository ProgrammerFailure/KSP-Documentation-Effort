using ns26;

namespace ns27;

public class IgnoreKerbalInventoryLimits : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.IgnoreKerbalInventoryLimits);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.IgnoreKerbalInventoryLimits = state;
	}
}
