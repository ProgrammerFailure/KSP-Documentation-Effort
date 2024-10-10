using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartResourceDefinitionList : IEnumerable
{
	[SerializeField]
	public Dictionary<int, PartResourceDefinition> dict;

	public PartResourceDefinition this[string name]
	{
		get
		{
			dict.TryGetValue(name.GetHashCode(), out var value);
			return value;
		}
	}

	public PartResourceDefinition this[int id]
	{
		get
		{
			dict.TryGetValue(id, out var value);
			return value;
		}
	}

	public int Count => dict.Count;

	public PartResourceDefinitionList()
	{
		dict = new Dictionary<int, PartResourceDefinition>();
	}

	public PartResourceDefinitionList(PartResourceDefinitionList old)
	{
		dict = new Dictionary<int, PartResourceDefinition>(old.dict);
	}

	public PartResourceDefinition Add(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogWarning("Config has no name field");
			return null;
		}
		string value = node.GetValue("name");
		if (Contains(value))
		{
			Debug.LogWarning("PartResourceDefinition list already contains definition for '" + value + "'");
			return null;
		}
		PartResourceDefinition partResourceDefinition = new PartResourceDefinition();
		partResourceDefinition.Load(node);
		PDebug.Log("ResourceDefinition: " + partResourceDefinition.name + "(" + partResourceDefinition.id + ")", PDebug.DebugLevel.ResourceNetwork);
		dict.Add(partResourceDefinition.id, partResourceDefinition);
		return partResourceDefinition;
	}

	public void Add(PartResourceDefinition def)
	{
		if (Contains(def.name))
		{
			Debug.Log("PartResourceList: Already contains resource of name '" + def.name + "'");
		}
		else
		{
			dict.Add(def.id, def);
		}
	}

	public bool Contains(string name)
	{
		return dict.ContainsKey(name.GetHashCode());
	}

	public IEnumerator<PartResourceDefinition> GetEnumerator()
	{
		return dict.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dict.Values.GetEnumerator();
	}

	public void Clear()
	{
		dict.Clear();
	}
}
