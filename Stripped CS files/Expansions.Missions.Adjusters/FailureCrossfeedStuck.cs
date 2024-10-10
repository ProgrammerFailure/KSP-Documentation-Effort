using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureCrossfeedStuck : AdjusterToggleCrossfeedBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureActivationState stuckState;

	public FailureCrossfeedStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100106";
	}

	public FailureCrossfeedStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100106";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100351"));
		ModuleToggleCrossfeed moduleToggleCrossfeed = adjustedModule as ModuleToggleCrossfeed;
		if (moduleToggleCrossfeed != null)
		{
			if (stuckState == FailureActivationState.On)
			{
				moduleToggleCrossfeed.ToggleAction(KSPActionType.Activate);
			}
			else if (stuckState == FailureActivationState.Off)
			{
				moduleToggleCrossfeed.ToggleAction(KSPActionType.Deactivate);
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
