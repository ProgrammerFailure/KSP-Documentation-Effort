using ns2;
using ns9;
using UnityEngine;

namespace ns11;

public class AlarmClockApp : UIApp
{
	public class UsageStats
	{
		public bool UsageFound => false;

		public void Reset()
		{
		}
	}

	public static AlarmClockApp Instance;

	public static bool Ready;

	[SerializeField]
	public AlarmClockUIFrame appFramePrefab;

	public AlarmClockUIFrame appFrame;

	public RectTransform appRectTransform;

	[SerializeField]
	public int appWidth = 230;

	public int appHeight = 300;

	public UsageStats usage;

	public override void OnAppInitialized()
	{
		if (Instance != null)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		AlarmClockScenario.appInstance = this;
		appFrame = Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appRectTransform = appFrame.transform as RectTransform;
		appFrame.Setup(base.appLauncherButton, base.name, Localizer.Format("#autoLOC_8003500"), appWidth, appHeight);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		HideApp();
		if (usage == null)
		{
			usage = new UsageStats();
		}
		Ready = true;
	}

	public override void OnAppDestroy()
	{
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(appFrame.Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
			if (InputLockManager.GetControlLock("UIAlarmClockApp") != ControlTypes.None)
			{
				RemoveAppFrameInputLock();
			}
		}
		Ready = false;
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override bool OnAppAboutToStart()
	{
		if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE))
		{
			return false;
		}
		if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.EDITOR && HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			return HighLogic.LoadedScene == GameScenes.TRACKSTATION;
		}
		return true;
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
		}
	}

	public override void HideApp()
	{
		if (appFrame != null && !appFrame.MouseOver)
		{
			appFrame.AddAlarmPaneCloseImmediate();
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.ALWAYS;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return defaultAnchorPos;
	}

	public void Start()
	{
	}

	public void Update()
	{
	}

	public static bool AnyTextFieldHasFocus()
	{
		if (Instance == null)
		{
			return false;
		}
		return Instance.TextFieldHasFocus();
	}

	public static bool AppFrameHasLock()
	{
		return InputLockManager.GetControlLock("UIAlarmClockApp") > ControlTypes.None;
	}

	public void RemoveAppFrameInputLock()
	{
		InputLockManager.RemoveControlLock("UIAlarmClockApp");
	}

	public bool TextFieldHasFocus()
	{
		if (appFrame != null)
		{
			return false;
		}
		return appFrame.AnyTextFieldHasFocus();
	}

	public static bool EditAlarm(AlarmTypeBase alarm)
	{
		if (Instance == null)
		{
			Debug.LogWarning($"[AlarmClockApp]: App is not running at this time - unable to edit alarm id={alarm.Id}");
			return false;
		}
		Instance.DisplayApp();
		if (Instance.appFrame != null)
		{
			Instance.appLauncherButton.SetTrue(makeCall: false);
			return Instance.appFrame.EditAlarm(alarm);
		}
		return false;
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if (usage.UsageFound)
		{
			usage.Reset();
		}
	}
}
