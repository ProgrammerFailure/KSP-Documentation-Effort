using System.Collections.Generic;
using UnityEngine;

namespace Experience;

public class ExperienceSystemConfig
{
	public List<ExperienceTraitConfig> categories = new List<ExperienceTraitConfig>();

	public List<string> traitNames = new List<string>();

	public List<string> traitNamesNoTourist = new List<string>();

	public List<ExperienceTraitConfig> Categories => categories;

	public List<string> TraitNames => traitNames;

	public List<string> TraitNamesNoTourist => traitNamesNoTourist;

	public ExperienceSystemConfig()
	{
		LoadTraitConfigs();
	}

	public void LoadTraitConfigs()
	{
		categories = new List<ExperienceTraitConfig>();
		traitNames.Clear();
		traitNamesNoTourist.Clear();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("EXPERIENCE_TRAIT");
		int i = 0;
		for (int num = configNodes.Length; i < num; i++)
		{
			ExperienceTraitConfig experienceTraitConfig = ExperienceTraitConfig.Create(configNodes[i]);
			if (experienceTraitConfig == null)
			{
				continue;
			}
			ExperienceTraitConfig experienceTraitConfig2 = GetExperienceTraitConfig(experienceTraitConfig.Name);
			if (experienceTraitConfig2 != null)
			{
				bool flag = false;
				for (int j = 0; j < experienceTraitConfig.Effects.Count; j++)
				{
					List<string> traitsWithEffect = GetTraitsWithEffect(experienceTraitConfig.Effects[j].Name);
					if (traitsWithEffect.Count > 0)
					{
						for (int k = 0; k < traitsWithEffect.Count; k++)
						{
							if (traitsWithEffect[k] == experienceTraitConfig.Name)
							{
								Debug.LogError("ExperienceSystemConfig: Experience trait '" + experienceTraitConfig.Name + "' already exists, and trying to add a duplicate Effect of '" + experienceTraitConfig.Effects[j].Name + "'");
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Debug.LogFormat("ExperienceSystemConfig: Added Effect '{0}' to Trait '{1}'", experienceTraitConfig.Effects[j].Name, experienceTraitConfig2.Title);
						experienceTraitConfig2.Effects.Add(experienceTraitConfig.Effects[j]);
					}
				}
			}
			else
			{
				categories.Add(experienceTraitConfig);
				traitNames.Add(experienceTraitConfig.Name);
				if (experienceTraitConfig.Name != KerbalRoster.touristTrait)
				{
					traitNamesNoTourist.Add(experienceTraitConfig.Name);
				}
			}
		}
	}

	public ExperienceTraitConfig GetExperienceTraitConfig(string name)
	{
		int num = 0;
		int count = categories.Count;
		while (true)
		{
			if (num < count)
			{
				if (categories[num].Name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return categories[num];
	}

	public List<string> GetTraitsWithEffect(string effectName)
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = Categories.Count; i < count; i++)
		{
			ExperienceTraitConfig experienceTraitConfig = Categories[i];
			int j = 0;
			for (int count2 = experienceTraitConfig.Effects.Count; j < count2; j++)
			{
				if (experienceTraitConfig.Effects[j].Name == effectName)
				{
					list.Add(experienceTraitConfig.Title);
					break;
				}
			}
		}
		return list;
	}

	public List<string> GetTraitsNamesWithEffect(string effectName)
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = Categories.Count; i < count; i++)
		{
			ExperienceTraitConfig experienceTraitConfig = Categories[i];
			int j = 0;
			for (int count2 = experienceTraitConfig.Effects.Count; j < count2; j++)
			{
				if (experienceTraitConfig.Effects[j].Name == effectName)
				{
					list.Add(experienceTraitConfig.Name);
					break;
				}
			}
		}
		return list;
	}
}
