using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Quad Mesh Colliders")]
public class PQSMod_QuadMeshColliders : PQSMod
{
	public int maxLevelOffset;

	public int minLevel;

	public PhysicMaterial physicsMaterial;

	public void Reset()
	{
		maxLevelOffset = 0;
	}

	public override void OnSetup()
	{
		minLevel = sphere.maxLevel - Mathf.Abs(maxLevelOffset);
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		if (quad.subdivision >= minLevel)
		{
			if (quad.meshCollider == null)
			{
				quad.meshCollider = quad.gameObject.AddComponent<MeshCollider>();
			}
			quad.meshCollider.enabled = true;
			quad.meshCollider.sharedMesh = quad.mesh;
			quad.meshCollider.sharedMaterial = physicsMaterial;
		}
	}

	public override void OnQuadDestroy(GClass3 quad)
	{
		if (quad.meshCollider != null)
		{
			quad.meshCollider.enabled = false;
			quad.meshCollider.sharedMesh = null;
			quad.meshCollider.sharedMaterial = null;
		}
	}
}
