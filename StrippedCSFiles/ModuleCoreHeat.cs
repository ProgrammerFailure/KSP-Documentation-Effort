using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleCoreHeat : PartModule
{
	[KSPField]
	public double CoreTempGoal;

	[KSPField]
	public double CoreShutdownTemp;

	[KSPField(isPersistant = true)]
	public double CoreTempGoalAdjustment;

	[KSPField(isPersistant = true)]
	public double CoreThermalEnergy;

	[KSPField]
	public double HeatRadiantMultiplier;

	[KSPField]
	public double CoolingRadiantMultiplier;

	[KSPField]
	public double HeatTransferMultiplier;

	[KSPField]
	public double CoolantTransferMultiplier;

	[KSPField]
	public FloatCurve PassiveEnergy;

	[KSPField]
	public double CoreEnergyMultiplier;

	[KSPField]
	public double radiatorCoolingFactor;

	[KSPField]
	public double radiatorHeatingFactor;

	[KSPField]
	public double MaxCalculationWarp;

	[KSPField]
	public double CoreToPartRatio;

	[KSPField]
	public double MaxCoolant;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001813")]
	public string D_CTE;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001814")]
	public string D_PTE;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001815")]
	public string D_GE;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001816")]
	public string D_EDiff;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001817")]
	public string D_partXfer;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001818")]
	public string D_coreXfer;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001819")]
	public string D_RadSat;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001820")]
	public string D_RadCap;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001821")]
	public string D_TRU;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001822")]
	public string D_RCA;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001823")]
	public string D_CoolPercent;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001824")]
	public string D_CoolAmt;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001825")]
	public string D_POT;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001826")]
	public string D_XTP;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001827")]
	public string D_Excess;

	protected double lastFlux;

	protected double lastUpdateTime;

	protected BaseField Dfld_CTE;

	protected BaseField Dfld_PTE;

	protected BaseField Dfld_GE;

	protected BaseField Dfld_EDiff;

	protected BaseField Dfld_partXfer;

	protected BaseField Dfld_coreXfer;

	protected BaseField Dfld_RadSat;

	protected BaseField Dfld_RadCap;

	protected BaseField Dfld_TRU;

	protected BaseField Dfld_RCA;

	protected BaseField Dfld_CoolPercent;

	protected BaseField Dfld_CoolAmt;

	protected BaseField Dfld_POT;

	protected BaseField Dfld_XTP;

	protected BaseField Dfld_Excess;

	protected List<Part> vRadList;

	protected List<Part> pRadList;

	protected List<BaseConverter> converterCache;

	protected double energyTemp;

	protected double lastCoreTemp;

	protected List<Part> activeRadiatorParts;

	protected List<Part> coreHeatParts;

	protected bool partCachesDirty;

	public virtual double CoreTemperature
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleCoreHeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckDebugFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void AddEnergyToCore(double energy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetLastFlux()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckCoreShutdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ResolveNetEnergy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ResolveConverterEnergy(double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool HasActiveConverters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool HasEnabledConverters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void MoveCoreEnergyToRadiators(double excess, double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AddFluxToRadiator(Part rad, double xferToPart, double deltatime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckStartingTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double GetTransferAmount(double energy, double multiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double GetDeltaTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateConverterModuleCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnVesselStandardModification(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckPartCaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
