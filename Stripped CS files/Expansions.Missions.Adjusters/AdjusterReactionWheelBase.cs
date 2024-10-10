using System;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterReactionWheelBase : AdjusterPartModuleBase
{
	public AdjusterReactionWheelBase()
	{
	}

	public AdjusterReactionWheelBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleReactionWheel);
	}

	public virtual Vector3 ApplyTorqueAdjustment(Vector3 torque)
	{
		return torque;
	}

	public virtual bool IsReactionWheelBroken()
	{
		return false;
	}
}
