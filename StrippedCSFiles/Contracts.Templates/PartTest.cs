using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Templates;

[Serializable]
public class PartTest : Contract
{
	[SerializeField]
	public string PartName;

	[SerializeField]
	public bool PartIsExperimental;

	[NonSerialized]
	protected AvailablePart tgtPart;

	[SerializeField]
	protected CelestialBody destination;

	[SerializeField]
	protected Vessel.Situations tgtSituation;

	[SerializeField]
	protected bool hauled;

	public bool Hauled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartTest()
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
	public string GetSituationString(Vessel.Situations sit, CelestialBody body, string biomeName)
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
	protected override void OnAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartResearched(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveSituations(List<Vessel.Situations> list, params Vessel.Situations[] remove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CombineSituations(List<Vessel.Situations> list, List<Vessel.Situations> check)
	{
		throw null;
	}
}
