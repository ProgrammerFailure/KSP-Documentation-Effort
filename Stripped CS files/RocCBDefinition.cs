using System;
using System.Collections.Generic;
using UniLinq;

[Serializable]
public class RocCBDefinition
{
	public string name;

	public List<string> biomes;

	public RocCBDefinition()
	{
	}

	public RocCBDefinition(string name, List<string> biomes)
		: this()
	{
		this.name = name;
		this.biomes = biomes;
	}

	public void Load(ConfigNode node)
	{
		name = node.GetValue("Name");
		biomes = node.GetValues("Biome").ToList();
	}
}
