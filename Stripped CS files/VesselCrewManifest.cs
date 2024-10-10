using System;
using System.Collections.Generic;
using Expansions.Missions;
using UnityEngine;

public class VesselCrewManifest
{
	public Dictionary<uint, PartCrewManifest> partLookup;

	public Dictionary<string, PartCrewManifest> crewLookup;

	public List<PartCrewManifest> partManifests;

	public float crewCountOptimizedForFloat;

	public List<PartCrewManifest> PartManifests => partManifests;

	public int CrewCount => crewLookup.Keys.Count;

	public float CrewCountOptimizedForFloat => crewCountOptimizedForFloat;

	public int PartCount => partLookup.Keys.Count;

	public VesselCrewManifest()
	{
		partLookup = new Dictionary<uint, PartCrewManifest>();
		crewLookup = new Dictionary<string, PartCrewManifest>();
		partManifests = new List<PartCrewManifest>();
	}

	public static VesselCrewManifest FromConfigNode(ConfigNode craftNode)
	{
		VesselCrewManifest vesselCrewManifest = new VesselCrewManifest();
		int count = craftNode.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = craftNode.nodes[i];
			if (!(configNode.name != "PART"))
			{
				PartCrewManifest partCrewManifest = PartCrewManifest.FromConfigNode(configNode, vesselCrewManifest);
				if (partCrewManifest != null)
				{
					vesselCrewManifest.SetPartManifest(partCrewManifest.PartID, partCrewManifest);
				}
			}
		}
		return vesselCrewManifest;
	}

	public static VesselCrewManifest CloneOf(VesselCrewManifest original, bool blank)
	{
		VesselCrewManifest vesselCrewManifest = new VesselCrewManifest();
		int count = original.partManifests.Count;
		for (int i = 0; i < count; i++)
		{
			PartCrewManifest partCrewManifest = original.partManifests[i];
			vesselCrewManifest.SetPartManifest(partCrewManifest.PartID, PartCrewManifest.CloneOf(partCrewManifest, vesselCrewManifest, blank));
		}
		return vesselCrewManifest;
	}

	public static void MergeInto(VesselCrewManifest m1, VesselCrewManifest m2, Func<PartCrewManifest, bool> inclusionFilter = null)
	{
		int count = m1.partManifests.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = m1.partManifests[count];
			if (inclusionFilter != null && !inclusionFilter(partCrewManifest))
			{
				m1.RemovePartManifest(partCrewManifest);
			}
		}
		int count2 = m2.partManifests.Count;
		for (int i = 0; i < count2; i++)
		{
			PartCrewManifest partCrewManifest = m2.partManifests[i];
			if (inclusionFilter != null && inclusionFilter(partCrewManifest))
			{
				m1.SetPartManifestNoOverwrite(partCrewManifest.PartID, partCrewManifest);
			}
		}
	}

	public List<ProtoCrewMember> GetAllCrew(bool includeNulls)
	{
		return GetAllCrew(includeNulls, HighLogic.CurrentGame.CrewRoster);
	}

	public List<ProtoCrewMember> GetAllCrew(bool includeNulls, KerbalRoster roster)
	{
		List<ProtoCrewMember> list = new List<ProtoCrewMember>();
		int count = partManifests.Count;
		for (int i = 0; i < count; i++)
		{
			PartCrewManifest partCrewManifest = partManifests[i];
			int num = partCrewManifest.partCrew.Length;
			for (int j = 0; j < num; j++)
			{
				if (!string.IsNullOrEmpty(partCrewManifest.partCrew[j]) || includeNulls)
				{
					ProtoCrewMember protoCrewMember = roster[partCrewManifest.partCrew[j]];
					if (protoCrewMember != null || includeNulls)
					{
						list.Add(protoCrewMember);
					}
				}
			}
		}
		return list;
	}

	public float GetCrewInventoryCosts(KerbalRoster roster = null)
	{
		float num = 0f;
		if (roster != null || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster != null))
		{
			List<ProtoCrewMember> allCrew = GetAllCrew(includeNulls: false, (roster != null) ? roster : HighLogic.CurrentGame.CrewRoster);
			for (int i = 0; i < allCrew.Count; i++)
			{
				num += allCrew[i].InventoryCosts();
			}
		}
		return num;
	}

	public float GetCrewInventoryMass()
	{
		return GetCrewInventoryMass(null);
	}

	public float GetCrewInventoryMass(KerbalRoster roster)
	{
		float num = 0f;
		if (roster != null || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster != null))
		{
			List<ProtoCrewMember> allCrew = GetAllCrew(includeNulls: false, (roster != null) ? roster : HighLogic.CurrentGame.CrewRoster);
			for (int i = 0; i < allCrew.Count; i++)
			{
				num += allCrew[i].InventoryMass();
			}
		}
		return num;
	}

	public float GetCrewResourceMass()
	{
		return GetCrewResourceMass(null);
	}

	public float GetCrewResourceMass(KerbalRoster roster)
	{
		float num = 0f;
		if (roster != null || (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster != null))
		{
			List<ProtoCrewMember> allCrew = GetAllCrew(includeNulls: false, (roster != null) ? roster : HighLogic.CurrentGame.CrewRoster);
			for (int i = 0; i < allCrew.Count; i++)
			{
				num += allCrew[i].ResourceMass();
			}
		}
		return num;
	}

	public bool Contains(ProtoCrewMember crew)
	{
		return crewLookup.ContainsKey(crew.name);
	}

	public List<PartCrewManifest> GetCrewableParts()
	{
		List<PartCrewManifest> list = new List<PartCrewManifest>(partManifests);
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].PartInfo.partPrefab.CrewCapacity == 0)
			{
				list.RemoveAt(count);
			}
		}
		return list;
	}

	public void SetPartManifestNoOverwrite(uint id, PartCrewManifest newPCMtoUse)
	{
		PartCrewManifest value = null;
		if (partLookup.TryGetValue(id, out value))
		{
			if (value == newPCMtoUse)
			{
				newPCMtoUse.vcm = this;
				return;
			}
			int num = value.partCrew.Length;
			while (num-- > 0)
			{
				if (!string.IsNullOrEmpty(value.partCrew[num]))
				{
					crewLookup.Remove(value.partCrew[num]);
					crewCountOptimizedForFloat = crewLookup.Keys.Count;
				}
			}
		}
		newPCMtoUse.vcm = this;
		partLookup[id] = newPCMtoUse;
		if (value != null)
		{
			partManifests.Remove(value);
		}
		partManifests.Add(newPCMtoUse);
		int num2 = newPCMtoUse.partCrew.Length;
		while (num2-- > 0)
		{
			if (!string.IsNullOrEmpty(newPCMtoUse.partCrew[num2]))
			{
				if (crewLookup.ContainsKey(newPCMtoUse.partCrew[num2]))
				{
					newPCMtoUse.partCrew[num2] = string.Empty;
					crewCountOptimizedForFloat = crewLookup.Keys.Count;
				}
				else
				{
					crewLookup[newPCMtoUse.partCrew[num2]] = newPCMtoUse;
					crewCountOptimizedForFloat = crewLookup.Keys.Count;
				}
			}
		}
	}

	public void SetPartManifest(uint id, PartCrewManifest newPCMtoUse)
	{
		PartCrewManifest value = null;
		if (partLookup.TryGetValue(id, out value))
		{
			if (value == newPCMtoUse)
			{
				newPCMtoUse.vcm = this;
				return;
			}
			int num = value.partCrew.Length;
			while (num-- > 0)
			{
				if (!string.IsNullOrEmpty(value.partCrew[num]))
				{
					crewLookup.Remove(value.partCrew[num]);
					crewCountOptimizedForFloat = crewLookup.Keys.Count;
				}
			}
		}
		newPCMtoUse.vcm = this;
		partLookup[id] = newPCMtoUse;
		if (value != null)
		{
			partManifests.Remove(value);
		}
		partManifests.Add(newPCMtoUse);
		int num2 = newPCMtoUse.partCrew.Length;
		while (num2-- > 0)
		{
			if (!string.IsNullOrEmpty(newPCMtoUse.partCrew[num2]))
			{
				if (crewLookup.TryGetValue(newPCMtoUse.partCrew[num2], out value) && value != newPCMtoUse)
				{
					value.RemoveCrew(newPCMtoUse.partCrew[num2]);
				}
				crewLookup[newPCMtoUse.partCrew[num2]] = newPCMtoUse;
				crewCountOptimizedForFloat = crewLookup.Keys.Count;
			}
		}
	}

	public void UpdatePartManifest(uint id, PartCrewManifest referencePCM)
	{
		PartCrewManifest value = null;
		PartCrewManifest value2 = null;
		if (!partLookup.TryGetValue(id, out value))
		{
			return;
		}
		if (value == referencePCM)
		{
			value.vcm = this;
			return;
		}
		int num = referencePCM.partCrew.Length;
		while (num-- > 0)
		{
			if (!string.IsNullOrEmpty(referencePCM.partCrew[num]))
			{
				if (crewLookup.TryGetValue(referencePCM.partCrew[num], out value2) && value2 != value)
				{
					value2.RemoveCrew(referencePCM.partCrew[num]);
				}
				value.partCrew[num] = referencePCM.partCrew[num];
				crewLookup[value.partCrew[num]] = value;
				crewCountOptimizedForFloat = crewLookup.Keys.Count;
			}
		}
	}

	public void RemovePartManifest(PartCrewManifest pcm)
	{
		int num = pcm.partCrew.Length;
		while (num-- > 0)
		{
			if (!string.IsNullOrEmpty(pcm.partCrew[num]))
			{
				crewLookup.Remove(pcm.partCrew[num]);
				crewCountOptimizedForFloat = crewLookup.Keys.Count;
			}
		}
		partLookup.Remove(pcm.PartID);
		partManifests.Remove(pcm);
	}

	public void SetCrewMember(string cName, PartCrewManifest pcm)
	{
		PartCrewManifest value = null;
		if (crewLookup.TryGetValue(cName, out value) && value != pcm)
		{
			value.RemoveCrew(cName);
		}
		if (pcm != value)
		{
			crewLookup[cName] = pcm;
			crewCountOptimizedForFloat = crewLookup.Keys.Count;
		}
	}

	public void RemoveCrewMember(string cName)
	{
		PartCrewManifest value = null;
		if (crewLookup.TryGetValue(cName, out value))
		{
			crewLookup.Remove(cName);
			value.RemoveCrew(cName);
			crewCountOptimizedForFloat = crewLookup.Keys.Count;
		}
	}

	public PartCrewManifest GetPartCrewManifest(uint id)
	{
		if (partLookup.TryGetValue(id, out var value))
		{
			return value;
		}
		return null;
	}

	public PartCrewManifest GetPartForCrew(ProtoCrewMember crew)
	{
		if (crewLookup.TryGetValue(crew.name, out var value))
		{
			return value;
		}
		return null;
	}

	public void DebugManifest()
	{
		foreach (PartCrewManifest value in partLookup.Values)
		{
			for (int i = 0; i < value.GetPartCrew().Length; i++)
			{
				if (value.GetPartCrew()[i] != null)
				{
					Debug.Log("Crewmember " + value.GetPartCrew()[i].name + " is on seat #" + i + " on " + value.PartInfo.title);
				}
				else
				{
					Debug.Log("Seat #" + i + " on " + value.PartInfo.title + " is empty");
				}
			}
		}
	}

	public void Filter(Func<PartCrewManifest, bool> inclusionFilter)
	{
		int count = partManifests.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = partManifests[count];
			if (!inclusionFilter(partCrewManifest))
			{
				RemovePartManifest(partCrewManifest);
			}
		}
	}

	public bool HasAnyCrew()
	{
		return HasAnyCrew(HighLogic.CurrentGame.CrewRoster);
	}

	public bool HasAnyCrew(KerbalRoster roster)
	{
		int count = partManifests.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = partManifests[count];
			if (partCrewManifest.PartInfo.partPrefab.CrewCapacity <= 0)
			{
				continue;
			}
			int num = partCrewManifest.partCrew.Length;
			while (num-- > 0)
			{
				if (!string.IsNullOrEmpty(partCrewManifest.partCrew[num]))
				{
					if (roster[partCrewManifest.partCrew[num]] != null)
					{
						return true;
					}
					partCrewManifest.RemoveCrewFromSeat(num);
				}
			}
		}
		return false;
	}

	public bool AnyCrewInState(ProtoCrewMember.RosterStatus state, bool notInState = false)
	{
		return AnyCrewInState(state, HighLogic.CurrentGame.CrewRoster, notInState);
	}

	public bool AnyCrewInState(ProtoCrewMember.RosterStatus state, KerbalRoster roster, bool notInState = false)
	{
		int count = partManifests.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = partManifests[count];
			int num = partCrewManifest.partCrew.Length;
			while (num-- > 0)
			{
				if (string.IsNullOrEmpty(partCrewManifest.partCrew[num]))
				{
					continue;
				}
				ProtoCrewMember protoCrewMember = roster[partCrewManifest.partCrew[num]];
				if (protoCrewMember == null)
				{
					partCrewManifest.RemoveCrewFromSeat(num);
				}
				else if (notInState)
				{
					if (protoCrewMember.rosterStatus != state)
					{
						return true;
					}
				}
				else if (protoCrewMember.rosterStatus == state)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool AnyCrewWithTrait(string trait, bool noCrewWithTrait = false)
	{
		return AnyCrewWithTrait(trait, HighLogic.CurrentGame.CrewRoster, noCrewWithTrait);
	}

	public bool AnyCrewWithTrait(string trait, KerbalRoster roster, bool noCrewWithTrait = false)
	{
		int count = partManifests.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = partManifests[count];
			int num = partCrewManifest.partCrew.Length;
			while (num-- > 0)
			{
				if (!string.IsNullOrEmpty(partCrewManifest.partCrew[num]))
				{
					ProtoCrewMember protoCrewMember = roster[partCrewManifest.partCrew[num]];
					if (protoCrewMember == null)
					{
						partCrewManifest.RemoveCrewFromSeat(num);
					}
					else if (!noCrewWithTrait && protoCrewMember.trait == trait)
					{
						return true;
					}
				}
			}
		}
		return !noCrewWithTrait;
	}

	public void AssignCrewToVessel(ShipConstruct ship)
	{
		AssignCrewToVessel(ship, HighLogic.CurrentGame.CrewRoster);
	}

	public void AssignCrewToVessel(ShipConstruct ship, KerbalRoster roster)
	{
		int count = partManifests.Count;
		PartCrewManifest pcm;
		for (int i = 0; i < count; i++)
		{
			pcm = partManifests[i];
			int num = pcm.partCrew.Length;
			if (num == 0)
			{
				continue;
			}
			Part part = ship.parts.Find((Part prt) => prt.craftID == pcm.PartID);
			if (part != null)
			{
				for (int j = 0; j < num; j++)
				{
					ProtoCrewMember protoCrewMember = roster[pcm.partCrew[j]];
					if (protoCrewMember != null)
					{
						part.AddCrewmemberAt(protoCrewMember, j);
						Debug.Log("Crewmember " + protoCrewMember.name + " assigned to " + part.partInfo.title + ", seat # " + j + " (crew seat index: " + protoCrewMember.seatIdx + ")", part.gameObject);
					}
				}
			}
			else
			{
				Debug.LogWarning(("[CrewAssignment]: No " + pcm.PartInfo.name + " with id " + pcm.PartID + " was found in " + ship.shipName) ?? "");
			}
		}
	}

	public VesselCrewManifest UpdateCrewForVessel(ConfigNode vesselNode, Func<PartCrewManifest, bool> persistFilter = null)
	{
		VesselCrewManifest vesselCrewManifest = FromConfigNode(vesselNode);
		int count = partManifests.Count;
		for (int i = 0; i < count; i++)
		{
			PartCrewManifest partCrewManifest = partManifests[i];
			if (partCrewManifest.PartInfo.partPrefab.CrewCapacity > 0 && persistFilter != null && persistFilter(partCrewManifest))
			{
				vesselCrewManifest.UpdatePartManifest(partCrewManifest.PartID, partCrewManifest);
			}
		}
		return vesselCrewManifest;
	}

	public void AddCrewMembers(ref List<Crew> vesselCrew, KerbalRoster roster)
	{
		int count = vesselCrew.Count;
		while (count-- > 0)
		{
			PartCrewManifest partCrewManifest = GetPartCrewManifest(vesselCrew[count].partPersistentID);
			if (partCrewManifest != null)
			{
				int count2 = vesselCrew[count].crewNames.Count;
				while (count2-- > 0)
				{
					ProtoCrewMember protoCrewMember = roster[vesselCrew[count].crewNames[count2]];
					if (protoCrewMember != null)
					{
						if (protoCrewMember.seatIdx >= 0 && protoCrewMember.seatIdx <= partCrewManifest.PartInfo.partPrefab.CrewCapacity)
						{
							partCrewManifest.AddCrewToSeat(protoCrewMember, protoCrewMember.seatIdx);
							continue;
						}
						protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
						vesselCrew[count].crewNames.RemoveAt(count2);
						Debug.LogFormat("[CrewManifest]: Unable to assign {0} to part {1}. Set kerbal roster status back to Available.", protoCrewMember.name, partCrewManifest.PartInfo.name);
					}
				}
				continue;
			}
			int count3 = vesselCrew[count].crewNames.Count;
			while (count3-- > 0)
			{
				ProtoCrewMember protoCrewMember2 = roster[vesselCrew[count].crewNames[count3]];
				if (protoCrewMember2 != null)
				{
					protoCrewMember2.rosterStatus = ProtoCrewMember.RosterStatus.Available;
					vesselCrew[count].crewNames.RemoveAt(count3);
				}
			}
			vesselCrew.RemoveAt(count);
		}
	}
}
