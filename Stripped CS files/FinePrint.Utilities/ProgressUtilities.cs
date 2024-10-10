using System;
using System.Collections.Generic;
using Contracts;
using KSPAchievements;
using UnityEngine;

namespace FinePrint.Utilities;

public class ProgressUtilities
{
	public static bool HavePartTech(string partName, bool logging = true)
	{
		partName = partName.Replace('_', '.');
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(partName);
		if (partInfoByName != null)
		{
			if (ResearchAndDevelopment.PartTechAvailable(partInfoByName))
			{
				return true;
			}
		}
		else if (logging)
		{
			Debug.LogWarning("Contract Log: Attempted to check for nonexistent technology: \"" + partName + "\".");
		}
		return false;
	}

	public static bool HaveModuleTech(string moduleName, string excludeModule = null)
	{
		int count = PartLoader.LoadedPartsList.Count;
		while (true)
		{
			if (count-- > 0)
			{
				AvailablePart availablePart = PartLoader.LoadedPartsList[count];
				if (availablePart.partPrefab == null || availablePart.partPrefab.Modules == null || !ResearchAndDevelopment.PartTechAvailable(availablePart))
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				int count2 = availablePart.partPrefab.Modules.Count;
				while (count2-- > 0)
				{
					PartModule partModule = availablePart.partPrefab.Modules[count2];
					if (partModule.moduleName == moduleName)
					{
						flag = true;
					}
					if (excludeModule != null && partModule.moduleName == excludeModule)
					{
						flag2 = true;
					}
				}
				if (flag && !flag2)
				{
					break;
				}
				continue;
			}
			return false;
		}
		return true;
	}

