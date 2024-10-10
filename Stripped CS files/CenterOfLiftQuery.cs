using UnityEngine;

public class CenterOfLiftQuery
{
	public Vector3 refVector;

	public double refAltitude;

	public double refStaticPressure;

	public double refAirDensity;

	public Vector3 pos;

	public Vector3 dir;

	public float lift;

	public void Reset()
	{
		refVector.Zero();
		pos.Zero();
		dir.Zero();
		double num = 0.0;
		refAirDensity = 0.0;
		double num2 = num;
		num = 0.0;
		refStaticPressure = num2;
		refAltitude = num;
		lift = 0f;
	}
}
