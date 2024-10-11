using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/VoronoiCraters")]
public class PQSMod_VoronoiCraters : PQSMod
{
	private Voronoi voronoi;

	public double deformation;

	public int voronoiSeed;

	public double voronoiDisplacement;

	public double voronoiFrequency;

	public AnimationCurve craterCurve;

	public AnimationCurve jitterCurve;

	private Simplex simplex;

	public int simplexSeed;

	public double simplexOctaves;

	public double simplexPersistence;

	public double simplexFrequency;

	public float jitter;

	public float jitterHeight;

	public Gradient craterColourRamp;

	private float vorH;

	private float spxH;

	private float jtt;

	private float r;

	private float h;

	public float rFactor;

	public float rOffset;

	public float colorOpacity;

	public bool DebugColorMapping;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VoronoiCraters()
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
