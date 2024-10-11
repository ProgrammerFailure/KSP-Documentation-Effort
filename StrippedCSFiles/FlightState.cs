using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class FlightState
{
	public const int lastCompatibleMajor = 0;

	public const int lastCompatibleMinor = 18;

	public const int lastCompatibleRev = 0;

	public bool compatible;

	public List<ProtoVessel> protoVessels;

	public Dictionary<string, KSPParseable> sceneStateValues;

	public int file_version_major;

	public int file_version_minor;

	public int file_version_revision;

	public double universalTime;

	public int activeVesselIdx;

	public int mapViewFilterState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightState(ConfigNode rootNode, Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode rootNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsFlightID(uint id)
	{
		throw null;
	}
}
