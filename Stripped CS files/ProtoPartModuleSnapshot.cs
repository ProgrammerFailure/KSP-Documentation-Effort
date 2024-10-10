using System;
using System.Collections.Generic;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ProtoPartModuleSnapshot
{
	public ConfigNode moduleValues;

	public string moduleName;

	public PartModule moduleRef;

	public bool hasSaved;

	public ProtoPartModuleSnapshot(PartModule module)
	{
		moduleRef = module;
		moduleName = module.ClassName;
		moduleValues = new ConfigNode("MODULE");
		if ((module.part != null && module.part.isCargoPart()) || (module.vessel != null && (!module.vessel.loaded || module.vessel.isUnloading || module.vessel.isBackingUp)))
		{
			module.Save(moduleValues);
			hasSaved = true;
		}
		module.snapshot = this;
		if (hasSaved)
		{
			GameEvents.onProtoPartModuleSnapshotSave.Fire(new GameEvents.FromToAction<ProtoPartModuleSnapshot, ConfigNode>(this, null));
		}
	}

	public ProtoPartModuleSnapshot(ConfigNode node)
	{
		GameEvents.onProtoPartModuleSnapshotLoad.Fire(new GameEvents.FromToAction<ProtoPartModuleSnapshot, ConfigNode>(this, node));
		moduleValues = new ConfigNode("MODULE");
		node.CopyTo(moduleValues);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string name = value.name;
			if (name == "name")
			{
				moduleName = value.value;
			}
		}
	}

	public void Save(ConfigNode node)
	{
		EnsureModuleValuesIsInitialized();
		moduleValues.CopyTo(node);
		GameEvents.onProtoPartModuleSnapshotSave.Fire(new GameEvents.FromToAction<ProtoPartModuleSnapshot, ConfigNode>(this, node));
	}

	public PartModule Load(Part hostPart, ref int moduleIndex)
	{
		GameEvents.onProtoPartModuleSnapshotLoad.Fire(new GameEvents.FromToAction<ProtoPartModuleSnapshot, ConfigNode>(this, null));
		PartModule partModule = hostPart.LoadModule(moduleValues, ref moduleIndex);
		if (partModule == null)
		{
			Debug.LogWarning("PartModule is null.", hostPart.gameObject);
			return null;
		}
		partModule.snapshot = this;
		moduleRef = partModule;
		return partModule;
	}

	public void EnsureModuleValuesIsInitialized()
	{
		if (!hasSaved && moduleRef != null)
		{
			moduleRef.Save(moduleValues);
			hasSaved = true;
		}
	}

	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < moduleAdjusters.Count; i++)
			{
				AddPartModuleAdjuster(moduleAdjusters[i]);
			}
		}
	}

	public void AddPartModuleAdjuster(AdjusterPartModuleBase newAdjuster)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(moduleName))
		{
			List<Type> list = MissionsUtils.adjusterTypesSupportedByPartModule[moduleName];
			if (newAdjuster != null && list.Contains(newAdjuster.GetType()))
			{
				EnsureModuleValuesIsInitialized();
				string name = "ADJUSTERS";
				ConfigNode node = new ConfigNode(name);
				moduleValues.TryGetNode(name, ref node);
				newAdjuster.Save(node.AddNode("ADJUSTERMODULE"));
				moduleValues.SetNode(name, node, createIfNotFound: true);
				GameEvents.onProtoPartModuleAdjusterAdded.Fire(this);
			}
		}
	}

	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			return;
		}
		List<Type> list = MissionsUtils.adjusterTypesSupportedByPartModule[moduleName];
		for (int i = 0; i < moduleAdjusters.Count; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (moduleAdjusters[i].GetType() == list[j])
				{
					RemovePartModuleAdjuster(moduleAdjusters[i]);
				}
			}
		}
	}

	public void RemovePartModuleAdjuster(AdjusterPartModuleBase removeAdjuster)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			EnsureModuleValuesIsInitialized();
			string name = "ADJUSTERSTOREMOVE";
			ConfigNode node = new ConfigNode(name);
			moduleValues.TryGetNode(name, ref node);
			removeAdjuster.Save(node.AddNode("ADJUSTERMODULE"));
			moduleValues.SetNode(name, node, createIfNotFound: true);
			GameEvents.onProtoPartModuleAdjusterRemoved.Fire(this);
		}
	}

	public void RemovePartModuleAdjuster(Guid removeAdjusterID)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			return;
		}
		EnsureModuleValuesIsInitialized();
		string name = "ADJUSTERS";
		ConfigNode node = new ConfigNode();
		moduleValues.TryGetNode(name, ref node);
		ConfigNode[] nodes = node.GetNodes();
		string name2 = "ADJUSTERSTOREMOVE";
		ConfigNode node2 = new ConfigNode(name2);
		moduleValues.TryGetNode(name2, ref node2);
		for (int i = 0; i < nodes.Length; i++)
		{
			if (nodes[i].HasValue("adjusterID") && new Guid(nodes[i].GetValue("adjusterID")) == removeAdjusterID)
			{
				node2.AddNode(nodes[i]);
			}
		}
		moduleValues.SetNode(name, node2, createIfNotFound: true);
		GameEvents.onProtoPartModuleAdjusterRemoved.Fire(this);
	}

	public List<T> GetListOfActiveAdjusters<T>() where T : class
	{
		List<AdjusterPartModuleBase> list = new List<AdjusterPartModuleBase>();
		List<AdjusterPartModuleBase> list2 = new List<AdjusterPartModuleBase>();
		EnsureModuleValuesIsInitialized();
		ConfigNode node = new ConfigNode();
		if (moduleValues.TryGetNode("ADJUSTERS", ref node))
		{
			list.AddRange(AdjusterPartModuleBase.CreateModuleAdjusterList(node));
		}
		ConfigNode node2 = new ConfigNode();
		if (moduleValues.TryGetNode("ADJUSTERSTOREMOVE", ref node2))
		{
			list2.AddRange(AdjusterPartModuleBase.CreateModuleAdjusterList(node2));
		}
		for (int num = list2.Count - 1; num >= 0; num--)
		{
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				if (list[num2].adjusterID == list2[num].adjusterID)
				{
					list.RemoveAt(num2);
				}
			}
		}
		List<T> list3 = new List<T>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] is T item)
			{
				list3.Add(item);
			}
		}
		return list3;
	}

	public void ProtoPartModuleRepair()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			EnsureModuleValuesIsInitialized();
			string name = "ADJUSTERS";
			ConfigNode node = new ConfigNode();
			moduleValues.TryGetNode(name, ref node);
			ConfigNode[] nodes = node.GetNodes();
			string name2 = "ADJUSTERSTOREMOVE";
			ConfigNode node2 = new ConfigNode(name2);
			moduleValues.TryGetNode(name2, ref node2);
			for (int i = 0; i < nodes.Length; i++)
			{
				node2.AddNode(nodes[i]);
			}
			moduleValues.SetNode(name2, node2, createIfNotFound: true);
			GameEvents.onProtoPartModuleRepaired.Fire(this);
		}
	}
}
