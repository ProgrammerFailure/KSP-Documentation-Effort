using System;
using UnityEngine;

[Serializable]
public class KerbalRagdollNode
{
	public GameObject go;

	public Rigidbody rb;

	public Collider collider;

	public Vector3 velocity;

	public Vector3 lastPos;

	public void updateVelocity(Vector3 rootPos, Vector3d rootVel, float fdtRecip)
	{
		Vector3 vector = go.transform.position - rootPos;
		velocity = (vector - lastPos) * fdtRecip + rootVel;
		lastPos = vector;
	}
}
