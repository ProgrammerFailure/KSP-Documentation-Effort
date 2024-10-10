namespace Experience.Effects;

public class EVAChuteSkill : ExperienceEffect
{
	public EVAChuteSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public EVAChuteSkill(ExperienceTrait parent, int level)
		: base(parent, level)
	{
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.EVAChuteSkill.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.EVAChuteSkill.Remove(GetValue);
	}

	public int GetValue()
	{
		int num = 0;
		while (true)
		{
			if (num < base.Parent.Effects.Count)
			{
				if (base.Parent.Effects[num].GetType() == GetType())
				{
					break;
				}
				num++;
				continue;
			}
			return base.Level;
		}
		return base.Parent.Effects[num].Level;
	}
}
