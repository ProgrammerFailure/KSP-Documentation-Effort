namespace Expansions.Missions.Editor;

public class MEGUI_TextArea : MEGUI_Control
{
	public int CharacterLimit;

	public InputContentType ContentType;

	public MEGUI_TextArea()
	{
		CharacterLimit = 0;
		ContentType = InputContentType.Standard;
	}
}
