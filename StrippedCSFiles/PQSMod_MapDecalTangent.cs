using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Decal (Tangent)")]
public class PQSMod_MapDecalTangent : PQSMod
{
	public double radius;

	public Vector3 position;

	public float angle;

	public MapSO heightMap;

	public double heightMapDeformity;

	public bool cullBlack;

	public bool useAlphaHeightSmoothing;

	public bool absolute;

	public double absoluteOffset;

	public MapSO colorMap;

	public float smoothHeight;

	public float smoothColor;

	public bool removeScatter;

	public bool DEBUG_HighlightInclusion;

	private double inclusionAngle;

	private bool quadActive;

	private bool vertActive;

	private Vector3d posNorm;

	private double quadAngle;

	private Vector3d vertRot;

	private Quaternion rot;

	private float u;

	private float v;

	private Color c1;

	private Color c2;

	private MapSO.HeightAlpha ha;

	private float smoothCR;

	private float smoothC1M;

	private float smoothHR;

	private float smoothH1M;

	private float smoothU;

	private float smoothV;

	private float smoothFactor;

	private bool buildHeight;

	private double testAngle;

	private double vHeight;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MapDecalTangent()
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
	public override void OnQuadPreBuild(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuildHeight(PQS.VertexBuildData vbData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetHeightSmoothing(float u, float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetColorSmoothing(float u, float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData vbData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double v2, double v1, double dt)
	{
		throw null;
	}
}
