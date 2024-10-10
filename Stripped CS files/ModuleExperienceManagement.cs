using System.Collections.Generic;
using CommNet;
using ns9;
using UnityEngine;

public class ModuleExperienceManagement : PartModule
{
	[KSPField]
	public float costPerKerbal;

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().EnableKerbalExperience || HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().ImmediateLevelUp || HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			base.enabled = false;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_230278");
	}

	[KSPEvent(active = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001413")]
	public void FinalizeExperience()
	{
		if (CommNetScenario.CommNetEnabled && (base.vessel.connection == null || base.vessel.connection.Signal == SignalStrength.None))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230288"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		int num = 0;
		List<ProtoCrewMember> vesselCrew = base.vessel.GetVesselCrew();
		int count = vesselCrew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = vesselCrew[count];
			if (KerbalRoster.CalculateExperience(protoCrewMember.careerLog, protoCrewMember.flightLog) > protoCrewMember.experience)
			{
				if (costPerKerbal != 0f && !Funding.CanAfford(costPerKerbal))
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230323"), 5f, ScreenMessageStyle.UPPER_CENTER);
					break;
				}
				if (costPerKerbal > 0f)
				{
					Funding.Instance.AddFunds(0f - costPerKerbal, TransactionReasons.Vessels);
				}
				int experienceLevel = protoCrewMember.experienceLevel;
				protoCrewMember.ArchiveFlightLog();
				string text = "";
				if (protoCrewMember.experienceLevel > experienceLevel)
				{
					text = Localizer.Format("#autoLOC_6001016", protoCrewMember.nameWithGender, protoCrewMember.experienceLevel, protoCrewMember.experienceTrait.Title.ToLower());
				}
				string text2 = Localizer.Format("#autoLOC_7002001");
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001015", protoCrewMember.nameWithGender, text2, text), 5f, ScreenMessageStyle.UPPER_CENTER);
				num++;
				Debug.Log($"Archived flight log for {protoCrewMember.name}.");
			}
		}
		if (num == 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230331"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003041");
	}
}
