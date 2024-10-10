using ns2;
using ns9;
using UnityEngine;

public class AlarmTypeSOI : AlarmTypeFlightNodeBase
{
	public string cantSetAlarmText = "";

	public AlarmTypeSOI()
	{
		iconURL = "soi";
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
			if (orbitCache.patchEndTransition != Orbit.PatchTransitionType.ENCOUNTER && orbitCache.patchEndTransition != Orbit.PatchTransitionType.ESCAPE)
			{
				cantSetAlarmText = Localizer.Format("#autoLOC_8003611", GetDefaultTitle());
				return false;
			}
		}
		return true;
	}

	public override string GetDefaultTitle()
	{
		return Localizer.Format("#autoLOC_8003561");
	}

	public override string CannotSetAlarmText()
	{
		return cantSetAlarmText;
	}

	public override MapObject.ObjectType MapNodeType()
	{
		return MapObject.ObjectType.PatchTransition;
	}

	public override bool ShowAlarmMapObject(MapObject mapObject)
	{
		return mapObject.orbit == mapObject.vesselRef.orbit;
	}

	public override bool InitializeFromMapObject(MapObject mapObject)
	{
		if (mapObject.vesselRef == null)
		{
			Debug.LogError("[AlarmTypeSOI]: Unable to create alarm - there is no vessel reference");
			return false;
		}
		orbitCache = base.Vessel.orbit;
		return true;
	}

	public override double GetNodeUT()
	{
		if (orbitCache != null)
		{
			return orbitCache.EndUT;
		}
		return 0.0;
	}
}
