using System;
using System.Collections.Generic;
using System.Text;
using Expansions.Missions.Adjusters;
using ns9;
using Radiators;

public class ModuleActiveRadiator : PartModule
{
	[KSPField(isPersistant = true)]
	public bool IsCooling;

	[KSPField]
	public double maxEnergyTransfer = 100000.0;

	public double originalMaxEnergyTranfer = 100000.0;

	[KSPField]
	public double overcoolFactor = 1.0;

	[KSPField]
	public double energyTransferScale = 0.1;

	[KSPField]
	public bool isCoreRadiator;

	[KSPField]
	public bool parentCoolingOnly;

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001414")]
	public string status = Localizer.Format("#autoLOC_6001415");

	public ModuleDeployableRadiator _depRad;

	public double statusValue = -1.0;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001854")]
	public string D_CoolParts = "???";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001855")]
	public string D_RadCount = "???";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001856")]
	public string D_HeadRoom = "???";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001857")]
	public string D_Excess = "???";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001858")]
	public string D_XferBase = "???";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001859")]
	public string D_XferFin = "???";

	public BaseField Dfld_RadCount;

	public BaseField Dfld_CoolParts;

	public BaseField Dfld_HeadRoom;

	public BaseField Dfld_Excess;

	public BaseField Dfld_XferBase;

	public BaseField Dfld_XferFin;

	public List<RadiatorData> coolParts = new List<RadiatorData>();

	public List<Part> hotParts = new List<Part>();

	public List<AdjusterActiveRadiatorBase> adjusterCache = new List<AdjusterActiveRadiatorBase>();

	public List<Part> activeRadiatorParts = new List<Part>();

	public List<Part> nonRadiatorParts = new List<Part>();

	public bool partCachesDirty = true;

	public static string cacheAutoLOC_6001415;

	public static string cacheAutoLOC_232021;

	public static string cacheAutoLOC_232036;

	public static string cacheAutoLOC_232067;

	public static string cacheAutoLOC_232071;

	public static string cacheAutoLOC_7000029;

	[KSPAction("#autoLOC_6001416")]
	public virtual void ToggleRadiatorAction(KSPActionParam param)
	{
		IsCooling = !IsCooling;
		UpdateStatus();
	}

	public void ApplyRadiatorActivation(KSPActionType action)
	{
		switch (action)
		{
		case KSPActionType.Activate:
			Activate();
			break;
		case KSPActionType.Deactivate:
			Shutdown();
			break;
		}
		UpdateStatus();
	}

	[KSPAction("#autoLOC_6001417")]
	public virtual void ActivateAction(KSPActionParam param)
	{
		Activate();
	}

	[KSPAction("#autoLOC_6001418")]
	public virtual void ShutdownAction(KSPActionParam param)
	{
		Shutdown();
	}

	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001417")]
	public virtual void Activate()
	{
		IsCooling = true;
		UpdateStatus();
	}

	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001418")]
	public virtual void Shutdown()
	{
		IsCooling = false;
		UpdateStatus();
	}

	public virtual void UpdateStatus()
	{
		if (!(_depRad != null))
		{
			bool num = base.Events["Activate"].active != IsCooling;
			base.Events["Activate"].active = !IsCooling;
			base.Events["Shutdown"].active = IsCooling;
			if (num)
			{
				MonoUtilities.RefreshPartContextWindow(base.part);
			}
		}
	}

	public virtual void Start()
	{
		originalMaxEnergyTranfer = maxEnergyTransfer;
		Dfld_CoolParts = base.Fields["D_CoolParts"];
		Dfld_RadCount = base.Fields["D_RadCount"];
		Dfld_HeadRoom = base.Fields["D_HeadRoom"];
		Dfld_Excess = base.Fields["D_Excess"];
		Dfld_XferBase = base.Fields["D_XferBase"];
		Dfld_XferFin = base.Fields["D_XferFin"];
		CheckIfDeployable();
		UpdateStatus();
	}

	public virtual void CheckIfDeployable()
	{
		base.part.isRadiator(out _depRad);
		if (!(_depRad == null))
		{
			base.Actions["ToggleRadiatorAction"].active = false;
			base.Actions["ActivateAction"].active = false;
			base.Actions["ShutdownAction"].active = false;
			base.Events["Activate"].active = false;
			base.Events["Shutdown"].active = false;
		}
	}

