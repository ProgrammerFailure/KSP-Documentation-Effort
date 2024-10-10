using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns10;

[Serializable]
public class FireworkFXList : IEnumerable
{
	[SerializeField]
	public Dictionary<int, FireworkFXDefinition> fireworkFX;

	public FireworkFXList()
	{
		fireworkFX = new Dictionary<int, FireworkFXDefinition>();
	}

	public FireworkFXDefinition Add(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogWarning("[FireworkFX]: Config has no name field");
			return null;
		}
		string value = node.GetValue("name");
		if (Contains(value))
		{
			Debug.LogWarning("FireworkFXist already contains definition for '" + value + "'");
			return null;
		}
		FireworkFXDefinition fireworkFXDefinition = new FireworkFXDefinition();
		fireworkFXDefinition.Load(node);
		fireworkFX.Add(fireworkFXDefinition.id, fireworkFXDefinition);
		return fireworkFXDefinition;
	}

	public bool Contains(string name)
	{
		return fireworkFX.ContainsKey(name.GetHashCode());
	}

	public int Count()
	{
		return fireworkFX.Count;
	}

	public IEnumerator<FireworkFXDefinition> GetEnumerator()
	{
		return fireworkFX.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return fireworkFX.Values.GetEnumerator();
	}
}
