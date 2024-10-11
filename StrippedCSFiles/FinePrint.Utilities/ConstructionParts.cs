using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint.Utilities;

[Serializable]
public class ConstructionParts
{
	[SerializeField]
	private List<ConstructionPart> constructionPartDefinitions;

	private Dictionary<string, List<ConstructionPart>> cachedLists;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConstructionParts(List<ConstructionPart> constructionPartDefinitions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ConstructionPart> PartsForContractType(string contractType)
	{
		throw null;
	}
}
