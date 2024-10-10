using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureActiveRadiatorStuck : AdjusterActiveRadiatorBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureActivationState stuckState;

	public FailureActiveRadiatorStuck()
	{
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100096";
	}

	public FailureActiveRadiatorStuck(MENode node)
		: base(node)
	{
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100096";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100096"));
		ModuleActiveRadiator moduleActiveRadiator = adjustedModule as ModuleActiveRadiator;
		if (!(moduleActiveRadiator != null))
		{
			return;
		}
		if (stuckState == FailureActivationState.CurrentState)
		{
			if (moduleActiveRadiator.IsCooling)
			{
				stuckState = FailureActivationState.On;
			}
			else
			{
				stuckState = FailureActivationState.Off;
			}
		}
		if (stuckState == FailureActivationState.On)
		{
			moduleActiveRadiator.ApplyRadiatorActivation(KSPActionType.Activate);
		}
		else if (stuckState == FailureActivationState.Off)
		{
			moduleActiveRadiator.ApplyRadiatorActivation(KSPActionType.Deactivate);
		}
	}

	public override bool IsBlockingCooling()
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
			stuckState = (FailureActivationState)Enum.Parse(typeof(FailureActivationState), node.GetValue("stuckState"));
		}
	}
}
