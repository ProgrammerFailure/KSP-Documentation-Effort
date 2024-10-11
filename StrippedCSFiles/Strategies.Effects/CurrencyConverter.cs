using System.Runtime.CompilerServices;

namespace Strategies.Effects;

public class CurrencyConverter : StrategyEffect
{
	private float minRate;

	private float maxRate;

	private float minShare;

	private float maxShare;

	private Currency input;

	private Currency output;

	private TransactionReasons AffectReasons;

	private string effectDescription;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyConverter(Strategy parent, float minRate, float maxRate, float minShare, float maxShare, Currency input, Currency output, TransactionReasons affectReasons, string effectDescription)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyConverter(Strategy parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoadFromConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
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
	private void OnEffectQuery(CurrencyModifierQuery qry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanActivate(ref string reason)
	{
		throw null;
	}
}
