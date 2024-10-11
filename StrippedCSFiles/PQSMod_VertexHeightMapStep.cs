using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Map (Coastal Step)")]
public class PQSMod_VertexHeightMapStep : PQSMod
{
	public Texture2D heightMap;

	public double heightMapDeformity;

	public double heightMapOffset;

	public bool scaleDeformityByRadius;

	public double coastHeight;

	private double heightDeformity;

	private double h;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexHeightMapStep()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double v2, double v1, double dt)
	{
		throw null;
	}
}
