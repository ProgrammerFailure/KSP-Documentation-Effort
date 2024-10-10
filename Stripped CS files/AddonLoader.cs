using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddonLoader : MonoBehaviour
{
	public class LoadedBehaviour
	{
		public Type type;

		public AssemblyLoader.LoadedAssembly loadedAssembly;

		public LoadedBehaviour(Type typeName, AssemblyLoader.LoadedAssembly assemblyName)
		{
			type = typeName;
			loadedAssembly = assemblyName;
		}
	}

	public List<LoadedBehaviour> loadedOnceList;

	public static AddonLoader Instance { get; set; }

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		loadedOnceList = new List<LoadedBehaviour>();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public bool LoadedOnceContains(Type typeName, AssemblyLoader.LoadedAssembly assemblyName)
	{
		int num = 0;
		int count = loadedOnceList.Count;
		while (true)
		{
			if (num < count)
			{
				if (loadedOnceList[num].type == typeName && loadedOnceList[num].loadedAssembly == assemblyName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes loadedLevel)
	{
		StartAddons((KSPAddon.Startup)loadedLevel);
	}

	public void StartAddons(KSPAddon.Startup level)
	{
		if (AssemblyLoader.loadedAssemblies == null)
		{
			return;
		}
		int i = 0;
		for (int count = AssemblyLoader.loadedAssemblies.Count; i < count; i++)
		{
			AssemblyLoader.LoadedAssembly loadedAssembly = AssemblyLoader.loadedAssemblies[i];
			try
			{
				Type[] types = loadedAssembly.assembly.GetTypes();
				int j = 0;
				for (int num = types.Length; j < num; j++)
				{
					Type type = types[j];
					if (type.IsSubclassOf(typeof(MonoBehaviour)) || !(type != typeof(MonoBehaviour)))
					{
						KSPAddon[] array = (KSPAddon[])type.GetCustomAttributes(typeof(KSPAddon), inherit: true);
						if (array.Length != 0)
						{
							StartAddon(loadedAssembly, type, array[0], level);
						}
					}
				}
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex is ReflectionTypeLoadException)
				{
					text += "\n\nAdditional information about this exception:";
					Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
					foreach (Exception ex2 in loaderExceptions)
					{
						text = text + "\n\n " + ex2.ToString();
					}
				}
				Debug.LogError("[AddonLoader]: Exception iterating assemblies: " + text);
			}
		}
	}

	public void StartAddon(AssemblyLoader.LoadedAssembly asm, Type type, KSPAddon addon, KSPAddon.Startup level)
	{
		if (AddonLevelTest(addon, level) && (!addon.once || !LoadedOnceContains(type, asm)))
		{
			Debug.Log("[AddonLoader]: Instantiating addon '" + type.Name + "' from assembly '" + asm.name + "'");
			new GameObject(type.Name).AddComponent(type);
			if (addon.once)
			{
				loadedOnceList.Add(new LoadedBehaviour(type, asm));
			}
		}
	}

	public bool AddonLevelTest(KSPAddon addon, KSPAddon.Startup level)
	{
		if (addon.startup < (KSPAddon.Startup)0)
		{
			if (addon.startup == KSPAddon.Startup.EveryScene)
			{
				return true;
			}
			if (addon.startup == KSPAddon.Startup.AllGameScenes)
			{
				if (level != KSPAddon.Startup.EditorAny && level != KSPAddon.Startup.Flight && level != KSPAddon.Startup.SpaceCentre)
				{
					return level == KSPAddon.Startup.TrackingStation;
				}
				return true;
			}
			if (addon.startup == KSPAddon.Startup.FlightAndEditor)
			{
				if (level != KSPAddon.Startup.Flight)
				{
					return level == KSPAddon.Startup.EditorAny;
				}
				return true;
			}
			if (addon.startup == KSPAddon.Startup.FlightAndKSC)
			{
				if (level != KSPAddon.Startup.Flight)
				{
					return level == KSPAddon.Startup.SpaceCentre;
				}
				return true;
			}
			if (addon.startup == KSPAddon.Startup.FlightEditorAndKSC)
			{
				if (level != KSPAddon.Startup.Flight && level != KSPAddon.Startup.EditorAny)
				{
					return level == KSPAddon.Startup.SpaceCentre;
				}
				return true;
			}
		}
		if (addon.startup == level)
		{
			return true;
		}
		return false;
	}
}
