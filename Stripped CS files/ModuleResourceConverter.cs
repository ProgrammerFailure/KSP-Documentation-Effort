using System;
using System.Text;
using ns9;
using UnityEngine;

public class ModuleResourceConverter : BaseConverter
{
	[KSPField]
	public bool ConvertByMass;

	public ConversionRecipe _recipe;

	public virtual ConversionRecipe Recipe => _recipe ?? (_recipe = LoadRecipe());

	public override ConversionRecipe PrepareRecipe(double deltatime)
	{
		UpdateConverterStatus();
		if (!IsActivated)
		{
			return null;
		}
		return Recipe;
	}

	public virtual ConversionRecipe LoadRecipe()
	{
		ConversionRecipe conversionRecipe = new ConversionRecipe();
		try
		{
			conversionRecipe.Inputs.AddRange(inputList);
			conversionRecipe.Outputs.AddRange(outputList);
			conversionRecipe.Requirements.AddRange(reqList);
			if (ConvertByMass)
			{
				ConvertRecipeToUnits(conversionRecipe);
			}
		}
		catch (Exception)
		{
			MonoBehaviour.print("[RESOURCES] Error creating recipe");
		}
		return conversionRecipe;
	}

	public override string GetInfo()
	{
		ConversionRecipe conversionRecipe = LoadRecipe();
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(ConverterName);
		stringBuilder.Append(Localizer.Format("#autoLOC_259572"));
		int count = conversionRecipe.Inputs.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceRatio resourceRatio = conversionRecipe.Inputs[i];
			stringBuilder.Append("\n - ");
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceRatio.ResourceName.GetHashCode());
			string text = "";
			text = ((definition == null) ? Localizer.Format(KSPUtil.PrintLocalizedModuleName(resourceRatio.ResourceName)) : definition.displayName.LocalizeRemoveGender());
			stringBuilder.Append(text);
			stringBuilder.Append(": ");
			if (resourceRatio.Ratio < 0.0001)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001057", (resourceRatio.Ratio * (double)KSPUtil.dateTimeFormatter.Day).ToString("0.00")));
			}
			else if (resourceRatio.Ratio < 0.01)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001058", (resourceRatio.Ratio * (double)KSPUtil.dateTimeFormatter.Hour).ToString("0.00")));
			}
			else
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001059", resourceRatio.Ratio.ToString("0.00")));
			}
		}
		stringBuilder.Append(Localizer.Format("#autoLOC_259594"));
		int count2 = conversionRecipe.Outputs.Count;
		for (int j = 0; j < count2; j++)
		{
			ResourceRatio resourceRatio2 = conversionRecipe.Outputs[j];
			stringBuilder.Append("\n - ");
			PartResourceDefinition definition2 = PartResourceLibrary.Instance.GetDefinition(resourceRatio2.ResourceName.GetHashCode());
			string text2 = "";
			text2 = ((definition2 == null) ? Localizer.Format(KSPUtil.PrintLocalizedModuleName(resourceRatio2.ResourceName)) : definition2.displayName.LocalizeRemoveGender());
			stringBuilder.Append(text2);
			stringBuilder.Append(": ");
			if (resourceRatio2.Ratio < 0.0001)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001060", (resourceRatio2.Ratio * (double)KSPUtil.dateTimeFormatter.Day).ToString("0.00")));
			}
			else if (resourceRatio2.Ratio < 0.01)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001061", (resourceRatio2.Ratio * (double)KSPUtil.dateTimeFormatter.Hour).ToString("0.00")));
			}
			else
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001062", resourceRatio2.Ratio.ToString("0.00")));
			}
		}
		int count3 = conversionRecipe.Requirements.Count;
		if (count3 > 0)
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_259620"));
			for (int k = 0; k < count3; k++)
			{
				ResourceRatio resourceRatio3 = conversionRecipe.Requirements[k];
				stringBuilder.Append("\n - ");
				PartResourceDefinition definition3 = PartResourceLibrary.Instance.GetDefinition(resourceRatio3.ResourceName.GetHashCode());
				string text3 = "";
				text3 = ((definition3 == null) ? Localizer.Format(KSPUtil.PrintLocalizedModuleName(resourceRatio3.ResourceName)) : definition3.displayName.LocalizeRemoveGender());
				stringBuilder.Append(text3);
				stringBuilder.Append(": ");
				stringBuilder.AppendFormat("{0:0.00}", resourceRatio3.Ratio);
			}
			stringBuilder.Append("\n");
		}
		return stringBuilder.ToStringAndRelease();
	}

	public override bool IsSituationValid()
	{
		return true;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003053");
	}
}
