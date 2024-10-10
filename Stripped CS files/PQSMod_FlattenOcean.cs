using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Flatten Ocean")]
public class PQSMod_FlattenOcean : PQSMod
{
	public double oceanRadius;

	public double oceanRad;

	public void Reset()
	{
		oceanRadius = 1.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		oceanRad = (float)(sphere.radius + oceanRadius);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		if (data.vertHeight < oceanRad)
		{
			data.vertHeight = oceanRad;
		}
	}

	public override double GetVertexMaxHeight()
	{
		return 0.0;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}
}
