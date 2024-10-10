using System;
using System.Collections.Generic;
using ns9;

public class ModuleOrbitalSurveyor : PartModule, IAnimatedModule
{
	[KSPField]
	public double minThreshold = 25000.0;

	[KSPField]
	public double maxThreshold = 1500000.0;

	[KSPField]
	public bool useCustomThresholds;

	[KSPField]
	public int ScanTime = 5;

	[KSPField]
	public int SciBonus = 10;

	public BaseEvent perfSurv;

	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001486")]
	public virtual void PerformSurvey()
	{
		if (base.vessel.mainBody.Radius < minThreshold && useCustomThresholds)
		{
			rescaleThresholds();
		}
		double num = Math.Max(base.vessel.mainBody.Radius / 10.0, minThreshold);
		double num2 = Math.Min(base.vessel.mainBody.Radius * 5.0, maxThreshold);
		if (base.vessel.orbit.PeA > num && base.vessel.orbit.ApA < num2 && base.vessel.situation == Vessel.Situations.ORBITING && base.vessel.orbit.inclination >= 80.0)
		{
			sendDataToComms();
			return;
		}
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001056", (num / 1000.0).ToString("0"), (num2 / 1000.0).ToString("0")), 5f, ScreenMessageStyle.UPPER_CENTER);
	}

	public override void OnStart(StartState state)
	{
		perfSurv = base.Events["PerformSurvey"];
		if (HighLogic.LoadedSceneIsFlight && base.vessel.mainBody.Radius < minThreshold && useCustomThresholds)
		{
			rescaleThresholds();
		}
		GameEvents.OnTriggeredDataTransmission.Add(CompleteSurvey);
	}

	public void rescaleThresholds()
	{
		double num = base.vessel.mainBody.sphereOfInfluence - base.vessel.mainBody.Radius;
		double num2 = 1.0;
		if (maxThreshold > num)
		{
			num2 = Math.Sqrt(num / maxThreshold);
			maxThreshold = num;
		}
		if (minThreshold > maxThreshold)
		{
			minThreshold = maxThreshold;
		}
		minThreshold = Math.Max(3000.0, minThreshold * num2);
		double num3 = 0.95 * maxThreshold;
		if (minThreshold > num3)
		{
			minThreshold = num3;
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.OnTriggeredDataTransmission.Remove(CompleteSurvey);
	}

	public virtual void sendDataToComms()
	{
		IScienceDataTransmitter bestTransmitter = ScienceUtil.GetBestTransmitter(base.vessel);
		if (bestTransmitter != null)
		{
			ScienceData item = new ScienceData((float)base.vessel.mainBody.Radius * 0.001f / (float)ScanTime, 1f, 0f, "survey@" + base.vessel.mainBody.bodyName, Localizer.Format("#autoLOC_259338"), triggered: true, base.part.flightID);
			List<ScienceData> list = new List<ScienceData>();
			list.Add(item);
			bestTransmitter.TransmitData(list);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_259348"), 3f, ScreenMessageStyle.UPPER_CENTER);
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}

	public virtual void CompleteSurvey(ScienceData data, Vessel origin, bool xmitAborted)
	{
		if (!(data.container != base.part.flightID || !data.subjectID.StartsWith("survey@") || xmitAborted))
		{
			CelestialBody mainBody = base.vessel.mainBody;
			ScreenMessage screenMessage = new ScreenMessage("", 8f, ScreenMessageStyle.UPPER_LEFT);
			screenMessage.message = Localizer.Format("#autoLOC_259361", mainBody.displayName);
			if (ResearchAndDevelopment.Instance != null)
			{
				float num = (float)SciBonus * base.vessel.mainBody.scienceValues.RecoveryValue;
				screenMessage.message += ResearchAndDevelopment.ScienceTransmissionRewardString(num);
				ResearchAndDevelopment.Instance.AddScience(num, TransactionReasons.ScienceTransmission);
			}
			screenMessage.message += "</color>";
			ScreenMessages.PostScreenMessage(screenMessage);
			ResourceMap.Instance.UnlockPlanet(base.vessel.mainBody.flightGlobalsIndex);
			base.Events["PerformSurvey"].active = false;
			OverlayGenerator.Instance.DisplayBody = mainBody;
			OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
			MonoUtilities.RefreshPartContextWindow(base.part);
			GameEvents.OnOrbitalSurveyCompleted.Fire(base.vessel, mainBody);
		}
	}

	public virtual void CheckForScanData()
	{
		bool active;
		if (perfSurv.active != (active = !ResourceMap.Instance.IsPlanetScanned(base.vessel.mainBody.flightGlobalsIndex)))
		{
			perfSurv.active = active;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}

	public virtual void Update()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			CheckForScanData();
		}
	}

	public virtual void EnableModule()
	{
		isEnabled = true;
	}

	public virtual void DisableModule()
	{
		isEnabled = false;
	}

	public virtual bool ModuleIsActive()
	{
		return isEnabled;
	}

	public virtual bool IsSituationValid()
	{
		return true;
	}

	public override string GetInfo()
	{
		return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("" + Localizer.Format("#autoLOC_259425"), Localizer.Format("#autoLOC_259426")), Localizer.Format("#autoLOC_259427", ScanTime)), Localizer.Format("#autoLOC_259428", XKCDColors.HexFormat.Cyan, SciBonus)), Localizer.Format("#autoLOC_259429", minThreshold)), Localizer.Format("#autoLOC_259430", XKCDColors.HexFormat.KSPUIGrey)), Localizer.Format("#autoLOC_259431", maxThreshold)), Localizer.Format("#autoLOC_259432", XKCDColors.HexFormat.KSPUIGrey));
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003047");
	}
}
