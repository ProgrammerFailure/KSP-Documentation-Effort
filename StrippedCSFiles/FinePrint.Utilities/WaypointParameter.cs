using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Utilities;

public abstract class WaypointParameter : ContractParameter
{
	public CelestialBody TargetBody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected WaypointParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetupWaypoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CleanupWaypoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateWaypoints(bool focused)
	{
		throw null;
	}
}
