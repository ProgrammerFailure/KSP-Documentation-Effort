using System;

public class DirectionTarget : PositionTarget
{
	public Vector3d direction;

	public DirectionTarget(string name)
		: base(name)
	{
	}

	public DirectionTarget(string name, string displayName)
		: base(name, displayName)
	{
	}

	public new void Update(Vector3d direction)
	{
		this.direction = direction;
		base.Update(FlightGlobals.ActiveVessel.transform.position + 10000.0 * direction);
	}

	public void Update(Vessel v, double pitch, double heading, bool degrees = false)
	{
		if (degrees)
		{
			pitch = UtilMath.DegreesToRadians(pitch);
			heading = UtilMath.DegreesToRadians(heading);
		}
		Vector3d vector3d = Math.Cos(heading) * v.north + Math.Sin(heading) * v.east;
		Vector3d vector3d2 = Math.Cos(pitch) * vector3d + Math.Sin(pitch) * v.up;
		Update(vector3d2);
	}
}