	public virtual void CheckDebugFields()
	{
		if (Dfld_CoolParts != null)
		{
			Dfld_CoolParts.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_RadCount != null)
		{
			Dfld_RadCount.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_HeadRoom != null)
		{
			Dfld_HeadRoom.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_Excess != null)
		{
			Dfld_Excess.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_XferBase != null)
		{
			Dfld_XferBase.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
		if (Dfld_XferFin != null)
		{
			Dfld_XferFin.guiActive = PhysicsGlobals.ThermalDataDisplay;
		}
	}

	public override void OnAwake()
	{
		GameEvents.onVesselStandardModification.Add(OnVesselStandardModification);
	}

	public virtual void OnDestroy()
	{
		GameEvents.onVesselStandardModification.Remove(OnVesselStandardModification);
	}

	public virtual void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || base.vessel == null)
		{
			return;
		}
		CheckPartCaches();
		if (_depRad != null && !IsAdjusterBlockingCooling())
		{
			IsCooling = _depRad.deployState == ModuleDeployablePart.DeployState.EXTENDED;
		}
		if (!IsCooling)
		{
			status = cacheAutoLOC_6001415;
			statusValue = -1.0;
		}
		else
		{
			if (!CheckResources())
			{
				return;
			}
			maxEnergyTransfer = ApplyMaxEnergyTransferAdjustments(originalMaxEnergyTranfer);
			double num = Math.Round(100.0 * base.part.temperature / (base.part.maxTemp * base.part.radiatorHeadroom), 2);
			if (num != statusValue)
			{
				status = StringBuilderCache.Format("{0:0.00}%", num);
				statusValue = num;
			}
			CheckDebugFields();
			base.part.isRadiator(out var radiator);
			if (radiator != null && radiator.deployState != ModuleDeployablePart.DeployState.EXTENDED)
			{
				return;
			}
			int num2 = 0;
			int count = activeRadiatorParts.Count;
			while (count-- > 0)
			{
				activeRadiatorParts[count].isRadiator(out var radiator2);
				if (radiator2 != null)
				{
					if (radiator2.deployState == ModuleDeployablePart.DeployState.EXTENDED)
					{
						num2++;
					}
				}
				else
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				RadiatorData thermalData = RadiatorUtilities.GetThermalData(base.part);
				InternalCooling(thermalData, num2);
			}
		}
	}

	public virtual void InternalCooling(RadiatorData rad, int radCount)
	{
		if (Dfld_RadCount.guiActive)
		{
			D_RadCount = radCount.ToString();
		}
		coolParts.Clear();
		hotParts.Clear();
		int count = nonRadiatorParts.Count;
		while (count-- > 0)
		{
			Part part = nonRadiatorParts[count];
			if (part.temperature > part.maxTemp * part.radiatorMax && part.temperature > base.vessel.externalTemperature)
			{
				hotParts.Add(part);
			}
		}
		int count2 = hotParts.Count;
		for (int i = 0; i < count2; i++)
		{
			Part part2 = hotParts[i];
			bool flag = true;
			if (parentCoolingOnly)
			{
				flag = IsSibling(part2);
			}
			if (!flag)
			{
				continue;
			}
			double num = base.part.temperature * overcoolFactor;
			if (part2.temperature > num)
			{
				RadiatorData thermalData = RadiatorUtilities.GetThermalData(part2);
				if (thermalData.Energy - thermalData.MaxEnergy > 0.0)
				{
					coolParts.Add(thermalData);
				}
			}
		}
		int count3 = coolParts.Count;
		if (Dfld_CoolParts.guiActive)
		{
			D_CoolParts = StringBuilderCache.Format("{0}/{1}", count3, hotParts.Count);
		}
		for (int j = 0; j < count3; j++)
		{
			RadiatorData radiatorData = coolParts[j];
			double num2 = (radiatorData.Energy - radiatorData.MaxEnergy) / (double)TimeWarp.fixedDeltaTime;
			num2 /= (double)(radCount + count3);
			double val = Math.Min(rad.EnergyCap - rad.Energy, maxEnergyTransfer) / (double)TimeWarp.fixedDeltaTime;
			double num3 = Math.Min(val, num2);
			if (Dfld_XferBase.guiActive)
			{
				D_XferBase = num3.ToString();
			}
			num3 *= Math.Min(1.0, energyTransferScale);
			if (num3 > 0.0 && !base.vessel.IsFirstFrame())
			{
				radiatorData.Part.AddThermalFlux(0.0 - num3);
				base.part.AddThermalFlux(num3);
			}
			if (Dfld_Excess.guiActive)
			{
				D_Excess = num2.ToString();
			}
			if (Dfld_HeadRoom.guiActive)
			{
				D_HeadRoom = val.ToString();
			}
			if (Dfld_XferFin.guiActive)
			{
				D_XferFin = num3.ToString();
			}
		}
	}

	public virtual bool IsSibling(Part targetPart)
	{
		Part parent = base.part.parent;
		if (targetPart == parent)
		{
			return true;
		}
		if (parent.parent != null && targetPart == parent.parent)
		{
			return true;
		}
		if (targetPart.parent != null && targetPart.parent == parent)
		{
			return true;
		}
		return false;
	}

	public virtual bool CheckResources()
	{
		base.part.isRadiator(out var radiator);
		if (radiator != null && radiator.deployState != ModuleDeployablePart.DeployState.EXTENDED)
		{
			status = cacheAutoLOC_232021;
			statusValue = -1.0;
			return false;
		}
		return resHandler.UpdateModuleResourceInputs(ref status, 1.0, 0.9, returnOnFirstLack: true);
	}

	public override string GetInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (resHandler.inputResources.Count > 0)
		{
			stringBuilder.Append(resHandler.PrintModuleResources());
		}
		stringBuilder.Append("\n<b><color=#99ff00ff>" + cacheAutoLOC_232036 + "</color></b>\n");
		double num = maxEnergyTransfer / 50.0;
		stringBuilder.AppendFormat(Localizer.Format("#autoLOC_232038", num));
		double num2 = 0.0;
		int count = base.part.DragCubes.Cubes.Count;
		while (count-- > 0)
		{
			DragCube dragCube = base.part.DragCubes.Cubes[count];
			double num3 = dragCube.Area[0] + dragCube.Area[1] + dragCube.Area[2] + dragCube.Area[3] + dragCube.Area[4] + dragCube.Area[5];
			if (num3 > num2)
			{
				num2 = num3;
			}
		}
		if (num2 != 0.0)
		{
			double num4 = base.part.skinMaxTemp * base.part.radiatorHeadroom;
			num4 *= num4;
			num4 *= num4;
			num4 = num2 * base.part.emissiveConstant * num4 * PhysicsGlobals.StefanBoltzmanConstant * 0.001;
			stringBuilder.Append(Localizer.Format("#autoLOC_232057", num4.ToString("F0")));
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.AppendFormat("{0:0.00}", energyTransferScale * 100.0);
		stringBuilder.Append(Localizer.Format("#autoLOC_232060", stringBuilder2.ToString()));
		if (overcoolFactor < 1.0)
		{
			if (overcoolFactor > 0.0)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_232065", (1.0 / overcoolFactor).ToString("G2")) + "\n");
			}
			else
			{
				stringBuilder.Append(cacheAutoLOC_232067);
			}
		}
		if (parentCoolingOnly)
		{
			stringBuilder.Append(cacheAutoLOC_232071);
		}
		return stringBuilder.ToString();
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterActiveRadiatorBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterActiveRadiatorBase item = adjuster as AdjusterActiveRadiatorBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public bool IsAdjusterBlockingCooling()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsBlockingCooling())
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

	public double ApplyMaxEnergyTransferAdjustments(double maximumEnergyTransfer)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			maximumEnergyTransfer = adjusterCache[i].ApplyMaxEnergyTransferAdjustment(maximumEnergyTransfer);
		}
		return maximumEnergyTransfer;
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
		nonRadiatorParts.Clear();
		int count = base.vessel.parts.Count;
		while (count-- > 0)
		{
			Part part = base.vessel.parts[count];
			if (part.HasModuleImplementing<ModuleActiveRadiator>())
			{
				activeRadiatorParts.Add(part);
			}
			else
			{
				nonRadiatorParts.Add(part);
			}
		}
		partCachesDirty = false;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_7000029;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6001415 = Localizer.Format("#autoLOC_6001415");
		cacheAutoLOC_232021 = Localizer.Format("#autoLOC_232021");
		cacheAutoLOC_232036 = Localizer.Format("#autoLOC_232036");
		cacheAutoLOC_232067 = Localizer.Format("#autoLOC_232067");
		cacheAutoLOC_232071 = Localizer.Format("#autoLOC_232071");
		cacheAutoLOC_7000029 = Localizer.Format("#autoLOC_7000029");
	}
}
