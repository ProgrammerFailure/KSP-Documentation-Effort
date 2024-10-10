using ns26;

namespace ns27;

public class NoCrashDamage : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.NoCrashDamage);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.NoCrashDamage = state;
	}
}
