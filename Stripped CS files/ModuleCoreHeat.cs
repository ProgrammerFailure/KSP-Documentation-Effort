using System;
using System.Collections.Generic;
using System.Text;
using ns9;
using UnityEngine;

public class ModuleCoreHeat : PartModule
{
	[KSPField]
	public double CoreTempGoal = 500.0;

	[KSPField]
	public double CoreShutdownTemp = 1000.0;

	[KSPField(isPersistant = true)]
	public double CoreTempGoalAdjustment;

	[KSPField(isPersistant = true)]
	public double CoreThermalEnergy;

	[KSPField]
	public double HeatRadiantMultiplier = 0.1;

	[KSPField]
	public double CoolingRadiantMultiplier = 0.1;

	[KSPField]
	public double HeatTransferMultiplier = 0.1;

	[KSPField]
	public double CoolantTransferMultiplier = 0.1;

	[KSPField]
	public FloatCurve PassiveEnergy;

	[KSPField]
	public double CoreEnergyMultiplier = 0.1;

	[KSPField]
	public double radiatorCoolingFactor = 1.0;

	[KSPField]
	public double radiatorHeatingFactor = 0.1;

	[KSPField]
	public double MaxCalculationWarp = 1000.0;

	[KSPField]
	public double CoreToPartRatio = 0.1;

	[KSPField]
	public double MaxCoolant = 10000.0;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001813")]
	public string D_CTE = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001814")]
	public string D_PTE = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001815")]
	public string D_GE = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001816")]
	public string D_EDiff = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001817")]
	public string D_partXfer = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001818")]
	public string D_coreXfer = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001819")]
	public string D_RadSat = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001820")]
	public string D_RadCap = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001821")]
	public string D_TRU = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001822")]
	public string D_RCA = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001823")]
	public string D_CoolPercent = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001824")]
	public string D_CoolAmt = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001825")]
	public string D_POT = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001826")]
	public string D_XTP = "???";

	[KSPField(guiActive = true, guiName = "#autoLOC_6001827")]
	public string D_Excess = "???";

	public double lastFlux;

	public double lastUpdateTime;

	public BaseField Dfld_CTE;

	public BaseField Dfld_PTE;

	public BaseField Dfld_GE;

	public BaseField Dfld_EDiff;

	public BaseField Dfld_partXfer;

	public BaseField Dfld_coreXfer;

	public BaseField Dfld_RadSat;

	public BaseField Dfld_RadCap;

	public BaseField Dfld_TRU;

	public BaseField Dfld_RCA;

	public BaseField Dfld_CoolPercent;

	public BaseField Dfld_CoolAmt;

	public BaseField Dfld_POT;

	public BaseField Dfld_XTP;

	public BaseField Dfld_Excess;

	public List<Part> vRadList = new List<Part>();

	public List<Part> pRadList = new List<Part>();

	public List<BaseConverter> converterCache;

	public double energyTemp;

	public double lastCoreTemp;

	public List<Part> activeRadiatorParts = new List<Part>();

	public List<Part> coreHeatParts = new List<Part>();

	public bool partCachesDirty = true;

	public virtual double CoreTemperature
	{
		get
		{
			if (double.IsNaN(CoreThermalEnergy))
			{
				CheckStartingTemperature();
			}
			return CoreThermalEnergy / base.part.thermalMass;
		}
	}

	public virtual void Start()
	{
		Dfld_CTE = base.Fields["D_CTE"];
		Dfld_PTE = base.Fields["D_PTE"];
		Dfld_GE = base.Fields["D_GE"];
		Dfld_EDiff = base.Fields["D_EDiff"];
		Dfld_partXfer = base.Fields["D_partXfer"];
		Dfld_coreXfer = base.Fields["D_coreXfer"];
		Dfld_RadSat = base.Fields["D_RadSat"];
		Dfld_RadCap = base.Fields["D_RadCap"];
		Dfld_TRU = base.Fields["D_TRU"];
		Dfld_RCA = base.Fields["D_RCA"];
		Dfld_CoolPercent = base.Fields["D_CoolPercent"];
		Dfld_CoolAmt = base.Fields["D_CoolAmt"];
		Dfld_POT = base.Fields["D_POT"];
		Dfld_XTP = base.Fields["D_XTP"];
		Dfld_Excess = base.Fields["D_Excess"];
		UpdateConverterModuleCache();
	}

