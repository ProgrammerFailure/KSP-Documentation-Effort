using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Solid")]
public class PQSMod_VertexColorSolidBlend : PQSMod
{
	public Color color;

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		data.vertColor = color;
	}
}
