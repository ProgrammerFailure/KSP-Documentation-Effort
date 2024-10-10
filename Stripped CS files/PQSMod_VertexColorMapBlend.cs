using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Map (Blend)")]
public class PQSMod_VertexColorMapBlend : PQSMod
{
	public MapSO vertexColorMap;

	public float blend;

	public void Reset()
	{
		blend = 1f;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		data.vertColor = Color.Lerp(data.vertColor, vertexColorMap.GetPixelColor(data.u, data.v), blend);
	}
}
