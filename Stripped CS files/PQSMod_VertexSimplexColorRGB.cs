using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Simplex (RGB)")]
public class PQSMod_VertexSimplexColorRGB : PQSMod
{
	public int seed;

	public float blend;

	public float rBlend;

	public float gBlend;

	public float bBlend;

	public double octaves;

	public double persistence;

	public double frequency;

	public float n;

	public Color c;

	public Simplex simplex;

	public void Reset()
	{
		blend = 1f;
		rBlend = 1f;
		gBlend = 1f;
		bBlend = 1f;
		octaves = 3.0;
		persistence = 0.5;
		frequency = 1.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
		simplex = new Simplex(seed, octaves, persistence, frequency);
		c = Color.white;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		n = (float)((simplex.noise(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z) + 1.0) * 0.5);
		c.r = n * rBlend;
		c.g = n * gBlend;
		c.b = n * bBlend;
		data.vertColor = Color.Lerp(data.vertColor, c, blend);
	}
}
