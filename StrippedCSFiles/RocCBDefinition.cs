using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class RocCBDefinition
{
	public string name;

	public List<string> biomes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RocCBDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RocCBDefinition(string name, List<string> biomes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
