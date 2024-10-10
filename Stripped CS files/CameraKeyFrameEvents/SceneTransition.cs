using UnityEngine;

namespace CameraKeyFrameEvents;

public class SceneTransition : CameraKeyFrameEvent
{
	public GameScenes destination = GameScenes.MAINMENU;

	public bool Test = true;

	public override void RunEvent()
	{
		if (!Test)
		{
			HighLogic.LoadScene(destination);
			return;
		}
		Debug.Log("=============== Scene Transition to " + destination.ToString() + " ==================");
		Debug.Break();
	}
}
