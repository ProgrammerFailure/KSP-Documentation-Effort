using System.Runtime.CompilerServices;

public class AttachNodeSnapshot
{
	public string id;

	public int partIdx;

	public string srfAttachMeshName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNodeSnapshot(AttachNode node, ProtoVessel pVesselRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNodeSnapshot(string attachNodeString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string Save()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part findAttachedPart(Vessel vessel)
	{
		throw null;
	}
}
