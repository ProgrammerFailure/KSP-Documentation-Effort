using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class CometOrbitType
{
	[SerializeField]
	public string name;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string description;

	[SerializeField]
	public float inclination1Min;

	[SerializeField]
	public float inclination1Max;

	[SerializeField]
	public float inclination2Min;

	[SerializeField]
	public float inclination2Max;

	[SerializeField]
	public float inclination1Chance;

	[SerializeField]
	public float peMin;

	[SerializeField]
	public float peMax;

	[SerializeField]
	public float apMin;

	[SerializeField]
	public float apMax;

	[SerializeField]
	public bool useEccentricity;

	[SerializeField]
	public float eccentricityMin;

	[SerializeField]
	public float eccentricityMax;

	[SerializeField]
	public UntrackedObjectClass minCometClass;

	[SerializeField]
	public UntrackedObjectClass maxCometClass;

	[SerializeField]
	public int chanceWeight;

	private double trueAnom;

	private double eccAnom;

	private double meanAnomalyAtEpoch;

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal CometOrbitType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometOrbitType(string name, float inclination1Min, float inclination1Max, float inclination2Min, float inclination2Max, float inclination1Chance, float PEMin, float PEMax, float APMin, float APMax, bool UseEcc, float EccMin, float EccMax, UntrackedObjectClass minCometClass, UntrackedObjectClass maxCometClass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit CalculateHomeOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit CalculateSentinelOrbit(Orbit sentinelOrbit, double minDiscoveryDistance, out bool validOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Orbit CalculateOrbit(bool homeOrbit, Orbit sentinelOrbit, double minDiscoveryDistance, out bool validOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UntrackedObjectClass GetRandomObjClass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
