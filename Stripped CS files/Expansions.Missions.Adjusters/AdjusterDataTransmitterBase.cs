using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDataTransmitterBase : AdjusterPartModuleBase
{
	public AdjusterDataTransmitterBase()
	{
	}

	public AdjusterDataTransmitterBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleDataTransmitter);
	}

	public virtual double ApplyPowerAdjustment(double power)
	{
		return power;
	}

	public virtual bool IsDataTransmitterBroken()
	{
		return false;
	}
}
