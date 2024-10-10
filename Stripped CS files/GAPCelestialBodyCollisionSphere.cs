using UnityEngine;

public class GAPCelestialBodyCollisionSphere : MonoBehaviour
{
	public SphereCollider sphereCollider;

	public MeshFilter cbMesh;

	public void Setup(CelestialBody celestialBody)
	{
		base.transform.NestToParent(celestialBody.transform);
		float radius = celestialBody.scaledBody.GetComponent<SphereCollider>().radius;
		base.transform.localScale = Vector3d.one * (celestialBody.Radius / (double)radius);
		sphereCollider.radius = radius;
		cbMesh.mesh = celestialBody.scaledBody.GetComponent<MeshFilter>().mesh;
		ToggleVisibility(toggleValue: true);
	}

	public void ToggleVisibility(bool toggleValue)
	{
		cbMesh.gameObject.GetComponent<MeshRenderer>().enabled = toggleValue;
	}
}
