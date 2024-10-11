using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Scaled Space/Planet Fader")]
[RequireComponent(typeof(Renderer))]
public class ScaledSpaceFader : MonoBehaviour
{
	public CelestialBody celestialBody;

	public string floatName;

	public float fadeStart;

	public float fadeEnd;

	public float highQualityShaderFadeStart;

	public float highQualityShaderFadeEnd;

	public static double faderMult;

	[HideInInspector]
	public Renderer r;

	private double a;

	private float t;

	private Texture2D resourceMap;

	private int floatID;

	private Renderer rend;

	private int shaderResourceID;

	private int shaderFadeID;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScaledSpaceFader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ScaledSpaceFader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetAngularSize(Transform t, Renderer r, Camera c)
	{
		throw null;
	}
}
