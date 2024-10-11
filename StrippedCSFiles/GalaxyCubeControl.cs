using System.Runtime.CompilerServices;
using UnityEngine;

public class GalaxyCubeControl : MonoBehaviour
{
	public Color minGalaxyColor;

	public Color maxGalaxyColor;

	public float atmosFadeLimit;

	public float glareFadeLimit;

	public float daytimeFadeLimit;

	public float airPressureFade;

	public float glareFadeLerpRate;

	public Sun sunRef;

	public Camera tgt;

	private double sunSrfAngle;

	private double sunCamAngle;

	public QuaternionD initRot;

	private Renderer[] cubeRenderers;

	private bool lineOfSightToSun;

	private float atmosFade;

	private float dayTimeFade;

	private float glareFade;

	private float totalFade;

	private int layerMask;

	private MaterialPropertyBlock mpb;

	public static GalaxyCubeControl Instance;

	private RaycastHit lineOfSightToSunHit;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GalaxyCubeControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEnabled(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
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
	private bool LineOfSightToSun()
	{
		throw null;
	}
}
