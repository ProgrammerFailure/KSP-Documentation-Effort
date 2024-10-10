using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinePrint.Utilities;

[Serializable]
public class ConstructionParts
{
	[SerializeField]
	public List<ConstructionPart> constructionPartDefinitions;

	public Dictionary<string, List<ConstructionPart>> cachedLists;

	public ConstructionParts(List<ConstructionPart> constructionPartDefinitions)
	{
		this.constructionPartDefinitions = constructionPartDefinitions;
		cachedLists = new Dictionary<string, List<ConstructionPart>>();
	}

	public List<ConstructionPart> PartsForContractType(string contractType)
	{
		if (cachedLists.ContainsKey(contractType))
		{
			return cachedLists[contractType];
		}
		List<ConstructionPart> list = new List<ConstructionPart>();
		for (int i = 0; i < constructionPartDefinitions.Count; i++)
		{
			if (constructionPartDefinitions[i].contractTypes.Contains(contractType))
			{
				list.Add(constructionPartDefinitions[i]);
			}
		}
		cachedLists.Add(contractType, list);
		return list;
	}
}
