using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class MissionScoreInfo : IConfigNode
{
	private static string saveFileName;

	private ListDictionary<Guid, ScoreInfo> scoreInfoDictionary;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionScoreInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MissionScoreInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddScore(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteScore(Guid scoreId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteMissionScores(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<ScoreInfo> GetMissionScores(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreInfo GetMissionScore(Mission mission, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreInfo GetMissionScore(Guid missionId, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int CompareScoreInfo(ScoreInfo a, ScoreInfo b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetMissionAwards(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveScores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionScoreInfo LoadScores()
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
