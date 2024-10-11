using System;
using System.Runtime.CompilerServices;
using Contracts;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class CollectDeployedScience : ContractParameter
{
	protected CelestialBody targetBody;

	protected string subjectId;

	protected float sciencePercentage;

	public float SciencePercentage;

	protected float scienceCollected;

	public float ScienceCollected;

	protected string scienceTitle;

	public string ScienceTitle;

	public CelestialBody TargetBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SubjectId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectDeployedScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectDeployedScience(CelestialBody targetBody, string subjectId, string scienceTitle, float sciencePercentage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		throw null;
	}
}