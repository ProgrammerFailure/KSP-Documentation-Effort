using UnityEngine;

namespace CameraKeyFrameEvents;

public class Fade : CameraKeyFrameEvent
{
	public float fadeDuration = 1f;

	public Color T0 = Color.black;

	public Color T1 = Color.clear;

	public CameraFade fadeController;

	public override void RunEvent()
	{
		if (fadeController != null)
		{
			fadeController.SetScreenOverlayColor(T0);
			fadeController.StartFade(T1, fadeDuration);
		}
	}
}
