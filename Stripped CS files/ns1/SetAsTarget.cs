using System;
using ns9;

namespace ns1;

public class SetAsTarget : MapContextMenuOption
{
	public readonly Func<ITargetable> getOTTarget;

	public ITargetable currTgt;

	public ITargetable tgt;

	public SetAsTarget(ITargetable tgt, Func<ITargetable> getOTTarget)
		: base("Set Target")
	{
		this.getOTTarget = getOTTarget;
		this.tgt = tgt;
	}

	public override bool OnCheckEnabled(out string fbText)
	{
		currTgt = getOTTarget();
		if (currTgt != tgt)
		{
			fbText = Localizer.Format("#autoLOC_465801");
		}
		else
		{
			fbText = Localizer.Format("#autoLOC_465805");
		}
		return true;
	}

	public override void OnSelect()
	{
		if (currTgt != tgt)
		{
			FlightGlobals.fetch.SetVesselTarget(tgt);
		}
		else
		{
			FlightGlobals.fetch.SetVesselTarget(null);
		}
	}

	public override bool CheckAvailable()
	{
		if (currTgt == tgt)
		{
			return currTgt != null;
		}
		if (!(tgt.GetOrbitDriver().celestialBody != null))
		{
			return tgt.GetOrbitDriver().vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors);
		}
		return true;
	}
}
