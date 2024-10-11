using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint.Utilities;

[Serializable]
public class PreBuiltCraft
{
	[SerializeField]
	private List<PreBuiltCraftDefinition> preBuiltDefinitions;

	private Dictionary<string, List<PreBuiltCraftDefinition>> cachedLists;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreBuiltCraft(List<PreBuiltCraftDefinition> preBuiltDefinitions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PreBuiltCraftDefinition> CraftForContractType(string contractType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreBuiltCraftDefinition CraftByURL(string url)
	{
		throw null;
	}
}
