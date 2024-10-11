using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Altitude to Alpha")]
public class PQSMod_AltitudeAlpha : PQSMod
{
	public double atmosphereDepth;

	public bool invert;

	private double h;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_AltitudeAlpha()
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
