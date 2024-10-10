using KSPAssets.KSPedia;
using UnityEngine;

namespace ns11;

public class KSPediaApp : UIApp
{
	public override void DisplayApp()
	{
		KSPediaSpawner.Show(base.appLauncherButton);
	}

	public override void HideApp()
	{
		KSPediaSpawner.Hide();
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.ALWAYS;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return new Vector3(ApplicationLauncher.Instance.transform.position.x, defaultAnchorPos.y, defaultAnchorPos.z);
	}

	public override bool OnAppAboutToStart()
	{
		return true;
	}

	public override void OnAppDestroy()
	{
	}

	public override void OnAppInitialized()
	{
	}
}
