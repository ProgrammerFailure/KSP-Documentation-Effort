using System;
using UnityEngine;

public class PartCrewManifest
{
	public AvailablePart partInfo;

	public VesselCrewManifest vcm;

	public string[] partCrew;

	public uint partID;

	public AvailablePart PartInfo => partInfo;

	public uint PartID => partID;

	public PartCrewManifest(VesselCrewManifest v)
	{
		vcm = v;
	}

	public static PartCrewManifest FromConfigNode(ConfigNode node, VesselCrewManifest v)
	{
		PartCrewManifest partCrewManifest = new PartCrewManifest(v);
		string[] array = node.GetValue("part").Split('_');
		partCrewManifest.partInfo = PartLoader.getPartInfoByName(array[0]);
		if (partCrewManifest.partInfo == null)
		{
			return null;
		}
		int crewCapacity = partCrewManifest.partInfo.partPrefab.CrewCapacity;
		partCrewManifest.partCrew = new string[crewCapacity];
		partCrewManifest.partID = uint.Parse(array[1]);
		for (int i = 0; i < crewCapacity; i++)
		{
			partCrewManifest.partCrew[i] = "";
		}
		return partCrewManifest;
	}

	public static PartCrewManifest CloneOf(PartCrewManifest original, VesselCrewManifest vcm, bool blank)
	{
		PartCrewManifest partCrewManifest = new PartCrewManifest(vcm);
		partCrewManifest.partInfo = original.partInfo;
		int num = original.partCrew.Length;
		partCrewManifest.partCrew = new string[num];
		partCrewManifest.partID = original.partID;
		for (int i = 0; i < num; i++)
		{
			if (!blank)
			{
				partCrewManifest.partCrew[i] = original.partCrew[i];
				vcm.SetCrewMember(partCrewManifest.partCrew[i], partCrewManifest);
			}
			else
			{
				partCrewManifest.partCrew[i] = "";
			}
		}
		return partCrewManifest;
	}

	public void AddCrewToSeat(ProtoCrewMember crew, int seatIndex)
	{
		if (seatIndex >= 0 && seatIndex <= partInfo.partPrefab.CrewCapacity)
		{
			if (partCrew[seatIndex] != string.Empty)
			{
				Debug.LogError("[PartCrewManifest Error]: seat " + seatIndex + " on " + partInfo.name + " was occupied already.");
			}
			partCrew[seatIndex] = crew.name;
			vcm.SetCrewMember(crew.name, this);
			if ((HighLogic.CurrentGame != null && !HighLogic.CurrentGame.Parameters.Difficulty.persistKerbalInventories) || crew.InventoryNode == null)
			{
				crew.SetDefaultInventory();
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				GameEvents.onEditorShipCrewModified.Fire(vcm);
			}
			return;
		}
		throw new IndexOutOfRangeException("[PartCrewManifest Error]: seat index out of range: i = " + seatIndex + " while " + partInfo.name + " has " + partInfo.partPrefab.CrewCapacity + " seats");
	}

	public void RemoveCrewFromSeat(int seatIndex)
	{
		if (seatIndex >= 0 && seatIndex <= partInfo.partPrefab.CrewCapacity)
		{
			string cName = partCrew[seatIndex];
			partCrew[seatIndex] = string.Empty;
			vcm.RemoveCrewMember(cName);
			if (HighLogic.LoadedSceneIsEditor)
			{
				GameEvents.onEditorShipCrewModified.Fire(vcm);
			}
			return;
		}
		throw new IndexOutOfRangeException("[PartCrewManifest Error]: seat index out of range: i = " + seatIndex + " while part has " + partInfo.partPrefab.CrewCapacity + " seats");
	}

	public void RemoveCrew(string name)
	{
		int crewSeat = GetCrewSeat(name);
		if (crewSeat != -1)
		{
			RemoveCrewFromSeat(crewSeat);
		}
	}

	public ProtoCrewMember[] GetPartCrew()
	{
		ProtoCrewMember[] crewArray = new ProtoCrewMember[partCrew.Length];
		GetPartCrew(ref crewArray);
		return crewArray;
	}

	public void GetPartCrew(ref ProtoCrewMember[] crewArray)
	{
		if (crewArray == null || crewArray.Length != partCrew.Length)
		{
			crewArray = new ProtoCrewMember[partCrew.Length];
		}
		int i = 0;
		for (int num = partCrew.Length; i < num; i++)
		{
			if (partCrew[i] != string.Empty)
			{
				crewArray[i] = HighLogic.CurrentGame.CrewRoster[partCrew[i]];
			}
			else
			{
				crewArray[i] = null;
			}
		}
	}

	public bool Contains(ProtoCrewMember crew)
	{
		return GetCrewSeat(crew.name) >= 0;
	}

	public int GetCrewSeat(ProtoCrewMember crew)
	{
		return GetCrewSeat(crew.name);
	}

	public int GetCrewSeat(string name)
	{
		int num = partCrew.Length;
		int num2 = 0;
		while (true)
		{
			if (num2 < num)
			{
				if (partCrew[num2] == name)
				{
					break;
				}
				num2++;
				continue;
			}
			return -1;
		}
		return num2;
	}

	public bool AnySeats()
	{
		return partCrew.Length != 0;
	}

	public bool NoSeats()
	{
		return partCrew.Length == 0;
	}

	public bool AllSeatsEmpty()
	{
		return AllSeatsEmpty(HighLogic.CurrentGame.CrewRoster);
	}

	public bool AllSeatsEmpty(KerbalRoster roster)
	{
		int num = 0;
		int num2 = partCrew.Length;
		while (true)
		{
			if (num < num2)
			{
				if (!string.IsNullOrEmpty(partCrew[num]) && roster[partCrew[num]] != null)
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public bool AnySeatTaken()
	{
		return AnySeatTaken(HighLogic.CurrentGame.CrewRoster);
	}

	public bool AnySeatTaken(KerbalRoster roster)
	{
		int num = 0;
		int num2 = partCrew.Length;
		while (true)
		{
			if (num < num2)
			{
				if (!string.IsNullOrEmpty(partCrew[num]) && roster[partCrew[num]] != null)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public int CountCrewType(ProtoCrewMember.KerbalType type)
	{
		return CountCrewType(type, HighLogic.CurrentGame.CrewRoster);
	}

	public int CountCrewType(ProtoCrewMember.KerbalType type, KerbalRoster roster)
	{
		int num = 0;
		int i = 0;
		for (int num2 = partCrew.Length; i < num2; i++)
		{
			if (!(partCrew[i] == string.Empty))
			{
				ProtoCrewMember protoCrewMember = roster[partCrew[i]];
				if (protoCrewMember != null && protoCrewMember.type == type)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int CountCrewNotType(ProtoCrewMember.KerbalType type)
	{
		return CountCrewNotType(type, HighLogic.CurrentGame.CrewRoster);
	}

	public int CountCrewNotType(ProtoCrewMember.KerbalType type, KerbalRoster roster)
	{
		int num = 0;
		int i = 0;
		for (int num2 = partCrew.Length; i < num2; i++)
		{
			if (!(partCrew[i] == string.Empty))
			{
				ProtoCrewMember protoCrewMember = roster[partCrew[i]];
				if (protoCrewMember != null && protoCrewMember.type != type)
				{
					num++;
				}
			}
		}
		return num;
	}
}
