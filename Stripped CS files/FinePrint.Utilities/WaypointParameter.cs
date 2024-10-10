using Contracts;

namespace FinePrint.Utilities;

public abstract class WaypointParameter : ContractParameter
{
	public CelestialBody TargetBody;

	public WaypointParameter()
	{
	}

	public virtual void SetupWaypoints()
	{
	}

	public virtual void CleanupWaypoints()
	{
	}

	public virtual void UpdateWaypoints(bool focused)
	{
	}
}
