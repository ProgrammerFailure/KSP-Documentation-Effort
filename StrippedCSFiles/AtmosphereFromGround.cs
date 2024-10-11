using System.Runtime.CompilerServices;
using UnityEngine;

public class AtmosphereFromGround : MonoBehaviour
{
	public Transform mainCamera;

	public GameObject sunLight;

	public Vector3 sunLightDirection;

	public Vector3 cameraPos;

	public Color waveLength;

	public Color invWaveLength;

	public float cameraHeight;

	public float cameraHeight2;

	public float outerRadius;

	public float outerRadius2;

	public float innerRadius;

	public float innerRadius2;

	public float ESun;

	public float Kr;

	public float Km;

	public float KrESun;

	public float KmESun;

	public float Kr4PI;

	public float Km4PI;

	public float scale;

	public float scaleDepth;

	public float scaleOverScaleDepth;

	public float samples;

	public float g;

	public float g2;

	public float exposure;

	public float camHeightUnderwater;

	public float underwaterOpacityAltBase;

	public float underwaterOpacityAltMult;

	public Color underwaterColorStart;

	public Color underwaterColorEnd;

	public float lightDot;

	public float dawnFactor;

	public float underwaterDepth;

	public float initialKrESun;

	public float lerpKrESun;

	public bool useKrESunLerp;

	public float oceanDepthRecip;

	public float pRadius;

	public CelestialBody planet;

	public bool doScale;

	public float scaleToApply;

	public bool DEBUG_alwaysUpdateAll;

	private Renderer r;

	private Transform t;

	private static int shaderPropertyCameraPos;

	private static int shaderPropertyOffsetTransform;

	private static int shaderPropertyLightDir;

	private static int shaderPropertyInvWaveLength;

	private static int shaderPropertyV4CameraHeight;

	private static int shaderPropertyV4CameraHeight2;

	private static int shaderPropertyKrESun;

	private static int shaderPropertyExposure;

	private static int shaderPropertyCamHeightUnderwater;

	private static int shaderPropertyLightDot;

	private MaterialPropertyBlock mpb;

	private Color invisible;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AtmosphereFromGround()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetKrESunLerp(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAtmosphere(bool updateAll)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMaterial(bool initialSet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float pow(float f, int p)
	{
		throw null;
	}
}
