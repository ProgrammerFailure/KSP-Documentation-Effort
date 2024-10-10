using Expansions;
using FinePrint;
using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class RecordsDistance : ProgressNode
{
	public double record;

	public Vessel recordHolder;

	public double rewardThreshold;

	public int rewardInterval = 1;

	public double distanceCeiling = -1.0;

	public double maxDistance
	{
		get
		{
			if (distanceCeiling >= 0.0)
			{
				return distanceCeiling;
			}
			if (ContractDefs.Instance == null)
			{
				return 100000.0;
			}
			distanceCeiling = ContractDefs.Progression.MaxDistanceRecord;
			return distanceCeiling;
		}
	}

	public RecordsDistance()
		: base("RecordsDistance", startReached: false)
	{
		OnIterateVessels = iterateVessels;
	}

	public void iterateVessels(Vessel v)
	{
		if (v == null || v != FlightGlobals.ActiveVessel || !v.isCommandable || v.situation == Vessel.Situations.PRELAUNCH || v.DiscoveryInfo.Level != DiscoveryLevels.Owned || SpaceCenter.Instance == null || SpaceCenter.Instance.cb == null)
		{
			return;
		}
		bool flag = false;
		double num = 0.0;
		if ((v.launchedFrom == "" || v.launchedFrom == "Runway" || v.launchedFrom == "LaunchPad") && v.mainBody.name == SpaceCenter.Instance.cb.name)
		{
			num = SpaceCenter.Instance.GreatCircleDistance(SpaceCenter.Instance.cb.GetRelSurfaceNVector(v.latitude, v.longitude));
			flag = true;
		}
		else if (v.launchedFrom != "" && ExpansionsLoader.IsExpansionInstalled("MakingHistory") && PSystemSetup.Instance.IsStockLaunchSite(v.launchedFrom))
		{
			LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(v.launchedFrom);
			if (launchSite != null && v.mainBody.name == launchSite.Body.name)
			{
				num = launchSite.GreatCircleDistance(launchSite.Body.GetRelSurfaceNVector(v.latitude, v.longitude));
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		if (num > record)
		{
			if (num > maxDistance)
			{
				record = maxDistance;
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
				rewardThreshold = ProgressUtilities.FindNextRecord(0.0, maxDistance, 1000.0, ref rewardInterval);
			}
			while (record >= rewardThreshold)
			{
				AwardProgressInterval(Localizer.Format("#autoLOC_298052", StringUtilities.ValueAndUnits(RecordTrackType.DISTANCE, rewardThreshold)), rewardInterval, ContractDefs.Progression.RecordSplit, ProgressType.DISTANCERECORD);
				if (rewardInterval >= ContractDefs.Progression.RecordSplit || rewardThreshold >= maxDistance)
				{
					break;
				}
				rewardThreshold = ProgressUtilities.FindNextRecord(rewardThreshold, maxDistance, 1000.0, ref rewardInterval);
			}
		}
		if (num >= maxDistance)
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
			rewardThreshold = ProgressUtilities.FindNextRecord(record, maxDistance, 1000.0, ref rewardInterval);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("record", record);
	}
}
