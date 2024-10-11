using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Fade Altitude")]
public class PQSMod_MaterialFadeAltitude : PQSMod
{
	public string floatName;

	public float fadeStart;

	public float fadeEnd;

	public float valueStart;

	public float valueEnd;

	[HideInInspector]
	public Material mat;

	private double a;

	private float t;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MaterialFadeAltitude()
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
}
