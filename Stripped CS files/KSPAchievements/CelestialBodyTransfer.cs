using FinePrint.Utilities;
using ns9;

namespace KSPAchievements;

public class CelestialBodyTransfer : ProgressNode
{
	public CelestialBody body;

	public CelestialBodyTransfer(CelestialBody cb)
		: base("CelestialBodyTransfer", startReached: false)
	{
		body = cb;
		OnDeploy = delegate
		{
			GameEvents.onCrewTransferred.Add(OnCrewTransfer);
		};
		OnStow = delegate
		{
			GameEvents.onCrewTransferred.Remove(OnCrewTransfer);
		};
	}

	public void OnCrewTransfer(GameEvents.HostedFromToAction<ProtoCrewMember, Part> fp)
	{
		if (!(fp.from == null) && !(fp.to == null) && fp.from.missionID != fp.to.missionID && fp.to.vessel.mainBody == body)
		{
			if (!base.IsComplete)
			{
				Complete();
				AwardProgressStandard(Localizer.Format("#autoLOC_296191", body.displayName), ProgressType.CREWTRANSFER, body);
			}
			Achieve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
	}

	public override void OnSave(ConfigNode node)
	{
	}
}
