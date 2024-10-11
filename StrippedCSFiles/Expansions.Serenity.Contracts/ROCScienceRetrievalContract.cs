using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class ROCScienceRetrievalContract : Contract
{
	private class PotentialScienceCombo
	{
		public string body;

		public ROCDefinition rocDef;

		public string id;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PotentialScienceCombo(string body, ROCDefinition roc, string id)
		{
			throw null;
		}
	}

	[SerializeField]
	protected CelestialBody targetBody;

	[SerializeField]
	protected string subjectId;

	protected string rocType;

	protected string scienceTitle;

	protected List<string> biomes;

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
	public ROCScienceRetrievalContract()
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
}
