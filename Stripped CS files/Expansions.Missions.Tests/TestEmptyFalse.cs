using ns9;

namespace Expansions.Missions.Tests;

public class TestEmptyFalse : TestModule
{
	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8003075");
	}

	public override bool Test()
	{
		return false;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003076");
	}
}
