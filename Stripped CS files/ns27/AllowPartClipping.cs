using ns26;

namespace ns27;

public class AllowPartClipping : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.AllowPartClipping);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.AllowPartClipping = state;
	}
}
