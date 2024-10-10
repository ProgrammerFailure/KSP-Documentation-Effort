namespace KSPAchievements;

public class VesselRef : IConfigNode
{
	public string name;

	public string flagURL;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string FlagURL
	{
		get
		{
			return flagURL;
		}
		set
		{
			flagURL = value;
		}
	}

	public VesselRef()
	{
		name = "";
		flagURL = "";
	}

	public static VesselRef FromVessel(Vessel v)
	{
		VesselRef vesselRef = new VesselRef();
		if (v == null)
		{
			return vesselRef;
		}
		if (v.loaded)
		{
			vesselRef.Name = v.vesselName;
			if (v.rootPart != null)
			{
				vesselRef.flagURL = v.rootPart.flagURL;
			}
		}
		else if (v.protoVessel != null)
		{
			vesselRef.Name = v.protoVessel.vesselName;
			if (v.protoVessel.protoPartSnapshots[0] != null)
			{
				vesselRef.flagURL = v.protoVessel.protoPartSnapshots[0].flagURL;
			}
		}
		return vesselRef;
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("name"))
		{
			name = node.GetValue("name");
		}
		if (node.HasValue("flag"))
		{
			flagURL = node.GetValue("flag");
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("flag", flagURL);
	}
}
