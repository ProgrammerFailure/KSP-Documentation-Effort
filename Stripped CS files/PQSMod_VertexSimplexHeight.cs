using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Simplex")]
public class PQSMod_VertexSimplexHeight : PQSMod
{
	public int seed;

	public double deformity;

	public double octaves;

	public double persistence;

	public double frequency;

	public double n;

	public Simplex simplex;

	public void Reset()
	{
		deformity = 10.0;
		octaves = 3.0;
		persistence = 0.5;
		frequency = 1.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		simplex = new Simplex(seed, octaves, persistence, frequency);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		data.vertHeight += simplex.noise(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z) * deformity;
	}

	public override double GetVertexMaxHeight()
	{
		return deformity;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0 - deformity;
	}
}
