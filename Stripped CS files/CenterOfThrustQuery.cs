using UnityEngine;

public class CenterOfThrustQuery
{
	public Vector3 pos;

	public Vector3 dir;

	public float thrust;

	public void Reset()
	{
		pos.Zero();
		dir.Zero();
		thrust = 0f;
	}
}
