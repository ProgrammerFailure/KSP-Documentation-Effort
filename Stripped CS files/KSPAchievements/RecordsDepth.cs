using System;
using FinePrint;
using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class RecordsDepth : ProgressNode
{
	public double record;

	public Vessel recordHolder;

	public double rewardThreshold;

	public int rewardInterval = 1;

	public double depthCeiling = -1.0;

	public double maxDepth
	{
		get
		{
			if (depthCeiling >= 0.0)
			{
				return depthCeiling;
			}
			if (ContractDefs.Instance == null)
			{
				return 750.0;
			}
			depthCeiling = ContractDefs.Progression.MaxDepthRecord;
			return depthCeiling;
		}
	}

	public RecordsDepth()
		: base("RecordsDepth", startReached: false)
	{
		OnIterateVessels = iterateVessels;
	}

	public void iterateVessels(Vessel v)
	{
		if (v == null || v != FlightGlobals.ActiveVessel || !v.isCommandable || v.DiscoveryInfo.Level != DiscoveryLevels.Owned || v.mainBody.name != Planetarium.fetch.Home.name || v.situation != Vessel.Situations.SPLASHED || !(v.altitude <= 0.0))
		{
			return;
		}
		double num = Math.Abs(v.altitude);
		if (num > record)
		{
			if (num > maxDepth)
			{
				record = maxDepth;
			}
			else
			{
				record = num;
			}
			if (recordHolder != v)
			{
				Reach();
				recordHolder = v;
			}
			else
			{
				AchieveDate = Planetarium.GetUniversalTime();
				Achieve();
			}
		}
		if (!base.IsComplete)
		{
			if (rewardThreshold <= 0.0)
			{
				rewardThreshold = ProgressUtilities.FindNextRecord(0.0, maxDepth, 10.0, ref rewardInterval);
			}
			while (record >= rewardThreshold)
			{
				AwardProgressInterval(Localizer.Format("#autoLOC_297943", StringUtilities.ValueAndUnits(RecordTrackType.DEPTH, rewardThreshold)), rewardInterval, ContractDefs.Progression.RecordSplit, ProgressType.DEPTHRECORD);
				if (rewardInterval >= ContractDefs.Progression.RecordSplit || rewardThreshold >= maxDepth)
				{
					break;
				}
				rewardThreshold = ProgressUtilities.FindNextRecord(rewardThreshold, maxDepth, 10.0, ref rewardInterval);
			}
		}
		if (num >= maxDepth)
		{
			Complete();
			OnIterateVessels = null;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("record"))
		{
			record = double.Parse(node.GetValue("record"));
		}
		if (base.IsComplete)
		{
			OnIterateVessels = null;
		}
		else
		{
			rewardThreshold = ProgressUtilities.FindNextRecord(record, maxDepth, 10.0, ref rewardInterval);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("record", record);
	}
}
