using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class PrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class PrefabWrapper
	{
		public GameObject prefab;

		public Transform parent;

		public bool parentToThis;

		public SpawnStep spawnStep;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PrefabWrapper()
		{
			throw null;
		}
	}

	public enum SpawnStep
	{
		Awake,
		Start
	}

	public PrefabWrapper[] prefabs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PrefabSpawner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Spawn(SpawnStep spawnStep)
	{
		throw null;
	}
}
