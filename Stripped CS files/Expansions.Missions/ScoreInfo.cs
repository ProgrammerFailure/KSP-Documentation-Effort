using System;
using System.Collections.Generic;

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

	public ScoreInfo()
	{
		id = Guid.NewGuid();
		score = 0f;
		timeTaken = 0.0;
		awardIds = new List<string>();
		resultsText = "";
		completedAt = null;
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("id", ref id);
		node.TryGetValue("missionId", ref missionId);
		node.TryGetValue("score", ref score);
		node.TryGetValue("timeTaken", ref timeTaken);
		node.TryGetValue("resultsText", ref resultsText);
		long value = 0L;
		completedAt = null;
		if (node.TryGetValue("completedAt", ref value))
		{
			completedAt = DateTime.FromFileTimeUtc(value);
		}
		awardIds = node.GetValuesList("awardId");
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", id);
		node.AddValue("missionId", missionId);
		node.AddValue("score", score);
		node.AddValue("timeTaken", timeTaken);
		string value = resultsText.Replace("\n", "\\n").Replace("\t", "\\t");
		node.AddValue("resultsText", value);
		if (completedAt.HasValue)
		{
			node.AddValue("completedAt", completedAt.Value.ToFileTimeUtc());
		}
		for (int i = 0; i < awardIds.Count; i++)
		{
			node.AddValue("awardId", awardIds[i]);
		}
	}
}
