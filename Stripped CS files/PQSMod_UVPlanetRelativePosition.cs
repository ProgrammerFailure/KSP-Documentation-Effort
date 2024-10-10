using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/UV planet relative position")]
public class PQSMod_UVPlanetRelativePosition : PQSMod
{
	public int i;

	public Vector3d v;

	public override void OnQuadBuilt(GClass3 quad)
	{
		for (i = 0; i < GClass4.cacheVertCount; i++)
		{
			v = GClass4.verts[i];
			GClass4.cacheUVs[i].x = (float)v.x;
			GClass4.cacheUVs[i].y = (float)v.y;
			GClass4.cacheUV2s[i].x = (float)v.z;
			GClass4.cacheUV2s[i].y = (float)(1.0 - Vector3d.Dot(v.normalized, quad.vertNormals[i]));
		}
		quad.mesh.uv = GClass4.cacheUVs;
		quad.mesh.uv2 = GClass4.cacheUV2s;
	}

	public override void OnQuadUpdateNormals(GClass3 quad)
	{
		for (i = 0; i < GClass4.cacheVertCount; i++)
		{
			v = quad.verts[i] + quad.positionPlanet;
			GClass4.cacheUV2s[i].x = (float)v.z;
			GClass4.cacheUV2s[i].y = (float)(1.0 - Vector3d.Dot(v.normalized, quad.vertNormals[i]));
		}
		quad.mesh.uv2 = GClass4.cacheUV2s;
	}
}
