using System;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureCargoBayStuck : AdjusterCargoBayBase
{
	[MEGUI_Dropdown(guiName = "#autoLOC_8100097")]
	public FailureOpenState stuckState;

	public FailureCargoBayStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100098";
	}

	public FailureCargoBayStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100098";
	}

	public override void Activate()
	{
		base.Activate();
		ModuleCargoBay moduleCargoBay = adjustedModule as ModuleCargoBay;
		if (moduleCargoBay != null)
		{
			moduleCargoBay.ForceCargoBayStuck(stuckState);
			UpdateStatusMessage(Localizer.Format("#autoLOC_8100324", moduleCargoBay.partTypeName));
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		ModuleCargoBay moduleCargoBay = adjustedModule as ModuleCargoBay;
		if (moduleCargoBay != null)
		{
			moduleCargoBay.RemoveAdjusterForcingCargoBayStuck();
		}
	}

	public override bool IsCargoBayStuck()
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
			stuckState = (FailureOpenState)Enum.Parse(typeof(FailureOpenState), node.GetValue("stuckState"));
		}
	}
}
