using System;
using ns9;
using UnityEngine;

namespace Strategies.Effects;

public class CurrencyConverter : StrategyEffect
{
	public float minRate;

	public float maxRate;

	public float minShare;

	public float maxShare;

	public Currency input;

	public Currency output;

	public TransactionReasons AffectReasons;

	public string effectDescription;

	public CurrencyConverter(Strategy parent, float minRate, float maxRate, float minShare, float maxShare, Currency input, Currency output, TransactionReasons affectReasons, string effectDescription)
		: base(parent)
	{
		this.minRate = minRate;
		this.maxRate = maxRate;
		this.minShare = minShare;
		this.maxShare = maxShare;
		this.input = input;
		this.output = output;
		AffectReasons = affectReasons;
		this.effectDescription = effectDescription;
		if (input == output)
		{
			Debug.LogError("[CurrencyConverter]: Input and Output currencies must not be the same!");
		}
	}

	public CurrencyConverter(Strategy parent)
		: base(parent)
	{
	}

	public override void OnLoadFromConfig(ConfigNode node)
	{
		if (node.HasValue("input"))
		{
			input = (Currency)Enum.Parse(typeof(Currency), node.GetValue("input"));
		}
		if (node.HasValue("output"))
		{
			output = (Currency)Enum.Parse(typeof(Currency), node.GetValue("output"));
		}
		if (node.HasValue("maxRate"))
		{
			maxRate = float.Parse(node.GetValue("maxRate"));
		}
		if (node.HasValue("minRate"))
		{
			minRate = float.Parse(node.GetValue("minRate"));
		}
		if (node.HasValue("maxShare"))
		{
			maxShare = float.Parse(node.GetValue("maxShare"));
		}
		if (node.HasValue("minShare"))
		{
			minShare = float.Parse(node.GetValue("minShare"));
		}
		if (node.HasValue("AffectReasons"))
		{
			string[] array = node.GetValue("AffectReasons").Split(',');
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				AffectReasons |= (TransactionReasons)Enum.Parse(typeof(TransactionReasons), array[i].Trim());
			}
		}
		if (node.HasValue("effectDescription"))
		{
			effectDescription = node.GetValue("effectDescription");
		}
		if (input == output)
		{
			Debug.LogError("[CurrencyConverter]: Input and Output currencies must not be the same!");
		}
	}

	public override string GetDescription()
	{
		if (ParentLerp(minRate, maxRate) < 1f)
		{
			return Localizer.Format("#autoLOC_303660", (ParentLerp(minShare, maxShare) * 100f).ToString("0.##"), input.displayDescription(), effectDescription) + "\n" + Localizer.Format("#autoLOC_303661", output.displayDescription(), (1f / ParentLerp(minRate, maxRate)).ToString("0.###"), input.displayDescription());
		}
		return Localizer.Format("#autoLOC_303665", (ParentLerp(minShare, maxShare) * 100f).ToString("0.##"), input.displayDescription(), effectDescription) + "\n" + Localizer.Format("#autoLOC_303666", ParentLerp(minRate, maxRate).ToString("0.###"), output.displayDescription(), input.displayDescription());
	}

	public override void OnRegister()
	{
		GameEvents.Modifiers.OnCurrencyModifierQuery.Add(OnEffectQuery);
	}

	public override void OnUnregister()
	{
		GameEvents.Modifiers.OnCurrencyModifierQuery.Remove(OnEffectQuery);
	}

	public void OnEffectQuery(CurrencyModifierQuery qry)
	{
		if ((qry.reason & AffectReasons) != 0)
		{
			float num = qry.GetInput(input) * ParentLerp(minShare, maxShare);
			qry.AddDelta(input, 0f - num);
			qry.AddDelta(output, ParentLerp(minRate, maxRate) * num);
			Debug.Log("[CurrencyConverter for " + base.Parent.Title + "]: " + num + " " + input.ToString() + " taken, yields " + ParentLerp(minRate, maxRate) * num + " " + output);
		}
	}

	public override bool CanActivate(ref string reason)
	{
		if (StrategySystem.Instance != null)
		{
			float num = ParentLerp(minShare, maxShare);
			int count = StrategySystem.Instance.Strategies.Count;
			for (int i = 0; i < count; i++)
			{
				Strategy strategy = StrategySystem.Instance.Strategies[i];
				if (!strategy.IsActive)
				{
					continue;
				}
				int count2 = strategy.Effects.Count;
				for (int j = 0; j < count2; j++)
				{
					if (strategy.Effects[j] is CurrencyConverter currencyConverter && currencyConverter.input == input)
					{
						num += currencyConverter.ParentLerp(currencyConverter.minShare, currencyConverter.maxShare);
					}
				}
			}
			if (num <= 1f)
			{
				return true;
			}
			reason = Localizer.Format("#autoLOC_303729", input.displayDescription());
			return false;
		}
		return true;
	}
}
