using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class DeployedScienceContract : Contract
{
	private class PotentialScienceCombo
	{
		public string experimentId;

		public CelestialBody body;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PotentialScienceCombo(string id, CelestialBody body)
		{
			throw null;
		}
	}

	[SerializeField]
	protected CelestialBody targetBody;

	[SerializeField]
	protected string subjectId;

	[SerializeField]
	protected float sciencePercentage;

	public float SciencePercentage;

	protected string scienceTitle;

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
	public DeployedScienceContract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override List<CelestialBody> GetWeightBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetSynopsys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string MessageCompleted()
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
	protected override bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GenerateContract_Surface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PotentialsContainBody(CelestialBody body, List<PotentialScienceCombo> potentialCombos, string id)
	{
		throw null;
	}
}
