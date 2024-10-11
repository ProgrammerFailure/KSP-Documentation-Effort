using System.Runtime.CompilerServices;

namespace Experience;

public class ExperienceEffectConfig
{
	private string name;

	private ConfigNode config;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigNode Config
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceEffectConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ExperienceEffectConfig Create(ConfigNode node)
	{
		throw null;
	}
}
