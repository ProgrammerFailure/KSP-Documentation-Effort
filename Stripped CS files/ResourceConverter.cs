using System;
using ns9;

public class ResourceConverter
{
	public readonly IResourceBroker _broker;

	public static string cacheAutoLOC_6002353;

	public static string cacheAutoLOC_261274;

	public ResourceConverter(IResourceBroker broker)
	{
		_broker = broker;
	}

	public ResourceConverter()
		: this(new ResourceBroker())
	{
	}

	public ConverterResults ProcessRecipe(double deltaTime, ConversionRecipe recipe, Part resPart, PartModule resModule, float efficiencyBonus)
	{
		ConverterResults result = default(ConverterResults);
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
		{
			result.Status = cacheAutoLOC_6002353;
		}
		double num = 1.0;
		int count = recipe.Requirements.Count;
		int num2 = 0;
		ResourceRatio resourceRatio;
		while (true)
		{
			if (num2 < count)
			{
				resourceRatio = recipe.Requirements[num2];
				double num3 = resPart.Resources[resourceRatio.ResourceName].amount / Math.Abs(resourceRatio.Ratio);
				if (resourceRatio.Ratio < 0.0)
				{
					num3 = 1.0 - num3;
				}
				if (num3 < num)
				{
					num = num3;
				}
				if (!(num > 1E-09))
				{
					break;
				}
				num2++;
				continue;
			}
			num *= (double)efficiencyBonus;
			if (num <= 1E-09)
			{
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
				{
					result.Status = cacheAutoLOC_261274;
				}
				result.TimeFactor = 0.0;
				return result;
			}
			double num4 = deltaTime;
			if (!CheatOptions.InfinitePropellant)
			{
				int count2 = recipe.Inputs.Count;
				for (int i = 0; i < count2; i++)
				{
					ResourceRatio resourceRatio2 = recipe.Inputs[i];
					if (CheatOptions.InfiniteElectricity && resourceRatio2.ResourceName == "ElectricCharge")
					{
						continue;
					}
					double num5 = _broker.AmountAvailable(resPart, resourceRatio2.ResourceName, deltaTime, resourceRatio2.FlowMode) * (double)recipe.TakeAmount;
					double num6 = resourceRatio2.Ratio * num4 * num;
					if (!(num5 < num6))
					{
						continue;
					}
					num4 *= num5 / num6;
					if (!(num4 > 1E-09))
					{
						PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceRatio2.ResourceName);
						if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
						{
							result.Status = Localizer.Format("#autoLOC_261304", (definition != null) ? definition.displayName : resourceRatio2.ResourceName);
						}
						result.TimeFactor = 0.0;
						return result;
					}
				}
			}
			int count3 = recipe.Outputs.Count;
			int num7 = 0;
			ResourceRatio resourceRatio3;
			while (true)
			{
				if (num7 < count3)
				{
					resourceRatio3 = recipe.Outputs[num7];
					if (!resourceRatio3.DumpExcess)
					{
						double num8 = _broker.StorageAvailable(resPart, resourceRatio3.ResourceName, deltaTime, resourceRatio3.FlowMode, recipe.FillAmount);
						double num9 = resourceRatio3.Ratio * num4 * num;
						if (num8 < num9)
						{
							num4 *= num8 / num9;
							if (!(num4 > 1E-09))
							{
								break;
							}
						}
					}
					num7++;
					continue;
				}
				int count4 = recipe.Inputs.Count;
				for (int j = 0; j < count4; j++)
				{
					ResourceRatio resourceRatio4 = recipe.Inputs[j];
					double resAmount = resourceRatio4.Ratio * num4 * num;
					_broker.RequestResource(resPart, resourceRatio4.ResourceName, resAmount, deltaTime, resourceRatio4.FlowMode);
				}
				int count5 = recipe.Outputs.Count;
				for (int k = 0; k < count5; k++)
				{
					ResourceRatio resourceRatio5 = recipe.Outputs[k];
					double num10 = resourceRatio5.Ratio * num4 * num;
					_broker.StoreResource(resPart, resourceRatio5.ResourceName, num10, deltaTime, resourceRatio5.FlowMode);
					GameEvents.OnResourceConverterOutput.Fire(resModule, resourceRatio5.ResourceName, num10);
				}
				result.TimeFactor = num4 * num;
				return result;
			}
			if (recipe.FillAmount < 1f)
			{
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
				{
					result.Status = Localizer.Format("#autoLOC_261332", recipe.FillAmount.ToString("0%"));
				}
			}
			else
			{
				PartResourceDefinition definition2 = PartResourceLibrary.Instance.GetDefinition(resourceRatio3.ResourceName);
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
				{
					result.Status = Localizer.Format("#autoLOC_261334", (definition2 != null) ? definition2.displayName : resourceRatio3.ResourceName);
				}
			}
			result.TimeFactor = 0.0;
			return result;
		}
		PartResourceDefinition definition3 = PartResourceLibrary.Instance.GetDefinition(resourceRatio.ResourceName);
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(resPart, includeSymmetryCounterparts: false))
		{
			result.Status = Localizer.Format("#autoLOC_261263", (definition3 != null) ? definition3.displayName : resourceRatio.ResourceName);
		}
		result.TimeFactor = 0.0;
		return result;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002353 = Localizer.Format("#autoLOC_6002353");
		cacheAutoLOC_261274 = Localizer.Format("#autoLOC_261274");
	}
}
