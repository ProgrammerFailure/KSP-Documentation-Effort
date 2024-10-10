using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Simplex")]
public class PQSMod_VertexSimplexNoiseColor : PQSMod
{
	public int seed;

	public float blend;

	public Color colorStart;

	public Color colorEnd;

	public double octaves;

	public double persistence;

	public double frequency;

	public float n;

	public Simplex simplex;

	public void Reset()
	{
		colorStart = Color.yellow;
		colorEnd = Color.green;
		blend = 1f;
		octaves = 3.0;
		persistence = 0.5;
		frequency = 1.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
		simplex = new Simplex(seed, octaves, persistence, frequency);
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		n = (float)((simplex.noise(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z) + 1.0) / 2.0);
		data.vertColor = Color.Lerp(data.vertColor, Color.Lerp(colorStart, colorEnd, n), blend);
	}
}
