using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/LibNoise (HeightCurve2)")]
public class PQSMod_VertexRidgedAltitudeCurve : PQSMod
{
	public float deformity;

	public int ridgedAddSeed;

	public float ridgedAddFrequency;

	public float ridgedAddLacunarity;

	public int ridgedAddOctaves;

	public float ridgedMinimum;

	public NoiseQuality ridgedMode;

	public AnimationCurve simplexCurve;

	public double simplexHeightStart;

	public double simplexHeightEnd;

	public int simplexSeed;

	public double simplexOctaves;

	public double simplexPersistence;

	public double simplexFrequency;

	private Simplex simplex;

	private RidgedMultifractal ridgedAdd;

	private double hDeltaR;

	private double h;

	private double r;

	private double s;

	private float t;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexRidgedAltitudeCurve()
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
	public override void OnVertexBuildHeight(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMaxHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMinHeight()
	{
		throw null;
	}
}
