using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartResourceList : IEnumerable<PartResource>, IEnumerable
{
	[SerializeField]
	public DictionaryValueList<int, PartResource> dict;

	[SerializeField]
	public Part part;

	[SerializeField]
	public bool simulationSet;

	public bool IsValid => dict != null;

	public PartResource this[int index] => dict.At(index);

	public PartResource this[string name]
	{
		get
		{
			if (dict.TryGetValue(name.GetHashCode(), out var val))
			{
				return val;
			}
			return null;
		}
	}

	public int Count => dict.Count;

	public PartResourceList(Part part)
	{
		Initializer(part);
	}

	public PartResourceList(Part part, bool simulationSet)
	{
		Initializer(part);
		this.simulationSet = simulationSet;
	}

	public PartResourceList(Part part, PartResourceList refList)
	{
		Initializer(part);
		RefListInitializer(part, refList, simulationSet: false);
	}

	public PartResourceList(Part part, PartResourceList refList, bool simulationSet)
	{
		Initializer(part);
		RefListInitializer(part, refList, simulationSet);
	}

	public void Initializer(Part part)
	{
		dict = new DictionaryValueList<int, PartResource>();
		this.part = part;
	}

	public void RefListInitializer(Part part, PartResourceList refList, bool simulationSet)
	{
		if (refList != null && refList.IsValid)
		{
			int count = refList.Count;
			for (int i = 0; i < count; i++)
			{
				PartResource partResource = new PartResource(refList[i], simulationSet);
				partResource.part = part;
				dict.Add(partResource.info.id, partResource);
			}
		}
	}

	public void RefreshSimulationListAmounts(PartResourceList refList)
	{
		if (refList == null || !refList.IsValid)
		{
			return;
		}
		int count = refList.Count;
		for (int i = 0; i < count; i++)
		{
			PartResource partResource = Get(refList[i].resourceName);
			if (partResource == null)
			{
				partResource = new PartResource(refList[i], simulationSet);
				partResource.part = refList[i].part;
				dict.Add(partResource.info.id, partResource);
			}
			else
			{
				partResource.amount = refList[i].amount;
				partResource._flowMode = refList[i].flowMode;
				partResource._flowState = refList[i].flowState;
				partResource.simulationResource = true;
			}
		}
	}

	public bool Contains(string name)
	{
		return Contains(name.GetHashCode());
	}

	public bool Contains(int id)
	{
		return dict.Contains(id);
	}

	public PartResource Get(string name)
	{
		return Get(name.GetHashCode());
	}

	public PartResource Get(int id)
	{
		if (dict.TryGetValue(id, out var val))
		{
			return val;
		}
		return null;
	}

	public bool GetAll(List<PartResource> sources, int id)
	{
		PartResource partResource = Get(id);
		if (partResource != null)
		{
			sources.Add(partResource);
			return true;
		}
		return false;
	}

	public bool GetAllFlowing(List<PartResource> sources, int id)
	{
		PartResource partResource = Get(id);
		if (partResource != null && partResource.flowState)
		{
			sources.Add(partResource);
			return true;
		}
		return false;
	}

	public void GetFlowingTotals(int id, out double amount, out double maxAmount, bool pulling)
	{
		PartResource flowing = GetFlowing(id, pulling);
		if (flowing != null)
		{
			if (pulling)
			{
				amount = flowing.amount;
				maxAmount = flowing.maxAmount;
			}
			else
			{
				amount = flowing.amount;
				maxAmount = flowing.maxAmount;
			}
		}
		else
		{
			double num = 0.0;
			maxAmount = 0.0;
			amount = num;
		}
	}

	public PartResource GetFlowing(int id, bool pulling)
	{
		PartResource partResource = Get(id);
		if (partResource != null && partResource.flowState)
		{
			if (pulling)
			{
				if ((partResource.flowMode & PartResource.FlowMode.Out) > PartResource.FlowMode.None)
				{
					return partResource;
				}
			}
			else if ((partResource.flowMode & PartResource.FlowMode.In) > PartResource.FlowMode.None)
			{
				return partResource;
			}
		}
		return null;
	}

	public PartResource Add(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogWarning("ConfigNode has no value 'name'");
			return null;
		}
		string value = node.GetValue("name");
		if (Contains(value))
		{
			Debug.LogWarning("Part already contains " + (simulationSet ? " simulation resource '" : " resource '") + value + "'");
			return null;
		}
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(value);
		if (definition == null)
		{
			Debug.LogWarning("Could not create " + (simulationSet ? "simulation PartResource" : "PartResource") + " of type '" + value);
			return null;
		}
		PartResource partResource = new PartResource(part, simulationSet);
		partResource.SetInfo(definition);
		partResource.Load(node);
		dict.Add(definition.id, partResource);
		if (!simulationSet)
		{
			GameEvents.onPartResourceListChange.Fire(part);
		}
		return partResource;
	}

	public PartResource Add(PartResource res)
	{
		PartResource partResource = new PartResource(part);
		partResource.Copy(res);
		dict.Add(partResource.info.id, partResource);
		if (!simulationSet)
		{
			GameEvents.onPartResourceListChange.Fire(part);
		}
		return partResource;
	}

	public PartResource Add(string name, double amount, double maxAmount, bool flowState, bool isTweakable, bool hideFlow, bool isVisible, PartResource.FlowMode flow)
	{
		if (Contains(name))
		{
			Debug.LogWarning("Part already contains" + (simulationSet ? " simulation resource '" : " resource '") + name + "'");
			return null;
		}
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(name);
		if (definition == null)
		{
			Debug.LogWarning("Could not create" + (simulationSet ? " simulation PartResource " : " PartResource ") + "of type '" + name);
			return null;
		}
		PartResource partResource = new PartResource(part);
		partResource.resourceName = name;
		partResource.SetInfo(definition);
		partResource.amount = amount;
		partResource.maxAmount = maxAmount;
		partResource.flowState = flowState;
		partResource.isTweakable = isTweakable;
		partResource.hideFlow = hideFlow;
		partResource.isVisible = isVisible;
		partResource.flowMode = flow;
		dict.Add(definition.id, partResource);
		if (!simulationSet)
		{
			GameEvents.onPartResourceListChange.Fire(part);
		}
		return partResource;
	}

	public bool Remove(PartResource res)
	{
		if (dict.Remove(res.info.id))
		{
			if (!simulationSet)
			{
				GameEvents.onPartResourceListChange.Fire(part);
			}
			return true;
		}
		return false;
	}

	public void Clear()
	{
		dict.Clear();
	}

	public bool Remove(string resName)
	{
		if (dict.Remove(resName.GetHashCode()))
		{
			if (!simulationSet)
			{
				GameEvents.onPartResourceListChange.Fire(part);
			}
			return true;
		}
		return false;
	}

	public bool Remove(int resID)
	{
		if (dict.Remove(resID))
		{
			if (!simulationSet)
			{
				GameEvents.onPartResourceListChange.Fire(part);
			}
			return true;
		}
		return false;
	}

	public bool HasFlowable()
	{
		int count = Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (dict.At(count).info.resourceFlowMode <= ResourceFlowMode.NO_FLOW);
		return true;
	}

	public bool HasFlowableUnhidden()
	{
		int count = Count;
		PartResource partResource;
		do
		{
			if (count-- > 0)
			{
				partResource = dict.At(count);
				continue;
			}
			return false;
		}
		while (partResource.info.resourceFlowMode <= ResourceFlowMode.NO_FLOW || !partResource.isVisible);
		return true;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dict.GetListEnumerator();
	}

	public IEnumerator<PartResource> GetEnumerator()
	{
		return dict.GetListEnumerator();
	}
}
