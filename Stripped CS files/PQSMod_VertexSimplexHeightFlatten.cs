using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Flatten Simplex")]
public class PQSMod_VertexSimplexHeightFlatten : PQSMod
{
	public int seed;

	public double cutoff;

	public double deformity;

	public double octaves;

	public double persistence;

	public double frequency;

	public double n;

	public Simplex simplex;

	public double val;

	public double valRatio;

	public void Reset()
	{
		cutoff = 0.5;
		deformity = 10.0;
		octaves = 3.0;
		persistence = 0.5;
		frequency = 1.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		simplex = new Simplex(seed, octaves, persistence, frequency);
		valRatio = 1.0 / cutoff;
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		val = simplex.noiseNormalized(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z);
		if (val > cutoff)
		{
			data.vertHeight += deformity * ((val - cutoff) * valRatio);
		}
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
