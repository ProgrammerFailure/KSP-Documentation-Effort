using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class CometSampleContract : Contract
{
	public uint cometPersistentId;

	public string cometName;

	public string scienceSubjectId;

	public CometScienceParameter scienceParameter;

	public uint CometPersistentId => cometPersistentId;

	public string CometName => cometName;

	public string ScienceSubjectId => scienceSubjectId;

	public override bool Generate()
	{
		if (!ValidCometAvailable())
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		CometSampleContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<CometSampleContract>();
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
		if (num < ContractDefs.CometSample.MaximumAvailable && num2 < ContractDefs.CometSample.MaximumActive)
		{
			float num4 = 1f;
			AddParameter(new CrewTraitParameter("Scientist", 1, Localizer.Format("#autoLOC_8004217")));
			AddParameter(new CometScienceParameter(cometPersistentId, cometName, scienceSubjectId));
			AddKeywords("Scientific");
			Vessel vessel = null;
			FlightGlobals.FindVessel(cometPersistentId, out vessel);
			if (vessel != null)
			{
				if (vessel.orbit.ApR > 0.0 && vessel.orbit.ApR < FlightGlobals.GetHomeBody().orbit.semiMajorAxis * 15.0)
				{
					prestige = ContractPrestige.Significant;
				}
				else
				{
					prestige = ContractPrestige.Exceptional;
				}
			}
			SetExpiry(ContractDefs.CometSample.Expire.MinimumExpireDays, ContractDefs.CometSample.Expire.MaximumExpireDays);
			SetDeadlineDays((float)ContractDefs.CometSample.Expire.DeadlineDays * num4);
			switch (prestige)
			{
			default:
				SetScience(Mathf.RoundToInt(ContractDefs.CometSample.Science.BaseReward * num4 * ContractDefs.CometSample.Trivial.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseReward * num4 * ContractDefs.CometSample.Trivial.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseFailure * num4 * ContractDefs.CometSample.Trivial.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseAdvance * num4 * ContractDefs.CometSample.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseReward * num4 * ContractDefs.CometSample.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseFailure * num4 * ContractDefs.CometSample.Trivial.FundsMultiplier));
				break;
			case ContractPrestige.Exceptional:
				SetScience(Mathf.RoundToInt(ContractDefs.CometSample.Science.BaseReward * num4 * ContractDefs.CometSample.Exceptional.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseReward * num4 * ContractDefs.CometSample.Exceptional.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseFailure * num4 * ContractDefs.CometSample.Exceptional.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseAdvance * num4 * ContractDefs.CometSample.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseReward * num4 * ContractDefs.CometSample.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseFailure * num4 * ContractDefs.CometSample.Exceptional.FundsMultiplier));
				break;
			case ContractPrestige.Significant:
				SetScience(Mathf.RoundToInt(ContractDefs.CometSample.Science.BaseReward * num4 * ContractDefs.CometSample.Significant.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseReward * num4 * ContractDefs.CometSample.Significant.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Reputation.BaseFailure * num4 * ContractDefs.CometSample.Significant.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseAdvance * num4 * ContractDefs.CometSample.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseReward * num4 * ContractDefs.CometSample.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometSample.Funds.BaseFailure * num4 * ContractDefs.CometSample.Significant.FundsMultiplier));
				break;
			}
			return true;
		}
		return false;
	}

	public override void OnRegister()
	{
		GameEvents.onVesselPersistentIdChanged.Add(VesselIdChanged);
		GameEvents.onVesselRename.Add(OnVesselRename);
		GameEvents.OnDiscoverableObjectExpired.Add(OnDiscoverableObjectExpired);
		scienceParameter = GetParameter(typeof(CometScienceParameter)) as CometScienceParameter;
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselPersistentIdChanged.Remove(VesselIdChanged);
		GameEvents.onVesselRename.Remove(OnVesselRename);
		GameEvents.OnDiscoverableObjectExpired.Remove(OnDiscoverableObjectExpired);
	}

	public void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> hostedFromTo)
	{
		if (!(hostedFromTo.host == null))
		{
			if (scienceParameter == null)
			{
				scienceParameter = GetParameter(typeof(CometScienceParameter)) as CometScienceParameter;
			}
			if ((scienceParameter == null || !scienceParameter.ScienceDataCollected) && cometPersistentId == hostedFromTo.host.persistentId)
			{
				cometName = hostedFromTo.to;
				scienceSubjectId = "cometSample_" + hostedFromTo.host.Comet.typeName + "@SunInSpaceHigh_" + cometName;
			}
		}
	}

	public void OnDiscoverableObjectExpired(Vessel v)
	{
		if (scienceParameter == null)
		{
			scienceParameter = GetParameter(typeof(CometScienceParameter)) as CometScienceParameter;
		}
		if (scienceParameter == null || !scienceParameter.ScienceDataCollected)
		{
			FlightGlobals.FindVessel(cometPersistentId, out var vessel);
			if (v == vessel)
			{
				Fail();
			}
		}
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (cometPersistentId == oldId)
		{
			cometPersistentId = newId;
		}
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
		return Localizer.Format("#autoLOC_8012043", cometName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("CometSample", base.Agent.Title, cometName, Localizer.Format("#autoLOC_6002299"), base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		Vessel vessel = null;
		FlightGlobals.FindVessel(cometPersistentId, out vessel);
		if (vessel != null)
		{
			if (vessel.orbit.semiMajorAxis < 0.0)
			{
				return Localizer.Format("#autoLOC_8012044", cometName, Localizer.Format("#autoLOC_8012049"));
			}
			if (vessel.orbit.ApR < FlightGlobals.GetHomeBody().orbit.semiMajorAxis * 6.0)
			{
				return Localizer.Format("#autoLOC_8012044", cometName, Localizer.Format("#autoLOC_8012047"));
			}
			if (vessel.orbit.ApR > FlightGlobals.GetHomeBody().orbit.semiMajorAxis * 6.0 && vessel.orbit.ApR < FlightGlobals.GetHomeBody().orbit.semiMajorAxis * 15.0)
			{
				return Localizer.Format("#autoLOC_8012044", cometName, Localizer.Format("#autoLOC_8012048"));
			}
			if (vessel.orbit.ApR > FlightGlobals.GetHomeBody().orbit.semiMajorAxis * 15.0)
			{
				return Localizer.Format("#autoLOC_8012044", cometName, Localizer.Format("#autoLOC_8012049"));
			}
		}
		return Localizer.Format("#autoLOC_8012044", cometName);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_8012046", cometName);
	}

	public override void OnLoad(ConfigNode node)
	{
		node.TryGetValue("cometName", ref cometName);
		node.TryGetValue("cometPersistentId", ref cometPersistentId);
		node.TryGetValue("scienceSubjectId", ref scienceSubjectId);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("cometName", cometName);
		node.AddValue("cometPersistentId", cometPersistentId);
		node.AddValue("scienceSubjectId", scienceSubjectId);
	}

	public override bool MeetRequirements()
	{
		if (ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, bodyReached: true, progressComplete: true, MannedStatus.const_2, (CelestialBody cb) => cb != Planetarium.fetch.Home).Count <= 0)
		{
			return false;
		}
		return true;
	}

	public bool ValidCometAvailable()
	{
		List<Vessel> discoveredComets = CometManager.Instance.DiscoveredComets;
		if (discoveredComets.Count <= 0)
		{
			return false;
		}
		int index = Random.Range(0, discoveredComets.Count);
		CometSampleContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<CometSampleContract>();
		int num = 0;
		while (true)
		{
			if (num < currentContracts.Length)
			{
				if (currentContracts[num].CometPersistentId == discoveredComets[index].persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			CometSampleContract[] completedContracts = ContractSystem.Instance.GetCompletedContracts<CometSampleContract>();
			int num2 = 0;
			while (true)
			{
				if (num2 < completedContracts.Length)
				{
					if (completedContracts[num2].CometPersistentId == discoveredComets[index].persistentId)
					{
						break;
					}
					num2++;
					continue;
				}
				scienceSubjectId = "cometSample_" + discoveredComets[index].Comet.typeName + "@SunInSpaceHigh_" + discoveredComets[index].vesselName;
				ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(scienceSubjectId);
				if (subjectByID != null && subjectByID.science > 0f)
				{
					return false;
				}
				cometPersistentId = discoveredComets[index].persistentId;
				cometName = discoveredComets[index].vesselName;
				return true;
			}
			Debug.Log("Comet " + discoveredComets[index].name + "already completed.");
			return false;
		}
		Debug.Log("Comet " + discoveredComets[index].name + "already assigned.");
		return false;
	}
}
