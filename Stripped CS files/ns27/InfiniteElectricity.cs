using ns26;

namespace ns27;

public class InfiniteElectricity : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.InfiniteElectricity);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.InfiniteElectricity = state;
	}
}
