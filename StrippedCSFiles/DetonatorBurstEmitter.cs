using System.Runtime.CompilerServices;
using UnityEngine;

public class DetonatorBurstEmitter : DetonatorComponent
{
	private ParticleSystem _particleSystem;

	private float _baseDamping;

	private float _baseSize;

	private Color _baseColor;

	public float damping;

	public float startRadius;

	public float maxScreenSize;

	public bool explodeOnAwake;

	public bool oneShot;

	public float sizeVariation;

	public float particleSize;

	public float count;

	public float sizeGrow;

	public bool exponentialGrowth;

	public float durationVariation;

	public bool useWorldSpace;

	public float upwardsBias;

	public float angularVelocity;

	public bool randomRotation;

	public ParticleSystemRenderMode renderModeNewSystem;

	public bool useExplicitColorAnimation;

	public Color[] colorAnimation;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public Material material;

	private float _emitTime;

	private float speed;

	private float initFraction;

	private static float epsilon;

	private float _tmpParticleSize;

	private Vector3 _tmpPos;

	private Vector3 _thisPos;

	private float _tmpDuration;

	private float _tmpCount;

	private float _scaledDuration;

	private float _scaledDurationVariation;

	private float _scaledStartRadius;

	private float _scaledColor;

	private float _tmpAngularVelocity;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorBurstEmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DetonatorBurstEmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SizeFunction(float elapsedTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Explode()
	{
		throw null;
	}
}
