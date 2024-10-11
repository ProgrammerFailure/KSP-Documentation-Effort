using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Detonator/Force")]
[RequireComponent(typeof(Detonator))]
public class DetonatorForce : DetonatorComponent
{
	private float _baseRadius;

	private float _basePower;

	private float _scaledRange;

	private float _scaledIntensity;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public float radius;

	public float power;

	public GameObject fireObject;

	public float fireObjectLife;

	private Collider[] _colliders;

	private GameObject _tempFireObject;

	private Vector3 _explosionPosition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorForce()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Explode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}
}
