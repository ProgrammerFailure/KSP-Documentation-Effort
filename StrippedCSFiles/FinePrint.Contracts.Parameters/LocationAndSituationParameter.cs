using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Contracts.Parameters;

public class LocationAndSituationParameter : ContractParameter
{
	private CelestialBody targetBody;

	private Vessel.Situations targetSituation;

	private string noun;

	private bool finalObjective;

	private bool validVessel;

	private bool dirtyVessel;

	private int successCounter;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LocationAndSituationParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LocationAndSituationParameter(CelestialBody targetBody, Vessel.Situations targetSituation, string noun, bool finalObjective = false)
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
	protected override void OnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeBody(GameEvents.HostedFromToAction<Vessel, CelestialBody> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> action)
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
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUpdate()
	{
		throw null;
	}
}
