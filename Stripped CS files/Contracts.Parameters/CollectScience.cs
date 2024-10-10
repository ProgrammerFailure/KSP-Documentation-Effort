using System;
using ns9;

namespace Contracts.Parameters;

[Serializable]
public class CollectScience : ContractParameter
{
	public CelestialBody targetBody;

	public BodyLocation targetLocation;

	public CelestialBody TargetBody => targetBody;

	public BodyLocation TargetLocation => targetLocation;

	public CollectScience()
	{
	}

	public CollectScience(CelestialBody targetBody, BodyLocation location)
	{
		this.targetBody = targetBody;
		targetLocation = location;
	}

	public override string GetHashString()
	{
		return targetLocation.ToString() + " " + targetBody.name;
	}

	public override string GetTitle()
	{
		if (targetLocation == BodyLocation.Space)
		{
			return Localizer.Format("#autoLOC_269461", targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_269463", targetBody.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("body"))
		{
			targetBody = FlightGlobals.fetch.bodies[int.Parse(node.GetValue("body"))];
		}
		if (node.HasValue("location"))
		{
			targetLocation = (BodyLocation)Enum.Parse(typeof(BodyLocation), node.GetValue("location"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("body", targetBody.flightGlobalsIndex);
		node.AddValue("location", targetLocation.ToString());
	}

	public override void OnRegister()
	{
		GameEvents.OnScienceRecieved.Add(OnScience);
		GameEvents.OnTriggeredDataTransmission.Add(OnTriggeredScience);
	}

	public override void OnUnregister()
	{
		GameEvents.OnScienceRecieved.Remove(OnScience);
		GameEvents.OnTriggeredDataTransmission.Remove(OnTriggeredScience);
	}

	public void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		if (reverseEngineered)
		{
			return;
		}
		if (targetLocation == BodyLocation.Space)
		{
			if (subject.IsFromBody(targetBody) && (subject.IsFromSituation(ExperimentSituations.InSpaceHigh) || subject.IsFromSituation(ExperimentSituations.InSpaceLow)))
			{
				SetComplete();
			}
		}
		else if (subject.IsFromBody(targetBody) && (subject.IsFromSituation(ExperimentSituations.SrfLanded) || subject.IsFromSituation(ExperimentSituations.SrfSplashed)))
		{
			SetComplete();
		}
	}

	public void OnTriggeredScience(ScienceData data, Vessel origin, bool xmitAborted)
	{
		if (data == null || origin == null || origin.mainBody == null || xmitAborted || data.dataAmount <= 0f || origin.mainBody != targetBody)
		{
			return;
		}
		if (targetLocation == BodyLocation.Space)
		{
			if (origin.situation == Vessel.Situations.ORBITING || origin.situation == Vessel.Situations.SUB_ORBITAL)
			{
				SetComplete();
			}
		}
		else if (origin.situation == Vessel.Situations.LANDED || origin.situation == Vessel.Situations.SPLASHED)
		{
			SetComplete();
		}
	}
}
