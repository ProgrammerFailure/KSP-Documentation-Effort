using System;
using FinePrint;

public class ProtoWaypointInfo : IConfigNode
{
	public enum Type
	{
		Custom,
		Survey,
		Null
	}

	public string name;

	public Guid navigationId;

	public Type waypointType;

	public ProtoWaypointInfo(ProtoWaypointInfo pWI)
	{
		name = pWI.name;
		navigationId = pWI.navigationId;
		waypointType = pWI.waypointType;
	}

	public ProtoWaypointInfo(Waypoint wp)
	{
		if (wp != null)
		{
			waypointType = Type.Custom;
			navigationId = wp.navigationId;
		}
		else
		{
			waypointType = Type.Null;
			navigationId = Guid.Empty;
		}
	}

	public ProtoWaypointInfo()
	{
		waypointType = Type.Null;
	}

	public ProtoWaypointInfo(ConfigNode node)
	{
		waypointType = Type.Null;
		Load(node);
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("type"))
		{
			waypointType = (Type)Enum.Parse(typeof(Type), node.GetValue("type"));
		}
		if (node.HasValue("navigationID"))
		{
			navigationId = new Guid(node.GetValue("navigationID"));
		}
	}

	public void Save(ConfigNode node)
	{
		if (waypointType != Type.Null)
		{
			node.AddValue("type", waypointType.ToString());
			node.AddValue("navigationID", navigationId.ToString());
		}
	}

	public Waypoint FindWaypoint()
	{
		return waypointType switch
		{
			Type.Custom => WaypointManager.FindWaypoint(navigationId), 
			Type.Survey => WaypointManager.FindWaypoint(navigationId), 
			Type.Null => null, 
			_ => null, 
		};
	}
}
