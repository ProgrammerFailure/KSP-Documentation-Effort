using System.Runtime.CompilerServices;
using CameraFXModules;
using UnityEngine;

public class AerodynamicsFX : MonoBehaviour
{
	public FXCamera fxCamera;

	public FXDepthCamera fxDepthCamera;

	public Light fxLight;

	public AudioSource airspeedNoise;

	public AeroFXState Condensation;

	public AeroFXState ReentryHeat;

	public Vector3 velocity;

	public double densityFactor;

	public double airDensity;

	public double airSpeed;

	public float machNumber;

	public double airTemp;

	public double densityCutoffMultiplier;

	public double heatFlux;

	public float FxScalar;

	private double p0;

	public float state;

	public float transitionFade;

	public int detailLevel;

	private Wobble machCameraFX;

	private Wobble reentryCameraFX;

	public WobbleFXParams cameraFXmach;

	public WobbleFXParams cameraFXreentry;

	private Camera _fxcamera_camera;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AerodynamicsFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSettingsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUnpause()
	{
		throw null;
	}
}
