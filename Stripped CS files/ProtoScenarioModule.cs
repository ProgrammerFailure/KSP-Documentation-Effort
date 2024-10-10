using System.Collections.Generic;
using UnityEngine;

public class ProtoScenarioModule
{
	public ConfigNode moduleValues;

	public string moduleName;

	public List<GameScenes> targetScenes;

	public ScenarioModule moduleRef;

	public ProtoScenarioModule(ScenarioModule module)
	{
		moduleRef = module;
		moduleName = module.ClassName;
		targetScenes = new List<GameScenes>(module.targetScenes);
		moduleValues = new ConfigNode("SCENARIO");
		module.Save(moduleValues);
		module.snapshot = this;
	}

	public ProtoScenarioModule(ConfigNode node)
	{
		moduleValues = new ConfigNode("SCENARIO");
		node.CopyTo(moduleValues);
		targetScenes = new List<GameScenes>();
		int count = node.values.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string name = value.name;
			if (!(name == "name"))
			{
				if (name == "scene")
				{
					string[] array = value.value.Split(',');
					int num = array.Length;
					for (int j = 0; j < num; j++)
					{
						targetScenes.Add((GameScenes)int.Parse(array[j].Trim()));
					}
				}
			}
			else
			{
				moduleName = value.value;
			}
		}
	}

	public void SetTargetScenes(GameScenes[] scenes)
	{
		targetScenes = new List<GameScenes>(scenes);
		string text = "";
		int i = 0;
		for (int num = scenes.Length; i < num; i++)
		{
			string text2 = text;
			int num2 = (int)scenes[i];
			text = text2 + num2;
			if (i < scenes.Length - 1)
			{
				text += ", ";
			}
		}
		if (moduleValues.HasValue("scene"))
		{
			moduleValues.SetValue("scene", text);
		}
		else
		{
			moduleValues.AddValue("scene", text);
		}
	}

	public void Save(ConfigNode node)
	{
		moduleValues.CopyTo(node);
	}

	public ScenarioModule Load(ScenarioRunner host)
	{
		ScenarioModule scenarioModule = host.AddModule(moduleValues);
		if (scenarioModule == null)
		{
			Debug.LogError("ScenarioModule is null.");
			return null;
		}
		scenarioModule.snapshot = this;
		scenarioModule.targetScenes = new List<GameScenes>(targetScenes);
		moduleRef = scenarioModule;
		return scenarioModule;
	}

	public ConfigNode GetData()
	{
		return moduleValues;
	}
}
