using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureEnginesDeadEngine : AdjusterEnginesBase
{
	public FailureEnginesDeadEngine()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100333";
	}

	public FailureEnginesDeadEngine(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100333";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100334"));
		ModuleEngines moduleEngines = adjustedModule as ModuleEngines;
		if (moduleEngines != null)
		{
			bool allowShutdown = moduleEngines.allowShutdown;
			moduleEngines.allowShutdown = true;
			moduleEngines.Shutdown();
			moduleEngines.allowShutdown = allowShutdown;
		}
	}

	public override bool IsEngineDead()
	{
		return true;
	}
}
