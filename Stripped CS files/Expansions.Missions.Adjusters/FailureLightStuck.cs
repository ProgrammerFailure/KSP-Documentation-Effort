using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureLightStuck : AdjusterLightBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureActivationState stuckState;

	public FailureLightStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100102";
	}

	public FailureLightStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100102";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100312"));
		ModuleLight moduleLight = adjustedModule as ModuleLight;
		if (moduleLight != null)
		{
			if (stuckState == FailureActivationState.On)
			{
				moduleLight.ToggleLightAction(KSPActionType.Activate);
			}
			else if (stuckState == FailureActivationState.Off)
			{
				moduleLight.ToggleLightAction(KSPActionType.Deactivate);
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
