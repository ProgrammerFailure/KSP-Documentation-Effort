using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Simplex (Multi)")]
public class PQSMod_VertexSimplexMultiChromatic : PQSMod
{
	public float blend;

	public int redSeed;

	public double redOctaves;

	public double redPersistence;

	public double redFrequency;

	private Simplex redSimplex;

	public int blueSeed;

	public double blueOctaves;

	public double bluePersistence;

	public double blueFrequency;

	private Simplex blueSimplex;

	public int greenSeed;

	public double greenOctaves;

	public double greenPersistence;

	public double greenFrequency;

	private Simplex greenSimplex;

	public int alphaSeed;

	public double alphaOctaves;

	public double alphaPersistence;

	public double alphaFrequency;

	private Simplex alphaSimplex;

	private float n;

	private Color c;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexSimplexMultiChromatic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}
}
