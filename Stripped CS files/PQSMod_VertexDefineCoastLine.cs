using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Step Coast")]
public class PQSMod_VertexDefineCoastLine : PQSMod
{
	public double oceanRadiusOffset;

	public double depthOffset;

	public double oceanRadius;

	public void Reset()
	{
		oceanRadiusOffset = 1.0;
		depthOffset = -2.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		oceanRadius = (float)(sphere.radius + oceanRadiusOffset);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		if (data.vertHeight < oceanRadius)
		{
			data.vertHeight += depthOffset;
		}
	}

	public override double GetVertexMaxHeight()
	{
		return 0.0;
	}

	public override double GetVertexMinHeight()
	{
		return depthOffset;
	}
}
