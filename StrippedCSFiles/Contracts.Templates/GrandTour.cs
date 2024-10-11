using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FinePrint.Utilities;
using UnityEngine;

namespace Contracts.Templates;

[Serializable]
public class GrandTour : Contract
{
	[SerializeField]
	protected List<CelestialBody> targetBodies;

	private ProgressType progressType;

	private bool finalDestination;

	private bool focused;

	private CelestialBody focusBody;

	private CelestialBody finalBody;

	private Vessel.Situations finalSituation;

	public List<CelestialBody> TargetBodies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private int TotalBodies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private string FormattedDestinations
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GrandTour()
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
	protected override void OnAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetNotes()
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
	private void AttemptRandomTour(KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttemptFocusedTour(KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<CelestialBody> GetFocusedChallenges(ProgressType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttemptFinalDestination(KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}
}
