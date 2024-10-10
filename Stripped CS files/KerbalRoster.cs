using System;
using System.Collections.Generic;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using Experience;
using FinePrint.Utilities;
using ns2;
using ns9;
using UnityEngine;

public class KerbalRoster
{
	public DictionaryValueList<string, ProtoCrewMember> kerbals;

	public Game.Modes mode;

	public static string pilotTrait = "Pilot";

	public static string engineerTrait = "Engineer";

	public static string scientistTrait = "Scientist";

	public static string touristTrait = "Tourist";

	public static int applicantGroupSizeTarget = 10;

	public static int applicantGroupSizeVariance = 3;

	public static List<Type> ExperienceTraitTypes = null;

	public static List<Type> ExperienceEffectTypes = null;

	public static float[] xpPerLevel = new float[5] { 2f, 8f, 16f, 32f, 64f };

	public static List<float> xpHomeValues = new List<float>();

	public static List<string> xpHomeTypes = new List<string>();

	public static List<string> xpHomeTypeNames = new List<string>();

	public static List<float> xpNotHomeValues = new List<float>();

	public static List<string> xpNotHomeTypes = new List<string>();

	public static List<string> xpNotHomeTypeNames = new List<string>();

	public Game.Modes GameMode => mode;

	public ProtoCrewMember this[int index]
	{
		get
		{
			if (index >= 0 && index < kerbals.Count && kerbals.Count > 0)
			{
				return kerbals.At(index);
			}
			return null;
		}
	}

	public ProtoCrewMember this[string name]
	{
		get
		{
			ProtoCrewMember val = null;
			kerbals.TryGetValue(name, out val);
			return val;
		}
	}

	public int Count => kerbals.Count;

