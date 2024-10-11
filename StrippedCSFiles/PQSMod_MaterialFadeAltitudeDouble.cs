using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Fade Altitude Double")]
public class PQSMod_MaterialFadeAltitudeDouble : PQSMod
{
	public string floatName;

	public float inFadeStart;

	public float inFadeEnd;

	public float outFadeStart;

	public float outFadeEnd;

	public float valueStart;

	public float valueMid;

	public float valueEnd;

	[HideInInspector]
	public Material mat;

	private double a;

	private float t;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MaterialFadeAltitudeDouble()
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
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}
}
