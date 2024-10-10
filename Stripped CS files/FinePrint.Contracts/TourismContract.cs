using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using Contracts.Parameters;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts;

public class TourismContract : Contract
{
	public string contractSubject = string.Empty;

	public List<string> tourists;

	public string preposition;

	public bool homeDestinations;

	public bool isGeeAdventure;

	public List<string> Tourists => tourists;

	public int TouristCount
	{
		get
		{
			if (tourists != null && tourists.Count > 0)
			{
				return tourists.Count;
			}
			return 0;
		}
	}

	public bool SingleDestination => base.ParameterCount == 1;

	public string FirstTourist
	{
		get
		{
			if (tourists != null && tourists.Count >= 1)
			{
				return tourists[0];
			}
			return string.Empty;
		}
	}

	public string TouristNamesJoined
	{
		get
		{
			if (tourists == null)
			{
				return string.Empty;
			}
			List<string> list = new List<string>();
			int count = tourists.Count;
			for (int i = 0; i < count; i++)
			{
				list.Add(StringUtilities.ShortKerbalName(tourists[i]));
			}
			return StringUtilities.ThisThisAndThat(list);
		}
	}

	public static ProtoCrewMember GetKerbal(string name)
	{
		if (HighLogic.CurrentGame.CrewRoster.Exists(name))
		{
			return HighLogic.CurrentGame.CrewRoster[name];
		}
		return null;
	}

