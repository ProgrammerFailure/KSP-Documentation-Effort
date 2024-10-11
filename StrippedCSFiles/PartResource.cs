using System;
using System.Runtime.CompilerServices;

[Serializable]
public class PartResource
{
	public enum FlowMode
	{
		None,
		Out,
		In,
		Both
	}

	public PartResourceDefinition info;

	public string resourceName;

	public Part part;

	public double amount;

	public double maxAmount;

	public bool _flowState;

	public bool isTweakable;

	public bool isVisible;

	public bool hideFlow;

	public bool simulationResource;

	public FlowMode _flowMode;

	public bool flowState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public FlowMode flowMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResource(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResource(Part p, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResource(PartResource res)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResource(PartResource res, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Copy(PartResource res)
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
	public void SetInfo(PartResourceDefinition info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanProvide(double demand)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanProvide(bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Flowing(bool pulling)
	{
		throw null;
	}
}
