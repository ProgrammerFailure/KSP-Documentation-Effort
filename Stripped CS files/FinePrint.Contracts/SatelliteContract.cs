using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class SatelliteContract : Contract, IUpdateWaypoints, ICheckSpecificVessels
{
	public enum SatellitePlacementMode
	{
		const_0,
		ADJUST,
		NETWORK,
		NETWORKADJUST
	}

	public CelestialBody targetBody;

	public double deviation = 10.0;

	public OrbitType orbitType = OrbitType.RANDOM;

	public KSPRandom generator;

	public double altitudeFactor = 0.5;

	public double inclinationFactor = 0.5;

	public SatellitePlacementMode placementMode;

	public static List<string> objectiveTypes;

	public override bool Generate()
	{
		if (!AreSatellitesUnlocked())
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		SatelliteContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<SatelliteContract>();
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
		if (num < ContractDefs.Satellite.MaximumAvailable && num2 < ContractDefs.Satellite.MaximumActive)
		{
			int seed = SystemUtilities.SuperSeed(this);
			generator = new KSPRandom(seed);
			float fundsMultiplier = 1f;
			float scienceMultiplier = 1f;
			float reputationMultiplier = 1f;
			int num4 = (int)prestige;
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, progressComplete: true, (CelestialBody cb) => cb != Planetarium.fetch.Sun);
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(num4, MannedStatus.UNMANNED, (CelestialBody cb) => cb != Planetarium.fetch.Sun));
			if (ContractDefs.Satellite.PreferHome && generator.Next(0, num4 + 1) == num4)
			{
				bodiesProgress.Add(Planetarium.fetch.Home);
			}
			if (ContractDefs.Satellite.AllowSolar && generator.Next(0, 3) < num4 && ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Sun.name, "Orbit"))
			{
				bodiesProgress.Add(Planetarium.fetch.Sun);
			}
			if (bodiesProgress.Count <= 0)
			{
				bodiesProgress.Add(Planetarium.fetch.Home);
			}
			switch (prestige)
			{
			case ContractPrestige.Trivial:
				deviation = ContractDefs.Satellite.TrivialDeviation;
				altitudeFactor = ContractDefs.Satellite.TrivialAltitudeDifficulty;
				inclinationFactor = ContractDefs.Satellite.TrivialInclinationDifficulty;
				break;
			case ContractPrestige.Significant:
				deviation = ContractDefs.Satellite.SignificantDeviation;
				altitudeFactor = ContractDefs.Satellite.SignificantAltitudeDifficulty;
				inclinationFactor = ContractDefs.Satellite.SignificantInclinationDifficulty;
				break;
			case ContractPrestige.Exceptional:
				deviation = ContractDefs.Satellite.ExceptionalDeviation;
				altitudeFactor = ContractDefs.Satellite.ExceptionalAltitudeDifficulty;
				inclinationFactor = ContractDefs.Satellite.ExceptionalInclinationDifficulty;
				break;
			}
			targetBody = WeightedBodyChoice(bodiesProgress, generator);
			if (targetBody == null)
			{
				targetBody = Planetarium.fetch.Home;
			}
			switch (prestige)
			{
			case ContractPrestige.Trivial:
				pickEasy();
				break;
			case ContractPrestige.Significant:
				pickMedium();
				break;
			case ContractPrestige.Exceptional:
				pickHard();
				break;
			}
			List<Vessel> list = VesselUtilities.SpecificVesselClassAt(Vessel.Situations.ORBITING, targetBody, VesselType.Probe, requireOwned: true, excludeActive: true, useVesselType: false);
			Orbit orbit = null;
			double stationaryLongitude = 0.0;
			Vessel v = null;
			Vessel adjustVessel = null;
			float num5 = ((targetBody == Planetarium.fetch.Home) ? ContractDefs.Satellite.ContextualHomeAssets : ContractDefs.Satellite.ContextualAssets);
			int num6 = Mathf.RoundToInt(ContractDefs.Satellite.ContextualChance * (Math.Min(list.Count, num5) / num5));
			if (generator.Next(0, 100) < num6)
			{
				SystemUtilities.ShuffleList(ref list, generator);
				int count = list.Count;
				while (count-- > 0 && placementMode == SatellitePlacementMode.const_0)
				{
					Vessel vessel = list[count];
					OrbitType orbitType = OrbitUtilities.IdentifyOrbit(vessel.loaded ? vessel.orbit : vessel.protoVessel.orbitSnapShot.Load());
					if ((uint)orbitType > 1u && (uint)(orbitType - 4) > 1u)
					{
						v = vessel;
						placementMode = SatellitePlacementMode.ADJUST;
						this.orbitType = OrbitType.RANDOM;
					}
					else
					{
						if (!CheckSatelliteNetwork(NetworkSatelliteCount(targetBody), list, orbitType, ref orbit, ref stationaryLongitude, ref adjustVessel, vessel))
						{
							continue;
						}
						if (adjustVessel == null)
						{
							if (PrestigeAppropriateOrbitType(orbitType))
							{
								v = vessel;
								placementMode = SatellitePlacementMode.NETWORK;
							}
						}
						else if (base.Prestige >= ContractPrestige.Significant)
						{
							v = adjustVessel;
							placementMode = SatellitePlacementMode.NETWORKADJUST;
						}
						this.orbitType = orbitType;
					}
				}
				if (placementMode == SatellitePlacementMode.ADJUST || placementMode == SatellitePlacementMode.NETWORKADJUST)
				{
					switch (base.Prestige)
					{
					case ContractPrestige.Exceptional:
						fundsMultiplier *= 0.4f;
						scienceMultiplier *= 0.4f;
						reputationMultiplier *= 0.4f;
						break;
					default:
						fundsMultiplier *= 0.2f;
						scienceMultiplier *= 0.2f;
						reputationMultiplier *= 0.2f;
						break;
					case ContractPrestige.Significant:
						fundsMultiplier *= 0.3f;
						scienceMultiplier *= 0.3f;
						reputationMultiplier *= 0.3f;
						break;
					}
					AddParameter(new SpecificVesselParameter(v));
				}
			}
			if (objectiveTypes == null)
			{
				objectiveTypes = new List<string> { "Antenna", "Generator" };
			}
			AddParameter(new VesselSystemsParameter(objectiveTypes, new List<string>
			{
				Localizer.Format("#autoLOC_7001075"),
				Localizer.Format("#autoLOC_7001077")
			}, Localizer.Format("#autoLOC_7001055"), MannedStatus.UNMANNED, placementMode == SatellitePlacementMode.const_0 || placementMode == SatellitePlacementMode.NETWORK));
			double eccentricityOverride = 0.0;
			if (this.orbitType == OrbitType.SYNCHRONOUS)
			{
				eccentricityOverride = generator.NextDouble() * (altitudeFactor / 2.0);
			}
			Orbit orbit2;
			switch (placementMode)
			{
			case SatellitePlacementMode.NETWORK:
			case SatellitePlacementMode.NETWORKADJUST:
				orbit2 = orbit;
				break;
			default:
				orbit2 = OrbitUtilities.GenerateOrbit(seed, targetBody, this.orbitType, altitudeFactor, inclinationFactor, eccentricityOverride);
				break;
			case SatellitePlacementMode.ADJUST:
				orbit2 = base.Prestige switch
				{
					ContractPrestige.Exceptional => VesselUtilities.GenerateAdjustedVesselOrbit(deviation, ContractDefs.Satellite.TrivialDeviation, 3, generator, v), 
					ContractPrestige.Significant => VesselUtilities.GenerateAdjustedVesselOrbit(deviation, ContractDefs.Satellite.SignificantDeviation, 2, generator, v), 
					_ => VesselUtilities.GenerateAdjustedVesselOrbit(deviation, ContractDefs.Satellite.ExceptionalDeviation, 1, generator, v), 
				};
				break;
			}
			if (orbit2 == null)
			{
				return false;
			}
			AddParameter(new SpecificOrbitParameter(this.orbitType, orbit2.inclination, orbit2.eccentricity, orbit2.semiMajorAxis, orbit2.double_0, orbit2.argumentOfPeriapsis, orbit2.meanAnomalyAtEpoch, orbit2.epoch, targetBody, deviation));
			if (this.orbitType == OrbitType.STATIONARY)
			{
				if (placementMode != SatellitePlacementMode.NETWORK && placementMode != SatellitePlacementMode.NETWORKADJUST)
				{
					WaypointManager.ChooseRandomPosition(out var _, out stationaryLongitude, targetBody.GetName(), waterAllowed: false, equatorial: true);
				}
				AddParameter(new StationaryPointParameter(targetBody, stationaryLongitude, deviation));
			}
			ConfigNode node = ContractDefs.config.GetNode("Satellite");
			if (node != null && placementMode != SatellitePlacementMode.ADJUST && placementMode != SatellitePlacementMode.NETWORKADJUST)
			{
				SystemUtilities.ProcessSideRequests(this, node, targetBody, Localizer.Format("#autoLOC_500053"), ref fundsMultiplier, ref scienceMultiplier, ref reputationMultiplier);
			}
			switch (this.orbitType)
			{
			case OrbitType.SYNCHRONOUS:
				fundsMultiplier *= ContractDefs.Satellite.Funds.SynchronousMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.SynchronousMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.SynchronousMultiplier;
				break;
			case OrbitType.STATIONARY:
				fundsMultiplier *= ContractDefs.Satellite.Funds.StationaryMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.StationaryMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.StationaryMultiplier;
				break;
			case OrbitType.POLAR:
				fundsMultiplier *= ContractDefs.Satellite.Funds.PolarMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.PolarMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.PolarMultiplier;
				break;
			case OrbitType.KOLNIYA:
				fundsMultiplier *= ContractDefs.Satellite.Funds.KolniyaMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.KolniyaMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.KolniyaMultiplier;
				break;
			case OrbitType.TUNDRA:
				fundsMultiplier *= ContractDefs.Satellite.Funds.TundraMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.TundraMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.TundraMultiplier;
				break;
			}
			if (targetBody == Planetarium.fetch.Home)
			{
				AddKeywords("Commercial");
				fundsMultiplier *= ContractDefs.Satellite.Funds.HomeMultiplier;
				scienceMultiplier *= ContractDefs.Satellite.Science.HomeMultiplier;
				reputationMultiplier *= ContractDefs.Satellite.Reputation.HomeMultiplier;
			}
			else
			{
				AddKeywords(SystemUtilities.CoinFlip(generator) ? "Commercial" : "Scientific");
			}
			AddParameter(new StabilityParameter(10f));
			float num7 = (float)Math.Max(FundsCompletion, ContractDefs.Satellite.Funds.BaseReward);
			float num8 = Math.Max(ScienceCompletion, ContractDefs.Satellite.Science.BaseReward);
			float num9 = Math.Max(ReputationCompletion, ContractDefs.Satellite.Reputation.BaseReward);
			SetExpiry(ContractDefs.Satellite.Expire.MinimumExpireDays, ContractDefs.Satellite.Expire.MaximumExpireDays);
			SetDeadlineDays(ContractDefs.Satellite.Expire.DeadlineDays, targetBody);
			SetFunds(Mathf.Round(ContractDefs.Satellite.Funds.BaseAdvance * fundsMultiplier), Mathf.Round(num7 * fundsMultiplier), Mathf.Round(ContractDefs.Satellite.Funds.BaseFailure * fundsMultiplier), targetBody);
			SetScience(Mathf.Round(num8 * scienceMultiplier));
			SetReputation(Mathf.Round(num9 * reputationMultiplier), Mathf.Round(ContractDefs.Satellite.Reputation.BaseFailure * reputationMultiplier));
			if (ContractSystem.Instance.GetCurrentContracts((SatelliteContract contract) => contract.targetBody == targetBody).Length >= 2)
			{
				return false;
			}
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
		if (placementMode == SatellitePlacementMode.ADJUST)
		{
			return Localizer.Format("#autoLOC_279915", StringUtilities.SpecificVesselName(this), targetBody.displayName);
		}
		switch (orbitType)
		{
		default:
			return Localizer.Format("#autoLOC_279970", targetBody.displayName);
		case OrbitType.SYNCHRONOUS:
		{
			string text2 = ContractDefs.OtherSynchronousName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text2 = ContractDefs.SunSynchronousName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text2 = ContractDefs.HomeSynchronousName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_279963", text2, targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_279966", StringUtilities.SpecificVesselName(this), text2, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_279968", text2, targetBody.displayName);
		}
		case OrbitType.STATIONARY:
		{
			string text = ContractDefs.OtherStationaryName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text = ContractDefs.SunStationaryName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text = ContractDefs.HomeStationaryName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_279948", text, targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_279951", StringUtilities.SpecificVesselName(this), text, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_279953", text, targetBody.displayName);
		}
		case OrbitType.POLAR:
			return Localizer.Format("#autoLOC_279922", targetBody.displayName);
		case OrbitType.EQUATORIAL:
			return Localizer.Format("#autoLOC_279920", targetBody.displayName);
		case OrbitType.KOLNIYA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_279925", StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_279928", StringUtilities.SpecificVesselName(this), StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_279930", StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
		case OrbitType.TUNDRA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_279933", targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_279936", StringUtilities.SpecificVesselName(this), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_279938", targetBody.displayName);
		}
	}

	public override string GetDescription()
	{
		string topic = "Satellite";
		string text = ((placementMode == SatellitePlacementMode.NETWORK || placementMode == SatellitePlacementMode.NETWORKADJUST) ? "Network" : "Orbit");
		switch (orbitType)
		{
		case OrbitType.SYNCHRONOUS:
			topic = "Synchronous " + text;
			break;
		case OrbitType.STATIONARY:
			topic = "Stationary " + text;
			break;
		case OrbitType.POLAR:
			topic = "Polar " + text;
			break;
		case OrbitType.EQUATORIAL:
			topic = "Equatorial " + text;
			break;
		case OrbitType.KOLNIYA:
			topic = StringUtilities.TitleCase(ContractDefs.MolniyaName) + " " + text;
			break;
		case OrbitType.TUNDRA:
			topic = "Tundra " + text;
			break;
		}
		if (placementMode == SatellitePlacementMode.ADJUST || placementMode == SatellitePlacementMode.NETWORKADJUST)
		{
			topic = StringUtilities.SpecificVesselName(this);
		}
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, topic, targetBody.displayName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: true, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (placementMode == SatellitePlacementMode.ADJUST)
		{
			return Localizer.Format("#autoLOC_280010", StringUtilities.SpecificVesselName(this), targetBody.displayName);
		}
		switch (orbitType)
		{
		default:
			return Localizer.Format("#autoLOC_280065", targetBody.displayName);
		case OrbitType.SYNCHRONOUS:
		{
			string text2 = ContractDefs.OtherSynchronousName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text2 = ContractDefs.SunSynchronousName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text2 = ContractDefs.HomeSynchronousName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280058", targetBody.displayName, text2);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280061", StringUtilities.SpecificVesselName(this), text2, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280063", text2, targetBody.displayName);
		}
		case OrbitType.STATIONARY:
		{
			string text = ContractDefs.OtherStationaryName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text = ContractDefs.SunStationaryName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text = ContractDefs.HomeStationaryName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280043", targetBody.displayName, text);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280046", StringUtilities.SpecificVesselName(this), text, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280048", text, targetBody.displayName);
		}
		case OrbitType.POLAR:
			return Localizer.Format("#autoLOC_280017", targetBody.displayName);
		case OrbitType.EQUATORIAL:
			return Localizer.Format("#autoLOC_280015", targetBody.displayName);
		case OrbitType.KOLNIYA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280020", targetBody.displayName, ContractDefs.MolniyaName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280023", StringUtilities.SpecificVesselName(this), ContractDefs.MolniyaName, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280025", StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
		case OrbitType.TUNDRA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280028", targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280031", StringUtilities.SpecificVesselName(this), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280033", targetBody.displayName);
		}
	}

	public override string MessageCompleted()
	{
		if (placementMode == SatellitePlacementMode.ADJUST)
		{
			return Localizer.Format("#autoLOC_280072", StringUtilities.SpecificVesselName(this), targetBody.displayName);
		}
		switch (orbitType)
		{
		default:
			return Localizer.Format("#autoLOC_280127", targetBody.displayName);
		case OrbitType.SYNCHRONOUS:
		{
			string text2 = ContractDefs.OtherSynchronousName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text2 = ContractDefs.SunSynchronousName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text2 = ContractDefs.HomeSynchronousName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280120", text2, targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280123", StringUtilities.SpecificVesselName(this), text2, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280125", text2, targetBody.displayName);
		}
		case OrbitType.STATIONARY:
		{
			string text = ContractDefs.OtherStationaryName.ToLower();
			if (targetBody == Planetarium.fetch.Sun)
			{
				text = ContractDefs.SunStationaryName.ToLower();
			}
			else if (targetBody == Planetarium.fetch.Home)
			{
				text = ContractDefs.HomeStationaryName.ToLower();
			}
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280105", text, targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280108", StringUtilities.SpecificVesselName(this), text, targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280110", text, targetBody.displayName);
		}
		case OrbitType.POLAR:
			return Localizer.Format("#autoLOC_280079", targetBody.displayName);
		case OrbitType.EQUATORIAL:
			return Localizer.Format("#autoLOC_280077", targetBody.displayName);
		case OrbitType.KOLNIYA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280082", StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280085", StringUtilities.SpecificVesselName(this), StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280087", StringUtilities.TitleCase(ContractDefs.MolniyaName), targetBody.displayName);
		case OrbitType.TUNDRA:
			if (placementMode == SatellitePlacementMode.NETWORK)
			{
				return Localizer.Format("#autoLOC_280090", targetBody.displayName);
			}
			if (placementMode == SatellitePlacementMode.NETWORKADJUST)
			{
				return Localizer.Format("#autoLOC_280093", StringUtilities.SpecificVesselName(this), targetBody.displayName);
			}
			return Localizer.Format("#autoLOC_280095", targetBody.displayName);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("deviation", deviation);
		node.AddValue("orbitType", (int)orbitType);
		node.AddValue("placementMode", (int)placementMode);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SatelliteContract", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SatelliteContract", "deviation", ref deviation, 10.0);
		SystemUtilities.LoadNode(node, "SatelliteContract", "orbitType", ref orbitType, OrbitType.RANDOM);
		SystemUtilities.LoadNode(node, "SatelliteContract", "placementMode", ref placementMode, SatellitePlacementMode.const_0);
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Orbit"))
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

	public override void OnAccepted()
	{
		VesselSystemsParameter parameter = GetParameter<VesselSystemsParameter>();
		if (parameter != null && parameter.requireNew)
		{
			parameter.launchID = HighLogic.CurrentGame.launchID;
		}
	}

	public override void OnFinished()
	{
		SpecificOrbitParameter parameter = GetParameter<SpecificOrbitParameter>();
		SpecificVesselParameter parameter2 = GetParameter<SpecificVesselParameter>();
		parameter?.CleanupRenderer();
		parameter2?.CleanupWaypoints();
	}

	public static bool AreSatellitesUnlocked()
	{
		if (ProbeCoresUnlocked() && ResearchAndDevelopment.ResearchedValidContractObjectives("Antenna", "Generator"))
		{
			return true;
		}
		return false;
	}

	public static bool ProbeCoresUnlocked()
	{
		int count = PartLoader.LoadedPartsList.Count;
		AvailablePart availablePart;
		do
		{
			if (count-- > 0)
			{
				availablePart = PartLoader.LoadedPartsList[count];
				continue;
			}
			return false;
		}
		while (availablePart.partPrefab == null || availablePart.partPrefab.Modules == null || !ResearchAndDevelopment.PartTechAvailable(availablePart) || availablePart.partPrefab.isControlSource <= Vessel.ControlLevel.NONE || availablePart.partPrefab.CrewCapacity != 0);
		return true;
	}

	public void pickEasy()
	{
		if (generator != null)
		{
			int num = generator.Next(0, 101);
			if (num <= 33)
			{
				setOrbitType(OrbitType.RANDOM, altitudeFactor);
			}
			else if (num > 33 && num <= 66)
			{
				setOrbitType(OrbitType.POLAR, altitudeFactor);
			}
			else
			{
				setOrbitType(OrbitType.EQUATORIAL, altitudeFactor);
			}
		}
	}

	public void pickMedium()
	{
		if (generator != null)
		{
			int num = generator.Next(0, 101);
			if (num <= 33)
			{
				pickEasy();
			}
			else if (num > 33 && num <= 66)
			{
				setOrbitType(OrbitType.RANDOM, altitudeFactor);
			}
			else
			{
				setOrbitType(OrbitType.SYNCHRONOUS, altitudeFactor);
			}
		}
	}

	public void pickHard()
	{
		if (generator != null)
		{
			int num = generator.Next(0, 101);
			if (num <= 20)
			{
				pickMedium();
			}
			else if (num > 20 && num <= 40)
			{
				setOrbitType(OrbitType.RANDOM, altitudeFactor);
			}
			else if (num > 40 && num <= 60)
			{
				setOrbitType(OrbitType.KOLNIYA, altitudeFactor);
			}
			else if (num > 60 && num <= 80)
			{
				setOrbitType(OrbitType.TUNDRA, altitudeFactor);
			}
			else
			{
				setOrbitType(OrbitType.STATIONARY, altitudeFactor);
			}
		}
	}

	public void setOrbitType(OrbitType targetType, double altitudeFactor)
	{
		if ((object)targetBody == null)
		{
			return;
		}
		switch (targetType)
		{
		case OrbitType.SYNCHRONOUS:
			if (!ContractDefs.Satellite.AllowSynchronous)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		case OrbitType.STATIONARY:
			if (!ContractDefs.Satellite.AllowStationary)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		case OrbitType.POLAR:
			if (!ContractDefs.Satellite.AllowPolar)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		case OrbitType.EQUATORIAL:
			if (!ContractDefs.Satellite.AllowEquatorial)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		case OrbitType.KOLNIYA:
			if (!ContractDefs.Satellite.AllowKolniya)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		case OrbitType.TUNDRA:
			if (!ContractDefs.Satellite.AllowTundra)
			{
				orbitType = OrbitType.RANDOM;
				return;
			}
			break;
		}
		switch (targetType)
		{
		case OrbitType.SYNCHRONOUS:
			if (CelestialUtilities.CanBodyBeSynchronous(targetBody, altitudeFactor / 2.0))
			{
				orbitType = targetType;
			}
			else
			{
				orbitType = OrbitType.RANDOM;
			}
			break;
		case OrbitType.STATIONARY:
			if (CelestialUtilities.CanBodyBeSynchronous(targetBody, altitudeFactor / 2.0))
			{
				orbitType = targetType;
			}
			else
			{
				orbitType = OrbitType.RANDOM;
			}
			break;
		case OrbitType.KOLNIYA:
			if (CelestialUtilities.CanBodyBeKolniya(targetBody))
			{
				orbitType = targetType;
			}
			else
			{
				orbitType = OrbitType.RANDOM;
			}
			break;
		case OrbitType.TUNDRA:
			if (CelestialUtilities.CanBodyBeTundra(targetBody))
			{
				orbitType = targetType;
			}
			else
			{
				orbitType = OrbitType.RANDOM;
			}
			break;
		default:
			orbitType = targetType;
			break;
		}
	}

	public bool PrestigeAppropriateOrbitType(OrbitType ot)
	{
		switch (ot)
		{
		default:
			return true;
		case OrbitType.KOLNIYA:
		case OrbitType.TUNDRA:
			return base.Prestige >= ContractPrestige.Exceptional;
		case OrbitType.SYNCHRONOUS:
		case OrbitType.STATIONARY:
			return base.Prestige >= ContractPrestige.Significant;
		}
	}

	public int NetworkSatelliteCount(CelestialBody body)
	{
		int seed = (391 + ScenarioContractEvents.GameSeed) * 23 + body.flightGlobalsIndex;
		int minValue = Math.Min(ContractDefs.Satellite.NetworkMinimum, ContractDefs.Satellite.NetworkMaximum);
		int maxValue = Math.Max(ContractDefs.Satellite.NetworkMinimum, ContractDefs.Satellite.NetworkMaximum) + 1;
		return new KSPRandom(seed).Next(minValue, maxValue);
	}

	public bool CheckSatelliteNetwork(int networkCount, List<Vessel> possibleSatellites, OrbitType orbitType, ref Orbit orbit, ref double stationaryLongitude, ref Vessel adjustVessel, Vessel startVessel = null)
	{
		if (VesselUtilities.ActiveVesselFallback(ref startVessel) && networkCount > 1)
		{
			Orbit orbit2 = (startVessel.loaded ? startVessel.orbit : startVessel.protoVessel.orbitSnapShot.Load());
			orbit = new Orbit(orbit2.inclination, orbit2.eccentricity, orbit2.semiMajorAxis, orbit2.double_0, orbit2.argumentOfPeriapsis, orbit2.meanAnomalyAtEpoch, orbit2.epoch, orbit2.referenceBody);
			stationaryLongitude = orbit.referenceBody.GetLongitude(orbit.getPositionAtUT(Planetarium.GetUniversalTime()));
			double num = 360.0 / (double)networkCount;
			networkCount--;
			int num2 = networkCount;
			List<Vessel> list;
			List<Vessel> list2;
			do
			{
				if (num2-- > 0)
				{
					list = new List<Vessel>();
					list2 = new List<Vessel>();
					if (orbitType == OrbitType.STATIONARY)
					{
						stationaryLongitude = (stationaryLongitude + num) % 360.0;
					}
					else
					{
						orbit.double_0 = (orbit.double_0 + num) % 360.0;
					}
					int count = possibleSatellites.Count;
					while (count-- > 0)
					{
						Vessel vessel = possibleSatellites[count];
						if (vessel == startVessel)
						{
							continue;
						}
						if (orbitType == OrbitType.STATIONARY)
						{
							if (VesselUtilities.VesselAtOrbit(orbit, ContractDefs.Satellite.SignificantDeviation, vessel))
							{
								Orbit orbit3 = (vessel.loaded ? vessel.orbit : vessel.protoVessel.orbitSnapShot.Load());
								double longitude = orbit.referenceBody.GetLongitude(orbit3.getPositionAtUT(Planetarium.GetUniversalTime()));
								double num3 = Math.Abs(stationaryLongitude - longitude) % 360.0;
								if (num3 > 180.0)
								{
									num3 = 360.0 - num3;
								}
								if (num3 <= ContractDefs.Satellite.SignificantDeviation / 100.0 * 360.0)
								{
									list2.Add(vessel);
								}
								else if (num3 <= ContractDefs.Satellite.TrivialDeviation / 100.0 * 360.0)
								{
									list.Add(vessel);
								}
							}
							else if (VesselUtilities.VesselAtOrbit(orbit, ContractDefs.Satellite.TrivialDeviation, vessel))
							{
								list.Add(vessel);
							}
						}
						else if (VesselUtilities.VesselAtOrbit(orbit, ContractDefs.Satellite.SignificantDeviation, vessel))
						{
							list2.Add(vessel);
						}
						else if (VesselUtilities.VesselAtOrbit(orbit, ContractDefs.Satellite.TrivialDeviation, vessel))
						{
							list.Add(vessel);
						}
					}
					continue;
				}
				return false;
			}
			while (list2.Count > 0);
			if (list.Count > 0)
			{
				adjustVessel = list[0];
			}
			return true;
		}
		return false;
	}
}
