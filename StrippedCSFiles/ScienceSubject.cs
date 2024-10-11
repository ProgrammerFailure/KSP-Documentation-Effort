using System;
using System.Runtime.CompilerServices;

[Serializable]
public class ScienceSubject : IConfigNode
{
	public string id;

	public string title;

	public float dataScale;

	public float scientificValue;

	public bool applyScienceScale;

	public float subjectValue;

	public float scienceCap;

	public float science;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubject(string id, string title, float dataScale, float subjectValue, float scienceCap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubject(string id, string title, float dataScale, float subjectValue, float scienceCap, bool applyScienceScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubject(ScienceExperiment exp, ExperimentSituations sit, CelestialBody body, string biome = "", string displaybiome = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubject(ScienceExperiment exp, ExperimentSituations sit, string sourceUid, string sourceTitle, CelestialBody body, string biome = "", string displaybiome = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubject(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFromBody(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFromSituation(ExperimentSituations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasPartialIDstring(string pId)
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
}
