using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ProtoTechNode
{
	public string techID;

	public RDTech.State state;

	public int scienceCost;

	public List<AvailablePart> partsPurchased;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTechNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTechNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromTechNode(RDTech nodeRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
