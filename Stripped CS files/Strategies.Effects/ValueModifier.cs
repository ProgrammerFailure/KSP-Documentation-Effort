using System;
using ns9;

namespace Strategies.Effects;

[Serializable]
public class ValueModifier : StrategyEffect
{
	public float minValue;

	public float maxValue;

	public string valueId;

	public string effectDescription;

	public ValueModifier(Strategy parent)
		: base(parent)
	{
	}

	public ValueModifier(Strategy parent, string valueId, string description, float minValue, float maxValue)
		: base(parent)
	{
		this.valueId = valueId;
		effectDescription = description;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_6002357", ToPercentage(ParentLerp(minValue, maxValue), "0.##"), effectDescription);
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
		if (node.HasValue("valueId"))
		{
			valueId = node.GetValue("valueId");
		}
		if (node.HasValue("effectDescription"))
		{
			effectDescription = node.GetValue("effectDescription");
		}
	}

	public override void OnRegister()
	{
		GameEvents.Modifiers.onValueModifierQuery.Add(OnValueModifierQuery);
	}

	public override void OnUnregister()
	{
		GameEvents.Modifiers.onValueModifierQuery.Remove(OnValueModifierQuery);
	}

	public void OnValueModifierQuery(ValueModifierQuery q)
	{
		if (q.ValueId == valueId)
		{
			q.AddDelta(q.GetInput() * ParentLerp(minValue, maxValue) - q.GetInput());
		}
	}
}
