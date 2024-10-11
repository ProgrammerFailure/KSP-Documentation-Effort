using System.Runtime.CompilerServices;
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CenterOfLiftQuery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}
}
