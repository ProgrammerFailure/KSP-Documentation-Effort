using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class UIAppSpawner : MonoBehaviour
{
	[Serializable]
	public class AppWrapper
	{
		public GameObject prefab;

		public List<GameScenes> scenes;

		[NonSerialized]
		public GameObject instantiatedApp;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AppWrapper()
		{
			throw null;
		}
	}

	public UICanvasPrefab ApplauncherScreenPrefab;

	public AppWrapper[] apps;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIAppSpawner()
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
	private void OnLevelLoaded(GameScenes scene)
	{
		throw null;
	}
}
