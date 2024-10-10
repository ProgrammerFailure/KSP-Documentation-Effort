using System;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterRCSBase : AdjusterPartModuleBase
{
	public AdjusterRCSBase()
	{
	}

	public AdjusterRCSBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleRCS);
	}

	public virtual Vector3 ApplyInputRotationAdjustment(Vector3 inputRotation)
	{
		return inputRotation;
	}

	public virtual Vector3 ApplyInputLinearAdjustment(Vector3 inputLinear)
	{
		return inputLinear;
	}

	public virtual bool IsRCSBroken()
	{
		return false;
	}
}
