using System;
using System.Collections.Generic;
using System.Text;
using ns9;
using UnityEngine;

public class ModuleCometDrill : BaseDrill
{
	[KSPField(isPersistant = true)]
	public bool DirectAttach;

	[KSPField]
	public float PowerConsumption = 1f;

	[KSPField]
	public bool RockOnly;

	public bool _isValidSituation = true;

	public Transform impactTransformCache;

	public ConversionRecipe recipe = new ConversionRecipe();

	public double _drilledMass;

	public string _status;

	public RaycastHit impactHitInfo;

	public Part _potato;

	public ModuleCometInfo _info;

	public static string cacheAutoLOC_6002530;

	public static string cacheAutoLOC_6002531;

	public static string cacheAutoLOC_258419;

	public static string cacheAutoLOC_258428;

	public static string cacheAutoLOC_258436;

	public static string cacheAutoLOC_258443;

	public static string cacheAutoLOC_258451;

	public static string cacheAutoLOC_258501;

	public override ConversionRecipe PrepareRecipe(double deltaTime)
	{
		_status = "Connected";
		_potato = GetAttachedPotato();
		_drilledMass = 0.0;
		if (_potato == null)
		{
			_status = cacheAutoLOC_6002530;
			IsActivated = false;
			return null;
		}
		if (DirectAttach && !base.part.children.Contains(_potato) && !(base.part.parent == _potato))
		{
			_status = cacheAutoLOC_6002531;
			IsActivated = false;
			return null;
		}
		if (!RockOnly && !_potato.Modules.Contains("ModuleCometResource"))
		{
			_status = cacheAutoLOC_258419;
			IsActivated = false;
			return null;
		}
		List<ModuleCometResource> list = _potato.FindModulesImplementing<ModuleCometResource>();
		if (!CheckForImpact())
		{
			_status = cacheAutoLOC_258428;
			IsActivated = false;
			return null;
		}
		_info = _potato.FindModuleImplementing<ModuleCometInfo>();
		if (_info == null)
		{
			_status = cacheAutoLOC_258436;
			IsActivated = false;
			return null;
		}
		if (_info.massThresholdVal >= _info.currentMassVal)
		{
			_status = cacheAutoLOC_258443;
			IsActivated = false;
			return null;
		}
		if (ResBroker.AmountAvailable(base.part, "ElectricCharge", deltaTime, ResourceFlowMode.NULL) <= deltaTime * (double)PowerConsumption)
		{
			_status = cacheAutoLOC_258451;
			IsActivated = false;
			return null;
		}
		UpdateConverterStatus();
		if (!IsActivated)
		{
			return null;
		}
		double efficiencyMultiplier = GetEfficiencyMultiplier();
		bool flag = false;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			ModuleCometResource moduleCometResource = list[i];
			double num = ResBroker.StorageAvailable(base.part, moduleCometResource.resourceName, deltaTime, moduleCometResource._flowMode, FillAmount);
			double num2 = deltaTime * (double)moduleCometResource.abundance * (double)Efficiency * efficiencyMultiplier;
			if (!(num <= num2))
			{
				flag = true;
				break;
			}
		}
		recipe.Clear();
		if (flag)
		{
			recipe.Inputs.Add(new ResourceRatio
			{
				ResourceName = "ElectricCharge",
				Ratio = PowerConsumption,
				FlowMode = ResourceFlowMode.NULL
			});
			for (int j = 0; j < count; j++)
			{
				ModuleCometResource moduleCometResource2 = list[j];
				if (!((double)moduleCometResource2.abundance <= 1E-09))
				{
					PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(moduleCometResource2.resourceName);
					double val = deltaTime * (double)moduleCometResource2.abundance * (double)Efficiency * efficiencyMultiplier;
					double val2 = (_info.currentMassVal - _info.massThresholdVal) / (double)definition.density;
					double num3 = Math.Min(val, val2);
					_drilledMass += (double)definition.density * num3;
					ResourceRatio resourceRatio = default(ResourceRatio);
					resourceRatio.ResourceName = moduleCometResource2.resourceName;
					resourceRatio.Ratio = num3 / deltaTime;
					resourceRatio.DumpExcess = true;
					resourceRatio.FlowMode = ResourceFlowMode.NULL;
					ResourceRatio item = resourceRatio;
					recipe.Outputs.Add(item);
				}
			}
		}
		else
		{
			_status = cacheAutoLOC_258501;
		}
		return recipe;
	}

	public virtual bool CheckForImpact()
	{
		if (!(ImpactTransform == string.Empty) && !(impactTransformCache == null))
		{
			Vector3 position = impactTransformCache.position;
			Ray ray = new Ray(position, impactTransformCache.forward);
			ModuleComet moduleComet = null;
			if (Physics.Raycast(ray, out impactHitInfo, ImpactRange))
			{
				moduleComet = impactHitInfo.collider.gameObject.GetComponentUpwards<ModuleComet>();
			}
			return moduleComet != null;
		}
		return true;
	}

	public virtual Part GetAttachedPotato()
	{
		ModuleComet moduleComet = null;
		int count = base.vessel.parts.Count;
		do
		{
			if (count-- > 0)
			{
				moduleComet = base.vessel.parts[count].FindModuleImplementing<ModuleComet>();
				continue;
			}
			return null;
		}
		while (!(moduleComet != null));
		return moduleComet.part;
	}

	public override bool IsSituationValid()
	{
		int count = base.vessel.parts.Count;
		while (count-- > 0)
		{
			Part part = base.vessel.parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				if (part.Modules[count2] is ModuleComet)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void OnUpdate()
	{
		bool isValidSituation;
		if ((isValidSituation = IsSituationValid()) != _isValidSituation)
		{
			_isValidSituation = isValidSituation;
			isEnabled = _isValidSituation;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
		if (!_isValidSituation && isEnabled)
		{
			isEnabled = false;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
		base.OnUpdate();
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsFlight)
		{
			_isValidSituation = IsSituationValid();
			isEnabled = _isValidSituation;
			_preCalculateEfficiency = true;
			impactTransformCache = base.part.FindModelTransform(ImpactTransform);
		}
	}

	public override string GetInfo()
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(ConverterName);
		stringBuilder.Append("\n");
		stringBuilder.Append(Localizer.Format("#autoLOC_6002532", (int)(Efficiency * 100f)));
		stringBuilder.Append(Localizer.Format("#autoLOC_258587"));
		stringBuilder.Append("\n - ");
		stringBuilder.Append(Localizer.Format("#autoLOC_501004"));
		stringBuilder.Append(": ");
		if ((double)(PowerConsumption * Efficiency) < 0.0001)
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_6001046", (PowerConsumption * (float)KSPUtil.dateTimeFormatter.Day * Efficiency).ToString("0.00")));
		}
		else if ((double)(PowerConsumption * Efficiency) < 0.01)
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_6001047", (PowerConsumption * (float)KSPUtil.dateTimeFormatter.Hour * Efficiency).ToString("0.00")));
		}
		else
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_6001048", (PowerConsumption * Efficiency).ToString("0.00")));
		}
		return stringBuilder.ToStringAndRelease();
	}

	public override void PostProcess(ConverterResults result, double deltaTime)
	{
		base.PostProcess(result, deltaTime);
		if (!(_drilledMass < 1E-09) && !(_potato == null) && !(_info == null))
		{
			double currentMassVal = _info.currentMassVal - _drilledMass;
			_info.currentMassVal = currentMassVal;
			status = _status;
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6002533");
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_6002530 = Localizer.Format("#autoLOC_6002530");
		cacheAutoLOC_6002531 = Localizer.Format("#autoLOC_6002531");
		cacheAutoLOC_258419 = Localizer.Format("#autoLOC_258419");
		cacheAutoLOC_258428 = Localizer.Format("#autoLOC_258428");
		cacheAutoLOC_258436 = Localizer.Format("#autoLOC_258436");
		cacheAutoLOC_258443 = Localizer.Format("#autoLOC_258443");
		cacheAutoLOC_258451 = Localizer.Format("#autoLOC_258451");
		cacheAutoLOC_258501 = Localizer.Format("#autoLOC_258501");
	}
}
