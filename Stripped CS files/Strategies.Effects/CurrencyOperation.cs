using System;
using ns9;

namespace Strategies.Effects;

[Serializable]
public class CurrencyOperation : StrategyEffect
{
	public enum Operator
	{
		Add,
		Multiply,
		Set
	}

	public float minValue;

	public float maxValue;

	public Currency currency;

	public TransactionReasons AffectReasons;

	public string effectDescription;

	public Operator op;

	public CurrencyOperation(Strategy parent)
		: base(parent)
	{
	}

	public CurrencyOperation(Strategy parent, float minValue, float maxValue, Currency currency, Operator op, TransactionReasons AffectReasons, string description)
		: base(parent)
	{
		this.minValue = minValue;
		this.maxValue = maxValue;
		this.currency = currency;
		this.op = op;
		this.AffectReasons = AffectReasons;
		effectDescription = description;
	}

	public override string GetDescription()
	{
		return op switch
		{
			Operator.Add => Localizer.Format("#autoLOC_6002355", (ParentLerp(minValue, maxValue) > 0f) ? "+" : "", ParentLerp(minValue, maxValue).ToString("N1"), currency.displayDescription(), effectDescription), 
			Operator.Multiply => Localizer.Format("#autoLOC_6002356", ToPercentage(ParentLerp(minValue, maxValue), "0.##"), currency.displayDescription(), effectDescription), 
			Operator.Set => Localizer.Format("#autoLOC_303952", currency.displayDescription(), ParentLerp(minValue, maxValue).ToString("N1"), effectDescription), 
			_ => Localizer.Format("#autoLOC_303954", effectDescription), 
		};
	}

	public override void OnLoadFromConfig(ConfigNode node)
	{
		string value = node.GetValue("minValue");
		if (value != null)
		{
			minValue = float.Parse(value);
		}
		value = node.GetValue("maxValue");
		if (value != null)
		{
			maxValue = float.Parse(value);
		}
		if (node.HasValue("currency"))
		{
			currency = (Currency)Enum.Parse(typeof(Currency), node.GetValue("currency"));
		}
		if (node.HasValue("operation"))
		{
			op = (Operator)Enum.Parse(typeof(Operator), node.GetValue("operation"));
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
	}

	public override void OnRegister()
	{
		GameEvents.Modifiers.OnCurrencyModifierQuery.Add(OnEffectQuery);
	}

	public override void OnUnregister()
	{
		GameEvents.Modifiers.OnCurrencyModifierQuery.Remove(OnEffectQuery);
	}

	public virtual void OnEffectQuery(CurrencyModifierQuery qry)
	{
		if ((qry.reason & AffectReasons) != 0)
		{
			switch (op)
			{
			case Operator.Add:
				qry.AddDelta(currency, ParentLerp(minValue, maxValue));
				break;
			case Operator.Multiply:
				qry.AddDelta(currency, ParentLerp(minValue, maxValue) * qry.GetInput(currency) - qry.GetInput(currency));
				break;
			case Operator.Set:
				qry.AddDelta(currency, ParentLerp(minValue, maxValue) - qry.GetInput(currency));
				break;
			}
		}
	}
}
