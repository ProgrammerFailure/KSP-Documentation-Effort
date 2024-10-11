using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/OceanFX")]
public class PQSMod_OceanFX : PQSMod
{
	public Texture2D[] watermain;

	[HideInInspector]
	public Texture2D refraction;

	[HideInInspector]
	public Texture2D bump;

	[HideInInspector]
	public Texture2D fresnel;

	[HideInInspector]
	public Cubemap reflection;

	public float framesPerSecond;

	public bool cycleTextures;

	[HideInInspector]
	public double spaceAltitude;

	[HideInInspector]
	public float blendA;

	[HideInInspector]
	public float blendB;

	[HideInInspector]
	public Material waterMat;

	[HideInInspector]
	public float waterMainLength;

	[HideInInspector]
	public int txIndex;

	[HideInInspector]
	public float texBlend;

	[HideInInspector]
	public float angle;

	[HideInInspector]
	public Color specColor;

	[HideInInspector]
	public float oceanOpacity;

	[HideInInspector]
	public float spaceSurfaceBlend;

	private float t;

	private static int waterTexPropertyID;

	private static int waterTex1PropertyID;

	private static int mixPropertyID;

	private static int specColorPropertyID;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_OceanFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdateFinished()
	{
		throw null;
	}
}
