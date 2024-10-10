using System;
using Expansions.Missions.Editor;
using ModuleWheels;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureWheelDeploymentStuck : AdjusterWheelDeploymentBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureDeploymentState stuckState;

	public FailureWheelDeploymentStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100352";
	}

	public FailureWheelDeploymentStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100352";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100353"));
		ModuleWheelDeployment moduleWheelDeployment = adjustedModule as ModuleWheelDeployment;
		if (moduleWheelDeployment != null)
		{
			if (stuckState == FailureDeploymentState.Deployed)
			{
				moduleWheelDeployment.ActionToggle(KSPActionType.Activate);
			}
			else if (stuckState == FailureDeploymentState.Retracted)
			{
				moduleWheelDeployment.ActionToggle(KSPActionType.Deactivate);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("stuckState", stuckState.ToString());
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (node.HasValue("stuckState"))
		{
			stuckState = (FailureDeploymentState)Enum.Parse(typeof(FailureDeploymentState), node.GetValue("stuckState"));
		}
	}
}
