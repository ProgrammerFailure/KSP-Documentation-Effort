using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/VoronoiCraters2")]
public class PQSMod_VoronoiCraters2 : PQSMod
{
	public double deformation;

	public int voronoiSeed;

	public double voronoiFrequency;

	public AnimationCurve craterCurve;

	public int jitterSeed;

	public double jitterOctaves;

	public double jitterPersistence;

	public double jitterFrequency;

	public double jitter;

	public int deformationSeed;

	public double deformationOctaves;

	public double deformationPersistence;

	public double deformationFrequency;

	public Gradient craterColourRamp;

	public bool DebugColorMapping;

	private Voronoi voronoi;

	private Simplex jitterSimplex;

	private Simplex deformationSimplex;

	private double h;

	private double s;

	private double r;

	private double d;

	private double xd;

	private double yd;

	private double zd;

	private float rf;

	private float rfN;

	private Vector3d nearest;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VoronoiCraters2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuildHeight(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}
}
