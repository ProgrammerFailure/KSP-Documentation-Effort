namespace Expansions.Missions.Editor;

public class MEGUI_InputField : MEGUI_Control
{
	public int CharacterLimit;

	public InputContentType ContentType;

	public MEGUI_InputField()
	{
		CharacterLimit = 0;
		ContentType = InputContentType.Standard;
	}
}
