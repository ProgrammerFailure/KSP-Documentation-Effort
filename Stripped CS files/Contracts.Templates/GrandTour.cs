using System;
using System.Collections.Generic;
using System.Globalization;
using FinePrint;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

[Serializable]
public class GrandTour : Contract
{
	[SerializeField]
	public List<CelestialBody> targetBodies;

	public ProgressType progressType;

	public bool finalDestination;

	public bool focused;

	public CelestialBody focusBody;

	public CelestialBody finalBody;

	public Vessel.Situations finalSituation;

	public List<CelestialBody> TargetBodies => targetBodies;

	public int TotalBodies
	{
		get
		{
			if (!finalDestination)
			{
				return targetBodies.Count;
			}
			return targetBodies.Count + 1;
		}
	}

	public string FormattedDestinations
	{
		get
		{
			List<string> list = new List<string>();
			int count = targetBodies.Count;
			for (int i = 0; i < count; i++)
			{
				list.Add(Localizer.Format("#autoLOC_7001301", targetBodies[i].displayName));
			}
			return StringUtilities.ThisThisAndThat(list);
		}
	}

	public override bool MeetRequirements()
	{
		return (float)(SystemUtilities.HashNumber((uint)base.MissionSeed) % 100) <= ContractDefs.Grand.Rarity;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return targetBodies;
	}

	public override string GetDescription()
	{
		string text = (focused ? focusBody.displayName : ((targetBodies.Count <= 0) ? FlightGlobals.GetHomeBodyDisplayName() : targetBodies[0].displayName));
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, text, text, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		ProgressType progressType = this.progressType;
		text = ((progressType != ProgressType.LANDING) ? Localizer.Format("#autoLOC_6200001", FormattedDestinations) : Localizer.Format("#autoLOC_6200000", FormattedDestinations));
		if (finalDestination)
		{
			text2 = Localizer.Format("#autoLOC_272642");
			switch (finalSituation)
			{
			case Vessel.Situations.SPLASHED:
				text3 = Localizer.Format("#autoLOC_272656");
				break;
			case Vessel.Situations.LANDED:
				text3 = Localizer.Format("#autoLOC_272650");
				break;
			case Vessel.Situations.ORBITING:
				text3 = Localizer.Format("#autoLOC_272653");
				break;
			case Vessel.Situations.SUB_ORBITAL:
				text3 = Localizer.Format("#autoLOC_272659");
				break;
			case Vessel.Situations.FLYING:
				text3 = Localizer.Format("#autoLOC_272647");
				break;
			}
			text4 = finalBody.displayName;
		}
		text5 = Localizer.Format("#autoLOC_272666");
		return Localizer.Format("#autoLOC_6002377", text, text2, text3, text4, text5);
	}

