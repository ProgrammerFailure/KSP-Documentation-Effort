using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;

namespace FinePrint.Contracts;

public class ExplorationContract : Contract
{
	private class MilestoneComparer : IComparer<ProgressMilestone>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public MilestoneComparer()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int Compare(ProgressMilestone first, ProgressMilestone second)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int ProgressTypeValue(ProgressMilestone milestone)
		{
			throw null;
		}
	}

	private List<ProgressTrackingParameter> trackingParameters;

	private CelestialBody targetBody;

	private float bodyRatio;

	private bool rendezvousUnlocked;

	private bool dockUnlocked;

	private bool evaUnlocked;

	private bool flagsUnlocked;

	public bool IsTutorial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ProgressTrackingParameter> TrackingParameters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExplorationContract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeDeclined()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
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
	protected override string MessageCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int AllowableContracts(List<ProgressMilestone> milestones)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLocals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPrestige()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetRewards()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ProgressMilestone> GetMissionMilestones()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ProgressMilestone> GetSeedMilestones(KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ProgressMilestone> GetThemedMilestones(ProgressMilestone seed)
	{
		throw null;
	}
}
