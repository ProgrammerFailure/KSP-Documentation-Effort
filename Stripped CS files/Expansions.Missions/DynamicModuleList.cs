using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions;

public class DynamicModuleList : IConfigNode, ICloneable
{
	public List<DynamicModule> activeModules;

	public List<Type> supportedTypes;

	public string rootNodeName = "MODULES";

	public string moduleNodeName = "MODULE";

	public MENode node { get; set; }

	public DynamicModuleList(MENode node)
	{
		this.node = node;
		activeModules = new List<DynamicModule>();
		supportedTypes = new List<Type>();
	}

	public virtual List<Type> GetSupportedTypes()
	{
		return supportedTypes;
	}

	public List<IMENodeDisplay> GetInternalParameters()
	{
		List<IMENodeDisplay> list = new List<IMENodeDisplay>();
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			list.Add(activeModules[i]);
		}
		return list;
	}

	public bool ContainsModule(Type module)
	{
		int count = activeModules.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(activeModules[count].GetType() == module));
		return true;
	}

	public void AddDynamicModule(ConfigNode moduleNode)
	{
		string value = "";
		if (!moduleNode.TryGetValue("name", ref value))
		{
			return;
		}
		Type classByName = AssemblyLoader.GetClassByName(typeof(DynamicModule), value);
		if (classByName != null)
		{
			DynamicModule dynamicModule = (DynamicModule)Activator.CreateInstance(classByName, new object[1] { node });
			dynamicModule.SetNode(node);
			if (moduleNode != null)
			{
				dynamicModule?.Load(moduleNode);
			}
			activeModules.Add(dynamicModule);
		}
		else
		{
			Debug.Log($"Error {value} type was not found in AssemblyLoader");
		}
	}

	public void SetSupportedTypes(List<Type> supportedModuleTypes)
	{
		supportedTypes = supportedModuleTypes;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DynamicModuleList dynamicModuleList))
		{
			return false;
		}
		if (activeModules.Count != dynamicModuleList.activeModules.Count)
		{
			return false;
		}
		int count = activeModules.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (activeModules[count].Equals(dynamicModuleList.activeModules[count]));
		return false;
	}

	public override int GetHashCode()
	{
		return activeModules.GetHashCode();
	}

	public void Load(ConfigNode node)
	{
		if (node.HasNode(rootNodeName))
		{
			ConfigNode[] nodes = node.GetNode(rootNodeName).GetNodes(moduleNodeName);
			activeModules.Clear();
			ConfigNode[] array = nodes;
			foreach (ConfigNode moduleNode in array)
			{
				AddDynamicModule(moduleNode);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode(rootNodeName);
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			ConfigNode configNode2 = configNode.AddNode(moduleNodeName);
			activeModules[i].Save(configNode2);
		}
	}

	public virtual object Clone()
	{
		DynamicModuleList dynamicModuleList = (DynamicModuleList)Activator.CreateInstance(GetType(), new object[1] { node });
		dynamicModuleList.activeModules = new List<DynamicModule>();
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			dynamicModuleList.activeModules.Add(activeModules[i].Clone() as DynamicModule);
		}
		dynamicModuleList.supportedTypes = new List<Type>(supportedTypes);
		return dynamicModuleList;
	}
}
