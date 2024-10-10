using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts.Parameters;
using FinePrint;
using FinePrint.Utilities;
using ns11;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

public class RecoverAsset : Contract
{
	public enum RecoveryLocation
	{
		NONE,
		ORBITLOW,
		ORBITHIGH,
		SURFACE
	}

	public enum RecoveryType
	{
		NONE,
		KERBAL,
		PART,
		COMPOUND
	}

	public CelestialBody targetBody;

	public RecoveryLocation recoveryLocation;

	public RecoveryType recoveryType;

	public ProtoCrewMember recoveryKerbal;

	public string partName = "";

	public uint partID;

	public const string evaStringMale = "kerbalEVA";

	public const string evaStringFemale = "kerbalEVAfemale";

	public const string evaStringMaleVintage = "kerbalEVAVintage";

	public const string evaStringFemaleVintage = "kerbalEVAfemaleVintage";

	public static readonly string[] RecoveringKerbalDescriptionStrings = new string[10] { "Shipwreck", "Wreckage", "Pod", "Capsule", "Derelict", "Heap", "Hulk", "Craft", "Debris", "Scrap" };

	public static readonly string[] DescriptionStrings = new string[6] { "Prototype", "Device", "Part", "Module", "Unit", "Component" };

	public ProtoCrewMember RecoveryKerbal => recoveryKerbal;

	public bool PartIsValid
	{
		get
		{
			if (partName != "kerbalEVA" && partName != "kerbalEVAfemale" && partName != "kerbalEVAVintage" && partName != "kerbalEVAfemaleVintage")
			{
				return PartLoader.getPartInfoByName(partName) != null;
			}
			return false;
		}
	}

	public bool RecoveringKerbal
	{
		get
		{
			if (recoveryType != RecoveryType.KERBAL)
			{
				return recoveryType == RecoveryType.COMPOUND;
			}
			return true;
		}
	}

	public bool RecoveringPart
	{
		get
		{
			if (recoveryType != RecoveryType.PART)
			{
				return recoveryType == RecoveryType.COMPOUND;
			}
			return true;
		}
	}

	public string VesselDescription
	{
		get
		{
			KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
			if (RecoveringKerbal)
			{
				return RecoveringKerbalDescriptionStrings[kSPRandom.Next(0, RecoveringKerbalDescriptionStrings.Length)];
			}
			return DescriptionStrings[kSPRandom.Next(0, DescriptionStrings.Length)];
		}
	}

