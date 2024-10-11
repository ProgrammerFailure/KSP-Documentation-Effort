using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class KerbalRagdollNode
{
	public GameObject go;

	public Rigidbody rb;

	public Collider collider;

	public Vector3 velocity;

	public Vector3 lastPos;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalRagdollNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void updateVelocity(Vector3 rootPos, Vector3d rootVel, float fdtRecip)
	{
		throw null;
	}
}
