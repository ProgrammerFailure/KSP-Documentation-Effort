using System;
using UnityEngine;

namespace ns2;

public class PrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class PrefabWrapper
	{
		public GameObject prefab;

		public Transform parent;

		public bool parentToThis = true;

		public SpawnStep spawnStep;
	}

	public enum SpawnStep
	{
		Awake,
		Start
	}

	public PrefabWrapper[] prefabs;

	public void Awake()
	{
		Spawn(SpawnStep.Awake);
	}

	public void Start()
	{
		Spawn(SpawnStep.Start);
	}

	public void Spawn(SpawnStep spawnStep)
	{
		int i = 0;
		for (int num = prefabs.Length; i < num; i++)
		{
			PrefabWrapper prefabWrapper = prefabs[i];
			if (!(prefabWrapper.prefab == null) && prefabWrapper.spawnStep == spawnStep)
			{
				Debug.Log("PrefabSpawner " + base.gameObject.name + " spawning " + prefabWrapper.prefab.name + " in " + spawnStep);
				GameObject gameObject = UnityEngine.Object.Instantiate(prefabWrapper.prefab);
				if (prefabWrapper.parent != null)
				{
					gameObject.transform.SetParent(prefabWrapper.parent, worldPositionStays: false);
				}
				else if (prefabWrapper.parentToThis)
				{
					gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				}
			}
		}
	}
}
