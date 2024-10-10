using System;

namespace Strategies;

[Serializable]
public class StrategyEffectConfig
{
	public string name;

	public ConfigNode config;

	public string Name => name;

	public ConfigNode Config => config;

	public static StrategyEffectConfig Create(ConfigNode node)
	{
		StrategyEffectConfig strategyEffectConfig = new StrategyEffectConfig();
		string value = node.GetValue("name");
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}
		strategyEffectConfig.name = value;
		strategyEffectConfig.config = node.CreateCopy();
		return strategyEffectConfig;
	}
}
