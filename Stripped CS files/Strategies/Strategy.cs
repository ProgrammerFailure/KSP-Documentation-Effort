using System;
using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

namespace Strategies;

[Serializable]
public class Strategy
{
	public bool isActive;

	public float factor;

	public double dateActivated;

	public List<StrategyEffect> effects = new List<StrategyEffect>();

	public float requiredReputationMin;

	public float requiredReputationMax;

	public float initialCostFundsMin;

	public float initialCostScienceMin;

	public float initialCostReputationMin;

	public float initialCostFundsMax;

	public float initialCostScienceMax;

	public float initialCostReputationMax;

	public double minLeastDuration;

	public double maxLeastDuration;

	public double minLongestDuration;

	public double maxLongestDuration;

	public bool hasFactorSlider;

	public float factorSliderDefault;

	public int factorSliderSteps;

	public static string cacheAutoLOC_304551;

	public static string cacheAutoLOC_304558;

	public static string cacheAutoLOC_304560;

	public static string cacheAutoLOC_304637;

	public static string cacheAutoLOC_6002417;

	public static string cacheAutoLOC_304641;

	public static string cacheAutoLOC_304646;

	public static string cacheAutoLOC_304654;

	public static string cacheAutoLOC_304658;

	public static string cacheAutoLOC_304612;

	public static string cacheAutoLOC_304674;

	public static string cacheAutoLOC_304827;

	public static string cacheAutoLOC_304841;

	public static string cacheAutoLOC_304848;

	public static string cacheAutoLOC_304855;

	public static string cacheAutoLOC_304887;

	public static string cacheAutoLOC_304890;

	public static string cacheAutoLOC_304909;

	public bool IsActive => isActive;

	public StrategyConfig Config { get; set; }

	public string DepartmentName => Config.DepartmentName;

	public DepartmentConfig Department => Config.Department;

	public string Title => Config.Title;

	public string Description => Config.Description;

	public string[] GroupTags => Config.GroupTags;

	public string Effect => GetEffectText();

	public string Text => GetText();

	public float Factor
	{
		get
		{
			return factor;
		}
		set
		{
			factor = value;
		}
	}

	public double DateActivated => dateActivated;

	public List<StrategyEffect> Effects => effects;

	public float RequiredReputationMin
	{
		get
		{
			if (requiredReputationMin == 0f)
			{
				return Config.RequiredReputationMin;
			}
			return requiredReputationMin;
		}
	}

	public float RequiredReputationMax
	{
		get
		{
			if (requiredReputationMax == 0f)
			{
				return Config.RequiredReputationMax;
			}
			return requiredReputationMax;
		}
	}

	public float RequiredReputation => FactorLerp(RequiredReputationMin, RequiredReputationMax);

	public float InitialCostFunds => FactorLerp(InitialCostFundsMin, InitialCostFundsMax);

	public float InitialCostScience => FactorLerp(InitialCostScienceMin, InitialCostScienceMax);

	public float InitialCostReputation => FactorLerp(InitialCostReputationMin, InitialCostReputationMax);

	public float InitialCostFundsMin
	{
		get
		{
			if (initialCostFundsMin == 0f)
			{
				return Config.InitialCostFundsMin;
			}
			return initialCostFundsMin;
		}
	}

	public float InitialCostScienceMin
	{
		get
		{
			if (initialCostScienceMin == 0f)
			{
				return Config.InitialCostScienceMin;
			}
			return initialCostScienceMin;
		}
	}

	public float InitialCostReputationMin
	{
		get
		{
			if (initialCostReputationMin == 0f)
			{
				return Config.InitialCostReputationMin;
			}
			return initialCostReputationMin;
		}
	}

	public float InitialCostFundsMax
	{
		get
		{
			if (initialCostFundsMax == 0f)
			{
				return Config.InitialCostFundsMax;
			}
			return initialCostFundsMax;
		}
	}

	public float InitialCostScienceMax
	{
		get
		{
			if (initialCostScienceMax == 0f)
			{
				return Config.InitialCostScienceMax;
			}
			return initialCostScienceMax;
		}
	}

	public float InitialCostReputationMax
	{
		get
		{
			if (initialCostReputationMax == 0f)
			{
				return Config.InitialCostReputationMax;
			}
			return initialCostReputationMax;
		}
	}

	public double MinLeastDuration
	{
		get
		{
			if (minLeastDuration == 0.0)
			{
				return Config.MinLeastDuration;
			}
			return minLeastDuration;
		}
	}

	public double MaxLeastDuration
	{
		get
		{
			if (maxLeastDuration == 0.0)
			{
				return Config.MaxLeastDuration;
			}
			return maxLeastDuration;
		}
	}

