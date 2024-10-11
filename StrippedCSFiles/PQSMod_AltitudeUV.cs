using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Altitude to UV3")]
public class PQSMod_AltitudeUV : PQSMod
{
	public double atmosphereHeight;

	public double oceanDepth;

	public bool invert;

	private double h;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_AltitudeUV()
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
	public override void OnVertexBuild(PQS.VertexBuildData vbData)
	{
		throw null;
	}
}
