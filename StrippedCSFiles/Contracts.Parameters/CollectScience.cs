using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class CollectScience : ContractParameter
{
	protected CelestialBody targetBody;

	protected BodyLocation targetLocation;

	public CelestialBody TargetBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BodyLocation TargetLocation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectScience(CelestialBody targetBody, BodyLocation location)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
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
	private void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTriggeredScience(ScienceData data, Vessel origin, bool xmitAborted)
	{
		throw null;
	}
}
