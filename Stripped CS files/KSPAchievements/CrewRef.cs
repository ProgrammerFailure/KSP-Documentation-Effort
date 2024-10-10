using System.Collections.Generic;

namespace KSPAchievements;

public class CrewRef : IConfigNode
{
	public List<ProtoCrewMember> crews;

	public List<ProtoCrewMember> Crews
	{
		get
		{
			return crews;
		}
		set
		{
			crews = value;
		}
	}

	public bool HasAny => crews.Count > 0;

	public CrewRef()
	{
		crews = new List<ProtoCrewMember>();
	}

	public static CrewRef FromVessel(Vessel v)
	{
		CrewRef crewRef = new CrewRef();
		if (v == null)
		{
			return crewRef;
		}
		if (v.loaded)
		{
			crewRef.crews = new List<ProtoCrewMember>(v.GetVesselCrew());
		}
		else if (v.protoVessel != null)
		{
			crewRef.crews = new List<ProtoCrewMember>(v.protoVessel.GetVesselCrew());
		}
		return crewRef;
	}

	public static CrewRef FromProtoVessel(ProtoVessel pv)
	{
		CrewRef crewRef = new CrewRef();
		if (pv == null)
		{
			return crewRef;
		}
		crewRef.crews = new List<ProtoCrewMember>(pv.GetVesselCrew());
		return crewRef;
	}

	public void Load(ConfigNode node)
	{
		if (!node.HasValue("crews"))
		{
			return;
		}
		string[] array = node.GetValue("crews").Split(',');
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			int result = -1;
			string text = array[i].Trim();
			if (int.TryParse(text, out result))
			{
				ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[result];
				if (protoCrewMember != null)
				{
					crews.Add(protoCrewMember);
				}
			}
			else
			{
				ProtoCrewMember protoCrewMember2 = HighLogic.CurrentGame.CrewRoster[text];
				if (protoCrewMember2 != null)
				{
					crews.Add(protoCrewMember2);
				}
			}
		}
	}

	public void Save(ConfigNode node)
	{
		string text = string.Empty;
		int count = crews.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = crews[i];
			if (protoCrewMember != null)
			{
				text += protoCrewMember.name;
				if (i < count - 1)
				{
					text += ", ";
				}
			}
		}
		node.AddValue("crews", text);
	}
}
