using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedPrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class NestedPrefab
	{
		public GameObject prefab;

		public Transform tgtTransform;

		public bool setTags;

		public GameObject instance;

		public int defaultLayer;

		public string defaultTag;

		public GameObject Instance => instance;

		public void Spawn(LayerMask layers, string[] tags)
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.Instantiate(prefab);
				instance.transform.parent = tgtTransform.parent;
				instance.transform.localPosition = tgtTransform.localPosition;
				instance.transform.localRotation = tgtTransform.localRotation;
				instance.transform.localScale = tgtTransform.localScale;
				instance.name = tgtTransform.name;
				defaultLayer = tgtTransform.gameObject.layer;
				defaultTag = tgtTransform.gameObject.tag;
				setLayersAndTagsRecursive(instance.transform, layers, tgtTransform.gameObject.layer, tags, tgtTransform.gameObject.tag);
				UnityEngine.Object.DestroyImmediate(tgtTransform.gameObject);
			}
			else
			{
				Debug.LogError("[NestedPrefabSpawner]: Instances for the linked prefabs already exist! Did you forget to clean up before playing?", instance);
			}
		}

		public void Despawn()
		{
			if (instance != null)
			{
				tgtTransform = new GameObject(instance.name).transform;
				tgtTransform.transform.parent = instance.transform.parent;
				tgtTransform.localPosition = instance.transform.localPosition;
				tgtTransform.localRotation = instance.transform.localRotation;
				tgtTransform.localScale = instance.transform.localScale;
				tgtTransform.gameObject.layer = defaultLayer;
				tgtTransform.gameObject.tag = defaultTag;
				UnityEngine.Object.DestroyImmediate(instance.gameObject);
			}
		}

		public void setLayersAndTagsRecursive(Transform trf, LayerMask layers, int defaultLayer, string[] allowedTags, string defaultTag)
		{
			if ((layers.value & trf.gameObject.layer) == 0)
			{
				trf.gameObject.layer = defaultLayer;
			}
			if (setTags)
			{
				bool flag = true;
				int num = allowedTags.Length;
				while (num-- > 0)
				{
					if (trf.CompareTag(allowedTags[num]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					trf.tag = defaultTag;
				}
			}
			for (int i = 0; i < trf.childCount; i++)
			{
				setLayersAndTagsRecursive(trf.GetChild(i), layers, defaultLayer, allowedTags, defaultTag);
			}
		}
	}

	public enum SpawnOnEvent
	{
		None,
		Awake,
		StartPlusDelay
	}

	public EventData<NestedPrefab> OnSpawn = new EventData<NestedPrefab>("OnSpawn");

	public List<NestedPrefab> Prefabs;

	public SpawnOnEvent SpawnOn = SpawnOnEvent.Awake;

	public int startDelay;

	public LayerMask allowedLayers = -1;

	public string[] allowedTags;

	public bool Spawned { get; set; }

	public void Awake()
	{
		if (SpawnOn == SpawnOnEvent.Awake)
		{
			SpawnPrefabs();
		}
	}

	public IEnumerator Start()
	{
		if (SpawnOn == SpawnOnEvent.StartPlusDelay)
		{
			int i = 0;
			while (i < startDelay)
			{
				yield return null;
				int num = i + 1;
				i = num;
			}
			SpawnPrefabs();
		}
	}

	[ContextMenu("Place Prefabs")]
	public void SpawnPrefabs()
	{
		int count = Prefabs.Count;
		for (int i = 0; i < count; i++)
		{
			Prefabs[i].Spawn(allowedLayers, allowedTags);
			OnSpawn.Fire(Prefabs[i]);
		}
		Spawned = true;
	}

	[ContextMenu("Commit")]
	public void CleanUp()
	{
		int count = Prefabs.Count;
		for (int i = 0; i < count; i++)
		{
			Prefabs[i].Despawn();
		}
		Spawned = false;
	}
}
