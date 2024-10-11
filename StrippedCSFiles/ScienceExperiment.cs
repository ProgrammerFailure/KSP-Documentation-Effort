using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class ScienceExperiment
{
	public string id;

	public string experimentTitle;

	public float baseValue;

	public float scienceCap;

	public uint situationMask;

	public uint biomeMask;

	public float dataScale;

	public bool requireAtmosphere;

	public bool requireNoAtmosphere;

	public float requiredExperimentLevel;

	public bool applyScienceScale;

	private Dictionary<string, string> results;

	public Dictionary<string, string> Results
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceExperiment()
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
	public bool IsAvailableWhile(ExperimentSituations situation, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool BiomeIsRelevantWhile(ExperimentSituations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsUnlocked()
	{
		throw null;
	}
}
