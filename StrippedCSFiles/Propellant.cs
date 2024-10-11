using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Propellant
{
	public string name;

	public string displayName;

	public int id;

	public float ratio;

	public double minResToLeave;

	public bool ignoreForIsp;

	public bool ignoreForThrustCurve;

	public double currentRequirement;

	public double currentAmount;

	public bool isDeprived;

	public bool drawStackGauge;

	private PartResourceDefinition _cachedResourceDef;

	private int displayNameLimit;

	[SerializeField]
	private ResourceFlowMode flowMode;

	[SerializeField]
	private ResourceFlowMode fMode;

	public double actualTotalAvailable;

	public double totalResourceCapacity;

	public bool hasAlternatePropellant;

	public PartResourceDefinition resourceDef
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double totalResourceAvailable
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
	public Propellant()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateConnectedResources(Part p)
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
	public PartResourceDefinition getPartResourceDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceFlowMode GetFlowMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetFlowModeDescription()
	{
		throw null;
	}
}
