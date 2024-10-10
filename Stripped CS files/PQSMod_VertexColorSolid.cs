using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Solid (Blend)")]
public class PQSMod_VertexColorSolid : PQSMod
{
	public Color color;

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
		data.vertColor = Color.Lerp(data.vertColor, color, blend);
	}
}
