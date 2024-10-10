using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureResourceIntakeStuck : AdjusterResourceIntakeBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureActivationState stuckState;

	public FailureResourceIntakeStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100104";
	}

	public FailureResourceIntakeStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100104";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100354"));
		ModuleResourceIntake moduleResourceIntake = adjustedModule as ModuleResourceIntake;
		if (moduleResourceIntake != null)
		{
			if (stuckState == FailureActivationState.On)
			{
				moduleResourceIntake.ToggleAction(KSPActionType.Activate);
			}
			else if (stuckState == FailureActivationState.Off)
			{
				moduleResourceIntake.ToggleAction(KSPActionType.Deactivate);
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
			stuckState = (FailureActivationState)Enum.Parse(typeof(FailureActivationState), node.GetValue("stuckState"));
		}
	}
}
