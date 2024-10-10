using System;
using System.Collections.Generic;
using ns2;
using UnityEngine;

namespace ns11;

public class UIAppSpawner : MonoBehaviour
{
	[Serializable]
	public class AppWrapper
	{
		public GameObject prefab;

		public List<GameScenes> scenes;

		[NonSerialized]
		public GameObject instantiatedApp;
	}

	public UICanvasPrefab ApplauncherScreenPrefab;

	public AppWrapper[] apps;

	public void Awake()
	{
		GameEvents.onLevelWasLoaded.Add(OnLevelLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(OnLevelLoaded);
	}

	public void OnLevelLoaded(GameScenes scene)
	{
		if ((scene == GameScenes.EDITOR || scene == GameScenes.FLIGHT || scene == GameScenes.MAINMENU || scene == GameScenes.SPACECENTER || scene == GameScenes.TRACKSTATION) && ApplicationLauncher.Instance == null)
		{
			UIMasterController.Instance.AddCanvas(UIMasterController.Instance.appCanvas, ApplauncherScreenPrefab, removeOnSceneSwitch: false);
		}
		int i = 0;
		for (int num = apps.Length; i < num; i++)
		{
			AppWrapper appWrapper = apps[i];
			if (appWrapper.scenes.Contains(HighLogic.LoadedScene) && appWrapper.instantiatedApp == null)
			{
				appWrapper.instantiatedApp = UnityEngine.Object.Instantiate(appWrapper.prefab);
			}
		}
	}
}
