using System;
using System.Runtime.CompilerServices;

namespace Strategies.Effects;

[Serializable]
public class ValueModifier : StrategyEffect
{
	private float minValue;

	private float maxValue;

	private string valueId;

	private string effectDescription;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValueModifier(Strategy parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValueModifier(Strategy parent, string valueId, string description, float minValue, float maxValue)
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
	private void OnValueModifierQuery(ValueModifierQuery q)
	{
		throw null;
	}
}
