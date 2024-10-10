using System;
using CommNet;
using ns9;

[Serializable]
public class ExperimentResultDialogPage
{
	public string title;

	public string resultText;

	public float dataSize;

	public float refValue;

	public float scienceValueRatio;

	public float scienceValue;

	public float baseTransmitValue;

	public float TransmitBonus;

	public float remainingScience;

	public float valueAfterRecovery;

	public float valueAfterTransmit;

	public float xmitDataScalar;

	public bool showTransmitWarning;

	public string transmitWarningMessage;

	public bool showReset;

	public ScienceLabSearch labSearch;

	public float CommBonus;

	public Callback<ScienceData> OnDiscardData;

	public Callback<ScienceData> OnKeepData;

	public Callback<ScienceData> OnTransmitData;

	public Callback<ScienceData> OnSendToLab;

	public Part host;

	public ScienceData pageData;

	public static string[] sandboxResults = new string[6]
	{
		Localizer.Format("#autoLOC_457681"),
		Localizer.Format("#autoLOC_457682"),
		Localizer.Format("#autoLOC_457683"),
		Localizer.Format("#autoLOC_457684"),
		Localizer.Format("#autoLOC_457685"),
		Localizer.Format("#autoLOC_457686")
	};

	public ExperimentResultDialogPage(Part host, ScienceData experimentData, float xmitBase, float xmitBonus, bool showTransmitWarning, string transmitWarningMessage, bool showResetOption, ScienceLabSearch labSearch, Callback<ScienceData> onDiscardData, Callback<ScienceData> onKeepData, Callback<ScienceData> onTransmitData, Callback<ScienceData> onSendToLab)
	{
		this.host = host;
		pageData = experimentData;
		double num = 0.0;
		if (CommNetScenario.CommNetEnabled && host.vessel != null && host.vessel.connection != null)
		{
			CommNode comm = host.vessel.connection.Comm;
			float num2 = (float)host.vessel.connection.ControlPath.signalStrength;
			if (comm.scienceCurve != null)
			{
				num = comm.scienceCurve.Evaluate(num2);
			}
		}
		ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(experimentData.subjectID);
		if (subjectByID != null)
		{
			remainingScience = (subjectByID.scienceCap - subjectByID.science) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			refValue = ResearchAndDevelopment.GetReferenceDataValue(experimentData.dataAmount, subjectByID) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			scienceValue = ResearchAndDevelopment.GetScienceValue(experimentData.dataAmount, experimentData.scienceValueRatio, subjectByID) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			baseTransmitValue = ResearchAndDevelopment.GetScienceValue(experimentData.dataAmount, experimentData.scienceValueRatio, subjectByID, xmitBase) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			scienceValueRatio = experimentData.scienceValueRatio;
			if (scienceValue > 0f)
			{
				xmitBonus = (float)(1.0 + num);
				pageData.baseTransmitValue = xmitBase;
				if (baseTransmitValue * xmitBonus > scienceValue)
				{
					xmitBonus = scienceValue / baseTransmitValue;
				}
				pageData.transmitBonus = xmitBonus;
				if (xmitBase * xmitBonus > 0f && baseTransmitValue > 0f && refValue > 0f)
				{
					CommBonus = xmitBonus - 1f;
				}
				else
				{
					CommBonus = 0f;
				}
			}
			else
			{
				xmitBonus = 1f;
				pageData.baseTransmitValue = 0f;
				pageData.transmitBonus = 0f;
				CommBonus = 0f;
			}
			TransmitBonus = xmitBonus;
			valueAfterRecovery = ResearchAndDevelopment.GetNextScienceValue(experimentData.dataAmount, subjectByID) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			valueAfterTransmit = ResearchAndDevelopment.GetNextScienceValue(experimentData.dataAmount, subjectByID, xmitBase * xmitBonus) * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			resultText = experimentData.title + ":\n\n";
			resultText += ResearchAndDevelopment.GetResults(subjectByID.id);
			if (!string.IsNullOrEmpty(experimentData.extraResultString))
			{
				resultText = resultText + "\n\n" + Localizer.Format(experimentData.extraResultString);
			}
		}
		else
		{
			refValue = 1f;
			scienceValue = 0f;
			baseTransmitValue = 0f;
			TransmitBonus = 1f;
			valueAfterRecovery = 0f;
			valueAfterTransmit = 0f;
			scienceValueRatio = 1f;
			resultText = experimentData.title + ":\n\n";
			resultText += ResearchAndDevelopment.GetResults(experimentData.subjectID);
			resultText = resultText + "\n\n" + Localizer.Format(experimentData.extraResultString);
		}
		ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(experimentData.subjectID.Split('@')[0]);
		title = experiment.experimentTitle;
		dataSize = experimentData.dataAmount;
		xmitDataScalar = xmitBase;
		this.showTransmitWarning = showTransmitWarning;
		this.transmitWarningMessage = transmitWarningMessage;
		showReset = showResetOption;
		this.labSearch = labSearch;
		OnDiscardData = onDiscardData;
		OnKeepData = onKeepData;
		OnTransmitData = onTransmitData;
		OnSendToLab = onSendToLab;
	}

	public float UpdatePageLabValue()
	{
		pageData.labValue = 0f;
		if (FlightGlobals.ActiveVessel != null)
		{
			pageData.labValue = refValue;
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			ModuleScienceLab moduleScienceLab = activeVessel.FindPartModuleImplementing<ModuleScienceLab>();
			if (moduleScienceLab != null)
			{
				if (activeVessel.Landed)
				{
					pageData.labValue *= 1f + moduleScienceLab.SurfaceBonus;
				}
				if (pageData.subjectID.Contains(FlightGlobals.currentMainBody.bodyName))
				{
					pageData.labValue *= 1f + moduleScienceLab.ContextBonus;
				}
				if ((activeVessel.Landed || activeVessel.Splashed) && activeVessel.mainBody == FlightGlobals.GetHomeBody())
				{
					pageData.labValue *= moduleScienceLab.homeworldMultiplier;
				}
			}
			pageData.labValue = (float)Math.Round(pageData.labValue);
		}
		return pageData.labValue;
	}
}
