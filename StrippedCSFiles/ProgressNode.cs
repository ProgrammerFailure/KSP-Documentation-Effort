using System;
using System.Runtime.CompilerServices;
using FinePrint.Utilities;

public class ProgressNode : IConfigNode
{
	private string id;

	private bool reached;

	private bool complete;

	private ProgressTree subtree;

	protected double AchieveDate;

	public Callback OnDeploy;

	public Callback OnStow;

	public Action<Vessel> OnIterateVessels;

	protected Func<string, string> OnGenerateSummary;

	public string Id
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsReached
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsCompleteManned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public bool IsCompleteUnmanned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public ProgressTree Subtree
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressNode(string id, bool startReached)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Achieve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Complete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeSummary(string baseID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CrewSensitiveComplete(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CrewSensitiveComplete(ProtoVessel pv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AwardProgress(string description, float funds = 0f, float science = 0f, float reputation = 0f, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddOrAppendWorldFirstMessage(string title, string body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AwardProgressStandard(string description, ProgressType progress, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AwardProgressInterval(string description, int currentInterval, int totalIntervals, ProgressType progress, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AwardProgressRandomTech(string description, int seed)
	{
		throw null;
	}
}
