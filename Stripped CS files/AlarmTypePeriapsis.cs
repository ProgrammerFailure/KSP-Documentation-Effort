using ns2;
using ns9;
using UnityEngine;

public class AlarmTypePeriapsis : AlarmTypeFlightNodeBase
{
	public string cantSetAlarmText = "";

	public AlarmTypePeriapsis()
	{
		iconURL = "pe";
	}

	public override bool CanSetAlarm(AlarmUIDisplayMode displayMode)
	{
		if (displayMode == AlarmUIDisplayMode.Add)
		{
			if (orbitCache == null)
			{
				cantSetAlarmText = Localizer.Format("#autoLOC_8003610");
				return false;
			}
			if (orbitCache.timeToPe < 0.0 || orbitCache.timeToPe > orbitCache.EndUT - orbitCache.StartUT)
			{
				cantSetAlarmText = Localizer.Format("#autoLOC_8003611", GetDefaultTitle());
				return false;
			}
		}
		return true;
	}

	public override string GetDefaultTitle()
	{
		return Localizer.Format("#autoLOC_8003539");
	}

	public override string CannotSetAlarmText()
	{
		return cantSetAlarmText;
	}

	public override MapObject.ObjectType MapNodeType()
	{
		return MapObject.ObjectType.Periapsis;
	}

	public override bool ShowAlarmMapObject(MapObject mapObject)
	{
		return mapObject.orbit == mapObject.vesselRef.orbit;
	}

	public override bool InitializeFromMapObject(MapObject mapObject)
	{
		if (mapObject.vesselRef == null)
		{
			Debug.LogError("[AlarmTypePeriapsis]: Unable to create alarm - there is no vessel reference");
			return false;
		}
		orbitCache = base.Vessel.orbit;
		return true;
	}

	public override double GetNodeUT()
	{
		if (orbitCache != null)
		{
			return orbitCache.StartUT + orbitCache.timeToPe;
		}
		return 0.0;
	}
}
