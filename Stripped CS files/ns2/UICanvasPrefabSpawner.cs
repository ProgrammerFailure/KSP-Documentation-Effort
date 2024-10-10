using System;
using UnityEngine;

namespace ns2;

public class UICanvasPrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class UICanvasPrefabWrapper
	{
		public UICanvasPrefab prefab;

		public bool removeOnSceneSwitch = true;
	}

	public UICanvasPrefabWrapper[] prefabs;

	public void Awake()
	{
		if (UIMasterController.Instance == null)
		{
			Debug.LogError("UICanvasPrefabSpawner: UIMasterController is not present. Cannot spawn anything.");
			return;
		}
		int i = 0;
		for (int num = prefabs.Length; i < num; i++)
		{
			UICanvasPrefabWrapper uICanvasPrefabWrapper = prefabs[i];
			Debug.Log("UICanvasPrefabSpawner " + base.gameObject.name + " spawning " + uICanvasPrefabWrapper.prefab.canvasName);
			if (UIMasterController.Instance.CanSpawnCanvasPrefab(uICanvasPrefabWrapper.prefab))
			{
				UIMasterController.Instance.AddCanvas(uICanvasPrefabWrapper.prefab, uICanvasPrefabWrapper.removeOnSceneSwitch);
			}
		}
	}
}
