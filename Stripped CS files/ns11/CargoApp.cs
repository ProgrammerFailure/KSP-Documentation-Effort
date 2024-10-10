using UnityEngine;

namespace ns11;

public class CargoApp : UIApp
{
	public override void DisplayApp()
	{
		if ((bool)EVAConstructionModeController.Instance)
		{
			EVAConstructionModeController.Instance.OpenCargoPanel();
		}
	}

	public override void HideApp()
	{
		if ((bool)EVAConstructionModeController.Instance)
		{
			EVAConstructionModeController.Instance.ClosePanel();
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.FLIGHT;
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
		if ((bool)EVAConstructionModeController.Instance)
		{
			EVAConstructionModeController.Instance.RegisterAppButtonCargo(base.appLauncherButton);
		}
	}
}
