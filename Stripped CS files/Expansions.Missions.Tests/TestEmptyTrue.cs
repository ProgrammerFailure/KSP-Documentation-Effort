using ns9;

namespace Expansions.Missions.Tests;

public class TestEmptyTrue : TestModule
{
	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8003077");
	}

	public override bool Test()
	{
		return true;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003078");
	}
}
