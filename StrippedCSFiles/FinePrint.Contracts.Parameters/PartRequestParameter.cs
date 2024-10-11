using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Contracts.Parameters;

public class PartRequestParameter : ContractParameter
{
	private int successCounter;

	private string article;

	private string partDescription;

	private string vesselDescription;

	private List<string> partNames;

	private List<string> moduleNames;

	private List<uint> existingParts;

	private bool validVessel;

	private bool dirtyVessel;

	private int activePartCount;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartRequestParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartRequestParameter(ConfigNode requestNode, Vessel v = null)
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
	private void CheckLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VesselHasPartRequest(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScanVesselForExistingParts(Vessel v)
	{
		throw null;
	}
}
