using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicShadowSettings : MonoBehaviour
{
	[Serializable]
	public class SceneShadowSettings
	{
		public float distanceSimple = 30f;

		public float distanceGood = 10000f;

		public float distanceBeautiful = 10000f;

		public float distanceFantastic = 10000f;

		public ShadowProjection shadowProjection;

		public float cascadeSimple = 0.333f;

		public Vector3 cascadeGood;

		public Vector3 cascadeBeautiful;

		public Vector3 cascadeFantastic;
	}

	public static DynamicShadowSettings Instance;

	public SceneShadowSettings Flight;

	public SceneShadowSettings sceneShadowSettings_0;

	public SceneShadowSettings TrackingStation;

	public SceneShadowSettings Editors;

	public SceneShadowSettings MainMenu;

	public SceneShadowSettings Default;

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		Flight.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_FLIGHT_PROJECTION;
		sceneShadowSettings_0.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_KSC_PROJECTION;
		TrackingStation.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_TRACKING_PROJECTION;
		Editors.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_EDITORS_PROJECTION;
		MainMenu.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_MAIN_PROJECTION;
		Default.shadowProjection = (ShadowProjection)GameSettings.SHADOWS_DEFAULT_PROJECTION;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes level)
	{
		switch (level)
		{
		case GameScenes.MAINMENU:
			ApplyShadowSettings(MainMenu);
			break;
		default:
			ApplyShadowSettings(Default);
			break;
		case GameScenes.SPACECENTER:
			ApplyShadowSettings(sceneShadowSettings_0);
			break;
		case GameScenes.EDITOR:
			ApplyShadowSettings(Editors);
			break;
		case GameScenes.FLIGHT:
			ApplyShadowSettings(Flight);
			break;
		case GameScenes.TRACKSTATION:
			ApplyShadowSettings(TrackingStation);
			break;
		}
	}

	public void ApplyShadowSettings(SceneShadowSettings sss)
	{
		switch (QualitySettings.GetQualityLevel())
		{
		case 2:
			QualitySettings.shadowDistance = sss.distanceSimple;
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowCascade2Split = sss.cascadeSimple;
			break;
		case 3:
			QualitySettings.shadowDistance = sss.distanceGood;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = sss.cascadeGood;
			break;
		default:
			QualitySettings.shadowDistance = sss.distanceBeautiful;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = sss.cascadeBeautiful;
			break;
		case 5:
			QualitySettings.shadowDistance = sss.distanceFantastic;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = sss.cascadeFantastic;
			break;
		case 0:
		case 1:
			break;
		}
		QualitySettings.shadowProjection = sss.shadowProjection;
	}
}
