using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Experience;

public class ExperienceSystemConfig
{
	private List<ExperienceTraitConfig> categories;

	private List<string> traitNames;

	private List<string> traitNamesNoTourist;

	public List<ExperienceTraitConfig> Categories
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<string> TraitNames
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<string> TraitNamesNoTourist
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceSystemConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadTraitConfigs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceTraitConfig GetExperienceTraitConfig(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetTraitsWithEffect(string effectName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetTraitsNamesWithEffect(string effectName)
	{
		throw null;
	}
}