	public override string GetTitle()
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		string text = "";
		string template = "";
		string text2 = "";
		string text3 = "";
		text = Localizer.Format("#autoLOC_272675");
		if (finalDestination)
		{
			switch (prestige)
			{
			case ContractPrestige.Trivial:
				template = Localizer.Format("#autoLOC_272682");
				break;
			case ContractPrestige.Significant:
				template = Localizer.Format("#autoLOC_272685");
				break;
			case ContractPrestige.Exceptional:
				template = Localizer.Format("#autoLOC_272688");
				break;
			}
		}
		text2 = (focused ? ((focusBody == Planetarium.fetch.Sun) ? "" : (focusBody.bodyAdjectiveDisplayName + " " + TotalBodies + " ")) : (targetBodies[kSPRandom.Next(0, targetBodies.Count)].bodyAdjectiveDisplayName + " " + TotalBodies + " "));
		if (!focused)
		{
			if (progressType == ProgressType.FLYBY)
			{
				switch (prestige)
				{
				case ContractPrestige.Trivial:
					text3 = Localizer.Format("#autoLOC_272705");
					break;
				case ContractPrestige.Significant:
					text3 = Localizer.Format("#autoLOC_272708");
					break;
				case ContractPrestige.Exceptional:
					text3 = Localizer.Format("#autoLOC_272711");
					break;
				}
			}
			else
			{
				switch (prestige)
				{
				case ContractPrestige.Trivial:
					text3 = Localizer.Format("#autoLOC_272720");
					break;
				case ContractPrestige.Significant:
					text3 = Localizer.Format("#autoLOC_272723");
					break;
				case ContractPrestige.Exceptional:
					text3 = Localizer.Format("#autoLOC_272726");
					break;
				}
			}
		}
		else
		{
			text3 = Localizer.Format("#autoLOC_272732");
		}
		template = Localizer.Format(template, text3);
		return Localizer.Format("#autoLOC_6002376", text, template, text2, text3);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_272741", TotalBodies);
	}

	public override void OnAccepted()
	{
		VesselSystemsParameter parameter = GetParameter<VesselSystemsParameter>();
		if (parameter != null && parameter.requireNew)
		{
			parameter.launchID = HighLogic.CurrentGame.launchID;
		}
	}

	public override string GetNotes()
	{
		if (finalDestination && CelestialUtilities.IsGasGiant(finalBody))
		{
			string text = "";
			if (finalSituation != Vessel.Situations.FLYING && finalSituation != Vessel.Situations.SUB_ORBITAL)
			{
				return null;
			}
			text = Localizer.Format("#autoLOC_272752");
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
			{
				text += "\n";
			}
			return text;
		}
		return null;
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "Template.GrandTour", "progressType", ref progressType, ProgressType.FLYBY);
		SystemUtilities.LoadNode(node, "Template.GrandTour", "finalDestination", ref finalDestination, defaultValue: false);
		SystemUtilities.LoadNode(node, "Template.GrandTour", "focused", ref focused, defaultValue: false);
		if (node.HasValue("body"))
		{
			targetBodies = new List<CelestialBody>();
			string[] values = node.GetValues("body");
			int num = values.Length;
			for (int i = 0; i < num; i++)
			{
				targetBodies.Add(FlightGlobals.fetch.bodies[int.Parse(values[i])]);
			}
		}
		if (focused)
		{
			SystemUtilities.LoadNode(node, "Template.GrandTour", "focusBody", ref focusBody, Planetarium.fetch.Home);
		}
		if (finalDestination)
		{
			SystemUtilities.LoadNode(node, "Template.GrandTour", "finalBody", ref finalBody, Planetarium.fetch.Home);
			SystemUtilities.LoadNode(node, "Template.GrandTour", "finalSituation", ref finalSituation, Vessel.Situations.ORBITING);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("progressType", (int)progressType);
		node.AddValue("finalDestination", finalDestination);
		node.AddValue("focused", focused);
		int count = targetBodies.Count;
		for (int i = 0; i < count; i++)
		{
			node.AddValue("body", targetBodies[i].flightGlobalsIndex);
		}
		if (focused)
		{
			node.AddValue("focusBody", focusBody.flightGlobalsIndex);
		}
		if (finalDestination)
		{
			node.AddValue("finalBody", finalBody.flightGlobalsIndex);
			node.AddValue("finalSituation", (int)finalSituation);
		}
	}

	public override bool Generate()
	{
		KSPRandom generator = new KSPRandom(base.MissionSeed);
		targetBodies = new List<CelestialBody>();
		focused = false;
		focusBody = null;
		if (base.Prestige == ContractPrestige.Exceptional && SystemUtilities.CoinFlip(generator))
		{
			AttemptFocusedTour(generator);
		}
		if (!focused)
		{
			AttemptRandomTour(generator);
		}
		AttemptFinalDestination(generator);
		if (targetBodies.Count <= 0)
		{
			return false;
		}
		string text = Localizer.Format("#autoLOC_7001016");
		AddParameter(new VesselSystemsParameter(null, null, text));
		int count = targetBodies.Count;
		for (int i = 0; i < count; i++)
		{
			FlightLog.EntryType targetType = ((progressType == ProgressType.FLYBY) ? FlightLog.EntryType.Flyby : FlightLog.EntryType.Land);
			AddParameter(new VesselDestinationParameter(targetBodies[i], targetType));
		}
		if (finalDestination)
		{
			AddParameter(new LocationAndSituationParameter(finalBody, finalSituation, text, finalObjective: true));
		}
		switch (prestige)
		{
		default:
			if (!focused)
			{
				AddKeywords("Pioneer", "Commercial", "Scientific");
			}
			else
			{
				AddKeywords("Record");
			}
			break;
		case ContractPrestige.Significant:
			AddKeywords("Commercial", "Scientific");
			break;
		case ContractPrestige.Trivial:
			AddKeywords("Commercial");
			break;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		count = targetBodies.Count;
		for (int j = 0; j < count; j++)
		{
			CelestialBody celestialBody = targetBodies[j];
			float num8 = ProgressUtilities.ScoreProgressType(progressType, celestialBody);
			float recoveryValue = celestialBody.scienceValues.RecoveryValue;
			num += ContractDefs.Grand.Funds.BaseAdvance * num8 * recoveryValue;
			num2 += ContractDefs.Grand.Funds.BaseReward * num8 * recoveryValue;
			num3 += ContractDefs.Grand.Funds.BaseFailure * num8 * recoveryValue;
			num4 += ContractDefs.Grand.Science.BaseReward * num8;
			num5 += ContractDefs.Grand.Reputation.BaseReward * num8;
			num6 += ContractDefs.Grand.Reputation.BaseFailure * num8;
			num7 += (float)ContractDefs.Grand.Expire.DeadlineDays * recoveryValue;
		}
		SetExpiry(ContractDefs.Grand.Expire.MinimumExpireDays, ContractDefs.Grand.Expire.MaximumExpireDays);
		SetDeadlineDays(num7);
		SetFunds(num, num2, num3);
		SetScience(num4);
		SetReputation(num5, num6);
		return true;
	}

	public void AttemptRandomTour(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.FLYBY, progressComplete: true, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb != Planetarium.fetch.Sun);
		List<CelestialBody> bodiesProgress2 = ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, progressComplete: true, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
		if (prestige == ContractPrestige.Trivial)
		{
			bodiesProgress2.Clear();
		}
		int num = 3;
		if (prestige == ContractPrestige.Significant)
		{
			num = 4;
		}
		else if (prestige == ContractPrestige.Exceptional)
		{
			num = 6;
		}
		if (bodiesProgress.Count < num && bodiesProgress2.Count < num)
		{
			return;
		}
		List<CelestialBody> list = new List<CelestialBody>();
		if (bodiesProgress2.Count >= num && bodiesProgress.Count < num)
		{
			progressType = ProgressType.LANDING;
			list.AddRange(bodiesProgress2);
		}
		if (bodiesProgress2.Count < num && bodiesProgress.Count >= num)
		{
			progressType = ProgressType.FLYBY;
			list.AddRange(bodiesProgress);
		}
		if (bodiesProgress2.Count >= num && bodiesProgress.Count >= num)
		{
			progressType = (SystemUtilities.CoinFlip(generator) ? ProgressType.FLYBY : ProgressType.LANDING);
			list.AddRange((progressType == ProgressType.LANDING) ? bodiesProgress2 : bodiesProgress);
		}
		for (int i = 0; i < num; i++)
		{
			CelestialBody item = WeightedBodyChoice(list, generator);
			list.Remove(item);
			if (!targetBodies.Contains(item))
			{
				targetBodies.Add(item);
			}
		}
	}

	public void AttemptFocusedTour(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		List<CelestialBody> focusedChallenges = GetFocusedChallenges(ProgressType.LANDING);
		List<CelestialBody> focusedChallenges2 = GetFocusedChallenges(ProgressType.FLYBY);
		if (prestige == ContractPrestige.Trivial)
		{
			focusedChallenges.Clear();
		}
		if (focusedChallenges.Count <= 0 && focusedChallenges2.Count <= 0)
		{
			return;
		}
		if (focusedChallenges.Count > 0 && focusedChallenges2.Count <= 0)
		{
			progressType = ProgressType.LANDING;
		}
		if (focusedChallenges.Count <= 0 && focusedChallenges2.Count > 0)
		{
			progressType = ProgressType.FLYBY;
		}
		if (focusedChallenges.Count > 0 && focusedChallenges2.Count > 0)
		{
			progressType = (SystemUtilities.CoinFlip(generator) ? ProgressType.FLYBY : ProgressType.LANDING);
		}
		if (progressType == ProgressType.FLYBY)
		{
			focused = true;
			focusBody = WeightedBodyChoice(focusedChallenges2, generator);
			targetBodies.Add(focusBody);
			targetBodies.AddRange(CelestialUtilities.ChildrenOf(focusBody));
		}
		if (progressType != ProgressType.LANDING)
		{
			return;
		}
		focused = true;
		focusBody = WeightedBodyChoice(focusedChallenges, generator);
		if (!CelestialUtilities.IsGasGiant(focusBody))
		{
			targetBodies.Add(focusBody);
		}
		List<CelestialBody> list = CelestialUtilities.ChildrenOf(focusBody);
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			CelestialBody celestialBody = list[i];
			if (!CelestialUtilities.IsGasGiant(celestialBody))
			{
				targetBodies.Add(celestialBody);
			}
		}
	}

	public List<CelestialBody> GetFocusedChallenges(ProgressType type)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		int count = FlightGlobals.Bodies.Count;
		for (int i = 0; i < count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (CelestialUtilities.ChildrenOf(celestialBody).Count > 1)
			{
				list.Add(celestialBody);
			}
		}
		List<CelestialBody> list2 = new List<CelestialBody>();
		ProgressMilestone progressMilestone = new ProgressMilestone(Planetarium.fetch.Home, type);
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			CelestialBody celestialBody = list[j];
			List<CelestialBody> list3 = CelestialUtilities.ChildrenOf(celestialBody);
			list3.Add(celestialBody);
			int num = 0;
			bool flag = true;
			int count2 = list3.Count;
			for (int k = 0; k < count2; k++)
			{
				progressMilestone.body = list3[k];
				if (progressMilestone.possible)
				{
					num++;
					if (!progressMilestone.complete)
					{
						flag = false;
						break;
					}
				}
			}
			if (num > 0 && flag)
			{
				list2.Add(celestialBody);
			}
		}
		return list2;
	}

	public void AttemptFinalDestination(KSPRandom generator = null)
	{
		finalDestination = false;
		finalBody = Planetarium.fetch.Home;
		finalSituation = Vessel.Situations.ORBITING;
		if (prestige == ContractPrestige.Trivial || targetBodies.Count < 2)
		{
			return;
		}
		if (generator == null)
		{
			generator = new KSPRandom(base.MissionSeed);
		}
		finalDestination = targetBodies.Contains(Planetarium.fetch.Home) || SystemUtilities.CoinFlip(generator);
		if (!finalDestination)
		{
			return;
		}
		if (focused)
		{
			finalBody = (targetBodies.Contains(Planetarium.fetch.Home) ? Planetarium.fetch.Home : targetBodies[generator.Next(0, targetBodies.Count)]);
			targetBodies.RemoveAll((CelestialBody body) => body == finalBody);
		}
		else
		{
			finalBody = targetBodies[generator.Next(0, targetBodies.Count)];
			targetBodies.RemoveAll((CelestialBody body) => body == finalBody);
			if (SystemUtilities.CoinFlip(generator))
			{
				finalBody = Planetarium.fetch.Home;
			}
		}
		List<Vessel.Situations> list = new List<Vessel.Situations>
		{
			Vessel.Situations.ORBITING,
			Vessel.Situations.SUB_ORBITAL
		};
		if (CelestialUtilities.IsFlyablePlanet(finalBody))
		{
			list.Add(Vessel.Situations.FLYING);
		}
		if (!CelestialUtilities.IsGasGiant(finalBody))
		{
			if (ProgressUtilities.GetBodyProgress(ProgressType.LANDING, finalBody))
			{
				list.Add(Vessel.Situations.LANDED);
			}
			if (finalBody.ocean && ProgressUtilities.GetBodyProgress(ProgressType.SPLASHDOWN, finalBody))
			{
				list.Add(Vessel.Situations.SPLASHED);
			}
		}
		if (finalBody == Planetarium.fetch.Home)
		{
			list.Clear();
			list.Add((progressType == ProgressType.LANDING) ? Vessel.Situations.LANDED : Vessel.Situations.SUB_ORBITAL);
		}
		finalSituation = list[generator.Next(0, list.Count)];
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(this).ToString(CultureInfo.InvariantCulture);
	}
}
