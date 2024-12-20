using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Contracts.Parameters;

public class CometParameter : ContractParameter
{
	private bool forStation;

	private List<int> cometSeeds;

	private string cometClass;

	private int successCounter;

	private bool validVessel;

	private bool dirtyVessel;

	private int activePartCount;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometParameter(string size, bool forStation)
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
	protected override string GetNotes()
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
	private void VesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PartModified(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDock(GameEvents.FromToAction<Part, Part> action)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VesselTowingComet(Vessel v)
	{
		throw null;
	}
}
