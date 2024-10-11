using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class ReachSpeedEnvelope : ContractParameter
{
	public double minSpeed;

	public double maxSpeed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachSpeedEnvelope()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachSpeedEnvelope(float minSpd, float maxSpd)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkVesselWithinEnvelope(Vessel v)
	{
		throw null;
	}
}
