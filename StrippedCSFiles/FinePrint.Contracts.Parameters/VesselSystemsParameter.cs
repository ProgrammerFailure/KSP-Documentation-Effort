using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using FinePrint.Utilities;

namespace FinePrint.Contracts.Parameters;

public class VesselSystemsParameter : ContractParameter
{
	public bool requireNew;

	private List<string> checkModuleTypes;

	private List<string> checkModuleDescriptions;

	private MannedStatus mannedStatus;

	private string vesselDescription;

	public uint launchID;

	private int successCounter;

	private bool validVessel;

	private bool dirtyVessel;

	private int activePartCount;

	private bool eventsAdded;

	private string mannedString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSystemsParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSystemsParameter(List<string> checkModuleTypes, List<string> checkModuleDescriptions, string vesselDescription, MannedStatus mannedStatus = MannedStatus.ANY, bool requireNew = true)
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
	private bool isVesselValid(Vessel v)
	{
		throw null;
	}
}
