using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class KerbalDeaths : ContractParameter
{
	public int countMax;

	public int countCurrent;

	public int CountMax => countMax;

	public int CountCurrent => countCurrent;

	public KerbalDeaths()
	{
	}

	public KerbalDeaths(int countMax)
	{
		state = ParameterState.Complete;
		this.countMax = countMax;
		countCurrent = 0;
	}

	public override string GetHashString()
	{
		return null;
	}

	public override string GetTitle()
	{
		if (countMax > 1)
		{
			return Localizer.Format("#autoLOC_269684", countMax.ToString());
		}
		return Localizer.Format("#autoLOC_269686");
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("count"))
		{
			countCurrent = int.Parse(node.GetValue("count"));
		}
		if (node.HasValue("countMax"))
		{
			countMax = int.Parse(node.GetValue("countMax"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("count", countCurrent);
		node.AddValue("countMax", countMax);
	}

	public override void OnRegister()
	{
		GameEvents.onCrewKilled.Add(OnCrewKilled);
	}

	public override void OnUnregister()
	{
		GameEvents.onCrewKilled.Remove(OnCrewKilled);
	}

	public void OnCrewKilled(EventReport report)
	{
		if (report.eventType == FlightEvents.CREW_KILLED)
		{
			countCurrent++;
			if (countCurrent == countMax)
			{
				SetFailed();
			}
		}
	}
}
