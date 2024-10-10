using UnityEngine;

namespace Expansions.Missions;

public class ScoreRange : IConfigNode
{
	public double minRange;

	public double maxRange;

	public float score;

	public ScoreRange()
	{
		minRange = 0.0;
		maxRange = 1.0;
		score = 0f;
	}

	public bool isValueInRange(double value)
	{
		if (value >= minRange)
		{
			return value <= maxRange;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreRange scoreRange))
		{
			return false;
		}
		if (UtilMath.Approximately(minRange, scoreRange.minRange) && UtilMath.Approximately(maxRange, scoreRange.maxRange))
		{
			return Mathf.Approximately(score, scoreRange.score);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return minRange.GetHashCode() ^ maxRange.GetHashCode() ^ score.GetHashCode();
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("minRange", ref minRange);
		node.TryGetValue("maxRange", ref maxRange);
		node.TryGetValue("score", ref score);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("minRange", minRange);
		node.AddValue("maxRange", maxRange);
		node.AddValue("score", score);
	}
}
