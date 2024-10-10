using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Surface Object Quads")]
public class PQSMod_SurfaceObjectQuads : PQSMod
{
	public int maxLevelOffset;

	public int minLevel;

	public CelestialBody cb;

	public void Reset()
	{
		maxLevelOffset = 0;
	}

	public override void OnSetup()
	{
		minLevel = sphere.maxLevel - Mathf.Abs(maxLevelOffset);
		cb = sphere.GetComponentInParent<CelestialBody>();
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		if (quad.subdivision >= minLevel && !quad.GetComponent<SurfaceObject>())
		{
			SurfaceObject.Create(quad.gameObject, cb, 1, KFSMUpdateMode.LATEUPDATE);
		}
	}

	public override void OnQuadDestroy(GClass3 quad)
	{
		SurfaceObject component = quad.gameObject.GetComponent<SurfaceObject>();
		if (component != null)
		{
			component.Terminate();
		}
	}
}
