using System;
using System.Globalization;
using Experience.Effects;
using ns9;

public class ModuleScienceConverter : BaseConverter
{
	[KSPField(guiActive = true, guiName = "#autoLOC_6001432")]
	public string sciString = "";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001433")]
	public string datString = "";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001434")]
	public string rateString = "";

	[KSPField]
	public float scientistBonus = 0.25f;

	[KSPField]
	public int researchTime = 8;

	[KSPField]
	public int scienceMultiplier = 5;

	[KSPField]
	public float dataProcessingMultiplier = 0.5f;

	[KSPField]
	public int scienceCap = 500;

	[KSPField]
	public float powerRequirement = 5f;

	public ModuleScienceLab _lab;

	public static string cacheAutoLOC_237911;

	public static string cacheAutoLOC_237928;

	public static string cacheAutoLOC_237931;

	public virtual ModuleScienceLab Lab => _lab ?? (_lab = base.part.FindModuleImplementing<ModuleScienceLab>());

	public override void OnStart(StartState state)
	{
		base.Fields["status"].guiName = "#autoLOC_6001435";
		base.OnStart(state);
	}

	public virtual string DataExpectationSummary(float dataAmount)
	{
		string text = Localizer.Format("#autoLOC_6001023", dataAmount.ToString("f0"), CalculateScienceAmount(dataAmount).ToString("f0"));
		string text2 = "n/a";
		double time = 0.0 - Math.Round(CalculateResearchTime(dataAmount + Lab.dataStored));
		if (dataAmount + Lab.dataStored > 0f)
		{
			text2 = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(KSPUtil.PrintDateDelta(time, includeTime: false, includeSeconds: false, useAbs: true).ToLowerInvariant());
		}
		return Localizer.Format("#autoLOC_6001024", text, Lab.dataStored + dataAmount, text2);
	}

	public virtual double CalculateScienceAmount(float dataAmount)
	{
		return dataAmount * (float)scienceMultiplier;
	}

	public virtual double CalculateScienceRate(float dataAmount)
	{
		float scientists = GetScientists();
		return (double)((float)KSPUtil.dateTimeFormatter.Day * scientists * dataAmount * dataProcessingMultiplier * (float)scienceMultiplier) / Math.Pow(10.0, researchTime);
	}

	public virtual double CalculateResearchTime(float dataAmount)
	{
		double num = CalculateScienceRate(Lab.dataStored + dataAmount);
		return (double)KSPUtil.dateTimeFormatter.Day * ((double)dataAmount / num);
	}

	public virtual void UpdateDisplayStrings()
	{
		sciString = $"{Lab.storedScience:0.000}/{scienceCap:0}";
		datString = $"{Lab.dataStored:0.000}/{Lab.dataStorage:0}";
		rateString = Localizer.Format("#autoLOC_6001025", CalculateScienceRate(Lab.dataStored).ToString("0.0000"));
	}

	public override bool IsSituationValid()
	{
		return true;
	}

	public override ConversionRecipe PrepareRecipe(double deltatime)
	{
		return new ConversionRecipe
		{
			Inputs = 
			{
				new ResourceRatio
				{
					FlowMode = ResourceFlowMode.ALL_VESSEL,
					Ratio = powerRequirement,
					ResourceName = "ElectricCharge",
					DumpExcess = true
				}
			}
		};
	}

	public override void PreProcessing()
	{
		if (UIPartActionController.Instance != null && base.part != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
		{
			UpdateDisplayStrings();
		}
		base.PostUpdateCleanup();
	}

	public override void PostProcess(ConverterResults result, double deltaTime)
	{
		if ((double)Lab.dataStored < 1E-09)
		{
			Lab.dataStored = 0f;
		}
		if (Lab.dataStored > Lab.dataStorage)
		{
			Lab.dataStored = Lab.dataStorage;
		}
		if (Lab.storedScience > (float)scienceCap)
		{
			Lab.storedScience = scienceCap;
		}
		float scientists = GetScientists();
		if ((double)scientists < 1E-09)
		{
			if (IsActivated)
			{
				StopResourceConverter();
				UpdateConverterStatus();
				if (base.vessel != null && FlightGlobals.ActiveVessel == base.vessel)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237902", base.part.partInfo.title) + "</color>", 5f);
				}
			}
			return;
		}
		double num = result.TimeFactor / deltaTime;
		if (num < 1E-09)
		{
			status = cacheAutoLOC_237911;
			return;
		}
		double num2 = deltaTime * num * (double)dataProcessingMultiplier * (double)scientists * (double)Lab.dataStored / Math.Pow(10.0, researchTime);
		double num3 = (double)scienceMultiplier * num2;
		if (num3 + (double)Lab.storedScience > (double)scienceCap)
		{
			status = cacheAutoLOC_237928;
			return;
		}
		status = cacheAutoLOC_237931;
		Lab.dataStored -= (float)num2;
		Lab.storedScience += (float)num3;
	}

	public virtual float GetScientists()
	{
		float num = 0f;
		int count = base.part.protoModuleCrew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[count];
			if (protoCrewMember.HasEffect<ScienceSkill>())
			{
				num += 1f + scientistBonus * (float)protoCrewMember.experienceLevel;
			}
		}
		return num;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_237911 = Localizer.Format("#autoLOC_237911");
		cacheAutoLOC_237928 = Localizer.Format("#autoLOC_237928");
		cacheAutoLOC_237931 = Localizer.Format("#autoLOC_237931");
	}
}
