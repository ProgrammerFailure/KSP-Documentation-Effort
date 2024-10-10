using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinePrint.Utilities;

[Serializable]
public class PreBuiltCraft
{
	[SerializeField]
	public List<PreBuiltCraftDefinition> preBuiltDefinitions;

	public Dictionary<string, List<PreBuiltCraftDefinition>> cachedLists;

	public PreBuiltCraft(List<PreBuiltCraftDefinition> preBuiltDefinitions)
	{
		this.preBuiltDefinitions = preBuiltDefinitions;
		cachedLists = new Dictionary<string, List<PreBuiltCraftDefinition>>();
	}

	public List<PreBuiltCraftDefinition> CraftForContractType(string contractType)
	{
		if (cachedLists.ContainsKey(contractType))
		{
			return cachedLists[contractType];
		}
		List<PreBuiltCraftDefinition> list = new List<PreBuiltCraftDefinition>();
		for (int i = 0; i < preBuiltDefinitions.Count; i++)
		{
			if (preBuiltDefinitions[i].contractTypes.Contains(contractType))
			{
				list.Add(preBuiltDefinitions[i]);
			}
		}
		cachedLists.Add(contractType, list);
		return list;
	}

	public PreBuiltCraftDefinition CraftByURL(string url)
	{
		int startIndex = url.LastIndexOf("GameData", StringComparison.Ordinal);
		url = url.Substring(startIndex);
		int num = 0;
		while (true)
		{
			if (num < preBuiltDefinitions.Count)
			{
				int startIndex2 = preBuiltDefinitions[num].craftURL.LastIndexOf("GameData", StringComparison.Ordinal);
				if (preBuiltDefinitions[num].craftURL.Substring(startIndex2).Equals(url))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return preBuiltDefinitions[num];
	}
}
