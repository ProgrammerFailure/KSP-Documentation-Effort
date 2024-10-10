using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class FailureReactionWheelStuck : AdjusterReactionWheelBase
{
	public Vector3 stuckTorque = Vector3.zero;

	public bool loadedFromConfigNode;

	public FailureReactionWheelStuck()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100349";
	}

	public FailureReactionWheelStuck(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100349";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100350"));
		if (!loadedFromConfigNode)
		{
			ModuleReactionWheel moduleReactionWheel = adjustedModule as ModuleReactionWheel;
			if (moduleReactionWheel != null)
			{
				stuckTorque = moduleReactionWheel.GetAppliedTorque();
			}
		}
	}

	public override Vector3 ApplyTorqueAdjustment(Vector3 torque)
	{
		return stuckTorque;
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("stuckTorque", KSPUtil.WriteVector(stuckTorque));
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("stuckTorque", ref stuckTorque);
		loadedFromConfigNode = true;
	}
}
