namespace CameraKeyFrameEvents;

public class TextHeaderFlip : CameraKeyFrameEvent
{
	public CreditsMultiTextHeader headerRef;

	public override void RunEvent()
	{
		if (headerRef != null)
		{
			headerRef.NextText();
		}
	}
}
