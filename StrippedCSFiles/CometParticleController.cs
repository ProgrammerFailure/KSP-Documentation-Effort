using System.Runtime.CompilerServices;
using UnityEngine;

public class CometParticleController : MonoBehaviour
{
	[SerializeField]
	internal ParticleSystem pSystem;

	[SerializeField]
	private ParticleSystem.MainModule pMain;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pStartSize;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pStartLife;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pStartSpeed;

	[SerializeField]
	private ParticleSystem.EmissionModule pEmission;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pEmissionRate;

	[SerializeField]
	private ParticleSystem.ShapeModule pShape;

	[SerializeField]
	private ParticleSystem.ForceOverLifetimeModule pForce;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pForceX;

	[SerializeField]
	private ParticleSystem.MinMaxCurve pForceY;

	[SerializeField]
	private ParticleSystem.SizeOverLifetimeModule pSize;

	public CometVessel comet;

	public float tailWidthComaRatio;

	public float tailWidthToEmitterScale;

	public float maxEmitterLength;

	public FloatCurve logLengthToSpeed;

	public FloatCurve logLengthToRate;

	public float tailWidthToParticleSizeMin;

	public float tailWidthToParticleSizeMax;

	public FloatCurve speedToSpreadForce;

	public float retrogradeSpreadRatio;

	public float normalSpreadRatio;

	[SerializeField]
	private float minLifeRatio;

	[Tooltip("Color that will be set on the emitter to drive the particles")]
	public Color emitterColor;

	public float calcedEmitterRadius;

	public float maxSimSpeed;

	public float minSimSpeed;

	[SerializeField]
	private float currentSimSpeed;

	[SerializeField]
	private float maxWarpSpeed;

	[SerializeField]
	private bool vesselDead;

	internal float debugEmitterMultiplier;

	internal float debugSizeMultiplier;

	internal float initialSizeMax;

	private bool triggerPreWarm;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometParticleController()
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
	private void OnPartWillDie(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
