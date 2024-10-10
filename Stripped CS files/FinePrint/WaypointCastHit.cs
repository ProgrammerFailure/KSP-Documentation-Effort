using UnityEngine;

namespace FinePrint;

public struct WaypointCastHit : IScreenCaster
{
	public Waypoint waypoint;

	public WaypointCastHit(Waypoint waypoint)
	{
		this.waypoint = waypoint;
	}

	public Vector3 GetScreenSpacePoint()
	{
		return PlanetariumCamera.Camera.WorldToScreenPoint(waypoint.worldPosition);
	}
}
