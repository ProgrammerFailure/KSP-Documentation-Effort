using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterActiveRadiatorBase : AdjusterPartModuleBase
{
	public AdjusterActiveRadiatorBase()
	{
	}

	public AdjusterActiveRadiatorBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleActiveRadiator);
	}

	public virtual bool IsBlockingCooling()
	{
		return false;
	}

	public virtual double ApplyMaxEnergyTransferAdjustment(double maxEnergyTransfer)
	{
		return maxEnergyTransfer;
	}
}
