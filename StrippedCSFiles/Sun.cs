using System.Runtime.CompilerServices;
using UnityEngine;

public class Sun : MonoBehaviour
{
	public static Sun Instance;

	public Transform target;

	public CelestialBody sun;

	public LensFlare sunFlare;

	public double AU;

	public Vector3d sunDirection;

	public Vector3d sunRotation;

	public AnimationCurve brightnessCurve;

	public float brightnessMultiplier;

	public bool useLocalSpaceSunLight;

	protected Light scaledSunLight;

	public float localTime;

	public float fadeStart;

	public float fadeEnd;

	private CelestialBody mainBody;

	private double targetAltitude;

	private double horizonDistance;

	private double horizonAngle;

	private float horizonScalar;

	private float dayNightRatio;

	private float fadeEndAtAlt;

	private float fadeStartAtAlt;

	public float shadowBiasSpaceCentre;

	public float shadowBiasFlight;

	public float shadowNmlBiasZero;

	public float shadowNmlBiasMid;

	public float shadowNmlBiasFar;

	public float shadowNmlBiasMidDist;

	public float shadowNmlBiasFarDist;

	public int sunRotationPrecision;

	public int sunRotationPrecisionMapView;

	public int sunRotationPrecisionDefault;

	private Light lgt;

	public float showCascadeZeroZoomed;

	public float showCascadeOneZoomed;

	public float shadowCascadeCamDist;

	public float showCascadeZeroMidRange;

	public float showCascadeOneMidRange;

	public float shadowCascadeCamDistMidRange;

	[SerializeField]
	private float camDistance;

	[SerializeField]
	private bool processSunBias;

	[SerializeField]
	private bool processShadowCascades;

	public Light sunLight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Sun()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SunlightEnabled(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetLocalTimeAtPosition(Vector3d wPos, CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetLocalTimeAtPosition(double latitude, double longitude, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetLocalTimeAtPosition(double latitude, double longitude, double altitude, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}
}
