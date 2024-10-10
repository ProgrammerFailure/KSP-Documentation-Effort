using System;
using System.Collections.Generic;
using ns11;
using UnityEngine;

namespace Experience;

public class ExperienceTrait
{
	public ProtoCrewMember protoCrewMember;

	public List<ExperienceEffect> effects = new List<ExperienceEffect>();

	public ExperienceTraitConfig Config { get; set; }

	public ProtoCrewMember CrewMember => protoCrewMember;

	public string TypeName
	{
		get
		{
			Type type = GetType();
			if (type == typeof(ExperienceTrait))
			{
				return Config.Title;
			}
			return type.Name;
		}
	}

	public string Title => Config.Title;

	public string Description => Config.Description;

	public string DescriptionEffects
	{
		get
		{
			string text = "";
			int i = 0;
			for (int count = effects.Count; i < count; i++)
			{
				string description = effects[i].Description;
				if (!string.IsNullOrEmpty(description))
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text += description;
				}
			}
			return text;
		}
	}

	public List<ExperienceEffect> Effects => effects;

	public static ExperienceTrait Create(Type type, ExperienceTraitConfig config, ProtoCrewMember pcm)
	{
		if (config == null)
		{
			Debug.LogError("ExperienceTrait: Config cannot be null");
			return null;
		}
		if (type == null)
		{
			type = typeof(ExperienceTrait);
		}
		ExperienceTrait obj = (ExperienceTrait)Activator.CreateInstance(type);
		obj.SetupConfig(config);
		obj.protoCrewMember = pcm;
		return obj;
	}

	public void SetupConfig(ExperienceTraitConfig cfg)
	{
		Config = cfg;
		for (int i = 0; i < cfg.Effects.Count; i++)
		{
			AddEffect(cfg.Effects[i]);
		}
	}

	public void AddEffect(ExperienceEffectConfig cfg)
	{
		Type experienceEffectType = KerbalRoster.GetExperienceEffectType(cfg.Name);
		if (experienceEffectType == null)
		{
			Debug.LogError("ExperienceTrait: Cannot add effect '" + cfg.Name + "' as it does not exist.");
			return;
		}
		ExperienceEffect experienceEffect = (ExperienceEffect)Activator.CreateInstance(experienceEffectType, this);
		if (experienceEffect != null)
		{
			experienceEffect.LoadFromConfig(cfg.Config);
			effects.Add(experienceEffect);
		}
	}

	public int CrewMemberExperienceLevel(int levelCap = -1)
	{
		if (levelCap == -1)
		{
			return Mathf.Max(CrewMember.experienceLevel, 0);
		}
		return Mathf.Min(Mathf.Max(CrewMember.experienceLevel, 0), levelCap - 1);
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnRegister(Part part)
	{
	}

	public virtual void OnUnregister(Part part)
	{
	}

	public void Register(Part part)
	{
		OnRegister(part);
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Register(part);
		}
	}

	public void Unregister(Part part)
	{
		OnUnregister(part);
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Unregister(part);
		}
	}

	public void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, "<b>" + Title + "</>\n\n" + message, color, icon));
		}
		Debug.Log("Experience (" + Title + "): " + message);
	}
}
