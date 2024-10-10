using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioRunner : MonoBehaviour
{
	public List<ProtoScenarioModule> protoModules = new List<ProtoScenarioModule>();

	public List<ScenarioModule> modules = new List<ScenarioModule>();

	public static ScenarioRunner Instance { get; set; }

	public virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded);
	}

	public virtual void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Remove(onLevelWasLoaded);
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public virtual void OnGameSceneLoadRequested(GameScenes scene)
	{
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			if (modules[i] != null)
			{
				UnityEngine.Object.DestroyImmediate(modules[i]);
			}
		}
		modules.Clear();
	}

	public virtual void onLevelWasLoaded(GameScenes scene)
	{
		if (scene == GameScenes.MAINMENU)
		{
			AddMainMenuScenarios();
		}
	}

	public void AddMainMenuScenarios()
	{
		List<ProtoScenarioModule> list = new List<ProtoScenarioModule>();
		List<KSPScenarioType> allScenarioTypesInAssemblies = KSPScenarioType.GetAllScenarioTypesInAssemblies();
		int count = allScenarioTypesInAssemblies.Count;
		for (int i = 0; i < count; i++)
		{
			KSPScenarioType kSPScenarioType = allScenarioTypesInAssemblies[i];
			if (!kSPScenarioType.ScenarioAttributes.HasTargetScene(GameScenes.MAINMENU))
			{
				continue;
			}
			ConfigNode configNode = new ConfigNode("SCENARIO");
			configNode.AddValue("name", kSPScenarioType.ModuleType.Name);
			string text = "";
			int j = 0;
			for (int num = kSPScenarioType.ScenarioAttributes.TargetScenes.Length; j < num; j++)
			{
				string text2 = text;
				int num2 = (int)kSPScenarioType.ScenarioAttributes.TargetScenes[j];
				text = text2 + num2;
				if (j < kSPScenarioType.ScenarioAttributes.TargetScenes.Length - 1)
				{
					text += ", ";
				}
			}
			configNode.AddValue("scene", text);
			ProtoScenarioModule item = new ProtoScenarioModule(configNode);
			list.Add(item);
		}
		LoadModules(list);
	}

	public ScenarioModule AddModule(string moduleName)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(ScenarioModule), moduleName);
		if (classByName == null)
		{
			Debug.LogError("Cannot find a Module of typename '" + moduleName + "'");
			return null;
		}
		ScenarioModule scenarioModule = (ScenarioModule)Instance.gameObject.AddComponent(classByName);
		if (scenarioModule == null)
		{
			Debug.LogError("Cannot create a Module of typename '" + moduleName + "'");
			return null;
		}
		Instance.modules.Add(scenarioModule);
		return scenarioModule;
	}

	public ScenarioModule AddModule(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add a Module because ConfigNode contains no module name");
			return null;
		}
		string value = node.GetValue("name");
		ScenarioModule scenarioModule = AddModule(value);
		if (scenarioModule == null)
		{
			return null;
		}
		try
		{
			scenarioModule.Load(node);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception loading ScenarioModule " + value + ": " + ex);
		}
		return scenarioModule;
	}

	public virtual void LoadModules(ProtoScenarioModule protoModule)
	{
		LoadModules(new List<ProtoScenarioModule> { protoModule });
	}

	public virtual void LoadModules(List<ProtoScenarioModule> protoModules)
	{
		this.protoModules = protoModules;
		int i = 0;
		for (int count = protoModules.Count; i < count; i++)
		{
			ProtoScenarioModule protoScenarioModule = protoModules[i];
			if (protoScenarioModule.moduleName == "MissionSystem" && protoScenarioModule.targetScenes.Contains(HighLogic.LoadedScene))
			{
				protoScenarioModule.Load(this);
			}
		}
		int j = 0;
		for (int count2 = protoModules.Count; j < count2; j++)
		{
			ProtoScenarioModule protoScenarioModule = protoModules[j];
			if (protoScenarioModule.moduleName != "MissionSystem" && protoScenarioModule.targetScenes.Contains(HighLogic.LoadedScene))
			{
				protoScenarioModule.Load(this);
			}
		}
	}

	public virtual void ClearModules()
	{
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			UnityEngine.Object.Destroy(modules[i]);
		}
		modules.Clear();
	}

	public virtual void UpdateModules()
	{
		int i = 0;
		for (int count = protoModules.Count; i < count; i++)
		{
			if (protoModules[i].moduleRef != null)
			{
				protoModules[i] = new ProtoScenarioModule(protoModules[i].moduleRef);
			}
		}
	}

	public static List<ScenarioModule> GetLoadedModules()
	{
		return Instance.modules;
	}

	public static List<ProtoScenarioModule> GetUpdatedProtoModules()
	{
		Instance.UpdateModules();
		return Instance.protoModules;
	}

	public static void SetProtoModules(List<ProtoScenarioModule> protoModules)
	{
		Instance.LoadModules(protoModules);
	}

	public static void SetProtoModules(ProtoScenarioModule protoModule)
	{
		Instance.LoadModules(protoModule);
	}

	public static void RemoveModule(ScenarioModule module)
	{
		if (module.runner != Instance)
		{
			Debug.LogError("Cannot destroy a module which does not belong to this runner");
			return;
		}
		Instance.modules.Remove(module);
		UnityEngine.Object.DestroyImmediate(module);
		module = null;
	}
}
