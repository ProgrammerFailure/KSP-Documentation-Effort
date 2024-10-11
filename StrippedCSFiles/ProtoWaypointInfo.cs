using System;
using System.Runtime.CompilerServices;
using FinePrint;

public class ProtoWaypointInfo : IConfigNode
{
	private enum Type
	{
		Custom,
		Survey,
		Null
	}

	public string name;

	public Guid navigationId;

	private Type waypointType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoWaypointInfo(ProtoWaypointInfo pWI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoWaypointInfo(Waypoint wp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoWaypointInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoWaypointInfo(ConfigNode node)
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
	public Waypoint FindWaypoint()
	{
		throw null;
	}
}
