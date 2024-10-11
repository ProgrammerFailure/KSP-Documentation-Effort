using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Aerial Perspective")]
public class PQSMod_AerialPerspectiveMaterial : PQSMod
{
	public float globalDensity;

	public float heightFalloff;

	public float atmosphereDepth;

	public float oceanDepth;

	public bool DEBUG_SetEveryFrame;

	public double cameraAlt;

	public float cameraAtmosAlt;

	public float heightDensAtViewer;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_AerialPerspectiveMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdateFinished()
	{
		throw null;
	}
}
