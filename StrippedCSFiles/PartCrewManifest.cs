using System.Runtime.CompilerServices;

public class PartCrewManifest
{
	private AvailablePart partInfo;

	public VesselCrewManifest vcm;

	public string[] partCrew;

	private uint partID;

	public AvailablePart PartInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint PartID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartCrewManifest(VesselCrewManifest v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartCrewManifest FromConfigNode(ConfigNode node, VesselCrewManifest v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartCrewManifest CloneOf(PartCrewManifest original, VesselCrewManifest vcm, bool blank)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddCrewToSeat(ProtoCrewMember crew, int seatIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrewFromSeat(int seatIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrew(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember[] GetPartCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPartCrew(ref ProtoCrewMember[] crewArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewSeat(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewSeat(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnySeats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NoSeats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AllSeatsEmpty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AllSeatsEmpty(KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnySeatTaken()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnySeatTaken(KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CountCrewType(ProtoCrewMember.KerbalType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CountCrewType(ProtoCrewMember.KerbalType type, KerbalRoster roster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CountCrewNotType(ProtoCrewMember.KerbalType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CountCrewNotType(ProtoCrewMember.KerbalType type, KerbalRoster roster)
	{
		throw null;
	}
}
