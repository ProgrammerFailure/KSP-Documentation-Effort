using System;
using System.Collections.Generic;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using KSPAchievements;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class ExplorationContract : Contract
{
	public class MilestoneComparer : IComparer<ProgressMilestone>
	{
		public int Compare(ProgressMilestone first, ProgressMilestone second)
		{
			int num = ProgressTypeValue(first);
			int value = ProgressTypeValue(second);
			return num.CompareTo(value);
		}

		public int ProgressTypeValue(ProgressMilestone milestone)
		{
			return milestone.type switch
			{
				ProgressType.FLYBY => 0, 
				ProgressType.FLYBYRETURN => 6, 
				ProgressType.LANDING => 2, 
				ProgressType.LANDINGRETURN => 4, 
				ProgressType.ORBIT => 1, 
				ProgressType.ORBITRETURN => 5, 
				_ => 3, 
			};
		}
	}

	public List<ProgressTrackingParameter> trackingParameters;

	public CelestialBody targetBody;

	public float bodyRatio;

	public bool rendezvousUnlocked;

	public bool dockUnlocked;

	public bool evaUnlocked;

	public bool flagsUnlocked;

	public bool IsTutorial
	{
		get
		{
			if (TrackingParameters.Count == 1)
			{
				return TrackingParameters[0].IsTutorial;
			}
			return false;
		}
	}

	public List<ProgressTrackingParameter> TrackingParameters
	{
		get
		{
			if (trackingParameters != null)
			{
				return trackingParameters;
			}
			if (base.ParameterCount < 1)
			{
				return new List<ProgressTrackingParameter>();
			}
			trackingParameters = new List<ProgressTrackingParameter>();
			for (int i = 0; i < base.ParameterCount; i++)
			{
				if (GetParameter(i) is ProgressTrackingParameter item)
				{
					trackingParameters.Add(item);
				}
			}
			return trackingParameters;
		}
	}

	public override bool Generate()
	{
		IgnoresWeight = true;
		List<ProgressMilestone> missionMilestones = GetMissionMilestones();
		if (missionMilestones != null && missionMilestones.Count >= 1)
		{
			if (ContractSystem.Instance.GetCurrentContracts<ExplorationContract>().Length >= AllowableContracts(missionMilestones))
			{
				return false;
			}
			int i = 0;
			for (int count = missionMilestones.Count; i < count; i++)
			{
				AddParameter(new ProgressTrackingParameter(missionMilestones[i]));
			}
			SetLocals();
			SetPrestige();
			SetRewards();
			SetExpiry();
			AddKeywordsRequired("Record");
			deadlineType = DeadlineType.None;
			return true;
		}
		return false;
	}

	public override bool CanBeCancelled()
	{
		return false;
	}

	public override bool CanBeDeclined()
	{
		return false;
	}

	public override bool CanBeFailed()
	{
		return false;
	}

	public override string GetTitle()
	{
		if (IsTutorial)
		{
			ProgressMilestone milestone = TrackingParameters[0].milestone;
			switch (milestone.type)
			{
			case ProgressType.ORBIT:
				return Localizer.Format("#autoLOC_278641", milestone.body.displayName);
			case ProgressType.FIRSTLAUNCH:
				return Localizer.Format("#autoLOC_278637");
			case ProgressType.SCIENCE:
				return Localizer.Format("#autoLOC_278643", milestone.body.displayName);
			case ProgressType.REACHSPACE:
				return Localizer.Format("#autoLOC_278639");
			}
		}
		return Localizer.Format("#autoLOC_278647", targetBody.displayName);
	}

	public override string GetDescription()
	{
		if (IsTutorial)
		{
			switch (TrackingParameters[0].milestone.type)
			{
			case ProgressType.ORBIT:
				return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, "Orbit", "Orbit", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
			case ProgressType.FIRSTLAUNCH:
				return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, "Launch", "Launch", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
			case ProgressType.SCIENCE:
				return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, "Science", "Science", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
			case ProgressType.REACHSPACE:
				return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, "Space", "Space", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
			}
		}
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, targetBody.displayName, targetBody.displayName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: true, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (IsTutorial)
		{
			return TrackingParameters[0].FlavorNote;
		}
		if (bodyRatio < 0.33f)
		{
			return Localizer.Format("#autoLOC_278678", targetBody.displayName);
		}
		if (bodyRatio > 0.66f)
		{
			return Localizer.Format("#autoLOC_278680", targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_278682", targetBody.displayName);
	}

	public override string MessageCompleted()
	{
		if (IsTutorial)
		{
			switch (TrackingParameters[0].milestone.type)
			{
			case ProgressType.ORBIT:
				return Localizer.Format("#autoLOC_278698");
			case ProgressType.FIRSTLAUNCH:
				return Localizer.Format("#autoLOC_278694");
			case ProgressType.SCIENCE:
				return Localizer.Format("#autoLOC_278700");
			case ProgressType.REACHSPACE:
				return Localizer.Format("#autoLOC_278696");
			}
		}
		if (bodyRatio < 0.33f)
		{
			return Localizer.Format("#autoLOC_278705", targetBody.displayName);
		}
		if (bodyRatio > 0.66f)
		{
			return Localizer.Format("#autoLOC_278707", targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_278709", targetBody.displayName);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("bodyRatio", bodyRatio);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ExplorationContract", "targetBody", ref targetBody, Planetarium.fetch.Sun);
		SystemUtilities.LoadNode(node, "ExplorationContract", "bodyRatio", ref bodyRatio, 0.5f);
	}

	public override bool MeetRequirements()
	{
		if (base.ContractState == State.Generated || base.ContractState == State.Offered)
		{
			int count = TrackingParameters.Count;
			while (count-- > 0)
			{
				if (TrackingParameters[count].milestone.complete)
				{
					return false;
				}
			}
		}
		return true;
	}

	public int AllowableContracts(List<ProgressMilestone> milestones)
	{
		if (milestones.Count != 1)
		{
			return 1;
		}
		int num = 0;
		ProgressMilestone progressMilestone = milestones[0];
		if (progressMilestone.type != ProgressType.FIRSTLAUNCH && (progressMilestone.type != ProgressType.SCIENCE || !(progressMilestone.body == Planetarium.fetch.Home)) && progressMilestone.type != ProgressType.REACHSPACE && (progressMilestone.type != ProgressType.ORBIT || !(progressMilestone.body == Planetarium.fetch.Home)))
		{
			num = 1;
		}
		else
		{
			if (!ProgressTracking.Instance.NodeComplete("FirstLaunch"))
			{
				num++;
			}
			if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Science"))
			{
				num++;
			}
			if (!ProgressTracking.Instance.NodeComplete("ReachedSpace"))
			{
				num++;
			}
			if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Orbit"))
			{
				num++;
			}
		}
		return num;
	}

	public void SetLocals()
	{
		if (TrackingParameters.Count < 1)
		{
			targetBody = Planetarium.fetch.Home;
			bodyRatio = 0.5f;
			return;
		}
		targetBody = TrackingParameters[0].milestone.body;
		ProgressTree subtree = targetBody.progressTree.Subtree;
		bodyRatio = 0f;
		int count = subtree.Count;
		while (count-- > 0)
		{
			if (subtree[count].IsComplete)
			{
				bodyRatio += 1f;
			}
		}
		bodyRatio /= subtree.Count;
	}

	public void SetPrestige()
	{
		if (TrackingParameters.Count < 1)
		{
			prestige = ContractPrestige.Trivial;
			return;
		}
		if (IsTutorial)
		{
			switch (TrackingParameters[0].milestone.type)
			{
			case ProgressType.ORBIT:
				prestige = ContractPrestige.Exceptional;
				break;
			case ProgressType.FIRSTLAUNCH:
			case ProgressType.SCIENCE:
				prestige = ContractPrestige.Trivial;
				break;
			case ProgressType.REACHSPACE:
				prestige = ContractPrestige.Significant;
				break;
			}
			return;
		}
		int count = TrackingParameters.Count;
		do
		{
			if (count-- <= 0)
			{
				prestige = (ContractPrestige)(TrackingParameters.Count - 1);
				return;
			}
		}
		while (!ProgressUtilities.OutlierWorldFirstContract(TrackingParameters[count].milestone));
		prestige = ContractPrestige.Exceptional;
	}

	public void SetRewards()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int count = TrackingParameters.Count;
		while (count-- > 0)
		{
			ProgressTrackingParameter progressTrackingParameter = TrackingParameters[count];
			ProgressMilestone milestone = progressTrackingParameter.milestone;
			float num4 = Mathf.Ceil(ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.CONTRACT, Currency.Funds, milestone.type, milestone.body) / 2f);
			float num5 = Mathf.Ceil(ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.CONTRACT, Currency.Science, milestone.type, milestone.body) / 2f);
			float num6 = Mathf.Ceil(ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.CONTRACT, Currency.Reputation, milestone.type, milestone.body) / 2f);
			progressTrackingParameter.SetFunds(num4, targetBody);
			progressTrackingParameter.SetScience(num5);
			progressTrackingParameter.SetReputation(num6);
			num += num4;
			num2 += num5;
			num3 += num6;
		}
		float num7 = num * 0.35f;
		num -= num7;
		SetFunds(num7, num, targetBody);
		SetScience(num2);
		SetReputation(num3);
	}

	public List<ProgressMilestone> GetMissionMilestones()
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		rendezvousUnlocked = ProgressUtilities.GetProgressLevel() >= 4;
		dockUnlocked = rendezvousUnlocked && ResearchAndDevelopment.ResearchedValidContractObjectives("Dock");
		evaUnlocked = GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		flagsUnlocked = GameVariables.Instance.UnlockedEVAFlags(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		List<ProgressMilestone> list = GetSeedMilestones(kSPRandom);
		if (list.Count <= 0)
		{
			return list;
		}
		ProgressMilestone progressMilestone = list[kSPRandom.Next(list.Count)];
		list.Remove(progressMilestone);
		list.AddRange(GetThemedMilestones(progressMilestone));
		SystemUtilities.ShuffleList(ref list, kSPRandom);
		list.Insert(0, progressMilestone);
		list = list.GetRange(0, Math.Min(kSPRandom.Next(3) + 1, list.Count));
		list.Sort(new MilestoneComparer());
		return list;
	}

	public List<ProgressMilestone> GetSeedMilestones(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		if (ProgressTracking.Instance == null)
		{
			return new List<ProgressMilestone>();
		}
		bool flag = ProgressTracking.Instance.NodeComplete("FirstLaunch");
		bool flag2 = ProgressTracking.Instance.NodeComplete("ReachedSpace");
		bool flag3 = ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Orbit");
		bool flag4 = ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Science");
		CelestialBody homeBody = FlightGlobals.GetHomeBody();
		if (!ContractDefs.Progression.DisableTutorialContracts && (!flag || !flag4 || !flag2 || !flag3))
		{
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			ExplorationContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<ExplorationContract>();
			int num = currentContracts.Length;
			while (num-- > 0)
			{
				ExplorationContract explorationContract = currentContracts[num];
				if (explorationContract.TrackingParameters.Count < 1)
				{
					continue;
				}
				int count = explorationContract.TrackingParameters.Count;
				while (count-- > 0)
				{
					ProgressMilestone milestone = explorationContract.TrackingParameters[count].milestone;
					if (milestone == null)
					{
						continue;
					}
					switch (milestone.type)
					{
					case ProgressType.ORBIT:
						if (milestone.body == Planetarium.fetch.Home)
						{
							flag8 = true;
						}
						break;
					case ProgressType.FIRSTLAUNCH:
						flag5 = true;
						break;
					case ProgressType.SCIENCE:
						if (milestone.body == Planetarium.fetch.Home)
						{
							flag6 = true;
						}
						break;
					case ProgressType.REACHSPACE:
						flag7 = true;
						break;
					}
				}
			}
			if (!flag && !flag5)
			{
				return new List<ProgressMilestone>
				{
					new ProgressMilestone(Planetarium.fetch.Home, ProgressType.FIRSTLAUNCH)
				};
			}
			if (!flag4 && !flag6)
			{
				return new List<ProgressMilestone>
				{
					new ProgressMilestone(Planetarium.fetch.Home, ProgressType.SCIENCE)
				};
			}
			if (!flag2 && !flag7)
			{
				return new List<ProgressMilestone>
				{
					new ProgressMilestone(Planetarium.fetch.Home, ProgressType.REACHSPACE)
				};
			}
			if (!flag3 && !flag8)
			{
				return new List<ProgressMilestone>
				{
					new ProgressMilestone(Planetarium.fetch.Home, ProgressType.ORBIT)
				};
			}
		}
		if (ContractDefs.Progression.DisableProgressionContracts)
		{
			return new List<ProgressMilestone>();
		}
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true);
		List<ProgressMilestone> list = new List<ProgressMilestone>();
		List<ProgressMilestone> list2 = new List<ProgressMilestone>();
		bool flag9;
		if (!(flag9 = bodiesProgress.Count >= FlightGlobals.Bodies.Count / 2))
		{
			bodiesProgress.RemoveAll((CelestialBody body) => body == Planetarium.fetch.Sun);
		}
		List<ProgressMilestone> list3 = new List<ProgressMilestone>();
		int count2 = bodiesProgress.Count;
		while (count2-- > 0)
		{
			list3.Clear();
			CelestialBody celestialBody = bodiesProgress[count2];
			CelestialBodySubtree progressTree = celestialBody.progressTree;
			if (progressTree == null)
			{
				list.AddRange(list3);
				continue;
			}
			bool anyBodyProgress = ProgressUtilities.GetAnyBodyProgress(celestialBody, MannedStatus.MANNED);
			if (celestialBody.isHomeWorld)
			{
				if (list3.Count <= 0 && !progressTree.orbit.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.ORBIT));
				}
				if (list3.Count <= 0 && evaUnlocked && anyBodyProgress && !progressTree.spacewalk.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.SPACEWALK));
				}
				if (list3.Count <= 0 && !progressTree.returnFromOrbit.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.ORBITRETURN));
				}
			}
			else
			{
				if (list3.Count <= 0 && !progressTree.flyBy.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.FLYBY));
				}
				if (list3.Count <= 0 && !progressTree.orbit.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.ORBIT));
				}
				if (list3.Count <= 0)
				{
					if (!progressTree.science.IsComplete)
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.SCIENCE));
					}
					if (evaUnlocked && anyBodyProgress && !progressTree.spacewalk.IsComplete)
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.SPACEWALK));
					}
					if (CelestialUtilities.IsFlyablePlanet(celestialBody) && !progressTree.flight.IsComplete)
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.FLIGHT));
					}
				}
				if (list3.Count <= 0)
				{
					if (!progressTree.returnFromFlyby.IsComplete)
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.FLYBYRETURN));
					}
					if (!progressTree.returnFromOrbit.IsComplete)
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.ORBITRETURN));
					}
					if (!CelestialUtilities.IsGasGiant(celestialBody))
					{
						if (!progressTree.landing.IsComplete)
						{
							list3.Add(new ProgressMilestone(celestialBody, ProgressType.LANDING));
						}
						if (celestialBody.ocean && !progressTree.splashdown.IsComplete)
						{
							list3.Add(new ProgressMilestone(celestialBody, ProgressType.SPLASHDOWN));
						}
					}
				}
			}
			if (list3.Count > 0)
			{
				list.AddRange(list3);
				continue;
			}
			if (celestialBody != Planetarium.fetch.Home && !CelestialUtilities.IsGasGiant(celestialBody))
			{
				if (!progressTree.returnFromSurface.IsComplete)
				{
					if (celestialBody.atmosphere)
					{
						list2.Add(new ProgressMilestone(celestialBody, ProgressType.LANDINGRETURN));
					}
					else
					{
						list3.Add(new ProgressMilestone(celestialBody, ProgressType.LANDINGRETURN));
					}
				}
				if (evaUnlocked && anyBodyProgress && !progressTree.surfaceEVA.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.SURFACEEVA));
				}
				if (evaUnlocked && flagsUnlocked && anyBodyProgress && !progressTree.flagPlant.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.FLAGPLANT));
				}
				if (dockUnlocked && anyBodyProgress && !progressTree.baseConstruction.IsComplete)
				{
					list2.Add(new ProgressMilestone(celestialBody, ProgressType.BASECONSTRUCTION));
				}
			}
			if (celestialBody.isHomeWorld && !progressTree.escape.IsComplete && ProgressUtilities.ReachedHomeBodies())
			{
				list3.Add(new ProgressMilestone(celestialBody, ProgressType.ESCAPE));
			}
			if (rendezvousUnlocked)
			{
				if (!progressTree.rendezvous.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.RENDEZVOUS));
				}
				else if (anyBodyProgress && !progressTree.crewTransfer.IsComplete)
				{
					list3.Add(new ProgressMilestone(celestialBody, ProgressType.CREWTRANSFER));
				}
			}
			if (dockUnlocked && !progressTree.docking.IsComplete)
			{
				list3.Add(new ProgressMilestone(celestialBody, ProgressType.DOCKING));
			}
			if (dockUnlocked && anyBodyProgress && !progressTree.stationConstruction.IsComplete)
			{
				list2.Add(new ProgressMilestone(celestialBody, ProgressType.STATIONCONSTRUCTION));
			}
			list.AddRange(list3);
		}
		bool flag10 = true;
		int count3 = list.Count;
		while (count3-- > 0)
		{
			ProgressMilestone progressMilestone = list[count3];
			switch (progressMilestone.type)
			{
			case ProgressType.FLYBYRETURN:
			case ProgressType.ORBITRETURN:
				if (progressMilestone.body == homeBody || progressMilestone.body.referenceBody == homeBody)
				{
					flag10 = false;
				}
				break;
			case ProgressType.FLIGHT:
			case ProgressType.FLYBY:
			case ProgressType.LANDING:
			case ProgressType.ORBIT:
			case ProgressType.SCIENCE:
			case ProgressType.SPACEWALK:
			case ProgressType.SPLASHDOWN:
				flag10 = false;
				break;
			}
		}
		if (flag10)
		{
			List<CelestialBody> list4 = (flag9 ? ProgressUtilities.GetNextUnreached(1) : ProgressUtilities.GetNextUnreached(1, (CelestialBody body) => body != Planetarium.fetch.Sun));
			int count4 = list4.Count;
			while (count4-- > 0)
			{
				list.Add(new ProgressMilestone(list4[count4], ProgressType.FLYBY));
			}
		}
		if (list.Count < 1)
		{
			list = list2;
		}
		if (list.Count > 0)
		{
			List<CelestialBody> list5 = new List<CelestialBody>();
			int count5 = list.Count;
			while (count5-- > 0)
			{
				CelestialBody body2 = list[count5].body;
				if (body2 != null && !list5.Contains(body2))
				{
					list5.Add(body2);
				}
			}
			CelestialBody celestialBody2 = WeightedBodyChoice(list5, generator);
			int count6 = list.Count;
			while (count6-- > 0)
			{
				if (list[count6].body != celestialBody2)
				{
					list.Remove(list[count6]);
				}
			}
		}
		return list;
	}

	public List<ProgressMilestone> GetThemedMilestones(ProgressMilestone seed)
	{
		List<ProgressMilestone> list = new List<ProgressMilestone>();
		CelestialBody body = seed.body;
		CelestialBodySubtree progressTree = body.progressTree;
		if (progressTree == null)
		{
			return list;
		}
		bool anyBodyProgress = ProgressUtilities.GetAnyBodyProgress(body, MannedStatus.MANNED);
		bool hasSolidSurface;
		bool flag = (hasSolidSurface = body.hasSolidSurface) && body.ocean;
		if (!body.isHomeWorld)
		{
			switch (seed.type)
			{
			case ProgressType.FLIGHT:
				if (hasSolidSurface && !progressTree.landing.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.LANDING));
				}
				if (hasSolidSurface && flag && !progressTree.splashdown.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SPLASHDOWN));
				}
				break;
			case ProgressType.FLYBY:
				if (!progressTree.returnFromFlyby.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.FLYBYRETURN));
				}
				if (!progressTree.science.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SCIENCE));
				}
				break;
			case ProgressType.LANDING:
				if (!progressTree.returnFromSurface.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.LANDINGRETURN));
				}
				if (hasSolidSurface && evaUnlocked && anyBodyProgress && !progressTree.surfaceEVA.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SURFACEEVA));
				}
				if (hasSolidSurface && evaUnlocked && flagsUnlocked && anyBodyProgress && !progressTree.flagPlant.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.FLAGPLANT));
				}
				break;
			case ProgressType.ORBIT:
				if (!progressTree.returnFromOrbit.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.ORBITRETURN));
				}
				if (!progressTree.science.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SCIENCE));
				}
				if (evaUnlocked && anyBodyProgress && !progressTree.spacewalk.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SPACEWALK));
				}
				break;
			case ProgressType.FLYBYRETURN:
			case ProgressType.ORBITRETURN:
				if (rendezvousUnlocked && !progressTree.rendezvous.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.RENDEZVOUS));
				}
				if (dockUnlocked && !progressTree.docking.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.DOCKING));
				}
				if (rendezvousUnlocked && anyBodyProgress && !progressTree.crewTransfer.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.CREWTRANSFER));
				}
				break;
			case ProgressType.SCIENCE:
				if (!progressTree.returnFromFlyby.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.FLYBYRETURN));
				}
				if (!progressTree.returnFromOrbit.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.ORBITRETURN));
				}
				if (hasSolidSurface && evaUnlocked && anyBodyProgress && !progressTree.surfaceEVA.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SURFACEEVA));
				}
				break;
			case ProgressType.SPACEWALK:
				if (!progressTree.returnFromFlyby.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.FLYBYRETURN));
				}
				if (!progressTree.returnFromOrbit.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.ORBITRETURN));
				}
				if (rendezvousUnlocked && !progressTree.rendezvous.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.RENDEZVOUS));
				}
				if (rendezvousUnlocked && anyBodyProgress && !progressTree.crewTransfer.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.CREWTRANSFER));
				}
				break;
			case ProgressType.SPLASHDOWN:
				if (!progressTree.returnFromSurface.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.LANDINGRETURN));
				}
				if (hasSolidSurface && evaUnlocked && anyBodyProgress && !progressTree.surfaceEVA.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.SURFACEEVA));
				}
				break;
			}
		}
		else
		{
			switch (seed.type)
			{
			case ProgressType.SPACEWALK:
				if (!progressTree.returnFromOrbit.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.ORBITRETURN));
				}
				if (rendezvousUnlocked && !progressTree.rendezvous.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.RENDEZVOUS));
				}
				if (rendezvousUnlocked && anyBodyProgress && !progressTree.crewTransfer.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.CREWTRANSFER));
				}
				break;
			case ProgressType.ORBITRETURN:
				if (rendezvousUnlocked && !progressTree.rendezvous.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.RENDEZVOUS));
				}
				if (dockUnlocked && !progressTree.docking.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.DOCKING));
				}
				if (rendezvousUnlocked && anyBodyProgress && !progressTree.crewTransfer.IsComplete)
				{
					list.Add(new ProgressMilestone(body, ProgressType.CREWTRANSFER));
				}
				break;
			}
		}
		return list;
	}
}
