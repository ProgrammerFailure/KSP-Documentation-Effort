using ns9;

namespace ns1;

public class AutoWarpToUT : MapContextMenuOption
{
	public double destUT;

	public bool featureUnlocked;

	public PatchedConics.PatchCastHit hit;

	public int patchesAheadLimit;

	public PatchedConicRenderer pcr;

	public double tgtUT;

	public AutoWarpToUT(double destUT, PatchedConicRenderer pcr, PatchedConics.PatchCastHit hit, int patchesAheadLimit)
		: base("Warp Here")
	{
		this.patchesAheadLimit = patchesAheadLimit;
		this.hit = hit;
		this.pcr = pcr;
		this.destUT = destUT;
		tgtUT = destUT;
		featureUnlocked = GameVariables.Instance.UnlockedFlightPlanning(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl));
	}

	public override void OnSelect()
	{
		TimeWarp.fetch.WarpTo(tgtUT);
	}

	public override bool OnCheckEnabled(out string fbText)
	{
		if ((pcr.patchRenders.Contains(hit.pr) && pcr.patchRenders.IndexOf(hit.pr) <= patchesAheadLimit) || (pcr.flightPlanRenders.Contains(hit.pr) && pcr.flightPlanRenders.IndexOf(hit.pr) <= patchesAheadLimit))
		{
			if (pcr.solver.maneuverNodes.Count != 0 && hit.UTatTA >= pcr.solver.maneuverNodes[0].double_0)
			{
				fbText = Localizer.Format("#autoLOC_465524");
				_ = pcr.solver.maneuverNodes[0].double_0;
				_ = pcr.solver.maneuverNodes[0].startBurnIn;
				tgtUT = ((pcr.solver.maneuverNodes[0].startBurnIn > 0.0) ? (pcr.solver.maneuverNodes[0].startBurnIn + Planetarium.GetUniversalTime() - (double)GameSettings.WARP_TO_MANNODE_MARGIN) : (pcr.solver.maneuverNodes[0].double_0 - (double)GameSettings.WARP_TO_MANNODE_MARGIN));
			}
			else
			{
				fbText = Localizer.Format("#autoLOC_465519");
				tgtUT = destUT;
			}
			return true;
		}
		fbText = Localizer.Format("#autoLOC_465531");
		return false;
	}

	public override bool CheckAvailable()
	{
		return featureUnlocked;
	}
}