	public double MinLongestDuration
	{
		get
		{
			if (minLongestDuration == 0.0)
			{
				return Config.MinLongestDuration;
			}
			return minLongestDuration;
		}
	}

	public double MaxLongestDuration
	{
		get
		{
			if (maxLongestDuration == 0.0)
			{
				return Config.MaxLongestDuration;
			}
			return maxLongestDuration;
		}
	}

	public double LeastDuration => FactorLerp(minLeastDuration, maxLeastDuration);

	public double LongestDuration => FactorLerp(minLeastDuration, maxLeastDuration);

	public bool NoDuration
	{
		get
		{
			if (Config.MinLeastDuration == 0.0 && Config.MaxLeastDuration == 0.0 && Config.MinLongestDuration == 0.0)
			{
				return Config.MaxLongestDuration == 0.0;
			}
			return false;
		}
	}

	public bool HasFactorSlider
	{
		get
		{
			if (!hasFactorSlider)
			{
				return Config.HasFactorSlider;
			}
			return hasFactorSlider;
		}
	}

	public float FactorSliderDefault
	{
		get
		{
			if (factorSliderDefault == 0f)
			{
				return Config.FactorSliderDefault;
			}
			return factorSliderDefault;
		}
	}

	public int FactorSliderSteps
	{
		get
		{
			if (factorSliderSteps == 0)
			{
				return Config.FactorSliderSteps;
			}
			return factorSliderSteps;
		}
	}

	public static Strategy Create(Type type, StrategyConfig config)
	{
		if (config == null)
		{
			Debug.LogError("Strategy: Config cannot be null");
			return null;
		}
		if (type == null)
		{
			type = typeof(Strategy);
		}
		Strategy strategy = (Strategy)Activator.CreateInstance(type);
		strategy.SetupConfig(config);
		if (strategy.FactorSliderDefault != 0f && strategy.Factor == 0f)
		{
			strategy.factor = strategy.FactorSliderDefault;
		}
		return strategy;
	}

	public void SetupConfig(StrategyConfig cfg)
	{
		Config = cfg;
		for (int i = 0; i < cfg.Effects.Count; i++)
		{
			AddEffect(cfg.Effects[i]);
		}
	}

	public void AddEffect(StrategyEffectConfig cfg)
	{
		Type strategyEffectType = StrategySystem.GetStrategyEffectType(cfg.Name);
		if (strategyEffectType == null)
		{
			Debug.LogError("Strategy: Cannot add effect '" + cfg.Name + "' as it does not exist.");
			return;
		}
		StrategyEffect strategyEffect = (StrategyEffect)Activator.CreateInstance(strategyEffectType, this);
		if (strategyEffect != null)
		{
			strategyEffect.LoadFromConfig(cfg.Config);
			effects.Add(strategyEffect);
		}
	}

	public float FactorLerp(float minValue, float maxValue)
	{
		return Mathf.Lerp(minValue, maxValue, Factor);
	}

	public double FactorLerp(double minValue, double maxValue)
	{
		return Mathfx.Lerp(minValue, maxValue, Factor);
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnRegister()
	{
	}

	public virtual void OnUnregister()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual string MessageDeactivatedMaxTime()
	{
		return cacheAutoLOC_304551;
	}

	public virtual string GetText()
	{
		string text = "";
		text += RichTextUtil.Title(cacheAutoLOC_304558);
		text += Localizer.Format("#autoLOC_304559", Description);
		text += RichTextUtil.Title(cacheAutoLOC_304560);
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			StrategyEffect strategyEffect = effects[i];
			text = text + "<b><color=#" + RUIutils.ColorToHex(RichTextUtil.colorParams) + ">* " + strategyEffect.Description + "</color></b>\n";
		}
		text += "\n";
		if (isActive)
		{
			if (LeastDuration != 0.0)
			{
				text = ((!(LeastDuration - Planetarium.fetch.time > 0.0)) ? (text + KSPRichTextUtil.TextDateCompact(cacheAutoLOC_304641, LeastDuration)) : (text + RichTextUtil.TextParam(cacheAutoLOC_304637, cacheAutoLOC_6002417)));
			}
			if (LongestDuration != 0.0)
			{
				text += KSPRichTextUtil.TextDateCompact(cacheAutoLOC_304646, LongestDuration);
			}
		}
		else
		{
			if (LeastDuration != 0.0)
			{
				text += KSPRichTextUtil.TextDate(cacheAutoLOC_304654, LeastDuration);
			}
			if (LongestDuration != 0.0)
			{
				text += KSPRichTextUtil.TextDate(cacheAutoLOC_304658, LongestDuration);
			}
			string text2 = "";
			if (InitialCostFunds != 0f)
			{
				text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + InitialCostFunds.ToString("F0") + "   </color>";
			}
			if (InitialCostScience != 0f)
			{
				text2 = text2 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + InitialCostScience.ToString("F0") + "   </color>";
			}
			if (InitialCostReputation != 0f)
			{
				text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + InitialCostReputation.ToString("F0") + "   </color>";
			}
			if (text2 != string.Empty)
			{
				text += RichTextUtil.TextAdvance(cacheAutoLOC_304612, text2);
			}
		}
		return text;
	}

