public class VesselNaming : IConfigNode
{
	public string vesselName;

	public VesselType vesselType;

	public int namingPriority;

	public VesselNaming()
	{
		vesselType = VesselType.Ship;
	}

	public VesselNaming(ConfigNode node)
	{
		Load(node);
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("name", ref vesselName);
		node.TryGetEnum("type", ref vesselType, VesselType.Ship);
		node.TryGetValue("priority", ref namingPriority);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", vesselName);
		node.AddValue("type", vesselType);
		node.AddValue("priority", namingPriority);
	}

	public static Part FindPriorityNamePart(Vessel vessel)
	{
		Part result = null;
		int num = 0;
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			VesselNaming vesselNaming = vessel.parts[i].vesselNaming;
			if (vesselNaming != null && vesselNaming.namingPriority > num)
			{
				num = vesselNaming.namingPriority;
				result = vessel.parts[i];
			}
		}
		return result;
	}

	public static Part FindPriorityNamePart(ShipConstruct ship)
	{
		Part result = null;
		int num = 0;
		for (int i = 0; i < ship.parts.Count; i++)
		{
			VesselNaming vesselNaming = ship.parts[i].vesselNaming;
			if (vesselNaming != null && vesselNaming.namingPriority > num)
			{
				num = vesselNaming.namingPriority;
				result = ship.parts[i];
			}
		}
		return result;
	}
}
