using FinePrint;
using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class RecordsAltitude : ProgressNode
{
	public double record;

	public Vessel recordHolder;

	public double rewardThreshold;

	public int rewardInterval = 1;

	public double atmosphereDepth = -1.0;

	public double maxAltitude
	{
		get
		{
			if (atmosphereDepth >= 0.0)
			{
				return atmosphereDepth;
			}
			CelestialBody homeBody = FlightGlobals.GetHomeBody();
			if (homeBody == null)
			{
				return 70000.0;
			}
			atmosphereDepth = homeBody.atmosphereDepth;
			return atmosphereDepth;
		}
	}

	public RecordsAltitude()
		: base("RecordsAltitude", startReached: false)
	{
		OnIterateVessels = iterateVessels;
	}

	public void iterateVessels(Vessel v)
	{
		if (v == null || v != FlightGlobals.ActiveVessel || !v.isCommandable || v.situation == Vessel.Situations.PRELAUNCH || v.DiscoveryInfo.Level != DiscoveryLevels.Owned || !(v.mainBody.name == Planetarium.fetch.Home.name))
		{
			return;
		}
		if (v.altitude > record)
		{
			if (v.altitude > maxAltitude)
			{
				record = maxAltitude;
			}
			else
			{
				record = v.altitude;
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
				rewardThreshold = ProgressUtilities.FindNextRecord(0.0, maxAltitude, 500.0, ref rewardInterval);
			}
			while (record >= rewardThreshold)
			{
				AwardProgressInterval(Localizer.Format("#autoLOC_297833", StringUtilities.ValueAndUnits(RecordTrackType.ALTITUDE, rewardThreshold)), rewardInterval, ContractDefs.Progression.RecordSplit, ProgressType.ALTITUDERECORD);
				if (rewardInterval >= ContractDefs.Progression.RecordSplit || rewardThreshold >= maxAltitude)
				{
					break;
				}
				rewardThreshold = ProgressUtilities.FindNextRecord(rewardThreshold, maxAltitude, 500.0, ref rewardInterval);
			}
		}
		if (v.altitude >= maxAltitude)
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
			rewardThreshold = ProgressUtilities.FindNextRecord(record, maxAltitude, 500.0, ref rewardInterval);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("record", record);
	}
}
