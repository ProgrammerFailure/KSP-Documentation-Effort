using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureDeployablePartStuck : AdjusterDeployablePartBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureDeploymentState stuckState;

	public FailureDeployablePartStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100100";
	}

	public FailureDeployablePartStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100100";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100283"));
		ModuleDeployablePart moduleDeployablePart = adjustedModule as ModuleDeployablePart;
		if (moduleDeployablePart != null)
		{
			if (stuckState == FailureDeploymentState.Deployed)
			{
				moduleDeployablePart.ControlPanelsWithoutUsingSymmetry(KSPActionType.Activate);
			}
			else if (stuckState == FailureDeploymentState.Retracted)
			{
				moduleDeployablePart.ControlPanelsWithoutUsingSymmetry(KSPActionType.Deactivate);
			}
		}
	}

	public override bool IsDeployablePartStuck()
	{
		return true;
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
