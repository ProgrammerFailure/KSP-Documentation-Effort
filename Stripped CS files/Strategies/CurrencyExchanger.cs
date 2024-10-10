using System;
using ns11;
using ns9;
using UnityEngine;

namespace Strategies;

public class CurrencyExchanger : StrategyEffect
{
	public float minRate;

	public float maxRate;

	public float minShare;

	public float maxShare;

	public Currency input;

	public Currency output;

	public string effectDescription;

	public CurrencyExchanger(Strategy parent, float minRate, float maxRate, float minShare, float maxShare, Currency input, Currency output)
		: base(parent)
	{
		this.minRate = minRate;
		this.maxRate = maxRate;
		this.minShare = minShare;
		this.maxShare = maxShare;
		this.input = input;
		this.output = output;
		if (input == output)
		{
			Debug.LogError("[CurrencyConverter]: Input and Output currencies must not be the same!");
		}
	}

	public CurrencyExchanger(Strategy parent)
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
		if (input == output)
		{
			Debug.LogError("[CurrencyExchanger]: Input and Output currencies must not be the same!");
		}
	}

	public override string GetDescription()
	{
		double totalInput = GetTotalInput(ParentLerp(minShare, maxShare));
		return Localizer.Format("#autoLOC_6002354", GetTotalOutput(totalInput, ParentLerp(minRate, maxRate)).ToString("0.###"), output.displayDescription(), totalInput.ToString("0.##"), input.displayDescription(), Localizer.Format("#autoLOC_303823"));
	}

	public double GetTotalInput(float shareLerp)
	{
		double result = 0.0;
		switch (input)
		{
		case Currency.Funds:
			result = Funding.Instance.Funds * (double)shareLerp;
			break;
		case Currency.Science:
			result = ResearchAndDevelopment.Instance.Science * shareLerp;
			break;
		case Currency.Reputation:
			result = (Reputation.Instance.reputation + 1000f) * shareLerp;
			break;
		}
		return result;
	}

	public double GetTotalOutput(double input, double rateLerp)
	{
		return input * rateLerp;
	}

	public override void OnRegister()
	{
		double totalInput = GetTotalInput(ParentLerp(minShare, maxShare));
		double totalOutput = GetTotalOutput(totalInput, ParentLerp(minRate, maxRate));
		switch (input)
		{
		case Currency.Funds:
			Funding.Instance.AddFunds(0.0 - totalInput, TransactionReasons.StrategyInput);
			break;
		case Currency.Science:
			ResearchAndDevelopment.Instance.AddScience((float)(0.0 - totalInput), TransactionReasons.StrategyInput);
			break;
		case Currency.Reputation:
			Reputation.Instance.AddReputation((float)(0.0 - totalInput), TransactionReasons.StrategyInput);
			break;
		}
		switch (output)
		{
		case Currency.Funds:
			Funding.Instance.AddFunds(totalOutput, TransactionReasons.StrategyOutput);
			break;
		case Currency.Science:
			ResearchAndDevelopment.Instance.AddScience((float)totalOutput, TransactionReasons.StrategyOutput);
			break;
		case Currency.Reputation:
			Reputation.Instance.AddReputation((float)totalOutput, TransactionReasons.StrategyOutput);
			break;
		}
		StrategySystem.Instance.StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			base.Parent.Deactivate();
			Administration.Instance.RedrawPanels();
		}));
	}

	public override void OnUnregister()
	{
	}

	public override bool CanActivate(ref string reason)
	{
		return true;
	}
}
