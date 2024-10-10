using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/Remove Quads (Map)")]
public class PQSMod_RemoveQuadMap : PQSMod
{
	public MapSO map;

	public float mapDeformity;

	public float minHeight;

	public float maxHeight;

	public bool quadVisible;

	public double mapHeight;

	public void Reset()
	{
		minHeight = 0f;
		maxHeight = 1f;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.VertexMapCoords;
	}

	public override void OnQuadPreBuild(GClass3 quad)
	{
		quadVisible = false;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData vbData)
	{
		if (!quadVisible)
		{
			mapHeight = map.GetPixelFloat(vbData.u, vbData.v);
			if (mapHeight >= (double)minHeight && mapHeight <= (double)maxHeight)
			{
				quadVisible = true;
			}
		}
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		quad.isForcedInvisible = !quadVisible;
	}
}
