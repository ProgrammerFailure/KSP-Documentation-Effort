using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/Billboard Object")]
public class PQSMod_BillboardObject : PQSMod
{
	public Vector3d posVector;

	public void FixedUpdate()
	{
		if (sphere != null && sphere.target != null)
		{
			posVector = sphere.GetRelativePosition(sphere.target.transform.position);
			base.transform.localRotation = Quaternion.FromToRotation(Vector3.up, posVector);
		}
	}
}
