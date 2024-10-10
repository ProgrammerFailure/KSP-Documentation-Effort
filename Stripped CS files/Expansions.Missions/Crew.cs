using System;
using System.Collections.Generic;
using ns9;

namespace Expansions.Missions;

[Serializable]
public class Crew : IConfigNode
{
	public uint partPersistentID;

	public uint vesselPersistentID;

	public List<string> crewNames;

	public void Load(ConfigNode node)
	{
		crewNames = new List<string>();
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "crew":
				crewNames.Add(value.value);
				break;
			case "vesselPersistentId":
				vesselPersistentID = uint.Parse(value.value);
				break;
			case "partPersistentId":
				partPersistentID = uint.Parse(value.value);
				break;
			}
		}
	}

	public string LocalizeCrewMember(int nameIndex)
	{
		string text = "";
		return crewNames[nameIndex] switch
		{
			"Valentina Kerman" => Localizer.Format("#autoLOC_1100005"), 
			"Bob Kerman" => Localizer.Format("#autoLOC_1100004"), 
			"Bill Kerman" => Localizer.Format("#autoLOC_1100003"), 
			"Jebediah Kerman" => Localizer.Format("#autoLOC_1100002"), 
			_ => crewNames[nameIndex], 
		};
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("partPersistentId", partPersistentID);
		node.AddValue("vesselPersistentId", vesselPersistentID);
		int i = 0;
		for (int count = crewNames.Count; i < count; i++)
		{
			node.AddValue("crew", crewNames[i]);
		}
	}
}
