using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class TestGroup : IConfigNode
{
	public string title = "";

	public string description = "";

	public List<ITestModule> testModules;

	public MENode node { get; set; }

	public bool isActive { get; set; }

	public TestGroup()
	{
		testModules = new List<ITestModule>();
	}

	public TestGroup(MENode node)
		: this()
	{
		this.node = node;
	}

	public TestGroup(MENode node, string title)
		: this(node)
	{
		this.title = title;
	}

	public bool Test()
	{
		int num = 0;
		while (true)
		{
			if (num < testModules.Count)
			{
				if (!testModules[num].Test())
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public TestGroup initializeGroup()
	{
		GameEvents.Mission.onTestGroupInitialized.Fire(this);
		Initialized();
		InitializeTestModules();
		isActive = true;
		return this;
	}

	public TestGroup ClearGroup()
	{
		GameEvents.Mission.onTestGroupCleared.Fire(this);
		Cleared();
		ClearTestModules();
		isActive = false;
		return this;
	}

	public void InitializeTestModules()
	{
		for (int i = 0; i < testModules.Count; i++)
		{
			testModules[i].InitializeTest();
		}
	}

	public void ClearTestModules()
	{
		for (int i = 0; i < testModules.Count; i++)
		{
			testModules[i].ClearTest();
		}
	}

	public void AddTestModule(string testName, ConfigNode cfg)
	{
		Type type = Type.GetType("Expansions.Missions.Tests." + testName, throwOnError: false, ignoreCase: false);
		if (type == null)
		{
			type = AssemblyLoader.GetClassByName(typeof(TestModule), testName);
		}
		if (type != null)
		{
			ITestModule testModule = (ITestModule)node.gameObject.AddComponent(type);
			testModule.Initialize(this);
			if (cfg != null)
			{
				testModule?.Load(cfg);
			}
			testModules.Add(testModule);
		}
		else
		{
			Debug.Log($"Error! Test module {testName} type was not found in Assembly");
		}
	}

	public void RunValidationWrapper(MissionEditorValidator validator)
	{
		RunValidation(validator);
		for (int i = 0; i < testModules.Count; i++)
		{
			testModules[i].RunValidationWrapper(validator);
		}
	}

	public virtual void RunValidation(MissionEditorValidator validator)
	{
	}

	public virtual void Initialized()
	{
	}

	public virtual void Cleared()
	{
	}

	public virtual void OnCloned(TestGroup testGroupBase)
	{
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("title", ref title);
		ConfigNode[] nodes = node.GetNodes("TESTMODULE");
		foreach (ConfigNode configNode in nodes)
		{
			AddTestModule(configNode.GetValue("name"), configNode);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("title", title);
		foreach (ITestModule testModule in testModules)
		{
			ConfigNode configNode = node.AddNode("TESTMODULE");
			testModule.Save(configNode);
		}
	}
}
