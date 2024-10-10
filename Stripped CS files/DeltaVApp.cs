using ns11;
using ns2;
using ns9;
using UnityEngine;

public class DeltaVApp : UIApp
{
	public class UsageStats
	{
		public int body;

		public int situation;

		public int atmosphereSlider;

		public int atmosphereText;

		public int displayFields;

		public bool UsageFound
		{
			get
			{
				if (body <= 0 && situation <= 0 && atmosphereSlider <= 0 && atmosphereText <= 0)
				{
					return displayFields > 0;
				}
				return true;
			}
		}

		public void Reset()
		{
			body = 0;
			situation = 0;
			atmosphereSlider = 0;
			atmosphereText = 0;
			displayFields = 0;
		}
	}

	public static DeltaVApp Instance;

	public static bool Ready;

	[SerializeField]
	public GenericAppFrame appFramePrefab;

	public GenericAppFrame appFrame;

	[SerializeField]
	public DeltaVAppSituation situationPrefab;

	public DeltaVAppSituation situation;

	[SerializeField]
	public DeltaVAppStageInfo stageInfoPrefab;

	public DeltaVAppStageInfo stageInfo;

	public RectTransform appRectTransform;

	[SerializeField]
	public int appWidth = 230;

	public int appHeight;

	[SerializeField]
	public int appHeightSituation = 161;

	[SerializeField]
	public int appHeightStageInfo = 173;

	[SerializeField]
	public int appStageInfoLineHeight = 20;

	public bool initialHeightSet;

	public UsageStats usage;

	public override void OnAppInitialized()
	{
		if (!GameSettings.DELTAV_APP_ENABLED)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		DeltaVAppValues deltaVAppValues = DeltaVGlobals.DeltaVAppValues;
		bool num = situationPrefab != null && HighLogic.LoadedSceneIsEditor;
		bool flag = (stageInfoPrefab != null && HighLogic.LoadedSceneIsEditor) || HighLogic.LoadedSceneIsFlight;
		appHeight = 27;
		if (num)
		{
			appHeight += appHeightSituation;
		}
		if (flag)
		{
			appHeight += appHeightStageInfo;
			int num2 = 7;
			if (deltaVAppValues != null)
			{
				num2 = deltaVAppValues.infoLines.Count;
			}
			if (GameSettings.DELTAV_APP_TWOCOLUMN_MODE)
			{
				appHeight += appStageInfoLineHeight * ((num2 + 1) / 2);
			}
			else
			{
				appHeight += appStageInfoLineHeight * num2;
			}
		}
		appFrame = Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appRectTransform = appFrame.transform as RectTransform;
		appFrame.Setup(base.appLauncherButton, base.name, Localizer.Format("#autoLOC_8003216"), appWidth, appHeight);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		if (num)
		{
			situation = Object.Instantiate(situationPrefab);
			situation.transform.SetParent(appFrame.scrollList.transform, worldPositionStays: false);
		}
		if (flag)
		{
			stageInfo = Object.Instantiate(stageInfoPrefab);
			stageInfo.transform.SetParent(appFrame.scrollList.transform, worldPositionStays: false);
			stageInfo.infoLineHeight = appStageInfoLineHeight;
		}
		HideApp();
		if (usage == null)
		{
			usage = new UsageStats();
		}
		Ready = true;
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
		GameEvents.onGUIDeltaVAppReady.Fire();
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
		}
		Ready = false;
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		GameEvents.onGUIDeltaVAppDestroy.Fire();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override bool OnAppAboutToStart()
	{
		if (!HighLogic.LoadedSceneIsEditor)
		{
			return HighLogic.LoadedSceneIsFlight;
		}
		return true;
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			if (!initialHeightSet)
			{
				appRectTransform.sizeDelta = new Vector2(appWidth, appHeight);
				initialHeightSet = true;
			}
			if (stageInfo != null)
			{
				stageInfo.SetColumnLayout(GameSettings.DELTAV_APP_TWOCOLUMN_MODE);
			}
			appFrame.gameObject.SetActive(value: true);
		}
	}

	public override void HideApp()
	{
		if (appFrame != null && (situation == null || !situation.IsBodyDropDownExpanded))
		{
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.flag_5 | ApplicationLauncher.AppScenes.flag_6;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return defaultAnchorPos;
	}

	public static bool AnyTextFieldHasFocus()
	{
		if (Instance == null)
		{
			return false;
		}
		return Instance.TextFieldHasFocus();
	}

	public bool TextFieldHasFocus()
	{
		if (situation == null)
		{
			return false;
		}
		return situation.AltitudeHasFocus();
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if ((scn.from == GameScenes.EDITOR || scn.from == GameScenes.FLIGHT) && usage.UsageFound)
		{
			AnalyticsUtil.DeltaVAppUsage(HighLogic.CurrentGame, scn.from, usage);
			usage.Reset();
		}
	}
}
