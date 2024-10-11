using System.Runtime.CompilerServices;

namespace CameraKeyFrameEvents;

public class SceneTransition : CameraKeyFrameEvent
{
	public GameScenes destination;

	public bool Test;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SceneTransition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunEvent()
	{
		throw null;
	}
}
