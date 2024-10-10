using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class SurveyContract : Contract
{
	public CelestialBody targetBody;

	public double centerLatitude;

	public double centerLongitude;

	public string dataName = "generic";

	public string anomalyName = "anomalies";

	public string resultName = "results";

	public string locationName = "location";

	public bool focusedSurvey;

	public FlightBand targetBand;

	public bool contextual;

	public string vesselName = "vessel";

	public string cardinalName = "cardinal";

	public override bool Generate()
	{
		int progressLevel = ProgressUtilities.GetProgressLevel();
		int num = 0;
		int num2 = 0;
		int maximumAvailable = ContractDefs.Survey.MaximumAvailable;
		int maximumActive = ContractDefs.Survey.MaximumActive;
		SurveyContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<SurveyContract>();
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
		if (num < maximumAvailable && num2 < maximumActive)
		{
			KSPRandom kSPRandom = new KSPRandom(SystemUtilities.SuperSeed(this));
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Sun);
			List<CelestialBody> nextUnreached = ProgressUtilities.GetNextUnreached(2, (CelestialBody cb) => cb != Planetarium.fetch.Sun);
			if (base.Prestige == ContractPrestige.Exceptional)
			{
				bodiesProgress.AddRange(nextUnreached);
			}
			if (bodiesProgress.Count == 0)
			{
				return false;
			}
			targetBody = WeightedBodyChoice(bodiesProgress, kSPRandom);
			int num4 = Mathf.Max((int)Mathf.Round(2f - CelestialUtilities.PlanetScienceRanking(targetBody) * 2f), 0);
			int num5 = 0;
			float num6 = 1f;
			float num7 = 1f;
			float num8 = 1f;
			switch (base.Prestige)
			{
			case ContractPrestige.Trivial:
				num5 = ContractDefs.Survey.TrivialWaypoints;
				num5 += num4;
				break;
			case ContractPrestige.Significant:
				num5 = ContractDefs.Survey.SignificantWaypoints;
				num5 += num4;
				break;
			case ContractPrestige.Exceptional:
				num5 = ContractDefs.Survey.ExceptionalWaypoints;
				num5 += num4;
				break;
			}
			focusedSurvey = false;
			if (progressLevel < 4)
			{
				int num9 = (int)(base.Prestige + 1);
				if (progressLevel == num9)
				{
					focusedSurvey = true;
				}
			}
			List<SurveyDefinition> list = PossibleDefinitions();
			if (list.Count == 0)
			{
				return false;
			}
			contextual = false;
			Vessel vessel = null;
			FlightBand flightBand = FlightBand.GROUND;
			if (!focusedSurvey)
			{
				List<KeyValuePair<Vessel, List<string>>> list2 = CachePossibleVessels(targetBody, list);
				int num10 = Mathf.RoundToInt(ContractDefs.Survey.ContextualChance * (Math.Min(list2.Count, ContractDefs.Survey.ContextualAssets) / ContractDefs.Survey.ContextualAssets));
				if (kSPRandom.Next(0, 100) < num10)
				{
					contextual = true;
					KeyValuePair<Vessel, List<string>> keyValuePair = list2[kSPRandom.Next(0, list2.Count)];
					vessel = keyValuePair.Key;
					vesselName = (vessel.loaded ? vessel.GetDisplayName() : vessel.protoVessel.GetDisplayName());
					flightBand = base.Prestige switch
					{
						ContractPrestige.Exceptional => FlightBand.const_2, 
						ContractPrestige.Significant => SystemUtilities.CoinFlip(kSPRandom) ? FlightBand.GROUND : FlightBand.const_2, 
						_ => FlightBand.GROUND, 
					};
					if ((double)num5 * ParamRange(base.Prestige, targetBody, FlightBand.const_2) > Math.PI * targetBody.Radius)
					{
						flightBand = FlightBand.GROUND;
					}
					int count = list.Count;
					while (count-- > 0)
					{
						SurveyDefinition surveyDefinition = list[count];
						int count2 = surveyDefinition.Parameters.Count;
						while (count2-- > 0)
						{
							SurveyDefinitionParameter surveyDefinitionParameter = surveyDefinition.Parameters[count2];
							if (!keyValuePair.Value.Contains(surveyDefinitionParameter.Experiment))
							{
								surveyDefinition.Parameters.Remove(surveyDefinitionParameter);
							}
						}
						if (surveyDefinition.Parameters.Count <= 0)
						{
							list.Remove(surveyDefinition);
						}
					}
					if (list.Count <= 0)
					{
						return false;
					}
				}
			}
			SurveyDefinition surveyDefinition2 = list[kSPRandom.Next(0, list.Count)];
			List<SurveyDefinitionParameter> list3 = surveyDefinition2.Parameters;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int count3 = list3.Count;
			while (count3-- > 0)
			{
				SurveyDefinitionParameter surveyDefinitionParameter2 = list3[count3];
				flag = flag || surveyDefinitionParameter2.AllowGround;
				flag3 = flag3 || surveyDefinitionParameter2.AllowLow;
				flag2 = flag2 || surveyDefinitionParameter2.AllowHigh;
			}
			if (focusedSurvey)
			{
				flag = false;
			}
			if (contextual)
			{
				flag = flightBand == FlightBand.GROUND;
				flag3 = flightBand == FlightBand.const_2;
				flag2 = false;
			}
			if (CelestialUtilities.IsGasGiant(targetBody))
			{
				flag = false;
				flag3 = false;
			}
			List<FlightBand> list4 = new List<FlightBand>();
			if (flag)
			{
				list4.Add(FlightBand.GROUND);
			}
			if (flag3)
			{
				list4.Add(FlightBand.const_2);
			}
			if (flag2)
			{
				list4.Add(FlightBand.HIGH);
			}
			if (base.Root.Prestige >= ContractPrestige.Significant)
			{
				if (flag3 && flag2)
				{
					list4.Add(FlightBand.HIGHMIX);
				}
				if (base.Root.Prestige == ContractPrestige.Exceptional)
				{
					if (flag && flag3)
					{
						list4.Add(FlightBand.LOWMIX);
					}
					if (flag && flag3 && flag2)
					{
						list4.Add(FlightBand.ANYMIX);
					}
				}
			}
			if (list4.Count <= 0)
			{
				return false;
			}
			targetBand = list4[kSPRandom.Next(0, list4.Count)];
			int count4 = list3.Count;
			while (count4-- > 0)
			{
				SurveyDefinitionParameter surveyDefinitionParameter3 = list3[count4];
				switch (targetBand)
				{
				default:
					if (!surveyDefinitionParameter3.AllowGround && !surveyDefinitionParameter3.AllowLow && !surveyDefinitionParameter3.AllowHigh)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				case FlightBand.GROUND:
					if (!surveyDefinitionParameter3.AllowGround)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				case FlightBand.const_2:
					if (!surveyDefinitionParameter3.AllowLow)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				case FlightBand.HIGH:
					if (!surveyDefinitionParameter3.AllowHigh)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				case FlightBand.LOWMIX:
					if (!surveyDefinitionParameter3.AllowGround && !surveyDefinitionParameter3.AllowLow)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				case FlightBand.HIGHMIX:
					if (!surveyDefinitionParameter3.AllowLow && !surveyDefinitionParameter3.AllowHigh)
					{
						list3.Remove(surveyDefinitionParameter3);
					}
					break;
				}
			}
			if (list3.Count <= 0)
			{
				return false;
			}
			int totalParameters = int.MaxValue;
			if (contextual)
			{
				totalParameters = num5;
				num5 = 1;
				switch (base.Prestige)
				{
				case ContractPrestige.Exceptional:
					num6 *= 0.4f;
					num7 *= 0.4f;
					num8 *= 0.4f;
					break;
				default:
					num6 *= 0.2f;
					num7 *= 0.2f;
					num8 *= 0.2f;
					break;
				case ContractPrestige.Significant:
					num6 *= 0.3f;
					num7 *= 0.3f;
					num8 *= 0.3f;
					break;
				}
			}
			if (focusedSurvey)
			{
				float num11 = ContractDefs.Survey.TrivialWaypoints + ContractDefs.Survey.SignificantWaypoints + ContractDefs.Survey.ExceptionalWaypoints + num4 * 3;
				num11 /= 3f;
				float num12 = 1f / num11;
				num5 = 1;
				num6 *= num12;
				num7 *= num12;
				num8 *= num12;
			}
			bool flag4 = false;
			if (targetBody.ocean && (targetBand == FlightBand.GROUND || targetBand == FlightBand.LOWMIX || targetBand == FlightBand.ANYMIX))
			{
				int count5 = list3.Count;
				while (count5-- > 0 && !(flag4 = !list3[count5].AllowWater))
				{
				}
			}
			if (contextual && vessel != null)
			{
				centerLatitude = (vessel.loaded ? vessel.latitude : vessel.protoVessel.latitude);
				centerLongitude = (vessel.loaded ? vessel.longitude : vessel.protoVessel.longitude);
			}
			else
			{
				float num13 = 0f;
				if (targetBody == Planetarium.fetch.Home)
				{
					float num14 = (float)(progressLevel + prestige);
					float homeNearbyProgressCap = ContractDefs.Survey.HomeNearbyProgressCap;
					if (num14 < 0f)
					{
						num14 = 0f;
					}
					else if (num14 > homeNearbyProgressCap)
					{
						num14 = homeNearbyProgressCap;
					}
					num13 = 1f - num14 / homeNearbyProgressCap;
				}
				if (targetBody == Planetarium.fetch.Home && (focusedSurvey || kSPRandom.NextDouble() < (double)num13))
				{
					RandomizeNearKSC(prestige, !flag4, kSPRandom);
				}
				else
				{
					WaypointManager.ChooseRandomPosition(out centerLatitude, out centerLongitude, targetBody.GetName(), !flag4, equatorial: false, kSPRandom);
				}
			}
			List<KeyValuePair<SurveyDefinitionParameter, FlightBand>> list5 = new List<KeyValuePair<SurveyDefinitionParameter, FlightBand>>();
			int num15 = 0;
			int num16 = 0;
			for (int i = 0; i < num5; i++)
			{
				SurveyDefinitionParameter surveyDefinitionParameter4 = list3[kSPRandom.Next(0, list3.Count)];
				List<FlightBand> list6 = new List<FlightBand>();
				bool flag5 = surveyDefinitionParameter4.AllowGround;
				bool flag6 = surveyDefinitionParameter4.AllowLow;
				bool flag7 = surveyDefinitionParameter4.AllowHigh;
				bool crewRequired = surveyDefinitionParameter4.CrewRequired;
				bool eVARequired = surveyDefinitionParameter4.EVARequired;
				if (focusedSurvey)
				{
					flag5 = false;
				}
				if (contextual)
				{
					flag5 = flightBand == FlightBand.GROUND;
					flag6 = flightBand == FlightBand.const_2;
					flag7 = false;
				}
				if (eVARequired && targetBody == Planetarium.fetch.Home && !GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
				{
					flag6 = false;
					flag7 = false;
				}
				switch (targetBand)
				{
				case FlightBand.GROUND:
					flag6 = false;
					flag7 = false;
					break;
				case FlightBand.const_2:
					flag5 = false;
					flag7 = false;
					break;
				case FlightBand.HIGH:
					flag5 = false;
					flag6 = false;
					break;
				case FlightBand.LOWMIX:
					flag7 = false;
					break;
				case FlightBand.HIGHMIX:
					flag5 = false;
					break;
				}
				if (CelestialUtilities.IsGasGiant(targetBody))
				{
					if (flag7)
					{
						list6.Add(FlightBand.HIGH);
					}
				}
				else
				{
					if (flag7)
					{
						list6.Add(FlightBand.HIGH);
					}
					if (flag5)
					{
						if (base.Prestige != ContractPrestige.Exceptional)
						{
							if (ProgressUtilities.VisitedSurfaceOf(targetBody, (!crewRequired) ? MannedStatus.const_2 : MannedStatus.MANNED))
							{
								list6.Add(FlightBand.GROUND);
							}
						}
						else
						{
							list6.Add(FlightBand.GROUND);
						}
					}
					if (flag6)
					{
						list6.Add(FlightBand.const_2);
					}
				}
				if (list6.Count != 0)
				{
					FlightBand flightBand2 = list6[kSPRandom.Next(list6.Count)];
					num15++;
					if (flightBand2 == FlightBand.GROUND)
					{
						num16++;
					}
					list5.Add(new KeyValuePair<SurveyDefinitionParameter, FlightBand>(surveyDefinitionParameter4, flightBand2));
				}
			}
			if (list5.Count == 0)
			{
				return false;
			}
			int num17 = 0;
			int count6 = list5.Count;
			for (int j = 0; j < count6; j++)
			{
				KeyValuePair<SurveyDefinitionParameter, FlightBand> pair = list5[j];
				if (pair.Value == FlightBand.GROUND)
				{
					SetupParam(num17++, pair, totalParameters, contextual ? vessel : null);
				}
			}
			for (int k = 0; k < count6; k++)
			{
				KeyValuePair<SurveyDefinitionParameter, FlightBand> pair = list5[k];
				if (pair.Value != FlightBand.GROUND)
				{
					SetupParam(num17++, pair, totalParameters, contextual ? vessel : null);
				}
			}
			dataName = surveyDefinition2.DataName;
			anomalyName = surveyDefinition2.AnomalyName;
			resultName = surveyDefinition2.ResultName;
			WaypointClusterState waypointClusterState = WaypointClusterState.MIXED;
			if (!contextual)
			{
				if (num15 == num16)
				{
					waypointClusterState = WaypointClusterState.FULL;
				}
				if (num16 == 1)
				{
					waypointClusterState = WaypointClusterState.SINGLE;
				}
			}
			if (waypointClusterState == WaypointClusterState.FULL && !contextual)
			{
				locationName = Localizer.Format("#autoLOC_281212", targetBody.displayName, StringUtilities.GenerateSiteName(SystemUtilities.SuperSeed(this), targetBody, landLocked: true));
			}
			else if (contextual)
			{
				locationName = Localizer.Format("#autoLOC_281214", targetBody.displayName, vesselName);
			}
			else
			{
				locationName = targetBody.displayName;
			}
			IEnumerator<ContractParameter> enumerator = base.AllParameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is SurveyWaypointParameter surveyWaypointParameter)
				{
					surveyWaypointParameter.clusterState = waypointClusterState;
				}
			}
			AddKeywords(SystemUtilities.CoinFlip(kSPRandom) ? "Scientific" : "Commercial");
			if (nextUnreached.Contains(targetBody))
			{
				AddKeywords("Pioneer");
			}
			SetExpiry(ContractDefs.Survey.Expire.MinimumExpireDays, ContractDefs.Survey.Expire.MaximumExpireDays);
			SetDeadlineDays(ContractDefs.Survey.Expire.DeadlineDays, targetBody);
			SetFunds(Mathf.Round(surveyDefinition2.FundsAdvance * num6), Mathf.Round(surveyDefinition2.FundsReward * num6), Mathf.Round(surveyDefinition2.FundsFailure * num6), targetBody);
			SetScience(Mathf.Round(surveyDefinition2.ScienceReward * num7));
			SetReputation(Mathf.Round(surveyDefinition2.ReputationReward * num8), Mathf.Round(surveyDefinition2.ReputationFailure * num8));
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
		if (focusedSurvey)
		{
			return Localizer.Format("#autoLOC_281262", dataName, locationName);
		}
		return Localizer.Format("#autoLOC_281264", dataName, locationName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, targetBody.displayName, targetBody.displayName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (focusedSurvey)
		{
			return Localizer.Format("#autoLOC_281275", targetBody.displayName, dataName, resultName);
		}
		if (contextual)
		{
			return Localizer.Format("#autoLOC_281278", vesselName, targetBody.displayName, dataName, anomalyName, cardinalName, resultName);
		}
		return Localizer.Format("#autoLOC_281280", dataName, anomalyName, locationName, resultName);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_281285", dataName, resultName);
	}

	public override string GetNotes()
	{
		string text = "";
		if (CelestialUtilities.IsGasGiant(targetBody))
		{
			text += Localizer.Format("#autoLOC_281293");
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
			{
				text += "\n";
			}
			return text;
		}
		return null;
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("centerLatitude", centerLatitude);
		node.AddValue("centerLongitude", centerLongitude);
		node.AddValue("dataName", dataName);
		node.AddValue("anomalyName", anomalyName);
		node.AddValue("reportName", resultName);
		node.AddValue("locationName", locationName);
		node.AddValue("focusedSurvey", focusedSurvey);
		node.AddValue("targetBand", targetBand);
		node.AddValue("contextual", contextual);
		if (contextual)
		{
			node.AddValue("vesselName", vesselName);
			node.AddValue("cardinalName", cardinalName);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SurveyContract", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SurveyContract", "centerLatitude", ref centerLatitude, 0.0);
		SystemUtilities.LoadNode(node, "SurveyContract", "centerLongitude", ref centerLongitude, 0.0);
		SystemUtilities.LoadNode(node, "SurveyContract", "dataName", ref dataName, "generic");
		SystemUtilities.LoadNode(node, "SurveyContract", "anomalyName", ref anomalyName, "anomalies");
		SystemUtilities.LoadNode(node, "SurveyContract", "reportName", ref resultName, "results");
		SystemUtilities.LoadNode(node, "SurveyContract", "locationName", ref locationName, "location");
		SystemUtilities.LoadNode(node, "SurveyContract", "focusedSurvey", ref focusedSurvey, defaultValue: false);
		SystemUtilities.LoadNode(node, "SurveyContract", "targetBand", ref targetBand, FlightBand.NONE);
		SystemUtilities.LoadNode(node, "SurveyContract", "contextual", ref contextual, defaultValue: false);
		if (contextual)
		{
			SystemUtilities.LoadNode(node, "SurveyContract", "vesselName", ref vesselName, "vessel");
			SystemUtilities.LoadNode(node, "SurveyContract", "cardinalName", ref cardinalName, "cardinal");
		}
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete("FirstLaunch"))
		{
			return false;
		}
		int progressLevel = ProgressUtilities.GetProgressLevel();
		int num = (int)(base.Prestige + 1);
		if (progressLevel < num)
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { targetBody };
	}

	public void SetupParam(int paramIndex, KeyValuePair<SurveyDefinitionParameter, FlightBand> pair, int totalParameters, Vessel contextualVessel = null)
	{
		SurveyDefinitionParameter key = pair.Key;
		FlightBand value = pair.Value;
		UnityEngine.Random.InitState(base.MissionSeed);
		double num = ParamRange(base.Prestige, targetBody, value);
		bool waterAllowed = true;
		if (targetBody.ocean && value == FlightBand.GROUND)
		{
			waterAllowed = key.AllowWater;
		}
		float num2 = (contextual ? 0.5f : 1f);
		float f = key.FundsMultiplier * ContractDefs.Survey.Funds.WaypointDefaultReward * num2;
		float f2 = key.ScienceMultiplier * ContractDefs.Survey.Science.WaypointDefaultReward * num2;
		float f3 = key.ReputationMultiplier * ContractDefs.Survey.Reputation.WaypointDefaultReward * num2;
		Waypoint waypoint = new Waypoint
		{
			seed = SystemUtilities.SuperSeed(this),
			id = key.Texture,
			index = paramIndex,
			contractReference = this,
			celestialName = targetBody.GetName()
		};
		SurveyWaypointParameter surveyWaypointParameter;
		if (contextual)
		{
			waypoint.latitude = centerLatitude;
			waypoint.longitude = centerLongitude;
			double num3 = Math.Max(ParamRange(base.Prestige, targetBody, value), ContractDefs.Survey.MinimumTriggerRange);
			waypoint.RandomizeAwayFrom(centerLatitude, centerLongitude, num3, num3 * 2.0, 3, waterAllowed);
			cardinalName = StringUtilities.CardinalDirectionBetween(centerLatitude, centerLongitude, waypoint.latitude, waypoint.longitude);
			surveyWaypointParameter = new SurveyWaypointParameter(key.Experiment, key.Description, targetBody, waypoint, value, contextual, totalParameters, centerLatitude, centerLongitude);
		}
		else
		{
			waypoint.RandomizeNear(centerLatitude, centerLongitude, focusedSurvey ? 1.0 : num, waterAllowed);
			surveyWaypointParameter = new SurveyWaypointParameter(key.Experiment, key.Description, targetBody, waypoint, value);
		}
		AddParameter(surveyWaypointParameter);
		surveyWaypointParameter.ProcessWaypoint();
		if (!focusedSurvey)
		{
			surveyWaypointParameter.SetFunds(Mathf.Round(f), targetBody);
			surveyWaypointParameter.SetReputation(Mathf.Round(f3));
			surveyWaypointParameter.SetScience(Mathf.Round(f2));
		}
	}

	public static List<KeyValuePair<Vessel, List<string>>> CachePossibleVessels(CelestialBody body, List<SurveyDefinition> originalDefinitions)
	{
		List<KeyValuePair<Vessel, List<string>>> list = new List<KeyValuePair<Vessel, List<string>>>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		int count = originalDefinitions.Count;
		while (count-- > 0)
		{
			SurveyDefinition surveyDefinition = originalDefinitions[count];
			int count2 = surveyDefinition.Parameters.Count;
			while (count2-- > 0)
			{
				SurveyDefinitionParameter surveyDefinitionParameter = surveyDefinition.Parameters[count2];
				if (surveyDefinitionParameter.Experiment != string.Empty && !list2.Contains(surveyDefinitionParameter.Experiment))
				{
					list2.Add(surveyDefinitionParameter.Experiment);
					if (surveyDefinitionParameter.EVARequired && !list3.Contains(surveyDefinitionParameter.Experiment))
					{
						list3.Add(surveyDefinitionParameter.Experiment);
					}
				}
			}
		}
		if (list2.Count <= 0)
		{
			return list;
		}
		int count3 = FlightGlobals.Vessels.Count;
		while (count3-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count3];
			if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && vessel == FlightGlobals.ActiveVessel)
			{
				continue;
			}
			if (vessel.loaded)
			{
				if (vessel.vesselType <= VesselType.SpaceObject || vessel.vesselType == VesselType.DeployedScienceController || vessel.vesselType == VesselType.DeployedSciencePart || !vessel.Landed || !vessel.IsControllable || vessel.orbit.referenceBody != body)
				{
					continue;
				}
			}
			else if (vessel.protoVessel.vesselType <= VesselType.SpaceObject || vessel.vesselType == VesselType.DeployedScienceController || vessel.vesselType == VesselType.DeployedSciencePart || (vessel.protoVessel.situation != Vessel.Situations.LANDED && vessel.protoVessel.situation != Vessel.Situations.PRELAUNCH) || !vessel.protoVessel.wasControllable || FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex] != body)
			{
				continue;
			}
			if (!VesselUtilities.VesselIsOwned(vessel))
			{
				continue;
			}
			List<string> list4 = new List<string>();
			if (vessel.GetVesselCrew().Count > 0)
			{
				list4.AddRange(list3);
			}
			if (vessel.loaded)
			{
				int count4 = vessel.Parts.Count;
				while (count4-- > 0)
				{
					Part part = vessel.Parts[count4];
					int count5 = part.Modules.Count;
					while (count5-- > 0)
					{
						ModuleScienceExperiment moduleScienceExperiment = part.Modules[count5] as ModuleScienceExperiment;
						if (!(moduleScienceExperiment == null) && moduleScienceExperiment.experiment != null)
						{
							list4.Add(moduleScienceExperiment.experiment.id);
						}
					}
				}
			}
			else
			{
				int count6 = vessel.protoVessel.protoPartSnapshots.Count;
				while (count6-- > 0)
				{
					Part partPrefab = PartLoader.getPartInfoByName(vessel.protoVessel.protoPartSnapshots[count6].partName.Replace('_', '.')).partPrefab;
					int count7 = partPrefab.Modules.Count;
					while (count7-- > 0)
					{
						ModuleScienceExperiment moduleScienceExperiment2 = partPrefab.Modules[count7] as ModuleScienceExperiment;
						if (!(moduleScienceExperiment2 == null) && !moduleScienceExperiment2.experimentID.Contains("robotScannerArmROCScan"))
						{
							list4.Add(moduleScienceExperiment2.experimentID);
						}
					}
				}
			}
			if (list4.Count > 0)
			{
				list.Add(new KeyValuePair<Vessel, List<string>>(vessel, list4));
			}
		}
		int count8 = list.Count;
		while (count8-- > 0)
		{
			List<string> value = list[count8].Value;
			bool flag = false;
			int count9 = value.Count;
			while (count9-- > 0)
			{
				if (list2.Contains(value[count9]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Remove(list[count8]);
			}
		}
		return list;
	}

	public List<SurveyDefinition> PossibleDefinitions()
	{
		List<SurveyDefinition> surveyDefinitions = ContractDefs.SurveyDefinitions;
		int count = surveyDefinitions.Count;
		while (count-- > 0)
		{
			SurveyDefinition surveyDefinition = surveyDefinitions[count];
			int count2 = surveyDefinition.Parameters.Count;
			while (count2-- > 0)
			{
				SurveyDefinitionParameter surveyDefinitionParameter = surveyDefinition.Parameters[count2];
				bool flag = true;
				ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(surveyDefinitionParameter.Experiment);
				if (experiment != null)
				{
					if (!experiment.IsUnlocked())
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				if (surveyDefinitionParameter.CrewRequired && !ProgressUtilities.GetAnyBodyProgress(targetBody, MannedStatus.MANNED))
				{
					flag = false;
				}
				if (surveyDefinitionParameter.EVARequired && !GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
				{
					if (targetBody == Planetarium.fetch.Home)
					{
						if (!surveyDefinitionParameter.AllowGround)
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				if (!targetBody.atmosphere && !surveyDefinitionParameter.AllowVacuum)
				{
					flag = false;
				}
				if (focusedSurvey && !surveyDefinitionParameter.AllowLow && !surveyDefinitionParameter.AllowHigh)
				{
					flag = false;
				}
				if (contextual && !surveyDefinitionParameter.AllowGround && !surveyDefinitionParameter.AllowLow)
				{
					flag = false;
				}
				if (CelestialUtilities.IsGasGiant(targetBody) && !surveyDefinitionParameter.AllowHigh)
				{
					flag = false;
				}
				int count3 = surveyDefinitionParameter.Tech.Count;
				while (count3-- > 0)
				{
					string text = surveyDefinitionParameter.Tech[count3];
					if (text.Length > 0 && !ProgressUtilities.HavePartTech(text))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					surveyDefinition.Parameters.Remove(surveyDefinitionParameter);
				}
			}
			if (surveyDefinition.Parameters.Count <= 0)
			{
				surveyDefinitions.Remove(surveyDefinition);
			}
		}
		return surveyDefinitions;
	}

	public static double ParamRange(ContractPrestige prestige, CelestialBody body, FlightBand band)
	{
		double num = 10000.0;
		switch (prestige)
		{
		case ContractPrestige.Trivial:
			num = ContractDefs.Survey.TrivialRange;
			break;
		case ContractPrestige.Significant:
			num = ContractDefs.Survey.SignificantRange;
			break;
		case ContractPrestige.Exceptional:
			num = ContractDefs.Survey.ExceptionalRange;
			break;
		}
		double result = 2000.0;
		switch (band)
		{
		case FlightBand.GROUND:
			result = num / 2.0;
			result += result * body.GeeASL;
			break;
		case FlightBand.const_2:
			result = num * 25.0;
			break;
		case FlightBand.HIGH:
			result = num * 50.0;
			break;
		}
		return result;
	}

	public void RandomizeNearKSC(ContractPrestige prestigeLevel, bool allowWater, System.Random generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		CelestialBody celestialBody = ((SpaceCenter.Instance == null) ? FlightGlobals.GetHomeBody() : SpaceCenter.Instance.cb);
		Waypoint waypoint = new Waypoint
		{
			celestialName = celestialBody.name,
			latitude = ((SpaceCenter.Instance == null) ? 0.0 : SpaceCenter.Instance.Latitude),
			longitude = ((SpaceCenter.Instance == null) ? 0.0 : SpaceCenter.Instance.Longitude)
		};
		FlightBand band;
		switch (targetBand)
		{
		default:
			band = FlightBand.GROUND;
			break;
		case FlightBand.const_2:
		case FlightBand.LOWMIX:
			band = FlightBand.const_2;
			break;
		case FlightBand.HIGH:
		case FlightBand.HIGHMIX:
		case FlightBand.ANYMIX:
			band = FlightBand.HIGH;
			break;
		}
		double num = ParamRange(prestigeLevel, celestialBody, band);
		double num2 = SurveyWaypointParameter.TriggerRange(band);
		double num3 = (focusedSurvey ? num2 : (num2 + num));
		double maximumDistance = (focusedSurvey ? (num2 + num) : (num3 + 1.0));
		waypoint.RandomizeNear(waypoint.latitude, waypoint.longitude, num3, maximumDistance, allowWater, generator);
		centerLatitude = waypoint.latitude;
		centerLongitude = waypoint.longitude;
	}
}
