using System;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/VoronoiCraters2")]
public class PQSMod_VoronoiCraters2 : PQSMod
{
	public double deformation = 1000.0;

	public int voronoiSeed;

	public double voronoiFrequency = 1.0;

	public AnimationCurve craterCurve;

	public int jitterSeed = 123123;

	public double jitterOctaves = 2.0;

	public double jitterPersistence = 0.5;

	public double jitterFrequency = 20.0;

	public double jitter = 0.5;

	public int deformationSeed = 123123;

	public double deformationOctaves = 2.0;

	public double deformationPersistence = 0.5;

	public double deformationFrequency = 20.0;

	public Gradient craterColourRamp;

	public bool DebugColorMapping;

	public Voronoi voronoi;

	public Simplex jitterSimplex;

	public Simplex deformationSimplex;

	public double h;

	public double s;

	public double r;

	public double d;

	public double xd;

	public double yd;

	public double zd;

	public float rf;

	public float rfN;

	public Vector3d nearest;

	public override void OnSetup()
	{
		voronoi = new Voronoi(voronoiFrequency, 0.0, voronoiSeed, distanceEnabled: true);
		jitterSimplex = new Simplex(jitterSeed, jitterOctaves, jitterPersistence, jitterFrequency);
		deformationSimplex = new Simplex(deformationSeed, deformationOctaves, deformationPersistence, deformationFrequency);
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		nearest = voronoi.GetNearest(data.directionFromCenter);
		xd = nearest.x - data.directionFromCenter.x;
		yd = nearest.y - data.directionFromCenter.y;
		zd = nearest.z - data.directionFromCenter.z;
		h = System.Math.Sqrt(xd * xd + yd * yd + zd * zd) * 1.7320508075688772;
		if (h < 0.0)
		{
			h = 0.0;
		}
		else if (h > 1.0)
		{
			h = 1.0;
		}
		s = jitterSimplex.noise(data.directionFromCenter.x, data.directionFromCenter.y, data.directionFromCenter.z) * jitter;
		r = craterCurve.Evaluate((float)(h + s));
		d = deformationSimplex.noiseNormalized(nearest.x, nearest.y, nearest.z) * deformation;
		data.vertHeight += d * r;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		rf = (float)r;
		rfN = (rf + 1f) * 0.5f;
		if (DebugColorMapping)
		{
			data.vertColor = Color.Lerp(Color.magenta, data.vertColor, rfN);
		}
		else
		{
			data.vertColor = Color.Lerp(craterColourRamp.Evaluate(rf), data.vertColor, rfN);
		}
	}
}
