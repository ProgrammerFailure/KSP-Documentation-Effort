using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UICanvasPrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class UICanvasPrefabWrapper
	{
		public UICanvasPrefab prefab;

		public bool removeOnSceneSwitch;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UICanvasPrefabWrapper()
		{
			throw null;
		}
	}

	public UICanvasPrefabWrapper[] prefabs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UICanvasPrefabSpawner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}
}
