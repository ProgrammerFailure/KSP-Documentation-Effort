using System.Collections.Generic;
using UnityEngine;

namespace Experience;

public class ExperienceTraitConfig
{
	public string name;

	public string title;

	public string description;

	public string iconString;

	public Texture2D iconImage;

	public List<ExperienceEffectConfig> effects;

	public string Name => name;

	public string Title => title;

	public string Description => description;

	public string IconString => iconString;

	public Texture2D IconImage => iconImage;

	public List<ExperienceEffectConfig> Effects => effects;

	public static ExperienceTraitConfig Create(ConfigNode node)
	{
		ExperienceTraitConfig experienceTraitConfig = new ExperienceTraitConfig();
		experienceTraitConfig.name = node.GetValue("name");
		if (experienceTraitConfig.Name == null)
		{
			Debug.LogError("ExperienceTraitConfig: name cannot be null");
			return null;
		}
		string value = node.GetValue("title");
		if (!string.IsNullOrEmpty(value))
		{
			experienceTraitConfig.title = value;
		}
		value = node.GetValue("desc");
		if (!string.IsNullOrEmpty(value))
		{
			experienceTraitConfig.description = value;
		}
		value = node.GetValue("description");
		if (!string.IsNullOrEmpty(value))
		{
			experienceTraitConfig.description = value;
		}
		value = node.GetValue("icon");
		if (!string.IsNullOrEmpty(value))
		{
			experienceTraitConfig.iconString = value;
			experienceTraitConfig.iconImage = GameDatabase.Instance.GetTexture(experienceTraitConfig.iconString, asNormalMap: false);
		}
		experienceTraitConfig.effects = new List<ExperienceEffectConfig>();
		ConfigNode[] nodes = node.GetNodes("EFFECT");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ExperienceEffectConfig experienceEffectConfig = ExperienceEffectConfig.Create(nodes[i]);
			if (experienceEffectConfig != null)
			{
				experienceTraitConfig.Effects.Add(experienceEffectConfig);
			}
		}
		return experienceTraitConfig;
	}
}
