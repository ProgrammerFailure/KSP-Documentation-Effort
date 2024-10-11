using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class PartTest : ContractParameter
{
	public string partName;

	public AvailablePart tgtPartInfo;

	public List<uint> testedParts;

	public string testNotes;

	public string body;

	public string situation;

	public string uniqueID;

	public PartTestConstraint.TestRepeatability repeatability;

	public bool hauled;

	private bool eventsAdded;

	private bool validVessel;

	private bool dirtyVessel;

	private int successCounter;

	private int activePartCount;

	public string title;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartTest(AvailablePart tgtPart, string testNotes, PartTestConstraint.TestRepeatability partRepeatability, CelestialBody tgtBody, Vessel.Situations tgtSit, string id, bool hauled)
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
	private void OnPartRunTest(ModuleTestSubject p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTestPartDestroyed(ModuleTestSubject p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool hasTestedPart(Vessel v)
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
	protected override void OnUpdate()
	{
		throw null;
	}
}
