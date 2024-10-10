using System;
using System.Collections.Generic;
using ns9;

[Serializable]
public class ModuleResourceHandler
{
	public List<ModuleResource> inputResources = new List<ModuleResource>();

	public List<ModuleResource> outputResources = new List<ModuleResource>();

	public bool moduleResourceBasedPrimaryIsInput = true;

	public bool currentResourceLowerThanLayoff;

	public PartModule partModule;

	public virtual void OnAwake()
	{
		if (inputResources == null)
		{
			inputResources = new List<ModuleResource>();
		}
		if (outputResources == null)
		{
			outputResources = new List<ModuleResource>();
		}
	}

	public virtual void OnLoad(ConfigNode node)
	{
		bool flag = true;
		bool flag2 = true;
		int count = node.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			switch (configNode.name)
			{
			case "OUTPUT_RESOURCE":
				if (!string.IsNullOrEmpty(configNode.GetValue("name")))
				{
					if (flag2)
					{
						outputResources.Clear();
						flag2 = false;
					}
					ModuleResource moduleResource = new ModuleResource();
					moduleResource.Load(configNode);
					outputResources.Add(moduleResource);
				}
				break;
			case "INPUT_RESOURCE":
				if (!string.IsNullOrEmpty(configNode.GetValue("name")))
				{
					if (flag)
					{
						inputResources.Clear();
						flag = false;
					}
					ModuleResource moduleResource = new ModuleResource();
					moduleResource.Load(configNode);
					inputResources.Add(moduleResource);
				}
				break;
			case "RESOURCE":
				LoadResList(moduleResourceBasedPrimaryIsInput ? inputResources : outputResources, node);
				return;
			}
		}
	}

	public void SetPartModule(PartModule p)
	{
		partModule = p;
	}

	public void LoadResList(List<ModuleResource> list, ConfigNode node)
	{
		bool flag = true;
		int count = node.nodes.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (configNode.name == "RESOURCE")
			{
				if (flag)
				{
					list.Clear();
					flag = false;
				}
				ModuleResource moduleResource = new ModuleResource();
				moduleResource.Load(configNode);
				list.Add(moduleResource);
			}
		}
	}

	public virtual string PrintModuleResources(double mult = 1.0)
	{
		return PrintModuleResources(showFlowModeDesc: false, mult);
	}

	public virtual string PrintModuleResources(bool showFlowModeDesc, double mult = 1.0)
	{
		if (inputResources.Count > 0)
		{
			if (outputResources.Count > 0)
			{
				return PartModuleUtil.PrintResourceRequirements(showFlowModeDesc, Localizer.Format("#autoLOC_244332"), "orange", mult, inputResources.ToArray()) + PartModuleUtil.PrintResourceRequirements(showFlowModeDesc, Localizer.Format("#autoLOC_244333"), mult, outputResources.ToArray());
			}
			return PartModuleUtil.PrintResourceRequirements(showFlowModeDesc, Localizer.Format("#autoLOC_244335"), "orange", mult, inputResources.ToArray());
		}
		if (outputResources.Count > 0)
		{
			return PartModuleUtil.PrintResourceRequirements(showFlowModeDesc, Localizer.Format("#autoLOC_244339"), mult, outputResources.ToArray());
		}
		return string.Empty;
	}

	public virtual double GetAverageInput()
	{
		int count = inputResources.Count;
		if (count == 0)
		{
			return 0.0;
		}
		double num = 0.0;
		int index = count;
		while (index-- > 0)
		{
			num += inputResources[index].rate;
		}
		return num / (double)count;
	}

	public virtual double UpdateModuleResourceInputs(ref string error, double rateMultiplier, double threshold, bool returnOnFirstLack, bool average, bool stringOps = true)
	{
		return UpdateModuleResourceInputs(ref error, useFlowMode: false, rateMultiplier, threshold, returnOnFirstLack, average, stringOps);
	}

	public virtual double UpdateModuleResourceInputs(ref string error, bool useFlowMode, double rateMultiplier, double threshold, bool returnOnFirstLack, bool average, bool stringOps = true)
	{
		int count = inputResources.Count;
		if (count == 0)
		{
			return 1.0;
		}
		bool flag = true;
		double num = (average ? 0.0 : 1.0);
		double num2 = TimeWarp.fixedDeltaTime;
		int num3 = 0;
		ModuleResource moduleResource;
		double num4;
		while (true)
		{
			if (num3 < count)
			{
				moduleResource = inputResources[num3];
				num4 = moduleResource.rate * rateMultiplier;
				moduleResource.currentRequest = num4 * num2;
				if (CheatOptions.InfinitePropellant)
				{
					moduleResource.currentAmount = moduleResource.currentRequest;
					moduleResource.available = true;
					if (average)
					{
						num += 1.0;
					}
				}
				else
				{
					partModule.part.vessel.GetConnectedResourceTotals(moduleResource.id, out var amount, out var maxAmount);
					if (useFlowMode)
					{
						moduleResource.currentAmount = partModule.part.RequestResource(moduleResource.id, moduleResource.currentRequest, moduleResource.flowMode);
					}
					else
					{
						moduleResource.currentAmount = partModule.part.RequestResource(moduleResource.id, moduleResource.currentRequest);
					}
					if (moduleResource.shutOffHandler)
					{
						if (moduleResource.shutOffStartUpUsePercent)
						{
							double num5 = amount / maxAmount * 100.0;
							if (moduleResource.shutOffPercent >= (float)num5)
							{
								threshold = 1.1;
								currentResourceLowerThanLayoff = true;
								moduleResource.autoStateShifter.ToggleWarningVisibility(state: true);
								GameEvents.onPartResourceNonemptyEmpty.Fire(moduleResource.autoStateShifter.partResource);
							}
							else
							{
								threshold = Math.Max(threshold, 0.001);
								currentResourceLowerThanLayoff = false;
								moduleResource.autoStateShifter.ToggleWarningVisibility(state: false);
							}
						}
						else if ((double)moduleResource.shutOffAmount >= amount)
						{
							threshold = 1.1;
							currentResourceLowerThanLayoff = true;
							moduleResource.autoStateShifter.ToggleWarningVisibility(state: true);
							GameEvents.onPartResourceNonemptyEmpty.Fire(moduleResource.autoStateShifter.partResource);
						}
						else
						{
							threshold = Math.Max(threshold, 0.001);
							currentResourceLowerThanLayoff = false;
							moduleResource.autoStateShifter.ToggleWarningVisibility(state: false);
						}
					}
					else
					{
						threshold = Math.Max(threshold, 0.001);
					}
					if (moduleResource.currentAmount < moduleResource.currentRequest * threshold)
					{
						moduleResource.available = false;
						if (returnOnFirstLack)
						{
							break;
						}
						if (flag)
						{
							flag = false;
							if (stringOps)
							{
								error = Localizer.Format("#autoLOC_244419", KSPUtil.PrintLocalizedModuleName(moduleResource.name));
							}
							num = ((!moduleResource.shutOffHandler) ? ((!average) ? (moduleResource.currentAmount / moduleResource.currentRequest) : (num + moduleResource.currentAmount / moduleResource.currentRequest)) : ((!average) ? 0.0 : (num + amount / maxAmount)));
						}
						else
						{
							if (stringOps)
							{
								error = error + ", " + KSPUtil.PrintLocalizedModuleName(moduleResource.name);
							}
							num = ((!moduleResource.shutOffHandler) ? ((!average) ? (moduleResource.currentAmount / moduleResource.currentRequest) : (num + UtilMath.Clamp01(moduleResource.currentAmount / moduleResource.currentRequest))) : ((!average) ? 0.0 : (num + amount / maxAmount)));
						}
					}
					else
					{
						moduleResource.available = true;
						if (average)
						{
							num += 1.0;
						}
					}
				}
				num3++;
				continue;
			}
			if (average)
			{
				num /= (double)count;
			}
			return num;
		}
		if (stringOps)
		{
			error = Localizer.Format("#autoLOC_6001043", KSPUtil.PrintLocalizedModuleName(moduleResource.name), (moduleResource.currentAmount / num2).ToString("0.0"), num4.ToString("0.0"));
		}
		return 0.0;
	}

	public virtual bool UpdateModuleResourceInputs(ref string error, bool useFlowMode, double rateMultiplier, double threshold, bool returnOnFirstLack, bool stringOps = true)
	{
		return UpdateModuleResourceInputs(ref error, useFlowMode, rateMultiplier, threshold, returnOnFirstLack, average: false, stringOps) > 0.0;
	}

	public virtual bool UpdateModuleResourceInputs(ref string error, double rateMultiplier, double threshold, bool returnOnFirstLack, bool stringOps = true)
	{
		return UpdateModuleResourceInputs(ref error, rateMultiplier, threshold, returnOnFirstLack, average: false, stringOps) > 0.0;
	}

	public virtual double UpdateModuleResourceOutputs(double rateMultiplier = 1.0, double minAbsValue = 0.0)
	{
		int count = outputResources.Count;
		if ((double)count == 0.0)
		{
			return 0.0;
		}
		double num = TimeWarp.fixedDeltaTime;
		double num2 = 0.0;
		for (int i = 0; i < count; i++)
		{
			ModuleResource moduleResource = outputResources[i];
			double num3 = moduleResource.rate * rateMultiplier;
			moduleResource.currentRequest = num3 * num;
			if (moduleResource.currentRequest > minAbsValue)
			{
				moduleResource.currentAmount = Math.Abs(partModule.part.RequestResource(moduleResource.id, 0.0 - moduleResource.currentRequest));
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
		}
		return num2;
	}

	public bool HasEnoughResourcesToAutoStart()
	{
		int count = inputResources.Count;
		if (count == 0)
		{
			return false;
		}
		if (CheatOptions.InfinitePropellant)
		{
			return true;
		}
		_ = TimeWarp.fixedDeltaTime;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				ModuleResource moduleResource = inputResources[num];
				partModule.part.vessel.GetConnectedResourceTotals(moduleResource.id, out var amount, out var maxAmount);
				if (moduleResource.startUpHandler)
				{
					if (moduleResource.shutOffStartUpUsePercent)
					{
						double num2 = amount / maxAmount * 100.0;
						if (moduleResource.startUpPercent <= (float)num2)
						{
							return true;
						}
					}
					else if (!(amount < (double)moduleResource.startUpAmount))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return false;
		}
		return true;
	}

	public bool IsResourceBelowShutOffLimit(PartResource partRes)
	{
		int count = inputResources.Count;
		if (count == 0)
		{
			return false;
		}
		_ = TimeWarp.fixedDeltaTime;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				ModuleResource moduleResource = inputResources[num];
				if (moduleResource.shutOffHandler)
				{
					if (moduleResource.name.Equals(partRes.resourceName) && moduleResource.currentAmount > (double)moduleResource.shutOffAmount)
					{
						return true;
					}
				}
				else if (!(moduleResource.currentAmount < 0.009999999776482582))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
