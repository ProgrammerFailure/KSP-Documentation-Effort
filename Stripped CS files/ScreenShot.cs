using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	public Camera maincamera;

	public int i;

	public bool uiToggle;

	public bool allowUiHidingWithF2;

	public bool ScreenShotCameraMode;

	public bool useConfigSuperSize = true;

	public int superSize;

	public bool listenerAdded;

	public FlightCamera.TargetMode targetMode;

	public Transform target;

	public void Start()
	{
		i = 0;
		if (!Directory.Exists((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../Screenshots") : Path.Combine(Application.dataPath, "../Screenshots")))
		{
			Directory.CreateDirectory((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../Screenshots") : Path.Combine(Application.dataPath, "../Screenshots"));
		}
		if (allowUiHidingWithF2)
		{
			GameEvents.onShowUI.Add(ShowUI);
			GameEvents.onHideUI.Add(HideUI);
			listenerAdded = true;
		}
		if (useConfigSuperSize)
		{
			superSize = GameSettings.SCREENSHOT_SUPERSIZE;
		}
		GameEvents.onGameUnpause.Add(OnGameUnpause);
	}

	public void OnDestroy()
	{
		if (listenerAdded)
		{
			GameEvents.onShowUI.Remove(ShowUI);
			GameEvents.onHideUI.Remove(HideUI);
		}
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Update()
	{
		if (GameSettings.TAKE_SCREENSHOT.GetKeyDown())
		{
			i = 0;
			while (File.Exists(((Application.platform != RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../Screenshots") : Path.Combine(Application.dataPath, "../../Screenshots")) + "/screenshot" + i + ".png"))
			{
				i++;
			}
			ScreenCapture.CaptureScreenshot(((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../Screenshots") : Path.Combine(Application.dataPath, "../Screenshots")) + "/screenshot" + i + ".png", superSize);
			MonoBehaviour.print("SCREENSHOT!!");
		}
		if (!GameSettings.TOGGLE_UI.GetKeyDown() || !allowUiHidingWithF2 || HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			return;
		}
		if (uiToggle)
		{
			OnGameUnpause();
			GameEvents.onShowUI.Fire();
			return;
		}
		if (FlightDriver.Pause && FlightCamera.fetch != null)
		{
			ScreenShotCameraMode = true;
			targetMode = FlightCamera.fetch.targetMode;
			target = FlightCamera.fetch.Target;
			float distance = FlightCamera.fetch.Distance;
			FlightCamera.fetch.SetTarget(target, targetMode);
			FlightCamera.fetch.SetDistanceImmediate(distance);
		}
		GameEvents.onHideUI.Fire();
	}

	public void OnGameUnpause()
	{
		if (ScreenShotCameraMode)
		{
			ScreenShotCameraMode = false;
			if (FlightCamera.fetch != null)
			{
				FlightCamera.fetch.SetTarget(target, keepWorldPos: true, targetMode);
			}
		}
	}

	public void ShowUI()
	{
		uiToggle = false;
	}

	public void HideUI()
	{
		uiToggle = true;
	}
}
