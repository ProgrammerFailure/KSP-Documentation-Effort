using LibNoise;
using LibNoise.Modifiers;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (Multi)")]
public class PQSMod_VertexNoise : PQSMod
{
	public int seed;

	public float noiseDeformity;

	public int noisePasses;

	public float smoothness;

	public float falloff;

	public float mesaVsPlainsBias;

	public float plainsVsMountainSmoothness;

	public float plainsVsMountainThreshold;

	public float plainSmoothness;

	public Select terrainHeightMap;

	public void Reset()
	{
		seed = Random.Range(0, int.MaxValue);
		noiseDeformity = 10f;
		noisePasses = 8;
		smoothness = 1.5f;
		falloff = 2f;
		mesaVsPlainsBias = 2f;
		plainsVsMountainSmoothness = 6000f;
		plainsVsMountainThreshold = 0.125f;
		plainSmoothness = 10f;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		Billow billow = new Billow();
		billow.Seed = seed;
		billow.Frequency = 2.0 * (double)(1f / smoothness);
		ScaleBiasOutput inputmodule = new ScaleBiasOutput(1f / plainSmoothness, mesaVsPlainsBias, billow);
		RidgedMultifractal ridgedMultifractal = new RidgedMultifractal();
		ridgedMultifractal.Seed = seed;
		ridgedMultifractal.OctaveCount = noisePasses;
		ridgedMultifractal.Frequency = 1.0 / (double)smoothness;
		Perlin perlin = new Perlin();
		perlin.Seed = seed;
		perlin.OctaveCount = noisePasses;
		perlin.Frequency = 1.0 / (double)plainsVsMountainSmoothness;
		perlin.Persistence = 1.0 / (double)falloff;
		terrainHeightMap = new Select(0.0, 1.0, plainsVsMountainThreshold, inputmodule, ridgedMultifractal);
		terrainHeightMap.ControlModule = perlin;
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		data.vertHeight += terrainHeightMap.GetValue(data.directionFromCenter * sphere.radius) * (double)noiseDeformity;
	}

	public override double GetVertexMaxHeight()
	{
		return noiseDeformity;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}
}
