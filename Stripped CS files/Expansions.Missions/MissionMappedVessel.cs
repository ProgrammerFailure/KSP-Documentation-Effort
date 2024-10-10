using System;
using System.Collections.Generic;

namespace Expansions.Missions;

[Serializable]
public class MissionMappedVessel
{
	public uint partPersistentId;

	public uint mappedVesselPersistentId;

	public uint currentVesselPersistentId;

	public uint originalVesselPersistentId;

	public string partVesselName;

	public string craftFileName;

	public string situationVesselName;

	public MissionMappedVessel(uint partPersistentId, uint originalVesselId, uint currentVesselId, string partVslName, string craftFileName, string situationVslName = "")
	{
		mappedVesselPersistentId = FlightGlobals.GetUniquepersistentId();
		currentVesselPersistentId = currentVesselId;
		originalVesselPersistentId = originalVesselId;
		this.partPersistentId = partPersistentId;
		partVesselName = partVslName;
		this.craftFileName = craftFileName;
		situationVesselName = situationVslName;
	}

	public static List<MissionMappedVessel> Load(ConfigNode node)
	{
		List<MissionMappedVessel> list = new List<MissionMappedVessel>();
		ConfigNode[] nodes = node.GetNodes("MAPPEDVESSEL");
		foreach (ConfigNode configNode in nodes)
		{
			uint value = 0u;
			if (!configNode.TryGetValue("partId", ref value))
			{
				continue;
			}
			uint value2 = 0u;
			uint value3 = 0u;
			uint num = 0u;
			string value4 = "";
			string value5 = "";
			string value6 = "";
			if (configNode.TryGetValue("mappedId", ref value2) && configNode.TryGetValue("currentId", ref value3))
			{
				num = value3;
				if (configNode.TryGetValue("partVslName", ref value4))
				{
					configNode.TryGetValue("originalId", ref num);
					configNode.TryGetValue("situationVslName", ref value6);
					configNode.TryGetValue("craftFileName", ref value5);
					MissionMappedVessel missionMappedVessel = new MissionMappedVessel(value, num, value3, value4, value5, value6);
					missionMappedVessel.mappedVesselPersistentId = value2;
					list.Add(missionMappedVessel);
				}
			}
		}
		return list;
	}

	public static ConfigNode Save(ConfigNode node, List<MissionMappedVessel> list)
	{
		if (list.Count > 0)
		{
			ConfigNode configNode = node.AddNode("MAPPEDVESSELS");
			for (int i = 0; i < list.Count; i++)
			{
				ConfigNode configNode2 = configNode.AddNode("MAPPEDVESSEL");
				configNode2.AddValue("partId", list[i].partPersistentId);
				configNode2.AddValue("mappedId", list[i].mappedVesselPersistentId);
				configNode2.AddValue("originalId", list[i].originalVesselPersistentId);
				configNode2.AddValue("currentId", list[i].currentVesselPersistentId);
				configNode2.AddValue("partVslName", list[i].partVesselName);
				configNode2.AddValue("craftFileName", list[i].craftFileName);
				configNode2.AddValue("situationVslName", list[i].situationVesselName);
			}
		}
		return node;
	}
}
