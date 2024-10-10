using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Sphere/Sphere Collider")]
public class PQSMod_CreateSphereCollider : PQSMod
{
	public float radiusOffset;

	public SphereCollider sphereCollider;

	public void Reset()
	{
		radiusOffset = 0f;
	}

	public override void OnSetup()
	{
		sphereCollider = GetComponent<SphereCollider>();
		if (sphereCollider == null)
		{
			sphereCollider = base.gameObject.AddComponent<SphereCollider>();
			sphereCollider.hideFlags = HideFlags.HideInInspector;
		}
		sphereCollider.radius = (float)(sphere.radius + (double)radiusOffset);
	}
}
