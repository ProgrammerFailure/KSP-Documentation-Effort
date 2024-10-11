using System.Runtime.CompilerServices;

public class DockedVesselInfo : IConfigNode
{
	public enum OldVesselType
	{
		Debris,
		SpaceObject,
		Unknown,
		Probe,
		Rover,
		Lander,
		Ship,
		Station,
		Base,
		EVA,
		Flag,
		Plane,
		Relay,
		DeployedScienceController,
		DeployedSciencePart
	}

	public string name;

	public uint rootPartUId;

	public VesselType vesselType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DockedVesselInfo()
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
}
