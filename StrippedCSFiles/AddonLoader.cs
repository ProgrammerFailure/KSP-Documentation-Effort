using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddonLoader : MonoBehaviour
{
	private class LoadedBehaviour
	{
		public Type type;

		public AssemblyLoader.LoadedAssembly loadedAssembly;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedBehaviour(Type typeName, AssemblyLoader.LoadedAssembly assemblyName)
		{
			throw null;
		}
	}

	private List<LoadedBehaviour> loadedOnceList;

	public static AddonLoader Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AddonLoader()
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
	private bool LoadedOnceContains(Type typeName, AssemblyLoader.LoadedAssembly assemblyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes loadedLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartAddons(KSPAddon.Startup level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartAddon(AssemblyLoader.LoadedAssembly asm, Type type, KSPAddon addon, KSPAddon.Startup level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool AddonLevelTest(KSPAddon addon, KSPAddon.Startup level)
	{
		throw null;
	}
}