	public IEnumerable<ProtoCrewMember> Applicants
	{
		get
		{
			int i = 0;
			int iC = kerbals.Count;
			while (i < iC)
			{
				if (kerbals.At(i).type == ProtoCrewMember.KerbalType.Applicant)
				{
					yield return kerbals.At(i);
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public IEnumerable<ProtoCrewMember> Crew
	{
		get
		{
			int i = 0;
			int iC = kerbals.Count;
			while (i < iC)
			{
				if (kerbals.At(i).type == ProtoCrewMember.KerbalType.Crew)
				{
					yield return kerbals.At(i);
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public IEnumerable<ProtoCrewMember> Tourist
	{
		get
		{
			int i = 0;
			int iC = kerbals.Count;
			while (i < iC)
			{
				if (kerbals.At(i).type == ProtoCrewMember.KerbalType.Tourist)
				{
					yield return kerbals.At(i);
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public IEnumerable<ProtoCrewMember> Unowned
	{
		get
		{
			int i = 0;
			int iC = kerbals.Count;
			while (i < iC)
			{
				if (kerbals.At(i).type == ProtoCrewMember.KerbalType.Unowned)
				{
					yield return kerbals.At(i);
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public KerbalRoster(Game.Modes mode)
	{
		InitializeExperienceTypes();
		GenerateExperienceTypes();
		this.mode = mode;
		kerbals = new DictionaryValueList<string, ProtoCrewMember>();
	}

	public KerbalRoster(ConfigNode node, Game.Modes mode)
	{
		InitializeExperienceTypes();
		GenerateExperienceTypes();
		this.mode = mode;
		kerbals = new DictionaryValueList<string, ProtoCrewMember>();
		int count = node.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (!(configNode.name == "KERBAL") && !(configNode.name == "CREW"))
			{
				if (!(configNode.name == "APPLICANTS"))
				{
					continue;
				}
				int count2 = configNode.nodes.Count;
				for (int j = 0; j < count2; j++)
				{
					if (configNode.nodes[j].name == "RECRUIT")
					{
						ProtoCrewMember protoCrewMember = new ProtoCrewMember(mode, configNode.nodes[j], ProtoCrewMember.KerbalType.Applicant);
						kerbals.Add(protoCrewMember.name, protoCrewMember);
					}
				}
			}
			else
			{
				ProtoCrewMember protoCrewMember = new ProtoCrewMember(mode, configNode);
				kerbals.Add(protoCrewMember.name, protoCrewMember);
			}
		}
	}

	public static KerbalRoster GenerateInitialCrewRoster(Game.Modes mode)
	{
		ProtoCrewMember protoCrewMember = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, Localizer.Format("#autoLOC_20803"));
		protoCrewMember.gender = ProtoCrewMember.Gender.Male;
		protoCrewMember.stupidity = 0.5f;
		protoCrewMember.courage = 0.5f;
		protoCrewMember.isBadass = true;
		protoCrewMember.seatIdx = -1;
		protoCrewMember.veteran = true;
		protoCrewMember.isHero = true;
		ProtoCrewMember protoCrewMember2 = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, Localizer.Format("#autoLOC_20811"));
		protoCrewMember2.gender = ProtoCrewMember.Gender.Male;
		protoCrewMember2.stupidity = 0.8f;
		protoCrewMember2.courage = 0.5f;
		protoCrewMember2.isBadass = false;
		protoCrewMember2.seatIdx = -1;
		protoCrewMember2.veteran = true;
		protoCrewMember2.isHero = true;
		ProtoCrewMember protoCrewMember3 = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, Localizer.Format("#autoLOC_20819"));
		protoCrewMember3.gender = ProtoCrewMember.Gender.Male;
		protoCrewMember3.stupidity = 0.1f;
		protoCrewMember3.courage = 0.3f;
		protoCrewMember3.isBadass = false;
		protoCrewMember3.seatIdx = -1;
		protoCrewMember3.veteran = true;
		protoCrewMember3.isHero = true;
		ProtoCrewMember protoCrewMember4 = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, Localizer.Format("#autoLOC_20827"));
		protoCrewMember4.gender = ProtoCrewMember.Gender.Female;
		protoCrewMember4.stupidity = 0.4f;
		protoCrewMember4.courage = 0.55f;
		protoCrewMember4.isBadass = true;
		protoCrewMember4.seatIdx = -1;
		protoCrewMember4.veteran = true;
		protoCrewMember4.isHero = true;
		KerbalRoster kerbalRoster = new KerbalRoster(mode);
		SetExperienceTrait(protoCrewMember, pilotTrait);
		SetExperienceTrait(protoCrewMember2, engineerTrait);
		SetExperienceTrait(protoCrewMember3, scientistTrait);
		SetExperienceTrait(protoCrewMember4, pilotTrait);
		kerbalRoster.AddCrewMember(protoCrewMember);
		kerbalRoster.AddCrewMember(protoCrewMember2);
		kerbalRoster.AddCrewMember(protoCrewMember3);
		kerbalRoster.AddCrewMember(protoCrewMember4);
		return kerbalRoster;
	}

	public void RepopulateCrewRoster(int rosterSize)
	{
		int num = 0;
		for (int i = 0; i < Count; i++)
		{
			if (this[i].rosterStatus == ProtoCrewMember.RosterStatus.Available)
			{
				num++;
			}
		}
		for (int j = num; j < rosterSize; j++)
		{
			GetNewKerbal();
		}
	}

	public void Init(Game st)
	{
		if (st.flightState != null)
		{
			CheckRosterRespawn(st.UniversalTime, st.Parameters);
		}
	}

	public void CheckRosterRespawn(double universalTime, GameParameters parameters)
	{
		int count = kerbals.Count;
		while (count-- > 0)
		{
			kerbals.At(count).CheckRespawnTimer(universalTime, parameters);
		}
	}

	public void CheckRosterRespawn()
	{
		CheckRosterRespawn(Planetarium.GetUniversalTime(), HighLogic.CurrentGame.Parameters);
	}

	public void Save(ConfigNode node)
	{
		int count = kerbals.Count;
		for (int i = 0; i < count; i++)
		{
			kerbals.At(i).Save(node.AddNode("KERBAL"));
		}
	}

	public ProtoCrewMember GetNextOrNewKerbal(ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew)
	{
		ProtoCrewMember nextAvailableKerbal = GetNextAvailableKerbal(type);
		if (nextAvailableKerbal == null)
		{
			return GetNewKerbal(type);
		}
		return nextAvailableKerbal;
	}

	public ProtoCrewMember GetNewKerbal(ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew)
	{
		ProtoCrewMember protoCrewMember;
		do
		{
			protoCrewMember = CrewGenerator.RandomCrewMemberPrototype(type);
		}
		while (!AddCrewMember(protoCrewMember));
		return protoCrewMember;
	}

	public ProtoCrewMember GetNextAvailableKerbal(ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew)
	{
		ProtoCrewMember result = null;
		int count = kerbals.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(i);
			if (protoCrewMember.type == type)
			{
				if (protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Missing)
				{
					protoCrewMember.CheckRespawnTimer(Planetarium.GetUniversalTime(), HighLogic.CurrentGame.Parameters);
				}
				if (protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available)
				{
					result = protoCrewMember;
					break;
				}
			}
		}
		return result;
	}

	public bool AddCrewMember(ProtoCrewMember crew)
	{
		string value = CrewGenerator.RemoveLastName(crew.name);
		int num = 0;
		while (true)
		{
			if (num < kerbals.Count)
			{
				if (kerbals.KeyAt(num).Contains(value))
				{
					break;
				}
				num++;
				continue;
			}
			SetExperienceTrait(crew);
			GameEvents.onKerbalAdded.Fire(crew);
			kerbals.Add(crew.name, crew);
			GameEvents.onKerbalAddComplete.Fire(crew);
			return true;
		}
		return false;
	}

	public bool ChangeNameCalledFromPCM(ProtoCrewMember crew, string oldName, string newName)
	{
		if (kerbals.ContainsKey(newName))
		{
			Debug.LogFormat("[KerbalRoster]: A Kerbal with name:{0} already exists in the roster. Unable to rename {1} with that new name.", newName, oldName);
			return false;
		}
		if (kerbals.Contains(oldName))
		{
			kerbals.Remove(oldName);
			kerbals.Add(newName, crew);
			return true;
		}
		Debug.LogFormat("[KerbalRoster]: A Kerbal with name:{0} does not exist in the current roster. Unable to rename to {1}", oldName, newName);
		return false;
	}

	public bool ChangeName(string oldName, string newName)
	{
		if (kerbals.TryGetValue(oldName, out var val))
		{
			return val.ChangeName(newName);
		}
		Debug.LogFormat("[KerbalRoster]: A Kerbal with name:{0} does not exist in the current roster. Unable to rename to {1}", oldName, newName);
		return false;
	}

	public bool Exists(string name)
	{
		return kerbals.Contains(name);
	}

	public bool Remove(string name)
	{
		ProtoCrewMember val = null;
		if (kerbals.TryGetValue(name, out val))
		{
			kerbals.Remove(name);
			GameEvents.onKerbalRemoved.Fire(val);
			return true;
		}
		return false;
	}

	public bool Remove(ProtoCrewMember crew)
	{
		return Remove(crew.name);
	}

	public void Remove(int i)
	{
		Remove(kerbals.At(i).name);
	}

	public void ValidateAssignments(Game st)
	{
		if (st.flightState == null)
		{
			return;
		}
		int count = st.flightState.protoVessels.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoVessel protoVessel = st.flightState.protoVessels[i];
			int count2 = protoVessel.protoPartSnapshots.Count;
			for (int j = 0; j < count2; j++)
			{
				ProtoPartSnapshot protoPartSnapshot = protoVessel.protoPartSnapshots[j];
				for (int k = 0; k < protoPartSnapshot.protoModuleCrew.Count; k++)
				{
					if (this[protoPartSnapshot.protoCrewNames[k]] != null && protoPartSnapshot.protoModuleCrew[k] != this[protoPartSnapshot.protoCrewNames[k]])
					{
						Debug.LogWarning(string.Concat("[Protocrewmember]: Instance of crewmember ", protoPartSnapshot.protoModuleCrew[k], " in part ", protoPartSnapshot.partName, " on ", protoVessel.GetDisplayName(), " did not match instance of ", this[protoPartSnapshot.protoCrewNames[k]].name, " on crew roster. (Stored index: ", protoPartSnapshot.protoCrewNames[k], ")"));
						protoPartSnapshot.protoModuleCrew[k] = this[protoPartSnapshot.protoCrewNames[k]];
					}
					else if (this[protoPartSnapshot.protoCrewNames[k]] == null)
					{
						Debug.LogWarning(string.Concat("[Protocrewmember]: Instance of crewmember ", protoPartSnapshot.protoModuleCrew[k], " in part ", protoPartSnapshot.partName, " on ", protoVessel.GetDisplayName(), " does not exist in the roster"));
						if (protoPartSnapshot.protoModuleCrew[k] != null)
						{
							SystemUtilities.ExpungeKerbal(protoPartSnapshot.protoModuleCrew[k]);
						}
						else
						{
							protoVessel.GetVesselCrew();
						}
					}
				}
			}
		}
		IEnumerator<ProtoCrewMember> enumerator = Crew.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ProtoCrewMember current = enumerator.Current;
			bool flag = false;
			int count3 = st.flightState.protoVessels.Count;
			while (count3-- > 0)
			{
				if (st.flightState.protoVessels[count3].GetVesselCrew().Contains(current))
				{
					flag = true;
					break;
				}
			}
			if (current.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
			{
				if (flag)
				{
					continue;
				}
				if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER || HighLogic.CurrentGame.Mode == Game.Modes.MISSION) && MissionSystem.missions != null && MissionSystem.missions.Count > 0)
				{
					bool flag2 = false;
					for (int l = 0; l < MissionSystem.missions.Count; l++)
					{
						Mission mission = MissionSystem.missions[l];
						if (mission != null && ExpansionsLoader.IsExpansionInstalled("MakingHistory", mission.HashBytes, mission.Signature))
						{
							ProtoCrewMember protoCrewMember = mission.situation.crewRoster[current.name];
							if (protoCrewMember != null && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						continue;
					}
				}
				Debug.LogWarning("[ProtoCrewMember Warning]: Crewmember " + current.name + " found assigned but no vessels reference him. " + current.name + " set as missing.");
				current.StartRespawnPeriod(2000.0);
			}
			else if (flag)
			{
				Debug.LogWarning("[ProtoCrewMember Warning]: Crewmember " + current.name + " found inside a part but status is set as missing. Vessel must have failed to save earlier. Restoring assigned status.");
				current.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
			}
		}
	}

	public VesselCrewManifest DefaultCrewForVessel(ConfigNode vesselNode, VesselCrewManifest previous = null, bool autohire = true, bool usePreviousVCMToFill = false)
	{
		VesselCrewManifest vesselCrewManifest = VesselCrewManifest.FromConfigNode(vesselNode);
		List<ProtoCrewMember> list = new List<ProtoCrewMember>(Kerbals(ProtoCrewMember.KerbalType.Crew, default(ProtoCrewMember.RosterStatus)));
		bool flag = previous == null || previous.AnyCrewWithTrait(pilotTrait, noCrewWithTrait: true);
		int count = vesselCrewManifest.PartManifests.Count;
		for (int i = 0; i < count; i++)
		{
			PartCrewManifest partCrewManifest = vesselCrewManifest.PartManifests[i];
			bool flag2 = false;
			if (partCrewManifest.PartInfo.partPrefab.isControlSource == Vessel.ControlLevel.NONE && partCrewManifest.PartInfo.partPrefab.CrewCapacity > 0 && partCrewManifest.PartInfo.partPrefab.FindModuleImplementing<KerbalSeat>() != null)
			{
				flag2 = true;
			}
			if (!((partCrewManifest.PartInfo.partPrefab.isControlSource > Vessel.ControlLevel.NONE && partCrewManifest.PartInfo.partPrefab.CrewCapacity > 0) || flag2))
			{
				continue;
			}
			if (!flag2 && list.Count < partCrewManifest.PartInfo.partPrefab.CrewCapacity)
			{
				if (HighLogic.CurrentGame.Parameters.Difficulty.AutoHireCrews && autohire)
				{
					if (GetActiveCrewCount() < GameVariables.Instance.GetActiveCrewLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
					{
						Debug.Log("[Crew Assignment]: Part " + partCrewManifest.PartInfo.title + " holds " + partCrewManifest.PartInfo.partPrefab.CrewCapacity + " crew, but only " + list.Count + " are available.");
						int num = partCrewManifest.PartInfo.partPrefab.CrewCapacity - list.Count;
						Update(Planetarium.GetUniversalTime());
						for (int j = 0; j < num; j++)
						{
							if (Funding.CanAfford(GameVariables.Instance.GetRecruitHireCost(GetActiveCrewCount())))
							{
								ProtoCrewMember protoCrewMember = GetNextApplicant();
								if (protoCrewMember == null)
								{
									protoCrewMember = GetNewKerbal(ProtoCrewMember.KerbalType.Applicant);
								}
								HireApplicant(protoCrewMember);
								list.Add(protoCrewMember);
								if (Funding.Instance != null)
								{
									ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_21131", XKCDColors.HexFormat.ElectricLime, protoCrewMember.name, GameVariables.Instance.GetRecruitHireCost(GetActiveCrewCount() - 1).ToString("N2")), 5f, ScreenMessageStyle.UPPER_LEFT);
								}
								else
								{
									ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003260", XKCDColors.HexFormat.ElectricLime, protoCrewMember.name), 5f, ScreenMessageStyle.UPPER_LEFT);
								}
							}
							else
							{
								ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_21144"), 5f, ScreenMessageStyle.UPPER_LEFT);
							}
						}
					}
					else
					{
						ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_21150"), 5f, ScreenMessageStyle.UPPER_LEFT);
					}
				}
				else
				{
					Debug.Log("[Crew Assignment]: Part " + partCrewManifest.PartInfo.title + " holds " + partCrewManifest.PartInfo.partPrefab.CrewCapacity + " crew, but only " + list.Count + " are available. Auto-hiring is disabled or not allowed at this point.");
				}
			}
			for (int k = 0; k < partCrewManifest.PartInfo.partPrefab.CrewCapacity; k++)
			{
				if (!usePreviousVCMToFill && list.Count == 0)
				{
					break;
				}
				ProtoCrewMember protoCrewMember2 = null;
				if (previous != null)
				{
					if (usePreviousVCMToFill)
					{
						PartCrewManifest partCrewManifest2 = previous.GetPartCrewManifest(partCrewManifest.PartID);
						if (partCrewManifest2 != null)
						{
							ProtoCrewMember[] partCrew = partCrewManifest2.GetPartCrew();
							if (partCrew.Length > k)
							{
								protoCrewMember2 = partCrew[k];
							}
						}
					}
					if (!flag2 && protoCrewMember2 == null)
					{
						if (flag)
						{
							protoCrewMember2 = list.Find((ProtoCrewMember c) => c.trait == pilotTrait && !c.inactive && !previous.Contains(c)) ?? list.Find((ProtoCrewMember c) => !previous.Contains(c));
							flag = false;
						}
						else
						{
							protoCrewMember2 = list.Find((ProtoCrewMember c) => !previous.Contains(c) && !c.inactive);
						}
					}
				}
				else if (!flag2)
				{
					if (flag)
					{
						protoCrewMember2 = list.Find((ProtoCrewMember c) => c.trait == pilotTrait && !c.inactive) ?? list[0];
						flag = false;
					}
					else
					{
						protoCrewMember2 = list.Find((ProtoCrewMember c) => !c.inactive);
					}
				}
				if (protoCrewMember2 != null)
				{
					partCrewManifest.AddCrewToSeat(protoCrewMember2, k);
					list.Remove(protoCrewMember2);
				}
				else if (!flag2)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_21195", k, partCrewManifest.PartInfo.partPrefab.CrewCapacity, partCrewManifest.PartInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
					break;
				}
			}
			break;
		}
		return vesselCrewManifest;
	}

	public int IndexOf(ProtoCrewMember pcm)
	{
		return kerbals.IndexOf(pcm);
	}

	public IEnumerable<ProtoCrewMember> Kerbals(ProtoCrewMember.KerbalType type, params ProtoCrewMember.RosterStatus[] status)
	{
		int kC = kerbals.Count;
		int i = 0;
		while (i < kC)
		{
			if (kerbals.At(i).type == type)
			{
				if (status.Length != 0)
				{
					int j = 0;
					for (int num = status.Length; j < num; j++)
					{
						if (kerbals.At(i).rosterStatus == status[j])
						{
							yield return kerbals.At(i);
							break;
						}
					}
				}
				else
				{
					yield return kerbals.At(i);
				}
			}
			int num2 = i + 1;
			i = num2;
		}
	}

	public IEnumerable<ProtoCrewMember> Kerbals(params ProtoCrewMember.RosterStatus[] status)
	{
		int kC = kerbals.Count;
		int i = 0;
		while (i < kC)
		{
			if (status.Length != 0)
			{
				int j = 0;
				for (int num = status.Length; j < num; j++)
				{
					if (kerbals.At(i).rosterStatus == status[j])
					{
						yield return kerbals.At(i);
						break;
					}
				}
			}
			else
			{
				yield return kerbals.At(i);
			}
			int num2 = i + 1;
			i = num2;
		}
	}

	public int GetActiveCrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && (protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned || protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available || protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Missing))
			{
				num++;
			}
		}
		return num;
	}

	public int GetAssignedCrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAvailableCrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available)
			{
				num++;
			}
		}
		return num;
	}

	public int GetLostCrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && (protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Dead || protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Missing))
			{
				num++;
			}
		}
		return num;
	}

