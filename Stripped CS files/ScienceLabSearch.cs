using ns9;
using UnityEngine;

public class ScienceLabSearch
{
	public enum SearchError
	{
		NONE,
		const_1,
		MIXED,
		NO_LABS,
		NO_MANNED,
		NO_SPACE,
		ALL_RESEARCHED,
		ALL_PROCESSING
	}

	public Vessel Vessel;

	public ScienceData Data;

	public ModuleScienceLab NextLabForData;

	public SearchError Error;

	public int LabsTotal;

	public int LabsManned;

	public int LabsWithSpace;

	public int LabsNotResearched;

	public int LabsNotProcessing;

	public bool HasAnyLabs => LabsTotal > 0;

	public bool HasMultipleLabs => LabsTotal > 1;

	public bool NextLabForDataFound => NextLabForData != null;

	public string ErrorString
	{
		get
		{
			switch (Error)
			{
			default:
				return Localizer.Format("#autoLOC_239591");
			case SearchError.MIXED:
				return Localizer.Format("#autoLOC_239579");
			case SearchError.NO_LABS:
				return Localizer.Format("#autoLOC_239581");
			case SearchError.NO_MANNED:
				if (!HasMultipleLabs)
				{
					return Localizer.Format("#autoLOC_6001034");
				}
				return Localizer.Format("#autoLOC_6001033");
			case SearchError.NO_SPACE:
				if (!HasMultipleLabs)
				{
					return Localizer.Format("#autoLOC_6001036");
				}
				return Localizer.Format("#autoLOC_6001035");
			case SearchError.ALL_RESEARCHED:
				if (!HasMultipleLabs)
				{
					return Localizer.Format("#autoLOC_6001038");
				}
				return Localizer.Format("#autoLOC_6001037");
			case SearchError.ALL_PROCESSING:
				if (!HasMultipleLabs)
				{
					return Localizer.Format("#autoLOC_6001032");
				}
				return Localizer.Format("#autoLOC_6001031");
			}
		}
	}

	public double ScienceExpectation
	{
		get
		{
			if (!(NextLabForData == null) && !(NextLabForData.Converter == null))
			{
				return NextLabForData.Converter.CalculateScienceAmount(Data.labValue);
			}
			return 0.0;
		}
	}

	public double TimeExpectation
	{
		get
		{
			if (!(NextLabForData == null) && !(NextLabForData.Converter == null))
			{
				return NextLabForData.Converter.CalculateResearchTime(Data.labValue);
			}
			return 0.0;
		}
	}

	public string DataExpectationSummary
	{
		get
		{
			if (!(NextLabForData == null) && !(NextLabForData.Converter == null))
			{
				return NextLabForData.Converter.DataExpectationSummary(Data.labValue);
			}
			return string.Empty;
		}
	}

	public ScienceLabSearch(Vessel vessel, ScienceData data)
	{
		Vessel = vessel;
		Data = data;
		if (!(vessel == null) && data != null && vessel.loaded)
		{
			ModuleScienceLab moduleScienceLab = null;
			int count = vessel.Parts.Count;
			while (count-- > 0)
			{
				Part part = vessel.Parts[count];
				int count2 = part.Modules.Count;
				while (count2-- > 0)
				{
					if (part.Modules[count2] is ModuleScienceLab)
					{
						ModuleScienceLab moduleScienceLab2 = part.Modules[count2] as ModuleScienceLab;
						LabsTotal++;
						bool num = moduleScienceLab2.IsOperational();
						bool flag = data.labValue + moduleScienceLab2.dataStored <= moduleScienceLab2.dataStorage;
						bool flag2 = !moduleScienceLab2.ExperimentData.Contains(data.subjectID);
						bool flag3 = !moduleScienceLab2.processingData;
						if (num)
						{
							LabsManned++;
						}
						if (flag)
						{
							LabsWithSpace++;
						}
						if (flag2)
						{
							LabsNotResearched++;
						}
						if (flag3)
						{
							LabsNotProcessing++;
						}
						if (num && flag && flag2 && flag3)
						{
							moduleScienceLab = moduleScienceLab2;
						}
					}
				}
			}
			if (moduleScienceLab != null)
			{
				NextLabForData = moduleScienceLab;
			}
			else if (LabsTotal <= 0)
			{
				Error = SearchError.NO_LABS;
			}
			else if (LabsNotProcessing <= 0)
			{
				Error = SearchError.ALL_PROCESSING;
			}
			else if (LabsManned <= 0)
			{
				Error = SearchError.NO_MANNED;
			}
			else if (LabsNotResearched <= 0)
			{
				Error = SearchError.ALL_RESEARCHED;
			}
			else if (LabsWithSpace <= 0)
			{
				Error = SearchError.NO_SPACE;
			}
			else
			{
				Error = SearchError.MIXED;
			}
		}
		else
		{
			Error = SearchError.const_1;
			Debug.LogError("A science lab data search has failed due to invalid arguments.");
		}
	}

	public void PostErrorToScreen()
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_239631", ErrorString.ToLowerInvariant()), 3f, ScreenMessageStyle.UPPER_CENTER);
	}
}
