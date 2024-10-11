using System;
using System.Runtime.CompilerServices;

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

	private float minValue;

	private float maxValue;

	private Currency currency;

	private TransactionReasons AffectReasons;

	private string effectDescription;

	private Operator op;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyOperation(Strategy parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyOperation(Strategy parent, float minValue, float maxValue, Currency currency, Operator op, TransactionReasons AffectReasons, string description)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoadFromConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnEffectQuery(CurrencyModifierQuery qry)
	{
		throw null;
	}
}