	public virtual void CheckDebugFields()
	{
		if (Dfld_CTE != null)
		{
			Dfld_CTE.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_PTE != null)
		{
			Dfld_PTE.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_GE != null)
		{
			Dfld_GE.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_EDiff != null)
		{
			Dfld_EDiff.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_partXfer != null)
		{
			Dfld_partXfer.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_coreXfer != null)
		{
			Dfld_coreXfer.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_RadSat != null)
		{
			Dfld_RadSat.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_RadCap != null)
		{
			Dfld_RadCap.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_TRU != null)
		{
			Dfld_TRU.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_RCA != null)
		{
			Dfld_RCA.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_CoolPercent != null)
		{
			Dfld_CoolPercent.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_CoolAmt != null)
		{
			Dfld_CoolAmt.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_POT != null)
		{
			Dfld_POT.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_XTP != null)
		{
			Dfld_XTP.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_Excess != null)
		{
			Dfld_Excess.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
	}

	public virtual void AddEnergyToCore(double energy)
	{
		energyTemp += energy;
	}

	public virtual double GetLastFlux()
	{
		return lastFlux;
	}

	public virtual void CheckCoreShutdown()
	{
		if (CoreTemperature <= CoreShutdownTemp)
		{
			return;
		}
		List<BaseConverter> list = base.part.FindModulesImplementing<BaseConverter>();
		int count = list.Count;
		while (count-- > 0)
		{
			if (!list[count].AutoShutdown)
			{
				ScreenMessages.PostScreenMessage(string.Format(Localizer.Format("#autoLOC_204715")), 5f, ScreenMessageStyle.UPPER_CENTER);
				list[count].StopResourceConverter();
			}
		}
	}

	public virtual void FixedUpdate()
	{
		double deltaTime = GetDeltaTime();
		CheckDebugFields();
		if (!HighLogic.LoadedSceneIsFlight || base.vessel == null)
		{
			return;
		}
		CheckPartCaches();
		CheckCoreShutdown();
		CheckStartingTemperature();
		if (!(deltaTime < 1E-09))
		{
			AddEnergyToCore((double)PassiveEnergy.Evaluate((float)CoreTemperature) * deltaTime);
			double num = base.part.thermalMass * base.part.temperature;
			double num2 = base.part.thermalMass * (CoreTempGoal + CoreTempGoalAdjustment);
			double num3 = 0.0;
			double num4 = 0.0;
			double energy = 0.0;
			if (CoreThermalEnergy > num)
			{
				energy = (CoreThermalEnergy - num) * CoreEnergyMultiplier * CoreToPartRatio;
				num3 = GetTransferAmount(energy, HeatRadiantMultiplier);
				double num5 = CoreThermalEnergy * CoreEnergyMultiplier * CoreToPartRatio * CoolantTransferMultiplier;
				double val = CoreThermalEnergy - num2;
				num4 = ((!HasActiveConverters()) ? (0.0 - num5) : (0.0 - Math.Min(num5, val)));
			}
			else if (CoreThermalEnergy < num)
			{
				energy = (num - CoreThermalEnergy) * CoreEnergyMultiplier * CoreToPartRatio;
				num3 = 0.0 - GetTransferAmount(energy, CoolingRadiantMultiplier);
				num4 = CoreThermalEnergy * CoreEnergyMultiplier * CoreToPartRatio * HeatTransferMultiplier;
			}
			lastFlux = num3;
			if (!base.vessel.IsFirstFrame())
			{
				base.part.AddThermalFlux(lastFlux);
			}
			num4 *= deltaTime;
			AddEnergyToCore(num4);
			ResolveConverterEnergy(deltaTime);
			ResolveNetEnergy();
			if (CoreThermalEnergy > num2)
			{
				double excess = CoreThermalEnergy - num2;
				MoveCoreEnergyToRadiators(excess, deltaTime);
			}
			else if (CoreThermalEnergy > num && !HasEnabledConverters())
			{
				double excess2 = CoreThermalEnergy - num;
				MoveCoreEnergyToRadiators(excess2, deltaTime);
			}
			ResolveNetEnergy();
			if (Dfld_CTE.guiActive)
			{
				D_CTE = CoreThermalEnergy.ToString();
			}
			if (Dfld_PTE.guiActive)
			{
				D_PTE = num.ToString();
			}
			if (Dfld_GE.guiActive)
			{
				D_GE = $"{num2} [{CoreTempGoal + CoreTempGoalAdjustment}]";
			}
			if (Dfld_partXfer.guiActive)
			{
				D_partXfer = num3.ToString();
			}
			if (Dfld_coreXfer.guiActive)
			{
				D_coreXfer = num4.ToString();
			}
			if (Dfld_EDiff.guiActive)
			{
				D_EDiff = energy.ToString();
			}
		}
	}

	public virtual void ResolveNetEnergy()
	{
		CoreThermalEnergy += energyTemp;
		energyTemp = 0.0;
	}

	public virtual void ResolveConverterEnergy(double deltaTime)
	{
		int count = converterCache.Count;
		for (int i = 0; i < count; i++)
		{
			BaseConverter baseConverter = converterCache[i];
			if (baseConverter.IsActivated && baseConverter.GeneratesHeat && !(baseConverter.lastTimeFactor < 1E-09))
			{
				double num = (double)baseConverter.TemperatureModifier.Evaluate((float)CoreTemperature) * deltaTime;
				if (baseConverter.lastTimeFactor > 1.0)
				{
					num /= baseConverter.lastTimeFactor;
				}
				if (CoreTemperature > CoreTempGoal + CoreTempGoalAdjustment)
				{
					num -= num * (double)baseConverter.GetCrewHeatBonus();
				}
				AddEnergyToCore(num);
				baseConverter.lastHeatFlux = GetLastFlux();
			}
		}
	}

	public virtual bool HasActiveConverters()
	{
		int count = base.part.Modules.Count;
		BaseConverter baseConverter;
		do
		{
			if (count-- > 0)
			{
				baseConverter = base.part.Modules[count] as BaseConverter;
				continue;
			}
			return false;
		}
		while (!(baseConverter != null) || !baseConverter.IsActivated || baseConverter.lastTimeFactor <= 1E-09);
		return true;
	}

	public virtual bool HasEnabledConverters()
	{
		int count = base.part.Modules.Count;
		BaseConverter baseConverter;
		do
		{
			if (count-- > 0)
			{
				baseConverter = base.part.Modules[count] as BaseConverter;
				continue;
			}
			return false;
		}
		while (!(baseConverter != null) || !baseConverter.IsActivated);
		return true;
	}

	public override void OnAwake()
	{
		if (PassiveEnergy == null)
		{
			PassiveEnergy = new FloatCurve();
			PassiveEnergy.Add(0f, 0f);
		}
		GameEvents.onVesselStandardModification.Add(OnVesselStandardModification);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselStandardModification.Remove(OnVesselStandardModification);
	}

	public virtual void MoveCoreEnergyToRadiators(double excess, double deltaTime)
	{
		vRadList.Clear();
		pRadList.Clear();
		double num = 0.0;
		int count = activeRadiatorParts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = activeRadiatorParts[i];
			part.isRadiator(out var radiator);
			if (radiator != null && radiator.deployState != ModuleDeployablePart.DeployState.EXTENDED)
			{
				continue;
			}
			ModuleActiveRadiator moduleActiveRadiator = part.FindModuleImplementing<ModuleActiveRadiator>();
			if (moduleActiveRadiator.IsCooling && (!moduleActiveRadiator.parentCoolingOnly || moduleActiveRadiator.IsSibling(base.part)))
			{
				num += moduleActiveRadiator.maxEnergyTransfer;
				if (moduleActiveRadiator.parentCoolingOnly)
				{
					pRadList.Add(part);
				}
				else
				{
					vRadList.Add(part);
				}
			}
		}
		int num2 = pRadList.Count + vRadList.Count;
		if (num2 == 0)
		{
			return;
		}
		double num3 = 0.0;
		count = coreHeatParts.Count;
		for (int j = 0; j < count; j++)
		{
			Part part2 = coreHeatParts[j];
			ModuleCoreHeat moduleCoreHeat = part2.FindModuleImplementing<ModuleCoreHeat>();
			double num4 = moduleCoreHeat.MaxCoolant * moduleCoreHeat.radiatorHeatingFactor;
			if (moduleCoreHeat.CoreTemperature <= part2.temperature)
			{
				continue;
			}
			int num5 = vRadList.Count;
			int count2 = pRadList.Count;
			for (int k = 0; k < count2; k++)
			{
				if (pRadList[k].FindModuleImplementing<ModuleActiveRadiator>().IsSibling(part2))
				{
					num5++;
				}
			}
			num4 *= (double)(num5 / num2);
			if (moduleCoreHeat.CoreTemperature < moduleCoreHeat.CoreTempGoal + moduleCoreHeat.CoreTempGoalAdjustment)
			{
				num4 = 0.0;
			}
			num3 += num4;
		}
		if (num3 < 1E-09)
		{
			return;
		}
		double num6 = MaxCoolant * radiatorHeatingFactor;
		if (CoreTemperature < CoreTempGoal + CoreTempGoalAdjustment)
		{
			num6 = 0.0;
		}
		double num7 = num6 / num3;
		double val = num * num7 * deltaTime;
		val = Math.Min(val, MaxCoolant * 50.0 * deltaTime);
		double num8 = Math.Min(val, excess);
		double num9 = num8 * radiatorCoolingFactor;
		double num10 = num8 * radiatorHeatingFactor;
		AddEnergyToCore(0.0 - num9);
		vRadList.AddRange(pRadList);
		count = vRadList.Count;
		for (int l = 0; l < count; l++)
		{
			Part part3 = vRadList[l];
			double num11 = part3.FindModuleImplementing<ModuleActiveRadiator>().maxEnergyTransfer / 50.0 / num;
			if (Dfld_POT.guiActive)
			{
				D_POT = num11.ToString();
			}
			double xferToPart = num10 * num11 / deltaTime;
			if (Dfld_XTP.guiActive)
			{
				D_XTP = xferToPart.ToString();
			}
			AddFluxToRadiator(part3, xferToPart, deltaTime);
		}
		if (Dfld_Excess.guiActive)
		{
			D_Excess = excess.ToString();
		}
		if (Dfld_RadCap.guiActive)
		{
			D_RadCap = num.ToString();
		}
		if (Dfld_RadSat.guiActive)
		{
			D_RadSat = num3.ToString();
		}
		if (Dfld_TRU.guiActive)
		{
			D_TRU = val.ToString();
		}
		if (Dfld_CoolPercent.guiActive)
		{
			D_CoolPercent = num7.ToString();
		}
		if (Dfld_RCA.guiActive)
		{
			D_RCA = val.ToString();
		}
		if (Dfld_CoolAmt.guiActive)
		{
			D_CoolAmt = num8.ToString();
		}
	}

