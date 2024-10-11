using System.Runtime.CompilerServices;

namespace Strategies;

public class CurrencyExchanger : StrategyEffect
{
	private float minRate;

	private float maxRate;

	private float minShare;

	private float maxShare;

	private Currency input;

	private Currency output;

	private string effectDescription;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyExchanger(Strategy parent, float minRate, float maxRate, float minShare, float maxShare, Currency input, Currency output)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyExchanger(Strategy parent)
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
	private double GetTotalInput(float shareLerp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetTotalOutput(double input, double rateLerp)
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
	public override bool CanActivate(ref string reason)
	{
		throw null;
	}
}
