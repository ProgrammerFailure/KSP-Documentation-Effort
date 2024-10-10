using System;

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
		const_9,
		Flag,
		Plane,
		Relay,
		DeployedScienceController,
		DeployedSciencePart
	}

	public string name;

	public uint rootPartUId;

	public VesselType vesselType;

	public void Load(ConfigNode node)
	{
		if (node.HasValue("vesselName"))
		{
			name = node.GetValue("vesselName");
		}
		if (node.HasValue("rootUId"))
		{
			rootPartUId = uint.Parse(node.GetValue("rootUId"));
		}
		if (!node.HasValue("vesselType"))
		{
			return;
		}
		if (int.TryParse(node.GetValue("vesselType"), out var result))
		{
			switch ((OldVesselType)result)
			{
			case OldVesselType.Debris:
				vesselType = VesselType.Debris;
				break;
			case OldVesselType.SpaceObject:
				vesselType = VesselType.SpaceObject;
				break;
			case OldVesselType.Unknown:
				vesselType = VesselType.Unknown;
				break;
			case OldVesselType.Probe:
				vesselType = VesselType.Probe;
				break;
			case OldVesselType.Rover:
				vesselType = VesselType.Rover;
				break;
			case OldVesselType.Lander:
				vesselType = VesselType.Lander;
				break;
			default:
				vesselType = VesselType.Ship;
				break;
			case OldVesselType.Station:
				vesselType = VesselType.Station;
				break;
			case OldVesselType.Base:
				vesselType = VesselType.Base;
				break;
			case OldVesselType.const_9:
				vesselType = VesselType.const_11;
				break;
			case OldVesselType.Flag:
				vesselType = VesselType.Flag;
				break;
			case OldVesselType.Plane:
				vesselType = VesselType.Plane;
				break;
			case OldVesselType.Relay:
				vesselType = VesselType.Relay;
				break;
			case OldVesselType.DeployedScienceController:
				vesselType = VesselType.DeployedScienceController;
				break;
			case OldVesselType.DeployedSciencePart:
				vesselType = VesselType.DeployedSciencePart;
				break;
			}
		}
		else
		{
			vesselType = (VesselType)Enum.Parse(typeof(VesselType), node.GetValue("vesselType"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("vesselName", name);
		node.AddValue("vesselType", vesselType);
		node.AddValue("rootUId", rootPartUId);
	}
}
