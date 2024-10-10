using ns26;

namespace ns27;

public class UnbreakableJoints : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.UnbreakableJoints);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.UnbreakableJoints = state;
	}
}
