using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions;

public class VesselCrewManifest
{
	private Dictionary<uint, PartCrewManifest> partLookup;

	private Dictionary<string, PartCrewManifest> crewLookup;

	private List<PartCrewManifest> partManifests;

	private float crewCountOptimizedForFloat;

	public List<PartCrewManifest> PartManifests
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CrewCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float CrewCountOptimizedForFloat
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int PartCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselCrewManifest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselCrewManifest FromConfigNode(ConfigNode craftNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselCrewManifest CloneOf(VesselCrewManifest original, bool blank)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void MergeInto(VesselCrewManifest m1, VesselCrewManifest m2, Func<PartCrewManifest, bool> inclusionFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoCrewMember> GetAllCrew(bool includeNulls)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ProtoCrewMember> GetAllCrew(bool includeNulls, KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCrewInventoryCosts(KerbalRoster roster = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCrewInventoryMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCrewInventoryMass(KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCrewResourceMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCrewResourceMass(KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartCrewManifest> GetCrewableParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartManifestNoOverwrite(uint id, PartCrewManifest newPCMtoUse)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartManifest(uint id, PartCrewManifest newPCMtoUse)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePartManifest(uint id, PartCrewManifest referencePCM)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartManifest(PartCrewManifest pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCrewMember(string cName, PartCrewManifest pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrewMember(string cName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartCrewManifest GetPartCrewManifest(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartCrewManifest GetPartForCrew(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DebugManifest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Filter(Func<PartCrewManifest, bool> inclusionFilter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasAnyCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasAnyCrew(KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyCrewInState(ProtoCrewMember.RosterStatus state, bool notInState = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyCrewInState(ProtoCrewMember.RosterStatus state, KerbalRoster roster, bool notInState = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyCrewWithTrait(string trait, bool noCrewWithTrait = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyCrewWithTrait(string trait, KerbalRoster roster, bool noCrewWithTrait = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssignCrewToVessel(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssignCrewToVessel(ShipConstruct ship, KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselCrewManifest UpdateCrewForVessel(ConfigNode vesselNode, Func<PartCrewManifest, bool> persistFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddCrewMembers(ref List<Crew> vesselCrew, KerbalRoster roster)
	{
		throw null;
	}
}
