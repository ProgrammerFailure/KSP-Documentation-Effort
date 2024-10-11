using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Tangent Texture Ranges")]
public class PQSMod_TangentTextureRanges : PQSMod
{
	public double modulo;

	public double lowStart;

	public double lowEnd;

	public double highStart;

	public double highEnd;

	private static float[] tangentX;

	private static double height;

	private static double low;

	private static double med;

	private static double high;

	private static double t;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_TangentTextureRanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData vbData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMeshBuild()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double SmoothStep(double a, double b, double x)
	{
		throw null;
	}
}
