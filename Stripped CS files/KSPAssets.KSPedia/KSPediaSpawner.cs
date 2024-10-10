using System;
using KSPAssets.Loaders;
using ns11;
using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaSpawner : MonoBehaviour
{
	public KSPedia prefab;

	public float canvasBorder = 40f;

	public KSPedia kspedia;

	public ApplicationLauncherButton applauncherButton;

	public Button toolbarButton;

	public static KSPediaSpawner Instance { get; set; }

	public static void Show(string screen = null, Button toolbarButton = null)
	{
		Instance.toolbarButton = toolbarButton;
		Instance.ShowKSPedia(screen);
	}

	public static void Show(ApplicationLauncherButton applauncherButton)
	{
		Instance.applauncherButton = applauncherButton;
		if (HighLogic.LoadedSceneHasPlanetarium && PauseMenu.exists && PauseMenu.isOpen)
		{
			PauseMenu.Close();
			FlightDriver.SetPause(pauseState: true);
		}
		AnalyticsUtil.LogKSPedia();
		Instance.ShowKSPedia();
	}

	public static void Hide()
	{
		Instance.HideKSPedia();
		if (Instance.applauncherButton != null)
		{
			Instance.applauncherButton.SetFalse();
			Instance.applauncherButton = null;
		}
		if (Instance.toolbarButton != null)
		{
			Instance.toolbarButton.gameObject.SetActive(value: true);
			Instance.toolbarButton.interactable = true;
		}
		if (FlightDriver.Pause)
		{
			FlightDriver.SetPause(pauseState: false);
		}
	}

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		kspedia = UnityEngine.Object.Instantiate(prefab);
		kspedia.transform.SetParent(UIMasterController.Instance.dialogCanvas.transform, worldPositionStays: false);
		kspedia.gameObject.SetActive(value: false);
		kspedia.GetComponent<KSPediaAspectController>().screenCanvas = UIMasterController.Instance.dialogCanvas;
		KSPedia kSPedia = kspedia;
		kSPedia.readyCallback = (KSPedia.ReadyCallback)Delegate.Combine(kSPedia.readyCallback, new KSPedia.ReadyCallback(KSPediaReadyCallback));
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void ShowKSPedia(string screen = null)
	{
		if (InputLockManager.GetControlLock("KSPedia") == ControlTypes.None)
		{
			InputLockManager.SetControlLock(ControlTypes.All, "KSPedia");
		}
		kspedia.gameObject.SetActive(value: true);
		(kspedia.transform as RectTransform).sizeDelta = new Vector2(0f, (UIMasterController.Instance.dialogCanvas.transform as RectTransform).sizeDelta.y - canvasBorder);
		if (screen != null)
		{
			kspedia.ShowScreen(screen);
		}
		else
		{
			kspedia.OnUnhide();
		}
		GameEvents.onGUIKSPediaSpawn.Fire();
		if (Instance.applauncherButton != null)
		{
			Instance.applauncherButton.Disable(makeCall: false);
		}
		if (Instance.toolbarButton != null)
		{
			Instance.toolbarButton.interactable = false;
		}
	}

	public void HideKSPedia()
	{
		kspedia.gameObject.SetActive(value: false);
		InputLockManager.RemoveControlLock("KSPedia");
		AssetLoader.Instance.ReleaseKSPediaAssets();
		Instance.applauncherButton.Enable(makeCall: false);
		GameEvents.onGUIKSPediaDespawn.Fire();
	}

	public void KSPediaReadyCallback()
	{
		if (Instance.applauncherButton != null)
		{
			Instance.applauncherButton.Enable(makeCall: false);
		}
	}
}
