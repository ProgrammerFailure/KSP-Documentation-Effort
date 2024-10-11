using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class ModuleResourceHandler
{
	public List<ModuleResource> inputResources;

	public List<ModuleResource> outputResources;

	public bool moduleResourceBasedPrimaryIsInput;

	public bool currentResourceLowerThanLayoff;

	public PartModule partModule;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResourceHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartModule(PartModule p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LoadResList(List<ModuleResource> list, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string PrintModuleResources(double mult = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string PrintModuleResources(bool showFlowModeDesc, double mult = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetAverageInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double UpdateModuleResourceInputs(ref string error, double rateMultiplier, double threshold, bool returnOnFirstLack, bool average, bool stringOps = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double UpdateModuleResourceInputs(ref string error, bool useFlowMode, double rateMultiplier, double threshold, bool returnOnFirstLack, bool average, bool stringOps = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UpdateModuleResourceInputs(ref string error, bool useFlowMode, double rateMultiplier, double threshold, bool returnOnFirstLack, bool stringOps = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UpdateModuleResourceInputs(ref string error, double rateMultiplier, double threshold, bool returnOnFirstLack, bool stringOps = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double UpdateModuleResourceOutputs(double rateMultiplier = 1.0, double minAbsValue = 0.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEnoughResourcesToAutoStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsResourceBelowShutOffLimit(PartResource partRes)
	{
		throw null;
	}
}