	public virtual string GetEffectText()
	{
		string text = "";
		text = text + "<b><color=" + XKCDColors.HexFormat.KSPNotSoGoodOrange + ">" + cacheAutoLOC_304560 + " </color></b>\n";
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			StrategyEffect strategyEffect = effects[i];
			text = text + "<b><color=#" + RUIutils.ColorToHex(RichTextUtil.colorParams) + ">* " + strategyEffect.Description + "</color></b>\n";
		}
		text += "\n";
		if (isActive)
		{
			if (LeastDuration != 0.0)
			{
				text = ((!(LeastDuration - Planetarium.fetch.time > 0.0)) ? (text + "\n" + KSPRichTextUtil.TextDateCompact(cacheAutoLOC_304641, LeastDuration)) : (text + "\n" + RichTextUtil.TextParam(cacheAutoLOC_304637, "True")));
			}
			if (LongestDuration != 0.0)
			{
				text = text + "\n" + KSPRichTextUtil.TextDateCompact(cacheAutoLOC_304646, LongestDuration);
			}
		}
		else
		{
			if (LeastDuration != 0.0)
			{
				text += KSPRichTextUtil.TextDate(cacheAutoLOC_304654, LeastDuration);
			}
			if (LongestDuration != 0.0)
			{
				text += KSPRichTextUtil.TextDate(cacheAutoLOC_304658, LongestDuration);
			}
			string text2 = "";
			if (InitialCostFunds != 0f)
			{
				text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + InitialCostFunds.ToString("F0") + "   </color>";
			}
			if (InitialCostScience != 0f)
			{
				text2 = text2 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + InitialCostScience.ToString("F0") + "   </color>";
			}
			if (InitialCostReputation != 0f)
			{
				text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + InitialCostReputation.ToString("F0") + "   </color>";
			}
			if (text2 != string.Empty)
			{
				text += RichTextUtil.TextAdvance(cacheAutoLOC_304674, text2);
			}
		}
		return text;
	}

	public virtual bool CanActivate(ref string reason)
	{
		return true;
	}

	public virtual bool CanDeactivate(ref string reason)
	{
		return true;
	}

	public void Load(ConfigNode node)
	{
		string value = node.GetValue("date");
		if (value != null)
		{
			dateActivated = double.Parse(value);
		}
		value = node.GetValue("factor");
		if (value != null)
		{
			factor = float.Parse(value);
		}
		OnLoad(node);
		List<ConfigNode> list = new List<ConfigNode>(node.GetNodes("EFFECT"));
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			ConfigNode configNode = null;
			string name = effects[i].GetType().Name;
			int count2 = list.Count;
			while (count2-- > 0)
			{
				if (list[count2].GetValue("name") == name)
				{
					configNode = list[count2];
					list.RemoveAt(count2);
					break;
				}
			}
			if (configNode != null)
			{
				effects[i].Load(configNode);
			}
		}
		isActive = true;
		Register();
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", Config.Name);
		node.AddValue("date", dateActivated.ToString("G17"));
		node.AddValue("factor", factor.ToString("G9"));
		OnSave(node);
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Save(node.AddNode("EFFECT"));
		}
	}

	public void Register()
	{
		OnRegister();
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Register();
		}
	}

	public void Unregister()
	{
		OnUnregister();
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Unregister();
		}
	}

	public bool Activate()
	{
		if (CanBeActivated(out var _))
		{
			isActive = true;
			Register();
			dateActivated = Planetarium.fetch.time;
			if (InitialCostFunds != 0f)
			{
				Funding.Instance.AddFunds(0f - Mathf.Abs(InitialCostFunds), TransactionReasons.StrategySetup);
			}
			if (InitialCostReputation != 0f)
			{
				Reputation.Instance.AddReputation(0f - Mathf.Abs(InitialCostReputation), TransactionReasons.StrategySetup);
			}
			if (InitialCostScience != 0f)
			{
				ResearchAndDevelopment.Instance.AddScience(0f - Mathf.Abs(InitialCostScience), TransactionReasons.StrategySetup);
			}
			return true;
		}
		return false;
	}

	public bool Deactivate()
	{
		if (CanBeDeactivated(out var _))
		{
			isActive = false;
			Unregister();
			return true;
		}
		return false;
	}

	public bool CanBeActivated(out string reason)
	{
		reason = "";
		if (Administration.Instance.ActiveStrategyCount >= Administration.Instance.MaxActiveStrategies)
		{
			reason = Localizer.Format("#autoLOC_304820", Administration.Instance.MaxActiveStrategies);
			return false;
		}
		if (StrategySystem.Instance.HasConflictingActiveStrategies(GroupTags))
		{
			reason = cacheAutoLOC_304827;
			return false;
		}
		if (Factor > Administration.Instance.MaxStrategyCommitLevel)
		{
			reason = Localizer.Format("#autoLOC_304834", Administration.Instance.MaxStrategyCommitLevel * 100f);
			return false;
		}
		if ((double)InitialCostFunds != 0.0 && Funding.Instance != null && Funding.Instance.Funds < (double)InitialCostFunds)
		{
			reason = cacheAutoLOC_304841;
			return false;
		}
		if (InitialCostReputation != 0f && Reputation.Instance != null && Reputation.Instance.reputation < InitialCostReputation)
		{
			reason = cacheAutoLOC_304848;
			return false;
		}
		if (InitialCostScience != 0f && ResearchAndDevelopment.Instance != null && !ResearchAndDevelopment.CanAfford(InitialCostScience))
		{
			reason = cacheAutoLOC_304855;
			return false;
		}
		if (RequiredReputation != 0f && Mathf.Ceil(Reputation.CurrentRep) < Mathf.Floor(RequiredReputation))
		{
			reason = Localizer.Format("#autoLOC_304862", RequiredReputation.ToString("N0"), Reputation.CurrentRep.ToString("N0"));
			return false;
		}
		if (!CanActivate(ref reason))
		{
			return false;
		}
		int count = effects.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (effects[count].CanActivate(ref reason));
		return false;
	}

	public bool CanBeDeactivated(out string reason)
	{
		reason = cacheAutoLOC_304887;
		if (LeastDuration != 0.0 && dateActivated + LeastDuration < Planetarium.fetch.time)
		{
			reason = cacheAutoLOC_304890;
			return false;
		}
		if (!CanDeactivate(ref reason))
		{
			return false;
		}
		return true;
	}

	public void Update()
	{
		if (LongestDuration != 0.0 && dateActivated + LongestDuration <= Planetarium.fetch.time)
		{
			SendStateMessage(cacheAutoLOC_304909, MessageDeactivatedMaxTime(), MessageSystemButton.MessageButtonColor.ORANGE, MessageSystemButton.ButtonIcons.ALERT);
			Deactivate();
			return;
		}
		OnUpdate();
		int i = 0;
		for (int count = effects.Count; i < count; i++)
		{
			effects[i].Update();
		}
	}

	public void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, "<b>" + Title + "</>\n\n" + message, color, icon));
		}
		Debug.Log("Strategy (" + Title + "): " + message);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_304551 = Localizer.Format("#autoLOC_304551");
		cacheAutoLOC_304558 = Localizer.Format("#autoLOC_304558");
		cacheAutoLOC_304560 = Localizer.Format("#autoLOC_304560");
		cacheAutoLOC_304637 = Localizer.Format("#autoLOC_304637");
		cacheAutoLOC_6002417 = Localizer.Format("#autoLOC_6002417");
		cacheAutoLOC_304641 = Localizer.Format("#autoLOC_304641");
		cacheAutoLOC_304646 = Localizer.Format("#autoLOC_304646");
		cacheAutoLOC_304654 = Localizer.Format("#autoLOC_304654");
		cacheAutoLOC_304658 = Localizer.Format("#autoLOC_304658");
		cacheAutoLOC_304612 = Localizer.Format("#autoLOC_304612");
		cacheAutoLOC_304674 = Localizer.Format("#autoLOC_304674");
		cacheAutoLOC_304827 = Localizer.Format("#autoLOC_304827");
		cacheAutoLOC_304841 = Localizer.Format("#autoLOC_304841");
		cacheAutoLOC_304848 = Localizer.Format("#autoLOC_304848");
		cacheAutoLOC_304855 = Localizer.Format("#autoLOC_304855");
		cacheAutoLOC_304887 = Localizer.Format("#autoLOC_304887");
		cacheAutoLOC_304890 = Localizer.Format("#autoLOC_304890");
		cacheAutoLOC_304909 = Localizer.Format("#autoLOC_304909");
	}
}
