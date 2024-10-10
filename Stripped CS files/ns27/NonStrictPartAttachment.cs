using ns26;

namespace ns27;

public class NonStrictPartAttachment : DebugScreenToggle
{
	public override void SetupValues()
	{
		SetToggle(CheatOptions.NonStrictAttachmentOrientation);
	}

	public override void OnToggleChanged(bool state)
	{
		CheatOptions.NonStrictAttachmentOrientation = state;
	}
}
