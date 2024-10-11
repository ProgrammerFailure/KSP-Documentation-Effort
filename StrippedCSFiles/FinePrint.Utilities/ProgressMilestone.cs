using System.Runtime.CompilerServices;
using KSPAchievements;

namespace FinePrint.Utilities;

public class ProgressMilestone
{
	public CelestialBody body;

	public ProgressType type;

	public MannedStatus manned;

	private CelestialBodySubtree progressTree;

	public CelestialBodySubtree progress
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool complete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool possible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool bodySensitive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool crewSensitive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool impliesManned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressMilestone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressMilestone(CelestialBody body, ProgressType type, MannedStatus manned = MannedStatus.ANY)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckCompletion(ProgressNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckCompletion(string nodeID)
	{
		throw null;
	}
}
