using ns26;
using ns9;

namespace ns27;

public class StringsOverrideMELock : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(Localizer.OverrideMELock);
	}

	public override void OnToggleChanged(bool state)
	{
		Localizer.OverrideMELock = state;
		GameEvents.Mission.onLocalizationLockOverriden.Fire();
	}
}
