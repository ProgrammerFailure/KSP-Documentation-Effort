using System;
using System.Collections.Generic;

namespace Expansions.Missions;

public class MissionScoreInfo : IConfigNode
{
	public static string saveFileName = "MissionScoreInfo.cfg";

	public ListDictionary<Guid, ScoreInfo> scoreInfoDictionary;

	public MissionScoreInfo()
	{
		scoreInfoDictionary = new ListDictionary<Guid, ScoreInfo>();
	}

	public void AddScore(Mission mission)
	{
		if (mission.isScoreEnabled)
		{
			scoreInfoDictionary.Add(mission.id, new ScoreInfo
			{
				missionId = mission.id,
				score = mission.currentScore,
				timeTaken = HighLogic.CurrentGame.UniversalTime,
				awardIds = mission.awards.awardedAwards,
				completedAt = KSPUtil.SystemDateTime.DateTimeNow(),
				resultsText = mission.PrintScoreObjectives(onlyPrintActivatedNodes: true, startWithActiveNode: false, onlyAwardedScores: true)
			});
			scoreInfoDictionary[mission.id].Sort(CompareScoreInfo);
		}
	}

	public void DeleteScore(Guid scoreId)
	{
		Dictionary<Guid, List<ScoreInfo>>.Enumerator enumerator = scoreInfoDictionary.GetEnumerator();
		try
		{
			ScoreInfo scoreInfo = null;
			while (enumerator.MoveNext())
			{
				int i = 0;
				for (int count = enumerator.Current.Value.Count; i < count; i++)
				{
					scoreInfo = enumerator.Current.Value[i];
					if (scoreInfo.id == scoreId)
					{
						scoreInfoDictionary.Remove(enumerator.Current.Key, scoreInfo);
						return;
					}
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public void DeleteMissionScores(Mission mission)
	{
		if (mission != null)
		{
			scoreInfoDictionary.Remove(mission.id);
		}
	}

	public List<ScoreInfo> GetMissionScores(Mission mission)
	{
		if (mission == null)
		{
			return new List<ScoreInfo>();
		}
		return scoreInfoDictionary[mission.id];
	}

	public ScoreInfo GetMissionScore(Mission mission, int index)
	{
		if (mission == null)
		{
			return null;
		}
		return GetMissionScore(mission.id, index);
	}

	public ScoreInfo GetMissionScore(Guid missionId, int index)
	{
		if (scoreInfoDictionary.TryGetValue(missionId, out var val) && index < val.Count)
		{
			return val[index];
		}
		return null;
	}

	public int CompareScoreInfo(ScoreInfo a, ScoreInfo b)
	{
		if (a.score > b.score)
		{
			return -1;
		}
		if (a.score < b.score)
		{
			return 1;
		}
		if (a.timeTaken < b.timeTaken)
		{
			return -1;
		}
		if (a.timeTaken > b.timeTaken)
		{
			return 1;
		}
		return 0;
	}

	public List<string> GetMissionAwards(Mission mission)
	{
		if (mission == null)
		{
			return new List<string>();
		}
		List<ScoreInfo> list = scoreInfoDictionary[mission.id];
		List<string> list2 = new List<string>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list2.AddUniqueRange(list[i].awardIds);
		}
		return list2;
	}

	public void SaveScores()
	{
		string fileFullName = MissionsUtils.UsersMissionsPath + saveFileName;
		ConfigNode configNode = new ConfigNode();
		ConfigNode node = configNode.AddNode("MISSIONSCORES");
		Save(node);
		configNode.Save(fileFullName);
	}

	public static MissionScoreInfo LoadScores()
	{
		ConfigNode configNode = ConfigNode.Load(MissionsUtils.UsersMissionsPath + saveFileName);
		MissionScoreInfo missionScoreInfo = new MissionScoreInfo();
		if (configNode != null)
		{
			ConfigNode node = new ConfigNode();
			if (configNode.TryGetNode("MISSIONSCORES", ref node))
			{
				missionScoreInfo.Load(node);
			}
		}
		return missionScoreInfo;
	}

	public void Load(ConfigNode node)
	{
		ConfigNode[] nodes = node.GetNodes("SCOREINFO");
		foreach (ConfigNode node2 in nodes)
		{
			ScoreInfo scoreInfo = new ScoreInfo();
			scoreInfo.Load(node2);
			scoreInfoDictionary.Add(scoreInfo.missionId, scoreInfo);
		}
	}

	public void Save(ConfigNode node)
	{
		Dictionary<Guid, List<ScoreInfo>>.Enumerator enumerator = scoreInfoDictionary.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int i = 0;
				for (int count = enumerator.Current.Value.Count; i < count; i++)
				{
					enumerator.Current.Value[i].Save(node.AddNode("SCOREINFO"));
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}
}
