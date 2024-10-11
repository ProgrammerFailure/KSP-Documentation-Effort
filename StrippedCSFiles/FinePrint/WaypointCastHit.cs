using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint;

public struct WaypointCastHit : IScreenCaster
{
	public Waypoint waypoint;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WaypointCastHit(Waypoint waypoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetScreenSpacePoint()
	{
		throw null;
	}
}
