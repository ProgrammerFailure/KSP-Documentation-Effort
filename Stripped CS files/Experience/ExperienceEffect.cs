using System;

namespace Experience;

public class ExperienceEffect
{
	public ExperienceTrait parent;

	public string name;

	public float[] levelModifiers;

	public int level;

	public ExperienceTrait Parent => parent;

	public string Description => GetDescription();

	public string Name
	{
		get
		{
			if (name == null)
			{
				name = GetType().Name;
			}
			return name;
		}
	}

	public float[] LevelModifiers => levelModifiers;

	public int Level => level;

	public ExperienceEffect(ExperienceTrait parent)
	{
		this.parent = parent;
	}

	public ExperienceEffect(ExperienceTrait parent, float[] modifiers)
	{
		modifiers.CopyTo(LevelModifiers, 0);
	}

	public ExperienceEffect(ExperienceTrait parent, int level)
	{
		this.level = level;
	}

	public void Register(Part part)
	{
		OnRegister(part);
	}

	public void Unregister(Part part)
	{
		OnUnregister(part);
	}

	public void LoadFromConfig(ConfigNode node)
	{
		string value = node.GetValue("modifiers");
		if (value != null)
		{
			string[] array = value.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			levelModifiers = new float[array.Length + 1];
			levelModifiers[0] = GetDefaultValue();
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				levelModifiers[i + 1] = float.Parse(array[i]);
			}
		}
		string value2 = node.GetValue("level");
		if (value2 != null)
		{
			level = int.Parse(value2);
		}
		OnLoadFromConfig(node);
	}

	public virtual string GetDescription()
	{
		return "";
	}

	public virtual void OnRegister(Part part)
	{
	}

	public virtual void OnUnregister(Part part)
	{
	}

	public virtual void OnLoadFromConfig(ConfigNode node)
	{
	}

	public virtual float GetDefaultValue()
	{
		return 0f;
	}
}
