using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ROCEmitter : EffectBehaviour
{
	public Transform idleEmitter;

	public Transform burstEmitter;

	private ParticleSystem[] idleEmitters;

	private ParticleSystem[] burstEmitters;

	[KSPField]
	public string idleEffectName;

	[KSPField]
	public string burstEffectName;

	public float secondsTillBurstEmission;

	private float burstEmitterMinWait;

	private float burstEmitterMaxWait;

	private float burstTime;

	private float burstTimeFraction;

	public float burstFraction;

	private bool burstEmitting;

	private ROCsSFX[] rocsSFX;

	private bool timedBurstEmissionsEnabled;

	private bool isPaused;

	public Vector2 forceRadius;

	private float forceByDistance;

	public Vector3 forceDirection;

	private List<Part> vesselPart;

	public float vfxBaseForce;

	private float forceScale;

	private float forceMagnitude;

	public Transform forceOrigin;

	private bool applyForces;

	[SerializeField]
	private ROC roc;

	private float forceStartTime;

	private CapsuleCollider forceTrigger;

	public bool TimedBurstEmissionsEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROCEmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddForceTarget(GameObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveForceTarget(GameObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTriggerEnter(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTriggerExit(Collider other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBurstEmitterMinWait(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBurstEmitterMaxWait(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetSecondsTillBurst()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartIdleEmision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopIdleEmision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartBurstEmision(bool resetTimer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BurstTimer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopBurstEmision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSFXPower(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSFXClipPath(string idlepClip, string burstClip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyForces()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetForceScale(float burstClipTime)
	{
		throw null;
	}
}
