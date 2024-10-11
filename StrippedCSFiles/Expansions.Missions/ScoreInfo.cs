using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class ScoreInfo : IConfigNode
{
	public Guid id;

	public Guid missionId;

	public float score;

	public double timeTaken;

	public List<string> awardIds;

	public string resultsText;

	public DateTime? completedAt;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreInfo()
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
