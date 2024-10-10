using ns26;

namespace ns27;

public class InfinitePropellant : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.InfinitePropellant);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.InfinitePropellant = state;
	}
}