	public virtual void AddFluxToRadiator(Part rad, double xferToPart, double deltatime)
	{
		if ((!(base.vessel.lastUT > 1E-09) || !(deltatime > (double)TimeWarp.fixedDeltaTime - 1E-09)) && !base.vessel.IsFirstFrame())
		{
			rad.AddThermalFlux(xferToPart);
		}
	}

	public virtual void CheckStartingTemperature()
	{
		if (double.IsNaN(CoreThermalEnergy))
		{
			CoreThermalEnergy = base.part.thermalMass * base.part.temperature;
		}
		if (CoreThermalEnergy / base.part.thermalMass < 5.0)
		{
			CoreThermalEnergy = base.part.thermalMass * base.part.temperature;
		}
	}

	public virtual double GetTransferAmount(double energy, double multiplier)
	{
		return energy * multiplier;
	}

	public virtual double GetDeltaTime()
	{
		try
		{
			if (!(Time.timeSinceLevelLoad < 1f) && FlightGlobals.ready)
			{
				if (Math.Abs(lastUpdateTime) < 1E-09)
				{
					lastUpdateTime = Planetarium.GetUniversalTime();
					return -1.0;
				}
				if (Math.Abs(lastCoreTemp - CoreTemperature) < 1E-09)
				{
					lastUpdateTime = Planetarium.GetUniversalTime();
					return TimeWarp.fixedDeltaTime;
				}
				lastCoreTemp = CoreTemperature;
				double num = Math.Min(Planetarium.GetUniversalTime() - lastUpdateTime, MaxCalculationWarp / 50.0);
				lastUpdateTime += num;
				return num;
			}
			return -1.0;
		}
		catch (Exception ex)
		{
			MonoBehaviour.print("[COREHEAT] - Error in - GetDeltaTime - " + ex);
			return 0.0;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (base.vessel == null)
		{
			return;
		}
		try
		{
			base.OnLoad(node);
			lastUpdateTime = ResourceUtilities.GetValue(node, "lastUpdateTime", lastUpdateTime);
		}
		catch (Exception ex)
		{
			Debug.LogError("[COREHEAT] - Error in - OnLoad - " + ex);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		node.AddValue("lastUpdateTime", lastUpdateTime);
	}

	public override string GetInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		double num = 0.0;
		if (converterCache == null)
		{
			UpdateConverterModuleCache();
		}
		int count = converterCache.Count;
		while (count-- > 0)
		{
			if (converterCache[count].GeneratesHeat)
			{
				float num2 = converterCache[count].TemperatureModifier.Evaluate((float)CoreTempGoal);
				if ((double)num2 > num)
				{
					num = num2;
				}
			}
		}
		stringBuilder.Append(Localizer.Format("#autoLOC_205093", $"{CoreTempGoal:0}"));
		stringBuilder.Append(Localizer.Format("#autoLOC_205094", $"{CoreShutdownTemp:0}"));
		stringBuilder.Append(Localizer.Format("#autoLOC_205095", $"{1.0 - CoreEnergyMultiplier * HeatRadiantMultiplier:0}"));
		if (num > 1E-09)
		{
			num = num / 50.0 * radiatorCoolingFactor;
			stringBuilder.Append(Localizer.Format("#autoLOC_205100", $"{num:0}"));
		}
		stringBuilder.Append(Localizer.Format("#autoLOC_205103", $"{MaxCoolant:0}"));
		return stringBuilder.ToString();
	}

	public virtual void UpdateConverterModuleCache()
	{
		converterCache = base.part.FindModulesImplementing<BaseConverter>();
	}

	public virtual void OnVesselStandardModification(Vessel v)
	{
		if (!partCachesDirty && v == base.vessel)
		{
			partCachesDirty = true;
		}
	}

	public virtual void CheckPartCaches()
	{
		if (!partCachesDirty)
		{
			return;
		}
		activeRadiatorParts.Clear();
		coreHeatParts.Clear();
		int count = base.vessel.parts.Count;
		while (count-- > 0)
		{
			Part part = base.vessel.parts[count];
			if (part.HasModuleImplementing<ModuleActiveRadiator>())
			{
				activeRadiatorParts.Add(part);
			}
			if (part.HasModuleImplementing<ModuleCoreHeat>())
			{
				coreHeatParts.Add(part);
			}
		}
		partCachesDirty = false;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003033");
	}
}