	public static bool HaveAnyTech(IList<string> partNames, IList<string> moduleNames, bool logging = true)
	{
		if (partNames != null)
		{
			int count = partNames.Count;
			while (count-- > 0)
			{
				if (HavePartTech(partNames[count], logging))
				{
					return true;
				}
			}
		}
		if (moduleNames != null)
		{
			int count2 = moduleNames.Count;
			while (count2-- > 0)
			{
				if (HaveModuleTech(moduleNames[count2]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool GetBodyProgress(ProgressType progress, CelestialBody body, MannedStatus manned = MannedStatus.const_2)
	{
		return new ProgressMilestone(body, progress, manned).complete;
	}

	public static bool GetAnyBodyProgress(CelestialBody body, MannedStatus manned = MannedStatus.const_2)
	{
		if (body.isHomeWorld)
		{
			return true;
		}
		ProgressType[] array = (ProgressType[])Enum.GetValues(typeof(ProgressType));
		ProgressMilestone progressMilestone = new ProgressMilestone(body, ProgressType.ORBIT, manned);
		int num = array.Length;
		do
		{
			if (num-- > 0)
			{
				progressMilestone.type = array[num];
				continue;
			}
			return false;
		}
		while (!progressMilestone.bodySensitive || (manned != MannedStatus.const_2 && !progressMilestone.crewSensitive && !progressMilestone.impliesManned) || !progressMilestone.complete);
		return true;
	}

	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool bodyReached, bool progressComplete, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		if (bodies == null)
		{
			bodies = FlightGlobals.Bodies;
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count = bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = bodies[count];
			if (!(celestialBody == null) && (where == null || where(celestialBody)) && GetAnyBodyProgress(celestialBody, manned) == bodyReached && GetBodyProgress(type, celestialBody, manned) == progressComplete)
			{
				list.Add(celestialBody);
			}
		}
		return list;
	}

	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool bodyReached, bool progressComplete, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		return GetBodiesProgress(type, bodyReached, progressComplete, MannedStatus.const_2, where, bodies);
	}

	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool progressComplete, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		if (bodies == null)
		{
			bodies = FlightGlobals.Bodies;
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count = bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = bodies[count];
			if (!(celestialBody == null) && (where == null || where(celestialBody)) && GetBodyProgress(type, celestialBody, manned) == progressComplete)
			{
				list.Add(celestialBody);
			}
		}
		return list;
	}

	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool progressComplete, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		return GetBodiesProgress(type, progressComplete, MannedStatus.const_2, where, bodies);
	}

	public static List<CelestialBody> GetBodiesProgress(bool bodyReached, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		if (bodies == null)
		{
			bodies = FlightGlobals.Bodies;
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count = bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = bodies[count];
			if (!(celestialBody == null) && (where == null || where(celestialBody)) && GetAnyBodyProgress(celestialBody, manned) == bodyReached)
			{
				list.Add(celestialBody);
			}
		}
		return list;
	}

	public static List<CelestialBody> GetBodiesProgress(bool bodyReached, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		return GetBodiesProgress(bodyReached, MannedStatus.const_2, where, bodies);
	}

	public static List<CelestialBody> GetNextUnreached(int count, MannedStatus manned, Func<CelestialBody, bool> where = null)
	{
		Stack<CelestialBody> stack = new Stack<CelestialBody>();
		List<CelestialBody> list = new List<CelestialBody>();
		List<CelestialBody> list2 = new List<CelestialBody>();
		CelestialBody sun = Planetarium.fetch.Sun;
		if (sun != null)
		{
			list.Add(sun);
			if (sun.orbitingBodies != null)
			{
				list.AddRange(sun.orbitingBodies);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		for (int num = list.Count - 1; num >= 0; num--)
		{
			stack.Push(list[num]);
		}
		while (stack.Count > 0 && list2.Count < count)
		{
			CelestialBody celestialBody = stack.Pop();
			if (celestialBody == null)
			{
				continue;
			}
			if (!GetAnyBodyProgress(celestialBody, manned) && (where == null || where(celestialBody)))
			{
				list2.Add(celestialBody);
			}
			if (!(celestialBody == sun) && celestialBody.orbitingBodies != null)
			{
				list = new List<CelestialBody>(celestialBody.orbitingBodies);
				list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
				int count2 = list.Count;
				while (count2-- > 0)
				{
					stack.Push(list[count2]);
				}
			}
		}
		return list2;
	}

	public static List<CelestialBody> GetNextUnreached(int count, Func<CelestialBody, bool> where = null)
	{
		return GetNextUnreached(count, MannedStatus.const_2, where);
	}

	public static Dictionary<CelestialBody, int> CelestialCrewCounts(List<Vessel.Situations> situations)
	{
		Dictionary<CelestialBody, int> dictionary = new Dictionary<CelestialBody, int>();
		int count = HighLogic.CurrentGame.flightState.protoVessels.Count;
		while (count-- > 0)
		{
			ProtoVessel protoVessel = HighLogic.CurrentGame.flightState.protoVessels[count];
			int count2 = protoVessel.GetVesselCrew().Count;
			if (count2 == 0)
			{
				continue;
			}
			int count3 = situations.Count;
			while (count3-- > 0)
			{
				if (protoVessel.situation == situations[count3])
				{
					if (dictionary.ContainsKey(protoVessel.vesselRef.mainBody))
					{
						dictionary[protoVessel.vesselRef.mainBody] += count2;
					}
					else
					{
						dictionary[protoVessel.vesselRef.mainBody] = count2;
					}
				}
			}
		}
		return dictionary;
	}

	public static double CurrentTrackRecord(RecordTrackType type)
	{
		switch (type)
		{
		case RecordTrackType.ALTITUDE:
			if (ProgressTracking.Instance.FindNode("RecordsAltitude") is RecordsAltitude recordsAltitude)
			{
				return recordsAltitude.record;
			}
			break;
		case RecordTrackType.DEPTH:
			if (ProgressTracking.Instance.FindNode("RecordsDepth") is RecordsDepth recordsDepth)
			{
				return recordsDepth.record;
			}
			break;
		case RecordTrackType.DISTANCE:
			if (ProgressTracking.Instance.FindNode("RecordsDistance") is RecordsDistance recordsDistance)
			{
				return recordsDistance.record;
			}
			break;
		case RecordTrackType.SPEED:
			if (ProgressTracking.Instance.FindNode("RecordsSpeed") is RecordsSpeed recordsSpeed)
			{
				return recordsSpeed.record;
			}
			break;
		}
		return 0.0;
	}

	public static float AverageFacilityLevel()
	{
		return (ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Administration) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.LaunchPad) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Runway) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.SpaceplaneHangar) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation) + ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.VehicleAssemblyBuilding)) / 9f;
	}

	public static int GetProgressLevel()
	{
		int num = 0;
		int count = FlightGlobals.Bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[count];
			if (!(celestialBody == null) && !(celestialBody == Planetarium.fetch.Sun) && !(celestialBody == Planetarium.fetch.Home) && ProgressTracking.Instance.NodeReached(celestialBody.name))
			{
				num++;
			}
		}
		if (ProgressTracking.Instance.NodeComplete("FirstLaunch"))
		{
			num++;
		}
		if (ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Orbit"))
		{
			num++;
		}
		return num + Math.Max(0, (int)Math.Round(AverageFacilityLevel() / ContractDefs.FacilityProgressionFactor));
	}

	public static bool IntroWorldFirstContract(ProgressMilestone milestone)
	{
		if (milestone.type == ProgressType.FIRSTLAUNCH)
		{
			return true;
		}
		if (milestone.type == ProgressType.REACHSPACE)
		{
			return true;
		}
		if (milestone.type == ProgressType.ORBIT && milestone.body == Planetarium.fetch.Home)
		{
			return true;
		}
		return false;
	}

	public static bool OutlierWorldFirstContract(ProgressType type, CelestialBody body)
	{
		if (type == ProgressType.LANDINGRETURN && body != null && body.atmosphere)
		{
			return true;
		}
		return type switch
		{
			ProgressType.BASECONSTRUCTION => true, 
			ProgressType.STATIONCONSTRUCTION => true, 
			_ => false, 
		};
	}

	public static bool OutlierWorldFirstContract(ProgressMilestone milestone)
	{
		return OutlierWorldFirstContract(milestone.type, milestone.body);
	}

	public static float ScoreProgressType(ProgressType type, CelestialBody body)
	{
		bool flag = body == null || body == Planetarium.fetch.Home;
		switch (type)
		{
		case ProgressType.ESCAPE:
			if (!flag)
			{
				return 0.6f;
			}
			return 1.1f;
		default:
			return 0.1f;
		case ProgressType.FLIGHT:
			if (!flag)
			{
				return 1.2f;
			}
			return 0.5f;
		case ProgressType.LANDING:
		case ProgressType.LANDINGRETURN:
			if (!flag)
			{
				return 1.5f;
			}
			return 0.1f;
		case ProgressType.ORBIT:
		case ProgressType.ORBITRETURN:
		case ProgressType.RENDEZVOUS:
			if (!flag)
			{
				return 0.8f;
			}
			return 1f;
		case ProgressType.SPACEWALK:
			return 0.6f;
		case ProgressType.ALTITUDERECORD:
		case ProgressType.DEPTHRECORD:
		case ProgressType.DISTANCERECORD:
		case ProgressType.SPEEDRECORD:
			return 2f;
		case ProgressType.SPLASHDOWN:
			if (!flag)
			{
				return 1.5f;
			}
			return 0.3f;
		case ProgressType.REACHSPACE:
		case ProgressType.STATIONCONSTRUCTION:
			return 0.8f;
		case ProgressType.BASECONSTRUCTION:
		case ProgressType.CREWTRANSFER:
		case ProgressType.DOCKING:
		case ProgressType.POINTOFINTEREST:
		case ProgressType.STUNT:
			if (!flag)
			{
				return 0.8f;
			}
			return 0.3f;
		case ProgressType.FLYBY:
		case ProgressType.FLYBYRETURN:
		case ProgressType.SUBORBIT:
			if (!flag)
			{
				return 1f;
			}
			return 0.8f;
		case ProgressType.FLAGPLANT:
		case ProgressType.SCIENCE:
		case ProgressType.SURFACEEVA:
			if (!flag)
			{
				return 0.6f;
			}
			return 0.2f;
		}
	}

	public static Contract.ContractPrestige ProgressTypePrestige(ProgressType type, CelestialBody body = null)
	{
		if (OutlierWorldFirstContract(type, body))
		{
			return Contract.ContractPrestige.Exceptional;
		}
		float num = ScoreProgressType(type, body);
		if (!(body == null) && !(body == Planetarium.fetch.Home))
		{
			if (num <= ScoreProgressType(ProgressType.ESCAPE, body))
			{
				return Contract.ContractPrestige.Trivial;
			}
			if (num >= ScoreProgressType(ProgressType.FLIGHT, body))
			{
				return Contract.ContractPrestige.Exceptional;
			}
		}
		else
		{
			if (num <= ScoreProgressType(ProgressType.FLIGHT, body))
			{
				return Contract.ContractPrestige.Trivial;
			}
			if (num >= ScoreProgressType(ProgressType.ORBIT, body))
			{
				return Contract.ContractPrestige.Exceptional;
			}
		}
		return Contract.ContractPrestige.Significant;
	}

	public static Contract.ContractPrestige ProgressTypePrestige(ProgressMilestone milestone)
	{
		return ProgressTypePrestige(milestone.type, milestone.body);
	}

	public static float WorldFirstStandardReward(ProgressRewardType reward, Currency currency, ProgressType progress, CelestialBody body = null)
	{
		float num = 0f;
		switch (currency)
		{
		case Currency.Funds:
			num = ContractDefs.Progression.Funds.BaseReward;
			break;
		case Currency.Science:
			num = ContractDefs.Progression.Science.BaseReward;
			break;
		case Currency.Reputation:
			num = ContractDefs.Progression.Reputation.BaseReward;
			break;
		}
		num *= ((reward == ProgressRewardType.CONTRACT) ? (1f - ContractDefs.Progression.PassiveBaseRatio) : ContractDefs.Progression.PassiveBaseRatio);
		num *= ScoreProgressType(progress, body);
		if (OutlierWorldFirstContract(progress, body))
		{
			num *= ContractDefs.Progression.OutlierMilestoneMultiplier;
		}
		if (reward == ProgressRewardType.CONTRACT)
		{
			return num;
		}
		switch (currency)
		{
		case Currency.Funds:
			num *= GameVariables.Instance.GetContractFundsCompletionFactor(ProgressTypePrestige(progress));
			break;
		case Currency.Science:
			num *= GameVariables.Instance.GetContractScienceCompletionFactor(ProgressTypePrestige(progress));
			break;
		case Currency.Reputation:
			num *= GameVariables.Instance.GetContractReputationCompletionFactor(ProgressTypePrestige(progress));
			break;
		}
		if (currency == Currency.Funds && body != null)
		{
			num *= 1f + (GameVariables.Instance.GetContractDestinationWeight(body) - 1f) * ContractDefs.Progression.PassiveBodyRatio;
		}
		return Mathf.Round(num);
	}

	public static float WorldFirstIntervalReward(ProgressRewardType reward, Currency currency, ProgressType progress, CelestialBody body = null, int currentInterval = 1, int totalIntervals = 10)
	{
		currentInterval = ((currentInterval < 1) ? 1 : currentInterval);
		totalIntervals = ((totalIntervals < currentInterval) ? currentInterval : totalIntervals);
		float num = WorldFirstStandardReward(reward, currency, progress, body);
		float num2 = num / (float)totalIntervals;
		if (num2 > 1f)
		{
			return num2;
		}
		if (num2 <= 0f)
		{
			return 0f;
		}
		float f = num2 * (float)(currentInterval - 1);
		float f2 = num2 * (float)currentInterval;
		if (Mathf.Floor(f) < Mathf.Floor(f2))
		{
			return 1f;
		}
		if (currentInterval == totalIntervals)
		{
			return Mathf.Round(num % 1f);
		}
		return 0f;
	}

	public static bool ExperimentPossibleAt(string experimentID, CelestialBody body, double latitude, double longitude, double altitude, double terrainHeight)
	{
		if (experimentID == null)
		{
			return false;
		}
		ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentID);
		if (experiment == null)
		{
			return false;
		}
		ExperimentSituations situation = ((terrainHeight < 0.0 && body.ocean && altitude <= 0.0) ? ExperimentSituations.SrfSplashed : ((altitude <= terrainHeight) ? ExperimentSituations.SrfLanded : ((!body.atmosphere || !(altitude < body.atmosphereDepth)) ? ((altitude < (double)body.scienceValues.spaceAltitudeThreshold) ? ExperimentSituations.InSpaceLow : ExperimentSituations.InSpaceHigh) : ((altitude < (double)body.scienceValues.flyingAltitudeThreshold) ? ExperimentSituations.FlyingLow : ExperimentSituations.FlyingHigh))));
		return experiment.IsAvailableWhile(situation, body);
	}

	public static bool ReachedHomeBodies()
	{
		if (ProgressTracking.Instance == null)
		{
			return false;
		}
		CelestialBodySubtree bodyTree = ProgressTracking.Instance.GetBodyTree(Planetarium.fetch.Home);
		if (bodyTree != null && bodyTree.IsReached)
		{
			int count = Planetarium.fetch.Home.orbitingBodies.Count;
			CelestialBodySubtree bodyTree2;
			do
			{
				if (count-- > 0)
				{
					bodyTree2 = ProgressTracking.Instance.GetBodyTree(Planetarium.fetch.Home.orbitingBodies[count]);
					continue;
				}
				return true;
			}
			while (bodyTree2 != null && bodyTree2.IsReached);
			return false;
		}
		return false;
	}

	public static double FindNextRecord(double currentRecord, double maximumRecord, double roundValue)
	{
		double num = ContractDefs.Progression.RecordSplit;
		double x = Math.Pow(100.0, 1.0 / (num - 1.0));
		int num2 = 1;
		double num3;
		while (true)
		{
			if ((double)num2 <= num)
			{
				num3 = maximumRecord * (0.01 * Math.Pow(x, Convert.ToDouble(num2 - 1)));
				num3 = Math.Round(num3 / roundValue) * roundValue;
				if (!(num3 <= currentRecord))
				{
					break;
				}
				num2++;
				continue;
			}
			return double.MaxValue;
		}
		return num3;
	}

	public static double FindNextRecord(double currentRecord, double maximumRecord, double roundValue, ref int interval)
	{
		double num = ContractDefs.Progression.RecordSplit;
		double x = Math.Pow(100.0, 1.0 / (num - 1.0));
		int num2 = 1;
		double num3;
		while (true)
		{
			if ((double)num2 <= num)
			{
				num3 = maximumRecord * (0.01 * Math.Pow(x, Convert.ToDouble(num2 - 1)));
				num3 = Math.Round(num3 / roundValue) * roundValue;
				if (!(num3 <= currentRecord))
				{
					break;
				}
				num2++;
				continue;
			}
			interval = int.MaxValue;
			return double.MaxValue;
		}
		interval = num2;
		return num3;
	}

	public static bool VisitedSurfaceOf(CelestialBody body, MannedStatus manned = MannedStatus.const_2)
	{
		if (body.isHomeWorld)
		{
			return true;
		}
		bool flag;
		bool flag2;
		switch (manned)
		{
		default:
			flag = body.hasSolidSurface && ProgressTracking.Instance.NodeComplete(body.name, "Landing");
			flag2 = body.ocean && ProgressTracking.Instance.NodeComplete(body.name, "Splashdown");
			break;
		case MannedStatus.UNMANNED:
			flag = body.hasSolidSurface && ProgressTracking.Instance.NodeCompleteUnmanned(body.name, "Landing");
			flag2 = body.ocean && ProgressTracking.Instance.NodeCompleteUnmanned(body.name, "Splashdown");
			break;
		case MannedStatus.MANNED:
			flag = body.hasSolidSurface && ProgressTracking.Instance.NodeCompleteManned(body.name, "Landing");
			flag2 = body.ocean && ProgressTracking.Instance.NodeCompleteManned(body.name, "Splashdown");
			break;
		}
		return flag || flag2;
	}
}
