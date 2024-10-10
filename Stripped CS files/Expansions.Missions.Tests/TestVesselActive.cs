using System;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestVesselActive : TestVessel
{
	public override void Awake()
	{
		base.Awake();
		title = "#autoLOC_8004217";
		useActiveVessel = true;
	}

	public override bool Test()
	{
		base.Test();
		return vessel == FlightGlobals.ActiveVessel;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8006116");
	}
}
