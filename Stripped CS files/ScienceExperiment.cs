using System;
using System.Collections.Generic;

[Serializable]
public class ScienceExperiment
{
	public string id;

	public string experimentTitle;

	public float baseValue;

	public float scienceCap;

	public uint situationMask;

	public uint biomeMask;

	public float dataScale;

	public bool requireAtmosphere;

	public bool requireNoAtmosphere;

	public float requiredExperimentLevel;

	public bool applyScienceScale;

	public Dictionary<string, string> results;

	public Dictionary<string, string> Results => results;

	public ScienceExperiment()
	{
		results = new Dictionary<string, string>();
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("id", ref id);
		node.TryGetValue("title", ref experimentTitle);
		node.TryGetValue("baseValue", ref baseValue);
		node.TryGetValue("scienceCap", ref scienceCap);
		node.TryGetValue("situationMask", ref situationMask);
		node.TryGetValue("biomeMask", ref biomeMask);
		node.TryGetValue("dataScale", ref dataScale);
		node.TryGetValue("requireAtmosphere", ref requireAtmosphere);
		node.TryGetValue("requireNoAtmosphere", ref requireNoAtmosphere);
		node.TryGetValue("requiredExperimentLevel", ref requiredExperimentLevel);
		applyScienceScale = true;
		node.TryGetValue("applyScienceScale", ref applyScienceScale);
		if (!node.HasNode("RESULTS"))
		{
			return;
		}
		ConfigNode node2 = node.GetNode("RESULTS");
		int count = node2.values.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode.Value value = node2.values[i];
			while (results.ContainsKey(value.name))
			{
				value.name += "*";
			}
			results.Add(value.name, value.value);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", id);
		node.AddValue("title", experimentTitle);
		node.AddValue("baseValue", baseValue);
		node.AddValue("situationMask", situationMask);
		node.AddValue("biomeMask", biomeMask);
		node.AddValue("scienceCap", scienceCap);
		node.AddValue("dataScale", dataScale);
		node.AddValue("requireAtmosphere", requireAtmosphere);
		node.AddValue("requireNoAtmosphere", requireNoAtmosphere);
		node.AddValue("requiredExperimentLevel", requiredExperimentLevel);
		node.AddValue("applyScienceScale", applyScienceScale);
	}

	public bool IsAvailableWhile(ExperimentSituations situation, CelestialBody body)
	{
		if (requireAtmosphere)
		{
			if (body.atmosphere)
			{
				return (situationMask & (uint)situation) != 0;
			}
			return false;
		}
		if (requireNoAtmosphere)
		{
			if (!body.atmosphere)
			{
				return (situationMask & (uint)situation) != 0;
			}
			return false;
		}
		return (situationMask & (uint)situation) != 0;
	}

	public bool BiomeIsRelevantWhile(ExperimentSituations situation)
	{
		return (biomeMask & (uint)situation) != 0;
	}

	public bool IsUnlocked()
	{
		return GameVariables.Instance.GetExperimentLevel(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment)) >= requiredExperimentLevel;
	}
}
