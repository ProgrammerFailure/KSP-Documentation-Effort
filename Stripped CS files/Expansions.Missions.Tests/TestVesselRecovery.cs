using ns11;
using ns9;

namespace Expansions.Missions.Tests;

public class TestVesselRecovery : TestVessel
{
	public bool eventFound;

	public override void Awake()
	{
		base.Awake();
		title = "#autoLOC_8001046";
	}

	public override void Initialized()
	{
		GameEvents.onVesselRecoveryProcessingComplete.Add(Recovered);
	}

	public override void Cleared()
	{
		GameEvents.onVesselRecoveryProcessingComplete.Remove(Recovered);
	}

	public void Recovered(ProtoVessel v, MissionRecoveryDialog mrd, float x)
	{
		if (vesselID == v.persistentId || vesselID == 0)
		{
			eventFound = true;
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8001047");
	}
}