	public override bool Generate()
	{
		int num = 0;
		int num2 = 0;
		TourismContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<TourismContract>();
		int num3 = currentContracts.Length;
		while (num3-- > 0)
		{
			switch (currentContracts[num3].ContractState)
			{
			case State.Active:
				num2++;
				break;
			case State.Offered:
				num++;
				break;
			}
		}
		if (num < ContractDefs.Tour.MaximumAvailable && num2 < ContractDefs.Tour.MaximumActive)
		{
			KSPRandom kSPRandom = new KSPRandom(SystemUtilities.SuperSeed(this));
			tourists = new List<string>();
			int num4 = 0;
			switch (prestige)
			{
			case ContractPrestige.Trivial:
				num4 = kSPRandom.Next(1, 3);
				break;
			case ContractPrestige.Significant:
				num4 = kSPRandom.Next(2, 5);
				break;
			case ContractPrestige.Exceptional:
				num4 = kSPRandom.Next(3, 7);
				break;
			}
			isGeeAdventure = HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits && kSPRandom.NextDouble() < (double)ContractDefs.Tour.GeeAdventureChance;
			CelestialBody celestialBody = Planetarium.fetch.Home;
			if (!isGeeAdventure)
			{
				List<KeyValuePair<CelestialBody, FlightLog.EntryType>> list = BuildPossibleItineraries(kSPRandom);
				if (list.Count == 0)
				{
					return false;
				}
				homeDestinations = true;
				for (int i = 0; i < num4; i++)
				{
					ProtoCrewMember protoCrewMember = null;
					bool flag = false;
					while (!flag)
					{
						protoCrewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Unowned);
						if (!SystemUtilities.CheckTouristRecoveryContractKerbals(CrewGenerator.RemoveLastName(protoCrewMember.name)))
						{
							flag = true;
						}
					}
					protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
					protoCrewMember.trait = KerbalRoster.touristTrait;
					protoCrewMember.seat = null;
					protoCrewMember.seatIdx = -1;
					tourists.Add(protoCrewMember.name);
					int val = 0;
					switch (prestige)
					{
					case ContractPrestige.Trivial:
						val = kSPRandom.Next(1, 3);
						break;
					case ContractPrestige.Significant:
						val = kSPRandom.Next(2, 4);
						break;
					case ContractPrestige.Exceptional:
						val = kSPRandom.Next(3, 5);
						break;
					}
					List<KeyValuePair<CelestialBody, FlightLog.EntryType>> list2 = new List<KeyValuePair<CelestialBody, FlightLog.EntryType>>(list);
					List<KeyValuePair<CelestialBody, FlightLog.EntryType>> list3 = new List<KeyValuePair<CelestialBody, FlightLog.EntryType>>();
					val = Math.Min(val, list2.Count);
					if (val != 0)
					{
						for (int j = 0; j < val; j++)
						{
							KeyValuePair<CelestialBody, FlightLog.EntryType> item = WeightedItineraryChoice(list2, kSPRandom);
							list3.Add(item);
							list2.Remove(item);
						}
						ContractParameter contractParameter = AddParameter(new KerbalTourParameter(protoCrewMember.name, protoCrewMember.gender));
						CelestialBody celestialBody2 = Planetarium.fetch.Home;
						int count = list3.Count;
						for (int k = 0; k < count; k++)
						{
							KeyValuePair<CelestialBody, FlightLog.EntryType> item = list3[k];
							if (item.Key.scienceValues.RecoveryValue >= celestialBody2.scienceValues.RecoveryValue)
							{
								celestialBody2 = item.Key;
							}
							if (item.Key.scienceValues.RecoveryValue >= celestialBody.scienceValues.RecoveryValue)
							{
								celestialBody = item.Key;
							}
							if (homeDestinations && !item.Key.isHomeWorld)
							{
								homeDestinations = false;
							}
							contractParameter.AddParameter(new KerbalDestinationParameter(item.Key, item.Value, protoCrewMember.name));
						}
						float num5 = ContractDefs.Tour.Funds.DefaultFare * (float)contractParameter.ParameterCount;
						float completion = num5 * 0.25f / (float)contractParameter.ParameterCount;
						float completion2 = num5 * 0.75f;
						IEnumerator<ContractParameter> enumerator = contractParameter.AllParameters.GetEnumerator();
						while (enumerator.MoveNext())
						{
							enumerator.Current?.SetFunds(completion, celestialBody2);
						}
						contractParameter.SetFunds(completion2, celestialBody2);
						continue;
					}
					return false;
				}
			}
			else
			{
				for (int l = 0; l < num4; l++)
				{
					ProtoCrewMember newKerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Unowned);
					newKerbal.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
					newKerbal.trait = KerbalRoster.touristTrait;
					newKerbal.seat = null;
					newKerbal.seatIdx = -1;
					tourists.Add(newKerbal.name);
					ContractParameter contractParameter2 = AddParameter(new KerbalGeeAdventureParameter(newKerbal.name));
					contractParameter2.AddParameter(new ReachDestination(FlightGlobals.GetHomeBody(), "")).DisableOnStateChange = false;
					Vessel.Situations sit = ((prestige == ContractPrestige.Trivial) ? Vessel.Situations.FLYING : ((prestige == ContractPrestige.Significant) ? Vessel.Situations.SUB_ORBITAL : Vessel.Situations.ORBITING));
					contractParameter2.AddParameter(new ReachSituation(sit, "")).DisableOnStateChange = false;
					contractParameter2.SetFunds(ContractDefs.Tour.Funds.DefaultFare, FlightGlobals.GetHomeBody());
				}
			}
			GetContractSubject();
			SetExpiry(ContractDefs.Tour.Expire.MinimumExpireDays, ContractDefs.Tour.Expire.MaximumExpireDays);
			SetDeadlineDays(ContractDefs.Tour.Expire.DeadlineDays, celestialBody);
			SetScience(ContractDefs.Tour.Science.BaseReward);
			SetReputation(ContractDefs.Tour.Reputation.BaseReward, ContractDefs.Tour.Reputation.BaseFailure);
			return true;
		}
		return false;
	}

	public override bool CanBeCancelled()
	{
		return true;
	}

	public override bool CanBeDeclined()
	{
		return true;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(this).ToString(CultureInfo.InvariantCulture);
	}

	public override string GetTitle()
	{
		if (isGeeAdventure)
		{
			return Localizer.Format("#autoLOC_7000002", TouristCount);
		}
		return Localizer.Format("#autoLOC_7000003", TouristCount, contractSubject, Convert.ToInt32(SingleDestination), Convert.ToInt32(homeDestinations));
	}

	public override string GetDescription()
	{
		string fullKerbalName = ((TouristCount <= 0) ? "#autoLOC_7000019" : FirstTourist);
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, StringUtilities.ShortKerbalName(fullKerbalName), "#autoLOC_7000020", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: true, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		string text = ((TouristCount > 1) ? TouristNamesJoined : FirstTourist);
		if (isGeeAdventure)
		{
			return Localizer.Format("#autoLOC_7000000", text, TouristCount, contractSubject);
		}
		return Localizer.Format("#autoLOC_7000001", text, TouristCount, preposition, contractSubject);
	}

	public override string GetNotes()
	{
		if (isGeeAdventure)
		{
			return string.Empty;
		}
		string text = "";
		text += Localizer.Format("#autoLOC_7000004", TouristCount, contractSubject, Planetarium.fetch.Home.displayName);
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits && ContractDefs.Tour.FailOnInactive)
		{
			text += Localizer.Format("#autoLOC_281970");
		}
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			text += "\n";
		}
		return text;
	}

	public override string MessageCompleted()
	{
		string text = ((TouristCount > 1) ? TouristNamesJoined : FirstTourist);
		if (isGeeAdventure)
		{
			return Localizer.Format("#autoLOC_7000005", text, contractSubject);
		}
		return Localizer.Format("#autoLOC_7000006", text);
	}

	public override string MessageFailed()
	{
		return Localizer.Format("#autoLOC_7000007", tourists.Count);
	}

	public override string MessageCancelled()
	{
		string text = ((TouristCount > 1) ? TouristNamesJoined : FirstTourist);
		return Localizer.Format("#autoLOC_7003400", text, contractSubject);
	}

	public override string MessageDeadlineExpired()
	{
		string text = ((TouristCount > 1) ? TouristNamesJoined : FirstTourist);
		return Localizer.Format("#autoLOC_7000008", text, contractSubject);
	}

	public override void OnSave(ConfigNode node)
	{
		if (!string.IsNullOrEmpty(preposition))
		{
			node.AddValue("preposition", preposition);
		}
		node.AddValue("homeDestinations", homeDestinations);
		node.AddValue("isGeeAdventure", isGeeAdventure);
		SystemUtilities.SaveNodeList(node, "tourists", tourists);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "TourismContract", "preposition", ref preposition, "of the solar system");
		SystemUtilities.LoadNode(node, "TourismContract", "homeDestinations", ref homeDestinations, defaultValue: false);
		SystemUtilities.LoadNode(node, "TourismContract", "isGeeAdventure", ref isGeeAdventure, defaultValue: false);
		SystemUtilities.LoadNodeList(node, "TourismContract", "tourists", ref tourists);
		GetContractSubject();
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete("ReachedSpace"))
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		List<CelestialBody> list = new List<CelestialBody>();
		IEnumerator<ContractParameter> enumerator = base.AllParameters.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ContractParameter current = enumerator.Current;
			if (!(current is KerbalTourParameter))
			{
				continue;
			}
			IEnumerator<ContractParameter> enumerator2 = current.AllParameters.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				if (enumerator2.Current is KerbalDestinationParameter kerbalDestinationParameter && !list.Contains(kerbalDestinationParameter.targetBody))
				{
					list.Add(kerbalDestinationParameter.targetBody);
				}
			}
		}
		return list;
	}

	public override void OnAccepted()
	{
		int count = tourists.Count;
		while (count-- > 0)
		{
			ProtoCrewMember kerbal = GetKerbal(tourists[count]);
			if (kerbal != null)
			{
				kerbal.type = ProtoCrewMember.KerbalType.Tourist;
				KerbalRoster.SetExperienceTrait(kerbal, KerbalRoster.touristTrait);
				kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			}
		}
	}

	public List<KeyValuePair<CelestialBody, FlightLog.EntryType>> BuildPossibleItineraries(Random generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<KeyValuePair<CelestialBody, FlightLog.EntryType>> list = new List<KeyValuePair<CelestialBody, FlightLog.EntryType>>();
		List<CelestialBody> list2 = PossibleBodies(generator);
		if (list2 != null && list2.Count > 0)
		{
			if (base.Prestige <= ContractPrestige.Significant)
			{
				preposition = Localizer.Format("#autoLOC_7000038", (!list2[0].isHomeWorld) ? 1 : 0, list2[0].displayName);
			}
			else
			{
				preposition = Localizer.Format("#autoLOC_282101");
			}
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				CelestialBody celestialBody = list2[i];
				List<FlightLog.EntryType> list3 = PossibleSituations(celestialBody);
				int count2 = list3.Count;
				for (int j = 0; j < count2; j++)
				{
					list.Add(new KeyValuePair<CelestialBody, FlightLog.EntryType>(celestialBody, list3[j]));
				}
			}
			return list;
		}
		return list;
	}

	public List<CelestialBody> PossibleBodies(Random generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<CelestialBody> list = new List<CelestialBody> { Planetarium.fetch.Home };
		list.AddRange(ProgressUtilities.GetBodiesProgress(bodyReached: true, MannedStatus.MANNED, (CelestialBody cb) => cb != Planetarium.fetch.Home));
		List<CelestialBody> list2 = new List<CelestialBody>();
		switch (prestige)
		{
		default:
			list2.AddRange(list);
			break;
		case ContractPrestige.Significant:
			list2.AddRange(RandomReachedBodyGrouping(list, generator));
			break;
		case ContractPrestige.Trivial:
			list2.Add(WeightedBodyChoice(list, generator));
			break;
		}
		return list2;
	}

	public List<FlightLog.EntryType> PossibleSituations(CelestialBody body)
	{
		List<FlightLog.EntryType> list = new List<FlightLog.EntryType>();
		if (body == Planetarium.fetch.Home)
		{
			if (ProgressUtilities.GetBodyProgress(ProgressType.REACHSPACE, body, MannedStatus.MANNED))
			{
				list.Add(FlightLog.EntryType.Suborbit);
			}
			if (ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, body, MannedStatus.MANNED))
			{
				list.Add(FlightLog.EntryType.Orbit);
			}
		}
		else
		{
			if (ProgressUtilities.GetBodyProgress(ProgressType.FLYBY, body, MannedStatus.MANNED))
			{
				list.Add(FlightLog.EntryType.Flyby);
			}
			if (ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, body, MannedStatus.MANNED))
			{
				list.Add(FlightLog.EntryType.Orbit);
			}
			if (!CelestialUtilities.IsGasGiant(body) && (ProgressUtilities.GetBodyProgress(ProgressType.LANDING, body, MannedStatus.MANNED) || ProgressUtilities.GetBodyProgress(ProgressType.SPLASHDOWN, body, MannedStatus.MANNED)))
			{
				list.Add(FlightLog.EntryType.Land);
			}
			if (CelestialUtilities.IsFlyablePlanet(body) && ProgressUtilities.GetBodyProgress(ProgressType.FLIGHT, body, MannedStatus.MANNED))
			{
				list.Add(FlightLog.EntryType.Flight);
			}
		}
		return list;
	}

	public List<CelestialBody> RandomReachedBodyGrouping(List<CelestialBody> reachedBodies, Random generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count = reachedBodies.Count;
		while (count-- > 0)
		{
			if (reachedBodies[count].referenceBody != null && reachedBodies[count].referenceBody == Planetarium.fetch.Sun)
			{
				list.Add(reachedBodies[count]);
			}
		}
		if (list.Count == 0)
		{
			return list;
		}
		CelestialBody celestialBody = WeightedBodyChoice(list, generator);
		List<CelestialBody> list2 = new List<CelestialBody> { celestialBody };
		int count2 = celestialBody.orbitingBodies.Count;
		while (count2-- > 0)
		{
			CelestialBody item = celestialBody.orbitingBodies[count2];
			if (reachedBodies.Contains(item))
			{
				list2.Add(item);
			}
		}
		return list2;
	}

	public KeyValuePair<CelestialBody, FlightLog.EntryType> WeightedItineraryChoice(List<KeyValuePair<CelestialBody, FlightLog.EntryType>> itineraryChoices, Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		List<KeyValuePair<CelestialBody, FlightLog.EntryType>> list = new List<KeyValuePair<CelestialBody, FlightLog.EntryType>>(itineraryChoices);
		List<CelestialBody> list2 = new List<CelestialBody>();
		int count = list.Count;
		while (count-- > 0)
		{
			CelestialBody key = list[count].Key;
			if (!list2.Contains(key))
			{
				list2.Add(key);
			}
		}
		CelestialBody celestialBody = WeightedBodyChoice(list2, (kSPRandom != null) ? kSPRandom : generator);
		int count2 = list.Count;
		while (count2-- > 0)
		{
			KeyValuePair<CelestialBody, FlightLog.EntryType> item = list[count2];
			if (item.Key != celestialBody)
			{
				list.Remove(item);
			}
		}
		return list[kSPRandom?.Next(0, list.Count) ?? generator.Next(0, list.Count)];
	}

	public void ClearKerbalsSoft()
	{
		int count = tourists.Count;
		while (count-- > 0)
		{
			ProtoCrewMember kerbal = GetKerbal(tourists[count]);
			if (kerbal != null)
			{
				if (kerbal.type == ProtoCrewMember.KerbalType.Tourist && kerbal.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
				{
					kerbal.hasToured = true;
				}
				else
				{
					HighLogic.CurrentGame.CrewRoster.Remove(kerbal.name);
				}
			}
		}
	}

	public override void OnCancelled()
	{
		ClearKerbalsSoft();
	}

	public override void OnDeadlineExpired()
	{
		ClearKerbalsSoft();
	}

	public override void OnFailed()
	{
		ClearKerbalsSoft();
	}

	public override void OnFinished()
	{
		ClearKerbalsSoft();
	}

	public void ClearKerbalsHard()
	{
		int count = tourists.Count;
		while (count-- > 0)
		{
			ProtoCrewMember kerbal = GetKerbal(tourists[count]);
			if (kerbal != null)
			{
				HighLogic.CurrentGame.CrewRoster.Remove(kerbal.name);
			}
		}
	}

	public override void OnGenerateFailed()
	{
		ClearKerbalsHard();
	}

	public override void OnWithdrawn()
	{
		ClearKerbalsHard();
	}

	public override void OnDeclined()
	{
		ClearKerbalsHard();
	}

	public override void OnOfferExpired()
	{
		ClearKerbalsHard();
	}

	public void GetContractSubject()
	{
		if (tourists.Count > 1)
		{
			contractSubject = "^p";
			return;
		}
		ProtoCrewMember kerbal = GetKerbal(FirstTourist);
		if (kerbal != null)
		{
			contractSubject = ((kerbal.gender == ProtoCrewMember.Gender.Male) ? "^m" : "^f");
		}
		else
		{
			contractSubject = "^m";
		}
	}

	public string Pluralize(string single, string plural)
	{
		if (TouristCount != 1)
		{
			return plural;
		}
		return single;
	}
}
