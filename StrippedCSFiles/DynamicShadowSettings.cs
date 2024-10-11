using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicShadowSettings : MonoBehaviour
{
	[Serializable]
	public class SceneShadowSettings
	{
		public float distanceSimple;

		public float distanceGood;

		public float distanceBeautiful;

		public float distanceFantastic;

		public ShadowProjection shadowProjection;

		public float cascadeSimple;

		public Vector3 cascadeGood;

		public Vector3 cascadeBeautiful;

		public Vector3 cascadeFantastic;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SceneShadowSettings()
		{
			throw null;
		}
	}

	public static DynamicShadowSettings Instance;

	public SceneShadowSettings Flight;

	public SceneShadowSettings KSC;

	public SceneShadowSettings TrackingStation;

	public SceneShadowSettings Editors;

	public SceneShadowSettings MainMenu;

	public SceneShadowSettings Default;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DynamicShadowSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyShadowSettings(SceneShadowSettings sss)
	{
		throw null;
	}
}
