using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterGrappleNodeBase : AdjusterPartModuleBase
{
	public AdjusterGrappleNodeBase()
	{
	}

	public AdjusterGrappleNodeBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleGrappleNode);
	}

	public virtual bool IsBlockingGrappleRelease()
	{
		return false;
	}

	public virtual bool IsBlockingGrappleGrab()
	{
		return false;
	}
}
