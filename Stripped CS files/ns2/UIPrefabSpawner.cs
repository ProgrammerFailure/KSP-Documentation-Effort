using System;
using System.Collections;
using UnityEngine;

namespace ns2;

public class UIPrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class UIPrefabWrapper
	{
		public RectTransform prefab;

		public RectTransform parent;

		public SpawnStep spawnStep;
	}

	public enum SpawnStep
	{
		Awake,
		Start,
		Delayed
	}

	public UIPrefabWrapper[] prefabs;

	public int delayFrames = 1;

	public bool forceLocalTransform = true;

	public void Awake()
	{
		Spawn(SpawnStep.Awake);
	}

	public IEnumerator Start()
	{
		Spawn(SpawnStep.Start);
		int i = 0;
		while (i < delayFrames)
		{
			yield return null;
			int num = i + 1;
			i = num;
		}
		Spawn(SpawnStep.Delayed);
	}

	public void Spawn(SpawnStep spawnStep)
	{
		int i = 0;
		for (int num = prefabs.Length; i < num; i++)
		{
			UIPrefabWrapper uIPrefabWrapper = prefabs[i];
			if (!(uIPrefabWrapper.prefab == null) && !(uIPrefabWrapper.parent == null) && uIPrefabWrapper.spawnStep == spawnStep)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate(uIPrefabWrapper.prefab);
				rectTransform.SetParent(uIPrefabWrapper.parent, worldPositionStays: false);
				if (forceLocalTransform)
				{
					rectTransform.localPosition = Vector3.zero;
					rectTransform.localScale = Vector3.one;
					rectTransform.localRotation = Quaternion.identity;
				}
			}
		}
	}
}
