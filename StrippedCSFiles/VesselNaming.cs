using System.Runtime.CompilerServices;

public class VesselNaming : IConfigNode
{
	public string vesselName;

	public VesselType vesselType;

	public int namingPriority;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselNaming()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselNaming(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindPriorityNamePart(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindPriorityNamePart(ShipConstruct ship)
	{
		throw null;
	}
}
