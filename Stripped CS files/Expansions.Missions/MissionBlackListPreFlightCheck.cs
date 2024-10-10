using System.Collections.Generic;
using Expansions.Missions.Runtime;
using ns9;
using PreFlightTests;

namespace Expansions.Missions;

public class MissionBlackListPreFlightCheck : IPreFlightTest
{
	public VesselSituation vesselSituation;

	public List<string> missionExcludedParts;

	public bool missionCheckReqd;

	public Dictionary<AvailablePart, int> blackListedPartsOnShip;

	public MissionBlackListPreFlightCheck(VesselSituation situation, ShipConstruct ship)
	{
		vesselSituation = situation;
		missionExcludedParts = new List<string>();
		blackListedPartsOnShip = new Dictionary<AvailablePart, int>();
		MissionSituation missionSituation = ((vesselSituation == null) ? ((MissionSystem.missions.Count > 0) ? MissionSystem.missions[0].situation : null) : vesselSituation.node.mission.situation);
		if (missionSituation != null && missionSituation.partFilter != null)
		{
			missionExcludedParts = missionSituation.partFilter.GetExcludedParts();
			if (missionExcludedParts.Count > 0)
			{
				missionCheckReqd = true;
			}
		}
		if (vesselSituation != null && vesselSituation.partFilter != null && vesselSituation.partFilter.GetExcludedParts().Count > 0)
		{
			missionExcludedParts.AddUniqueRange(vesselSituation.partFilter.GetExcludedParts());
			missionCheckReqd = true;
		}
		if (!missionCheckReqd)
		{
			return;
		}
		int count = ship.parts.Count;
		while (count-- > 0)
		{
			if (missionExcludedParts.Contains(ship.parts[count].partInfo.name))
			{
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(ship.parts[count].partInfo.name);
				if (!blackListedPartsOnShip.ContainsKey(partInfoByName))
				{
					blackListedPartsOnShip.Add(partInfoByName, 1);
				}
				else
				{
					blackListedPartsOnShip[partInfoByName] += 1;
				}
			}
		}
	}

	public bool Test()
	{
		if (blackListedPartsOnShip.Count > 0)
		{
			return false;
		}
		return true;
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_8100208");
	}

	public string GetWarningDescription()
	{
		string text = Localizer.Format("#autoLOC_8002063") + "\n\n";
		Dictionary<AvailablePart, int>.KeyCollection.Enumerator enumerator = blackListedPartsOnShip.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			text = text + "<color=orange><b>" + blackListedPartsOnShip[enumerator.Current] + "x " + enumerator.Current.title + "</b></color>\n";
		}
		enumerator.Dispose();
		return text;
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253243");
	}
}
