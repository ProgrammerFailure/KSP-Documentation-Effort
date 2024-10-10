using System;

[Serializable]
public class VesselRanges : IConfigNode
{
	[Serializable]
	public class Situation : IConfigNode
	{
		public float load;

		public float unload;

		public float pack;

		public float unpack;

		public Situation(float load, float unload, float pack, float unpack)
		{
			this.load = load;
			this.unload = unload;
			this.pack = pack;
			this.unpack = unpack;
		}

		public Situation(Situation copyFrom)
		{
			load = copyFrom.load;
			unload = copyFrom.unload;
			pack = copyFrom.pack;
			unpack = copyFrom.unpack;
		}

		public void Load(ConfigNode node)
		{
			int count = node.values.Count;
			while (count-- > 0)
			{
				ConfigNode.Value value = node.values[count];
				switch (value.name)
				{
				case "unpack":
					unpack = float.Parse(value.value);
					break;
				case "pack":
					pack = float.Parse(value.value);
					break;
				case "unload":
					unload = float.Parse(value.value);
					break;
				case "load":
					load = float.Parse(value.value);
					break;
				}
			}
		}

		public void Save(ConfigNode node)
		{
			node.SetValue("load", load, createIfNotFound: true);
			node.SetValue("unload", unload, createIfNotFound: true);
			node.SetValue("pack", pack, createIfNotFound: true);
			node.SetValue("unpack", unpack, createIfNotFound: true);
		}
	}

	public Situation prelaunch = new Situation(2250f, 2500f, 350f, 200f);

	public Situation landed = new Situation(2250f, 2500f, 350f, 200f);

	public Situation splashed = new Situation(2250f, 2500f, 350f, 200f);

	public Situation flying = new Situation(2250f, 22500f, 25000f, 2000f);

	public Situation subOrbital = new Situation(2250f, 15000f, 10000f, 200f);

	public Situation orbit = new Situation(2250f, 2500f, 350f, 200f);

	public Situation escaping = new Situation(2250f, 2500f, 350f, 200f);

	public VesselRanges()
	{
	}

	public VesselRanges(VesselRanges copyFrom)
	{
		prelaunch = new Situation(copyFrom.prelaunch);
		landed = new Situation(copyFrom.landed);
		splashed = new Situation(copyFrom.splashed);
		flying = new Situation(copyFrom.flying);
		orbit = new Situation(copyFrom.orbit);
		subOrbital = new Situation(copyFrom.subOrbital);
		escaping = new Situation(copyFrom.escaping);
	}

	public Situation GetSituationRanges(Vessel.Situations situation)
	{
		return situation switch
		{
			Vessel.Situations.FLYING => flying, 
			Vessel.Situations.LANDED => landed, 
			Vessel.Situations.SPLASHED => splashed, 
			Vessel.Situations.PRELAUNCH => prelaunch, 
			Vessel.Situations.ESCAPING => escaping, 
			Vessel.Situations.SUB_ORBITAL => subOrbital, 
			_ => orbit, 
		};
	}

	public void Load(ConfigNode node)
	{
		int count = node.nodes.Count;
		while (count-- > 0)
		{
			ConfigNode configNode = node.nodes[count];
			switch (configNode.name)
			{
			case "flying":
				flying.Load(configNode);
				break;
			case "escaping":
				escaping.Load(configNode);
				break;
			case "subOrbital":
				subOrbital.Load(configNode);
				break;
			case "landed":
				landed.Load(configNode);
				break;
			case "prelaunch":
				prelaunch.Load(configNode);
				break;
			case "orbit":
				orbit.Load(configNode);
				break;
			case "splashed":
				splashed.Load(configNode);
				break;
			}
		}
	}

	public void Save(ConfigNode node)
	{
		prelaunch.Save(node.AddNode("prelaunch"));
		landed.Save(node.AddNode("landed"));
		splashed.Save(node.AddNode("splashed"));
		flying.Save(node.AddNode("flying"));
		orbit.Save(node.AddNode("orbit"));
		subOrbital.Save(node.AddNode("subOrbital"));
		escaping.Save(node.AddNode("escaping"));
	}
}
