using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Map")]
public class PQSMod_VertexColorMap : PQSMod
{
	public MapSO vertexColorMap;

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		data.vertColor = vertexColorMap.GetPixelColor(data.u, data.v);
	}
}