	public int GetMissingCrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Missing)
			{
				num++;
			}
		}
		return num;
	}

	public int GetKIACrewCount()
	{
		int num = 0;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Dead)
			{
				num++;
			}
		}
		return num;
	}

	public ProtoCrewMember GetNextApplicant()
	{
		int num = 0;
		int count = kerbals.Count;
		while (true)
		{
			if (num < count)
			{
				if (kerbals.At(num).type == ProtoCrewMember.KerbalType.Applicant)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return kerbals.At(num);
	}

	public void PCMUpdate(double double_0)
	{
		int i = 0;
		for (int count = kerbals.Count; i < count; i++)
		{
			kerbals.At(i).Update(double_0);
		}
	}

	public void Update(double double_0)
	{
		int num = 0;
		bool flag = false;
		int count = kerbals.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = kerbals.At(count);
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Applicant)
			{
				if (protoCrewMember.UTaR > 0.0 && protoCrewMember.UTaR < double_0)
				{
					flag = true;
					Remove(protoCrewMember);
				}
				else
				{
					num++;
				}
			}
		}
		if (flag || num == 0)
		{
			int b = applicantGroupSizeTarget + UnityEngine.Random.Range(-applicantGroupSizeVariance, applicantGroupSizeVariance);
			int num2 = Mathf.Max(num, b) - num;
			while (num2-- > 0)
			{
				AddApplicant(double_0);
			}
		}
	}

	public void UpdateExperience()
	{
		for (int i = 0; i < Count; i++)
		{
			this[i].UpdateExperience();
		}
	}

	public void AddApplicant(double double_0)
	{
		ProtoCrewMember protoCrewMember;
		do
		{
			protoCrewMember = CrewGenerator.RandomCrewMemberPrototype(ProtoCrewMember.KerbalType.Applicant);
		}
		while (Exists(protoCrewMember.name));
		protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
		protoCrewMember.UTaR = double_0 + (double)(UnityEngine.Random.Range(1f, 3f) * 86400f);
		protoCrewMember = new ProtoCrewMember(protoCrewMember);
		AddCrewMember(protoCrewMember);
	}

	public void HireApplicant(ProtoCrewMember ap)
	{
		if (kerbals.Contains(ap.name) && ap.type == ProtoCrewMember.KerbalType.Applicant)
		{
			GameEvents.OnCrewmemberHired.Fire(ap, GetActiveCrewCount());
			ap.type = ProtoCrewMember.KerbalType.Crew;
		}
		else
		{
			Debug.LogError("[Applicants List Error]: Cannot Hire " + ap.name + ", is not an applicant");
		}
	}

	public void SackAvailable(ProtoCrewMember ap)
	{
		if (kerbals.Contains(ap.name) && ap.type == ProtoCrewMember.KerbalType.Crew && ap.rosterStatus == ProtoCrewMember.RosterStatus.Available)
		{
			ap.type = ProtoCrewMember.KerbalType.Applicant;
			GameEvents.OnCrewmemberSacked.Fire(ap, GetActiveCrewCount());
		}
		else
		{
			Debug.LogError("[Applicants List Error]: Cannot sack " + ap.name + ", is not part of the available crew");
		}
	}

	public void RemoveMIA(ProtoCrewMember ap)
	{
		if (kerbals.Contains(ap.name) && ap.type == ProtoCrewMember.KerbalType.Crew && ap.rosterStatus == ProtoCrewMember.RosterStatus.Missing)
		{
			ap.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
			GameEvents.OnCrewmemberLeftForDead.Fire(ap, GetActiveCrewCount());
		}
		else
		{
			Debug.LogError("[Applicants List Error]: Cannot give up " + ap.name + " for dead, is not MIA");
		}
	}

	public void RemoveDead(ProtoCrewMember ap)
	{
		if (kerbals.Contains(ap.name) && ap.type == ProtoCrewMember.KerbalType.Crew && ap.rosterStatus == ProtoCrewMember.RosterStatus.Dead)
		{
			Remove(ap);
		}
		else
		{
			Debug.LogError("[Applicants List Error]: Cannot remove " + ap.name + ", is not dead");
		}
	}

	public static void GenerateExperienceTypes()
	{
		GenerateExperienceTraitTypes();
		GenerateExperienceEffectTypes();
	}

	public static void GenerateExperienceTraitTypes()
	{
		if (ExperienceTraitTypes != null)
		{
			return;
		}
		ExperienceTraitTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(ExperienceTrait)) && !(t == typeof(ExperienceTrait)))
			{
				ExperienceTraitTypes.Add(t);
			}
		});
		Debug.Log("[ExperienceSystem]: Found " + ExperienceTraitTypes.Count + " trait types");
	}

	public static Type GetExperienceTraitType(string typeName)
	{
		int num = 0;
		int count = ExperienceTraitTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (ExperienceTraitTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return ExperienceTraitTypes[num];
	}

	public static void GenerateExperienceEffectTypes()
	{
		if (ExperienceEffectTypes != null)
		{
			return;
		}
		ExperienceEffectTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(ExperienceEffect)) && !(t == typeof(ExperienceEffect)))
			{
				ExperienceEffectTypes.Add(t);
			}
		});
		Debug.Log("[ExperienceSystem]: Found " + ExperienceEffectTypes.Count + " effect types");
	}

	public static Type GetExperienceEffectType(string typeName)
	{
		int num = 0;
		int count = ExperienceEffectTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (ExperienceEffectTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return ExperienceEffectTypes[num];
	}

	public static void SetExperienceTrait(ProtoCrewMember pcm, string traitName = null)
	{
		if (!string.IsNullOrEmpty(traitName))
		{
			pcm.trait = traitName;
		}
		if (pcm.type == ProtoCrewMember.KerbalType.Tourist && GameDatabase.Instance.ExperienceConfigs.GetExperienceTraitConfig(touristTrait) != null)
		{
			pcm.trait = touristTrait;
		}
		if (pcm.type != ProtoCrewMember.KerbalType.Tourist && traitName == touristTrait)
		{
			pcm.trait = null;
		}
		traitName = pcm.trait;
		if (string.IsNullOrEmpty(traitName))
		{
			pcm.trait = (traitName = GameDatabase.Instance.ExperienceConfigs.TraitNamesNoTourist[Math.Abs(pcm.name.GetHashCode_Net35()) % GameDatabase.Instance.ExperienceConfigs.TraitNamesNoTourist.Count]);
		}
		if (pcm.experienceTrait != null && !(pcm.experienceTrait.TypeName != traitName))
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < GameDatabase.Instance.ExperienceConfigs.Categories.Count)
			{
				if (traitName == GameDatabase.Instance.ExperienceConfigs.Categories[num].Name)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Type type = GetExperienceTraitType(traitName);
		if (type == null)
		{
			type = typeof(ExperienceTrait);
		}
		pcm.experienceTrait = ExperienceTrait.Create(type, GameDatabase.Instance.ExperienceConfigs.Categories[num], pcm);
	}

	public static bool TryGetExperienceTraitConfig(string traitName, out ExperienceTraitConfig traitConfig)
	{
		int num = 0;
		while (true)
		{
			if (num < GameDatabase.Instance.ExperienceConfigs.Categories.Count)
			{
				if (traitName == GameDatabase.Instance.ExperienceConfigs.Categories[num].Name)
				{
					break;
				}
				num++;
				continue;
			}
			traitConfig = null;
			return false;
		}
		traitConfig = GameDatabase.Instance.ExperienceConfigs.Categories[num];
		return true;
	}

	public static void SetExperienceLevel(ProtoCrewMember pcm, int level)
	{
		SetExperienceLevel(pcm, level, HighLogic.CurrentGame.Parameters, HighLogic.CurrentGame.Mode);
	}

	public static void SetExperienceLevel(ProtoCrewMember pcm, int level, GameParameters parameters, Game.Modes mode)
	{
		if (!parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(mode))
		{
			level = xpPerLevel.Length;
		}
		level = Mathf.Clamp(level, 0, xpPerLevel.Length);
		pcm.careerLog.Clear();
		if (level != 0)
		{
			FlightLog.EntryType entryType = FlightLog.EntryType.Training1;
			pcm.careerLog.AddEntryUnique(level switch
			{
				1 => FlightLog.EntryType.Training1, 
				2 => FlightLog.EntryType.Training2, 
				3 => FlightLog.EntryType.Training3, 
				4 => FlightLog.EntryType.Training4, 
				_ => FlightLog.EntryType.Training5, 
			}, FlightGlobals.GetHomeBody().name);
			pcm.experience = pcm.CalculateExperiencePoints(parameters, mode);
			pcm.experienceLevel = CalculateExperienceLevel(pcm.experience);
		}
	}

	public static float GetExperienceLevelRequirement(int level)
	{
		if (level < 0)
		{
			return 0f;
		}
		if (level >= xpPerLevel.Length)
		{
			return xpPerLevel[xpPerLevel.Length - 1];
		}
		return xpPerLevel[level];
	}

	public static int GetExperienceMaxLevel()
	{
		return xpPerLevel.Length;
	}

	public static int CalculateExperienceLevel(float xp)
	{
		int num = 0;
		int num2 = xpPerLevel.Length;
		while (true)
		{
			if (num < num2)
			{
				if (!(xp >= xpPerLevel[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return GetExperienceMaxLevel();
		}
		return num;
	}

	public static float CalculateExperience(params FlightLog[] logs)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<float> list3 = new List<float>();
		int num = logs.Length;
		while (num-- > 0)
		{
			if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && num != 0)
			{
				continue;
			}
			List<FlightLog> flights = logs[num].GetFlights();
			int i = 0;
			for (int count = flights.Count; i < count; i++)
			{
				if ((HighLogic.CurrentGame == null || HighLogic.CurrentGame.Mode != Game.Modes.MISSION || i == 0) && !ExperienceAddFlight(flights[i], list, list2, list3))
				{
					list.Clear();
					list3.Clear();
					list2.Clear();
				}
			}
		}
		float num2 = 0f;
		int j = 0;
		for (int count2 = list3.Count; j < count2; j++)
		{
			num2 += list3[j];
		}
		return num2;
	}

	public static string GenerateExperienceLog(FlightLog log)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<float> list3 = new List<float>();
		List<FlightLog> flights = log.GetFlights();
		int i = 0;
		for (int count = flights.Count; i < count; i++)
		{
			if (!ExperienceAddFlight(flights[i], list, list2, list3))
			{
				list.Clear();
				list3.Clear();
				list2.Clear();
			}
		}
		string text = "";
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			text = text + Localizer.Format(list2[j], gettargetdisplayName(list[j])) + " = " + list3[j].ToString("F0");
		}
		return text;
	}

	public static string gettargetdisplayName(string bodyName)
	{
		int num = 0;
		while (true)
		{
			if (num < FlightGlobals.Bodies.Count)
			{
				if (FlightGlobals.Bodies[num].bodyName == bodyName)
				{
					break;
				}
				num++;
				continue;
			}
			return bodyName;
		}
		return FlightGlobals.Bodies[num].displayName;
	}

	public static string GetLocalizedExperienceTraitName(string queriedTrait)
	{
		string text = "";
		return queriedTrait switch
		{
			"Tourist" => Localizer.Format("#autoLOC_900446"), 
			"Scientist" => Localizer.Format("#autoLOC_900442"), 
			"Engineer" => Localizer.Format("#autoLOC_900439"), 
			"Pilot" => Localizer.Format("#autoLOC_900433"), 
			_ => Localizer.Format("#autoLOC_8000022"), 
		};
	}

	public static void InitializeExperienceTypes()
	{
		int i = 0;
		for (int num = xpPerLevel.Length; i < num; i++)
		{
			AddExperienceType("Training" + (i + 1), "#autoLOC_6002265", xpPerLevel[i], xpPerLevel[i]);
		}
		AddExperienceType("PlantFlag", "#autoLOC_270135", 2.5f);
		AddExperienceType("Land", "#autoLOC_286262", 2.3f);
		AddExperienceType("Flight", "#autoLOC_6002260", 2f, 1f);
		AddExperienceType("Suborbit", "#autoLOC_6002261", 0f, 1f);
		AddExperienceType("Flyby", "#autoLOC_6002262", 1f, 1.5f);
		AddExperienceType("Escape", "#autoLOC_6002263", 1f);
		AddExperienceType("Orbit", "#autoLOC_6002264", 1.5f, 2f);
	}

	public static void AddExperienceType(string type, string typeName, float notHomeValue, float homeValue = 0f)
	{
		if (homeValue != 0f)
		{
			xpHomeValues.Add(homeValue);
			xpHomeTypes.Add(type);
			xpHomeTypeNames.Add(typeName);
		}
		if (notHomeValue != 0f)
		{
			xpNotHomeValues.Add(notHomeValue);
			xpNotHomeTypes.Add(type);
			xpNotHomeTypeNames.Add(typeName);
		}
	}

	public static bool ExperienceAddFlight(FlightLog flight, List<string> targets, List<string> types, List<float> targetXP)
	{
		FlightLog.Entry entry = flight.Last();
		if (entry != null && !(entry.type == "Die"))
		{
			int i = 0;
			for (int count = flight.Count; i < count; i++)
			{
				entry = flight[i];
				if (string.IsNullOrEmpty(entry.target))
				{
					continue;
				}
				string type = null;
				float value = 0f;
				if (!ExperienceBodyValue(entry.target, entry.type, out type, out value))
				{
					continue;
				}
				int num = targets.IndexOf(entry.target);
				if (num != -1)
				{
					if (targetXP[num] < value)
					{
						types[num] = type;
						targetXP[num] = value;
					}
				}
				else
				{
					targets.Add(entry.target);
					types.Add(type);
					targetXP.Add(value);
				}
			}
			return true;
		}
		return false;
	}

	public static bool ExperienceBodyValue(string bodyName, string entryType, out string type, out float value)
	{
		type = null;
		value = 0f;
		if (string.IsNullOrEmpty(bodyName))
		{
			return false;
		}
		int num = 0;
		CelestialBody celestialBody;
		while (true)
		{
			if (num < PSystemManager.Instance.localBodies.Count)
			{
				celestialBody = PSystemManager.Instance.localBodies[num];
				if (celestialBody.name == bodyName || celestialBody.displayName == bodyName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		if (celestialBody.isHomeWorld)
		{
			return ExperienceHomeTypeValue(entryType, celestialBody.scienceValues.RecoveryValue, out type, out value);
		}
		return ExperienceNotHomeTypeValue(entryType, celestialBody.scienceValues.RecoveryValue, out type, out value);
	}

	public static bool ExperienceHomeTypeValue(string typeName, float baseValue, out string type, out float value)
	{
		type = null;
		value = baseValue;
		if (string.IsNullOrEmpty(typeName))
		{
			return false;
		}
		int count = xpHomeTypes.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(typeName == xpHomeTypes[count]));
		value *= xpHomeValues[count];
		type = xpHomeTypeNames[count];
		return true;
	}

	public static bool ExperienceNotHomeTypeValue(string typeName, float baseValue, out string type, out float value)
	{
		type = null;
		value = baseValue;
		if (string.IsNullOrEmpty(typeName))
		{
			return false;
		}
		int count = xpNotHomeTypes.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(typeName == xpNotHomeTypes[count]));
		value *= xpNotHomeValues[count];
		type = xpNotHomeTypeNames[count];
		return true;
	}

	public static void CheatExperience()
	{
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.CrewRoster == null || FlightGlobals.Bodies == null || !HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode))
		{
			return;
		}
		foreach (ProtoCrewMember item in HighLogic.CurrentGame.CrewRoster.Crew)
		{
			SetExperienceLevel(item, GetExperienceMaxLevel());
		}
		CrewListItem[] array = UnityEngine.Object.FindObjectsOfType<CrewListItem>();
		int num = array.Length;
		while (num-- > 0)
		{
			CrewListItem crewListItem = array[num];
			if (!(crewListItem == null))
			{
				ProtoCrewMember crewRef = crewListItem.GetCrewRef();
				if (crewRef != null && crewRef.type == ProtoCrewMember.KerbalType.Crew)
				{
					crewListItem.SetXP(crewRef);
				}
			}
		}
	}
}
