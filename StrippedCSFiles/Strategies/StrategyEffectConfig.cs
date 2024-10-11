using System;
using System.Runtime.CompilerServices;

namespace Strategies;

[Serializable]
public class StrategyEffectConfig
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
	public StrategyEffectConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static StrategyEffectConfig Create(ConfigNode node)
	{
		throw null;
	}
}
