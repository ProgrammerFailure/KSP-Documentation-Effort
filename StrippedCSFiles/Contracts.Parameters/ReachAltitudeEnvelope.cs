using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class ReachAltitudeEnvelope : ContractParameter
{
	public double minAltitude;

	public double maxAltitude;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachAltitudeEnvelope()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachAltitudeEnvelope(float minAlt, float maxAlt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUpdate()
	{
		throw null;
	}
}