	public string VesselName
	{
		get
		{
			if (!RecoveringKerbal)
			{
				return Localizer.Format("#autoLOC_6100053", VesselDescription, StringUtilities.AlphaNumericDesignation(base.MissionSeed));
			}
			return Localizer.Format("#autoLOC_6100052", StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender), VesselDescription);
		}
	}

	public ProtoCrewMember ProtoKerbal => HighLogic.CurrentGame.CrewRoster[recoveryKerbal.name];

	public RecoverAsset()
	{
		RecoveringKerbalDescriptionStrings[0] = Localizer.Format("#autoLOC_6100036");
		RecoveringKerbalDescriptionStrings[1] = Localizer.Format("#autoLOC_6100037");
		RecoveringKerbalDescriptionStrings[2] = Localizer.Format("#autoLOC_6100038");
		RecoveringKerbalDescriptionStrings[3] = Localizer.Format("#autoLOC_6100039");
		RecoveringKerbalDescriptionStrings[4] = Localizer.Format("#autoLOC_6100040");
		RecoveringKerbalDescriptionStrings[5] = Localizer.Format("#autoLOC_6100041");
		RecoveringKerbalDescriptionStrings[6] = Localizer.Format("#autoLOC_6100042");
		RecoveringKerbalDescriptionStrings[7] = Localizer.Format("#autoLOC_6100043");
		RecoveringKerbalDescriptionStrings[8] = Localizer.Format("#autoLOC_6100044");
		RecoveringKerbalDescriptionStrings[9] = Localizer.Format("#autoLOC_6100045");
		DescriptionStrings[0] = Localizer.Format("#autoLOC_6100046");
		DescriptionStrings[1] = Localizer.Format("#autoLOC_6100047");
		DescriptionStrings[2] = Localizer.Format("#autoLOC_6100048");
		DescriptionStrings[3] = Localizer.Format("#autoLOC_6100049");
		DescriptionStrings[4] = Localizer.Format("#autoLOC_6100050");
		DescriptionStrings[5] = Localizer.Format("#autoLOC_6100051");
	}

	public override bool Generate()
	{
		int num = 0;
		int num2 = 0;
		RecoverAsset[] currentContracts = ContractSystem.Instance.GetCurrentContracts<RecoverAsset>();
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
		if (num < ContractDefs.Recovery.MaximumAvailable && num2 < ContractDefs.Recovery.MaximumActive)
		{
			if (!Initialize())
			{
				return false;
			}
			float multiplier = GetMultiplier();
			if (RecoveringKerbal)
			{
				bool flag = false;
				while (!flag)
				{
					recoveryKerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Unowned);
					if (!SystemUtilities.CheckTouristRecoveryContractKerbals(CrewGenerator.RemoveLastName(recoveryKerbal.name)))
					{
						flag = true;
					}
				}
				recoveryKerbal.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
				recoveryKerbal.seat = null;
				recoveryKerbal.seatIdx = -1;
				AcquireCrew obj = (AcquireCrew)AddParameter(new AcquireCrew(Localizer.Format("#autoLOC_274219", recoveryKerbal.nameWithGender)));
				obj.SetFunds(ContractDefs.Recovery.Funds.BaseReward * multiplier * 0.1f, targetBody);
				obj.SetReputation(ContractDefs.Recovery.Reputation.BaseReward * multiplier * 0.3f);
			}
			if (RecoveringPart)
			{
				AcquirePart obj2 = (AcquirePart)AddParameter(new AcquirePart(Localizer.Format("#autoLOC_274226", VesselName)));
				obj2.SetFunds(ContractDefs.Recovery.Funds.BaseReward * multiplier * 0.1f, targetBody);
				obj2.SetReputation(ContractDefs.Recovery.Reputation.BaseReward * multiplier * 0.3f);
			}
			if (RecoveringKerbal)
			{
				AddParameter(new RecoverKerbal(Localizer.Format("#autoLOC_274236", recoveryKerbal.nameWithGender, Planetarium.fetch.Home.displayName)));
			}
			if (RecoveringPart)
			{
				AddParameter(new RecoverPart(Localizer.Format("#autoLOC_274236", VesselName, Planetarium.fetch.Home.displayName), RecoverPart.CompleteCondition.AllCandidates, RecoverPart.CompleteCondition.AnyCandidate));
			}
			SetExpiry(ContractDefs.Recovery.Expire.MinimumExpireDays, ContractDefs.Recovery.Expire.MaximumExpireDays);
			SetDeadlineDays(ContractDefs.Recovery.Expire.DeadlineDays, targetBody);
			SetFunds(ContractDefs.Recovery.Funds.BaseAdvance * multiplier, ContractDefs.Recovery.Funds.BaseReward * multiplier * 0.9f, ContractDefs.Recovery.Funds.BaseFailure * multiplier, targetBody);
			SetReputation(ContractDefs.Recovery.Reputation.BaseReward * multiplier * 0.7f, ContractDefs.Recovery.Reputation.BaseFailure * multiplier * 0.7f);
			SetScience(ContractDefs.Recovery.Science.BaseReward * multiplier);
			return true;
		}
		return false;
	}

	public override void OnAccepted()
	{
		if (RecoveringKerbal)
		{
			GetParameter<AcquireCrew>().AddKerbal(recoveryKerbal.name);
			GetParameter<RecoverKerbal>().AddKerbal(recoveryKerbal.name);
		}
		if (recoveryLocation == RecoveryLocation.SURFACE)
		{
			GenerateLandRecovery();
		}
		else
		{
			GenerateOrbitalRecovery();
		}
		if (RecoveringPart)
		{
			GetParameter<AcquirePart>().AddPart(partID);
			GetParameter<RecoverPart>().AddPartToRecover(partID);
		}
	}

	public override void OnParameterStateChange(ContractParameter p)
	{
		if (p.State == ParameterState.Complete)
		{
			if (RecoveringKerbal)
			{
				ProtoCrewMember protoKerbal = ProtoKerbal;
				if (protoKerbal != null)
				{
					if (p is AcquireCrew)
					{
						protoKerbal.type = ProtoCrewMember.KerbalType.Crew;
						SendStateMessage(Localizer.Format("#autoLOC_6001089", recoveryKerbal.nameWithGender), Localizer.Format("#autoLOC_6001090", recoveryKerbal.nameWithGender), MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.MESSAGE);
					}
					else if (p is RecoverKerbal)
					{
						protoKerbal.type = ProtoCrewMember.KerbalType.Crew;
						CheckComplete();
					}
				}
			}
			if (p is RecoverPart)
			{
				CheckComplete();
			}
		}
		base.OnParameterStateChange(p);
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(this).ToString(CultureInfo.InvariantCulture);
	}

	public override string GetTitle()
	{
		string text = ((recoveryKerbal != null) ? StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender) : string.Empty);
		switch (recoveryType)
		{
		default:
		{
			RecoveryLocation recoveryLocation = this.recoveryLocation;
			if (recoveryLocation == RecoveryLocation.SURFACE)
			{
				return Localizer.Format("#autoLOC_6100058", text, VesselDescription.ToLower(), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_6100059", text, VesselDescription.ToLower(), targetBody.displayName);
		}
		case RecoveryType.PART:
		{
			RecoveryLocation recoveryLocation = this.recoveryLocation;
			if (recoveryLocation == RecoveryLocation.SURFACE)
			{
				return Localizer.Format("#autoLOC_6100056", VesselName, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_6100057", VesselName, targetBody.displayName);
		}
		case RecoveryType.KERBAL:
		{
			RecoveryLocation recoveryLocation = this.recoveryLocation;
			if (recoveryLocation == RecoveryLocation.SURFACE)
			{
				return Localizer.Format("#autoLOC_6100054", text, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_6100055", text, targetBody.displayName);
		}
		}
	}

	public override string GetDescription()
	{
		if (RecoveringKerbal)
		{
			return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender), "RecoverKerbal", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
		}
		if (RecoveringPart)
		{
			return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, VesselName, "RecoverPart", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
		}
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, Planetarium.fetch.Home.displayName, "Kerbal", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		switch (recoveryLocation)
		{
		case RecoveryLocation.SURFACE:
			Localizer.Format("#autoLOC_7001231", targetBody.displayName);
			break;
		default:
			Localizer.Format("#autoLOC_7001233", targetBody.displayName);
			break;
		case RecoveryLocation.ORBITLOW:
			Localizer.Format("#autoLOC_7001232", targetBody.displayName);
			break;
		}
		switch (recoveryType)
		{
		default:
			return recoveryLocation switch
			{
				RecoveryLocation.SURFACE => Localizer.Format("#autoLOC_6100067", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				RecoveryLocation.ORBITLOW => Localizer.Format("#autoLOC_6100066", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				_ => Localizer.Format("#autoLOC_6100068", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
			};
		case RecoveryType.PART:
		{
			KSPRandom generator = new KSPRandom(base.MissionSeed);
			ProtoCrewMember.Gender gender = ((!SystemUtilities.CoinFlip(generator)) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male);
			string randomName = CrewGenerator.GetRandomName(gender, generator);
			randomName = randomName.LocalizeName(gender);
			return recoveryLocation switch
			{
				RecoveryLocation.SURFACE => Localizer.Format("#autoLOC_6100064", StringUtilities.ShortKerbalName(randomName), VesselName, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				RecoveryLocation.ORBITLOW => Localizer.Format("#autoLOC_6100063", StringUtilities.ShortKerbalName(randomName), VesselName, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				_ => Localizer.Format("#autoLOC_6100065", StringUtilities.ShortKerbalName(randomName), VesselName, targetBody.displayName, Planetarium.fetch.Home.displayName), 
			};
		}
		case RecoveryType.KERBAL:
			return recoveryLocation switch
			{
				RecoveryLocation.SURFACE => Localizer.Format("#autoLOC_6100061", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				RecoveryLocation.ORBITLOW => Localizer.Format("#autoLOC_6100060", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
				_ => Localizer.Format("#autoLOC_6100062", recoveryKerbal.nameWithGender, targetBody.displayName, Planetarium.fetch.Home.displayName), 
			};
		}
	}

	public override string GetNotes()
	{
		if (RecoveringPart)
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(partName);
			if (partInfoByName != null)
			{
				Vector3 vector = VesselUtilities.EstimatePartSize(partInfoByName.partPrefab);
				string text = Localizer.Format("#autoLOC_274385", VesselName, partInfoByName.partPrefab.mass.ToString("N1"), vector.y.ToString("N1"), vector.x.ToString("N1"), vector.z.ToString("N1"));
				if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
				{
					return text;
				}
				return text + "\n";
			}
		}
		return null;
	}

	public override string MessageCompleted()
	{
		return recoveryType switch
		{
			RecoveryType.PART => Localizer.Format("#autoLOC_274401", VesselName), 
			RecoveryType.KERBAL => Localizer.Format("#autoLOC_6001091", recoveryKerbal.nameWithGender), 
			_ => Localizer.Format("#autoLOC_6001092", StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender), VesselDescription.ToLower()), 
		};
	}

	public override string MessageFailed()
	{
		ContractParameter parameter = GetParameter<AcquireCrew>();
		ContractParameter parameter2 = GetParameter<AcquirePart>();
		ContractParameter parameter3 = GetParameter<RecoverKerbal>();
		ContractParameter parameter4 = GetParameter<RecoverPart>();
		bool flag = false;
		bool flag2 = false;
		if (parameter != null && parameter.State == ParameterState.Failed)
		{
			flag = true;
		}
		if (parameter2 != null && parameter2.State == ParameterState.Failed)
		{
			flag2 = true;
		}
		if (parameter3 != null && parameter3.State == ParameterState.Failed)
		{
			flag = true;
		}
		if (parameter4 != null && parameter4.State == ParameterState.Failed)
		{
			flag2 = true;
		}
		if (flag && flag2)
		{
			return Localizer.Format("#autoLOC_274430", recoveryKerbal.nameWithGender, VesselName);
		}
		if (flag)
		{
			return Localizer.Format("#autoLOC_274433", recoveryKerbal.nameWithGender);
		}
		if (flag2)
		{
			return Localizer.Format("#autoLOC_274436", VesselName);
		}
		return Localizer.Format("#autoLOC_274438");
	}

	public override string MessageCancelled()
	{
		return recoveryType switch
		{
			RecoveryType.PART => Localizer.Format("#autoLOC_274448", VesselName), 
			RecoveryType.KERBAL => Localizer.Format("#autoLOC_274446", recoveryKerbal.nameWithGender), 
			_ => Localizer.Format("#autoLOC_6001093", StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender), VesselDescription.ToLower()), 
		};
	}

	public override string MessageDeadlineExpired()
	{
		return recoveryType switch
		{
			RecoveryType.PART => Localizer.Format("#autoLOC_274461", VesselName), 
			RecoveryType.KERBAL => Localizer.Format("#autoLOC_274459", recoveryKerbal.nameWithGender), 
			_ => Localizer.Format("#autoLOC_6001094", StringUtilities.ShortKerbalName(recoveryKerbal.nameWithGender), VesselDescription.ToLower()), 
		};
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "Jebediah Kerman";
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "kerbalName", ref value, value);
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "partName", ref partName, "mk1pod.v2");
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "partID", ref partID, 9999u);
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "recoveryLocation", ref recoveryLocation, RecoveryLocation.NONE);
		SystemUtilities.LoadNode(node, "Template.RecoverAsset", "recoveryType", ref recoveryType, RecoveryType.NONE);
		recoveryKerbal = HighLogic.CurrentGame.CrewRoster[value];
	}

	public override void OnSave(ConfigNode node)
	{
		string value = ((recoveryKerbal != null) ? recoveryKerbal.name : "Jebediah Kerman");
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("kerbalName", value);
		node.AddValue("partName", partName);
		node.AddValue("partID", partID);
		node.AddValue("recoveryLocation", (int)recoveryLocation);
		node.AddValue("recoveryType", (int)recoveryType);
	}

	public override bool MeetRequirements()
	{
		if (ProgressTracking.Instance == null)
		{
			return base.Root.ContractState == State.Active;
		}
		if (!ProgressTracking.Instance.NodeComplete("ReachedSpace"))
		{
			return false;
		}
		if (ProgressUtilities.GetProgressLevel() < 4)
		{
			return false;
		}
		if (GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) < GameVariables.OrbitDisplayMode.AllOrbits)
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { targetBody };
	}

	public void Cleanup()
	{
		if (RecoveringKerbal)
		{
			ProtoCrewMember protoKerbal = ProtoKerbal;
			if (protoKerbal != null && protoKerbal.type == ProtoCrewMember.KerbalType.Unowned)
			{
				SystemUtilities.ExpungeKerbal(protoKerbal);
			}
		}
		if (!PartIsValid || partID == 0)
		{
			return;
		}
		List<Vessel> list = new List<Vessel>();
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if (vessel.loaded && !vessel.packed)
			{
				int count2 = vessel.parts.Count;
				while (count2-- > 0)
				{
					if (vessel.parts[count2].flightID == partID)
					{
						list.Add(vessel);
						break;
					}
				}
				continue;
			}
			int count3 = vessel.protoVessel.protoPartSnapshots.Count;
			while (count3-- > 0)
			{
				if (vessel.protoVessel.protoPartSnapshots[count3].flightID == partID)
				{
					list.Add(vessel);
					break;
				}
			}
		}
		int count4 = list.Count;
		while (count4-- > 0)
		{
			Vessel vessel2 = list[count4];
			if (vessel2.loaded && !vessel2.packed)
			{
				if (vessel2.DiscoveryInfo.Level != DiscoveryLevels.Owned)
				{
					vessel2.Die();
				}
				continue;
			}
			ProtoVessel protoVessel = vessel2.protoVessel;
			DiscoveryLevels discoveryLevels = DiscoveryLevels.Unowned;
			if (protoVessel.discoveryInfo.HasValue("state"))
			{
				discoveryLevels = (DiscoveryLevels)int.Parse(protoVessel.discoveryInfo.GetValue("state"));
			}
			if (discoveryLevels != DiscoveryLevels.Owned)
			{
				vessel2.Die();
			}
		}
	}

	public override void OnGenerateFailed()
	{
		Cleanup();
	}

	public override void OnDeclined()
	{
		Cleanup();
	}

	public override void OnOfferExpired()
	{
		Cleanup();
	}

	public override void OnFinished()
	{
		Cleanup();
	}

	public bool Initialize()
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		bool flag = GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		bool num = ResearchAndDevelopment.ResearchedValidContractObjectives("Grapple");
		List<RecoveryType> list = new List<RecoveryType>();
		if (flag && ContractDefs.Recovery.AllowKerbalRescue)
		{
			list.Add(RecoveryType.KERBAL);
		}
		if (num && ContractDefs.Recovery.AllowPartRecovery)
		{
			list.Add(RecoveryType.PART);
			if (flag && prestige != 0 && ContractDefs.Recovery.AllowCompoundRecovery)
			{
				list.Add(RecoveryType.COMPOUND);
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		recoveryType = list[kSPRandom.Next(0, list.Count)];
		ChooseLocation(kSPRandom);
		partName = ChooseVesselType(kSPRandom);
		if (!PartIsValid)
		{
			partName = "kerbalEVA";
			if (!list.Contains(RecoveryType.KERBAL))
			{
				return false;
			}
			recoveryType = RecoveryType.KERBAL;
		}
		return true;
	}

	public void ChooseLocation(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		MannedStatus manned = ((recoveryType != RecoveryType.KERBAL && recoveryType != RecoveryType.COMPOUND) ? MannedStatus.const_2 : MannedStatus.MANNED);
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, progressComplete: true, manned);
		List<CelestialBody> bodiesProgress2 = ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, progressComplete: true, manned, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb.hasSolidSurface);
		if (prestige == ContractPrestige.Exceptional)
		{
			bodiesProgress.Remove(Planetarium.fetch.Home);
		}
		if (!ContractDefs.Recovery.AllowLandedVacuum)
		{
			bodiesProgress2.RemoveAll((CelestialBody cb) => !cb.atmosphere);
		}
		if (!ContractDefs.Recovery.AllowLandedAtmosphere)
		{
			bodiesProgress2.RemoveAll((CelestialBody cb) => cb.atmosphere);
		}
		List<RecoveryLocation> list = new List<RecoveryLocation>();
		if (bodiesProgress.Count > 0)
		{
			list.Add(RecoveryLocation.ORBITHIGH);
			list.Add(RecoveryLocation.ORBITLOW);
		}
		if (bodiesProgress2.Count > 0 && prestige == ContractPrestige.Exceptional)
		{
			list.Add(RecoveryLocation.SURFACE);
		}
		if (prestige != 0 && list.Count > 0 && bodiesProgress2.Count + bodiesProgress.Count > 0)
		{
			recoveryLocation = list[generator.Next(0, list.Count)];
			targetBody = ((recoveryLocation == RecoveryLocation.SURFACE) ? WeightedBodyChoice(bodiesProgress2, generator) : WeightedBodyChoice(bodiesProgress, generator));
		}
		else
		{
			recoveryLocation = RecoveryLocation.ORBITLOW;
			targetBody = Planetarium.fetch.Home;
		}
	}

	public string ChooseVesselType(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<Part> list = new List<Part>();
		int count = PartLoader.LoadedPartsList.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			if (ResearchAndDevelopment.GetTechnologyState(availablePart.TechRequired) != RDTech.State.Available)
			{
				continue;
			}
			Part partPrefab = availablePart.partPrefab;
			if (RecoveringKerbal)
			{
				if (PartHasSeats(VesselUtilities.GetPartName(partPrefab)))
				{
					list.Add(partPrefab);
				}
			}
			else if (AcceptablePart(partPrefab))
			{
				list.Add(partPrefab);
			}
		}
		if (list.Count <= 0)
		{
			return "kerbalEVA";
		}
		list.Sort((Part a, Part b) => a.partInfo.cost.CompareTo(b.partInfo.cost));
		int index = Math.Min(generator.Next(0, list.Count), generator.Next(0, list.Count));
		return VesselUtilities.GetPartName(list[index]);
	}

	public bool AcceptablePart(Part p)
	{
		if (!(p.mass <= 0.5f) && !(p.partInfo.cost <= 0f) && !(p.partInfo.partSize <= 1.5f) && !p.Modules.Contains<ModuleWheelBase>())
		{
			if (p.Modules.Contains<ModuleDeployablePart>())
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public Orbit ChooseOrbit()
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		Orbit orbit;
		switch (recoveryLocation)
		{
		default:
			orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, targetBody);
			break;
		case RecoveryLocation.ORBITHIGH:
			orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, ContractDefs.Recovery.HighOrbitDifficulty, ContractDefs.Recovery.HighOrbitDifficulty);
			break;
		case RecoveryLocation.ORBITLOW:
		{
			double num = targetBody.minOrbitalDistance - targetBody.Radius;
			orbit = Orbit.CreateRandomOrbitAround(targetBody, targetBody.Radius + num * 1.100000023841858, targetBody.Radius + num * 1.25);
			orbit.meanAnomalyAtEpoch = kSPRandom.NextDouble() * 2.0 * Math.PI;
			break;
		}
		}
		return orbit;
	}

	public ConfigNode CreateProcessedPartNode(string part, uint id, params ProtoCrewMember[] crew)
	{
		ConfigNode configNode = ProtoVessel.CreatePartNode(part, id, crew);
		if (part != "kerbalEVA" && part != "kerbalEVAfemale" && part != "kerbalEVAVintage" && part != "kerbalEVAfemaleVintage")
		{
			ConfigNode[] nodes = configNode.GetNodes("RESOURCE");
			int num = nodes.Length;
			while (num-- > 0)
			{
				ConfigNode configNode2 = nodes[num];
				if (configNode2.HasValue("amount"))
				{
					configNode2.SetValue("amount", 0.ToString(CultureInfo.InvariantCulture));
				}
			}
		}
		configNode.SetValue("flag", base.Agent.LogoURL, createIfNotFound: true);
		return configNode;
	}

	public ConfigNode GenerateLoneKerbal()
	{
		ProtoCrewMember protoKerbal = ProtoKerbal;
		if (protoKerbal == null)
		{
			return null;
		}
		uint uniqueFlightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		ConfigNode configNode = ProtoVessel.CreateVesselNode(recoveryKerbal.name, VesselType.const_11, ChooseOrbit(), 0, new ConfigNode[1] { CreateProcessedPartNode((protoKerbal.gender == ProtoCrewMember.Gender.Male) ? "kerbalEVA" : "kerbalEVAfemale", uniqueFlightID, protoKerbal) }, ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Unowned, UntrackedObjectClass.const_0, TimeDeadline * 2.0, TimeDeadline * 2.0));
		configNode.AddValue("prst", value: true);
		return configNode;
	}

	public ConfigNode GenerateLonePart()
	{
		if (!PartIsValid)
		{
			return GenerateLoneKerbal();
		}
		partID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		ConfigNode configNode = ProtoVessel.CreateVesselNode(VesselName, (!RecoveringKerbal) ? VesselType.Unknown : ((recoveryLocation == RecoveryLocation.SURFACE) ? VesselType.Lander : VesselType.Ship), ChooseOrbit(), 0, new ConfigNode[1] { CreateProcessedPartNode(partName, partID) }, new ConfigNode("ACTIONGROUPS"), ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Unowned, UntrackedObjectClass.const_0, TimeDeadline * 2.0, TimeDeadline * 2.0));
		configNode.AddValue("prst", value: true);
		return configNode;
	}

	public ConfigNode GenerateKerbalInPart()
	{
		ProtoCrewMember protoKerbal = ProtoKerbal;
		if (protoKerbal == null)
		{
			return null;
		}
		if (PartIsValid && SeatKerbal(protoKerbal))
		{
			partID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
			ConfigNode configNode = ProtoVessel.CreateVesselNode(VesselName, (recoveryLocation == RecoveryLocation.SURFACE) ? VesselType.Lander : VesselType.Ship, ChooseOrbit(), 0, new ConfigNode[1] { CreateProcessedPartNode(partName, partID, protoKerbal) }, new ConfigNode("ACTIONGROUPS"), ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Unowned, UntrackedObjectClass.const_0, TimeDeadline * 2.0, TimeDeadline * 2.0));
			configNode.AddValue("prst", value: true);
			return configNode;
		}
		return GenerateLoneKerbal();
	}

	public void GroundNode(ref ConfigNode node, double latitude, double longitude)
	{
		if (node.HasValue("sit"))
		{
			node.SetValue("sit", "LANDED");
		}
		if (node.HasValue("landed"))
		{
			node.SetValue("landed", "True");
		}
		if (node.HasValue("lat"))
		{
			node.SetValue("lat", latitude.ToString(CultureInfo.InvariantCulture));
		}
		if (node.HasValue("lon"))
		{
			node.SetValue("lon", longitude.ToString(CultureInfo.InvariantCulture));
		}
		if (node.HasValue("alt"))
		{
			node.SetValue("alt", (CelestialUtilities.TerrainAltitude(targetBody, latitude, longitude) + 50.0).ToString(CultureInfo.InvariantCulture));
		}
		if (node.HasValue("hgt"))
		{
			node.SetValue("hgt", "0");
		}
		if (node.HasValue("rot"))
		{
			node.SetValue("rot", KSPUtil.WriteQuaternion(Quaternion.LookRotation(targetBody.GetSurfaceNVector(latitude, longitude))));
		}
	}

	public bool PartHasSeats(string part)
	{
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(part);
		InternalModel internalModel = null;
		if (partInfoByName != null)
		{
			string text = "";
			if (partInfoByName.partPrefab != null && partInfoByName.partPrefab.CrewCapacity <= 0)
			{
				return false;
			}
			if (partInfoByName.internalConfig.HasValue("name"))
			{
				text = partInfoByName.internalConfig.GetValue("name");
			}
			int count = PartLoader.Instance.internalParts.Count;
			while (count-- > 0)
			{
				InternalModel internalModel2 = PartLoader.Instance.internalParts[count];
				if (!(internalModel2.internalName != text))
				{
					internalModel = internalModel2;
					break;
				}
			}
		}
		if (internalModel != null && internalModel.seats != null && internalModel.seats.Count > 0)
		{
			return true;
		}
		return false;
	}

	public bool SeatKerbal(ProtoCrewMember kerbal)
	{
		if (kerbal != null && PartHasSeats(partName))
		{
			kerbal.seatIdx = 0;
			return true;
		}
		return false;
	}

	public void GenerateOrbitalRecovery()
	{
		ConfigNode configNode = ((!PartIsValid) ? GenerateLoneKerbal() : (RecoveringKerbal ? GenerateKerbalInPart() : GenerateLonePart()));
		if (configNode != null)
		{
			HighLogic.CurrentGame.AddVessel(configNode);
		}
	}

	public void GenerateLandRecovery()
	{
		KSPRandom generator = new KSPRandom(base.MissionSeed);
		double latitude = 0.0;
		double longitude = 0.0;
		WaypointManager.ChooseRandomPosition(out latitude, out longitude, targetBody.GetName(), waterAllowed: false, equatorial: false, generator);
		if (RecoveringKerbal)
		{
			ConfigNode node = GenerateLoneKerbal();
			if (node != null)
			{
				GroundNode(ref node, latitude, longitude);
				HighLogic.CurrentGame.AddVessel(node);
			}
		}
		if (PartIsValid)
		{
			ShiftCoordinates(ref latitude, ref longitude, 10.0, generator);
			ConfigNode node2 = GenerateLonePart();
			if (node2 != null)
			{
				GroundNode(ref node2, latitude, longitude);
				HighLogic.CurrentGame.AddVessel(node2);
			}
		}
	}

	public float GetMultiplier()
	{
		if (recoveryType == RecoveryType.NONE)
		{
			return 1f;
		}
		if (recoveryLocation == RecoveryLocation.SURFACE)
		{
			float num = (float)targetBody.GeeASL;
			num = ((!targetBody.atmosphere) ? (num / 4f) : (num / 2f));
			return recoveryType switch
			{
				RecoveryType.PART => 1f + num, 
				RecoveryType.KERBAL => 1f + num - 0.1f, 
				_ => 1f + num + 0.1f, 
			};
		}
		return recoveryType switch
		{
			RecoveryType.PART => 1f, 
			RecoveryType.KERBAL => 0.9f, 
			_ => 1.1f, 
		};
	}

	public void CheckComplete()
	{
		RecoverKerbal parameter = GetParameter<RecoverKerbal>();
		RecoverPart parameter2 = GetParameter<RecoverPart>();
		if (recoveryType == RecoveryType.KERBAL && parameter != null && parameter.State == ParameterState.Complete)
		{
			Complete();
		}
		if (recoveryType == RecoveryType.PART && parameter2 != null && parameter2.State == ParameterState.Complete)
		{
			Complete();
		}
		if (recoveryType == RecoveryType.COMPOUND && parameter != null && parameter.State == ParameterState.Complete && parameter2 != null && parameter2.State == ParameterState.Complete)
		{
			Complete();
		}
	}

	public void ShiftCoordinates(ref double latitude, ref double longitude, double distance, KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		double num = targetBody.Radius * 2.0 * 3.1415927410125732 / 360.0;
		switch (generator.Next(0, 4))
		{
		default:
			longitude -= distance / num;
			break;
		case 0:
			latitude += distance / num;
			break;
		case 1:
			latitude -= distance / num;
			break;
		case 2:
			longitude += distance / num;
			break;
		}
	}

	public void DiscoverAsset(Vessel v)
	{
		if (RecoveringKerbal)
		{
			ProtoCrewMember protoKerbal = ProtoKerbal;
			if (protoKerbal != null && v.GetVesselCrew().Contains(protoKerbal))
			{
				VesselUtilities.DiscoverVessel(v);
			}
		}
		if (!PartIsValid)
		{
			return;
		}
		int count = v.parts.Count;
		do
		{
			if (count-- <= 0)
			{
				return;
			}
		}
		while (v.parts[count].flightID != partID);
		VesselUtilities.DiscoverVessel(v);
	}
}
