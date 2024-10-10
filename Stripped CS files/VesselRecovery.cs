using System;
using System.Collections.Generic;
using ns11;
using ns19;
using ns9;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.EDITOR
})]
public class VesselRecovery : ScenarioModule
{
	public override void OnAwake()
	{
		GameEvents.onVesselRecovered.Add(OnVesselRecovered);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselRecovered.Remove(OnVesselRecovered);
	}

	public void Start()
	{
	}

	public void OnVesselRecovered(ProtoVessel pv, bool quick)
	{
		if (pv == null)
		{
			return;
		}
		CelestialBody cb = SpaceCenter.Instance.cb;
		double num = 0.0;
		double num2 = 1.0;
		if (cb != null)
		{
			num = SpaceCenter.Instance.GreatCircleDistance(cb.GetRelSurfaceNVector(pv.latitude, pv.longitude));
			num2 = cb.Radius * Math.PI;
		}
		else if (!quick)
		{
			Debug.LogWarning("[VesselRecovery]: No 'home' found. This should never happen in a build!");
		}
		ValueModifierQuery valueModifierQuery = ValueModifierQuery.RunQuery("RecoveryMinimumDelta", 1f);
		ValueModifierQuery valueModifierQuery2 = ValueModifierQuery.RunQuery("RecoveryMaximumDelta", 1f);
		float b = Math.Max(0f, Math.Min(1f, 0.1f + valueModifierQuery.GetEffectDelta()));
		float num3 = Mathf.Lerp(Math.Max(0f, Math.Min(1f, 0.98f + valueModifierQuery2.GetEffectDelta())), b, (float)(num / num2));
		float num4 = Mathf.Lerp(0.98f, 0.1f, (float)(num / num2));
		if (pv.landedAt.Contains("KSC"))
		{
			num3 = 0.98f;
			num4 = 0.98f;
		}
		if (pv.landedAt.Contains("Runway") || pv.landedAt.Contains("LaunchPad"))
		{
			num3 = 1f;
			num4 = 1f;
		}
		float recoveryFactor = 0f;
		float vanillaFactor = 0f;
		if (checkLaunchSites(pv, out recoveryFactor, out vanillaFactor))
		{
			num3 = recoveryFactor;
			num4 = vanillaFactor;
		}
		float num5 = num3 / num4 - 1f;
		string text = "";
		if (num5 != 0f)
		{
			text = ((num5 > 0f) ? "<color=#caff00>(+" : "<color=#feb200>(") + (num5 * 100f).ToString("N1") + "%)</color>";
		}
		if (Funding.Instance == null && Reputation.Instance == null && ResearchAndDevelopment.Instance == null && pv.vesselRef != null && pv.vesselRef.GetCrewCount() <= 0)
		{
			GameEvents.onVesselRecoveryProcessingComplete.Fire(pv, null, num3);
			return;
		}
		MissionRecoveryDialog missionRecoveryDialog = (quick ? null : MissionRecoveryDialog.CreateFullDialog(pv));
		if (!quick)
		{
			if (Funding.Instance != null)
			{
				missionRecoveryDialog.beforeMissionFunds = Funding.Instance.Funds;
			}
			if (Reputation.Instance != null)
			{
				missionRecoveryDialog.beforeMissionReputation = Reputation.Instance.reputation;
			}
			if (ResearchAndDevelopment.Instance != null)
			{
				missionRecoveryDialog.beforeMissionScience = ResearchAndDevelopment.Instance.Science;
			}
			if (!string.IsNullOrEmpty(pv.landedAt))
			{
				string empty = string.Empty;
				empty = ((!(pv.displaylandedAt == string.Empty)) ? Localizer.Format(pv.displaylandedAt) : ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(Vessel.GetLandedAtString(pv.landedAt), formatted: true));
				missionRecoveryDialog.recoveryLocation = Localizer.Format("#autoLOC_303271", empty);
				missionRecoveryDialog.recoveryFactor = KSPUtil.LocalizeNumber(num3 * 100f, "0.0") + "% " + text;
			}
			else
			{
				missionRecoveryDialog.recoveryLocation = Localizer.Format("#autoLOC_303276", (num * 0.0010000000474974513).ToString("0.0"));
				missionRecoveryDialog.recoveryFactor = KSPUtil.LocalizeNumber(num3 * 100f, "0.0") + "% " + text;
			}
			Debug.Log("[VesselRecovery]: " + pv.GetDisplayName() + " recovered " + missionRecoveryDialog.recoveryLocation + ". Recovery Value: " + missionRecoveryDialog.recoveryFactor);
		}
		recoverVesselCrew(pv, missionRecoveryDialog);
		GameEvents.onVesselRecoveryProcessing.Fire(pv, missionRecoveryDialog, num3);
		if (quick)
		{
			GameEvents.onVesselRecoveryProcessingComplete.Fire(pv, null, num3);
			return;
		}
		if (Funding.Instance != null)
		{
			missionRecoveryDialog.totalFunds = Funding.Instance.Funds;
		}
		if (Reputation.Instance != null)
		{
			missionRecoveryDialog.totalReputation = Reputation.CurrentRep;
		}
		if (ResearchAndDevelopment.Instance != null)
		{
			missionRecoveryDialog.totalScience = ResearchAndDevelopment.Instance.Science;
		}
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.VesselRecovery, (float)missionRecoveryDialog.fundsEarned, missionRecoveryDialog.scienceEarned, missionRecoveryDialog.reputationEarned);
		missionRecoveryDialog.fundsEarned += currencyModifierQuery.GetEffectDelta(Currency.Funds);
		missionRecoveryDialog.FundsModifier = currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N1", CurrencyModifierQuery.TextStyling.OnGUI);
		missionRecoveryDialog.reputationEarned += currencyModifierQuery.GetEffectDelta(Currency.Reputation);
		missionRecoveryDialog.RepModifier = currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N1", CurrencyModifierQuery.TextStyling.OnGUI);
		missionRecoveryDialog.scienceEarned += currencyModifierQuery.GetEffectDelta(Currency.Science);
		missionRecoveryDialog.ScienceModifier = currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N1", CurrencyModifierQuery.TextStyling.OnGUI);
		GameEvents.onVesselRecoveryProcessingComplete.Fire(pv, missionRecoveryDialog, num3);
	}

	public bool checkLaunchSites(ProtoVessel pv, out float recoveryFactor, out float vanillaFactor)
	{
		bool result = false;
		recoveryFactor = 0f;
		vanillaFactor = 0f;
		string text = MiniBiome.ConvertTagtoLandedAt(pv.landedAt);
		for (int i = 0; i < PSystemSetup.Instance.LaunchSites.Count; i++)
		{
			LaunchSite launchSite = PSystemSetup.Instance.LaunchSites[i];
			if (text.Contains(MiniBiome.ConvertTagtoLandedAt(launchSite.name)))
			{
				recoveryFactor = 1f;
				vanillaFactor = 1f;
				result = true;
				break;
			}
		}
		return result;
	}

	public void recoverVesselCrew(ProtoVessel pv, MissionRecoveryDialog mrDialog)
	{
		List<ProtoCrewMember> vesselCrew = pv.GetVesselCrew();
		int i = 0;
		for (int count = vesselCrew.Count; i < count; i++)
		{
			ProtoCrewMember protoCrewMember = vesselCrew[i];
			protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			protoCrewMember.flightLog.AddEntryUnique(FlightLog.EntryType.Recover);
			if (protoCrewMember.ChuteNode != null)
			{
				protoCrewMember.ChuteNode.SetValue("persistentState", "STOWED");
			}
			if (mrDialog != null)
			{
				Debug.Log("Crewmember " + protoCrewMember.name + " is available again");
				mrDialog.AddCrewWidget(CrewWidget.Create(protoCrewMember, mrDialog));
			}
			protoCrewMember.ArchiveFlightLog();
		}
	}
}
