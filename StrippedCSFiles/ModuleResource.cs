using System;
using System.Runtime.CompilerServices;

[Serializable]
public class ModuleResource : IConfigNode
{
	public string name;

	public string title;

	public int id;

	public double amount;

	public double rate;

	public double currentRequest;

	public double currentAmount;

	public bool varyTime;

	public ResourceFlowMode flowMode;

	private PartResourceDefinition _cachedResourceDef;

	public bool useSI;

	public double displayUnitMult;

	public string unitName;

	public bool available;

	public bool isDeprived;

	public bool shutOffStartUpUsePercent;

	public bool shutOffHandler;

	public bool startUpHandler;

	public float shutOffAmount;

	public float shutOffPercent;

	public float startUpAmount;

	public float startUpPercent;

	public ModuleResourceAutoShiftState autoStateShifter;

	public PartResourceDefinition resourceDef
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintRate(double mult = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintRate(bool showFlowMode, double mult = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsDeprived(float requestThreshold = 0.9f)
	{
		throw null;
	}
}
