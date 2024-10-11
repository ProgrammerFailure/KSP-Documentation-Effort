using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class RecoverPart : ContractParameter
{
	public enum CompleteCondition
	{
		None,
		AllCandidates,
		AnyCandidate
	}

	public List<uint> partsToRecover;

	public string title;

	public CompleteCondition failCondition;

	public CompleteCondition winCondition;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RecoverPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RecoverPart(string title, CompleteCondition winCondition, CompleteCondition failCondition)
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
	private void OnVesselRecovered(ProtoVessel v, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDie(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartToRecover(uint partFlightId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartToRecover(uint partFlightId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselLoad(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlightReady()
	{
		throw null;
	}
}
